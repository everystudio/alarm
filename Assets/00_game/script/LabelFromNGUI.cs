using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LabelFromNGUI : MonoBehaviour {

	[SerializeField]
	private UILabel ngui_text;
	private Text text;

	void Awake()
	{
		text = gameObject.GetComponent<Text>();

	}


	void Update()
	{
		text.text = ngui_text.text;
	}

}
