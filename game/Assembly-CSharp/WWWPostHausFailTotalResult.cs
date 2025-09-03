using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AESWithJavaCS;
using UnityEngine;
using WyrmTale;

// Token: 0x0200015B RID: 347
public class WWWPostHausFailTotalResult : WWWObject
{
	// Token: 0x06000AD1 RID: 2769 RVA: 0x0004D24C File Offset: 0x0004B44C
	public override void StartLoad()
	{
		Hashtable headers = new WWWForm().headers;
		headers["Content-Type"] = "application/json";
		headers["Version"] = GameData.XYCLON_VERSION;
		headers["Authorization"] = Singleton<GameManager>.instance.authorization;
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
		int num = GameData.Stage + 1;
		JSON json = new JSON();
		HouseStage[] houseSelectSong = Singleton<SongManager>.instance.HouseSelectSong;
		string[] array = new string[num];
		for (int i = 0; i < num; i++)
		{
			HouseStage houseStage = houseSelectSong[i];
			array[i] = houseStage.strHistoryId;
		}
		json["allCleared"] = false;
		json["machineId"] = GameData.MACHINE_ID;
		json["relatedMusicHistoryIds"] = array;
		string text = AESWithJava.Encrypt(json.serialized, Singleton<WWWManager>.instance.SECRET_KEY);
		string text2 = Singleton<WWWManager>.instance.SERVER_URL + string.Format("stages/user/{0}/final/histories", Singleton<GameManager>.instance.s_UserID);
		this.www = new WWW(text2, Encoding.UTF8.GetBytes(text), headers);
	}

	// Token: 0x06000AD2 RID: 2770 RVA: 0x0004D36C File Offset: 0x0004B56C
	public override void CompleteLoad()
	{
		if (this.CheckCancel)
		{
			return;
		}
		if (this.www.error != null)
		{
			foreach (KeyValuePair<string, string> keyValuePair in this.www.responseHeaders)
			{
			}
			return;
		}
	}
}
