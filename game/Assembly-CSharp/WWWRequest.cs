using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020004AA RID: 1194
public static class WWWRequest
{
	// Token: 0x060015B2 RID: 5554
	public static WWW Create(string endpoint, string body = null)
	{
		string text = Singleton<WWWManager>.instance.SERVER_URL + endpoint;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["Content-Type"] = "application/json";
		dictionary["Version"] = GameData.XYCLON_VERSION;
		dictionary["BNCVersion"] = GameData.XYCLON_VERSION;
		dictionary["Authorization"] = Singleton<GameManager>.instance.authorization;
		dictionary["machineId"] = GameData.MACHINE_ID;
		byte[] array = null;
		if (!string.IsNullOrEmpty(body))
		{
			array = Encoding.UTF8.GetBytes(body);
		}
		Logger.Log("WWWRequest", "{0}", new object[] { text });
		return new WWW(text, array, dictionary);
	}
}
