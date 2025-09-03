using System;
using UnityEngine;

// Token: 0x0200019B RID: 411
public class ClubTourResultQuest : MonoBehaviour
{
	// Token: 0x06000C3E RID: 3134 RVA: 0x00057138 File Offset: 0x00055338
	private void Awake()
	{
		this.m_iQuestValue = new int[this.m_iQuestMaxCount];
		this.m_lQuests = new UILabel[this.m_iQuestMaxCount];
		this.m_spQuestArrow = new UISprite[this.m_iQuestMaxCount];
		this.m_v3ArrowOriginPos = new Vector3[this.m_iQuestMaxCount];
		this.m_spQuestCheckIcon = new UISprite[this.m_iQuestMaxCount];
		this.m_lQuestName = new UILabel[this.m_iQuestMaxCount];
		this.m_spQuestRank = new UISprite[this.m_iQuestMaxCount];
		for (int i = 0; i < this.m_iQuestMaxCount; i++)
		{
			this.m_lQuests[i] = base.transform.FindChild("Label_Quest_" + (i + 1).ToString()).GetComponent<UILabel>();
			this.m_spQuestArrow[i] = base.transform.FindChild("Sprite_Arrow_" + (i + 1).ToString()).GetComponent<UISprite>();
			this.m_lQuestName[i] = base.transform.FindChild("Label_QuestName_" + (i + 1).ToString()).GetComponent<UILabel>();
			this.m_spQuestCheckIcon[i] = base.transform.FindChild("Sprite_Quest_Check_" + (i + 1).ToString()).GetComponent<UISprite>();
			this.m_spQuestRank[i] = base.transform.FindChild("Sprite_Rank_" + (i + 1).ToString()).GetComponent<UISprite>();
		}
		this.m_ClubTourResult = base.transform.parent.GetComponent<ClubTourResult>();
	}

	// Token: 0x06000C3F RID: 3135 RVA: 0x00003648 File Offset: 0x00001848
	public void DefaultQuestSetting()
	{
	}

	// Token: 0x06000C40 RID: 3136 RVA: 0x000572CC File Offset: 0x000554CC
	public void QuestSetting()
	{
		MissionData mission = Singleton<SongManager>.instance.Mission;
		for (int i = 0; i < this.m_iQuestMaxCount; i++)
		{
			this.m_v3ArrowOriginPos[i] = this.m_spQuestArrow[i].transform.localPosition;
		}
		for (int j = 0; j < mission.ArrMissionType.Length; j++)
		{
			if (j == this.m_iQuestMaxCount)
			{
				return;
			}
			this.m_lQuestName[j].gameObject.SetActive(false);
			this.m_spQuestCheckIcon[j].gameObject.SetActive(false);
			this.m_lQuests[j].gameObject.SetActive(false);
			this.m_spQuestArrow[j].gameObject.SetActive(false);
			this.m_spQuestRank[j].gameObject.SetActive(false);
			MISSIONTYPE missiontype = mission.ArrMissionType[j];
			MISSIONTERM missionterm = mission.ArrMissionTerm[j];
			if (mission.IsClear(j))
			{
				if (missiontype != MISSIONTYPE.None)
				{
					this.m_spQuestCheckIcon[j].gameObject.SetActive(true);
					this.m_spQuestCheckIcon[j].spriteName = "ClubTourResult_CheckIcon";
					this.m_spQuestCheckIcon[j].MakePixelPerfect();
					this.m_spQuestCheckIcon[j].transform.localScale = Vector3.one * 2f;
				}
			}
			else if (missiontype != MISSIONTYPE.None)
			{
				this.m_spQuestCheckIcon[j].gameObject.SetActive(true);
				this.m_spQuestCheckIcon[j].spriteName = "ClubTourResult_FailIcon";
				this.m_spQuestCheckIcon[j].MakePixelPerfect();
				this.m_spQuestCheckIcon[j].transform.localScale = Vector3.one * 2f;
			}
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
				this.m_lQuestName[j].gameObject.SetActive(true);
				this.m_lQuestName[j].text = GameData.MISSION_DATATYPE[missiontype].ToUpper();
				this.m_AniKind[j] = ClubTourMission.QuestAniKind_e.Rank;
				this.m_spQuestRank[j].gameObject.SetActive(true);
				this.m_spQuestArrow[j].gameObject.SetActive(true);
				this.SetRankType(this.m_spQuestRank[j], missiontype);
				this.SetTerm(this.m_spQuestArrow[j], missionterm);
				this.ArrowPosSetting();
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
				if (missiontype == MISSIONTYPE.ExtremeBonus || missiontype == MISSIONTYPE.ExtremeCount || missiontype == MISSIONTYPE.FeverBonus || missiontype == MISSIONTYPE.FeverCount)
				{
					this.m_lQuestName[j].fontSize = 26;
				}
				else
				{
					this.m_lQuestName[j].fontSize = 30;
				}
				this.m_lQuests[j].gameObject.SetActive(true);
				this.m_spQuestArrow[j].gameObject.SetActive(true);
				this.m_lQuestName[j].gameObject.SetActive(true);
				this.m_lQuestName[j].text = GameData.MISSION_DATATYPE[missiontype].ToUpper();
				this.m_AniKind[j] = ClubTourMission.QuestAniKind_e.Count;
				this.SetTerm(this.m_spQuestArrow[j], missionterm);
				if (missiontype == MISSIONTYPE.Accuracy)
				{
					this.m_lQuests[j].text = mission.ArrMissionCount[j].ToString() + "%";
				}
				else
				{
					this.m_lQuests[j].text = mission.ArrMissionCount[j].ToString();
				}
				this.m_iQuestValue[j] = mission.ArrMissionCount[j];
				this.ArrowPosSetting();
				break;
			case MISSIONTYPE.PerfectPlay:
			case MISSIONTYPE.AllCombo:
				this.m_lQuestName[j].gameObject.SetActive(true);
				this.m_lQuestName[j].text = GameData.MISSION_DATATYPE[missiontype].ToUpper();
				this.m_AniKind[j] = ClubTourMission.QuestAniKind_e.Count;
				break;
			case MISSIONTYPE.FeverMultiple_X2:
			case MISSIONTYPE.FeverMultiple_X3:
			case MISSIONTYPE.FeverMultiple_X4:
			case MISSIONTYPE.FeverMultiple_X5:
			case MISSIONTYPE.ExtremeMultiple_X2:
			case MISSIONTYPE.ExtremeMultiple_X3:
				if (missiontype == MISSIONTYPE.ExtremeMultiple_X3)
				{
					this.m_lQuestName[j].fontSize = 20;
				}
				else
				{
					this.m_lQuestName[j].fontSize = 26;
				}
				this.m_lQuests[j].gameObject.SetActive(true);
				this.m_spQuestArrow[j].gameObject.SetActive(true);
				this.m_lQuestName[j].gameObject.SetActive(true);
				this.m_lQuestName[j].text = GameData.MISSION_DATATYPE[missiontype].ToUpper();
				this.m_AniKind[j] = ClubTourMission.QuestAniKind_e.Count;
				this.SetTerm(this.m_spQuestArrow[j], missionterm);
				this.m_lQuests[j].text = mission.ArrMissionCount[j].ToString();
				this.m_iQuestValue[j] = mission.ArrMissionCount[j];
				this.ArrowPosSetting();
				break;
			case MISSIONTYPE.Clear:
				this.m_lQuestName[j].gameObject.SetActive(true);
				this.m_lQuestName[j].text = GameData.MISSION_DATATYPE[missiontype].ToUpper();
				this.m_AniKind[j] = ClubTourMission.QuestAniKind_e.None;
				break;
			}
		}
	}

	// Token: 0x06000C41 RID: 3137 RVA: 0x00057804 File Offset: 0x00055A04
	private void ArrowPosSetting()
	{
		for (int i = 0; i < this.m_iQuestMaxCount; i++)
		{
			ClubTourMission.QuestAniKind_e questAniKind_e = this.m_AniKind[i];
			if (questAniKind_e != ClubTourMission.QuestAniKind_e.Count)
			{
				if (questAniKind_e == ClubTourMission.QuestAniKind_e.Rank)
				{
					this.m_spQuestArrow[i].transform.localPosition = new Vector3(this.m_v3ArrowOriginPos[i].x - (float)(this.m_spQuestRank[i].width * 2), this.m_v3ArrowOriginPos[i].y, this.m_v3ArrowOriginPos[i].z);
				}
			}
			else
			{
				this.m_spQuestArrow[i].transform.localPosition = new Vector3(this.m_v3ArrowOriginPos[i].x - (float)this.m_lQuests[i].text.Length * this.m_fQuestArrowSpaceValue, this.m_v3ArrowOriginPos[i].y, this.m_v3ArrowOriginPos[i].z);
			}
		}
	}

	// Token: 0x06000C42 RID: 3138 RVA: 0x000537E0 File Offset: 0x000519E0
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

	// Token: 0x06000C43 RID: 3139 RVA: 0x0005368C File Offset: 0x0005188C
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

	// Token: 0x04000C39 RID: 3129
	private UILabel[] m_lQuests;

	// Token: 0x04000C3A RID: 3130
	private UILabel[] m_lQuestName;

	// Token: 0x04000C3B RID: 3131
	private UISprite[] m_spQuestArrow;

	// Token: 0x04000C3C RID: 3132
	private UISprite[] m_spQuestCheckIcon;

	// Token: 0x04000C3D RID: 3133
	private UISprite[] m_spQuestRank;

	// Token: 0x04000C3E RID: 3134
	private ClubTourMission.QuestAniKind_e[] m_AniKind = new ClubTourMission.QuestAniKind_e[3];

	// Token: 0x04000C3F RID: 3135
	private string m_sCompleteIcon = "ClubTourResult_CheckIcon";

	// Token: 0x04000C40 RID: 3136
	private string m_sFailIcon = "ClubTourResult_FailIcon";

	// Token: 0x04000C41 RID: 3137
	private ClubTourResult m_ClubTourResult;

	// Token: 0x04000C42 RID: 3138
	private int m_iQuestMaxCount = 2;

	// Token: 0x04000C43 RID: 3139
	private Vector3[] m_v3ArrowOriginPos;

	// Token: 0x04000C44 RID: 3140
	private int[] m_iQuestValue;

	// Token: 0x04000C45 RID: 3141
	private float m_fQuestArrowSpaceValue = 25f;
}
