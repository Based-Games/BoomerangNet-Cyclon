using System;
using UnityEngine;

// Token: 0x020001AD RID: 429
public class TutorialScript : MonoBehaviour
{
	// Token: 0x06000CBA RID: 3258 RVA: 0x0000B78B File Offset: 0x0000998B
	private void Awake()
	{
		this.m_aMovie = base.transform.FindChild("Movie").GetComponent<AVProWindowsMediaMovie>();
		this.m_aMovie._folder = "../Movie/";
		this.m_aMovie._filename = "Tutorial.wmv";
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x00059054 File Offset: 0x00057254
	private void Start()
	{
		Singleton<GameManager>.instance.ActivieLed(LEDSTATE.WING_NOCOIN);
		if (Singleton<GameManager>.instance.DemoSound)
		{
			Singleton<SoundSourceManager>.instance.PlayBgm(BGMINDEX.BGM_TUTO, false);
			Singleton<SoundSourceManager>.instance.getNowBGM().time = 1f;
		}
		this.m_Blind = base.transform.FindChild("Sprite_Blind").GetComponent<TweenAlpha>();
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x0000B7C8 File Offset: 0x000099C8
	private void CompleteLoad()
	{
		Singleton<GameManager>.instance.DEMOPLAY = true;
		Singleton<SceneSwitcher>.instance.LoadNextScene("game");
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x000590B4 File Offset: 0x000572B4
	private void Update()
	{
		this.m_fTime += Time.deltaTime;
		if (this.m_aMovie.IsFinishedPlaying)
		{
			if (this.m_bFinish)
			{
				return;
			}
			this.m_bFinish = true;
			this.m_Blind.from = 0f;
			this.m_Blind.to = 1f;
			this.m_Blind.delay = 0f;
			this.m_Blind.ResetToBeginning();
			this.m_Blind.Play(true);
			base.Invoke("CompleteLoad", 1f);
		}
	}

	// Token: 0x04000C93 RID: 3219
	private float m_fTime;

	// Token: 0x04000C94 RID: 3220
	private AVProWindowsMediaMovie m_aMovie;

	// Token: 0x04000C95 RID: 3221
	private TweenAlpha m_Blind;

	// Token: 0x04000C96 RID: 3222
	private bool m_bFinish;
}
