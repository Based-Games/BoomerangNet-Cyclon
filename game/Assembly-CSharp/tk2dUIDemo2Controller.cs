using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002C1 RID: 705
public class tk2dUIDemo2Controller : tk2dUIBaseDemoController
{
	// Token: 0x060014C6 RID: 5318 RVA: 0x00011D78 File Offset: 0x0000FF78
	private void Start()
	{
		this.rectMin[0] = this.windowLayout.GetMinBounds();
		this.rectMax[0] = this.windowLayout.GetMaxBounds();
	}

	// Token: 0x060014C7 RID: 5319 RVA: 0x0008B97C File Offset: 0x00089B7C
	private IEnumerator NextButtonPressed()
	{
		if (!this.allowButtonPress)
		{
			yield break;
		}
		this.allowButtonPress = false;
		this.currRect = (this.currRect + 1) % this.rectMin.Length;
		Vector3 min = this.rectMin[this.currRect];
		Vector3 max = this.rectMax[this.currRect];
		yield return base.StartCoroutine(base.coResizeLayout(this.windowLayout, min, max, 0.15f));
		this.allowButtonPress = true;
		yield break;
	}

	// Token: 0x060014C8 RID: 5320 RVA: 0x0008B998 File Offset: 0x00089B98
	private void LateUpdate()
	{
		int num = this.rectMin.Length - 1;
		this.rectMin[num].Set(tk2dCamera.Instance.ScreenExtents.xMin, tk2dCamera.Instance.ScreenExtents.yMin, 0f);
		this.rectMax[num].Set(tk2dCamera.Instance.ScreenExtents.xMax, tk2dCamera.Instance.ScreenExtents.yMax, 0f);
	}

	// Token: 0x04001636 RID: 5686
	public tk2dUILayout windowLayout;

	// Token: 0x04001637 RID: 5687
	private Vector3[] rectMin = new Vector3[]
	{
		Vector3.zero,
		new Vector3(-0.8f, -0.7f, 0f),
		new Vector3(-0.9f, -0.9f, 0f),
		new Vector3(-1f, -0.9f, 0f),
		new Vector3(-1f, -1f, 0f),
		Vector3.zero
	};

	// Token: 0x04001638 RID: 5688
	private Vector3[] rectMax = new Vector3[]
	{
		Vector3.one,
		new Vector3(0.8f, 0.7f, 0f),
		new Vector3(0.9f, 0.9f, 0f),
		new Vector3(0.6f, 0.7f, 0f),
		new Vector3(1f, 1f, 0f),
		Vector3.one
	};

	// Token: 0x04001639 RID: 5689
	private int currRect;

	// Token: 0x0400163A RID: 5690
	private bool allowButtonPress = true;
}
