using System;
using UnityEngine;

// Token: 0x02000190 RID: 400
public class ClubTourMyInfoManager : MonoBehaviour
{
	// Token: 0x06000BE8 RID: 3048 RVA: 0x00054620 File Offset: 0x00052820
	private void Awake()
	{
		this.m_lMyName = base.transform.FindChild("Label_Myname").GetComponent<UILabel>();
		this.m_lClubName = base.transform.FindChild("Label_ClubName").GetComponent<UILabel>();
		this.m_UserLevel = base.transform.FindChild("Label_UserLevel").GetComponent<ImageFontLabel>();
		this.m_ilBeatPoint = base.transform.FindChild("ImageFont_BeatPoint").GetComponent<ImageFontLabel>();
		this.m_spClubImage = base.transform.FindChild("Sprite_ClubMark").GetComponent<UISprite>();
		this.m_spUserImage = base.transform.FindChild("UserPic").GetComponent<UISprite>();
	}

	// Token: 0x06000BE9 RID: 3049 RVA: 0x0000B035 File Offset: 0x00009235
	private void Start()
	{
		base.Invoke("Init", 0.15f);
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x000546D0 File Offset: 0x000528D0
	private void Init()
	{
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		this.m_spClubImage.spriteName = "club_" + userData.ClubIcon.ToString();
		this.m_lMyName.text = userData.Name;
		this.m_UserLevel.text = userData.Level.ToString();
		this.NamePosSetting();
		this.m_ilBeatPoint.text = userData.BeatPoint.ToString();
		this.m_lClubName.text = userData.ClubName;
		this.m_spUserImage.spriteName = userData.Icon.ToString();
		if (userData.Icon == 0)
		{
			this.m_spUserImage.MakePixelPerfect();
		}
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x00054798 File Offset: 0x00052998
	private void NamePosSetting()
	{
		float num = this.m_fNameSpaceValue;
		for (int i = 0; i < this.m_UserLevel.transform.childCount; i++)
		{
			num += this.m_UserLevel.transform.GetChild(i).GetComponent<UISprite>().localSize.x;
		}
		this.m_lMyName.transform.localPosition = new Vector3(this.m_UserLevel.transform.localPosition.x + num, this.m_lMyName.transform.localPosition.y, this.m_lMyName.transform.localPosition.z);
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x0000B047 File Offset: 0x00009247
	public void LevelSetting(int lv)
	{
		this.m_UserLevel.text = lv.ToString();
		this.NamePosSetting();
	}

	// Token: 0x04000B9A RID: 2970
	private UILabel m_lMyName;

	// Token: 0x04000B9B RID: 2971
	private UILabel m_lClubName;

	// Token: 0x04000B9C RID: 2972
	private ImageFontLabel m_ilBeatPoint;

	// Token: 0x04000B9D RID: 2973
	private ImageFontLabel m_UserLevel;

	// Token: 0x04000B9E RID: 2974
	private UISprite m_spClubImage;

	// Token: 0x04000B9F RID: 2975
	private UISprite m_spUserImage;

	// Token: 0x04000BA0 RID: 2976
	private float m_fNameSpaceValue = 10f;
}
