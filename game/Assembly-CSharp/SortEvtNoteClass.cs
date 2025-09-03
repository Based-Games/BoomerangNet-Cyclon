using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x020000D9 RID: 217
public class SortEvtNoteClass : IComparer, IComparer<ScoreEventBase>
{
	// Token: 0x06000752 RID: 1874 RVA: 0x00008AE6 File Offset: 0x00006CE6
	public int Compare(ScoreEventBase a, ScoreEventBase b)
	{
		if (a.Tick >= b.Tick)
		{
			return 1;
		}
		return -1;
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x00008AFC File Offset: 0x00006CFC
	public int Compare(object x, object y)
	{
		return this.Compare((ScoreEventBase)x, (ScoreEventBase)y);
	}
}
