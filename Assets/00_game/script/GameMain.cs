using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Prime31;

public class GameMain : PageBase {

	private static GameMain instance;
	public static GameMain Instance {
		get {
			if (instance == null) {
				GameObject obj = GameObject.Find ("GameMain");
				if (obj == null) {
					obj = new GameObject("GameMain");
					//Debug.LogError ("Not Exist AtlasManager!!");
				}
				instance = obj.GetComponent<GameMain> ();
				if (instance == null) {
					//Debug.LogError ("Not Exist AtlasManager Script!!");
					instance = obj.AddComponent<GameMain>() as GameMain;
				}
			}
			return instance;
		}
	}

	public AlarmMain m_AlarmMain;
	public AlarmParam EditingAlarmParam;
	public AlarmData m_AlarmData = new AlarmData ();

	public VoiceMain m_VoiceMain;
	public void Purchase( string _strProductId ){
		m_VoiceMain.Purchase (_strProductId);
	}

	public KvsData kvs_data{
		get{ 
			if (m_KvsData == null) {

				m_KvsData = new KvsData ();

			}
			return m_KvsData;
		}
	}
	public KvsData m_KvsData;

	public PageBase m_PageNow;
	public List<PageBase> m_PageBaseList = new List<PageBase> ();



	public PageFooter m_PageFooter;
	public int m_iPagePre;

	public TimeSet m_TimeSet;
	public void TimeSetRefresh(){
		m_TimeSet.DisplayReflresh ();
	}

	public void timeRefreshFromToday(){

		string strNow = TimeManager.StrNow ();
		DateTime dateTime = TimeManager.GetNow ();
		foreach( AlarmParam alarm in m_AlarmData.list ){
			TimeSpan ts = TimeManager.Instance.GetDiff (strNow , alarm.time);
			if (0 < ts.Seconds) {
			} else {
			}

		}




	}

	public void reloadTime(AlarmParam _param){
		if (_param.status == 0) {
			return;
		}
	}

	public AlarmParam GetNearParam(){

		int iSerial = 0;

		return new AlarmParam ();


	}
	public List<AlarmReserve> reserve_list = new List<AlarmReserve> ();

	public void setupAlarmReserve( ref List<AlarmReserve> _insertList , List<AlarmParam> _alarmList ){

		LocalNotificationManager.Instance.ClearLocalNotification ();
		_insertList.Clear ();

		DateTime datetimeNow = TimeManager.GetNow();
		foreach (AlarmParam param in _alarmList) {
			if ( param.status <= 0) {
				continue;
			}
			//Debug.Log ( string.Format( "serial:{0} repeat_type:{1}",param.serial,  param.repeat_type));
			if (param.repeat_type == 0) {
				DateTime checkDate = TimeManager.Instance.MakeDateTime (param.time);
				//string strCheckDate = string.Format ("{0}-{1:D2}-{2:D2} {3:D2}:{4:D2}:00", datetimeNow.Year, datetimeNow.Month, datetimeNow.Day, checkDate.Hour, checkDate.Minute);
				//Debug.Log (TimeManager.Instance.GetDiffNow (strCheckDate).TotalSeconds);
				string strCheckDate = param.time;
				TimeSpan time_span = TimeManager.Instance.GetDiffNow (strCheckDate);
				AlarmReserve insert_data = new AlarmReserve ();
				if (0 < time_span.TotalSeconds) {
					insert_data.m_strTime = strCheckDate;
					insert_data.m_iVoiceType = param.voice_type;
					insert_data.m_iSnoozeType = param.snooze;
					insert_data.m_lTime = (long)TimeManager.Instance.GetDiffNow (insert_data.m_strTime).TotalSeconds;
					_insertList.Add (insert_data);
				} else {

				}
				/*
				 * こっちは旧式
				else {

					DateTime tomorrowDateTime = TimeManager.GetNow();
					tomorrowDateTime = tomorrowDateTime.AddDays (1);
					string strTomorrow = string.Format ("{0}-{1:D2}-{2:D2} {3:D2}:{4:D2}:00", tomorrowDateTime.Year, tomorrowDateTime.Month, tomorrowDateTime.Day, checkDate.Hour, checkDate.Minute);
					insert_data.m_strTime = strTomorrow;
				}
				insert_data.m_iVoiceType = param.voice_type;
				insert_data.m_lTime = (long)TimeManager.Instance.GetDiffNow (insert_data.m_strTime).TotalSeconds;
				_insertList.Add (insert_data);
				*/
			} else {
				int iNowWeek = TimeManager.Instance.GetWeekIndex (TimeManager.StrGetTime ());

				for (int i = 0; i < DataManagerAlarm.Instance.STR_WEEK_ARR.Length; i++) {
					if (0 < (param.repeat_type & (1<<i))) {
						// 曜日にひっかかった
						string strStartDate = "";
						int iOffset = i - iNowWeek;
						DateTime checkDate = TimeManager.Instance.MakeDateTime (param.time);
						if (iOffset == 0) {
							string strCheckDate = string.Format ("{0}-{1:D2}-{2:D2} {3:D2}:{4:D2}:00", datetimeNow.Year, datetimeNow.Month, datetimeNow.Day, checkDate.Hour, checkDate.Minute);
							TimeSpan time_span = TimeManager.Instance.GetDiffNow (strCheckDate);
							if (0 < time_span.TotalSeconds) {
							} else {
								iOffset = 7;
							}
						} else if (iOffset < 0) {
							iOffset += DataManagerAlarm.Instance.STR_WEEK_ARR.Length;
						} else {
							// そのまま
						}
						DateTime nextDateTime = TimeManager.GetNow();
						nextDateTime = nextDateTime.AddDays (iOffset);

						for (int count = 0; count < 10; count++) {
							string strNext = string.Format ("{0}-{1:D2}-{2:D2} {3:D2}:{4:D2}:00", nextDateTime.Year, nextDateTime.Month, nextDateTime.Day, checkDate.Hour, checkDate.Minute);
							strStartDate = strNext;
							AlarmReserve insert_data = new AlarmReserve ();
							insert_data.m_strTime = strNext;
							insert_data.m_iVoiceType = param.voice_type;
							insert_data.m_iSnoozeType = param.snooze;
							insert_data.m_lTime = (long)TimeManager.Instance.GetDiffNow (insert_data.m_strTime).TotalSeconds;
							_insertList.Add (insert_data);

							// 次の準備
							nextDateTime = nextDateTime.AddDays (7);
						}
					}
				}
			}
			_insertList.Sort ((a, b) => (int)(a.m_lTime - b.m_lTime));
		}

		foreach (AlarmReserve reserve in _insertList) {
			int iLoop = 1;

			long lOffset = 0;
			switch (reserve.m_iSnoozeType) {
			case 1:
				iLoop = 10;
				lOffset = 5 * 60;
				break;
			case 2:
				iLoop = 5;
				lOffset = 10 * 60;
				break;
			default:
				iLoop = 1;
				lOffset = 0;
				break;
			}

			for (int i = 0; i < iLoop; i++) {
				LocalNotificationManager.Instance.AddLocalNotification (
					reserve.m_lTime + i*lOffset,
					ConfigManager.Instance.GetEditPlayerSettingsData ().projectData.m_strProductName,
					"時刻になりました",
					GetAssetName (reserve.m_iVoiceType, true)
				);
			}
		}

		return;
	}


	public void reserveTimeReset(){
		m_AlarmData.Load (AlarmData.FILENAME);
		setupAlarmReserve (ref reserve_list, m_AlarmData.list);
		m_AlarmMain.setNextTimer (reserve_list);

	}
	void Start(){
		//Screen.fullScreen = false;
		instance = this;
		EditingAlarmParam = new AlarmParam ();
		m_iPagePre = 0;

		string key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwoEtQFcqjLFQJ0wXu8mkFjowH8t4I7tcG1G6Ais7Vx8qZWYidwNPzdp2pvPCQS4/BZDgtRyk1+FsPbaCOndof2e4OlVmdlGUXVQOtJl5hT40xxmlotliBG9IzO1A5Huvy0tjv2pQ6Et0g72k1qxJPFI1O/L7mzQDHPzawYEqHv47U/yGD1GTE6jHK0u1apgxUI89UJsiYIhVlwdZ40390LGWAR8+LrUhk+q//NYjxfKBd3fotgV4QZecNPQks1fz9bk5oWOwOpOz2pQ3aZ62RInlueAk8ttsfow6+M4rmdfBDVGOkVKgScwhBjeCAcsXQaO+qwWdr1GhLPNuYck39wIDAQAB";

		GoogleIAB.init (key);

		if (m_AlarmData == null) {
			m_AlarmData = new AlarmData ();
		}
		reserveTimeReset ();

		kvs_data.Load (KvsData.FILE_NAME);
		int iTest = kvs_data.ReadInt ("test");
		iTest += 1;
		kvs_data.WriteInt ("test", iTest );
		kvs_data.Save ();
		m_PageNow = m_PageBaseList [m_iPagePre];
		InitPage (m_PageNow , m_iPagePre);
	}

	public float m_fCheckIntervalTime;

	void Update(){

		if (m_PageFooter.ButtonPushed) {
			if (m_iPagePre != m_PageFooter.Index) {

				ClosePage (m_PageNow, m_iPagePre);
				m_PageNow = m_PageBaseList [m_PageFooter.Index];
				InitPage (m_PageNow  , m_PageFooter.Index);
				m_iPagePre = m_PageFooter.Index;
			}
			m_PageFooter.TriggerClearAll ();
		}

		m_fCheckIntervalTime += Time.deltaTime;
		if (2.0f < m_fCheckIntervalTime) {
			m_fCheckIntervalTime -= 2.0f;
			RemoveAlarm (true);
		}
	}

	public void RemoveAlarm( bool _bCall ){
		List <AlarmReserve> remove_list = new List<AlarmReserve>();
		List<int> remove_index_list = new List<int> ();
		int iRemoveNum = 0;

		int iCount = 0;
		foreach (AlarmReserve reserve_param in reserve_list) {
			if (TimeManager.Instance.GetDiffNow (reserve_param.m_strTime).TotalSeconds < 0 ) {
				iRemoveNum += 1;
				remove_list.Add (reserve_param);
			}
		}

		int voice_type = 0;
		foreach( AlarmReserve remove_param in remove_list ){
			voice_type = remove_param.m_iVoiceType;
			reserve_list.Remove( remove_param );
		}

		if (0 < iRemoveNum) {
			Debug.LogError ("remove");
			m_AlarmMain.setNextTimer (reserve_list);

			if (_bCall) {
				CallVoice (voice_type);
			}
		}

		return;
	}

	public string GetAssetName(int _iVliceType , bool _bAddPath = false ){
		List<CsvVoicesetData> sound_list = new List<CsvVoicesetData> ();

		foreach (CsvVoicesetData data in DataManagerAlarm.Instance.master_voiceset_list) {
			if (_iVliceType == data.id) {
				sound_list.Add (data);
			}
		}
		int iIndex = UtilRand.GetRand (sound_list.Count);

		CsvVoicesetData use_data = sound_list [iIndex];

		string strRet = "";
		if (_bAddPath == false) {
			strRet = use_data.name;
		} else {
			strRet = string.Format( "{0}/{1}.{2}" , use_data.path , use_data.name , use_data.kakucho );
		}
		return strRet;
	}

	public void CallVoice( int _iVoiceType ){
		string strFilename = GetAssetName (_iVoiceType);
		SoundManager.Instance.PlaySE ( strFilename );
		return;
	}
	void OnApplicationPause(bool pauseStatus) {
		if (pauseStatus == false) {
			RemoveAlarm (false);
		}
	}

	public void InitPage( PageBase _pageBase , int _iPageIndex ){
		//_obj.SetActive (true);
		_pageBase.gameObject.transform.localPosition = Vector3.zero;
		_pageBase.Initialize ();

		m_PageFooter.SetIndex (_iPageIndex);
	}

	public void ClosePage(PageBase _pageBase , int _iIndex ){
		//_obj.SetActive (false);
		_pageBase.gameObject.transform.localPosition = new Vector3( 640.0f , 1136.0f * (2 - _iIndex ) , 0.0f );
		_pageBase.Close ();
	}



}
