using System;
using UnityEngine;

// Token: 0x02000091 RID: 145
[AddComponentMenu("NGUI/Tween/Tween Orthographic Size")]
[RequireComponent(typeof(Camera))]
public class TweenOrthoSize : UITweener
{
	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06000474 RID: 1140 RVA: 0x000068CE File Offset: 0x00004ACE
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

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x06000475 RID: 1141 RVA: 0x000068F3 File Offset: 0x00004AF3
	// (set) Token: 0x06000476 RID: 1142 RVA: 0x000068FB File Offset: 0x00004AFB
	[Obsolete("Use 'value' instead")]
	public float orthoSize
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

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x06000477 RID: 1143 RVA: 0x00006904 File Offset: 0x00004B04
	// (set) Token: 0x06000478 RID: 1144 RVA: 0x00006911 File Offset: 0x00004B11
	public float value
	{
		get
		{
			return this.cachedCamera.orthographicSize;
		}
		set
		{
			this.cachedCamera.orthographicSize = value;
		}
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x0000691F File Offset: 0x00004B1F
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x00025F90 File Offset: 0x00024190
	public static TweenOrthoSize Begin(GameObject go, float duration, float to)
	{
		TweenOrthoSize tweenOrthoSize = UITweener.Begin<TweenOrthoSize>(go, duration);
		tweenOrthoSize.from = tweenOrthoSize.value;
		tweenOrthoSize.to = to;
		if (duration <= 0f)
		{
			tweenOrthoSize.Sample(1f, true);
			tweenOrthoSize.enabled = false;
		}
		return tweenOrthoSize;
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x0000693E File Offset: 0x00004B3E
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x0000694C File Offset: 0x00004B4C
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x04000358 RID: 856
	public float from = 1f;

	// Token: 0x04000359 RID: 857
	public float to = 1f;

	// Token: 0x0400035A RID: 858
	private Camera mCam;
}
