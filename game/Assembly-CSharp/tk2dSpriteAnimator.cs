using System;
using UnityEngine;

// Token: 0x0200025C RID: 604
[AddComponentMenu("2D Toolkit/Sprite/tk2dSpriteAnimator")]
public class tk2dSpriteAnimator : MonoBehaviour
{
	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x060011A7 RID: 4519 RVA: 0x0000EF3D File Offset: 0x0000D13D
	// (set) Token: 0x060011A8 RID: 4520 RVA: 0x0000EF4C File Offset: 0x0000D14C
	public static bool g_Paused
	{
		get
		{
			return (tk2dSpriteAnimator.globalState & tk2dSpriteAnimator.State.Paused) != tk2dSpriteAnimator.State.Init;
		}
		set
		{
			tk2dSpriteAnimator.globalState = ((!value) ? tk2dSpriteAnimator.State.Init : tk2dSpriteAnimator.State.Paused);
		}
	}

	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x060011A9 RID: 4521 RVA: 0x0000EF60 File Offset: 0x0000D160
	// (set) Token: 0x060011AA RID: 4522 RVA: 0x0000EF70 File Offset: 0x0000D170
	public bool Paused
	{
		get
		{
			return (this.state & tk2dSpriteAnimator.State.Paused) != tk2dSpriteAnimator.State.Init;
		}
		set
		{
			if (value)
			{
				this.state |= tk2dSpriteAnimator.State.Paused;
			}
			else
			{
				this.state &= (tk2dSpriteAnimator.State)(-3);
			}
		}
	}

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x060011AB RID: 4523 RVA: 0x0000EF9A File Offset: 0x0000D19A
	// (set) Token: 0x060011AC RID: 4524 RVA: 0x0000EFA2 File Offset: 0x0000D1A2
	public tk2dSpriteAnimation Library
	{
		get
		{
			return this.library;
		}
		set
		{
			this.library = value;
		}
	}

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x060011AD RID: 4525 RVA: 0x0000EFAB File Offset: 0x0000D1AB
	// (set) Token: 0x060011AE RID: 4526 RVA: 0x0000EFB3 File Offset: 0x0000D1B3
	public int DefaultClipId
	{
		get
		{
			return this.defaultClipId;
		}
		set
		{
			this.defaultClipId = value;
		}
	}

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x060011AF RID: 4527 RVA: 0x0000EFBC File Offset: 0x0000D1BC
	public tk2dSpriteAnimationClip DefaultClip
	{
		get
		{
			return this.GetClipById(this.defaultClipId);
		}
	}

	// Token: 0x060011B0 RID: 4528 RVA: 0x0000EFCA File Offset: 0x0000D1CA
	private void OnEnable()
	{
		if (this.Sprite == null)
		{
			base.enabled = false;
		}
	}

	// Token: 0x060011B1 RID: 4529 RVA: 0x0000EFE4 File Offset: 0x0000D1E4
	private void Start()
	{
		if (this.playAutomatically)
		{
			this.Play(this.DefaultClip);
		}
	}

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x060011B2 RID: 4530 RVA: 0x0000EFFD File Offset: 0x0000D1FD
	public virtual tk2dBaseSprite Sprite
	{
		get
		{
			if (this._sprite == null)
			{
				this._sprite = base.GetComponent<tk2dBaseSprite>();
				if (this._sprite == null)
				{
					Debug.LogError("Sprite not found attached to tk2dSpriteAnimator.");
				}
			}
			return this._sprite;
		}
	}

	// Token: 0x060011B3 RID: 4531 RVA: 0x0007B368 File Offset: 0x00079568
	public static tk2dSpriteAnimator AddComponent(GameObject go, tk2dSpriteAnimation anim, int clipId)
	{
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip = anim.clips[clipId];
		tk2dSpriteAnimator tk2dSpriteAnimator = go.AddComponent<tk2dSpriteAnimator>();
		tk2dSpriteAnimator.Library = anim;
		tk2dSpriteAnimator.SetSprite(tk2dSpriteAnimationClip.frames[0].spriteCollection, tk2dSpriteAnimationClip.frames[0].spriteId);
		return tk2dSpriteAnimator;
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x0007B3B0 File Offset: 0x000795B0
	private tk2dSpriteAnimationClip GetClipByNameVerbose(string name)
	{
		if (this.library == null)
		{
			Debug.LogError("Library not set");
			return null;
		}
		tk2dSpriteAnimationClip clipByName = this.library.GetClipByName(name);
		if (clipByName == null)
		{
			Debug.LogError("Unable to find clip '" + name + "' in library");
			return null;
		}
		return clipByName;
	}

	// Token: 0x060011B5 RID: 4533 RVA: 0x0000F03D File Offset: 0x0000D23D
	public void Play()
	{
		if (this.currentClip == null)
		{
			this.currentClip = this.DefaultClip;
		}
		this.Play(this.currentClip);
	}

	// Token: 0x060011B6 RID: 4534 RVA: 0x0000F062 File Offset: 0x0000D262
	public void Play(string name)
	{
		this.Play(this.GetClipByNameVerbose(name));
	}

	// Token: 0x060011B7 RID: 4535 RVA: 0x0000F071 File Offset: 0x0000D271
	public void Play(tk2dSpriteAnimationClip clip)
	{
		this.Play(clip, 0f, tk2dSpriteAnimator.DefaultFps);
	}

	// Token: 0x060011B8 RID: 4536 RVA: 0x0000F084 File Offset: 0x0000D284
	public void PlayFromFrame(int frame)
	{
		if (this.currentClip == null)
		{
			this.currentClip = this.DefaultClip;
		}
		this.PlayFromFrame(this.currentClip, frame);
	}

	// Token: 0x060011B9 RID: 4537 RVA: 0x0000F0AA File Offset: 0x0000D2AA
	public void PlayFromFrame(string name, int frame)
	{
		this.PlayFromFrame(this.GetClipByNameVerbose(name), frame);
	}

	// Token: 0x060011BA RID: 4538 RVA: 0x0000F0BA File Offset: 0x0000D2BA
	public void PlayFromFrame(tk2dSpriteAnimationClip clip, int frame)
	{
		this.PlayFrom(clip, ((float)frame + 0.001f) / clip.fps);
	}

	// Token: 0x060011BB RID: 4539 RVA: 0x0000F0D2 File Offset: 0x0000D2D2
	public void PlayFrom(float clipStartTime)
	{
		if (this.currentClip == null)
		{
			this.currentClip = this.DefaultClip;
		}
		this.PlayFrom(this.currentClip, clipStartTime);
	}

	// Token: 0x060011BC RID: 4540 RVA: 0x0007B408 File Offset: 0x00079608
	public void PlayFrom(string name, float clipStartTime)
	{
		tk2dSpriteAnimationClip tk2dSpriteAnimationClip = ((!this.library) ? null : this.library.GetClipByName(name));
		if (tk2dSpriteAnimationClip == null)
		{
			this.ClipNameError(name);
		}
		else
		{
			this.PlayFrom(tk2dSpriteAnimationClip, clipStartTime);
		}
	}

	// Token: 0x060011BD RID: 4541 RVA: 0x0000F0F8 File Offset: 0x0000D2F8
	public void PlayFrom(tk2dSpriteAnimationClip clip, float clipStartTime)
	{
		this.Play(clip, clipStartTime, tk2dSpriteAnimator.DefaultFps);
	}

	// Token: 0x060011BE RID: 4542 RVA: 0x0007B454 File Offset: 0x00079654
	public void Play(tk2dSpriteAnimationClip clip, float clipStartTime, float overrideFps)
	{
		if (clip != null)
		{
			float num = ((overrideFps <= 0f) ? clip.fps : overrideFps);
			bool flag = clipStartTime == 0f && this.IsPlaying(clip);
			if (flag)
			{
				this.clipFps = num;
			}
			else
			{
				this.state |= tk2dSpriteAnimator.State.Playing;
				this.currentClip = clip;
				this.clipFps = num;
				if (this.currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.Single || this.currentClip.frames == null)
				{
					this.WarpClipToLocalTime(this.currentClip, 0f);
					this.state &= (tk2dSpriteAnimator.State)(-2);
				}
				else if (this.currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.RandomFrame || this.currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.RandomLoop)
				{
					int num2 = UnityEngine.Random.Range(0, this.currentClip.frames.Length);
					this.WarpClipToLocalTime(this.currentClip, (float)num2);
					if (this.currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.RandomFrame)
					{
						this.previousFrame = -1;
						this.state &= (tk2dSpriteAnimator.State)(-2);
					}
				}
				else
				{
					float num3 = clipStartTime * this.clipFps;
					if (this.currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.Once && num3 >= this.clipFps * (float)this.currentClip.frames.Length)
					{
						this.WarpClipToLocalTime(this.currentClip, (float)(this.currentClip.frames.Length - 1));
						this.state &= (tk2dSpriteAnimator.State)(-2);
					}
					else
					{
						this.WarpClipToLocalTime(this.currentClip, num3);
						this.clipTime = num3;
					}
				}
			}
		}
		else
		{
			Debug.LogError("Calling clip.Play() with a null clip");
			this.OnAnimationCompleted();
			this.state &= (tk2dSpriteAnimator.State)(-2);
		}
	}

	// Token: 0x060011BF RID: 4543 RVA: 0x0000F107 File Offset: 0x0000D307
	public void Stop()
	{
		this.state &= (tk2dSpriteAnimator.State)(-2);
	}

	// Token: 0x060011C0 RID: 4544 RVA: 0x0000F118 File Offset: 0x0000D318
	public void StopAndResetFrame()
	{
		if (this.currentClip != null)
		{
			this.SetSprite(this.currentClip.frames[0].spriteCollection, this.currentClip.frames[0].spriteId);
		}
		this.Stop();
	}

	// Token: 0x060011C1 RID: 4545 RVA: 0x0000F155 File Offset: 0x0000D355
	public bool IsPlaying(string name)
	{
		return this.Playing && this.CurrentClip != null && this.CurrentClip.name == name;
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x0000F181 File Offset: 0x0000D381
	public bool IsPlaying(tk2dSpriteAnimationClip clip)
	{
		return this.Playing && this.CurrentClip != null && this.CurrentClip == clip;
	}

	// Token: 0x170002AB RID: 683
	// (get) Token: 0x060011C3 RID: 4547 RVA: 0x0000F1A5 File Offset: 0x0000D3A5
	public bool Playing
	{
		get
		{
			return (this.state & tk2dSpriteAnimator.State.Playing) != tk2dSpriteAnimator.State.Init;
		}
	}

	// Token: 0x170002AC RID: 684
	// (get) Token: 0x060011C4 RID: 4548 RVA: 0x0000F1B5 File Offset: 0x0000D3B5
	public tk2dSpriteAnimationClip CurrentClip
	{
		get
		{
			return this.currentClip;
		}
	}

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x060011C5 RID: 4549 RVA: 0x0000F1BD File Offset: 0x0000D3BD
	public float ClipTimeSeconds
	{
		get
		{
			return (this.clipFps <= 0f) ? (this.clipTime / this.currentClip.fps) : (this.clipTime / this.clipFps);
		}
	}

	// Token: 0x170002AE RID: 686
	// (get) Token: 0x060011C6 RID: 4550 RVA: 0x0000F1F3 File Offset: 0x0000D3F3
	// (set) Token: 0x060011C7 RID: 4551 RVA: 0x0000F1FB File Offset: 0x0000D3FB
	public float ClipFps
	{
		get
		{
			return this.clipFps;
		}
		set
		{
			if (this.currentClip != null)
			{
				this.clipFps = ((value <= 0f) ? this.currentClip.fps : value);
			}
		}
	}

	// Token: 0x060011C8 RID: 4552 RVA: 0x0000F22A File Offset: 0x0000D42A
	public tk2dSpriteAnimationClip GetClipById(int id)
	{
		if (this.library == null)
		{
			return null;
		}
		return this.library.GetClipById(id);
	}

	// Token: 0x170002AF RID: 687
	// (get) Token: 0x060011C9 RID: 4553 RVA: 0x0000F24B File Offset: 0x0000D44B
	public static float DefaultFps
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x0000F252 File Offset: 0x0000D452
	public int GetClipIdByName(string name)
	{
		return (!this.library) ? (-1) : this.library.GetClipIdByName(name);
	}

	// Token: 0x060011CB RID: 4555 RVA: 0x0000F276 File Offset: 0x0000D476
	public tk2dSpriteAnimationClip GetClipByName(string name)
	{
		return (!this.library) ? null : this.library.GetClipByName(name);
	}

	// Token: 0x060011CC RID: 4556 RVA: 0x0000F29A File Offset: 0x0000D49A
	public void Pause()
	{
		this.state |= tk2dSpriteAnimator.State.Paused;
	}

	// Token: 0x060011CD RID: 4557 RVA: 0x0000F2AA File Offset: 0x0000D4AA
	public void Resume()
	{
		this.state &= (tk2dSpriteAnimator.State)(-3);
	}

	// Token: 0x060011CE RID: 4558 RVA: 0x0000F2BB File Offset: 0x0000D4BB
	public void SetFrame(int currFrame)
	{
		this.SetFrame(currFrame, true);
	}

	// Token: 0x060011CF RID: 4559 RVA: 0x0007B618 File Offset: 0x00079818
	public void SetFrame(int currFrame, bool triggerEvent)
	{
		if (this.currentClip == null)
		{
			this.currentClip = this.DefaultClip;
		}
		if (this.currentClip != null)
		{
			int num = currFrame % this.currentClip.frames.Length;
			this.SetFrameInternal(num);
			if (triggerEvent && this.currentClip.frames.Length > 0 && currFrame >= 0)
			{
				this.ProcessEvents(num - 1, num, 1);
			}
		}
	}

	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x060011D0 RID: 4560 RVA: 0x0007B68C File Offset: 0x0007988C
	public int CurrentFrame
	{
		get
		{
			switch (this.currentClip.wrapMode)
			{
			case tk2dSpriteAnimationClip.WrapMode.Loop:
			case tk2dSpriteAnimationClip.WrapMode.RandomLoop:
				break;
			case tk2dSpriteAnimationClip.WrapMode.LoopSection:
			{
				int num = (int)this.clipTime;
				int num2 = this.currentClip.loopStart + (num - this.currentClip.loopStart) % (this.currentClip.frames.Length - this.currentClip.loopStart);
				if (num >= this.currentClip.loopStart)
				{
					return num2;
				}
				return num;
			}
			case tk2dSpriteAnimationClip.WrapMode.Once:
				return Mathf.Min((int)this.clipTime, this.currentClip.frames.Length);
			case tk2dSpriteAnimationClip.WrapMode.PingPong:
			{
				int num3 = (int)this.clipTime % (this.currentClip.frames.Length + this.currentClip.frames.Length - 2);
				if (num3 >= this.currentClip.frames.Length)
				{
					num3 = 2 * this.currentClip.frames.Length - 2 - num3;
				}
				return num3;
			}
			case tk2dSpriteAnimationClip.WrapMode.RandomFrame:
				goto IL_FF;
			default:
				goto IL_FF;
			}
			IL_49:
			return (int)this.clipTime % this.currentClip.frames.Length;
			IL_FF:
			Debug.LogError("Unhandled clip wrap mode");
			goto IL_49;
		}
	}

	// Token: 0x060011D1 RID: 4561 RVA: 0x0007B7A8 File Offset: 0x000799A8
	public void UpdateAnimation(float deltaTime)
	{
		tk2dSpriteAnimator.State state = this.state | tk2dSpriteAnimator.globalState;
		if (state != tk2dSpriteAnimator.State.Playing)
		{
			return;
		}
		this.clipTime += deltaTime * this.clipFps;
		int num = this.previousFrame;
		switch (this.currentClip.wrapMode)
		{
		case tk2dSpriteAnimationClip.WrapMode.Loop:
		case tk2dSpriteAnimationClip.WrapMode.RandomLoop:
		{
			int num2 = (int)this.clipTime % this.currentClip.frames.Length;
			this.SetFrameInternal(num2);
			if (num2 < num)
			{
				this.ProcessEvents(num, this.currentClip.frames.Length - 1, 1);
				this.ProcessEvents(-1, num2, 1);
			}
			else
			{
				this.ProcessEvents(num, num2, 1);
			}
			break;
		}
		case tk2dSpriteAnimationClip.WrapMode.LoopSection:
		{
			int num3 = (int)this.clipTime;
			int num4 = this.currentClip.loopStart + (num3 - this.currentClip.loopStart) % (this.currentClip.frames.Length - this.currentClip.loopStart);
			if (num3 >= this.currentClip.loopStart)
			{
				this.SetFrameInternal(num4);
				num3 = num4;
				if (num < this.currentClip.loopStart)
				{
					this.ProcessEvents(num, this.currentClip.loopStart - 1, 1);
					this.ProcessEvents(this.currentClip.loopStart - 1, num3, 1);
				}
				else if (num3 < num)
				{
					this.ProcessEvents(num, this.currentClip.frames.Length - 1, 1);
					this.ProcessEvents(this.currentClip.loopStart - 1, num3, 1);
				}
				else
				{
					this.ProcessEvents(num, num3, 1);
				}
			}
			else
			{
				this.SetFrameInternal(num3);
				this.ProcessEvents(num, num3, 1);
			}
			break;
		}
		case tk2dSpriteAnimationClip.WrapMode.Once:
		{
			int num5 = (int)this.clipTime;
			if (num5 >= this.currentClip.frames.Length)
			{
				this.SetFrameInternal(this.currentClip.frames.Length - 1);
				this.state &= (tk2dSpriteAnimator.State)(-2);
				this.ProcessEvents(num, this.currentClip.frames.Length - 1, 1);
				this.OnAnimationCompleted();
			}
			else
			{
				this.SetFrameInternal(num5);
				this.ProcessEvents(num, num5, 1);
			}
			break;
		}
		case tk2dSpriteAnimationClip.WrapMode.PingPong:
		{
			int num6 = (int)this.clipTime % (this.currentClip.frames.Length + this.currentClip.frames.Length - 2);
			int num7 = 1;
			if (num6 >= this.currentClip.frames.Length)
			{
				num6 = 2 * this.currentClip.frames.Length - 2 - num6;
				num7 = -1;
			}
			if (num6 < num)
			{
				num7 = -1;
			}
			this.SetFrameInternal(num6);
			this.ProcessEvents(num, num6, num7);
			break;
		}
		}
	}

	// Token: 0x060011D2 RID: 4562 RVA: 0x0000F2C5 File Offset: 0x0000D4C5
	private void ClipNameError(string name)
	{
		Debug.LogError("Unable to find clip named '" + name + "' in library");
	}

	// Token: 0x060011D3 RID: 4563 RVA: 0x0000F2DC File Offset: 0x0000D4DC
	private void ClipIdError(int id)
	{
		Debug.LogError("Play - Invalid clip id '" + id.ToString() + "' in library");
	}

	// Token: 0x060011D4 RID: 4564 RVA: 0x0007BA58 File Offset: 0x00079C58
	private void WarpClipToLocalTime(tk2dSpriteAnimationClip clip, float time)
	{
		this.clipTime = time;
		int num = (int)this.clipTime % clip.frames.Length;
		tk2dSpriteAnimationFrame tk2dSpriteAnimationFrame = clip.frames[num];
		this.SetSprite(tk2dSpriteAnimationFrame.spriteCollection, tk2dSpriteAnimationFrame.spriteId);
		if (tk2dSpriteAnimationFrame.triggerEvent && this.AnimationEventTriggered != null)
		{
			this.AnimationEventTriggered(this, clip, num);
		}
		this.previousFrame = num;
	}

	// Token: 0x060011D5 RID: 4565 RVA: 0x0000F2F9 File Offset: 0x0000D4F9
	private void SetFrameInternal(int currFrame)
	{
		if (this.previousFrame != currFrame)
		{
			this.SetSprite(this.currentClip.frames[currFrame].spriteCollection, this.currentClip.frames[currFrame].spriteId);
			this.previousFrame = currFrame;
		}
	}

	// Token: 0x060011D6 RID: 4566 RVA: 0x0007BAC4 File Offset: 0x00079CC4
	private void ProcessEvents(int start, int last, int direction)
	{
		if (this.AnimationEventTriggered == null || start == last)
		{
			return;
		}
		int num = last + direction;
		tk2dSpriteAnimationFrame[] frames = this.currentClip.frames;
		for (int num2 = start + direction; num2 != num; num2 += direction)
		{
			if (frames[num2].triggerEvent && this.AnimationEventTriggered != null)
			{
				this.AnimationEventTriggered(this, this.currentClip, num2);
			}
		}
	}

	// Token: 0x060011D7 RID: 4567 RVA: 0x0000F338 File Offset: 0x0000D538
	private void OnAnimationCompleted()
	{
		this.previousFrame = -1;
		if (this.AnimationCompleted != null)
		{
			this.AnimationCompleted(this, this.currentClip);
		}
	}

	// Token: 0x060011D8 RID: 4568 RVA: 0x0000F35E File Offset: 0x0000D55E
	public virtual void LateUpdate()
	{
		this.UpdateAnimation(Time.deltaTime);
	}

	// Token: 0x060011D9 RID: 4569 RVA: 0x0000F36B File Offset: 0x0000D56B
	public virtual void SetSprite(tk2dSpriteCollectionData spriteCollection, int spriteId)
	{
		this.Sprite.SetSprite(spriteCollection, spriteId);
	}

	// Token: 0x04001309 RID: 4873
	[SerializeField]
	private tk2dSpriteAnimation library;

	// Token: 0x0400130A RID: 4874
	[SerializeField]
	private int defaultClipId;

	// Token: 0x0400130B RID: 4875
	public bool playAutomatically;

	// Token: 0x0400130C RID: 4876
	private static tk2dSpriteAnimator.State globalState;

	// Token: 0x0400130D RID: 4877
	private tk2dSpriteAnimationClip currentClip;

	// Token: 0x0400130E RID: 4878
	private float clipTime;

	// Token: 0x0400130F RID: 4879
	private float clipFps = -1f;

	// Token: 0x04001310 RID: 4880
	private int previousFrame = -1;

	// Token: 0x04001311 RID: 4881
	public Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip> AnimationCompleted;

	// Token: 0x04001312 RID: 4882
	public Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int> AnimationEventTriggered;

	// Token: 0x04001313 RID: 4883
	private tk2dSpriteAnimator.State state;

	// Token: 0x04001314 RID: 4884
	protected tk2dBaseSprite _sprite;

	// Token: 0x0200025D RID: 605
	private enum State
	{
		// Token: 0x04001316 RID: 4886
		Init,
		// Token: 0x04001317 RID: 4887
		Playing,
		// Token: 0x04001318 RID: 4888
		Paused
	}
}
