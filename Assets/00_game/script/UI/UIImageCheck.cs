using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImageCheck : CPanel {
	[SerializeField]
	private Image m_imgMain;
	public UtilSwitchSprite m_switchSprite;
	public int m_iSelectingId;
	public string m_strFilename;

	public GameObject m_goOkButton;
	public enum STEP
	{
		NONE = 0,
		LOAD,
		IDLE,
		END,
		MAX,
		LOAD_FILENAME,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	protected override void awake()
	{
		base.awake();

		m_goOkButton.GetComponent<Button>().onClick.AddListener(OnOK);
	}

	public void OnOK()
	{
		GameMain.Instance.kvs_data.WriteInt(DataManagerAlarm.KEY_SELECTING_IMAGE_ID, m_iSelectingId);
		GameMain.Instance.kvs_data.Save(CsvKvs.FILE_NAME);
		UIAssistant.main.ShowPreviousPage();
	}

	protected override void panelStart()
	{
		base.panelStart();

		m_iSelectingId = DataManagerAlarm.Instance.ImageCheckIndex;
		m_eStep = STEP.LOAD;
		m_eStepPre = STEP.MAX;

		m_goOkButton.SetActive(true);

		foreach (CsvImageData data in DataManagerAlarm.Instance.master_image_list)
		{
			if (data.id == m_iSelectingId)
			{
				Debug.LogError(string.Format("id:{0} name:{1}" , m_iSelectingId, data.name_image));
				m_strFilename = data.name_image;
				m_switchSprite.SetSprite(m_strFilename);
				m_switchSprite.OnShowed.AddListener(OnShowed);
			}
		}
	}
	private void OnShowed()
	{
		m_imgMain.SetNativeSize();
		m_switchSprite.OnShowed.RemoveListener(OnShowed);
	}

}





