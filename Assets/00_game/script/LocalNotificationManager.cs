﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LocalNotificationManager : MonoBehaviour {

	public List<string> add_message_list = new List<string>();

	public List<int> id_list = new List<int>();
	#if UNITY_ANDROID
	static AndroidJavaObject m_plugin2 = null;
	#endif
	protected static LocalNotificationManager instance = null;
	public static LocalNotificationManager Instance {
		get {
			if (instance == null) {
				GameObject obj = GameObject.Find ("LocalNotificationManager");
				if (obj == null) {
					obj = new GameObject("LocalNotificationManager");
					//Debug.LogError ("Not Exist AtlasManager!!");
				}
				instance = obj.GetComponent<LocalNotificationManager> ();
				if (instance == null) {
					//Debug.LogError ("Not Exist AtlasManager Script!!");
					instance = obj.AddComponent<LocalNotificationManager>() as LocalNotificationManager;
				}
				instance.Initialize ();
			}
			return instance;
		}
	}

	public int m_iLocalNotificationIndex;
	public void AddLocalNotification( long _lTime , string _strTitle , string _strMessage ){
		if (m_plugin2 != null) {
			m_iLocalNotificationIndex += 1;
			m_plugin2.Call ("sendNotification", _lTime, m_iLocalNotificationIndex, _strTitle, _strMessage);
			Debug.LogError (string.Format( "time:{0} index{1} title{2}", _lTime, m_iLocalNotificationIndex, _strTitle));
		} else {
			Debug.LogError ("null m_plugin2");
		}
	}

	public void ClearLocalNotification(){
		if (m_plugin2 != null) {
			for (int i = 0; i < m_iLocalNotificationIndex; i++) {
				m_plugin2.Call ("clearNotification", i + 1);
			}
		}
		m_iLocalNotificationIndex = 0;
	}

	private void Initialize(){
		DontDestroyOnLoad(gameObject);
		localnotificate_list.Clear ();
		add_message_list.Clear ();
		id_list.Clear ();
		#if UNITY_ANDROID && !UNITY_EDITOR
		// プラグイン名をパッケージ名+クラス名で指定する。
		m_plugin2 = new AndroidJavaObject( "com.everystudio.test001.TestLocalnotification" );
		#endif

	}

	/*
	public bool Add( CsvLocalNotificationData _data ){

		bool bHit = false;
		foreach (CsvLocalNotificationData data in m_localNotificationDataList) {
			if (data.id == _data.id) {
				bHit = true;
			}
		}
		if (bHit == false) {
			m_localNotificationDataList.Add (_data);
			add_message_list.Add (_data.message);

			//Debug.Log (string.Format ("second:{0} message{1}", _data.second, _data.message));
		}
		return bHit;
	}
	*/

	private List<int> localnotificate_list = new List<int> ();
	void OnApplicationPause(bool pauseStatus) {
		// ローカル通知用
		if (pauseStatus) {
			//TODO
			#if UNITY_IPHONE
			foreach( CsvLocalNotificationData data in m_localNotificationDataList ){
				ISN_LocalNotification local_notification = new ISN_LocalNotification (
					DateTime.Now.AddSeconds (data.second),
					data.message,
					false);

				id_list.Add( local_notification.Id );
				IOSNotificationController.Instance.ScheduleNotification (local_notification);
			}

			#elif UNITY_ANDROID

			/*
			int iTemp = 0;
			foreach( CsvLocalNotificationData data in m_localNotificationDataList ){
				m_plugin2.Call ("sendNotification", (long)data.second , iTemp , data.title, data.message);
				iTemp += 1;
			}
			*/
			#endif
		} else {
			#if UNITY_IPHONE
			// こっちの削除はなくてもいいらしい
			foreach( int set_id in id_list ){
				IOSNotificationController.Instance.CancelLocalNotificationById( set_id );
			}
			//IOSNotificationController.Instance.CancelAllLocalNotifications ();
			#elif UNITY_ANDROID
			/*
			int iTemp = 0;
			foreach( CsvLocalNotificationData data in m_localNotificationDataList ){
				m_plugin2.Call ("clearNotification", iTemp );
				iTemp += 1;
			}
			*/
			#endif
		}




	}
}
