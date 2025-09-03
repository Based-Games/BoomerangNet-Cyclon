using System;
using UnityEngine;

// Token: 0x02000095 RID: 149
[AddComponentMenu("NGUI/Tween/Tween Transform")]
public class TweenTransform : UITweener
{
	// Token: 0x060004A2 RID: 1186 RVA: 0x00026190 File Offset: 0x00024390
	protected override void OnUpdate(float factor, bool isFinished)
	{
		if (this.to != null)
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
				this.mPos = this.mTrans.position;
				this.mRot = this.mTrans.rotation;
				this.mScale = this.mTrans.localScale;
			}
			if (this.from != null)
			{
				this.mTrans.position = this.from.position * (1f - factor) + this.to.position * factor;
				this.mTrans.localScale = this.from.localScale * (1f - factor) + this.to.localScale * factor;
				this.mTrans.rotation = Quaternion.Slerp(this.from.rotation, this.to.rotation, factor);
			}
			else
			{
				this.mTrans.position = this.mPos * (1f - factor) + this.to.position * factor;
				this.mTrans.localScale = this.mScale * (1f - factor) + this.to.localScale * factor;
				this.mTrans.rotation = Quaternion.Slerp(this.mRot, this.to.rotation, factor);
			}
			if (this.parentWhenFinished && isFinished)
			{
				this.mTrans.parent = this.to;
			}
		}
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x00006B58 File Offset: 0x00004D58
	public static TweenTransform Begin(GameObject go, float duration, Transform to)
	{
		return TweenTransform.Begin(go, duration, null, to);
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x00026358 File Offset: 0x00024558
	public static TweenTransform Begin(GameObject go, float duration, Transform from, Transform to)
	{
		TweenTransform tweenTransform = UITweener.Begin<TweenTransform>(go, duration);
		tweenTransform.from = from;
		tweenTransform.to = to;
		if (duration <= 0f)
		{
			tweenTransform.Sample(1f, true);
			tweenTransform.enabled = false;
		}
		return tweenTransform;
	}

	// Token: 0x04000366 RID: 870
	public Transform from;

	// Token: 0x04000367 RID: 871
	public Transform to;

	// Token: 0x04000368 RID: 872
	public bool parentWhenFinished;

	// Token: 0x04000369 RID: 873
	private Transform mTrans;

	// Token: 0x0400036A RID: 874
	private Vector3 mPos;

	// Token: 0x0400036B RID: 875
	private Quaternion mRot;

	// Token: 0x0400036C RID: 876
	private Vector3 mScale;
}
