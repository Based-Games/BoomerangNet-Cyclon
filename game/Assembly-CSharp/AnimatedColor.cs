using System;
using UnityEngine;

// Token: 0x0200008A RID: 138
[RequireComponent(typeof(UIWidget))]
[ExecuteInEditMode]
public class AnimatedColor : MonoBehaviour
{
	// Token: 0x0600043B RID: 1083 RVA: 0x000065D6 File Offset: 0x000047D6
	private void Awake()
	{
		this.mWidget = base.GetComponent<UIWidget>();
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x000065E4 File Offset: 0x000047E4
	private void Update()
	{
		this.mWidget.color = this.color;
	}

	// Token: 0x0400033C RID: 828
	public Color color = Color.white;

	// Token: 0x0400033D RID: 829
	private UIWidget mWidget;
}
