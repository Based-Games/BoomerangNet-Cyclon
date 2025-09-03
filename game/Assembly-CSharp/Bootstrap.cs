using System;
using UnityEngine;

// Token: 0x0200048F RID: 1167
public class Bootstrap : MonoBehaviour
{
	// Token: 0x06001577 RID: 5495 RVA: 0x0008E7F8 File Offset: 0x0008C9F8
	private void Awake()
	{
		if (Bootstrap.instance != null)
		{
			Logger.Log("Bootstrap", "Duplicate Bootstrap instance detected, destroying this one.", new object[0]);
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Bootstrap.instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Logger.Log("Bootstrap", "Initialized Bootstrap", new object[0]);
		Camera camera = Camera.main;
		if (camera == null)
		{
			Logger.Warn("Bootstrap", "No main camera found, creating new camera.", new object[0]);
			camera = new GameObject("MainCamera", new Type[] { typeof(Camera) })
			{
				tag = "MainCamera"
			}.GetComponent<Camera>();
		}
		AspectUtility aspectUtility = camera.GetComponent<AspectUtility>();
		if (aspectUtility == null)
		{
			Logger.Log("Bootstrap", "Adding AspectUtility to GameObject: {0}", new object[] { camera.gameObject.name });
			aspectUtility = camera.gameObject.AddComponent<AspectUtility>();
		}
		else
		{
			Logger.Log("Bootstrap", "AspectUtility already attached to GameObject: {0}", new object[] { camera.gameObject.name });
		}
		aspectUtility.enabled = true;
		camera.gameObject.SetActive(true);
		Logger.Log("Bootstrap", "Bootstrap initialized AspectUtility", new object[0]);
	}

	// Token: 0x04001CE0 RID: 7392
	private static Bootstrap instance;
}
