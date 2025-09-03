using System;
using UnityEngine;

// Token: 0x02000089 RID: 137
public class AnimatedAlpha : MonoBehaviour
{
	// Token: 0x06000438 RID: 1080 RVA: 0x000065A3 File Offset: 0x000047A3
	private void Awake()
	{
		this.mWidget = base.GetComponent<UIWidget>();
		this.mPanel = base.GetComponent<UIPanel>();
		this.Update();
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x000259B8 File Offset: 0x00023BB8
	private void Update()
	{
		if (this.mWidget != null)
		{
			this.mWidget.alpha = this.alpha;
		}
		if (this.mPanel != null)
		{
			this.mPanel.alpha = this.alpha;
		}
	}

	// Token: 0x04000339 RID: 825
	public float alpha = 1f;

	// Token: 0x0400033A RID: 826
	private UIWidget mWidget;

	// Token: 0x0400033B RID: 827
	private UIPanel mPanel;
}
