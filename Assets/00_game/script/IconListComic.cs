using UnityEngine;
using System.Collections;

public class IconListComic : IconList {

	public UILabel m_lbTitle;
	new public void Initialize( int _iSelectingId , int _iIndex , CsvImageData _data , UIGrid _grid = null ){
		m_csvImageData = _data;
		Index = _iIndex;
		SetSelect (_iSelectingId);
		m_lbTitle.text = _data.name;
		//m_Grid = _grid;
		m_eStep = STEP.LOADING;
		m_eStepPre = STEP.MAX;
		return;
	}
}
