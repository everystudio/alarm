using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageSnooze : OtherPage {

	public UIGrid m_Grid;
	public List<BannerSnooze> m_BannerSnoozeList = new List<BannerSnooze>();

	public int m_iSnoozeType;

	public ButtonManager m_bmSnoozeType;

	public override void Initialize ()
	{
		Debug.Log ("Initialize");
		if (m_bInitialized == false) {
			base.Initialize ();

			m_bmSnoozeType.ButtonRefresh ();

			for (int i = 0; i < DataManagerAlarm.Instance.STR_SNOOZE_ARR.Length; i++) {
				GameObject obj = PrefabManager.Instance.MakeObject ("prefab/BannerSnooze", m_Grid.gameObject);
				obj.transform.localScale = Vector3.one;
				obj.name = string.Format ("{0}", i);
				BannerSnooze script = obj.GetComponent<BannerSnooze> ();
				script.Initialize (DataManagerAlarm.Instance.STR_SNOOZE_ARR [i], false);
				m_BannerSnoozeList.Add (script);
				m_bmSnoozeType.AddButtonBase ( obj);
			}
			m_Grid.enabled = true;
			m_bmSnoozeType.ButtonInit ();
		}

		int iCount = 0;
		foreach (BannerSnooze snooze in m_BannerSnoozeList) {
			snooze.Initialize (DataManagerAlarm.Instance.STR_SNOOZE_ARR [iCount], iCount == GameMain.Instance.EditingAlarmParam.snooze);
		}

		m_bInitialized = true;
	}

	public void InStart ( AlarmParam _param )
	{
		base.InStart ();
		int iCount = 0;
		foreach (BannerSnooze snooze in m_BannerSnoozeList) {
			snooze.Initialize (DataManagerAlarm.Instance.STR_SNOOZE_ARR [iCount], iCount == _param.snooze);
			iCount += 1;
		}

	}


	/*
	void Update(){
		if (ButtonPushed) {
			OutStart ();
			TriggerClearAll ();
		}

		if (m_bmSnoozeType.ButtonPushed) {
			Debug.Log (string.Format ("Pushed:{0}", m_bmSnoozeType.Index));
			GameMain.Instance.EditingAlarmParam.snooze = m_bmSnoozeType.Index;

			OutStart ();
			m_bmSnoozeType.TriggerClearAll ();
		}
	}
	*/

}
