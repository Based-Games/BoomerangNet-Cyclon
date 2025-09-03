using System;
using UnityEngine;

// Token: 0x02000259 RID: 601
[Serializable]
public class tk2dSpriteAnimationClip
{
	// Token: 0x06001199 RID: 4505 RVA: 0x0000EE46 File Offset: 0x0000D046
	public tk2dSpriteAnimationClip()
	{
	}

	// Token: 0x0600119A RID: 4506 RVA: 0x0000EE64 File Offset: 0x0000D064
	public tk2dSpriteAnimationClip(tk2dSpriteAnimationClip source)
	{
		this.CopyFrom(source);
	}

	// Token: 0x0600119B RID: 4507 RVA: 0x0007B124 File Offset: 0x00079324
	public void CopyFrom(tk2dSpriteAnimationClip source)
	{
		this.name = source.name;
		if (source.frames == null)
		{
			this.frames = null;
		}
		else
		{
			this.frames = new tk2dSpriteAnimationFrame[source.frames.Length];
			for (int i = 0; i < this.frames.Length; i++)
			{
				if (source.frames[i] == null)
				{
					this.frames[i] = null;
				}
				else
				{
					this.frames[i] = new tk2dSpriteAnimationFrame();
					this.frames[i].CopyFrom(source.frames[i]);
				}
			}
		}
		this.fps = source.fps;
		this.loopStart = source.loopStart;
		this.wrapMode = source.wrapMode;
		if (this.wrapMode == tk2dSpriteAnimationClip.WrapMode.Single && this.frames.Length > 1)
		{
			this.frames = new tk2dSpriteAnimationFrame[] { this.frames[0] };
			Debug.LogError(string.Format("Clip: '{0}' Fixed up frames for WrapMode.Single", this.name));
		}
	}

	// Token: 0x0600119C RID: 4508 RVA: 0x0000EE89 File Offset: 0x0000D089
	public void Clear()
	{
		this.name = string.Empty;
		this.frames = new tk2dSpriteAnimationFrame[0];
		this.fps = 30f;
		this.loopStart = 0;
		this.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
	}

	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x0600119D RID: 4509 RVA: 0x0000EEBB File Offset: 0x0000D0BB
	public bool Empty
	{
		get
		{
			return this.name.Length == 0 || this.frames == null || this.frames.Length == 0;
		}
	}

	// Token: 0x0600119E RID: 4510 RVA: 0x0000EEE6 File Offset: 0x0000D0E6
	public tk2dSpriteAnimationFrame GetFrame(int frame)
	{
		return this.frames[frame];
	}

	// Token: 0x040012FB RID: 4859
	public string name = "Default";

	// Token: 0x040012FC RID: 4860
	public tk2dSpriteAnimationFrame[] frames;

	// Token: 0x040012FD RID: 4861
	public float fps = 30f;

	// Token: 0x040012FE RID: 4862
	public int loopStart;

	// Token: 0x040012FF RID: 4863
	public tk2dSpriteAnimationClip.WrapMode wrapMode;

	// Token: 0x0200025A RID: 602
	public enum WrapMode
	{
		// Token: 0x04001301 RID: 4865
		Loop,
		// Token: 0x04001302 RID: 4866
		LoopSection,
		// Token: 0x04001303 RID: 4867
		Once,
		// Token: 0x04001304 RID: 4868
		PingPong,
		// Token: 0x04001305 RID: 4869
		RandomFrame,
		// Token: 0x04001306 RID: 4870
		RandomLoop,
		// Token: 0x04001307 RID: 4871
		Single
	}
}
