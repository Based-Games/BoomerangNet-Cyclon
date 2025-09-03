using System;
using UnityEngine;

// Token: 0x02000180 RID: 384
public class WWWObject
{
	// Token: 0x06000B8D RID: 2957 RVA: 0x00003648 File Offset: 0x00001848
	public virtual void StartLoad()
	{
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x0000ABA1 File Offset: 0x00008DA1
	public virtual void CompleteLoad()
	{
		if (this.CheckCancel)
		{
			Logger.Log("WWWObject", "Request canceled", new object[0]);
			return;
		}
	}

	// Token: 0x04000B1B RID: 2843
	public WWW www;

	// Token: 0x04000B1C RID: 2844
	public string strPath = string.Empty;

	// Token: 0x04000B1D RID: 2845
	public float m_tTime;

	// Token: 0x04000B1E RID: 2846
	public bool CheckCancel;

	// Token: 0x04000B1F RID: 2847
	public WWWObject.CompleteCallBack CallBack;

	// Token: 0x04000B20 RID: 2848
	public WWWObject.CompleteCallBack CallBackFail;

	// Token: 0x04000B21 RID: 2849
	public WWWObject.CompleteReturnCallBack CallReturnBack;

	// Token: 0x04000B22 RID: 2850
	public WWWObject.CompleteTexttureCallBack CallBackTexture;

	// Token: 0x04000B23 RID: 2851
	public WWWObject.CompleteMovieCallBack CallBackMovie;

	// Token: 0x02000181 RID: 385
	// (Invoke) Token: 0x06000B90 RID: 2960
	public delegate void CompleteCallBack();

	// Token: 0x02000182 RID: 386
	// (Invoke) Token: 0x06000B94 RID: 2964
	public delegate void CompleteReturnCallBack(string strContent);

	// Token: 0x02000183 RID: 387
	// (Invoke) Token: 0x06000B98 RID: 2968
	public delegate void CompleteTexttureCallBack(Texture tTexture);

	// Token: 0x02000184 RID: 388
	// (Invoke) Token: 0x06000B9C RID: 2972
	public delegate void CompleteMovieCallBack(TextAsset tMt);
}
