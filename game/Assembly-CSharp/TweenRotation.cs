using System;
using UnityEngine;

// Token: 0x02000093 RID: 147
[AddComponentMenu("NGUI/Tween/Tween Rotation")]
public class TweenRotation : UITweener
{
	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x0600048A RID: 1162 RVA: 0x00006A16 File Offset: 0x00004C16
	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x0600048B RID: 1163 RVA: 0x00006A3B File Offset: 0x00004C3B
	// (set) Token: 0x0600048C RID: 1164 RVA: 0x00006A43 File Offset: 0x00004C43
	[Obsolete("Use 'value' instead")]
	public Quaternion rotation
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x0600048D RID: 1165 RVA: 0x00006A4C File Offset: 0x00004C4C
	// (set) Token: 0x0600048E RID: 1166 RVA: 0x00006A59 File Offset: 0x00004C59
	public Quaternion value
	{
		get
		{
			return this.cachedTransform.localRotation;
		}
		set
		{
			this.cachedTransform.localRotation = value;
		}
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x00006A67 File Offset: 0x00004C67
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Quaternion.Slerp(Quaternion.Euler(this.from), Quaternion.Euler(this.to), factor);
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x00026020 File Offset: 0x00024220
	public static TweenRotation Begin(GameObject go, float duration, Quaternion rot)
	{
		TweenRotation tweenRotation = UITweener.Begin<TweenRotation>(go, duration);
		tweenRotation.from = tweenRotation.value.eulerAngles;
		tweenRotation.to = rot.eulerAngles;
		if (duration <= 0f)
		{
			tweenRotation.Sample(1f, true);
			tweenRotation.enabled = false;
		}
		return tweenRotation;
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x00026078 File Offset: 0x00024278
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value.eulerAngles;
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x0002609C File Offset: 0x0002429C
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value.eulerAngles;
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x00006A8B File Offset: 0x00004C8B
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = Quaternion.Euler(this.from);
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x00006A9E File Offset: 0x00004C9E
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = Quaternion.Euler(this.to);
	}

	// Token: 0x0400035E RID: 862
	public Vector3 from;

	// Token: 0x0400035F RID: 863
	public Vector3 to;

	// Token: 0x04000360 RID: 864
	private Transform mTrans;
}
