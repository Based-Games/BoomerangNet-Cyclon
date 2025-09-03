using System;
using UnityEngine;

// Token: 0x020001E8 RID: 488
public class ResultPlayResult : MonoBehaviour
{
	// Token: 0x06000E20 RID: 3616 RVA: 0x0006585C File Offset: 0x00063A5C
	private void Awake()
	{
		this.m_ResultDiscInfo = base.transform.FindChild("0_Discinfo").GetComponent<ResultDiscInfo>();
		this.m_ResultMyRecord = base.transform.FindChild("1_MyRecord").GetComponent<ResultMyRecord>();
		this.m_ResultAccuracy = base.transform.FindChild("2_Accuracy").GetComponent<ResultAccuracy>();
		this.m_ResultScore = base.transform.FindChild("3_Score").GetComponent<ResultScore>();
		this.m_ResultExp = base.transform.FindChild("4_Exp").GetComponent<ResultExp>();
		this.m_ResultBeatPoint = base.transform.FindChild("5_BeatPoint").GetComponent<ResultBeatPoint>();
		this.m_ResultRank = base.transform.FindChild("6_Ranks").GetComponent<ResultRank>();
		this.m_ResultTrophy = base.transform.FindChild("7_Trophy").GetComponent<ResultTrophy>();
		this.m_ResultNewRecord = base.transform.FindChild("8_NewRecord").GetComponent<ResultNewRecord>();
	}

	// Token: 0x06000E21 RID: 3617 RVA: 0x0000B171 File Offset: 0x00009371
	private void Start()
	{
		base.Invoke("Init", Time.deltaTime);
	}

	// Token: 0x06000E22 RID: 3618 RVA: 0x0000C555 File Offset: 0x0000A755
	private void Init()
	{
		this.Setting();
		this.StartAni_Step1();
	}

	// Token: 0x06000E23 RID: 3619 RVA: 0x0006595C File Offset: 0x00063B5C
	private void Setting()
	{
		this.m_ResultDiscInfo.DiscInfoSetting();
		this.m_ResultAccuracy.AccuracySetting();
		this.m_ResultScore.ScoreSetting();
		this.m_ResultExp.ExpSetting();
		this.m_ResultBeatPoint.BeatPointSetting();
		this.m_ResultRank.RankSetting();
		this.m_ResultTrophy.TrophySetting();
		if (Singleton<GameManager>.instance.ONLOGIN)
		{
			RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
			PatternScoreInfo patternScoreInfo = Singleton<GameManager>.instance.netHouseMixData.DicPatternInfo[resultData.PTTYPE];
			if (patternScoreInfo.BestScore == -1)
			{
				this.m_ResultMyRecord.MyRecordSetting(resultData.SCORE, resultData.MAXCOMBO, resultData.GetAccuracy(), (int)resultData.GRADETYPE, resultData.IsAllComboPlay(), resultData.IsPerfectPlay(), resultData.TrophyName);
			}
			else
			{
				this.m_ResultMyRecord.MyRecordSetting(patternScoreInfo.BestScore, patternScoreInfo.MaxCombo, patternScoreInfo.Accuracy, (int)patternScoreInfo.RankClass, patternScoreInfo.AllCombo, patternScoreInfo.PerfectPlay, patternScoreInfo.strTrophy);
			}
		}
		else
		{
			this.m_ResultMyRecord.MyRecordSetting(-1, -1, -1, -1, false, false, string.Empty);
		}
	}

	// Token: 0x06000E24 RID: 3620 RVA: 0x00065A84 File Offset: 0x00063C84
	private void DefaultValueSetting()
	{
		this.m_ResultDiscInfo.DefaultDiscInfoSetting();
		this.m_ResultAccuracy.DefaultAccuracySetting();
		this.m_ResultScore.DefaultScoreSetting();
		this.m_ResultExp.DefualtExpSetting();
		this.m_ResultBeatPoint.DefualtBeatPointSetting();
		this.m_ResultRank.DefualtRankSetting();
		this.m_ResultTrophy.DefaultTrophySetting();
		this.m_ResultMyRecord.MyRecordSetting(-1, -1, -1, -1, false, false, string.Empty);
	}

	// Token: 0x06000E25 RID: 3621 RVA: 0x00065AF4 File Offset: 0x00063CF4
	private void StartAni_Step1()
	{
		base.Invoke("AccuracyAni", this.m_fAccAniFrame);
		base.Invoke("ScoreAni", this.m_fScoreAniFrame);
		base.Invoke("BeatPointAni", this.m_fBeatPointAniFrame);
		base.Invoke("StartAni_Step2", this.m_fBeatPointAniFrame);
	}

	// Token: 0x06000E26 RID: 3622 RVA: 0x00065B48 File Offset: 0x00063D48
	public void StartAni_Step2()
	{
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
		PatternScoreInfo patternScoreInfo = Singleton<GameManager>.instance.netHouseMixData.DicPatternInfo[resultData.PTTYPE];
		base.Invoke("RankAni", this.m_fRankAniFrame);
		base.Invoke("TrophyAni", this.m_fTrophyAniFrame);
		if (patternScoreInfo.BestScore < resultData.SCORE)
		{
			if (Singleton<GameManager>.instance.ONLOGIN)
			{
				base.Invoke("NewRecordAni", this.m_fNewRecordAniFrame);
			}
			base.Invoke("ExpAni", this.m_fExpAniFrame);
		}
		else
		{
			base.Invoke("ExpAni", this.m_fNewRecordAniFrame);
		}
	}

	// Token: 0x06000E27 RID: 3623 RVA: 0x0000C563 File Offset: 0x0000A763
	private void AccuracyAni()
	{
		this.m_ResultAccuracy.StartAni();
	}

	// Token: 0x06000E28 RID: 3624 RVA: 0x0000C570 File Offset: 0x0000A770
	private void ScoreAni()
	{
		this.m_ResultScore.StartAni();
	}

	// Token: 0x06000E29 RID: 3625 RVA: 0x0000C57D File Offset: 0x0000A77D
	private void ExpAni()
	{
		this.m_ResultExp.StartAni();
	}

	// Token: 0x06000E2A RID: 3626 RVA: 0x0000C58A File Offset: 0x0000A78A
	private void BeatPointAni()
	{
		this.m_ResultBeatPoint.StartAni();
	}

	// Token: 0x06000E2B RID: 3627 RVA: 0x0000C597 File Offset: 0x0000A797
	private void RankAni()
	{
		this.m_ResultRank.StartAni();
	}

	// Token: 0x06000E2C RID: 3628 RVA: 0x0000C5A4 File Offset: 0x0000A7A4
	private void TrophyAni()
	{
		this.m_ResultTrophy.StartAni();
	}

	// Token: 0x06000E2D RID: 3629 RVA: 0x0000C5B1 File Offset: 0x0000A7B1
	private void NewRecordAni()
	{
		this.m_ResultNewRecord.StartAni();
	}

	// Token: 0x04000F0D RID: 3853
	private ResultManager.StepKind_e nowStep = ResultManager.StepKind_e.None;

	// Token: 0x04000F0E RID: 3854
	private ResultDiscInfo m_ResultDiscInfo;

	// Token: 0x04000F0F RID: 3855
	private ResultMyRecord m_ResultMyRecord;

	// Token: 0x04000F10 RID: 3856
	private ResultAccuracy m_ResultAccuracy;

	// Token: 0x04000F11 RID: 3857
	private ResultScore m_ResultScore;

	// Token: 0x04000F12 RID: 3858
	private ResultExp m_ResultExp;

	// Token: 0x04000F13 RID: 3859
	private ResultBeatPoint m_ResultBeatPoint;

	// Token: 0x04000F14 RID: 3860
	private ResultRank m_ResultRank;

	// Token: 0x04000F15 RID: 3861
	private ResultTrophy m_ResultTrophy;

	// Token: 0x04000F16 RID: 3862
	private ResultNewRecord m_ResultNewRecord;

	// Token: 0x04000F17 RID: 3863
	private float m_fAccAniFrame = 1f;

	// Token: 0x04000F18 RID: 3864
	private float m_fScoreAniFrame = 2f;

	// Token: 0x04000F19 RID: 3865
	private float m_fExpAniFrame = 3.75f;

	// Token: 0x04000F1A RID: 3866
	private float m_fBeatPointAniFrame = 2.5f;

	// Token: 0x04000F1B RID: 3867
	private float m_fRankAniFrame = 0.5f;

	// Token: 0x04000F1C RID: 3868
	private float m_fTrophyAniFrame = 1.5f;

	// Token: 0x04000F1D RID: 3869
	private float m_fNewRecordAniFrame = 2.25f;
}
