using System;
using UnityEngine;

// Token: 0x0200008F RID: 143
[RequireComponent(typeof(Camera))]
[AddComponentMenu("NGUI/Tween/Tween Field of View")]
public class TweenFOV : UITweener
{
	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x0600045C RID: 1116 RVA: 0x00006767 File Offset: 0x00004967
	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = base.camera;
			}
			return this.mCam;
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x0600045D RID: 1117 RVA: 0x0000678C File Offset: 0x0000498C
	// (set) Token: 0x0600045E RID: 1118 RVA: 0x00006794 File Offset: 0x00004994
	[Obsolete("Use 'value' instead")]
	public float fov
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

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x0600045F RID: 1119 RVA: 0x0000679D File Offset: 0x0000499D
	// (set) Token: 0x06000460 RID: 1120 RVA: 0x000067AA File Offset: 0x000049AA
	public float value
	{
		get
		{
			return this.cachedCamera.fieldOfView;
		}
		set
		{
			this.cachedCamera.fieldOfView = value;
		}
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x000067B8 File Offset: 0x000049B8
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x00025E78 File Offset: 0x00024078
	public static TweenFOV Begin(GameObject go, float duration, float to)
	{
		TweenFOV tweenFOV = UITweener.Begin<TweenFOV>(go, duration);
		tweenFOV.from = tweenFOV.value;
		tweenFOV.to = to;
		if (duration <= 0f)
		{
			tweenFOV.Sample(1f, true);
			tweenFOV.enabled = false;
		}
		return tweenFOV;
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x000067D7 File Offset: 0x000049D7
	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x000067E5 File Offset: 0x000049E5
	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x000067F3 File Offset: 0x000049F3
	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x00006801 File Offset: 0x00004A01
	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}

	// Token: 0x04000350 RID: 848
	public float from = 45f;

	// Token: 0x04000351 RID: 849
	public float to = 45f;

	// Token: 0x04000352 RID: 850
	private Camera mCam;
}
