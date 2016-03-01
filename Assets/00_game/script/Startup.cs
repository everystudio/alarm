using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using EveryStudioLibrary;

public class Startup : Singleton<Startup> {

	static public bool InitializeCheck = false;

	public enum STEP
	{
		NONE				= 0,
		CHECK_CONFIG		,
		CHECK_SCENARIO		,
		CHECK_DOWNLOAD		,
		DATA_DOWNLOAD		,
		CHECK_COMIC_LIST	,
		CHECK_IMAGE_LIST	,
		GOTO_GAME			,
		END					,
		MAX					,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;
	public int m_iNetworkSerial;
	public List<SpreadSheetData> m_ssdSample;
	public CsvScript m_scriptData;

	private FileDownload fd;

	public override void Initialize ()
	{
		base.Initialize ();
		InitializeCheck = true;
		m_eStep = STEP.CHECK_CONFIG;
		m_eStepPre = STEP.MAX;
		return;
	}

	void Update () {
		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
			Debug.Log (m_eStep);
		}
		switch (m_eStep) {
		case STEP.CHECK_CONFIG:
			if (bInit) {
				m_iNetworkSerial = CommonNetwork.Instance.RecieveSpreadSheet ("1ih_CiMkZU0VkylrxDfs1gCstGmhEh6oTp4b-o_aPWSQ" , "od6");
			}
			if (CommonNetwork.Instance.IsConnected (m_iNetworkSerial)) {
				TNetworkData data = EveryStudioLibrary.CommonNetwork.Instance.GetData (m_iNetworkSerial);
				m_ssdSample = EveryStudioLibrary.CommonNetwork.Instance.ConvertSpreadSheetData (data.m_dictRecievedData);
				CsvConfig config_data = new CsvConfig ();
				config_data.Input (m_ssdSample);
				config_data.Save (DataManagerAlarm.Instance.FILENAME_CONFIG);

				if (false == config_data.Read (DataManagerAlarm.Instance.KEY_CONFIG_VERSION).Equals (DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_CONFIG_VERSION))) {
					config_data.Save (DataManagerAlarm.Instance.FILENAME_CONFIG);
					DataManagerAlarm.Instance.config.Load (DataManagerAlarm.Instance.FILENAME_CONFIG);
				}
				m_eStep = STEP.GOTO_GAME;
				Debug.LogError (DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_DOWNLOAD_VERSION));
				Debug.LogError (DataManagerAlarm.Instance.kvs.Read (DataManagerAlarm.Instance.KEY_DOWNLOAD_VERSION));
				if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_DOWNLOAD_VERSION).Equals (DataManagerAlarm.Instance.kvs.Read (DataManagerAlarm.Instance.KEY_DOWNLOAD_VERSION))) {
					m_eStep = STEP.CHECK_DOWNLOAD;
				} else if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_COMIC_LIST_VERSION).Equals (DataManagerAlarm.Instance.kvs.Read (DataManagerAlarm.Instance.KEY_COMIC_LIST_VERSION))) {
					m_eStep = STEP.CHECK_COMIC_LIST;
				} else if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_IMAGE_LIST_VERSION).Equals (DataManagerAlarm.Instance.kvs.Read (DataManagerAlarm.Instance.KEY_IMAGE_LIST_VERSION))) {
					m_eStep = STEP.CHECK_IMAGE_LIST;
				}
			}
			break;

		case STEP.CHECK_DOWNLOAD:
			if (bInit) {
				m_iNetworkSerial = CommonNetwork.Instance.RecieveSpreadSheet (
					"1ih_CiMkZU0VkylrxDfs1gCstGmhEh6oTp4b-o_aPWSQ",
					DataManagerAlarm.Instance.config.Read ("download"));
			}
			if (CommonNetwork.Instance.IsConnected (m_iNetworkSerial)) {
				TNetworkData data = EveryStudioLibrary.CommonNetwork.Instance.GetData (m_iNetworkSerial);
				m_ssdSample = EveryStudioLibrary.CommonNetwork.Instance.ConvertSpreadSheetData (data.m_dictRecievedData);
				CsvDownload download_list = new CsvDownload();
				download_list.Input (m_ssdSample);
				download_list.Save (DataManagerAlarm.Instance.FILENAME_DOWNLOAD_LIST);
				m_eStep = STEP.DATA_DOWNLOAD;
			}
			break;

		case STEP.DATA_DOWNLOAD:
			if (bInit) {
				CsvDownload download_list = new CsvDownload ();
				download_list.Load (DataManagerAlarm.Instance.FILENAME_DOWNLOAD_LIST);
				Debug.Log (TimeManager.StrGetTime ());
				FileDownloadManager.Instance.Download (download_list.list);
			}
			if (FileDownloadManager.Instance.IsIdle ()) {
				Debug.Log (TimeManager.StrGetTime ());
				DataManagerAlarm.Instance.kvs.Write (DataManagerAlarm.Instance.KEY_DOWNLOAD_VERSION, DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_DOWNLOAD_VERSION));
				DataManagerAlarm.Instance.kvs.Save (DataManagerAlarm.Instance.FILENAME_KVS);
				m_eStep = STEP.GOTO_GAME;
				if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_COMIC_LIST_VERSION).Equals (DataManagerAlarm.Instance.kvs.Read (DataManagerAlarm.Instance.KEY_COMIC_LIST_VERSION))) {
					m_eStep = STEP.CHECK_COMIC_LIST;
				} else if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_IMAGE_LIST_VERSION).Equals (DataManagerAlarm.Instance.kvs.Read (DataManagerAlarm.Instance.KEY_IMAGE_LIST_VERSION))) {
					m_eStep = STEP.CHECK_IMAGE_LIST;
				}
			}
			break;

		case STEP.CHECK_COMIC_LIST:
			if (bInit) {
				m_iNetworkSerial = CommonNetwork.Instance.RecieveSpreadSheet (
					"1ih_CiMkZU0VkylrxDfs1gCstGmhEh6oTp4b-o_aPWSQ",
					DataManagerAlarm.Instance.config.Read ("comic_list"));
			}
			if (CommonNetwork.Instance.IsConnected (m_iNetworkSerial)) {
				TNetworkData data = EveryStudioLibrary.CommonNetwork.Instance.GetData (m_iNetworkSerial);
				m_ssdSample = EveryStudioLibrary.CommonNetwork.Instance.ConvertSpreadSheetData (data.m_dictRecievedData);
				CsvImage download_list = new CsvImage();
				download_list.Input (m_ssdSample);
				download_list.Save (DataManagerAlarm.Instance.FILENAME_COMIC_LIST);
				DataManagerAlarm.Instance.m_csvComic.Load (DataManagerAlarm.Instance.FILENAME_COMIC_LIST);
				m_eStep = STEP.GOTO_GAME;
				if (false == DataManagerAlarm.Instance.config.Read (DataManagerAlarm.Instance.KEY_IMAGE_LIST_VERSION).Equals (DataManagerAlarm.Instance.kvs.Read (DataManagerAlarm.Instance.KEY_IMAGE_LIST_VERSION))) {
					m_eStep = STEP.CHECK_IMAGE_LIST;
				}
			}
			break;

		case STEP.CHECK_IMAGE_LIST:
			if (bInit) {
				m_iNetworkSerial = CommonNetwork.Instance.RecieveSpreadSheet (
					"1ih_CiMkZU0VkylrxDfs1gCstGmhEh6oTp4b-o_aPWSQ",
					DataManagerAlarm.Instance.config.Read ("image_list"));
			}
			if (CommonNetwork.Instance.IsConnected (m_iNetworkSerial)) {
				TNetworkData data = EveryStudioLibrary.CommonNetwork.Instance.GetData (m_iNetworkSerial);
				m_ssdSample = EveryStudioLibrary.CommonNetwork.Instance.ConvertSpreadSheetData (data.m_dictRecievedData);
				CsvImage download_list = new CsvImage ();
				download_list.Input (m_ssdSample);
				download_list.Save (DataManagerAlarm.Instance.FILENAME_IMAGE_LIST);
				DataManagerAlarm.Instance.m_csvImage.Load (DataManagerAlarm.Instance.FILENAME_IMAGE_LIST);
				m_eStep = STEP.GOTO_GAME;
			}
			break;

		case STEP.GOTO_GAME:
			if (bInit) {
				SceneManager.LoadScene ("main");
			}
			break;
		}
	}
}
