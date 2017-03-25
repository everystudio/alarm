using UnityEngine;
using System.Collections;
//using Prime31;

public class BannerShop : BannerVoiceBase {

	public UILabel m_lbDescription;
	public UILabel m_lbName;
	public UILabel m_lbPrice;

	public ButtonBase m_btnBuy;
	public GameObject m_goPurchased;
	public new CsvVoiceData m_csvVoiceData;

	private void CheckPurchase(){
		foreach (string product_id in DataManagerAlarm.Instance.purchased_list) {
			Purchase (product_id);
		}
	}

	// 一方通行
	public void Purchase( string _strProductId ){
		if (m_csvVoiceData.name_voice.Equals (_strProductId)) {
			m_btnBuy.gameObject.SetActive (false);
			m_goPurchased.SetActive (true);
		}
	}

	public override void initialize (CsvVoiceData _data)
	{
		base.initialize (_data);
		m_lbDescription.text = "";
		m_lbName.text = "";
		m_lbPrice.text = "";

		m_lbDescription.text = _data.description;
		m_lbName.text = _data.name;
		m_csvVoiceData = _data;

		m_goPurchased.SetActive (false);

		foreach (string product_id in DataManagerAlarm.Instance.purchased_list) {

			Purchase (product_id);
			/*
			if (product_id.Equals (_data.name_voice)) {
				m_btnBuy.gameObject.SetActive (false);
				m_goPurchased.SetActive (true);
			}
			*/
		}

		#if UNITY_ANDROID
		#endif

	}

	/*
	new void Update()
	{
		base.Update();

		if (m_bPurchased == false) {
			CheckPurchase ();
			if (m_btnBuy.ButtonPushed) {
				Debug.LogError(m_csvVoiceData.name_voice);
				GameBillingManager.purchase (m_csvVoiceData.name_voice);
				m_btnBuy.TriggerClear ();
			}
		}
	}
	*/


}
