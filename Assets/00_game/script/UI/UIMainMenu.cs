using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : CPanel {
	public UtilSwitchSprite m_switchSprite;


	public Text m_lbNowDate;
	public Text m_lbNowTime;

	public Text m_lbNextTime;
	public Text m_lbNextWeek;

	public void refreshTime()
	{
		DateTime now = TimeManager.GetNow();
		m_lbNowDate.text = string.Format("{0} {1:D2} {2}", DataManagerAlarm.Instance.STR_MONTH_SHORT_ARR[now.Month], now.Day, DataManagerAlarm.Instance.STR_WEEK_SHORT_ARR[TimeManager.Instance.GetWeekIndex(TimeManager.StrGetTime())]);
		m_lbNowTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}", now.Hour, now.Minute, now.Second);
		Invoke("refreshTime", UPDTE_INTERVAL);
	}
	public void setNextTimer(List<AlarmReserve> _list)
	{
		if (0 < _list.Count)
		{
			string strTime = _list[0].m_strTime;
			//Debug.Log(strTime);
			DateTime dateTime = TimeManager.Instance.MakeDateTime(strTime);
			m_lbNextTime.text = string.Format("{0:D2};{1:D2}", dateTime.Hour, dateTime.Minute);
			//Debug.LogError(TimeManager.Instance.GetWeekIndex(strTime));
			//Debug.LogError(DataManagerAlarm.Instance.STR_WEEK_SHORT_ARR[TimeManager.Instance.GetWeekIndex(strTime)]);
			m_lbNextWeek.text = DataManagerAlarm.Instance.STR_WEEK_SHORT_ARR[TimeManager.Instance.GetWeekIndex(strTime)];
		}
		else {
			m_lbNextTime.text = "--:--";
			m_lbNextWeek.text = "";
		}
	}

	protected override void panelStart()
	{
		base.panelStart();
		refreshTime();

		GameMain.Instance.reserveTimeReset();
		setNextTimer(GameMain.Instance.reserve_list);

		int iSelectingImageId = GameMain.Instance.kvs_data.ReadInt(DataManagerAlarm.KEY_SELECTING_IMAGE_ID);
		foreach (CsvImageData data in DataManagerAlarm.Instance.master_image_list)
		{
			if (iSelectingImageId == 0)
			{
				iSelectingImageId = data.id;
				GameMain.Instance.kvs_data.WriteInt(DataManagerAlarm.KEY_SELECTING_IMAGE_ID, iSelectingImageId);
			}
			if (data.id == iSelectingImageId)
			{
				Debug.LogError(data.name_image);
				m_switchSprite.SetSprite(data.name_image);
			}
		}
	}

	public const float UPDTE_INTERVAL = 0.5f;
	public float m_fUpdateInterval;

	protected override void awake()
	{
		base.awake();
		refreshTime();
	}




}
