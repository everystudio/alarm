using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRepeatSetting : CPanel {

	[SerializeField]
	private GameObject m_goRootRepeat;

	private List<BannerRepeat> listRepeat = new List<BannerRepeat>();


	protected override void panelStart()
	{
		base.panelStart();

		BannerRepeat[] dellist = m_goRootRepeat.GetComponentsInChildren<BannerRepeat>();
		foreach (BannerRepeat script in dellist)
		{
			Destroy(script.gameObject);
		}
		listRepeat.Clear();

		for (int i = 0; i < 7; i++)
		{
			BannerRepeat script = PrefabManager.Instance.MakeScript<BannerRepeat>("prefab/BannerRepeat", m_goRootRepeat);
			script.gameObject.transform.localScale = Vector3.one;
			//script.Initialize(DataManagerAlarm.Instance.STR_WEEK_ARR[i], false, i);
			listRepeat.Add(script);
		}

		for (int i = 0; i < listRepeat.Count; i++)
		{
			bool bFlag = 0 < (GameMain.Instance.EditingAlarmParam.repeat_type & (ulong)(1 << i));
			listRepeat[i].Initialize(DataManagerAlarm.Instance.STR_WEEK_ARR[i], bFlag, i);
		}
	}



}


