using System;
using UnityEngine;

// Token: 0x020001D7 RID: 471
public class MyInfoManager : MonoBehaviour
{
	// Token: 0x06000DB8 RID: 3512 RVA: 0x000622AC File Offset: 0x000604AC
	private void Awake()
	{
		this.m_UserLevel = base.transform.FindChild("Label_UserLevel").GetComponent<ImageFontLabel>();
		this.m_ilBeatPoint = base.transform.FindChild("ImageFont_BeatPoint").GetComponent<ImageFontLabel>();
		this.m_spClubImage = base.transform.FindChild("Sprite_ClubMark").GetComponent<UISprite>();
		this.m_lMyName = base.transform.FindChild("Label_Myname").GetComponent<UILabel>();
		this.m_lClubName = base.transform.FindChild("Label_ClubName").GetComponent<UILabel>();
		this.m_spUserImage = base.transform.FindChild("UserPic").GetComponent<UISprite>();
	}

	// Token: 0x06000DB9 RID: 3513 RVA: 0x0000B035 File Offset: 0x00009235
	private void Start()
	{
		base.Invoke("Init", 0.15f);
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x0006235C File Offset: 0x0006055C
	private void Init()
	{
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		this.m_lMyName.text = userData.Name;
		this.m_spClubImage.spriteName = "club_" + userData.ClubIcon.ToString();
		int level = userData.Level;
		this.m_UserLevel.text = userData.Level.ToString();
		this.m_ilBeatPoint.text = userData.BeatPoint.ToString();
		if (!Singleton<GameManager>.instance.ONLOGIN)
		{
			this.m_UserLevel.text = "0";
			this.m_ilBeatPoint.text = "N/A";
		}
		this.NamePosSetting();
		if (this.m_bIsClubToureResult)
		{
			this.m_ilBeatPoint.text = (userData.BeatPoint - Singleton<SongManager>.instance.Mission.iMissionCost).ToString();
		}
		this.m_lClubName.text = userData.ClubName;
		this.m_spUserImage.spriteName = userData.Icon.ToString();
		if (userData.Icon == 0)
		{
			this.m_spUserImage.MakePixelPerfect();
			this.m_spUserImage.transform.localScale = Vector3.one * 2f;
		}
	}

	// Token: 0x06000DBB RID: 3515 RVA: 0x000624A0 File Offset: 0x000606A0
	private void NamePosSetting()
	{
		float num = this.m_fNameSpaceValue;
		for (int i = 0; i < this.m_UserLevel.transform.childCount; i++)
		{
			num += this.m_UserLevel.transform.GetChild(i).GetComponent<UISprite>().localSize.x * 2f;
		}
		this.m_lMyName.transform.localPosition = new Vector3(this.m_UserLevel.transform.localPosition.x + num, this.m_lMyName.transform.localPosition.y, this.m_lMyName.transform.localPosition.z);
	}

	// Token: 0x06000DBC RID: 3516 RVA: 0x0000C1B2 File Offset: 0x0000A3B2
	public void LevelSetting(int lv)
	{
		this.m_UserLevel.text = lv.ToString();
		this.NamePosSetting();
	}

	// Token: 0x04000E22 RID: 3618
	public bool m_bIsClubToureResult;

	// Token: 0x04000E23 RID: 3619
	private UILabel m_lMyName;

	// Token: 0x04000E24 RID: 3620
	private UILabel m_lClubName;

	// Token: 0x04000E25 RID: 3621
	private ImageFontLabel m_ilBeatPoint;

	// Token: 0x04000E26 RID: 3622
	private ImageFontLabel m_UserLevel;

	// Token: 0x04000E27 RID: 3623
	private UISprite m_spClubImage;

	// Token: 0x04000E28 RID: 3624
	private UISprite m_spUserImage;

	// Token: 0x04000E29 RID: 3625
	private float m_fNameSpaceValue = 10f;
}
