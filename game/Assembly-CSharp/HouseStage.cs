using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000141 RID: 321
public class HouseStage
{
	// Token: 0x06000A60 RID: 2656 RVA: 0x0004A7EC File Offset: 0x000489EC
	public HouseStage()
	{
		this.Id = 101;
		this.iStage = 1;
		this.PtType = PTLEVEL.EZ;
		this.iSong = 1;
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0004A86C File Offset: 0x00048A6C
	public int PtDif
	{
		get
		{
			DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(this.iSong);
			DiscInfo.PtInfo ptInfo = discInfo.DicPtInfo[this.PtType];
			return ptInfo.iDif;
		}
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x0004A8A4 File Offset: 0x00048AA4
	public PTLEVEL GetFirstLv()
	{
		for (PTLEVEL ptlevel = PTLEVEL.EZ; ptlevel < PTLEVEL.MAX; ptlevel++)
		{
			if (this.DicSelectPt[ptlevel])
			{
				return ptlevel;
			}
		}
		return PTLEVEL.EZ;
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x0004A8D8 File Offset: 0x00048AD8
	public PTLEVEL GetRandPt()
	{
		ArrayList arrayList = new ArrayList();
		for (PTLEVEL ptlevel = PTLEVEL.EZ; ptlevel < PTLEVEL.MAX; ptlevel++)
		{
			if (this.DicSelectPt[ptlevel])
			{
				arrayList.Add(ptlevel);
			}
		}
		int num = UnityEngine.Random.Range(0, arrayList.Count);
		return (PTLEVEL)((int)arrayList[num]);
	}

	// Token: 0x040009C6 RID: 2502
	public int Id;

	// Token: 0x040009C7 RID: 2503
	public int iStage;

	// Token: 0x040009C8 RID: 2504
	public int iSong;

	// Token: 0x040009C9 RID: 2505
	public PTLEVEL PtType;

	// Token: 0x040009CA RID: 2506
	public string strHistoryId = string.Empty;

	// Token: 0x040009CB RID: 2507
	public Dictionary<PTLEVEL, bool> DicSelectPt = new Dictionary<PTLEVEL, bool>
	{
		{
			PTLEVEL.EZ,
			false
		},
		{
			PTLEVEL.NM,
			false
		},
		{
			PTLEVEL.HD,
			false
		},
		{
			PTLEVEL.PR,
			false
		},
		{
			PTLEVEL.MX,
			false
		},
		{
			PTLEVEL.S1,
			false
		},
		{
			PTLEVEL.S2,
			false
		}
	};
}
