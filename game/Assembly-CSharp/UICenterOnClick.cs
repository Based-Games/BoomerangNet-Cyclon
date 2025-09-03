using System;
using UnityEngine;

// Token: 0x02000033 RID: 51
[AddComponentMenu("NGUI/Interaction/Center Scroll View on Click")]
public class UICenterOnClick : MonoBehaviour
{
	// Token: 0x06000146 RID: 326 RVA: 0x000044FB File Offset: 0x000026FB
	private void Start()
	{
		this.mCenter = NGUITools.FindInParents<UICenterOnChild>(base.gameObject);
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00016F84 File Offset: 0x00015184
	public void ClickEvent()
	{
		if (this.mCenter != null)
		{
			if (this.mCenter.enabled)
			{
				this.mCenter.CenterOn(base.transform);
			}
		}
		else if (this.mPanel != null && this.mPanel.clipping != UIDrawCall.Clipping.None)
		{
			SpringPanel.Begin(this.mPanel.cachedGameObject, this.mPanel.cachedTransform.InverseTransformPoint(base.transform.position), 6f);
		}
	}

	// Token: 0x06000148 RID: 328 RVA: 0x00016F84 File Offset: 0x00015184
	private void OnClick()
	{
		if (this.mCenter != null)
		{
			if (this.mCenter.enabled)
			{
				this.mCenter.CenterOn(base.transform);
			}
		}
		else if (this.mPanel != null && this.mPanel.clipping != UIDrawCall.Clipping.None)
		{
			SpringPanel.Begin(this.mPanel.cachedGameObject, this.mPanel.cachedTransform.InverseTransformPoint(base.transform.position), 6f);
		}
	}

	// Token: 0x0400011E RID: 286
	private UIPanel mPanel;

	// Token: 0x0400011F RID: 287
	private UICenterOnChild mCenter;
}
