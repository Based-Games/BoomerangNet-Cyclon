using System;
using UnityEngine;

// Token: 0x020001E9 RID: 489
public class ResultRank : MonoBehaviour
{
	// Token: 0x06000E2F RID: 3631 RVA: 0x00065BF8 File Offset: 0x00063DF8
	private void Awake()
	{
		this.m_OldRankPanel = base.transform.FindChild("beforeRankPanel").GetComponent<TweenAlpha>();
		this.m_RankArrowPanel = base.transform.FindChild("beforeRankArrowPanel").GetComponent<TweenAlpha>();
		this.m_NowRankPanel = base.transform.FindChild("RankImagePanel").GetComponent<TweenScale>();
		this.m_NowRank = this.m_NowRankPanel.transform.FindChild("NowRank").GetComponent<TweenScale>();
		this.m_spNewRank = this.m_NowRankPanel.transform.FindChild("NowRank").GetComponent<UISprite>();
		this.m_spOldRank = base.transform.FindChild("beforeRankPanel").FindChild("beforeRank").GetComponent<UISprite>();
		this.m_gRankPlus = new GameObject[2];
		this.m_gRankPlusEff = new GameObject[2];
		this.m_gOldRankPlus = new GameObject[2];
		for (int i = 0; i < this.m_gRankPlus.Length; i++)
		{
			this.m_gRankPlus[i] = base.transform.FindChild("Plus_" + (i + 1).ToString()).FindChild("Sprite_RankPlus_" + (i + 1).ToString()).gameObject;
			this.m_gRankPlusEff[i] = base.transform.FindChild("Sprite_RankPlus_" + (i + 1).ToString() + "_Eff").gameObject;
			this.m_gOldRankPlus[i] = this.m_OldRankPanel.transform.FindChild("Sprite_Plus_" + (i + 1).ToString()).gameObject;
		}
		this.m_tRankArrow = this.m_RankArrowPanel.transform.FindChild("Sprite_UpDownArrow");
		this.m_gRankEff = base.transform.FindChild("NowRank_eff").gameObject;
		this.m_StayNowRankEff = base.transform.FindChild("Stay_NowRank").GetComponent<TweenAlpha>();
		this.m_StayPlus_1eff = base.transform.FindChild("Stay_RankPlus_1").GetComponent<TweenAlpha>();
		this.m_StayPlus_2eff = base.transform.FindChild("Stay_RankPlus_2").GetComponent<TweenAlpha>();
		switch (this.m_ResultKind)
		{
		case ResultKind_e.AllClearResult:
			this.m_ResultAllClearPlayResult = base.transform.parent.GetComponent<ResultAllClearPlayResult>();
			break;
		case ResultKind_e.RaveUpClearResult:
			this.m_RaveUpResult = base.transform.parent.GetComponent<RaveUpResult>();
			break;
		}
	}

	// Token: 0x06000E30 RID: 3632 RVA: 0x00065E90 File Offset: 0x00064090
	public void DefualtRankSetting()
	{
		this.m_iNowResultRankIndex = 9;
		this.m_iOldResultRankIndex = 3;
		if (this.m_ResultKind != ResultKind_e.AllClearResult && this.m_ResultKind != ResultKind_e.ClubTourClearResult)
		{
			this.RankImageSetting(this.m_iOldResultRankIndex, this.m_spOldRank, this.m_gOldRankPlus, "Result_OldRank_", null, null, false);
			this.m_spOldRank.MakePixelPerfect();
			this.m_spOldRank.transform.localScale = this.m_v3BaseScale;
		}
		this.RankImageSetting(this.m_iNowResultRankIndex, this.m_spNewRank, this.m_gRankPlus, "Result_Rank_", this.m_gRankEff.GetComponent<UISprite>(), this.m_StayNowRankEff.GetComponent<UISprite>(), true);
		this.m_spNewRank.MakePixelPerfect();
		this.m_gRankEff.GetComponent<UISprite>().MakePixelPerfect();
		this.m_gRankEff.transform.localScale = this.m_v3BaseScale;
		this.m_tRankArrow.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		this.m_tRankArrow.GetComponent<UISprite>().spriteName = "result_rank_up";
		if (this.m_iOldResultRankIndex == -1)
		{
			this.m_tRankArrow.gameObject.SetActive(false);
		}
		else
		{
			if (this.m_iNowResultRankIndex < this.m_iOldResultRankIndex)
			{
				this.m_tRankArrow.GetComponent<UISprite>().spriteName = "result_rank_down";
			}
			else if (this.m_iNowResultRankIndex == this.m_iOldResultRankIndex)
			{
				this.m_tRankArrow.GetComponent<UISprite>().spriteName = "result_arrow_none";
			}
			this.m_tRankArrow.GetComponent<UISprite>().MakePixelPerfect();
			this.m_tRankArrow.localScale = this.m_v3BaseScale;
		}
	}

	// Token: 0x06000E31 RID: 3633 RVA: 0x00066038 File Offset: 0x00064238
	public void RankSetting()
	{
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
		this.m_iOldResultRankIndex = -1;
		switch (this.m_ResultKind)
		{
		case ResultKind_e.ClearResult:
		{
			PatternScoreInfo patternScoreInfo = Singleton<GameManager>.instance.netHouseMixData.DicPatternInfo[resultData.PTTYPE];
			this.m_iNowResultRankIndex = (int)resultData.GRADETYPE;
			this.m_iOldResultRankIndex = (int)patternScoreInfo.RankClass;
			this.RankImageSetting(this.m_iOldResultRankIndex, this.m_spOldRank, this.m_gOldRankPlus, "Result_OldRank_", null, null, false);
			this.m_spOldRank.MakePixelPerfect();
			this.m_spOldRank.transform.localScale = this.m_v3BaseScale;
			break;
		}
		case ResultKind_e.AllClearResult:
			this.m_iNowResultRankIndex = (int)resultData.TOTALGRADETYPE;
			this.m_ResultAllClearPlayResult.m_AllClearPostInfo.m_iNowResultRankIndex = this.m_iNowResultRankIndex;
			break;
		case ResultKind_e.RaveUpClearResult:
			this.m_iNowResultRankIndex = (int)resultData.GRADETYPE;
			this.m_iOldResultRankIndex = (int)Singleton<GameManager>.instance.netUserRecordData.RankClass;
			break;
		case ResultKind_e.ClubTourClearResult:
			this.m_iNowResultRankIndex = (int)resultData.GRADETYPE;
			this.m_iOldResultRankIndex = (int)Singleton<GameManager>.instance.netUserRecordData.RankClass;
			break;
		}
		if (this.m_ResultKind != ResultKind_e.AllClearResult && this.m_ResultKind != ResultKind_e.ClubTourClearResult)
		{
			this.RankImageSetting(this.m_iOldResultRankIndex, this.m_spOldRank, this.m_gOldRankPlus, "Result_OldRank_", null, null, false);
			this.m_spOldRank.MakePixelPerfect();
			this.m_spOldRank.transform.localScale = this.m_v3BaseScale;
		}
		this.RankImageSetting(this.m_iNowResultRankIndex, this.m_spNewRank, this.m_gRankPlus, "Result_Rank_", this.m_gRankEff.GetComponent<UISprite>(), this.m_StayNowRankEff.GetComponent<UISprite>(), true);
		this.m_spNewRank.MakePixelPerfect();
		this.m_gRankEff.GetComponent<UISprite>().MakePixelPerfect();
		this.m_gRankEff.transform.localScale = this.m_v3BaseScale;
		this.m_tRankArrow.transform.localEulerAngles = Vector3.zero;
		this.m_tRankArrow.GetComponent<UISprite>().spriteName = "result_rank_up";
		if (this.m_iOldResultRankIndex == -1 || !Singleton<GameManager>.instance.ONLOGIN)
		{
			this.m_tRankArrow.gameObject.SetActive(false);
			this.m_spOldRank.gameObject.SetActive(false);
		}
		else
		{
			if (this.m_iNowResultRankIndex < this.m_iOldResultRankIndex)
			{
				this.m_tRankArrow.GetComponent<UISprite>().spriteName = "result_rank_down";
			}
			else if (this.m_iNowResultRankIndex == this.m_iOldResultRankIndex)
			{
				this.m_tRankArrow.GetComponent<UISprite>().spriteName = "result_arrow_none";
			}
			this.m_tRankArrow.GetComponent<UISprite>().MakePixelPerfect();
			this.m_tRankArrow.localScale = this.m_v3BaseScale;
		}
	}

	// Token: 0x06000E32 RID: 3634 RVA: 0x00066304 File Offset: 0x00064504
	private void RankImageSetting(int RankIndex, UISprite MainSprite, GameObject[] Plus = null, string SpriteName = "Result_Rank_", UISprite MainSpriteEff = null, UISprite m_StayNowRankEff = null, bool isLineRank = false)
	{
		this.m_iPlusCount = 0;
		int num = 0;
		if (RankIndex >= 4 && RankIndex <= 6)
		{
			if (MainSpriteEff != null)
			{
				MainSpriteEff.spriteName = SpriteName + 4.ToString();
				if (isLineRank)
				{
					MainSprite.spriteName = SpriteName + 4.ToString() + "_line";
				}
				else
				{
					MainSprite.spriteName = SpriteName + 4.ToString();
				}
			}
			else if (isLineRank)
			{
				MainSprite.spriteName = SpriteName + 4.ToString() + "_line";
			}
			else
			{
				MainSprite.spriteName = SpriteName + 4.ToString();
			}
			if (m_StayNowRankEff != null)
			{
				m_StayNowRankEff.GetComponent<UISprite>().spriteName = SpriteName + 4.ToString();
			}
			num = 4;
			this.m_iPlusCount = RankIndex - 4;
		}
		else if (RankIndex >= 7 && RankIndex <= 9)
		{
			if (this.m_gRankEff != null)
			{
				this.m_gRankEff.GetComponent<UISprite>().spriteName = SpriteName + 7.ToString();
				if (isLineRank)
				{
					MainSprite.spriteName = SpriteName + 7.ToString() + "_line";
				}
				else
				{
					MainSprite.spriteName = SpriteName + 7.ToString();
				}
			}
			else if (isLineRank)
			{
				MainSprite.spriteName = SpriteName + 7.ToString() + "_line";
			}
			else
			{
				MainSprite.spriteName = SpriteName + 7.ToString();
			}
			if (m_StayNowRankEff != null)
			{
				m_StayNowRankEff.GetComponent<UISprite>().spriteName = SpriteName + 7.ToString();
			}
			num = 7;
			this.m_iPlusCount = RankIndex - 7;
		}
		else
		{
			if (this.m_gRankEff != null)
			{
				this.m_gRankEff.GetComponent<UISprite>().spriteName = SpriteName + RankIndex.ToString();
				if (isLineRank)
				{
					MainSprite.spriteName = SpriteName + RankIndex.ToString() + "_line";
				}
				else
				{
					MainSprite.spriteName = SpriteName + RankIndex.ToString();
				}
			}
			else if (isLineRank)
			{
				MainSprite.spriteName = SpriteName + RankIndex.ToString() + "_line";
			}
			else
			{
				MainSprite.spriteName = SpriteName + RankIndex.ToString();
			}
			if (m_StayNowRankEff != null)
			{
				m_StayNowRankEff.GetComponent<UISprite>().spriteName = SpriteName + RankIndex.ToString();
			}
		}
		if (Plus != null)
		{
			for (int i = 0; i < Plus.Length; i++)
			{
				Plus[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < this.m_iPlusCount; j++)
			{
				Plus[j].gameObject.SetActive(true);
				if (SpriteName == "MyRecord_Rank_")
				{
					Plus[j].GetComponent<UISprite>().spriteName = "Plus_" + num.ToString();
					Plus[j].GetComponent<UISprite>().MakePixelPerfect();
					Plus[j].transform.localScale = Vector3.one * 2f;
				}
			}
		}
	}

	// Token: 0x06000E33 RID: 3635 RVA: 0x0000C5FE File Offset: 0x0000A7FE
	public void StartAni()
	{
		this.m_AniStep = ResultManager.StepKind_e.Step1;
	}

	// Token: 0x06000E34 RID: 3636 RVA: 0x0006667C File Offset: 0x0006487C
	private void RankStep()
	{
		if (this.m_iNowResultRankIndex == 0)
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_RANK_F, false);
		}
		else if (this.m_iNowResultRankIndex >= 4 && this.m_iNowResultRankIndex < 7)
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_RANK_A, false);
		}
		else if (this.m_iNowResultRankIndex >= 7 && this.m_iNowResultRankIndex < 10)
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_RANK_S, false);
		}
		else
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_RANK_B_D, false);
		}
		this.m_NowRankPanel.enabled = true;
		this.m_NowRank.enabled = true;
		this.m_gRankEff.GetComponent<TweenScale>().enabled = true;
		this.m_gRankEff.GetComponent<TweenAlpha>().enabled = true;
		this.m_StayNowRankEff.enabled = true;
		if (this.m_ResultKind != ResultKind_e.AllClearResult && this.m_ResultKind != ResultKind_e.ClubTourClearResult)
		{
			base.Invoke("OldRankStep", 1f);
		}
	}

	// Token: 0x06000E35 RID: 3637 RVA: 0x00066778 File Offset: 0x00064978
	private void RankPlusStep()
	{
		if (this.m_iNowPlusIndex >= 2)
		{
			return;
		}
		this.m_gRankPlus[this.m_iNowPlusIndex].GetComponent<TweenScale>().enabled = true;
		this.m_gRankPlus[this.m_iNowPlusIndex].transform.parent.GetComponent<TweenScale>().enabled = true;
		this.m_gRankPlusEff[this.m_iNowPlusIndex].GetComponent<TweenScale>().enabled = true;
		this.m_gRankPlusEff[this.m_iNowPlusIndex].GetComponent<TweenAlpha>().enabled = true;
		this.m_iNowPlusIndex++;
	}

	// Token: 0x06000E36 RID: 3638 RVA: 0x0000C607 File Offset: 0x0000A807
	private void OldRankStep()
	{
		this.m_OldRankPanel.enabled = true;
		this.m_RankArrowPanel.enabled = true;
	}

	// Token: 0x06000E37 RID: 3639 RVA: 0x0006680C File Offset: 0x00064A0C
	private void Update()
	{
		if (this.m_bAniEndState)
		{
			return;
		}
		switch (this.m_AniStep)
		{
		case ResultManager.StepKind_e.Step1:
		{
			this.RankStep();
			for (int i = 0; i < this.m_iPlusCount; i++)
			{
				base.Invoke("RankPlusStep", this.m_fRankAniTime + this.m_fPlusAniDelayTime * (float)(i + 1));
			}
			this.m_bAniEndState = true;
			break;
		}
		}
	}

	// Token: 0x04000F1E RID: 3870
	public ResultKind_e m_ResultKind;

	// Token: 0x04000F1F RID: 3871
	private ResultManager.StepKind_e m_AniStep = ResultManager.StepKind_e.None;

	// Token: 0x04000F20 RID: 3872
	private UISprite m_spNewRank;

	// Token: 0x04000F21 RID: 3873
	private UISprite m_spOldRank;

	// Token: 0x04000F22 RID: 3874
	private GameObject[] m_gRankPlus;

	// Token: 0x04000F23 RID: 3875
	private GameObject[] m_gRankPlusEff;

	// Token: 0x04000F24 RID: 3876
	private GameObject[] m_gOldRankPlus;

	// Token: 0x04000F25 RID: 3877
	private int m_iNowPlusIndex;

	// Token: 0x04000F26 RID: 3878
	private int m_iPlusCount;

	// Token: 0x04000F27 RID: 3879
	private Transform m_tRankArrow;

	// Token: 0x04000F28 RID: 3880
	private GameObject m_gRankEff;

	// Token: 0x04000F29 RID: 3881
	private int m_iNowResultRankIndex;

	// Token: 0x04000F2A RID: 3882
	private int m_iOldResultRankIndex;

	// Token: 0x04000F2B RID: 3883
	private TweenAlpha m_StayNowRankEff;

	// Token: 0x04000F2C RID: 3884
	private TweenAlpha m_StayPlus_1eff;

	// Token: 0x04000F2D RID: 3885
	private TweenAlpha m_StayPlus_2eff;

	// Token: 0x04000F2E RID: 3886
	private TweenAlpha m_OldRankPanel;

	// Token: 0x04000F2F RID: 3887
	private TweenAlpha m_RankArrowPanel;

	// Token: 0x04000F30 RID: 3888
	private TweenScale m_NowRankPanel;

	// Token: 0x04000F31 RID: 3889
	private TweenScale m_NowRank;

	// Token: 0x04000F32 RID: 3890
	private bool m_bAniEndState;

	// Token: 0x04000F33 RID: 3891
	private float m_fRankAniTime = 0.25f;

	// Token: 0x04000F34 RID: 3892
	private float m_fPlusAniDelayTime = 0.15f;

	// Token: 0x04000F35 RID: 3893
	private Vector3 m_v3BaseScale = new Vector3(2f, 2f, 1f);

	// Token: 0x04000F36 RID: 3894
	private RaveUpResult m_RaveUpResult;

	// Token: 0x04000F37 RID: 3895
	private ResultAllClearPlayResult m_ResultAllClearPlayResult;
}
