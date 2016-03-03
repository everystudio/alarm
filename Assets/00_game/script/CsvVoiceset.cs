using UnityEngine;
using System.Collections;

public class CsvVoicesetData : CsvDataParam
{
	public int id { get;  set; }
	public string name { get;  set; }
	public string path { get;  set; }
	public string url { get;  set; }
	public string kakucho { get;  set; }
}

public class CsvVoiceset : CsvData<CsvVoicesetData> {
	public void Load() {
		Load (DataManagerAlarm.Instance.FILENAME_VOICESET_LIST);
	}

	protected override CsvVoicesetData makeParam (System.Collections.Generic.List<SpreadSheetData> _list, int _iSerial, int _iRow)
	{
		SpreadSheetData id = SpreadSheetData.GetSpreadSheet( _list, _iRow , 1);
		SpreadSheetData name= SpreadSheetData.GetSpreadSheet( _list,_iRow , 2 );
		SpreadSheetData path= SpreadSheetData.GetSpreadSheet( _list,_iRow , 3 );
		SpreadSheetData url= SpreadSheetData.GetSpreadSheet( _list,_iRow , 4 );
		SpreadSheetData kakucho= SpreadSheetData.GetSpreadSheet( _list,_iRow , 5 );

		CsvVoicesetData retParam = new CsvVoicesetData ();
		retParam.id = int.Parse(id.param);
		retParam.name = name.param;
		retParam.path = path.param;
		retParam.url = url.param;
		retParam.kakucho = kakucho.param;
		return retParam;

	}
}



