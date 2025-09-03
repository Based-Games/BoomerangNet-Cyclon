using System;
using UnityEngine;

// Token: 0x020001EB RID: 491
public class ResultRankingManager : MonoBehaviour
{
	// Token: 0x06000E3E RID: 3646 RVA: 0x0000C6A6 File Offset: 0x0000A8A6
	private void Awake()
	{
		this.RankCellPrefab = Resources.Load("Prefab/HouseMixResult/ResultRankingCell") as GameObject;
		this.CreateCell();
	}

	// Token: 0x06000E3F RID: 3647 RVA: 0x00066894 File Offset: 0x00064A94
	private void CreateCell()
	{
		for (int i = 0; i < this.RankingCount; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.RankCellPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			gameObject.transform.parent = this.Grid;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = new Vector3(0f, (float)i * -this.CellSize.y, 0f);
			if (i < 9)
			{
				gameObject.GetComponent<ResultRankingCell>().UserRankingNum.text = "0" + (i + 1).ToString();
			}
			else
			{
				gameObject.GetComponent<ResultRankingCell>().UserRankingNum.text = (i + 1).ToString();
			}
			gameObject.GetComponent<ResultRankingCell>().UserName.text = "Test_" + (i + 1).ToString();
			gameObject.GetComponent<ResultRankingCell>().m_UIScroll = this.UIScroll;
			gameObject.GetComponent<ResultRankingCell>().m_ResultRankingManager = base.GetComponent<ResultRankingManager>();
			if (this.MyRank - 1 == i)
			{
				gameObject.GetComponent<ResultRankingCell>().bg.spriteName = "Result_RankBG_Select";
				gameObject.GetComponent<ResultRankingCell>().UserImage.spriteName = "MyPic";
				gameObject.GetComponent<ResultRankingCell>().UserImage.MakePixelPerfect();
				gameObject.GetComponent<ResultRankingCell>().UserImage.transform.localScale = new Vector3(1f, 1.2f, 1f);
				gameObject.GetComponent<ResultRankingCell>().UserName.text = "NURIJOY";
			}
		}
		this.UIScroll.CellsSetting(0);
		Vector3 localPosition = this.UIScroll.transform.localPosition;
		this.UIScroll.transform.localPosition = new Vector3(localPosition.x, this.CellSize.y * (float)this.RankingCount - this.m_UIPanel.GetViewSize().y, localPosition.z);
		base.Invoke("RankEff", 1f);
	}

	// Token: 0x06000E40 RID: 3648 RVA: 0x00066AB4 File Offset: 0x00064CB4
	private void RankEff()
	{
		Vector3 localPosition = this.UIScroll.transform.localPosition;
		Vector3 zero = Vector3.zero;
		if (this.MyRank + 5 >= this.RankingCount)
		{
			zero = new Vector3(localPosition.x, this.CellSize.y * (float)this.RankingCount - this.m_UIPanel.GetViewSize().y, localPosition.z);
		}
		else if (this.MyRank - 5 <= 0)
		{
			zero = new Vector3(localPosition.x, 0f, localPosition.z);
		}
		else
		{
			zero = new Vector3(localPosition.x, this.CellSize.y * (float)(this.MyRank + 5) - this.m_UIPanel.GetViewSize().y, localPosition.z);
		}
		this.UIScroll.transform.localPosition = new Vector3(localPosition.x, this.CellSize.y * (float)this.RankingCount - this.m_UIPanel.GetViewSize().y, localPosition.z);
		this.UIScroll.GetComponent<TweenPosition>().from = this.UIScroll.transform.localPosition;
		this.UIScroll.GetComponent<TweenPosition>().to = zero;
		this.UIScroll.GetComponent<TweenPosition>().enabled = true;
	}

	// Token: 0x06000E41 RID: 3649 RVA: 0x0000C6C3 File Offset: 0x0000A8C3
	private void StartAutoScroll()
	{
		this.AutoScroll = true;
	}

	// Token: 0x06000E42 RID: 3650 RVA: 0x00066C24 File Offset: 0x00064E24
	private void AutoScrollMove()
	{
		if (!this.AutoScroll)
		{
			return;
		}
		Vector3 localPosition = this.UIScroll.transform.localPosition;
		this.UIScroll.transform.localPosition = new Vector3(localPosition.x, localPosition.y + this.DirValue * (this.AutoScrollSpeed * Time.deltaTime), localPosition.z);
		if (localPosition.y < 0f)
		{
			this.DirValue = 1f;
		}
		else if (localPosition.y > this.CellSize.y * (float)this.RankingCount - this.m_UIPanel.GetViewSize().y)
		{
			this.DirValue = -1f;
		}
	}

	// Token: 0x06000E43 RID: 3651 RVA: 0x00066CEC File Offset: 0x00064EEC
	private void CheckMoveFrame()
	{
		if (this.MoveStartState)
		{
			return;
		}
		if (this.UIScroll.transform.localPosition.y == this.UIScroll.GetComponent<TweenPosition>().to.y)
		{
			this.MoveStartState = true;
			base.Invoke("StartAutoScroll", 2f);
		}
	}

	// Token: 0x06000E44 RID: 3652 RVA: 0x0000C6CC File Offset: 0x0000A8CC
	private void Update()
	{
		this.AutoScrollMove();
		this.CheckMoveFrame();
	}

	// Token: 0x04000F42 RID: 3906
	public UIPanel m_UIPanel;

	// Token: 0x04000F43 RID: 3907
	public UIScroll UIScroll;

	// Token: 0x04000F44 RID: 3908
	public Transform Grid;

	// Token: 0x04000F45 RID: 3909
	public Vector3 CellSize;

	// Token: 0x04000F46 RID: 3910
	private GameObject RankCellPrefab;

	// Token: 0x04000F47 RID: 3911
	private int RankingCount = 100;

	// Token: 0x04000F48 RID: 3912
	[HideInInspector]
	public bool AutoScroll;

	// Token: 0x04000F49 RID: 3913
	private float DirValue = -1f;

	// Token: 0x04000F4A RID: 3914
	private float AutoScrollSpeed = 70f;

	// Token: 0x04000F4B RID: 3915
	private int MyRank = 15;

	// Token: 0x04000F4C RID: 3916
	private bool MoveStartState;
}
