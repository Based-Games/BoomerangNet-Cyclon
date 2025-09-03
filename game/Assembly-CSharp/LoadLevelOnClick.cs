using System;
using UnityEngine;

// Token: 0x02000022 RID: 34
[AddComponentMenu("NGUI/Examples/Load Level On Click")]
public class LoadLevelOnClick : MonoBehaviour
{
	// Token: 0x060000ED RID: 237 RVA: 0x00003EC1 File Offset: 0x000020C1
	private void OnClick()
	{
		if (!string.IsNullOrEmpty(this.levelName))
		{
			Application.LoadLevel(this.levelName);
		}
	}

	// Token: 0x040000D4 RID: 212
	public string levelName;
}
