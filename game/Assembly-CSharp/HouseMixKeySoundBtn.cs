using System;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public class HouseMixKeySoundBtn : MonoBehaviour
{
	// Token: 0x06000D3D RID: 3389 RVA: 0x0000BD09 File Offset: 0x00009F09
	private void Awake()
	{
		base.transform.FindChild("Sprite_btn").gameObject.SetActive(false);
	}
}
