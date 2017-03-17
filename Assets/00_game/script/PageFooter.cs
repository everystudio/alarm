using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageFooter : ButtonManager {

	//public List<FooterButton> m_fooerButtonList = new List<FooterButton>();

	public UIGrid m_grid;

	public readonly string[] FOOTER_NAME = new string[] {
		"Alarm",
		"Voice",
		"Image",
		"Special",
		"Comic"
	};

	void Start(){
		string strFooterInfo = DataManagerAlarm.Instance.config.Read("footer");
		string [] strArr = strFooterInfo.Split('-');
		for( int i = 0; i < strArr.Length; i++)
		{
			string strPrefabName = string.Format("prefab/FooterButton/FooterButton{0}", strArr[i]);
			Debug.Log(strPrefabName);

			GameObject obj = PrefabManager.Instance.MakeObject(strPrefabName, m_grid.gameObject);
			obj.transform.localScale = Vector3.one;
			ButtonBase button = obj.GetComponent<ButtonBase>();

			int iButtonIndex = 0;
			foreach( string button_name in FOOTER_NAME)
			{
				if( true == button_name.Equals(strArr[i]) )
				{
					break;
				}
				iButtonIndex += 1;
			}
			button.ButtonInit(iButtonIndex);
			AddButtonBase( obj);
		}
		m_grid.enabled = true;

		//ButtonInit ();
		TriggerClearAll();
	}

	public void SetIndex( int _iIndex ){
		for( int i = 0 ; i < m_csButtonList.Count ; i++ ){
			FooterButton script = m_csButtonList [i].gameObject.GetComponent<FooterButton> ();

			bool bSet = false;

			if (_iIndex == script.Index) {
				bSet = true;
			}
			script.SetImage (bSet);
		}
	}

}
