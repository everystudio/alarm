using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



namespace Prime31
{
	public class GoogleIABEventListener : MonoBehaviour
	{
#if UNITY_ANDROID
		void OnEnable()
		{
			// Listen to all events for illustration purposes
			GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
			GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
			GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
			GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
			GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
			GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
			GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
			GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
		}
	
	
		void OnDisable()
		{
			// Remove all event handlers
			GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
			GoogleIABManager.billingNotSupportedEvent -= billingNotSupportedEvent;
			GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
			GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
			GoogleIABManager.purchaseCompleteAwaitingVerificationEvent -= purchaseCompleteAwaitingVerificationEvent;
			GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
			GoogleIABManager.purchaseFailedEvent -= purchaseFailedEvent;
			GoogleIABManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
			GoogleIABManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
		}
	
	
	
		void billingSupportedEvent()
		{
			Debug.Log( "billingSupportedEvent" );

			// enter all the available skus from the Play Developer Console in this array so that item information can be fetched for them
			//int iCount = 0;
			List<string> product_id_list = new List<string> ();
			foreach (CsvVoiceData data in DataManagerAlarm.Instance.master_voice_list) {
				if (data.type == 2) {
					product_id_list.Add (data.name_voice);
				}
			}

			//var skus = new string[] { "com.prime31.testproduct", "android.test.purchased", "com.prime31.managedproduct", "com.prime31.testsubscription" };
			var skus = new string[product_id_list.Count];// { "com.prime31.testproduct", "android.test.purchased", "com.prime31.managedproduct", "com.prime31.testsubscription" };
			for (int i = 0; i < product_id_list.Count; i++) {
				skus [i] = product_id_list [i];
			}
			GoogleIAB.queryInventory( skus );


		}
	
	
		void billingNotSupportedEvent( string error )
		{
			Debug.Log( "billingNotSupportedEvent: " + error );
		}
	
	
		void queryInventorySucceededEvent( List<GooglePurchase> purchases, List<GoogleSkuInfo> skus )
		{
			//Debug.Log( string.Format( "queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count ) );
			DataManagerAlarm.Instance.purchased_list.Clear ();
			foreach (GooglePurchase purchase in purchases) {
				DataManagerAlarm.Instance.purchased_list.Add (purchase.productId);
				//Debug.LogError( string.Format( "productId:{0}" ,purchase.productId ));
			}
			DataManagerAlarm.Instance.product_data_list = skus;
			/*
			foreach (GoogleSkuInfo info in DataManagerAlarm.Instance.product_data_list) {
				Debug.LogError (string.Format ("product_id:{0}", info.productId));
			}
			*/

			//Prime31.Utils.logObject( purchases );
			//Prime31.Utils.logObject( skus );
		}
	
	
		void queryInventoryFailedEvent( string error )
		{
			Debug.Log( "queryInventoryFailedEvent: " + error );
		}
	
	
		void purchaseCompleteAwaitingVerificationEvent( string purchaseData, string signature )
		{
			Debug.Log( "purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature );
		}
	
	
		void purchaseSucceededEvent( GooglePurchase purchase )
		{
			Debug.Log( "purchaseSucceededEvent: " + purchase );

			DataManagerAlarm.Instance.purchased_list.Add (purchase.productId);
			GameMain.Instance.Purchase (purchase.productId);
		}
	
	
		void purchaseFailedEvent( string error, int response )
		{
			Debug.Log( "purchaseFailedEvent: " + error + ", response: " + response );
		}
	
	
		void consumePurchaseSucceededEvent( GooglePurchase purchase )
		{
			Debug.Log( "consumePurchaseSucceededEvent: " + purchase );
		}
	
	
		void consumePurchaseFailedEvent( string error )
		{
			Debug.Log( "consumePurchaseFailedEvent: " + error );
		}
	
	
#endif
	}

}
	
	
