using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
[AddComponentMenu("NGUI/Examples/Lag Position")]
public class LagPosition : MonoBehaviour
{
	// Token: 0x060000E7 RID: 231 RVA: 0x00003E4E File Offset: 0x0000204E
	private void OnEnable()
	{
		this.mTrans = base.transform;
		this.mAbsolute = this.mTrans.position;
		this.mRelative = this.mTrans.localPosition;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x000158C8 File Offset: 0x00013AC8
	private void Update()
	{
		Transform parent = this.mTrans.parent;
		if (parent != null)
		{
			float num = ((!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime);
			Vector3 vector = parent.position + parent.rotation * this.mRelative;
			this.mAbsolute.x = Mathf.Lerp(this.mAbsolute.x, vector.x, Mathf.Clamp01(num * this.speed.x));
			this.mAbsolute.y = Mathf.Lerp(this.mAbsolute.y, vector.y, Mathf.Clamp01(num * this.speed.y));
			this.mAbsolute.z = Mathf.Lerp(this.mAbsolute.z, vector.z, Mathf.Clamp01(num * this.speed.z));
			this.mTrans.position = this.mAbsolute;
		}
	}

	// Token: 0x040000C8 RID: 200
	public int updateOrder;

	// Token: 0x040000C9 RID: 201
	public Vector3 speed = new Vector3(10f, 10f, 10f);

	// Token: 0x040000CA RID: 202
	public bool ignoreTimeScale;

	// Token: 0x040000CB RID: 203
	private Transform mTrans;

	// Token: 0x040000CC RID: 204
	private Vector3 mRelative;

	// Token: 0x040000CD RID: 205
	private Vector3 mAbsolute;
}
