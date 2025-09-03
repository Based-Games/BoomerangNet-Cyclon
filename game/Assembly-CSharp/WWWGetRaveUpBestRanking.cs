using System;
using System.Collections.Generic;
using MiniJSON;

// Token: 0x02000166 RID: 358
public class WWWGetRaveUpBestRanking : WWWObject
{
	// Token: 0x06000AFB RID: 2811 RVA: 0x0004FAA4 File Offset: 0x0004DCA4
	public override void StartLoad()
	{
		this.m_sData = Singleton<GameManager>.instance.netRaveUpRankData;
		this.m_sData.Init();
		string text = string.Format("album/{0}/bestRankings", this.strAlbumId);
		this.www = WWWRequest.Create(text, null);
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x0004FAEC File Offset: 0x0004DCEC
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
		this.ParsingRaveUpBestRanking(text);
		if (this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x0004FB4C File Offset: 0x0004DD4C
	private void ParsingRaveUpBestRanking(string strContent)
	{
		this.m_sData.Loading = false;
		this.m_sData.OnComplete = true;
		Dictionary<string, object> dictionary = Json.Deserialize(strContent) as Dictionary<string, object>;
		if (dictionary.ContainsKey("weeklyTop"))
		{
			this.m_sData.ArrLocalRank.Clear();
			List<object> list = (List<object>)dictionary["weeklyTop"];
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					Dictionary<string, object> dictionary2 = list[i] as Dictionary<string, object>;
					RankInfo rankInfo = new RankInfo();
					if (dictionary2.ContainsKey("name"))
					{
						rankInfo.Name = (string)dictionary2["name"];
					}
					if (dictionary2.ContainsKey("nation"))
					{
						rankInfo.Nation = (string)dictionary2["nation"];
					}
					if (dictionary2.ContainsKey("score"))
					{
						rankInfo.Score = (int)((long)dictionary2["score"]);
					}
					if (dictionary2.ContainsKey("ranking"))
					{
						rankInfo.Ranking = (int)((long)dictionary2["ranking"]);
					}
					if (dictionary2.ContainsKey("iconId"))
					{
						rankInfo.Icon = (int)((long)dictionary2["iconId"]);
					}
					this.m_sData.ArrLocalRank.Add(rankInfo);
				}
			}
		}
	}

	// Token: 0x04000A80 RID: 2688
	public string strAlbumId = "535e06912c658fc297b5aa24";

	// Token: 0x04000A81 RID: 2689
	private RaveUpRankData m_sData;
}
