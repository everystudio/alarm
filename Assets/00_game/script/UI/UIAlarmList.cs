using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAlarmList : CPanel {

	[SerializeField]
	private GameObject bannerRoot;

	public List<BannerTimer> m_BannerTimerList = new List<BannerTimer>();

	protected override void panelStart()
	{
		base.panelStart();

		BannerTimer[] dellist = bannerRoot.GetComponentsInChildren<BannerTimer>();
		foreach(BannerTimer banner in dellist)
		{
			Destroy(banner.gameObject);
		}
		m_BannerTimerList.Clear();

		GameMain.Instance.m_AlarmData.Load(AlarmData.FILENAME);

		foreach (AlarmParam param in GameMain.Instance.m_AlarmData.list)
		{
			// マイナスは非表示
			if (0 <= param.m_status)
			{
				if (0 != param.m_status)
				{
					if (param.repeat_type == 0)
					{
						if (TimeManager.Instance.GetDiffNow(param.time).TotalSeconds < 0)
						{
							param.m_status = 0;
							Debug.LogError(string.Format("close(serial:{0}", param.serial));
						}
					}
				}

				GameObject obj = PrefabManager.Instance.MakeObject("prefab/BannerTimer", bannerRoot);
				obj.transform.localScale = Vector3.one;
				BannerTimer bt = obj.GetComponent<BannerTimer>();
				bt.Initialize(param);

				m_BannerTimerList.Add(bt);
			}
		}
		GameMain.Instance.m_AlarmData.Save(AlarmData.FILENAME);
	}

}
