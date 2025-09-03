using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000203 RID: 515
public class RaveUpManager : MonoBehaviour
{
	// Token: 0x06000EEA RID: 3818 RVA: 0x0006B43C File Offset: 0x0006963C
	private void Awake()
	{
		this.Panel_SelectAlbum = base.transform.FindChild("AlbumSelectMode").gameObject;
		this.Panel_DiscSelectMode = base.transform.FindChild("DiscSelectMode").gameObject;
		this.m_RaveUpSelectAlbum = this.Panel_SelectAlbum.transform.FindChild("Panel_SelectAlbum").GetComponent<RaveUpSelectAlbum>();
		this.m_RaveUpDiscSelectMode = this.Panel_DiscSelectMode.GetComponent<RaveUpDiscSelectMode>();
		this.m_AlbumScroll = this.m_RaveUpSelectAlbum.transform.FindChild("Panel_AlbumList").FindChild("Album").FindChild("ScrollView")
			.GetComponent<UIScroll>();
		this.AlbumGrid = this.m_AlbumScroll.transform.FindChild("Grid");
		this.m_RaveUpSelect = new RaveUpSelect[3];
		this.m_AlbumSelectMode = new UIPanel[3];
		for (int i = 0; i < this.m_RaveUpSelect.Length; i++)
		{
			this.m_RaveUpSelect[i] = base.transform.FindChild("DiscSelectMode").FindChild("StageSelect").FindChild("Select_" + (i + 1).ToString())
				.GetComponent<RaveUpSelect>();
		}
		this.m_AlbumSelectMode[0] = this.Panel_SelectAlbum.GetComponent<UIPanel>();
		this.m_AlbumSelectMode[1] = this.Panel_SelectAlbum.transform.FindChild("Panel_Rank").GetComponent<UIPanel>();
		this.m_AlbumSelectMode[2] = this.Panel_SelectAlbum.transform.FindChild("Panel_SelectAlbum").FindChild("Panel_AlbumList").FindChild("Album")
			.GetComponent<UIPanel>();
		this.AlbumSelectModeAni = base.transform.FindChild("AlbumSelectModeAni").GetComponent<TweenPosition>();
		this.m_DiscSelectMode = base.transform.FindChild("DiscSelectMode").GetComponent<UIPanel>();
		this.DiscSelectModeAni = base.transform.FindChild("DiscSelectModeAni").GetComponent<TweenPosition>();
		this.SetLevel = base.transform.FindChild("DiscSelectMode").FindChild("ui").FindChild("Label_SetLevel")
			.GetComponent<UILabel>();
		Transform transform = base.transform.FindChild("DiscSelectMode").FindChild("StageSelect").FindChild("Select_4");
		this.Stage4_on = transform.FindChild("On").gameObject;
		this.Stage4_off = transform.FindChild("Off").gameObject;
		this.RaveUpCell = Resources.Load("prefab/Raveup/RaveupAlbumCell") as GameObject;
		this.m_HouseMixRankManager = this.m_AlbumSelectMode[1].GetComponent<HouseMixRankManager>();
		this.m_HouseMixMyRecord = this.Panel_DiscSelectMode.transform.FindChild("MyRecord").GetComponent<HouseMixMyRecord>();
		this.GateTexture = base.transform.FindChild("Panel_GateTexture").FindChild("gateTexture").GetComponent<UITexture>();
		this.m_sTimer = base.transform.FindChild("Timer").GetComponent<TimerScript>();
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x0006B730 File Offset: 0x00069930
	private void Start()
	{
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_COMMON_TIMER);
		this.StartTimer();
		this.DiscImages = new Texture[6];
		this.m_sDiscImageName = new string[6];
		if (this.RaveUpCell == null)
		{
			this.RaveUpCell = Resources.Load("prefab/Raveup/RaveupAlbumCell") as GameObject;
		}
		this.CreatAlbumCell();
		base.Invoke("FirstSelect", 0.5f);
		base.Invoke("DestroyGate", 1.75f);
	}

	// Token: 0x06000EEC RID: 3820 RVA: 0x0006B7B0 File Offset: 0x000699B0
	private void GetRaveUpRanking()
	{
		AlbumInfo currentAlbum = Singleton<SongManager>.instance.GetCurrentAlbum();
		WWWGetRaveUpBestRanking wwwgetRaveUpBestRanking = new WWWGetRaveUpBestRanking();
		wwwgetRaveUpBestRanking.strAlbumId = currentAlbum.AlbumServerId;
		wwwgetRaveUpBestRanking.CallBack = new WWWObject.CompleteCallBack(this.RaveUpRankingCompleteLoad);
		wwwgetRaveUpBestRanking.CallBackFail = new WWWObject.CompleteCallBack(this.RaveUpRankingFailLoad);
		Singleton<WWWManager>.instance.AddQueue(wwwgetRaveUpBestRanking);
	}

	// Token: 0x06000EED RID: 3821 RVA: 0x0000CF6E File Offset: 0x0000B16E
	private void RaveUpRankingFailLoad()
	{
		this.m_HouseMixRankManager.CreateLocalRankCell();
	}

	// Token: 0x06000EEE RID: 3822 RVA: 0x0000CF6E File Offset: 0x0000B16E
	private void RaveUpRankingCompleteLoad()
	{
		this.m_HouseMixRankManager.CreateLocalRankCell();
	}

	// Token: 0x06000EEF RID: 3823 RVA: 0x0006B80C File Offset: 0x00069A0C
	private void GetRaveUpUserInfo()
	{
		AlbumInfo currentAlbum = Singleton<SongManager>.instance.GetCurrentAlbum();
		WWWGetRaveUpUserRecord wwwgetRaveUpUserRecord = new WWWGetRaveUpUserRecord();
		wwwgetRaveUpUserRecord.strAlbumId = currentAlbum.AlbumServerId;
		wwwgetRaveUpUserRecord.CallBack = new WWWObject.CompleteCallBack(this.RaveUpUserInfoCompleteLoad);
		wwwgetRaveUpUserRecord.CallBackFail = new WWWObject.CompleteCallBack(this.RaveUpUserInfoFailLoad);
		Singleton<WWWManager>.instance.AddQueue(wwwgetRaveUpUserRecord);
	}

	// Token: 0x06000EF0 RID: 3824 RVA: 0x00003648 File Offset: 0x00001848
	private void RaveUpUserInfoFailLoad()
	{
	}

	// Token: 0x06000EF1 RID: 3825 RVA: 0x0006B868 File Offset: 0x00069A68
	private void RaveUpUserInfoCompleteLoad()
	{
		UserRecordData netUserRecordData = Singleton<GameManager>.instance.netUserRecordData;
		this.m_HouseMixMyRecord.ValueSetting(netUserRecordData.Score, netUserRecordData.MaxCombo, netUserRecordData.Accuracy, (int)netUserRecordData.RankClass, netUserRecordData.PerfectPlay, netUserRecordData.AllCombo, string.Empty);
	}

	// Token: 0x06000EF2 RID: 3826 RVA: 0x0000CF7B File Offset: 0x0000B17B
	private void DestroyGate()
	{
		UnityEngine.Object.DestroyObject(this.GateTexture.transform.parent.gameObject);
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x0000CF97 File Offset: 0x0000B197
	private void StartTimer()
	{
		this.m_sTimer.StartTimer(80, 10);
		this.m_sTimer.CallBackTimeover = new TimerScript.CompleteTimeOver(this.TimeOverStart);
	}

	// Token: 0x06000EF4 RID: 3828 RVA: 0x0006B8B4 File Offset: 0x00069AB4
	private void CreatAlbumCell()
	{
		for (int i = 0; i < this.m_arrAlbmList.Count; i++)
		{
			UnityEngine.Object.Destroy((GameObject)this.m_arrAlbmList[i]);
		}
		this.m_arrAlbmList.Clear();
		int albumTotalCnt = Singleton<SongManager>.instance.GetAlbumTotalCnt();
		int num = 0;
		if (albumTotalCnt >= 10)
		{
			this.AlbumGrid.transform.localPosition = Vector3.zero;
			this.m_bRollingState = true;
			this.DownAlbumCount = (albumTotalCnt - 1) / 2;
			this.UpAlbumCount = albumTotalCnt - 1 - this.DownAlbumCount;
			num = this.DownAlbumCount;
			this.m_AlbumScroll.m_ScrollOption = UIScroll.ScrollOption_e.Rolling;
		}
		else
		{
			this.AlbumGrid.transform.localPosition = new Vector3(0f, 400f, 0f);
			this.m_AlbumScroll.m_ScrollOption = UIScroll.ScrollOption_e.Normal;
		}
		for (int j = 0; j < albumTotalCnt; j++)
		{
			AlbumInfo idxAlbumInfo = Singleton<SongManager>.instance.GetIdxAlbumInfo(j);
			GameObject gameObject = UnityEngine.Object.Instantiate(this.RaveUpCell, Vector3.zero, Quaternion.identity) as GameObject;
			gameObject.transform.parent = this.AlbumGrid.transform;
			gameObject.transform.localScale = Vector3.one;
			if (this.m_bRollingState)
			{
				if (j <= this.UpAlbumCount)
				{
					gameObject.transform.localPosition = new Vector3(0f, (float)j * this.AlbumSize - (float)j * this.AlbumIntervalSize, 0f);
				}
				else
				{
					gameObject.transform.localPosition = new Vector3(0f, (float)num * -this.AlbumSize + (float)num * this.AlbumIntervalSize, 0f);
					num--;
				}
			}
			else
			{
				gameObject.transform.localPosition = new Vector3(0f, (float)j * -this.AlbumSize, 0f);
			}
			this.m_arrAlbmList.Add(gameObject);
			gameObject.name = "Albm" + (j + 1).ToString();
			gameObject.SendMessage("setAlbumInfo", idxAlbumInfo);
			gameObject.SendMessage("setUIScroll", this.m_AlbumScroll);
			gameObject.SendMessage("setRaveUpManager", base.GetComponent<RaveUpManager>());
			gameObject.SendMessage("setAlbumImage");
		}
		this.m_AlbumScroll.CellsSetting(0);
	}

	// Token: 0x06000EF5 RID: 3829 RVA: 0x0000CFBF File Offset: 0x0000B1BF
	private void FirstSelect()
	{
		Singleton<SoundSourceManager>.instance.PlayNamedBgm("bgm_raveup", true);
		((GameObject)this.m_arrAlbmList[0]).GetComponent<RaveUpAlbumCell>().ClickProcess();
	}

	// Token: 0x06000EF6 RID: 3830 RVA: 0x0006BAFC File Offset: 0x00069CFC
	public void SelectAlbum(AlbumInfo ai)
	{
		Singleton<SongManager>.instance.SelectAlbumId = ai.Id;
		this.isSelectAlbumIndex = ai.Id - 1;
		this.m_RaveUpSelectAlbum.SendMessage("setAlbumInfo", ai);
		this.m_RaveUpSelectAlbum.SendMessage("setSelectAlbumTransform", ((GameObject)this.m_arrAlbmList[this.isSelectAlbumIndex]).transform);
		this.GetRaveUpRanking();
		for (int i = 0; i < this.m_arrAlbmList.Count; i++)
		{
			if (i == this.isSelectAlbumIndex)
			{
				((GameObject)this.m_arrAlbmList[i]).GetComponent<RaveUpAlbumCell>().m_bisSelect = true;
				((GameObject)this.m_arrAlbmList[i]).GetComponent<RaveUpAlbumCell>().m_gSelectAlbumBG.SetActive(true);
				((GameObject)this.m_arrAlbmList[i]).GetComponent<RaveUpAlbumCell>().m_txAlbumTexture.alpha = 0.7f;
			}
			else
			{
				((GameObject)this.m_arrAlbmList[i]).GetComponent<RaveUpAlbumCell>().m_bisSelect = false;
				((GameObject)this.m_arrAlbmList[i]).GetComponent<RaveUpAlbumCell>().m_gSelectAlbumBG.SetActive(false);
				((GameObject)this.m_arrAlbmList[i]).GetComponent<RaveUpAlbumCell>().m_txAlbumTexture.alpha = 1f;
			}
		}
		this.m_RaveUpSelectAlbum.setSelectAlbum();
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x0006BC60 File Offset: 0x00069E60
	private void NextBtnClick()
	{
		Singleton<SoundSourceManager>.instance.StopBgm();
		Singleton<SoundSourceManager>.instance.PlayNamedBgm("bgm_discsel", true);
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RAVEUP_ALBUM_NEXT, false);
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_NARRATION_ALBUM, false);
		this.GetRaveUpUserInfo();
		for (int i = 0; i < this.DiscImages.Length; i++)
		{
			this.DiscImages[i] = this.m_RaveUpSelectAlbum.m_txInDiscs[i].mainTexture;
			this.m_sDiscImageName[i] = this.m_RaveUpSelectAlbum.m_sInDiscName[i];
		}
		this.AlbumImage = this.m_RaveUpSelectAlbum.m_txSelectAlbum.mainTexture;
		this.AlbumSelectModeAni.Play(true);
		this.DiscSelectModeAni.Play(true);
		this.m_DiscSelectMode.transform.localPosition = Vector3.zero;
		this.SelectChange = true;
		this.m_AlbumSelectMode[1].gameObject.SetActive(false);
		this.m_AlbumSelectMode[2].gameObject.SetActive(false);
		ArrayList arrDiscInfo = this.m_RaveUpSelectAlbum.m_arrDiscInfo;
		this.m_RaveUpDiscSelectMode.m_lAlbumName.text = this.m_RaveUpSelectAlbum.m_lAlbumName.text;
		this.m_RaveUpDiscSelectMode.m_lAlbumKind.text = this.m_RaveUpSelectAlbum.m_lAlbumKind.text;
		for (int j = 0; j < this.DiscImages.Length; j++)
		{
			this.m_RaveUpDiscSelectMode.SettingDisc(j, (DiscInfo)arrDiscInfo[j], this.m_sDiscImageName[j]);
		}
		this.m_RaveUpDiscSelectMode.m_txAlbumImage.mainTexture = this.AlbumImage;
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x0006BDF0 File Offset: 0x00069FF0
	private void TweenAni()
	{
		if (!this.SelectChange)
		{
			return;
		}
		for (int i = 0; i < this.m_AlbumSelectMode.Length; i++)
		{
			this.m_AlbumSelectMode[i].alpha = this.AlbumSelectModeAni.transform.localPosition.x;
		}
		this.m_DiscSelectMode.alpha = this.DiscSelectModeAni.transform.localPosition.x;
		if (this.m_DiscSelectMode.alpha == 1f)
		{
			this.m_AlbumSelectMode[0].gameObject.SetActive(false);
			this.SelectChange = false;
		}
	}

	// Token: 0x06000EF9 RID: 3833 RVA: 0x0006BE88 File Offset: 0x0006A088
	private void SetLevelCheck()
	{
		int num = 0;
		for (int i = 0; i < this.m_RaveUpSelect.Length; i++)
		{
			if (!this.m_RaveUpSelect[i].m_bisHaveCD)
			{
				this.Stage4_on.SetActive(false);
				this.Stage4_off.SetActive(false);
				base.CancelInvoke("OnStage4");
				this.Stage4_off.SetActive(true);
				this.SetLevel.text = "?";
				return;
			}
			num += this.m_RaveUpSelect[i].m_iSelectLevel;
		}
		if ((num / 3).ToString() == this.SetLevel.text)
		{
			return;
		}
		base.Invoke("OnStage4", 0.25f);
		this.SetLevel.text = (num / 3).ToString();
	}

	// Token: 0x06000EFA RID: 3834 RVA: 0x0000CFEC File Offset: 0x0000B1EC
	private void OnStage4()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RAVEUP_DISC_HIDDEN_MOUNT, false);
		this.Stage4_on.SetActive(false);
		this.Stage4_off.SetActive(false);
		this.Stage4_on.SetActive(true);
	}

	// Token: 0x06000EFB RID: 3835 RVA: 0x0000D01F File Offset: 0x0000B21F
	private void Update()
	{
		this.SetLevelCheck();
		this.TweenAni();
	}

	// Token: 0x06000EFC RID: 3836 RVA: 0x0006BF50 File Offset: 0x0006A150
	private void TimeOverStart()
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 1; i < 7; i++)
		{
			bool flag = false;
			for (int j = 0; j < this.m_RaveUpSelect.Length; j++)
			{
				if (this.m_RaveUpSelect[j].m_iSelectIndex == i)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				arrayList.Add(i);
			}
		}
		for (int k = 0; k < this.m_RaveUpSelect.Length; k++)
		{
			if (this.m_RaveUpSelect[k].m_iSelectIndex == -1)
			{
				int num = new System.Random().Next(0, arrayList.Count);
				this.m_RaveUpSelect[k].m_iSelectIndex = (int)arrayList[num];
				arrayList.RemoveAt(num);
			}
		}
		arrayList.Clear();
		this.PlayRaveup();
	}

	// Token: 0x06000EFD RID: 3837 RVA: 0x0006C010 File Offset: 0x0006A210
	private void StartSetEffCount()
	{
		int num = Singleton<GameManager>.instance.UserData.PlayNormalItem[GameData.PLAYITEM];
		int num2 = Singleton<GameManager>.instance.UserData.PlayRefillItem[GameData.REFILLITEM];
		int num3 = Singleton<GameManager>.instance.UserData.PlayShieldItem[GameData.SHIELDITEM];
		if (num > 0 && GameData.PLAYITEM != PLAYFNORMALITEM.NONE)
		{
			Dictionary<PLAYFNORMALITEM, int> playNormalItem = Singleton<GameManager>.instance.UserData.PlayNormalItem;
			PLAYFNORMALITEM playitem;
			int num4 = playNormalItem[playitem = GameData.PLAYITEM];
			playNormalItem[playitem] = num4 - 1;
		}
		if (num2 > 0 && GameData.REFILLITEM != PLAYFREFILLITEM.NONE)
		{
			Dictionary<PLAYFREFILLITEM, int> playRefillItem = Singleton<GameManager>.instance.UserData.PlayRefillItem;
			PLAYFREFILLITEM refillitem;
			int num5 = playRefillItem[refillitem = GameData.REFILLITEM];
			playRefillItem[refillitem] = num5 - 1;
		}
		if (num3 > 0 && GameData.SHIELDITEM != PLAYFSHIELDITEM.NONE)
		{
			Dictionary<PLAYFSHIELDITEM, int> playShieldItem = Singleton<GameManager>.instance.UserData.PlayShieldItem;
			PLAYFSHIELDITEM shielditem;
			int num6 = playShieldItem[shielditem = GameData.SHIELDITEM];
			playShieldItem[shielditem] = num6 - 1;
		}
	}

	// Token: 0x06000EFE RID: 3838 RVA: 0x0006C108 File Offset: 0x0006A308
	private void PlayRaveup()
	{
		this.StartSetEffCount();
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_COMMON_TIMER);
		Singleton<SoundSourceManager>.instance.getNowBGM().volume = 1f;
		int[] array = new int[4];
		int num = 0;
		Singleton<SongManager>.instance.GetRaveUpAlbumStage(Singleton<SongManager>.instance.SelectAlbumId);
		if (this.m_RaveUpSelect.Length == 0)
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RAVEUP_DISC_START_NOREADY, false);
			return;
		}
		for (int i = 0; i < this.m_RaveUpSelect.Length; i++)
		{
			if (this.m_RaveUpSelect[i].m_iSelectIndex == -1)
			{
				Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RAVEUP_DISC_START_NOREADY, false);
				return;
			}
			array[num] = this.m_RaveUpSelect[i].m_iSelectIndex;
			num++;
		}
		RaveUpStage raveUpAlbumHiddenStage = Singleton<SongManager>.instance.GetRaveUpAlbumHiddenStage(Singleton<SongManager>.instance.SelectAlbumId);
		array[num] = raveUpAlbumHiddenStage.Id;
		int num2 = 0;
		ArrayList allRaveUpStage = Singleton<SongManager>.instance.AllRaveUpStage;
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j] == -1)
			{
				return;
			}
			foreach (object obj in allRaveUpStage)
			{
				RaveUpStage raveUpStage = (RaveUpStage)obj;
				if (raveUpStage.Id == array[j] && raveUpStage.iAlbum == Singleton<SongManager>.instance.SelectAlbumId)
				{
					if (num2 >= array.Length)
					{
						break;
					}
					Singleton<SongManager>.instance.RaveUpSelectSong[num2] = raveUpStage;
					num2++;
				}
			}
		}
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_TITLE_START_ALRIGHT, false);
		Singleton<SongManager>.instance.Mode = GAMEMODE.RAVEUP;
		Singleton<SoundSourceManager>.instance.StopBgm();
		Singleton<SceneSwitcher>.instance.LoadNextScene("game");
		GameData.AUTO_PLAY = false;
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x0000AD4A File Offset: 0x00008F4A
	private void OnDestroy()
	{
		Singleton<SoundSourceManager>.instance.StopBgm();
	}

	// Token: 0x0400105A RID: 4186
	private ArrayList m_arrAlbmList = new ArrayList();

	// Token: 0x0400105B RID: 4187
	[HideInInspector]
	public RaveUpSelect[] m_RaveUpSelect;

	// Token: 0x0400105C RID: 4188
	private UIScroll m_AlbumScroll;

	// Token: 0x0400105D RID: 4189
	private Transform AlbumGrid;

	// Token: 0x0400105E RID: 4190
	private HouseMixRankManager m_HouseMixRankManager;

	// Token: 0x0400105F RID: 4191
	private HouseMixMyRecord m_HouseMixMyRecord;

	// Token: 0x04001060 RID: 4192
	private RaveUpSelectAlbum m_RaveUpSelectAlbum;

	// Token: 0x04001061 RID: 4193
	private RaveUpDiscSelectMode m_RaveUpDiscSelectMode;

	// Token: 0x04001062 RID: 4194
	private GameObject Stage4_on;

	// Token: 0x04001063 RID: 4195
	private GameObject Stage4_off;

	// Token: 0x04001064 RID: 4196
	private GameObject Panel_SelectAlbum;

	// Token: 0x04001065 RID: 4197
	private GameObject Panel_DiscSelectMode;

	// Token: 0x04001066 RID: 4198
	private GameObject RaveUpCell;

	// Token: 0x04001067 RID: 4199
	private UIPanel m_DiscSelectMode;

	// Token: 0x04001068 RID: 4200
	private UIPanel[] m_AlbumSelectMode;

	// Token: 0x04001069 RID: 4201
	private TweenPosition AlbumSelectModeAni;

	// Token: 0x0400106A RID: 4202
	private TweenPosition DiscSelectModeAni;

	// Token: 0x0400106B RID: 4203
	private UILabel SetLevel;

	// Token: 0x0400106C RID: 4204
	private Texture[] DiscImages;

	// Token: 0x0400106D RID: 4205
	private string[] m_sDiscImageName;

	// Token: 0x0400106E RID: 4206
	private Texture AlbumImage;

	// Token: 0x0400106F RID: 4207
	private UITexture GateTexture;

	// Token: 0x04001070 RID: 4208
	private bool SelectChange;

	// Token: 0x04001071 RID: 4209
	private int AlbumCount = 10;

	// Token: 0x04001072 RID: 4210
	private int DownAlbumCount;

	// Token: 0x04001073 RID: 4211
	private int UpAlbumCount;

	// Token: 0x04001074 RID: 4212
	private int isSelectAlbumIndex;

	// Token: 0x04001075 RID: 4213
	private float AlbumSize = 330f;

	// Token: 0x04001076 RID: 4214
	private float AlbumIntervalSize = 20f;

	// Token: 0x04001077 RID: 4215
	private bool m_bRollingState;

	// Token: 0x04001078 RID: 4216
	private TimerScript m_sTimer;
}
