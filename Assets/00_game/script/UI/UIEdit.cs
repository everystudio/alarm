using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEdit : CPanel {

	[SerializeField]
	private Text m_lbRepeat;
	[SerializeField]
	private Text m_lbSnooze;
	[SerializeField]
	private Text m_lbVoice;

	[SerializeField]
	private ScrollSnapSelector sclollHour;
	[SerializeField]
	private ScrollSnapSelector sclollMinute;

	public int m_iHour;
	public int m_iMinute;

	protected override void panelStart()
	{
		base.panelStart();
		
		DateTime time = TimeManager.Instance.MakeDateTime(GameMain.Instance.EditingAlarmParam.time);
		sclollHour.SetText("時");
		sclollHour.SetIndex(time.Hour);
		sclollMinute.SetText("分");
		sclollMinute.SetIndex(time.Minute);

	}

	// Update is called once per frame
	void Update () {
		string strRepeat = "None";

		m_iHour = sclollHour.target_index;
		m_iMinute = sclollMinute.target_index;

		AlarmParam param = GameMain.Instance.EditingAlarmParam;
		for (int i = 0; i < DataManagerAlarm.Instance.STR_WEEK_ARR.Length; i++)
		{
			bool bFlag = 0 < (param.repeat_type & (ulong)(1 << i));
			if (bFlag)
			{
				if (strRepeat.Equals("None") == true)
				{
					strRepeat = DataManagerAlarm.Instance.STR_WEEK_ARR[i];
				}
				else {
					strRepeat = string.Format("{0},{1}", strRepeat, DataManagerAlarm.Instance.STR_WEEK_ARR[i]);
				}
			}
		}
		m_lbRepeat.text = strRepeat;
		string strSnooze = DataManagerAlarm.Instance.STR_SNOOZE_ARR[param.snooze];
		m_lbSnooze.text = strSnooze;

		m_lbVoice.text = "";
		foreach (CsvVoiceData voice_data in DataManagerAlarm.Instance.master_voice_list)
		{
			if (param.voice_type == voice_data.id)
			{
				m_lbVoice.text = voice_data.description;
			}
		}
	}

	public void OnPushSave()
	{

		//Debug.LogError (GameMain.Instance.EditingAlarmParam.serial);
		GameMain.Instance.EditingAlarmParam.time = string.Format("1982-10-10 {0:D2}:{1:D2}:00", m_iHour, m_iMinute);
		GameMain.Instance.m_AlarmData.Load(AlarmData.FILENAME);
		if (0 < GameMain.Instance.EditingAlarmParam.serial)
		{
			foreach (AlarmParam param in GameMain.Instance.m_AlarmData.list)
			{
				if (param.serial == GameMain.Instance.EditingAlarmParam.serial)
				{
					param.repeat_type = GameMain.Instance.EditingAlarmParam.repeat_type;
					param.snooze = GameMain.Instance.EditingAlarmParam.snooze;
					param.status = GameMain.Instance.EditingAlarmParam.status;
					param.voice_type = GameMain.Instance.EditingAlarmParam.voice_type;
					param.time = GameMain.Instance.EditingAlarmParam.time;
				}
			}
		}
		else {
			int iSerial = 0;
			foreach (AlarmParam param in GameMain.Instance.m_AlarmData.list)
			{
				if (iSerial <= param.serial)
				{
					iSerial = param.serial;
				}
			}
			GameMain.Instance.EditingAlarmParam.serial = GameMain.Instance.m_AlarmData.list.Count + 1;
			GameMain.Instance.m_AlarmData.list.Add(GameMain.Instance.EditingAlarmParam);
		}
	}

	public void OnPushDelete()
	{
		GameMain.Instance.EditingAlarmParam.time = string.Format("1982-10-10 {0:D2}:{1:D2}:00", m_iHour, m_iMinute);
		GameMain.Instance.m_AlarmData.Load(AlarmData.FILENAME);
		//Debug.LogError (GameMain.Instance.EditingAlarmParam.serial);
		if (0 < GameMain.Instance.EditingAlarmParam.serial)
		{
			foreach (AlarmParam param in GameMain.Instance.m_AlarmData.list)
			{
				//Debug.LogError (param.serial);
				if (param.serial == GameMain.Instance.EditingAlarmParam.serial)
				{
					param.status = -1;
				}
			}
		}
		else {
		}
		GameMain.Instance.m_AlarmData.Save(AlarmData.FILENAME);
		GameMain.Instance.reserveTimeReset();
		GameMain.Instance.TimeSetRefresh();

	}




}
