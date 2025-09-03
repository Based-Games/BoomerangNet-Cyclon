using System;
using UnityEngine;

// Token: 0x020002AC RID: 684
[AddComponentMenu("2D Toolkit/UI/tk2dUIUpDownHoverButton")]
public class tk2dUIUpDownHoverButton : tk2dUIBaseItemControl
{
	// Token: 0x1400000F RID: 15
	// (add) Token: 0x060013F6 RID: 5110 RVA: 0x00011303 File Offset: 0x0000F503
	// (remove) Token: 0x060013F7 RID: 5111 RVA: 0x0001131C File Offset: 0x0000F51C
	public event Action<tk2dUIUpDownHoverButton> OnToggleOver;

	// Token: 0x17000305 RID: 773
	// (get) Token: 0x060013F8 RID: 5112 RVA: 0x00011335 File Offset: 0x0000F535
	public bool UseOnReleaseInsteadOfOnUp
	{
		get
		{
			return this.useOnReleaseInsteadOfOnUp;
		}
	}

	// Token: 0x17000306 RID: 774
	// (get) Token: 0x060013F9 RID: 5113 RVA: 0x0001133D File Offset: 0x0000F53D
	// (set) Token: 0x060013FA RID: 5114 RVA: 0x00088070 File Offset: 0x00086270
	public bool IsOver
	{
		get
		{
			return this.isDown || this.isHover;
		}
		set
		{
			if (value != this.isDown || this.isHover)
			{
				if (value)
				{
					this.isHover = true;
					this.SetState();
					if (this.OnToggleOver != null)
					{
						this.OnToggleOver(this);
					}
				}
				else if (this.isDown && this.isHover)
				{
					this.isDown = false;
					this.isHover = false;
					this.SetState();
					if (this.OnToggleOver != null)
					{
						this.OnToggleOver(this);
					}
				}
				else if (this.isDown)
				{
					this.isDown = false;
					this.SetState();
					if (this.OnToggleOver != null)
					{
						this.OnToggleOver(this);
					}
				}
				else
				{
					this.isHover = false;
					this.SetState();
					if (this.OnToggleOver != null)
					{
						this.OnToggleOver(this);
					}
				}
				base.DoSendMessage(this.SendMessageOnToggleOverMethodName, this);
			}
		}
	}

	// Token: 0x060013FB RID: 5115 RVA: 0x00011353 File Offset: 0x0000F553
	private void Start()
	{
		this.SetState();
	}

	// Token: 0x060013FC RID: 5116 RVA: 0x00088170 File Offset: 0x00086370
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown += this.ButtonDown;
			if (this.useOnReleaseInsteadOfOnUp)
			{
				this.uiItem.OnRelease += this.ButtonUp;
			}
			else
			{
				this.uiItem.OnUp += this.ButtonUp;
			}
			this.uiItem.OnHoverOver += this.ButtonHoverOver;
			this.uiItem.OnHoverOut += this.ButtonHoverOut;
		}
	}

	// Token: 0x060013FD RID: 5117 RVA: 0x00088210 File Offset: 0x00086410
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown -= this.ButtonDown;
			if (this.useOnReleaseInsteadOfOnUp)
			{
				this.uiItem.OnRelease -= this.ButtonUp;
			}
			else
			{
				this.uiItem.OnUp -= this.ButtonUp;
			}
			this.uiItem.OnHoverOver -= this.ButtonHoverOver;
			this.uiItem.OnHoverOut -= this.ButtonHoverOut;
		}
	}

	// Token: 0x060013FE RID: 5118 RVA: 0x0001135B File Offset: 0x0000F55B
	private void ButtonUp()
	{
		if (this.isDown)
		{
			this.isDown = false;
			this.SetState();
			if (!this.isHover && this.OnToggleOver != null)
			{
				this.OnToggleOver(this);
			}
		}
	}

	// Token: 0x060013FF RID: 5119 RVA: 0x00011397 File Offset: 0x0000F597
	private void ButtonDown()
	{
		if (!this.isDown)
		{
			this.isDown = true;
			this.SetState();
			if (!this.isHover && this.OnToggleOver != null)
			{
				this.OnToggleOver(this);
			}
		}
	}

	// Token: 0x06001400 RID: 5120 RVA: 0x000113D3 File Offset: 0x0000F5D3
	private void ButtonHoverOver()
	{
		if (!this.isHover)
		{
			this.isHover = true;
			this.SetState();
			if (!this.isDown && this.OnToggleOver != null)
			{
				this.OnToggleOver(this);
			}
		}
	}

	// Token: 0x06001401 RID: 5121 RVA: 0x0001140F File Offset: 0x0000F60F
	private void ButtonHoverOut()
	{
		if (this.isHover)
		{
			this.isHover = false;
			this.SetState();
			if (!this.isDown && this.OnToggleOver != null)
			{
				this.OnToggleOver(this);
			}
		}
	}

	// Token: 0x06001402 RID: 5122 RVA: 0x000882B0 File Offset: 0x000864B0
	public void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.upStateGO, !this.isDown && !this.isHover);
		if (this.downStateGO == this.hoverOverStateGO)
		{
			tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.downStateGO, this.isDown || this.isHover);
		}
		else
		{
			tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.downStateGO, this.isDown);
			tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.hoverOverStateGO, this.isHover);
		}
	}

	// Token: 0x06001403 RID: 5123 RVA: 0x0001144B File Offset: 0x0000F64B
	public void InternalSetUseOnReleaseInsteadOfOnUp(bool state)
	{
		this.useOnReleaseInsteadOfOnUp = state;
	}

	// Token: 0x04001575 RID: 5493
	public GameObject upStateGO;

	// Token: 0x04001576 RID: 5494
	public GameObject downStateGO;

	// Token: 0x04001577 RID: 5495
	public GameObject hoverOverStateGO;

	// Token: 0x04001578 RID: 5496
	[SerializeField]
	private bool useOnReleaseInsteadOfOnUp;

	// Token: 0x04001579 RID: 5497
	private bool isDown;

	// Token: 0x0400157A RID: 5498
	private bool isHover;

	// Token: 0x0400157B RID: 5499
	public string SendMessageOnToggleOverMethodName = string.Empty;
}
