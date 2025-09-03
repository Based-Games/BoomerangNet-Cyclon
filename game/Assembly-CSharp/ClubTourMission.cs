using System;
using UnityEngine;

// Token: 0x02000187 RID: 391
public class ClubTourMission : MonoBehaviour
{
	// Token: 0x06000BB3 RID: 2995 RVA: 0x000527B8 File Offset: 0x000509B8
	private void Awake()
	{
		this.m_bisAcc = new bool[this.m_iMaxDiscCount];
		this.m_txMissionDisc = new UITexture[this.m_iMaxDiscCount];
		this.m_txMissionDisc_Ex = new UITexture[this.m_iMaxDiscCount];
		this.m_spMissionPt = new UISprite[this.m_iMaxDiscCount];
		this.m_tLevelGrid = new Transform[this.m_iMaxDiscCount];
		this.m_spEff = new UISprite[this.m_iEffMaxCount];
		for (int i = 0; i < this.m_iMaxDiscCount; i++)
		{
			Transform transform = base.transform.FindChild("MissionDisc_" + (i + 1).ToString());
			this.m_txMissionDisc[i] = transform.FindChild("Texture_DiscImage").GetComponent<UITexture>();
			this.m_txMissionDisc_Ex[i] = transform.FindChild("Texture_ex").GetComponent<UITexture>();
			this.m_spMissionPt[i] = transform.FindChild("Sprite_Pt").GetComponent<UISprite>();
			this.m_tLevelGrid[i] = base.transform.FindChild("LevelGrid_" + (i + 1).ToString());
		}
		for (int j = 0; j < this.m_iEffMaxCount; j++)
		{
			this.m_spEff[j] = base.transform.FindChild("Effect").FindChild("eff_" + (j + 1).ToString()).GetComponent<UISprite>();
		}
		this.m_gSpeedEff = base.transform.FindChild("Effect").FindChild("Speed_Eff").gameObject;
		for (int k = 0; k < 3; k++)
		{
			Transform transform2 = base.transform.FindChild((k + 1).ToString() + "_Quest");
			this.m_lQuest[k] = transform2.FindChild("Label_QuestValue").GetComponent<UILabel>();
			this.m_lQuestName[k] = transform2.FindChild("Label_QuestName").GetComponent<UILabel>();
			this.m_spQuestArrow[k] = transform2.FindChild("Sprite_Arrow").GetComponent<UISprite>();
			this.m_QuestAni[k] = transform2.FindChild("Ani").GetComponent<TweenPosition>();
			this.m_spRank[k] = transform2.FindChild("Sprite_Rank").GetComponent<UISprite>();
		}
		Transform transform3 = base.transform.FindChild("Cost");
		this.m_lCost = transform3.FindChild("Label_Cost").GetComponent<UILabel>();
		this.m_CostAni = transform3.FindChild("Ani").GetComponent<TweenPosition>();
		Transform transform4 = base.transform.FindChild("Panel_Rewards");
		Transform transform5 = transform4.FindChild("Bp");
		Transform transform6 = transform4.FindChild("Exp");
		Transform transform7 = transform4.FindChild("Other");
		this.m_lBp = transform5.FindChild("Label_Bp").GetComponent<UILabel>();
		this.m_BpAni = transform5.FindChild("Ani").GetComponent<TweenPosition>();
		this.m_lExp = transform6.FindChild("Label_Exp").GetComponent<UILabel>();
		this.m_ExpAni = transform6.FindChild("Ani").GetComponent<TweenPosition>();
		this.m_OtherAni = transform7.GetComponent<TweenAlpha>();
		this.m_txCD = transform7.FindChild("Texture_DiscCD").GetComponent<UITexture>();
		this.m_lMissionTitle = base.transform.FindChild("MissionName").FindChild("Label_MissionName").GetComponent<UILabel>();
		this.m_gDiscStarPrefab = Resources.Load("prefab/DiscDifficultStar") as GameObject;
	}

	// Token: 0x06000BB4 RID: 2996 RVA: 0x00052B34 File Offset: 0x00050D34
	private void Start()
	{
		for (int i = 0; i < 3; i++)
		{
			this.m_v3ArrowOriginPos[i] = this.m_spQuestArrow[i].transform.localPosition;
		}
		this.Init();
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x00052B7C File Offset: 0x00050D7C
	private void Init()
	{
		this.m_bAniState = false;
		base.CancelInvoke("DiscSetting");
		base.CancelInvoke("ImageChange");
		base.CancelInvoke("EffAniStart");
		for (int i = 0; i < 3; i++)
		{
			this.m_QuestAni[i].transform.localPosition = Vector3.zero;
			this.m_QuestAni[i].enabled = false;
			this.m_lQuestName[i].text = string.Empty;
			this.m_lQuest[i].text = "0";
			this.m_lQuest[i].gameObject.SetActive(false);
			this.m_spRank[i].GetComponent<TweenAlpha>().enabled = false;
			this.m_spRank[i].alpha = 0f;
			for (int j = 0; j < this.m_spRank[i].transform.childCount; j++)
			{
				this.m_spRank[i].transform.GetChild(j).gameObject.SetActive(false);
			}
			this.m_spQuestArrow[i].GetComponent<TweenAlpha>().enabled = false;
			this.m_spQuestArrow[i].alpha = 0f;
			this.m_spQuestArrow[i].gameObject.SetActive(false);
			this.m_bQuestAniState[i] = false;
			this.m_bisAcc[i] = false;
		}
		Behaviour otherAni = this.m_OtherAni;
		bool flag = false;
		this.m_CostAni.enabled = flag;
		flag = flag;
		this.m_BpAni.enabled = flag;
		flag = flag;
		this.m_ExpAni.enabled = flag;
		otherAni.enabled = flag;
		this.m_lCost.text = "0";
		this.m_lBp.text = "0";
		this.m_lExp.text = "0";
		this.m_OtherAni.GetComponent<UIWidget>().alpha = 0f;
		for (int k = 0; k < this.m_iEffMaxCount; k++)
		{
			this.m_spEff[k].GetComponent<TweenAlpha>().enabled = false;
			this.m_spEff[k].alpha = 0f;
		}
		this.m_isAniStep = ClubTourMission.MissionAniStep_e.AniDisc;
		this.m_iDiscAniNowNum = 0;
		this.m_iDiscImageChangeNum = 0;
		this.m_iEffAniNowNum = 0;
		this.m_CostAni.transform.localPosition = Vector3.zero;
		this.m_BpAni.transform.localPosition = Vector3.zero;
		this.m_ExpAni.transform.localPosition = Vector3.zero;
		this.m_bExpAniState = (this.m_bBpAniState = (this.m_bEffAniState = (this.m_bCostAniState = (this.m_bDiscAniState = false))));
	}

	// Token: 0x06000BB6 RID: 2998 RVA: 0x00052E0C File Offset: 0x0005100C
	public void MyRecordSetting()
	{
		UserRecordData netUserRecordData = Singleton<GameManager>.instance.netUserRecordData;
		this.m_ClubTourMyRecord.ValueSetting(netUserRecordData.Score, netUserRecordData.MaxCombo, netUserRecordData.Accuracy, (int)netUserRecordData.RankClass, netUserRecordData.AllCombo, netUserRecordData.PerfectPlay, string.Empty);
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x00052E58 File Offset: 0x00051058
	public void MissionSetting(MissionData md)
	{
		this.Init();
		this.m_MissionData = md;
		for (int i = 0; i < this.m_iMaxDiscCount; i++)
		{
			DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(md.Song[i]);
			Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.DISC_145, discInfo, this.m_txMissionDisc_Ex[i], null, null);
			this.m_txMissionDisc_Ex[i].alpha = 0f;
		}
		this.m_lMissionTitle.text = md.strMissionTitle;
		for (int j = 0; j < md.ArrMissionType.Length; j++)
		{
			MISSIONTYPE missiontype = md.ArrMissionType[j];
			MISSIONTERM missionterm = md.ArrMissionTerm[j];
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
				this.m_lQuestName[j].text = GameData.MISSION_DATATYPE[missiontype].ToUpper();
				this.m_AniKind[j] = ClubTourMission.QuestAniKind_e.Rank;
				this.m_spRank[j].gameObject.SetActive(true);
				this.m_spQuestArrow[j].gameObject.SetActive(true);
				this.SetRankType(this.m_spRank[j], missiontype);
				this.SetTerm(this.m_spQuestArrow[j], missionterm);
				break;
			case MISSIONTYPE.Perfect:
			case MISSIONTYPE.Great:
			case MISSIONTYPE.Good:
			case MISSIONTYPE.Poor:
			case MISSIONTYPE.Break:
			case MISSIONTYPE.FeverCount:
			case MISSIONTYPE.FeverBonus:
			case MISSIONTYPE.ExtremeCount:
			case MISSIONTYPE.ExtremeBonus:
			case MISSIONTYPE.Accuracy:
			case MISSIONTYPE.MaxCombo:
			case MISSIONTYPE.Score:
				this.m_lQuest[j].gameObject.SetActive(true);
				this.m_spQuestArrow[j].gameObject.SetActive(true);
				this.m_lQuestName[j].text = GameData.MISSION_DATATYPE[missiontype].ToUpper();
				this.m_AniKind[j] = ClubTourMission.QuestAniKind_e.Count;
				this.SetTerm(this.m_spQuestArrow[j], missionterm);
				this.m_iQuestValue[j] = md.ArrMissionCount[j];
				if (missiontype == MISSIONTYPE.Accuracy)
				{
					this.m_bisAcc[j] = true;
				}
				break;
			case MISSIONTYPE.PerfectPlay:
			case MISSIONTYPE.AllCombo:
				this.m_lQuestName[j].text = GameData.MISSION_DATATYPE[missiontype].ToUpper();
				this.m_AniKind[j] = ClubTourMission.QuestAniKind_e.None;
				break;
			case MISSIONTYPE.FeverMultiple_X2:
			case MISSIONTYPE.FeverMultiple_X3:
			case MISSIONTYPE.FeverMultiple_X4:
			case MISSIONTYPE.FeverMultiple_X5:
				this.m_lQuest[j].gameObject.SetActive(true);
				this.m_spQuestArrow[j].gameObject.SetActive(true);
				this.m_lQuestName[j].text = GameData.MISSION_DATATYPE[missiontype].ToUpper();
				this.m_AniKind[j] = ClubTourMission.QuestAniKind_e.Count;
				this.SetTerm(this.m_spQuestArrow[j], missionterm);
				this.m_iQuestValue[j] = md.ArrMissionCount[j];
				break;
			case MISSIONTYPE.ExtremeMultiple_X2:
			case MISSIONTYPE.ExtremeMultiple_X3:
				this.m_lQuest[j].gameObject.SetActive(true);
				this.m_spQuestArrow[j].gameObject.SetActive(true);
				this.m_lQuestName[j].text = GameData.MISSION_DATATYPE[missiontype].ToUpper();
				this.m_AniKind[j] = ClubTourMission.QuestAniKind_e.Count;
				this.SetTerm(this.m_spQuestArrow[j], missionterm);
				this.m_iQuestValue[j] = md.ArrMissionCount[j];
				break;
			case MISSIONTYPE.Clear:
				this.m_lQuestName[j].text = GameData.MISSION_DATATYPE[missiontype].ToUpper();
				this.m_AniKind[j] = ClubTourMission.QuestAniKind_e.None;
				break;
			}
		}
		this.m_iCostValue = md.iMissionCost;
		this.EffectorSetting(md);
		this.m_iBpValue = md.iRewardBeatPoint;
		this.m_iExpValue = md.iRewardExp;
		if (md.iRewardSong != 0)
		{
			DiscInfo discInfo2 = Singleton<SongManager>.instance.GetDiscInfo(md.iRewardSong);
			Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.CD_96, discInfo2, this.m_txCD, null, null);
			this.m_txCD.transform.FindChild("Sprite_PT").GetComponent<UISprite>().spriteName = "level_" + md.RewardPattern[0].ToString().ToLower() + "_sm";
		}
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x0005325C File Offset: 0x0005145C
	public void StarSetting(int index, int Diff)
	{
		for (int i = 0; i < this.m_tLevelGrid[index].childCount; i++)
		{
			UnityEngine.Object.DestroyObject(this.m_tLevelGrid[index].GetChild(i).gameObject);
		}
		if (this.m_gDiscStarPrefab != null)
		{
			int num = 0;
			for (int j = 0; j < 12; j++)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.m_gDiscStarPrefab);
				StarSetting component = gameObject.GetComponent<StarSetting>();
				gameObject.transform.parent = this.m_tLevelGrid[index];
				gameObject.transform.localScale = new Vector3(0.55f, 0.55f, 1f);
				string text = string.Empty;
				if (j >= 5 && j < 10)
				{
					text = "_1";
				}
				else if (j >= 10 && j < 13)
				{
					text = "_2";
				}
				gameObject.transform.localPosition = new Vector3(10f * (float)j, 0f, 0f);
				if (Diff <= j)
				{
					component.setStar(false, string.Empty);
					gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
				}
				else
				{
					component.setStar(true, text);
				}
				num++;
			}
		}
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x0000AD56 File Offset: 0x00008F56
	private void PtSetting(int index, PTLEVEL kind)
	{
		this.m_spMissionPt[index].spriteName = "level_" + kind.ToString().ToLower() + "_sm";
	}

	// Token: 0x06000BBA RID: 3002 RVA: 0x000533B4 File Offset: 0x000515B4
	private void EffectorSetting(MissionData md)
	{
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			if (i == 0)
			{
				EFFECTOR_SPEED eff_Speed = md.Eff_Speed;
				this.m_spEff[i].gameObject.SetActive(true);
				if (eff_Speed != EFFECTOR_SPEED.NONE)
				{
					this.m_gSpeedEff.SetActive(false);
				}
				EFFECTOR_SPEED effector_SPEED = eff_Speed;
				switch (effector_SPEED + 1)
				{
				case EFFECTOR_SPEED.X_0_5:
					this.m_spEff[i].gameObject.SetActive(false);
					this.m_gSpeedEff.SetActive(true);
					break;
				case EFFECTOR_SPEED.X_1:
				case EFFECTOR_SPEED.X_1_5:
				case EFFECTOR_SPEED.X_2:
				case EFFECTOR_SPEED.X_2_5:
				case EFFECTOR_SPEED.X_3:
				case EFFECTOR_SPEED.X_3_5:
				case EFFECTOR_SPEED.X_4:
				case EFFECTOR_SPEED.X_5:
				case EFFECTOR_SPEED.X_6:
				case EFFECTOR_SPEED.MAX_SPEED:
					this.m_spEff[i].spriteName = "speed_" + (int)md.Eff_Fader + "_select";
					break;
				case EFFECTOR_SPEED.CHAOS_W:
					this.m_spEff[i].spriteName = "speed_12_select";
					break;
				case EFFECTOR_SPEED.CHAOS_UP:
					this.m_spEff[i].spriteName = "speed_13_select";
					break;
				case EFFECTOR_SPEED.CHAOS_DN:
					this.m_spEff[i].spriteName = "speed_14_select";
					break;
				case EFFECTOR_SPEED.MAX:
					this.m_spEff[i].spriteName = "speed_15_select";
					break;
				}
			}
			else if (i == 1)
			{
				this.m_spEff[i].gameObject.SetActive(true);
				switch (md.Eff_Fader)
				{
				case EFFECTOR_FADER.NONE:
					this.m_spEff[i].gameObject.SetActive(false);
					break;
				case EFFECTOR_FADER.FADEIN:
				case EFFECTOR_FADER.FADEOUT:
				case EFFECTOR_FADER.BLINK:
					num++;
					this.m_spEff[i].spriteName = "fader_" + (int)md.Eff_Fader + "_select";
					break;
				case EFFECTOR_FADER.BLANK:
					num++;
					this.m_spEff[i].spriteName = "fader_5_select";
					break;
				}
			}
			else if (i == 2)
			{
				this.m_spEff[i].gameObject.SetActive(true);
				EFFECTOR_RAND eff_Rand = md.Eff_Rand;
				switch (eff_Rand)
				{
				case EFFECTOR_RAND.NONE:
					this.m_spEff[i].gameObject.SetActive(false);
					break;
				case EFFECTOR_RAND.MIRROR:
				case EFFECTOR_RAND.RANDOM:
				case EFFECTOR_RAND.ROTATE_LEFT:
				case EFFECTOR_RAND.ROTATE_RIGHT:
				case EFFECTOR_RAND.ROTATE_RANDOM:
					this.m_spEff[i].spriteName = "note_" + (int)eff_Rand + "_select";
					break;
				case EFFECTOR_RAND.CYCLON:
					this.m_spEff[i].spriteName = "note_6_select";
					break;
				}
				this.m_spEff[i].transform.localPosition = new Vector3((float)(22 + num * 72), 0f, 0f);
			}
		}
	}

	// Token: 0x06000BBB RID: 3003 RVA: 0x0005368C File Offset: 0x0005188C
	private void SetRankType(UISprite image, MISSIONTYPE isType)
	{
		switch (isType)
		{
		case MISSIONTYPE.Rank_F:
		case MISSIONTYPE.Rank_D:
		case MISSIONTYPE.Rank_C:
		case MISSIONTYPE.Rank_B:
		case MISSIONTYPE.Rank_A:
		case MISSIONTYPE.Rank_S:
			image.spriteName = "MyRecord_Rank_" + (int)isType;
			break;
		case MISSIONTYPE.Rank_AP:
		case MISSIONTYPE.Rank_SP:
			image.spriteName = "MyRecord_Rank_" + (isType - MISSIONTYPE.Rank_D);
			image.transform.FindChild("Sprite_Plus_1").gameObject.SetActive(true);
			break;
		case MISSIONTYPE.Rank_APP:
		case MISSIONTYPE.Rank_SPP:
		{
			image.spriteName = "MyRecord_Rank_" + (isType - MISSIONTYPE.Rank_C);
			for (int i = 0; i < 2; i++)
			{
				image.transform.FindChild("Sprite_Plus_" + (i + 1).ToString()).gameObject.SetActive(true);
			}
			break;
		}
		}
		string text = "4";
		if (isType > MISSIONTYPE.Rank_APP && isType <= MISSIONTYPE.Rank_SPP)
		{
			text = "7";
		}
		for (int j = 0; j < 2; j++)
		{
			image.transform.FindChild("Sprite_Plus_" + (j + 1).ToString()).GetComponent<UISprite>().spriteName = "Plus_" + text;
		}
	}

	// Token: 0x06000BBC RID: 3004 RVA: 0x000537E0 File Offset: 0x000519E0
	private void SetTerm(UISprite target, MISSIONTERM kind)
	{
		switch (kind + 1)
		{
		case MISSIONTERM.Under:
			target.spriteName = "result_arrow_up";
			break;
		case MISSIONTERM.Same:
			target.spriteName = "result_arrow_down";
			break;
		case (MISSIONTERM)3:
			target.spriteName = "result_arrow_none";
			break;
		}
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x0000AD84 File Offset: 0x00008F84
	public void PlayAni()
	{
		base.CancelInvoke("DiscSound");
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_CLUBTOUR_SPREAD);
		this.m_bAniState = true;
	}

	// Token: 0x06000BBE RID: 3006 RVA: 0x0000ADA4 File Offset: 0x00008FA4
	private void Update()
	{
		if (!this.m_bAniState)
		{
			return;
		}
		this.Ani();
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x00053844 File Offset: 0x00051A44
	private void Ani()
	{
		ClubTourMission.MissionAniStep_e isAniStep = this.m_isAniStep;
		if (isAniStep == ClubTourMission.MissionAniStep_e.AniDisc)
		{
			this.DiscAni();
			this.QuestAni();
			this.CostAni();
			this.EffAni();
			this.BpAni();
			this.ExpAni();
			this.CDAni();
			if (this.m_iDiscImageChangeNum == this.m_iMaxDiscCount)
			{
				this.m_bAniState = false;
			}
		}
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x000538AC File Offset: 0x00051AAC
	private void DiscAni()
	{
		if (this.m_bDiscAniState)
		{
			return;
		}
		base.Invoke("DiscSound", 0.01f);
		for (int i = 0; i < this.m_iMaxDiscCount; i++)
		{
			base.Invoke("DiscSetting", 0.01f + this.m_fDiscAniFrame * (float)i);
		}
		this.m_iDiscImageChangeNum = 0;
		this.m_bDiscAniState = true;
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x0000ADB8 File Offset: 0x00008FB8
	private void DiscSound()
	{
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_CLUBTOUR_SPREAD, false);
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x00053914 File Offset: 0x00051B14
	private void DiscSetting()
	{
		TweenRotation component = this.m_txMissionDisc[this.m_iDiscAniNowNum].transform.parent.GetComponent<TweenRotation>();
		component.ResetToBeginning();
		component.Play(true);
		this.m_iDiscAniNowNum++;
		base.Invoke("ImageChange", component.duration * 0.45f);
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x00053970 File Offset: 0x00051B70
	private void ImageChange()
	{
		DiscInfo discInfo = Singleton<SongManager>.instance.GetDiscInfo(this.m_MissionData.Song[this.m_iDiscImageChangeNum]);
		this.m_txMissionDisc[this.m_iDiscImageChangeNum].mainTexture = this.m_txMissionDisc_Ex[this.m_iDiscImageChangeNum].mainTexture;
		this.PtSetting(this.m_iDiscImageChangeNum, this.m_MissionData.Pattern[this.m_iDiscImageChangeNum]);
		this.StarSetting(this.m_iDiscImageChangeNum, discInfo.DicPtInfo[this.m_MissionData.Pattern[this.m_iDiscImageChangeNum]].iDif);
		this.m_iDiscImageChangeNum++;
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x00053A18 File Offset: 0x00051C18
	private void QuestAni()
	{
		for (int i = 0; i < 3; i++)
		{
			ClubTourMission.QuestAniKind_e questAniKind_e = this.m_AniKind[i];
			if (questAniKind_e != ClubTourMission.QuestAniKind_e.Count)
			{
				if (questAniKind_e == ClubTourMission.QuestAniKind_e.Rank)
				{
					if (!this.m_bQuestAniState[i])
					{
						this.m_bQuestAniState[i] = true;
						this.m_spRank[i].GetComponent<TweenAlpha>().ResetToBeginning();
						this.m_spRank[i].GetComponent<TweenAlpha>().Play(true);
						this.m_spQuestArrow[i].transform.localPosition = new Vector3(this.m_v3ArrowOriginPos[i].x - (float)this.m_spRank[i].width, this.m_v3ArrowOriginPos[i].y, this.m_v3ArrowOriginPos[i].z);
						this.m_spQuestArrow[i].GetComponent<TweenAlpha>().ResetToBeginning();
						this.m_spQuestArrow[i].GetComponent<TweenAlpha>().Play(true);
					}
				}
			}
			else
			{
				int num = 0;
				if (!this.m_bisAcc[i])
				{
					this.m_lQuest[i].text = ((int)((float)this.m_iQuestValue[i] * this.m_QuestAni[i].transform.localPosition.x)).ToString();
				}
				else
				{
					this.m_lQuest[i].text = ((int)((float)this.m_iQuestValue[i] * this.m_QuestAni[i].transform.localPosition.x)).ToString() + "%";
					num++;
				}
				if (!this.m_bQuestAniState[i])
				{
					this.m_bQuestAniState[i] = true;
					this.m_QuestAni[i].ResetToBeginning();
					this.m_QuestAni[i].Play(true);
					this.m_spQuestArrow[i].transform.localPosition = new Vector3(this.m_v3ArrowOriginPos[i].x - (float)(this.m_iQuestValue[i].ToString().Length + num) * this.m_fQuestArrowSpaceValue, this.m_v3ArrowOriginPos[i].y, this.m_v3ArrowOriginPos[i].z);
					this.m_spQuestArrow[i].GetComponent<TweenAlpha>().ResetToBeginning();
					this.m_spQuestArrow[i].GetComponent<TweenAlpha>().Play(true);
				}
				if (this.m_QuestAni[i].transform.localPosition.x == 1f)
				{
				}
			}
		}
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x00053C98 File Offset: 0x00051E98
	private void CostAni()
	{
		if (!this.m_bCostAniState)
		{
			this.m_bCostAniState = true;
			this.m_CostAni.ResetToBeginning();
			this.m_CostAni.Play(true);
		}
		this.m_lCost.text = ((int)((float)this.m_iCostValue * this.m_CostAni.transform.localPosition.x)).ToString();
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x00053D04 File Offset: 0x00051F04
	private void EffAni()
	{
		if (this.m_bEffAniState)
		{
			return;
		}
		this.m_bEffAniState = true;
		for (int i = 0; i < this.m_iEffMaxCount; i++)
		{
			base.Invoke("EffAniStart", this.m_fEffAniFrame * (float)i);
		}
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x00053D50 File Offset: 0x00051F50
	private void EffAniStart()
	{
		this.m_spEff[this.m_iEffAniNowNum].alpha = 0f;
		TweenScale component = this.m_spEff[this.m_iEffAniNowNum].GetComponent<TweenScale>();
		TweenAlpha component2 = this.m_spEff[this.m_iEffAniNowNum].GetComponent<TweenAlpha>();
		component.ResetToBeginning();
		component.Play(true);
		component2.ResetToBeginning();
		component2.Play(true);
		this.m_iEffAniNowNum++;
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x00053DC4 File Offset: 0x00051FC4
	private void BpAni()
	{
		if (!this.m_bBpAniState)
		{
			this.m_bBpAniState = true;
			this.m_BpAni.ResetToBeginning();
			this.m_BpAni.Play(true);
		}
		this.m_lBp.text = ((int)((float)this.m_iBpValue * this.m_BpAni.transform.localPosition.x)).ToString();
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x00053E30 File Offset: 0x00052030
	private void ExpAni()
	{
		if (!this.m_bExpAniState)
		{
			this.m_bExpAniState = true;
			this.m_ExpAni.ResetToBeginning();
			this.m_ExpAni.Play(true);
		}
		this.m_lExp.text = ((int)((float)this.m_iExpValue * this.m_ExpAni.transform.localPosition.x)).ToString();
	}

	// Token: 0x06000BCA RID: 3018 RVA: 0x0000ADC7 File Offset: 0x00008FC7
	private void CDAni()
	{
		if (this.m_MissionData.iRewardSong > 0)
		{
			this.m_OtherAni.ResetToBeginning();
			this.m_OtherAni.Play(true);
		}
	}

	// Token: 0x04000B36 RID: 2870
	public ClubTourMyRecord m_ClubTourMyRecord;

	// Token: 0x04000B37 RID: 2871
	private UILabel m_lMissionTitle;

	// Token: 0x04000B38 RID: 2872
	private UITexture[] m_txMissionDisc;

	// Token: 0x04000B39 RID: 2873
	private UITexture[] m_txMissionDisc_Ex;

	// Token: 0x04000B3A RID: 2874
	private UISprite[] m_spMissionPt;

	// Token: 0x04000B3B RID: 2875
	private Transform[] m_tLevelGrid;

	// Token: 0x04000B3C RID: 2876
	private UISprite[] m_spEff;

	// Token: 0x04000B3D RID: 2877
	private GameObject m_gSpeedEff;

	// Token: 0x04000B3E RID: 2878
	private ClubTourMission.QuestAniKind_e[] m_AniKind = new ClubTourMission.QuestAniKind_e[3];

	// Token: 0x04000B3F RID: 2879
	private UILabel[] m_lQuest = new UILabel[3];

	// Token: 0x04000B40 RID: 2880
	private UILabel[] m_lQuestName = new UILabel[3];

	// Token: 0x04000B41 RID: 2881
	private UISprite[] m_spQuestArrow = new UISprite[3];

	// Token: 0x04000B42 RID: 2882
	private Vector3[] m_v3ArrowOriginPos = new Vector3[3];

	// Token: 0x04000B43 RID: 2883
	private TweenPosition[] m_QuestAni = new TweenPosition[3];

	// Token: 0x04000B44 RID: 2884
	private UISprite[] m_spRank = new UISprite[3];

	// Token: 0x04000B45 RID: 2885
	private int[] m_iQuestValue = new int[3];

	// Token: 0x04000B46 RID: 2886
	private UILabel m_lCost;

	// Token: 0x04000B47 RID: 2887
	private TweenPosition m_CostAni;

	// Token: 0x04000B48 RID: 2888
	private int m_iCostValue;

	// Token: 0x04000B49 RID: 2889
	private UILabel m_lBp;

	// Token: 0x04000B4A RID: 2890
	private TweenPosition m_BpAni;

	// Token: 0x04000B4B RID: 2891
	private int m_iBpValue;

	// Token: 0x04000B4C RID: 2892
	private UILabel m_lExp;

	// Token: 0x04000B4D RID: 2893
	private TweenPosition m_ExpAni;

	// Token: 0x04000B4E RID: 2894
	private int m_iExpValue;

	// Token: 0x04000B4F RID: 2895
	private TweenAlpha m_OtherAni;

	// Token: 0x04000B50 RID: 2896
	private UITexture m_txCD;

	// Token: 0x04000B51 RID: 2897
	private int m_iMaxDiscCount = 3;

	// Token: 0x04000B52 RID: 2898
	private int m_iEffMaxCount = 3;

	// Token: 0x04000B53 RID: 2899
	private int m_iDiscAniNowNum;

	// Token: 0x04000B54 RID: 2900
	private int m_iDiscImageChangeNum;

	// Token: 0x04000B55 RID: 2901
	private int m_iEffAniNowNum;

	// Token: 0x04000B56 RID: 2902
	private float m_fDiscAniFrame = 0.175f;

	// Token: 0x04000B57 RID: 2903
	private float m_fEffAniFrame = 0.075f;

	// Token: 0x04000B58 RID: 2904
	private float m_fQuestArrowSpaceValue = 15f;

	// Token: 0x04000B59 RID: 2905
	private bool m_bAniState;

	// Token: 0x04000B5A RID: 2906
	private bool m_bDiscAniState;

	// Token: 0x04000B5B RID: 2907
	private bool[] m_bQuestAniState = new bool[3];

	// Token: 0x04000B5C RID: 2908
	private bool m_bCostAniState;

	// Token: 0x04000B5D RID: 2909
	private bool m_bEffAniState;

	// Token: 0x04000B5E RID: 2910
	private bool m_bBpAniState;

	// Token: 0x04000B5F RID: 2911
	private bool m_bExpAniState;

	// Token: 0x04000B60 RID: 2912
	private bool[] m_bisAcc;

	// Token: 0x04000B61 RID: 2913
	private MissionData m_MissionData;

	// Token: 0x04000B62 RID: 2914
	private ClubTourMission.MissionAniStep_e m_isAniStep;

	// Token: 0x04000B63 RID: 2915
	private GameObject m_gDiscStarPrefab;

	// Token: 0x02000188 RID: 392
	public enum MissionAniStep_e
	{
		// Token: 0x04000B65 RID: 2917
		AniDisc,
		// Token: 0x04000B66 RID: 2918
		AniQuest,
		// Token: 0x04000B67 RID: 2919
		AniCost,
		// Token: 0x04000B68 RID: 2920
		AniEff,
		// Token: 0x04000B69 RID: 2921
		AniBp,
		// Token: 0x04000B6A RID: 2922
		AniExp,
		// Token: 0x04000B6B RID: 2923
		AniCD,
		// Token: 0x04000B6C RID: 2924
		End
	}

	// Token: 0x02000189 RID: 393
	public enum QuestAniKind_e
	{
		// Token: 0x04000B6E RID: 2926
		None,
		// Token: 0x04000B6F RID: 2927
		Count,
		// Token: 0x04000B70 RID: 2928
		Rank
	}

	// Token: 0x0200018A RID: 394
	public enum EffKind_e
	{
		// Token: 0x04000B72 RID: 2930
		speed,
		// Token: 0x04000B73 RID: 2931
		fader,
		// Token: 0x04000B74 RID: 2932
		rand,
		// Token: 0x04000B75 RID: 2933
		max
	}
}
