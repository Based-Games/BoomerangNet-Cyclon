using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x02000150 RID: 336
public class WWWMissionRankScript : WWWObject
{
	// Token: 0x06000AA6 RID: 2726
	public override void StartLoad()
	{
		this.m_sData = Singleton<GameManager>.instance.netRaveUpResultData;
		MissionData mission = Singleton<SongManager>.instance.Mission;
		string text = string.Format("/mission/{0}/nearRankings?score={1}", mission.strServerKey, this.Score);
		this.www = WWWRequest.Create(text, null);
	}

	// Token: 0x06000AA7 RID: 2727
	public override void CompleteLoad()
	{
		if (this.CheckCancel)
		{
			return;
		}
		if (!Singleton<GameManager>.instance.ONLOGIN)
		{
			if (this.CallBackFail != null)
			{
				this.CallBackFail();
			}
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
		this.ParsingMissionRanking(text);
		if (this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x06000AA8 RID: 2728
	private void ParsingMissionRanking(string strContent)
	{
		this.m_sData.Loading = false;
		this.m_sData.OnComplete = true;
		List<object> list = Json.Deserialize(strContent) as List<object>;
		this.m_sData.ArrRanking.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
			RankInfo rankInfo = new RankInfo();
			if (dictionary.ContainsKey("name"))
			{
				rankInfo.Name = (string)dictionary["name"];
			}
			if (dictionary.ContainsKey("nation"))
			{
				rankInfo.Nation = (string)dictionary["nation"];
			}
			if (dictionary.ContainsKey("score"))
			{
				rankInfo.Score = (int)((long)dictionary["score"]);
			}
			if (dictionary.ContainsKey("ranking"))
			{
				rankInfo.Ranking = (int)((long)dictionary["ranking"]);
			}
			if (dictionary.ContainsKey("iconId"))
			{
				rankInfo.Icon = (int)((long)dictionary["iconId"]);
			}
			this.m_sData.ArrRanking.Add(rankInfo);
		}
	}

	// Token: 0x04000A6A RID: 2666
	private RaveUpResultRank m_sData;

	// Token: 0x04000A6B RID: 2667
	public int Score = 1000;
}
