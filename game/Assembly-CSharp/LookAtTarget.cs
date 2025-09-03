using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
[AddComponentMenu("NGUI/Examples/Look At Target")]
public class LookAtTarget : MonoBehaviour
{
	// Token: 0x060000EF RID: 239 RVA: 0x00003EF1 File Offset: 0x000020F1
	private void Start()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x00015A50 File Offset: 0x00013C50
	private void LateUpdate()
	{
		if (this.target != null)
		{
			Vector3 vector = this.target.position - this.mTrans.position;
			float magnitude = vector.magnitude;
			if (magnitude > 0.001f)
			{
				Quaternion quaternion = Quaternion.LookRotation(vector);
				this.mTrans.rotation = Quaternion.Slerp(this.mTrans.rotation, quaternion, Mathf.Clamp01(this.speed * Time.deltaTime));
			}
		}
	}

	// Token: 0x040000D5 RID: 213
	public int level;

	// Token: 0x040000D6 RID: 214
	public Transform target;

	// Token: 0x040000D7 RID: 215
	public float speed = 8f;

	// Token: 0x040000D8 RID: 216
	private Transform mTrans;
}
