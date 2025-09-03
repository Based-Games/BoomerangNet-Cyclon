using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000178 RID: 376
public class HouseMixSelectData
{
	// Token: 0x06000B7E RID: 2942 RVA: 0x00051B9C File Offset: 0x0004FD9C
	public void RankingInit()
	{
		this.ArrLocalRank.Clear();
		ArrayList allSampleUser = Singleton<SongManager>.instance.AllSampleUser;
		for (int i = 0; i < 100; i++)
		{
			RankInfo rankInfo = new RankInfo();
			rankInfo.Name = (string)allSampleUser[i];
			rankInfo.Ranking = i;
			if (i == 0)
			{
				rankInfo.Score = UnityEngine.Random.Range(3000000, 6000000);
			}
			else
			{
				rankInfo.Score = UnityEngine.Random.Range(((RankInfo)this.ArrLocalRank[i - 1]).Score - 10000, ((RankInfo)this.ArrLocalRank[i - 1]).Score);
			}
			this.ArrLocalRank.Add(rankInfo);
		}
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x00051C60 File Offset: 0x0004FE60
	public void Init()
	{
		this.ArrLocalRank.Clear();
		for (int i = 0; i < 100; i++)
		{
			RankInfo rankInfo = new RankInfo();
			rankInfo.Name = "DJ CYCLON" + i.ToString();
			rankInfo.Ranking = i;
			if (i == 0)
			{
				rankInfo.Score = UnityEngine.Random.Range(3000000, 6000000);
			}
			else
			{
				rankInfo.Score = UnityEngine.Random.Range(((RankInfo)this.ArrLocalRank[i - 1]).Score - 10000, ((RankInfo)this.ArrLocalRank[i - 1]).Score);
			}
			this.ArrLocalRank.Add(rankInfo);
		}
		this.DicPatternInfo.Clear();
		for (PTLEVEL ptlevel = PTLEVEL.EZ; ptlevel < PTLEVEL.MAX; ptlevel++)
		{
			this.DicPatternInfo.Add(ptlevel, new PatternScoreInfo());
		}
	}

	// Token: 0x04000AF4 RID: 2804
	public bool Loading;

	// Token: 0x04000AF5 RID: 2805
	public bool OnComplete;

	// Token: 0x04000AF6 RID: 2806
	public ArrayList ArrLocalRank = new ArrayList();

	// Token: 0x04000AF7 RID: 2807
	public Dictionary<PTLEVEL, PatternScoreInfo> DicPatternInfo = new Dictionary<PTLEVEL, PatternScoreInfo>();

	// Token: 0x04000AF8 RID: 2808
	public DiscInfo dInfo;
}
