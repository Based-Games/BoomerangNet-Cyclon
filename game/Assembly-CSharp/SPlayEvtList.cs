using System;
using System.Collections;

// Token: 0x020000EC RID: 236
public class SPlayEvtList
{
	// Token: 0x060007FA RID: 2042 RVA: 0x00008FCC File Offset: 0x000071CC
	public void Clear()
	{
		this.ClearEvent();
		this.isPlayTrack = false;
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x00008FDB File Offset: 0x000071DB
	public void ClearEvent()
	{
		this.evtVec.Clear();
		this.m_iCurEvt = this.evtVec.Count;
		this.isPlayedEvent = false;
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x00009000 File Offset: 0x00007200
	public void AddEvt(ScoreEventBase pEvt)
	{
		this.evtVec.Add(pEvt);
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x0000900F File Offset: 0x0000720F
	public void FirstIter()
	{
		this.m_iCurEvt = 0;
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x00009018 File Offset: 0x00007218
	public void NextIter()
	{
		if (this.IsPlayedEvent())
		{
			this.SetPlayedEvent(false);
		}
		this.m_iCurEvt++;
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x0000903A File Offset: 0x0000723A
	public int TestCur()
	{
		return this.m_iCurEvt;
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x00009042 File Offset: 0x00007242
	public bool IsPlayedEvent()
	{
		return this.isPlayedEvent;
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x0000904A File Offset: 0x0000724A
	public void SetPlayedEvent(bool isTrue)
	{
		this.isPlayedEvent = isTrue;
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x00009053 File Offset: 0x00007253
	public ScoreEventBase GetCurEvt()
	{
		return (this.m_iCurEvt < this.evtVec.Count) ? ((ScoreEventBase)this.evtVec[this.m_iCurEvt]) : null;
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x0003DB08 File Offset: 0x0003BD08
	public void CollectViewNote()
	{
		this.evtView.Clear();
		int num = this.m_iCurEvt - 1;
		for (int i = num; i < num + 10; i++)
		{
			if (0 <= i)
			{
				if (this.evtVec.Count > i)
				{
					this.evtView.Add((ScoreEventBase)this.evtVec[i]);
				}
			}
		}
	}

	// Token: 0x04000686 RID: 1670
	public bool isPlayTrack;

	// Token: 0x04000687 RID: 1671
	public bool isPlayedEvent;

	// Token: 0x04000688 RID: 1672
	public ArrayList evtVec = new ArrayList();

	// Token: 0x04000689 RID: 1673
	public ArrayList evtView = new ArrayList();

	// Token: 0x0400068A RID: 1674
	public int m_iCurEvt;
}
