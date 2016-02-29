using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageRepeat : OtherPage {

	public UIGrid m_Grid;
	public List<BannerRepeat> m_BannerRepeatList = new List<BannerRepeat>();

	public override void Initialize ()
	{
		if (m_bInitialized == false) {
			base.Initialize ();

			for (int i = 0; i < DataManagerAlarm.Instance.STR_WEEK_ARR.Length; i++) {
				GameObject obj = PrefabManager.Instance.MakeObject ("prefab/BannerRepeat", m_Grid.gameObject);
				obj.name = string.Format ("{0}", i);
				BannerRepeat script = obj.GetComponent<BannerRepeat> ();
				script.Initialize (DataManagerAlarm.Instance.STR_WEEK_ARR [i], false , i );
				m_BannerRepeatList.Add (script);
			}
			m_Grid.enabled = true;
		}
	}

	public void InStart ( AlarmParam _param )
	{
		base.InStart ();

		for (int i = 0; i < m_BannerRepeatList.Count; i++) {
			bool bFlag = 0 < (_param.repeat_type & (ulong)(1<<i));
			m_BannerRepeatList [i].Initialize (DataManagerAlarm.Instance.STR_WEEK_ARR [i], bFlag , i );
		}

	}

	/*
	void Update(){
		if (ButtonPushed) {
			OutStart ();
			TriggerClearAll ();
		}
	}
	*/

}
