using System;
using UnityEngine;

// Token: 0x0200002A RID: 42
[AddComponentMenu("NGUI/Interaction/Button Activate")]
public class UIButtonActivate : MonoBehaviour
{
	// Token: 0x0600010E RID: 270 RVA: 0x00004121 File Offset: 0x00002321
	private void OnClick()
	{
		if (this.target != null)
		{
			NGUITools.SetActive(this.target, this.state);
		}
	}

	// Token: 0x040000EC RID: 236
	public GameObject target;

	// Token: 0x040000ED RID: 237
	public bool state = true;
}
