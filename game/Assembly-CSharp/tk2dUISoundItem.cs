using System;
using UnityEngine;

// Token: 0x020002A4 RID: 676
[AddComponentMenu("2D Toolkit/UI/tk2dUISoundItem")]
public class tk2dUISoundItem : tk2dUIBaseItemControl
{
	// Token: 0x060013A1 RID: 5025 RVA: 0x00087028 File Offset: 0x00085228
	private void OnEnable()
	{
		if (this.uiItem)
		{
			if (this.downButtonSound != null)
			{
				this.uiItem.OnDown += this.PlayDownSound;
			}
			if (this.upButtonSound != null)
			{
				this.uiItem.OnUp += this.PlayUpSound;
			}
			if (this.clickButtonSound != null)
			{
				this.uiItem.OnClick += this.PlayClickSound;
			}
			if (this.releaseButtonSound != null)
			{
				this.uiItem.OnRelease += this.PlayReleaseSound;
			}
		}
	}

	// Token: 0x060013A2 RID: 5026 RVA: 0x000870E8 File Offset: 0x000852E8
	private void OnDisable()
	{
		if (this.uiItem)
		{
			if (this.downButtonSound != null)
			{
				this.uiItem.OnDown -= this.PlayDownSound;
			}
			if (this.upButtonSound != null)
			{
				this.uiItem.OnUp -= this.PlayUpSound;
			}
			if (this.clickButtonSound != null)
			{
				this.uiItem.OnClick -= this.PlayClickSound;
			}
			if (this.releaseButtonSound != null)
			{
				this.uiItem.OnRelease -= this.PlayReleaseSound;
			}
		}
	}

	// Token: 0x060013A3 RID: 5027 RVA: 0x00010E6B File Offset: 0x0000F06B
	private void PlayDownSound()
	{
		this.PlaySound(this.downButtonSound);
	}

	// Token: 0x060013A4 RID: 5028 RVA: 0x00010E79 File Offset: 0x0000F079
	private void PlayUpSound()
	{
		this.PlaySound(this.upButtonSound);
	}

	// Token: 0x060013A5 RID: 5029 RVA: 0x00010E87 File Offset: 0x0000F087
	private void PlayClickSound()
	{
		this.PlaySound(this.clickButtonSound);
	}

	// Token: 0x060013A6 RID: 5030 RVA: 0x00010E95 File Offset: 0x0000F095
	private void PlayReleaseSound()
	{
		this.PlaySound(this.releaseButtonSound);
	}

	// Token: 0x060013A7 RID: 5031 RVA: 0x00010EA3 File Offset: 0x0000F0A3
	private void PlaySound(AudioClip source)
	{
		tk2dUIAudioManager.Instance.Play(source);
	}

	// Token: 0x0400153F RID: 5439
	public AudioClip downButtonSound;

	// Token: 0x04001540 RID: 5440
	public AudioClip upButtonSound;

	// Token: 0x04001541 RID: 5441
	public AudioClip clickButtonSound;

	// Token: 0x04001542 RID: 5442
	public AudioClip releaseButtonSound;
}
