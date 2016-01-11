using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PageVoice : VoiceMain {
	private const float MOVE_TIME = 0.1f;
	private Vector3 POS_OUT = new Vector3 (0.0f, -1136.0f, 0.0f);

	public bool m_bInitialized;

	public override void Initialize(){
		base.Initialize ();
		if (m_bInitialized == false) {
			myTransform.localPosition = POS_OUT;
			ButtonInit ();
		}
		m_bInitialized = true;
	}

	public void InStart(){
		ButtonInit ();
		TweenPosition.Begin (gameObject, MOVE_TIME, Vector3.zero);
	}

	public void OutStart(){
		TweenPosition.Begin (gameObject, MOVE_TIME, POS_OUT);
	}




}
