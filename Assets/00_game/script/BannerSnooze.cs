using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BannerSnooze : BannerBase {

	public UILabel m_lbText;

	public UI2DSprite m_sprBack;
	public UI2DSprite m_sprSelecting;

	public bool m_bFlag;

	public void Initialize( string _strLabel , bool _bOn  ){
		m_lbText.text = _strLabel;
		m_sprSelecting.gameObject.SetActive (_bOn);

		m_sprBack.sprite2D = sprite_list [Index];
	}

	public List<Sprite> sprite_list = new List<Sprite>();


}
