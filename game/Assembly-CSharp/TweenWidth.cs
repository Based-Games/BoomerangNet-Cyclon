using System;
using UnityEngine;

// Token: 0x02000097 RID: 151
[AddComponentMenu("NGUI/Tween/Tween Width")]
[RequireComponent(typeof(UIWidget))]
public class TweenWidth : UITweener
{
	// Token: 0x170000CB RID: 203
	// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00006C49 File Offset: 0x00004E49
	public UIWidget cachedWidget
	{
		get
		{
			if (this.mWidget == null)
			{
				this.mWidget = base.GetComponent<UIWidget>();
			}
			return this.mWidget;
		}
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00006C6E File Offset: 0x00004E6E
	// (set) Token: 0x060004B2 RID: 1202 RVA: 0x00006C76 File Offset: 0x00004E76
	[Obsolete("Use 'value' instead")]
	public int width
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

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00006C7F File Offset: 0x00004E7F
	// (set) Token: 0x060004B4 RID: 1204 RVA: 0x00006C8C File Offset: 0x00004E8C
	public int value
	{
		get
		{
			return this.cachedWidget.width;
		}
		set
		{
			this.cachedWidget.width = value;
		}
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x00026438 File Offset: 0x00024638
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Mathf.RoundToInt((float)this.from * (1f - factor) + (float)this.to * factor);
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

	// Token: 0x060004B6 RID: 1206 RVA: 0x000264BC File Offset: 0x000246BC
	public static TweenWidth Begin(UIWidget widget, float duration, int width)
	{
		TweenWidth tweenWidth = UITweener.Begin<TweenWidth>(widget.gameObject, duration);
		tweenWidth.from = widget.width;
		tweenWidth.to = width;
		if (duration <= 0f)
		{
			tweenWidth.Sample(1f, true);
			tweenWidth.enabled = false;
		}
		return tweenWidth;
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x00006C9A File Offset: 0x00004E9A
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x00006CA8 File Offset: 0x00004EA8
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x00006CB6 File Offset: 0x00004EB6
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x00006CC4 File Offset: 0x00004EC4
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04000370 RID: 880
	public int from = 100;

	// Token: 0x04000371 RID: 881
	public int to = 100;

	// Token: 0x04000372 RID: 882
	public bool updateTable;

	// Token: 0x04000373 RID: 883
	private UIWidget mWidget;

	// Token: 0x04000374 RID: 884
	private UITable mTable;
}
