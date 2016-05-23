using UnityEngine;
using System;
using System.Collections.Generic;

public class Test001 : MonoBehaviour
{
#if UNITY_ANDROID
#if UNITY_ANDROID && !UNITY_EDITOR
	static AndroidJavaObject m_plugin = null;
	static AndroidJavaObject m_plugin2 = null;
#endif
	//static GameObject m_instance;

	public void Awake ()
	{
		// gameObject変数はstaticでないのでstatic関数から呼ぶことが出来ない.
		// そのためstaticの変数にあらかじめコピーしておく.

		Debug.Log ("Test001 Awake() here");
		//m_instance = gameObject;
#if UNITY_ANDROID && !UNITY_EDITOR
			// プラグイン名をパッケージ名+クラス名で指定する。
		m_plugin = new AndroidJavaObject( "com.everystudio.test001.TestClass" );
		m_plugin2 = new AndroidJavaObject( "com.everystudio.test001.TestLocalnotification" );
#endif
	}
	//UNIXエポック時刻
	public readonly static DateTime dtUnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	void Start(){
		CallFuncA ("callfuncA");
		CallFuncB ( "callfuncB");
		CallFuncC (gameObject.name,"callfuncC");

		//現在時刻のDateTimeオブジェクト
		DateTime dt = DateTime.Now;
		//UTC時刻に変更
		dt = dt.ToUniversalTime();
#if UNITY_ANDROID && !UNITY_EDITOR
		long unix_time = (long)dt.Subtract(dtUnixEpoch).TotalSeconds - 5;
		m_plugin2.Call ("test");
		m_plugin2.Call ("sendNotification", unix_time, 1, "testTitle", "testText");
#endif
	}

	// NativeコードのFuncA 関数を呼び出す.
	// Native側のコードが引数を持たない関数なら、
	// m_plugin.Call("FuncA"); のように引数を省略すればOK。
	public static void CallFuncA (string str)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
			if (m_plugin != null){
		Debug.Log ("calledA");
			m_plugin.Call("FuncA", str);
			}
#endif
	}
	// NativeコードのFuncB 関数を呼び出す.
	public static string CallFuncB (string str)
	{
		string modoriValue = null;
#if UNITY_ANDROID && !UNITY_EDITOR
			if (m_plugin != null){
		Debug.Log ("calledB");
			modoriValue = m_plugin.Call<string>("FuncB", str);
		Debug.Log(modoriValue);
		}
#endif
		return modoriValue;
	}
	// NativeコードのFuncC 関数を呼び出す.
	public static void CallFuncC (string _filename , string str)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
			if (m_plugin != null){
		Debug.Log (string.Format("calledC:{0}" ,_filename));
		m_plugin.Call("FuncC", _filename , str);
			}
#endif
	}
	// ネイティブコードから呼ばれる関数
	// publicでかつ、非static関数でないと呼ばれない.
	public void onCallBack (string str)
	{
		Debug.Log ("Call From Native. (" + str + ")");
	}
#endif
	}














