using System;
using System.Collections;

// Token: 0x02000177 RID: 375
public class PatternScoreInfo
{
	// Token: 0x06000B7C RID: 2940 RVA: 0x00051B40 File Offset: 0x0004FD40
	public void Init()
	{
		this.BestScore = -1;
		this.NoteScore = -1;
		this.MaxCombo = -1;
		this.RealCombo = -1;
		this.Accuracy = -1;
		this.RankClass = GRADE.NON;
		this.PerfectPlay = false;
		this.AllCombo = false;
		this.strTrophy = string.Empty;
		this.ArrScores.Clear();
	}

	// Token: 0x04000AE8 RID: 2792
	public int PatternIdx = -1;

	// Token: 0x04000AE9 RID: 2793
	public PTLEVEL PTTYPE;

	// Token: 0x04000AEA RID: 2794
	public int BestScore = -1;

	// Token: 0x04000AEB RID: 2795
	public int NoteScore = -1;

	// Token: 0x04000AEC RID: 2796
	public int MaxCombo = -1;

	// Token: 0x04000AED RID: 2797
	public int RealCombo = -1;

	// Token: 0x04000AEE RID: 2798
	public int Accuracy = -1;

	// Token: 0x04000AEF RID: 2799
	public GRADE RankClass = GRADE.NON;

	// Token: 0x04000AF0 RID: 2800
	public bool PerfectPlay;

	// Token: 0x04000AF1 RID: 2801
	public bool AllCombo;

	// Token: 0x04000AF2 RID: 2802
	public string strTrophy = string.Empty;

	// Token: 0x04000AF3 RID: 2803
	public ArrayList ArrScores = new ArrayList();
}
