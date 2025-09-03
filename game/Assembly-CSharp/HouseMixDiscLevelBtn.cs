using System;
using UnityEngine;

// Token: 0x020001B8 RID: 440
public class HouseMixDiscLevelBtn : MonoBehaviour
{
	// Token: 0x06000D10 RID: 3344 RVA: 0x0000BBBF File Offset: 0x00009DBF
	private void Awake()
	{
		this.m_sEvtCnt = base.transform.FindChild("Cnt").GetComponent<UISprite>();
		this.m_sEvtCnt.gameObject.SetActive(false);
	}

	// Token: 0x06000D11 RID: 3345 RVA: 0x0000BBED File Offset: 0x00009DED
	private void SetManager(HouseMixManager sManager)
	{
		this.m_sManager = sManager;
	}

	// Token: 0x06000D12 RID: 3346 RVA: 0x0005AC0C File Offset: 0x00058E0C
	public void SetEventCount(int iCount)
	{
		if (0 >= iCount)
		{
			this.m_sEvtCnt.gameObject.SetActive(false);
			return;
		}
		this.m_sEvtCnt.gameObject.SetActive(true);
		float num;
		switch (iCount)
		{
		case 1:
			this.m_sEvtCnt.spriteName = "Cnt1";
			num = 0.2f;
			break;
		case 2:
			this.m_sEvtCnt.spriteName = "Cnt2";
			num = 0.5f;
			break;
		case 3:
			this.m_sEvtCnt.spriteName = "Cnt3";
			num = 0.8f;
			break;
		case 4:
			this.m_sEvtCnt.spriteName = "Cnt4";
			num = 1.2f;
			break;
		case 5:
			this.m_sEvtCnt.spriteName = "Cnt5";
			num = 1.4f;
			break;
		case 6:
			this.m_sEvtCnt.spriteName = "Cnt6";
			num = 1.8f;
			break;
		case 7:
			this.m_sEvtCnt.spriteName = "Cnt7";
			num = 2f;
			break;
		default:
			this.m_sEvtCnt.spriteName = "Cnt8";
			num = 2f;
			break;
		}
		TweenColor component = this.m_sEvtCnt.GetComponent<TweenColor>();
		component.tweenFactor = 1f;
		component.from = new Color(1f, 1f, 1f, 1f);
		component.to = new Color(1f, 1f, 1f, 0.2f);
		component.duration = num;
		component.style = UITweener.Style.Loop;
	}

	// Token: 0x06000D13 RID: 3347 RVA: 0x0005ADB0 File Offset: 0x00058FB0
	public void ClickProcess()
	{
		if (this.isSelect)
		{
			return;
		}
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_SONGSELECT_PATT_SEL, false);
		this.m_HouseMixSelectDiscInfo.setSelectLevel(this.m_ePtPLevel);
		HouseMixManager.LastPtType = this.m_ePtPLevel;
		this.m_sManager.SelectPtType = this.m_ePtPLevel;
	}

	// Token: 0x04000CF5 RID: 3317
	private HouseMixManager m_sManager;

	// Token: 0x04000CF6 RID: 3318
	public HouseMixSelectDiscInfo m_HouseMixSelectDiscInfo;

	// Token: 0x04000CF7 RID: 3319
	public PTLEVEL m_ePtPLevel;

	// Token: 0x04000CF8 RID: 3320
	public UISprite LevelImage;

	// Token: 0x04000CF9 RID: 3321
	public GameObject SelectAni;

	// Token: 0x04000CFA RID: 3322
	public GameObject SpinAni;

	// Token: 0x04000CFB RID: 3323
	private UISprite m_sEvtCnt;

	// Token: 0x04000CFC RID: 3324
	[HideInInspector]
	public bool isSelect;
}
