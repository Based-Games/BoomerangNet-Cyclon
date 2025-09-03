using System;
using UnityEngine;

// Token: 0x020001B7 RID: 439
public class HouseMixCenterPosCheck : MonoBehaviour
{
	// Token: 0x06000D05 RID: 3333 RVA: 0x0005A934 File Offset: 0x00058B34
	private void Awake()
	{
		this.m_spIcon = base.transform.FindChild("Sprite_Icon").GetComponent<UISprite>();
		this.m_lCount = base.transform.FindChild("Label_Count").GetComponent<UILabel>();
		this.m_Widget = base.GetComponent<UIWidget>();
	}

	// Token: 0x06000D06 RID: 3334 RVA: 0x0000BAC3 File Offset: 0x00009CC3
	public void setEffManager(EffManager fm)
	{
		this.m_EffManager = fm;
		this.m_SelectPanel = this.m_EffManager.GetComponent<UIPanel>();
	}

	// Token: 0x06000D07 RID: 3335 RVA: 0x0005A984 File Offset: 0x00058B84
	public void setCount(int count)
	{
		this.m_lCount.gameObject.SetActive(false);
		if (count <= 1)
		{
			return;
		}
		this.m_lCount.gameObject.SetActive(true);
		if (count > 99)
		{
			this.m_lCount.text = "99";
		}
		else
		{
			this.m_lCount.text = count.ToString();
		}
	}

	// Token: 0x06000D08 RID: 3336 RVA: 0x0000BADD File Offset: 0x00009CDD
	public void setImage(string name)
	{
		this.m_spIcon.spriteName = name;
		this.m_spIcon.MakePixelPerfect();
		this.m_spIcon.transform.localScale = Vector3.one * 2f;
	}

	// Token: 0x06000D09 RID: 3337 RVA: 0x0005A9EC File Offset: 0x00058BEC
	private void ClickSelect()
	{
		if (!this.m_EffManager.m_bClickSelectUse)
		{
			this.m_EffManager.m_bClickSelectUse = true;
			return;
		}
		if (this.m_UIScroll.GetComponent<TweenPosition>().enabled)
		{
			return;
		}
		this.m_EffManager.SelectEffect(this.m_iIndex);
		this.m_EffManager.EffClickSelect(base.transform);
		string name = base.gameObject.name;
		if (name != "NONE")
		{
			this.m_spIcon.spriteName = name + "_select";
		}
		this.m_spIcon.MakePixelPerfect();
		this.m_spIcon.transform.localScale = Vector3.one * 2f;
	}

	// Token: 0x06000D0A RID: 3338 RVA: 0x0005AAAC File Offset: 0x00058CAC
	private void Update()
	{
		this.m_Widget.alpha = this.m_SelectPanel.alpha;
		if (this.m_SelectPanel.alpha < 1f)
		{
			return;
		}
		float y = base.transform.localPosition.y;
		float y2 = this.m_UIScroll.transform.localPosition.y;
		float num = Mathf.Abs(y + y2);
		string name = base.gameObject.name;
		if (num < this.m_EffManager.m_v2CellSize.y * 0.5f)
		{
			if (name + "_select" != this.m_spIcon.spriteName)
			{
				if (this.m_EffManager.m_iCenterIndex != this.m_iIndex)
				{
					Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_SONGSELECT_EFFECT_SCROLL, false);
				}
				this.m_EffManager.SelectEffect(this.m_iIndex);
				if (name != "NONE")
				{
					this.m_spIcon.spriteName = name + "_select";
				}
			}
		}
		else if (name != "NONE")
		{
			this.m_spIcon.spriteName = name;
		}
		this.m_spIcon.MakePixelPerfect();
		this.m_spIcon.transform.localScale = Vector3.one * 2f;
	}

	// Token: 0x06000D0B RID: 3339 RVA: 0x0000BB15 File Offset: 0x00009D15
	private void UIPress()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_EffManager.SendMessage("PressProcess");
	}

	// Token: 0x06000D0C RID: 3340 RVA: 0x0000BB37 File Offset: 0x00009D37
	private void UIClick()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.ClickSelect();
	}

	// Token: 0x06000D0D RID: 3341 RVA: 0x0000BB4F File Offset: 0x00009D4F
	private void UIDrag()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_UIScroll.SendMessage("DragProcess");
		this.m_EffManager.SendMessage("DragProcess");
	}

	// Token: 0x06000D0E RID: 3342 RVA: 0x0000BB81 File Offset: 0x00009D81
	private void UIDragEnd()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_UIScroll.SendMessage("ClickProcess");
		this.m_EffManager.SendMessage("DragEndProcess");
		this.m_EffManager.m_bClickSelectUse = true;
	}

	// Token: 0x04000CEC RID: 3308
	public int m_iIndex;

	// Token: 0x04000CED RID: 3309
	[HideInInspector]
	public EffManager m_EffManager;

	// Token: 0x04000CEE RID: 3310
	[HideInInspector]
	public UISprite m_spIcon;

	// Token: 0x04000CEF RID: 3311
	[HideInInspector]
	public UIScroll m_UIScroll;

	// Token: 0x04000CF0 RID: 3312
	private UIPanel m_SelectPanel;

	// Token: 0x04000CF1 RID: 3313
	private UIScroll.ScrollKind_e ScrollKind = UIScroll.ScrollKind_e.Vertical;

	// Token: 0x04000CF2 RID: 3314
	private UILabel m_lCount;

	// Token: 0x04000CF3 RID: 3315
	private UIWidget m_Widget;

	// Token: 0x04000CF4 RID: 3316
	private bool m_bDragNow;
}
