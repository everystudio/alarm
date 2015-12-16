using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoiceMain : PageBase {

	public ButtonManager m_TabButtonManager;
	private FooterButton m_btnList;
	private FooterButton m_btnStore;

	public GameObject m_goPanelList;
	public GameObject m_goPanelStore;

	public UIGrid m_gridList;
	public UIGrid m_gridStore;

	public ButtonManager m_bmBannerList;
	public ButtonManager m_bmBannerListSelect;
	public ButtonManager m_bmBannerShop;
	public ButtonManager m_bmBannerShopSelect;

	public List<BannerList> m_bannerList = new List<BannerList> ();
	public List<BannerShop> m_bannerShop = new List<BannerShop> ();

	public int m_iTabIndex;

	public void Purchase( string _strProductId ){
		foreach (BannerShop shop in m_bannerShop) {
			shop.Purchase (_strProductId);
		}
	}

	public enum STEP
	{
		NONE		,
		IDLE		,
		CLOSE		,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	public bool m_bSubStarted;
	void subStart(){
		//m_bmBannerList = new ButtonManager ();
		//m_bmBannerShop = new ButtonManager ();
		foreach (CsvVoiceData data in DataManagerAlarm.Instance.master_voice_list) {
			if (data.type == 1) {

				GameObject obj = PrefabManager.Instance.MakeObject ("prefab/BannerList", m_gridList.gameObject);
				BannerList script = obj.GetComponent<BannerList> ();
				m_bmBannerList.AddButtonBaseList (obj);
				script.Initialize (data);
				m_bmBannerListSelect.AddButtonBaseList (script.m_goSelect);
				m_bannerList.Add (script);

			}
			else if (data.type == 2) {
				GameObject obj = PrefabManager.Instance.MakeObject ("prefab/BannerShop", m_gridStore.gameObject);
				BannerShop script = obj.GetComponent<BannerShop> ();
				m_bmBannerShop.AddButtonBaseList (obj);
				script.Initialize (data);
				m_bmBannerShopSelect.AddButtonBaseList (script.m_goSelect);
				m_bannerShop.Add (script);
			} else {
			}
		}
		m_bmBannerList.SetButtonbaseFromList ();
		m_bmBannerListSelect.SetButtonbaseFromList ();
		m_bmBannerShop.SetButtonbaseFromList ();
		m_bmBannerShopSelect.SetButtonbaseFromList ();

		m_gridList.enabled = true;
		m_gridStore.enabled = true;
	}

	public override void Initialize ()
	{
		if (m_bSubStarted == false) {
			subStart ();
			m_bSubStarted = true;
		}
		m_TabButtonManager.ButtonInit ();
		m_btnList = m_TabButtonManager.GetButtonBase (0).gameObject.GetComponent<FooterButton> ();
		m_btnStore = m_TabButtonManager.GetButtonBase (1).gameObject.GetComponent<FooterButton> ();

		m_TabButtonManager.TriggerClearAll ();
		m_iTabIndex = m_TabButtonManager.Index;
		tab_switch (m_iTabIndex);

		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;

	}

	public override void Close ()
	{
		base.Close ();

		m_eStep = STEP.CLOSE;

	}

	private void tab_switch( int _iIndex ){
		if (_iIndex == 0) {
			m_goPanelList.SetActive (true);
			m_goPanelStore.SetActive (false);

			m_btnList.SetImage (true);
			m_btnStore.SetImage (false);
		} else if (_iIndex == 1) {
			m_goPanelList.SetActive (false);
			m_goPanelStore.SetActive (true);
			m_btnList.SetImage (false);
			m_btnStore.SetImage (true);
		}

		foreach (BannerList banner in m_bannerList) {
			banner.Reset ();
		}
		foreach (BannerShop banner in m_bannerShop) {
			banner.Reset ();
		}
		m_bmBannerList.TriggerClearAll ();
		m_bmBannerListSelect.TriggerClearAll ();
		m_bmBannerShop.TriggerClearAll ();
		m_bmBannerShopSelect.TriggerClearAll ();
		m_iListIndex = -1;
		m_iShopIndex = -1;
		SoundManager.Instance.StopAll (AUDIO_TYPE.SE);
	}
	public int m_iListIndex;
	public int m_iShopIndex;
	void Update(){

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		if (m_bmBannerList.ButtonPushed) {
			//Debug.Log (m_bmBannerList.Index);
			if (m_iListIndex != m_bmBannerList.Index ) {
				if (0 <= m_iListIndex) {
					m_bannerList [m_iListIndex].Reset ();
				}
				m_iListIndex = m_bmBannerList.Index;
			}
			m_bannerList[m_iListIndex].Pushed();
			m_bmBannerList.TriggerClearAll ();
		}

		if (m_bmBannerShop != null) {
			if (m_bmBannerShop.ButtonPushed) {
				if (m_iShopIndex != m_bmBannerShop.Index) {
					if (0 <= m_iShopIndex) {
						m_bannerShop [m_iShopIndex].Reset ();
					}
					m_iShopIndex = m_bmBannerShop.Index;
				}
				m_bannerShop [m_iShopIndex].Pushed ();
				m_bmBannerShop.TriggerClearAll ();
			}
		}

		switch (m_eStep) {
		case STEP.IDLE:
			break;
		}

		if (m_TabButtonManager.ButtonPushed) {
			if (m_iTabIndex != m_TabButtonManager.Index) {
				m_iTabIndex = m_TabButtonManager.Index;
				tab_switch (m_iTabIndex);
			}

			m_TabButtonManager.TriggerClearAll ();
		}

	}

}




