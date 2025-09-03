using System;
using UnityEngine;

// Token: 0x020001F8 RID: 504
public class ModeSelectScript : MonoBehaviour
{
	// Token: 0x06000E9B RID: 3739 RVA: 0x00069270 File Offset: 0x00067470
	private void Awake()
	{
		this.Popup = base.transform.FindChild("Popup").gameObject;
		this.NoCardUserPopup = this.Popup.transform.FindChild("Popup_NoCardUserEn").GetComponent<TweenRotation>();
		this.m_sTimer = base.transform.FindChild("Timer").GetComponent<TimerScript>();
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_COMMON_TIMER);
		this.SetMovie();
	}

	// Token: 0x06000E9C RID: 3740 RVA: 0x000692E8 File Offset: 0x000674E8
	private void SetMovie()
	{
		this.m_iMovieIdx = 0;
		this.m_mMovie = base.transform.FindChild("Texture").GetComponent<AVProWindowsMediaMaterialApply>();
		this.m_aMovie = base.transform.FindChild("MovieFirst").GetComponent<AVProWindowsMediaMovie>();
		this.m_mMovie._movie = this.m_aMovie;
	}

	// Token: 0x06000E9D RID: 3741 RVA: 0x00069344 File Offset: 0x00067544
	private void PlayMovie(ModeSelectScript.MODEMOVIE eMode)
	{
		string[] array = new string[] { "ModeSelect_HausMix.mov", "ModeSelect_RaveUp.mov", "ModeSelect_ClubTour.mov", "Gate_HausMix.mov", "Gate_RaveUp.mov", "Gate_ClubTour.mov", "Gate_Tutorial.mov" };
		this.m_aMovie._filename = array[(int)eMode];
		this.m_aMovie._folder = "../Movie/";
		this.m_aMovie.LoadMovie(true);
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x000693BC File Offset: 0x000675BC
	private void Start()
	{
		Singleton<SoundSourceManager>.instance.PlayNamedBgm("bgm_modesel", true);
		Singleton<SongManager>.instance.Mode = GAMEMODE.HAUSMIX;
		this.m_isSelectMode = ModeSelectScript.ModeKind_e.HausMix;
		this.PlayMovie(ModeSelectScript.MODEMOVIE.HAUSMIX);
		this.m_sTimer.StartTimer(15, 5);
		this.m_sTimer.CallBackTimeover = new TimerScript.CompleteTimeOver(this.GameStart);
	}

	// Token: 0x06000E9F RID: 3743 RVA: 0x0000CAD3 File Offset: 0x0000ACD3
	public void HausMixSelect()
	{
		if (this.m_bStartClickState)
		{
			return;
		}
		if (this.m_bisTimeOver)
		{
			return;
		}
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_MODESELECT_TOUCH_HAUS, false);
		if (this.m_isSelectMode == ModeSelectScript.ModeKind_e.HausMix)
		{
			return;
		}
		this.m_isSelectMode = ModeSelectScript.ModeKind_e.HausMix;
		this.PlayMovie(ModeSelectScript.MODEMOVIE.HAUSMIX);
	}

	// Token: 0x06000EA0 RID: 3744 RVA: 0x0000CB0B File Offset: 0x0000AD0B
	public void RaveUpSelect()
	{
		if (this.m_bStartClickState)
		{
			return;
		}
		if (this.m_bisTimeOver)
		{
			return;
		}
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_MODESELECT_TOUCH_RAVEUP, false);
		if (this.m_isSelectMode == ModeSelectScript.ModeKind_e.RaveUp)
		{
			return;
		}
		this.m_isSelectMode = ModeSelectScript.ModeKind_e.RaveUp;
		this.PlayMovie(ModeSelectScript.MODEMOVIE.RAVEUP);
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x00069418 File Offset: 0x00067618
	private void PopupShow()
	{
		this.Popup.SetActive(true);
		this.NoCardUserPopup.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
		this.NoCardUserPopup.ResetToBeginning();
		this.NoCardUserPopup.Play(true);
		base.CancelInvoke("PopupHide");
		base.Invoke("PopupHide", 2f);
		this.Popup.transform.FindChild("Popup_NoCardUser").gameObject.SetActive(false);
	}

	// Token: 0x06000EA2 RID: 3746 RVA: 0x0000CB44 File Offset: 0x0000AD44
	private void PopupHide()
	{
		this.Popup.SetActive(false);
	}

	// Token: 0x06000EA3 RID: 3747 RVA: 0x000694A8 File Offset: 0x000676A8
	public void ClubTourSelect()
	{
		if (GameData.INCOMETEST)
		{
			return;
		}
		if (this.m_bStartClickState)
		{
			return;
		}
		if (!Singleton<GameManager>.instance.ONLOGIN)
		{
			this.PopupShow();
			return;
		}
		if (this.m_bisTimeOver)
		{
			return;
		}
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_MODESELECT_TOUCH_CLUBTOUR, false);
		if (this.m_isSelectMode == ModeSelectScript.ModeKind_e.ClubTour)
		{
			return;
		}
		this.m_isSelectMode = ModeSelectScript.ModeKind_e.ClubTour;
		this.PlayMovie(ModeSelectScript.MODEMOVIE.CLUBTOUR);
	}

	// Token: 0x06000EA4 RID: 3748 RVA: 0x00069508 File Offset: 0x00067708
	public void GameStart()
	{
		this.PopupHide();
		if (this.m_bStartClickState)
		{
			return;
		}
		this.m_bStartClickState = true;
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_COMMON_TIMER);
		Singleton<SoundSourceManager>.instance.StopBgm();
		Singleton<SongManager>.instance.Mode = (GAMEMODE)this.m_isSelectMode;
		GameData.Stage = 0;
		USERDATA userData = Singleton<GameManager>.instance.UserData;
		if (0 >= userData.TotalExp)
		{
			this.m_isSelectMode = ModeSelectScript.ModeKind_e.Tutorial;
		}
		SOUNDINDEX soundindex = SOUNDINDEX.SFX_TITLE_START_ALRIGHT;
		switch (this.m_isSelectMode)
		{
		case ModeSelectScript.ModeKind_e.HausMix:
			soundindex = SOUNDINDEX.SFX_MODESELECT_IN_HAUS;
			this.PlayMovie(ModeSelectScript.MODEMOVIE.GATE_HAUSMIX);
			break;
		case ModeSelectScript.ModeKind_e.RaveUp:
			soundindex = SOUNDINDEX.SFX_MODESELECT_IN_RAVEUP;
			this.PlayMovie(ModeSelectScript.MODEMOVIE.GATE_RAVEUP);
			break;
		case ModeSelectScript.ModeKind_e.ClubTour:
			soundindex = SOUNDINDEX.SFX_MODESELECT_IN_CLUBTOUR;
			this.PlayMovie(ModeSelectScript.MODEMOVIE.GATE_CLUBTOUR);
			break;
		case ModeSelectScript.ModeKind_e.Tutorial:
			this.PlayMovie(ModeSelectScript.MODEMOVIE.GATE_TUTORIAL);
			break;
		}
		this.m_sTimer.StopTimer();
		Singleton<SoundSourceManager>.instance.Play(soundindex, false);
		base.Invoke("NextScene", 4f);
	}

	// Token: 0x06000EA5 RID: 3749 RVA: 0x000695E8 File Offset: 0x000677E8
	private void NextScene()
	{
		switch (this.m_isSelectMode)
		{
		case ModeSelectScript.ModeKind_e.HausMix:
			Singleton<SceneSwitcher>.instance.LoadNextScene("HausMix");
			Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("Picking a song...", "Playing HAUS MIX");
			return;
		case ModeSelectScript.ModeKind_e.RaveUp:
			Singleton<SceneSwitcher>.instance.LoadNextScene("RaveUp");
			Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("Picking a disc-set...", "Playing RAVE UP");
			return;
		case ModeSelectScript.ModeKind_e.ClubTour:
			Singleton<SceneSwitcher>.instance.LoadNextScene("ClubTour");
			Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("Picking a mission...", "Playing CLUB TOUR");
			return;
		case ModeSelectScript.ModeKind_e.Tutorial:
			Singleton<SceneSwitcher>.instance.LoadNextScene("PlayTutorial");
			return;
		default:
			return;
		}
	}

	// Token: 0x06000EA6 RID: 3750 RVA: 0x0000CB52 File Offset: 0x0000AD52
	private void TimeOver()
	{
		this.m_bisTimeOver = true;
		this.GameStart();
	}

	// Token: 0x06000EA7 RID: 3751 RVA: 0x0000CB61 File Offset: 0x0000AD61
	private void ClickPopup()
	{
		this.PopupHide();
	}

	// Token: 0x06000EA8 RID: 3752 RVA: 0x00003648 File Offset: 0x00001848
	private void Update()
	{
	}

	// Token: 0x06000EA9 RID: 3753 RVA: 0x00069690 File Offset: 0x00067890
	public void WorldGpSelect()
	{
		if (GameData.INCOMETEST)
		{
			return;
		}
		if (this.m_bStartClickState)
		{
			return;
		}
		if (!Singleton<GameManager>.instance.ONLOGIN)
		{
			this.PopupShow();
			return;
		}
		if (this.m_bisTimeOver)
		{
			return;
		}
		if (this.m_bClickDelayState)
		{
			return;
		}
		Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_MODESELECT_TOUCH_WORLDGP, false);
		this.m_isSelectMode = ModeSelectScript.ModeKind_e.ClubTour;
		this.PlayMovie(ModeSelectScript.MODEMOVIE.CLUBTOUR);
	}

	// Token: 0x04000FD4 RID: 4052
	private const int IDX_MODE_HAUSMIX = 0;

	// Token: 0x04000FD5 RID: 4053
	private const int IDX_MODE_RAVEUP = 1;

	// Token: 0x04000FD6 RID: 4054
	private const int IDX_MODE_CLUBMISSION = 2;

	// Token: 0x04000FD7 RID: 4055
	private const int IDX_GATE_HAUSMIX = 3;

	// Token: 0x04000FD8 RID: 4056
	private const int IDX_GATE_RAVEUP = 4;

	// Token: 0x04000FD9 RID: 4057
	private const int IDX_GATE_CLUBMISSION = 5;

	// Token: 0x04000FDA RID: 4058
	private const int MAX_TEMPMOVIE = 2;

	// Token: 0x04000FDB RID: 4059
	private float m_fClickDelayTime = 0.5f;

	// Token: 0x04000FDC RID: 4060
	private ModeSelectScript.ModeKind_e m_isSelectMode = ModeSelectScript.ModeKind_e.None;

	// Token: 0x04000FDD RID: 4061
	private bool m_bisTimeOver;

	// Token: 0x04000FDE RID: 4062
	private bool m_bClickDelayState;

	// Token: 0x04000FDF RID: 4063
	private bool m_bStartClickState;

	// Token: 0x04000FE0 RID: 4064
	private AVProWindowsMediaMovie m_aMovie;

	// Token: 0x04000FE1 RID: 4065
	private AVProWindowsMediaMaterialApply m_mMovie;

	// Token: 0x04000FE2 RID: 4066
	private int m_iMovieIdx;

	// Token: 0x04000FE3 RID: 4067
	private GameObject Popup;

	// Token: 0x04000FE4 RID: 4068
	private TweenRotation NoCardUserPopup;

	// Token: 0x04000FE5 RID: 4069
	private TimerScript m_sTimer;

	// Token: 0x020001F9 RID: 505
	public enum ModeKind_e
	{
		// Token: 0x04000FE7 RID: 4071
		None = -1,
		// Token: 0x04000FE8 RID: 4072
		HausMix,
		// Token: 0x04000FE9 RID: 4073
		RaveUp,
		// Token: 0x04000FEA RID: 4074
		ClubTour,
		// Token: 0x04000FEB RID: 4075
		Tutorial
	}

	// Token: 0x020001FA RID: 506
	private enum MODEMOVIE
	{
		// Token: 0x04000FED RID: 4077
		HAUSMIX,
		// Token: 0x04000FEE RID: 4078
		RAVEUP,
		// Token: 0x04000FEF RID: 4079
		CLUBTOUR,
		// Token: 0x04000FF0 RID: 4080
		GATE_HAUSMIX,
		// Token: 0x04000FF1 RID: 4081
		GATE_RAVEUP,
		// Token: 0x04000FF2 RID: 4082
		GATE_CLUBTOUR,
		// Token: 0x04000FF3 RID: 4083
		MAX = 7,
		// Token: 0x04000FF4 RID: 4084
		GATE_TUTORIAL = 6
	}
}
