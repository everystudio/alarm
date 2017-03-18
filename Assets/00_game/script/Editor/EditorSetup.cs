using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class EditorSetup
{
	[MenuItem("Setup/Copy")]
	static public void BuildSetupCopy()
	{
		Debug.LogError("BuildSetupCopy");
#if UNITY_IOS
		EditDirectory.AddCopy ("Assets/00_game/alarm.Resources/sound", "Assets/StreamingAssets");
#else
		EditDirectory.AddCopy("Assets/00_game/alarm.Resources/sound", "Assets/Plugins/Android/AN_Res/res/raw");
		EditDirectory.AddCopy("Assets/00_game/alarm.Resources/drawable", "Assets/Plugins/Android/AN_Res/res/drawable");
#endif
	}
	[MenuItem("Setup/Remove")]
	static public void BuildSetupRemove()
	{
		Debug.LogError("BuildSetupRemove");
#if UNITY_IOS
		EditDirectory.Delete ("Assets/StreamingAssets");
#else
		File.Delete("Assets/Plugins/Android/AN_Res/res/drawable/notice_icon.png");
		EditDirectory.Delete("Assets/Plugins/Android/AN_Res/res/raw");
#endif
	}
}
