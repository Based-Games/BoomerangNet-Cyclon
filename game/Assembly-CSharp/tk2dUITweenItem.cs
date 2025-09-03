using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002A9 RID: 681
[AddComponentMenu("2D Toolkit/UI/tk2dUITweenItem")]
public class tk2dUITweenItem : tk2dUIBaseItemControl
{
	// Token: 0x17000301 RID: 769
	// (get) Token: 0x060013DE RID: 5086 RVA: 0x0001125D File Offset: 0x0000F45D
	public bool UseOnReleaseInsteadOfOnUp
	{
		get
		{
			return this.useOnReleaseInsteadOfOnUp;
		}
	}

	// Token: 0x060013DF RID: 5087 RVA: 0x00011265 File Offset: 0x0000F465
	private void Awake()
	{
		this.onUpScale = base.transform.localScale;
	}

	// Token: 0x060013E0 RID: 5088 RVA: 0x00087BC4 File Offset: 0x00085DC4
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown += this.ButtonDown;
			if (this.canButtonBeHeldDown)
			{
				if (this.useOnReleaseInsteadOfOnUp)
				{
					this.uiItem.OnRelease += this.ButtonUp;
				}
				else
				{
					this.uiItem.OnUp += this.ButtonUp;
				}
			}
		}
		this.internalTweenInProgress = false;
		this.tweenTimeElapsed = 0f;
		base.transform.localScale = this.onUpScale;
	}

	// Token: 0x060013E1 RID: 5089 RVA: 0x00087C64 File Offset: 0x00085E64
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown -= this.ButtonDown;
			if (this.canButtonBeHeldDown)
			{
				if (this.useOnReleaseInsteadOfOnUp)
				{
					this.uiItem.OnRelease -= this.ButtonUp;
				}
				else
				{
					this.uiItem.OnUp -= this.ButtonUp;
				}
			}
		}
	}

	// Token: 0x060013E2 RID: 5090 RVA: 0x00087CE4 File Offset: 0x00085EE4
	private void ButtonDown()
	{
		if (this.tweenDuration <= 0f)
		{
			base.transform.localScale = this.onDownScale;
		}
		else
		{
			base.transform.localScale = this.onUpScale;
			this.tweenTargetScale = this.onDownScale;
			this.tweenStartingScale = base.transform.localScale;
			if (!this.internalTweenInProgress)
			{
				base.StartCoroutine(this.ScaleTween());
				this.internalTweenInProgress = true;
			}
		}
	}

	// Token: 0x060013E3 RID: 5091 RVA: 0x00087D64 File Offset: 0x00085F64
	private void ButtonUp()
	{
		if (this.tweenDuration <= 0f)
		{
			base.transform.localScale = this.onUpScale;
		}
		else
		{
			this.tweenTargetScale = this.onUpScale;
			this.tweenStartingScale = base.transform.localScale;
			if (!this.internalTweenInProgress)
			{
				base.StartCoroutine(this.ScaleTween());
				this.internalTweenInProgress = true;
			}
		}
	}

	// Token: 0x060013E4 RID: 5092 RVA: 0x00087DD4 File Offset: 0x00085FD4
	private IEnumerator ScaleTween()
	{
		this.tweenTimeElapsed = 0f;
		while (this.tweenTimeElapsed < this.tweenDuration)
		{
			base.transform.localScale = Vector3.Lerp(this.tweenStartingScale, this.tweenTargetScale, this.tweenTimeElapsed / this.tweenDuration);
			yield return null;
			this.tweenTimeElapsed += tk2dUITime.deltaTime;
		}
		base.transform.localScale = this.tweenTargetScale;
		this.internalTweenInProgress = false;
		if (!this.canButtonBeHeldDown)
		{
			if (this.tweenDuration <= 0f)
			{
				base.transform.localScale = this.onUpScale;
			}
			else
			{
				this.tweenTargetScale = this.onUpScale;
				this.tweenStartingScale = base.transform.localScale;
				base.StartCoroutine(this.ScaleTween());
				this.internalTweenInProgress = true;
			}
		}
		yield break;
	}

	// Token: 0x060013E5 RID: 5093 RVA: 0x00011278 File Offset: 0x0000F478
	public void InternalSetUseOnReleaseInsteadOfOnUp(bool state)
	{
		this.useOnReleaseInsteadOfOnUp = state;
	}

	// Token: 0x04001565 RID: 5477
	private Vector3 onUpScale;

	// Token: 0x04001566 RID: 5478
	public Vector3 onDownScale = new Vector3(0.9f, 0.9f, 0.9f);

	// Token: 0x04001567 RID: 5479
	public float tweenDuration = 0.1f;

	// Token: 0x04001568 RID: 5480
	public bool canButtonBeHeldDown = true;

	// Token: 0x04001569 RID: 5481
	[SerializeField]
	private bool useOnReleaseInsteadOfOnUp;

	// Token: 0x0400156A RID: 5482
	private bool internalTweenInProgress;

	// Token: 0x0400156B RID: 5483
	private Vector3 tweenTargetScale = Vector3.one;

	// Token: 0x0400156C RID: 5484
	private Vector3 tweenStartingScale = Vector3.one;

	// Token: 0x0400156D RID: 5485
	private float tweenTimeElapsed;
}
