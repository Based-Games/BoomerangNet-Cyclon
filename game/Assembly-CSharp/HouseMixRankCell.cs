using System;
using UnityEngine;

// Token: 0x020001CA RID: 458
public class HouseMixRankCell : MonoBehaviour
{
	// Token: 0x06000D65 RID: 3429 RVA: 0x0000BE5D File Offset: 0x0000A05D
	private void setRankInfo(RankInfo info)
	{
		this.m_RankInfo = info;
	}

	// Token: 0x06000D66 RID: 3430 RVA: 0x0005F4A0 File Offset: 0x0005D6A0
	private void Awake()
	{
		this.m_spRankingBG = base.transform.FindChild("RankingBG").GetComponent<UISprite>();
		this.m_ilRankLabel = base.transform.FindChild("Label_RankNum").GetComponent<UILabel>();
		this.m_ilUserName = base.transform.FindChild("Label_UserName").GetComponent<UILabel>();
		this.m_gScoreInfo = base.transform.FindChild("ScoreInfo").gameObject;
		this.m_lScore = this.m_gScoreInfo.transform.FindChild("Label_Score").GetComponent<UILabel>();
		this.m_txUserIcon = this.m_gScoreInfo.transform.FindChild("Texture").GetComponent<UITexture>();
		this.m_spUserIcon = this.m_gScoreInfo.transform.FindChild("UserPic").GetComponent<UISprite>();
	}

	// Token: 0x06000D67 RID: 3431 RVA: 0x0005F57C File Offset: 0x0005D77C
	public void InfoMode(bool state)
	{
		this.m_gScoreInfo.SetActive(state);
		this.m_lScore.text = this.m_RankInfo.Score.ToString();
		this.m_gScoreInfo.transform.FindChild("Texture").gameObject.SetActive(state);
		this.m_gScoreInfo.transform.FindChild("UserPic").gameObject.SetActive(state);
		if (state)
		{
			USERDATA userData = Singleton<GameManager>.instance.UserData;
			this.m_spUserIcon.spriteName = this.m_RankInfo.Icon.ToString();
			this.m_spUserIcon.MakePixelPerfect();
			this.m_spUserIcon.transform.localScale = Vector3.one;
		}
	}

	// Token: 0x06000D68 RID: 3432 RVA: 0x0005F63C File Offset: 0x0005D83C
	public void SetRankNum(string num, ImageFontLabel.FontKind_e fontkind = ImageFontLabel.FontKind_e.RankingFont_None)
	{
		if (num.Length == 1)
		{
			this.m_ilRankLabel.text = "0" + num;
		}
		else
		{
			this.m_ilRankLabel.text = num;
		}
		this.m_spRankingBG.transform.localScale = Vector3.one * 2f;
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x0000BE66 File Offset: 0x0000A066
	private void UIPress()
	{
		UIInputManager.instance.isPickObjScroll = this.m_ScrollKind;
		if (this.m_bisWorldRankCell)
		{
			this.m_HouseMixRankManager.m_bWorldAutoScroll = false;
			return;
		}
		this.m_HouseMixRankManager.m_bLocalAutoScroll = false;
	}

	// Token: 0x06000D6A RID: 3434 RVA: 0x0000BE99 File Offset: 0x0000A099
	private void UIDrag()
	{
		UIInputManager.instance.isPickObjScroll = this.m_ScrollKind;
		this.m_UIScroll.DragProcess();
	}

	// Token: 0x06000D6B RID: 3435 RVA: 0x0000BEB6 File Offset: 0x0000A0B6
	private void UIDragEnd()
	{
		UIInputManager.instance.isPickObjScroll = this.m_ScrollKind;
		this.m_UIScroll.ClickProcess();
	}

	// Token: 0x06000D6C RID: 3436 RVA: 0x0000BED3 File Offset: 0x0000A0D3
	private void UIClick()
	{
		UIInputManager.instance.isPickObjScroll = this.m_ScrollKind;
	}

	// Token: 0x04000DA2 RID: 3490
	[HideInInspector]
	public HouseMixRankManager m_HouseMixRankManager;

	// Token: 0x04000DA3 RID: 3491
	public UIScroll.ScrollKind_e m_ScrollKind = UIScroll.ScrollKind_e.None;

	// Token: 0x04000DA4 RID: 3492
	[HideInInspector]
	public UILabel m_ilRankLabel;

	// Token: 0x04000DA5 RID: 3493
	[HideInInspector]
	public UILabel m_ilUserName;

	// Token: 0x04000DA6 RID: 3494
	[HideInInspector]
	public UISprite m_spRankingBG;

	// Token: 0x04000DA7 RID: 3495
	[HideInInspector]
	public UITexture m_txFlag;

	// Token: 0x04000DA8 RID: 3496
	[HideInInspector]
	public UIScroll m_UIScroll;

	// Token: 0x04000DA9 RID: 3497
	[HideInInspector]
	public bool m_bisWorldRankCell;

	// Token: 0x04000DAA RID: 3498
	[HideInInspector]
	public GameObject m_gScoreInfo;

	// Token: 0x04000DAB RID: 3499
	[HideInInspector]
	public UILabel m_lScore;

	// Token: 0x04000DAC RID: 3500
	[HideInInspector]
	public UITexture m_txUserIcon;

	// Token: 0x04000DAD RID: 3501
	[HideInInspector]
	public UISprite m_spUserIcon;

	// Token: 0x04000DAE RID: 3502
	[HideInInspector]
	public RankInfo m_RankInfo;
}
