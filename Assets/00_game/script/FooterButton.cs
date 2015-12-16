using UnityEngine;
using System.Collections;

public class FooterButton : ButtonBase {

	public UI2DSprite m_sprite;
	public UnityEngine.Sprite m_sprOn;
	public UnityEngine.Sprite m_sprOff;

	public void SetImage( bool _isOn ){
		if (_isOn) {
			m_sprite.sprite2D = m_sprOn;
		} else {
			m_sprite.sprite2D = m_sprOff;
		}
	}

}
