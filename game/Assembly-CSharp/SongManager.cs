using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MiniJSON;

// Token: 0x0200013C RID: 316
public class SongManager : Singleton<SongManager>
{
	// Token: 0x170001ED RID: 493
	// (get) Token: 0x06000A14 RID: 2580 RVA: 0x0000A12F File Offset: 0x0000832F
	// (set) Token: 0x06000A15 RID: 2581 RVA: 0x0000A137 File Offset: 0x00008337
	public ArrayList AllRaveUpStage { get; set; }

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x06000A16 RID: 2582 RVA: 0x0000A140 File Offset: 0x00008340
	// (set) Token: 0x06000A17 RID: 2583 RVA: 0x0000A148 File Offset: 0x00008348
	public ArrayList AllRaveUpAlbum { get; set; }

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x06000A18 RID: 2584 RVA: 0x0000A151 File Offset: 0x00008351
	// (set) Token: 0x06000A19 RID: 2585 RVA: 0x0000A159 File Offset: 0x00008359
	public ArrayList AllClubMission { get; set; }

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x06000A1A RID: 2586 RVA: 0x0000A162 File Offset: 0x00008362
	// (set) Token: 0x06000A1B RID: 2587 RVA: 0x0000A16A File Offset: 0x0000836A
	public ArrayList AllSampleUser { get; set; }

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x06000A1C RID: 2588 RVA: 0x0000A173 File Offset: 0x00008373
	// (set) Token: 0x06000A1D RID: 2589 RVA: 0x0000A17B File Offset: 0x0000837B
	public ArrayList AllLevel { get; set; }

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x06000A1E RID: 2590 RVA: 0x0000A184 File Offset: 0x00008384
	// (set) Token: 0x06000A1F RID: 2591 RVA: 0x0000A18C File Offset: 0x0000838C
	public ArrayList AllExtremeGage { get; set; }

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06000A20 RID: 2592 RVA: 0x0000A195 File Offset: 0x00008395
	// (set) Token: 0x06000A21 RID: 2593 RVA: 0x0000A19D File Offset: 0x0000839D
	public ArrayList AllEventData { get; set; }

	// Token: 0x06000A22 RID: 2594 RVA: 0x0000A1A6 File Offset: 0x000083A6
	private void Awake()
	{
		this.InitUnPacker();
		this.Init();
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x00047CFC File Offset: 0x00045EFC
	private void Init()
	{
		this.AllDiscStock = new List<object>();
		this.AllHouseStage = new List<object>();
		this.AllRaveUpAlbum = new ArrayList();
		this.AllRaveUpStage = new ArrayList();
		this.AllClubMission = new ArrayList();
		this.AllSampleUser = new ArrayList();
		this.AllLevel = new ArrayList();
		this.AllExtremeGage = new ArrayList();
		this.AllEventData = new ArrayList();
		this.LoadLocalSampleUser();
		this.LoadDiscStock();
		this.LoadHouseStage();
		this.LoadRaveUp();
		this.LoadLocalCommonData();
		this.LoadLocalLevelUp();
		this.LoadLocalMission();
		this.HouseSelectSong[0] = (HouseStage)this.AllHouseStage[0];
		this.HouseSelectSong[1] = (HouseStage)this.AllHouseStage[1];
		this.HouseSelectSong[2] = (HouseStage)this.AllHouseStage[2];
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x0000A1B4 File Offset: 0x000083B4
	private void InitUnPacker()
	{
		GameData.MACHINE_ID = ConfigManager.Instance.Get<string>("network.machine_id", false);
		Singleton<SoundSourceManager>.instance.InitKeySound();
		Singleton<SoundSourceManager>.instance.InitBGM();
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x0000A1DF File Offset: 0x000083DF
	public byte[] LoadUnPackerFile(string strName)
	{
		if (!File.Exists("../Data/" + strName))
		{
			return new byte[0];
		}
		return File.ReadAllBytes("../Data/" + strName);
	}

	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06000A26 RID: 2598 RVA: 0x00047DE4 File Offset: 0x00045FE4
	// (set) Token: 0x06000A27 RID: 2599 RVA: 0x00047E78 File Offset: 0x00046078
	public int SelectAlbumId
	{
		get
		{
			foreach (object obj in this.AllRaveUpAlbum)
			{
				AlbumInfo albumInfo = (AlbumInfo)obj;
				if (this.m_iSelectAlbumId == albumInfo.Id)
				{
					return this.m_iSelectAlbumId;
				}
			}
			AlbumInfo albumInfo2 = (AlbumInfo)this.AllRaveUpAlbum[0];
			this.m_iSelectAlbumId = albumInfo2.Id;
			return this.m_iSelectAlbumId;
		}
		set
		{
			this.m_iSelectAlbumId = value;
			foreach (object obj in this.AllRaveUpAlbum)
			{
				AlbumInfo albumInfo = (AlbumInfo)obj;
				int iSelectAlbumId = this.m_iSelectAlbumId;
				int id = albumInfo.Id;
			}
		}
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x0000A20A File Offset: 0x0000840A
	private string GetByteToString(byte[] bTemp)
	{
		return Encoding.GetEncoding("UTF-8").GetString(bTemp);
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x00047EE0 File Offset: 0x000460E0
	private void LoadLocalCommonData()
	{
		byte[] array = this.LoadUnPackerFile("Script/Common.csv");
		string byteToString = this.GetByteToString(array);
		this.LoadCommonData(byteToString);
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x00047F08 File Offset: 0x00046108
	private void LoadCommonData(string xmlText)
	{
		xmlText = xmlText.Replace("\r", string.Empty);
		xmlText = xmlText.Replace("\t", string.Empty);
		string[] array = xmlText.Split(new char[] { "\n"[0] });
		int num = array[0].Split(new char[] { ","[0] }).Length;
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(new char[] { ","[0] });
			if (num < array2.Length)
			{
				num = array2.Length;
			}
		}
		int num2 = array.Length;
		string[,] array3 = new string[num, num2];
		for (int j = 0; j < num2; j++)
		{
			string[] array4 = array[j].Split(new char[] { ","[0] });
			for (int k = 0; k < array4.Length; k++)
			{
				array3[k, j] = array4[k];
			}
		}
		for (int l = 0; l < 10; l++)
		{
			GameData.ARR_TIME[l] = float.Parse(array3[2, l]);
		}
		int num3 = 9;
		for (int m = 0; m < 10; m++)
		{
			int num4 = m + 13;
			GameData.RANK_VALUE[num3 - m] = float.Parse(array3[2, num4]) * 0.01f;
		}
		for (int n = 0; n < 10; n++)
		{
			int num5 = n + 24;
			GameData.FEVER_CHANGE_TIME[n] = float.Parse(array3[2, num5]);
		}
		int num6 = 5;
		GameData.Judgment_Score[0] = int.Parse(array3[num6, 4]);
		GameData.Judgment_Score[1] = int.Parse(array3[num6, 3]);
		GameData.Judgment_Score[2] = int.Parse(array3[num6, 2]);
		GameData.Judgment_Score[3] = int.Parse(array3[num6, 1]);
		GameData.Judgment_Score[4] = int.Parse(array3[num6, 0]);
		GameData.FEVER_TIME[0] = float.Parse(array3[num6, 6]);
		GameData.FEVER_TIME[1] = float.Parse(array3[num6, 7]);
		GameData.FEVER_TIME[2] = float.Parse(array3[num6, 8]);
		GameData.FEVER_TIME[3] = float.Parse(array3[num6, 9]);
		GameData.FEVER_TIME[4] = float.Parse(array3[num6, 10]);
		GameData.FEVER_TIME[5] = float.Parse(array3[num6, 11]);
		GameData.EXTREME_TIME[0] = float.Parse(array3[num6, 12]);
		GameData.EXTREME_TIME[1] = float.Parse(array3[num6, 13]);
		GameData.MAXENERGY = float.Parse(array3[num6, 14]);
		GameData.PerfectAdd = float.Parse(array3[num6, 15]);
		GameData.BreakeMinus = float.Parse(array3[num6, 16]);
		GameData.PoorMinus = float.Parse(array3[num6, 17]);
		int num7 = 18;
		GameData.FeverAddGage[0] = float.Parse(array3[num6, num7 + 4]);
		GameData.FeverAddGage[1] = float.Parse(array3[num6, num7 + 3]);
		GameData.FeverAddGage[2] = float.Parse(array3[num6, num7 + 2]);
		GameData.FeverAddGage[3] = float.Parse(array3[num6, num7 + 1]);
		GameData.FeverAddGage[4] = float.Parse(array3[num6, num7]);
		int num8 = 23;
		GameData.CHAOS_UP_SPEED[0] = float.Parse(array3[num6, num8]);
		GameData.CHAOS_UP_SPEED[1] = float.Parse(array3[num6, num8 + 1]);
		GameData.CHAOS_UP_SPEED[2] = float.Parse(array3[num6, num8 + 2]);
		GameData.CHAOS_UP_SPEED[3] = float.Parse(array3[num6, num8 + 3]);
		GameData.CHAOS_UP_SPEED[4] = float.Parse(array3[num6, num8 + 4]);
		GameData.CHAOS_UP_SPEED[5] = float.Parse(array3[num6, num8 + 5]);
		GameData.CHAOS_UP_SPEED[6] = float.Parse(array3[num6, num8 + 6]);
		GameData.CHAOS_UP_SPEED[7] = float.Parse(array3[num6, num8 + 7]);
		int num9 = 31;
		GameData.CHAOS_DN_SPEED[0] = float.Parse(array3[num6, num9]);
		GameData.CHAOS_DN_SPEED[1] = float.Parse(array3[num6, num9 + 1]);
		GameData.CHAOS_DN_SPEED[2] = float.Parse(array3[num6, num9 + 2]);
		GameData.CHAOS_DN_SPEED[3] = float.Parse(array3[num6, num9 + 3]);
		GameData.CHAOS_DN_SPEED[4] = float.Parse(array3[num6, num9 + 4]);
		GameData.CHAOS_DN_SPEED[5] = float.Parse(array3[num6, num9 + 5]);
		GameData.CHAOS_DN_SPEED[6] = float.Parse(array3[num6, num9 + 6]);
		GameData.CHAOS_DN_SPEED[7] = float.Parse(array3[num6, num9 + 7]);
		int num10 = 39;
		GameData.CHAOS_X[0] = float.Parse(array3[num6, num10]);
		GameData.CHAOS_X[1] = float.Parse(array3[num6, num10 + 1]);
		GameData.CHAOS_X[2] = float.Parse(array3[num6, num10 + 2]);
		GameData.CHAOS_X[3] = float.Parse(array3[num6, num10 + 3]);
		GameData.CHAOS_X[4] = float.Parse(array3[num6, num10 + 4]);
		GameData.CHAOS_X[5] = float.Parse(array3[num6, num10 + 5]);
		GameData.CHAOS_X[6] = float.Parse(array3[num6, num10 + 6]);
		GameData.CHAOS_X[7] = float.Parse(array3[num6, num10 + 7]);
		GameData.CHAOS_X[8] = float.Parse(array3[num6, num10 + 8]);
		GameData.CHAOS_X[9] = float.Parse(array3[num6, num10 + 9]);
		GameData.CHAOS_X[10] = float.Parse(array3[num6, num10 + 10]);
		GameData.CHAOS_X[11] = float.Parse(array3[num6, num10 + 11]);
		num6 = 8;
		GameData.JogJudgmentDistance = float.Parse(array3[num6, 0]);
		GameData.FADEOUT_START = float.Parse(array3[num6, 1]);
		GameData.FADEOUT_END = float.Parse(array3[num6, 2]);
		GameData.FADEIN_START = float.Parse(array3[num6, 3]);
		GameData.FADEIN_END = float.Parse(array3[num6, 4]);
		GameData.BLINK_VALUE = float.Parse(array3[num6, 5]);
		GameData.MAXCOMBO = (float)int.Parse(array3[num6, 6]);
		GameData.CHAOS_W_MIN = float.Parse(array3[num6, 8]);
		GameData.CHAOS_W_MAX = float.Parse(array3[num6, 9]);
		GameData.CHAOS_W_TIME = float.Parse(array3[num6, 10]);
		int num11 = num6 - 1;
		GameData.InJudgment_Per[4] = int.Parse(array3[num11, 14]);
		GameData.InJudgment_Per[3] = int.Parse(array3[num11, 17]);
		GameData.InJudgment_Per[2] = int.Parse(array3[num11, 20]);
		GameData.InJudgment_Per[1] = int.Parse(array3[num11, 24]);
		GameData.InJudgment_Per[0] = int.Parse(array3[num11, 25]);
		GameData.InJudgment_View[0] = int.Parse(array3[num11, 12]);
		GameData.InJudgment_View[1] = int.Parse(array3[num11, 13]);
		GameData.InJudgment_View[2] = int.Parse(array3[num11, 14]);
		GameData.InJudgment_View[3] = int.Parse(array3[num11, 15]);
		GameData.InJudgment_View[4] = int.Parse(array3[num11, 16]);
		GameData.InJudgment_View[5] = int.Parse(array3[num11, 17]);
		GameData.InJudgment_View[6] = int.Parse(array3[num11, 18]);
		GameData.InJudgment_View[7] = int.Parse(array3[num11, 19]);
		GameData.InJudgment_View[8] = int.Parse(array3[num11, 20]);
		GameData.InJudgment_View[9] = int.Parse(array3[num11, 21]);
		GameData.InJudgment_View[10] = int.Parse(array3[num11, 22]);
		GameData.InJudgment_View[11] = int.Parse(array3[num11, 23]);
		GameData.InJudgment_View[12] = int.Parse(array3[num11, 24]);
		GameData.InJudgment_View[13] = int.Parse(array3[num11, 25]);
		GameData.InJudgment_Time = int.Parse(array3[num6, 26]);
		GameData.OutJudgment_Per[4] = int.Parse(array3[num11, 30]);
		GameData.OutJudgment_Per[3] = int.Parse(array3[num11, 32]);
		GameData.OutJudgment_Per[2] = int.Parse(array3[num11, 36]);
		GameData.OutJudgment_Per[1] = int.Parse(array3[num11, 40]);
		GameData.OutJudgment_Per[0] = int.Parse(array3[num11, 41]);
		GameData.OutJudgment_View[0] = int.Parse(array3[num11, 28]);
		GameData.OutJudgment_View[1] = int.Parse(array3[num11, 29]);
		GameData.OutJudgment_View[2] = int.Parse(array3[num11, 30]);
		GameData.OutJudgment_View[3] = int.Parse(array3[num11, 31]);
		GameData.OutJudgment_View[4] = int.Parse(array3[num11, 32]);
		GameData.OutJudgment_View[5] = int.Parse(array3[num11, 33]);
		GameData.OutJudgment_View[6] = int.Parse(array3[num11, 34]);
		GameData.OutJudgment_View[7] = int.Parse(array3[num11, 35]);
		GameData.OutJudgment_View[8] = int.Parse(array3[num11, 36]);
		GameData.OutJudgment_View[9] = int.Parse(array3[num11, 37]);
		GameData.OutJudgment_View[10] = int.Parse(array3[num11, 38]);
		GameData.OutJudgment_View[11] = int.Parse(array3[num11, 39]);
		GameData.OutJudgment_View[12] = int.Parse(array3[num11, 40]);
		GameData.OutJudgment_View[13] = int.Parse(array3[num11, 41]);
		GameData.OutJudgment_Time = int.Parse(array3[num6, 42]);
		GameData.TEST_PLAYTIME = float.Parse(array3[num6, 43]);
		GameData.MODE_EZ_DEFICULT[0] = float.Parse(array3[num11, 45]) * 0.01f;
		GameData.MODE_EZ_DEFICULT[1] = float.Parse(array3[num11, 46]) * 0.01f;
		GameData.MODE_EZ_DEFICULT[2] = float.Parse(array3[num11, 47]) * 0.01f;
		GameData.MODE_NM_DEFICULT[0] = float.Parse(array3[num6, 45]) * 0.01f;
		GameData.MODE_NM_DEFICULT[1] = float.Parse(array3[num6, 46]) * 0.01f;
		GameData.MODE_NM_DEFICULT[2] = float.Parse(array3[num6, 47]) * 0.01f;
		GameData.MODE_HD_DEFICULT[0] = float.Parse(array3[num6 + 1, 45]) * 0.01f;
		GameData.MODE_HD_DEFICULT[1] = float.Parse(array3[num6 + 1, 46]) * 0.01f;
		GameData.MODE_HD_DEFICULT[2] = float.Parse(array3[num6 + 1, 47]) * 0.01f;
		GameData.AUTOLONGNOTE = int.Parse(array3[num6, 48]);
		GameData.AUTOMOVENOTE = int.Parse(array3[num6, 49]);
		GameData.MINAUTOLONGNOTE = int.Parse(array3[num6, 50]);
		GameData.DIRMOVE = float.Parse(array3[num6, 51]);
		this.AllExtremeGage.Clear();
		for (int num12 = 1; num12 < num2; num12++)
		{
			if (array3[10, num12] != null && !(string.Empty == array3[10, num12]))
			{
				ExtremeLevelGage extremeLevelGage = new ExtremeLevelGage();
				extremeLevelGage.Level = int.Parse(array3[10, num12]);
				extremeLevelGage.fGage = float.Parse(array3[11, num12]);
				this.AllExtremeGage.Add(extremeLevelGage);
			}
		}
		num6 = 13;
		GameData.PTTYPE_BONUS.Clear();
		GameData.PTTYPE_BONUS.Add(PTLEVEL.EZ, int.Parse(array3[num6, 1]));
		GameData.PTTYPE_BONUS.Add(PTLEVEL.NM, int.Parse(array3[num6, 2]));
		GameData.PTTYPE_BONUS.Add(PTLEVEL.HD, int.Parse(array3[num6, 3]));
		GameData.PTTYPE_BONUS.Add(PTLEVEL.PR, int.Parse(array3[num6, 4]));
		GameData.PTTYPE_BONUS.Add(PTLEVEL.MX, int.Parse(array3[num6, 5]));
		GameData.PTTYPE_BONUS.Add(PTLEVEL.S1, int.Parse(array3[num6, 6]));
		GameData.PTTYPE_BONUS.Add(PTLEVEL.S2, int.Parse(array3[num6, 7]));
		GameData.GRADE_BONUSRATE.Clear();
		GameData.GRADE_BONUSRATE.Add(GRADE.S_PP, float.Parse(array3[num6, 10]) * 0.01f);
		GameData.GRADE_BONUSRATE.Add(GRADE.S_P, float.Parse(array3[num6, 11]) * 0.01f);
		GameData.GRADE_BONUSRATE.Add(GRADE.S, float.Parse(array3[num6, 12]) * 0.01f);
		GameData.GRADE_BONUSRATE.Add(GRADE.A_PP, float.Parse(array3[num6, 13]) * 0.01f);
		GameData.GRADE_BONUSRATE.Add(GRADE.A_P, float.Parse(array3[num6, 14]) * 0.01f);
		GameData.GRADE_BONUSRATE.Add(GRADE.A, float.Parse(array3[num6, 15]) * 0.01f);
		GameData.GRADE_BONUSRATE.Add(GRADE.B, float.Parse(array3[num6, 16]) * 0.01f);
		GameData.GRADE_BONUSRATE.Add(GRADE.C, float.Parse(array3[num6, 17]) * 0.01f);
		GameData.GRADE_BONUSRATE.Add(GRADE.D, float.Parse(array3[num6, 18]) * 0.01f);
		GameData.GRADE_BONUSRATE.Add(GRADE.F, float.Parse(array3[num6, 19]) * 0.01f);
		GameData.DISCSET_BONUSRATE.Clear();
		GameData.DISCSET_BONUSRATE.Add(DISCSET_DIFFICULT.EASY, float.Parse(array3[num6, 22]) * 0.01f);
		GameData.DISCSET_BONUSRATE.Add(DISCSET_DIFFICULT.NORMAL, float.Parse(array3[num6, 23]) * 0.01f);
		GameData.DISCSET_BONUSRATE.Add(DISCSET_DIFFICULT.HARD, float.Parse(array3[num6, 24]) * 0.01f);
		GameData.MAXCOMBO_SCORE = int.Parse(array3[num6, 26]);
		GameData.MAXFEVER_SCORE = int.Parse(array3[num6, 27]);
		GameData.EXTREME_BONUSRATE[0] = float.Parse(array3[num6, 28]);
		GameData.EXTREME_BONUSRATE[1] = float.Parse(array3[num6, 29]);
		GameData.Perfect10LvAdd = float.Parse(array3[num6, 30]);
		GameData.Perfect5LvAdd = float.Parse(array3[num6, 31]);
		GameData.PerfectS1S2Add = float.Parse(array3[num6, 32]);
		num6 = 15;
		GameData.BASEGET_EXP = int.Parse(array3[num6, 2]);
		GameData.ALLCLEAR_ADDEXP = int.Parse(array3[num6, 3]);
		GameData.MISSIONCLEAR_EXP = int.Parse(array3[num6, 4]);
		GameData.DIF_BONUSEXP.Clear();
		GameData.DIF_BONUSBEATPOINT.Clear();
		int num13 = 8;
		for (int num14 = 0; num14 < 12; num14++)
		{
			int num15 = num14 + 1;
			int num16 = num13 + num14;
			GameData.DIF_BONUSEXP.Add(num15, int.Parse(array3[num6, num16]));
			GameData.DIF_BONUSBEATPOINT.Add(num15, int.Parse(array3[num6 + 1, num16]));
		}
		num6 = 16;
		GameData.BASEGET_BEATPOINT = int.Parse(array3[num6, 2]);
		GameData.ALLCLEAR_ADDBEATPOINT = int.Parse(array3[num6, 3]);
		GameData.MISSIONCLEAR_BEATPOINT = int.Parse(array3[num6, 4]);
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x00048F3C File Offset: 0x0004713C
	private void LoadLocalSampleUser()
	{
		byte[] array = this.LoadUnPackerFile("Script/NonNetWorkId.txt");
		string[] array2 = this.GetByteToString(array).Split(new char[] { "\n"[0] });
		this.AllSampleUser.Clear();
		foreach (string text in array2)
		{
			this.AllSampleUser.Add(text);
		}
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x00048FA0 File Offset: 0x000471A0
	private void LoadLocalLevelUp()
	{
		byte[] array = this.LoadUnPackerFile("Script/LevelUp.csv");
		string byteToString = this.GetByteToString(array);
		this.LoadLevelUp(byteToString);
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x00048FC8 File Offset: 0x000471C8
	public void LoadLevelUp(string strTxt)
	{
		strTxt = strTxt.Replace("\r", string.Empty);
		string[] array = strTxt.Split(new char[] { "\n"[0] });
		this.AllLevel.Clear();
		for (int i = 1; i < array.Length; i++)
		{
			LevelData levelData = new LevelData();
			string[] array2 = array[i].Split(new char[] { ","[0] });
			if (!(string.Empty == array2[0]))
			{
				levelData.Level = int.Parse(array2[0]);
				levelData.Exp = int.Parse(array2[1]);
				this.AllLevel.Add(levelData);
			}
		}
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x00049078 File Offset: 0x00047278
	private void LoadLocalMission()
	{
		byte[] array = this.LoadUnPackerFile("Script/Mission.csv");
		string byteToString = this.GetByteToString(array);
		this.LoadMission(byteToString);
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x000490A0 File Offset: 0x000472A0
	public PTLEVEL GetPtIdToPtLevel(int iPtId)
	{
		foreach (object obj in this.AllDiscStock)
		{
			DiscInfo discInfo = (DiscInfo)obj;
			for (PTLEVEL ptlevel = PTLEVEL.EZ; ptlevel < PTLEVEL.MAX; ptlevel++)
			{
				if (discInfo.ContainPt(ptlevel) && discInfo.DicPtInfo[ptlevel].iPtServerId == iPtId)
				{
					return ptlevel;
				}
			}
		}
		return PTLEVEL.EZ;
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x00049124 File Offset: 0x00047324
	private void LoadMission(string strTxt)
	{
		strTxt = strTxt.Replace("\r", string.Empty);
		string[] array = strTxt.Split(new char[] { "\n"[0] });
		this.AllClubMission.Clear();
		for (int i = 1; i < array.Length; i++)
		{
			MissionData missionData = new MissionData();
			string[] array2 = array[i].Split(new char[] { ","[0] });
			if (!(string.Empty == array2[0]))
			{
				missionData.iType = int.Parse(array2[0]);
				missionData.iPackId = int.Parse(array2[1]);
				missionData.strPackTitle = array2[2];
				missionData.iMissionId = int.Parse(array2[3]);
				missionData.strMissionTitle = array2[4];
				missionData.iMissionCost = 0;
				int.TryParse(array2[6], out missionData.iMissionCost);
				missionData.iMissionOpenLevel = int.Parse(array2[7]);
				missionData.iMissionOpenMission = int.Parse(array2[8]);
				int num = 12;
				missionData.Song[0] = int.Parse(array2[num]);
				missionData.Song[1] = int.Parse(array2[num + 1]);
				missionData.Song[2] = int.Parse(array2[num + 2]);
				num = 15;
				for (int j = 0; j < 3; j++)
				{
					int num2 = num + j;
					int enumIndex = GameData.GetEnumIndex<PTLEVEL>(array2[num2]);
					if (enumIndex != -1)
					{
						int num3 = (int)Enum.GetValues(typeof(PTLEVEL)).GetValue(enumIndex);
						missionData.Pattern[j] = (PTLEVEL)num3;
					}
				}
				num = 18;
				int enumIndex2 = GameData.GetEnumIndex<EFFECTOR_SPEED>(array2[num]);
				if (enumIndex2 != -1)
				{
					int num4 = (int)Enum.GetValues(typeof(EFFECTOR_SPEED)).GetValue(enumIndex2);
					missionData.Eff_Speed = (EFFECTOR_SPEED)num4;
				}
				int enumIndex3 = GameData.GetEnumIndex<EFFECTOR_FADER>(array2[num + 1]);
				if (enumIndex3 != -1)
				{
					int num5 = (int)Enum.GetValues(typeof(EFFECTOR_FADER)).GetValue(enumIndex3);
					missionData.Eff_Fader = (EFFECTOR_FADER)num5;
				}
				int enumIndex4 = GameData.GetEnumIndex<EFFECTOR_RAND>(array2[num + 2]);
				if (enumIndex4 != -1)
				{
					int num6 = (int)Enum.GetValues(typeof(EFFECTOR_RAND)).GetValue(enumIndex4);
					missionData.Eff_Rand = (EFFECTOR_RAND)num6;
				}
				num = 21;
				for (int k = 0; k < 3; k++)
				{
					int num7 = num + k * 3;
					int enumIndex5 = GameData.GetEnumIndex<MISSIONTYPE>(array2[num7]);
					if (enumIndex5 != -1)
					{
						int num8 = (int)Enum.GetValues(typeof(MISSIONTYPE)).GetValue(enumIndex5);
						missionData.ArrMissionType[k] = (MISSIONTYPE)num8;
					}
					int num9 = num + 1 + k * 3;
					int enumIndex6 = GameData.GetEnumIndex<MISSIONTERM>(array2[num9]);
					if (enumIndex6 != -1)
					{
						int num10 = (int)Enum.GetValues(typeof(MISSIONTERM)).GetValue(enumIndex6);
						missionData.ArrMissionTerm[k] = (MISSIONTERM)num10;
					}
					int num11 = num + 2 + k * 3;
					missionData.ArrMissionCount[k] = int.Parse(array2[num11]);
				}
				num = 31;
				missionData.iRewardBeatPoint = int.Parse(array2[num]);
				missionData.iRewardClubPoint = int.Parse(array2[num + 1]);
				missionData.iRewardExp = int.Parse(array2[num + 2]);
				missionData.iRewardDjIcon = int.Parse(array2[num + 3]);
				missionData.iRewardSong = int.Parse(array2[num + 4]);
				num = 36;
				int enumIndex7 = GameData.GetEnumIndex<PTLEVEL>(array2[num]);
				if (enumIndex7 != -1)
				{
					int num12 = (int)Enum.GetValues(typeof(PTLEVEL)).GetValue(enumIndex7);
					missionData.RewardPattern[0] = (PTLEVEL)num12;
				}
				num = 39;
				missionData.strServerKey = array2[num];
				MissionPackData missionPackData = this.GetMissionPack(missionData.iPackId);
				if (missionPackData != null)
				{
					if (!missionPackData.ArrMissionData.Contains(missionData))
					{
						missionPackData.ArrMissionData.Add(missionData);
					}
				}
				else
				{
					missionPackData = new MissionPackData();
					missionPackData.iPackId = missionData.iPackId;
					missionPackData.strPackTitle = missionData.strPackTitle;
					if (!missionPackData.ArrMissionData.Contains(missionData))
					{
						missionPackData.ArrMissionData.Add(missionData);
					}
					if (!this.AllClubMission.Contains(missionPackData))
					{
						this.AllClubMission.Add(missionPackData);
					}
				}
			}
		}
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x0004953C File Offset: 0x0004773C
	public MissionPackData GetMissionPack(int iId)
	{
		foreach (object obj in this.AllClubMission)
		{
			MissionPackData missionPackData = (MissionPackData)obj;
			if (missionPackData.iPackId == iId)
			{
				return missionPackData;
			}
		}
		return null;
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x000495A0 File Offset: 0x000477A0
	public bool IsContainPattern(int iSongId, PTLEVEL ePtType)
	{
		foreach (object obj in this.AllDiscStock)
		{
			DiscInfo discInfo = (DiscInfo)obj;
			if (discInfo.Id == iSongId)
			{
				return discInfo.DicPtInfo.ContainsKey(ePtType);
			}
		}
		return false;
	}

	// Token: 0x06000A33 RID: 2611 RVA: 0x0004960C File Offset: 0x0004780C
	public DiscInfo GetDiscInfo(int iId)
	{
		foreach (object obj in this.AllDiscStock)
		{
			DiscInfo discInfo = (DiscInfo)obj;
			if (discInfo.Id == iId)
			{
				return discInfo;
			}
		}
		return (DiscInfo)this.AllDiscStock[0];
	}

	// Token: 0x06000A34 RID: 2612 RVA: 0x00049680 File Offset: 0x00047880
	public DiscInfo GetDiscInfoServerKey(string strServerKey)
	{
		foreach (object obj in this.AllDiscStock)
		{
			DiscInfo discInfo = (DiscInfo)obj;
			if (discInfo.ServerID == strServerKey)
			{
				return discInfo;
			}
		}
		return null;
	}

	// Token: 0x06000A35 RID: 2613 RVA: 0x0000A21C File Offset: 0x0000841C
	public bool IsContainEventPt(int iSong)
	{
		return Singleton<GameManager>.instance.isNewSongEvent && (iSong == 50 || iSong == 51 || iSong == 52);
	}

	// Token: 0x06000A36 RID: 2614 RVA: 0x000496E8 File Offset: 0x000478E8
	public ArrayList GetHouseStage(bool bBase = false)
	{
		int num = GameData.Stage;
		if (Singleton<GameManager>.instance.ONLOGIN && !bBase && Singleton<GameManager>.instance.isAllSongMode)
		{
			num = 3;
		}
		ArrayList arrayList = new ArrayList();
		foreach (object obj in this.AllHouseStage)
		{
			HouseStage houseStage = (HouseStage)obj;
			if (houseStage.iStage - 1 == num)
			{
				arrayList.Add(houseStage);
			}
		}
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		bool flag = false;
		foreach (object obj2 in userData.ArrHausPattern)
		{
			BonusPattern bonusPattern = (BonusPattern)obj2;
			if (num + 1 == bonusPattern.iStage)
			{
				flag = false;
				foreach (object obj3 in arrayList)
				{
					HouseStage houseStage2 = (HouseStage)obj3;
					if (houseStage2.iSong == bonusPattern.iSong)
					{
						houseStage2.DicSelectPt[bonusPattern.PTTYPE] = true;
						flag = true;
					}
				}
				if (!flag)
				{
					HouseStage houseStage3 = new HouseStage();
					houseStage3.Id = this.AllHouseStage.Count;
					houseStage3.iStage = num + 1;
					houseStage3.iSong = bonusPattern.iSong;
					houseStage3.PtType = bonusPattern.PTTYPE;
					houseStage3.DicSelectPt[bonusPattern.PTTYPE] = true;
					arrayList.Add(houseStage3);
				}
			}
		}
		return arrayList;
	}

	// Token: 0x06000A37 RID: 2615 RVA: 0x0000A23D File Offset: 0x0000843D
	public int GetAlbumTotalCnt()
	{
		return this.AllRaveUpAlbum.Count;
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x0000A24A File Offset: 0x0000844A
	public AlbumInfo GetIdxAlbumInfo(int iIdx)
	{
		if (this.AllRaveUpAlbum.Count > iIdx)
		{
			return (AlbumInfo)this.AllRaveUpAlbum[iIdx];
		}
		return (AlbumInfo)this.AllRaveUpAlbum[0];
	}

	// Token: 0x06000A39 RID: 2617 RVA: 0x000498B0 File Offset: 0x00047AB0
	public AlbumInfo GetAlbumInfo(int iId)
	{
		foreach (object obj in this.AllRaveUpAlbum)
		{
			AlbumInfo albumInfo = (AlbumInfo)obj;
			if (albumInfo.Id == iId)
			{
				return albumInfo;
			}
		}
		return (AlbumInfo)this.AllRaveUpAlbum[0];
	}

	// Token: 0x06000A3A RID: 2618 RVA: 0x00049924 File Offset: 0x00047B24
	public AlbumInfo GetAlbumServerKeyInfo(string strId)
	{
		foreach (object obj in this.AllRaveUpAlbum)
		{
			AlbumInfo albumInfo = (AlbumInfo)obj;
			if (albumInfo.AlbumServerId == strId)
			{
				return albumInfo;
			}
		}
		return (AlbumInfo)this.AllRaveUpAlbum[0];
	}

	// Token: 0x06000A3B RID: 2619 RVA: 0x0004999C File Offset: 0x00047B9C
	public ArrayList GetRaveUpAlbumStage(int iAlbum)
	{
		ArrayList arrayList = new ArrayList();
		foreach (object obj in this.AllRaveUpStage)
		{
			RaveUpStage raveUpStage = (RaveUpStage)obj;
			if (raveUpStage.iAlbum == iAlbum && raveUpStage.Id != 7 && raveUpStage.Id != 8)
			{
				arrayList.Add(raveUpStage);
			}
		}
		return arrayList;
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x00049A18 File Offset: 0x00047C18
	public RaveUpStage GetRaveUpAlbumHiddenStage(int iAlbum)
	{
		ArrayList arrayList = new ArrayList();
		foreach (object obj in this.AllRaveUpStage)
		{
			RaveUpStage raveUpStage = (RaveUpStage)obj;
			if (raveUpStage.iAlbum == iAlbum && (raveUpStage.Id == 7 || raveUpStage.Id == 8))
			{
				arrayList.Add(raveUpStage);
			}
		}
		RaveUpStage raveUpStage2 = this.RaveUpSelectSong[0];
		RaveUpStage raveUpStage3 = this.RaveUpSelectSong[1];
		RaveUpStage raveUpStage4 = this.RaveUpSelectSong[2];
		int num = raveUpStage2.Id + raveUpStage3.Id + raveUpStage4.Id;
		int num2 = 0;
		if (10 < num)
		{
			num2 = arrayList.Count - 1;
		}
		return (RaveUpStage)arrayList[num2];
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x00049AEC File Offset: 0x00047CEC
	public RaveUpStage GetRaveUpStage(int iId)
	{
		foreach (object obj in this.AllRaveUpStage)
		{
			RaveUpStage raveUpStage = (RaveUpStage)obj;
			if (raveUpStage.Id == iId)
			{
				return raveUpStage;
			}
		}
		return (RaveUpStage)this.AllRaveUpStage[0];
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x00049B60 File Offset: 0x00047D60
	public DiscInfo GetCurrentDisc()
	{
		Dictionary<int, PTLEVEL> dictionary = new Dictionary<int, PTLEVEL>
		{
			{
				0,
				PTLEVEL.EZ
			},
			{
				1,
				PTLEVEL.NM
			},
			{
				2,
				PTLEVEL.HD
			},
			{
				3,
				PTLEVEL.PR
			},
			{
				4,
				PTLEVEL.MX
			},
			{
				5,
				PTLEVEL.S1
			},
			{
				6,
				PTLEVEL.S2
			},
			{
				7,
				PTLEVEL.MAX
			}
		};
		if (Singleton<GameManager>.instance.inAttract())
		{
			SongManager.DemoSong demoSong = this.demoSongs.FirstOrDefault((SongManager.DemoSong song) => song.id == Singleton<GameManager>.instance.DEMOPLAYNUM);
			this.HouseSelectSong[GameData.Stage].PtType = dictionary[demoSong.pt];
			return this.GetDiscInfo(demoSong.songId);
		}
		if (this.Mode == GAMEMODE.HAUSMIX)
		{
			if (3 <= GameData.Stage)
			{
				GameData.Stage = 2;
			}
			HouseStage houseStage = Singleton<SongManager>.instance.HouseSelectSong[GameData.Stage];
			return this.GetDiscInfo(houseStage.iSong);
		}
		if (this.Mode == GAMEMODE.RAVEUP)
		{
			RaveUpStage raveUpStage = Singleton<SongManager>.instance.RaveUpSelectSong[GameData.Stage];
			return this.GetDiscInfo(raveUpStage.iSong);
		}
		if (this.Mode == GAMEMODE.MISSION)
		{
			MissionData mission = Singleton<SongManager>.instance.Mission;
			return this.GetDiscInfo(mission.Song[GameData.Stage]);
		}
		return this.GetDiscInfo(0);
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x0000A27D File Offset: 0x0000847D
	public AlbumInfo GetCurrentAlbum()
	{
		return this.GetAlbumInfo(this.SelectAlbumId);
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x00049CA0 File Offset: 0x00047EA0
	public MissionData GetMissionData(int iId)
	{
		foreach (object obj in this.AllClubMission)
		{
			foreach (object obj2 in ((MissionPackData)obj).ArrMissionData)
			{
				MissionData missionData = (MissionData)obj2;
				if (missionData.iMissionId == iId)
				{
					return missionData;
				}
			}
		}
		return (MissionData)((MissionPackData)this.AllClubMission[0]).ArrMissionData[0];
	}

	// Token: 0x06000A41 RID: 2625 RVA: 0x00049D68 File Offset: 0x00047F68
	public int GetServerKeyToId(string strServerKey)
	{
		for (int i = 0; i < this.AllClubMission.Count; i++)
		{
			MissionPackData missionPackData = (MissionPackData)this.AllClubMission[i];
			for (int j = 0; j < missionPackData.ArrMissionData.Count; j++)
			{
				MissionData missionData = (MissionData)missionPackData.ArrMissionData[j];
				if (missionData.strServerKey == strServerKey)
				{
					return missionData.iMissionId;
				}
			}
		}
		return -1;
	}

	// Token: 0x06000A42 RID: 2626 RVA: 0x00049DDC File Offset: 0x00047FDC
	public void UpdateEventData(int iSong, PTLEVEL pLv, int iCount)
	{
		for (int i = 0; i < this.AllEventData.Count; i++)
		{
			EventSongData eventSongData = (EventSongData)this.AllEventData[i];
			if (eventSongData.iSongId == iSong && eventSongData.ePt == pLv)
			{
				eventSongData.iCount = iCount;
				return;
			}
		}
		EventSongData eventSongData2 = new EventSongData();
		eventSongData2.iSongId = iSong;
		eventSongData2.ePt = pLv;
		eventSongData2.iCount = iCount;
		this.AllEventData.Add(eventSongData2);
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x00049E54 File Offset: 0x00048054
	public int GetEventSongCount(int iSong, PTLEVEL pLv)
	{
		for (int i = 0; i < this.AllEventData.Count; i++)
		{
			EventSongData eventSongData = (EventSongData)this.AllEventData[i];
			if (eventSongData.iSongId == iSong && eventSongData.ePt == pLv)
			{
				return eventSongData.iCount;
			}
		}
		return 8;
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x00049EA4 File Offset: 0x000480A4
	public int GetExpToLv(int iExp)
	{
		if (0 >= this.AllLevel.Count)
		{
			return 1;
		}
		for (int i = 0; i < this.AllLevel.Count; i++)
		{
			LevelData levelData = (LevelData)this.AllLevel[i];
			if (levelData.MaxExp > iExp)
			{
				return levelData.Level;
			}
		}
		int num = this.AllLevel.Count - 1;
		return ((LevelData)this.AllLevel[num]).Level;
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x00049F20 File Offset: 0x00048120
	public int GetLvMaxExp(int iLv)
	{
		if (0 >= this.AllLevel.Count)
		{
			return 0;
		}
		for (int i = 0; i < this.AllLevel.Count; i++)
		{
			LevelData levelData = (LevelData)this.AllLevel[i];
			if (levelData.Level == iLv)
			{
				return levelData.MaxExp;
			}
		}
		return 0;
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x00049F78 File Offset: 0x00048178
	public int GetLvCurrentExp(int iLv)
	{
		if (0 >= this.AllLevel.Count)
		{
			return 0;
		}
		for (int i = 0; i < this.AllLevel.Count; i++)
		{
			LevelData levelData = (LevelData)this.AllLevel[i];
			if (levelData.Level == iLv)
			{
				return levelData.Exp;
			}
		}
		return 0;
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x00049FD0 File Offset: 0x000481D0
	public float GetExtremeGage(int iLv)
	{
		for (int i = 0; i < this.AllExtremeGage.Count; i++)
		{
			ExtremeLevelGage extremeLevelGage = (ExtremeLevelGage)this.AllExtremeGage[i];
			if (extremeLevelGage.Level >= iLv)
			{
				return extremeLevelGage.fGage;
			}
		}
		return ((ExtremeLevelGage)this.AllExtremeGage[this.AllExtremeGage.Count - 1]).fGage;
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x0004A038 File Offset: 0x00048238
	private void LoadDiscStock()
	{
		List<object> list = (List<object>)(Json.Deserialize(this.ReadSystemJSONFile("musicLibrary")) as Dictionary<string, object>)["songs"];
		this.AllDiscStock.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
			DiscInfo discInfo = new DiscInfo();
			discInfo.Id = Convert.ToInt32(dictionary["id"]);
			discInfo.Name = dictionary["fileName"].ToString();
			discInfo.FullName = dictionary["inGameTitleEn"].ToString();
			discInfo.Genre = dictionary["genre"].ToString();
			discInfo.Difficult = Convert.ToInt32(dictionary["difficulty"]);
			discInfo.Bpm = Convert.ToInt32(dictionary["BPM"]);
			discInfo.Composer = dictionary["composedBy"].ToString();
			discInfo.Artist = dictionary["artist"].ToString();
			Dictionary<PTLEVEL, string> dictionary2 = new Dictionary<PTLEVEL, string>
			{
				{
					PTLEVEL.EZ,
					"EZ"
				},
				{
					PTLEVEL.NM,
					"NM"
				},
				{
					PTLEVEL.HD,
					"HD"
				},
				{
					PTLEVEL.PR,
					"PR"
				},
				{
					PTLEVEL.MX,
					"MX"
				},
				{
					PTLEVEL.S1,
					"S1"
				},
				{
					PTLEVEL.S2,
					"S2"
				}
			};
			Dictionary<PTLEVEL, string> dictionary3 = new Dictionary<PTLEVEL, string>
			{
				{
					PTLEVEL.EZ,
					"EZNote"
				},
				{
					PTLEVEL.NM,
					"NMNote"
				},
				{
					PTLEVEL.HD,
					"HDNote"
				},
				{
					PTLEVEL.PR,
					"PRNote"
				},
				{
					PTLEVEL.MX,
					"MXNote"
				},
				{
					PTLEVEL.S1,
					"S1Note"
				},
				{
					PTLEVEL.S2,
					"S2Note"
				}
			};
			Dictionary<PTLEVEL, string> dictionary4 = new Dictionary<PTLEVEL, string>
			{
				{
					PTLEVEL.EZ,
					"EZCombo"
				},
				{
					PTLEVEL.NM,
					"NMCombo"
				},
				{
					PTLEVEL.HD,
					"HDCombo"
				},
				{
					PTLEVEL.PR,
					"PRCombo"
				},
				{
					PTLEVEL.MX,
					"MXCombo"
				},
				{
					PTLEVEL.S1,
					"S1Combo"
				},
				{
					PTLEVEL.S2,
					"S2Combo"
				}
			};
			string[] array = dictionary["ptInfo"].ToString().Split(new char[] { "_"[0] });
			discInfo.DicPtInfo.Clear();
			for (int j = 0; j < array.Length; j++)
			{
				DiscInfo.PtInfo ptInfo = new DiscInfo.PtInfo();
				PTLEVEL ptlevel = PTLEVEL.EZ;
				string[] array2 = array[j].Split(new char[] { "-"[0] });
				int enumIndex = GameData.GetEnumIndex<PTLEVEL>(array2[0]);
				if (enumIndex != -1)
				{
					ptlevel = (PTLEVEL)((int)Enum.GetValues(typeof(PTLEVEL)).GetValue(enumIndex));
				}
				ptInfo.PTTYPE = ptlevel;
				ptInfo.iDif = int.Parse(array2[1]);
				ptInfo.iPtServerId = Convert.ToInt32(dictionary[dictionary2[ptlevel]]);
				ptInfo.iMaxNote = Convert.ToInt32(dictionary[dictionary3[ptlevel]]);
				ptInfo.iMaxCombo = Convert.ToInt32(dictionary[dictionary4[ptlevel]]);
				if (!discInfo.DicPtInfo.ContainsKey(ptlevel))
				{
					discInfo.DicPtInfo.Add(ptlevel, ptInfo);
				}
			}
			discInfo.ServerID = dictionary["serverId"].ToString();
			discInfo.ServerID = discInfo.ServerID.Replace("\r", string.Empty);
			discInfo.GroupSet = Convert.ToInt32(dictionary["groupSet"]);
			this.AllDiscStock.Add(discInfo);
		}
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x00009F5B File Offset: 0x0000815B
	private string ReadSystemJSONFile(string fileName)
	{
		return File.ReadAllText("../Data/System/JSON/" + fileName + ".json");
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x0004A3E4 File Offset: 0x000485E4
	public void LoadHouseStage()
	{
		this.AllHouseStage.Clear();
		List<object> list = (List<object>)(Json.Deserialize(this.ReadSystemJSONFile("hausStages")) as Dictionary<string, object>)["stages"];
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
			HouseStage houseStage = new HouseStage();
			houseStage.Id = Convert.ToInt32(dictionary["id"]);
			houseStage.iStage = Convert.ToInt32(dictionary["stage"]);
			houseStage.iSong = Convert.ToInt32(dictionary["songId"]);
			Dictionary<PTLEVEL, string> dictionary2 = new Dictionary<PTLEVEL, string>
			{
				{
					PTLEVEL.EZ,
					"EZ"
				},
				{
					PTLEVEL.NM,
					"NM"
				},
				{
					PTLEVEL.HD,
					"HD"
				},
				{
					PTLEVEL.PR,
					"PR"
				},
				{
					PTLEVEL.MX,
					"MX"
				},
				{
					PTLEVEL.S1,
					"S1"
				},
				{
					PTLEVEL.S2,
					"S2"
				}
			};
			for (PTLEVEL ptlevel = PTLEVEL.EZ; ptlevel < PTLEVEL.MAX; ptlevel++)
			{
				houseStage.DicSelectPt[ptlevel] = Convert.ToBoolean(dictionary[dictionary2[ptlevel]]);
			}
			this.AllHouseStage.Add(houseStage);
		}
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x06000A4B RID: 2635 RVA: 0x0000A28B File Offset: 0x0000848B
	// (set) Token: 0x06000A4C RID: 2636 RVA: 0x0000A293 File Offset: 0x00008493
	public List<object> AllDiscStock { get; private set; }

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x06000A4D RID: 2637 RVA: 0x0000A29C File Offset: 0x0000849C
	// (set) Token: 0x06000A4E RID: 2638 RVA: 0x0000A2A4 File Offset: 0x000084A4
	public List<object> AllHouseStage { get; set; }

	// Token: 0x06000A4F RID: 2639 RVA: 0x0004A528 File Offset: 0x00048728
	private void LoadRaveUp()
	{
		List<object> list = (List<object>)(Json.Deserialize(this.ReadSystemJSONFile("raveupLibrary")) as Dictionary<string, object>)["albums"];
		this.AllRaveUpAlbum.Clear();
		this.AllRaveUpStage.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
			AlbumInfo albumInfo = new AlbumInfo();
			albumInfo.Id = Convert.ToInt32(dictionary["id"]);
			albumInfo.Name = Convert.ToString(dictionary["setName"]);
			albumInfo.FullName = Convert.ToString(dictionary["title"]);
			albumInfo.SubText = Convert.ToString(dictionary["subtitle"]);
			int enumIndex = GameData.GetEnumIndex<DISCSET_DIFFICULT>(Convert.ToString(dictionary["difficulty"]));
			if (enumIndex != -1)
			{
				int num = (int)Enum.GetValues(typeof(DISCSET_DIFFICULT)).GetValue(enumIndex);
				albumInfo.eDifficult = (DISCSET_DIFFICULT)num;
			}
			albumInfo.AlbumServerId = Convert.ToString(dictionary["serverId"]);
			this.AllRaveUpAlbum.Add(albumInfo);
			List<object> list2 = (List<object>)dictionary["songs"];
			for (int j = 0; j < list2.Count; j++)
			{
				Dictionary<string, object> dictionary2 = list2[j] as Dictionary<string, object>;
				RaveUpStage raveUpStage = new RaveUpStage();
				raveUpStage.Id = Convert.ToInt32(dictionary2["id"]);
				raveUpStage.iAlbum = Convert.ToInt32(dictionary["id"]);
				raveUpStage.iSong = Convert.ToInt32(dictionary2["songId"]);
				int enumIndex2 = GameData.GetEnumIndex<PTLEVEL>(Convert.ToString(dictionary2["chart"]));
				if (enumIndex2 != -1)
				{
					int num2 = (int)Enum.GetValues(typeof(PTLEVEL)).GetValue(enumIndex2);
					raveUpStage.PtType = (PTLEVEL)num2;
				}
				this.AllRaveUpStage.Add(raveUpStage);
			}
		}
	}

	// Token: 0x0400099C RID: 2460
	private const int HIDDENSTAGE1 = 7;

	// Token: 0x0400099D RID: 2461
	private const int HIDDENSTAGE2 = 8;

	// Token: 0x0400099E RID: 2462
	private const int SET_HIDDENSTAGE = 10;

	// Token: 0x0400099F RID: 2463
	public Dictionary<int, int> DicIconInfo = new Dictionary<int, int>();

	// Token: 0x040009A0 RID: 2464
	private int m_iSelectAlbumId;

	// Token: 0x040009A1 RID: 2465
	public GAMEMODE Mode;

	// Token: 0x040009A2 RID: 2466
	public HouseStage[] HouseSelectSong = new HouseStage[]
	{
		new HouseStage(),
		new HouseStage(),
		new HouseStage()
	};

	// Token: 0x040009A3 RID: 2467
	public RaveUpStage[] RaveUpSelectSong = new RaveUpStage[]
	{
		new RaveUpStage(),
		new RaveUpStage(),
		new RaveUpStage(),
		new RaveUpStage()
	};

	// Token: 0x040009A4 RID: 2468
	public MissionData Mission = new MissionData();

	// Token: 0x040009AE RID: 2478
	public List<SongManager.DemoSong> demoSongs = new List<SongManager.DemoSong>
	{
		new SongManager.DemoSong
		{
			id = 0,
			pt = 3,
			songId = 56
		},
		new SongManager.DemoSong
		{
			id = 1,
			pt = 3,
			songId = 51
		},
		new SongManager.DemoSong
		{
			id = 2,
			pt = 3,
			songId = 99
		}
	};

	// Token: 0x0200013D RID: 317
	public class DemoSong
	{
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x0000A2AD File Offset: 0x000084AD
		// (set) Token: 0x06000A51 RID: 2641 RVA: 0x0000A2B5 File Offset: 0x000084B5
		public int id { get; set; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000A52 RID: 2642 RVA: 0x0000A2BE File Offset: 0x000084BE
		// (set) Token: 0x06000A53 RID: 2643 RVA: 0x0000A2C6 File Offset: 0x000084C6
		public int songId { get; set; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x0000A2CF File Offset: 0x000084CF
		// (set) Token: 0x06000A55 RID: 2645 RVA: 0x0000A2D7 File Offset: 0x000084D7
		public int pt { get; set; }
	}
}
