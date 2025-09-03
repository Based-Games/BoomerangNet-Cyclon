using System;
using UnityEngine;

// Token: 0x020002B0 RID: 688
[Serializable]
public class tk2dUILayoutItem
{
	// Token: 0x0600143F RID: 5183 RVA: 0x000889A4 File Offset: 0x00086BA4
	public static tk2dUILayoutItem FixedSizeLayoutItem()
	{
		return new tk2dUILayoutItem
		{
			fixedSize = true
		};
	}

	// Token: 0x0400159A RID: 5530
	public tk2dBaseSprite sprite;

	// Token: 0x0400159B RID: 5531
	public tk2dUIMask UIMask;

	// Token: 0x0400159C RID: 5532
	public tk2dUILayout layout;

	// Token: 0x0400159D RID: 5533
	public GameObject gameObj;

	// Token: 0x0400159E RID: 5534
	public bool snapLeft;

	// Token: 0x0400159F RID: 5535
	public bool snapRight;

	// Token: 0x040015A0 RID: 5536
	public bool snapTop;

	// Token: 0x040015A1 RID: 5537
	public bool snapBottom;

	// Token: 0x040015A2 RID: 5538
	public bool fixedSize;

	// Token: 0x040015A3 RID: 5539
	public float fillPercentage = -1f;

	// Token: 0x040015A4 RID: 5540
	public float sizeProportion = 1f;

	// Token: 0x040015A5 RID: 5541
	public bool inLayoutList;

	// Token: 0x040015A6 RID: 5542
	public int childDepth;

	// Token: 0x040015A7 RID: 5543
	public Vector3 oldPos = Vector3.zero;
}
