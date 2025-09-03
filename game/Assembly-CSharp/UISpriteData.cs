using System;

// Token: 0x020000C0 RID: 192
[Serializable]
public class UISpriteData
{
	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06000653 RID: 1619 RVA: 0x000080B5 File Offset: 0x000062B5
	public bool hasBorder
	{
		get
		{
			return (this.borderLeft | this.borderRight | this.borderTop | this.borderBottom) != 0;
		}
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06000654 RID: 1620 RVA: 0x000080D8 File Offset: 0x000062D8
	public bool hasPadding
	{
		get
		{
			return (this.paddingLeft | this.paddingRight | this.paddingTop | this.paddingBottom) != 0;
		}
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x000080FB File Offset: 0x000062FB
	public void SetRect(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x0000811A File Offset: 0x0000631A
	public void SetPadding(int left, int bottom, int right, int top)
	{
		this.paddingLeft = left;
		this.paddingBottom = bottom;
		this.paddingRight = right;
		this.paddingTop = top;
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x00008139 File Offset: 0x00006339
	public void SetBorder(int left, int bottom, int right, int top)
	{
		this.borderLeft = left;
		this.borderBottom = bottom;
		this.borderRight = right;
		this.borderTop = top;
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x00032028 File Offset: 0x00030228
	public void CopyFrom(UISpriteData sd)
	{
		this.name = sd.name;
		this.x = sd.x;
		this.y = sd.y;
		this.width = sd.width;
		this.height = sd.height;
		this.borderLeft = sd.borderLeft;
		this.borderRight = sd.borderRight;
		this.borderTop = sd.borderTop;
		this.borderBottom = sd.borderBottom;
		this.paddingLeft = sd.paddingLeft;
		this.paddingRight = sd.paddingRight;
		this.paddingTop = sd.paddingTop;
		this.paddingBottom = sd.paddingBottom;
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x00008158 File Offset: 0x00006358
	public void CopyBorderFrom(UISpriteData sd)
	{
		this.borderLeft = sd.borderLeft;
		this.borderRight = sd.borderRight;
		this.borderTop = sd.borderTop;
		this.borderBottom = sd.borderBottom;
	}

	// Token: 0x040004D7 RID: 1239
	public string name = "Sprite";

	// Token: 0x040004D8 RID: 1240
	public int x;

	// Token: 0x040004D9 RID: 1241
	public int y;

	// Token: 0x040004DA RID: 1242
	public int width;

	// Token: 0x040004DB RID: 1243
	public int height;

	// Token: 0x040004DC RID: 1244
	public int borderLeft;

	// Token: 0x040004DD RID: 1245
	public int borderRight;

	// Token: 0x040004DE RID: 1246
	public int borderTop;

	// Token: 0x040004DF RID: 1247
	public int borderBottom;

	// Token: 0x040004E0 RID: 1248
	public int paddingLeft;

	// Token: 0x040004E1 RID: 1249
	public int paddingRight;

	// Token: 0x040004E2 RID: 1250
	public int paddingTop;

	// Token: 0x040004E3 RID: 1251
	public int paddingBottom;
}
