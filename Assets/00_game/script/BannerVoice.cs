using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BannerVoice : BannerVoiceBase {
	public Text m_lbDescription;
	public Text m_lbName;

	//public Text m_lbPrice;

	private bool m_bPurchased;
	public GameObject m_goPurchased;
	public new CsvVoiceData m_csvVoiceData;
	[SerializeField]
	private Button m_btnBuy;
	[SerializeField]
	private Button m_btnSelect;

	public string m_strNameVoice;

	private void CheckPurchase()
	{
		foreach (string product_id in DataManagerAlarm.Instance.purchased_list)
		{
			Purchase(product_id);
		}
	}

	// 一方通行
	public void Purchase(string _strProductId)
	{
		if (m_csvVoiceData.name_voice.Equals(_strProductId))
		{
			m_btnBuy.gameObject.SetActive(false);
			m_goPurchased.SetActive(false);
			m_bPurchased = true;
		}
	}

	public override void initialize(CsvVoiceData _data)
	{
		m_bPurchased = false;
		base.initialize(_data);
		m_lbDescription.text = "";
		m_lbName.text = "";
		//m_lbPrice.text = "";

		m_lbDescription.text = _data.description;
		m_lbName.text = _data.name;
		m_csvVoiceData = _data;
		m_strNameVoice = m_csvVoiceData.name_voice;

		m_goPurchased.SetActive(_data.type == 2);

		foreach (string product_id in DataManagerAlarm.Instance.purchased_list)
		{
			Purchase(product_id);
		}
		m_btnBuy.onClick.RemoveAllListeners();
		m_btnBuy.onClick.AddListener(OnClick);
	}

#if UNITY_ANDROID 
	private void OnProductPurchased(BillingResult result)
	{
		foreach (string product_id in DataManagerAlarm.Instance.purchased_list)
		{
			Purchase(product_id);
		}
		AndroidInAppPurchaseManager.ActionProductPurchased -= OnProductPurchased;
	}
#elif UNITY_IPHONE
#endif

	public void OnClick()
	{
		//Debug.LogError(m_csvVoiceData.name_voice);
#if UNITY_ANDROID
		AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;
#elif UNITY_IPHONE
#endif
		GameBillingManager.purchase(m_csvVoiceData.name_voice);
	}

	public void ShowSelectButton()
	{
		bool bShow = false;
		if( m_csvVoiceData.type == 2 && m_bPurchased)
		{
			bShow = true;
		}
		else if (m_csvVoiceData.type == 1)
		{
			bShow = true;
		}
		if( bShow)
		{
			m_btnSelect.gameObject.SetActive(true);
		}
	}

	public void Select()
	{
		Debug.LogError("BannerVoice.Select");
		GameMain.Instance.EditingAlarmParam.voice_type = m_csvVoiceData.id;
		UIAssistant.main.ShowPreviousPage();
	}


}


