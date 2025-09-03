using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200017B RID: 379
public class RaveUpRankData
{
	// Token: 0x06000B85 RID: 2949 RVA: 0x00051ED4 File Offset: 0x000500D4
	public void Init()
	{
		this.ArrLocalRank.Clear();
		this.ArrGlobalRank.Clear();
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

	// Token: 0x04000AFF RID: 2815
	public bool Loading;

	// Token: 0x04000B00 RID: 2816
	public bool OnComplete;

	// Token: 0x04000B01 RID: 2817
	public ArrayList ArrLocalRank = new ArrayList();

	// Token: 0x04000B02 RID: 2818
	public ArrayList ArrGlobalRank = new ArrayList();
}
