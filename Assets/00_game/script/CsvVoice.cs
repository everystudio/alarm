using UnityEngine;
using System.Collections;

public class CsvVoiceData : CsvDataParam
{
	public int id { get;  set; }
	public int type { get;  set; }
	public string name { get;  set; }
	public string description { get;  set; }
	public string name_icon { get;  set; }
	public string name_voice { get;  set; }
}


public class CsvVoice : CsvData<CsvVoiceData> {
//	private static readonly string FilePath = "csv/voice_list";
	public void Load() {
		Load(DataManagerAlarm.Instance.FILENAME_VOICE_LIST);
	}

	protected override CsvVoiceData makeParam (System.Collections.Generic.List<SpreadSheetData> _list, int _iSerial, int _iRow)
	{
		SpreadSheetData id = SpreadSheetData.GetSpreadSheet( _list, _iRow , 1);
		SpreadSheetData type= SpreadSheetData.GetSpreadSheet( _list,_iRow , 2 );
		SpreadSheetData name= SpreadSheetData.GetSpreadSheet( _list,_iRow , 3 );
		SpreadSheetData description= SpreadSheetData.GetSpreadSheet( _list,_iRow , 4 );
		SpreadSheetData name_icon= SpreadSheetData.GetSpreadSheet( _list,_iRow , 5 );
		SpreadSheetData name_voice= SpreadSheetData.GetSpreadSheet( _list,_iRow , 6 );

		CsvVoiceData retParam = new CsvVoiceData ();
		retParam.id = int.Parse(id.param);
		retParam.type = int.Parse(type.param);
		retParam.name = name.param;
		retParam.description = description.param;
		retParam.name_icon= name_icon.param;
		retParam.name_voice = name_voice.param;
		return retParam;
	
	}


}



