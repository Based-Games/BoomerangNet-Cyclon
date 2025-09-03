using System;
using UnityEngine;

// Token: 0x0200018D RID: 397
public class ClubTourMissionInfoManager : MonoBehaviour
{
	// Token: 0x06000BD8 RID: 3032 RVA: 0x0005403C File Offset: 0x0005223C
	private void Awake()
	{
		this.m_spMissionBtn = new UISprite[this.m_iMissionBtnMaxCount];
		this.m_gClearCheck = new GameObject[this.m_iMissionBtnMaxCount];
		this.m_gClearCheck = new GameObject[this.m_iMissionBtnMaxCount];
		this.m_gLock = new GameObject[this.m_iMissionBtnMaxCount];
		for (int i = 0; i < this.m_iMissionBtnMaxCount; i++)
		{
			this.m_spMissionBtn[i] = base.transform.FindChild("MissionBtn").FindChild("Mission_" + (i + 1).ToString()).GetComponent<UISprite>();
			this.m_gClearCheck[i] = base.transform.FindChild("MissionBtn").FindChild("Mission_" + (i + 1).ToString()).FindChild("Sprite_Clear")
				.gameObject;
			this.m_gLock[i] = base.transform.FindChild("MissionBtn").FindChild("Mission_" + (i + 1).ToString()).FindChild("Sprite_Lock")
				.gameObject;
		}
		this.m_ClubTourMission = base.transform.FindChild("Panel_Mission").GetComponent<ClubTourMission>();
		this.m_KeySountBtn = base.transform.FindChild("Panel_keysoundBtn").GetComponent<TweenAlpha>();
		this.m_gMissionLock = base.transform.FindChild("MissionLock").gameObject;
		this.m_gClearBG = base.transform.FindChild("Panel_Mission").FindChild("Panel_Rewards").FindChild("Sprite_Clear")
			.gameObject;
		this.m_ClubTourManager = base.transform.parent.GetComponent<ClubTourManager>();
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x000541F4 File Offset: 0x000523F4
	public void MissionClick(ClubTourMissionInfoManager.MissionBtn_e btnNum)
	{
		MissionData missionData = (MissionData)this.m_MissionPackData.ArrMissionData[(int)btnNum];
		this.GetNetUserRecord(missionData);
		this.m_MissionData = missionData;
		this.m_iIsSelectMissionBtn = btnNum;
		this.BtnSetting();
		this.m_ClubTourMission.MissionSetting(missionData);
		this.m_ClubTourMission.PlayAni();
		this.ClearCheck();
		this.LockCheck();
		this.setBtnLock((int)btnNum, this.m_MissionData.Lock);
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x0000AF36 File Offset: 0x00009136
	public void setBtnLock(int index, bool st)
	{
		this.MissionBtnLockCheck();
		this.m_gLock[index].SetActive(st);
	}

	// Token: 0x06000BDB RID: 3035 RVA: 0x00054268 File Offset: 0x00052468
	private void GetNetUserRecord(MissionData mData)
	{
		WWWMissionUserRecordScript wwwmissionUserRecordScript = new WWWMissionUserRecordScript();
		wwwmissionUserRecordScript.strMissionID = mData.strServerKey;
		wwwmissionUserRecordScript.CallBackFail = new WWWObject.CompleteCallBack(this.setUserRecord);
		wwwmissionUserRecordScript.CallBack = new WWWObject.CompleteCallBack(this.setUserRecord);
		Singleton<WWWManager>.instance.AddQueue(wwwmissionUserRecordScript);
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x0000AF4C File Offset: 0x0000914C
	private void setUserRecord()
	{
		this.m_ClubTourMission.MyRecordSetting();
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x000542B8 File Offset: 0x000524B8
	public void MissionBtnClearCheck()
	{
		for (int i = 0; i < this.m_MissionPackData.ArrMissionData.Count; i++)
		{
			if (((MissionData)this.m_MissionPackData.ArrMissionData[i]).Cleared)
			{
				this.m_gClearCheck[i].SetActive(true);
			}
			else
			{
				this.m_gClearCheck[i].SetActive(false);
			}
		}
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x0005431C File Offset: 0x0005251C
	public void MissionBtnLockCheck()
	{
		for (int i = 0; i < this.m_MissionPackData.ArrMissionData.Count; i++)
		{
			if (((MissionData)this.m_MissionPackData.ArrMissionData[i]).Lock)
			{
				this.m_gLock[i].SetActive(true);
			}
			else
			{
				this.m_gLock[i].SetActive(false);
			}
		}
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x0000AF59 File Offset: 0x00009159
	private void ClearCheck()
	{
		if (this.m_MissionData.Cleared)
		{
			if (this.m_MissionData.iRewardSong > 0)
			{
				this.m_gClearBG.SetActive(true);
				return;
			}
		}
		else
		{
			this.m_gClearBG.SetActive(false);
		}
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x0000AF8F File Offset: 0x0000918F
	private void LockCheck()
	{
		this.m_gMissionLock.SetActive(false);
		if (this.m_MissionData.Lock)
		{
			this.m_gMissionLock.SetActive(true);
			return;
		}
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x00054380 File Offset: 0x00052580
	public void BtnAni()
	{
		for (int i = 0; i < this.m_iMissionBtnMaxCount; i++)
		{
			this.m_spMissionBtn[i].depth = 0;
			if (this.m_iIsSelectMissionBtn == (ClubTourMissionInfoManager.MissionBtn_e)i)
			{
				this.m_spMissionBtn[i].depth = 2;
			}
			else
			{
				TweenPosition component = this.m_spMissionBtn[i].GetComponent<TweenPosition>();
				component.ResetToBeginning();
				component.Play(true);
			}
		}
	}

	// Token: 0x06000BE2 RID: 3042 RVA: 0x000543E0 File Offset: 0x000525E0
	private void BtnSetting()
	{
		for (int i = 0; i < this.m_iMissionBtnMaxCount; i++)
		{
			if (this.m_iIsSelectMissionBtn != (ClubTourMissionInfoManager.MissionBtn_e)i)
			{
				this.m_spMissionBtn[i].spriteName = "ClubTour_Mission_Btn_" + (i + 1).ToString() + "_Back";
				this.m_spMissionBtn[i].MakePixelPerfect();
				this.m_spMissionBtn[i].GetComponent<ClubTourMissionBtn>().m_bIsSelect = false;
				this.m_spMissionBtn[i].transform.FindChild("Sprite_Light").gameObject.SetActive(false);
			}
		}
		this.m_spMissionBtn[(int)this.m_iIsSelectMissionBtn].transform.FindChild("Sprite_Light").gameObject.SetActive(true);
		TweenPosition component = this.m_spMissionBtn[(int)this.m_iIsSelectMissionBtn].transform.FindChild("Sprite_Light").GetComponent<TweenPosition>();
		TweenAlpha component2 = this.m_spMissionBtn[(int)this.m_iIsSelectMissionBtn].transform.FindChild("Sprite_Light").GetComponent<TweenAlpha>();
		component.enabled = false;
		component2.enabled = false;
		component.duration = (component2.duration = this.m_fLightSpeed);
		component.ResetToBeginning();
		component2.ResetToBeginning();
		component.Play(true);
		component2.Play(true);
		this.m_spMissionBtn[(int)this.m_iIsSelectMissionBtn].spriteName = "ClubTour_Mission_Btn_" + ((int)(this.m_iIsSelectMissionBtn + 1)).ToString() + "_Select";
		this.m_spMissionBtn[(int)this.m_iIsSelectMissionBtn].MakePixelPerfect();
	}

	// Token: 0x06000BE3 RID: 3043 RVA: 0x0005455C File Offset: 0x0005275C
	public void PlayClick()
	{
		if (this.m_MissionData.Lock)
		{
			this.m_ClubTourManager.FirstClick();
		}
		if (Singleton<GameManager>.instance.UserData.BeatPoint < this.m_MissionData.iMissionCost)
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_RAVEUP_DISC_UNMOUNT, false);
			return;
		}
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_TITLE_START_ALRIGHT, false);
		Singleton<SongManager>.instance.Mode = GAMEMODE.MISSION;
		Singleton<SongManager>.instance.Mission = this.m_MissionData;
		if (this.m_MissionData.Eff_Speed != EFFECTOR_SPEED.NONE)
		{
			GameData.SPEEDEFFECTOR = this.m_MissionData.Eff_Speed;
		}
		GameData.FADEEFFCTOR = this.m_MissionData.Eff_Fader;
		GameData.RANDEFFECTOR = this.m_MissionData.Eff_Rand;
		Singleton<SceneSwitcher>.instance.LoadNextScene("game");
	}

	// Token: 0x04000B86 RID: 2950
	[HideInInspector]
	public MissionPackData m_MissionPackData;

	// Token: 0x04000B87 RID: 2951
	private MissionData m_MissionData;

	// Token: 0x04000B88 RID: 2952
	private ClubTourMission m_ClubTourMission;

	// Token: 0x04000B89 RID: 2953
	private UISprite[] m_spMissionBtn;

	// Token: 0x04000B8A RID: 2954
	private GameObject[] m_gClearCheck;

	// Token: 0x04000B8B RID: 2955
	private TweenAlpha m_KeySountBtn;

	// Token: 0x04000B8C RID: 2956
	private int m_iMissionBtnMaxCount = 3;

	// Token: 0x04000B8D RID: 2957
	private float m_fLightSpeed = 0.75f;

	// Token: 0x04000B8E RID: 2958
	private ClubTourMissionInfoManager.MissionBtn_e m_iIsSelectMissionBtn;

	// Token: 0x04000B8F RID: 2959
	private GameObject m_gMissionLock;

	// Token: 0x04000B90 RID: 2960
	private GameObject m_gClearBG;

	// Token: 0x04000B91 RID: 2961
	private GameObject[] m_gLock;

	// Token: 0x04000B92 RID: 2962
	private ClubTourManager m_ClubTourManager;

	// Token: 0x0200018E RID: 398
	public enum MissionBtn_e
	{
		// Token: 0x04000B94 RID: 2964
		mission_1,
		// Token: 0x04000B95 RID: 2965
		mission_2,
		// Token: 0x04000B96 RID: 2966
		mission_3
	}
}
