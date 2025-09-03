using System;

// Token: 0x02000103 RID: 259
public class RESULTDATA
{
	// Token: 0x1700019D RID: 413
	// (get) Token: 0x060008F3 RID: 2291 RVA: 0x0000973C File Offset: 0x0000793C
	// (set) Token: 0x060008F4 RID: 2292 RVA: 0x00009744 File Offset: 0x00007944
	public int SCORE
	{
		get
		{
			return this.m_iScore;
		}
		set
		{
			this.m_iScore = value;
			if (this.BESTSCORE < this.m_iScore)
			{
				this.BESTSCORE = this.m_iScore;
			}
		}
	}

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x060008F5 RID: 2293 RVA: 0x0000976A File Offset: 0x0000796A
	// (set) Token: 0x060008F6 RID: 2294 RVA: 0x00009772 File Offset: 0x00007972
	public int BESTSCORE { get; set; }

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x060008F7 RID: 2295 RVA: 0x0000977B File Offset: 0x0000797B
	// (set) Token: 0x060008F8 RID: 2296 RVA: 0x00009783 File Offset: 0x00007983
	public int NOTESCORE { get; set; }

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x060008F9 RID: 2297 RVA: 0x0000978C File Offset: 0x0000798C
	// (set) Token: 0x060008FA RID: 2298 RVA: 0x00009794 File Offset: 0x00007994
	public int COMBO
	{
		get
		{
			return this.m_iCombo;
		}
		set
		{
			this.m_iCombo = value;
			if (this.MAXCOMBO < this.m_iCombo)
			{
				this.MAXCOMBO = this.m_iCombo;
			}
		}
	}

	// Token: 0x170001A1 RID: 417
	// (get) Token: 0x060008FB RID: 2299 RVA: 0x000097BA File Offset: 0x000079BA
	// (set) Token: 0x060008FC RID: 2300 RVA: 0x000097C2 File Offset: 0x000079C2
	public int MAXCOMBO { get; set; }

	// Token: 0x170001A2 RID: 418
	// (get) Token: 0x060008FD RID: 2301 RVA: 0x000097CB File Offset: 0x000079CB
	// (set) Token: 0x060008FE RID: 2302 RVA: 0x000097D3 File Offset: 0x000079D3
	public int TOTAL_NOBONUSCOMBOCOUNT { get; set; }

	// Token: 0x170001A3 RID: 419
	// (get) Token: 0x060008FF RID: 2303 RVA: 0x000097DC File Offset: 0x000079DC
	// (set) Token: 0x06000900 RID: 2304 RVA: 0x000097E4 File Offset: 0x000079E4
	public int TOTAL_COMBOCOUNT { get; set; }

	// Token: 0x170001A4 RID: 420
	// (get) Token: 0x06000901 RID: 2305 RVA: 0x000097ED File Offset: 0x000079ED
	// (set) Token: 0x06000902 RID: 2306 RVA: 0x000097F5 File Offset: 0x000079F5
	public int EXTREMEBONUS { get; set; }

	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x06000903 RID: 2307 RVA: 0x000097FE File Offset: 0x000079FE
	// (set) Token: 0x06000904 RID: 2308 RVA: 0x00009806 File Offset: 0x00007A06
	public int TOTALNOTECOUNT { get; set; }

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x06000905 RID: 2309 RVA: 0x0000980F File Offset: 0x00007A0F
	// (set) Token: 0x06000906 RID: 2310 RVA: 0x00009817 File Offset: 0x00007A17
	public int FEVERCOUNT { get; set; }

	// Token: 0x170001A7 RID: 423
	// (get) Token: 0x06000907 RID: 2311 RVA: 0x00009820 File Offset: 0x00007A20
	// (set) Token: 0x06000908 RID: 2312 RVA: 0x00009828 File Offset: 0x00007A28
	public int FEVERCOUNT_x2 { get; set; }

	// Token: 0x170001A8 RID: 424
	// (get) Token: 0x06000909 RID: 2313 RVA: 0x00009831 File Offset: 0x00007A31
	// (set) Token: 0x0600090A RID: 2314 RVA: 0x00009839 File Offset: 0x00007A39
	public int FEVERCOUNT_x3 { get; set; }

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x0600090B RID: 2315 RVA: 0x00009842 File Offset: 0x00007A42
	// (set) Token: 0x0600090C RID: 2316 RVA: 0x0000984A File Offset: 0x00007A4A
	public int FEVERCOUNT_x4 { get; set; }

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x0600090D RID: 2317 RVA: 0x00009853 File Offset: 0x00007A53
	// (set) Token: 0x0600090E RID: 2318 RVA: 0x0000985B File Offset: 0x00007A5B
	public int FEVERCOUNT_x5 { get; set; }

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x0600090F RID: 2319 RVA: 0x00009864 File Offset: 0x00007A64
	// (set) Token: 0x06000910 RID: 2320 RVA: 0x0000986C File Offset: 0x00007A6C
	public int EXTREMECOUNT { get; set; }

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06000911 RID: 2321 RVA: 0x00009875 File Offset: 0x00007A75
	// (set) Token: 0x06000912 RID: 2322 RVA: 0x0000987D File Offset: 0x00007A7D
	public int EXTREMECOUNT_x2 { get; set; }

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06000913 RID: 2323 RVA: 0x00009886 File Offset: 0x00007A86
	// (set) Token: 0x06000914 RID: 2324 RVA: 0x0000988E File Offset: 0x00007A8E
	public int EXTREMECOUNT_x3 { get; set; }

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06000915 RID: 2325 RVA: 0x00009897 File Offset: 0x00007A97
	// (set) Token: 0x06000916 RID: 2326 RVA: 0x0000989F File Offset: 0x00007A9F
	public float FEVER_GAGE { get; set; }

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06000917 RID: 2327 RVA: 0x000098A8 File Offset: 0x00007AA8
	// (set) Token: 0x06000918 RID: 2328 RVA: 0x000098B0 File Offset: 0x00007AB0
	public float LIFE_GAGE { get; set; }

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x06000919 RID: 2329 RVA: 0x000098B9 File Offset: 0x00007AB9
	// (set) Token: 0x0600091A RID: 2330 RVA: 0x000098C1 File Offset: 0x00007AC1
	public bool ONEXTREME { get; set; }

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x0600091B RID: 2331 RVA: 0x000098CA File Offset: 0x00007ACA
	// (set) Token: 0x0600091C RID: 2332 RVA: 0x000098D2 File Offset: 0x00007AD2
	public int EXTREMECOMBO { get; set; }

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x0600091D RID: 2333 RVA: 0x000098DB File Offset: 0x00007ADB
	// (set) Token: 0x0600091E RID: 2334 RVA: 0x000098E3 File Offset: 0x00007AE3
	public int EXP { get; set; }

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x0600091F RID: 2335 RVA: 0x000098EC File Offset: 0x00007AEC
	// (set) Token: 0x06000920 RID: 2336 RVA: 0x000098F4 File Offset: 0x00007AF4
	public int BEATPOINT { get; set; }

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06000921 RID: 2337 RVA: 0x000098FD File Offset: 0x00007AFD
	// (set) Token: 0x06000922 RID: 2338 RVA: 0x00009905 File Offset: 0x00007B05
	public int ALLCLEAR_EXP { get; set; }

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x06000923 RID: 2339 RVA: 0x0000990E File Offset: 0x00007B0E
	// (set) Token: 0x06000924 RID: 2340 RVA: 0x00009916 File Offset: 0x00007B16
	public int ALLCLEAR_BEATPOINT { get; set; }

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x06000925 RID: 2341 RVA: 0x0000991F File Offset: 0x00007B1F
	// (set) Token: 0x06000926 RID: 2342 RVA: 0x00009927 File Offset: 0x00007B27
	public PTLEVEL PTTYPE { get; set; }

	// Token: 0x06000927 RID: 2343 RVA: 0x00009930 File Offset: 0x00007B30
	public void SetDiscInfo()
	{
		this.DISCINFO = Singleton<SongManager>.instance.GetDiscInfo(0);
		this.PTTYPE = this.DISCINFO.GetFirstPtInfo().PTTYPE;
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x00009959 File Offset: 0x00007B59
	public void Init()
	{
		this.InitData();
		this.InitContinueData();
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x00009967 File Offset: 0x00007B67
	private void InitContinueData()
	{
		this.FEVER_GAGE = 0f;
		this.LIFE_GAGE = GameData.MAXENERGY;
		this.InitExtremeCombo();
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x00009985 File Offset: 0x00007B85
	public void InitExtremeCombo()
	{
		this.EXTREMECOMBO = 0;
		this.EXTREMESTATE = EXTREME_STATE.NONE;
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x00043954 File Offset: 0x00041B54
	private void InitData()
	{
		for (int i = 0; i < 5; i++)
		{
			this.m_arrTotalCnt[i] = 0;
		}
		this.ONEXTREME = false;
		this.m_iCombo = 0;
		this.MAXCOMBO = 0;
		this.TOTAL_NOBONUSCOMBOCOUNT = 0;
		this.TOTAL_COMBOCOUNT = 0;
		this.m_fAccuracy = 0f;
		this.m_iScore = 0;
		this.BESTSCORE = 0;
		this.NOTESCORE = 0;
		this.EXTREMEBONUS = 0;
		this.TOTALNOTECOUNT = 0;
		this.EXTREMECOUNT = 0;
		this.EXTREMECOUNT_x2 = 0;
		this.EXTREMECOUNT_x3 = 0;
		this.FEVERCOUNT = 0;
		this.FEVERCOUNT_x2 = 0;
		this.FEVERCOUNT_x3 = 0;
		this.FEVERCOUNT_x4 = 0;
		this.FEVERCOUNT_x5 = 0;
		this.m_eGrade = GRADE.F;
		this.m_eEmblem = EMBLEM.NONE;
		this.EXP = 0;
		this.BEATPOINT = 0;
		this.ALLCLEAR_EXP = 0;
		this.ALLCLEAR_BEATPOINT = 0;
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x00043A30 File Offset: 0x00041C30
	public RESULTDATA RestoreData(int iTotalStage)
	{
		this.InitData();
		for (int i = 0; i < iTotalStage; i++)
		{
			RESULTDATA stageResult = Singleton<GameManager>.instance.GetStageResult(i);
			this.SCORE += stageResult.SCORE;
			this.COMBO += stageResult.COMBO;
			if (this.BESTSCORE < stageResult.BESTSCORE)
			{
				this.BESTSCORE = stageResult.BESTSCORE;
			}
			this.NOTESCORE += stageResult.NOTESCORE;
			if (this.MAXCOMBO < stageResult.MAXCOMBO)
			{
				this.MAXCOMBO = stageResult.MAXCOMBO;
			}
			this.EXTREMEBONUS += stageResult.EXTREMEBONUS;
			this.TOTAL_NOBONUSCOMBOCOUNT += stageResult.TOTAL_NOBONUSCOMBOCOUNT;
			this.TOTAL_COMBOCOUNT += stageResult.TOTAL_COMBOCOUNT;
			this.FEVERCOUNT += stageResult.FEVERCOUNT;
			this.FEVERCOUNT_x2 += stageResult.FEVERCOUNT_x2;
			this.FEVERCOUNT_x3 += stageResult.FEVERCOUNT_x3;
			this.FEVERCOUNT_x4 += stageResult.FEVERCOUNT_x4;
			this.FEVERCOUNT_x5 += stageResult.FEVERCOUNT_x5;
			this.EXTREMECOUNT += this.EXTREMECOUNT;
			this.EXTREMECOUNT_x2 += this.EXTREMECOUNT_x2;
			this.EXTREMECOUNT_x3 += this.EXTREMECOUNT_x3;
			for (int j = 0; j < 5; j++)
			{
				this.m_arrTotalCnt[j] += stageResult.m_arrTotalCnt[j];
			}
		}
		return this;
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x00043BD0 File Offset: 0x00041DD0
	public void SetJudgment(JUDGMENT_TYPE eType)
	{
		this.TOTALNOTECOUNT++;
		this.NOTESCORE += GameData.Judgment_Score[(int)eType];
		this.m_arrTotalCnt[(int)eType]++;
		this.SCORE = this.NOTESCORE + this.GetMaxComboBonus() + this.GetFeverBonus() + this.EXTREMEBONUS;
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x00043C34 File Offset: 0x00041E34
	public int GetJudgmentCnt(JUDGMENT_TYPE eType)
	{
		return this.m_arrTotalCnt[(int)eType];
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x00043C4C File Offset: 0x00041E4C
	public bool IsPerfectPlay()
	{
		int num = 0;
		for (int i = 0; i < 5; i++)
		{
			if (i != 4)
			{
				num += this.m_arrTotalCnt[i];
			}
		}
		return 0 == num;
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x00009995 File Offset: 0x00007B95
	public bool IsAllComboPlay()
	{
		return 0 == this.m_arrTotalCnt[0];
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x000099A2 File Offset: 0x00007BA2
	public int GetMaxComboBonus()
	{
		return GameData.MAXCOMBO_SCORE * this.MAXCOMBO;
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x000099B0 File Offset: 0x00007BB0
	public int GetFeverBonus()
	{
		return GameData.MAXFEVER_SCORE * (this.TOTAL_COMBOCOUNT - this.TOTAL_NOBONUSCOMBOCOUNT);
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x00043C84 File Offset: 0x00041E84
	public int GetAccuracy()
	{
		int num = 0;
		if (0 < this.TOTALNOTECOUNT)
		{
			this.SetAccuracy();
			num = (int)(this.m_fAccuracy * 100f);
		}
		return num;
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x00043CB4 File Offset: 0x00041EB4
	private void SetAccuracy()
	{
		int num = this.TOTALNOTECOUNT * GameData.Judgment_Score[4];
		this.m_fAccuracy = (float)this.NOTESCORE / (float)num;
	}

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x06000935 RID: 2357 RVA: 0x000099C5 File Offset: 0x00007BC5
	public float Accuracy
	{
		get
		{
			this.SetAccuracy();
			return this.m_fAccuracy;
		}
	}

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x06000936 RID: 2358 RVA: 0x000099D3 File Offset: 0x00007BD3
	// (set) Token: 0x06000937 RID: 2359 RVA: 0x000099F3 File Offset: 0x00007BF3
	public GRADE GRADETYPE
	{
		get
		{
			this.GetAccuracy();
			this.m_eGrade = GameData.GetGrade(this.m_fAccuracy);
			return this.m_eGrade;
		}
		set
		{
			this.m_eGrade = value;
		}
	}

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x06000938 RID: 2360 RVA: 0x00043CE0 File Offset: 0x00041EE0
	public GRADE TOTALGRADETYPE
	{
		get
		{
			float num = 0f;
			for (int i = 0; i < 3; i++)
			{
				RESULTDATA stageResult = Singleton<GameManager>.instance.GetStageResult(i);
				num += stageResult.Accuracy;
			}
			if (num != 0f)
			{
				num /= 3f;
			}
			return GameData.GetGrade(num);
		}
	}

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x06000939 RID: 2361 RVA: 0x000099FC File Offset: 0x00007BFC
	// (set) Token: 0x0600093A RID: 2362 RVA: 0x00009A15 File Offset: 0x00007C15
	public EMBLEM EMBLEMTYPE
	{
		get
		{
			this.m_eEmblem = GameData.GetEmblem(this.GRADETYPE);
			return this.m_eEmblem;
		}
		set
		{
			this.m_eEmblem = value;
		}
	}

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x0600093B RID: 2363 RVA: 0x00009A1E File Offset: 0x00007C1E
	public string TrophyName
	{
		get
		{
			return this.EMBLEMTYPE.ToString() + "-" + this.PTTYPE.ToString();
		}
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x00043D34 File Offset: 0x00041F34
	public void SaveStage()
	{
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.HAUSMIX)
		{
			this.SaveHausMixStage();
		}
		else if (Singleton<SongManager>.instance.Mode == GAMEMODE.RAVEUP)
		{
			int stage = GameData.Stage;
			this.SaveRaveUpStage(stage);
		}
		else if (Singleton<SongManager>.instance.Mode == GAMEMODE.MISSION)
		{
			int num = GameData.Stage;
			if (3 < num)
			{
				num = 2;
			}
			this.LIFE_GAGE = GameData.MAXENERGY;
			this.SaveRaveUpStage(num);
		}
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x00009A4A File Offset: 0x00007C4A
	private void SaveHausMixStage()
	{
		this.CalculateStageBeatPoint(null);
		this.CalculateStageExp(null);
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x00043DB0 File Offset: 0x00041FB0
	private void SaveRaveUpStage(int iStage)
	{
		RESULTDATA resultdata = Singleton<GameManager>.instance.TotalResult[iStage];
		if (iStage == 0)
		{
			for (int i = 0; i < 5; i++)
			{
				resultdata.m_arrTotalCnt[i] = this.m_arrTotalCnt[i];
			}
			resultdata.SCORE = this.SCORE;
			resultdata.COMBO = this.COMBO;
			resultdata.NOTESCORE = this.NOTESCORE;
			resultdata.EXTREMEBONUS = this.EXTREMEBONUS;
			resultdata.TOTAL_NOBONUSCOMBOCOUNT = this.TOTAL_NOBONUSCOMBOCOUNT;
			resultdata.TOTAL_COMBOCOUNT = this.TOTAL_COMBOCOUNT;
			resultdata.TOTALNOTECOUNT = this.TOTALNOTECOUNT;
			resultdata.NOTESCORE = this.NOTESCORE;
			resultdata.FEVERCOUNT = this.FEVERCOUNT;
			resultdata.FEVERCOUNT_x2 = this.FEVERCOUNT_x2;
			resultdata.FEVERCOUNT_x3 = this.FEVERCOUNT_x3;
			resultdata.FEVERCOUNT_x4 = this.FEVERCOUNT_x4;
			resultdata.FEVERCOUNT_x5 = this.FEVERCOUNT_x5;
			resultdata.EXTREMECOUNT = this.EXTREMECOUNT;
			resultdata.EXTREMECOUNT_x2 = this.EXTREMECOUNT_x2;
			resultdata.EXTREMECOUNT_x3 = this.EXTREMECOUNT_x3;
			resultdata.PTTYPE = this.PTTYPE;
			resultdata.DISCINFO = this.DISCINFO;
		}
		else
		{
			for (int j = 0; j < 5; j++)
			{
				resultdata.m_arrTotalCnt[j] = this.m_arrTotalCnt[j];
			}
			resultdata.SCORE = this.SCORE;
			resultdata.COMBO = this.COMBO;
			resultdata.NOTESCORE = this.NOTESCORE;
			resultdata.EXTREMEBONUS = this.EXTREMEBONUS;
			resultdata.TOTAL_NOBONUSCOMBOCOUNT = this.TOTAL_NOBONUSCOMBOCOUNT;
			resultdata.TOTAL_COMBOCOUNT = this.TOTAL_COMBOCOUNT;
			resultdata.TOTALNOTECOUNT = this.TOTALNOTECOUNT;
			resultdata.NOTESCORE = this.NOTESCORE;
			resultdata.FEVERCOUNT = this.FEVERCOUNT;
			resultdata.FEVERCOUNT_x2 = this.FEVERCOUNT_x2;
			resultdata.FEVERCOUNT_x3 = this.FEVERCOUNT_x3;
			resultdata.FEVERCOUNT_x4 = this.FEVERCOUNT_x4;
			resultdata.FEVERCOUNT_x5 = this.FEVERCOUNT_x5;
			resultdata.EXTREMECOUNT = this.EXTREMECOUNT;
			resultdata.EXTREMECOUNT_x2 = this.EXTREMECOUNT_x2;
			resultdata.EXTREMECOUNT_x3 = this.EXTREMECOUNT_x3;
			resultdata.PTTYPE = this.PTTYPE;
			resultdata.DISCINFO = this.DISCINFO;
			for (int k = 0; k < iStage; k++)
			{
				RESULTDATA resultdata2 = Singleton<GameManager>.instance.TotalResult[k];
				for (int l = 0; l < 5; l++)
				{
					resultdata.m_arrTotalCnt[l] -= resultdata2.m_arrTotalCnt[l];
				}
				resultdata.SCORE -= resultdata2.SCORE;
				resultdata.COMBO -= resultdata2.COMBO;
				resultdata.NOTESCORE -= resultdata2.NOTESCORE;
				resultdata.EXTREMEBONUS -= resultdata2.EXTREMEBONUS;
				resultdata.TOTAL_NOBONUSCOMBOCOUNT -= resultdata2.TOTAL_NOBONUSCOMBOCOUNT;
				resultdata.TOTAL_COMBOCOUNT -= resultdata2.TOTAL_COMBOCOUNT;
				resultdata.TOTALNOTECOUNT -= resultdata2.TOTALNOTECOUNT;
				resultdata.FEVERCOUNT -= resultdata2.FEVERCOUNT;
				resultdata.FEVERCOUNT_x2 -= resultdata2.FEVERCOUNT_x2;
				resultdata.FEVERCOUNT_x3 -= resultdata2.FEVERCOUNT_x3;
				resultdata.FEVERCOUNT_x4 -= resultdata2.FEVERCOUNT_x4;
				resultdata.FEVERCOUNT_x5 -= resultdata2.FEVERCOUNT_x5;
				resultdata.EXTREMECOUNT -= resultdata2.EXTREMECOUNT;
				resultdata.EXTREMECOUNT_x2 -= resultdata2.EXTREMECOUNT_x2;
				resultdata.EXTREMECOUNT_x3 -= resultdata2.EXTREMECOUNT_x3;
			}
		}
		this.CalculateStageBeatPoint(resultdata);
		this.CalculateStageExp(resultdata);
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x00044154 File Offset: 0x00042354
	public void SaveTotalResult()
	{
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.HAUSMIX)
		{
			this.SaveHausMixTotalResult();
		}
		else if (Singleton<SongManager>.instance.Mode == GAMEMODE.RAVEUP)
		{
			this.SaveRaveUpTotalResult();
		}
		else if (Singleton<SongManager>.instance.Mode == GAMEMODE.MISSION)
		{
			this.SaveMissionTotalResult();
		}
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x000441AC File Offset: 0x000423AC
	private void SaveHausMixTotalResult()
	{
		this.SCORE = 0;
		this.COMBO = 0;
		this.BEATPOINT = 0;
		this.EXP = 0;
		for (int i = 0; i < 5; i++)
		{
			this.m_arrTotalCnt[i] = 0;
		}
		int num = 0;
		for (int j = 0; j < 3; j++)
		{
			HouseStage houseStage = Singleton<SongManager>.instance.HouseSelectSong[j];
			num += houseStage.PtDif;
		}
		this.ALLCLEAR_BEATPOINT = num * GameData.ALLCLEAR_ADDBEATPOINT;
		this.ALLCLEAR_EXP = num * GameData.ALLCLEAR_ADDEXP;
		for (int k = 0; k < 3; k++)
		{
			RESULTDATA stageResult = Singleton<GameManager>.instance.GetStageResult(k);
			this.SCORE += stageResult.SCORE;
			this.COMBO += stageResult.COMBO;
			this.EXTREMEBONUS += stageResult.EXTREMEBONUS;
			this.TOTALNOTECOUNT += stageResult.TOTALNOTECOUNT;
			this.NOTESCORE += stageResult.NOTESCORE;
			this.BEATPOINT += stageResult.BEATPOINT;
			this.EXP += stageResult.EXP;
			for (int l = 0; l < 5; l++)
			{
				this.m_arrTotalCnt[l] += stageResult.m_arrTotalCnt[l];
			}
		}
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x00044318 File Offset: 0x00042518
	private void SaveRaveUpTotalResult()
	{
		AlbumInfo currentAlbum = Singleton<SongManager>.instance.GetCurrentAlbum();
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < 4; i++)
		{
			RESULTDATA stageResult = Singleton<GameManager>.instance.GetStageResult(i);
			num += stageResult.BEATPOINT;
			num2 += stageResult.EXP;
		}
		int num3 = num2;
		int num4 = num;
		num = (int)((float)num * GameData.GRADE_BONUSRATE[this.GRADETYPE]);
		num2 = (int)((float)num2 * GameData.GRADE_BONUSRATE[this.GRADETYPE]);
		num = (int)((float)num * GameData.DISCSET_BONUSRATE[currentAlbum.eDifficult]);
		num2 = (int)((float)num2 * GameData.DISCSET_BONUSRATE[currentAlbum.eDifficult]);
		this.EXP = num2;
		this.BEATPOINT = num;
		this.ALLCLEAR_BEATPOINT = this.BEATPOINT - num4;
		this.ALLCLEAR_EXP = this.EXP - num3;
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x000443F0 File Offset: 0x000425F0
	private void SaveMissionTotalResult()
	{
		MissionData mission = Singleton<SongManager>.instance.Mission;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < 3; i++)
		{
			RESULTDATA stageResult = Singleton<GameManager>.instance.GetStageResult(i);
			num += stageResult.BEATPOINT;
			num2 += stageResult.EXP;
		}
		this.EXP = num2;
		this.BEATPOINT = num;
		this.ALLCLEAR_BEATPOINT = mission.iRewardBeatPoint;
		this.ALLCLEAR_EXP = mission.iRewardExp;
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x00044468 File Offset: 0x00042668
	private void CalculateStageBeatPoint(RESULTDATA rData = null)
	{
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.HAUSMIX)
		{
			int stage = GameData.Stage;
			HouseStage houseStage = Singleton<SongManager>.instance.HouseSelectSong[stage];
			int num = GameData.BASEGET_BEATPOINT;
			num += GameData.DIF_BONUSBEATPOINT[houseStage.PtDif];
			num += GameData.PTTYPE_BONUS[houseStage.PtType];
			num = (int)((float)num * GameData.GRADE_BONUSRATE[this.GRADETYPE]);
			this.BEATPOINT = num;
		}
		else if (Singleton<SongManager>.instance.Mode == GAMEMODE.RAVEUP)
		{
			int num2 = GameData.Stage;
			if (4 <= num2)
			{
				num2 = 3;
			}
			RaveUpStage raveUpStage = Singleton<SongManager>.instance.RaveUpSelectSong[num2];
			int num3 = GameData.BASEGET_BEATPOINT;
			num3 += GameData.DIF_BONUSBEATPOINT[raveUpStage.PtDif];
			num3 += GameData.PTTYPE_BONUS[raveUpStage.PtType];
			rData.BEATPOINT = num3;
		}
		else if (Singleton<SongManager>.instance.Mode == GAMEMODE.MISSION)
		{
			int num4 = GameData.Stage;
			MissionData mission = Singleton<SongManager>.instance.Mission;
			if (3 <= num4)
			{
				num4 = 2;
			}
			DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(mission.Song[num4]);
			DiscInfo.PtInfo ptInfo = discInfo.DicPtInfo[mission.Pattern[num4]];
			int num5 = GameData.BASEGET_BEATPOINT;
			num5 += GameData.DIF_BONUSBEATPOINT[ptInfo.iDif];
			num5 += GameData.PTTYPE_BONUS[mission.Pattern[num4]];
			num5 = (int)((float)num5 * GameData.GRADE_BONUSRATE[rData.GRADETYPE]);
			rData.BEATPOINT = num5;
		}
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x00044604 File Offset: 0x00042804
	private void CalculateStageExp(RESULTDATA rData = null)
	{
		if (Singleton<SongManager>.instance.Mode == GAMEMODE.HAUSMIX)
		{
			int stage = GameData.Stage;
			HouseStage houseStage = Singleton<SongManager>.instance.HouseSelectSong[stage];
			int num = GameData.BASEGET_EXP;
			num += GameData.DIF_BONUSEXP[houseStage.PtDif];
			num += GameData.PTTYPE_BONUS[houseStage.PtType];
			num = (int)((float)num * GameData.GRADE_BONUSRATE[this.GRADETYPE]);
			this.EXP = num;
		}
		else if (Singleton<SongManager>.instance.Mode == GAMEMODE.RAVEUP)
		{
			int stage2 = GameData.Stage;
			RaveUpStage raveUpStage = Singleton<SongManager>.instance.RaveUpSelectSong[stage2];
			int num2 = GameData.BASEGET_EXP;
			num2 += GameData.DIF_BONUSEXP[raveUpStage.PtDif];
			num2 += GameData.PTTYPE_BONUS[raveUpStage.PtType];
			rData.EXP = num2;
		}
		else if (Singleton<SongManager>.instance.Mode == GAMEMODE.MISSION)
		{
			int num3 = GameData.Stage;
			MissionData mission = Singleton<SongManager>.instance.Mission;
			if (3 <= num3)
			{
				num3 = 2;
			}
			DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(mission.Song[num3]);
			DiscInfo.PtInfo ptInfo = discInfo.DicPtInfo[mission.Pattern[num3]];
			int num4 = GameData.BASEGET_EXP;
			num4 += GameData.DIF_BONUSEXP[ptInfo.iDif];
			num4 += GameData.PTTYPE_BONUS[mission.Pattern[num3]];
			num4 = (int)((float)num4 * GameData.GRADE_BONUSRATE[rData.GRADETYPE]);
			rData.EXP = num4;
		}
	}

	// Token: 0x04000750 RID: 1872
	public int[] m_arrTotalCnt = new int[5];

	// Token: 0x04000751 RID: 1873
	private int m_iScore;

	// Token: 0x04000752 RID: 1874
	private int m_iCombo;

	// Token: 0x04000753 RID: 1875
	private float m_fAccuracy;

	// Token: 0x04000754 RID: 1876
	private GRADE m_eGrade = GRADE.NON;

	// Token: 0x04000755 RID: 1877
	private EMBLEM m_eEmblem = EMBLEM.NONE;

	// Token: 0x04000756 RID: 1878
	public EXTREME_STATE EXTREMESTATE;

	// Token: 0x04000757 RID: 1879
	public DiscInfo DISCINFO;
}
