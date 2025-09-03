using System;
using UnityEngine;

// Token: 0x020001C9 RID: 457
public class HouseMixPlayBtn : MonoBehaviour
{
	// Token: 0x06000D62 RID: 3426 RVA: 0x0000BE36 File Offset: 0x0000A036
	private void Awake()
	{
		this.m_HouseMixSelectDiscInfo = base.transform.parent.GetComponent<HouseMixSelectDiscInfo>();
	}

	// Token: 0x06000D63 RID: 3427 RVA: 0x00003648 File Offset: 0x00001848
	private void Update()
	{
	}

	// Token: 0x04000DA0 RID: 3488
	private HouseMixSelectDiscInfo m_HouseMixSelectDiscInfo;

	// Token: 0x04000DA1 RID: 3489
	private bool m_bAutoSelect;
}
