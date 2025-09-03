using System;
using UnityEngine;

// Token: 0x02000025 RID: 37
[AddComponentMenu("NGUI/Examples/Spin")]
public class Spin : MonoBehaviour
{
	// Token: 0x060000F5 RID: 245 RVA: 0x00003F73 File Offset: 0x00002173
	private void Start()
	{
		this.mTrans = base.transform;
		this.mRb = base.rigidbody;
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00003F8D File Offset: 0x0000218D
	private void Update()
	{
		if (this.mRb == null)
		{
			this.ApplyDelta(Time.deltaTime);
		}
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00003FAB File Offset: 0x000021AB
	private void FixedUpdate()
	{
		if (this.mRb != null)
		{
			this.ApplyDelta(Time.deltaTime);
		}
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00015BD8 File Offset: 0x00013DD8
	public void ApplyDelta(float delta)
	{
		delta *= 360f;
		Quaternion quaternion = Quaternion.Euler(this.rotationsPerSecond * delta);
		if (this.mRb == null)
		{
			this.mTrans.rotation = this.mTrans.rotation * quaternion;
		}
		else
		{
			this.mRb.MoveRotation(this.mRb.rotation * quaternion);
		}
	}

	// Token: 0x040000DE RID: 222
	public Vector3 rotationsPerSecond = new Vector3(0f, 0.1f, 0f);

	// Token: 0x040000DF RID: 223
	private Rigidbody mRb;

	// Token: 0x040000E0 RID: 224
	private Transform mTrans;
}
