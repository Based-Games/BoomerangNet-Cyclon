using System;
using System.Collections.Generic;

// Token: 0x0200013F RID: 319
public class DiscInfo
{
	// Token: 0x06000A5A RID: 2650 RVA: 0x0004A734 File Offset: 0x00048934
	public DiscInfo()
	{
		this.Id = 0;
		this.Name = "";
		this.FullName = "";
		this.Artist = "";
		this.Genre = "";
		this.Difficult = 0;
		this.Composer = "";
		this.EnName = "";
		this.GroupSet = 1;
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x06000A5B RID: 2651 RVA: 0x0000A300 File Offset: 0x00008500
	// (set) Token: 0x06000A5C RID: 2652 RVA: 0x0000A308 File Offset: 0x00008508
	public string FullName
	{
		get
		{
			return this.m_strName;
		}
		set
		{
			this.m_strName = value;
		}
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x0004A7B4 File Offset: 0x000489B4
	public DiscInfo.PtInfo GetFirstPtInfo()
	{
		for (PTLEVEL ptlevel = PTLEVEL.EZ; ptlevel < PTLEVEL.MAX; ptlevel++)
		{
			if (this.DicPtInfo.ContainsKey(ptlevel))
			{
				return this.DicPtInfo[ptlevel];
			}
		}
		return null;
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x0000A311 File Offset: 0x00008511
	public bool ContainPt(PTLEVEL pType)
	{
		return this.DicPtInfo.ContainsKey(pType);
	}

	// Token: 0x040009B4 RID: 2484
	public int Id;

	// Token: 0x040009B5 RID: 2485
	public string Name;

	// Token: 0x040009B6 RID: 2486
	public string EnName;

	// Token: 0x040009B7 RID: 2487
	public string InGameName;

	// Token: 0x040009B8 RID: 2488
	public string Artist;

	// Token: 0x040009B9 RID: 2489
	public string Genre;

	// Token: 0x040009BA RID: 2490
	public int Difficult;

	// Token: 0x040009BB RID: 2491
	public string Composer;

	// Token: 0x040009BC RID: 2492
	public int Bpm;

	// Token: 0x040009BD RID: 2493
	public string ServerID;

	// Token: 0x040009BE RID: 2494
	public Dictionary<PTLEVEL, DiscInfo.PtInfo> DicPtInfo = new Dictionary<PTLEVEL, DiscInfo.PtInfo>();

	// Token: 0x040009BF RID: 2495
	public int GroupSet;

	// Token: 0x040009C0 RID: 2496
	public string m_strName = string.Empty;

	// Token: 0x02000140 RID: 320
	public class PtInfo
	{
		// Token: 0x040009C1 RID: 2497
		public int iDif = 1;

		// Token: 0x040009C2 RID: 2498
		public int iPtServerId = 1;

		// Token: 0x040009C3 RID: 2499
		public int iMaxNote = 1;

		// Token: 0x040009C4 RID: 2500
		public int iMaxCombo = 1;

		// Token: 0x040009C5 RID: 2501
		public PTLEVEL PTTYPE = PTLEVEL.EZ;
	}
}
