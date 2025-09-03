using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200017D RID: 381
public class RaveUpResultRank
{
	// Token: 0x06000B89 RID: 2953 RVA: 0x00051FA4 File Offset: 0x000501A4
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

	// Token: 0x04000B0C RID: 2828
	public bool Loading;

	// Token: 0x04000B0D RID: 2829
	public bool OnComplete;

	// Token: 0x04000B0E RID: 2830
	public ArrayList ArrRanking = new ArrayList();
}
