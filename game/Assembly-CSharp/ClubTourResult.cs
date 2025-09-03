using System;
using UnityEngine;

// Token: 0x02000194 RID: 404
public class ClubTourResult : MonoBehaviour
{
	// Token: 0x06000C0B RID: 3083 RVA: 0x000558C4 File Offset: 0x00053AC4
	private void Awake()
	{
		this.m_RaveUpAlbumDiscInfo = base.transform.FindChild("0_Discinfo").GetComponent<ClubTourResultMissionInfo>();
		this.m_ResultMyRecord = base.transform.FindChild("1_MyRecord").GetComponent<ClubTourResultMyRecord>();
		this.m_ClubTourResultQuest = base.transform.FindChild("2_Quest").GetComponent<ClubTourResultQuest>();
		this.m_ResultAccuracy = base.transform.FindChild("3_Accuracy").GetComponent<ClubTourResultAccuracy>();
		this.m_ResultScore = base.transform.FindChild("4_Score").GetComponent<ResultScore>();
		this.m_ResultExp = base.transform.FindChild("5_Exp").GetComponent<ResultExp>();
		this.m_ResultBeatPoint = base.transform.FindChild("6_BeatPoint").GetComponent<ResultBeatPoint>();
		this.m_ResultRank = base.transform.FindChild("7_Ranks").GetComponent<ResultRank>();
		this.m_ResultTrophy = base.transform.FindChild("8_Trophy").GetComponent<ResultTrophy>();
		this.m_ResultNewRecord = base.transform.FindChild("9_NewRecord").GetComponent<ResultNewRecord>();
		this.m_gPopup = base.transform.FindChild("10_PopUp").gameObject;
		this.m_LevelGiftPopup = base.transform.FindChild("LevelGiftPopup").GetComponent<LevelGiftPopup>();
	}

	// Token: 0x06000C0C RID: 3084 RVA: 0x0000B171 File Offset: 0x00009371
	private void Start()
	{
		base.Invoke("Init", Time.deltaTime);
	}

	// Token: 0x06000C0D RID: 3085 RVA: 0x0000B183 File Offset: 0x00009383
	private void Init()
	{
		this.Setting();
		this.StartAni_Step1();
	}

	// Token: 0x06000C0E RID: 3086 RVA: 0x0000B191 File Offset: 0x00009391
	private void PostScore()
	{
		if (!Singleton<GameManager>.instance.ONLOGIN)
		{
			return;
		}
	}

	// Token: 0x06000C0F RID: 3087 RVA: 0x00055A18 File Offset: 0x00053C18
	private void Setting()
	{
		this.m_RaveUpAlbumDiscInfo.DiscInfoSetting();
		this.m_ClubTourResultQuest.QuestSetting();
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

	// Token: 0x06000C10 RID: 3088 RVA: 0x00055B0C File Offset: 0x00053D0C
	private void DefaultValueSetting()
	{
		this.m_RaveUpAlbumDiscInfo.DefaultDiscInfoSetting();
		this.m_ClubTourResultQuest.DefaultQuestSetting();
		this.m_ResultAccuracy.DefaultAccuracySetting();
		this.m_ResultScore.DefaultScoreSetting();
		this.m_ResultExp.DefualtExpSetting();
		this.m_ResultBeatPoint.DefualtBeatPointSetting();
		this.m_ResultRank.DefualtRankSetting();
		this.m_ResultTrophy.DefaultTrophySetting();
		this.m_ResultMyRecord.MyRecordSetting(-1, -1, -1, -1, false, false, string.Empty);
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x00055B88 File Offset: 0x00053D88
	private void StartAni_Step1()
	{
		base.Invoke("AccuracyAni", this.m_fAccAniFrame);
		base.Invoke("ScoreAni", this.m_fScoreAniFrame);
		base.Invoke("BeatPointAni", this.m_fBeatPointAniFrame);
		base.Invoke("StartAni_Step2", this.m_fBeatPointAniFrame);
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x00055BDC File Offset: 0x00053DDC
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
		}
		else
		{
			base.Invoke("ExpAni", this.m_fNewRecordAniFrame);
		}
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x00055C6C File Offset: 0x00053E6C
	public void Popup(bool LevelUpGift)
	{
		this.m_bLevelUpGift = LevelUpGift;
		if (Singleton<SongManager>.instance.Mission.AllClear)
		{
			base.Invoke("PopupOpen", 1f);
			base.Invoke("PopupClose", 5f);
			if (Singleton<SongManager>.instance.Mission.Cleared)
			{
				return;
			}
			switch (Singleton<GameManager>.instance.RewardState)
			{
			case MISSIONREWARD_STATE.ALL_REWARD:
				base.CancelInvoke("PopupClose");
				base.Invoke("PopupClose", 10f);
				break;
			}
		}
	}

	// Token: 0x06000C14 RID: 3092 RVA: 0x0000B1A3 File Offset: 0x000093A3
	private void PopupOpen()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RESULT_POPUP, false);
		this.m_gPopup.SetActive(true);
		this.m_gPopup.GetComponent<ClubTourResultPopup>().CheckSongPopup();
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x00055D18 File Offset: 0x00053F18
	private void PopupClose()
	{
		this.m_gPopup.SetActive(false);
		if (this.m_bLevelUpGift)
		{
			this.m_LevelGiftPopup.gameObject.SetActive(true);
			this.m_LevelGiftPopup.CheckLevelGift(Singleton<GameManager>.instance.UserData.Level);
		}
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x0000B1CE File Offset: 0x000093CE
	private void AccuracyAni()
	{
		this.m_ResultAccuracy.StartAni();
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x0000B1DB File Offset: 0x000093DB
	private void ScoreAni()
	{
		this.m_ResultScore.StartAni();
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x0000B1E8 File Offset: 0x000093E8
	private void ExpAni()
	{
		this.m_ResultExp.StartAni();
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x0000B1F5 File Offset: 0x000093F5
	private void BeatPointAni()
	{
		this.m_ResultBeatPoint.StartAni();
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x0000B202 File Offset: 0x00009402
	private void RankAni()
	{
		this.m_ResultRank.StartAni();
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x0000B20F File Offset: 0x0000940F
	private void TrophyAni()
	{
		this.m_ResultTrophy.StartAni();
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x0000B21C File Offset: 0x0000941C
	private void NewRecordAni()
	{
		this.m_ResultNewRecord.StartAni();
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x00055D68 File Offset: 0x00053F68
	private void OnDestroy()
	{
		for (int i = 0; i < 75; i++)
		{
			Singleton<SoundSourceManager>.instance.Stop((SOUNDINDEX)i);
		}
		Singleton<SoundSourceManager>.instance.StopBgm();
	}

	// Token: 0x04000BD3 RID: 3027
	[HideInInspector]
	public RaveUpPostData m_RaveUpPostData = new RaveUpPostData();

	// Token: 0x04000BD4 RID: 3028
	private ResultManager.StepKind_e nowStep = ResultManager.StepKind_e.None;

	// Token: 0x04000BD5 RID: 3029
	private ClubTourResultMissionInfo m_RaveUpAlbumDiscInfo;

	// Token: 0x04000BD6 RID: 3030
	private ClubTourResultMyRecord m_ResultMyRecord;

	// Token: 0x04000BD7 RID: 3031
	private ClubTourResultQuest m_ClubTourResultQuest;

	// Token: 0x04000BD8 RID: 3032
	private ClubTourResultAccuracy m_ResultAccuracy;

	// Token: 0x04000BD9 RID: 3033
	private ResultScore m_ResultScore;

	// Token: 0x04000BDA RID: 3034
	private ResultExp m_ResultExp;

	// Token: 0x04000BDB RID: 3035
	private ResultBeatPoint m_ResultBeatPoint;

	// Token: 0x04000BDC RID: 3036
	private ResultRank m_ResultRank;

	// Token: 0x04000BDD RID: 3037
	private ResultTrophy m_ResultTrophy;

	// Token: 0x04000BDE RID: 3038
	private ResultNewRecord m_ResultNewRecord;

	// Token: 0x04000BDF RID: 3039
	private GameObject m_gPopup;

	// Token: 0x04000BE0 RID: 3040
	private LevelGiftPopup m_LevelGiftPopup;

	// Token: 0x04000BE1 RID: 3041
	private float m_fAccAniFrame = 1f;

	// Token: 0x04000BE2 RID: 3042
	private float m_fScoreAniFrame = 2f;

	// Token: 0x04000BE3 RID: 3043
	private float m_fBeatPointAniFrame = 2.5f;

	// Token: 0x04000BE4 RID: 3044
	private float m_fRankAniFrame = 0.5f;

	// Token: 0x04000BE5 RID: 3045
	private float m_fTrophyAniFrame = 1.5f;

	// Token: 0x04000BE6 RID: 3046
	private float m_fNewRecordAniFrame = 2.25f;

	// Token: 0x04000BE7 RID: 3047
	private float m_fExpAniFrame = 3.75f;

	// Token: 0x04000BE8 RID: 3048
	private bool m_bLevelUpGift;

	// Token: 0x04000BE9 RID: 3049
	[HideInInspector]
	public bool m_bMissionComplete = true;
}
