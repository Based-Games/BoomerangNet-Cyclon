using System;
using UnityEngine;

// Token: 0x0200020F RID: 527
public class TitleManager : MonoBehaviour
{
	// Token: 0x06000F40 RID: 3904 RVA: 0x00003648 File Offset: 0x00001848
	private void Awake()
	{
	}

	// Token: 0x06000F41 RID: 3905 RVA: 0x0006D9D4 File Offset: 0x0006BBD4
	private void Init()
	{
		Singleton<GameManager>.instance.DEMOPLAYNUM = (Singleton<GameManager>.instance.DEMOPLAYNUM + 1) % 3;
		Singleton<GameManager>.instance.ONLOGIN = false;
		Singleton<GameManager>.instance.DEMOPLAY = false;
		GameObject gameObject = base.transform.FindChild("ControlObject").gameObject;
		this.m_Panel = gameObject.transform.FindChild("Panel_Btn").GetComponent<UIPanel>();
		this.m_PanelAni = gameObject.transform.FindChild("Panel_Ani").GetComponent<TweenScale>();
		this.m_gInsertCoin = this.m_Panel.transform.FindChild("InsertCoin").gameObject;
		this.m_gTouchToStart = this.m_Panel.transform.FindChild("TouchToStart").gameObject;
		this.m_TouchToStartimage = this.m_gTouchToStart.transform.FindChild("btn_Sprite_touchtostart").GetComponent<TweenAlpha>();
		this.m_TouchToStartEff = this.m_gTouchToStart.transform.FindChild("btn_Sprite_touchtostart_eff").GetComponent<TweenAlpha>();
		this.m_InsertCoinImage = this.m_gInsertCoin.transform.FindChild("btn_Sprite_insertcoin_image").GetComponent<TweenAlpha>();
		this.m_gInsertCoinEff_1 = this.m_gInsertCoin.transform.FindChild("btn_Sprite_insertcoin_eff_1").gameObject;
		this.m_gInsertCoinEff_2 = this.m_gInsertCoin.transform.FindChild("btn_Sprite_insertcoin_eff_2").gameObject;
		GameObject gameObject2 = base.transform.FindChild("PanelMovie").gameObject;
		this.m_aMovieTitle = gameObject2.transform.FindChild("MovieTitle").GetComponent<AVProWindowsMediaMovie>();
		this.m_aMovieLoop = gameObject2.transform.FindChild("MovieLoop").GetComponent<AVProWindowsMediaMovie>();
		this.m_aMovie = gameObject2.transform.FindChild("Texture").GetComponent<AVProWindowsMediaMaterialApply>();
		this.m_Blind = base.transform.FindChild("Blind").GetComponent<TweenAlpha>();
		Singleton<GameManager>.instance.SetSceneControl(base.gameObject);
		Singleton<WWWManager>.instance.GetGameCenterPoint();
		Singleton<WWWManager>.instance.GetEmergencyCheck();
		Singleton<WWWManager>.instance.GetNotice();
		Singleton<WWWManager>.instance.GetNewSongEventCheck();
	}

	// Token: 0x06000F42 RID: 3906 RVA: 0x0000D22A File Offset: 0x0000B42A
	private void SetPause(bool bEnabled)
	{
		this.m_bErrPopup = !bEnabled;
	}

	// Token: 0x06000F43 RID: 3907 RVA: 0x0000D236 File Offset: 0x0000B436
	private void FirstSoundPlay()
	{
		if (Singleton<GameManager>.instance.DemoSound)
		{
			Singleton<SoundSourceManager>.instance.PlayBgm(BGMINDEX.BGM_TITLE, false);
		}
	}

	// Token: 0x06000F44 RID: 3908 RVA: 0x0006DBF8 File Offset: 0x0006BDF8
	private void Start()
	{
		this.Init();
		this.FirstSoundPlay();
		this.m_aMovie._movie = this.m_aMovieTitle;
		this.m_aMovieTitle.LoadMovie(true);
		this.m_aMovieLoop.enabled = false;
		this.m_bSoundVolumDown = false;
		Singleton<GameManager>.instance.PLAYUSER = false;
		CardManager.eject();
		this.m_gTouchToStart.collider.enabled = false;
		this.m_gTouchToStart.SetActive(false);
		GameData.Stage = 0;
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x0006DC78 File Offset: 0x0006BE78
	private void ClickProcess()
	{
		if (this.m_bisStartState && Singleton<GameManager>.instance.CanPlayCheck() && this.m_gTouchToStart.collider.enabled)
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_TITLE_START_TITLE, false);
			Singleton<SoundSourceManager>.instance.getNowBGM().volume = 1f;
			Singleton<SoundSourceManager>.instance.StopBgm();
			Singleton<SceneSwitcher>.instance.LoadNextScene("warning");
			base.StopCoroutine("ViewUi");
		}
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x0006DCF0 File Offset: 0x0006BEF0
	private void PanelAniUpdate()
	{
		Vector4 baseClipRegion = this.m_Panel.baseClipRegion;
		this.m_Panel.baseClipRegion = new Vector4(baseClipRegion.x, baseClipRegion.y, this.m_PanelAni.transform.localScale.x, baseClipRegion.w);
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x0006DD40 File Offset: 0x0006BF40
	public void StartColActive()
	{
		if (this.m_bEndAni)
		{
			this.m_gTouchToStart.collider.enabled = true;
			this.m_TouchToStartEff.enabled = true;
			return;
		}
		this.m_bEndAni = true;
		this.m_PanelAni.from = this.m_PanelAni.transform.localScale;
		this.m_PanelAni.to = new Vector3(400f, 1f, 1f);
		this.m_PanelAni.ResetToBeginning();
		this.m_PanelAni.Play(true);
		this.m_gInsertCoin.collider.enabled = false;
		this.m_gTouchToStart.SetActive(true);
		this.m_TouchToStartimage.enabled = true;
		this.m_TouchToStartEff.enabled = true;
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x0000D250 File Offset: 0x0000B450
	public void TouchToStartAlpha()
	{
		this.m_TouchToStartimage.GetComponent<UISprite>().alpha = 0.3f;
	}

	// Token: 0x06000F49 RID: 3913 RVA: 0x0006DE00 File Offset: 0x0006C000
	private void DemoCheck()
	{
		if (Singleton<GameManager>.instance.DemoSound)
		{
			Singleton<SoundSourceManager>.instance.getNowBGM().volume = 1f;
		}
		else
		{
			Singleton<SoundSourceManager>.instance.getNowBGM().volume = 0f;
		}
		this.m_fDemoframe += Time.deltaTime;
		if (!this.m_bFadeOut && this.m_fDemoStartTime - 5f < this.m_fDemoframe)
		{
			this.m_bFadeOut = true;
			this.m_bSoundVolumDown = true;
			this.m_fDemoframe = 0f;
			this.m_Blind.gameObject.SetActive(true);
			base.Invoke("TutorialScene", 1f);
			return;
		}
		if (this.m_aMovie._movie.IsFinishedPlaying)
		{
			this.PlayLoopMovie();
		}
	}

	// Token: 0x06000F4A RID: 3914 RVA: 0x0006DEC4 File Offset: 0x0006C0C4
	private void SoundDown()
	{
		if (Singleton<GameManager>.instance.DemoSound)
		{
			if (!this.m_bSoundVolumDown)
			{
				return;
			}
			if (Singleton<SoundSourceManager>.instance.getNowBGM().volume > 0f)
			{
				Singleton<SoundSourceManager>.instance.getNowBGM().volume = Singleton<SoundSourceManager>.instance.getNowBGM().volume - Time.deltaTime;
				return;
			}
			Singleton<SoundSourceManager>.instance.StopBgm();
		}
	}

	// Token: 0x06000F4B RID: 3915 RVA: 0x0006DF2C File Offset: 0x0006C12C
	private void TitleSoundCheck()
	{
		if (Singleton<GameManager>.instance.DemoSound)
		{
			if (3.75f <= Singleton<SoundSourceManager>.instance.getNowBGM().time)
			{
				this.m_bisSoundJump = true;
			}
			if (this.m_bisSoundJump && !Singleton<SoundSourceManager>.instance.getNowBGM().isPlaying)
			{
				Singleton<SoundSourceManager>.instance.PlayBgm(BGMINDEX.BGM_TITLE, false);
				Singleton<SoundSourceManager>.instance.getNowBGM().time = 3.75f;
			}
		}
	}

	// Token: 0x06000F4C RID: 3916 RVA: 0x0000D267 File Offset: 0x0000B467
	private void UpdateKeyBoard()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			this.ClickProcess();
		}
	}

	// Token: 0x06000F4D RID: 3917 RVA: 0x0006DF9C File Offset: 0x0006C19C
	private void Update()
	{
		this.UpdateKeyBoard();
		this.UpdataCard();
		if (!this.m_bErrPopup)
		{
			this.SoundLoopCheck();
			return;
		}
		this.SoundLoopCheck();
		this.DemoCheck();
		this.SoundDown();
		if (!this.m_Blind.gameObject.activeSelf)
		{
			this.TitleSoundCheck();
		}
		this.PanelAniUpdate();
		if (Singleton<GameManager>.instance.HaveCredit() && !this.m_bisStartState)
		{
			this.m_PanelAni.Play(true);
			this.m_bisStartState = true;
			this.m_InsertCoinImage.ResetToBeginning();
			this.m_InsertCoinImage.enabled = true;
			this.m_gInsertCoinEff_1.gameObject.SetActive(false);
		}
		if (this.m_aMovie._movie.IsFinishedPlaying)
		{
			this.PlayLoopMovie();
		}
	}

	// Token: 0x06000F4E RID: 3918 RVA: 0x0006E05C File Offset: 0x0006C25C
	private void SoundLoopCheck()
	{
		if (Singleton<GameManager>.instance.DemoSound && this.m_bLoopingMovie && Singleton<SoundSourceManager>.instance.getNowBGM().time >= 0f && Singleton<SoundSourceManager>.instance.getNowBGM().time < 3.75f)
		{
			Singleton<SoundSourceManager>.instance.PlayBgm(BGMINDEX.BGM_TITLE, false);
			Singleton<SoundSourceManager>.instance.getNowBGM().time = 3.75f;
		}
	}

	// Token: 0x06000F4F RID: 3919 RVA: 0x0006E0CC File Offset: 0x0006C2CC
	private void PlayLoopMovie()
	{
		if (!this.m_bLoopingMovie)
		{
			this.m_bLoopingMovie = true;
			this.m_aMovieTitle.enabled = false;
			this.m_aMovieLoop.enabled = true;
			this.m_aMovieLoop.LoadMovie(true);
			this.m_aMovie._movie = this.m_aMovieLoop;
		}
	}

	// Token: 0x06000F50 RID: 3920 RVA: 0x0000D278 File Offset: 0x0000B478
	private void OnDestroy()
	{
		Singleton<SoundSourceManager>.instance.getNowBGM().volume = 1f;
		Singleton<SoundSourceManager>.instance.StopBgm();
	}

	// Token: 0x06000F51 RID: 3921 RVA: 0x0000D298 File Offset: 0x0000B498
	private void UpdataCard()
	{
		if (CardManagerR1.m_bInit && CardManager.isInsertCard())
		{
			this.ClickProcess();
		}
	}

	// Token: 0x06000F52 RID: 3922 RVA: 0x0000D2AE File Offset: 0x0000B4AE
	private void TutorialScene()
	{
		if (Singleton<GameManager>.instance.DemoSound)
		{
			Singleton<SoundSourceManager>.instance.StopBgm();
			Singleton<SoundSourceManager>.instance.getNowBGM().volume = 1f;
		}
		Singleton<SceneSwitcher>.instance.LoadNextScene("Tutorial");
	}

	// Token: 0x040010EC RID: 4332
	private UIPanel m_Panel;

	// Token: 0x040010ED RID: 4333
	private TweenScale m_PanelAni;

	// Token: 0x040010EE RID: 4334
	private GameObject m_gInsertCoin;

	// Token: 0x040010EF RID: 4335
	private GameObject m_gTouchToStart;

	// Token: 0x040010F0 RID: 4336
	private TweenAlpha m_TouchToStartimage;

	// Token: 0x040010F1 RID: 4337
	private TweenAlpha m_TouchToStartEff;

	// Token: 0x040010F2 RID: 4338
	private TweenAlpha m_InsertCoinImage;

	// Token: 0x040010F3 RID: 4339
	private GameObject m_gInsertCoinEff_1;

	// Token: 0x040010F4 RID: 4340
	private GameObject m_gInsertCoinEff_2;

	// Token: 0x040010F5 RID: 4341
	private TweenAlpha m_Blind;

	// Token: 0x040010F6 RID: 4342
	private float m_fDemoStartTime = 60f;

	// Token: 0x040010F7 RID: 4343
	private float m_fDemoframe;

	// Token: 0x040010F8 RID: 4344
	private bool m_bisStartState;

	// Token: 0x040010F9 RID: 4345
	private bool m_bEndAni;

	// Token: 0x040010FA RID: 4346
	private bool m_bFadeOut;

	// Token: 0x040010FB RID: 4347
	private bool m_bMovieLoop;

	// Token: 0x040010FC RID: 4348
	private bool m_bisSoundJump;

	// Token: 0x040010FD RID: 4349
	private AVProWindowsMediaMovie m_aMovieTitle;

	// Token: 0x040010FE RID: 4350
	private AVProWindowsMediaMovie m_aMovieLoop;

	// Token: 0x040010FF RID: 4351
	private AVProWindowsMediaMaterialApply m_aMovie;

	// Token: 0x04001100 RID: 4352
	private bool m_bSoundVolumDown;

	// Token: 0x04001101 RID: 4353
	private bool m_bLoopingMovie;

	// Token: 0x04001102 RID: 4354
	private bool m_bErrPopup = true;
}
