using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x02000167 RID: 359
public class WWWGetRaveUpResultNearRanking : WWWObject
{
	// Token: 0x06000AFF RID: 2815 RVA: 0x0004FCAC File Offset: 0x0004DEAC
	public override void StartLoad()
	{
		this.m_sData = Singleton<GameManager>.instance.netRaveUpResultData;
		string text = string.Format("/album/{0}/nearRankings?score={1}", this.strAlbumId, this.Score);
		this.www = WWWRequest.Create(text, null);
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x0004FCF4 File Offset: 0x0004DEF4
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
		this.ParsingRaveUpResultNearRanking(text);
		if (this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x0004FD54 File Offset: 0x0004DF54
	private void ParsingRaveUpResultNearRanking(string strContent)
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

	// Token: 0x04000A82 RID: 2690
	public string strAlbumId = "535e06912c658fc297b5aa24";

	// Token: 0x04000A83 RID: 2691
	public int Score = 1000;

	// Token: 0x04000A84 RID: 2692
	private RaveUpResultRank m_sData;
}
