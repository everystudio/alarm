using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson_AppMarket;

public class AppMarketHelper : MonoBehaviour {

	// License
	public const int LICENSE_RESPONSE_RESULT_OWN = 0;
	public const int LICENSE_RESPONSE_RESULT_NOT_OWN = 1;
	public const int LICENSE_RESPONSE_RESULT_INVALID_VERSION = 3;

	// Default policy, can be licensed under the offline environment for a short term
	public const string SERVER_MANAGED_POLICY = "ServerManagedPolicy";
	
	// Strict policy, must need valid server response everytime
	public const string STRICT_POLICY = "StrictPolicy";

	// Billing response codes
	public const int BILLING_RESPONSE_RESULT_OK = 0;
	public const int BILLING_RESPONSE_RESULT_USER_CANCELED = 1;
	public const int BILLING_RESPONSE_RESULT_BILLING_UNAVAILABLE = 3;
	public const int BILLING_RESPONSE_RESULT_ITEM_UNAVAILABLE = 4;
	public const int BILLING_RESPONSE_RESULT_DEVELOPER_ERROR = 5;
	public const int BILLING_RESPONSE_RESULT_ERROR = 6;
	public const int BILLING_RESPONSE_RESULT_ITEM_ALREADY_OWNED = 7;
	public const int BILLING_RESPONSE_RESULT_ITEM_NOT_OWNED = 8;
	
	// IAB Helper error codes
	public const int IABHELPER_ERROR_BASE = -1000;
	public const int IABHELPER_REMOTE_EXCEPTION = -1001;
	public const int IABHELPER_BAD_RESPONSE = -1002;
	public const int IABHELPER_VERIFICATION_FAILED = -1003;
	public const int IABHELPER_SEND_INTENT_FAILED = -1004;
	public const int IABHELPER_USER_CANCELLED = -1005;
	public const int IABHELPER_UNKNOWN_PURCHASE_RESPONSE = -1006;
	public const int IABHELPER_MISSING_TOKEN = -1007;
	public const int IABHELPER_UNKNOWN_ERROR = -1008;
	public const int IABHELPER_SUBSCRIPTIONS_NOT_AVAILABLE = -1009;
	public const int IABHELPER_INVALID_CONSUMPTION = -1010;
	
	// Keys for the responses from InAppBillingService
	public const string RESPONSE_CODE = "RESPONSE_CODE";
	public const string RESPONSE_GET_SKU_DETAILS_LIST = "DETAILS_LIST";
	public const string RESPONSE_BUY_INTENT = "BUY_INTENT";
	public const string RESPONSE_INAPP_PURCHASE_DATA = "INAPP_PURCHASE_DATA";
	public const string RESPONSE_INAPP_SIGNATURE = "INAPP_DATA_SIGNATURE";
	public const string RESPONSE_INAPP_ITEM_LIST = "INAPP_PURCHASE_ITEM_LIST";
	public const string RESPONSE_INAPP_PURCHASE_DATA_LIST = "INAPP_PURCHASE_DATA_LIST";
	public const string RESPONSE_INAPP_SIGNATURE_LIST = "INAPP_DATA_SIGNATURE_LIST";
	public const string INAPP_CONTINUATION_TOKEN = "INAPP_CONTINUATION_TOKEN";
	
	// Item types
	public const string ITEM_TYPE_INAPP = "inapp";
	public const string ITEM_TYPE_SUBS = "subs";
	
	public const string ACTION_SET_UP = "setUp";
	public const string ACTION_ERROR = "error";

	public const string ACTION_INIT_BILLING = "initBilling";
	public const string ACTION_QUERY_INVENTORY = "queryInventory";
	public const string ACTION_LAUNCH_PURCHASE_FLOW = "launchPurchaseFlow";
	public const string ACTION_LAUNCH_SUBSCRIPTION_PURCHASE_FLOW = "launchSubscriptionPurchaseFlow";
	public const string ACTION_CONSUME = "consume";
	public const string ACTION_QUERY_SKU_DETAILS = "querySkuDetails";

	public const string ACTION_INIT_LICENSE = "initLicense";
	public const string ACTION_CHECK_LICENSE = "checkLicense";

	const string CALLBACK_METHOD_NAME = "RakutenAppMarketHelperRawCallback";

	public static AppMarketHelper Instance { get; private set; }
	
	public string PackageName { get; private set; }
	
	AndroidJavaClass ActivityCls;
	AndroidJavaObject ActivityObj;
	
	string CallbackSpec = "";
	string CallbackMessage = "";
	int CallbackPacketCnt = 0;
	int CallbackPacketIdx = 0;
	
	void Awake()
	{
		DebugLogger.Log("AppMarketHelper.Awake");

		// Save a reference as the singleton instance
		Instance = this;
		
		DebugLogger.Log("AppMarketHelper.Awake loading jar");

		ActivityCls = new AndroidJavaClass ("jp.co.rakuten.appmarket.unitysdk.AppMarketActivity");
		ActivityObj = ActivityCls.CallStatic<AndroidJavaObject> ("getInstance");

		PackageName = ActivityObj.Call<string> ("getPackageName");

#if DEBUG
		ActivityObj.Call("setRunningAsDebug");
#endif

		DebugLogger.Log("AppMarketHelper.Awake loading jar done");
	}
	
	AppMarketHelperCallback StoredCallback;
	
	public void SetUp(AppMarketHelperCallback callback, string base64EncodedKey)
	{
		DebugLogger.Log("AppMarketHelper.SetUp");
        StoredCallback = callback;
		ActivityObj.Call(ACTION_SET_UP, new object[] {
			base64EncodedKey,
			gameObject.name,
			CALLBACK_METHOD_NAME,
		});
	}
	
	public void InitBilling()
	{
		DebugLogger.Log("AppMarketHelper.InitBilling");
        ActivityObj.Call(ACTION_INIT_BILLING);
    }
    
	public void InitLicense (string policy)
	{
		DebugLogger.Log("AppMarketHelper.InitLicense " + policy);
		ActivityObj.Call(ACTION_INIT_LICENSE, new object[] { policy });
	}
	
	public void QueryInventory()
	{
		DebugLogger.Log("AppMarketHelper.QueryInventory");
		ActivityObj.Call(ACTION_QUERY_INVENTORY);
	}
	
	public void LaunchPurchaseFlow(string itemId, string payload)
	{
		DebugLogger.Log("AppMarketHelper.LaunchPurchaseFlow " + itemId);
		ActivityObj.Call(ACTION_LAUNCH_PURCHASE_FLOW, new object[] { itemId, payload });
	}
	
	public void LaunchSubscriptionPurchaseFlow(string itemId, string payload)
	{
		DebugLogger.Log("AppMarketHelper.LaunchSubscriptionPurchaseFlow " + itemId);
		ActivityObj.Call(ACTION_LAUNCH_SUBSCRIPTION_PURCHASE_FLOW, new object[] { itemId, payload });
	}
	
	public void Consume(string itemId)
	{
		DebugLogger.Log("AppMarketHelper.Consume " + itemId);
		ActivityObj.Call(ACTION_CONSUME, new object[] { itemId });
	}
	
	public void QuerySkuDetails(string[] itemIds)
	{
		DebugLogger.Log("AppMarketHelper.QuerySkuDetails " + itemIds);
		ActivityObj.Call(ACTION_QUERY_SKU_DETAILS, new object[] { itemIds });
	}

	public void CheckLicense ()
	{
		DebugLogger.Log("AppMarketHelper.CheckLicense");
        ActivityObj.Call (ACTION_CHECK_LICENSE);
    }
    
    public void RakutenAppMarketHelperRawCallback(string packet)
	{
		DebugLogger.Log("AppMarketHelper.RakutenAppMarketHelperRawCallback " + packet);
		
		if (StoredCallback == null) {
			DebugLogger.Log ("Callback is not ready.");
			return;
		}
		
		if (CallbackPacketCnt == 0) {
			string[] splitted = packet.Split (new char[] {':'}, 2);
			CallbackSpec = splitted[0];
			CallbackMessage = "";
			CallbackPacketIdx = 0;
			CallbackPacketCnt = int.Parse(splitted[1]);
			
			DebugLogger.Log("CallbackSpec: " + CallbackSpec + ", CallbackPacketCnt:" + CallbackPacketCnt);
			
			return;
		}
		CallbackMessage += packet;
		
		if (++CallbackPacketIdx < CallbackPacketCnt) {
			return;
		}
		
		// DebugLogger.Log("CallbackMessage: " + CallbackMessage);
		
		CallbackPacketCnt = 0;
		JsonData paramsJson = JsonMapper.ToObject(CallbackMessage);
		
		switch (CallbackSpec) {
		case ACTION_SET_UP: {
			StoredCallback.OnAppMarketSetupFinished();
			break;
		}
			
		case ACTION_INIT_BILLING: {
			IabResult param1 = new IabResult(paramsJson[0]);
			StoredCallback.OnInitBIllingFinished(param1);
            break;
        }
            
		case ACTION_INIT_LICENSE: {
			StoredCallback.OnInitLicenseFinished();
			break;
        }
            
        case ACTION_QUERY_INVENTORY: {
			IabResult param1 = new IabResult(paramsJson[0]);
			IabInventory param2 = null;
			if (1 < paramsJson.Count) {
				param2 = new IabInventory(paramsJson[1]);
			}
			StoredCallback.OnIabQueryInventoryFinished(param1, param2);
			break;
		}
		case ACTION_LAUNCH_PURCHASE_FLOW: {
			IabResult param1 = new IabResult(paramsJson[0]);
			IabPurchase param2 = null;
			if (1 < paramsJson.Count) {
				param2 = new IabPurchase(paramsJson[1]);
			}
			StoredCallback.OnIabPurchaseFinished(param1, param2);
			break;
		}
		case ACTION_LAUNCH_SUBSCRIPTION_PURCHASE_FLOW: {
			IabResult param1 = new IabResult(paramsJson[0]);
			IabPurchase param2 = null;
			if (1 < paramsJson.Count) {
				param2 = new IabPurchase(paramsJson[1]);
			}
			StoredCallback.OnIabSubscriptionPurchaseFinished(param1, param2);
			break;
		}
		case ACTION_CONSUME: {
			IabResult param1 = new IabResult(paramsJson[0]);
			IabPurchase param2 = null;
			if (1 < paramsJson.Count) {
				param2 = new IabPurchase(paramsJson[1]);
			}
			StoredCallback.OnIabConsumeFinished(param1, param2);
			break;
		}
		case ACTION_QUERY_SKU_DETAILS: {
			IabResult param1 = new IabResult(paramsJson[0]);
			Dictionary<string, IabSkuDetails> param2 = new Dictionary<string, IabSkuDetails>();
			if (1 < paramsJson.Count) {
				foreach (string k in paramsJson[1].Keys) {
					param2.Add (k, new IabSkuDetails(paramsJson[1][k]["ItemType"].ToString(), paramsJson[1][k]));
				}
			}
			StoredCallback.OnIabQuerySkuDetailsFinished(param1, param2);
			break;
		}
		case ACTION_CHECK_LICENSE:
		{
			bool param1 = (bool)paramsJson[0];
			bool param2 = (bool)paramsJson[1];
			StoredCallback.OnLicenseCheckFinished(param1, param2);
            break;
        }
        case ACTION_ERROR: {
			string param1 = paramsJson[0].ToString();
			string param2 = paramsJson[1].ToString();
			StoredCallback.OnAppMarketError(param1, param2);
            break;
        }
        }
	}
	
	
	/**
     * Returns a human-readable description for the given response code.
     *
     * @param code The response code
     * @return A human-readable string explaining the result code.
     *     It also includes the result code numerically.
     */
	public static string GetResponseDesc(int code) {
		string[] iab_msgs = ("0:OK/1:User Canceled/2:Unknown/" +
		                     "3:Billing Unavailable/4:Item unavailable/" +
		                     "5:Developer Error/6:Error/7:Item Already Owned/" +
		                     "8:Item not owned").Split(new char[] { '/' });
		string[] iabhelper_msgs = ("0:OK/-1001:Remote exception during initialization/" +
		                           "-1002:Bad response received/" +
		                           "-1003:Purchase signature verification failed/" +
		                           "-1004:Send intent failed/" +
		                           "-1005:User cancelled/" +
		                           "-1006:Unknown purchase response/" +
		                           "-1007:Missing token/" +
		                           "-1008:Unknown error/" +
		                           "-1009:Subscriptions not available/" +
		                           "-1010:Invalid consumption attempt").Split(new char[] { '/' });
		
		if (code <= IABHELPER_ERROR_BASE) {
			int index = IABHELPER_ERROR_BASE - code;
			if (index >= 0 && index < iabhelper_msgs.Length) return iabhelper_msgs[index];
			else return code.ToString() + ":Unknown IAB Helper Error";
		} else if (code < 0 || code >= iab_msgs.Length)
			return code.ToString() + ":Unknown";
		else {
			return iab_msgs[code];
		}
	}
	
}
