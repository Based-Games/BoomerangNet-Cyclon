using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000067 RID: 103
[Serializable]
public class BMFont
{
	// Token: 0x17000056 RID: 86
	// (get) Token: 0x06000269 RID: 617 RVA: 0x0000540E File Offset: 0x0000360E
	public bool isValid
	{
		get
		{
			return this.mSaved.Count > 0;
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x0600026A RID: 618 RVA: 0x0000541E File Offset: 0x0000361E
	// (set) Token: 0x0600026B RID: 619 RVA: 0x00005426 File Offset: 0x00003626
	public int charSize
	{
		get
		{
			return this.mSize;
		}
		set
		{
			this.mSize = value;
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x0600026C RID: 620 RVA: 0x0000542F File Offset: 0x0000362F
	// (set) Token: 0x0600026D RID: 621 RVA: 0x00005437 File Offset: 0x00003637
	public int baseOffset
	{
		get
		{
			return this.mBase;
		}
		set
		{
			this.mBase = value;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x0600026E RID: 622 RVA: 0x00005440 File Offset: 0x00003640
	// (set) Token: 0x0600026F RID: 623 RVA: 0x00005448 File Offset: 0x00003648
	public int texWidth
	{
		get
		{
			return this.mWidth;
		}
		set
		{
			this.mWidth = value;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x06000270 RID: 624 RVA: 0x00005451 File Offset: 0x00003651
	// (set) Token: 0x06000271 RID: 625 RVA: 0x00005459 File Offset: 0x00003659
	public int texHeight
	{
		get
		{
			return this.mHeight;
		}
		set
		{
			this.mHeight = value;
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x06000272 RID: 626 RVA: 0x00005462 File Offset: 0x00003662
	public int glyphCount
	{
		get
		{
			return (!this.isValid) ? 0 : this.mSaved.Count;
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06000273 RID: 627 RVA: 0x00005480 File Offset: 0x00003680
	// (set) Token: 0x06000274 RID: 628 RVA: 0x00005488 File Offset: 0x00003688
	public string spriteName
	{
		get
		{
			return this.mSpriteName;
		}
		set
		{
			this.mSpriteName = value;
		}
	}

	// Token: 0x06000275 RID: 629 RVA: 0x0001D6FC File Offset: 0x0001B8FC
	public BMGlyph GetGlyph(int index, bool createIfMissing)
	{
		BMGlyph bmglyph = null;
		if (this.mDict.Count == 0)
		{
			int i = 0;
			int count = this.mSaved.Count;
			while (i < count)
			{
				BMGlyph bmglyph2 = this.mSaved[i];
				this.mDict.Add(bmglyph2.index, bmglyph2);
				i++;
			}
		}
		if (!this.mDict.TryGetValue(index, out bmglyph) && createIfMissing)
		{
			bmglyph = new BMGlyph();
			bmglyph.index = index;
			this.mSaved.Add(bmglyph);
			this.mDict.Add(index, bmglyph);
		}
		return bmglyph;
	}

	// Token: 0x06000276 RID: 630 RVA: 0x00005491 File Offset: 0x00003691
	public BMGlyph GetGlyph(int index)
	{
		return this.GetGlyph(index, false);
	}

	// Token: 0x06000277 RID: 631 RVA: 0x0000549B File Offset: 0x0000369B
	public void Clear()
	{
		this.mDict.Clear();
		this.mSaved.Clear();
	}

	// Token: 0x06000278 RID: 632 RVA: 0x0001D798 File Offset: 0x0001B998
	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		if (this.isValid)
		{
			int i = 0;
			int count = this.mSaved.Count;
			while (i < count)
			{
				BMGlyph bmglyph = this.mSaved[i];
				if (bmglyph != null)
				{
					bmglyph.Trim(xMin, yMin, xMax, yMax);
				}
				i++;
			}
		}
	}

	// Token: 0x04000278 RID: 632
	[HideInInspector]
	[SerializeField]
	private int mSize;

	// Token: 0x04000279 RID: 633
	[HideInInspector]
	[SerializeField]
	private int mBase;

	// Token: 0x0400027A RID: 634
	[HideInInspector]
	[SerializeField]
	private int mWidth;

	// Token: 0x0400027B RID: 635
	[SerializeField]
	[HideInInspector]
	private int mHeight;

	// Token: 0x0400027C RID: 636
	[SerializeField]
	[HideInInspector]
	private string mSpriteName;

	// Token: 0x0400027D RID: 637
	[SerializeField]
	[HideInInspector]
	private List<BMGlyph> mSaved = new List<BMGlyph>();

	// Token: 0x0400027E RID: 638
	private Dictionary<int, BMGlyph> mDict = new Dictionary<int, BMGlyph>();
}
