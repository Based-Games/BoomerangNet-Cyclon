using System;
using UnityEngine;

// Token: 0x02000377 RID: 887
public class SceneSwitcher : MonoBehaviour
{
	// Token: 0x1700033B RID: 827
	// (get) Token: 0x0600152B RID: 5419 RVA: 0x0001202F File Offset: 0x0001022F
	public static SceneSwitcher Instance
	{
		get
		{
			return SceneSwitcher.instance;
		}
	}

	// Token: 0x0600152C RID: 5420 RVA: 0x0008D4A4 File Offset: 0x0008B6A4
	private void Awake()
	{
		if (SceneSwitcher.instance != null && SceneSwitcher.instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SceneSwitcher.instance = this;
		Logger.Log("SceneSwitcher", "SceneSwitcher initialized", new object[0]);
	}

	// Token: 0x0600152D RID: 5421 RVA: 0x00012036 File Offset: 0x00010236
	public void LoadNextScene(string nextSceneName)
	{
		this.LoadScene(nextSceneName);
	}

	// Token: 0x0600152F RID: 5423 RVA: 0x0008D4F4 File Offset: 0x0008B6F4
	private void LoadScene(string nextSceneName)
	{
		base.CancelInvoke();
		Singleton<SoundSourceManager>.instance.Stop(SOUNDINDEX.SFX_COMMON_TIMER);
		Singleton<SoundSourceManager>.instance.StopBgm();
		if (Singleton<GameManager>.instance.ONNETWORK)
		{
			Singleton<GameManager>.instance.ClosePopUp();
		}
		base.StartCoroutine(Singleton<GameManager>.instance.SetMark(nextSceneName));
		if ("HausMixResult" == GameData.S_CURSCENE)
		{
			Singleton<GameManager>.instance.UserData.SetViewValue();
		}
		Singleton<GameManager>.instance.SaveData(nextSceneName);
		if (nextSceneName == "Title")
		{
			GC.GetTotalMemory(true);
			GC.Collect();
			Logger.Log("SceneSwitcher", "Garbage collected", new object[0]);
		}
		GameData.S_CURSCENE = nextSceneName;
		Resources.UnloadUnusedAssets();
		Application.LoadLevel(nextSceneName);
		Logger.Log("SceneSwitcher", "Moved to scene: " + nextSceneName, new object[0]);
	}

	// Token: 0x04001913 RID: 6419
	private static SceneSwitcher instance;
}
