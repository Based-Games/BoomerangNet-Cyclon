using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000202 RID: 514
public class RaveUpDiscSelectMode : MonoBehaviour
{
	// Token: 0x06000EE7 RID: 3815 RVA: 0x0006B1E0 File Offset: 0x000693E0
	private void Awake()
	{
		this.m_txAlbumImage = base.transform.FindChild("DiscSet").FindChild("Texture_Album").GetComponent<UITexture>();
		Transform transform = base.transform.FindChild("Discs");
		this.m_RaveUpDisc = new RaveUpDisc[6];
		this.m_txDiscImages = new UITexture[6];
		for (int i = 0; i < this.m_RaveUpDisc.Length; i++)
		{
			this.m_RaveUpDisc[i] = transform.FindChild("Disc_" + (i + 1).ToString()).GetComponent<RaveUpDisc>();
			this.m_txDiscImages[i] = this.m_RaveUpDisc[i].transform.FindChild("Texture_Disc").GetComponent<UITexture>();
		}
		this.m_lAlbumName = base.transform.FindChild("DiscSet").FindChild("Label_AlbumName").GetComponent<UILabel>();
		this.m_lAlbumKind = base.transform.FindChild("DiscSet").FindChild("Label_AlbumKind").GetComponent<UILabel>();
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x0006B2EC File Offset: 0x000694EC
	public void SettingDisc(int index, DiscInfo di, string discName)
	{
		ArrayList raveUpAlbumStage = Singleton<SongManager>.instance.GetRaveUpAlbumStage(Singleton<SongManager>.instance.SelectAlbumId);
		Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.SONG_500, di, this.m_RaveUpDisc[index].m_txDiscImage, null, null);
		this.m_RaveUpDisc[index].StarSetting(di.DicPtInfo[((RaveUpStage)raveUpAlbumStage[index]).PtType].iDif);
		this.m_RaveUpDisc[index].m_sName = discName;
		this.m_RaveUpDisc[index].m_spDiscLevel.spriteName = "level_" + ((RaveUpStage)raveUpAlbumStage[index]).PtType.ToString().ToLower() + "_sm";
		Texture texture = new Texture();
		Texture texture2 = new Texture();
		this.m_RaveUpDisc[index].m_gCD.GetComponent<RaveUpCD>().m_sDiscName = di.Name;
		this.m_RaveUpDisc[index].m_gCD.GetComponent<RaveUpCD>().m_dInfo = di;
		if (texture2 != null)
		{
			this.m_RaveUpDisc[index].m_gCD.GetComponent<UITexture>().mainTexture = texture2;
		}
		this.m_RaveUpDisc[index].m_gCD.GetComponent<RaveUpCD>().m_tx500 = texture2;
		this.m_RaveUpDisc[index].m_gCD.GetComponent<RaveUpCD>().m_tx96 = texture;
	}

	// Token: 0x04001055 RID: 4181
	public UITexture m_txAlbumImage;

	// Token: 0x04001056 RID: 4182
	public RaveUpDisc[] m_RaveUpDisc;

	// Token: 0x04001057 RID: 4183
	public UITexture[] m_txDiscImages;

	// Token: 0x04001058 RID: 4184
	public UILabel m_lAlbumName;

	// Token: 0x04001059 RID: 4185
	public UILabel m_lAlbumKind;
}
