using System;
using UnityEngine;

// Token: 0x02000211 RID: 529
public class UIBtn : MonoBehaviour
{
	// Token: 0x06000F55 RID: 3925 RVA: 0x0006E120 File Offset: 0x0006C320
	private void SendMessageProcess(UIInputManager.MouseState_e kind)
	{
		for (int i = 0; i < this.m_SendTargetFunc.Length; i++)
		{
			if (this.m_SendTargetFunc[i].m_State == kind)
			{
				if (this.m_SendTargetFunc[i].Target != null)
				{
					this.m_SendTargetFunc[i].Target.SendMessage(this.m_SendTargetFunc[i].FuncName);
				}
			}
		}
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x0000D2F8 File Offset: 0x0000B4F8
	private void UIPress()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.SendMessageProcess(UIInputManager.MouseState_e.Press);
	}

	// Token: 0x06000F57 RID: 3927 RVA: 0x0000D311 File Offset: 0x0000B511
	private void UIClick()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.SendMessageProcess(UIInputManager.MouseState_e.Click);
	}

	// Token: 0x06000F58 RID: 3928 RVA: 0x0000D32A File Offset: 0x0000B52A
	private void UIDrag()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.SendMessageProcess(UIInputManager.MouseState_e.Drag);
	}

	// Token: 0x06000F59 RID: 3929 RVA: 0x0000D343 File Offset: 0x0000B543
	private void UIDragEnd()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.SendMessageProcess(UIInputManager.MouseState_e.DragEnd);
	}

	// Token: 0x04001106 RID: 4358
	public UIScroll.ScrollKind_e ScrollKind = UIScroll.ScrollKind_e.None;

	// Token: 0x04001107 RID: 4359
	public SendFunc_c[] m_SendTargetFunc;
}
