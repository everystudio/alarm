using UnityEngine;
using System.Collections;
using Prime31;

public class BannerShop : BannerVoiceBase {

	public UILabel m_lbDescription;
	public UILabel m_lbName;
	public UILabel m_lbPrice;

	public ButtonBase m_btnBuy;
	public GameObject m_goPurchased;
	public CsvVoiceData m_csvVoiceData;

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

		/*
		foreach (GoogleSkuInfo info in DataManagerAlarm.Instance.product_data_list) {
			if (info.productId.Equals (m_csvVoiceData.name_voice)) {

				Debug.LogError ("here" + m_csvVoiceData.name_voice );

				string[] stArrayData = info.title.Split (' ');

				if (0 < stArrayData.Length) {
				}
				m_lbName.text = info.description;
				m_lbDescription.text = stArrayData [0];
				m_lbPrice.text = string.Format ("{0}円", info.price);
			}
		}
		*/

	}

	void Update(){

		base.Update ();

		if (m_btnBuy.ButtonPushed) {
			m_btnBuy.TriggerClear ();
			GoogleIAB.purchaseProduct( m_csvVoiceData.name_voice );
		}
	}


}
