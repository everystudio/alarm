////////////////////////////////////////////////////////////////////////////////
//  
// @module Android Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////
 
using UnityEngine;
using System.Collections;

public class GameBillingManager : MonoBehaviour {

	private static bool _isInited = false;
	
	//--------------------------------------
	//  INITIALIZE
	//--------------------------------------


	//replace with your consumable item
	public const string COINS_ITEM = "small_coins_bag";

	//replace with your non-consumable item
	public const string COINS_BOOST = "coins_bonus";


	
	private static bool ListnersAdded = false;
	public static void init() {
		Debug.LogError("GameBillingManager.init");
		if(ListnersAdded) {
			return;
		}
		AndroidNativeSettings.Instance.base64EncodedPublicKey = DataManagerAlarm.Instance.config.Read("publickey");

		//Filling product list
		//You can skip this if you alredy did this in Editor settings menu
		//AndroidInAppPurchaseManager.Client.AddProduct(COINS_ITEM);
		//AndroidInAppPurchaseManager.Client.AddProduct(COINS_BOOST);

		for (int i = 0; i < 10; i++) {
			string strKey = string.Format ("item{0:D3}", i);
			if (DataManagerAlarm.Instance.core_config.HasKey (strKey)) {
				Debug.LogError (strKey);
				string sku = DataManagerAlarm.Instance.core_config.Read (strKey);
				AndroidInAppPurchaseManager.Client.AddProduct (sku);
			}
		}



		
		//listening for purchase and consume events

		AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;
		AndroidInAppPurchaseManager.ActionProductConsumed += OnProductConsumed;
		AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;

	

		//you may use loadStore function without parametr if you have filled base64EncodedPublicKey in plugin settings
		AndroidInAppPurchaseManager.Client.Connect();

		ListnersAdded = true;
		
		
	}
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	
	public static void purchase(string SKU) {
		AndroidInAppPurchaseManager.Client.Purchase (SKU);
	}
	
	public static void consume(string SKU) {
		AndroidInAppPurchaseManager.Client.Consume (SKU);
	}
	
	//--------------------------------------
	//  GET / SET
	//--------------------------------------
	
	public static bool isInited {
		get {
			return _isInited;
		}
	}
	
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	
	private static void OnProcessingPurchasedProduct(GooglePurchaseTemplate purchase) {
		//some stuff for processing product purchse. Add coins, unlock track, etc

		switch(purchase.SKU) {
		case COINS_ITEM:
			consume(COINS_ITEM);
			break;
		case COINS_BOOST:
			GameDataExample.EnableCoinsBoost();
			break;

		default:
			DataManagerAlarm.Instance.AddPurchasedList (purchase.SKU);

			break;
		}
	}
	
	private static void OnProcessingConsumeProduct(GooglePurchaseTemplate purchase) {
		switch(purchase.SKU) {
		case COINS_ITEM:
			GameDataExample.AddCoins(100);
			break;
		}
	}
	
	private static void OnProductPurchased(BillingResult result) {

		//this flag will tell you if purchase is available
		//result.IsSuccess


		//infomation about purchase stored here
		//result.purchase

		//here is how for example you can get product SKU
		//result.purchase.SKU

		
		if(result.IsSuccess) {
			OnProcessingPurchasedProduct (result.Purchase);
		} else {
			//AndroidMessage.Create("Product Purchase Failed", result.Response.ToString() + " " + result.Message);
		}
		
		Debug.Log ("Purchased Responce: " + result.Response.ToString() + " " + result.Message);
	}
	
	
	private static void OnProductConsumed(BillingResult result) {
		
		if(result.IsSuccess) {
			OnProcessingConsumeProduct (result.Purchase);
		} else {
			//AndroidMessage.Create("Product Cousume Failed", result.Response.ToString() + " " + result.Message);
		}
		
		Debug.Log ("Cousume Responce: " + result.Response.ToString() + " " + result.Message);
	}
	
	
	private static void OnBillingConnected(BillingResult result) {

		AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;
		
		
		if (result.IsSuccess) {
			//Store connection is Successful. Next we loading product and customer purchasing details
			AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetrieveProductsFinised;
			AndroidInAppPurchaseManager.Client.RetrieveProducDetails ();

		} else {
		
			//AndroidMessage.Create ("errConnection Responce", result.Response.ToString () + " msg:" + result.Message);
			Debug.Log ("Connection Responce: " + result.Response.ToString () + " " + result.Message);
		}
	}
	
	
	
	
	private static void OnRetrieveProductsFinised(BillingResult result) {
		AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= OnRetrieveProductsFinised;
		if(result.IsSuccess) {
			UpdateStoreData();
			_isInited = true;
		} else {
			//AndroidMessage.Create("Connection Responce", result.Response.ToString() + " " + result.Message);
		}
	}



	private static void UpdateStoreData() {

		foreach(GoogleProductTemplate p in AndroidInAppPurchaseManager.Client.Inventory.Products) {
			Debug.Log("Loaded product: " + p.Title);
		}

		//chisking if we already own some consuamble product but forget to consume those
		/* sample
		if(AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased(COINS_ITEM)) {
			consume(COINS_ITEM);
		}
		*/




		//Check if non-consumable rpduct was purchased, but we do not have local data for it.
		//It can heppens if game was reinstalled or download on oher device
		//This is replacment for restore purchase fnunctionality on IOS

		/* sample
		if(AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased(COINS_BOOST)) {
			GameDataExample.EnableCoinsBoost();
		}
		*/

		for (int i = 0; i < 10; i++) {
			string strKey = string.Format ("item{0:D3}", i);
			if (DataManagerAlarm.Instance.core_config.HasKey (strKey)) {
				if(AndroidInAppPurchaseManager.Client.Inventory.IsProductPurchased(strKey)) {
					DataManagerAlarm.Instance.AddPurchasedList (strKey);
					// 開放処理
				}
			}
		}

	}
	
}
