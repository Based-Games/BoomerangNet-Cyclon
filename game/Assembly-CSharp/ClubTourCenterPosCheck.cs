using System;
using UnityEngine;

// Token: 0x02000185 RID: 389
public class ClubTourCenterPosCheck : MonoBehaviour
{
	// Token: 0x06000BA0 RID: 2976 RVA: 0x0000ABD0 File Offset: 0x00008DD0
	private void Awake()
	{
		this.m_spIcon = base.transform.GetChild(0).GetComponent<UISprite>();
		this.m_SelectPanel = this.m_ClubTourSpeedEff.GetComponent<UIPanel>();
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x0000ABFA File Offset: 0x00008DFA
	private void Start()
	{
		this.m_spIcon.MakePixelPerfect();
		this.m_spIcon.transform.localScale = Vector3.one * 2f;
	}

	// Token: 0x06000BA2 RID: 2978 RVA: 0x00052068 File Offset: 0x00050268
	private void ClickSelect()
	{
		if (!this.m_ClubTourSpeedEff.m_bClickSelectUse)
		{
			this.m_ClubTourSpeedEff.m_bClickSelectUse = true;
			return;
		}
		this.m_ClubTourSpeedEff.SelectEffect(this.m_iIndex);
		this.m_ClubTourSpeedEff.EffClickSelect(base.transform.localPosition);
		string name = base.gameObject.name;
		if (name != "NONE")
		{
			this.m_spIcon.spriteName = name + "_select";
		}
		this.m_spIcon.MakePixelPerfect();
		this.m_spIcon.transform.localScale = Vector3.one * 2f;
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x00052118 File Offset: 0x00050318
	private void Update()
	{
		if (this.m_SelectPanel.alpha < 1f)
		{
			return;
		}
		float y = base.transform.localPosition.y;
		float y2 = this.m_tUIScroll.localPosition.y;
		float num = Mathf.Abs(y + y2);
		string name = base.gameObject.name;
		if (num < this.m_ClubTourSpeedEff.m_v2CellSize.y * 0.5f)
		{
			if (name + "_select" != this.m_spIcon.spriteName)
			{
				if (this.m_ClubTourSpeedEff.m_iCenterIndex != this.m_iIndex)
				{
					Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_SONGSELECT_EFFECT_SCROLL, false);
				}
				this.m_ClubTourSpeedEff.SelectEffect(this.m_iIndex);
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

	// Token: 0x06000BA4 RID: 2980 RVA: 0x0000AC26 File Offset: 0x00008E26
	private void UIPress()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_ClubTourSpeedEff.SendMessage("PressProcess");
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x0000AC48 File Offset: 0x00008E48
	private void UIClick()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.ClickSelect();
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x0000AC60 File Offset: 0x00008E60
	private void UIDrag()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_tUIScroll.SendMessage("DragProcess");
		this.m_ClubTourSpeedEff.SendMessage("DragProcess");
	}

	// Token: 0x06000BA7 RID: 2983 RVA: 0x0000AC92 File Offset: 0x00008E92
	private void UIDragEnd()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_tUIScroll.SendMessage("ClickProcess");
		this.m_ClubTourSpeedEff.SendMessage("DragEndProcess");
		this.m_ClubTourSpeedEff.m_bClickSelectUse = true;
	}

	// Token: 0x04000B24 RID: 2852
	public int m_iIndex;

	// Token: 0x04000B25 RID: 2853
	public ClubTourSpeedEff m_ClubTourSpeedEff;

	// Token: 0x04000B26 RID: 2854
	public Transform m_tUIScroll;

	// Token: 0x04000B27 RID: 2855
	private UIPanel m_SelectPanel;

	// Token: 0x04000B28 RID: 2856
	[HideInInspector]
	public UISprite m_spIcon;

	// Token: 0x04000B29 RID: 2857
	private UIScroll.ScrollKind_e ScrollKind = UIScroll.ScrollKind_e.Vertical;
}
