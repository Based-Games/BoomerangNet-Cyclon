using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020001C5 RID: 453
public class HouseMixManager : MonoBehaviour
{
	// Token: 0x06000D40 RID: 3392 RVA: 0x0005D598 File Offset: 0x0005B798
	private void Awake()
	{
		this.m_gDiscGrid = base.transform.FindChild("Panel_MusicList").FindChild("Musics").FindChild("ScrollView")
			.FindChild("DiscGrid")
			.gameObject;
		this.m_tpSelectTag = base.transform.FindChild("Panel_MusicList").FindChild("Musics").FindChild("ScrollView")
			.FindChild("SelectBG")
			.GetComponent<TweenPosition>();
		this.m_HouseMixSelectDiscInfo = base.transform.FindChild("Panel_SelectDiscInfo").GetComponent<HouseMixSelectDiscInfo>();
		this.m_spStageNum = base.transform.FindChild("Panel_Title").FindChild("Sprite_StageNum").GetComponent<UISprite>();
		this.m_txGateTexture = base.transform.FindChild("Panel_GateTexture").FindChild("gateTexture").GetComponent<UITexture>();
		this.m_pDiscScrollObj = this.m_gDiscGrid.transform.parent.parent.GetComponent<UIPanel>();
		this.m_scDiscScroll = this.m_gDiscGrid.transform.parent.GetComponent<UIScroll>();
		this.m_gDiscPrefab = Resources.Load("prefab/HouseMix/HouseMixCell") as GameObject;
		this.m_gSortTabPrefab = Resources.Load("prefab/HouseMix/HouseMixSortTab") as GameObject;
		this.m_HouseMixSelectDiscInfo.SendMessage("SetManger", this);
		this.m_sTimer = base.transform.FindChild("Timer").GetComponent<TimerScript>();
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x0005D710 File Offset: 0x0005B910
	private void Start()
	{
		Singleton<SongManager>.instance.HouseSelectSong[GameData.Stage].Id = -1;
		this.m_ClickBtnKind = HouseMixSortBtn.DiscSortKind_e.sort_lv;
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_COMMON_TIMER);
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_SCORE_COUNT);
		this.m_spStageNum.spriteName = "stage_" + (GameData.Stage + 1).ToString();
		this.m_spStageNum.MakePixelPerfect();
		this.m_spStageNum.transform.localScale = new Vector3(2f, 2f, 1f);
		this.CreateDisc();
		base.Invoke("FirstSort", 0.1f);
		this.m_HouseMixSelectDiscInfo.m_arrAllDisc = this.m_arrSongList;
		if (GameData.Stage == 0)
		{
			USERDATA userData = Singleton<GameManager>.instance.UserData;
			GameData.PLAYITEM = PLAYFNORMALITEM.NONE;
			GameData.FADEEFFCTOR = EFFECTOR_FADER.NONE;
			GameData.RANDEFFECTOR = EFFECTOR_RAND.NONE;
			GameData.SPEEDEFFECTOR = userData.UserSpeed;
			GameData.REFILLITEM = PLAYFREFILLITEM.NONE;
			GameData.SHIELDITEM = PLAYFSHIELDITEM.NONE;
		}
		Singleton<GameManager>.instance.ActivieLed(LEDSTATE.WING_OUTGAME);
		this.m_sTimer.StartTimer(70, 10);
		this.m_sTimer.CallBackTimeover = new TimerScript.CompleteTimeOver(this.PressPlayButton);
		base.Invoke("FirstSelect", 0.5f);
		Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("Picking a song...", "Playing HAUS MIX");
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x0000BD61 File Offset: 0x00009F61
	private void DestroyGate()
	{
		UnityEngine.Object.DestroyObject(this.m_txGateTexture.transform.parent.gameObject);
	}

	// Token: 0x06000D43 RID: 3395 RVA: 0x0005D85C File Offset: 0x0005BA5C
	private void CreateDisc()
	{
		for (int i = 0; i < this.m_arrSongList.Count; i++)
		{
			UnityEngine.Object.Destroy((GameObject)this.m_arrSongList[i]);
		}
		this.m_arrSongList.Clear();
		ArrayList arrayList = new ArrayList();
		arrayList = Singleton<SongManager>.instance.GetHouseStage(false);
		this.SelectDisc = Singleton<SongManager>.instance.GetDiscInfo(0);
		this.SelectStage = (HouseStage)arrayList[0];
		this.SelectPtType = this.SelectStage.GetFirstLv();
		for (int j = 0; j < arrayList.Count; j++)
		{
			HouseStage houseStage = (HouseStage)arrayList[j];
			DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(houseStage.iSong);
			DiscInfo.PtInfo firstPtInfo = discInfo.GetFirstPtInfo();
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.m_gDiscPrefab);
			gameObject.transform.parent = this.m_gDiscGrid.transform;
			gameObject.transform.localScale = Vector3.one;
			float num = this.m_pDiscScrollObj.GetViewSize().x * -0.5f;
			gameObject.transform.localPosition = new Vector3(num + num * 410f + num * 0.5f, -35f, 0f);
			this.m_arrSongList.Add(gameObject);
			this.m_arrSaveDisc.Add(gameObject);
			gameObject.name = "Disc" + (j + 1).ToString();
			this.m_arrDiscName.Add(discInfo.Name);
			HouseMixCell component = gameObject.GetComponent<HouseMixCell>();
			component.SetManager(this);
			component.SetDiscInfo(discInfo);
			component.SetHouseStageInfo(houseStage);
			component.m_sDiscMusicFileName = discInfo.Name;
			component.m_HouseMixManager = this;
			component.m_UIScroll = this.m_gDiscGrid.transform.parent.GetComponent<UIScroll>();
			component.m_iDiscID = houseStage.Id;
			component.m_iSongID = discInfo.Id;
			component.m_HouseMixSelectDiscInfo = this.m_HouseMixSelectDiscInfo;
			component.m_sDiscImageName = discInfo.Name;
			component.m_sDiscArtist = discInfo.Artist.ToUpper();
			component.m_sDiscName = discInfo.FullName.ToUpper();
			component.m_iLevelDifficult = firstPtInfo.iDif;
			component.m_SelectTag = this.m_tpSelectTag;
			component.m_iAverageLevelDifficult = discInfo.Difficult;
			component.m_sSongKind = discInfo.Genre.ToUpper();
			Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.DISC_145, discInfo, component.m_txDiscPic, null, null);
		}
		this.m_gDiscGrid.transform.parent.GetComponent<UIScroll>().CellsSetting(0);
	}

	// Token: 0x06000D44 RID: 3396 RVA: 0x0005DB10 File Offset: 0x0005BD10
	private void FirstSort()
	{
		for (int i = 0; i < this.m_SortBtn.Length; i++)
		{
			if (this.m_ClickBtnKind != (HouseMixSortBtn.DiscSortKind_e)i)
			{
				this.m_SortBtn[i].HideBtn();
			}
		}
		this.m_SortBtn[(int)this.m_ClickBtnKind].ShowBtn(false);
		this.m_BtnState = HouseMixSortBtn.SortState_e._up;
		this.SortDisc(HouseMixSortBtn.DiscSortKind_e.sort_lv);
	}

	// Token: 0x06000D45 RID: 3397 RVA: 0x0005DB68 File Offset: 0x0005BD68
	private void FirstSelect()
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
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_NARRATION_MUSIC, false);
		if (GameData.Stage == 0)
		{
			for (int i = 0; i < this.m_arrSongList.Count; i++)
			{
				HouseMixCell component = ((GameObject)this.m_arrSongList[i]).GetComponent<HouseMixCell>();
				if (Singleton<GameManager>.instance.UserData.LastSongID == component.m_sDiscInfo.Id)
				{
					this.m_scDiscScroll.transform.position = new Vector3(Singleton<GameManager>.instance.UserData.lastScrollPosition, this.m_scDiscScroll.transform.position.y, this.m_scDiscScroll.transform.position.z);
					this.m_scDiscScroll.SetSmoothPosMove();
					((GameObject)this.m_arrSongList[i]).GetComponent<HouseMixCell>().ClickEvent(false);
					this.m_HouseMixSelectDiscInfo.setSelectLevel(dictionary[Singleton<GameManager>.instance.UserData.lastPT]);
					this.SelectPtType = dictionary[Singleton<GameManager>.instance.UserData.lastPT];
					return;
				}
			}
		}
		for (int j = 0; j < this.m_arrSongList.Count; j++)
		{
			HouseMixCell component2 = ((GameObject)this.m_arrSongList[j]).GetComponent<HouseMixCell>();
			if (HouseMixManager.LastDiscID == component2.m_sDiscInfo.Id)
			{
				this.m_scDiscScroll.transform.position = new Vector3(HouseMixManager.LastScrollPosition, this.m_scDiscScroll.transform.position.y, this.m_scDiscScroll.transform.position.z);
				this.m_scDiscScroll.SetSmoothPosMove();
				((GameObject)this.m_arrSongList[j]).GetComponent<HouseMixCell>().ClickEvent(false);
				this.m_HouseMixSelectDiscInfo.setSelectLevel(HouseMixManager.LastPtType);
				this.SelectPtType = HouseMixManager.LastPtType;
				return;
			}
		}
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x0005DDA8 File Offset: 0x0005BFA8
	private void Update()
	{
		Vector3 zero = Vector3.zero;
		zero.x = (float)Screen.width - Input.mousePosition.x;
		zero.y = (float)Screen.height - Input.mousePosition.y;
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x0005DDEC File Offset: 0x0005BFEC
	private void NextScene()
	{
		Singleton<SoundSourceManager>.instance.SetRandKeyIndex();
		ArrayList houseStage = Singleton<SongManager>.instance.GetHouseStage(false);
		int num = UnityEngine.Random.Range(0, houseStage.Count);
		HouseStage houseStage2 = (HouseStage)houseStage[num];
		houseStage2.PtType = houseStage2.GetRandPt();
		Singleton<SongManager>.instance.HouseSelectSong[GameData.Stage] = houseStage2;
		base.enabled = false;
		Singleton<SceneSwitcher>.instance.LoadNextScene("game");
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x0005DE5C File Offset: 0x0005C05C
	public void PressDisc()
	{
		this.SelectStage.PtType = this.SelectPtType;
		Singleton<SongManager>.instance.HouseSelectSong[GameData.Stage] = this.SelectStage;
		this.m_HouseMixSelectDiscInfo.SetDiscInfo(this.SelectDisc);
		this.m_HouseMixSelectDiscInfo.SetStage(this.SelectStage);
		this.m_HouseMixSelectDiscInfo.setSelectDisc(this.SelectDisc);
		HouseMixManager.LastScrollPosition = this.m_scDiscScroll.transform.position.x;
	}

	// Token: 0x06000D49 RID: 3401 RVA: 0x0005DEE0 File Offset: 0x0005C0E0
	public void PressPlayButton()
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
		this.SelectStage.PtType = this.SelectPtType;
		Singleton<SongManager>.instance.HouseSelectSong[GameData.Stage] = this.SelectStage;
		bool flag = this.SelectStage.DicSelectPt[this.SelectPtType];
		this.m_sTimer.StopTimer();
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_COMMON_TIMER);
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_TITLE_START_ALRIGHT, false);
		Singleton<SoundSourceManager>.instance.StopBgm();
		Singleton<SoundSourceManager>.instance.getNowBGM().volume = 1f;
		Singleton<SceneSwitcher>.instance.LoadNextScene("game");
		GameData.AUTO_PLAY = false;
	}

	// Token: 0x06000D4A RID: 3402 RVA: 0x0005E06C File Offset: 0x0005C26C
	public void DiscRange(HouseMixSortBtn.DiscSortKind_e Kind, HouseMixSortBtn.SortState_e sortstate)
	{
		if (this.m_bSortState)
		{
			return;
		}
		this.m_bSortStart = false;
		this.m_iSort_index = 0;
		this.m_iSortStartCount = 0;
		this.m_bSortState = false;
		this.m_ClickBtnKind = Kind;
		this.m_BtnState = sortstate;
		for (int i = 0; i < this.m_SortBtn.Length; i++)
		{
			if (Kind != (HouseMixSortBtn.DiscSortKind_e)i)
			{
				this.m_SortBtn[i].HideBtn();
			}
		}
		for (int j = 0; j < this.m_gDiscGrid.transform.childCount; j++)
		{
			float num = 0.005f * (float)j;
			base.Invoke("DiscSortposition", num);
		}
	}

	// Token: 0x06000D4B RID: 3403 RVA: 0x0005E100 File Offset: 0x0005C300
	public void DiscSortposition()
	{
		HouseMixCell component = this.m_gDiscGrid.transform.GetChild(this.m_iSort_index).GetComponent<HouseMixCell>();
		if (this.m_gDiscGrid.transform.childCount > this.m_iSort_index && component != null)
		{
			component.StartDiscAlphaAni(true);
			this.m_iSort_index++;
		}
	}

	// Token: 0x06000D4C RID: 3404 RVA: 0x0000BD7D File Offset: 0x00009F7D
	public void EndSort()
	{
		this.EndSortDiscCount++;
		if (this.EndSortDiscCount >= this.m_gDiscGrid.transform.childCount)
		{
			this.EndSortDiscCount = 0;
			this.m_bSortState = false;
		}
	}

	// Token: 0x06000D4D RID: 3405 RVA: 0x0005E160 File Offset: 0x0005C360
	public void SortStart()
	{
		this.m_iSortStartCount++;
		if (!this.m_bSortStart && this.m_iSortStartCount < this.m_arrSaveDisc.Count)
		{
			return;
		}
		this.m_bSortStart = true;
		this.SortDisc(this.m_ClickBtnKind);
		for (int i = 0; i < this.m_gDiscGrid.transform.childCount; i++)
		{
			HouseMixCell component = this.m_gDiscGrid.transform.GetChild(i).GetComponent<HouseMixCell>();
			if (component != null)
			{
				component.ShowDiscAlphaAni();
			}
		}
	}

	// Token: 0x06000D4E RID: 3406 RVA: 0x0005E1EC File Offset: 0x0005C3EC
	private void SortDisc(HouseMixSortBtn.DiscSortKind_e BtnKind)
	{
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		ArrayList arrayList3 = new ArrayList();
		for (int i = 0; i < this.m_arrSortTabObj.Count; i++)
		{
			UnityEngine.Object.DestroyImmediate(((GameObject)this.m_arrSortTabObj[i]).gameObject);
		}
		this.m_arrSortTabObj.Clear();
		for (int j = 0; j < this.m_arrSongList.Count; j++)
		{
			HouseCellInfo houseCellInfo = new HouseCellInfo();
			HouseMixCell component = ((GameObject)this.m_arrSongList[j]).GetComponent<HouseMixCell>();
			string text = string.Empty;
			switch (BtnKind)
			{
			case HouseMixSortBtn.DiscSortKind_e.sort_lv:
				houseCellInfo.Number = component.m_iAverageLevelDifficult;
				text = ((int)component.m_sDiscName[0]).ToString() + ((int)component.m_sDiscArtist[0]).ToString();
				houseCellInfo.Number2 = int.Parse(text);
				houseCellInfo.Name = component.m_sDiscName;
				houseCellInfo.obj = (GameObject)this.m_arrSongList[j];
				break;
			case HouseMixSortBtn.DiscSortKind_e.sort_name:
				houseCellInfo.Number = (int)component.m_sDiscName[0];
				text = component.m_iAverageLevelDifficult.ToString() + ((int)component.m_sDiscArtist[0]).ToString();
				houseCellInfo.Number2 = int.Parse(text);
				houseCellInfo.Name = component.m_sDiscName;
				houseCellInfo.obj = (GameObject)this.m_arrSongList[j];
				break;
			case HouseMixSortBtn.DiscSortKind_e.sort_artist:
				houseCellInfo.Number = (int)component.m_sDiscArtist[0];
				text = component.m_iAverageLevelDifficult.ToString() + ((int)component.m_sDiscName[0]).ToString();
				houseCellInfo.Number2 = int.Parse(text);
				houseCellInfo.Name = component.m_sDiscName;
				houseCellInfo.obj = (GameObject)this.m_arrSongList[j];
				break;
			}
			arrayList.Add(houseCellInfo);
		}
		for (int k = 0; k < arrayList.Count; k++)
		{
			arrayList2.Add(((GameObject)this.m_arrSongList[k]).transform.localPosition);
			arrayList3.Add(((GameObject)this.m_arrSongList[k]).GetComponent<TweenPosition>().from);
		}
		if (this.m_BtnState == HouseMixSortBtn.SortState_e._up)
		{
			arrayList.Sort(new SortDisc_Number());
		}
		else if (this.m_BtnState == HouseMixSortBtn.SortState_e._down)
		{
			arrayList.Sort(new ReverseDisc_Number());
		}
		int num = 0;
		int num2 = -1;
		int num3 = 120;
		int num4 = 0;
		int num5 = -1;
		Vector3 localPosition = this.m_pDiscScrollObj.transform.localPosition;
		this.m_fCellSizeX = this.m_scDiscScroll.CellSize.x;
		float num6 = this.m_pDiscScrollObj.GetViewSize().x * -0.5f;
		ArrayList arrayList4 = new ArrayList();
		ArrayList arrayList5 = new ArrayList();
		for (int l = 0; l < arrayList.Count; l++)
		{
			if (((HouseCellInfo)arrayList[l]).Number != num2)
			{
				if (num != 0)
				{
					arrayList5.Clear();
					arrayList5 = this.OtherSort(arrayList4, HouseMixSortBtn.DiscSortKind_e.sort_name, num5, num6, num, num3, this.m_BtnState);
					int num7 = 0;
					for (int m = num5; m < num5 + arrayList5.Count; m++)
					{
						arrayList[m] = arrayList5[num7];
						num7++;
					}
					num5 = -1;
					arrayList4.Clear();
				}
				num2 = ((HouseCellInfo)arrayList[l]).Number;
				GameObject gameObject = UnityEngine.Object.Instantiate(this.m_gSortTabPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				HouseMixSortTab component2 = gameObject.GetComponent<HouseMixSortTab>();
				gameObject.transform.parent = this.m_gDiscGrid.transform;
				component2.TabName.gameObject.SetActive(false);
				component2.TabName_Han.gameObject.SetActive(false);
				if (BtnKind == HouseMixSortBtn.DiscSortKind_e.sort_lv)
				{
					if (num2 >= 10)
					{
						component2.TabName.fontSize = 88;
						component2.TabName.transform.localPosition = new Vector3(-3f, -40f, 0f);
						if (num2 == 11)
						{
							component2.TabName.spacingX = 10;
						}
					}
					component2.LevelText.spriteName = "tab_average";
				}
				else if (BtnKind == HouseMixSortBtn.DiscSortKind_e.sort_name)
				{
					component2.LevelText.spriteName = "tab_title";
				}
				else if (BtnKind == HouseMixSortBtn.DiscSortKind_e.sort_artist)
				{
					component2.LevelText.spriteName = "tab_artist";
				}
				component2.LevelText.MakePixelPerfect();
				component2.LevelText.transform.localScale = Vector3.one * 2f;
				byte[] bytes = BitConverter.GetBytes((uint)num2);
				if (num2 == 50500)
				{
					component2.TabName_Han.gameObject.SetActive(true);
					component2.TabName_Han.text = "아";
				}
				else if (num2 == 53440)
				{
					component2.TabName_Han.gameObject.SetActive(true);
					component2.TabName_Han.text = "타";
				}
				else
				{
					component2.TabName.gameObject.SetActive(true);
					if (BtnKind == HouseMixSortBtn.DiscSortKind_e.sort_lv)
					{
						component2.TabName.text = num2.ToString();
					}
					else
					{
						component2.TabName.text = Encoding.ASCII.GetString(bytes);
					}
				}
				component2.m_UIScroll = this.m_gDiscGrid.transform.parent.GetComponent<UIScroll>();
				gameObject.transform.localScale = Vector3.one;
				if (num == 0)
				{
					gameObject.transform.localPosition = new Vector3(num6 + (float)(l * num3) + (float)num3 * 0.5f, -19f, 0f);
				}
				else
				{
					gameObject.transform.localPosition = new Vector3(((HouseCellInfo)arrayList[l - 1]).obj.transform.localPosition.x + this.m_fCellSizeX * 0.5f + (float)num3 * 0.5f, -19f, 0f);
				}
				this.m_arrSortTabObj.Add(gameObject);
				num++;
			}
			num4++;
			if (num5 == -1)
			{
				num5 = l;
			}
			((HouseCellInfo)arrayList[l]).obj.transform.localPosition = new Vector3(num6 + (float)l * this.m_fCellSizeX + this.m_fCellSizeX * 0.5f + (float)(num * num3), -35f, 0f);
			arrayList4.Add((HouseCellInfo)arrayList[l]);
		}
		arrayList5.Clear();
		arrayList5 = this.OtherSort(arrayList4, HouseMixSortBtn.DiscSortKind_e.sort_name, num5, num6, num, num3, this.m_BtnState);
		int num8 = 0;
		for (int n = num5; n < num5 + arrayList5.Count; n++)
		{
			arrayList[n] = arrayList5[num8];
			num8++;
		}
		arrayList4.Clear();
		this.m_gDiscGrid.transform.parent.GetComponent<UIScroll>().CellsSetting(num);
		this.m_arrSongList.Clear();
		for (int num9 = 0; num9 < arrayList.Count; num9++)
		{
			this.m_arrSongList.Add(((HouseCellInfo)arrayList[num9]).obj);
		}
		arrayList3.Clear();
		arrayList.Clear();
		arrayList2.Clear();
		base.Invoke("SortBtnAllEable", (float)this.m_gDiscGrid.transform.childCount * 0.005f + 0.15f);
		if (this.m_gSelectObj != null)
		{
			this.m_tpSelectTag.transform.localPosition = this.m_gSelectObj.transform.localPosition;
		}
		this.m_gDiscGrid.transform.parent.GetComponent<UIScroll>().SetSmoothPosMove();
	}

	// Token: 0x06000D4F RID: 3407 RVA: 0x0005E9D4 File Offset: 0x0005CBD4
	private void SortBtnAllEable()
	{
		for (int i = 0; i < this.m_SortBtn.Length; i++)
		{
			this.m_SortBtn[i].SortEnd();
		}
	}

	// Token: 0x06000D50 RID: 3408 RVA: 0x0005EA04 File Offset: 0x0005CC04
	private ArrayList OtherSort(ArrayList objlist, HouseMixSortBtn.DiscSortKind_e kind, int CellNum, float xsize, int tabCount, int tabsize, HouseMixSortBtn.SortState_e sortstate)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < objlist.Count; i++)
		{
			HouseCellInfo houseCellInfo = new HouseCellInfo();
			HouseMixCell component = ((HouseCellInfo)objlist[i]).obj.GetComponent<HouseMixCell>();
			switch (kind)
			{
			case HouseMixSortBtn.DiscSortKind_e.sort_lv:
				houseCellInfo.obj = ((HouseCellInfo)objlist[i]).obj;
				houseCellInfo.Number = ((HouseCellInfo)objlist[i]).Number2;
				houseCellInfo.Name = component.m_sDiscName;
				break;
			case HouseMixSortBtn.DiscSortKind_e.sort_name:
				houseCellInfo.obj = ((HouseCellInfo)objlist[i]).obj;
				houseCellInfo.Number = ((HouseCellInfo)objlist[i]).Number2;
				houseCellInfo.Name = component.m_sDiscName;
				break;
			case HouseMixSortBtn.DiscSortKind_e.sort_artist:
				houseCellInfo.obj = ((HouseCellInfo)objlist[i]).obj;
				houseCellInfo.Number = ((HouseCellInfo)objlist[i]).Number2;
				houseCellInfo.Name = component.m_sDiscName;
				break;
			}
			arrayList.Add(houseCellInfo);
		}
		if (sortstate == HouseMixSortBtn.SortState_e._up)
		{
			arrayList.Sort(new SortDisc_Number());
		}
		else if (sortstate == HouseMixSortBtn.SortState_e._down)
		{
			arrayList.Sort(new ReverseDisc_Number());
		}
		int num = 0;
		for (int j = CellNum; j < CellNum + arrayList.Count; j++)
		{
			((HouseCellInfo)arrayList[num]).obj.transform.localPosition = new Vector3(xsize + (float)j * this.m_fCellSizeX + this.m_fCellSizeX * 0.5f + (float)(tabCount * tabsize), -35f, 0f);
			num++;
		}
		return arrayList;
	}

	// Token: 0x06000D51 RID: 3409 RVA: 0x0000BDB3 File Offset: 0x00009FB3
	private void OnDestroy()
	{
		if (null != Singleton<SoundSourceManager>.instance)
		{
			Singleton<SoundSourceManager>.instance.StopBgm();
		}
	}

	// Token: 0x04000D6B RID: 3435
	public HouseMixSortBtn[] m_SortBtn;

	// Token: 0x04000D6C RID: 3436
	[HideInInspector]
	public GameObject m_gSelectObj;

	// Token: 0x04000D6D RID: 3437
	private HouseMixSelectDiscInfo m_HouseMixSelectDiscInfo;

	// Token: 0x04000D6E RID: 3438
	private TweenPosition m_tpSelectTag;

	// Token: 0x04000D6F RID: 3439
	private UISprite m_spStageNum;

	// Token: 0x04000D70 RID: 3440
	private UITexture m_txGateTexture;

	// Token: 0x04000D71 RID: 3441
	private UIPanel m_pDiscScrollObj;

	// Token: 0x04000D72 RID: 3442
	private UIScroll m_scDiscScroll;

	// Token: 0x04000D73 RID: 3443
	private GameObject m_gDiscGrid;

	// Token: 0x04000D74 RID: 3444
	private GameObject m_gDiscPrefab;

	// Token: 0x04000D75 RID: 3445
	private GameObject m_gSortTabPrefab;

	// Token: 0x04000D76 RID: 3446
	private HouseMixSortBtn.SortState_e m_BtnState;

	// Token: 0x04000D77 RID: 3447
	private HouseMixSortBtn.DiscSortKind_e m_ClickBtnKind;

	// Token: 0x04000D78 RID: 3448
	private ArrayList m_arrSongList = new ArrayList();

	// Token: 0x04000D79 RID: 3449
	private ArrayList m_arrSaveDisc = new ArrayList();

	// Token: 0x04000D7A RID: 3450
	private ArrayList m_arrDiscName = new ArrayList();

	// Token: 0x04000D7B RID: 3451
	private ArrayList m_arrSortTabObj = new ArrayList();

	// Token: 0x04000D7C RID: 3452
	private int m_iSortStartCount;

	// Token: 0x04000D7D RID: 3453
	private int m_iSort_index;

	// Token: 0x04000D7E RID: 3454
	private float m_fCellSizeX;

	// Token: 0x04000D7F RID: 3455
	private bool m_bSortState;

	// Token: 0x04000D80 RID: 3456
	private bool m_bSortStart;

	// Token: 0x04000D81 RID: 3457
	public DiscInfo SelectDisc;

	// Token: 0x04000D82 RID: 3458
	public HouseStage SelectStage;

	// Token: 0x04000D83 RID: 3459
	public PTLEVEL SelectPtType = PTLEVEL.EZ;

	// Token: 0x04000D84 RID: 3460
	private TimerScript m_sTimer;

	// Token: 0x04000D85 RID: 3461
	public bool Test;

	// Token: 0x04000D86 RID: 3462
	private bool AutoSelect;

	// Token: 0x04000D87 RID: 3463
	private int EndSortDiscCount;

	// Token: 0x04000D88 RID: 3464
	public static int LastDiscID;

	// Token: 0x04000D89 RID: 3465
	public static float LastScrollPosition;

	// Token: 0x04000D8A RID: 3466
	public static PTLEVEL LastPtType = PTLEVEL.EZ;
}
