using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000E3 RID: 227
public class InGameTouchInfo
{
	// Token: 0x060007A9 RID: 1961 RVA: 0x000398A0 File Offset: 0x00037AA0
	public void Init()
	{
		for (int i = 0; i < 12; i++)
		{
			this.SingleCheckInput[i] = false;
			this.MultiCheckInput[i] = false;
			this.MultiInputPos[i] = Vector3.zero;
		}
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x000398E8 File Offset: 0x00037AE8
	public void AddInterVal(Vector3 vPos, int iId)
	{
		if (this.ContainInterValKey(iId))
		{
			return;
		}
		IntervalPosInfo intervalPosInfo = new IntervalPosInfo();
		intervalPosInfo.iId = iId;
		intervalPosInfo.vPos = vPos;
		intervalPosInfo.fIntervalTime = 0f;
		intervalPosInfo.bCheck = false;
		intervalPosInfo.bPreCheck = false;
		this.ArrIntervalPos.Add(intervalPosInfo);
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x0003993C File Offset: 0x00037B3C
	public bool ContainInterValKey(int iId)
	{
		foreach (object obj in this.ArrIntervalPos)
		{
			IntervalPosInfo intervalPosInfo = (IntervalPosInfo)obj;
			if (intervalPosInfo.iId == iId)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x000399B0 File Offset: 0x00037BB0
	public bool IsSamePos(Vector3 vPos, int iId)
	{
		foreach (object obj in this.ArrIntervalPos)
		{
			IntervalPosInfo intervalPosInfo = (IntervalPosInfo)obj;
			if (0.3f < intervalPosInfo.fIntervalTime)
			{
				intervalPosInfo.bCheck = true;
			}
			if (iId == intervalPosInfo.iId)
			{
				if (0.3f < intervalPosInfo.fIntervalTime)
				{
					float num = Vector3.Distance(intervalPosInfo.vPos, vPos);
					if (10f > num && !intervalPosInfo.bPreCheck)
					{
						return true;
					}
				}
				else
				{
					float num2 = Vector3.Distance(intervalPosInfo.vPos, vPos);
					if (30f < num2)
					{
						intervalPosInfo.bPreCheck = true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x00039A9C File Offset: 0x00037C9C
	public void UpdateLast()
	{
		for (int i = 0; i < this.ArrIntervalPos.Count; i++)
		{
			IntervalPosInfo intervalPosInfo = (IntervalPosInfo)this.ArrIntervalPos[i];
			intervalPosInfo.fIntervalTime += Time.deltaTime;
			if (intervalPosInfo.bCheck && 0.3f < intervalPosInfo.fIntervalTime)
			{
				this.ArrIntervalPos.Remove(intervalPosInfo);
			}
		}
	}

	// Token: 0x04000620 RID: 1568
	private const float MAX_INTERVAL = 0.3f;

	// Token: 0x04000621 RID: 1569
	private const float MAX_CHECKDISTANCE = 10f;

	// Token: 0x04000622 RID: 1570
	private const float MIX_MOVEDISTANCE = 30f;

	// Token: 0x04000623 RID: 1571
	public bool[] SingleCheckInput = new bool[12];

	// Token: 0x04000624 RID: 1572
	public bool[] MultiCheckInput = new bool[12];

	// Token: 0x04000625 RID: 1573
	public Vector3[] MultiInputPos = new Vector3[12];

	// Token: 0x04000626 RID: 1574
	public ArrayList ArrIntervalPos = new ArrayList();

	// Token: 0x04000627 RID: 1575
	public string strInfo = string.Empty;
}
