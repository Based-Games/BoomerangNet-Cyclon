using System;
using UnityEngine;

// Token: 0x020001A4 RID: 420
public class CiManagerScript : MonoBehaviour
{
	// Token: 0x06000C84 RID: 3204 RVA: 0x000587D8 File Offset: 0x000569D8
	private void Awake()
	{
		Singleton<GameManager>.instance.PLAYUSER = false;
		this.m_aMovie = base.transform.FindChild("Movie").GetComponent<AVProWindowsMediaMovie>();
		if (!Singleton<GameManager>.instance.DemoSound)
		{
			this.m_aMovie._volume = 0f;
		}
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x0000B54F File Offset: 0x0000974F
	private void Start()
	{
		CardManager.eject();
		Singleton<GameManager>.instance.ActivieLed(LEDSTATE.WING_NOCOIN);
		if (!CiManagerScript.FirstStart)
		{
			bool incometest = GameData.INCOMETEST;
			CiManagerScript.FirstStart = true;
		}
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x0000B575 File Offset: 0x00009775
	private void CompleteLoad()
	{
		Singleton<SceneSwitcher>.instance.LoadNextScene("Title");
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x0000B586 File Offset: 0x00009786
	private void Update()
	{
		this.m_fTime += Time.deltaTime;
		if (this.m_aMovie.IsFinishedPlaying)
		{
			this.CompleteLoad();
		}
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x00003648 File Offset: 0x00001848
	private void OnDestroy()
	{
	}

	// Token: 0x04000C74 RID: 3188
	private float m_fTime;

	// Token: 0x04000C75 RID: 3189
	private AVProWindowsMediaMovie m_aMovie;

	// Token: 0x04000C76 RID: 3190
	public static bool FirstStart;
}
