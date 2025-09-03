using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AESWithJavaCS;
using MiniJSON;
using UnityEngine;
using WyrmTale;

// Token: 0x0200015A RID: 346
public class WWWPostHausFailStage : WWWObject
{
	// Token: 0x06000ACD RID: 2765 RVA: 0x0004D024 File Offset: 0x0004B224
	public override void StartLoad()
	{
		if (GameData.AUTO_PLAY)
		{
			return;
		}
		Hashtable headers = new WWWForm().headers;
		headers["Content-Type"] = "application/json";
		headers["Version"] = GameData.XYCLON_VERSION;
		headers["Authorization"] = Singleton<GameManager>.instance.authorization;
		this.m_iStage = GameData.Stage;
		Singleton<GameManager>.instance.GetStageResult(this.m_iStage);
		HouseStage houseStage = Singleton<SongManager>.instance.HouseSelectSong[this.m_iStage];
		DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(houseStage.iSong);
		DiscInfo.PtInfo ptInfo = discInfo.DicPtInfo[houseStage.PtType];
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		JSON json = new JSON();
		json["musicId"] = discInfo.ServerID;
		json["patternId"] = ptInfo.iPtServerId;
		json["stage"] = this.m_iStage + 1;
		json["cleared"] = false;
		json["useItems"] = userData.GetUserUseItem();
		json["effectors"] = null;
		json["machineId"] = GameData.MACHINE_ID;
		string text = AESWithJava.Encrypt(json.serialized, Singleton<WWWManager>.instance.SECRET_KEY);
		string text2 = Singleton<WWWManager>.instance.SERVER_URL + string.Format("stages/user/{0}/music/histories", Singleton<GameManager>.instance.s_UserID);
		this.www = new WWW(text2, Encoding.UTF8.GetBytes(text), headers);
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x0004D1B0 File Offset: 0x0004B3B0
	public override void CompleteLoad()
	{
		if (this.CheckCancel)
		{
			return;
		}
		if (this.www.error != null)
		{
			return;
		}
		string text = this.www.text;
		this.ParsingResult(text);
		if (this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x0004D1FC File Offset: 0x0004B3FC
	private void ParsingResult(string strContent)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(strContent) as Dictionary<string, object>;
		HouseStage houseStage = Singleton<SongManager>.instance.HouseSelectSong[this.m_iStage];
		if (dictionary.ContainsKey("historyId"))
		{
			houseStage.strHistoryId = (string)dictionary["historyId"];
		}
	}

	// Token: 0x04000A7B RID: 2683
	private int m_iStage;
}
