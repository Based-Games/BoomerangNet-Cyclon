using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020001C6 RID: 454
public class SortDisc_Number : IComparer, IComparer<HouseCellInfo>
{
	// Token: 0x06000D54 RID: 3412 RVA: 0x0000BDD4 File Offset: 0x00009FD4
	public int Compare(HouseCellInfo x, HouseCellInfo y)
	{
		return y.Number.CompareTo(x.Number);
	}

	// Token: 0x06000D55 RID: 3413 RVA: 0x0000BDE7 File Offset: 0x00009FE7
	public int Compare(object x, object y)
	{
		return this.Compare((HouseCellInfo)y, (HouseCellInfo)x);
	}
}
