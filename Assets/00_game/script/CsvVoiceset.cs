using UnityEngine;
using System.Collections;

public class CsvVoicesetData : CsvDataParam
{
	public int id { get; private set; }
	public string name { get; private set; }
	public string path { get; private set; }
	public string kakucho { get; private set; }
}


public class CsvVoiceset : CsvData<CsvVoicesetData> {
	private static readonly string FilePath = "csv/voiceset_list";
	public void Load() { LoadResources(FilePath); }
}



