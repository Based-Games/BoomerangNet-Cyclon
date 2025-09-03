using System;
using UnityEngine;

// Token: 0x020001C8 RID: 456
public class HouseMixMyRecord : MonoBehaviour
{
	// Token: 0x06000D5A RID: 3418 RVA: 0x0005EBAC File Offset: 0x0005CDAC
	private void Awake()
	{
		this.ClassPlus = new UISprite[2];
		Transform transform = base.transform.FindChild("4_Class").transform;
		if (base.transform.FindChild("localBG") != null)
		{
			this.LocalBG = base.transform.FindChild("localBG").gameObject;
		}
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
		for (int i = 0; i < 2; i++)
		{
			this.ClassPlus[i] = transform.FindChild("Sprite_plus" + (i + 1).ToString()).GetComponent<UISprite>();
		}
		this.ClassRank = transform.FindChild("Rank").GetComponent<UISprite>();
		this.ClassNone = transform.FindChild("Label_Rank").gameObject;
	}

	// Token: 0x06000D5B RID: 3419 RVA: 0x0000BE0F File Offset: 0x0000A00F
	private void Start()
	{
		if (this.LocalBG != null)
		{
			this.LocalBG.SetActive(false);
		}
	}

	// Token: 0x06000D5C RID: 3420 RVA: 0x0005EDA0 File Offset: 0x0005CFA0
	public void ValueSetting(int bs, int mx, int acc, int rank, bool _pf, bool all, string trophy = "")
	{
		Singleton<GameManager>.instance.UserData.BestScore = bs;
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
			this.Pf.transform.localScale = Vector3.one;
		}
		else
		{
			this.Pf.GetComponent<UISprite>().spriteName = "None";
			this.Pf.GetComponent<UISprite>().MakePixelPerfect();
			this.Pf.transform.localScale = Vector3.one * 2f;
		}
		if (all)
		{
			this.AllCombo.GetComponent<UISprite>().spriteName = "allcombo";
			this.AllCombo.GetComponent<UISprite>().MakePixelPerfect();
			this.AllCombo.transform.localScale = Vector3.one;
		}
		else
		{
			this.AllCombo.GetComponent<UISprite>().spriteName = "None";
			this.AllCombo.GetComponent<UISprite>().MakePixelPerfect();
			this.AllCombo.transform.localScale = Vector3.one * 2f;
		}
		if (trophy != string.Empty)
		{
			if (this.tr != null)
			{
				this.tr.GetComponent<UISprite>().spriteName = trophy;
				this.tr.GetComponent<UISprite>().MakePixelPerfect();
				this.tr.GetComponent<UISprite>().transform.localScale = new Vector3(0.9f, 0.9f, 1f);
			}
		}
		else if (this.tr != null)
		{
			this.tr.GetComponent<UISprite>().spriteName = "None";
			this.tr.GetComponent<UISprite>().MakePixelPerfect();
			this.tr.transform.localScale = Vector3.one * 2f;
		}
	}

	// Token: 0x06000D5D RID: 3421 RVA: 0x0005F18C File Offset: 0x0005D38C
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

	// Token: 0x06000D5E RID: 3422 RVA: 0x0005F244 File Offset: 0x0005D444
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
				text1.GetComponent<UILabel>().spacingX = 1 + num2 * 3;
				text1.GetComponent<UILabel>().text = num3.ToString();
			}
		}
		if (_TweenValue >= 1f)
		{
			State = false;
		}
	}

	// Token: 0x06000D5F RID: 3423 RVA: 0x0005F330 File Offset: 0x0005D530
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
		this.ClassRank.transform.localScale = Vector3.one * 2f;
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
				this.ClassPlus[j].transform.localScale = Vector3.one * 2f;
			}
		}
	}

	// Token: 0x06000D60 RID: 3424 RVA: 0x0000BE2E File Offset: 0x0000A02E
	private void Update()
	{
		this.UpdateAni();
	}

	// Token: 0x04000D8B RID: 3467
	private UISprite[] ClassPlus;

	// Token: 0x04000D8C RID: 3468
	public GameObject LocalBG;

	// Token: 0x04000D8D RID: 3469
	private UILabel BestScore;

	// Token: 0x04000D8E RID: 3470
	private UILabel MaxCombo;

	// Token: 0x04000D8F RID: 3471
	private UILabel Accuracy;

	// Token: 0x04000D90 RID: 3472
	private UILabel AccuracyPer;

	// Token: 0x04000D91 RID: 3473
	private UISprite ClassRank;

	// Token: 0x04000D92 RID: 3474
	private TweenPosition BestScoreAni;

	// Token: 0x04000D93 RID: 3475
	private TweenPosition MaxComboAni;

	// Token: 0x04000D94 RID: 3476
	private TweenPosition AccuracyAni;

	// Token: 0x04000D95 RID: 3477
	private GameObject ClassNone;

	// Token: 0x04000D96 RID: 3478
	private GameObject Pf;

	// Token: 0x04000D97 RID: 3479
	private GameObject AllCombo;

	// Token: 0x04000D98 RID: 3480
	private GameObject tr;

	// Token: 0x04000D99 RID: 3481
	private int BestScoreValue;

	// Token: 0x04000D9A RID: 3482
	private int MaxComboValue;

	// Token: 0x04000D9B RID: 3483
	private int AccuracyValue;

	// Token: 0x04000D9C RID: 3484
	private bool AniState;

	// Token: 0x04000D9D RID: 3485
	private bool BestScoreAniState;

	// Token: 0x04000D9E RID: 3486
	private bool MaxComboAniState;

	// Token: 0x04000D9F RID: 3487
	private bool AccuracyAniState;
}
