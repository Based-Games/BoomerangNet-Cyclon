using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200017A RID: 378
public class HouseMixTotalResultData
{
	// Token: 0x06000B83 RID: 2947 RVA: 0x00051E10 File Offset: 0x00050010
	public void Init()
	{
		this.ArrRanking.Clear();
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
				rankInfo.Score = UnityEngine.Random.Range(((RankInfo)this.ArrRanking[i - 1]).Score - 10000, ((RankInfo)this.ArrRanking[i - 1]).Score);
			}
			this.ArrRanking.Add(rankInfo);
		}
	}

	// Token: 0x04000AFC RID: 2812
	public bool Loading;

	// Token: 0x04000AFD RID: 2813
	public bool OnComplete;

	// Token: 0x04000AFE RID: 2814
	public ArrayList ArrRanking = new ArrayList();
}
