using System;
using UnityEngine;

// Token: 0x0200018F RID: 399
public class ClubTourMissionPack : MonoBehaviour
{
	// Token: 0x06000BE5 RID: 3045 RVA: 0x0000AFB7 File Offset: 0x000091B7
	private void Awake()
	{
		this.m_lMissionName = base.transform.FindChild("Label_PackName").GetComponent<UILabel>();
		this.m_txMissionImage = base.transform.FindChild("Texture_PackImage").GetComponent<UITexture>();
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x0000AFEF File Offset: 0x000091EF
	public void setMissionPack(MissionPackData mpd)
	{
		this.SelectID = mpd.iPackId;
		this.m_lMissionName.text = mpd.strPackTitle;
		Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.MISSIONPACK_287, null, this.m_txMissionImage, null, mpd);
	}

	// Token: 0x04000B97 RID: 2967
	private UILabel m_lMissionName;

	// Token: 0x04000B98 RID: 2968
	private UITexture m_txMissionImage;

	// Token: 0x04000B99 RID: 2969
	private int SelectID;
}
