using System;
using UnityEngine;

// Token: 0x0200021A RID: 538
public class DepthScript : MonoBehaviour
{
	// Token: 0x06000F7F RID: 3967 RVA: 0x000706C4 File Offset: 0x0006E8C4
	private void Awake()
	{
		if (!this.bDefault)
		{
			base.renderer.material.renderQueue += this.Depth;
			this.m_iTotalDepth += this.Depth;
		}
		else
		{
			this.SetDefaultDepth(this.Depth);
		}
	}

	// Token: 0x06000F80 RID: 3968 RVA: 0x00003648 File Offset: 0x00001848
	private void SetDepth(int iDepth)
	{
	}

	// Token: 0x06000F81 RID: 3969 RVA: 0x0000D50E File Offset: 0x0000B70E
	private void SetDefaultDepth(int iDepth)
	{
		base.renderer.material.renderQueue = 3000 + iDepth;
	}

	// Token: 0x06000F82 RID: 3970 RVA: 0x00003648 File Offset: 0x00001848
	private void ClearDepth()
	{
	}

	// Token: 0x04001152 RID: 4434
	public const int DEFAULTDEPTH = 3000;

	// Token: 0x04001153 RID: 4435
	public bool bDefault;

	// Token: 0x04001154 RID: 4436
	public int Depth;

	// Token: 0x04001155 RID: 4437
	private int m_iTotalDepth;
}
