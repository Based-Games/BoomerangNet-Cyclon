using System;

// Token: 0x0200022D RID: 557
public class TinyXmlReader
{
	// Token: 0x06001003 RID: 4099 RVA: 0x0000DAB2 File Offset: 0x0000BCB2
	public TinyXmlReader(string newXmlString)
	{
		this.xmlString = newXmlString;
	}

	// Token: 0x06001004 RID: 4100 RVA: 0x00072FE4 File Offset: 0x000711E4
	public bool Read()
	{
		this.idx = this.xmlString.IndexOf("<", this.idx);
		if (this.idx == -1)
		{
			return false;
		}
		this.idx++;
		int num = this.xmlString.IndexOf(">", this.idx);
		if (num == -1)
		{
			return false;
		}
		this.tagName = this.xmlString.Substring(this.idx, num - this.idx);
		this.idx = num;
		if (this.tagName.StartsWith("/"))
		{
			this.isOpeningTag = false;
			this.tagName = this.tagName.Remove(0, 1);
		}
		else
		{
			this.isOpeningTag = true;
		}
		if (this.isOpeningTag)
		{
			int num2 = this.xmlString.IndexOf("<", this.idx);
			if (num2 == -1)
			{
				return false;
			}
			this.content = this.xmlString.Substring(this.idx + 1, num2 - this.idx - 1);
			this.content = this.content.Trim();
		}
		return true;
	}

	// Token: 0x06001005 RID: 4101 RVA: 0x0007310C File Offset: 0x0007130C
	public bool Read(string endingTag)
	{
		bool flag = this.Read();
		if (this.tagName == endingTag && !this.isOpeningTag)
		{
			flag = false;
		}
		return flag;
	}

	// Token: 0x0400119D RID: 4509
	private string xmlString = string.Empty;

	// Token: 0x0400119E RID: 4510
	private int idx;

	// Token: 0x0400119F RID: 4511
	public string tagName = string.Empty;

	// Token: 0x040011A0 RID: 4512
	public bool isOpeningTag;

	// Token: 0x040011A1 RID: 4513
	public string content = string.Empty;
}
