using System;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

// Token: 0x02000158 RID: 344
public class WWWHausMixResult : WWWObject
{
	// Token: 0x06000AC4 RID: 2756 RVA: 0x0004CC60 File Offset: 0x0004AE60
	public override void StartLoad()
	{
		this.m_sData = Singleton<GameManager>.instance.netHouMixResultData;
		this.m_sData.Loading = true;
		this.m_sData.OnComplete = false;
		string text = string.Format("music/{0}/nearRankings?score={1}", this.MusicId, this.Score);
		this.www = WWWRequest.Create(text, null);
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x0000A636 File Offset: 0x00008836
	private void Update()
	{
		if (!this.CheckCancel)
		{
			this.m_fTime += Time.deltaTime;
			if (this.MAX_TIME < this.m_fTime)
			{
				this.CheckCancel = true;
			}
		}
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x0004CCC0 File Offset: 0x0004AEC0
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
			return;
		}
		string text = this.www.text;
		this.ParsingNearRanking(text);
		if (this.CallBack != null)
		{
			this.CallBack();
		}
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x0004CD2C File Offset: 0x0004AF2C
	private void ParsingNearRanking(string strContent)
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

	// Token: 0x04000A73 RID: 2675
	private HouseMixResultData m_sData;

	// Token: 0x04000A74 RID: 2676
	public string MusicId = "535e06912c658fc297b5aa12";

	// Token: 0x04000A75 RID: 2677
	public int Score = 1000;

	// Token: 0x04000A76 RID: 2678
	private float m_fTime;

	// Token: 0x04000A77 RID: 2679
	private float MAX_TIME = 3f;
}
