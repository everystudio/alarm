using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBtnAlarmAdd : UIShow {

	protected override void on_action()
	{
		base.on_action();
		GameMain.Instance.EditingAlarmParam = new AlarmParam();
		GameMain.Instance.EditingAlarmParam.time = TimeManager.StrGetTime();
		GameMain.Instance.EditingAlarmParam.repeat_type = 0;
		GameMain.Instance.EditingAlarmParam.snooze = 2;
		GameMain.Instance.EditingAlarmParam.voice_type = DataManagerAlarm.Instance.master_voice_list[0].id;

	}
}
