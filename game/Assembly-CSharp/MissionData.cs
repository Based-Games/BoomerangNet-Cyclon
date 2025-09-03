using System;

// Token: 0x02000144 RID: 324
public class MissionData
{
	// Token: 0x170001FE RID: 510
	// (get) Token: 0x06000A69 RID: 2665 RVA: 0x0000A35D File Offset: 0x0000855D
	public bool Cleared
	{
		get
		{
			return Singleton<GameManager>.instance.UserData.GetClearedMission(this.iMissionId);
		}
	}

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x06000A6A RID: 2666 RVA: 0x0004AA94 File Offset: 0x00048C94
	public bool Lock
	{
		get
		{
			if (this.iMissionId == 1)
			{
				return false;
			}
			USERDATA userData = Singleton<GameManager>.instance.UserData;
			return this.iMissionOpenLevel > userData.Level || !userData.GetContainMissionInfo(this.iMissionId);
		}
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x0004AAD8 File Offset: 0x00048CD8
	public int GetChekcNum(int iNum)
	{
		MISSIONTYPE missiontype = this.ArrMissionType[iNum];
		int num = 0;
		switch (missiontype)
		{
		case MISSIONTYPE.Rank_F:
		case MISSIONTYPE.Rank_D:
		case MISSIONTYPE.Rank_C:
		case MISSIONTYPE.Rank_B:
		case MISSIONTYPE.Rank_A:
		case MISSIONTYPE.Rank_AP:
		case MISSIONTYPE.Rank_APP:
		case MISSIONTYPE.Rank_S:
		case MISSIONTYPE.Rank_SP:
		case MISSIONTYPE.Rank_SPP:
			num = (int)Singleton<GameManager>.instance.ResultData.GRADETYPE;
			break;
		case MISSIONTYPE.Perfect:
			num = Singleton<GameManager>.instance.ResultData.GetJudgmentCnt(JUDGMENT_TYPE.PERFECT);
			break;
		case MISSIONTYPE.Great:
			num = Singleton<GameManager>.instance.ResultData.GetJudgmentCnt(JUDGMENT_TYPE.GREAT);
			break;
		case MISSIONTYPE.Good:
			num = Singleton<GameManager>.instance.ResultData.GetJudgmentCnt(JUDGMENT_TYPE.GOOD);
			break;
		case MISSIONTYPE.Poor:
			num = Singleton<GameManager>.instance.ResultData.GetJudgmentCnt(JUDGMENT_TYPE.POOR);
			break;
		case MISSIONTYPE.Break:
			num = Singleton<GameManager>.instance.ResultData.GetJudgmentCnt(JUDGMENT_TYPE.BREAK);
			break;
		case MISSIONTYPE.PerfectPlay:
			if (Singleton<GameManager>.instance.ResultData.IsPerfectPlay())
			{
				num = 1;
			}
			break;
		case MISSIONTYPE.FeverCount:
			num = Singleton<GameManager>.instance.ResultData.FEVERCOUNT;
			break;
		case MISSIONTYPE.FeverBonus:
			num = Singleton<GameManager>.instance.ResultData.GetFeverBonus();
			break;
		case MISSIONTYPE.FeverMultiple_X2:
			num = Singleton<GameManager>.instance.ResultData.FEVERCOUNT_x2;
			break;
		case MISSIONTYPE.FeverMultiple_X3:
			num = Singleton<GameManager>.instance.ResultData.FEVERCOUNT_x3;
			break;
		case MISSIONTYPE.FeverMultiple_X4:
			num = Singleton<GameManager>.instance.ResultData.FEVERCOUNT_x4;
			break;
		case MISSIONTYPE.FeverMultiple_X5:
			num = Singleton<GameManager>.instance.ResultData.FEVERCOUNT_x5;
			break;
		case MISSIONTYPE.ExtremeCount:
			num = Singleton<GameManager>.instance.ResultData.EXTREMECOUNT;
			break;
		case MISSIONTYPE.ExtremeBonus:
			num = Singleton<GameManager>.instance.ResultData.EXTREMEBONUS;
			break;
		case MISSIONTYPE.ExtremeMultiple_X2:
			num = Singleton<GameManager>.instance.ResultData.EXTREMECOUNT_x2;
			break;
		case MISSIONTYPE.ExtremeMultiple_X3:
			num = Singleton<GameManager>.instance.ResultData.EXTREMECOUNT_x3;
			break;
		case MISSIONTYPE.Accuracy:
			num = Singleton<GameManager>.instance.ResultData.GetAccuracy();
			break;
		case MISSIONTYPE.AllCombo:
			if (Singleton<GameManager>.instance.ResultData.IsAllComboPlay())
			{
				num = 1;
			}
			break;
		case MISSIONTYPE.MaxCombo:
			num = Singleton<GameManager>.instance.ResultData.MAXCOMBO;
			break;
		case MISSIONTYPE.Score:
			num = Singleton<GameManager>.instance.ResultData.SCORE;
			break;
		case MISSIONTYPE.Clear:
			num = 1;
			break;
		}
		return num;
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x0004AD30 File Offset: 0x00048F30
	public bool IsClear(int iNum)
	{
		MISSIONTYPE missiontype = this.ArrMissionType[iNum];
		int chekcNum = this.GetChekcNum(iNum);
		return this.IsCheckClear(iNum, chekcNum);
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x0004AD58 File Offset: 0x00048F58
	private bool IsCheckClear(int iNum, int iCount)
	{
		MISSIONTYPE missiontype = this.ArrMissionType[iNum];
		MISSIONTERM missionterm = this.ArrMissionTerm[iNum];
		int num = this.ArrMissionCount[iNum];
		switch (missiontype + 1)
		{
		case MISSIONTYPE.Rank_F:
			return true;
		case MISSIONTYPE.Rank_D:
		case MISSIONTYPE.Rank_C:
		case MISSIONTYPE.Rank_B:
		case MISSIONTYPE.Rank_A:
		case MISSIONTYPE.Rank_AP:
		case MISSIONTYPE.Rank_APP:
		case MISSIONTYPE.Rank_S:
		case MISSIONTYPE.Rank_SP:
		case MISSIONTYPE.Rank_SPP:
		case MISSIONTYPE.Perfect:
			num = (int)missiontype;
			if (missionterm == MISSIONTERM.Over && num <= iCount)
			{
				return true;
			}
			if (missionterm == MISSIONTERM.Same && num == iCount)
			{
				return true;
			}
			if (missionterm == MISSIONTERM.Under && num >= iCount)
			{
				return true;
			}
			break;
		case MISSIONTYPE.Great:
		case MISSIONTYPE.Good:
		case MISSIONTYPE.Poor:
		case MISSIONTYPE.Break:
		case MISSIONTYPE.PerfectPlay:
		case MISSIONTYPE.FeverBonus:
		case MISSIONTYPE.FeverMultiple_X2:
		case MISSIONTYPE.FeverMultiple_X3:
		case MISSIONTYPE.FeverMultiple_X4:
		case MISSIONTYPE.FeverMultiple_X5:
		case MISSIONTYPE.ExtremeCount:
		case MISSIONTYPE.ExtremeBonus:
		case MISSIONTYPE.ExtremeMultiple_X2:
		case MISSIONTYPE.ExtremeMultiple_X3:
		case MISSIONTYPE.ClearMember:
		case MISSIONTYPE.Accuracy:
		case MISSIONTYPE.AllCombo:
		case MISSIONTYPE.Score:
		case MISSIONTYPE.Clear:
			if (missionterm == MISSIONTERM.Over && num <= iCount)
			{
				return true;
			}
			if (missionterm == MISSIONTERM.Same && num == iCount)
			{
				return true;
			}
			if (missionterm == MISSIONTERM.Under && num >= iCount)
			{
				return true;
			}
			break;
		case MISSIONTYPE.FeverCount:
		case MISSIONTYPE.MaxCombo:
		case MISSIONTYPE.MAX:
			if (0 < iCount)
			{
				return true;
			}
			break;
		}
		return false;
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x06000A6E RID: 2670 RVA: 0x0000A374 File Offset: 0x00008574
	public bool AllClear
	{
		get
		{
			return this.IsClear(0) && this.IsClear(1);
		}
	}

	// Token: 0x040009D5 RID: 2517
	public int iType;

	// Token: 0x040009D6 RID: 2518
	public int iPackId;

	// Token: 0x040009D7 RID: 2519
	public string strPackTitle;

	// Token: 0x040009D8 RID: 2520
	public int iMissionId;

	// Token: 0x040009D9 RID: 2521
	public string strMissionTitle;

	// Token: 0x040009DA RID: 2522
	public int iMissionLevel;

	// Token: 0x040009DB RID: 2523
	public int iMissionCost;

	// Token: 0x040009DC RID: 2524
	public int iMissionOpenLevel;

	// Token: 0x040009DD RID: 2525
	public int iMissionOpenMission;

	// Token: 0x040009DE RID: 2526
	public string strOpenDate;

	// Token: 0x040009DF RID: 2527
	public string strEndDate;

	// Token: 0x040009E0 RID: 2528
	public int[] Song = new int[3];

	// Token: 0x040009E1 RID: 2529
	public PTLEVEL[] Pattern = new PTLEVEL[]
	{
		PTLEVEL.EZ,
		PTLEVEL.EZ,
		PTLEVEL.EZ
	};

	// Token: 0x040009E2 RID: 2530
	public DiscInfo[] ArrDiscInof = new DiscInfo[3];

	// Token: 0x040009E3 RID: 2531
	public EFFECTOR_SPEED Eff_Speed = EFFECTOR_SPEED.NONE;

	// Token: 0x040009E4 RID: 2532
	public EFFECTOR_FADER Eff_Fader;

	// Token: 0x040009E5 RID: 2533
	public EFFECTOR_RAND Eff_Rand;

	// Token: 0x040009E6 RID: 2534
	public MISSIONTYPE[] ArrMissionType = new MISSIONTYPE[]
	{
		MISSIONTYPE.None,
		MISSIONTYPE.None,
		MISSIONTYPE.None
	};

	// Token: 0x040009E7 RID: 2535
	public MISSIONTERM[] ArrMissionTerm = new MISSIONTERM[]
	{
		MISSIONTERM.None,
		MISSIONTERM.None,
		MISSIONTERM.None
	};

	// Token: 0x040009E8 RID: 2536
	public int[] ArrMissionCount = new int[3];

	// Token: 0x040009E9 RID: 2537
	public int iRewardBeatPoint;

	// Token: 0x040009EA RID: 2538
	public int iRewardClubPoint;

	// Token: 0x040009EB RID: 2539
	public int iRewardExp;

	// Token: 0x040009EC RID: 2540
	public int iRewardDjIcon;

	// Token: 0x040009ED RID: 2541
	public int iRewardSong;

	// Token: 0x040009EE RID: 2542
	public PTLEVEL[] RewardPattern = new PTLEVEL[]
	{
		PTLEVEL.EZ,
		PTLEVEL.EZ,
		PTLEVEL.EZ
	};

	// Token: 0x040009EF RID: 2543
	public string strServerKey;
}
