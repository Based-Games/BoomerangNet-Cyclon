using System;
using UnityEngine;

// Token: 0x020002B6 RID: 694
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUISpriteAnimator")]
public class tk2dUISpriteAnimator : tk2dSpriteAnimator
{
	// Token: 0x06001480 RID: 5248 RVA: 0x00011BFA File Offset: 0x0000FDFA
	public override void LateUpdate()
	{
		base.UpdateAnimation(tk2dUITime.deltaTime);
	}
}
