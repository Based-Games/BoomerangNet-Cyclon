using System;
using UnityEngine;

// Token: 0x020000C6 RID: 198
[AddComponentMenu("NGUI/UI/NGUI Texture")]
[ExecuteInEditMode]
public class UITexture : UIWidget
{
	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06000672 RID: 1650 RVA: 0x000082E1 File Offset: 0x000064E1
	// (set) Token: 0x06000673 RID: 1651 RVA: 0x000082E9 File Offset: 0x000064E9
	public override Texture mainTexture
	{
		get
		{
			return this.mTexture;
		}
		set
		{
			if (this.mTexture != value)
			{
				base.RemoveFromPanel();
				this.mTexture = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06000674 RID: 1652 RVA: 0x0000830F File Offset: 0x0000650F
	// (set) Token: 0x06000675 RID: 1653 RVA: 0x00008317 File Offset: 0x00006517
	public override Material material
	{
		get
		{
			return this.mMat;
		}
		set
		{
			if (this.mMat != value)
			{
				base.RemoveFromPanel();
				this.mMat = value;
				this.mPMA = -1;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000676 RID: 1654 RVA: 0x00032F04 File Offset: 0x00031104
	// (set) Token: 0x06000677 RID: 1655 RVA: 0x00008344 File Offset: 0x00006544
	public override Shader shader
	{
		get
		{
			if (this.mMat != null)
			{
				return this.mMat.shader;
			}
			if (this.mShader == null)
			{
				this.mShader = Shader.Find("Unlit/Transparent Colored");
			}
			return this.mShader;
		}
		set
		{
			if (this.mShader != value)
			{
				this.mShader = value;
				if (this.mMat == null)
				{
					this.mPMA = -1;
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06000678 RID: 1656 RVA: 0x00032F58 File Offset: 0x00031158
	public bool premultipliedAlpha
	{
		get
		{
			if (this.mPMA == -1)
			{
				Material material = this.material;
				this.mPMA = ((!(material != null) || !(material.shader != null) || !material.shader.name.Contains("Premultiplied")) ? 0 : 1);
			}
			return this.mPMA == 1;
		}
	}

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000679 RID: 1657 RVA: 0x0000837C File Offset: 0x0000657C
	// (set) Token: 0x0600067A RID: 1658 RVA: 0x00008384 File Offset: 0x00006584
	public Rect uvRect
	{
		get
		{
			return this.mRect;
		}
		set
		{
			if (this.mRect != value)
			{
				this.mRect = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x0600067B RID: 1659 RVA: 0x00032FC8 File Offset: 0x000311C8
	public override Vector4 drawingDimensions
	{
		get
		{
			Vector2 pivotOffset = base.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			Texture mainTexture = this.mainTexture;
			int num5 = ((!(mainTexture != null)) ? this.mWidth : mainTexture.width);
			int num6 = ((!(mainTexture != null)) ? this.mHeight : mainTexture.height);
			if ((num5 & 1) != 0)
			{
				num3 -= 1f / (float)num5 * (float)this.mWidth;
			}
			if ((num6 & 1) != 0)
			{
				num4 -= 1f / (float)num6 * (float)this.mHeight;
			}
			return new Vector4((this.mDrawRegion.x != 0f) ? Mathf.Lerp(num, num3, this.mDrawRegion.x) : num, (this.mDrawRegion.y != 0f) ? Mathf.Lerp(num2, num4, this.mDrawRegion.y) : num2, (this.mDrawRegion.z != 1f) ? Mathf.Lerp(num, num3, this.mDrawRegion.z) : num3, (this.mDrawRegion.w != 1f) ? Mathf.Lerp(num2, num4, this.mDrawRegion.w) : num4);
		}
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x00033158 File Offset: 0x00031358
	public override void MakePixelPerfect()
	{
		Texture mainTexture = this.mainTexture;
		if (mainTexture != null)
		{
			int num = mainTexture.width;
			if ((num & 1) == 1)
			{
				num++;
			}
			int num2 = mainTexture.height;
			if ((num2 & 1) == 1)
			{
				num2++;
			}
			base.width = num;
			base.height = num2;
		}
		base.MakePixelPerfect();
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x000331B4 File Offset: 0x000313B4
	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Color color = base.color;
		color.a = this.finalAlpha;
		Color32 color2 = ((!this.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color));
		Vector4 drawingDimensions = this.drawingDimensions;
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.y));
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.y));
		uvs.Add(new Vector2(this.mRect.xMin, this.mRect.yMin));
		uvs.Add(new Vector2(this.mRect.xMin, this.mRect.yMax));
		uvs.Add(new Vector2(this.mRect.xMax, this.mRect.yMax));
		uvs.Add(new Vector2(this.mRect.xMax, this.mRect.yMin));
		cols.Add(color2);
		cols.Add(color2);
		cols.Add(color2);
		cols.Add(color2);
	}

	// Token: 0x0400050B RID: 1291
	[HideInInspector]
	[SerializeField]
	private Rect mRect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x0400050C RID: 1292
	[SerializeField]
	[HideInInspector]
	private Texture mTexture;

	// Token: 0x0400050D RID: 1293
	[HideInInspector]
	[SerializeField]
	private Material mMat;

	// Token: 0x0400050E RID: 1294
	[SerializeField]
	[HideInInspector]
	private Shader mShader;

	// Token: 0x0400050F RID: 1295
	private int mPMA = -1;
}
