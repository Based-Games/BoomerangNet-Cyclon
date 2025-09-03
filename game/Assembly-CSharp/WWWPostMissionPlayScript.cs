using System;
using System.Collections;
using System.Text;
using AESWithJavaCS;
using UnityEngine;
using WyrmTale;

// Token: 0x02000153 RID: 339
public class WWWPostMissionPlayScript : WWWObject
{
	// Token: 0x06000AB2 RID: 2738 RVA: 0x0004BEE8 File Offset: 0x0004A0E8
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
		Singleton<GameManager>.instance.RewardState = MISSIONREWARD_STATE.NONE;
		JSON[] array = new JSON[3];
		MissionData mission = Singleton<SongManager>.instance.Mission;
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
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
		for (int i = 0; i < 3; i++)
		{
			JSON json5 = new JSON();
			DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(mission.Song[i]);
			DiscInfo.PtInfo ptInfo = discInfo.DicPtInfo[mission.Pattern[i]];
			json5["musicId"] = discInfo.ServerID;
			json5["patternId"] = ptInfo.iPtServerId;
			json5["stage"] = i + 1;
			json5["cleared"] = true;
			array[i] = json5;
		}
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		json["game"] = json2;
		json["missionId"] = mission.strServerKey;
		json["playMusics"] = array;
		json["cleared"] = true;
		json["conditionCleared"] = this.bCondition;
		json["useItems"] = userData.GetUserUseItem();
		json["effectors"] = null;
		json["machineId"] = GameData.MACHINE_ID;
		string text = AESWithJava.Encrypt(json.serialized, Singleton<WWWManager>.instance.SECRET_KEY);
		string text2 = Singleton<WWWManager>.instance.SERVER_URL + string.Format("mission/user/{0}/histories", Singleton<GameManager>.instance.s_UserID);
		this.www = new WWW(text2, Encoding.UTF8.GetBytes(text), headers);
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x0004C2C8 File Offset: 0x0004A4C8
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
		this.ParsingData(text);
		if (this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x06000AB4 RID: 2740 RVA: 0x0000A5B7 File Offset: 0x000087B7
	private void ParsingData(string strContent)
	{
		Singleton<GameManager>.instance.RewardState = MISSIONREWARD_STATE.ALL_REWARD;
	}

	// Token: 0x04000A6E RID: 2670
	public bool bCondition = true;
}
