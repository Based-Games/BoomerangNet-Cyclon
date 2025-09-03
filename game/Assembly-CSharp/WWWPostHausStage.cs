using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AESWithJavaCS;
using MiniJSON;
using UnityEngine;
using WyrmTale;

// Token: 0x0200015C RID: 348
public class WWWPostHausStage : WWWObject
{
	// Token: 0x06000AD4 RID: 2772 RVA: 0x0004D3D4 File Offset: 0x0004B5D4
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
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		RESULTDATA stageResult = Singleton<GameManager>.instance.GetStageResult(this.m_iStage);
		HouseStage houseStage = Singleton<SongManager>.instance.HouseSelectSong[this.m_iStage];
		DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(houseStage.iSong);
		DiscInfo.PtInfo ptInfo = discInfo.DicPtInfo[houseStage.PtType];
		JSON json = new JSON();
		JSON json2 = new JSON();
		JSON json3 = new JSON();
		JSON json4 = new JSON();
		json3["perfect"] = stageResult.GetJudgmentCnt(JUDGMENT_TYPE.PERFECT);
		json3["great"] = stageResult.GetJudgmentCnt(JUDGMENT_TYPE.GREAT);
		json3["good"] = stageResult.GetJudgmentCnt(JUDGMENT_TYPE.GOOD);
		json3["poor"] = stageResult.GetJudgmentCnt(JUDGMENT_TYPE.POOR);
		json3["breakk"] = stageResult.GetJudgmentCnt(JUDGMENT_TYPE.BREAK);
		json4["trophy"] = stageResult.TrophyName;
		json4["allCombo"] = stageResult.IsAllComboPlay();
		json4["perfectPlay"] = stageResult.IsPerfectPlay();
		json2["accuracy"] = json3;
		json2["emblem"] = json4;
		json2["totalAccuracy"] = stageResult.GetAccuracy();
		json2["maxCombo"] = stageResult.MAXCOMBO;
		json2["realCombo"] = stageResult.TOTAL_NOBONUSCOMBOCOUNT;
		json2["score"] = stageResult.SCORE;
		json2["noteScore"] = stageResult.NOTESCORE;
		json2["feverBonus"] = stageResult.GetFeverBonus();
		json2["extreamBonus"] = stageResult.EXTREMEBONUS;
		json2["maxComboBonus"] = stageResult.GetMaxComboBonus();
		json2["optainBeatPoint"] = stageResult.BEATPOINT;
		json2["optainExp"] = stageResult.EXP;
		json2["rankClass"] = stageResult.GRADETYPE.ToString();
		this.PostUseItem = userData.GetUserUseItem();
		json["game"] = json2;
		json["musicId"] = discInfo.ServerID;
		json["patternId"] = ptInfo.iPtServerId;
		json["stage"] = this.m_iStage + 1;
		json["cleared"] = true;
		json["useItems"] = this.PostUseItem;
		json["effectors"] = null;
		json["machineId"] = GameData.MACHINE_ID;
		json["lastSong"] = houseStage.Id;
		string text = AESWithJava.Encrypt(json.serialized, Singleton<WWWManager>.instance.SECRET_KEY);
		string text2 = Singleton<WWWManager>.instance.SERVER_URL + string.Format("stages/user/{0}/music/histories", Singleton<GameManager>.instance.s_UserID);
		this.www = new WWW(text2, Encoding.UTF8.GetBytes(text), headers);
	}

	// Token: 0x06000AD5 RID: 2773 RVA: 0x0004D780 File Offset: 0x0004B980
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
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		if (this.PostUseItem != null)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < userData.ArrItem.Count; i++)
			{
				USERITEM useritem = (USERITEM)userData.ArrItem[i];
				for (int j = 0; j < this.PostUseItem.Length; j++)
				{
					if (this.PostUseItem[j] == useritem.iPublishedItemNo)
					{
						arrayList.Add(useritem);
					}
				}
			}
			for (int k = 0; k < arrayList.Count; k++)
			{
				USERITEM useritem2 = (USERITEM)arrayList[k];
				if (userData.ArrItem.Contains(useritem2))
				{
					userData.ArrItem.Remove(useritem2);
				}
			}
		}
		this.ParsingResult(text);
		if (this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x06000AD6 RID: 2774 RVA: 0x0004D884 File Offset: 0x0004BA84
	private void ParsingResult(string strContent)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(strContent) as Dictionary<string, object>;
		HouseStage houseStage = Singleton<SongManager>.instance.HouseSelectSong[this.m_iStage];
		if (dictionary.ContainsKey("historyId"))
		{
			houseStage.strHistoryId = (string)dictionary["historyId"];
		}
	}

	// Token: 0x04000A7C RID: 2684
	private int m_iStage;

	// Token: 0x04000A7D RID: 2685
	private int[] PostUseItem;
}
