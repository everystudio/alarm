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
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	public int m_iSelectingId;

	public void InStart ( int _iSelectingId )
	{
		m_iSelectingId = _iSelectingId;
		m_eStep = STEP.LOAD;
		m_eStepPre = STEP.MAX;

		return;
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
						m_switchSprite.SetSprite (data.name_image);
					}
				}
			}

			if (m_switchSprite.IsIdle()) {
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
