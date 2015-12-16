using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AlarmParam : CsvDataParam {
	public int m_serial;
	public string m_time;
	public int m_status;
	public long m_repeat_type;
	public int m_snooze;
	public int m_voice_type;
	public int serial{ get{ return m_serial;} set{m_serial = value; } }
	public string time{ get{ return m_time;} set{m_time = value; } }
	public int status{ get{ return m_status;} set{m_status = value; } }
	public long repeat_type{ get{ return m_repeat_type;} set{m_repeat_type = value; } }
	public int snooze{ get{ return m_snooze;} set{m_snooze = value; } }
	public int voice_type{ get{ return m_voice_type;} set{m_voice_type = value; } }
}

public class AlarmData : CsvData<AlarmParam>{

	public const string FILENAME = "alarmparam";

	public override void Load (string _strFilename)
	{
		string file = string.Format ("{0}.csv", _strFilename);
		string pathDB = System.IO.Path.Combine (Application.persistentDataPath, file);
		if (System.IO.File.Exists (pathDB) == false ) {
			list.Clear ();
			AlarmParam param1 = new AlarmParam ();
			param1.serial = 1;
			param1.snooze = 2;
			param1.voice_type = 1;
			param1.time = "2015-10-10 06:00:00";
			AlarmParam param2 = new AlarmParam ();
			param2.serial = 2;
			param2.snooze = 2;
			param2.voice_type = 1;
			param2.time = "2015-10-10 07:00:00";
			AlarmParam param3 = new AlarmParam ();
			param3.serial = 3;
			param3.snooze = 2;
			param3.voice_type = 1;
			param3.time = "2015-10-10 08:00:00";
			AlarmParam param4 = new AlarmParam ();
			param4.serial = 4;
			param4.snooze = 2;
			param4.voice_type = 1;
			param4.time = "2015-10-10 09:00:00";
			AlarmParam param5 = new AlarmParam ();
			param5.serial = 5;
			param5.snooze = 2;
			param5.voice_type = 1;
			param5.time = "2015-10-10 10:00:00";
			list.Add (param1);
			list.Add (param2);
			list.Add (param3);
			list.Add (param4);
			list.Add (param5);
			/*
			list.Add (new AlarmParam (1,  "2015-10-10 06:00:00"));
			list.Add (new AlarmParam (2,  "2015-10-10 07:00:00"));
			list.Add (new AlarmParam (3,  "2015-10-10 08:00:00"));
			list.Add (new AlarmParam (4,  "2015-10-10 09:00:00"));
			list.Add (new AlarmParam (5,  "2015-10-10 10:00:00"));
			*/
			save ();
		}

		base.Load (_strFilename);
	}

	protected override void save ()
	{
		StreamWriter sw = Textreader.Open (string.Format ("{0}.csv", "alarmparam"));

		string strHead = string.Format ("{0},{1},{2},{3},{4},{5}",
			"serial",
			"time",
			"status",
			"repeat_type",
			"snooze",
			"voice_type"
		);

		Textreader.Write (sw, strHead);
		foreach (AlarmParam data in list) {
			string strData = string.Format ("{0},{1},{2},{3},{4},{5}",
				data.serial,
				data.time,
				data.status,
				data.repeat_type,
				data.snooze,
				data.voice_type
			);
			Textreader.Write (sw, strData);
			//Textreader.SaveWrite (string.Format ("{0}.csv", DBItem.FILE_NAME), strData);
		}
		Textreader.Close( sw );
		return;
	}

	public bool UpdateStatus( int _iSerial , int _iStatus ){
		foreach (AlarmParam param in list) {
			if (param.serial == _iSerial) {
				param.status = _iStatus;
				return true;
			}
		}
		return false;
	}


}










