using System;
using UnityEngine;

// Token: 0x020002AB RID: 683
[AddComponentMenu("2D Toolkit/UI/tk2dUIUpDownButton")]
public class tk2dUIUpDownButton : tk2dUIBaseItemControl
{
	// Token: 0x17000304 RID: 772
	// (get) Token: 0x060013ED RID: 5101 RVA: 0x00011292 File Offset: 0x0000F492
	public bool UseOnReleaseInsteadOfOnUp
	{
		get
		{
			return this.useOnReleaseInsteadOfOnUp;
		}
	}

	// Token: 0x060013EE RID: 5102 RVA: 0x0001129A File Offset: 0x0000F49A
	private void Start()
	{
		this.SetState();
	}

	// Token: 0x060013EF RID: 5103 RVA: 0x00087F88 File Offset: 0x00086188
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
		}
	}

	// Token: 0x060013F0 RID: 5104 RVA: 0x00087FFC File Offset: 0x000861FC
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
		}
	}

	// Token: 0x060013F1 RID: 5105 RVA: 0x000112A2 File Offset: 0x0000F4A2
	private void ButtonUp()
	{
		this.isDown = false;
		this.SetState();
	}

	// Token: 0x060013F2 RID: 5106 RVA: 0x000112B1 File Offset: 0x0000F4B1
	private void ButtonDown()
	{
		this.isDown = true;
		this.SetState();
	}

	// Token: 0x060013F3 RID: 5107 RVA: 0x000112C0 File Offset: 0x0000F4C0
	private void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.upStateGO, !this.isDown);
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.downStateGO, this.isDown);
	}

	// Token: 0x060013F4 RID: 5108 RVA: 0x000112E7 File Offset: 0x0000F4E7
	public void InternalSetUseOnReleaseInsteadOfOnUp(bool state)
	{
		this.useOnReleaseInsteadOfOnUp = state;
	}

	// Token: 0x04001571 RID: 5489
	public GameObject upStateGO;

	// Token: 0x04001572 RID: 5490
	public GameObject downStateGO;

	// Token: 0x04001573 RID: 5491
	[SerializeField]
	private bool useOnReleaseInsteadOfOnUp;

	// Token: 0x04001574 RID: 5492
	private bool isDown;
}
