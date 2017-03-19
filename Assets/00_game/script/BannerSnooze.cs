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

	public void Initialize(string _strLabel, int _iSnoozeType , bool _bOn)
	{
		m_lbText.text = _strLabel;
		Switch(_bOn);

		gameObject.GetComponent<Button>().onClick.AddListener(() =>
		{
			GameMain.Instance.EditingAlarmParam.snooze = _iSnoozeType;
			UIAssistant.main.ShowPreviousPage();
		});
	}

	public void Switch(bool _bOn)
	{
		m_sprSelecting.gameObject.SetActive(_bOn);
	}





}
