using System;
using WyrmTale;

// Token: 0x02000162 RID: 354
public class WWWNoCardLogIn : WWWObject
{
	// Token: 0x06000AEC RID: 2796
	public override void StartLoad()
	{
		JSON json = new JSON();
		json["machineId"] = GameData.MACHINE_ID;
		string serialized = json.serialized;
		string text = "auth/noCardLogin";
		this.www = WWWRequest.Create(text, serialized);
	}

	// Token: 0x06000AED RID: 2797
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
		this.ParsingLogIn(text);
		if (this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x06000AEE RID: 2798
	private void ParsingLogIn(string strContent)
	{
		JSON json = new JSON();
		json.serialized = strContent;
		Singleton<GameManager>.instance.authorization = json.ToString("authorization");
		Singleton<GameManager>.instance.s_UserID = json.ToString("userId");
	}

	// Token: 0x04000A7F RID: 2687
	public string CardId = "JYDTNYGX96MOM1CH";
}
