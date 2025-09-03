using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000186 RID: 390
public class ClubTourManager : MonoBehaviour
{
	// Token: 0x06000BA9 RID: 2985 RVA: 0x0005225C File Offset: 0x0005045C
	private void Awake()
	{
		this.m_ClubTourMissionInfoManager = base.transform.FindChild("Panel_MissionInfo").GetComponent<ClubTourMissionInfoManager>();
		Transform transform = base.transform.FindChild("Panel_MissionPack");
		this.m_ClubTourMissionPack = transform.FindChild("MissionPack").GetComponent<ClubTourMissionPack>();
		Transform transform2 = base.transform.FindChild("Panel_Missions").FindChild("UIScroll");
		this.m_tMissionGrid = transform2.FindChild("Grid");
		this.m_SelectBG = transform2.FindChild("SelectBG").GetComponent<TweenPosition>();
		this.m_gMissionCell = Resources.Load("Prefab/ClubTour/MissionCell") as GameObject;
		this.m_txGateTexture = base.transform.FindChild("Panel_GateTexture").FindChild("gateTexture").GetComponent<UITexture>();
		this.m_sTimer = base.transform.FindChild("Timer").GetComponent<TimerScript>();
	}

	// Token: 0x06000BAA RID: 2986 RVA: 0x00052344 File Offset: 0x00050544
	private void Start()
	{
		Singleton<SoundSourceManager>.instance.PlayNamedBgm("bgm_clubtour", true);
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_NARRATION_MISSION, false);
		this.m_aMovie = GameObject.Find("Movie").GetComponent<AVProWindowsMediaMovie>();
		this.m_aMovie._folder = "../Movie/";
		this.m_aMovie._filename = "clubtour.mp4";
		this.m_aMovie._loop = true;
		this.m_aMovie._volume = 0f;
		this.m_aMovie.SetElapsedTime(0f);
		this.m_aMovie.LoadMovie(true);
		this.CreateMission();
		this.m_sTimer.StartTimer(60, 10);
		this.m_sTimer.CallBackTimeover = new TimerScript.CompleteTimeOver(this.TimeOverStart);
		base.Invoke("FirstClick", 0.5f);
		base.Invoke("DestroyGate", 1.75f);
		Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("Picking a mission...", "Playing CLUB TOUR");
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x0000AD04 File Offset: 0x00008F04
	private void DestroyGate()
	{
		UnityEngine.Object.DestroyObject(this.m_txGateTexture.transform.parent.gameObject);
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x0000AD20 File Offset: 0x00008F20
	private void TimeOverStart()
	{
		this.m_ClubTourMissionInfoManager.PlayClick();
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x0000AD2D File Offset: 0x00008F2D
	public void FirstClick()
	{
		((GameObject)this.m_ArrMissionCell[0]).GetComponent<ClubTourMissionCell>().ClickProcess();
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x0005243C File Offset: 0x0005063C
	private void CreateMission()
	{
		ArrayList allClubMission = Singleton<SongManager>.instance.AllClubMission;
		for (int i = 0; i < allClubMission.Count; i++)
		{
			MissionPackData missionPackData = (MissionPackData)allClubMission[i];
			GameObject gameObject = UnityEngine.Object.Instantiate(this.m_gMissionCell, Vector3.zero, Quaternion.identity) as GameObject;
			gameObject.transform.parent = this.m_tMissionGrid;
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = new Vector3((float)i * this.m_v3MissionSize.x, 0f, 0f);
			gameObject.name = "MissionCell_" + i.ToString();
			ClubTourMissionCell component = gameObject.GetComponent<ClubTourMissionCell>();
			component.m_UIScroll = this.m_tMissionGrid.parent.GetComponent<UIScroll>();
			component.m_ClubTourManager = base.GetComponent<ClubTourManager>();
			component.m_MissionPackData = missionPackData;
			component.m_iNum = i;
			Singleton<GameManager>.instance.LoadEyeCatch(EYECATCHTYPE.MISSIONPACK_287, null, component.m_txMissionImage, null, missionPackData);
			component.LockCheck(Singleton<GameManager>.instance.UserData.Level);
			component.ClearMissionCheck();
			this.m_ArrMissionCell.Add(gameObject);
		}
		this.m_tMissionGrid.parent.GetComponent<UIScroll>().CellsSetting(0);
		this.m_tMissionGrid.parent.GetComponent<UIScroll>().transform.localPosition = Vector3.zero;
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x000525A4 File Offset: 0x000507A4
	public void MissionSetting(MissionPackData mpd, int index)
	{
		if (this.m_iSelectIndex == index)
		{
			return;
		}
		this.m_iSelectIndex = index;
		this.m_ClubTourMissionPack.setMissionPack(mpd);
		this.m_ClubTourMissionInfoManager.m_MissionPackData = mpd;
		this.m_ClubTourMissionInfoManager.MissionClick(ClubTourMissionInfoManager.MissionBtn_e.mission_1);
		this.m_ClubTourMissionInfoManager.MissionBtnClearCheck();
		this.m_ClubTourMissionInfoManager.MissionBtnLockCheck();
		this.m_ClubTourMissionInfoManager.BtnAni();
		this.SelectBGMove(index);
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x00052610 File Offset: 0x00050810
	private void SelectBGMove(int index)
	{
		for (int i = 0; i < this.m_ArrMissionCell.Count; i++)
		{
			ClubTourMissionCell component = ((GameObject)this.m_ArrMissionCell[i]).GetComponent<ClubTourMissionCell>();
			component.m_gSelectBG.SetActive(false);
			if (i == index)
			{
				component.m_gSelectBG.SetActive(true);
			}
		}
		this.m_SelectBG.enabled = false;
		this.m_SelectBG.from = this.m_SelectBG.transform.localPosition;
		this.m_SelectBG.to = new Vector3(((GameObject)this.m_ArrMissionCell[index]).transform.localPosition.x + this.m_tMissionGrid.localPosition.x, this.m_SelectBG.to.y, this.m_SelectBG.to.z);
		this.m_SelectBG.ResetToBeginning();
		this.m_SelectBG.Play(true);
	}

	// Token: 0x06000BB1 RID: 2993 RVA: 0x0000AD4A File Offset: 0x00008F4A
	private void OnDestroy()
	{
		Singleton<SoundSourceManager>.instance.StopBgm();
	}

	// Token: 0x04000B2A RID: 2858
	private ClubTourMissionInfoManager m_ClubTourMissionInfoManager;

	// Token: 0x04000B2B RID: 2859
	private ClubTourMissionPack m_ClubTourMissionPack;

	// Token: 0x04000B2C RID: 2860
	private ClubTourMyRecord m_ClubTourMyRecord;

	// Token: 0x04000B2D RID: 2861
	private Transform m_tMissionGrid;

	// Token: 0x04000B2E RID: 2862
	private TweenPosition m_SelectBG;

	// Token: 0x04000B2F RID: 2863
	private GameObject m_gMissionCell;

	// Token: 0x04000B30 RID: 2864
	private Vector3 m_v3MissionSize = new Vector3(250f, 176f, 0f);

	// Token: 0x04000B31 RID: 2865
	private ArrayList m_ArrMissionCell = new ArrayList();

	// Token: 0x04000B32 RID: 2866
	private UITexture m_txGateTexture;

	// Token: 0x04000B33 RID: 2867
	private int m_iSelectIndex = -1;

	// Token: 0x04000B34 RID: 2868
	private TimerScript m_sTimer;

	// Token: 0x04000B35 RID: 2869
	private AVProWindowsMediaMovie m_aMovie;
}
