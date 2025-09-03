using System;
using UnityEngine;

// Token: 0x020001E6 RID: 486
public class ResultMyRecord : MonoBehaviour
{
	// Token: 0x06000E16 RID: 3606 RVA: 0x000651E0 File Offset: 0x000633E0
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

	// Token: 0x06000E17 RID: 3607 RVA: 0x00065338 File Offset: 0x00063538
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
		if (TrophyName != string.Empty)
		{
			this.MyRecordTrophy.spriteName = TrophyName;
			this.MyRecordTrophy.MakePixelPerfect();
			this.MyRecordTrophy.transform.localScale = new Vector3(0.9f, 0.9f, 1f);
		}
		else
		{
			this.MyRecordTrophy.spriteName = "None";
			this.MyRecordTrophy.MakePixelPerfect();
			this.MyRecordTrophy.transform.localScale = Vector3.one * 2f;
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

	// Token: 0x06000E18 RID: 3608 RVA: 0x00056CD0 File Offset: 0x00054ED0
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

	// Token: 0x04000EFC RID: 3836
	private UILabel MyRecord_BestScore;

	// Token: 0x04000EFD RID: 3837
	private int MyRecord_BestScoreValue;

	// Token: 0x04000EFE RID: 3838
	private UILabel MyRecord_MaxCombo;

	// Token: 0x04000EFF RID: 3839
	private int MyRecord_MaxComboValue;

	// Token: 0x04000F00 RID: 3840
	private UILabel MyRecord_Accuracy;

	// Token: 0x04000F01 RID: 3841
	private int MyRecord_AccuracyValue;

	// Token: 0x04000F02 RID: 3842
	private UISprite MyRecord_Class;

	// Token: 0x04000F03 RID: 3843
	private int MyRecordClassValue;

	// Token: 0x04000F04 RID: 3844
	private GameObject[] MyRecord_Class_Plus;

	// Token: 0x04000F05 RID: 3845
	private UISprite MyRecordAllcombo;

	// Token: 0x04000F06 RID: 3846
	private UISprite MyRecordPerfect;

	// Token: 0x04000F07 RID: 3847
	private UISprite MyRecordTrophy;
}
