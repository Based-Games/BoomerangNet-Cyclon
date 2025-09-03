using System;
using UnityEngine;

// Token: 0x0200022F RID: 559
[Serializable]
public class Bezier4
{
	// Token: 0x0600100A RID: 4106 RVA: 0x0000DAF9 File Offset: 0x0000BCF9
	public void Set(Vector3 vStart, Vector3 vCtrl1, Vector3 vCtrl2, Vector3 vEnd)
	{
		this.m_vStart = vStart;
		this.m_vCtrl1 = vCtrl1;
		this.m_vCtrl2 = vCtrl2;
		this.m_vEnd = vEnd;
	}

	// Token: 0x0600100B RID: 4107 RVA: 0x0007323C File Offset: 0x0007143C
	public Vector3 GetVec3(float fRate)
	{
		Vector3 vector = default(Vector3);
		float num = 1f - fRate;
		float num2 = num * num * num;
		float num3 = fRate * fRate * fRate;
		vector.x = num2 * this.m_vStart.x + 3f * fRate * num * num * this.m_vCtrl1.x + 3f * fRate * fRate * num * this.m_vCtrl2.x + num3 * this.m_vEnd.x;
		vector.y = num2 * this.m_vStart.y + 3f * fRate * num * num * this.m_vCtrl1.y + 3f * fRate * fRate * num * this.m_vCtrl2.y + num3 * this.m_vEnd.y;
		vector.z = num2 * this.m_vStart.z + 3f * fRate * num * num * this.m_vCtrl1.z + 3f * fRate * fRate * num * this.m_vCtrl2.z + num3 * this.m_vEnd.z;
		return vector;
	}

	// Token: 0x040011A6 RID: 4518
	private Vector3 m_vStart;

	// Token: 0x040011A7 RID: 4519
	private Vector3 m_vCtrl1;

	// Token: 0x040011A8 RID: 4520
	private Vector3 m_vCtrl2;

	// Token: 0x040011A9 RID: 4521
	private Vector3 m_vEnd;
}
