using System;
using UnityEngine;

// Token: 0x02000299 RID: 665
[AddComponentMenu("2D Toolkit/UI/tk2dUIBaseItemControl")]
public abstract class tk2dUIBaseItemControl : MonoBehaviour
{
	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x06001308 RID: 4872 RVA: 0x0001031B File Offset: 0x0000E51B
	// (set) Token: 0x06001309 RID: 4873 RVA: 0x0001033B File Offset: 0x0000E53B
	public GameObject SendMessageTarget
	{
		get
		{
			if (this.uiItem != null)
			{
				return this.uiItem.sendMessageTarget;
			}
			return null;
		}
		set
		{
			if (this.uiItem != null)
			{
				this.uiItem.sendMessageTarget = value;
			}
		}
	}

	// Token: 0x0600130A RID: 4874 RVA: 0x00005BE5 File Offset: 0x00003DE5
	public static void ChangeGameObjectActiveState(GameObject go, bool isActive)
	{
		go.SetActive(isActive);
	}

	// Token: 0x0600130B RID: 4875 RVA: 0x0001035A File Offset: 0x0000E55A
	public static void ChangeGameObjectActiveStateWithNullCheck(GameObject go, bool isActive)
	{
		if (go != null)
		{
			tk2dUIBaseItemControl.ChangeGameObjectActiveState(go, isActive);
		}
	}

	// Token: 0x0600130C RID: 4876 RVA: 0x0001036F File Offset: 0x0000E56F
	protected void DoSendMessage(string methodName, object parameter)
	{
		if (this.SendMessageTarget != null && methodName.Length > 0)
		{
			this.SendMessageTarget.SendMessage(methodName, parameter, SendMessageOptions.RequireReceiver);
		}
	}

	// Token: 0x040014D9 RID: 5337
	public tk2dUIItem uiItem;
}
