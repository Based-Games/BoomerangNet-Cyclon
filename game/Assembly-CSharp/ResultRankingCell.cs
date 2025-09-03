using System;
using UnityEngine;

// Token: 0x020001EA RID: 490
public class ResultRankingCell : MonoBehaviour
{
	// Token: 0x06000E39 RID: 3641 RVA: 0x0000C630 File Offset: 0x0000A830
	private void UIPress()
	{
		this.m_ResultRankingManager.AutoScroll = false;
	}

	// Token: 0x06000E3A RID: 3642 RVA: 0x0000C63E File Offset: 0x0000A83E
	private void UIDrag()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_UIScroll.DragProcess();
	}

	// Token: 0x06000E3B RID: 3643 RVA: 0x0000C65B File Offset: 0x0000A85B
	private void UIDragEnd()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_UIScroll.ClickProcess();
	}

	// Token: 0x06000E3C RID: 3644 RVA: 0x0000C630 File Offset: 0x0000A830
	private void UIClick()
	{
		this.m_ResultRankingManager.AutoScroll = false;
	}

	// Token: 0x04000F38 RID: 3896
	[HideInInspector]
	public ResultRankingManager m_ResultRankingManager;

	// Token: 0x04000F39 RID: 3897
	public UIScroll.ScrollKind_e ScrollKind = UIScroll.ScrollKind_e.None;

	// Token: 0x04000F3A RID: 3898
	public UISprite UserImage;

	// Token: 0x04000F3B RID: 3899
	public UISprite UserMark;

	// Token: 0x04000F3C RID: 3900
	public UILabel UserName;

	// Token: 0x04000F3D RID: 3901
	public UILabel UserScore;

	// Token: 0x04000F3E RID: 3902
	public UILabel UserRankingNum;

	// Token: 0x04000F3F RID: 3903
	public UITexture UserFlag;

	// Token: 0x04000F40 RID: 3904
	public UISprite bg;

	// Token: 0x04000F41 RID: 3905
	[HideInInspector]
	public UIScroll m_UIScroll;
}
