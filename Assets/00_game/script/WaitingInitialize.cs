using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
public class WaitingInitialize : MonoBehaviour {

	private UILabel m_lbText;
	float m_fTimer;
	int m_iCount;
	public float INTERVAL = 1.5f;

	public string m_strMessage;

	// Use this for initialization
	void Start () {

		m_lbText = GetComponent<UILabel> ();
		m_fTimer = 0.0f;
		m_iCount = 0;
		m_strMessage = "データ準備中";
	}

	public void SetString( string _strString ){
		m_strMessage = _strString;
		return;
	}
	
	// Update is called once per frame
	void Update () {

		m_fTimer += Time.deltaTime;
		if (INTERVAL < m_fTimer) {
			m_fTimer -= INTERVAL;
			m_iCount += 1;
			m_iCount %= 4;
			m_lbText.text = m_strMessage;
			for( int i = 0 ; i < m_iCount ; i++ ){
				m_lbText.text += "・";
			}
		}
	
	}
}
