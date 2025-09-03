using System;
using UnityEngine;

// Token: 0x020001A8 RID: 424
public class GameOverScript : MonoBehaviour
{
	// Token: 0x06000C9A RID: 3226 RVA: 0x00003648 File Offset: 0x00001848
	private void Start()
	{
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x0000B61A File Offset: 0x0000981A
	private void Update()
	{
		this.m_fTime += Time.deltaTime;
		if (2f < this.m_fTime)
		{
			Singleton<SceneSwitcher>.instance.LoadNextScene("Title");
		}
	}

	// Token: 0x04000C7F RID: 3199
	private float m_fTime;
}
