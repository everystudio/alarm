using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]
public class FooterButton : MonoBehaviourEx {

	public UnityEventString OnPushed = new UnityEventString();

	public string m_TypeName;
	private Image img;

	public readonly string[] FOOTER_NAME = new string[] {
		"Alarm",
		"Voice",
		"Image",
		"Special",
		"Comic"
	};

	public UnityEngine.Sprite m_sprOn;
	public UnityEngine.Sprite m_sprOff;

	public void Initialize(string _strType)
	{
		m_TypeName = _strType;
		img = gameObject.GetComponent<Image>();
		int index = 0;
		for( int i = 0; i < FOOTER_NAME.Length; i++)
		{
			if (FOOTER_NAME[i].Equals(_strType))
			{
				index = i + 1;
			}
		}
		m_sprOn = SpriteManager.Instance.LoadSprite(string.Format("Texture/ui_footer_{0}_on", index));
		m_sprOff = SpriteManager.Instance.LoadSprite(string.Format("Texture/ui_footer_{0}_off", index));

		SetImage(false);
	}

	public void OnClick()
	{
		OnPushed.Invoke(m_TypeName);
	}

	public void SetImage( bool _isOn ){
		if (_isOn) {
			img.sprite = m_sprOn;
		} else {
			img.sprite = m_sprOff;
		}
	}

	public void OnSprite(string _strType)
	{
		SetImage(_strType.Equals(m_TypeName));
	}

}
