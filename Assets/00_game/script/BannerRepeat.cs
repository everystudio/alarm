using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Button))]
public class BannerRepeat : MonoBehaviour {

	public Text m_lbText;
	public Image m_imgSwitch;
	public Image m_sprYes;
	public Image m_sprNo;

	public bool m_bFlag;

	public  int m_iIndex;

	private void button( bool _bOn ){
		if (_bOn) {
			m_imgSwitch.sprite = SpriteManager.Instance.LoadSprite("Texture/btn_timer_on");
		} else {
			m_imgSwitch.sprite = SpriteManager.Instance.LoadSprite("Texture/btn_timer_off");
		}
	}

	public void Initialize( string _strLabel , bool _bOn , int _iIndex ){
		m_lbText.text = _strLabel;
		m_bFlag = _bOn;
		button (m_bFlag);
		m_iIndex = _iIndex;

		gameObject.GetComponent<Button>().onClick.AddListener(() =>
		{
			m_bFlag = !m_bFlag;
			Debug.LogError(m_bFlag);
			button(m_bFlag);

			if (m_bFlag)
			{
				GameMain.Instance.EditingAlarmParam.repeat_type |= (ulong)(1 << m_iIndex);
			}
			else {
				GameMain.Instance.EditingAlarmParam.repeat_type &= (ulong)~(1 << m_iIndex);
			}

		});
	}


}
