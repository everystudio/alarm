using UnityEngine;
using System.Collections;

public class DebugLogger {

	public static void Log(string message) {
#if DEBUG
		Debug.Log (message);
#endif
	}
}
