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
#if UNITY_ANDROID
		/*
		Product item = controller.products.WithID("alarm.type.001");
		AndroidMessage.Create("Purchase OnInitialized", "SUCCESS:"+ item.availableToPurchase.ToString() );
		*/
#elif UNITY_IPHONE
		//IOSNativePopUpManager.showMessage("StoreKit Init","Succeeded" );
#endif
		m_StoreController = controller;
		m_StoreExtensionProvider = extensions;
		m_StoreExtensionProvider.GetExtension<IAppleExtensions> ().RestoreTransactions (result => {
			if (result) {
			} else {
			}
		});
	}

	public void OnInitializeFailed (InitializationFailureReason error){
#if UNITY_ANDROID
		AndroidMessage.Create("Purchase OnInitializeFailed", "Failed:"+ error.ToString());
#elif UNITY_IPHONE
		//IOSNativePopUpManager.showMessage("StoreKit Init" ,"Failed:"+error.ToString() );
#endif

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
			builder.AddProduct ("alarm.type.001", UnityEngine.Purchasing.ProductType.NonConsumable);
			builder.AddProduct ("alarm.type.002", UnityEngine.Purchasing.ProductType.NonConsumable);
			builder.AddProduct ("alarm.type.003", UnityEngine.Purchasing.ProductType.NonConsumable);
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


		//PaymentManager.Instance.BuyProduct(productId);
	}
	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	

	public UnityEventBool OnPurchased = new UnityEventBool();


 
	
	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------


}
