using System;
using UnityEngine;

// Token: 0x020002AF RID: 687
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUIItem")]
public class tk2dUIItem : MonoBehaviour
{
	// Token: 0x14000010 RID: 16
	// (add) Token: 0x06001410 RID: 5136 RVA: 0x00011512 File Offset: 0x0000F712
	// (remove) Token: 0x06001411 RID: 5137 RVA: 0x0001152B File Offset: 0x0000F72B
	public event Action OnDown;

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06001412 RID: 5138 RVA: 0x00011544 File Offset: 0x0000F744
	// (remove) Token: 0x06001413 RID: 5139 RVA: 0x0001155D File Offset: 0x0000F75D
	public event Action OnUp;

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06001414 RID: 5140 RVA: 0x00011576 File Offset: 0x0000F776
	// (remove) Token: 0x06001415 RID: 5141 RVA: 0x0001158F File Offset: 0x0000F78F
	public event Action OnClick;

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x06001416 RID: 5142 RVA: 0x000115A8 File Offset: 0x0000F7A8
	// (remove) Token: 0x06001417 RID: 5143 RVA: 0x000115C1 File Offset: 0x0000F7C1
	public event Action OnRelease;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x06001418 RID: 5144 RVA: 0x000115DA File Offset: 0x0000F7DA
	// (remove) Token: 0x06001419 RID: 5145 RVA: 0x000115F3 File Offset: 0x0000F7F3
	public event Action OnHoverOver;

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x0600141A RID: 5146 RVA: 0x0001160C File Offset: 0x0000F80C
	// (remove) Token: 0x0600141B RID: 5147 RVA: 0x00011625 File Offset: 0x0000F825
	public event Action OnHoverOut;

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x0600141C RID: 5148 RVA: 0x0001163E File Offset: 0x0000F83E
	// (remove) Token: 0x0600141D RID: 5149 RVA: 0x00011657 File Offset: 0x0000F857
	public event Action<tk2dUIItem> OnDownUIItem;

	// Token: 0x14000017 RID: 23
	// (add) Token: 0x0600141E RID: 5150 RVA: 0x00011670 File Offset: 0x0000F870
	// (remove) Token: 0x0600141F RID: 5151 RVA: 0x00011689 File Offset: 0x0000F889
	public event Action<tk2dUIItem> OnUpUIItem;

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x06001420 RID: 5152 RVA: 0x000116A2 File Offset: 0x0000F8A2
	// (remove) Token: 0x06001421 RID: 5153 RVA: 0x000116BB File Offset: 0x0000F8BB
	public event Action<tk2dUIItem> OnClickUIItem;

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x06001422 RID: 5154 RVA: 0x000116D4 File Offset: 0x0000F8D4
	// (remove) Token: 0x06001423 RID: 5155 RVA: 0x000116ED File Offset: 0x0000F8ED
	public event Action<tk2dUIItem> OnReleaseUIItem;

	// Token: 0x1400001A RID: 26
	// (add) Token: 0x06001424 RID: 5156 RVA: 0x00011706 File Offset: 0x0000F906
	// (remove) Token: 0x06001425 RID: 5157 RVA: 0x0001171F File Offset: 0x0000F91F
	public event Action<tk2dUIItem> OnHoverOverUIItem;

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x06001426 RID: 5158 RVA: 0x00011738 File Offset: 0x0000F938
	// (remove) Token: 0x06001427 RID: 5159 RVA: 0x00011751 File Offset: 0x0000F951
	public event Action<tk2dUIItem> OnHoverOutUIItem;

	// Token: 0x06001428 RID: 5160 RVA: 0x0001176A File Offset: 0x0000F96A
	private void Awake()
	{
		if (this.isChildOfAnotherUIItem)
		{
			this.UpdateParent();
		}
	}

	// Token: 0x06001429 RID: 5161 RVA: 0x0001177D File Offset: 0x0000F97D
	private void Start()
	{
		if (tk2dUIManager.Instance == null)
		{
			Debug.LogError("Unable to find tk2dUIManager. Please create a tk2dUIManager in the scene before proceeding.");
		}
		if (this.isChildOfAnotherUIItem && this.parentUIItem == null)
		{
			this.UpdateParent();
		}
	}

	// Token: 0x1700030A RID: 778
	// (get) Token: 0x0600142A RID: 5162 RVA: 0x000117BB File Offset: 0x0000F9BB
	public bool IsPressed
	{
		get
		{
			return this.isPressed;
		}
	}

	// Token: 0x1700030B RID: 779
	// (get) Token: 0x0600142B RID: 5163 RVA: 0x000117C3 File Offset: 0x0000F9C3
	public tk2dUITouch Touch
	{
		get
		{
			return this.touch;
		}
	}

	// Token: 0x1700030C RID: 780
	// (get) Token: 0x0600142C RID: 5164 RVA: 0x000117CB File Offset: 0x0000F9CB
	public tk2dUIItem ParentUIItem
	{
		get
		{
			return this.parentUIItem;
		}
	}

	// Token: 0x0600142D RID: 5165 RVA: 0x000117D3 File Offset: 0x0000F9D3
	public void UpdateParent()
	{
		this.parentUIItem = this.GetParentUIItem();
	}

	// Token: 0x0600142E RID: 5166 RVA: 0x000117E1 File Offset: 0x0000F9E1
	public void ManuallySetParent(tk2dUIItem newParentUIItem)
	{
		this.parentUIItem = newParentUIItem;
	}

	// Token: 0x0600142F RID: 5167 RVA: 0x000117EA File Offset: 0x0000F9EA
	public void RemoveParent()
	{
		this.parentUIItem = null;
	}

	// Token: 0x06001430 RID: 5168 RVA: 0x000117F3 File Offset: 0x0000F9F3
	public bool Press(tk2dUITouch touch)
	{
		return this.Press(touch, null);
	}

	// Token: 0x06001431 RID: 5169 RVA: 0x00088454 File Offset: 0x00086654
	public bool Press(tk2dUITouch touch, tk2dUIItem sentFromChild)
	{
		if (this.isPressed)
		{
			return false;
		}
		if (!this.isPressed)
		{
			this.touch = touch;
			if ((this.registerPressFromChildren || sentFromChild == null) && base.enabled)
			{
				this.isPressed = true;
				if (this.OnDown != null)
				{
					this.OnDown();
				}
				if (this.OnDownUIItem != null)
				{
					this.OnDownUIItem(this);
				}
				this.DoSendMessage(this.SendMessageOnDownMethodName);
			}
			if (this.parentUIItem != null)
			{
				this.parentUIItem.Press(touch, this);
			}
		}
		return true;
	}

	// Token: 0x06001432 RID: 5170 RVA: 0x000117FD File Offset: 0x0000F9FD
	public void UpdateTouch(tk2dUITouch touch)
	{
		this.touch = touch;
		if (this.parentUIItem != null)
		{
			this.parentUIItem.UpdateTouch(touch);
		}
	}

	// Token: 0x06001433 RID: 5171 RVA: 0x00011823 File Offset: 0x0000FA23
	private void DoSendMessage(string methodName)
	{
		if (this.sendMessageTarget != null && methodName.Length > 0)
		{
			this.sendMessageTarget.SendMessage(methodName, this, SendMessageOptions.RequireReceiver);
		}
	}

	// Token: 0x06001434 RID: 5172 RVA: 0x00088504 File Offset: 0x00086704
	public void Release()
	{
		if (this.isPressed)
		{
			this.isPressed = false;
			if (this.OnUp != null)
			{
				this.OnUp();
			}
			if (this.OnUpUIItem != null)
			{
				this.OnUpUIItem(this);
			}
			this.DoSendMessage(this.SendMessageOnUpMethodName);
			if (this.OnClick != null)
			{
				this.OnClick();
			}
			if (this.OnClickUIItem != null)
			{
				this.OnClickUIItem(this);
			}
			this.DoSendMessage(this.SendMessageOnClickMethodName);
		}
		if (this.OnRelease != null)
		{
			this.OnRelease();
		}
		if (this.OnReleaseUIItem != null)
		{
			this.OnReleaseUIItem(this);
		}
		this.DoSendMessage(this.SendMessageOnReleaseMethodName);
		if (this.parentUIItem != null)
		{
			this.parentUIItem.Release();
		}
	}

	// Token: 0x06001435 RID: 5173 RVA: 0x000885EC File Offset: 0x000867EC
	public void CurrentOverUIItem(tk2dUIItem overUIItem)
	{
		if (overUIItem != this)
		{
			if (this.isPressed)
			{
				if (!this.CheckIsUIItemChildOfMe(overUIItem))
				{
					this.Exit();
					if (this.parentUIItem != null)
					{
						this.parentUIItem.CurrentOverUIItem(overUIItem);
					}
				}
			}
			else if (this.parentUIItem != null)
			{
				this.parentUIItem.CurrentOverUIItem(overUIItem);
			}
		}
	}

	// Token: 0x06001436 RID: 5174 RVA: 0x00088664 File Offset: 0x00086864
	public bool CheckIsUIItemChildOfMe(tk2dUIItem uiItem)
	{
		tk2dUIItem tk2dUIItem = null;
		bool flag = false;
		if (uiItem != null)
		{
			tk2dUIItem = uiItem.parentUIItem;
		}
		while (tk2dUIItem != null)
		{
			if (tk2dUIItem == this)
			{
				flag = true;
				break;
			}
			tk2dUIItem = tk2dUIItem.parentUIItem;
		}
		return flag;
	}

	// Token: 0x06001437 RID: 5175 RVA: 0x000886B4 File Offset: 0x000868B4
	public void Exit()
	{
		if (this.isPressed)
		{
			this.isPressed = false;
			if (this.OnUp != null)
			{
				this.OnUp();
			}
			if (this.OnUpUIItem != null)
			{
				this.OnUpUIItem(this);
			}
			this.DoSendMessage(this.SendMessageOnUpMethodName);
		}
	}

	// Token: 0x06001438 RID: 5176 RVA: 0x0008870C File Offset: 0x0008690C
	public bool HoverOver(tk2dUIItem prevHover)
	{
		bool flag = false;
		tk2dUIItem tk2dUIItem = null;
		if (!this.isHoverOver)
		{
			if (this.OnHoverOver != null)
			{
				this.OnHoverOver();
			}
			if (this.OnHoverOverUIItem != null)
			{
				this.OnHoverOverUIItem(this);
			}
			this.isHoverOver = true;
		}
		if (prevHover == this)
		{
			flag = true;
		}
		if (this.parentUIItem != null && this.parentUIItem.isHoverEnabled)
		{
			tk2dUIItem = this.parentUIItem;
		}
		if (tk2dUIItem == null)
		{
			return flag;
		}
		return tk2dUIItem.HoverOver(prevHover) || flag;
	}

	// Token: 0x06001439 RID: 5177 RVA: 0x000887B0 File Offset: 0x000869B0
	public void HoverOut(tk2dUIItem currHoverButton)
	{
		if (this.isHoverOver)
		{
			if (this.OnHoverOut != null)
			{
				this.OnHoverOut();
			}
			if (this.OnHoverOutUIItem != null)
			{
				this.OnHoverOutUIItem(this);
			}
			this.isHoverOver = false;
		}
		if (this.parentUIItem != null && this.parentUIItem.isHoverEnabled)
		{
			if (currHoverButton == null)
			{
				this.parentUIItem.HoverOut(currHoverButton);
			}
			else if (!this.parentUIItem.CheckIsUIItemChildOfMe(currHoverButton) && currHoverButton != this.parentUIItem)
			{
				this.parentUIItem.HoverOut(currHoverButton);
			}
		}
	}

	// Token: 0x0600143A RID: 5178 RVA: 0x00088868 File Offset: 0x00086A68
	private tk2dUIItem GetParentUIItem()
	{
		Transform transform = base.transform.parent;
		while (transform != null)
		{
			tk2dUIItem component = transform.GetComponent<tk2dUIItem>();
			if (component != null)
			{
				return component;
			}
			transform = transform.parent;
		}
		return null;
	}

	// Token: 0x0600143B RID: 5179 RVA: 0x000888B0 File Offset: 0x00086AB0
	public void SimulateClick()
	{
		if (this.OnDown != null)
		{
			this.OnDown();
		}
		if (this.OnDownUIItem != null)
		{
			this.OnDownUIItem(this);
		}
		this.DoSendMessage(this.SendMessageOnDownMethodName);
		if (this.OnUp != null)
		{
			this.OnUp();
		}
		if (this.OnUpUIItem != null)
		{
			this.OnUpUIItem(this);
		}
		this.DoSendMessage(this.SendMessageOnUpMethodName);
		if (this.OnClick != null)
		{
			this.OnClick();
		}
		if (this.OnClickUIItem != null)
		{
			this.OnClickUIItem(this);
		}
		this.DoSendMessage(this.SendMessageOnClickMethodName);
		if (this.OnRelease != null)
		{
			this.OnRelease();
		}
		if (this.OnReleaseUIItem != null)
		{
			this.OnReleaseUIItem(this);
		}
		this.DoSendMessage(this.SendMessageOnReleaseMethodName);
	}

	// Token: 0x0600143C RID: 5180 RVA: 0x00011850 File Offset: 0x0000FA50
	public void InternalSetIsChildOfAnotherUIItem(bool state)
	{
		this.isChildOfAnotherUIItem = state;
	}

	// Token: 0x0600143D RID: 5181 RVA: 0x00011859 File Offset: 0x0000FA59
	public bool InternalGetIsChildOfAnotherUIItem()
	{
		return this.isChildOfAnotherUIItem;
	}

	// Token: 0x04001580 RID: 5504
	public GameObject sendMessageTarget;

	// Token: 0x04001581 RID: 5505
	public string SendMessageOnDownMethodName = string.Empty;

	// Token: 0x04001582 RID: 5506
	public string SendMessageOnUpMethodName = string.Empty;

	// Token: 0x04001583 RID: 5507
	public string SendMessageOnClickMethodName = string.Empty;

	// Token: 0x04001584 RID: 5508
	public string SendMessageOnReleaseMethodName = string.Empty;

	// Token: 0x04001585 RID: 5509
	[SerializeField]
	private bool isChildOfAnotherUIItem;

	// Token: 0x04001586 RID: 5510
	public bool registerPressFromChildren;

	// Token: 0x04001587 RID: 5511
	public bool isHoverEnabled;

	// Token: 0x04001588 RID: 5512
	public Transform[] editorExtraBounds = new Transform[0];

	// Token: 0x04001589 RID: 5513
	public Transform[] editorIgnoreBounds = new Transform[0];

	// Token: 0x0400158A RID: 5514
	private bool isPressed;

	// Token: 0x0400158B RID: 5515
	private bool isHoverOver;

	// Token: 0x0400158C RID: 5516
	private tk2dUITouch touch;

	// Token: 0x0400158D RID: 5517
	private tk2dUIItem parentUIItem;
}
