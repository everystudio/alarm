using UnityEngine;
using System.Collections;

public class TimeComing : ButtonBase {

	public void Appear(){
		TweenAlphaAll (gameObject, 0.3f, 1.0f);
		TriggerClear ();
	}

	public void Disappear(){
		TweenAlphaAll (gameObject, 0.3f, 0.0f);

		LocalNotificationManager.Instance.sound_stop ();
	}

}
