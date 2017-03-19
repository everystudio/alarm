using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BannerVoiceBase : BannerBase {

	public UtilSwitchSprite m_switchSprite;

	public Image m_sprStop;
	public Image m_sprPlaying;

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

	public UnityEventInt OnPushed = new UnityEventInt();

	public void Pushed(){
		if (m_eStep == STEP.IDLE) {
			m_eStep = STEP.PLAYING;
		} else {
			m_eStep = STEP.STOP;
		}
		OnPushed.Invoke(m_csvVoiceData.id);
	}

	public void SetView( int _iId)
	{
		if( _iId == m_csvVoiceData.id)
		{
			m_sprStop.gameObject.SetActive(false);
			m_sprPlaying.gameObject.SetActive(true);
		}
		else
		{
			m_sprStop.gameObject.SetActive(true);
			m_sprPlaying.gameObject.SetActive(false);
		}
	}

	protected void Update(){

		return;

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
