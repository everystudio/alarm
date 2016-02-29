using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AlarmMain : PageBase2 {

	public UtilSwitchSprite m_switchSprite;
	public ButtonBase m_btnSetList;

	public TimeSet m_TimeSet;

	public UILabel m_lbNowDate;
	public UILabel m_lbNowTime;

	public UILabel m_lbNextTime;
	public UILabel m_lbNextWeek;

	public void refreshTime (){
		DateTime now = TimeManager.GetNow ();
		m_lbNowDate.text = string.Format( "{0} {1:D2} {2}" , DataManagerAlarm.Instance.STR_MONTH_SHORT_ARR[now.Month] , now.Day , DataManagerAlarm.Instance.STR_WEEK_SHORT_ARR[TimeManager.Instance.GetWeekIndex(TimeManager.StrGetTime())]);

		m_lbNowTime.text = string.Format ("{0:D2}:{1:D2}:{2:D2}", now.Hour, now.Minute, now.Second);
	}

	public void setNextTimer( List<AlarmReserve> _list ){
		if (0 < _list.Count) {
			string strTime = _list [0].m_strTime;
			Debug.Log (strTime);
			DateTime dateTime = TimeManager.Instance.MakeDateTime (strTime);
			m_lbNextTime.text = string.Format ("{0:D2};{1:D2}", dateTime.Hour, dateTime.Minute);
			m_lbNextWeek.text = DataManagerAlarm.Instance.STR_WEEK_SHORT_ARR[TimeManager.Instance.GetWeekIndex(strTime)];
		} else {
			m_lbNextTime.text = "--:--";
			m_lbNextWeek.text = "";
		}
	}

	public override void Initialize ()
	{
		base.Initialize ();
		refreshTime ();

		m_btnSetList.TriggerClear ();
		m_TimeSet.Initialize ();

		setNextTimer (GameMain.Instance.reserve_list);

		int iSelectingImageId = GameMain.Instance.kvs_data.ReadInt (DataManagerAlarm.KEY_SELECTING_IMAGE_ID);
		foreach (CsvImageData data in DataManagerAlarm.Instance.master_image_list) {
			if (iSelectingImageId == 0) {
				iSelectingImageId = data.id;
				GameMain.Instance.kvs_data.WriteInt (DataManagerAlarm.KEY_SELECTING_IMAGE_ID, iSelectingImageId);
			}
			if (data.id == iSelectingImageId) {
				m_switchSprite.SetSprite (data.name_image);
			}
		}

	}

	public override void Close ()
	{
		base.Close ();
	}

	public const float UPDTE_INTERVAL = 0.5f;
	public float m_fUpdateInterval;

	void Update(){

		m_fUpdateInterval += Time.deltaTime;
		if (UPDTE_INTERVAL < m_fUpdateInterval) {
			m_fUpdateInterval -= UPDTE_INTERVAL;
			refreshTime ();
		}

		if (m_btnSetList.ButtonPushed) {
			m_btnSetList.TriggerClear ();
			m_TimeSet.InStart ();
		}
		if (m_TimeSet.ButtonPushed) {
			if (m_TimeSet.Index == 0) {
				m_TimeSet.OutStart ();
				setNextTimer (GameMain.Instance.reserve_list);
			}
		}

	}



}











