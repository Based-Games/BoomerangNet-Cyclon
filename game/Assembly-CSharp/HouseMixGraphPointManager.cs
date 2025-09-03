using System;
using UnityEngine;

// Token: 0x020001BE RID: 446
public class HouseMixGraphPointManager : MonoBehaviour
{
	// Token: 0x06000D28 RID: 3368 RVA: 0x0005BA00 File Offset: 0x00059C00
	private void Awake()
	{
		this.P_ACC = base.transform.FindChild("Point_acc").GetComponent<TweenPosition>();
		this.P_CLASS = base.transform.FindChild("Point_class").GetComponent<TweenPosition>();
		this.P_SCORE = base.transform.FindChild("Point_score").GetComponent<TweenPosition>();
		this.P_MAXCOMBO = base.transform.FindChild("Point_maxcombo").GetComponent<TweenPosition>();
	}

	// Token: 0x06000D29 RID: 3369 RVA: 0x0005BA7C File Offset: 0x00059C7C
	private void Start()
	{
		this.m_v3AccOriginTo = this.P_ACC.to;
		this.m_v3ScoreOriginTo = this.P_SCORE.to;
		this.m_v3MaxComboOriginTo = this.P_MAXCOMBO.to;
		float num = 0.5f;
		this.P_MAXCOMBO.duration = (this.P_SCORE.duration = (this.P_CLASS.duration = (this.P_ACC.duration = num)));
	}

	// Token: 0x06000D2A RID: 3370 RVA: 0x0005BAF8 File Offset: 0x00059CF8
	public void SetValue(PatternScoreInfo pInfo, DiscInfo dInfo, PTLEVEL pLv)
	{
		this.P_ACC.to = this.m_v3AccOriginTo;
		this.P_SCORE.to = this.m_v3ScoreOriginTo;
		this.P_MAXCOMBO.to = this.m_v3MaxComboOriginTo;
		if (pInfo.RankClass == GRADE.NON)
		{
			return;
		}
		this.MaxComboMaxValue = dInfo.DicPtInfo[pLv].iMaxCombo;
		this.ScoreMaxValue = dInfo.DicPtInfo[pLv].iMaxNote * GameData.Judgment_Score[4];
		for (int i = 0; i < this.Points.Length; i++)
		{
			UISprite uisprite = this.Points[i];
			int num = (int)pLv;
			uisprite.spriteName = num.ToString();
		}
		if (0 >= this.AccMaxValue)
		{
			this.AccMaxValue = 1;
		}
		if (0 >= this.ScoreMaxValue)
		{
			this.ScoreMaxValue = 1;
		}
		if (0 >= this.MaxComboMaxValue)
		{
			this.MaxComboMaxValue = 1;
		}
		if (0 >= this.RankMaxValue)
		{
			this.RankMaxValue = 1;
		}
		float num2 = (float)pInfo.Accuracy / (float)this.AccMaxValue;
		float num3 = (float)pInfo.NoteScore / (float)this.ScoreMaxValue;
		float num4 = (float)pInfo.RealCombo / (float)this.MaxComboMaxValue;
		float num5 = (float)pInfo.RankClass / (float)this.RankMaxValue;
		if (num2 > 1f)
		{
		}
		if (num3 > 1f)
		{
			num3 = 1f;
		}
		if (num4 > 1f)
		{
			num4 = 1f;
		}
		if (num5 > 1f)
		{
			num5 = 1f;
		}
		this.P_ACC.transform.localPosition = this.P_ACC.from;
		this.P_CLASS.transform.localPosition = this.P_CLASS.from;
		this.P_SCORE.transform.localPosition = this.P_SCORE.from;
		this.P_MAXCOMBO.transform.localPosition = this.P_MAXCOMBO.from;
		this.P_ACC.to = new Vector3(this.P_ACC.to.x * num5, this.P_ACC.to.y * num5, this.P_ACC.from.z);
		this.P_SCORE.to = new Vector3(this.P_SCORE.to.x * num3, this.P_SCORE.to.y * num3, this.P_SCORE.from.z);
		this.P_MAXCOMBO.to = new Vector3(this.P_MAXCOMBO.to.x * num4, this.P_MAXCOMBO.to.y * num4, this.P_MAXCOMBO.from.z);
		base.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
		base.GetComponent<TweenRotation>().ResetToBeginning();
		base.GetComponent<TweenRotation>().Play(true);
		this.P_ACC.ResetToBeginning();
		this.P_ACC.Play(true);
		this.P_CLASS.ResetToBeginning();
		this.P_CLASS.Play(true);
		this.P_SCORE.ResetToBeginning();
		this.P_SCORE.Play(true);
		this.P_MAXCOMBO.ResetToBeginning();
		this.P_MAXCOMBO.Play(true);
		for (int j = 0; j < this.m_HouseMixGraphPointLine.Length; j++)
		{
			this.m_HouseMixGraphPointLine[j].StartLineUpdate();
		}
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x0005BE78 File Offset: 0x0005A078
	private void DefualtSetting()
	{
		this.MaxComboMaxValue = 500;
		this.ScoreMaxValue = 100000000;
		for (int i = 0; i < this.Points.Length; i++)
		{
			this.Points[i].spriteName = 1.ToString();
		}
		float num = 100f / (float)this.AccMaxValue;
		float num2 = 100000000f / (float)this.ScoreMaxValue;
		float num3 = 500f / (float)this.MaxComboMaxValue;
		if (num > 1f)
		{
			num = 1f;
		}
		if (num2 > 1f)
		{
			num2 = 1f;
		}
		if (num3 > 1f)
		{
			num3 = 1f;
		}
		this.P_ACC.transform.localPosition = this.P_ACC.from;
		this.P_SCORE.transform.localPosition = this.P_SCORE.from;
		this.P_MAXCOMBO.transform.localPosition = this.P_MAXCOMBO.from;
		this.P_ACC.to = new Vector3(this.P_ACC.to.x * num, this.P_ACC.to.y * num, this.P_ACC.from.z);
		this.P_SCORE.to = new Vector3(this.P_SCORE.to.x * num2, this.P_SCORE.to.y * num2, this.P_SCORE.from.z);
		this.P_MAXCOMBO.to = new Vector3(this.P_MAXCOMBO.to.x * num3, this.P_MAXCOMBO.to.y * num3, this.P_MAXCOMBO.from.z);
		base.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
		base.GetComponent<TweenRotation>().ResetToBeginning();
		base.GetComponent<TweenRotation>().Play(true);
		this.P_ACC.ResetToBeginning();
		this.P_ACC.Play(true);
		this.P_SCORE.ResetToBeginning();
		this.P_SCORE.Play(true);
		this.P_MAXCOMBO.ResetToBeginning();
		this.P_MAXCOMBO.Play(true);
		for (int j = 0; j < this.m_HouseMixGraphPointLine.Length; j++)
		{
			this.m_HouseMixGraphPointLine[j].StartLineUpdate();
		}
	}

	// Token: 0x04000D24 RID: 3364
	public UISprite[] Points;

	// Token: 0x04000D25 RID: 3365
	public HouseMixGraphPointLine[] m_HouseMixGraphPointLine;

	// Token: 0x04000D26 RID: 3366
	private TweenPosition P_ACC;

	// Token: 0x04000D27 RID: 3367
	private TweenPosition P_CLASS;

	// Token: 0x04000D28 RID: 3368
	private TweenPosition P_SCORE;

	// Token: 0x04000D29 RID: 3369
	private TweenPosition P_MAXCOMBO;

	// Token: 0x04000D2A RID: 3370
	private float MaxPintValue = 90f;

	// Token: 0x04000D2B RID: 3371
	private int AccMaxValue = 100;

	// Token: 0x04000D2C RID: 3372
	private int ScoreMaxValue = 100000000;

	// Token: 0x04000D2D RID: 3373
	private int MaxComboMaxValue = 10000;

	// Token: 0x04000D2E RID: 3374
	private int RankMaxValue = 9;

	// Token: 0x04000D2F RID: 3375
	private int accvalue;

	// Token: 0x04000D30 RID: 3376
	private int scorevalue;

	// Token: 0x04000D31 RID: 3377
	private int maxcombovalue;

	// Token: 0x04000D32 RID: 3378
	private int rankvalue;

	// Token: 0x04000D33 RID: 3379
	private Vector3 m_v3AccOriginTo;

	// Token: 0x04000D34 RID: 3380
	private Vector3 m_v3ScoreOriginTo;

	// Token: 0x04000D35 RID: 3381
	private Vector3 m_v3MaxComboOriginTo;
}
