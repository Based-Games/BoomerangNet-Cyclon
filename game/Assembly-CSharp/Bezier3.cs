using System;
using UnityEngine;

// Token: 0x0200022E RID: 558
[Serializable]
public class Bezier3
{
	// Token: 0x06001007 RID: 4103 RVA: 0x0000DAE2 File Offset: 0x0000BCE2
	public void Set(Vector3 vStart, Vector3 vCtrl, Vector3 vEnd)
	{
		this.m_vStart = vStart;
		this.m_vCtrl = vCtrl;
		this.m_vEnd = vEnd;
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x00073164 File Offset: 0x00071364
	public Vector3 GetVec3(float fRate)
	{
		float num = fRate * fRate;
		float num2 = 1f - fRate;
		float num3 = num2 * num2;
		this.m_vResult.x = this.m_vStart.x * num3 + 2f * this.m_vCtrl.x * num2 * fRate + this.m_vEnd.x * num;
		this.m_vResult.y = this.m_vStart.y * num3 + 2f * this.m_vCtrl.y * num2 * fRate + this.m_vEnd.y * num;
		this.m_vResult.z = this.m_vStart.z * num3 + 2f * this.m_vCtrl.z * num2 * fRate + this.m_vEnd.z * num;
		return this.m_vResult;
	}

	// Token: 0x040011A2 RID: 4514
	private Vector3 m_vStart;

	// Token: 0x040011A3 RID: 4515
	private Vector3 m_vCtrl;

	// Token: 0x040011A4 RID: 4516
	private Vector3 m_vEnd;

	// Token: 0x040011A5 RID: 4517
	private Vector3 m_vResult = default(Vector3);
}
