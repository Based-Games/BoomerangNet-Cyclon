using System;
using System.Collections;

// Token: 0x02000176 RID: 374
public class RankInfo
{
	// Token: 0x17000222 RID: 546
	// (get) Token: 0x06000B79 RID: 2937 RVA: 0x0000AA54 File Offset: 0x00008C54
	// (set) Token: 0x06000B7A RID: 2938 RVA: 0x0000AA5C File Offset: 0x00008C5C
	public int Icon { get; set; }

	// Token: 0x04000ADD RID: 2781
	public string Name = "DJ CYCLON";

	// Token: 0x04000ADE RID: 2782
	public string Nation = "kr";

	// Token: 0x04000ADF RID: 2783
	public string WebIcon = string.Empty;

	// Token: 0x04000AE0 RID: 2784
	public int Ranking = -1;

	// Token: 0x04000AE1 RID: 2785
	public int Score;

	// Token: 0x04000AE2 RID: 2786
	public int Level = 1;

	// Token: 0x04000AE3 RID: 2787
	public GRADE RankClass = GRADE.NON;

	// Token: 0x04000AE4 RID: 2788
	public AlbumInfo aInfo;

	// Token: 0x04000AE5 RID: 2789
	public ArrayList ArrHauseInfo = new ArrayList();

	// Token: 0x04000AE6 RID: 2790
	public ArrayList ArrRaveInfo = new ArrayList();
}
