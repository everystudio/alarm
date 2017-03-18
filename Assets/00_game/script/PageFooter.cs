using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageFooter : ButtonManager {

	//public List<FooterButton> m_fooerButtonList = new List<FooterButton>();

	public GameObject m_grid;

	public List<FooterButton> buttonList = new List<FooterButton>();

	void Start(){

		FooterButton[] del = m_grid.GetComponentsInChildren<FooterButton>();
		foreach(FooterButton ft in del)
		{
			Destroy(ft.gameObject);
		}
		buttonList.Clear();

		string strFooterInfo = DataManagerAlarm.Instance.config.Read("footer");
		string [] strArr = strFooterInfo.Split('-');
		for( int i = 0; i < strArr.Length; i++)
		{
			string strPrefabName = string.Format("prefab/FooterButton/FooterButton", strArr[i]);
			Debug.Log(strPrefabName);

			FooterButton script = PrefabManager.Instance.MakeScript<FooterButton>(strPrefabName, m_grid);
			script.gameObject.transform.localScale = Vector3.one;
			script.Initialize(strArr[i]);
			script.OnPushed.AddListener(OnPushedFooterButton);
			buttonList.Add(script);
		}
		OnPushedFooterButton(strArr[0]);
	}

	private void OnPushedFooterButton( string _strName)
	{
		foreach( FooterButton btn in buttonList)
		{
			btn.OnSprite(_strName);
		}

	}

	public void SetIndex( int _iIndex ){
	}

}
