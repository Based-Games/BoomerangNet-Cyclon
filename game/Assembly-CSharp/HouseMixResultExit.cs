using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020001CF RID: 463
public class HouseMixResultExit : MonoBehaviour
{
	// Token: 0x06000D8B RID: 3467 RVA: 0x0000BF30 File Offset: 0x0000A130
	private void Start()
	{
		base.Invoke("ClickProcess", 25f);
	}

	// Token: 0x06000D8C RID: 3468 RVA: 0x0000BF42 File Offset: 0x0000A142
	[Conditional("AUTOTEST")]
	private void AutoTest()
	{
		if (!this.AutoSelect)
		{
			base.Invoke("SelectSong", 3f);
			this.AutoSelect = true;
		}
	}

	// Token: 0x06000D8D RID: 3469 RVA: 0x0000BF63 File Offset: 0x0000A163
	private void SelectSong()
	{
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_SCORE_COUNT);
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_ACCURACY_COUNT);
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_BP_COUNT);
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_EXP_BAR);
		this.ClickProcess();
	}

	// Token: 0x06000D8E RID: 3470 RVA: 0x00003648 File Offset: 0x00001848
	private void TouchClick()
	{
	}

	// Token: 0x06000D8F RID: 3471 RVA: 0x0000BF9B File Offset: 0x0000A19B
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0) && base.collider.enabled)
		{
			Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_LOGIN_LOGIN_BUTTON, false);
			this.ClickProcess();
		}
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x0000BFC9 File Offset: 0x0000A1C9
	public void ColliderOn()
	{
		base.collider.enabled = true;
	}

	// Token: 0x06000D91 RID: 3473 RVA: 0x00061240 File Offset: 0x0005F440
	public void ClickProcess()
	{
		base.CancelInvoke("ClickProcess");
		if (this.isRaveUpResult)
		{
			if (Singleton<GameManager>.instance.RaveUpHurdleFail)
			{
				Singleton<SongManager>.instance.GetRaveUpAlbumStage(Singleton<SongManager>.instance.SelectAlbumId);
				GameData.Stage = 0;
			}
			Singleton<SceneSwitcher>.instance.LoadNextScene("ThanksForPlaying");
			return;
		}
		GameData.Stage++;
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_RESULT_SCORE_COUNT);
		if (Singleton<SongManager>.instance.Mode != GAMEMODE.HAUSMIX)
		{
			Singleton<SceneSwitcher>.instance.LoadNextScene("ThanksForPlaying");
			return;
		}
		if (GameData.Stage < 3)
		{
			Singleton<SceneSwitcher>.instance.LoadNextScene("HausMix");
			return;
		}
		if (!this.isAllClear)
		{
			Singleton<SceneSwitcher>.instance.LoadNextScene("HausMixAllClearResult");
			return;
		}
		Singleton<SceneSwitcher>.instance.LoadNextScene("ThanksForPlaying");
	}

	// Token: 0x04000DE4 RID: 3556
	public bool isAllClear;

	// Token: 0x04000DE5 RID: 3557
	public bool isRaveUpResult;

	// Token: 0x04000DE6 RID: 3558
	private bool m_bClickState;

	// Token: 0x04000DE7 RID: 3559
	private bool AutoSelect;
}
