using System;
using UnityEngine;

// Token: 0x020001D9 RID: 473
public class TweenAlphaContorl : MonoBehaviour
{
	// Token: 0x06000DC2 RID: 3522 RVA: 0x000625DC File Offset: 0x000607DC
	private void Awake()
	{
		for (int i = 0; i < this.AlphaObj.Length; i++)
		{
			if (this.AlphaObj[i].activeSelf && this.AlphaObj[i].GetComponent<TweenAlpha>() != null)
			{
				this.AlphaObj[i].GetComponent<TweenAlpha>().enabled = false;
			}
		}
	}

	// Token: 0x06000DC3 RID: 3523 RVA: 0x00062640 File Offset: 0x00060840
	public void PlayObj(bool st)
	{
		for (int i = 0; i < this.AlphaObj.Length; i++)
		{
			this.AlphaObj[i].GetComponent<TweenAlpha>().enabled = true;
			this.AlphaObj[i].GetComponent<TweenAlpha>().duration = this.Duration;
			this.AlphaObj[i].GetComponent<TweenAlpha>().Play(st);
		}
	}

	// Token: 0x04000E2C RID: 3628
	public GameObject[] AlphaObj;

	// Token: 0x04000E2D RID: 3629
	public float Duration = 0.5f;
}
