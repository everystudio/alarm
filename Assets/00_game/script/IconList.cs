using UnityEngine;
using System.Collections;

public class IconList : IconBase {

	public UtilSwitchSprite m_switchSprite;

	public UI2DSprite m_sprSelecting;
	public UI2DSprite m_sprIcon;

	public ImageCheck m_imageCheck;

	public CsvImageData m_csvImageData;
	protected UIGrid m_Grid;

	public enum STEP
	{
		INIT		= 0,
		LOADING		,
		IDLE		,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	public void SetSelect( int _iSelectingId ){
		//Debug.LogError (string.Format ("iconList:{0} == {1}", m_csvImageData.id, _iSelectingId));
		m_sprSelecting.gameObject.SetActive (m_csvImageData.id == _iSelectingId);
	}

	public void Initialize( int _iSelectingId , int _iIndex , CsvImageData _data , UIGrid _grid = null ){
		m_csvImageData = _data;
		Index = _iIndex;
		SetSelect (_iSelectingId);
		m_switchSprite.SetSize (212, 212);
		//Debug.Log(m_csvImageData.name_icon);
		m_switchSprite.SetSprite (m_csvImageData.name_icon );
		m_Grid = _grid;
		m_eStep = STEP.LOADING;
		m_eStepPre = STEP.MAX;
	}

	// Update is called once per frame
	void Update () {
		bool bInit = false;
		if( m_eStepPre != m_eStep)
		{
			m_eStepPre = m_eStep;
			bInit = true;
		}
		switch(m_eStep)
		{
		case STEP.LOADING:
			if (bInit) {
				//Debug.Log ("Loading,init");
					
			}
				if(true == SpriteManager.Instance.IsLoaded(m_csvImageData.name_icon))
				{
					if (m_Grid != null)
					{
						m_Grid.enabled = true;
					}
					m_eStep = STEP.IDLE;
				}
				break;
			case STEP.IDLE:
				break;
			default:
				break;
		}
	
	}
}





