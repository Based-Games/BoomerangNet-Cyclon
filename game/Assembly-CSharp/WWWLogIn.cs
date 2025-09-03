using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using WyrmTale;

// Token: 0x02000161 RID: 353
public class WWWLogIn : WWWObject
{
	// Token: 0x06000AE8 RID: 2792 RVA: 0x0004EFCC File Offset: 0x0004D1CC
	public override void StartLoad()
	{
		Hashtable headers = new WWWForm().headers;
		headers["Content-Type"] = "application/json";
		headers["Version"] = GameData.XYCLON_VERSION;
		JSON json = new JSON();
		json["cardId"] = this.CardId;
		json["machineId"] = GameData.MACHINE_ID;
		string serialized = json.serialized;
		string text = "auth/login";
		this.www = WWWRequest.Create(text, serialized);
	}

	// Token: 0x06000AE9 RID: 2793 RVA: 0x0004F044 File Offset: 0x0004D244
	public override void CompleteLoad()
	{
		if (this.CheckCancel)
		{
			return;
		}
		if (this.www.error != null)
		{
			if (this.CallBackFail != null)
			{
				this.CallBackFail();
			}
			return;
		}
		string text = this.www.text;
		if (this.ParsingLogIn(text) && this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x06000AEA RID: 2794 RVA: 0x0004F0A4 File Offset: 0x0004D2A4
	private bool ParsingLogIn(string strContent)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(strContent) as Dictionary<string, object>;
		if (!dictionary.ContainsKey("authorization") || !dictionary.ContainsKey("userId") || !dictionary.ContainsKey("isSuccess"))
		{
			return false;
		}
		Singleton<GameManager>.instance.cardSuccess = (bool)dictionary["isSuccess"];
		Singleton<GameManager>.instance.authorization = (string)dictionary["authorization"];
		Singleton<GameManager>.instance.s_UserID = (string)dictionary["userId"];
		return true;
	}

	// Token: 0x04000A7E RID: 2686
	public string CardId = "JYDTNYGX96MOM1CH";
}
