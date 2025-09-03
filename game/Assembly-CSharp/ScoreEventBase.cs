using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000D8 RID: 216
public class ScoreEventBase
{
	// Token: 0x17000173 RID: 371
	// (get) Token: 0x0600071D RID: 1821 RVA: 0x00008900 File Offset: 0x00006B00
	// (set) Token: 0x0600071E RID: 1822 RVA: 0x00008908 File Offset: 0x00006B08
	public bool MULTILINE { get; set; }

	// Token: 0x17000174 RID: 372
	// (get) Token: 0x0600071F RID: 1823 RVA: 0x00008911 File Offset: 0x00006B11
	// (set) Token: 0x06000720 RID: 1824 RVA: 0x00008919 File Offset: 0x00006B19
	public bool DRAW_DOUBLLINE { get; set; }

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x06000721 RID: 1825 RVA: 0x00008922 File Offset: 0x00006B22
	// (set) Token: 0x06000722 RID: 1826 RVA: 0x0000892A File Offset: 0x00006B2A
	public bool DRAW_MULTILINE { get; set; }

	// Token: 0x17000176 RID: 374
	// (get) Token: 0x06000723 RID: 1827 RVA: 0x00008933 File Offset: 0x00006B33
	// (set) Token: 0x06000724 RID: 1828 RVA: 0x0000893B File Offset: 0x00006B3B
	public bool NOT_SOUND { get; set; }

	// Token: 0x17000177 RID: 375
	// (get) Token: 0x06000725 RID: 1829 RVA: 0x00008944 File Offset: 0x00006B44
	// (set) Token: 0x06000726 RID: 1830 RVA: 0x0000894C File Offset: 0x00006B4C
	public JUDGMENT_TYPE JudgmentStart { get; set; }

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x06000727 RID: 1831 RVA: 0x00008955 File Offset: 0x00006B55
	// (set) Token: 0x06000728 RID: 1832 RVA: 0x0000895D File Offset: 0x00006B5D
	public JUDGMENT_TYPE JudgmentEnd { get; set; }

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x06000729 RID: 1833 RVA: 0x00008966 File Offset: 0x00006B66
	// (set) Token: 0x0600072A RID: 1834 RVA: 0x0000896E File Offset: 0x00006B6E
	public bool SameCheck { get; set; }

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x0600072B RID: 1835 RVA: 0x00008977 File Offset: 0x00006B77
	// (set) Token: 0x0600072C RID: 1836 RVA: 0x0000897F File Offset: 0x00006B7F
	public bool AutoCheck { get; set; }

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x0600072D RID: 1837 RVA: 0x00008988 File Offset: 0x00006B88
	// (set) Token: 0x0600072E RID: 1838 RVA: 0x00008990 File Offset: 0x00006B90
	public bool OnMark { get; set; }

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x0600072F RID: 1839 RVA: 0x00008999 File Offset: 0x00006B99
	// (set) Token: 0x06000730 RID: 1840 RVA: 0x000089A1 File Offset: 0x00006BA1
	public bool OnUpMark { get; set; }

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06000731 RID: 1841 RVA: 0x000089AA File Offset: 0x00006BAA
	// (set) Token: 0x06000732 RID: 1842 RVA: 0x000089B2 File Offset: 0x00006BB2
	public GameObject MarkNote { get; set; }

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06000733 RID: 1843 RVA: 0x000089BB File Offset: 0x00006BBB
	// (set) Token: 0x06000734 RID: 1844 RVA: 0x000089C3 File Offset: 0x00006BC3
	public float DurRate { get; set; }

	// Token: 0x06000735 RID: 1845 RVA: 0x00037370 File Offset: 0x00035570
	public void Init()
	{
		this.tick = 0;
		this.dur = 0;
		this.EndView = false;
		this.MULTILINE = false;
		this.SameCheck = false;
		this.OnMark = false;
		this.OnUpMark = false;
		this.DRAW_DOUBLLINE = false;
		this.DRAW_MULTILINE = false;
		this.NOT_SOUND = false;
		this.m_arrSameTime.Clear();
		this.m_iNextIdx = 0;
		this.JudgmentStart = JUDGMENT_TYPE.JUDGMENT_NONE;
		this.JudgmentEnd = JUDGMENT_TYPE.JUDGMENT_NONE;
		this.DurRate = 1f;
		this.bLongFail = false;
		this.iTickFail = 0;
		this.iComboCount = 0;
		this.PreDifValue = 0f;
		this.MoveDifValue = 1f;
		this.objControlNote = null;
		this.RenderFever = null;
		this.AutoCheck = false;
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x00037430 File Offset: 0x00035630
	public void SetTrack(int iTrack, bool bNotMove = false)
	{
		int track = this.Track;
		int num = iTrack - track;
		this.Track = iTrack;
		if (bNotMove)
		{
			return;
		}
		if (this.IsMoveNote())
		{
			foreach (object obj in this.m_arrMoveInfo)
			{
				ScoreEventBase scoreEventBase = (ScoreEventBase)obj;
				int num2 = scoreEventBase.Track;
				num2 += num;
				if (12 <= num2)
				{
					num2 -= 12;
				}
				if (0 > num2)
				{
					num2 += 12;
				}
				scoreEventBase.Track = num2;
			}
		}
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x000374E8 File Offset: 0x000356E8
	public void Clear()
	{
		SpawnPool spawnPool = PoolManager.Pools["NotePool"];
		this.SameCheck = false;
		if (null != this.objControlNote)
		{
			this.objControlNote.SendMessage("ClearLine");
			this.objControlNote.SendMessage("ClearRound");
			if (spawnPool.IsSpawned(this.objControlNote.transform))
			{
				spawnPool.Despawn(this.objControlNote.transform);
				this.SameCheck = false;
				this.iComboCount = 0;
				this.objControlNote = null;
			}
		}
		if (null != this.MarkNote && spawnPool.IsSpawned(this.MarkNote.transform))
		{
			spawnPool.Despawn(this.MarkNote.transform);
			this.OnMark = false;
			this.OnUpMark = false;
			foreach (object obj in this.m_arrMoveInfo)
			{
				ScoreEventBase scoreEventBase = (ScoreEventBase)obj;
				if (spawnPool.IsSpawned(scoreEventBase.MarkNote.transform))
				{
					spawnPool.Despawn(scoreEventBase.MarkNote.transform);
				}
			}
		}
	}

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x06000738 RID: 1848 RVA: 0x000089CC File Offset: 0x00006BCC
	public ArrayList MoveInfo
	{
		get
		{
			return this.m_arrMoveInfo;
		}
	}

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x06000739 RID: 1849 RVA: 0x000089D4 File Offset: 0x00006BD4
	public ArrayList SameInfo
	{
		get
		{
			return this.m_arrSameTime;
		}
	}

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x0600073A RID: 1850 RVA: 0x00037638 File Offset: 0x00035838
	// (set) Token: 0x0600073B RID: 1851 RVA: 0x000089DC File Offset: 0x00006BDC
	public int Tick
	{
		get
		{
			int num = this.tick;
			if (this.bLongFail)
			{
				num = this.iTickFail;
			}
			return num;
		}
		set
		{
			this.tick = value;
		}
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x0600073C RID: 1852 RVA: 0x00037660 File Offset: 0x00035860
	// (set) Token: 0x0600073D RID: 1853 RVA: 0x000089E5 File Offset: 0x00006BE5
	public int Duration
	{
		get
		{
			int num = this.dur;
			if (this.bLongFail)
			{
				num = this.dur - (this.iTickFail - this.Tick);
			}
			return num;
		}
		set
		{
			this.dur = value;
		}
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x0600073E RID: 1854 RVA: 0x000089EE File Offset: 0x00006BEE
	// (set) Token: 0x0600073F RID: 1855 RVA: 0x000089F6 File Offset: 0x00006BF6
	public int Track
	{
		get
		{
			return this.m_iTrack;
		}
		set
		{
			this.m_iTrack = value;
		}
	}

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x06000740 RID: 1856 RVA: 0x000089FF File Offset: 0x00006BFF
	// (set) Token: 0x06000741 RID: 1857 RVA: 0x00008A07 File Offset: 0x00006C07
	public int Attr
	{
		get
		{
			return this.attr;
		}
		set
		{
			this.attr = value;
		}
	}

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x06000742 RID: 1858 RVA: 0x00008A10 File Offset: 0x00006C10
	// (set) Token: 0x06000743 RID: 1859 RVA: 0x00008A18 File Offset: 0x00006C18
	public bool CanRender
	{
		get
		{
			return this.m_bCanRender;
		}
		set
		{
			this.m_bCanRender = value;
		}
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x06000744 RID: 1860 RVA: 0x00008A21 File Offset: 0x00006C21
	// (set) Token: 0x06000745 RID: 1861 RVA: 0x00008A29 File Offset: 0x00006C29
	public bool EndView
	{
		get
		{
			return this.m_bEndView;
		}
		set
		{
			this.m_bEndView = value;
		}
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x00008A32 File Offset: 0x00006C32
	public void SetJudgment(JUDGMENT_TYPE eType)
	{
		this.JudgmentStart = eType;
		this.JudgmentEnd = eType;
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x00008A42 File Offset: 0x00006C42
	public bool IsNoneState()
	{
		return JUDGMENT_TYPE.JUDGMENT_NONE == this.JudgmentEnd;
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x00008A4D File Offset: 0x00006C4D
	public int GetEndTick()
	{
		return this.tick + this.Duration;
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x00008A5C File Offset: 0x00006C5C
	public bool IsLongNote()
	{
		return this.Duration > 0;
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x00008A71 File Offset: 0x00006C71
	public bool IsMoveNote()
	{
		return 1 <= this.Attr && 9 >= this.Attr;
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x00008A91 File Offset: 0x00006C91
	public bool IsNormalNote()
	{
		return this.Duration == 0;
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x0600074C RID: 1868 RVA: 0x00008A9C File Offset: 0x00006C9C
	// (set) Token: 0x0600074D RID: 1869 RVA: 0x00008AA4 File Offset: 0x00006CA4
	public int NextIdx
	{
		get
		{
			return this.m_iNextIdx;
		}
		set
		{
			this.m_iNextIdx = value;
		}
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x00008AAD File Offset: 0x00006CAD
	public bool IsDirNote()
	{
		return this.IsDirUpNote() || this.IsDirDnNote();
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x00008ACA File Offset: 0x00006CCA
	public bool IsDirUpNote()
	{
		return (long)this.Attr == 10L;
	}

	// Token: 0x06000750 RID: 1872 RVA: 0x00008AD8 File Offset: 0x00006CD8
	public bool IsDirDnNote()
	{
		return (long)this.Attr == 11L;
	}

	// Token: 0x040005B6 RID: 1462
	private int tick;

	// Token: 0x040005B7 RID: 1463
	private int dur;

	// Token: 0x040005B8 RID: 1464
	private int m_iTrack;

	// Token: 0x040005B9 RID: 1465
	private int attr;

	// Token: 0x040005BA RID: 1466
	private bool m_bCanRender = true;

	// Token: 0x040005BB RID: 1467
	private ArrayList m_arrSameTime = new ArrayList();

	// Token: 0x040005BC RID: 1468
	private bool m_bEndView;

	// Token: 0x040005BD RID: 1469
	private ArrayList m_arrMoveInfo = new ArrayList();

	// Token: 0x040005BE RID: 1470
	private int m_iNextIdx;

	// Token: 0x040005BF RID: 1471
	public bool bLongFail;

	// Token: 0x040005C0 RID: 1472
	public int iTickFail;

	// Token: 0x040005C1 RID: 1473
	public int iComboCount;

	// Token: 0x040005C2 RID: 1474
	public float PreDifValue;

	// Token: 0x040005C3 RID: 1475
	public float MoveDifValue;

	// Token: 0x040005C4 RID: 1476
	public GameObject objControlNote;

	// Token: 0x040005C5 RID: 1477
	public GameObject RenderFever;

	// Token: 0x040005C6 RID: 1478
	public bool[] CheckLongNote = new bool[10];
}
