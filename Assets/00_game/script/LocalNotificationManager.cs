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
		if (m_plugin2 != null) {
			m_iLocalNotificationIndex += 1;

			/*
			//string sound_path = EditPlayerSettingsData.GetStreamingAssetsAssetBundlePath () + "/sample.mp3";
			//sound_path = sound_path.Replace ("jar:file://", "");
			string sound_path = System.IO.Path.Combine (Application.streamingAssetsPath, "AssetBundles/Android/sample.mp3" );
			sound_path = System.IO.Path.Combine (Application.streamingAssetsPath, "AssetBundles/Android/sample.mp3" );
			Debug.Log (sound_path);
			string permissive = Application.persistentDataPath + "/sample.mp3";
			permissive = Application.persistentDataPath + "/AssetBundles/Android/sample.mp3";
			Debug.Log (permissive);

			if (File.Exists (permissive)) {
				Debug.LogError ("seikou:" + permissive );
			} else {
				Debug.LogError ("not found");
				StartCoroutine (load (sound_path, permissive));
			}
			*/

			//sound_path = "content://settings/system/ringtone";
			//sound_path = Application.persistentDataPath + "/sample.mp3";
			//m_plugin2.Call ("sendNotification", _lTime, m_iLocalNotificationIndex, _strTitle, _strTitle, _strMessage , sound_path );
			m_plugin2.Call ("sendNotification", _lTime, m_iLocalNotificationIndex, _strTitle, _strTitle, _strMessage , _strSoundName );
			Debug.LogError (string.Format( "time:{0} index{1} title{2} sound_path:{3}", _lTime, m_iLocalNotificationIndex, _strTitle , _strSoundName ));
		} else {
			Debug.LogError ("null m_plugin2");
		}
	}

	const int MAX_LOCALNOTIFICATE_NUM = 100;
	public void ClearLocalNotification(){
		if (m_plugin2 != null) {
			for (int i = 0; i < MAX_LOCALNOTIFICATE_NUM; i++) {
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

		#if UNITY_ANDROID && !UNITY_EDITOR
		m_plugin2.Call ("sendNotification", (long)1, 10, "dummy_title", "dummy_title", "dummy_message" , "stop" );
		//m_plugin2.Call ("sendNotification", _lTime, m_iLocalNotificationIndex, _strTitle, _strTitle, _strMessage , permissive );
		#endif
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
