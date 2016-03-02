using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChangeSprite : MonoBehaviourEx {

	public UI2DSprite m_sprite;
	public List<Sprite> list = new List<Sprite> (); 


	public float m_fTimer;
	public float INTERVAL = 0.1f;

	public int m_iSpriteIndex;

	// Use this for initialization
	void Start () {
		m_iSpriteIndex = 0;
		m_sprite.sprite2D = list [m_iSpriteIndex];
		m_fTimer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		m_fTimer += Time.deltaTime;
		if (INTERVAL < m_fTimer) {
			m_fTimer -= INTERVAL;
			m_iSpriteIndex += 1;
			m_iSpriteIndex %= list.Count;
			m_sprite.sprite2D = list [m_iSpriteIndex];
		}
	}
}
