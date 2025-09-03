using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x0200015F RID: 351
public class WWWGameCenterPoint : WWWObject
{
	// Token: 0x06000AE0 RID: 2784 RVA: 0x0004E12C File Offset: 0x0004C32C
	public override void StartLoad()
	{
		string text = string.Format("games/gameCenters/points/machine/{0}", GameData.MACHINE_ID);
		this.www = WWWRequest.Create(text, null);
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x0004E158 File Offset: 0x0004C358
	public override void CompleteLoad()
	{
		if (this.CheckCancel)
		{
			return;
		}
		Singleton<GameManager>.instance.POINT = 0;
		if (this.www.error != null)
		{
			if (this.CallBackFail != null)
			{
				this.CallBackFail();
			}
			return;
		}
		string text = this.www.text;
		this.ParsingData(text);
		if (this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x0004E1C0 File Offset: 0x0004C3C0
	private void ParsingData(string strContent)
	{
		if (string.Empty == strContent)
		{
			return;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(strContent) as Dictionary<string, object>;
		if (dictionary.ContainsKey("point"))
		{
			Singleton<GameManager>.instance.POINT = (int)((long)dictionary["point"]);
		}
	}
}
