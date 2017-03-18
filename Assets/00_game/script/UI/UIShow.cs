using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIShow : MonoBehaviour {

	public string m_strPageName;

	private Button btn = null;

	void OnEnable()
	{
		if (m_strPageName.Equals(""))
		{
			Debug.LogError("no set");
		}
		if (btn == null)
		{
			btn = gameObject.GetComponent<Button>();
			btn.onClick.AddListener(OnClick);
		}
	}

	void OnDisable()
	{
	}

	protected virtual void on_action()
	{

	}

	private void OnClick()
	{
		on_action();
		UIAssistant.main.ShowPage(m_strPageName);
	}
}
