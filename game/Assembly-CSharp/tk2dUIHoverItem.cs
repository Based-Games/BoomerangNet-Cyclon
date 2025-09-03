using System;
using UnityEngine;

// Token: 0x0200029D RID: 669
[AddComponentMenu("2D Toolkit/UI/tk2dUIHoverItem")]
public class tk2dUIHoverItem : tk2dUIBaseItemControl
{
	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06001339 RID: 4921 RVA: 0x000106D2 File Offset: 0x0000E8D2
	// (remove) Token: 0x0600133A RID: 4922 RVA: 0x000106EB File Offset: 0x0000E8EB
	public event Action<tk2dUIHoverItem> OnToggleHover;

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x0600133B RID: 4923 RVA: 0x00010704 File Offset: 0x0000E904
	// (set) Token: 0x0600133C RID: 4924 RVA: 0x0001070C File Offset: 0x0000E90C
	public bool IsOver
	{
		get
		{
			return this.isOver;
		}
		set
		{
			if (this.isOver != value)
			{
				this.isOver = value;
				this.SetState();
				if (this.OnToggleHover != null)
				{
					this.OnToggleHover(this);
				}
				base.DoSendMessage(this.SendMessageOnToggleHoverMethodName, this);
			}
		}
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x0001074B File Offset: 0x0000E94B
	private void Start()
	{
		this.SetState();
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x00010753 File Offset: 0x0000E953
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnHoverOver += this.HoverOver;
			this.uiItem.OnHoverOut += this.HoverOut;
		}
	}

	// Token: 0x0600133F RID: 4927 RVA: 0x00010793 File Offset: 0x0000E993
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnHoverOver -= this.HoverOver;
			this.uiItem.OnHoverOut -= this.HoverOut;
		}
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x000107D3 File Offset: 0x0000E9D3
	private void HoverOver()
	{
		this.IsOver = true;
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x000107DC File Offset: 0x0000E9DC
	private void HoverOut()
	{
		this.IsOver = false;
	}

	// Token: 0x06001342 RID: 4930 RVA: 0x000107E5 File Offset: 0x0000E9E5
	public void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.overStateGO, this.isOver);
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.outStateGO, !this.isOver);
	}

	// Token: 0x040014F0 RID: 5360
	public GameObject outStateGO;

	// Token: 0x040014F1 RID: 5361
	public GameObject overStateGO;

	// Token: 0x040014F2 RID: 5362
	private bool isOver;

	// Token: 0x040014F3 RID: 5363
	public string SendMessageOnToggleHoverMethodName = string.Empty;
}
