using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class UIRestore : CPanel {

	[SerializeField]
	private Text m_txtMessage;

	[SerializeField]
	private Button m_btn;

	[SerializeField]
	private UIVoiceList m_uiVoiceList;

	protected override void panelStart()
	{
		base.panelStart();

		m_txtMessage.text = "データ復元中";
		m_btn.interactable = false;

		if ( PaymentManagerAlarm.Instance.getIExtensionProvider() == null)
		{
			m_txtMessage.text = "初期化失敗";
			Invoke("restore_failed", 2.0f);
		}
		else
		{
			PaymentManagerAlarm.Instance.getIExtensionProvider().GetExtension<IAppleExtensions>().RestoreTransactions(result => {
				if (result)
				{
					IStoreController controller = PaymentManagerAlarm.Instance.getIStoreController();
					DataManagerAlarm.Instance.restore_data.Clear();
					foreach (var product in controller.products.all)
					{
						if (product.hasReceipt)
						{ // リストア可能なレシートあるよ! 
						  // product.definition.id - アイテムID
						  // product.receipt - レシート関連(JSON) 
						  // サーバー検証して アイテム復元しなはれ
							DataManagerAlarm.Instance.restore_data.Add(product.definition.id);
						}
					}
					DataManagerAlarm.Instance.purchased_list.Clear();
					DataKvs purchaseCheck = new DataKvs();
					foreach (string id in DataManagerAlarm.Instance.restore_data)
					{
						DataManagerAlarm.Instance.purchased_list.Add(id);
						purchaseCheck.Write(id, id);
					}
					purchaseCheck.Save("data/purchaseCheck");
					Invoke("restore_finished", 2.0f);
				}
				else {
					m_txtMessage.text = "ログイン失敗";
					Invoke("restore_failed", 2.0f);
				}
			});

		}




	}

	private void restore_finished()
	{
		m_btn.interactable = true;
		m_txtMessage.text = "復元終了";

		m_uiVoiceList.CheckPurchase();
	}

	private void restore_failed()
	{
		m_btn.interactable = true;
		m_txtMessage.text = "復元失敗";

		m_uiVoiceList.CheckPurchase();

	}

	public void OnClick()
	{
		UIAssistant.main.ShowPreviousPage();
	}





}




