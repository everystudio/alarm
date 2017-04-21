////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SA.IOSNative.StoreKit;
using UnityEngine.Purchasing;

public class PaymentManagerAlarm :Singleton<PaymentManagerAlarm>, IStoreListener{
	
	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------
	
	public const string SMALL_PACK 	=  "your.product.id1.here";
	public const string NC_PACK 	=  "your.product.id2.here";


	public string lastTransactionProdudctId = string.Empty;

	private IStoreController m_StoreController;
	private IExtensionProvider m_StoreExtensionProvider;

	public void OnInitialized (IStoreController controller, IExtensionProvider extensions){
		//IOSNativePopUpManager.showMessage("StoreKit Init","Succeeded" );
		m_StoreController = controller;
		m_StoreExtensionProvider = extensions;
		m_StoreExtensionProvider.GetExtension<IAppleExtensions> ().RestoreTransactions (result => {
			if (result) {



				foreach (var product in m_StoreController.products.all)
				{
					if (product.hasReceipt)
					{ // リストア可能なレシートあるよ! 
					  // product.definition.id - アイテムID
					  // product.receipt - レシート関連(JSON) 
					  // サーバー検証して アイテム復元しなはれ
						DataManagerAlarm.Instance.restore_data.Add(product.definition.id);
					}
				}






			}
			else {
			}
		});
	}

	public void OnInitializeFailed (InitializationFailureReason error){
		IOSNativePopUpManager.showMessage("StoreKit Init" ,"Failed:"+error.ToString() );

	}

	public void OnPurchaseFailed (UnityEngine.Purchasing.Product i, PurchaseFailureReason p){
		OnPurchased.Invoke (false);
	}

	public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e){

		DataManagerAlarm.Instance.AddPurchasedList (e.purchasedProduct.definition.id);


		OnPurchased.Invoke (true);



		return PurchaseProcessingResult.Complete;
	}




	private bool IsInitialized = false;
	public override void Initialize ()
	{
		base.Initialize ();

		if(!IsInitialized) {

			//You do not have to add products by code if you already did it in seetings guid
			//Windows -> IOS Native -> Edit Settings
			//Billing tab.

			/*
			for (int i = 0; i < 10; i++) {
				string strKey = string.Format ("item{0:D3}", i);
				if (DataManagerAlarm.Instance.core_config.HasKey (strKey)) {
					string sku = DataManagerAlarm.Instance.core_config.Read (strKey);
					Debug.LogError (sku);
					PaymentManager.Instance.AddProductId(sku);
				}
			}
			*/

			var builder = ConfigurationBuilder.Instance (StandardPurchasingModule.Instance ());


			for (int i = 0; i < 10; i++)
			{
				string strKey = string.Format("item{0:D2}", i);
				if (DataManagerAlarm.Instance.core_config.HasKey(strKey))
				{
					Debug.LogError(strKey);
					string sku = DataManagerAlarm.Instance.core_config.Read(strKey);
					builder.AddProduct(sku, UnityEngine.Purchasing.ProductType.NonConsumable);
				}
			}
			//builder.AddProduct ("alarm.type.01", UnityEngine.Purchasing.ProductType.NonConsumable);
			//builder.AddProduct ("alarm.type.02", UnityEngine.Purchasing.ProductType.NonConsumable);
			//builder.AddProduct ("alarm.type.03", UnityEngine.Purchasing.ProductType.NonConsumable);
			UnityPurchasing.Initialize (this, builder);

			//Event Use Examples
			//PaymentManager.OnVerificationComplete += HandleOnVerificationComplete;
			//PaymentManager.OnStoreKitInitComplete += OnStoreKitInitComplete;


			//PaymentManager.OnTransactionComplete += OnTransactionComplete;
			//PaymentManager.OnRestoreComplete += OnRestoreComplete;


			IsInitialized = true;


			//PaymentManager.Instance.LoadStore();


		} 
			
	}



	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	
	public void buyItem(string productId) {

		UnityEngine.Purchasing.Product product = m_StoreController.products.WithID (productId);

		if (product != null && product.availableToPurchase) {
			m_StoreController.InitiatePurchase (product);
		}
		if (false == product.availableToPurchase)
		{
			IOSNativePopUpManager.showMessage("Purchase Failed", "not available:" + product.definition.id);
		}

		//PaymentManager.Instance.BuyProduct(productId);
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------


	private static void UnlockProducts(string productIdentifier) {
		switch(productIdentifier) {
		case SMALL_PACK:
			//code for adding small game money amount here
			break;
		case NC_PACK:
			//code for unlocking cool item here
			break;

		default:
			DataManagerAlarm.Instance.AddPurchasedList (productIdentifier);
			break;
		}


		//if you want to handle your transactions finish manually, call following method aftter trsansactions is finished
		PaymentManager.Instance.FinishTransaction (productIdentifier);


	}

	public UnityEventBool OnPurchased = new UnityEventBool();

	private static void OnTransactionComplete (PurchaseResult result) {

		ISN_Logger.Log("OnTransactionComplete: " + result.ProductIdentifier);
		ISN_Logger.Log("OnTransactionComplete: state: " + result.State);

		switch(result.State) {
		case PurchaseState.Purchased:
		case PurchaseState.Restored:
			//Our product been succsesly purchased or restored
			//So we need to provide content to our user depends on productIdentifier
			UnlockProducts (result.ProductIdentifier);

			break;
		case PurchaseState.Deferred:
			//iOS 8 introduces Ask to Buy, which lets parents approve any purchases initiated by children
			//You should update your UI to reflect this deferred state, and expect another Transaction Complete  to be called again with a new transaction state 
			//reflecting the parent’s decision or after the transaction times out. Avoid blocking your UI or gameplay while waiting for the transaction to be updated.
			break;
		case PurchaseState.Failed:
			//Our purchase flow is failed.
			//We can unlock intrefase and repor user that the purchase is failed. 
			ISN_Logger.Log("Transaction failed with error, code: " + result.Error.Code);
			ISN_Logger.Log("Transaction failed with error, description: " + result.Error.Message);


			break;
		}

		if(result.State == PurchaseState.Failed) {
			IOSNativePopUpManager.showMessage("Transaction Failed", "Error code: " + result.Error.Code + "\n" + "Error description:" + result.Error.Message);
		} else {
			IOSNativePopUpManager.showMessage("Store Kit Response", "product " + result.ProductIdentifier + " state: " + result.State.ToString());
		}

	}
 

	private static void OnRestoreComplete (RestoreResult res) {
		if(res.IsSucceeded) {
			IOSNativePopUpManager.showMessage("Success", "Restore Compleated");
		} else {
			IOSNativePopUpManager.showMessage("Error: " + res.Error.Code, res.Error.Message);
		}
	}	


	static void HandleOnVerificationComplete (VerificationResponse response) {
		IOSNativePopUpManager.showMessage("Verification", "Transaction verification status: " + response.Status.ToString());
		
		ISN_Logger.Log("ORIGINAL JSON: " + response.OriginalJSON);
	}
	

	private static void OnStoreKitInitComplete(SA.Common.Models.Result result) {

		if(result.IsSucceeded) {

			int avaliableProductsCount = 0;
			foreach(SA.IOSNative.StoreKit.Product tpl in PaymentManager.Instance.Products) {
				if(tpl.IsAvailable) {
					avaliableProductsCount++;
				}
			}
			IOSNativePopUpManager.showMessage("StoreKit Init Succeeded", "Available products count: " + avaliableProductsCount);
			ISN_Logger.Log("StoreKit Init Succeeded Available products count: " + avaliableProductsCount);
		} else {
			IOSNativePopUpManager.showMessage("StoreKit Init Failed",  "Error code: " + result.Error.Code + "\n" + "Error description:" + result.Error.Message);
		}
	}

	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
