using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class KVSParam : CsvDataParam {
	public string m_key;
	public string m_value;

	public string key{ get{ return m_key;} set{m_key = value; } }
	public string value{ get{ return m_value;} set{m_value = value; } }
}

public class KvsData : CsvData<KVSParam>{
	public string READ_ERROR_STRING = "sql_datamanager_read_error";
	public const string FILE_NAME = "kvs";

	public KvsData(){
	}
	protected override void save ()
	{
		//Debug.LogError (string.Format( "kvs.save {0}" , list.Count));
		StreamWriter sw = Textreader.Open (string.Format ("{0}.csv", FILE_NAME));
		string strHead = string.Format ("{0},{1}",
			"key",
			"value"
		);

		Textreader.Write (sw, strHead);
		foreach (KVSParam data in list) {
			string strData = string.Format ("{0},{1}",
				data.key,
				data.value
			);
			//Debug.LogError (strData);
			//Textreader.write (strData);
			Textreader.Write (sw, strData);
			//Textreader.SaveWrite (string.Format ("{0}.csv", DBItem.FILE_NAME), strData);
		}
		Textreader.Close( sw );
		return;
	}

	public void Write( string _strKey , string _strValue ){

		foreach (KVSParam data in list) {
			if (data.key.Equals (_strKey) == true) {
				data.value = _strValue;
				return;
			}
		}

		KVSParam insert_data = new KVSParam ();
		insert_data.key = _strKey;
		insert_data.value = _strValue;
		list.Add (insert_data);
		return;
	}
	public void WriteString(string _strKey , string _strValue){
		this.Write(_strKey , _strValue);
	}

	public void WriteInt( string _strKey , int _intValue ){
		this.WriteString( _strKey , _intValue.ToString());
	}

	public string Read( string _strKey ){
		foreach (KVSParam data in list) {
			if (data.key.Equals (_strKey) == true) {
				return data.value;
			}
		}
		return READ_ERROR_STRING;
	}

	public int ReadInt( string _strKey ){
		string strValue = this.Read(_strKey);
		if( READ_ERROR_STRING == strValue ){
			strValue = "0";
			WriteInt (_strKey, 0);
		}
		return int.Parse(strValue);
	}




}






