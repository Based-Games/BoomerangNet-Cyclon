using System;
using UnityEngine;

// Token: 0x0200029E RID: 670
[AddComponentMenu("2D Toolkit/UI/tk2dUIMultiStateToggleButton")]
public class tk2dUIMultiStateToggleButton : tk2dUIBaseItemControl
{
	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06001344 RID: 4932 RVA: 0x0001081F File Offset: 0x0000EA1F
	// (remove) Token: 0x06001345 RID: 4933 RVA: 0x00010838 File Offset: 0x0000EA38
	public event Action<tk2dUIMultiStateToggleButton> OnStateToggle;

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x06001346 RID: 4934 RVA: 0x00010851 File Offset: 0x0000EA51
	// (set) Token: 0x06001347 RID: 4935 RVA: 0x00085404 File Offset: 0x00083604
	public int Index
	{
		get
		{
			return this.index;
		}
		set
		{
			if (value >= this.states.Length)
			{
				value = this.states.Length;
			}
			if (value < 0)
			{
				value = 0;
			}
			if (this.index != value)
			{
				this.index = value;
				this.SetState();
				if (this.OnStateToggle != null)
				{
					this.OnStateToggle(this);
				}
				base.DoSendMessage(this.SendMessageOnStateToggleMethodName, this);
			}
		}
	}

	// Token: 0x06001348 RID: 4936 RVA: 0x00010859 File Offset: 0x0000EA59
	private void Start()
	{
		this.SetState();
	}

	// Token: 0x06001349 RID: 4937 RVA: 0x00010861 File Offset: 0x0000EA61
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick += this.ButtonClick;
			this.uiItem.OnDown += this.ButtonDown;
		}
	}

	// Token: 0x0600134A RID: 4938 RVA: 0x000108A1 File Offset: 0x0000EAA1
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnClick -= this.ButtonClick;
			this.uiItem.OnDown -= this.ButtonDown;
		}
	}

	// Token: 0x0600134B RID: 4939 RVA: 0x000108E1 File Offset: 0x0000EAE1
	private void ButtonClick()
	{
		if (!this.activateOnPress)
		{
			this.ButtonToggle();
		}
	}

	// Token: 0x0600134C RID: 4940 RVA: 0x000108F4 File Offset: 0x0000EAF4
	private void ButtonDown()
	{
		if (this.activateOnPress)
		{
			this.ButtonToggle();
		}
	}

	// Token: 0x0600134D RID: 4941 RVA: 0x00010907 File Offset: 0x0000EB07
	private void ButtonToggle()
	{
		if (this.Index + 1 >= this.states.Length)
		{
			this.Index = 0;
		}
		else
		{
			this.Index++;
		}
	}

	// Token: 0x0600134E RID: 4942 RVA: 0x00085470 File Offset: 0x00083670
	private void SetState()
	{
		for (int i = 0; i < this.states.Length; i++)
		{
			GameObject gameObject = this.states[i];
			if (gameObject != null)
			{
				if (i != this.index)
				{
					if (this.states[i].activeInHierarchy)
					{
						this.states[i].SetActive(false);
					}
				}
				else if (!this.states[i].activeInHierarchy)
				{
					this.states[i].SetActive(true);
				}
			}
		}
	}

	// Token: 0x040014F5 RID: 5365
	public GameObject[] states;

	// Token: 0x040014F6 RID: 5366
	public bool activateOnPress;

	// Token: 0x040014F7 RID: 5367
	private int index;

	// Token: 0x040014F8 RID: 5368
	public string SendMessageOnStateToggleMethodName = string.Empty;
}
