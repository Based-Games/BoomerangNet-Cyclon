using System;
using UnityEngine;

// Token: 0x02000199 RID: 409
public class ClubTourResultMyRecord : MonoBehaviour
{
	// Token: 0x06000C33 RID: 3123 RVA: 0x00056924 File Offset: 0x00054B24
	private void Awake()
	{
		this.MyRecord_BestScore = base.transform.FindChild("BestScore").FindChild("Label_NUM").GetComponent<UILabel>();
		this.MyRecord_MaxCombo = base.transform.FindChild("MaxCombo").FindChild("Label_NUM").GetComponent<UILabel>();
		this.MyRecord_Accuracy = base.transform.FindChild("Accuracy").FindChild("Label_NUM").GetComponent<UILabel>();
		Transform transform = base.transform.FindChild("Class").FindChild("rank");
		this.MyRecord_Class = transform.FindChild("Sprite_Rank").GetComponent<UISprite>();
		this.MyRecord_Class_Plus = new GameObject[2];
		for (int i = 0; i < this.MyRecord_Class_Plus.Length; i++)
		{
			this.MyRecord_Class_Plus[i] = transform.FindChild("Plus").FindChild("Sprite_Plus_" + (i + 1).ToString()).gameObject;
		}
		Transform transform2 = base.transform.FindChild("MarkIcon");
		this.MyRecordAllcombo = transform2.FindChild("allcombo").GetComponent<UISprite>();
		this.MyRecordPerfect = transform2.FindChild("pf").GetComponent<UISprite>();
		this.MyRecordTrophy = transform2.FindChild("tr").GetComponent<UISprite>();
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x00056A7C File Offset: 0x00054C7C
	public void MyRecordSetting(int _BestScore, int _MaxCombo, int _Accuracy, int _Class, bool AllComboState, bool PerfectState, string TrophyName = "")
	{
		this.MyRecord_BestScoreValue = _BestScore;
		this.MyRecord_MaxComboValue = _MaxCombo;
		this.MyRecord_AccuracyValue = _Accuracy;
		this.MyRecordClassValue = _Class;
		if (!AllComboState)
		{
			this.MyRecordAllcombo.spriteName = "None";
			this.MyRecordAllcombo.MakePixelPerfect();
			this.MyRecordAllcombo.transform.localScale = Vector3.one * 2f;
		}
		else
		{
			this.MyRecordAllcombo.spriteName = "allcombo";
			this.MyRecordAllcombo.MakePixelPerfect();
		}
		if (!PerfectState)
		{
			this.MyRecordPerfect.spriteName = "None";
			this.MyRecordPerfect.MakePixelPerfect();
			this.MyRecordPerfect.transform.localScale = Vector3.one * 2f;
		}
		else
		{
			this.MyRecordPerfect.spriteName = "perfect";
			this.MyRecordPerfect.MakePixelPerfect();
		}
		if (this.MyRecordClassValue >= 0)
		{
			this.RankImageSetting(this.MyRecordClassValue, this.MyRecord_Class, this.MyRecord_Class_Plus, "MyRecord_Rank_");
			this.MyRecord_Class.MakePixelPerfect();
			this.MyRecord_Class.transform.localScale = Vector3.one * 2f;
		}
		else
		{
			for (int i = 0; i < this.MyRecord_Class_Plus.Length; i++)
			{
				this.MyRecord_Class_Plus[i].SetActive(false);
			}
			this.MyRecord_Class.spriteName = "None";
			this.MyRecord_Class.MakePixelPerfect();
			this.MyRecord_Class.transform.localScale = Vector3.one * 2f;
		}
		if (this.MyRecord_BestScoreValue >= 0)
		{
			this.MyRecord_BestScore.text = this.MyRecord_BestScoreValue.ToString();
		}
		else
		{
			this.MyRecord_BestScore.text = "-";
		}
		if (this.MyRecord_MaxComboValue >= 0)
		{
			this.MyRecord_MaxCombo.text = this.MyRecord_MaxComboValue.ToString();
		}
		else
		{
			this.MyRecord_MaxCombo.text = "-";
		}
		if (this.MyRecord_AccuracyValue >= 0)
		{
			this.MyRecord_Accuracy.text = this.MyRecord_AccuracyValue.ToString() + "%";
		}
		else
		{
			this.MyRecord_Accuracy.text = "-";
		}
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x00056CD0 File Offset: 0x00054ED0
	private void RankImageSetting(int RankIndex, UISprite MainSprite, GameObject[] Plus = null, string SpriteName = "Result_Rank_")
	{
		int num = 0;
		int num2 = 0;
		if (RankIndex >= 4 && RankIndex <= 6)
		{
			MainSprite.spriteName = SpriteName + 4.ToString();
			num2 = 4;
			num = RankIndex - 4;
		}
		else if (RankIndex >= 7 && RankIndex <= 9)
		{
			MainSprite.spriteName = SpriteName + 7.ToString();
			num2 = 7;
			num = RankIndex - 7;
		}
		else
		{
			MainSprite.spriteName = SpriteName + RankIndex.ToString();
		}
		if (Plus != null)
		{
			for (int i = 0; i < Plus.Length; i++)
			{
				Plus[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < num; j++)
			{
				Plus[j].gameObject.SetActive(true);
				if (SpriteName == "MyRecord_Rank_")
				{
					Plus[j].GetComponent<UISprite>().spriteName = "Plus_" + num2.ToString();
					Plus[j].GetComponent<UISprite>().MakePixelPerfect();
					Plus[j].transform.localScale = Vector3.one * 2f;
				}
			}
		}
	}

	// Token: 0x04000C20 RID: 3104
	private UILabel MyRecord_BestScore;

	// Token: 0x04000C21 RID: 3105
	private int MyRecord_BestScoreValue;

	// Token: 0x04000C22 RID: 3106
	private UILabel MyRecord_MaxCombo;

	// Token: 0x04000C23 RID: 3107
	private int MyRecord_MaxComboValue;

	// Token: 0x04000C24 RID: 3108
	private UILabel MyRecord_Accuracy;

	// Token: 0x04000C25 RID: 3109
	private int MyRecord_AccuracyValue;

	// Token: 0x04000C26 RID: 3110
	private UISprite MyRecord_Class;

	// Token: 0x04000C27 RID: 3111
	private int MyRecordClassValue;

	// Token: 0x04000C28 RID: 3112
	private GameObject[] MyRecord_Class_Plus;

	// Token: 0x04000C29 RID: 3113
	private UISprite MyRecordAllcombo;

	// Token: 0x04000C2A RID: 3114
	private UISprite MyRecordPerfect;

	// Token: 0x04000C2B RID: 3115
	private UISprite MyRecordTrophy;
}
