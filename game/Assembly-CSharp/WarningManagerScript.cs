using System;
using System.Collections;
using System.IO;
using UnityEngine;

// Token: 0x020001AE RID: 430
public class WarningManagerScript : MonoBehaviour
{
	// Token: 0x06000CBF RID: 3263 RVA: 0x00059148 File Offset: 0x00057348
	private void Start()
	{
		Singleton<GameManager>.instance.ActivieLed(LEDSTATE.WING_OUTGAME);
		Singleton<SoundSourceManager>.instance.StopBgm();
		GameObject gameObject = base.transform.FindChild("TextureEn").gameObject;
		gameObject.SetActive(true);
		base.StartCoroutine(this.LoadPng(gameObject.GetComponent<UITexture>()));
	}

	// Token: 0x06000CC0 RID: 3264 RVA: 0x0000B7E4 File Offset: 0x000099E4
	private void Update()
	{
		this.m_fTime += Time.deltaTime;
		if (3f < this.m_fTime)
		{
			Singleton<SceneSwitcher>.instance.LoadNextScene("CardLogin");
		}
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x0000B814 File Offset: 0x00009A14
	private IEnumerator LoadPng(UITexture uTexture)
	{
		WWW www = new WWW("file:///" + Path.GetFullPath("../Data/System/Caution.png"));
		while (!www.isDone)
		{
		}
		uTexture.mainTexture = www.texture;
		yield break;
	}

	// Token: 0x04000C97 RID: 3223
	private float m_fTime;
}
