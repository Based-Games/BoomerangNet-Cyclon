using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D0 RID: 464
public class HouseMixSelectDiscInfo : MonoBehaviour
{
	// Token: 0x06000D93 RID: 3475 RVA: 0x0006130C File Offset: 0x0005F50C
	private void Awake()
	{
		this.m_HouseMixRankManager = base.transform.FindChild("Panel_Rank").GetComponent<HouseMixRankManager>();
		this.m_HouseMixMyRecord = base.transform.FindChild("Panel_MyRecord").GetComponent<HouseMixMyRecord>();
		this.m_tDiscStarGrid = base.transform.FindChild("DiscStar").FindChild("StarGrid").transform;
		this.m_HouseMixGraphManager = base.transform.FindChild("Panel_Graph").FindChild("Panel_Graph").GetComponent<HouseMixGraphManager>();
		this.m_HouseMixGraphPointManager = this.m_HouseMixGraphManager.transform.FindChild("PointAndLine").GetComponent<HouseMixGraphPointManager>();
		this.m_gDiscStarPrefab = Resources.Load("prefab/DiscDifficultStar") as GameObject;
		this.m_gDiscLevelPrefab = Resources.Load("prefab/HouseMixLevelBtn") as GameObject;
		Transform transform = base.transform.FindChild("Image");
		this.m_txSelectDisc = transform.FindChild("DiscImage").GetComponent<UITexture>();
		this.m_spSelectLevel = transform.FindChild("Sprite_selectLevel").GetComponent<UISprite>();
		this.m_sEventMark = transform.FindChild("EventMark").GetComponent<UISprite>();
		Transform transform2 = base.transform.FindChild("text");
		this.m_lDiscName = transform2.FindChild("Label_DiscName").GetComponent<UILabel>();
		this.m_lDiscName_Han = transform2.FindChild("Label_musicName_han").GetComponent<UILabel>();
		this.m_lDiscArtist = transform2.FindChild("Label_artist").GetComponent<UILabel>();
		this.m_lSongKind = transform2.FindChild("Label_SongKind").GetComponent<UILabel>();
	}

	// Token: 0x06000D94 RID: 3476 RVA: 0x000614A0 File Offset: 0x0005F6A0
	private void SetManger(HouseMixManager sManager)
	{
		this.m_sManager = sManager;
		GameObject gameObject = base.transform.FindChild("LevelBtn").gameObject;
		this.m_levelBtn.Clear();
		this.m_levelBtn.Add(PTLEVEL.EZ, gameObject.transform.FindChild("1_HouseMixLevelBtn_ez").GetComponent<HouseMixDiscLevelBtn>());
		this.m_levelBtn.Add(PTLEVEL.NM, gameObject.transform.FindChild("2_HouseMixLevelBtn_nm").GetComponent<HouseMixDiscLevelBtn>());
		this.m_levelBtn.Add(PTLEVEL.HD, gameObject.transform.FindChild("3_HouseMixLevelBtn_hd").GetComponent<HouseMixDiscLevelBtn>());
		this.m_levelBtn.Add(PTLEVEL.PR, gameObject.transform.FindChild("4_HouseMixLevelBtn_pr").GetComponent<HouseMixDiscLevelBtn>());
		this.m_levelBtn.Add(PTLEVEL.MX, gameObject.transform.FindChild("5_HouseMixLevelBtn_mx").GetComponent<HouseMixDiscLevelBtn>());
		this.m_levelBtn.Add(PTLEVEL.S1, gameObject.transform.FindChild("6_HouseMixLevelBtn_h1").GetComponent<HouseMixDiscLevelBtn>());
		this.m_levelBtn.Add(PTLEVEL.S2, gameObject.transform.FindChild("7_HouseMixLevelBtn_h2").GetComponent<HouseMixDiscLevelBtn>());
		for (PTLEVEL ptlevel = PTLEVEL.EZ; ptlevel < PTLEVEL.MAX; ptlevel++)
		{
			this.m_levelBtn[ptlevel].SendMessage("SetManager", sManager);
		}
	}

	// Token: 0x06000D95 RID: 3477 RVA: 0x000615E0 File Offset: 0x0005F7E0
	private void Start()
	{
		if (this.m_gDiscStarPrefab == null)
		{
			this.m_gDiscStarPrefab = Resources.Load("prefab/DiscDifficultStar") as GameObject;
		}
		if (this.m_gDiscLevelPrefab == null)
		{
			this.m_gDiscLevelPrefab = Resources.Load("prefab/HouseMixLevelBtn") as GameObject;
		}
	}

	// Token: 0x06000D96 RID: 3478 RVA: 0x0000BFF5 File Offset: 0x0000A1F5
	public void SetDiscInfo(DiscInfo di)
	{
		this.m_SelectDiscInfo = di;
	}

	// Token: 0x06000D97 RID: 3479 RVA: 0x0000BFFE File Offset: 0x0000A1FE
	public void SetStage(HouseStage hs)
	{
		this.m_SelectStage = hs;
	}

	// Token: 0x06000D98 RID: 3480 RVA: 0x00061634 File Offset: 0x0005F834
	public void DiscNameCheck()
	{
		string fullName = this.m_SelectDiscInfo.FullName;
		bool flag = GameData.isContainHangul(fullName);
		this.m_lDiscName.gameObject.SetActive(false);
		this.m_lDiscName_Han.gameObject.SetActive(false);
		if (!flag)
		{
			this.m_lDiscName.gameObject.SetActive(true);
			this.m_lDiscName.text = fullName;
		}
		else
		{
			this.m_lDiscName_Han.gameObject.SetActive(true);
			this.m_lDiscName_Han.text = fullName;
		}
		this.m_lSongKind.text = this.m_SelectDiscInfo.Genre;
		this.m_lDiscArtist.text = this.m_SelectDiscInfo.Artist;
	}

	// Token: 0x06000D99 RID: 3481 RVA: 0x000616E0 File Offset: 0x0005F8E0
	private void GetSongInfo(DiscInfo dInfo)
	{
		WWWHausMixRank wwwhausMixRank = new WWWHausMixRank();
		wwwhausMixRank.CallBack = new WWWObject.CompleteCallBack(this.CompleteLoad);
		wwwhausMixRank.dInfo = dInfo;
		wwwhausMixRank.MusicId = dInfo.ServerID;
		wwwhausMixRank.CallBackFail = new WWWObject.CompleteCallBack(this.FailLoad);
		Singleton<WWWManager>.instance.AddQueue(wwwhausMixRank);
	}

	// Token: 0x06000D9A RID: 3482 RVA: 0x0000C007 File Offset: 0x0000A207
	private void Ranking()
	{
		this.m_HouseMixRankManager.CreateLocalRankCell();
	}

	// Token: 0x06000D9B RID: 3483 RVA: 0x0000C014 File Offset: 0x0000A214
	private void OffNetWork()
	{
		this.m_HouseMixMyRecord.LocalBG.SetActive(true);
		this.Ranking();
		this.FailLoad();
	}

	// Token: 0x06000D9C RID: 3484 RVA: 0x00061738 File Offset: 0x0005F938
	private void FailLoad()
	{
		this.m_HouseMixMyRecord.LocalBG.SetActive(false);
		this.Ranking();
		if (Singleton<GameManager>.instance.netHouseMixData.DicPatternInfo.ContainsKey(this.m_isSelectLevel))
		{
			DiscInfo currentDisc = Singleton<SongManager>.instance.GetCurrentDisc();
			PatternScoreInfo patternScoreInfo = Singleton<GameManager>.instance.netHouseMixData.DicPatternInfo[this.m_isSelectLevel];
			this.m_HouseMixGraphPointManager.SetValue(patternScoreInfo, currentDisc, this.m_isSelectLevel);
		}
	}

	// Token: 0x06000D9D RID: 3485 RVA: 0x0000C033 File Offset: 0x0000A233
	private void CompleteLoad()
	{
		if (this.m_HouseMixMyRecord.LocalBG != null)
		{
			this.m_HouseMixMyRecord.LocalBG.SetActive(false);
		}
		this.Ranking();
		this.GraphSetting();
		this.SelectPatternInfo();
	}

	// Token: 0x06000D9E RID: 3486 RVA: 0x000617B4 File Offset: 0x0005F9B4
	private void SelectPatternInfo()
	{
		PTLEVEL isSelectLevel = this.m_isSelectLevel;
		this.m_HouseMixMyRecord.LocalBG.SetActive(false);
		DiscInfo currentDisc = Singleton<SongManager>.instance.GetCurrentDisc();
		PatternScoreInfo patternScoreInfo = Singleton<GameManager>.instance.netHouseMixData.DicPatternInfo[isSelectLevel];
		if (currentDisc.DicPtInfo.ContainsKey(isSelectLevel))
		{
			if (patternScoreInfo.BestScore > 0)
			{
				this.m_HouseMixGraphPointManager.gameObject.SetActive(true);
			}
			else
			{
				this.m_HouseMixGraphPointManager.gameObject.SetActive(false);
			}
			this.m_HouseMixGraphPointManager.SetValue(patternScoreInfo, currentDisc, isSelectLevel);
		}
		this.m_HouseMixMyRecord.ValueSetting(patternScoreInfo.BestScore, patternScoreInfo.MaxCombo, patternScoreInfo.Accuracy, (int)patternScoreInfo.RankClass, patternScoreInfo.PerfectPlay, patternScoreInfo.AllCombo, patternScoreInfo.strTrophy);
		this.m_HouseMixGraphManager.ViewGraph(isSelectLevel);
	}

	// Token: 0x06000D9F RID: 3487 RVA: 0x0000C06B File Offset: 0x0000A26B
	public void PlayDiscAudio(string DiscName)
	{
		Singleton<WWWManager>.instance.CallBackPreview = new WWWManager.CallBackLoadPreview(this.CompletePreview);
		Singleton<WWWManager>.instance.LoadPreview(DiscName);
	}

	// Token: 0x06000DA0 RID: 3488 RVA: 0x0000C08E File Offset: 0x0000A28E
	private void CompletePreview()
	{
		Singleton<SoundSourceManager>.instance.PlayBgm();
		Singleton<SoundSourceManager>.instance.getNowBGM().time = 0f;
		Singleton<SoundSourceManager>.instance.getNowBGM().loop = true;
	}

	// Token: 0x06000DA1 RID: 3489 RVA: 0x00061884 File Offset: 0x0005FA84
	public void SelectDiscCheck(int discID)
	{
		for (int i = 0; i < this.m_arrAllDisc.Count; i++)
		{
			HouseMixCell component = ((GameObject)this.m_arrAllDisc[i]).GetComponent<HouseMixCell>();
			component.m_gSelectBG.SetActive(false);
			component.m_txDiscPic.alpha = 1f;
			if (component.m_iDiscID == discID)
			{
				component.m_gSelectBG.SetActive(true);
				component.m_txDiscPic.alpha = 0.7f;
			}
		}
	}

	// Token: 0x06000DA2 RID: 3490 RVA: 0x00061900 File Offset: 0x0005FB00
	public void setSelectDisc(DiscInfo dInfo)
	{
		Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.SONG_500, this.m_SelectDiscInfo, this.m_txSelectDisc, null, null);
		this.DiscNameCheck();
		this.GetSongInfo(this.m_SelectDiscInfo);
		this.m_HouseMixGraphManager.AllHide();
		this.SelectDiscCheck(this.m_iDiscID);
		this.PlayDiscAudio(this.m_SelectDiscInfo.Name);
		this.PtBtnSetting(dInfo);
		this.m_aMovie = GameObject.Find("Movie").GetComponent<AVProWindowsMediaMovie>();
		this.m_aMovie._folder = "../Movie/InGame/";
		this.m_aMovie._filename = this.m_SelectDiscInfo.Name + ".mp4";
		this.m_aMovie._loop = true;
		this.m_aMovie._volume = 0f;
		this.m_aMovie.SetElapsedTime(10f);
		this.m_aMovie.LoadMovie(true);
	}

	// Token: 0x06000DA3 RID: 3491 RVA: 0x000619E8 File Offset: 0x0005FBE8
	private void PtBtnSetting(DiscInfo dInfo)
	{
		float num = 0f;
		float num2 = 210f;
		int num3 = 0;
		for (PTLEVEL ptlevel = PTLEVEL.EZ; ptlevel < PTLEVEL.MAX; ptlevel++)
		{
			if (this.m_SelectStage.DicSelectPt.ContainsKey(ptlevel) && this.m_SelectStage.DicSelectPt[ptlevel])
			{
				num3++;
			}
		}
		int num4 = num3 / 2;
		float num5;
		if (num4 % 2 == 0)
		{
			num5 = -num2 * (float)num4 + num2 / 2f;
		}
		else
		{
			num5 = -num2 * (float)num4;
		}
		for (PTLEVEL ptlevel2 = PTLEVEL.EZ; ptlevel2 < PTLEVEL.MAX; ptlevel2++)
		{
			if (this.m_SelectStage.DicSelectPt.ContainsKey(ptlevel2))
			{
				if (this.m_SelectStage.DicSelectPt[ptlevel2])
				{
					this.m_levelBtn[ptlevel2].gameObject.SetActive(true);
					this.m_levelBtn[ptlevel2].transform.localPosition = new Vector3(num5 + num * num2, 0f, this.m_levelBtn[ptlevel2].transform.localPosition.z);
					if (num == 0f)
					{
						this.setSelectLevel(ptlevel2);
					}
					num += 1f;
					DiscInfo.PtInfo ptInfo = dInfo.DicPtInfo[ptlevel2];
					this.m_levelBtn[ptlevel2].LevelImage.color = new Color(1f, 1f, 1f, 1f);
					this.m_levelBtn[ptlevel2].SpinAni.GetComponent<UISprite>().color = new Color(1f, 1f, 1f, 1f);
					this.m_levelBtn[ptlevel2].GetComponent<BoxCollider>().enabled = true;
					this.m_levelBtn[ptlevel2].SetEventCount(0);
				}
				else
				{
					this.m_levelBtn[ptlevel2].gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06000DA4 RID: 3492 RVA: 0x00061BD0 File Offset: 0x0005FDD0
	private void GraphSetting()
	{
		for (PTLEVEL ptlevel = PTLEVEL.EZ; ptlevel < PTLEVEL.MAX; ptlevel++)
		{
			if (this.m_SelectStage.DicSelectPt.ContainsKey(ptlevel) && this.m_SelectStage.DicSelectPt[ptlevel] && Singleton<GameManager>.instance.netHouseMixData.DicPatternInfo.ContainsKey(ptlevel))
			{
				Singleton<SongManager>.instance.GetCurrentDisc();
				PatternScoreInfo patternScoreInfo = Singleton<GameManager>.instance.netHouseMixData.DicPatternInfo[ptlevel];
				this.m_HouseMixGraphManager.ShowLevelGraph(ptlevel, patternScoreInfo.NoteScore, (int)patternScoreInfo.RankClass);
			}
		}
	}

	// Token: 0x06000DA5 RID: 3493 RVA: 0x00061C60 File Offset: 0x0005FE60
	public void setSelectLevel(PTLEVEL ePtType)
	{
		this.m_isSelectLevel = ePtType;
		for (PTLEVEL ptlevel = PTLEVEL.EZ; ptlevel < PTLEVEL.MAX; ptlevel++)
		{
			this.m_levelBtn[ptlevel].isSelect = false;
			this.m_levelBtn[ptlevel].SelectAni.SetActive(false);
			this.m_levelBtn[ptlevel].SpinAni.SetActive(false);
		}
		this.m_levelBtn[ePtType].isSelect = true;
		this.m_levelBtn[ePtType].SelectAni.SetActive(true);
		this.m_levelBtn[ePtType].SpinAni.SetActive(true);
		HouseStage houseStage = Singleton<SongManager>.instance.HouseSelectSong[GameData.Stage];
		houseStage.PtType = this.m_isSelectLevel;
		this.m_sEventMark.gameObject.SetActive(false);
		if (Singleton<SongManager>.instance.IsContainEventPt(houseStage.iSong))
		{
			this.m_sEventMark.gameObject.SetActive(true);
		}
		this.m_spSelectLevel.spriteName = "level_" + ePtType.ToString().ToLower() + "_sm";
		this.StarSetting(ePtType);
		PatternScoreInfo patternScoreInfo = Singleton<GameManager>.instance.netHouseMixData.DicPatternInfo[this.m_isSelectLevel];
		this.SelectPatternInfo();
	}

	// Token: 0x06000DA6 RID: 3494 RVA: 0x00003648 File Offset: 0x00001848
	private void test()
	{
	}

	// Token: 0x06000DA7 RID: 3495 RVA: 0x00061DA8 File Offset: 0x0005FFA8
	public void StarSetting(PTLEVEL ptType)
	{
		this.m_iLevelDifficult = this.m_SelectDiscInfo.DicPtInfo[ptType].iDif;
		for (int i = 0; i < this.m_tDiscStarGrid.childCount; i++)
		{
			UnityEngine.Object.DestroyObject(this.m_tDiscStarGrid.GetChild(i).gameObject);
		}
		if (this.m_gDiscStarPrefab != null)
		{
			int num = 0;
			for (int j = 0; j < 12; j++)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.m_gDiscStarPrefab);
				StarSetting component = gameObject.GetComponent<StarSetting>();
				gameObject.transform.parent = this.m_tDiscStarGrid;
				gameObject.transform.localScale = new Vector3(2f, 2f, 1f);
				string text = string.Empty;
				if (j >= 5 && j < 10)
				{
					text = "_1";
				}
				else if (j >= 10 && j < 13)
				{
					text = "_2";
				}
				gameObject.transform.localPosition = new Vector3(40f * (float)j, 0f, 0f);
				if (this.m_iLevelDifficult <= j)
				{
					component.setStar(false, string.Empty);
				}
				else
				{
					component.setStar(true, text);
				}
				num++;
			}
		}
	}

	// Token: 0x04000DE8 RID: 3560
	private HouseMixManager m_sManager;

	// Token: 0x04000DE9 RID: 3561
	private HouseMixRankManager m_HouseMixRankManager;

	// Token: 0x04000DEA RID: 3562
	private HouseMixMyRecord m_HouseMixMyRecord;

	// Token: 0x04000DEB RID: 3563
	private Dictionary<PTLEVEL, HouseMixDiscLevelBtn> m_levelBtn = new Dictionary<PTLEVEL, HouseMixDiscLevelBtn>();

	// Token: 0x04000DEC RID: 3564
	public ArrayList m_arrAllDisc = new ArrayList();

	// Token: 0x04000DED RID: 3565
	private DiscInfo m_SelectDiscInfo;

	// Token: 0x04000DEE RID: 3566
	private HouseStage m_SelectStage;

	// Token: 0x04000DEF RID: 3567
	[HideInInspector]
	public UITexture m_txSelectDisc;

	// Token: 0x04000DF0 RID: 3568
	[HideInInspector]
	public UILabel m_lDiscName;

	// Token: 0x04000DF1 RID: 3569
	[HideInInspector]
	public UILabel m_lDiscName_Han;

	// Token: 0x04000DF2 RID: 3570
	[HideInInspector]
	public UILabel m_lDiscArtist;

	// Token: 0x04000DF3 RID: 3571
	[HideInInspector]
	public UILabel m_lSongKind;

	// Token: 0x04000DF4 RID: 3572
	[HideInInspector]
	public UISprite m_spSelectLevel;

	// Token: 0x04000DF5 RID: 3573
	[HideInInspector]
	public int m_iLevelDifficult;

	// Token: 0x04000DF6 RID: 3574
	[HideInInspector]
	public bool m_bisShow;

	// Token: 0x04000DF7 RID: 3575
	[HideInInspector]
	public int m_iDiscID;

	// Token: 0x04000DF8 RID: 3576
	[HideInInspector]
	public int m_iSongID;

	// Token: 0x04000DF9 RID: 3577
	[HideInInspector]
	public string m_stDiscMusicFileName;

	// Token: 0x04000DFA RID: 3578
	[HideInInspector]
	public string m_sDiscImageName;

	// Token: 0x04000DFB RID: 3579
	private GameObject m_gDiscStarPrefab;

	// Token: 0x04000DFC RID: 3580
	private GameObject m_gDiscLevelPrefab;

	// Token: 0x04000DFD RID: 3581
	private Transform m_tDiscStarGrid;

	// Token: 0x04000DFE RID: 3582
	private PTLEVEL m_isSelectLevel;

	// Token: 0x04000DFF RID: 3583
	private HouseMixGraphManager m_HouseMixGraphManager;

	// Token: 0x04000E00 RID: 3584
	private HouseMixGraphPointManager m_HouseMixGraphPointManager;

	// Token: 0x04000E01 RID: 3585
	private UISprite m_sEventMark;

	// Token: 0x04000E02 RID: 3586
	private AVProWindowsMediaMovie m_aMovie;

	// Token: 0x04000E03 RID: 3587
	private UITexture m_aTexture;
}
