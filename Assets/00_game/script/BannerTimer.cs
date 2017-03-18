using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[RequireComponent(typeof(Button))]
public class BannerTimer : MonoBehaviour {

	public Text m_lbTimer;
	public Text m_lbName;

	public Image m_imgSwitch;
	public Button m_btnSwitch;

	//public ButtonBase m_btnTrigger;

	public AlarmParam m_AlarmParam;
	public void Initialize( AlarmParam _param ){
		m_AlarmParam = _param;
		//Debug.Log (_param.time);
		DateTime time = TimeManager.Instance.MakeDateTime (_param.time);

		m_lbTimer.text = string.Format( "{0:D2}:{1:D2}" , time.Hour , time.Minute );

		m_lbName.text = DataManagerAlarm.Instance.GetVoiceData(_param.voice_type).description;

		SetStatus (_param.status);

		gameObject.GetComponent<Button>().onClick.AddListener(OnClickBanner);
		m_btnSwitch.onClick.AddListener(OnClickSwitch);

	}

	public void SetStatus( int _iStatus ){
		if (_iStatus == 0) {
			m_imgSwitch.sprite = SpriteManager.Instance.LoadSprite("Texture/btn_timer_on");
		} else {
			m_imgSwitch.sprite = SpriteManager.Instance.LoadSprite("Texture/btn_timer_off");

			DateTime datetimeNow = TimeManager.GetNow();
			DateTime checkDate = TimeManager.Instance.MakeDateTime (m_AlarmParam.time);
			string strCheckDate = string.Format ("{0}-{1:D2}-{2:D2} {3:D2}:{4:D2}:00", datetimeNow.Year, datetimeNow.Month, datetimeNow.Day, checkDate.Hour, checkDate.Minute);

			TimeSpan time_span = TimeManager.Instance.GetDiffNow (strCheckDate);
			if (0 < time_span.TotalSeconds) {
			} else {
				DateTime tomorrowDateTime = TimeManager.GetNow();
				tomorrowDateTime = tomorrowDateTime.AddDays (1);
				string strTomorrow = string.Format ("{0}-{1:D2}-{2:D2} {3:D2}:{4:D2}:00", tomorrowDateTime.Year, tomorrowDateTime.Month, tomorrowDateTime.Day, checkDate.Hour, checkDate.Minute);
				strCheckDate = strTomorrow;
			}
			m_AlarmParam.time = strCheckDate;
		}
	}
	private void OnClickBanner()
	{
		GameMain.Instance.EditingAlarmParam = m_AlarmParam;
		UIAssistant.main.ShowPage("EditTime");

	}

	private void OnClickSwitch()
	{
		if (m_AlarmParam.status == 0)
		{
			m_AlarmParam.status = 1;
		}
		else {
			m_AlarmParam.status = 0;
		}
		SetStatus(m_AlarmParam.status);
		GameMain.Instance.m_AlarmData.UpdateStatus(m_AlarmParam.serial, m_AlarmParam.status);
		GameMain.Instance.m_AlarmData.Save(AlarmData.FILENAME);

	}


}
