using System;
using System.Collections;
using System.IO;
using UnityEngine;

// Token: 0x020001A5 RID: 421
public class CopyRightScript : MonoBehaviour
{
	// Token: 0x06000C8A RID: 3210 RVA: 0x00058828 File Offset: 0x00056A28
	private void Start()
	{
		Singleton<GameManager>.instance.InitNetData();
		Singleton<GameManager>.instance.ActivieLed(LEDSTATE.WING_ONPOWER);
		GameObject gameObject = base.transform.FindChild("TextureEn").gameObject;
		gameObject.SetActive(true);
		base.StartCoroutine(this.LoadPng(gameObject.GetComponent<UITexture>()));
		GameData.E_VERSION = VERSION.EN;
		Logger.Log("CopyRightScript", "USE OF THIS SOFTWARE IS PROVIDED WITH NO WARRANTY!", new object[0]);
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x0000B5AD File Offset: 0x000097AD
	private void Update()
	{
		this.m_fTime += Time.deltaTime;
		if (4f < this.m_fTime)
		{
			Singleton<SceneSwitcher>.instance.LoadNextScene("Ci");
		}
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x0000B5DD File Offset: 0x000097DD
	private IEnumerator LoadPng(UITexture uTexture)
	{
		WWW www = new WWW("file:///" + Path.GetFullPath("../Data/System/Warning.png"));
		while (!www.isDone)
		{
		}
		uTexture.mainTexture = www.texture;
		yield break;
	}

	// Token: 0x04000C77 RID: 3191
	private const float NEXT_SCENETIME = 4f;

	// Token: 0x04000C78 RID: 3192
	private float m_fTime;
}
