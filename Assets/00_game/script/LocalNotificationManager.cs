using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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
	public IEnumerator load (string _source , string _temp ){
		Debug.Log ("call");
		WWW www = new WWW(_source);
		yield return www;

		string toPath = _temp;
		File.WriteAllBytes(toPath, www.bytes);
	}

	public int m_iLocalNotificationIndex;
	public void AddLocalNotification( long _lTime , string _strTitle , string _strMessage , string _strSoundName ){
		m_iLocalNotificationIndex += 1;

		int notifi_id = id_list.Count + 1;
		#if UNITY_ANDROID

		_strSoundName = _strSoundName.Replace(".mp3","");
		AndroidNotificationBuilder builder = new AndroidNotificationBuilder (notifi_id, _strTitle, _strMessage, (int)_lTime);
		builder.ShowIfAppIsForeground( true );
		builder.SetIconName("notice_icon");
		builder.SetSoundName (_strSoundName);
		builder.SetVibration(false);
		AndroidNotificationManager.instance.ScheduleLocalNotification (builder);
		id_list.Add (notifi_id);

		#elif UNITY_IOS
		ISN_LocalNotification local_notification = new ISN_LocalNotification (
			DateTime.Now.AddSeconds (_lTime),
			_strMessage,
			true);
		Debug.Log(_strSoundName);
		//_strSoundName = _strSoundName.Replace(".mp3","");
		local_notification.SetBadgesNumber(1);
		local_notification.SetData( "data" );
		local_notification.SetSoundName (_strSoundName);
		//local_notification.Schedule();
		id_list.Add( local_notification.Id );
		IOSNotificationController.Instance.ScheduleNotification (local_notification);
		#endif


		//m_plugin2.Call ("sendNotification", _lTime, m_iLocalNotificationIndex, _strTitle, _strTitle, _strMessage , _strSoundName );
		Debug.LogError (string.Format ("time:{0} index{1} title{2} sound_path:{3}", _lTime, m_iLocalNotificationIndex, _strTitle, _strSoundName));
	}

	const int MAX_LOCALNOTIFICATE_NUM = 100;
	public void ClearLocalNotification(){
		m_iLocalNotificationIndex = 0;
		#if UNITY_ANDROID
		foreach( int id in id_list ){
			AndroidNotificationManager.instance.CancelLocalNotification(id);
		}
		AndroidNotificationManager.instance.CancelAllLocalNotifications ();
		#elif UNITY_IOS
		foreach( int id in id_list ){
			IOSNotificationController.Instance.CancelLocalNotificationById(id );
		}
		IOSNotificationController.Instance.CancelAllLocalNotifications();
		#endif
		id_list.Clear ();
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

	public void sound_stop(){
		Debug.LogError ("call STOP");
	}

	private List<int> localnotificate_list = new List<int> ();
	void OnApplicationPause(bool pauseStatus) {
		// ローカル通知用
		if (pauseStatus) {

			//TODO
			#if UNITY_IPHONE
			/*
			foreach( CsvLocalNotificationData data in m_localNotificationDataList ){
				ISN_LocalNotification local_notification = new ISN_LocalNotification (
					DateTime.Now.AddSeconds (data.second),
					data.message,
					false);

				id_list.Add( local_notification.Id );
				IOSNotificationController.Instance.ScheduleNotification (local_notification);
			}
			*/
			#elif UNITY_ANDROID

			#endif
		} else {
			#if UNITY_IPHONE
			// こっちの削除はなくてもいいらしい
			/*
			foreach( int set_id in id_list ){
				IOSNotificationController.Instance.CancelLocalNotificationById( set_id );
			}
			*/
			//IOSNotificationController.Instance.CancelAllLocalNotifications ();
			#elif UNITY_ANDROID
			#endif
		}




	}
}
