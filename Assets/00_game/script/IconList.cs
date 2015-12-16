using UnityEngine;
using System.Collections;

public class IconList : IconBase {

	public UtilSwitchSprite m_switchSprite;

	public UI2DSprite m_sprSelecting;
	public UI2DSprite m_sprIcon;

	public ImageCheck m_imageCheck;

	public CsvImageData m_csvImageData;

	public void SetSelect( int _iSelectingId ){
		m_sprSelecting.gameObject.SetActive (m_csvImageData.id == _iSelectingId);
	}

	public void Initialize( int _iSelectingId , int _iIndex , CsvImageData _data ){
		m_csvImageData = _data;
		Index = _iIndex;
		SetSelect (_iSelectingId);
		m_switchSprite.SetSize (212, 212);
		m_switchSprite.SetSprite (_data.name_icon );
	}

	// Update is called once per frame
	void Update () {
	
	}
}
