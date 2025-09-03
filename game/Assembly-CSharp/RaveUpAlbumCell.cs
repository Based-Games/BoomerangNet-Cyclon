using System;
using UnityEngine;

// Token: 0x020001FE RID: 510
public class RaveUpAlbumCell : MonoBehaviour
{
	// Token: 0x06000EC4 RID: 3780 RVA: 0x0000CC95 File Offset: 0x0000AE95
	private void Awake()
	{
		this.m_txAlbumTexture = base.transform.FindChild("Texture").GetComponent<UITexture>();
		this.m_gSelectAlbumBG = base.transform.FindChild("Sprite_selectBG").gameObject;
	}

	// Token: 0x06000EC5 RID: 3781 RVA: 0x0000CCCD File Offset: 0x0000AECD
	private void setAlbumInfo(AlbumInfo ai)
	{
		this.m_AlbumInfo = ai;
	}

	// Token: 0x06000EC6 RID: 3782 RVA: 0x0000CCD6 File Offset: 0x0000AED6
	private void setUIScroll(UIScroll scroll)
	{
		this.m_UIScroll = scroll;
	}

	// Token: 0x06000EC7 RID: 3783 RVA: 0x0000CCDF File Offset: 0x0000AEDF
	private void setRaveUpManager(RaveUpManager rum)
	{
		this.m_RaveUpManager = rum;
	}

	// Token: 0x06000EC8 RID: 3784 RVA: 0x0006A2D8 File Offset: 0x000684D8
	private void setAlbumImage()
	{
		if (this.m_txAlbumTexture == null)
		{
			this.m_txAlbumTexture = base.transform.FindChild("Texture").GetComponent<UITexture>();
		}
		Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.ALBUM_633, null, this.m_txAlbumTexture, this.m_AlbumInfo, null);
	}

	// Token: 0x06000EC9 RID: 3785 RVA: 0x0000CCE8 File Offset: 0x0000AEE8
	private void UIPress()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x0000CCFA File Offset: 0x0000AEFA
	private void UIDrag()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_UIScroll.DragProcess();
	}

	// Token: 0x06000ECB RID: 3787 RVA: 0x0000CD17 File Offset: 0x0000AF17
	private void UIDragEnd()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_UIScroll.ClickProcess();
	}

	// Token: 0x06000ECC RID: 3788 RVA: 0x0000CD34 File Offset: 0x0000AF34
	private void UIClick()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RAVEUP_ALBUM_SEL, false);
		this.ClickProcess();
	}

	// Token: 0x06000ECD RID: 3789 RVA: 0x0000CD49 File Offset: 0x0000AF49
	public void ClickProcess()
	{
		if (this.m_bisSelect)
		{
			return;
		}
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_RaveUpManager.SelectAlbum(this.m_AlbumInfo);
	}

	// Token: 0x04001021 RID: 4129
	public UIScroll.ScrollKind_e ScrollKind = UIScroll.ScrollKind_e.None;

	// Token: 0x04001022 RID: 4130
	[HideInInspector]
	public UITexture m_txAlbumTexture;

	// Token: 0x04001023 RID: 4131
	[HideInInspector]
	public GameObject m_gSelectAlbumBG;

	// Token: 0x04001024 RID: 4132
	[HideInInspector]
	public bool m_bisSelect;

	// Token: 0x04001025 RID: 4133
	private UIScroll m_UIScroll;

	// Token: 0x04001026 RID: 4134
	private RaveUpManager m_RaveUpManager;

	// Token: 0x04001027 RID: 4135
	private AlbumInfo m_AlbumInfo;
}
