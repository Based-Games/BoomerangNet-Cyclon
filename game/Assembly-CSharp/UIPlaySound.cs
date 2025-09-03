using System;
using UnityEngine;

// Token: 0x02000047 RID: 71
[AddComponentMenu("NGUI/Interaction/Play Sound")]
public class UIPlaySound : MonoBehaviour
{
	// Token: 0x060001AF RID: 431 RVA: 0x00018C58 File Offset: 0x00016E58
	private void OnHover(bool isOver)
	{
		if (base.enabled && ((isOver && this.trigger == UIPlaySound.Trigger.OnMouseOver) || (!isOver && this.trigger == UIPlaySound.Trigger.OnMouseOut)))
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00018CAC File Offset: 0x00016EAC
	private void OnPress(bool isPressed)
	{
		if (base.enabled && ((isPressed && this.trigger == UIPlaySound.Trigger.OnPress) || (!isPressed && this.trigger == UIPlaySound.Trigger.OnRelease)))
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x00004C37 File Offset: 0x00002E37
	private void OnClick()
	{
		if (base.enabled && this.trigger == UIPlaySound.Trigger.OnClick)
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}

	// Token: 0x040001A0 RID: 416
	public AudioClip audioClip;

	// Token: 0x040001A1 RID: 417
	public UIPlaySound.Trigger trigger;

	// Token: 0x040001A2 RID: 418
	[Range(0f, 1f)]
	public float volume = 1f;

	// Token: 0x040001A3 RID: 419
	[Range(0f, 2f)]
	public float pitch = 1f;

	// Token: 0x02000048 RID: 72
	public enum Trigger
	{
		// Token: 0x040001A5 RID: 421
		OnClick,
		// Token: 0x040001A6 RID: 422
		OnMouseOver,
		// Token: 0x040001A7 RID: 423
		OnMouseOut,
		// Token: 0x040001A8 RID: 424
		OnPress,
		// Token: 0x040001A9 RID: 425
		OnRelease
	}
}
