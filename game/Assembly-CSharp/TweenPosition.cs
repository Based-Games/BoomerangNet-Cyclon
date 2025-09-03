using System;
using UnityEngine;

// Token: 0x02000092 RID: 146
[AddComponentMenu("NGUI/Tween/Tween Position")]
public class TweenPosition : UITweener
{
	// Token: 0x170000BF RID: 191
	// (get) Token: 0x0600047E RID: 1150 RVA: 0x00006962 File Offset: 0x00004B62
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

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x0600047F RID: 1151 RVA: 0x00006987 File Offset: 0x00004B87
	// (set) Token: 0x06000480 RID: 1152 RVA: 0x0000698F File Offset: 0x00004B8F
	[Obsolete("Use 'value' instead")]
	public Vector3 position
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

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x06000481 RID: 1153 RVA: 0x00006998 File Offset: 0x00004B98
	// (set) Token: 0x06000482 RID: 1154 RVA: 0x000069A5 File Offset: 0x00004BA5
	public Vector3 value
	{
		get
		{
			return this.cachedTransform.localPosition;
		}
		set
		{
			this.cachedTransform.localPosition = value;
		}
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x000069B3 File Offset: 0x00004BB3
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x00025FD8 File Offset: 0x000241D8
	public static TweenPosition Begin(GameObject go, float duration, Vector3 pos)
	{
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration);
		tweenPosition.from = tweenPosition.value;
		tweenPosition.to = pos;
		if (duration <= 0f)
		{
			tweenPosition.Sample(1f, true);
			tweenPosition.enabled = false;
		}
		return tweenPosition;
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x000069DE File Offset: 0x00004BDE
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x000069EC File Offset: 0x00004BEC
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x000069FA File Offset: 0x00004BFA
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x00006A08 File Offset: 0x00004C08
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x0400035B RID: 859
	public Vector3 from;

	// Token: 0x0400035C RID: 860
	public Vector3 to;

	// Token: 0x0400035D RID: 861
	private Transform mTrans;
}
