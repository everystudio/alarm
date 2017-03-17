using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using EveryStudioLibrary;

public class Startup : Singleton<Startup> {

	static public bool InitializeCheck = false;

	public float m_fDelayTimer;

	public enum STEP
	{
		NONE				= 0,
		CHECK_CONFIG		,
		CHECK_SCENARIO		,
		CHECK_DOWNLOAD		,
		DATA_DOWNLOAD		,
		CHECK_COMIC_LIST	,
		CHECK_IMAGE_LIST	,
		CHECK_VOICE_LIST	,
		CHECK_VOICESET_LIST	,
		GOTO_GAME			,
		RETRY				,
		END					,
		MAX					,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;
	public int m_iNetworkSerial;
	public List<SpreadSheetData> m_ssdSample;
	public CsvScript m_scriptData;

	private FileDownload fd;

	public WaitingInitialize m_waitingInitialize;

	public override void Initialize ()
	{
		base.Initialize ();
		InitializeCheck = true;
		m_eStep = STEP.CHECK_CONFIG;
		m_eStepPre = STEP.MAX;
		m_fDelayTimer = 0.0f;

		Application.targetFrameRate = 60;

		// ios対応；基本保存させない
		#if UNITY_IOS
		UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath);

		#elif UNITY_ANDROID
		GameBillingManager.init();
		#endif
		return;
	}

	void Update () {
		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
			Debug.Log (m_eStep);
		}

		m_fDelayTimer += Time.deltaTime;
		switch (m_eStep) {
		case STEP.CHECK_CONFIG:
			if (bInit) {
				m_iNetworkSerial = CommonNetwork.Instance.RecieveSpreadSheet (DataManagerAlarm.Instance.SPREAD_SHEET, "od6");
			}
			if (CommonNetwork.Instance.IsConnected (m_iNetworkSerial)) {
				TNetworkData data = EveryStudioLibrary.CommonNetwork.Instance.GetData (m_iNetworkSerial);
				m_ssdSample = EveryStudioLibrary.CommonNetwork.Instance.ConvertSpreadSheetData (data.m_dictRecievedData);
				CsvConfig config_data = new CsvConfig ();
				config_data.Input (m_ssdSample);
				config_data.Save (CsvConfig.FILE_NAME);

				// configファイルのみconfigファイル同士で判定する
				if (false == config_data.Read (CsvConfig.KEY_CONFIG_VERSION).Equals (DataManagerAlarm.Instance.config.Read (CsvConfig.KEY_CONFIG_VERSION))) {
					config_data.Save (CsvConfig.FILE_NAME);
					DataManagerAlarm.Instance.config.Load (CsvConfig.FILE_NAME);
				}
				m_eStep = STEP.GOTO_GAME;
				//Debug.LogError (DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_DOWNLOAD_VERSION));
				//Debug.LogError (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_DOWNLOAD_VERSION));
				if (false == DataManagerAlarm.Instance.config.Read (FileDownloadManager.KEY_DOWNLOAD_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (FileDownloadManager.KEY_DOWNLOAD_VERSION))) {
					m_eStep = STEP.CHECK_DOWNLOAD;
				} else if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_COMIC_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_COMIC_LIST_VERSION))) {
					m_eStep = STEP.CHECK_COMIC_LIST;
				} else if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_IMAGE_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_IMAGE_LIST_VERSION))) {
					m_eStep = STEP.CHECK_IMAGE_LIST;
				} else if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_VOICE_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_VOICE_LIST_VERSION))) {
					m_eStep = STEP.CHECK_VOICE_LIST;
				} else if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_VOICESET_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_VOICESET_LIST_VERSION))) {
					m_eStep = STEP.CHECK_VOICESET_LIST;
				}
			} else if (CommonNetwork.Instance.IsError (m_iNetworkSerial)) {
			//} else if (true) {
				bool bSuccess = true;
				if (false == DataManagerAlarm.Instance.config.Read (FileDownloadManager.KEY_DOWNLOAD_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (FileDownloadManager.KEY_DOWNLOAD_VERSION))) {
					bSuccess = false;
				} else if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_COMIC_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_COMIC_LIST_VERSION))) {
					bSuccess = false;
				} else if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_IMAGE_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_IMAGE_LIST_VERSION))) {
					bSuccess = false;
				} else if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_VOICE_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_VOICE_LIST_VERSION))) {
					bSuccess = false;
				} else if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_VOICESET_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_VOICESET_LIST_VERSION))) {
					bSuccess = false;
				}
				if (bSuccess == true) {
					m_eStep = STEP.GOTO_GAME;
				} else {
					m_eStep = STEP.RETRY;
				}
			}
			break;
		case STEP.CHECK_DOWNLOAD:
			if (bInit) {
				m_iNetworkSerial = CommonNetwork.Instance.RecieveSpreadSheet (
					DataManagerAlarm.Instance.SPREAD_SHEET,
					DataManagerAlarm.Instance.config.Read ("download"));
			}
			if (CommonNetwork.Instance.IsConnected (m_iNetworkSerial)) {
				TNetworkData data = EveryStudioLibrary.CommonNetwork.Instance.GetData (m_iNetworkSerial);
				m_ssdSample = EveryStudioLibrary.CommonNetwork.Instance.ConvertSpreadSheetData (data.m_dictRecievedData);

				CsvDownload dest_download_list = new CsvDownload();
				dest_download_list.Input (m_ssdSample);
				dest_download_list.Save (FileDownloadManager.FILENAME_DOWNLOAD_LIST);
				m_eStep = STEP.DATA_DOWNLOAD;
			}
			break;
		case STEP.DATA_DOWNLOAD:
			if (bInit) {
				CsvDownload source_download_list = new CsvDownload ();
				source_download_list.Load (FileDownloadManager.FILENAME_DOWNLOAD_LIST);
				Debug.Log (DataManagerAlarm.Instance.config.ReadInt (FileDownloadManager.KEY_DOWNLOAD_VERSION));
				FileDownloadManager.Instance.Download (DataManagerAlarm.Instance.config.ReadInt (FileDownloadManager.KEY_DOWNLOAD_VERSION), source_download_list.list);
			}

			int iTotal = 0;
			int iNow = 0;

			if (FileDownloadManager.Instance.IsIdle (out iTotal, out iNow)) {
				DataManagerAlarm.Instance.data_kvs.Write (FileDownloadManager.KEY_DOWNLOAD_VERSION, DataManagerAlarm.Instance.config.Read (FileDownloadManager.KEY_DOWNLOAD_VERSION));
				DataManagerAlarm.Instance.data_kvs.Save (DataKvs.FILE_NAME);

				m_eStep = STEP.GOTO_GAME;
				if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_COMIC_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_COMIC_LIST_VERSION))) {
					m_eStep = STEP.CHECK_COMIC_LIST;
				} else if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_IMAGE_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_IMAGE_LIST_VERSION))) {
					m_eStep = STEP.CHECK_IMAGE_LIST;
				} else if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_VOICE_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_VOICE_LIST_VERSION))) {
					m_eStep = STEP.CHECK_VOICE_LIST;
				} else if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_VOICESET_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_VOICESET_LIST_VERSION))) {
					m_eStep = STEP.CHECK_VOICESET_LIST;
				}
				m_waitingInitialize.SetString ("データ準備中");
			} else {
				string strMessage = string.Format ("データダウンロード中\n({0}/{1})", iNow, iTotal);
				m_waitingInitialize.SetString (strMessage);
			}


			break;

		case STEP.CHECK_COMIC_LIST:
			if (bInit) {
				m_iNetworkSerial = CommonNetwork.Instance.RecieveSpreadSheet (
					DataManagerAlarm.Instance.SPREAD_SHEET,
					DataManagerAlarm.Instance.config.Read ("comic_list"));
			}
			if (CommonNetwork.Instance.IsConnected (m_iNetworkSerial)) {
				TNetworkData data = EveryStudioLibrary.CommonNetwork.Instance.GetData (m_iNetworkSerial);
				m_ssdSample = EveryStudioLibrary.CommonNetwork.Instance.ConvertSpreadSheetData (data.m_dictRecievedData);
				CsvImage download_list = new CsvImage();
				download_list.Input (m_ssdSample);
				download_list.Save (DataManagerAlarm.Instance.FILENAME_COMIC_LIST);
				DataManagerAlarm.Instance.m_csvComic.Load (DataManagerAlarm.Instance.FILENAME_COMIC_LIST);

				DataManagerAlarm.Instance.data_kvs.Write (DataManagerAlarm.Instance.KEY_COMIC_LIST_VERSION, DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_COMIC_LIST_VERSION));
				DataManagerAlarm.Instance.data_kvs.Save (DataKvs.FILE_NAME);

				m_eStep = STEP.GOTO_GAME;
				if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_IMAGE_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_IMAGE_LIST_VERSION))) {
					m_eStep = STEP.CHECK_IMAGE_LIST;
				} else if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_VOICE_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_VOICE_LIST_VERSION))) {
					m_eStep = STEP.CHECK_VOICE_LIST;
				} else if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_VOICESET_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_VOICESET_LIST_VERSION))) {
					m_eStep = STEP.CHECK_VOICESET_LIST;
				}
			}
			break;

		case STEP.CHECK_IMAGE_LIST:
			if (bInit) {
				m_iNetworkSerial = CommonNetwork.Instance.RecieveSpreadSheet (
					DataManagerAlarm.Instance.SPREAD_SHEET,
					DataManagerAlarm.Instance.config.Read ("image_list"));
			}
			if (CommonNetwork.Instance.IsConnected (m_iNetworkSerial)) {
				TNetworkData data = EveryStudioLibrary.CommonNetwork.Instance.GetData (m_iNetworkSerial);
				m_ssdSample = EveryStudioLibrary.CommonNetwork.Instance.ConvertSpreadSheetData (data.m_dictRecievedData);
				CsvImage download_list = new CsvImage ();
				download_list.Input (m_ssdSample);
				download_list.Save (DataManagerAlarm.Instance.FILENAME_IMAGE_LIST);
				DataManagerAlarm.Instance.m_csvImage.Load (DataManagerAlarm.Instance.FILENAME_IMAGE_LIST);

				DataManagerAlarm.Instance.data_kvs.Write (DataManagerAlarm.Instance.KEY_IMAGE_LIST_VERSION, DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_IMAGE_LIST_VERSION));
				DataManagerAlarm.Instance.data_kvs.Save (DataKvs.FILE_NAME);

				m_eStep = STEP.GOTO_GAME;
				if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_VOICE_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_VOICE_LIST_VERSION))) {
					m_eStep = STEP.CHECK_VOICE_LIST;
				} else if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_VOICESET_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_VOICESET_LIST_VERSION))) {
					m_eStep = STEP.CHECK_VOICESET_LIST;
				}
			}
			break;

		case STEP.CHECK_VOICE_LIST:
			if (bInit) {
				m_iNetworkSerial = CommonNetwork.Instance.RecieveSpreadSheet (
					DataManagerAlarm.Instance.SPREAD_SHEET,
					DataManagerAlarm.Instance.config.Read ("voice_list"));
			}
			if (CommonNetwork.Instance.IsConnected (m_iNetworkSerial)) {
				TNetworkData data = EveryStudioLibrary.CommonNetwork.Instance.GetData (m_iNetworkSerial);
				m_ssdSample = EveryStudioLibrary.CommonNetwork.Instance.ConvertSpreadSheetData (data.m_dictRecievedData);
				CsvVoice server_list = new CsvVoice ();
				server_list.Input (m_ssdSample);
				server_list.Save (DataManagerAlarm.Instance.FILENAME_VOICE_LIST);
				DataManagerAlarm.Instance.m_csvVoice.Load (DataManagerAlarm.Instance.FILENAME_VOICE_LIST);

				DataManagerAlarm.Instance.data_kvs.Write (DataManagerAlarm.Instance.KEY_VOICE_LIST_VERSION, DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_VOICE_LIST_VERSION));
				DataManagerAlarm.Instance.data_kvs.Save (DataKvs.FILE_NAME);

				m_eStep = STEP.GOTO_GAME;
				if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_VOICESET_LIST_VERSION).Equals (DataManagerAlarm.Instance.data_kvs.Read (DataManagerAlarm.Instance.KEY_VOICESET_LIST_VERSION))) {
					m_eStep = STEP.CHECK_VOICESET_LIST;
				}
			}
			break;
		case STEP.CHECK_VOICESET_LIST:
			if (bInit) {
				m_iNetworkSerial = CommonNetwork.Instance.RecieveSpreadSheet (
					DataManagerAlarm.Instance.SPREAD_SHEET,
					DataManagerAlarm.Instance.config.Read ("voiceset_list"));
			}
			if (CommonNetwork.Instance.IsConnected (m_iNetworkSerial)) {
				TNetworkData data = EveryStudioLibrary.CommonNetwork.Instance.GetData (m_iNetworkSerial);
				m_ssdSample = EveryStudioLibrary.CommonNetwork.Instance.ConvertSpreadSheetData (data.m_dictRecievedData);
				CsvVoiceset download_list = new CsvVoiceset ();
				download_list.Input (m_ssdSample);
				download_list.Save (DataManagerAlarm.Instance.FILENAME_VOICESET_LIST);
				DataManagerAlarm.Instance.m_csvVoiceset.Load (DataManagerAlarm.Instance.FILENAME_VOICESET_LIST);
				DataManagerAlarm.Instance.data_kvs.Write (DataManagerAlarm.Instance.KEY_VOICESET_LIST_VERSION, DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_VOICESET_LIST_VERSION));
				DataManagerAlarm.Instance.data_kvs.Save (DataKvs.FILE_NAME);
				m_eStep = STEP.GOTO_GAME;
			}
			break;

		case STEP.GOTO_GAME:
			if (bInit) {
			}

			if (1.5f < m_fDelayTimer) {
				SceneManager.LoadScene ("main");
				m_eStep = STEP.END;
			}
			break;

		case STEP.RETRY:
			if (bInit) {
				m_waitingInitialize.SetString ("データ更新に失敗しました\n通信が出来る場所で再起動してください");
			}
			break;
		case STEP.END:
		default:
			if (bInit) {
			}
			break;
		}
	}
}
