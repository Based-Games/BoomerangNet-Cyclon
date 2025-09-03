using System;
using UnityEngine;

// Token: 0x0200024E RID: 590
[AddComponentMenu("2D Toolkit/Sprite/tk2dAnimatedSprite (Obsolete)")]
public class tk2dAnimatedSprite : tk2dSprite
{
	// Token: 0x17000287 RID: 647
	// (get) Token: 0x060010DD RID: 4317 RVA: 0x0000E5B9 File Offset: 0x0000C7B9
	public tk2dSpriteAnimator Animator
	{
		get
		{
			this.CheckAddAnimatorInternal();
			return this._animator;
		}
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x00077CF8 File Offset: 0x00075EF8
	private void CheckAddAnimatorInternal()
	{
		if (this._animator == null)
		{
			this._animator = base.gameObject.GetComponent<tk2dSpriteAnimator>();
			if (this._animator == null)
			{
				this._animator = base.gameObject.AddComponent<tk2dSpriteAnimator>();
				this._animator.Library = this.anim;
				this._animator.DefaultClipId = this.clipId;
				this._animator.playAutomatically = this.playAutomatically;
			}
		}
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x0000E5C7 File Offset: 0x0000C7C7
	protected override bool NeedBoxCollider()
	{
		return this.createCollider;
	}

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x060010E0 RID: 4320 RVA: 0x0000E5CF File Offset: 0x0000C7CF
	// (set) Token: 0x060010E1 RID: 4321 RVA: 0x0000E5DC File Offset: 0x0000C7DC
	public tk2dSpriteAnimation Library
	{
		get
		{
			return this.Animator.Library;
		}
		set
		{
			this.Animator.Library = value;
		}
	}

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x060010E2 RID: 4322 RVA: 0x0000E5EA File Offset: 0x0000C7EA
	// (set) Token: 0x060010E3 RID: 4323 RVA: 0x0000E5F7 File Offset: 0x0000C7F7
	public int DefaultClipId
	{
		get
		{
			return this.Animator.DefaultClipId;
		}
		set
		{
			this.Animator.DefaultClipId = value;
		}
	}

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x060010E4 RID: 4324 RVA: 0x0000E605 File Offset: 0x0000C805
	// (set) Token: 0x060010E5 RID: 4325 RVA: 0x0000E60C File Offset: 0x0000C80C
	public static bool g_paused
	{
		get
		{
			return tk2dSpriteAnimator.g_Paused;
		}
		set
		{
			tk2dSpriteAnimator.g_Paused = value;
		}
	}

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x060010E6 RID: 4326 RVA: 0x0000E614 File Offset: 0x0000C814
	// (set) Token: 0x060010E7 RID: 4327 RVA: 0x0000E621 File Offset: 0x0000C821
	public bool Paused
	{
		get
		{
			return this.Animator.Paused;
		}
		set
		{
			this.Animator.Paused = value;
		}
	}

	// Token: 0x060010E8 RID: 4328 RVA: 0x00077D7C File Offset: 0x00075F7C
	private void ProxyCompletedHandler(tk2dSpriteAnimator anim, tk2dSpriteAnimationClip clip)
	{
		if (this.animationCompleteDelegate != null)
		{
			int num = -1;
			tk2dSpriteAnimationClip[] array = ((!(anim.Library != null)) ? null : anim.Library.clips);
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == clip)
					{
						num = i;
						break;
					}
				}
			}
			this.animationCompleteDelegate(this, num);
		}
	}

	// Token: 0x060010E9 RID: 4329 RVA: 0x0000E62F File Offset: 0x0000C82F
	private void ProxyEventTriggeredHandler(tk2dSpriteAnimator anim, tk2dSpriteAnimationClip clip, int frame)
	{
		if (this.animationEventDelegate != null)
		{
			this.animationEventDelegate(this, clip, clip.frames[frame], frame);
		}
	}

	// Token: 0x060010EA RID: 4330 RVA: 0x0000E652 File Offset: 0x0000C852
	private void OnEnable()
	{
		this.Animator.AnimationCompleted = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.ProxyCompletedHandler);
		this.Animator.AnimationEventTriggered = new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int>(this.ProxyEventTriggeredHandler);
	}

	// Token: 0x060010EB RID: 4331 RVA: 0x0000E682 File Offset: 0x0000C882
	private void OnDisable()
	{
		this.Animator.AnimationCompleted = null;
		this.Animator.AnimationEventTriggered = null;
	}

	// Token: 0x060010EC RID: 4332 RVA: 0x0000E69C File Offset: 0x0000C89C
	private void Start()
	{
		this.CheckAddAnimatorInternal();
	}

	// Token: 0x060010ED RID: 4333 RVA: 0x00077DF0 File Offset: 0x00075FF0
	public static tk2dAnimatedSprite AddComponent(GameObject go, tk2dSpriteAnimation anim, int clipId)
	{
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip = anim.clips[clipId];
		tk2dAnimatedSprite tk2dAnimatedSprite = go.AddComponent<tk2dAnimatedSprite>();
		tk2dAnimatedSprite.SetSprite(tk2dSpriteAnimationClip.frames[0].spriteCollection, tk2dSpriteAnimationClip.frames[0].spriteId);
		tk2dAnimatedSprite.anim = anim;
		return tk2dAnimatedSprite;
	}

	// Token: 0x060010EE RID: 4334 RVA: 0x0000E6A4 File Offset: 0x0000C8A4
	public void Play()
	{
		if (this.Animator.DefaultClip != null)
		{
			this.Animator.Play(this.Animator.DefaultClip);
		}
	}

	// Token: 0x060010EF RID: 4335 RVA: 0x0000E6CC File Offset: 0x0000C8CC
	public void Play(float clipStartTime)
	{
		if (this.Animator.DefaultClip != null)
		{
			this.Animator.PlayFrom(this.Animator.DefaultClip, clipStartTime);
		}
	}

	// Token: 0x060010F0 RID: 4336 RVA: 0x0000E6F5 File Offset: 0x0000C8F5
	public void PlayFromFrame(int frame)
	{
		if (this.Animator.DefaultClip != null)
		{
			this.Animator.PlayFromFrame(this.Animator.DefaultClip, frame);
		}
	}

	// Token: 0x060010F1 RID: 4337 RVA: 0x0000E71E File Offset: 0x0000C91E
	public void Play(string name)
	{
		this.Animator.Play(name);
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x0000E72C File Offset: 0x0000C92C
	public void PlayFromFrame(string name, int frame)
	{
		this.Animator.PlayFromFrame(name, frame);
	}

	// Token: 0x060010F3 RID: 4339 RVA: 0x0000E73B File Offset: 0x0000C93B
	public void Play(string name, float clipStartTime)
	{
		this.Animator.PlayFrom(name, clipStartTime);
	}

	// Token: 0x060010F4 RID: 4340 RVA: 0x0000E74A File Offset: 0x0000C94A
	public void Play(tk2dSpriteAnimationClip clip, float clipStartTime)
	{
		this.Animator.PlayFrom(clip, clipStartTime);
	}

	// Token: 0x060010F5 RID: 4341 RVA: 0x0000E759 File Offset: 0x0000C959
	public void Play(tk2dSpriteAnimationClip clip, float clipStartTime, float overrideFps)
	{
		this.Animator.Play(clip, clipStartTime, overrideFps);
	}

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x060010F6 RID: 4342 RVA: 0x0000E769 File Offset: 0x0000C969
	public tk2dSpriteAnimationClip CurrentClip
	{
		get
		{
			return this.Animator.CurrentClip;
		}
	}

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x060010F7 RID: 4343 RVA: 0x0000E776 File Offset: 0x0000C976
	public float ClipTimeSeconds
	{
		get
		{
			return this.Animator.ClipTimeSeconds;
		}
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x060010F8 RID: 4344 RVA: 0x0000E783 File Offset: 0x0000C983
	// (set) Token: 0x060010F9 RID: 4345 RVA: 0x0000E790 File Offset: 0x0000C990
	public float ClipFps
	{
		get
		{
			return this.Animator.ClipFps;
		}
		set
		{
			this.Animator.ClipFps = value;
		}
	}

	// Token: 0x060010FA RID: 4346 RVA: 0x0000E79E File Offset: 0x0000C99E
	public void Stop()
	{
		this.Animator.Stop();
	}

	// Token: 0x060010FB RID: 4347 RVA: 0x0000E7AB File Offset: 0x0000C9AB
	public void StopAndResetFrame()
	{
		this.Animator.StopAndResetFrame();
	}

	// Token: 0x060010FC RID: 4348 RVA: 0x0000E7B8 File Offset: 0x0000C9B8
	[Obsolete]
	public bool isPlaying()
	{
		return this.Animator.Playing;
	}

	// Token: 0x060010FD RID: 4349 RVA: 0x0000E7B8 File Offset: 0x0000C9B8
	public bool IsPlaying(string name)
	{
		return this.Animator.Playing;
	}

	// Token: 0x060010FE RID: 4350 RVA: 0x0000E7C5 File Offset: 0x0000C9C5
	public bool IsPlaying(tk2dSpriteAnimationClip clip)
	{
		return this.Animator.IsPlaying(clip);
	}

	// Token: 0x1700028F RID: 655
	// (get) Token: 0x060010FF RID: 4351 RVA: 0x0000E7B8 File Offset: 0x0000C9B8
	public bool Playing
	{
		get
		{
			return this.Animator.Playing;
		}
	}

	// Token: 0x06001100 RID: 4352 RVA: 0x0000E7D3 File Offset: 0x0000C9D3
	public int GetClipIdByName(string name)
	{
		return this.Animator.GetClipIdByName(name);
	}

	// Token: 0x06001101 RID: 4353 RVA: 0x0000E7E1 File Offset: 0x0000C9E1
	public tk2dSpriteAnimationClip GetClipByName(string name)
	{
		return this.Animator.GetClipByName(name);
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06001102 RID: 4354 RVA: 0x0000E7EF File Offset: 0x0000C9EF
	public static float DefaultFps
	{
		get
		{
			return tk2dSpriteAnimator.DefaultFps;
		}
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x0000E7F6 File Offset: 0x0000C9F6
	public void Pause()
	{
		this.Animator.Pause();
	}

	// Token: 0x06001104 RID: 4356 RVA: 0x0000E803 File Offset: 0x0000CA03
	public void Resume()
	{
		this.Animator.Resume();
	}

	// Token: 0x06001105 RID: 4357 RVA: 0x0000E810 File Offset: 0x0000CA10
	public void SetFrame(int currFrame)
	{
		this.Animator.SetFrame(currFrame);
	}

	// Token: 0x06001106 RID: 4358 RVA: 0x0000E81E File Offset: 0x0000CA1E
	public void SetFrame(int currFrame, bool triggerEvent)
	{
		this.Animator.SetFrame(currFrame, triggerEvent);
	}

	// Token: 0x06001107 RID: 4359 RVA: 0x0000E82D File Offset: 0x0000CA2D
	public void UpdateAnimation(float deltaTime)
	{
		this.Animator.UpdateAnimation(deltaTime);
	}

	// Token: 0x040012AE RID: 4782
	[SerializeField]
	private tk2dSpriteAnimator _animator;

	// Token: 0x040012AF RID: 4783
	[SerializeField]
	private tk2dSpriteAnimation anim;

	// Token: 0x040012B0 RID: 4784
	[SerializeField]
	private int clipId;

	// Token: 0x040012B1 RID: 4785
	public bool playAutomatically;

	// Token: 0x040012B2 RID: 4786
	public bool createCollider;

	// Token: 0x040012B3 RID: 4787
	public tk2dAnimatedSprite.AnimationCompleteDelegate animationCompleteDelegate;

	// Token: 0x040012B4 RID: 4788
	public tk2dAnimatedSprite.AnimationEventDelegate animationEventDelegate;

	// Token: 0x0200024F RID: 591
	// (Invoke) Token: 0x06001109 RID: 4361
	public delegate void AnimationCompleteDelegate(tk2dAnimatedSprite sprite, int clipId);

	// Token: 0x02000250 RID: 592
	// (Invoke) Token: 0x0600110D RID: 4365
	public delegate void AnimationEventDelegate(tk2dAnimatedSprite sprite, tk2dSpriteAnimationClip clip, tk2dSpriteAnimationFrame frame, int frameNum);
}
