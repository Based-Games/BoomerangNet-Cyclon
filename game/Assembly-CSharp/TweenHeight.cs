using System;
using UnityEngine;

// Token: 0x02000090 RID: 144
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/Tween/Tween Height")]
public class TweenHeight : UITweener
{
	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x06000468 RID: 1128 RVA: 0x00006827 File Offset: 0x00004A27
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

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x06000469 RID: 1129 RVA: 0x0000684C File Offset: 0x00004A4C
	// (set) Token: 0x0600046A RID: 1130 RVA: 0x00006854 File Offset: 0x00004A54
	[Obsolete("Use 'value' instead")]
	public int height
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

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x0600046B RID: 1131 RVA: 0x0000685D File Offset: 0x00004A5D
	// (set) Token: 0x0600046C RID: 1132 RVA: 0x0000686A File Offset: 0x00004A6A
	public int value
	{
		get
		{
			return this.cachedWidget.height;
		}
		set
		{
			this.cachedWidget.height = value;
		}
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x00025EC0 File Offset: 0x000240C0
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

	// Token: 0x0600046E RID: 1134 RVA: 0x00025F44 File Offset: 0x00024144
	public static TweenHeight Begin(UIWidget widget, float duration, int height)
	{
		TweenHeight tweenHeight = UITweener.Begin<TweenHeight>(widget.gameObject, duration);
		tweenHeight.from = widget.height;
		tweenHeight.to = height;
		if (duration <= 0f)
		{
			tweenHeight.Sample(1f, true);
			tweenHeight.enabled = false;
		}
		return tweenHeight;
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x00006878 File Offset: 0x00004A78
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x00006886 File Offset: 0x00004A86
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x00006894 File Offset: 0x00004A94
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x000068A2 File Offset: 0x00004AA2
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04000353 RID: 851
	public int from = 100;

	// Token: 0x04000354 RID: 852
	public int to = 100;

	// Token: 0x04000355 RID: 853
	public bool updateTable;

	// Token: 0x04000356 RID: 854
	private UIWidget mWidget;

	// Token: 0x04000357 RID: 855
	private UITable mTable;
}
