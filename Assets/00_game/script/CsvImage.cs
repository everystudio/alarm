using UnityEngine;
using System.Collections;

public class CsvImageData : CsvDataParam
{
	public int id { get; private set; }
	public int type { get; private set; }
	public string name { get; private set; }
	public string description { get; private set; }
	public string name_image { get; private set; }
	public string name_icon { get; private set; }
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
}


