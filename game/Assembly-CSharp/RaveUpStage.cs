using System;

// Token: 0x02000142 RID: 322
public class RaveUpStage
{
	// Token: 0x170001FC RID: 508
	// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0004A934 File Offset: 0x00048B34
	public int PtDif
	{
		get
		{
			DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(this.iSong);
			DiscInfo.PtInfo ptInfo = discInfo.DicPtInfo[this.PtType];
			return ptInfo.iDif;
		}
	}

	// Token: 0x040009CC RID: 2508
	public int Id;

	// Token: 0x040009CD RID: 2509
	public int iAlbum;

	// Token: 0x040009CE RID: 2510
	public int iSong;

	// Token: 0x040009CF RID: 2511
	public PTLEVEL PtType;

	// Token: 0x040009D0 RID: 2512
	public int iDif;

	// Token: 0x040009D1 RID: 2513
	public bool bCleard;
}
