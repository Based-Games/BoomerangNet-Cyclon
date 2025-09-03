using System;
using UnityEngine;

// Token: 0x020001E3 RID: 483
public class ResultExp : MonoBehaviour
{
	// Token: 0x06000E00 RID: 3584 RVA: 0x0000C47C File Offset: 0x0000A67C
	public bool GetAniEndState()
	{
		return this.m_bAniEndState;
	}

	// Token: 0x06000E01 RID: 3585 RVA: 0x000642B0 File Offset: 0x000624B0
	private void Awake()
	{
		this.m_lLevel = base.transform.FindChild("LevelMark").FindChild("ANI").FindChild("Label_UserLevel")
			.GetComponent<ImageFontLabel>();
		Transform transform = base.transform.FindChild("Bar");
		this.m_tExpAddStartLine = transform.FindChild("Sprite_StartExpLine");
		this.m_gExpAddLevelUpPoint = this.m_tExpAddStartLine.FindChild("Sprite_LevelUp").gameObject;
		this.m_ilAddExp = this.m_tExpAddStartLine.FindChild("text").GetComponent<ImageFontLabel>();
		this.m_ExpBar = transform.FindChild("Panel_exp").GetComponent<UIPanel>();
		this.m_gExpEff = this.m_ExpBar.transform.FindChild("expbarFilleff").gameObject;
		this.m_FinishExpBar = transform.FindChild("Panel_addexp").GetComponent<UIPanel>();
		this.m_AddExpAni = base.transform.FindChild("numAni_add").GetComponent<TweenPosition>();
		this.m_lNowExp = base.transform.FindChild("Label_nowAndMaxexp").GetComponent<UILabel>();
		this.m_LevelUpBG = base.transform.FindChild("LevelUpBG").GetComponent<TweenAlpha>();
		this.m_LevelMarkAni = base.transform.FindChild("LevelMark").FindChild("ANI").GetComponent<TweenScale>();
		this.m_gLevelMarkAniEff = base.transform.FindChild("LevelMark").FindChild("StayAniEff").gameObject;
		switch (this.m_ResultKind)
		{
		case ResultKind_e.ClearResult:
			this.m_ResultPlayResult = base.transform.parent.GetComponent<ResultPlayResult>();
			break;
		case ResultKind_e.AllClearResult:
			this.m_ResultAllClearPlayResult = base.transform.parent.GetComponent<ResultAllClearPlayResult>();
			break;
		case ResultKind_e.RaveUpClearResult:
			this.m_RaveUpResult = base.transform.parent.GetComponent<RaveUpResult>();
			break;
		case ResultKind_e.ClubTourClearResult:
			this.m_ClubTourResult = base.transform.parent.GetComponent<ClubTourResult>();
			break;
		}
	}

	// Token: 0x06000E02 RID: 3586 RVA: 0x0000C484 File Offset: 0x0000A684
	private void Start()
	{
		this.m_AddExpAni.duration = this.m_fDurationTime;
		this.m_AddExpAni.to = this.m_v3TweenToPos;
		this.m_gExpAddLevelUpPoint.SetActive(false);
		this.m_iEventBonusExp = 1;
	}

	// Token: 0x06000E03 RID: 3587 RVA: 0x000644BC File Offset: 0x000626BC
	private void LevelPosCheck()
	{
		if (Singleton<GameManager>.instance.UserData.Level.ToString().Length == 1)
		{
			this.m_lLevel.transform.localPosition = new Vector3(20f, 33f, 0f);
		}
		else
		{
			this.m_lLevel.transform.localPosition = new Vector3(-3.5f, 33f, 0f);
		}
	}

	// Token: 0x06000E04 RID: 3588 RVA: 0x00064538 File Offset: 0x00062738
	public void DefualtExpSetting()
	{
		this.m_iMaxExpValue = 1800;
		this.m_iSaveNowValue = (this.m_iNowExpValue = 980);
		this.m_iSaveAddValue = (this.m_iAddExpValue = 1000);
		this.m_lNowExp.text = this.m_iNowExpValue.ToString() + "/" + this.m_iMaxExpValue.ToString();
		Singleton<GameManager>.instance.UserData.Level = 9;
		this.m_lLevel.text = "9";
		this.LevelPosCheck();
		this.m_FinishExpBar.baseClipRegion = new Vector4(this.m_FinishExpBar.baseClipRegion.x, this.m_FinishExpBar.baseClipRegion.y, this.ProgressBarOriginPos + this.ProgressBarMaxMoveValue * ((float)(this.m_iSaveNowValue + this.m_iSaveAddValue) / (float)this.m_iMaxExpValue), this.m_FinishExpBar.baseClipRegion.z);
		this.m_ExpBar.baseClipRegion = new Vector4(this.m_ExpBar.baseClipRegion.x, this.m_ExpBar.baseClipRegion.y, this.ProgressBarOriginPos + this.ProgressBarMaxMoveValue * ((float)this.m_iNowExpValue / (float)this.m_iMaxExpValue), this.m_ExpBar.baseClipRegion.z);
		Vector3 localPosition = this.m_tExpAddStartLine.transform.localPosition;
		this.m_tExpAddStartLine.transform.localPosition = new Vector3(localPosition.x + (this.m_ExpBar.baseClipRegion.z - this.ProgressBarOriginPos) * 0.5f, localPosition.y, localPosition.z);
		if (this.m_tExpAddStartLine.transform.localPosition.x > 202f)
		{
			this.m_tExpAddStartLine.transform.localPosition = new Vector3(202f, localPosition.y, localPosition.z);
		}
	}

	// Token: 0x06000E05 RID: 3589 RVA: 0x0006474C File Offset: 0x0006294C
	public void ExpSetting()
	{
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
		int num = 0;
		switch (this.m_ResultKind)
		{
		case ResultKind_e.ClearResult:
			num = resultData.EXP;
			break;
		case ResultKind_e.AllClearResult:
			num = resultData.ALLCLEAR_EXP;
			break;
		case ResultKind_e.RaveUpClearResult:
			if (Singleton<GameManager>.instance.RaveUpHurdleFail)
			{
				num = 0;
			}
			else
			{
				num = resultData.EXP;
			}
			break;
		case ResultKind_e.ClubTourClearResult:
			num = resultData.EXP;
			break;
		}
		int num2 = Singleton<GameManager>.instance.UserData.ViewExp;
		if (!Singleton<GameManager>.instance.ONLOGIN)
		{
			num = 0;
			num2 = 0;
			this.m_tExpAddStartLine.gameObject.SetActive(false);
		}
		this.m_iSaveAddValue = (this.m_iAddExpValue = num);
		this.m_iSaveNowValue = (this.m_iNowExpValue = num2);
		this.m_iMaxExpValue = Singleton<SongManager>.instance.GetLvCurrentExp(Singleton<GameManager>.instance.UserData.Level);
		this.m_lNowExp.text = this.m_iNowExpValue.ToString() + "/" + this.m_iMaxExpValue.ToString();
		this.m_lLevel.text = Singleton<GameManager>.instance.UserData.Level.ToString();
		if (Singleton<GameManager>.instance.UserData.Level == 99)
		{
			this.m_iSaveAddValue = (this.m_iAddExpValue = 0);
			this.m_iSaveNowValue = (this.m_iNowExpValue = 0);
			this.m_iMaxExpValue = 0;
			this.m_tExpAddStartLine.gameObject.SetActive(false);
			this.m_lNowExp.gameObject.SetActive(false);
		}
		this.m_FinishExpBar.baseClipRegion = new Vector4(this.m_FinishExpBar.baseClipRegion.x, this.m_FinishExpBar.baseClipRegion.y, this.ProgressBarOriginPos + this.ProgressBarMaxMoveValue * ((float)(this.m_iSaveNowValue + this.m_iSaveAddValue) / (float)this.m_iMaxExpValue), this.m_FinishExpBar.baseClipRegion.z);
		this.m_ExpBar.baseClipRegion = new Vector4(this.m_ExpBar.baseClipRegion.x, this.m_ExpBar.baseClipRegion.y, this.ProgressBarOriginPos + this.ProgressBarMaxMoveValue * ((float)this.m_iNowExpValue / (float)this.m_iMaxExpValue), this.m_ExpBar.baseClipRegion.z);
		Vector3 localPosition = this.m_tExpAddStartLine.transform.localPosition;
		if (this.m_tExpAddStartLine.gameObject.activeSelf)
		{
			this.m_tExpAddStartLine.transform.localPosition = new Vector3(localPosition.x + (this.m_ExpBar.baseClipRegion.z - this.ProgressBarOriginPos) * 0.5f, localPosition.y, localPosition.z);
			if (this.m_tExpAddStartLine.transform.localPosition.x > 202f)
			{
				this.m_tExpAddStartLine.transform.localPosition = new Vector3(202f, localPosition.y, localPosition.z);
			}
		}
		this.LevelPosCheck();
	}

	// Token: 0x06000E06 RID: 3590 RVA: 0x0000C4BB File Offset: 0x0000A6BB
	public void StartAni()
	{
		this.m_AniStep = ResultManager.StepKind_e.Step1;
		if (Singleton<GameManager>.instance.ONLOGIN)
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_EXP_BAR, true);
		}
	}

	// Token: 0x06000E07 RID: 3591 RVA: 0x00064AA4 File Offset: 0x00062CA4
	private void Update()
	{
		if (this.m_bAniEndState)
		{
			return;
		}
		ResultManager.StepKind_e aniStep = this.m_AniStep;
		if (aniStep == ResultManager.StepKind_e.Step1)
		{
			this.ExpStep();
			if (this.m_AddExpAni.transform.localPosition.x == this.m_AddExpAni.to.x)
			{
				if (this.m_ResultKind == ResultKind_e.ClubTourClearResult)
				{
					this.m_ClubTourResult.Popup(this.m_bLevelGiftState);
				}
				else if (this.m_bLevelGiftState && !this.m_LevelGiftPopup.gameObject.activeSelf)
				{
					this.m_LevelGiftPopup.gameObject.SetActive(true);
					this.m_LevelGiftPopup.CheckLevelGift(Singleton<GameManager>.instance.UserData.Level);
				}
				this.m_gExpEff.SetActive(false);
				Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_EXP_BAR);
				this.m_FinishExpBar.gameObject.SetActive(false);
				this.m_bAniEndState = true;
				this.m_isExit.ColliderOn();
				return;
			}
		}
	}

	// Token: 0x06000E08 RID: 3592 RVA: 0x00064BB0 File Offset: 0x00062DB0
	private void ExpStep()
	{
		if (this.m_bExpLevelUp)
		{
			return;
		}
		if (this.m_iSaveAddValue > 0)
		{
			this.m_gExpEff.SetActive(true);
		}
		this.m_AddExpAni.enabled = true;
		this.m_iAddExpValue = (int)((float)this.m_iSaveAddValue * this.m_AddExpAni.transform.localPosition.x);
		this.m_iNowExpValue = this.m_iSaveNowValue + this.m_iAddExpValue;
		this.m_ilAddExp.text = (this.m_iAddOldExp + this.m_iAddExpValue).ToString();
		this.m_lNowExp.text = this.m_iNowExpValue.ToString() + "/" + this.m_iMaxExpValue.ToString();
		this.m_ExpBar.baseClipRegion = new Vector4(this.m_ExpBar.baseClipRegion.x, this.m_ExpBar.baseClipRegion.y, this.ProgressBarOriginPos + this.ProgressBarMaxMoveValue * ((float)this.m_iNowExpValue / (float)this.m_iMaxExpValue), this.m_ExpBar.baseClipRegion.z);
		if (this.m_ExpBar.baseClipRegion.z < this.ProgressBarMaxMoveValue + this.ProgressBarOriginPos)
		{
			return;
		}
		if (Singleton<GameManager>.instance.UserData.Level == 99)
		{
			this.m_AddExpAni.transform.localPosition = this.m_AddExpAni.to;
			return;
		}
		this.m_iAddOldExp = this.m_iMaxExpValue - this.m_iSaveNowValue;
		this.m_gExpEff.SetActive(false);
		this.m_bExpLevelUp = true;
		this.m_AddExpAni.enabled = false;
		this.m_lNowExp.text = this.m_iMaxExpValue.ToString() + "/" + this.m_iMaxExpValue.ToString();
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_EXP_BAR);
		base.Invoke("LevelUpEFF", 0.3f);
		base.Invoke("LevelUp", 1f);
	}

	// Token: 0x06000E09 RID: 3593 RVA: 0x00064DBC File Offset: 0x00062FBC
	private void LevelUpEFF()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_LEVEL_UP, false);
		this.m_LevelUpBG.gameObject.SetActive(true);
		this.m_LevelUpBG.from = 1f;
		this.m_LevelUpBG.GetComponent<UISprite>().alpha = 1f;
		this.m_LevelUpBG.ResetToBeginning();
		this.m_LevelUpBG.Play(true);
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x00064E24 File Offset: 0x00063024
	private void LevelUp()
	{
		this.m_bExpLevelUp = false;
		this.m_AddExpAni.enabled = true;
		this.m_AddExpAni.transform.localPosition = Vector3.zero;
		this.m_AddExpAni.from = Vector3.zero;
		this.m_AddExpAni.to = new Vector3(1f, 0f, 0f);
		this.m_AddExpAni.ResetToBeginning();
		this.m_AddExpAni.Play(true);
		int num = this.m_iAddExpValue - (this.m_iNowExpValue + this.m_iAddExpValue - this.m_iMaxExpValue);
		Singleton<GameManager>.instance.UserData.Level++;
		this.m_ExpBar.baseClipRegion = new Vector4(this.m_ExpBar.baseClipRegion.x, this.m_ExpBar.baseClipRegion.y, this.ProgressBarOriginPos, this.m_ExpBar.baseClipRegion.z);
		this.m_lLevel.text = Singleton<GameManager>.instance.UserData.Level.ToString();
		this.m_MyInfoLevel.transform.parent.GetComponent<MyInfoManager>().LevelSetting(Singleton<GameManager>.instance.UserData.Level);
		this.LevelPosCheck();
		if (Singleton<GameManager>.instance.UserData.Level % 10 == 0 && Singleton<GameManager>.instance.UserData.Level <= 60)
		{
			this.m_bLevelGiftState = true;
		}
		this.m_iMaxExpValue = Singleton<SongManager>.instance.GetLvCurrentExp(Singleton<GameManager>.instance.UserData.Level);
		if (this.m_tExpAddStartLine.gameObject.activeSelf)
		{
			this.m_tExpAddStartLine.localPosition = new Vector3(-287f, 53f, 0f);
		}
		this.m_iSaveAddValue -= this.m_iAddExpValue + num;
		this.m_FinishExpBar.gameObject.SetActive(false);
		this.m_iSaveNowValue = 0;
		this.m_gExpAddLevelUpPoint.SetActive(true);
		if (Singleton<GameManager>.instance.UserData.Level != 99)
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_EXP_BAR, false);
			return;
		}
		this.m_lNowExp.gameObject.SetActive(false);
		this.m_AddExpAni.transform.localPosition = this.m_AddExpAni.to;
	}

	// Token: 0x04000EC1 RID: 3777
	public LevelGiftPopup m_LevelGiftPopup;

	// Token: 0x04000EC2 RID: 3778
	public HouseMixResultExit m_isExit;

	// Token: 0x04000EC3 RID: 3779
	public ImageFontLabel m_MyInfoLevel;

	// Token: 0x04000EC4 RID: 3780
	public ResultKind_e m_ResultKind;

	// Token: 0x04000EC5 RID: 3781
	private ResultManager.StepKind_e m_AniStep = ResultManager.StepKind_e.None;

	// Token: 0x04000EC6 RID: 3782
	private ResultPlayResult m_ResultPlayResult;

	// Token: 0x04000EC7 RID: 3783
	private ResultAllClearPlayResult m_ResultAllClearPlayResult;

	// Token: 0x04000EC8 RID: 3784
	private RaveUpResult m_RaveUpResult;

	// Token: 0x04000EC9 RID: 3785
	private ClubTourResult m_ClubTourResult;

	// Token: 0x04000ECA RID: 3786
	private UIPanel m_ExpBar;

	// Token: 0x04000ECB RID: 3787
	private UIPanel m_FinishExpBar;

	// Token: 0x04000ECC RID: 3788
	private ImageFontLabel m_lLevel;

	// Token: 0x04000ECD RID: 3789
	private UILabel m_lNowExp;

	// Token: 0x04000ECE RID: 3790
	private ImageFontLabel m_ilAddExp;

	// Token: 0x04000ECF RID: 3791
	private TweenPosition m_AddExpAni;

	// Token: 0x04000ED0 RID: 3792
	private TweenAlpha m_LevelUpBG;

	// Token: 0x04000ED1 RID: 3793
	private TweenScale m_LevelMarkAni;

	// Token: 0x04000ED2 RID: 3794
	private Transform m_tExpAddStartLine;

	// Token: 0x04000ED3 RID: 3795
	private GameObject m_gExpAddLevelUpPoint;

	// Token: 0x04000ED4 RID: 3796
	private GameObject m_gLevelMarkAniEff;

	// Token: 0x04000ED5 RID: 3797
	private GameObject m_gExpEff;

	// Token: 0x04000ED6 RID: 3798
	private int m_iAddExpValue;

	// Token: 0x04000ED7 RID: 3799
	private int m_iNowExpValue;

	// Token: 0x04000ED8 RID: 3800
	private int m_iMaxExpValue;

	// Token: 0x04000ED9 RID: 3801
	private int m_iSaveNowValue;

	// Token: 0x04000EDA RID: 3802
	private int m_iSaveAddValue;

	// Token: 0x04000EDB RID: 3803
	private int m_iEventBonusExp;

	// Token: 0x04000EDC RID: 3804
	private bool m_bAniEndState;

	// Token: 0x04000EDD RID: 3805
	private bool m_bExpLevelUp;

	// Token: 0x04000EDE RID: 3806
	private Vector3 m_v3TweenToPos = new Vector3(1f, 0f, 0f);

	// Token: 0x04000EDF RID: 3807
	private float m_fDurationTime = 1f;

	// Token: 0x04000EE0 RID: 3808
	private float ProgressBarOriginPos = 425f;

	// Token: 0x04000EE1 RID: 3809
	private float ProgressBarMaxMoveValue = 970f;

	// Token: 0x04000EE2 RID: 3810
	private bool m_bLevelGiftState;

	// Token: 0x04000EE3 RID: 3811
	private int m_iAddOldExp;
}
