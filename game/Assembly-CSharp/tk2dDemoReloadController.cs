using System;
using UnityEngine;

// Token: 0x020002D1 RID: 721
[AddComponentMenu("2D Toolkit/Demo/tk2dDemoReloadController")]
public class tk2dDemoReloadController : MonoBehaviour
{
	// Token: 0x06001522 RID: 5410 RVA: 0x00012005 File Offset: 0x00010205
	private void Reload()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}
