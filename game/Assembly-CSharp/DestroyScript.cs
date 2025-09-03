using System;
using UnityEngine;

// Token: 0x0200021B RID: 539
public class DestroyScript : MonoBehaviour
{
	// Token: 0x06000F84 RID: 3972 RVA: 0x0000D53A File Offset: 0x0000B73A
	private void Start()
	{
		UnityEngine.Object.Destroy(base.gameObject, this.fTime);
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x00003648 File Offset: 0x00001848
	private void Update()
	{
	}

	// Token: 0x04001156 RID: 4438
	public float fTime = 5f;
}
