using UnityEngine;
using System.Collections;
using LitJson_AppMarket;

public class IabResult {
	public int Response;
	public string Message;

	public IabResult() {
	}

	public IabResult(JsonData jd) {
		Response = (int)jd["Response"];
		Message = (string)jd["Message"];
	}
	
	public IabResult(int response, string message) {
		Response = response;
		if (message == null || message.Trim().Length == 0) {
			Message = AppMarketHelper.GetResponseDesc(response);
		}
		else {
			Message = message + " (response: " + AppMarketHelper.GetResponseDesc(response) + ")";
		}
	}
	public bool IsSuccess() { return Response == AppMarketHelper.BILLING_RESPONSE_RESULT_OK; }
	public bool IsFailure() { return !IsSuccess(); }

	public static IabResult createFromJSON(string json) {
		return JsonMapper.ToObject<IabResult>(json);
	}
	
	public string toJSON() {
		return JsonMapper.ToJson(this);
	}

	public override string ToString ()
	{
		return string.Format ("[IabResult: Response={0}, Message={1}]", Response, Message);
	}
}
