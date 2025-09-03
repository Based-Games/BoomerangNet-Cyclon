using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000F9 RID: 249
public class ControlMissionScript : MonoBehaviour
{
	// Token: 0x060008B4 RID: 2228 RVA: 0x0004273C File Offset: 0x0004093C
	private void Start()
	{
		if (Singleton<SongManager>.instance.Mode != GAMEMODE.MISSION)
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		MissionData mission = Singleton<SongManager>.instance.Mission;
		for (int i = 0; i < 2; i++)
		{
			this.m_arrControl[i] = base.transform.FindChild("Mission" + i.ToString()).transform.gameObject;
			UILabel component = this.m_arrControl[i].transform.FindChild("MissionTitle").GetComponent<UILabel>();
			string text = mission.ArrMissionType[i].ToString();
			if (GameData.MISSION_DATATYPE.ContainsKey(mission.ArrMissionType[i]))
			{
				text = GameData.MISSION_DATATYPE[mission.ArrMissionType[i]];
			}
			component.text = text;
			this.m_arrMisssionNum[i] = this.m_arrControl[i].transform.FindChild("Mission").GetComponent<UILabel>();
			this.m_arrMisssionNum[i].text = mission.ArrMissionCount[i].ToString();
			UISprite component2 = this.m_arrControl[i].transform.FindChild("Term").GetComponent<UISprite>();
			this.SetTerm(component2, mission.ArrMissionTerm[i]);
			this.m_arrMisssionCheck[i] = this.m_arrControl[i].transform.FindChild("Mark").GetComponent<UISprite>();
			this.m_arrMisssionCheck[i].enabled = false;
			if (mission.ArrMissionType[i] == MISSIONTYPE.Clear)
			{
				component.transform.localPosition = new Vector3(0f, -10f, 0f);
				component2.gameObject.SetActive(false);
				this.m_arrMisssionCheck[i].gameObject.SetActive(false);
				this.m_arrMisssionNum[i].gameObject.SetActive(false);
			}
			if (mission.ArrMissionType[i] == MISSIONTYPE.None)
			{
				this.m_arrControl[i].SetActive(false);
			}
		}
		base.StartCoroutine(this.ViewMissionNum());
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x0004294C File Offset: 0x00040B4C
	private IEnumerator ViewMissionNum()
	{
		for (;;)
		{
			MissionData mData = Singleton<SongManager>.instance.Mission;
			string strNum = string.Empty;
			for (int i = 0; i < 2; i++)
			{
				if (mData.ArrMissionType[i] != MISSIONTYPE.Clear)
				{
					this.m_arrMisssionCheck[i].enabled = mData.IsClear(i);
					if (mData.ArrMissionType[i] == MISSIONTYPE.Accuracy)
					{
						strNum = string.Format("{0}% / {1}%", mData.GetChekcNum(i), mData.ArrMissionCount[i]);
					}
					else
					{
						strNum = string.Format("{0} / {1}", mData.GetChekcNum(i), mData.ArrMissionCount[i]);
					}
					for (MISSIONTYPE mType = MISSIONTYPE.Rank_F; mType < MISSIONTYPE.Perfect; mType++)
					{
						if (mType == mData.ArrMissionType[i])
						{
							RESULTDATA rData = Singleton<GameManager>.instance.ResultData;
							int iGrade = (int)rData.GRADETYPE;
							MISSIONTYPE mTemp = (MISSIONTYPE)iGrade;
							if (GameData.MISSION_DATATYPE.ContainsKey(mTemp))
							{
								strNum = GameData.MISSION_DATATYPE[mTemp];
							}
						}
					}
					this.m_arrMisssionNum[i].text = strNum;
				}
			}
			this.CheckMission();
			yield return new WaitForSeconds(0.3f);
		}
		yield break;
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x00042968 File Offset: 0x00040B68
	private void CheckMission()
	{
		if (Singleton<SongManager>.instance.Mode != GAMEMODE.MISSION)
		{
			return;
		}
		MissionData mission = Singleton<SongManager>.instance.Mission;
		for (int i = 0; i < 2; i++)
		{
			if (mission.ArrMissionType[i] != MISSIONTYPE.Clear)
			{
				this.m_arrMisssionCheck[i].enabled = mission.IsClear(i);
			}
		}
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x000429CC File Offset: 0x00040BCC
	private void SetTerm(UISprite uTerm, MISSIONTERM eTerm)
	{
		if (eTerm == MISSIONTERM.Same)
		{
			uTerm.spriteName = "markequal";
		}
		else if (eTerm == MISSIONTERM.Over)
		{
			uTerm.spriteName = "arrow_up";
		}
		else if (eTerm == MISSIONTERM.Under)
		{
			uTerm.spriteName = "arrow_down";
		}
		else if (eTerm == MISSIONTERM.None)
		{
			uTerm.gameObject.SetActive(false);
		}
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x00003648 File Offset: 0x00001848
	private void Update()
	{
	}

	// Token: 0x04000719 RID: 1817
	private GameObject[] m_arrControl = new GameObject[2];

	// Token: 0x0400071A RID: 1818
	private UISprite[] m_arrMisssionCheck = new UISprite[2];

	// Token: 0x0400071B RID: 1819
	private UILabel[] m_arrMisssionNum = new UILabel[2];
}
