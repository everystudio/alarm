using UnityEngine;
using System.Collections;
using LitJson_AppMarket;

public class IabSkuDetailsInner {
	public string productId;
	public string type;
	public string price;
	public string title;
	public string description;
}

public class IabSkuDetails {
	public string ItemType;
	public string Sku;
	public string Type;
	public string Price;
	public string Title;
	public string Description;

	public IabSkuDetails() {
	}
	
	public IabSkuDetails(string itemType, JsonData jd) {
		ItemType = itemType;
		Sku = (string)jd["productId"];
		Type = (string)jd["type"];
		Price = (string)jd["price"];
		Title = (string)jd["title"];
		Description = (string)jd["description"];
	}

	public override string ToString ()
	{
		return string.Format ("[IabSkuDetails: ItemType={0}, Sku={1}, Type={2}, Price={3}, Title={4}, Description={5}]", ItemType, Sku, Type, Price, Title, Description);
	}
}
