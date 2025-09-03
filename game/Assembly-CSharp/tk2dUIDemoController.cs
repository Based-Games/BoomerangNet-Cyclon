using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002CA RID: 714
[AddComponentMenu("2D Toolkit/Demo/tk2dUIDemoController")]
public class tk2dUIDemoController : tk2dUIBaseDemoController
{
	// Token: 0x060014FA RID: 5370 RVA: 0x00011E46 File Offset: 0x00010046
	private void Awake()
	{
		base.ShowWindow(this.window1.transform);
		base.HideWindow(this.window2.transform);
	}

	// Token: 0x060014FB RID: 5371 RVA: 0x00011E6A File Offset: 0x0001006A
	private void OnEnable()
	{
		this.nextPage.OnClick += this.GoToPage2;
		this.prevPage.OnClick += this.GoToPage1;
	}

	// Token: 0x060014FC RID: 5372 RVA: 0x00011E9A File Offset: 0x0001009A
	private void OnDisable()
	{
		this.nextPage.OnClick -= this.GoToPage2;
		this.prevPage.OnClick -= this.GoToPage1;
	}

	// Token: 0x060014FD RID: 5373 RVA: 0x00011ECA File Offset: 0x000100CA
	private void GoToPage1()
	{
		this.timeSincePageStart = 0f;
		base.AnimateHideWindow(this.window2.transform);
		base.AnimateShowWindow(this.window1.transform);
		this.currWindow = this.window1;
	}

	// Token: 0x060014FE RID: 5374 RVA: 0x0008C614 File Offset: 0x0008A814
	private void GoToPage2()
	{
		this.timeSincePageStart = 0f;
		if (this.currWindow != this.window2)
		{
			this.progressBar.Value = 0f;
			this.currWindow = this.window2;
			base.StartCoroutine(this.MoveProgressBar());
		}
		base.AnimateHideWindow(this.window1.transform);
		base.AnimateShowWindow(this.window2.transform);
	}

	// Token: 0x060014FF RID: 5375 RVA: 0x0008C690 File Offset: 0x0008A890
	private IEnumerator MoveProgressBar()
	{
		while (this.currWindow == this.window2 && this.progressBar.Value < 1f)
		{
			this.progressBar.Value = this.timeSincePageStart / 2f;
			yield return null;
			this.timeSincePageStart += tk2dUITime.deltaTime;
		}
		while (this.currWindow == this.window2)
		{
			float smoothTime = 0.5f;
			this.progressBar.Value = Mathf.SmoothDamp(this.progressBar.Value, this.slider.Value, ref this.progressBarChaseVelocity, smoothTime, float.PositiveInfinity, tk2dUITime.deltaTime);
			yield return 0;
		}
		yield break;
	}

	// Token: 0x04001661 RID: 5729
	private const float TIME_TO_COMPLETE_PROGRESS_BAR = 2f;

	// Token: 0x04001662 RID: 5730
	public tk2dUIItem nextPage;

	// Token: 0x04001663 RID: 5731
	public GameObject window1;

	// Token: 0x04001664 RID: 5732
	public tk2dUIItem prevPage;

	// Token: 0x04001665 RID: 5733
	public GameObject window2;

	// Token: 0x04001666 RID: 5734
	public tk2dUIProgressBar progressBar;

	// Token: 0x04001667 RID: 5735
	private float timeSincePageStart;

	// Token: 0x04001668 RID: 5736
	private float progressBarChaseVelocity;

	// Token: 0x04001669 RID: 5737
	public tk2dUIScrollbar slider;

	// Token: 0x0400166A RID: 5738
	private GameObject currWindow;
}
