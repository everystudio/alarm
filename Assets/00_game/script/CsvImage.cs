using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CsvImageData : CsvDataParam
{
	public int id { get;  set; }
	public int type { get;  set; }
	public string name { get;  set; }
	public string description { get;  set; }
	public string name_image { get;  set; }
	public string name_icon { get;  set; }
}


public class CsvImage : CsvData<CsvImageData> {
	public CsvImage(string _path = "")
	{
		if (false == _path.Equals(""))
		{
			FilePath = _path;
		}
	}
	private string FilePath = "csv/image_list";
	public void Load() { LoadResources(FilePath); }


	override protected CsvImageData makeParam( List<SpreadSheetData> _list , int _iSerial , int _iRow ){
		SpreadSheetData id = SpreadSheetData.GetSpreadSheet( _list, _iRow , 1);
		SpreadSheetData type= SpreadSheetData.GetSpreadSheet( _list,_iRow , 2 );
		SpreadSheetData name= SpreadSheetData.GetSpreadSheet( _list,_iRow , 3 );
		SpreadSheetData description= SpreadSheetData.GetSpreadSheet( _list,_iRow , 4 );
		SpreadSheetData name_image = SpreadSheetData.GetSpreadSheet( _list,_iRow , 5 );
		SpreadSheetData name_icon = SpreadSheetData.GetSpreadSheet( _list,_iRow , 6 );

		CsvImageData retParam = new CsvImageData ();
		retParam.id= int.Parse(id.param);
		retParam.type = int.Parse (type.param);
		retParam.name = name.param;
		retParam.description = description.param;
		retParam.name_image = name_image.param;
		retParam.name_icon = name_icon.param;
		return retParam;
	}
}


