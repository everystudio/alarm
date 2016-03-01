using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComicMain : PageBase2
{
	public int m_iSelectingId;
	public ButtonManager m_bmIconList;

	private List<IconList> m_iconList = new List<IconList>();

	public ImageCheck m_imageCheckComic;
	public UIPanel m_panelComic;

	public ImageCheck m_imageCheckNormal;

	public UIGrid m_Grid;

	public void IconSelect(int _iSelectIndex)
	{
		foreach (IconList icon in m_iconList)
		{
			icon.SetSelect(_iSelectIndex);
		}
		return;
	}

	// Use this for initialization
	void Start()
	{
		Debug.LogError ("here");
		Debug.LogError (DataManagerAlarm.Instance.config.Read ("footer"));
		if (DataManagerAlarm.Instance.config.Read ("footer").Contains ("Comic") == false) {
			gameObject.SetActive (false);
		}
		m_eStep = STEP.WAIT;
		m_eStepPre = STEP.MAX;
	}

	public enum STEP
	{
		NONE = 0,
		WAIT,
		IDLE,
		CHECKING,
		MAX,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;
	public override void Initialize()
	{
		base.Initialize();
		if (m_eStep != STEP.WAIT)
		{
			m_eStep = STEP.IDLE;
		}
		m_eStepPre = STEP.MAX;

		m_imageCheckNormal.gameObject.SetActive (false);
	}

	public override void Close()
	{
		base.Close();
		m_eStep = STEP.MAX;
		Debug.LogError (string.Format ("Close ComicMain"));
		m_imageCheckNormal.gameObject.SetActive (true);

		m_imageCheckComic.OutStart ();
	}
	// Update is called once per frame

	public int test_selecting_id = 0;
	void Update()
	{

		bool bInit = false;
		if (m_eStepPre != m_eStep)
		{
			m_eStepPre = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {
		case STEP.WAIT:

			if (SpriteManager.Instance.IsIdle ()) {
				//m_bmIconList.ButtonRefresh(DataManagerAlarm.Instance.master_comic_list.Count);
				m_bmIconList.ButtonRefresh ();

				m_iSelectingId = GameMain.Instance.kvs_data.ReadInt (DataManagerAlarm.KEY_SELECTING_IMAGE_ID);

				int iIndex = 0;

				foreach (CsvImageData data in DataManagerAlarm.Instance.master_comic_list) {
					GameObject obj = PrefabManager.Instance.MakeObject ("prefab/IconRoot", m_Grid.gameObject);
					IconList script = obj.GetComponent<IconList> ();

					script.Initialize (test_selecting_id, iIndex, data, m_Grid);

					m_iconList.Add (script);

					m_bmIconList.AddButtonBase (obj);

					iIndex += 1;
				}

				IconSelect (-1);
				m_bmIconList.TriggerClearAll ();

				m_imageCheckComic.Initialize ();

				m_eStep = STEP.IDLE;
			}

			break;
		case STEP.IDLE:
			if (bInit) {
				m_bmIconList.TriggerClearAll ();
			}
			if (m_bmIconList.ButtonPushed) {
				int iPushedId = m_iconList [m_bmIconList.Index].m_csvImageData.id;
				// なんか知らんけど補正かけないとうまく出ない
				m_iSelectingId = iPushedId-1;
				m_bmIconList.TriggerClearAll ();
				m_eStep = STEP.CHECKING;
			}
			break;
		case STEP.CHECKING:
			if (bInit) {
				m_panelComic.clipOffset = new Vector2 (0.0f, 48.0f);
				m_panelComic.transform.localPosition = Vector3.zero;
				m_imageCheckComic.TriggerClearAll ();
				m_imageCheckComic.InStart (DataManagerAlarm.Instance.master_comic_list [m_iSelectingId].name_image);
			}

			if (m_imageCheckComic.ButtonPushed) {
				/*
					if (m_imageCheck.Index == 0)
					{
					}
					else if (m_imageCheck.Index == 1)
					{
						IconSelect(m_iSelectingId);
						GameMain.Instance.kvs_data.WriteInt(DataManagerAlarm.KEY_SELECTING_IMAGE_ID, m_iSelectingId);
						GameMain.Instance.kvs_data.Save();
					}
					else {
					}
					*/
				m_imageCheckComic.OutStart ();
				m_eStep = STEP.IDLE;
			}
			break;
		case STEP.MAX:
		default:
			break;
		}
		return;

	}
}
