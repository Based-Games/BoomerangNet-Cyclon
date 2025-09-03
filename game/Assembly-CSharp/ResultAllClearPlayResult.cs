using System;
using UnityEngine;

// Token: 0x020001DF RID: 479
public class ResultAllClearPlayResult : MonoBehaviour
{
	// Token: 0x06000DDD RID: 3549 RVA: 0x000637B8 File Offset: 0x000619B8
	private void Awake()
	{
		this.m_ResultAllClearDiscInfo = new ResultAllClearDiscInfo[this.m_iMaxDiscInfo];
		for (int i = 0; i < this.m_iMaxDiscInfo; i++)
		{
			this.m_ResultAllClearDiscInfo[i] = base.transform.FindChild((i + 1).ToString() + "_Discinfo").GetComponent<ResultAllClearDiscInfo>();
		}
		this.m_ResultScore = base.transform.FindChild("4_Score").GetComponent<ResultAllClearScore>();
		this.m_ResultExp = base.transform.FindChild("5_Exp").GetComponent<ResultExp>();
		this.m_ResultBeatPoint = base.transform.FindChild("6_BeatPoint").GetComponent<ResultBeatPoint>();
		this.m_ResultRank = base.transform.FindChild("7_Ranks").GetComponent<ResultRank>();
		this.m_ResultTrophy = base.transform.FindChild("8_Trophy").GetComponent<ResultTrophy>();
		this.m_ResultNewRecord = base.transform.FindChild("9_NewRecord").GetComponent<ResultNewRecord>();
	}

	// Token: 0x06000DDE RID: 3550 RVA: 0x0000B171 File Offset: 0x00009371
	private void Start()
	{
		base.Invoke("Init", Time.deltaTime);
	}

	// Token: 0x06000DDF RID: 3551 RVA: 0x0000C2A5 File Offset: 0x0000A4A5
	private void Init()
	{
		this.Setting();
		this.StartAni_Step1();
	}

	// Token: 0x06000DE0 RID: 3552 RVA: 0x000638BC File Offset: 0x00061ABC
	private void Setting()
	{
		for (int i = 0; i < this.m_iMaxDiscInfo; i++)
		{
			this.m_ResultAllClearDiscInfo[i].DiscInfoSetting(i);
		}
		this.m_ResultScore.ScoreSetting();
		this.m_ResultExp.ExpSetting();
		this.m_ResultBeatPoint.BeatPointSetting();
		this.m_ResultRank.RankSetting();
		this.m_ResultTrophy.TrophySetting();
	}

	// Token: 0x06000DE1 RID: 3553 RVA: 0x0000C2B3 File Offset: 0x0000A4B3
	private void DefaultValueSetting()
	{
		this.m_ResultScore.DefaultScoreSetting();
		this.m_ResultExp.DefualtExpSetting();
		this.m_ResultBeatPoint.DefualtBeatPointSetting();
		this.m_ResultRank.DefualtRankSetting();
		this.m_ResultTrophy.DefaultTrophySetting();
	}

	// Token: 0x06000DE2 RID: 3554 RVA: 0x0000C2EC File Offset: 0x0000A4EC
	private void StartAni_Step1()
	{
		base.Invoke("ScoreAni", this.m_fScoreAniFrame);
		base.Invoke("BeatPointAni", this.m_fBeatPointAniFrame);
		base.Invoke("StartAni_Step2", this.m_fBeatPointAniFrame);
	}

	// Token: 0x06000DE3 RID: 3555 RVA: 0x0000C321 File Offset: 0x0000A521
	public void StartAni_Step2()
	{
		base.Invoke("RankAni", this.m_fRankAniFrame);
		base.Invoke("TrophyAni", this.m_fTrophyAniFrame);
		base.Invoke("ExpAni", this.m_fNewRecordAniFrame);
	}

	// Token: 0x06000DE4 RID: 3556 RVA: 0x0000C356 File Offset: 0x0000A556
	private void ScoreAni()
	{
		this.m_ResultScore.StartAni();
	}

	// Token: 0x06000DE5 RID: 3557 RVA: 0x0000C363 File Offset: 0x0000A563
	private void ExpAni()
	{
		this.m_ResultExp.StartAni();
	}

	// Token: 0x06000DE6 RID: 3558 RVA: 0x0000C370 File Offset: 0x0000A570
	private void BeatPointAni()
	{
		this.m_ResultBeatPoint.StartAni();
	}

	// Token: 0x06000DE7 RID: 3559 RVA: 0x0000C37D File Offset: 0x0000A57D
	private void RankAni()
	{
		this.m_ResultRank.StartAni();
	}

	// Token: 0x06000DE8 RID: 3560 RVA: 0x0000C38A File Offset: 0x0000A58A
	private void TrophyAni()
	{
		this.m_ResultTrophy.StartAni();
	}

	// Token: 0x06000DE9 RID: 3561 RVA: 0x0000C397 File Offset: 0x0000A597
	private void NewRecordAni()
	{
		this.m_ResultNewRecord.StartAni();
	}

	// Token: 0x04000E91 RID: 3729
	private ResultManager.StepKind_e nowStep = ResultManager.StepKind_e.None;

	// Token: 0x04000E92 RID: 3730
	private ResultAllClearScore m_ResultScore;

	// Token: 0x04000E93 RID: 3731
	private ResultExp m_ResultExp;

	// Token: 0x04000E94 RID: 3732
	private ResultBeatPoint m_ResultBeatPoint;

	// Token: 0x04000E95 RID: 3733
	private ResultRank m_ResultRank;

	// Token: 0x04000E96 RID: 3734
	private ResultTrophy m_ResultTrophy;

	// Token: 0x04000E97 RID: 3735
	private ResultNewRecord m_ResultNewRecord;

	// Token: 0x04000E98 RID: 3736
	private ResultAllClearDiscInfo[] m_ResultAllClearDiscInfo;

	// Token: 0x04000E99 RID: 3737
	private float m_fScoreAniFrame = 2f;

	// Token: 0x04000E9A RID: 3738
	private float m_fExpAniFrame = 3.75f;

	// Token: 0x04000E9B RID: 3739
	private float m_fBeatPointAniFrame = 2.5f;

	// Token: 0x04000E9C RID: 3740
	private float m_fRankAniFrame = 0.5f;

	// Token: 0x04000E9D RID: 3741
	private float m_fTrophyAniFrame = 1.5f;

	// Token: 0x04000E9E RID: 3742
	private float m_fNewRecordAniFrame = 2.25f;

	// Token: 0x04000E9F RID: 3743
	private int m_iMaxDiscInfo = 3;

	// Token: 0x04000EA0 RID: 3744
	[HideInInspector]
	public AllClearPostInfo m_AllClearPostInfo = new AllClearPostInfo();
}
