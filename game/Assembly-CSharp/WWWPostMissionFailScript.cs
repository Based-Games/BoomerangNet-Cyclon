using System;
using System.Collections;
using System.Text;
using AESWithJavaCS;
using UnityEngine;
using WyrmTale;

// Token: 0x02000152 RID: 338
public class WWWPostMissionFailScript : WWWObject
{
	// Token: 0x06000AAE RID: 2734 RVA: 0x0004BC94 File Offset: 0x00049E94
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
		for (int i = 0; i < 3; i++)
		{
			JSON json2 = new JSON();
			DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(mission.Song[i]);
			DiscInfo.PtInfo ptInfo = discInfo.DicPtInfo[mission.Pattern[i]];
			json2["musicId"] = discInfo.ServerID;
			json2["patternId"] = ptInfo.iPtServerId;
			json2["stage"] = i + 1;
			json2["cleared"] = true;
			array[i] = json2;
		}
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		json["missionId"] = mission.strServerKey;
		json["playMusics"] = array;
		json["cleared"] = false;
		json["conditionCleared"] = false;
		json["useItems"] = userData.GetUserUseItem();
		json["effectors"] = null;
		json["machineId"] = GameData.MACHINE_ID;
		string text = AESWithJava.Encrypt(json.serialized, Singleton<WWWManager>.instance.SECRET_KEY);
		string text2 = Singleton<WWWManager>.instance.SERVER_URL + string.Format("mission/user/{0}/histories", Singleton<GameManager>.instance.s_UserID);
		this.www = new WWW(text2, Encoding.UTF8.GetBytes(text), headers);
	}

	// Token: 0x06000AAF RID: 2735 RVA: 0x0004BE88 File Offset: 0x0004A088
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

	// Token: 0x06000AB0 RID: 2736 RVA: 0x0000A5B7 File Offset: 0x000087B7
	private void ParsingData(string strContent)
	{
		Singleton<GameManager>.instance.RewardState = MISSIONREWARD_STATE.ALL_REWARD;
	}
}
