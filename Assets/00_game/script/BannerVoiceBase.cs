using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BannerVoiceBase : BannerBase {

	public UtilSwitchSprite m_switchSprite;

	public UI2DSprite m_sprStop;
	public UI2DSprite m_sprPlaying;

	public GameObject m_goSelect;

	public enum STEP
	{
		NONE		= 0,
		IDLE		,
		PLAYING		,
		STOP		,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	public CsvVoiceData m_csvVoiceData;

	public void SetSprite( CsvVoiceData _data ){
		m_switchSprite.SetSize (161, 161);
		m_switchSprite.SetSprite (_data.name_icon);
	}

	public virtual void initialize(CsvVoiceData _data){
	}

	public void Initialize( CsvVoiceData _data ){
		m_csvVoiceData = _data;

		SetSprite (_data);
		initialize (_data);
	}

	public void Reset(){
		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;
	}

	public void Pushed(){
		if (m_eStep == STEP.IDLE) {
			m_eStep = STEP.PLAYING;
		} else {
			m_eStep = STEP.STOP;
		}
	}

	protected void Update(){

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}
		switch (m_eStep) {
		case STEP.IDLE:
			if (bInit) {
				m_sprStop.gameObject.SetActive (true);
				m_sprPlaying.gameObject.SetActive (false);
			}
			break;
		case STEP.PLAYING:
			if (bInit) {
				SoundManager.Instance.StopAll (AUDIO_TYPE.SE);

				GameMain.Instance.CallVoice (m_csvVoiceData.id);
				/*
				List<string> sound_list = new List<string> ();

				foreach (CsvVoicesetData data in DataManagerAlarm.Instance.master_voiceset_list) {
					if (m_csvVoiceData.id == data.id) {
						sound_list.Add (data.name);
					}
				}
				int iIndex = UtilRand.GetRand (sound_list.Count);
				SoundManager.Instance.PlaySE ( sound_list[iIndex] );
				*/
				m_sprStop.gameObject.SetActive (false);
				m_sprPlaying.gameObject.SetActive (true);
			}
			break;
		case STEP.STOP:
			SoundManager.Instance.StopAll (AUDIO_TYPE.SE);
			m_eStep = STEP.IDLE;
			break;
		default:
			break;
		}

	}






}
