using System;
using UnityEngine;

// Token: 0x02000094 RID: 148
[AddComponentMenu("NGUI/Tween/Tween Scale")]
public class TweenScale : UITweener
{
	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000496 RID: 1174 RVA: 0x00006ACF File Offset: 0x00004CCF
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

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x06000497 RID: 1175 RVA: 0x00006AF4 File Offset: 0x00004CF4
	// (set) Token: 0x06000498 RID: 1176 RVA: 0x00006B01 File Offset: 0x00004D01
	public Vector3 value
	{
		get
		{
			return this.cachedTransform.localScale;
		}
		set
		{
			this.cachedTransform.localScale = value;
		}
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x06000499 RID: 1177 RVA: 0x00006B0F File Offset: 0x00004D0F
	// (set) Token: 0x0600049A RID: 1178 RVA: 0x00006B17 File Offset: 0x00004D17
	[Obsolete("Use 'value' instead")]
	public Vector3 scale
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

	// Token: 0x0600049B RID: 1179 RVA: 0x000260C0 File Offset: 0x000242C0
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
		if (this.updateTable)
		{
			if (this.mTable == null)
			{
				this.mTable = NGUITools.FindInParents<UITable>(base.gameObject);
				if (this.mTable == null)
				{
					this.updateTable = false;
					return;
				}
			}
			this.mTable.repositionNow = true;
		}
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x00026148 File Offset: 0x00024348
	public static TweenScale Begin(GameObject go, float duration, Vector3 scale)
	{
		TweenScale tweenScale = UITweener.Begin<TweenScale>(go, duration);
		tweenScale.from = tweenScale.value;
		tweenScale.to = scale;
		if (duration <= 0f)
		{
			tweenScale.Sample(1f, true);
			tweenScale.enabled = false;
		}
		return tweenScale;
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x00006B20 File Offset: 0x00004D20
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x00006B2E File Offset: 0x00004D2E
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x00006B3C File Offset: 0x00004D3C
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x00006B4A File Offset: 0x00004D4A
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04000361 RID: 865
	public Vector3 from = Vector3.one;

	// Token: 0x04000362 RID: 866
	public Vector3 to = Vector3.one;

	// Token: 0x04000363 RID: 867
	public bool updateTable;

	// Token: 0x04000364 RID: 868
	private Transform mTrans;

	// Token: 0x04000365 RID: 869
	private UITable mTable;
}
