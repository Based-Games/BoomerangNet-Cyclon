using System;
using UnityEngine;

// Token: 0x0200008D RID: 141
[AddComponentMenu("NGUI/Tween/Tween Alpha")]
[RequireComponent(typeof(UIRect))]
public class TweenAlpha : UITweener
{
	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x06000446 RID: 1094 RVA: 0x00006641 File Offset: 0x00004841
	public UIRect cachedRect
	{
		get
		{
			if (this.mRect == null)
			{
				this.mRect = base.GetComponent<UIRect>();
			}
			return this.mRect;
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06000447 RID: 1095 RVA: 0x00006666 File Offset: 0x00004866
	// (set) Token: 0x06000448 RID: 1096 RVA: 0x0000666E File Offset: 0x0000486E
	[Obsolete("Use 'value' instead")]
	public float alpha
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

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x06000449 RID: 1097 RVA: 0x00006677 File Offset: 0x00004877
	// (set) Token: 0x0600044A RID: 1098 RVA: 0x00006684 File Offset: 0x00004884
	public float value
	{
		get
		{
			return this.cachedRect.alpha;
		}
		set
		{
			this.cachedRect.alpha = value;
		}
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x00006692 File Offset: 0x00004892
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Mathf.Lerp(this.from, this.to, factor);
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x00025C80 File Offset: 0x00023E80
	public static TweenAlpha Begin(GameObject go, float duration, float alpha)
	{
		TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(go, duration);
		tweenAlpha.from = tweenAlpha.value;
		tweenAlpha.to = alpha;
		if (duration <= 0f)
		{
			tweenAlpha.Sample(1f, true);
			tweenAlpha.enabled = false;
		}
		return tweenAlpha;
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x000066AC File Offset: 0x000048AC
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x000066BA File Offset: 0x000048BA
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x04000347 RID: 839
	[Range(0f, 1f)]
	public float from = 1f;

	// Token: 0x04000348 RID: 840
	[Range(0f, 1f)]
	public float to = 1f;

	// Token: 0x04000349 RID: 841
	private UIRect mRect;
}
