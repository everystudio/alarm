using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface AppMarketHelperCallback {

	void OnAppMarketSetupFinished();
	void OnAppMarketError(string action, string message);

	void OnInitBIllingFinished(IabResult result);
	void OnIabQueryInventoryFinished(IabResult result, IabInventory inventory);
	void OnIabQuerySkuDetailsFinished(IabResult result, Dictionary<string, IabSkuDetails> skuDetailsDictionary);
	void OnIabPurchaseFinished(IabResult result, IabPurchase purchase);
	void OnIabSubscriptionPurchaseFinished(IabResult result, IabPurchase purchase);
	void OnIabConsumeFinished(IabResult result, IabPurchase purchase);

	void OnInitLicenseFinished();
	void OnLicenseCheckFinished(bool isSucceed, bool isLicensed);
}
