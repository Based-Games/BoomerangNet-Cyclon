using System;
using UnityEngine;

// Token: 0x0200020B RID: 523
public class RaveUpResult : MonoBehaviour
{
	// Token: 0x06000F1F RID: 3871 RVA: 0x0006D148 File Offset: 0x0006B348
	private void Awake()
	{
		this.m_RaveUpAlbumDiscInfo = base.transform.FindChild("0_Discinfo").GetComponent<RaveUpAlbumDiscInfo>();
		this.m_ResultMyRecord = base.transform.FindChild("1_MyRecord").GetComponent<ResultMyRecord>();
		this.m_ResultAccuracy = base.transform.FindChild("2_Accuracy").GetComponent<RaveUpResultAccuracy>();
		this.m_ResultScore = base.transform.FindChild("3_Score").GetComponent<ResultScore>();
		this.m_ResultExp = base.transform.FindChild("4_Exp").GetComponent<ResultExp>();
		this.m_ResultBeatPoint = base.transform.FindChild("5_BeatPoint").GetComponent<ResultBeatPoint>();
		this.m_ResultRank = base.transform.FindChild("6_Ranks").GetComponent<ResultRank>();
		this.m_ResultTrophy = base.transform.FindChild("7_Trophy").GetComponent<ResultTrophy>();
		this.m_ResultNewRecord = base.transform.FindChild("8_NewRecord").GetComponent<ResultNewRecord>();
		Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("Viewing play results", null);
	}

	// Token: 0x06000F20 RID: 3872 RVA: 0x0000B171 File Offset: 0x00009371
	private void Start()
	{
		base.Invoke("Init", Time.deltaTime);
	}

	// Token: 0x06000F21 RID: 3873 RVA: 0x0000D10F File Offset: 0x0000B30F
	private void Init()
	{
		this.Setting();
		this.StartAni_Step1();
	}

	// Token: 0x06000F22 RID: 3874 RVA: 0x0006D258 File Offset: 0x0006B458
	private void PostScore()
	{
		WWWPostRaveUp wwwpostRaveUp = new WWWPostRaveUp();
		Singleton<WWWManager>.instance.AddQueue(wwwpostRaveUp);
	}

	// Token: 0x06000F23 RID: 3875 RVA: 0x0006D278 File Offset: 0x0006B478
	private void Setting()
	{
		this.m_RaveUpAlbumDiscInfo.DiscInfoSetting();
		this.m_ResultAccuracy.AccuracySetting();
		this.m_ResultScore.ScoreSetting();
		this.m_ResultExp.ExpSetting();
		this.m_ResultBeatPoint.BeatPointSetting();
		this.m_ResultRank.RankSetting();
		this.m_ResultTrophy.TrophySetting();
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
		UserRecordData netUserRecordData = Singleton<GameManager>.instance.netUserRecordData;
		if (netUserRecordData.Score == -1)
		{
			this.m_ResultMyRecord.MyRecordSetting(resultData.SCORE, resultData.MAXCOMBO, resultData.GetAccuracy(), (int)resultData.GRADETYPE, resultData.IsAllComboPlay(), resultData.IsPerfectPlay(), string.Empty);
		}
		else
		{
			this.m_ResultMyRecord.MyRecordSetting(netUserRecordData.Score, netUserRecordData.MaxCombo, netUserRecordData.Accuracy, (int)netUserRecordData.RankClass, netUserRecordData.AllCombo, netUserRecordData.PerfectPlay, string.Empty);
		}
		this.PostScore();
	}

	// Token: 0x06000F24 RID: 3876 RVA: 0x0006D364 File Offset: 0x0006B564
	private void DefaultValueSetting()
	{
		this.m_RaveUpAlbumDiscInfo.DefaultDiscInfoSetting();
		this.m_ResultAccuracy.DefaultAccuracySetting();
		this.m_ResultScore.DefaultScoreSetting();
		this.m_ResultExp.DefualtExpSetting();
		this.m_ResultBeatPoint.DefualtBeatPointSetting();
		this.m_ResultRank.DefualtRankSetting();
		this.m_ResultTrophy.DefaultTrophySetting();
		this.m_ResultMyRecord.MyRecordSetting(-1, -1, -1, -1, false, false, string.Empty);
	}

	// Token: 0x06000F25 RID: 3877 RVA: 0x0006D3D4 File Offset: 0x0006B5D4
	private void StartAni_Step1()
	{
		base.Invoke("AccuracyAni", this.m_fAccAniFrame);
		base.Invoke("ScoreAni", this.m_fScoreAniFrame);
		base.Invoke("BeatPointAni", this.m_fBeatPointAniFrame);
		base.Invoke("StartAni_Step2", this.m_fBeatPointAniFrame);
	}

	// Token: 0x06000F26 RID: 3878 RVA: 0x0006D428 File Offset: 0x0006B628
	public void StartAni_Step2()
	{
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
		UserRecordData netUserRecordData = Singleton<GameManager>.instance.netUserRecordData;
		base.Invoke("RankAni", this.m_fRankAniFrame);
		base.Invoke("TrophyAni", this.m_fTrophyAniFrame);
		if (netUserRecordData.Score < resultData.SCORE)
		{
			base.Invoke("NewRecordAni", this.m_fNewRecordAniFrame);
			base.Invoke("ExpAni", this.m_fExpAniFrame);
			return;
		}
		base.Invoke("ExpAni", this.m_fNewRecordAniFrame);
	}

	// Token: 0x06000F27 RID: 3879 RVA: 0x0000D11D File Offset: 0x0000B31D
	private void AccuracyAni()
	{
		this.m_ResultAccuracy.StartAni();
	}

	// Token: 0x06000F28 RID: 3880 RVA: 0x0000D12A File Offset: 0x0000B32A
	private void ScoreAni()
	{
		this.m_ResultScore.StartAni();
	}

	// Token: 0x06000F29 RID: 3881 RVA: 0x0000D137 File Offset: 0x0000B337
	private void ExpAni()
	{
		this.m_ResultExp.StartAni();
	}

	// Token: 0x06000F2A RID: 3882 RVA: 0x0000D144 File Offset: 0x0000B344
	private void BeatPointAni()
	{
		this.m_ResultBeatPoint.StartAni();
	}

	// Token: 0x06000F2B RID: 3883 RVA: 0x0000D151 File Offset: 0x0000B351
	private void RankAni()
	{
		this.m_ResultRank.StartAni();
	}

	// Token: 0x06000F2C RID: 3884 RVA: 0x0000D15E File Offset: 0x0000B35E
	private void TrophyAni()
	{
		this.m_ResultTrophy.StartAni();
	}

	// Token: 0x06000F2D RID: 3885 RVA: 0x0000D16B File Offset: 0x0000B36B
	private void NewRecordAni()
	{
		this.m_ResultNewRecord.StartAni();
	}

	// Token: 0x040010AB RID: 4267
	[HideInInspector]
	public RaveUpPostData m_RaveUpPostData = new RaveUpPostData();

	// Token: 0x040010AC RID: 4268
	private ResultManager.StepKind_e nowStep = ResultManager.StepKind_e.None;

	// Token: 0x040010AD RID: 4269
	private RaveUpAlbumDiscInfo m_RaveUpAlbumDiscInfo;

	// Token: 0x040010AE RID: 4270
	private ResultMyRecord m_ResultMyRecord;

	// Token: 0x040010AF RID: 4271
	private RaveUpResultAccuracy m_ResultAccuracy;

	// Token: 0x040010B0 RID: 4272
	private ResultScore m_ResultScore;

	// Token: 0x040010B1 RID: 4273
	private ResultExp m_ResultExp;

	// Token: 0x040010B2 RID: 4274
	private ResultBeatPoint m_ResultBeatPoint;

	// Token: 0x040010B3 RID: 4275
	private ResultRank m_ResultRank;

	// Token: 0x040010B4 RID: 4276
	private ResultTrophy m_ResultTrophy;

	// Token: 0x040010B5 RID: 4277
	private ResultNewRecord m_ResultNewRecord;

	// Token: 0x040010B6 RID: 4278
	private float m_fAccAniFrame = 1f;

	// Token: 0x040010B7 RID: 4279
	private float m_fScoreAniFrame = 2f;

	// Token: 0x040010B8 RID: 4280
	private float m_fExpAniFrame = 3.75f;

	// Token: 0x040010B9 RID: 4281
	private float m_fBeatPointAniFrame = 2.5f;

	// Token: 0x040010BA RID: 4282
	private float m_fRankAniFrame = 0.5f;

	// Token: 0x040010BB RID: 4283
	private float m_fTrophyAniFrame = 1.5f;

	// Token: 0x040010BC RID: 4284
	private float m_fNewRecordAniFrame = 2.25f;
}
