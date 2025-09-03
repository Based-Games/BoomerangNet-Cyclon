using System;
using UnityEngine;

// Token: 0x0200025B RID: 603
[AddComponentMenu("2D Toolkit/Backend/tk2dSpriteAnimation")]
public class tk2dSpriteAnimation : MonoBehaviour
{
	// Token: 0x060011A0 RID: 4512 RVA: 0x0007B228 File Offset: 0x00079428
	public tk2dSpriteAnimationClip GetClipByName(string name)
	{
		for (int i = 0; i < this.clips.Length; i++)
		{
			if (this.clips[i].name == name)
			{
				return this.clips[i];
			}
		}
		return null;
	}

	// Token: 0x060011A1 RID: 4513 RVA: 0x0000EEF0 File Offset: 0x0000D0F0
	public tk2dSpriteAnimationClip GetClipById(int id)
	{
		if (id < 0 || id >= this.clips.Length || this.clips[id].Empty)
		{
			return null;
		}
		return this.clips[id];
	}

	// Token: 0x060011A2 RID: 4514 RVA: 0x0007B270 File Offset: 0x00079470
	public int GetClipIdByName(string name)
	{
		for (int i = 0; i < this.clips.Length; i++)
		{
			if (this.clips[i].name == name)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060011A3 RID: 4515 RVA: 0x0007B2B4 File Offset: 0x000794B4
	public int GetClipIdByName(tk2dSpriteAnimationClip clip)
	{
		for (int i = 0; i < this.clips.Length; i++)
		{
			if (this.clips[i] == clip)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x060011A4 RID: 4516 RVA: 0x0007B2EC File Offset: 0x000794EC
	public tk2dSpriteAnimationClip FirstValidClip
	{
		get
		{
			for (int i = 0; i < this.clips.Length; i++)
			{
				if (!this.clips[i].Empty && this.clips[i].frames[0].spriteCollection != null && this.clips[i].frames[0].spriteId != -1)
				{
					return this.clips[i];
				}
			}
			return null;
		}
	}

	// Token: 0x04001308 RID: 4872
	public tk2dSpriteAnimationClip[] clips;
}
