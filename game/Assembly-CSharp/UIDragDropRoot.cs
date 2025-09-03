using System;
using UnityEngine;

// Token: 0x02000038 RID: 56
[AddComponentMenu("NGUI/Interaction/Drag and Drop Root")]
public class UIDragDropRoot : MonoBehaviour
{
	// Token: 0x0600015A RID: 346 RVA: 0x000046CC File Offset: 0x000028CC
	private void OnEnable()
	{
		UIDragDropRoot.root = base.transform;
	}

	// Token: 0x0600015B RID: 347 RVA: 0x000046D9 File Offset: 0x000028D9
	private void OnDisable()
	{
		if (UIDragDropRoot.root == base.transform)
		{
			UIDragDropRoot.root = null;
		}
	}

	// Token: 0x04000132 RID: 306
	public static Transform root;
}
