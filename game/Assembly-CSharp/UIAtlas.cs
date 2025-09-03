using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200009E RID: 158
[AddComponentMenu("NGUI/UI/Atlas")]
public class UIAtlas : MonoBehaviour
{
	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x060004E5 RID: 1253 RVA: 0x00006F85 File Offset: 0x00005185
	// (set) Token: 0x060004E6 RID: 1254 RVA: 0x00027730 File Offset: 0x00025930
	public Material spriteMaterial
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.material : this.mReplacement.spriteMaterial;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteMaterial = value;
			}
			else if (this.material == null)
			{
				this.mPMA = 0;
				this.material = value;
			}
			else
			{
				this.MarkAsChanged();
				this.mPMA = -1;
				this.material = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x060004E7 RID: 1255 RVA: 0x000277A0 File Offset: 0x000259A0
	public bool premultipliedAlpha
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.premultipliedAlpha;
			}
			if (this.mPMA == -1)
			{
				Material spriteMaterial = this.spriteMaterial;
				this.mPMA = ((!(spriteMaterial != null) || !(spriteMaterial.shader != null) || !spriteMaterial.shader.name.Contains("Premultiplied")) ? 0 : 1);
			}
			return this.mPMA == 1;
		}
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x060004E8 RID: 1256 RVA: 0x00006FAE File Offset: 0x000051AE
	// (set) Token: 0x060004E9 RID: 1257 RVA: 0x00006FEA File Offset: 0x000051EA
	public List<UISpriteData> spriteList
	{
		get
		{
			if (this.mReplacement != null)
			{
				return this.mReplacement.spriteList;
			}
			if (this.mSprites.Count == 0)
			{
				this.Upgrade();
			}
			return this.mSprites;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.spriteList = value;
			}
			else
			{
				this.mSprites = value;
			}
		}
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x060004EA RID: 1258 RVA: 0x0002782C File Offset: 0x00025A2C
	public Texture texture
	{
		get
		{
			return (!(this.mReplacement != null)) ? ((!(this.material != null)) ? null : this.material.mainTexture) : this.mReplacement.texture;
		}
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x060004EB RID: 1259 RVA: 0x00007015 File Offset: 0x00005215
	// (set) Token: 0x060004EC RID: 1260 RVA: 0x0002787C File Offset: 0x00025A7C
	public float pixelSize
	{
		get
		{
			return (!(this.mReplacement != null)) ? this.mPixelSize : this.mReplacement.pixelSize;
		}
		set
		{
			if (this.mReplacement != null)
			{
				this.mReplacement.pixelSize = value;
			}
			else
			{
				float num = Mathf.Clamp(value, 0.25f, 4f);
				if (this.mPixelSize != num)
				{
					this.mPixelSize = num;
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x060004ED RID: 1261 RVA: 0x0000703E File Offset: 0x0000523E
	// (set) Token: 0x060004EE RID: 1262 RVA: 0x000278D8 File Offset: 0x00025AD8
	public UIAtlas replacement
	{
		get
		{
			return this.mReplacement;
		}
		set
		{
			UIAtlas uiatlas = value;
			if (uiatlas == this)
			{
				uiatlas = null;
			}
			if (this.mReplacement != uiatlas)
			{
				if (uiatlas != null && uiatlas.replacement == this)
				{
					uiatlas.replacement = null;
				}
				if (this.mReplacement != null)
				{
					this.MarkAsChanged();
				}
				this.mReplacement = uiatlas;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x00027950 File Offset: 0x00025B50
	public UISpriteData GetSprite(string name)
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetSprite(name);
		}
		if (!string.IsNullOrEmpty(name))
		{
			if (this.mSprites.Count == 0)
			{
				this.Upgrade();
			}
			int i = 0;
			int count = this.mSprites.Count;
			while (i < count)
			{
				UISpriteData uispriteData = this.mSprites[i];
				if (!string.IsNullOrEmpty(uispriteData.name) && name == uispriteData.name)
				{
					return uispriteData;
				}
				i++;
			}
		}
		return null;
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x00007046 File Offset: 0x00005246
	public void SortAlphabetically()
	{
		this.mSprites.Sort((UISpriteData s1, UISpriteData s2) => s1.name.CompareTo(s2.name));
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x000279EC File Offset: 0x00025BEC
	public BetterList<string> GetListOfSprites()
	{
		if (this.mReplacement != null)
		{
			return this.mReplacement.GetListOfSprites();
		}
		if (this.mSprites.Count == 0)
		{
			this.Upgrade();
		}
		BetterList<string> betterList = new BetterList<string>();
		int i = 0;
		int count = this.mSprites.Count;
		while (i < count)
		{
			UISpriteData uispriteData = this.mSprites[i];
			if (uispriteData != null && !string.IsNullOrEmpty(uispriteData.name))
			{
				betterList.Add(uispriteData.name);
			}
			i++;
		}
		return betterList;
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x00027A84 File Offset: 0x00025C84
	public BetterList<string> GetListOfSprites(string match)
	{
		if (this.mReplacement)
		{
			return this.mReplacement.GetListOfSprites(match);
		}
		if (string.IsNullOrEmpty(match))
		{
			return this.GetListOfSprites();
		}
		if (this.mSprites.Count == 0)
		{
			this.Upgrade();
		}
		BetterList<string> betterList = new BetterList<string>();
		int i = 0;
		int count = this.mSprites.Count;
		while (i < count)
		{
			UISpriteData uispriteData = this.mSprites[i];
			if (uispriteData != null && !string.IsNullOrEmpty(uispriteData.name) && string.Equals(match, uispriteData.name, StringComparison.OrdinalIgnoreCase))
			{
				betterList.Add(uispriteData.name);
				return betterList;
			}
			i++;
		}
		string[] array = match.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = array[j].ToLower();
		}
		int k = 0;
		int count2 = this.mSprites.Count;
		while (k < count2)
		{
			UISpriteData uispriteData2 = this.mSprites[k];
			if (uispriteData2 != null && !string.IsNullOrEmpty(uispriteData2.name))
			{
				string text = uispriteData2.name.ToLower();
				int num = 0;
				for (int l = 0; l < array.Length; l++)
				{
					if (text.Contains(array[l]))
					{
						num++;
					}
				}
				if (num == array.Length)
				{
					betterList.Add(uispriteData2.name);
				}
			}
			k++;
		}
		return betterList;
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00027C20 File Offset: 0x00025E20
	private bool References(UIAtlas atlas)
	{
		return !(atlas == null) && (atlas == this || (this.mReplacement != null && this.mReplacement.References(atlas)));
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00007070 File Offset: 0x00005270
	public static bool CheckIfRelated(UIAtlas a, UIAtlas b)
	{
		return !(a == null) && !(b == null) && (a == b || a.References(b) || b.References(a));
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x00027C6C File Offset: 0x00025E6C
	public void MarkAsChanged()
	{
		if (this.mReplacement != null)
		{
			this.mReplacement.MarkAsChanged();
		}
		UISprite[] array = NGUITools.FindActive<UISprite>();
		int i = 0;
		int num = array.Length;
		while (i < num)
		{
			UISprite uisprite = array[i];
			if (UIAtlas.CheckIfRelated(this, uisprite.atlas))
			{
				UIAtlas atlas = uisprite.atlas;
				uisprite.atlas = null;
				uisprite.atlas = atlas;
			}
			i++;
		}
		UIFont[] array2 = Resources.FindObjectsOfTypeAll(typeof(UIFont)) as UIFont[];
		int j = 0;
		int num2 = array2.Length;
		while (j < num2)
		{
			UIFont uifont = array2[j];
			if (UIAtlas.CheckIfRelated(this, uifont.atlas))
			{
				UIAtlas atlas2 = uifont.atlas;
				uifont.atlas = null;
				uifont.atlas = atlas2;
			}
			j++;
		}
		UILabel[] array3 = NGUITools.FindActive<UILabel>();
		int k = 0;
		int num3 = array3.Length;
		while (k < num3)
		{
			UILabel uilabel = array3[k];
			if (uilabel.bitmapFont != null && UIAtlas.CheckIfRelated(this, uilabel.bitmapFont.atlas))
			{
				UIFont bitmapFont = uilabel.bitmapFont;
				uilabel.bitmapFont = null;
				uilabel.bitmapFont = bitmapFont;
			}
			k++;
		}
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x00027DB4 File Offset: 0x00025FB4
	private bool Upgrade()
	{
		if (this.mReplacement)
		{
			return this.mReplacement.Upgrade();
		}
		if (this.mSprites.Count == 0 && this.sprites.Count > 0 && this.material)
		{
			Texture mainTexture = this.material.mainTexture;
			int num = ((!(mainTexture != null)) ? 512 : mainTexture.width);
			int num2 = ((!(mainTexture != null)) ? 512 : mainTexture.height);
			for (int i = 0; i < this.sprites.Count; i++)
			{
				UIAtlas.Sprite sprite = this.sprites[i];
				Rect outer = sprite.outer;
				Rect inner = sprite.inner;
				if (this.mCoordinates == UIAtlas.Coordinates.TexCoords)
				{
					NGUIMath.ConvertToPixels(outer, num, num2, true);
					NGUIMath.ConvertToPixels(inner, num, num2, true);
				}
				UISpriteData uispriteData = new UISpriteData();
				uispriteData.name = sprite.name;
				uispriteData.x = Mathf.RoundToInt(outer.xMin);
				uispriteData.y = Mathf.RoundToInt(outer.yMin);
				uispriteData.width = Mathf.RoundToInt(outer.width);
				uispriteData.height = Mathf.RoundToInt(outer.height);
				uispriteData.paddingLeft = Mathf.RoundToInt(sprite.paddingLeft * outer.width);
				uispriteData.paddingRight = Mathf.RoundToInt(sprite.paddingRight * outer.width);
				uispriteData.paddingBottom = Mathf.RoundToInt(sprite.paddingBottom * outer.height);
				uispriteData.paddingTop = Mathf.RoundToInt(sprite.paddingTop * outer.height);
				uispriteData.borderLeft = Mathf.RoundToInt(inner.xMin - outer.xMin);
				uispriteData.borderRight = Mathf.RoundToInt(outer.xMax - inner.xMax);
				uispriteData.borderBottom = Mathf.RoundToInt(outer.yMax - inner.yMax);
				uispriteData.borderTop = Mathf.RoundToInt(inner.yMin - outer.yMin);
				this.mSprites.Add(uispriteData);
			}
			this.sprites.Clear();
			return true;
		}
		return false;
	}

	// Token: 0x040003AC RID: 940
	[HideInInspector]
	[SerializeField]
	private Material material;

	// Token: 0x040003AD RID: 941
	[SerializeField]
	[HideInInspector]
	private List<UISpriteData> mSprites = new List<UISpriteData>();

	// Token: 0x040003AE RID: 942
	[HideInInspector]
	[SerializeField]
	private float mPixelSize = 1f;

	// Token: 0x040003AF RID: 943
	[HideInInspector]
	[SerializeField]
	private UIAtlas mReplacement;

	// Token: 0x040003B0 RID: 944
	[HideInInspector]
	[SerializeField]
	private UIAtlas.Coordinates mCoordinates;

	// Token: 0x040003B1 RID: 945
	[SerializeField]
	[HideInInspector]
	private List<UIAtlas.Sprite> sprites = new List<UIAtlas.Sprite>();

	// Token: 0x040003B2 RID: 946
	private int mPMA = -1;

	// Token: 0x0200009F RID: 159
	[Serializable]
	private class Sprite
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x00028064 File Offset: 0x00026264
		public bool hasPadding
		{
			get
			{
				return this.paddingLeft != 0f || this.paddingRight != 0f || this.paddingTop != 0f || this.paddingBottom != 0f;
			}
		}

		// Token: 0x040003B4 RID: 948
		public string name = "Unity Bug";

		// Token: 0x040003B5 RID: 949
		public Rect outer = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x040003B6 RID: 950
		public Rect inner = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x040003B7 RID: 951
		public bool rotated;

		// Token: 0x040003B8 RID: 952
		public float paddingLeft;

		// Token: 0x040003B9 RID: 953
		public float paddingRight;

		// Token: 0x040003BA RID: 954
		public float paddingTop;

		// Token: 0x040003BB RID: 955
		public float paddingBottom;
	}

	// Token: 0x020000A0 RID: 160
	private enum Coordinates
	{
		// Token: 0x040003BD RID: 957
		Pixels,
		// Token: 0x040003BE RID: 958
		TexCoords
	}
}
