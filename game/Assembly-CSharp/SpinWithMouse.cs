using System;
using UnityEngine;

// Token: 0x02000026 RID: 38
[AddComponentMenu("NGUI/Examples/Spin With Mouse")]
public class SpinWithMouse : MonoBehaviour
{
	// Token: 0x060000FA RID: 250 RVA: 0x00003FDC File Offset: 0x000021DC
	private void Start()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00015C50 File Offset: 0x00013E50
	private void OnDrag(Vector2 delta)
	{
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
		if (this.target != null)
		{
			this.target.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * this.speed, 0f) * this.target.localRotation;
		}
		else
		{
			this.mTrans.localRotation = Quaternion.Euler(0f, -0.5f * delta.x * this.speed, 0f) * this.mTrans.localRotation;
		}
	}

	// Token: 0x040000E1 RID: 225
	public Transform target;

	// Token: 0x040000E2 RID: 226
	public float speed = 1f;

	// Token: 0x040000E3 RID: 227
	private Transform mTrans;
}
