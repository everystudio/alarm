using UnityEngine;
//using System.Collections;

public class ImageCheck : OtherPage {

	public UtilSwitchSprite m_switchSprite;

	public enum STEP
	{
		NONE		= 0,
		LOAD		,
		IDLE		,
		END			,
		MAX			,
		LOAD_FILENAME,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	public int m_iSelectingId;
	public string m_strFilename;

	public GameObject m_goOkButton;

	public void InStart ( int _iSelectingId )
	{
		m_iSelectingId = _iSelectingId;
		m_eStep = STEP.LOAD;
		m_eStepPre = STEP.MAX;

		m_goOkButton.SetActive(true);


		return;
	}

	public void InStart( string _strFilename)
	{
		m_strFilename = _strFilename;
		m_eStep = STEP.LOAD_FILENAME;
		m_eStepPre = STEP.MAX;
		m_goOkButton.SetActive(false);
	}

	public override void OutStart ()
	{
		base.OutStart ();
		m_eStep = STEP.END;
	}

	new void Update(){

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}
		switch (m_eStep) {
		case STEP.LOAD:
			if (bInit) {
				foreach (CsvImageData data in DataManagerAlarm.Instance.master_image_list) {
					if (data.id == m_iSelectingId) {
						Debug.LogError (data.name_image);

						m_strFilename = data.name_image;
						m_switchSprite.SetSprite (m_strFilename);
					}
				}
			}

			if (m_switchSprite.IsIdle() ) {
				m_eStep = STEP.IDLE;
			}
			break;

			case STEP.LOAD_FILENAME:
				if (bInit)
				{
					//m_switchSprite.SetSize(640, 1136);
					m_switchSprite.SetSprite(m_strFilename);
				}
				if (m_switchSprite.IsIdle())
				{
					m_eStep = STEP.IDLE;
				}

				break;

		case STEP.IDLE:
			if (bInit) {
				base.InStart ();
			}
			break;
		case STEP.END:
			break;

		case STEP.MAX:
		default:
			break;
		}
	}

}
