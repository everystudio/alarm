using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISnoozeSetting : CPanel
{
	[SerializeField]
	private GameObject m_goRoot;

	private List<BannerSnooze> list = new List<BannerSnooze>();

	protected override void panelStart()
	{
		base.panelStart();

		BannerSnooze[] dellist = m_goRoot.GetComponentsInChildren<BannerSnooze>();
		foreach (BannerSnooze script in dellist)
		{
			Destroy(script.gameObject);
		}
		list.Clear();

		for (int i = 0; i < DataManagerAlarm.Instance.STR_SNOOZE_ARR.Length; i++)
		{
			BannerSnooze script = PrefabManager.Instance.MakeScript<BannerSnooze>("prefab/BannerSnooze", m_goRoot);
			script.gameObject.name = string.Format("{0}", i);
			script.gameObject.transform.localScale = Vector3.one;
			script.Initialize(DataManagerAlarm.Instance.STR_SNOOZE_ARR[i], false);
			list.Add(script);
		}

		int iCount = 0;
		foreach (BannerSnooze snooze in list)
		{
			snooze.Initialize(DataManagerAlarm.Instance.STR_SNOOZE_ARR[iCount], iCount == GameMain.Instance.EditingAlarmParam.snooze);
			iCount += 1;
		}

	}

}
