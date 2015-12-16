using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class DataManagerAlarm : DataManagerBase<DataManagerAlarm> {


	public CsvImage m_csvImage = new CsvImage();
	public List<CsvImageData> master_image_list {
		get{ 
			return Instance.m_csvImage.All;
		}
	}
	public CsvVoice m_csvVoice = new CsvVoice();
	public List<CsvVoiceData> master_voice_list {
		get{ 
			return Instance.m_csvVoice.All;
		}
	}
	public CsvVoiceset m_csvVoiceset = new CsvVoiceset();
	public List<CsvVoicesetData> master_voiceset_list {
		get{ 
			return Instance.m_csvVoiceset.All;
		}
	}


	public override void Initialize ()
	{
		base.Initialize ();

		m_csvImage.Load ();
		m_csvVoice.Load ();
		m_csvVoiceset.Load ();

	}
	public string [] STR_MONTH_SHORT_ARR = new string[13]{
		"NONE",
		"JAN",
		"FEB",
		"MAR",
		"APR",
		"MAY",
		"JUN",
		"JUL",
		"AUG",
		"SEP",
		"OCT",
		"NOV",
		"DEC"
	};

	public string [] STR_WEEK_ARR = new string[7]{
		"Monday",
		"Tuesday",
		"Wednesday",
		"Thursday",
		"Friday",
		"Saturday",
		"Sunday"
	};
	public string [] STR_WEEK_SHORT_ARR = new string[7]{
		"Mon",
		"Tue",
		"Wed",
		"Thu",
		"Fri",
		"Sat",
		"Sun"
	};
	public string [] STR_SNOOZE_ARR = new string[3]{
		"5 min",
		"10 min",
		"None",
	};
	public const string KEY_SELECTING_IMAGE_ID = "selecting_image_id";

	public CsvVoiceData GetVoiceData( int _iId ){
		foreach (CsvVoiceData voice_data in DataManagerAlarm.Instance.master_voice_list) {
			if (_iId == voice_data.id) {
				return voice_data;
			}
		}
		return new CsvVoiceData ();
	}
	public List<GoogleSkuInfo> product_data_list = new List<GoogleSkuInfo> ();
	public List<string> purchased_list = new List<string> ();
}

[System.Serializable]
public class AlarmReserve{
	public long m_lTime;
	public string m_strTime;
	public int m_iVoiceType;
	public int m_iSnoozeType;
}


