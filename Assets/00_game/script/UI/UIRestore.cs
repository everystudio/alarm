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

		DataManagerAlarm.Instance.purchased_list.Clear();

		DataKvs purchaseCheck = new DataKvs();
		foreach ( string id in DataManagerAlarm.Instance.restore_data)
		{
			DataManagerAlarm.Instance.purchased_list.Add(id);
			purchaseCheck.Write(id, id);

		}
		purchaseCheck.Save("data/purchaseCheck");
		m_btn.interactable = false;

		Invoke("restore_finished", 2.0f);
	}

	private void restore_finished()
	{
		m_btn.interactable = true;
		m_txtMessage.text = "復元終了";

		m_uiVoiceList.CheckPurchase();
	}

	public void OnClick()
	{
		UIAssistant.main.ShowPreviousPage();
	}





}




