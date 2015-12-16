using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using LitJson_AppMarket;

public class IabPurchase {
	public string ItemType;
	public string OrderId;
	public string PackageName;
	public string Sku;
	public string PurchaseTime;
	public int PurchaseState;
	public string DeveloperPayload;
	public string Token;
	public string OriginalJson;
	public string Signature;

	public IabPurchase() {
	}
	
	public IabPurchase(JsonData jd) {
		foreach (string k in jd.Keys) {
			Debug.Log ("IabPurchase " + k + " : " + jd[k].ToString());
		}

		ItemType = (string)jd["ItemType"];
		OrderId = (string)jd["orderId"];
		PackageName = (string) jd["packageName"];
		Sku = (string) jd["productId"];
		if (jd["purchaseTime"].IsInt) {
			PurchaseTime = ((Int32) jd["purchaseTime"]).ToString();
		} else if (jd["purchaseTime"].IsLong) {
			PurchaseTime = ((Int64) jd["purchaseTime"]).ToString();
		} else if (jd["purchaseTime"].IsString) {
			PurchaseTime = (string) jd["purchaseTime"];
		}
		PurchaseState = (int) jd["purchaseState"];
		DeveloperPayload = (string) jd["developerPayload"];
		if (jd["token"].IsString) {
			Token = (string)jd["token"];
		} else {
			Token = (string)jd["purchaseToken"];
		}
		Signature = (string)jd["Signature"];
		OriginalJson = (string) jd["OriginalJson"];
	}

	public string toJSON() {
		return JsonMapper.ToJson(this);
	}
	public override string ToString ()
	{
		return string.Format ("[IabPurchase: ItemType={0}, OrderId={1}, PackageName={2}, Sku={3}, PurchaseTime={4}, PurchaseState={5}, DeveloperPayload={6}, Token={7}, OriginalJson={8}, Signature={9}]", ItemType, OrderId, PackageName, Sku, PurchaseTime, PurchaseState, DeveloperPayload, Token, OriginalJson, Signature);
	}
	
}
