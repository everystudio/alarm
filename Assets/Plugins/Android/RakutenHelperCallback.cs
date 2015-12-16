using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RakutenHelperCallback : AppMarketHelperCallback {
	public void OnAppMarketSetupFinished(){
		Debug.Log ("OnAppMarketSetupFinished");
	}
	public void OnAppMarketError(string action, string message){
		Debug.Log ( string.Format( "OnAppMarketError action:{0} message:{1}" , action , message));
	}

	public void OnInitBIllingFinished(IabResult result){
	}
	public void OnIabQueryInventoryFinished(IabResult result, IabInventory inventory){



	}
	public void OnIabQuerySkuDetailsFinished(IabResult result, Dictionary<string, IabSkuDetails> skuDetailsDictionary){
	}
	public void OnIabPurchaseFinished(IabResult result, IabPurchase purchase){
	}
	public void OnIabSubscriptionPurchaseFinished(IabResult result, IabPurchase purchase){
	}
	public void OnIabConsumeFinished(IabResult result, IabPurchase purchase){
	}

	public void OnInitLicenseFinished(){
	}
	public void OnLicenseCheckFinished(bool isSucceed, bool isLicensed){
	}

}




/*
	// AppMarketHelperCallback
public void OnAppMarketSetupFinished ()
{
	Debug.Log ("OnAppMarketSetupFinished");
	if (EvaluateResult(CallbackSpec.OnAppMarketSetupFinished, null, null)) {
		StepTest();
		return;
	}
	Debug.LogWarning("FAILED.");
}

public void OnAppMarketError (string action, string message)
{
	Debug.Log ("OnAppMarketError " + action + ", " + message);
	if (EvaluateResult(CallbackSpec.OnAppMarketError, action, message)) {
		StepTest();
		return;
	}
	Debug.LogWarning("FAILED.");
}

public void OnInitBIllingFinished (IabResult result)
{
	Debug.Log ("OnInitBIllingFinished " + result);
	if (EvaluateResult(CallbackSpec.OnInitBIllingFinished, result, null)) {
		StepTest();
		return;
	}
	Debug.LogWarning("FAILED.");
}

public void OnIabQueryInventoryFinished (IabResult result, IabInventory inventory)
{
	Debug.Log ("OnIabQueryInventoryFinished " + result + ", " + inventory);
	if (EvaluateResult(CallbackSpec.OnIabQueryInventoryFinished, result, inventory)) {
		StepTest();
		return;
	}
	Debug.LogWarning("FAILED.");
}

public void OnIabQuerySkuDetailsFinished (IabResult result, Dictionary<string, IabSkuDetails> skuDetailsDictionary)
{
	Debug.Log ("OnIabQuerySkuDetailsFinished " + result);
	if (EvaluateResult(CallbackSpec.OnIabQuerySkuDetailsFinished, result, skuDetailsDictionary)) {
		StepTest();
		return;
	}
	Debug.LogWarning("FAILED.");
}

public void OnIabPurchaseFinished (IabResult result, IabPurchase purchase)
{
	Debug.Log ("OnIabPurchaseFinished " + result + ", " + purchase);
	if (EvaluateResult(CallbackSpec.OnIabPurchaseFinished, result, purchase)) {
		StepTest();
		return;
	}
	Debug.LogWarning("FAILED.");
}

public void OnIabSubscriptionPurchaseFinished (IabResult result, IabPurchase purchase)
{
	Debug.Log ("OnIabSubscriptionPurchaseFinished " + result + ", " + purchase);
	if (EvaluateResult(CallbackSpec.OnIabSubscriptionPurchaseFinished, result, purchase)) {
		StepTest();
		return;
	}
	Debug.LogWarning("FAILED.");
}

public void OnIabConsumeFinished (IabResult result, IabPurchase purchase)
{
	Debug.Log ("OnIabConsumeFinished " + result + ", " + purchase);
	if (EvaluateResult(CallbackSpec.OnIabConsumeFinished, result, purchase)) {
		StepTest();
		return;
	}
	Debug.LogWarning("FAILED.");
}

public void OnInitLicenseFinished ()
{
	Debug.Log ("OnInitLicenseFinished ");
	if (EvaluateResult(CallbackSpec.OnInitLicenseFinished, null, null)) {
		StepTest();
		return;
	}
	Debug.LogWarning("FAILED.");
}

public void OnLicenseCheckFinished (bool isSucceed, bool isLicensed)
{
	Debug.Log ("OnLicenseCheckFinished " + isSucceed + ", " + isLicensed);
	if (EvaluateResult(CallbackSpec.OnLicenseCheckFinished, isSucceed, isLicensed)) {
		StepTest();
		return;
	}
	Debug.LogWarning("FAILED.");
}*/
