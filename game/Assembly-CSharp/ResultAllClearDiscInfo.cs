using System;
using UnityEngine;

// Token: 0x020001DB RID: 475
public class ResultAllClearDiscInfo : MonoBehaviour
{
	// Token: 0x06000DCD RID: 3533 RVA: 0x00063064 File Offset: 0x00061264
	private void Awake()
	{
		this.m_gLevelPrefab = Resources.Load("prefab/DiscDifficultStar") as GameObject;
		this.m_txDiscImage = base.transform.FindChild("Texture").GetComponent<UITexture>();
		this.m_spDiscLevel = base.transform.FindChild("Sprite_Level").GetComponent<UISprite>();
		this.m_lDiscName = base.transform.FindChild("Label_musicName").GetComponent<UILabel>();
		this.m_lDiscName_Han = base.transform.FindChild("Label_musicName_han").GetComponent<UILabel>();
		this.m_lDiscArtist = base.transform.FindChild("Label_musicArtist").GetComponent<UILabel>();
		this.m_lDiscKind = base.transform.FindChild("Label_musicKind").GetComponent<UILabel>();
		this.m_lLabel_acc = base.transform.FindChild("1_TotalAccuracy").FindChild("Label_num1").GetComponent<UILabel>();
		this.m_lLabel_maxcombo = base.transform.FindChild("2_maxcombo").FindChild("Label_num1").GetComponent<UILabel>();
		this.m_ilLabel_score = base.transform.FindChild("3_totalscore").FindChild("text").GetComponent<ImageFontLabel>();
		Transform transform = base.transform.FindChild("Rank");
		this.m_spRank = transform.FindChild("Sprite_rank").GetComponent<UISprite>();
		this.m_gRankPlus = new GameObject[2];
		for (int i = 0; i < this.m_gRankPlus.Length; i++)
		{
			this.m_gRankPlus[i] = transform.FindChild("Sprite_plus" + (i + 1).ToString()).gameObject;
		}
		this.m_tLevelGrid = base.transform.FindChild("LevelGrid");
	}

	// Token: 0x06000DCE RID: 3534 RVA: 0x00063224 File Offset: 0x00061424
	public void DiscInfoSetting(int StageIndex)
	{
		RESULTDATA stageResult = Singleton<GameManager>.instance.GetStageResult(StageIndex);
		this.LevelSetting(stageResult);
		Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.DISC_145, stageResult.DISCINFO, this.m_txDiscImage, null, null);
		string text = stageResult.DISCINFO.FullName.ToUpper();
		bool flag = GameData.isContainHangul(text);
		this.m_lDiscName.gameObject.SetActive(false);
		this.m_lDiscName_Han.gameObject.SetActive(false);
		if (!flag)
		{
			this.m_lDiscName.gameObject.SetActive(true);
			this.m_lDiscName.text = string.Empty;
			if (text.Length > 20)
			{
				for (int i = 0; i < 20; i++)
				{
					UILabel lDiscName = this.m_lDiscName;
					lDiscName.text += text[i].ToString().ToUpper();
				}
				UILabel lDiscName2 = this.m_lDiscName;
				lDiscName2.text += "...";
			}
			else
			{
				this.m_lDiscName.text = text;
			}
		}
		else
		{
			this.m_lDiscName_Han.gameObject.SetActive(true);
			this.m_lDiscName_Han.text = text;
		}
		this.m_lDiscKind.text = stageResult.DISCINFO.Genre.ToUpper();
		this.m_lLabel_acc.text = stageResult.GetAccuracy().ToString() + "%";
		this.m_lLabel_maxcombo.text = stageResult.MAXCOMBO.ToString();
		this.m_ilLabel_score.text = stageResult.SCORE.ToString();
		this.m_lDiscArtist.text = stageResult.DISCINFO.Artist.ToUpper();
		this.m_spDiscLevel.spriteName = "level_" + stageResult.PTTYPE.ToString().ToLower() + "_sm";
		this.RankImageSetting(stageResult);
	}

	// Token: 0x06000DCF RID: 3535 RVA: 0x0006341C File Offset: 0x0006161C
	private void RankImageSetting(RESULTDATA rData)
	{
		this.PlusCount = 0;
		GRADE gradetype = rData.GRADETYPE;
		switch (gradetype)
		{
		case GRADE.A:
		case GRADE.A_P:
		case GRADE.A_PP:
			this.m_spRank.spriteName = "Result_OldRank_" + 4.ToString();
			this.PlusCount = gradetype - GRADE.A;
			break;
		case GRADE.S:
		case GRADE.S_P:
		case GRADE.S_PP:
			this.m_spRank.spriteName = "Result_OldRank_" + 7.ToString();
			this.PlusCount = gradetype - GRADE.S;
			break;
		default:
		{
			int gradetype2 = (int)rData.GRADETYPE;
			this.m_spRank.spriteName = "Result_OldRank_" + gradetype2.ToString();
			break;
		}
		}
		if (this.m_gRankPlus != null)
		{
			for (int i = 0; i < this.m_gRankPlus.Length; i++)
			{
				this.m_gRankPlus[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < this.PlusCount; j++)
			{
				this.m_gRankPlus[j].gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06000DD0 RID: 3536 RVA: 0x00063544 File Offset: 0x00061744
	private void LevelSetting(RESULTDATA rData)
	{
		this.LevelCount = rData.DISCINFO.DicPtInfo[rData.PTTYPE].iDif;
		for (int i = 0; i < 12; i++)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.m_gLevelPrefab);
			StarSetting component = gameObject.GetComponent<StarSetting>();
			component.ResultMode = true;
			gameObject.transform.parent = this.m_tLevelGrid.transform;
			gameObject.transform.localScale = new Vector3(2f, 2f, 1f);
			string text = string.Empty;
			if (i >= 5 && i < 10)
			{
				text = "_1";
			}
			else if (i >= 10 && i < 13)
			{
				text = "_2";
			}
			gameObject.transform.localPosition = new Vector3(30f * (float)i, 0f, 0f);
			if (this.LevelCount <= i)
			{
				component.setStar(false, string.Empty);
			}
			else
			{
				component.setStar(true, text);
			}
		}
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x0000C257 File Offset: 0x0000A457
	private void TextSetting(ImageFontLabel text, string textvalue)
	{
		text.text = textvalue;
	}

	// Token: 0x04000E61 RID: 3681
	public int m_iStageIndex;

	// Token: 0x04000E62 RID: 3682
	private UITexture m_txDiscImage;

	// Token: 0x04000E63 RID: 3683
	private UISprite m_spDiscLevel;

	// Token: 0x04000E64 RID: 3684
	private UILabel m_lDiscName;

	// Token: 0x04000E65 RID: 3685
	private UILabel m_lDiscName_Han;

	// Token: 0x04000E66 RID: 3686
	private UILabel m_lDiscArtist;

	// Token: 0x04000E67 RID: 3687
	private UILabel m_lDiscKind;

	// Token: 0x04000E68 RID: 3688
	private UILabel m_lLabel_acc;

	// Token: 0x04000E69 RID: 3689
	private int accValue;

	// Token: 0x04000E6A RID: 3690
	private UILabel m_lLabel_maxcombo;

	// Token: 0x04000E6B RID: 3691
	private int maxcomboValue;

	// Token: 0x04000E6C RID: 3692
	private ImageFontLabel m_ilLabel_score;

	// Token: 0x04000E6D RID: 3693
	private int ScoreValue;

	// Token: 0x04000E6E RID: 3694
	private UISprite m_spRank;

	// Token: 0x04000E6F RID: 3695
	private GameObject[] m_gRankPlus;

	// Token: 0x04000E70 RID: 3696
	private int PlusCount;

	// Token: 0x04000E71 RID: 3697
	private int LevelCount;

	// Token: 0x04000E72 RID: 3698
	private Transform m_tLevelGrid;

	// Token: 0x04000E73 RID: 3699
	private GameObject m_gLevelPrefab;
}
