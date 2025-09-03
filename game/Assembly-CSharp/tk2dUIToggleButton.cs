using System;
using UnityEngine;

// Token: 0x020002A6 RID: 678
[AddComponentMenu("2D Toolkit/UI/tk2dUIToggleButton")]
public class tk2dUIToggleButton : tk2dUIBaseItemControl
{
	// Token: 0x1400000D RID: 13
	// (add) Token: 0x060013C0 RID: 5056 RVA: 0x00011065 File Offset: 0x0000F265
	// (remove) Token: 0x060013C1 RID: 5057 RVA: 0x0001107E File Offset: 0x0000F27E
	public event Action<tk2dUIToggleButton> OnToggle;

	// Token: 0x170002FC RID: 764
	// (get) Token: 0x060013C2 RID: 5058 RVA: 0x00011097 File Offset: 0x0000F297
	// (set) Token: 0x060013C3 RID: 5059 RVA: 0x0001109F File Offset: 0x0000F29F
	public bool IsOn
	{
		get
		{
			return this.isOn;
		}
		set
		{
			if (this.isOn != value)
			{
				this.isOn = value;
				this.SetState();
				if (this.OnToggle != null)
				{
					this.OnToggle(this);
				}
			}
		}
	}

	// Token: 0x170002FD RID: 765
	// (get) Token: 0x060013C4 RID: 5060 RVA: 0x000110D1 File Offset: 0x0000F2D1
	// (set) Token: 0x060013C5 RID: 5061 RVA: 0x000110D9 File Offset: 0x0000F2D9
	public bool IsInToggleGroup
	{
		get
		{
			return this.isInToggleGroup;
		}
		set
		{
			this.isInToggleGroup = value;
		}
	}

	// Token: 0x060013C6 RID: 5062 RVA: 0x000110E2 File Offset: 0x0000F2E2
	private void Start()
	{
		this.SetState();
	}

	// Token: 0x060013C7 RID: 5063 RVA: 0x000110EA File Offset: 0x0000F2EA
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick += this.ButtonClick;
			this.uiItem.OnDown += this.ButtonDown;
		}
	}

	// Token: 0x060013C8 RID: 5064 RVA: 0x0001112A File Offset: 0x0000F32A
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick -= this.ButtonClick;
			this.uiItem.OnDown -= this.ButtonDown;
		}
	}

	// Token: 0x060013C9 RID: 5065 RVA: 0x0001116A File Offset: 0x0000F36A
	private void ButtonClick()
	{
		if (!this.activateOnPress)
		{
			this.ButtonToggle();
		}
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x0001117D File Offset: 0x0000F37D
	private void ButtonDown()
	{
		if (this.activateOnPress)
		{
			this.ButtonToggle();
		}
	}

	// Token: 0x060013CB RID: 5067 RVA: 0x000878DC File Offset: 0x00085ADC
	private void ButtonToggle()
	{
		if (!this.isOn || !this.isInToggleGroup)
		{
			this.isOn = !this.isOn;
			this.SetState();
			if (this.OnToggle != null)
			{
				this.OnToggle(this);
			}
			base.DoSendMessage(this.SendMessageOnToggleMethodName, this);
		}
	}

	// Token: 0x060013CC RID: 5068 RVA: 0x00011190 File Offset: 0x0000F390
	private void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.offStateGO, !this.isOn);
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.onStateGO, this.isOn);
	}

	// Token: 0x04001557 RID: 5463
	public GameObject offStateGO;

	// Token: 0x04001558 RID: 5464
	public GameObject onStateGO;

	// Token: 0x04001559 RID: 5465
	public bool activateOnPress;

	// Token: 0x0400155A RID: 5466
	[SerializeField]
	private bool isOn = true;

	// Token: 0x0400155B RID: 5467
	private bool isInToggleGroup;

	// Token: 0x0400155C RID: 5468
	public string SendMessageOnToggleMethodName = string.Empty;
}
