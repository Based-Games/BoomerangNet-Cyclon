using System;
using UnityEngine;

// Token: 0x020001B6 RID: 438
public class HouseMixCell : MonoBehaviour
{
	// Token: 0x06000CF6 RID: 3318 RVA: 0x0005A74C File Offset: 0x0005894C
	private void Awake()
	{
		this.m_txDiscPic = base.transform.FindChild("Texture_main").GetComponent<UITexture>();
		this.m_gSelectBG = base.transform.FindChild("selectbg").gameObject;
		base.transform.FindChild("New").gameObject.SetActive(false);
	}

	// Token: 0x06000CF7 RID: 3319 RVA: 0x0000B9C0 File Offset: 0x00009BC0
	private void Start()
	{
		if (Singleton<SongManager>.instance.IsContainEventPt(this.m_iSongID))
		{
			base.transform.FindChild("New").gameObject.SetActive(true);
		}
	}

	// Token: 0x06000CF8 RID: 3320 RVA: 0x0000B9EF File Offset: 0x00009BEF
	public void SetManager(HouseMixManager sManager)
	{
		this.m_sManager = sManager;
	}

	// Token: 0x06000CF9 RID: 3321 RVA: 0x0000B9F8 File Offset: 0x00009BF8
	public void SetDiscInfo(DiscInfo dInfo)
	{
		this.m_sDiscInfo = dInfo;
	}

	// Token: 0x06000CFA RID: 3322 RVA: 0x0000BA01 File Offset: 0x00009C01
	public void SetHouseStageInfo(HouseStage sInfo)
	{
		this.m_sStageInfo = sInfo;
	}

	// Token: 0x06000CFB RID: 3323 RVA: 0x0000BA0A File Offset: 0x00009C0A
	private void UIPress()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
	}

	// Token: 0x06000CFC RID: 3324 RVA: 0x0000BA1C File Offset: 0x00009C1C
	private void UIDrag()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_UIScroll.DragProcess();
	}

	// Token: 0x06000CFD RID: 3325 RVA: 0x0000BA39 File Offset: 0x00009C39
	private void UIDragEnd()
	{
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_UIScroll.ClickProcess();
	}

	// Token: 0x06000CFE RID: 3326 RVA: 0x0000BA56 File Offset: 0x00009C56
	private void UIClick()
	{
		this.ClickEvent(true);
	}

	// Token: 0x06000CFF RID: 3327 RVA: 0x0005A7AC File Offset: 0x000589AC
	public void ClickEvent(bool isTouch = false)
	{
		if (isTouch)
		{
			if (this.m_sManager.SelectDisc.Id == this.m_sDiscInfo.Id)
			{
				return;
			}
			Singleton<SoundSourceManager>.instance.StopBgm();
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_SONGSELECT_TOUCH_SONG, false);
		}
		UIInputManager.instance.isPickObjScroll = this.ScrollKind;
		this.m_sManager.SelectDisc = this.m_sDiscInfo;
		this.m_sManager.SelectStage = this.m_sStageInfo;
		this.m_sManager.SelectPtType = this.m_sStageInfo.GetFirstLv();
		HouseMixManager.LastDiscID = this.m_sDiscInfo.Id;
		this.m_sManager.PressDisc();
		this.m_HouseMixManager.m_gSelectObj = base.gameObject;
		this.m_SelectTag.from = this.m_SelectTag.transform.localPosition;
		this.m_SelectTag.to = new Vector3(base.transform.localPosition.x, this.m_SelectTag.transform.localPosition.y, this.m_SelectTag.transform.localPosition.z);
		this.m_SelectTag.ResetToBeginning();
		this.m_SelectTag.Play(true);
	}

	// Token: 0x06000D00 RID: 3328 RVA: 0x0000BA5F File Offset: 0x00009C5F
	public void StartDiscAlphaAni(bool st)
	{
		this.m_bAlphaPlayState = st;
		base.GetComponent<TweenAlphaContorl>().PlayObj(this.m_bAlphaPlayState);
	}

	// Token: 0x06000D01 RID: 3329 RVA: 0x0000BA79 File Offset: 0x00009C79
	public void ShowDiscAlphaAni()
	{
		base.GetComponent<TweenAlphaContorl>().PlayObj(this.m_bAlphaPlayState);
	}

	// Token: 0x06000D02 RID: 3330 RVA: 0x0000BA8C File Offset: 0x00009C8C
	public void EndDiscAlphaAni()
	{
		if (this.m_bAlphaPlayState)
		{
			this.m_bAlphaPlayState = false;
			this.m_HouseMixManager.SortStart();
			return;
		}
		this.m_HouseMixManager.EndSort();
	}

	// Token: 0x06000D03 RID: 3331 RVA: 0x0005A8E4 File Offset: 0x00058AE4
	private void Update()
	{
		if (this.m_gSelectBG.activeSelf && this.m_HouseMixSelectDiscInfo.m_txSelectDisc.mainTexture == null)
		{
			this.m_HouseMixSelectDiscInfo.m_txSelectDisc.mainTexture = this.m_txDiscPic.mainTexture;
		}
	}

	// Token: 0x04000CD4 RID: 3284
	public UIScroll.ScrollKind_e ScrollKind = UIScroll.ScrollKind_e.None;

	// Token: 0x04000CD5 RID: 3285
	private HouseMixManager m_sManager;

	// Token: 0x04000CD6 RID: 3286
	public DiscInfo m_sDiscInfo;

	// Token: 0x04000CD7 RID: 3287
	private HouseStage m_sStageInfo;

	// Token: 0x04000CD8 RID: 3288
	[HideInInspector]
	public UIScroll m_UIScroll;

	// Token: 0x04000CD9 RID: 3289
	[HideInInspector]
	public string m_sDiscMusicFileName;

	// Token: 0x04000CDA RID: 3290
	[HideInInspector]
	public HouseMixSelectDiscInfo m_HouseMixSelectDiscInfo;

	// Token: 0x04000CDB RID: 3291
	[HideInInspector]
	public HouseMixManager m_HouseMixManager;

	// Token: 0x04000CDC RID: 3292
	[HideInInspector]
	public string m_sDiscName = string.Empty;

	// Token: 0x04000CDD RID: 3293
	[HideInInspector]
	public string m_sDiscArtist = string.Empty;

	// Token: 0x04000CDE RID: 3294
	[HideInInspector]
	public int m_iLevelDifficult;

	// Token: 0x04000CDF RID: 3295
	[HideInInspector]
	public int m_iAverageLevelDifficult;

	// Token: 0x04000CE0 RID: 3296
	[HideInInspector]
	public int m_iDiscID;

	// Token: 0x04000CE1 RID: 3297
	[HideInInspector]
	public int m_iSongID;

	// Token: 0x04000CE2 RID: 3298
	[HideInInspector]
	public string m_sSongKind;

	// Token: 0x04000CE3 RID: 3299
	[HideInInspector]
	public TweenPosition m_SelectTag;

	// Token: 0x04000CE4 RID: 3300
	[HideInInspector]
	public UITexture m_txDiscPic;

	// Token: 0x04000CE5 RID: 3301
	[HideInInspector]
	public GameObject m_gSelectBG;

	// Token: 0x04000CE6 RID: 3302
	[HideInInspector]
	public string m_sDiscImageName;

	// Token: 0x04000CE7 RID: 3303
	private bool m_bisTouch;

	// Token: 0x04000CE8 RID: 3304
	private bool m_bAlphaPlayState;

	// Token: 0x04000CE9 RID: 3305
	private bool m_bTweenAniState;

	// Token: 0x04000CEA RID: 3306
	private float m_fClickFrame;

	// Token: 0x04000CEB RID: 3307
	private float m_fClickDelayTime = 0.5f;
}
