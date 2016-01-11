using UnityEngine;
using System.Collections;

public class CsvConfigParam : CsvKvsParam
{

}

public class CsvConfig: CsvKvs {
	private static readonly string FilePath = "csv/config";
	public void Load() { LoadResources(FilePath); }
}



