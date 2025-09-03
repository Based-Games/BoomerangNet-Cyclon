using System;
using UnityEngine;

// Token: 0x0200008E RID: 142
[AddComponentMenu("NGUI/Tween/Tween Color")]
public class TweenColor : UITweener
{
	// Token: 0x06000450 RID: 1104 RVA: 0x00025CC8 File Offset: 0x00023EC8
	private void Cache()
	{
		this.mCached = true;
		this.mWidget = base.GetComponentInChildren<UIWidget>();
		Renderer renderer = base.renderer;
		if (renderer != null)
		{
			this.mMat = renderer.material;
		}
		this.mLight = base.light;
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x06000451 RID: 1105 RVA: 0x000066E6 File Offset: 0x000048E6
	// (set) Token: 0x06000452 RID: 1106 RVA: 0x000066EE File Offset: 0x000048EE
	[Obsolete("Use 'value' instead")]
	public Color color
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

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x06000453 RID: 1107 RVA: 0x00025D14 File Offset: 0x00023F14
	// (set) Token: 0x06000454 RID: 1108 RVA: 0x00025D90 File Offset: 0x00023F90
	public Color value
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mWidget != null)
			{
				return this.mWidget.color;
			}
			if (this.mLight != null)
			{
				return this.mLight.color;
			}
			if (this.mMat != null)
			{
				return this.mMat.color;
			}
			return Color.black;
		}
		set
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mWidget != null)
			{
				this.mWidget.color = value;
			}
			if (this.mMat != null)
			{
				this.mMat.color = value;
			}
			if (this.mLight != null)
			{
				this.mLight.color = value;
				this.mLight.enabled = value.r + value.g + value.b > 0.01f;
			}
		}
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x000066F7 File Offset: 0x000048F7
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Color.Lerp(this.from, this.to, factor);
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x00025E30 File Offset: 0x00024030
	public static TweenColor Begin(GameObject go, float duration, Color color)
	{
		TweenColor tweenColor = UITweener.Begin<TweenColor>(go, duration);
		tweenColor.from = tweenColor.value;
		tweenColor.to = color;
		if (duration <= 0f)
		{
			tweenColor.Sample(1f, true);
			tweenColor.enabled = false;
		}
		return tweenColor;
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x00006711 File Offset: 0x00004911
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x0000671F File Offset: 0x0000491F
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x0000672D File Offset: 0x0000492D
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x0000673B File Offset: 0x0000493B
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x0400034A RID: 842
	public Color from = Color.white;

	// Token: 0x0400034B RID: 843
	public Color to = Color.white;

	// Token: 0x0400034C RID: 844
	private bool mCached;

	// Token: 0x0400034D RID: 845
	private UIWidget mWidget;

	// Token: 0x0400034E RID: 846
	private Material mMat;

	// Token: 0x0400034F RID: 847
	private Light mLight;
}
