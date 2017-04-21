using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVoiceList : CPanel {

	[SerializeField]
	private RectTransform rtHeader;
	[SerializeField]
	private RectTransform rtSelect;
	[SerializeField]
	private RectTransform rtList;

	[SerializeField]
	private GameObject m_goRoot;

	public List<BannerVoice> voiceList = new List<BannerVoice>();

	public void CheckPurchase()
	{
		foreach (BannerVoice script in voiceList)
		{
			script.CheckPurchase();
		}
	}

	private void clear_list(int _iType , string _page )
	{
		m_iPrevId = 0;
		BannerVoice[] dellist = m_goRoot.GetComponentsInChildren<BannerVoice>();
		foreach (BannerVoice script in dellist)
		{
			Destroy(script.gameObject);
		}
		voiceList.Clear();

		foreach (CsvVoiceData data in DataManagerAlarm.Instance.master_voice_list)
		{
			if (data.type == _iType) // 1
			{
				BannerVoice script = PrefabManager.Instance.MakeScript<BannerVoice>("prefab/BannerVoice", m_goRoot);
				script.gameObject.transform.localScale = Vector3.one;
				script.Initialize(data);
				if (_page.Equals("AlarmVoice"))
				{
					script.ShowSelectButton();
				}
				script.OnPushed.AddListener(OnPushed);
				voiceList.Add(script);
			}
		}
#if UNITY_IOS
		// 課金は復元ボタンが！
		if (_iType == 2)
		{
			BannerVoice script = PrefabManager.Instance.MakeScript<BannerVoice>("prefab/BannerRestore", m_goRoot);
			script.gameObject.transform.localScale = Vector3.one;
			script.gameObject.GetComponent<Button>().onClick.AddListener(() =>
			{

			});
		}
#endif

		OnPushed(m_iPrevId);
	}

	public int m_iPrevId;

	public void OnPushed( int _iId )
	{
		SoundManager.Instance.StopAll(AUDIO_TYPE.SE);


		if (m_iPrevId != _iId)
		{
			GameMain.Instance.CallVoice(_iId);
		}
		else
		{
			_iId = 0;
		}

		foreach ( BannerVoice banner in voiceList)
		{
			banner.SetView(_iId);
		}
		m_iPrevId = _iId;
	}

	public void OnSelectList()
	{
		clear_list(1 , m_strCurrentPage );
	}
	public void OnSelectShop()
	{
		clear_list(2 , m_strCurrentPage);
	}

	protected override void awake()
	{
		base.awake();
		//CheckRectTransform(rtList, "rtList");
	}

	protected override void panelEndStart()
	{
		base.panelEndStart();
		SoundManager.Instance.StopAll(AUDIO_TYPE.SE);

	}

	private string m_strCurrentPage;

	protected override void panelStart()
	{
		base.panelStart();

		m_strCurrentPage = UIAssistant.main.GetCurrentPage();
		OnSelectList();

		if (m_strCurrentPage.Equals("AlarmVoice"))
		{
			rtHeader.gameObject.SetActive(true);

			rtSelect.pivot = new Vector2(0.5000f, 1.0000f);
			rtSelect.anchorMin = new Vector2(0.0000f, 1.0000f);
			rtSelect.anchorMax = new Vector2(1.0000f, 1.0000f);
			rtSelect.offsetMin = new Vector2(0.0000f, -176.0000f);
			rtSelect.offsetMax = new Vector2(0.0000f, -88.0000f);
			rtSelect.sizeDelta = new Vector2(0.0000f, 88.0000f);
			rtList.pivot = new Vector2(0.5000f, 0.5000f);
			rtList.anchorMin = new Vector2(0.0000f, 0.0000f);
			rtList.anchorMax = new Vector2(1.0000f, 1.0000f);
			rtList.offsetMin = new Vector2(0.0000f, 163.3900f);
			rtList.offsetMax = new Vector2(0.0000f, -176.0001f);
			rtList.sizeDelta = new Vector2(0.0000f, -339.3901f);

		}
		else {
			rtHeader.gameObject.SetActive(false);

			rtSelect.pivot = new Vector2(0.5000f, 1.0000f);
			rtSelect.anchorMin = new Vector2(0.0000f, 1.0000f);
			rtSelect.anchorMax = new Vector2(1.0000f, 1.0000f);
			rtSelect.offsetMin = new Vector2(0.0000f, -88.0000f);
			rtSelect.offsetMax = new Vector2(0.0000f, 0.0000f);
			rtSelect.sizeDelta = new Vector2(0.0000f, 88.0000f);


			rtList.pivot = new Vector2(0.5000f, 0.5000f);
			rtList.anchorMin = new Vector2(0.0000f, 0.0000f);
			rtList.anchorMax = new Vector2(1.0000f, 1.0000f);
			rtList.offsetMin = new Vector2(0.0000f, 194.2000f);
			rtList.offsetMax = new Vector2(0.0000f, -88.0001f);
			rtList.sizeDelta = new Vector2(0.0000f, -282.2001f);
		}

	}

}
