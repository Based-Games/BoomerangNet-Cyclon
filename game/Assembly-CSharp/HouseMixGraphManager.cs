using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001BA RID: 442
public class HouseMixGraphManager : MonoBehaviour
{
	// Token: 0x06000D1B RID: 3355 RVA: 0x0005B434 File Offset: 0x00059634
	private void Awake()
	{
		this.m_gPointLine = base.transform.FindChild("PointAndLine").gameObject;
		this.m_arrHouseMixGraph_Level.Clear();
		for (PTLEVEL ptlevel = PTLEVEL.EZ; ptlevel < PTLEVEL.MAX; ptlevel++)
		{
			HouseMixGraph_Level component = base.transform.FindChild(ptlevel.ToString()).GetComponent<HouseMixGraph_Level>();
			this.m_arrHouseMixGraph_Level.Add(ptlevel, component);
		}
	}

	// Token: 0x06000D1C RID: 3356 RVA: 0x0005B4A4 File Offset: 0x000596A4
	public void ViewGraph(PTLEVEL eSelectLv)
	{
		for (PTLEVEL ptlevel = PTLEVEL.EZ; ptlevel < PTLEVEL.MAX; ptlevel++)
		{
			this.m_arrHouseMixGraph_Level[ptlevel].transform.FindChild("View").gameObject.SetActive(false);
			this.m_arrHouseMixGraph_Level[ptlevel].TextSetting(false);
		}
		this.m_arrHouseMixGraph_Level[eSelectLv].transform.FindChild("View").gameObject.SetActive(true);
		this.m_arrHouseMixGraph_Level[eSelectLv].gameObject.SetActive(true);
		this.m_arrHouseMixGraph_Level[eSelectLv].StartAni();
	}

	// Token: 0x06000D1D RID: 3357 RVA: 0x0005B54C File Offset: 0x0005974C
	public void AllHide()
	{
		for (PTLEVEL ptlevel = PTLEVEL.EZ; ptlevel < PTLEVEL.MAX; ptlevel++)
		{
			if (this.m_arrHouseMixGraph_Level.ContainsKey(ptlevel))
			{
				this.m_arrHouseMixGraph_Level[ptlevel].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x0005B598 File Offset: 0x00059798
	public void ShowLevelGraph(PTLEVEL eSelectLv, int score = 0, int Rank = 0)
	{
		this.m_gPointLine.SetActive(true);
		if (score == 0 || score == -1)
		{
			this.m_gPointLine.SetActive(false);
		}
		this.m_arrHouseMixGraph_Level[eSelectLv].gameObject.SetActive(true);
		this.m_arrHouseMixGraph_Level[eSelectLv].m_iScoreValue = score;
		this.m_arrHouseMixGraph_Level[eSelectLv].m_iRankIndex = Rank;
		this.m_arrHouseMixGraph_Level[eSelectLv].TextSetting(false);
	}

	// Token: 0x04000D14 RID: 3348
	private GameObject m_gPointLine;

	// Token: 0x04000D15 RID: 3349
	private Dictionary<PTLEVEL, HouseMixGraph_Level> m_arrHouseMixGraph_Level = new Dictionary<PTLEVEL, HouseMixGraph_Level>();
}
