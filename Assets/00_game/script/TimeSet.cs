using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TimeSet : OtherPage {

	public UIGrid m_Grid;

	public TimeEdit m_TimeEdit;

	public UIScrollView m_scrollView;

	public List<BannerTimer> m_BannerTimerList = new List<BannerTimer> ();

	public void DisplayReflresh(){

		foreach (BannerTimer bt in m_BannerTimerList) {
			Destroy (bt.gameObject);
		}

		m_BannerTimerList.Clear ();
		GameMain.Instance.m_AlarmData.Load (AlarmData.FILENAME);
		GameObject prefBannerTimer = PrefabManager.Instance.PrefabLoadInstance ("prefab/BannerTimer");

		foreach (AlarmParam param in GameMain.Instance.m_AlarmData.list) {

			// マイナスは非表示
			if (0 <= param.m_status) {
				if (0 != param.m_status) {
					if (param.repeat_type == 0) {
						/*
					DateTime checkDate = TimeManager.Instance.MakeDateTime (param.time);
					string strCheckDate = string.Format ("{0}-{1:D2}-{2:D2} {3:D2}:{4:D2}:00", datetimeNow.Year, datetimeNow.Month, datetimeNow.Day, checkDate.Hour, checkDate.Minute);
					*/
						if (TimeManager.Instance.GetDiffNow (param.time).TotalSeconds < 0) {
							param.m_status = 0;
							Debug.LogError (string.Format ("close(serial:{0}", param.serial));
						}
					}
				}

				GameObject obj = PrefabManager.Instance.MakeObject ("prefab/BannerTimer", m_Grid.gameObject);
				BannerTimer bt = obj.GetComponent<BannerTimer > ();
				bt.Initialize (param);

				m_BannerTimerList.Add (bt);
			}
		}
		if (6 <= m_BannerTimerList.Count) {
			m_scrollView.enabled = true;
		} else {
			m_scrollView.enabled = false;
		}
		GameMain.Instance.m_AlarmData.Save ();
		m_Grid.enabled = true;
		return;
	}

	public override void Initialize ()
	{
		base.Initialize ();
		m_TimeEdit.Initialize ();
	}
	public override void InStart ()
	{
		base.InStart ();
		DisplayReflresh ();
	}

	void Update(){

		if (ButtonPushed) {
			//Debug.Log ("Pushed");
			if (Index == 0) {
				OutStart ();
				GameMain.Instance.reserveTimeReset ();
				GameMain.Instance.TimeSetRefresh ();
			} else if (Index == 1) {
				GameMain.Instance.EditingAlarmParam = new AlarmParam ();
				GameMain.Instance.EditingAlarmParam.time = TimeManager.StrGetTime ();
				GameMain.Instance.EditingAlarmParam.repeat_type = 0;
				GameMain.Instance.EditingAlarmParam.snooze = 2;
				GameMain.Instance.EditingAlarmParam.voice_type = DataManagerAlarm.Instance.master_voice_list[0].id;
				m_TimeEdit.InStart (GameMain.Instance.EditingAlarmParam);

			}
			TriggerClearAll ();
		}

		foreach (BannerTimer bt in m_BannerTimerList) {
			if (bt.ButtonPushed) {
				bt.TriggerClear ();

				Debug.Log( bt.m_AlarmParam.time);
				GameMain.Instance.EditingAlarmParam = bt.m_AlarmParam;
				m_TimeEdit.InStart (GameMain.Instance.EditingAlarmParam);
			}
		}

	}





}
