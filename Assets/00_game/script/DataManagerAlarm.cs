using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using Prime31;

public class DataManagerAlarm : DataManagerBase<DataManagerAlarm> {

	private string spread_sheet = "";
	public string SPREAD_SHEET{
		get{
			if (spread_sheet.Equals ("")) {
				/*
				CsvConfig core_config = new CsvConfig ();
				core_config.LoadResources ("csv/core_config");
				spread_sheet = core_config.Read ("spread_sheet");
				*/
				spread_sheet = core_config.Read ("spread_sheet");
			}
			return spread_sheet;
		}
	}

	public CsvConfig core_config{
		get{
			if (m_csvConfigCore == null) {
				CsvConfig core_config = new CsvConfig ();
				core_config.LoadResources ("csv/core_config");
				m_csvConfigCore = core_config;
			}
			return m_csvConfigCore;
		}
	}
	private CsvConfig m_csvConfigCore;


	public readonly string KEY_COMIC_LIST_VERSION = "comic_list_version";
	public readonly string KEY_IMAGE_LIST_VERSION = "image_list_version";
	public readonly string KEY_VOICE_LIST_VERSION = "voice_list_version";
	public readonly string KEY_VOICESET_LIST_VERSION = "voiceset_list_version";

	public readonly string FILENAME_COMIC_LIST = "comic_list";
	public readonly string FILENAME_IMAGE_LIST = "image_list";
	public readonly string FILENAME_VOICE_LIST = "voice_list";
	public readonly string FILENAME_VOICESET_LIST = "voiceset_list";

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
	public List<CsvVoicesetData> master_voiceset_list
	{
		get
		{
			return Instance.m_csvVoiceset.All;
		}
	}

	public CsvImage m_csvComic = new CsvImage("csv/comic_list");
	public List<CsvImageData> master_comic_list
	{
		get
		{
			return Instance.m_csvComic.All;
		}
	}
	public CsvAudio m_masterTableAudio = new CsvAudio();
	public CsvPrefab m_masterTablePrefab = new CsvPrefab();
	public CsvSprite m_masterTableSprite = new CsvSprite();

	public override void Initialize ()
	{
		SetDontDestroy(true);
		base.Initialize ();
		m_masterTableAudio.Load ("dummy");
		m_masterTablePrefab.Load ("dummy");
		m_masterTableSprite.Load ("dummy");
		SpriteManager.Instance.csv_sprite_list = m_masterTableSprite.All;
		PrefabManager.Instance.csv_prefab_list = m_masterTablePrefab.All;
		SoundManager.Instance.m_csvAudioDataList = m_masterTableAudio.All;

		m_csvImage.Load ();
		m_csvVoice.Load ();
		m_csvVoiceset.Load ();

		config.Load (CsvConfig.FILE_NAME);
		data_kvs.Load (DataKvs.FILE_NAME);
		if (config.Read ("footer").Contains ("Comic")) {
			m_csvComic.Load (FILENAME_COMIC_LIST);
		}
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
	public const string PRODUCT_NAME = "はるたけアプリ";
	public int ImageCheckIndex;

	public CsvVoiceData GetVoiceData( int _iId ){
		foreach (CsvVoiceData voice_data in DataManagerAlarm.Instance.master_voice_list) {
			if (_iId == voice_data.id) {
				return voice_data;
			}
		}
		return new CsvVoiceData ();
	}
	public List<string> purchased_list = new List<string> ();
	public void AddPurchasedList( string _strSKU ){
		purchased_list.Add (_strSKU);
	}

	public void AddAdsComponents()
	{
#if UNITY_ANDROID
		gameObject.AddComponent<AndroidGoogleAdsExample>();
#elif UNITY_IPHONE
#endif

	}


}

[System.Serializable]
public class AlarmReserve{
	public long m_lTime;
	public string m_strTime;
	public int m_iVoiceType;
	public int m_iSnoozeType;
}


