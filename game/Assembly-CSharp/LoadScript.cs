using System;
using UnityEngine;

// Token: 0x020001A9 RID: 425
public class LoadScript : MonoBehaviour
{
	// Token: 0x06000C9D RID: 3229 RVA: 0x0000B64A File Offset: 0x0000984A
	private void Awake()
	{
		this.m_sCopy = base.GetComponent<CopyRightScript>();
		this.m_sCopy.enabled = false;
	}

	// Token: 0x06000C9E RID: 3230 RVA: 0x00003648 File Offset: 0x00001848
	private void Start()
	{
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x0000B664 File Offset: 0x00009864
	private void Update()
	{
		this.m_sCopy.enabled = true;
	}

	// Token: 0x04000C80 RID: 3200
	private CopyRightScript m_sCopy;
}
