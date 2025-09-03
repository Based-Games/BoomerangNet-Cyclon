using System;
using UnityEngine;

// Token: 0x02000240 RID: 576
[Serializable]
public class tk2dTextMeshData
{
	// Token: 0x04001234 RID: 4660
	public int version;

	// Token: 0x04001235 RID: 4661
	public tk2dFontData font;

	// Token: 0x04001236 RID: 4662
	public string text = string.Empty;

	// Token: 0x04001237 RID: 4663
	public Color color = Color.white;

	// Token: 0x04001238 RID: 4664
	public Color color2 = Color.white;

	// Token: 0x04001239 RID: 4665
	public bool useGradient;

	// Token: 0x0400123A RID: 4666
	public int textureGradient;

	// Token: 0x0400123B RID: 4667
	public TextAnchor anchor = TextAnchor.LowerLeft;

	// Token: 0x0400123C RID: 4668
	public int renderLayer;

	// Token: 0x0400123D RID: 4669
	public Vector3 scale = Vector3.one;

	// Token: 0x0400123E RID: 4670
	public bool kerning;

	// Token: 0x0400123F RID: 4671
	public int maxChars = 16;

	// Token: 0x04001240 RID: 4672
	public bool inlineStyling;

	// Token: 0x04001241 RID: 4673
	public bool formatting;

	// Token: 0x04001242 RID: 4674
	public int wordWrapWidth;

	// Token: 0x04001243 RID: 4675
	public float spacing;

	// Token: 0x04001244 RID: 4676
	public float lineSpacing;
}
