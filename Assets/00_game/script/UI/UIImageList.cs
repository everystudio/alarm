using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIImageList : CPanel {

	[SerializeField]
	private GameObject m_goRoot;

	public int m_iSelectingId;
	private List<IconList> m_iconList = new List<IconList>();

	private bool bSet = false;

	private void OnChange( string _strName)
	{
		//Debug.LogError(_strName);
		refresh();
	}

	protected override void panelEndStart()
	{
		base.panelEndStart();
		UIAssistant.onShowPage -= OnChange;
	}

	protected override void panelStart()
	{
		UIAssistant.onShowPage += OnChange;

		base.panelStart();
		refresh();
	}

	private void refresh()
	{
		IconList[] dellist = m_goRoot.GetComponentsInChildren<IconList>();
		foreach (IconList script in dellist)
		{
			Destroy(script.gameObject);
		}
		m_iconList.Clear();

		m_iSelectingId = GameMain.Instance.kvs_data.ReadInt(DataManagerAlarm.KEY_SELECTING_IMAGE_ID);

		int iIndex = 0;
		foreach (CsvImageData data in DataManagerAlarm.Instance.master_image_list)
		{
			GameObject obj = PrefabManager.Instance.MakeObject("prefab/IconRoot", m_goRoot);
			obj.transform.localScale = Vector3.one;
			IconList script = obj.GetComponent<IconList>();

			script.Initialize(m_iSelectingId, iIndex, data);
			m_iconList.Add(script);
			/*
			m_bmIconList.AddButtonBase(obj);
			*/
			iIndex += 1;
		}

	}


	public void OnSelect( int _iIndex )
	{
		int iPushedId = m_iconList[_iIndex].m_csvImageData.id;
		m_iSelectingId = iPushedId;
	}



}



