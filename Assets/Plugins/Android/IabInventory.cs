using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson_AppMarket;

public class IabInventory {
	Dictionary<string, IabSkuDetails> mSkuMap = new Dictionary<string, IabSkuDetails>();
	Dictionary<string, IabPurchase> mPurchaseMap = new Dictionary<string, IabPurchase>();
	
	public IabInventory() { }

	public IabInventory(JsonData jd) {
		JsonData skuMap = jd["SkuMap"];
		JsonData purchaseMap = jd["PurchaseMap"];

		foreach (string k in skuMap.Keys) {
			Debug.Log ("mSkuMap " + k);
			mSkuMap.Add(k, new IabSkuDetails((string)skuMap[k]["ItemType"], skuMap[k]));
		}
		foreach (string k in purchaseMap.Keys) {
			Debug.Log ("mPurchaseMap " + k);
			mPurchaseMap.Add(k, new IabPurchase(purchaseMap[k]));
		}
	}
	
	/** Returns the listing details for an in-app product. */
	public IabSkuDetails getSkuDetails(string sku) {
		if (mSkuMap.ContainsKey(sku)) {
			return mSkuMap[sku];
		}
		return null;
	}
	
	/** Returns purchase information for a given product, or null if there is no purchase. */
	public IabPurchase getPurchase(string sku) {
		if (mPurchaseMap.ContainsKey(sku)) {
			return mPurchaseMap[sku];
		}
		return null;
	}
	
	/** Returns whether or not there exists a purchase of the given product. */
	public bool hasPurchase(string sku) {
		return mPurchaseMap.ContainsKey(sku);
	}
	
	/** Return whether or not details about the given product are available. */
	public bool hasDetails(string sku) {
		return mSkuMap.ContainsKey(sku);
	}
	
	/**
     * Erase a purchase (locally) from the inventory, given its product ID. This just
     * modifies the Inventory object locally and has no effect on the server! This is
     * useful when you have an existing Inventory object which you know to be up to date,
     * and you have just consumed an item successfully, which means that erasing its
     * purchase data from the Inventory you already have is quicker than querying for
     * a new Inventory.
     */
	public void erasePurchase(string sku) {
		if (mPurchaseMap.ContainsKey(sku)) mPurchaseMap.Remove(sku);
	}
	
	/** Returns a list of all owned product IDs. */
	public List<string> getAllOwnedSkus() {
		return new List<string>(mPurchaseMap.Keys);
	}
	
	/** Returns a list of all owned product IDs of a given type */
	public List<string> getAllOwnedSkus(string itemType) {
		List<string> result = new List<string>();
		foreach (IabPurchase p in mPurchaseMap.Values) {
			if (p.ItemType == itemType) result.Add(p.Sku);
		}
		return result;
	}
	
	/** Returns a list of all purchases. */
	public List<IabPurchase> getAllPurchases() {
		return new List<IabPurchase>(mPurchaseMap.Values);
	}
	
	void addSkuDetails(IabSkuDetails d) {
		mSkuMap[d.Sku] = d;
	}
	
	void addPurchase(IabPurchase p) {
		mPurchaseMap[p.Sku] = p;
	}

	public string toJSON() {
		return JsonMapper.ToJson(this);
	}

	public override string ToString ()
	{
		string ret = "IabInventory: { skumap: { ";
		foreach (string k in mSkuMap.Keys) {
			ret += k + ": " + mSkuMap[k] + ", ";
		}
		ret += " } purchaseMap: { ";
		foreach (string k in mPurchaseMap.Keys) {
			ret += k + ": " + mPurchaseMap[k] + ", ";
		}
		ret += " }";
		return ret;
	}
	
}
