using System;
using UnityEngine;

// Token: 0x020002A7 RID: 679
[AddComponentMenu("2D Toolkit/UI/tk2dUIToggleButtonGroup")]
public class tk2dUIToggleButtonGroup : MonoBehaviour
{
	// Token: 0x1400000E RID: 14
	// (add) Token: 0x060013CE RID: 5070 RVA: 0x000111CA File Offset: 0x0000F3CA
	// (remove) Token: 0x060013CF RID: 5071 RVA: 0x000111E3 File Offset: 0x0000F3E3
	public event Action<tk2dUIToggleButtonGroup> OnChange;

	// Token: 0x170002FE RID: 766
	// (get) Token: 0x060013D0 RID: 5072 RVA: 0x000111FC File Offset: 0x0000F3FC
	public tk2dUIToggleButton[] ToggleBtns
	{
		get
		{
			return this.toggleBtns;
		}
	}

	// Token: 0x170002FF RID: 767
	// (get) Token: 0x060013D1 RID: 5073 RVA: 0x00011204 File Offset: 0x0000F404
	// (set) Token: 0x060013D2 RID: 5074 RVA: 0x0001120C File Offset: 0x0000F40C
	public int SelectedIndex
	{
		get
		{
			return this.selectedIndex;
		}
		set
		{
			if (this.selectedIndex != value)
			{
				this.selectedIndex = value;
				this.SetToggleButtonUsingSelectedIndex();
			}
		}
	}

	// Token: 0x17000300 RID: 768
	// (get) Token: 0x060013D3 RID: 5075 RVA: 0x00011227 File Offset: 0x0000F427
	// (set) Token: 0x060013D4 RID: 5076 RVA: 0x0001122F File Offset: 0x0000F42F
	public tk2dUIToggleButton SelectedToggleButton
	{
		get
		{
			return this.selectedToggleButton;
		}
		set
		{
			this.ButtonToggle(value);
		}
	}

	// Token: 0x060013D5 RID: 5077 RVA: 0x00011238 File Offset: 0x0000F438
	protected virtual void Awake()
	{
		this.Setup();
	}

	// Token: 0x060013D6 RID: 5078 RVA: 0x00087938 File Offset: 0x00085B38
	protected void Setup()
	{
		foreach (tk2dUIToggleButton tk2dUIToggleButton in this.toggleBtns)
		{
			if (tk2dUIToggleButton != null)
			{
				tk2dUIToggleButton.IsInToggleGroup = true;
				tk2dUIToggleButton.IsOn = false;
				tk2dUIToggleButton.OnToggle += this.ButtonToggle;
			}
		}
		this.SetToggleButtonUsingSelectedIndex();
	}

	// Token: 0x060013D7 RID: 5079 RVA: 0x00011240 File Offset: 0x0000F440
	public void AddNewToggleButtons(tk2dUIToggleButton[] newToggleBtns)
	{
		this.ClearExistingToggleBtns();
		this.toggleBtns = newToggleBtns;
		this.Setup();
	}

	// Token: 0x060013D8 RID: 5080 RVA: 0x00087998 File Offset: 0x00085B98
	private void ClearExistingToggleBtns()
	{
		if (this.toggleBtns != null && this.toggleBtns.Length > 0)
		{
			foreach (tk2dUIToggleButton tk2dUIToggleButton in this.toggleBtns)
			{
				tk2dUIToggleButton.IsInToggleGroup = false;
				tk2dUIToggleButton.OnToggle -= this.ButtonToggle;
				tk2dUIToggleButton.IsOn = false;
			}
		}
	}

	// Token: 0x060013D9 RID: 5081 RVA: 0x00087A00 File Offset: 0x00085C00
	private void SetToggleButtonUsingSelectedIndex()
	{
		if (this.selectedIndex >= 0 && this.selectedIndex < this.toggleBtns.Length)
		{
			tk2dUIToggleButton tk2dUIToggleButton = this.toggleBtns[this.selectedIndex];
			tk2dUIToggleButton.IsOn = true;
		}
		else
		{
			tk2dUIToggleButton tk2dUIToggleButton = null;
			this.selectedIndex = -1;
			this.ButtonToggle(tk2dUIToggleButton);
		}
	}

	// Token: 0x060013DA RID: 5082 RVA: 0x00087A58 File Offset: 0x00085C58
	private void ButtonToggle(tk2dUIToggleButton toggleButton)
	{
		if (toggleButton == null || toggleButton.IsOn)
		{
			foreach (tk2dUIToggleButton tk2dUIToggleButton in this.toggleBtns)
			{
				if (tk2dUIToggleButton != toggleButton)
				{
					tk2dUIToggleButton.IsOn = false;
				}
			}
			if (toggleButton != this.selectedToggleButton)
			{
				this.selectedToggleButton = toggleButton;
				this.SetSelectedIndexFromSelectedToggleButton();
				if (this.OnChange != null)
				{
					this.OnChange(this);
				}
				if (this.sendMessageTarget != null && this.SendMessageOnChangeMethodName.Length > 0)
				{
					this.sendMessageTarget.SendMessage(this.SendMessageOnChangeMethodName, this, SendMessageOptions.RequireReceiver);
				}
			}
		}
	}

	// Token: 0x060013DB RID: 5083 RVA: 0x00087B18 File Offset: 0x00085D18
	private void SetSelectedIndexFromSelectedToggleButton()
	{
		this.selectedIndex = -1;
		for (int i = 0; i < this.toggleBtns.Length; i++)
		{
			tk2dUIToggleButton tk2dUIToggleButton = this.toggleBtns[i];
			if (tk2dUIToggleButton == this.selectedToggleButton)
			{
				this.selectedIndex = i;
				break;
			}
		}
	}

	// Token: 0x0400155E RID: 5470
	[SerializeField]
	private tk2dUIToggleButton[] toggleBtns;

	// Token: 0x0400155F RID: 5471
	public GameObject sendMessageTarget;

	// Token: 0x04001560 RID: 5472
	[SerializeField]
	private int selectedIndex;

	// Token: 0x04001561 RID: 5473
	private tk2dUIToggleButton selectedToggleButton;

	// Token: 0x04001562 RID: 5474
	public string SendMessageOnChangeMethodName = string.Empty;
}
