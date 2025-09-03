using System;
using UnityEngine;

// Token: 0x020001AA RID: 426
public class PlayTutorialScript : MonoBehaviour
{
	// Token: 0x06000CA1 RID: 3233 RVA: 0x00058948 File Offset: 0x00056B48
	private void Awake()
	{
		this.m_aMovie = base.transform.FindChild("Movie").GetComponent<AVProWindowsMediaMovie>();
		this.m_aMovie._folder = "../Movie/";
		this.m_aMovie._filename = "Tutorial.wmv";
		Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("Watching the tutorial", null);
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x0000B672 File Offset: 0x00009872
	private void Start()
	{
		Singleton<GameManager>.instance.ActivieLed(LEDSTATE.WING_OUTGAME);
		Singleton<SoundSourceManager>.instance.PlayBgm(BGMINDEX.BGM_TUTO, false);
		Singleton<SoundSourceManager>.instance.getNowBGM().time = 1f;
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x000589A0 File Offset: 0x00056BA0
	private void CompleteLoad()
	{
		GameObject.Find("Sprite").SetActive(false);
		Singleton<SoundSourceManager>.instance.SetBgmTime(0f);
		Singleton<SoundSourceManager>.instance.StopBgm();
		base.Invoke("RunScene", 4f);
		switch (Singleton<SongManager>.instance.Mode)
		{
		case GAMEMODE.HAUSMIX:
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_MODESELECT_IN_HAUS, false);
			this.gameMode = "HausMix";
			this.PlayGate(0);
			Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("Picking a song...", "Playing HAUS MIX");
			return;
		case GAMEMODE.RAVEUP:
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_MODESELECT_IN_RAVEUP, false);
			this.gameMode = "RaveUp";
			this.PlayGate(1);
			Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("Picking a disc-set...", "Playing RAVE UP");
			return;
		case GAMEMODE.MISSION:
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_MODESELECT_IN_CLUBTOUR, false);
			this.gameMode = "ClubTour";
			this.PlayGate(2);
			Singleton<DiscordRichPresenceController>.instance.UpdateDiscordPresence("Picking a mission...", "Playing CLUB TOUR");
			return;
		default:
			return;
		}
	}

	// Token: 0x06000CA4 RID: 3236 RVA: 0x0000B6A0 File Offset: 0x000098A0
	private void Update()
	{
		this.m_fTime += Time.deltaTime;
		if (this.m_aMovie.IsFinishedPlaying)
		{
			this.CompleteLoad();
		}
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x00058AA0 File Offset: 0x00056CA0
	private void PlayGate(int eMode)
	{
		string[] array = new string[] { "Gate_HausMix.mov", "Gate_RaveUp.mov", "Gate_ClubTour.mov" };
		this.m_aMovie._filename = array[eMode];
		this.m_aMovie._folder = "../Movie/";
		this.m_aMovie.LoadMovie(true);
	}

	// Token: 0x06000CA6 RID: 3238 RVA: 0x0000B6C7 File Offset: 0x000098C7
	private void RunScene()
	{
		Singleton<SceneSwitcher>.instance.LoadNextScene(this.gameMode);
	}

	// Token: 0x04000C81 RID: 3201
	private float m_fTime;

	// Token: 0x04000C82 RID: 3202
	private AVProWindowsMediaMovie m_aMovie;

	// Token: 0x04000C83 RID: 3203
	private string gameMode;
}
