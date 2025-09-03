using System;

// Token: 0x02000154 RID: 340
public class WWWDocumentData : WWWObject
{
	// Token: 0x06000AB6 RID: 2742 RVA: 0x0000A5D3 File Offset: 0x000087D3
	public override void StartLoad()
	{
		this.www = WWWRequest.Create(this.strPath, null);
	}

	// Token: 0x06000AB7 RID: 2743 RVA: 0x0004C334 File Offset: 0x0004A534
	public override void CompleteLoad()
	{
		if (this.CheckCancel)
		{
			return;
		}
		if (this.www.error != null)
		{
			Logger.Warn("WWWDocument", "Request failed: {0}, {1}", new object[]
			{
				this.strPath,
				this.www.error
			});
			return;
		}
		if (!this.www.isDone)
		{
			return;
		}
		string text = this.www.text;
		Logger.Log("WWWDocument", "Status: {0}, {1}", new object[] { this.strPath, text });
		if (this.CallReturnBack != null)
		{
			this.CallReturnBack(text);
		}
	}
}
