using System;
using UnityEngine;

// Token: 0x020001D4 RID: 468
public class HouseMixSortTab : MonoBehaviour
{
	// Token: 0x06000DB0 RID: 3504 RVA: 0x0000C136 File Offset: 0x0000A336
	private void UIPress()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
	}

	// Token: 0x06000DB1 RID: 3505 RVA: 0x0000C148 File Offset: 0x0000A348
	private void UIDrag()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_UIScroll.DragProcess();
	}

	// Token: 0x06000DB2 RID: 3506 RVA: 0x0000C165 File Offset: 0x0000A365
	private void UIDragEnd()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_UIScroll.ClickProcess();
	}

	// Token: 0x06000DB3 RID: 3507 RVA: 0x0000C136 File Offset: 0x0000A336
	private void UIClick()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
	}

	// Token: 0x04000E14 RID: 3604
	public UIScroll.ScrollKind_e ScrollKind = UIScroll.ScrollKind_e.None;

	// Token: 0x04000E15 RID: 3605
	public UILabel TabName;

	// Token: 0x04000E16 RID: 3606
	public UILabel TabName_Han;

	// Token: 0x04000E17 RID: 3607
	public UISprite LevelText;

	// Token: 0x04000E18 RID: 3608
	public GameObject Average;

	// Token: 0x04000E19 RID: 3609
	public UISprite discbg;

	// Token: 0x04000E1A RID: 3610
	[HideInInspector]
	public UIScroll m_UIScroll;
}
