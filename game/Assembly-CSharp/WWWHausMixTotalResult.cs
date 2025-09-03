using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x02000159 RID: 345
public class WWWHausMixTotalResult : WWWObject
{
	// Token: 0x06000AC9 RID: 2761 RVA: 0x0004CE44 File Offset: 0x0004B044
	public override void StartLoad()
	{
		this.m_sData = Singleton<GameManager>.instance.netHouMixTotalResultData;
		this.m_sData.Loading = true;
		this.m_sData.OnComplete = false;
		string text = string.Format("stages/nearRankings?score={0}", this.Score);
		this.www = WWWRequest.Create(text, null);
	}

	// Token: 0x06000ACA RID: 2762 RVA: 0x0004CE9C File Offset: 0x0004B09C
	public override void CompleteLoad()
	{
		if (this.CheckCancel)
		{
			return;
		}
		if (this.www.error != null)
		{
			Logger.Error("WWWHausMix TotalResult", this.www.error, new object[0]);
			this.CallBack();
			return;
		}
		string text = this.www.text;
		this.ParsingTotalNearRanking(text);
		if (this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x06000ACB RID: 2763 RVA: 0x0004CF0C File Offset: 0x0004B10C
	private void ParsingTotalNearRanking(string strContent)
	{
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

	// Token: 0x04000A78 RID: 2680
	private HouseMixTotalResultData m_sData;

	// Token: 0x04000A79 RID: 2681
	public string MusicId = "535e06912c658fc297b5aa12";

	// Token: 0x04000A7A RID: 2682
	public int Score = 1000;
}
