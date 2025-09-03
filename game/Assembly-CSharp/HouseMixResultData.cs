using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000179 RID: 377
public class HouseMixResultData
{
	// Token: 0x06000B81 RID: 2945 RVA: 0x00051D4C File Offset: 0x0004FF4C
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

	// Token: 0x04000AF9 RID: 2809
	public bool Loading;

	// Token: 0x04000AFA RID: 2810
	public bool OnComplete;

	// Token: 0x04000AFB RID: 2811
	public ArrayList ArrRanking = new ArrayList();
}
