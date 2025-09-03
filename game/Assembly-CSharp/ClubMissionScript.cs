using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001F7 RID: 503
public class ClubMissionScript : MonoBehaviour
{
	// Token: 0x06000E90 RID: 3728 RVA: 0x0000CA91 File Offset: 0x0000AC91
	private void Start()
	{
		this.Init();
		this.SetObject();
		this.InitMissionPack();
		this.SetMissionBtn();
		this.SetMissionData();
	}

	// Token: 0x06000E91 RID: 3729 RVA: 0x00068974 File Offset: 0x00066B74
	private void Init()
	{
		ArrayList allClubMission = Singleton<SongManager>.instance.AllClubMission;
		this.m_sPackData = (MissionPackData)allClubMission[0];
		this.m_sMissionData = (MissionData)this.m_sPackData.ArrMissionData[this.m_iSelectMission];
	}

	// Token: 0x06000E92 RID: 3730 RVA: 0x000689C0 File Offset: 0x00066BC0
	private void SetObject()
	{
		this.m_cCamera = base.transform.FindChild("Camera").GetComponent<Camera>();
		this.m_oPackSlot = base.transform.FindChild("PackSlot").gameObject;
		this.m_oPackSelect = this.m_oPackSlot.transform.FindChild("SlotSelect").gameObject;
		GameObject gameObject = base.transform.FindChild("MissionInfo").gameObject;
		this.m_tMissionTitle = gameObject.transform.FindChild("MissionTitle").GetComponent<UILabel>();
		this.m_tPackTitle = gameObject.transform.FindChild("PackTitle").GetComponent<UILabel>();
		this.m_arrSong[0] = gameObject.transform.FindChild("Song0").GetComponent<UILabel>();
		this.m_arrSong[1] = gameObject.transform.FindChild("Song1").GetComponent<UILabel>();
		this.m_arrSong[2] = gameObject.transform.FindChild("Song2").GetComponent<UILabel>();
		this.m_arrPattern[0] = gameObject.transform.FindChild("Pattern0").GetComponent<UILabel>();
		this.m_arrPattern[1] = gameObject.transform.FindChild("Pattern1").GetComponent<UILabel>();
		this.m_arrPattern[2] = gameObject.transform.FindChild("Pattern2").GetComponent<UILabel>();
		this.m_arrMission[0] = gameObject.transform.FindChild("Mission0").GetComponent<UILabel>();
		this.m_arrMission[1] = gameObject.transform.FindChild("Mission1").GetComponent<UILabel>();
		this.m_arrMission[2] = gameObject.transform.FindChild("Mission2").GetComponent<UILabel>();
		this.m_tReward = gameObject.transform.FindChild("Reward").GetComponent<UILabel>();
		this.m_tTempSpeed = base.transform.FindChild("Speed").GetComponent<UILabel>();
		this.m_tTempSpeed.text = this.TempSpeed.ToString();
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x00068BC8 File Offset: 0x00066DC8
	private void InitMissionPack()
	{
		ArrayList allClubMission = Singleton<SongManager>.instance.AllClubMission;
		GameObject gameObject = base.transform.FindChild("PackSlot").gameObject;
		for (int i = 0; i < allClubMission.Count; i++)
		{
			MissionPackData missionPackData = (MissionPackData)allClubMission[i];
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(this.PACK);
			gameObject2.name = "Pack" + i.ToString();
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.FindChild("txtPack").GetComponent<UILabel>().text = "Pack " + missionPackData.iPackId;
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.transform.localPosition = new Vector3(-500f + 250f * (float)i, -205f, 0f);
		}
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x00068CC0 File Offset: 0x00066EC0
	private void SetMissionBtn()
	{
		ArrayList arrMissionData = this.m_sPackData.ArrMissionData;
		for (int i = 0; i < arrMissionData.Count; i++)
		{
			MissionData missionData = (MissionData)arrMissionData[i];
		}
		GameObject gameObject = base.transform.FindChild("MissionBtn").gameObject;
		for (int j = 0; j < 5; j++)
		{
			string text = "Mission" + j.ToString();
			GameObject gameObject2 = gameObject.transform.FindChild(text).gameObject;
			if (arrMissionData.Count > j)
			{
				gameObject2.SetActive(true);
			}
			else
			{
				gameObject2.SetActive(false);
			}
			if (this.m_iSelectMission == j)
			{
				gameObject2.GetComponent<UISprite>().color = Color.red;
			}
			else
			{
				gameObject2.GetComponent<UISprite>().color = Color.white;
			}
		}
	}

	// Token: 0x06000E95 RID: 3733 RVA: 0x00068DA8 File Offset: 0x00066FA8
	private void SetMissionData()
	{
		this.m_tMissionTitle.text = this.m_sMissionData.strMissionTitle;
		this.m_tPackTitle.text = this.m_sMissionData.strPackTitle;
		for (int i = 0; i < 3; i++)
		{
			int num = this.m_sMissionData.Song[i];
			this.m_arrSong[i].text = Singleton<SongManager>.instance.GetDiscInfo(num).Name;
			this.m_arrPattern[i].text = this.m_sMissionData.Pattern[i].ToString();
		}
		for (int j = 0; j < 3; j++)
		{
			string text = string.Concat(new string[]
			{
				this.m_sMissionData.ArrMissionType[j].ToString(),
				"_",
				this.m_sMissionData.ArrMissionTerm[j].ToString(),
				"_",
				this.m_sMissionData.ArrMissionCount[j].ToString()
			});
			this.m_arrMission[j].text = text;
		}
		this.GetMissionInfo();
	}

	// Token: 0x06000E96 RID: 3734 RVA: 0x00068ED4 File Offset: 0x000670D4
	private void GetMissionInfo()
	{
		WWWMissionUserRecordScript wwwmissionUserRecordScript = new WWWMissionUserRecordScript();
		wwwmissionUserRecordScript.strMissionID = this.m_sMissionData.strServerKey;
		Singleton<WWWManager>.instance.AddQueue(wwwmissionUserRecordScript);
	}

	// Token: 0x06000E97 RID: 3735 RVA: 0x0000CAB1 File Offset: 0x0000ACB1
	private void Update()
	{
		this.UpdateInput();
	}

	// Token: 0x06000E98 RID: 3736 RVA: 0x00068F04 File Offset: 0x00067104
	private void UpdateInput()
	{
		if (iPhoneToMouse.instance.Arcade)
		{
			NewMultiTouch.UpdateCount();
			if (0 < NewMultiTouch.Count)
			{
				TouchInfo touch = NewMultiTouch.GetTouch(0);
				Vector3 touchToPos = Singleton<GameManager>.instance.GetTouchToPos(touch);
				this.InputProcess(touchToPos, touch.m_ePhase);
			}
		}
		else if (0 < iPhoneToMouse.instance.touchCount)
		{
			iPhoneToMouse.pos touch2 = iPhoneToMouse.instance.GetTouch(0);
			this.InputProcess(touch2.position, touch2.phase);
		}
	}

	// Token: 0x06000E99 RID: 3737 RVA: 0x00068F8C File Offset: 0x0006718C
	private void InputProcess(Vector3 touch, TouchPhase ePhase)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(this.m_cCamera.ScreenPointToRay(touch), out raycastHit))
		{
			string name = raycastHit.collider.gameObject.name;
			if (ePhase == TouchPhase.Began)
			{
				if (name.Contains("Pack"))
				{
					this.m_oPackSelect.transform.position = raycastHit.collider.transform.position;
					for (int i = 0; i < 10; i++)
					{
						if (name.Contains(i.ToString()))
						{
							ArrayList allClubMission = Singleton<SongManager>.instance.AllClubMission;
							this.m_sPackData = (MissionPackData)allClubMission[i];
							this.m_iSelectMission = 0;
							this.m_sMissionData = (MissionData)this.m_sPackData.ArrMissionData[this.m_iSelectMission];
							this.SetMissionBtn();
							this.SetMissionData();
							break;
						}
					}
				}
				if (name.Contains("Mission"))
				{
					for (int j = 0; j < 5; j++)
					{
						if (name.Contains(j.ToString()))
						{
							this.m_iSelectMission = j;
							this.m_sMissionData = (MissionData)this.m_sPackData.ArrMissionData[this.m_iSelectMission];
							this.SetMissionBtn();
							this.SetMissionData();
							break;
						}
					}
				}
				if ("UpSpeed" == name)
				{
					int num = (int)this.TempSpeed;
					num++;
					if (num == 10)
					{
						num = 9;
					}
					this.TempSpeed = (EFFECTOR_SPEED)num;
					this.m_tTempSpeed.text = this.TempSpeed.ToString();
				}
				if ("DnSpeed" == name)
				{
					int num2 = (int)this.TempSpeed;
					num2--;
					if (-1 >= num2)
					{
						num2 = 0;
					}
					this.TempSpeed = (EFFECTOR_SPEED)num2;
					this.m_tTempSpeed.text = this.TempSpeed.ToString();
				}
				if ("TempLt" == name)
				{
					Vector3 localPosition = this.m_oPackSlot.transform.localPosition;
					localPosition.x -= (float)(Screen.width / 2);
					this.m_oPackSlot.transform.localPosition = localPosition;
				}
				if ("TempRt" == name)
				{
					Vector3 localPosition2 = this.m_oPackSlot.transform.localPosition;
					localPosition2.x += (float)(Screen.width / 2);
					this.m_oPackSlot.transform.localPosition = localPosition2;
				}
				if ("Play" == name)
				{
					Singleton<SongManager>.instance.Mode = GAMEMODE.MISSION;
					Singleton<SongManager>.instance.Mission = this.m_sMissionData;
					Singleton<SoundSourceManager>.instance.EFF_NUM = -1;
					GameData.SPEEDEFFECTOR = this.TempSpeed;
					if (this.m_sMissionData.Eff_Speed != EFFECTOR_SPEED.NONE)
					{
						GameData.SPEEDEFFECTOR = this.m_sMissionData.Eff_Speed;
					}
					GameData.FADEEFFCTOR = this.m_sMissionData.Eff_Fader;
					GameData.RANDEFFECTOR = this.m_sMissionData.Eff_Rand;
					Singleton<SceneSwitcher>.instance.LoadNextScene("game");
				}
			}
		}
	}

	// Token: 0x04000FC2 RID: 4034
	private const float PACKSLOT_LT = -500f;

	// Token: 0x04000FC3 RID: 4035
	private const float PACKSLOT_WIDTH = 250f;

	// Token: 0x04000FC4 RID: 4036
	private const float PACKSLOT_HEIGHT = -205f;

	// Token: 0x04000FC5 RID: 4037
	public GameObject PACK;

	// Token: 0x04000FC6 RID: 4038
	private Camera m_cCamera;

	// Token: 0x04000FC7 RID: 4039
	private GameObject m_oPackSelect;

	// Token: 0x04000FC8 RID: 4040
	private MissionPackData m_sPackData;

	// Token: 0x04000FC9 RID: 4041
	private int m_iSelectMission;

	// Token: 0x04000FCA RID: 4042
	private MissionData m_sMissionData;

	// Token: 0x04000FCB RID: 4043
	private GameObject m_oPackSlot;

	// Token: 0x04000FCC RID: 4044
	private UILabel m_tPackTitle;

	// Token: 0x04000FCD RID: 4045
	private UILabel m_tMissionTitle;

	// Token: 0x04000FCE RID: 4046
	private UILabel m_tReward;

	// Token: 0x04000FCF RID: 4047
	private UILabel[] m_arrSong = new UILabel[3];

	// Token: 0x04000FD0 RID: 4048
	private UILabel[] m_arrPattern = new UILabel[3];

	// Token: 0x04000FD1 RID: 4049
	private UILabel[] m_arrMission = new UILabel[3];

	// Token: 0x04000FD2 RID: 4050
	private UILabel m_tTempSpeed;

	// Token: 0x04000FD3 RID: 4051
	private EFFECTOR_SPEED TempSpeed = EFFECTOR_SPEED.X_1;
}
