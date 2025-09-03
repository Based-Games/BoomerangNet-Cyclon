using System;
using UnityEngine;

// Token: 0x02000096 RID: 150
[AddComponentMenu("NGUI/Tween/Tween Volume")]
public class TweenVolume : UITweener
{
	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x060004A6 RID: 1190 RVA: 0x0002639C File Offset: 0x0002459C
	public AudioSource audioSource
	{
		get
		{
			if (this.mSource == null)
			{
				this.mSource = base.audio;
				if (this.mSource == null)
				{
					this.mSource = base.GetComponentInChildren<AudioSource>();
					if (this.mSource == null)
					{
						Debug.LogError("TweenVolume needs an AudioSource to work with", this);
						base.enabled = false;
					}
				}
			}
			return this.mSource;
		}
	}

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x060004A7 RID: 1191 RVA: 0x00006B81 File Offset: 0x00004D81
	// (set) Token: 0x060004A8 RID: 1192 RVA: 0x00006B89 File Offset: 0x00004D89
	[Obsolete("Use 'value' instead")]
	public float volume
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

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x060004A9 RID: 1193 RVA: 0x00006B92 File Offset: 0x00004D92
	// (set) Token: 0x060004AA RID: 1194 RVA: 0x00006BBA File Offset: 0x00004DBA
	public float value
	{
		get
		{
			return (!(this.audioSource != null)) ? 0f : this.mSource.volume;
		}
		set
		{
			if (this.audioSource != null)
			{
				this.mSource.volume = value;
			}
		}
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x00006BD9 File Offset: 0x00004DD9
	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = this.from * (1f - factor) + this.to * factor;
		this.mSource.enabled = this.mSource.volume > 0.01f;
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x0002640C File Offset: 0x0002460C
	public static TweenVolume Begin(GameObject go, float duration, float targetVolume)
	{
		TweenVolume tweenVolume = UITweener.Begin<TweenVolume>(go, duration);
		tweenVolume.from = tweenVolume.value;
		tweenVolume.to = targetVolume;
		return tweenVolume;
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x00006C15 File Offset: 0x00004E15
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x00006C23 File Offset: 0x00004E23
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	// Token: 0x0400036D RID: 877
	[Range(0f, 1f)]
	public float from = 1f;

	// Token: 0x0400036E RID: 878
	[Range(0f, 1f)]
	public float to = 1f;

	// Token: 0x0400036F RID: 879
	private AudioSource mSource;
}
