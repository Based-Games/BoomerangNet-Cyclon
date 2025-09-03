using System;
using UnityEngine;

// Token: 0x020001BB RID: 443
public class HouseMixGraphPanelAni : MonoBehaviour
{
	// Token: 0x06000D20 RID: 3360 RVA: 0x0000BC52 File Offset: 0x00009E52
	private void Start()
	{
		this.m_UIPanel = base.GetComponent<UIPanel>();
	}

	// Token: 0x06000D21 RID: 3361 RVA: 0x0005B618 File Offset: 0x00059818
	public void initAni()
	{
		this.PanelAni.enabled = false;
		if (this.m_MoveKind == HouseMixGraphPanelAni.MoveKind_e.Horizontal)
		{
			this.m_UIPanel.baseClipRegion = new Vector4(this.PanelAni.from.x, this.m_UIPanel.baseClipRegion.y, this.m_UIPanel.baseClipRegion.z, this.m_UIPanel.baseClipRegion.w);
		}
		else if (this.m_MoveKind == HouseMixGraphPanelAni.MoveKind_e.Vertical)
		{
			this.m_UIPanel.baseClipRegion = new Vector4(this.m_UIPanel.baseClipRegion.x, this.PanelAni.from.y, this.m_UIPanel.baseClipRegion.z, this.m_UIPanel.baseClipRegion.w);
		}
	}

	// Token: 0x06000D22 RID: 3362 RVA: 0x0000BC60 File Offset: 0x00009E60
	public void AniStart()
	{
		this.initAni();
		this.isStart = true;
		this.PanelAni.Play(true);
	}

	// Token: 0x06000D23 RID: 3363 RVA: 0x0005B704 File Offset: 0x00059904
	private void Update()
	{
		if (!this.isStart)
		{
			return;
		}
		if (this.m_MoveKind == HouseMixGraphPanelAni.MoveKind_e.Horizontal)
		{
			if (this.PanelAni.to.x == this.m_UIPanel.baseClipRegion.x)
			{
				this.isStart = false;
			}
			else
			{
				this.m_UIPanel.baseClipRegion = new Vector4(this.PanelAni.transform.localPosition.x, this.m_UIPanel.baseClipRegion.y, this.m_UIPanel.baseClipRegion.z, this.m_UIPanel.baseClipRegion.w);
			}
		}
		else if (this.m_MoveKind == HouseMixGraphPanelAni.MoveKind_e.Vertical)
		{
			if (this.PanelAni.to.y == this.m_UIPanel.baseClipRegion.y)
			{
				this.isStart = false;
			}
			else
			{
				this.m_UIPanel.baseClipRegion = new Vector4(this.m_UIPanel.baseClipRegion.x, this.PanelAni.transform.localPosition.y, this.m_UIPanel.baseClipRegion.z, this.m_UIPanel.baseClipRegion.w);
			}
		}
	}

	// Token: 0x04000D16 RID: 3350
	public HouseMixGraphPanelAni.MoveKind_e m_MoveKind;

	// Token: 0x04000D17 RID: 3351
	public TweenPosition PanelAni;

	// Token: 0x04000D18 RID: 3352
	public UIPanel m_UIPanel;

	// Token: 0x04000D19 RID: 3353
	[HideInInspector]
	public bool isStart;

	// Token: 0x020001BC RID: 444
	public enum MoveKind_e
	{
		// Token: 0x04000D1B RID: 3355
		Horizontal,
		// Token: 0x04000D1C RID: 3356
		Vertical,
		// Token: 0x04000D1D RID: 3357
		None
	}
}
