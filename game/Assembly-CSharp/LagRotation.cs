using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
[AddComponentMenu("NGUI/Examples/Lag Rotation")]
public class LagRotation : MonoBehaviour
{
	// Token: 0x060000EA RID: 234 RVA: 0x00003E91 File Offset: 0x00002091
	private void OnEnable()
	{
		this.mTrans = base.transform;
		this.mRelative = this.mTrans.localRotation;
		this.mAbsolute = this.mTrans.rotation;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x000159D4 File Offset: 0x00013BD4
	private void Update()
	{
		Transform parent = this.mTrans.parent;
		if (parent != null)
		{
			float num = ((!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime);
			this.mAbsolute = Quaternion.Slerp(this.mAbsolute, parent.rotation * this.mRelative, num * this.speed);
			this.mTrans.rotation = this.mAbsolute;
		}
	}

	// Token: 0x040000CE RID: 206
	public int updateOrder;

	// Token: 0x040000CF RID: 207
	public float speed = 10f;

	// Token: 0x040000D0 RID: 208
	public bool ignoreTimeScale;

	// Token: 0x040000D1 RID: 209
	private Transform mTrans;

	// Token: 0x040000D2 RID: 210
	private Quaternion mRelative;

	// Token: 0x040000D3 RID: 211
	private Quaternion mAbsolute;
}
