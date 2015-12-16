using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TimeEdit : OtherPage {

	public enum EDIT_TYPE
	{
		NONE		= 0,
		CHANGE		,
		NEW			,
		MAX			,
	}
	public EDIT_TYPE m_eEditType;

	#region SerializeFieldでの設定が必要なメンバー変数
	[SerializeField]
	private UICenterOnChild m_csCenterOnChildHour;
	[SerializeField]
	private UICenterOnChild m_csCenterOnChildMinute;
	#endregion
	private GameObject m_goCenterHour;
	private GameObject m_goCenterMinute;
	public List<GameObject> m_goHourList = new List<GameObject> ();
	public List<GameObject> m_goMinuteList = new List<GameObject> ();

	public UIGrid m_GridHour;
	public UIGrid m_GridMinute;

	public ButtonBase m_btnRepeat;
	public ButtonBase m_btnSnooze;
	public ButtonBase m_btnVoice;

	public PageRepeat m_PageRepeat;
	public PageSnooze m_PageSnooze;
	public PageVoice m_PageVoice;

	public UILabel m_lbRepeat;
	public UILabel m_lbSnooze;

	public UILabel m_lbVoice;

	public VoiceMain m_voiceMain;

	public int m_iHour;
	public int m_iMinute;

	private void _setup_time( UIGrid _grid , int _iMax , string _strTail , ref List<GameObject> _list ){
		for (int i = 0; i < _iMax; i++) {
			GameObject obj = PrefabManager.Instance.MakeObject ("prefab/Hour" , _grid.gameObject );
			obj.name = string.Format ("{0}", i);
			obj.GetComponent<UILabel> ().text = string.Format ("{0:D2}{1}", i , _strTail );
			_list.Add (obj);
		}
		_grid.enabled = true;
	}


	public override void Initialize ()
	{

		base.Initialize ();
		m_csCenterOnChildHour.onCenter = DragBannerHour;
		m_csCenterOnChildMinute.onCenter = DragBannerMinute;

		_setup_time (m_GridHour, 24, "時" , ref m_goHourList );
		_setup_time (m_GridMinute, 60, "分", ref m_goMinuteList);

		m_btnRepeat.TriggerClear();
		m_btnSnooze.TriggerClear();
		m_btnVoice.TriggerClear();

		m_PageRepeat.Initialize ();
		m_PageSnooze.Initialize ();


	}

	public void Initialize( AlarmParam _param ){
	}

	public override void InStart ()
	{
		base.InStart ();
	}

	public void InStart( AlarmParam _param ){
		InStart ();

		DateTime time = TimeManager.Instance.MakeDateTime (_param.time);

		SetBannerHour (time.Hour);
		SetBannerMinute (time.Minute);

		m_PageRepeat.Initialize ();
		m_PageSnooze.Initialize ();

		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;

	}

	public void paramRefresh( AlarmParam _param ){

		string strRepeat = "None";

		for (int i = 0; i < DataManagerAlarm.Instance.STR_WEEK_ARR.Length; i++) {
			bool bFlag = 0 < (_param.repeat_type & (1<<i));
			if (bFlag) {
				if (strRepeat.Equals ("None") == true) {
					strRepeat = DataManagerAlarm.Instance.STR_WEEK_ARR [i];
				} else {
					strRepeat = string.Format ("{0},{1}", strRepeat, DataManagerAlarm.Instance.STR_WEEK_ARR [i]);
				}
			}
		}
		m_lbRepeat.text = strRepeat;
		string strSnooze = DataManagerAlarm.Instance.STR_SNOOZE_ARR[_param.snooze];
		m_lbSnooze.text = strSnooze;

		m_lbVoice.text = "";
		foreach (CsvVoiceData voice_data in DataManagerAlarm.Instance.master_voice_list) {
			if (_param.voice_type == voice_data.id) {
				m_lbVoice.text = voice_data.description;
			}
		}
	}

	public enum STEP
	{
		NONE		= 0,
		IDLE		,
		REPEAT		,
		SNOOZE		,
		VOICE		,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	void Update(){
	
		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}


		switch (m_eStep) {
		case STEP.IDLE:
			if (bInit) {
				m_btnRepeat.TriggerClear ();
				m_btnSnooze.TriggerClear ();
				m_btnVoice.TriggerClear ();
			}
			if (m_btnRepeat.ButtonPushed) {
				m_eStep = STEP.REPEAT;
			} else if (m_btnSnooze.ButtonPushed) {
				m_eStep = STEP.SNOOZE;
			} else if (m_btnVoice.ButtonPushed) {
				m_eStep = STEP.VOICE;
			} else {
			}

			if (ButtonPushed) {
				//Debug.LogError (Index);
				TriggerClearAll ();
				if (Index == 0) {
					OutStart ();
					// 面倒なので再読み込み
				} else if (Index == 1) {
					OutStart ();
					// ここは保存
					//Debug.LogError (GameMain.Instance.EditingAlarmParam.serial);
					GameMain.Instance.EditingAlarmParam.time = string.Format ("1982-10-10 {0:D2}:{1:D2}:00", m_iHour, m_iMinute);
					GameMain.Instance.m_AlarmData.Load (AlarmData.FILENAME);
					if (0 < GameMain.Instance.EditingAlarmParam.serial) {
						foreach (AlarmParam param in GameMain.Instance.m_AlarmData.list) {
							if (param.serial == GameMain.Instance.EditingAlarmParam.serial) {
								param.repeat_type = GameMain.Instance.EditingAlarmParam.repeat_type;
								param.snooze = GameMain.Instance.EditingAlarmParam.snooze;
								param.status = GameMain.Instance.EditingAlarmParam.status;
								param.voice_type = GameMain.Instance.EditingAlarmParam.voice_type;
								param.time = GameMain.Instance.EditingAlarmParam.time;
							}
						}
					} else {
						int iSerial = 0;
						foreach (AlarmParam param in GameMain.Instance.m_AlarmData.list) {
							if (iSerial <= param.serial) {
								iSerial = param.serial;
							}
						}
						GameMain.Instance.EditingAlarmParam.serial = GameMain.Instance.m_AlarmData.list.Count + 1;
						GameMain.Instance.m_AlarmData.list.Add (GameMain.Instance.EditingAlarmParam);
					}
				}
				else if( Index == 2 ){
					OutStart ();
					GameMain.Instance.EditingAlarmParam.time = string.Format ("1982-10-10 {0:D2}:{1:D2}:00", m_iHour, m_iMinute);
					GameMain.Instance.m_AlarmData.Load (AlarmData.FILENAME);
					//Debug.LogError (GameMain.Instance.EditingAlarmParam.serial);
					if (0 < GameMain.Instance.EditingAlarmParam.serial) {
						foreach (AlarmParam param in GameMain.Instance.m_AlarmData.list) {
							//Debug.LogError (param.serial);
							if (param.serial == GameMain.Instance.EditingAlarmParam.serial) {
								param.status = -1;
							}
						}
					} else {
					}
					GameMain.Instance.m_AlarmData.Save ();
					GameMain.Instance.reserveTimeReset ();
					GameMain.Instance.TimeSetRefresh ();

				} else {
				}
				GameMain.Instance.m_AlarmData.Save ();
				GameMain.Instance.TimeSetRefresh ();
			}

			break;

		case STEP.REPEAT:
			if (bInit) {
				m_btnRepeat.TriggerClear ();
				m_PageRepeat.InStart (GameMain.Instance.EditingAlarmParam);
			}
			if (m_PageRepeat.ButtonPushed) {
				m_PageRepeat.OutStart ();
				m_PageRepeat.TriggerClearAll ();
				m_eStep = STEP.IDLE;
			}
			break;
		case STEP.SNOOZE:
			if (bInit) {
				m_btnSnooze.TriggerClear ();
				m_PageSnooze.InStart (GameMain.Instance.EditingAlarmParam);
			}
			if (m_PageSnooze.ButtonPushed) {
				m_PageSnooze.OutStart ();
				m_PageSnooze.TriggerClearAll ();
				m_eStep = STEP.IDLE;
			}
			if (m_PageSnooze.m_bmSnoozeType.ButtonPushed) {
				//Debug.Log (string.Format ("Pushed:{0}", m_PageSnooze.m_bmSnoozeType.Index));
				GameMain.Instance.EditingAlarmParam.snooze = m_PageSnooze.m_bmSnoozeType.Index;

				m_PageSnooze.OutStart ();
				m_PageSnooze.m_bmSnoozeType.TriggerClearAll ();
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.VOICE:
			if (bInit) {
				m_PageVoice.Initialize ();
				m_PageVoice.InStart ();
			}
			int select_index = 0;
			if (m_PageVoice.ButtonPushed) {
				m_PageVoice.OutStart ();
				m_eStep = STEP.IDLE;
			}
			else if (m_PageVoice.m_bmBannerListSelect.ButtonPushed) {
				select_index = m_PageVoice.m_bmBannerListSelect.Index;
				GameMain.Instance.EditingAlarmParam.voice_type = m_PageVoice.m_bannerList [select_index].m_csvVoiceData.id;
				m_PageVoice.OutStart ();
				m_eStep = STEP.IDLE;
			}
			else if (m_PageVoice.m_bmBannerShopSelect.ButtonPushed) {
				select_index = m_PageVoice.m_bmBannerShopSelect.Index;

				int shop_voice_id = m_PageVoice.m_bannerShop [select_index].m_csvVoiceData.id;

				string product_id = "";
				foreach (CsvVoiceData voice_data in DataManagerAlarm.Instance.master_voice_list) {
					if (voice_data.id == shop_voice_id) {
						product_id = voice_data.name_voice;
					}
				}
				bool bGood = false;
				foreach (string strPurchasedProductId in DataManagerAlarm.Instance.purchased_list) {
					if (strPurchasedProductId.Equals (product_id) == true) {
						bGood = true;
					}
				}
				if (bGood) {
					GameMain.Instance.EditingAlarmParam.voice_type = shop_voice_id;
				}
				m_PageVoice.OutStart ();
				m_eStep = STEP.IDLE;
			}

			break;

		case STEP.MAX:
		default:
			break;

		}

		paramRefresh (GameMain.Instance.EditingAlarmParam);


	}
	#region scroll関連
	// バナーがドラッグされて切り替わった際に呼ばれるイベント
	public void DragBannerHour(GameObject _goBanner) {
		m_iHour = int.Parse (_goBanner.name);
		int iBannerNo = 0;
		SetBannerHour(_goBanner);
		return;
	}
	public void SetBannerHour( int _iBannerId ){
		foreach (GameObject obj in m_goHourList) {
			int banner_id = int.Parse (obj.name);
			if (banner_id == _iBannerId) {
				SetBannerHour (obj);
				break;
			}
		}
		return;
	}
	public void SetBannerHour( GameObject _goBanner ){
		//Debug.Log (_goBanner.name);
		if (m_goCenterHour != _goBanner) {
			m_goCenterHour = _goBanner;
			m_csCenterOnChildHour.CenterOn (_goBanner.transform);
		} else {
		}
	}

	// バナーがドラッグされて切り替わった際に呼ばれるイベント
	public void DragBannerMinute(GameObject _goBanner) {
		//Debug.Log (_goBanner.name);
		m_iMinute = int.Parse (_goBanner.name);
		int iBannerNo = 0;
		SetBannerMinute(_goBanner);
		return;
	}
	public void SetBannerMinute( int _iBannerId ){
		foreach (GameObject obj in m_goMinuteList) {
			int banner_id = int.Parse (obj.name);
			if (banner_id == _iBannerId) {
				SetBannerMinute (obj);
				break;
			}
		}
		return;
	}
	public void SetBannerMinute( GameObject _goBanner ){
		//Debug.Log (_goBanner.name);
		if (m_goCenterMinute != _goBanner) {
			m_goCenterMinute = _goBanner;
			m_csCenterOnChildMinute.CenterOn (_goBanner.transform);
		} else {
		}
	}
	#endregion

}
