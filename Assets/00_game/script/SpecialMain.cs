using UnityEngine;
using System.Collections;

public class SpecialMain : PageBase {

	public override void Initialize ()
	{
		base.Initialize ();

		m_fPitch = 1.0f;
		//m_AudioChannelData.m_tAudioClipData.m_AudioClip.
		m_eStep = STEP.LOADING;
		if (m_bLoaded) {
			m_eStep = STEP.PAUSE;
		} else {
			m_fTimerMax = 0.0f;
		}
		m_eStepPre = STEP.MAX;

		m_fTimer = 0.0f;
	}

	public override void Close ()
	{
		base.Close ();
		m_AudioChannelData.m_tAudioSource.Stop ();
	}


	public enum STEP {
		NONE		= 0,
		LOADING		,
		PAUSE		,
		PLAYING		,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	public ButtonBase m_btnPlay;
	public ButtonBase m_btnUp;
	public ButtonBase m_btnDown;
	public ButtonBase m_btnPause;

	public UILabel m_lbTimeLeft;
	public UILabel m_lbTimeRight;
	public UILabel m_lbTitle;

	public GameObject m_goCursor;

	public bool m_bLoaded;

	public Camera m_useCamera;
	public float m_fPitch;


	public float m_fTimer;
	public float m_fTimerMax;


	public AudioChannelData m_AudioChannelData;

	void Start(){
		foreach (CsvVoiceData data in DataManagerAlarm.Instance.master_voice_list) {
			if (data.id == 4234) {
				Debug.LogError (data.id);
			}
		}
		m_AudioChannelData = SoundManager.Instance.PlayBGM ("demo_song");
		m_eStep = STEP.LOADING;
		m_eStepPre = STEP.MAX;
	}

	void Update(){

		bool bInit = false;
		bool bHitEvent = false;

		if(Input.GetMouseButtonDown(0)){
			RaycastHit hit;

			// 床との当たり判定のみを取りたいのでマスクするレイヤーを設定する
			//int layerNo = LayerMask.NameToLayer ("Floor");
			//int layerMask = 1 << layerNo;

			Ray ray = m_useCamera.ScreenPointToRay(Input.mousePosition);
			float fDistance = 100.0f;

			//レイを投射してオブジェクトを検出
			if(Physics.Raycast( ray , out hit , fDistance )){
				//Debug.LogError ( hit.collider.gameObject.name +":"+ hit.point.x);
				if (hit.collider.gameObject.name.Equals ("bar")) {
					bHitEvent = true;
					m_goCursor.transform.position = new Vector3 (hit.point.x, m_goCursor.transform.position.y, 0.0f);
				}
			}
		}

		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {
		case STEP.NONE:
			return;
			break;
		case STEP.LOADING:
			if (bInit) {
				m_btnPause.gameObject.SetActive (false);
				m_btnPlay.gameObject.SetActive (true);
			}
			if (m_AudioChannelData.m_tAudioSource.isPlaying) {
				m_bLoaded = true;

				m_fTimer = m_AudioChannelData.m_tAudioSource.time;
				m_fTimerMax = m_AudioChannelData.m_tAudioSource.clip.length;
				m_AudioChannelData.m_tAudioSource.Stop ();
				m_eStep = STEP.PAUSE;


			} else {
			}
			break;

		case STEP.PAUSE:
			if (bInit) {
				m_btnPause.gameObject.SetActive (false);
				m_btnPlay.gameObject.SetActive (true);
				//m_AudioChannelData.m_tAudioSource.pitch = 0.0f;
				m_AudioChannelData.m_tAudioSource.Stop ();
				m_btnPlay.TriggerClear ();
			}
			if (m_btnPlay.ButtonPushed) {
				m_eStep = STEP.PLAYING;
			}
			break;
		case STEP.PLAYING:
			if (bInit) {

				m_btnPause.gameObject.SetActive (true);
				m_btnPlay.gameObject.SetActive (false);
				m_fPitch = 1.0f;
				m_AudioChannelData.m_tAudioSource.time = m_fTimer;
				m_AudioChannelData.m_tAudioSource.pitch = m_fPitch;
				m_AudioChannelData.m_tAudioSource.Play ();
				m_btnPause.TriggerClear ();
			}
			if (m_btnPause.ButtonPushed) {
				m_eStep = STEP.PAUSE;
			}
			m_fTimer = m_AudioChannelData.m_tAudioSource.time;

			break;
		case STEP.MAX:
		default:
			break;
		}

		float fRate = GetRate (m_fTimer, 0.0f, m_fTimerMax);
		if (bHitEvent == true) {
			fRate = GetRate (m_goCursor.transform.localPosition.x, -320.0f, 320.0f);
			m_fTimer = fRate * m_fTimerMax;
			m_AudioChannelData.m_tAudioSource.time = m_fTimer;
		} else {
			m_goCursor.transform.localPosition = new Vector3 (Linear (fRate, -320.0f, 320.0f), 0.0f, 0.0f);
		}
		SetTime (m_fTimer, m_fTimerMax);

		if (m_btnUp.ButtonPushed) {
			m_btnUp.TriggerClear ();
			if (m_fPitch < 1.0f) {
				m_fPitch = 1.0f;
			}
			m_fPitch += 0.2f;
			m_AudioChannelData.m_tAudioSource.pitch = m_fPitch;
		}
		if (m_btnDown.ButtonPushed) {
			m_btnDown.TriggerClear ();
			if (-0.2 <= m_fPitch) {
				m_fPitch = 0.0f;
			}
			m_fPitch -= 0.2f;
			m_AudioChannelData.m_tAudioSource.pitch = m_fPitch;
		}

	}

	public void SetTime( float _fTime , float _fTimeMax ){

		int iTime = (int)_fTime;
		int iMax = (int)_fTimeMax;
		int iDiff = iMax - iTime;
		m_lbTimeLeft.text = string.Format ("{0}:{1:D2}", iTime / 60, iTime % 60);
		m_lbTimeRight.text = string.Format ("-{0}:{1:D2}", iDiff / 60, iDiff % 60);

		/*
		m_lbTimeLeft.text = GetTime (m_fTimer);
		m_lbTimeRight.text = GetTimeRight ();
		*/
	}


}
