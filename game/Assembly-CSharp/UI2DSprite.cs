using System;
using UnityEngine;

// Token: 0x0200009B RID: 155
[AddComponentMenu("NGUI/UI/NGUI Unity2D Sprite")]
[ExecuteInEditMode]
public class UI2DSprite : UIWidget
{
	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00006DF6 File Offset: 0x00004FF6
	// (set) Token: 0x060004D2 RID: 1234 RVA: 0x00006DFE File Offset: 0x00004FFE
	public Sprite sprite2D
	{
		get
		{
			return this.mSprite;
		}
		set
		{
			if (this.mSprite != value)
			{
				base.RemoveFromPanel();
				this.mSprite = value;
				this.nextSprite = null;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00006E2B File Offset: 0x0000502B
	// (set) Token: 0x060004D4 RID: 1236 RVA: 0x00006E33 File Offset: 0x00005033
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

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00026B5C File Offset: 0x00024D5C
	// (set) Token: 0x060004D6 RID: 1238 RVA: 0x00006E60 File Offset: 0x00005060
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
				base.RemoveFromPanel();
				this.mShader = value;
				if (this.mMat == null)
				{
					this.mPMA = -1;
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00006E9E File Offset: 0x0000509E
	public override Texture mainTexture
	{
		get
		{
			if (this.mSprite != null)
			{
				return this.mSprite.texture;
			}
			if (this.mMat != null)
			{
				return this.mMat.mainTexture;
			}
			return null;
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00026BB0 File Offset: 0x00024DB0
	public bool premultipliedAlpha
	{
		get
		{
			if (this.mPMA == -1)
			{
				Shader shader = this.shader;
				this.mPMA = ((!(shader != null) || !shader.name.Contains("Premultiplied")) ? 0 : 1);
			}
			return this.mPMA == 1;
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00026C08 File Offset: 0x00024E08
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

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x060004DA RID: 1242 RVA: 0x00026D98 File Offset: 0x00024F98
	public Rect uvRect
	{
		get
		{
			Texture mainTexture = this.mainTexture;
			if (mainTexture != null)
			{
				Rect textureRect = this.mSprite.textureRect;
				textureRect.xMin /= (float)mainTexture.width;
				textureRect.xMax /= (float)mainTexture.width;
				textureRect.yMin /= (float)mainTexture.height;
				textureRect.yMax /= (float)mainTexture.height;
				return textureRect;
			}
			return new Rect(0f, 0f, 1f, 1f);
		}
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x00026E34 File Offset: 0x00025034
	protected override void OnUpdate()
	{
		if (this.nextSprite != null)
		{
			if (this.nextSprite != this.mSprite)
			{
				this.sprite2D = this.nextSprite;
			}
			this.nextSprite = null;
		}
		base.OnUpdate();
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x00026E84 File Offset: 0x00025084
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

	// Token: 0x060004DD RID: 1245 RVA: 0x00026EE0 File Offset: 0x000250E0
	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Color color = base.color;
		color.a = this.finalAlpha;
		Color32 color2 = ((!this.premultipliedAlpha) ? color : NGUITools.ApplyPMA(color));
		Vector4 drawingDimensions = this.drawingDimensions;
		Rect uvRect = this.uvRect;
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.y));
		verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.w));
		verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.y));
		uvs.Add(new Vector2(uvRect.xMin, uvRect.yMin));
		uvs.Add(new Vector2(uvRect.xMin, uvRect.yMax));
		uvs.Add(new Vector2(uvRect.xMax, uvRect.yMax));
		uvs.Add(new Vector2(uvRect.xMax, uvRect.yMin));
		cols.Add(color2);
		cols.Add(color2);
		cols.Add(color2);
		cols.Add(color2);
	}

	// Token: 0x04000391 RID: 913
	[SerializeField]
	[HideInInspector]
	private Sprite mSprite;

	// Token: 0x04000392 RID: 914
	[HideInInspector]
	[SerializeField]
	private Material mMat;

	// Token: 0x04000393 RID: 915
	[SerializeField]
	[HideInInspector]
	private Shader mShader;

	// Token: 0x04000394 RID: 916
	public Sprite nextSprite;

	// Token: 0x04000395 RID: 917
	private int mPMA = -1;
}
