using System;
using UnityEngine;

// Token: 0x02000296 RID: 662
public class tk2dTileMapDemoCoin : MonoBehaviour
{
	// Token: 0x060012FB RID: 4859 RVA: 0x000101F4 File Offset: 0x0000E3F4
	private void Awake()
	{
		if (this.animator == null)
		{
			Debug.LogError("Coin - Assign animator in the inspector before proceeding.");
			base.enabled = false;
		}
		else
		{
			this.animator.enabled = false;
		}
	}

	// Token: 0x060012FC RID: 4860 RVA: 0x00010229 File Offset: 0x0000E429
	private void OnBecameInvisible()
	{
		if (this.animator.enabled)
		{
			this.animator.enabled = false;
		}
	}

	// Token: 0x060012FD RID: 4861 RVA: 0x00010247 File Offset: 0x0000E447
	private void OnBecameVisible()
	{
		if (!this.animator.enabled)
		{
			this.animator.enabled = true;
		}
	}

	// Token: 0x040014C7 RID: 5319
	public tk2dSpriteAnimator animator;
}
