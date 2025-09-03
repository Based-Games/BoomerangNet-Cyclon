using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020001C7 RID: 455
public class ReverseDisc_Number : IComparer, IComparer<HouseCellInfo>
{
	// Token: 0x06000D57 RID: 3415 RVA: 0x0000BDD4 File Offset: 0x00009FD4
	public int Compare(HouseCellInfo x, HouseCellInfo y)
	{
		return y.Number.CompareTo(x.Number);
	}

	// Token: 0x06000D58 RID: 3416 RVA: 0x0000BDFB File Offset: 0x00009FFB
	public int Compare(object x, object y)
	{
		return this.Compare((HouseCellInfo)x, (HouseCellInfo)y);
	}
}
