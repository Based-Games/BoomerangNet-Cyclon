using System;
using System.Collections.Generic;

// Token: 0x02000068 RID: 104
[Serializable]
public class BMGlyph
{
	// Token: 0x0600027A RID: 634 RVA: 0x0001D7EC File Offset: 0x0001B9EC
	public int GetKerning(int previousChar)
	{
		if (this.kerning != null)
		{
			int i = 0;
			int count = this.kerning.Count;
			while (i < count)
			{
				if (this.kerning[i] == previousChar)
				{
					return this.kerning[i + 1];
				}
				i += 2;
			}
		}
		return 0;
	}

	// Token: 0x0600027B RID: 635 RVA: 0x0001D844 File Offset: 0x0001BA44
	public void SetKerning(int previousChar, int amount)
	{
		if (this.kerning == null)
		{
			this.kerning = new List<int>();
		}
		for (int i = 0; i < this.kerning.Count; i += 2)
		{
			if (this.kerning[i] == previousChar)
			{
				this.kerning[i + 1] = amount;
				return;
			}
		}
		this.kerning.Add(previousChar);
		this.kerning.Add(amount);
	}

	// Token: 0x0600027C RID: 636 RVA: 0x0001D8C0 File Offset: 0x0001BAC0
	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		int num = this.x + this.width;
		int num2 = this.y + this.height;
		if (this.x < xMin)
		{
			int num3 = xMin - this.x;
			this.x += num3;
			this.width -= num3;
			this.offsetX += num3;
		}
		if (this.y < yMin)
		{
			int num4 = yMin - this.y;
			this.y += num4;
			this.height -= num4;
			this.offsetY += num4;
		}
		if (num > xMax)
		{
			this.width -= num - xMax;
		}
		if (num2 > yMax)
		{
			this.height -= num2 - yMax;
		}
	}

	// Token: 0x0400027F RID: 639
	public int index;

	// Token: 0x04000280 RID: 640
	public int x;

	// Token: 0x04000281 RID: 641
	public int y;

	// Token: 0x04000282 RID: 642
	public int width;

	// Token: 0x04000283 RID: 643
	public int height;

	// Token: 0x04000284 RID: 644
	public int offsetX;

	// Token: 0x04000285 RID: 645
	public int offsetY;

	// Token: 0x04000286 RID: 646
	public int advance;

	// Token: 0x04000287 RID: 647
	public int channel;

	// Token: 0x04000288 RID: 648
	public List<int> kerning;
}
