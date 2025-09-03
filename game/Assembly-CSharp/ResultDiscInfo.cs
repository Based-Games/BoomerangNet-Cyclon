using System;
using UnityEngine;

// Token: 0x020001E2 RID: 482
public class ResultDiscInfo : MonoBehaviour
{
	// Token: 0x06000DFB RID: 3579 RVA: 0x00063D74 File Offset: 0x00061F74
	private void Awake()
	{
		this.m_tLevelGrid = base.transform.FindChild("LevelGrid");
		this.m_txClearDiscImage = base.transform.FindChild("Disc_Texture").GetComponent<UITexture>();
		this.m_spClearLevel = base.transform.FindChild("Sprite_Level").GetComponent<UISprite>();
		this.m_lDiscName = base.transform.FindChild("Label_musicName").GetComponent<UILabel>();
		this.m_lDiscName_Han = base.transform.FindChild("Label_musicName_han").GetComponent<UILabel>();
		this.m_lDiscKind = base.transform.FindChild("Label_musicKind").GetComponent<UILabel>();
		this.m_lDiscArtist = base.transform.FindChild("Label_musicArtist").GetComponent<UILabel>();
		this.m_oDiscStarPrefab = Resources.Load("prefab/DiscDifficultStar") as GameObject;
	}

	// Token: 0x06000DFC RID: 3580 RVA: 0x00063E50 File Offset: 0x00062050
	public void DefaultDiscInfoSetting()
	{
		string text = "Default Test";
		bool flag = GameData.isContainHangul(text);
		this.m_lDiscName.gameObject.SetActive(false);
		this.m_lDiscName_Han.gameObject.SetActive(false);
		if (!flag)
		{
			this.m_lDiscName.gameObject.SetActive(true);
			this.m_lDiscName.text = string.Empty;
			if (text.Length > 23)
			{
				for (int i = 0; i < 23; i++)
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
		this.m_lDiscArtist.text = "DEFUALT";
		this.m_lDiscKind.text = "DEFAULT";
		this.m_spClearLevel.spriteName = "level_ez_sm";
	}

	// Token: 0x06000DFD RID: 3581 RVA: 0x00063F7C File Offset: 0x0006217C
	public void DiscInfoSetting()
	{
		RESULTDATA resultData = Singleton<GameManager>.instance.ResultData;
		Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.SONG_500, resultData.DISCINFO, this.m_txClearDiscImage, null, null);
		string text = resultData.DISCINFO.FullName.ToUpper();
		bool flag = GameData.isContainHangul(text);
		this.m_lDiscName.gameObject.SetActive(false);
		this.m_lDiscName_Han.gameObject.SetActive(false);
		if (!flag)
		{
			this.m_lDiscName.gameObject.SetActive(true);
			this.m_lDiscName.text = string.Empty;
			if (text.Length > 23)
			{
				for (int i = 0; i < 23; i++)
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
		this.m_lDiscArtist.text = resultData.DISCINFO.Artist;
		this.m_lDiscKind.text = resultData.DISCINFO.Genre;
		this.m_spClearLevel.spriteName = "level_" + resultData.PTTYPE.ToString().ToLower() + "_sm";
		this.LevelSetting(resultData);
		GameObject gameObject = base.transform.FindChild("EventMark").gameObject;
		if (Singleton<SongManager>.instance.IsContainEventPt(resultData.DISCINFO.Id))
		{
			gameObject.gameObject.SetActive(true);
			return;
		}
		gameObject.gameObject.SetActive(false);
	}

	// Token: 0x06000DFE RID: 3582 RVA: 0x00064144 File Offset: 0x00062344
	private void LevelSetting(RESULTDATA rData)
	{
		int iDif = rData.DISCINFO.DicPtInfo[rData.PTTYPE].iDif;
		for (int i = 0; i < 12; i++)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(this.m_oDiscStarPrefab);
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
			if (iDif <= i)
			{
				component.setStar(false, string.Empty);
			}
			else
			{
				component.setStar(true, text);
			}
		}
	}

	// Token: 0x04000EB9 RID: 3769
	private Transform m_tLevelGrid;

	// Token: 0x04000EBA RID: 3770
	private UITexture m_txClearDiscImage;

	// Token: 0x04000EBB RID: 3771
	private UISprite m_spClearLevel;

	// Token: 0x04000EBC RID: 3772
	private UILabel m_lDiscName;

	// Token: 0x04000EBD RID: 3773
	private UILabel m_lDiscName_Han;

	// Token: 0x04000EBE RID: 3774
	private UILabel m_lDiscKind;

	// Token: 0x04000EBF RID: 3775
	private UILabel m_lDiscArtist;

	// Token: 0x04000EC0 RID: 3776
	private GameObject m_oDiscStarPrefab;
}
