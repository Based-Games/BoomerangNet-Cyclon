using System;
using System.Collections;
using System.Text;
using AESWithJavaCS;
using UnityEngine;
using WyrmTale;

// Token: 0x02000169 RID: 361
public class WWWPostFailRaveUp : WWWObject
{
	// Token: 0x06000B07 RID: 2823 RVA: 0x0005012C File Offset: 0x0004E32C
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
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
		AlbumInfo currentAlbum = Singleton<SongManager>.instance.GetCurrentAlbum();
		JSON json = new JSON();
		JSON[] array = new JSON[4];
		RaveUpStage[] raveUpSelectSong = Singleton<SongManager>.instance.RaveUpSelectSong;
		for (int i = 0; i < 4; i++)
		{
			RaveUpStage raveUpStage = raveUpSelectSong[i];
			DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(raveUpStage.iSong);
			DiscInfo.PtInfo ptInfo = discInfo.DicPtInfo[raveUpStage.PtType];
			JSON json2 = new JSON();
			json2["musicId"] = discInfo.ServerID;
			json2["patternId"] = ptInfo.iPtServerId;
			json2["stage"] = i + 1;
			json2["cleared"] = raveUpStage.bCleard;
			array[i] = json2;
		}
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		json["albumId"] = currentAlbum.AlbumServerId;
		json["playMusics"] = array;
		json["cleared"] = false;
		json["useItems"] = userData.GetUserUseItem();
		json["effectors"] = null;
		json["machineId"] = GameData.MACHINE_ID;
		string text = AESWithJava.Encrypt(json.serialized, Singleton<WWWManager>.instance.SECRET_KEY);
		string text2 = Singleton<WWWManager>.instance.SERVER_URL + string.Format("album/user/{0}/histories", Singleton<GameManager>.instance.s_UserID);
		this.www = new WWW(text2, Encoding.UTF8.GetBytes(text), headers);
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x00050318 File Offset: 0x0004E518
	public override void CompleteLoad()
	{
		if (this.CheckCancel)
		{
			return;
		}
		if (this.CallBackFail != null)
		{
			this.CallBackFail();
		}
		if (this.www.error != null)
		{
			Logger.Error("WWWPostFailRaveUp", this.www.error, new object[0]);
			return;
		}
	}
}
