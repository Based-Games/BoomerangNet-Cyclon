using System;
using UnityEngine;

// Token: 0x02000191 RID: 401
public class ClubTourMyRecord : MonoBehaviour
{
	// Token: 0x06000BEE RID: 3054 RVA: 0x00054858 File Offset: 0x00052A58
	private void Awake()
	{
		Transform transform = base.transform.FindChild("4_Class").transform;
		this.BestScore = base.transform.FindChild("Label_BESTSCORE").GetComponent<UILabel>();
		this.MaxCombo = base.transform.FindChild("Label_MAXCOMBO").GetComponent<UILabel>();
		this.Accuracy = base.transform.FindChild("Label_ACCURRCY").GetComponent<UILabel>();
		this.AccuracyPer = base.transform.FindChild("Label_ACCURRCY_per").GetComponent<UILabel>();
		this.BestScoreAni = base.transform.FindChild("BestScoreNumberAni").GetComponent<TweenPosition>();
		this.MaxComboAni = base.transform.FindChild("MaxComboNumberAni").GetComponent<TweenPosition>();
		this.AccuracyAni = base.transform.FindChild("AccurrcyNumberAni").GetComponent<TweenPosition>();
		this.Pf = base.transform.FindChild("Sprite_pf").gameObject;
		this.AllCombo = base.transform.FindChild("Sprite_allcombo").gameObject;
		if (base.transform.FindChild("Sprite_trophy") != null)
		{
			this.tr = base.transform.FindChild("Sprite_trophy").gameObject;
		}
		this.ClassPlus = new UISprite[2];
		for (int i = 0; i < 2; i++)
		{
			this.ClassPlus[i] = transform.FindChild("Sprite_plus" + (i + 1).ToString()).GetComponent<UISprite>();
		}
		this.ClassRank = transform.FindChild("Rank").GetComponent<UISprite>();
		this.ClassNone = transform.FindChild("Label_Rank").gameObject;
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x00054A18 File Offset: 0x00052C18
	public void ValueSetting(int bs, int mx, int acc, int rank, bool _pf, bool all, string trophy = "")
	{
		if (bs > 0)
		{
			this.AniState = true;
			this.BestScore.text = "0";
			this.BestScoreAni.transform.localPosition = Vector3.zero;
			this.BestScoreAni.ResetToBeginning();
			this.BestScoreValue = bs;
			this.BestScoreAniState = true;
			this.BestScoreAni.Play(true);
		}
		else
		{
			this.AniState = false;
			this.BestScore.text = "-";
		}
		if (mx > 0)
		{
			this.MaxCombo.text = "0";
			this.MaxComboAni.transform.localPosition = Vector3.zero;
			this.MaxComboAni.ResetToBeginning();
			this.MaxComboValue = mx;
			this.MaxComboAniState = true;
			this.MaxComboAni.Play(true);
		}
		else
		{
			this.MaxCombo.text = "-";
		}
		if (acc > 0)
		{
			this.Accuracy.gameObject.SetActive(true);
			this.AccuracyPer.text = "%";
			this.Accuracy.text = "0";
			this.AccuracyAni.transform.localPosition = Vector3.zero;
			this.AccuracyAni.ResetToBeginning();
			this.AccuracyValue = acc;
			this.AccuracyAniState = true;
			this.AccuracyAni.Play(true);
		}
		else
		{
			this.Accuracy.gameObject.SetActive(false);
			this.AccuracyPer.text = "-";
		}
		if (rank >= 0)
		{
			this.ClassNone.gameObject.SetActive(false);
			this.ClassRank.gameObject.SetActive(true);
			this.MyRecordRankSetting(rank);
		}
		else
		{
			this.ClassPlus[0].gameObject.SetActive(false);
			this.ClassPlus[1].gameObject.SetActive(false);
			this.ClassRank.gameObject.SetActive(false);
			this.ClassNone.gameObject.SetActive(true);
		}
		if (_pf)
		{
			this.Pf.GetComponent<UISprite>().spriteName = "perfect";
			this.Pf.GetComponent<UISprite>().MakePixelPerfect();
			this.Pf.transform.localScale = new Vector3(0.55f, 0.55f, 1f);
		}
		else
		{
			this.Pf.GetComponent<UISprite>().spriteName = "None";
			this.Pf.GetComponent<UISprite>().MakePixelPerfect();
		}
		if (all)
		{
			this.AllCombo.GetComponent<UISprite>().spriteName = "allcombo";
			this.AllCombo.GetComponent<UISprite>().MakePixelPerfect();
			this.AllCombo.transform.localScale = new Vector3(0.55f, 0.55f, 1f);
		}
		else
		{
			this.AllCombo.GetComponent<UISprite>().spriteName = "None";
			this.AllCombo.GetComponent<UISprite>().MakePixelPerfect();
		}
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x00054D0C File Offset: 0x00052F0C
	private void UpdateAni()
	{
		if (!this.AniState)
		{
			return;
		}
		this.PlayAni(this.BestScoreAniState, this.BestScoreAni.transform.localPosition.x, this.BestScoreValue, this.BestScore.gameObject);
		this.PlayAni(this.MaxComboAniState, this.MaxComboAni.transform.localPosition.x, this.MaxComboValue, this.MaxCombo.gameObject);
		this.PlayAni(this.AccuracyAniState, this.AccuracyAni.transform.localPosition.x, this.AccuracyValue, this.Accuracy.gameObject);
	}

	// Token: 0x06000BF1 RID: 3057 RVA: 0x00054DC4 File Offset: 0x00052FC4
	private void PlayAni(bool State, float _TweenValue, int value1, GameObject text1 = null)
	{
		if (!State)
		{
			return;
		}
		if (text1.GetComponent<ImageFontLabel>() != null)
		{
			if (text1 != null)
			{
				text1.GetComponent<ImageFontLabel>().text = ((int)((float)value1 * _TweenValue)).ToString();
			}
		}
		else if (text1.GetComponent<UILabel>() != null)
		{
			int num = 4;
			int num2 = 0;
			int num3 = (int)((float)value1 * _TweenValue);
			if (text1 != null)
			{
				for (int i = 0; i < num3.ToString().Length; i++)
				{
					if (num3.ToString()[i] == '1')
					{
						num2++;
					}
				}
				if (num2 > num)
				{
					num2 = num;
				}
				text1.GetComponent<UILabel>().spacingX = 1 + num2 * 1;
				text1.GetComponent<UILabel>().text = num3.ToString();
			}
		}
		if (_TweenValue >= 1f)
		{
			State = false;
		}
	}

	// Token: 0x06000BF2 RID: 3058 RVA: 0x00054EB0 File Offset: 0x000530B0
	private void MyRecordRankSetting(int rank)
	{
		int num = 0;
		int num2 = 0;
		if (rank >= 4 && rank <= 6)
		{
			this.ClassRank.spriteName = "MyRecord_Rank_" + 4.ToString();
			num = rank - 4;
			num2 = 4;
		}
		else if (rank >= 7 && rank <= 9)
		{
			this.ClassRank.spriteName = "MyRecord_Rank_" + 7.ToString();
			num = rank - 7;
			num2 = 7;
		}
		else
		{
			this.ClassRank.spriteName = "MyRecord_Rank_" + rank.ToString();
		}
		this.ClassRank.MakePixelPerfect();
		if (this.ClassPlus != null)
		{
			for (int i = 0; i < this.ClassPlus.Length; i++)
			{
				this.ClassPlus[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < num; j++)
			{
				this.ClassPlus[j].gameObject.SetActive(true);
				this.ClassPlus[j].spriteName = "Plus_" + num2.ToString();
				this.ClassPlus[j].MakePixelPerfect();
			}
		}
	}

	// Token: 0x06000BF3 RID: 3059 RVA: 0x0000B061 File Offset: 0x00009261
	private void Update()
	{
		this.UpdateAni();
	}

	// Token: 0x04000BA1 RID: 2977
	public UISprite[] ClassPlus;

	// Token: 0x04000BA2 RID: 2978
	private UILabel BestScore;

	// Token: 0x04000BA3 RID: 2979
	private UILabel MaxCombo;

	// Token: 0x04000BA4 RID: 2980
	private UILabel Accuracy;

	// Token: 0x04000BA5 RID: 2981
	private UILabel AccuracyPer;

	// Token: 0x04000BA6 RID: 2982
	private UISprite ClassRank;

	// Token: 0x04000BA7 RID: 2983
	private TweenPosition BestScoreAni;

	// Token: 0x04000BA8 RID: 2984
	private TweenPosition MaxComboAni;

	// Token: 0x04000BA9 RID: 2985
	private TweenPosition AccuracyAni;

	// Token: 0x04000BAA RID: 2986
	private GameObject ClassNone;

	// Token: 0x04000BAB RID: 2987
	private GameObject Pf;

	// Token: 0x04000BAC RID: 2988
	private GameObject AllCombo;

	// Token: 0x04000BAD RID: 2989
	private GameObject tr;

	// Token: 0x04000BAE RID: 2990
	[HideInInspector]
	public GameObject m_gLoading;

	// Token: 0x04000BAF RID: 2991
	private int BestScoreValue;

	// Token: 0x04000BB0 RID: 2992
	private int MaxComboValue;

	// Token: 0x04000BB1 RID: 2993
	private int AccuracyValue;

	// Token: 0x04000BB2 RID: 2994
	private bool AniState;

	// Token: 0x04000BB3 RID: 2995
	private bool BestScoreAniState;

	// Token: 0x04000BB4 RID: 2996
	private bool MaxComboAniState;

	// Token: 0x04000BB5 RID: 2997
	private bool AccuracyAniState;
}
