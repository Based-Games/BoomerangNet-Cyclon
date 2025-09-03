using System;
using UnityEngine;

// Token: 0x020001EE RID: 494
public class ImageFontLabel : MonoBehaviour
{
	// Token: 0x06000E58 RID: 3672 RVA: 0x000677F0 File Offset: 0x000659F0
	private void Awake()
	{
		this.OriginPos = base.transform.localPosition;
		if (this.m_gImageFontPrefab == null)
		{
			this.m_gImageFontPrefab = Resources.Load("Prefab/ImageFont/ImageFont") as GameObject;
		}
		this.text = this.m_Text;
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06000E59 RID: 3673 RVA: 0x0000C79D File Offset: 0x0000A99D
	// (set) Token: 0x06000E5A RID: 3674 RVA: 0x00067840 File Offset: 0x00065A40
	public string text
	{
		get
		{
			return this.m_sText;
		}
		set
		{
			this.m_sText = value;
			this.m_Text = value;
			if (!this.m_bSetting)
			{
				base.Invoke("FontSetting", 0.01f);
			}
			else
			{
				this.FontSetting();
			}
		}
	}

	// Token: 0x06000E5B RID: 3675 RVA: 0x00067884 File Offset: 0x00065A84
	private void FontSetting()
	{
		this.m_bSetting = true;
		this.ClearFont();
		switch (this.isFontKind)
		{
		case ImageFontLabel.FontKind_e.RankingFont_None:
			this.m_sStrFontKind = string.Empty;
			break;
		case ImageFontLabel.FontKind_e.RankingFont_Select:
			this.m_sStrFontKind = "_";
			break;
		case ImageFontLabel.FontKind_e.TimerFont:
			this.m_sStrFontKind = "time_";
			break;
		case ImageFontLabel.FontKind_e.BeatPointFont:
			this.m_sStrFontKind = "bp_";
			break;
		case ImageFontLabel.FontKind_e.GraphFont:
			this.m_sStrFontKind = "graph_";
			break;
		case ImageFontLabel.FontKind_e.MyrecordFont:
			this.m_sStrFontKind = "myrecord_";
			break;
		case ImageFontLabel.FontKind_e.LevelFont:
			this.m_sStrFontKind = "level_";
			break;
		case ImageFontLabel.FontKind_e.ResultscoreFont:
			this.m_sStrFontKind = "resultscore_";
			break;
		case ImageFontLabel.FontKind_e.ResultBreakFont:
			this.m_sStrFontKind = "resultbreak_";
			break;
		case ImageFontLabel.FontKind_e.ResultAddExpFont:
			this.m_sStrFontKind = "resultaddexp_";
			break;
		case ImageFontLabel.FontKind_e.ResultExpFont:
			this.m_sStrFontKind = "resultexp_";
			break;
		case ImageFontLabel.FontKind_e.ClubTourResultAddExpFont:
			this.m_sStrFontKind = "ClubTourResult_addExp_";
			break;
		case ImageFontLabel.FontKind_e.ClubTourResultTotalScoreFont:
			this.m_sStrFontKind = "ClubTourResult_TotalScore_";
			break;
		case ImageFontLabel.FontKind_e.RankingSceneNum:
			this.m_sStrFontKind = "RankingSceneNum_";
			break;
		case ImageFontLabel.FontKind_e.RankingSceneLevel:
			this.m_sStrFontKind = "RankingSceneLevel_";
			break;
		case ImageFontLabel.FontKind_e.TitleRankingScore:
			this.m_sStrFontKind = "TitleRankingScore_";
			break;
		}
		if (this.m_sText == string.Empty || this.m_sText == " ")
		{
			return;
		}
		if (this.m_gImageFontPrefab == null)
		{
			this.m_gImageFontPrefab = Resources.Load("Prefab/ImageFont/ImageFont") as GameObject;
		}
		GameObject[] array = new GameObject[2];
		float num = 0f;
		int num2 = 0;
		if (this.isPivot == ImageFontLabel.Pivot_e.Left)
		{
			for (int i = 0; i < this.m_sText.Length; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(this.m_gImageFontPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				gameObject.transform.parent = base.transform;
				gameObject.layer = base.gameObject.layer;
				gameObject.GetComponent<UISprite>().pivot = UIWidget.Pivot.Left;
				gameObject.transform.localScale = Vector3.one;
				gameObject.GetComponent<UISprite>().spriteName = this.m_sStrFontKind + this.m_sText[i].ToString().ToLower();
				gameObject.GetComponent<UISprite>().MakePixelPerfect();
				gameObject.GetComponent<UISprite>().depth = this.m_iDepth;
				gameObject.transform.localScale = new Vector3(1f * this.Size, 1f * this.Size, 1f * this.Size);
				gameObject.transform.localEulerAngles = Vector3.zero;
				gameObject.transform.localPosition = new Vector3(num, 0f, 0f);
				if (gameObject.GetComponent<UISprite>().spriteName != null)
				{
					num += (float)gameObject.GetComponent<UISprite>().width * this.Size * (float)this.isPivot - this.RevisionValue;
				}
				num2++;
			}
		}
		else if (this.isPivot == ImageFontLabel.Pivot_e.Right)
		{
			for (int j = this.m_sText.Length - 1; j >= 0; j--)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(this.m_gImageFontPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				gameObject2.transform.parent = base.transform;
				gameObject2.layer = base.gameObject.layer;
				gameObject2.GetComponent<UISprite>().pivot = UIWidget.Pivot.Right;
				gameObject2.transform.localScale = Vector3.one;
				gameObject2.GetComponent<UISprite>().spriteName = this.m_sStrFontKind + this.m_sText[j].ToString().ToLower();
				gameObject2.GetComponent<UISprite>().MakePixelPerfect();
				gameObject2.GetComponent<UISprite>().depth = this.m_iDepth;
				gameObject2.transform.localScale = new Vector3(1f * this.Size, 1f * this.Size, 1f * this.Size);
				gameObject2.transform.localEulerAngles = Vector3.zero;
				gameObject2.transform.localPosition = new Vector3(num, 0f, 0f);
				if (gameObject2.GetComponent<UISprite>().spriteName != null)
				{
					num += (float)gameObject2.GetComponent<UISprite>().width * this.Size * (float)this.isPivot + this.RevisionValue;
				}
				num2++;
			}
		}
		if (this.Center)
		{
			Vector3 localPosition = base.transform.localPosition;
			float num3 = num / 2f;
			base.transform.localPosition = new Vector3(this.OriginPos.x - num3, localPosition.y, localPosition.z);
		}
	}

	// Token: 0x06000E5C RID: 3676 RVA: 0x00067DA8 File Offset: 0x00065FA8
	private void ClearFont()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			UnityEngine.Object.DestroyObject(base.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x04000F7A RID: 3962
	public bool Center;

	// Token: 0x04000F7B RID: 3963
	public ImageFontLabel.FontKind_e isFontKind;

	// Token: 0x04000F7C RID: 3964
	public ImageFontLabel.Pivot_e isPivot = ImageFontLabel.Pivot_e.Left;

	// Token: 0x04000F7D RID: 3965
	public int m_iDepth;

	// Token: 0x04000F7E RID: 3966
	public string m_Text;

	// Token: 0x04000F7F RID: 3967
	public float Size = 1f;

	// Token: 0x04000F80 RID: 3968
	public float RevisionValue;

	// Token: 0x04000F81 RID: 3969
	private string m_sStrFontKind;

	// Token: 0x04000F82 RID: 3970
	private string m_sText;

	// Token: 0x04000F83 RID: 3971
	private GameObject m_gImageFontPrefab;

	// Token: 0x04000F84 RID: 3972
	[HideInInspector]
	public Vector3 OriginPos = Vector3.zero;

	// Token: 0x04000F85 RID: 3973
	private bool m_bSetting;

	// Token: 0x020001EF RID: 495
	public enum FontKind_e
	{
		// Token: 0x04000F87 RID: 3975
		RankingFont_None,
		// Token: 0x04000F88 RID: 3976
		RankingFont_Select,
		// Token: 0x04000F89 RID: 3977
		TimerFont,
		// Token: 0x04000F8A RID: 3978
		BeatPointFont,
		// Token: 0x04000F8B RID: 3979
		GraphFont,
		// Token: 0x04000F8C RID: 3980
		MyrecordFont,
		// Token: 0x04000F8D RID: 3981
		LevelFont,
		// Token: 0x04000F8E RID: 3982
		ResultscoreFont,
		// Token: 0x04000F8F RID: 3983
		ResultBreakFont,
		// Token: 0x04000F90 RID: 3984
		ResultAddExpFont,
		// Token: 0x04000F91 RID: 3985
		ResultExpFont,
		// Token: 0x04000F92 RID: 3986
		ClubTourResultAddExpFont,
		// Token: 0x04000F93 RID: 3987
		ClubTourResultTotalScoreFont,
		// Token: 0x04000F94 RID: 3988
		RankingSceneNum,
		// Token: 0x04000F95 RID: 3989
		RankingSceneLevel,
		// Token: 0x04000F96 RID: 3990
		TitleRankingScore
	}

	// Token: 0x020001F0 RID: 496
	public enum Pivot_e
	{
		// Token: 0x04000F98 RID: 3992
		Left = 1,
		// Token: 0x04000F99 RID: 3993
		Right = -1
	}
}
