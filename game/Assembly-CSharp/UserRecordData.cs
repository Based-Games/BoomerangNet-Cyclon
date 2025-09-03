using System;

// Token: 0x0200017C RID: 380
public class UserRecordData
{
	// Token: 0x06000B87 RID: 2951 RVA: 0x0000AAF6 File Offset: 0x00008CF6
	public void Init()
	{
		this.Score = -1;
		this.MaxCombo = -1;
		this.Accuracy = -1;
		this.RankClass = GRADE.NON;
		this.strTrophy = string.Empty;
		this.AllCombo = false;
		this.PerfectPlay = false;
	}

	// Token: 0x04000B03 RID: 2819
	public bool Loading;

	// Token: 0x04000B04 RID: 2820
	public bool OnComplete;

	// Token: 0x04000B05 RID: 2821
	public int Score = -1;

	// Token: 0x04000B06 RID: 2822
	public int MaxCombo = -1;

	// Token: 0x04000B07 RID: 2823
	public int Accuracy = -1;

	// Token: 0x04000B08 RID: 2824
	public GRADE RankClass = GRADE.NON;

	// Token: 0x04000B09 RID: 2825
	public string strTrophy = string.Empty;

	// Token: 0x04000B0A RID: 2826
	public bool AllCombo;

	// Token: 0x04000B0B RID: 2827
	public bool PerfectPlay;
}
