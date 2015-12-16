using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageFooter : ButtonManager {

	//public List<FooterButton> m_fooerButtonList = new List<FooterButton>();

	void Start(){
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
