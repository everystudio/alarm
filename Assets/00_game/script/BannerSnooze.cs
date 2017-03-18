using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Button))]
public class BannerSnooze : MonoBehaviour {

	public Text m_lbText;

	public Image m_sprBack;
	public Image m_sprSelecting;

	public bool m_bFlag;

	public void Initialize(string _strLabel, bool _bOn)
	{
		m_lbText.text = _strLabel;
		Switch(_bOn);
	}

	public void Switch(bool _bOn)
	{
		m_sprSelecting.gameObject.SetActive(_bOn);
	}




}
