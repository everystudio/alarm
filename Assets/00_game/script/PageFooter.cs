using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageFooter : ButtonManager {

	//public List<FooterButton> m_fooerButtonList = new List<FooterButton>();

	public UIGrid m_grid;

	void Start(){

		Debug.LogError(DataManagerAlarm.Instance.m_csvConfig.Read("footer"));
		string strFooterInfo = DataManagerAlarm.Instance.m_csvConfig.Read("footer");
		string [] strArr = strFooterInfo.Split('-');
		for( int i = 0; i < strArr.Length; i++)
		{
			string strPrefabName = string.Format("prefab/FooterButton/FooterButton{0}", strArr[i]);
			Debug.Log(strPrefabName);

			GameObject obj = PrefabManager.Instance.MakeObject(strPrefabName, m_grid.gameObject);

			AddButtonBase(i, obj);
		}
		m_grid.enabled = true;

		ButtonInit ();
	}

	public void SetIndex( int _iIndex ){
		for( int i = 0 ; i < m_csButtonList.Length ; i++ ){
			FooterButton script = m_csButtonList [i].gameObject.GetComponent<FooterButton> ();

			bool bSet = false;
			if (_iIndex == i) {
				bSet = true;
			}
			script.SetImage (bSet);
		}
	}

}
