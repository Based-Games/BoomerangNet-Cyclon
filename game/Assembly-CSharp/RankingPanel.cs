using System;
using System.Collections;
using System.Linq;
using UnityEngine;

// Token: 0x020001FC RID: 508
public class RankingPanel : MonoBehaviour
{
	// Token: 0x06000EB1 RID: 3761 RVA: 0x00069BD4 File Offset: 0x00067DD4
	private void Awake()
	{
		this.m_RankingManagerScript = base.transform.parent.GetComponent<RankingManagerScript>();
		this.m_Panel = base.transform.Find("Panel").GetComponent<UIPanel>();
		this.m_tHausMixGrid = this.m_Panel.transform.Find("HausMixGrid");
		this.m_tRaveUpGrid = this.m_Panel.transform.Find("RaveUpGrid");
		this.m_tHausMixForm = this.m_tHausMixGrid.Find("Form");
		this.m_tRaveUpForm = this.m_tRaveUpGrid.Find("Form");
		this.m_gHausMixCells = new GameObject[this.m_tHausMixForm.childCount];
		this.m_gRaveUpCells = new GameObject[this.m_tRaveUpForm.childCount];
		for (int i = 0; i < this.m_gHausMixCells.Length; i++)
		{
			GameObject gameObject = this.m_tHausMixForm.GetChild(i).gameObject;
			gameObject.name = "HausMixRankingCell_" + i.ToString();
			gameObject.transform.localPosition = new Vector3(0f, -this.m_fCellSzie_Y * (float)i, 0f);
			this.m_gHausMixCells[i] = gameObject;
		}
		for (int j = 0; j < this.m_gRaveUpCells.Length; j++)
		{
			GameObject gameObject2 = this.m_tRaveUpForm.GetChild(j).gameObject;
			gameObject2.name = "RaveUpRankingCell_" + j.ToString();
			gameObject2.transform.localPosition = new Vector3(0f, -this.m_fCellSzie_Y * (float)j, 0f);
			this.m_gRaveUpCells[j] = gameObject2;
		}
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x00069D74 File Offset: 0x00067F74
	private void setRank()
	{
		this.m_arrTargetRank = Singleton<GameManager>.instance.getHausModeRank();
		this.m_tTargetGrid = this.m_tHausMixGrid;
		this.m_gTargetCell = this.m_gHausMixCells;
		this.m_v2PanelSize = this.m_Panel.GetViewSize();
		base.Invoke("FirstInit", 1.5f);
		base.Invoke("StartAni", 1.5f);
	}

	// Token: 0x06000EB3 RID: 3763 RVA: 0x0000CBDE File Offset: 0x0000ADDE
	private void Start()
	{
		this.GetRanking();
	}

	// Token: 0x06000EB4 RID: 3764 RVA: 0x0000CBE6 File Offset: 0x0000ADE6
	private void FirstInit()
	{
		this.Init(this.m_tTargetGrid, this.m_gTargetCell);
		this.m_initSetting = true;
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x00069DDC File Offset: 0x00067FDC
	private void Init(Transform grid, GameObject[] cells)
	{
		for (int i = 0; i < cells.Length; i++)
		{
			int num = this.m_iRankingCellMaxCount - this.m_iRankingCellMakeCount + i;
			RankInfo rankInfo = (RankInfo)this.m_arrTargetRank[num];
			cells[i].GetComponent<RankingCell>().setUserIcon(rankInfo.Icon);
			cells[i].GetComponent<RankingCell>().Setting(this.m_bIsRaveUpRanking, this.m_iRankingCellMaxCount - this.m_iRankingCellMakeCount + (i + 1), rankInfo.Name, rankInfo.Level, rankInfo.Score, (int)rankInfo.RankClass, this.m_bIsRaveUpRanking ? rankInfo.ArrRaveInfo : rankInfo.ArrHauseInfo, this.m_bIsRaveUpRanking ? rankInfo.aInfo.Id : (-1));
		}
		TweenPosition component = grid.GetComponent<TweenPosition>();
		float num2 = ((float)this.m_iRankingCellMakeCount - this.m_ViewCellCount) * this.m_fCellSzie_Y;
		grid.transform.localPosition = new Vector3(0f, num2, 0f);
		component.from = grid.transform.localPosition;
		component.to = component.from + new Vector3(0f, -this.m_ViewCellCount * this.m_fCellSzie_Y, 0f);
		component.duration = this.m_fMoveFrame;
		this.m_fFromPosition = component.from.y;
		this.m_fToPosition = component.to.y;
		this.m_fFinishPos = (float)this.m_iRankingCellMakeCount * -this.m_fCellSzie_Y;
	}

	// Token: 0x06000EB6 RID: 3766 RVA: 0x00069F60 File Offset: 0x00068160
	private void ViewOptimizationCheck(Transform grid, GameObject[] cells)
	{
		float num = grid.transform.localPosition.y - this.m_v2PanelSize.y;
		float num2 = grid.transform.localPosition.y + this.m_v2PanelSize.y;
		for (int i = 0; i < cells.Count<GameObject>(); i++)
		{
			GameObject gameObject = cells[i];
			float num3 = gameObject.transform.localPosition.y * -1f;
			if (num3 < num || num3 > num2)
			{
				gameObject.SetActive(false);
			}
			else if (!gameObject.activeSelf)
			{
				gameObject.SetActive(true);
				this.CellRepositionSetting(i, cells);
			}
		}
	}

	// Token: 0x06000EB7 RID: 3767 RVA: 0x0006A000 File Offset: 0x00068200
	private void CellRepositionSetting(int _index, GameObject[] Cells)
	{
		GameObject gameObject = Cells.FirstOrDefault((GameObject c) => !c.activeSelf);
		if (gameObject == null)
		{
			return;
		}
		gameObject.transform.localPosition = new Vector3(0f, Cells[_index].transform.localPosition.y + this.m_fCellSzie_Y, 0f);
		RankingCell component = Cells[_index].GetComponent<RankingCell>();
		int num = component.m_iRankingNum - 2;
		if (num < 0)
		{
			if (!this.m_bIsRaveUpRanking)
			{
				this.m_bEndAni = true;
			}
			return;
		}
		RankInfo rankInfo = (RankInfo)this.m_arrTargetRank[num];
		gameObject.GetComponent<RankingCell>().setUserIcon(rankInfo.Icon);
		gameObject.GetComponent<RankingCell>().Setting(this.m_bIsRaveUpRanking, component.m_iRankingNum - 1, rankInfo.Name, rankInfo.Level, rankInfo.Score, (int)rankInfo.RankClass, this.m_bIsRaveUpRanking ? rankInfo.ArrRaveInfo : rankInfo.ArrHauseInfo, this.m_bIsRaveUpRanking ? rankInfo.aInfo.Id : (-1));
	}

	// Token: 0x06000EB8 RID: 3768 RVA: 0x0006A118 File Offset: 0x00068318
	private void AutoMoveAction()
	{
		if (this.m_bCheckPos || this.m_tTargetGrid.transform.localPosition.y != this.m_fToPosition)
		{
			return;
		}
		if (this.m_bEndAni)
		{
			this.m_bEndAni = false;
			this.m_arrTargetRank = Singleton<GameManager>.instance.getRaveModeRank();
		}
		this.m_bCheckPos = true;
		this.SettingTweenPos();
	}

	// Token: 0x06000EB9 RID: 3769 RVA: 0x0006A178 File Offset: 0x00068378
	private void SettingTweenPos()
	{
		if (this.m_tTargetGrid.localPosition.y != this.m_fFinishPos)
		{
			TweenPosition component = this.m_tTargetGrid.GetComponent<TweenPosition>();
			Vector3 to = component.to;
			this.m_tTargetGrid.localPosition = to;
			component.from = to;
			component.to = new Vector3(0f, component.from.y + this.m_ViewCellCount * -this.m_fCellSzie_Y, 0f);
			component.ResetToBeginning();
			component.Play(true);
			this.m_fFromPosition = component.from.y;
			this.m_fToPosition = component.to.y;
			this.m_bCheckPos = false;
			return;
		}
		if (this.m_bIsRaveUpRanking)
		{
			base.Invoke("Blind", 4f);
			return;
		}
		this.m_bIsRaveUpRanking = true;
		this.m_RankingManagerScript.ChangeRanking();
		this.m_tTargetGrid = this.m_tRaveUpGrid;
		this.m_gTargetCell = this.m_gRaveUpCells;
		this.Init(this.m_tTargetGrid, this.m_gTargetCell);
		this.m_tHausMixForm.GetComponent<TweenPosition>().enabled = true;
		this.m_tRaveUpForm.GetComponent<TweenPosition>().enabled = true;
		float num = this.m_tRaveUpForm.GetComponent<TweenPosition>().duration + this.m_tRaveUpForm.GetComponent<TweenPosition>().delay;
		base.Invoke("RaveUpGridMove", num);
		this.m_bCheckPos = false;
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x0000CC01 File Offset: 0x0000AE01
	private void Blind()
	{
		this.m_RankingManagerScript.EndBlind();
		base.Invoke("CIScene", 2f);
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x0000B707 File Offset: 0x00009907
	private void CIScene()
	{
		Singleton<SceneSwitcher>.instance.LoadNextScene("CopyRight");
	}

	// Token: 0x06000EBC RID: 3772 RVA: 0x0000CC1E File Offset: 0x0000AE1E
	private void RaveUpGridMove()
	{
		this.m_tRaveUpGrid.GetComponent<TweenPosition>().enabled = true;
	}

	// Token: 0x06000EBD RID: 3773 RVA: 0x0000CC31 File Offset: 0x0000AE31
	private void Update()
	{
		if (!this.m_initSetting)
		{
			return;
		}
		this.AutoMoveAction();
		this.ViewOptimizationCheck(this.m_tTargetGrid, this.m_gTargetCell);
	}

	// Token: 0x06000EBE RID: 3774 RVA: 0x0000CC54 File Offset: 0x0000AE54
	private void GetRanking()
	{
		this.setRank();
	}

	// Token: 0x06000EBF RID: 3775 RVA: 0x0000CC5C File Offset: 0x0000AE5C
	private void StartAni()
	{
		this.m_tTargetGrid.GetComponent<TweenPosition>().enabled = true;
	}

	// Token: 0x04001007 RID: 4103
	private RankingManagerScript m_RankingManagerScript;

	// Token: 0x04001008 RID: 4104
	private float m_fCellSzie_Y = 117f;

	// Token: 0x04001009 RID: 4105
	private UIPanel m_Panel;

	// Token: 0x0400100A RID: 4106
	private Transform m_tHausMixGrid;

	// Token: 0x0400100B RID: 4107
	private Transform m_tRaveUpGrid;

	// Token: 0x0400100C RID: 4108
	private GameObject[] m_gHausMixCells;

	// Token: 0x0400100D RID: 4109
	private GameObject[] m_gRaveUpCells;

	// Token: 0x0400100E RID: 4110
	private Transform m_tHausMixForm;

	// Token: 0x0400100F RID: 4111
	private Transform m_tRaveUpForm;

	// Token: 0x04001010 RID: 4112
	private Transform m_tTargetGrid;

	// Token: 0x04001011 RID: 4113
	private GameObject[] m_gTargetCell;

	// Token: 0x04001012 RID: 4114
	private Vector2 m_v2PanelSize;

	// Token: 0x04001013 RID: 4115
	private int m_iRankingCellMakeCount = 15;

	// Token: 0x04001014 RID: 4116
	private int m_iRankingCellMaxCount = 30;

	// Token: 0x04001015 RID: 4117
	private float m_ViewCellCount = 5f;

	// Token: 0x04001016 RID: 4118
	private float m_fFromPosition;

	// Token: 0x04001017 RID: 4119
	private float m_fToPosition;

	// Token: 0x04001018 RID: 4120
	private float m_fMoveFrame = 1f;

	// Token: 0x04001019 RID: 4121
	private bool m_bCheckPos;

	// Token: 0x0400101A RID: 4122
	private float m_fFinishPos;

	// Token: 0x0400101B RID: 4123
	private bool m_bIsRaveUpRanking;

	// Token: 0x0400101C RID: 4124
	private bool m_initSetting;

	// Token: 0x0400101D RID: 4125
	private ArrayList m_arrTargetRank;

	// Token: 0x0400101E RID: 4126
	public bool m_bEndAni;
}
