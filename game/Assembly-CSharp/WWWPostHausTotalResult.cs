using System;
using System.Collections;
using System.Text;
using AESWithJavaCS;
using UnityEngine;
using WyrmTale;

// Token: 0x0200015D RID: 349
public class WWWPostHausTotalResult : WWWObject
{
	// Token: 0x06000AD8 RID: 2776 RVA: 0x0004D8D4 File Offset: 0x0004BAD4
	public override void StartLoad()
	{
		Hashtable headers = new WWWForm().headers;
		headers["Content-Type"] = "application/json";
		headers["Version"] = GameData.XYCLON_VERSION;
		headers["Authorization"] = Singleton<GameManager>.instance.authorization;
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
		JSON json = new JSON();
		HouseStage[] houseSelectSong = Singleton<SongManager>.instance.HouseSelectSong;
		string[] array = new string[3];
		for (int i = 0; i < 3; i++)
		{
			HouseStage houseStage = houseSelectSong[i];
			array[i] = houseStage.strHistoryId;
		}
		json["optainBeatPoint"] = resultData.ALLCLEAR_BEATPOINT;
		json["optainExp"] = resultData.ALLCLEAR_EXP;
		json["rankClass"] = resultData.GRADETYPE.ToString();
		json["allCleared"] = true;
		json["machineId"] = GameData.MACHINE_ID;
		json["relatedMusicHistoryIds"] = array;
		string text = AESWithJava.Encrypt(json.serialized, Singleton<WWWManager>.instance.SECRET_KEY);
		string text2 = Singleton<WWWManager>.instance.SERVER_URL + string.Format("stages/user/{0}/final/histories", Singleton<GameManager>.instance.s_UserID);
		this.www = new WWW(text2, Encoding.UTF8.GetBytes(text), headers);
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x0004DA38 File Offset: 0x0004BC38
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
		this.ParsingData(text);
	}

	// Token: 0x06000ADA RID: 2778 RVA: 0x00003648 File Offset: 0x00001848
	private void ParsingData(string strContent)
	{
	}
}
