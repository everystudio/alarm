using UnityEditor;
using System.Collections;

public class BuildCustom {

	public static void PreBuild ()
	{
		#if UNITY_IOS
		EditDirectory.AddCopy ("Assets/00_game/alarm.Resources/sound", "Assets/StreamingAssets");
		#else
		EditDirectory.AddCopy ("Assets/00_game/alarm.Resources/sound", "Assets/Plugins/Android/AN_Res/res/raw");
		EditDirectory.AddCopy ("Assets/00_game/alarm.Resources/drawable", "Assets/Plugins/Android/AN_Res/res/drawable");
		#endif
		return;
	}

	public static void AfterBuild ()
	{
		#if UNITY_IOS
		EditDirectory.Delete ("Assets/StreamingAssets");
		#else
		EditDirectory.Delete ("Assets/Plugins/Android/AN_Res/res/raw");
		#endif
		return;
	}


}
