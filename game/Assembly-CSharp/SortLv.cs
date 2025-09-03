using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000149 RID: 329
public class SortLv : IComparer, IComparer<LevelData>
{
	// Token: 0x06000A74 RID: 2676 RVA: 0x0000A39B File Offset: 0x0000859B
	public int Compare(LevelData x, LevelData y)
	{
		return y.Level.CompareTo(x.Level);
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x0000A3AE File Offset: 0x000085AE
	public int Compare(object x, object y)
	{
		return this.Compare((LevelData)y, (LevelData)x);
	}
}
