using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200026E RID: 622
[AddComponentMenu("2D Toolkit/Backend/tk2dSpriteCollection")]
public class tk2dSpriteCollection : MonoBehaviour
{
	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x060011F6 RID: 4598 RVA: 0x0000F53C File Offset: 0x0000D73C
	// (set) Token: 0x060011F7 RID: 4599 RVA: 0x0000F544 File Offset: 0x0000D744
	public Texture2D[] DoNotUse__TextureRefs
	{
		get
		{
			return this.textureRefs;
		}
		set
		{
			this.textureRefs = value;
		}
	}

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x060011F8 RID: 4600 RVA: 0x0000F54D File Offset: 0x0000D74D
	public bool HasPlatformData
	{
		get
		{
			return this.platforms.Count > 1;
		}
	}

	// Token: 0x060011F9 RID: 4601 RVA: 0x0007CAD0 File Offset: 0x0007ACD0
	public void Upgrade()
	{
		if (this.version == 4)
		{
			return;
		}
		Debug.Log("SpriteCollection '" + base.name + "' - Upgraded from version " + this.version.ToString());
		if (this.version == 0)
		{
			if (this.pixelPerfectPointSampled)
			{
				this.filterMode = FilterMode.Point;
			}
			else
			{
				this.filterMode = FilterMode.Bilinear;
			}
			this.userDefinedTextureSettings = true;
		}
		if (this.version < 3 && this.textureRefs != null && this.textureParams != null && this.textureRefs.Length == this.textureParams.Length)
		{
			for (int i = 0; i < this.textureRefs.Length; i++)
			{
				this.textureParams[i].texture = this.textureRefs[i];
			}
			this.textureRefs = null;
		}
		if (this.version < 4)
		{
			this.sizeDef.CopyFromLegacy(this.useTk2dCamera, this.targetOrthoSize, (float)this.targetHeight);
		}
		this.version = 4;
	}

	// Token: 0x040013A2 RID: 5026
	public const int CURRENT_VERSION = 4;

	// Token: 0x040013A3 RID: 5027
	[SerializeField]
	private tk2dSpriteCollectionDefinition[] textures;

	// Token: 0x040013A4 RID: 5028
	[SerializeField]
	private Texture2D[] textureRefs;

	// Token: 0x040013A5 RID: 5029
	public tk2dSpriteSheetSource[] spriteSheets;

	// Token: 0x040013A6 RID: 5030
	public tk2dSpriteCollectionFont[] fonts;

	// Token: 0x040013A7 RID: 5031
	public tk2dSpriteCollectionDefault defaults;

	// Token: 0x040013A8 RID: 5032
	public List<tk2dSpriteCollectionPlatform> platforms = new List<tk2dSpriteCollectionPlatform>();

	// Token: 0x040013A9 RID: 5033
	public bool managedSpriteCollection;

	// Token: 0x040013AA RID: 5034
	public bool loadable;

	// Token: 0x040013AB RID: 5035
	public int maxTextureSize = 2048;

	// Token: 0x040013AC RID: 5036
	public bool forceTextureSize;

	// Token: 0x040013AD RID: 5037
	public int forcedTextureWidth = 2048;

	// Token: 0x040013AE RID: 5038
	public int forcedTextureHeight = 2048;

	// Token: 0x040013AF RID: 5039
	public tk2dSpriteCollection.TextureCompression textureCompression;

	// Token: 0x040013B0 RID: 5040
	public int atlasWidth;

	// Token: 0x040013B1 RID: 5041
	public int atlasHeight;

	// Token: 0x040013B2 RID: 5042
	public bool forceSquareAtlas;

	// Token: 0x040013B3 RID: 5043
	public float atlasWastage;

	// Token: 0x040013B4 RID: 5044
	public bool allowMultipleAtlases;

	// Token: 0x040013B5 RID: 5045
	public bool removeDuplicates = true;

	// Token: 0x040013B6 RID: 5046
	public tk2dSpriteCollectionDefinition[] textureParams;

	// Token: 0x040013B7 RID: 5047
	public tk2dSpriteCollectionData spriteCollection;

	// Token: 0x040013B8 RID: 5048
	public bool premultipliedAlpha;

	// Token: 0x040013B9 RID: 5049
	public Material[] altMaterials;

	// Token: 0x040013BA RID: 5050
	public Material[] atlasMaterials;

	// Token: 0x040013BB RID: 5051
	public Texture2D[] atlasTextures;

	// Token: 0x040013BC RID: 5052
	[SerializeField]
	private bool useTk2dCamera;

	// Token: 0x040013BD RID: 5053
	[SerializeField]
	private int targetHeight = 640;

	// Token: 0x040013BE RID: 5054
	[SerializeField]
	private float targetOrthoSize = 10f;

	// Token: 0x040013BF RID: 5055
	public tk2dSpriteCollectionSize sizeDef = tk2dSpriteCollectionSize.Default();

	// Token: 0x040013C0 RID: 5056
	public float globalScale = 1f;

	// Token: 0x040013C1 RID: 5057
	public float globalTextureRescale = 1f;

	// Token: 0x040013C2 RID: 5058
	public List<tk2dSpriteCollection.AttachPointTestSprite> attachPointTestSprites = new List<tk2dSpriteCollection.AttachPointTestSprite>();

	// Token: 0x040013C3 RID: 5059
	[SerializeField]
	private bool pixelPerfectPointSampled;

	// Token: 0x040013C4 RID: 5060
	public FilterMode filterMode = FilterMode.Bilinear;

	// Token: 0x040013C5 RID: 5061
	public TextureWrapMode wrapMode = TextureWrapMode.Clamp;

	// Token: 0x040013C6 RID: 5062
	public bool userDefinedTextureSettings;

	// Token: 0x040013C7 RID: 5063
	public bool mipmapEnabled;

	// Token: 0x040013C8 RID: 5064
	public int anisoLevel = 1;

	// Token: 0x040013C9 RID: 5065
	public float physicsDepth = 0.1f;

	// Token: 0x040013CA RID: 5066
	public bool disableTrimming;

	// Token: 0x040013CB RID: 5067
	public tk2dSpriteCollection.NormalGenerationMode normalGenerationMode;

	// Token: 0x040013CC RID: 5068
	public int padAmount = -1;

	// Token: 0x040013CD RID: 5069
	public bool autoUpdate = true;

	// Token: 0x040013CE RID: 5070
	public float editorDisplayScale = 1f;

	// Token: 0x040013CF RID: 5071
	public int version;

	// Token: 0x040013D0 RID: 5072
	public string assetName = string.Empty;

	// Token: 0x0200026F RID: 623
	public enum NormalGenerationMode
	{
		// Token: 0x040013D2 RID: 5074
		None,
		// Token: 0x040013D3 RID: 5075
		NormalsOnly,
		// Token: 0x040013D4 RID: 5076
		NormalsAndTangents
	}

	// Token: 0x02000270 RID: 624
	public enum TextureCompression
	{
		// Token: 0x040013D6 RID: 5078
		Uncompressed,
		// Token: 0x040013D7 RID: 5079
		Reduced16Bit,
		// Token: 0x040013D8 RID: 5080
		Compressed,
		// Token: 0x040013D9 RID: 5081
		Dithered16Bit_Alpha,
		// Token: 0x040013DA RID: 5082
		Dithered16Bit_NoAlpha
	}

	// Token: 0x02000271 RID: 625
	[Serializable]
	public class AttachPointTestSprite
	{
		// Token: 0x060011FB RID: 4603 RVA: 0x0000F577 File Offset: 0x0000D777
		public bool CompareTo(tk2dSpriteCollection.AttachPointTestSprite src)
		{
			return src.attachPointName == this.attachPointName && src.spriteCollection == this.spriteCollection && src.spriteId == this.spriteId;
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x0000F5B6 File Offset: 0x0000D7B6
		public void CopyFrom(tk2dSpriteCollection.AttachPointTestSprite src)
		{
			this.attachPointName = src.attachPointName;
			this.spriteCollection = src.spriteCollection;
			this.spriteId = src.spriteId;
		}

		// Token: 0x040013DB RID: 5083
		public string attachPointName = string.Empty;

		// Token: 0x040013DC RID: 5084
		public tk2dSpriteCollectionData spriteCollection;

		// Token: 0x040013DD RID: 5085
		public int spriteId = -1;
	}
}
