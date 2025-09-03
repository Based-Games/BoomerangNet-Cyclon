using System;
using UnityEngine;

// Token: 0x020001AB RID: 427
public class RankingManagerScript : MonoBehaviour
{
	// Token: 0x06000CA8 RID: 3240 RVA: 0x00058AF8 File Offset: 0x00056CF8
	private void Awake()
	{
		this.m_HausMixTitle = base.transform.FindChild("Title").FindChild("HausMix").GetComponent<TweenPosition>();
		this.m_RaveUpTitle = base.transform.FindChild("Title").FindChild("RaveUp").GetComponent<TweenPosition>();
		this.m_Blind = base.transform.FindChild("UI").FindChild("Sprite_Blind").GetComponent<TweenAlpha>();
		this.m_aMovie = GameObject.Find("Movie").GetComponent<AVProWindowsMediaMovie>();
		this.m_aMovie._folder = "../Movie/";
		this.m_aMovie._filename = "ranking.mp4";
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x00058BAC File Offset: 0x00056DAC
	private void Start()
	{
		if (Singleton<GameManager>.instance.DemoSound)
		{
			Singleton<SoundSourceManager>.instance.PlayRandomBgm("bgm_ranking", true);
			Singleton<SoundSourceManager>.instance.getNowBGM().volume = 1f;
		}
		base.Invoke("StartBlind", 2f);
		Singleton<GameManager>.instance.ActivieLed(LEDSTATE.WING_NOCOIN);
	}

	// Token: 0x06000CAA RID: 3242 RVA: 0x0000B6D9 File Offset: 0x000098D9
	private void StartBlind()
	{
		this.m_Blind.GetComponent<UISprite>().alpha = 1f;
		this.m_Blind.ResetToBeginning();
		this.m_Blind.Play(true);
	}

	// Token: 0x06000CAB RID: 3243 RVA: 0x00058C04 File Offset: 0x00056E04
	public void EndBlind()
	{
		this.m_Blind.GetComponent<UISprite>().alpha = 0f;
		this.m_Blind.from = 0f;
		this.m_Blind.to = 1f;
		this.m_Blind.ResetToBeginning();
		this.m_Blind.Play(true);
	}

	// Token: 0x06000CAC RID: 3244 RVA: 0x0000B707 File Offset: 0x00009907
	private void CompleteLoad()
	{
		Singleton<SceneSwitcher>.instance.LoadNextScene("CopyRight");
	}

	// Token: 0x06000CAD RID: 3245 RVA: 0x0000B718 File Offset: 0x00009918
	public void ChangeRanking()
	{
		this.m_HausMixTitle.Play(true);
		this.m_RaveUpTitle.Play(true);
	}

	// Token: 0x04000C84 RID: 3204
	private const float COMPLETE_SCENE = 5f;

	// Token: 0x04000C85 RID: 3205
	private TweenPosition m_HausMixTitle;

	// Token: 0x04000C86 RID: 3206
	private TweenPosition m_RaveUpTitle;

	// Token: 0x04000C87 RID: 3207
	private RankingPanel m_RankingPanel;

	// Token: 0x04000C88 RID: 3208
	private TweenAlpha m_Blind;

	// Token: 0x04000C89 RID: 3209
	private float m_fTime;

	// Token: 0x04000C8A RID: 3210
	private AVProWindowsMediaMovie m_aMovie;
}
