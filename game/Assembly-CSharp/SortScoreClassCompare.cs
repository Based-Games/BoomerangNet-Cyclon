using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000171 RID: 369
public class SortScoreClassCompare : IComparer, IComparer<RankInfo>
{
	// Token: 0x06000B3E RID: 2878 RVA: 0x0000A87C File Offset: 0x00008A7C
	public int Compare(RankInfo x, RankInfo y)
	{
		return -1 * x.Score.CompareTo(y.Score);
	}

	// Token: 0x06000B3F RID: 2879 RVA: 0x0000A891 File Offset: 0x00008A91
	public int Compare(object x, object y)
	{
		return this.Compare((RankInfo)x, (RankInfo)y);
	}
}
