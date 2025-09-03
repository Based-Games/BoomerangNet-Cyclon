using System;
using System.Collections;
using System.Text;
using AESWithJavaCS;
using UnityEngine;
using WyrmTale;

// Token: 0x0200016A RID: 362
public class WWWPostRaveUp : WWWObject
{
	// Token: 0x06000B0A RID: 2826 RVA: 0x0005036C File Offset: 0x0004E56C
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
		JSON json2 = new JSON();
		JSON json3 = new JSON();
		JSON json4 = new JSON();
		json3["perfect"] = resultData.GetJudgmentCnt(JUDGMENT_TYPE.PERFECT);
		json3["great"] = resultData.GetJudgmentCnt(JUDGMENT_TYPE.GREAT);
		json3["good"] = resultData.GetJudgmentCnt(JUDGMENT_TYPE.GOOD);
		json3["poor"] = resultData.GetJudgmentCnt(JUDGMENT_TYPE.POOR);
		json3["breakk"] = resultData.GetJudgmentCnt(JUDGMENT_TYPE.BREAK);
		json4["trophy"] = resultData.TrophyName;
		json4["allCombo"] = resultData.IsAllComboPlay();
		json4["perfectPlay"] = resultData.IsPerfectPlay();
		json2["accuracy"] = json3;
		json2["emblem"] = json4;
		json2["totalAccuracy"] = resultData.GetAccuracy();
		json2["maxCombo"] = resultData.MAXCOMBO;
		json2["score"] = resultData.SCORE;
		json2["scores"] = null;
		json2["feverBonus"] = resultData.GetFeverBonus();
		json2["extreamBonus"] = resultData.EXTREMEBONUS;
		json2["maxComboBonus"] = resultData.GetMaxComboBonus();
		json2["optainBeatPoint"] = resultData.BEATPOINT;
		json2["optainExp"] = resultData.EXP;
		json2["rankClass"] = resultData.GRADETYPE.ToString();
		json["game"] = json2;
		json["albumId"] = currentAlbum.AlbumServerId;
		JSON[] array = new JSON[4];
		RaveUpStage[] raveUpSelectSong = Singleton<SongManager>.instance.RaveUpSelectSong;
		for (int i = 0; i < 4; i++)
		{
			RaveUpStage raveUpStage = raveUpSelectSong[i];
			DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(raveUpStage.iSong);
			DiscInfo.PtInfo ptInfo = discInfo.DicPtInfo[raveUpStage.PtType];
			JSON json5 = new JSON();
			json5["musicId"] = discInfo.ServerID;
			json5["patternId"] = ptInfo.iPtServerId;
			json5["stage"] = i + 1;
			json5["cleared"] = raveUpStage.bCleard;
			array[i] = json5;
		}
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		json["playMusics"] = array;
		json["cleared"] = true;
		json["useItems"] = userData.GetUserUseItem();
		json["effectors"] = null;
		json["machineId"] = GameData.MACHINE_ID;
		string text = AESWithJava.Encrypt(json.serialized, Singleton<WWWManager>.instance.SECRET_KEY);
		string text2 = Singleton<WWWManager>.instance.SERVER_URL + string.Format("album/user/{0}/histories", Singleton<GameManager>.instance.s_UserID);
		this.www = new WWW(text2, Encoding.UTF8.GetBytes(text), headers);
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x0000A702 File Offset: 0x00008902
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
		string error = this.www.error;
	}
}
