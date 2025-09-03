using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200023D RID: 573
[AddComponentMenu("2D Toolkit/Backend/tk2dFontData")]
public class tk2dFontData : MonoBehaviour
{
	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06001048 RID: 4168 RVA: 0x000747A0 File Offset: 0x000729A0
	public tk2dFontData inst
	{
		get
		{
			if (this.platformSpecificData == null || this.platformSpecificData.materialInst == null)
			{
				if (this.hasPlatformData)
				{
					string currentPlatform = tk2dSystem.CurrentPlatform;
					string text = string.Empty;
					for (int i = 0; i < this.fontPlatforms.Length; i++)
					{
						if (this.fontPlatforms[i] == currentPlatform)
						{
							text = this.fontPlatformGUIDs[i];
							break;
						}
					}
					if (text.Length == 0)
					{
						text = this.fontPlatformGUIDs[0];
					}
					this.platformSpecificData = tk2dSystem.LoadResourceByGUID<tk2dFontData>(text);
				}
				else
				{
					this.platformSpecificData = this;
				}
				this.platformSpecificData.Init();
			}
			return this.platformSpecificData;
		}
	}

	// Token: 0x06001049 RID: 4169 RVA: 0x00074864 File Offset: 0x00072A64
	private void Init()
	{
		if (this.needMaterialInstance)
		{
			if (this.spriteCollection)
			{
				tk2dSpriteCollectionData inst = this.spriteCollection.inst;
				for (int i = 0; i < inst.materials.Length; i++)
				{
					if (inst.materials[i] == this.material)
					{
						this.materialInst = inst.materialInsts[i];
						break;
					}
				}
				if (this.materialInst == null)
				{
					Debug.LogError("Fatal error - font from sprite collection is has an invalid material");
				}
			}
			else
			{
				this.materialInst = UnityEngine.Object.Instantiate(this.material) as Material;
				this.materialInst.hideFlags = HideFlags.DontSave;
			}
		}
		else
		{
			this.materialInst = this.material;
		}
	}

	// Token: 0x0600104A RID: 4170 RVA: 0x0000DDC3 File Offset: 0x0000BFC3
	public void ResetPlatformData()
	{
		if (this.hasPlatformData && this.platformSpecificData)
		{
			this.platformSpecificData = null;
		}
		this.materialInst = null;
	}

	// Token: 0x0600104B RID: 4171 RVA: 0x0000DDEE File Offset: 0x0000BFEE
	private void OnDestroy()
	{
		if (this.needMaterialInstance && this.spriteCollection == null)
		{
			UnityEngine.Object.DestroyImmediate(this.materialInst);
		}
	}

	// Token: 0x0600104C RID: 4172 RVA: 0x00074930 File Offset: 0x00072B30
	public void InitDictionary()
	{
		if (this.useDictionary && this.charDict == null)
		{
			this.charDict = new Dictionary<int, tk2dFontChar>(this.charDictKeys.Count);
			for (int i = 0; i < this.charDictKeys.Count; i++)
			{
				this.charDict[this.charDictKeys[i]] = this.charDictValues[i];
			}
		}
	}

	// Token: 0x0600104D RID: 4173 RVA: 0x000749A8 File Offset: 0x00072BA8
	public void SetDictionary(Dictionary<int, tk2dFontChar> dict)
	{
		this.charDictKeys = new List<int>(dict.Keys);
		this.charDictValues = new List<tk2dFontChar>();
		for (int i = 0; i < this.charDictKeys.Count; i++)
		{
			this.charDictValues.Add(dict[this.charDictKeys[i]]);
		}
	}

	// Token: 0x0400120F RID: 4623
	public const int CURRENT_VERSION = 2;

	// Token: 0x04001210 RID: 4624
	[HideInInspector]
	public int version;

	// Token: 0x04001211 RID: 4625
	public float lineHeight;

	// Token: 0x04001212 RID: 4626
	public tk2dFontChar[] chars;

	// Token: 0x04001213 RID: 4627
	[SerializeField]
	private List<int> charDictKeys;

	// Token: 0x04001214 RID: 4628
	[SerializeField]
	private List<tk2dFontChar> charDictValues;

	// Token: 0x04001215 RID: 4629
	public string[] fontPlatforms;

	// Token: 0x04001216 RID: 4630
	public string[] fontPlatformGUIDs;

	// Token: 0x04001217 RID: 4631
	private tk2dFontData platformSpecificData;

	// Token: 0x04001218 RID: 4632
	public bool hasPlatformData;

	// Token: 0x04001219 RID: 4633
	public bool managedFont;

	// Token: 0x0400121A RID: 4634
	public bool needMaterialInstance;

	// Token: 0x0400121B RID: 4635
	public bool isPacked;

	// Token: 0x0400121C RID: 4636
	public bool premultipliedAlpha;

	// Token: 0x0400121D RID: 4637
	public tk2dSpriteCollectionData spriteCollection;

	// Token: 0x0400121E RID: 4638
	public Dictionary<int, tk2dFontChar> charDict;

	// Token: 0x0400121F RID: 4639
	public bool useDictionary;

	// Token: 0x04001220 RID: 4640
	public tk2dFontKerning[] kerning;

	// Token: 0x04001221 RID: 4641
	public float largestWidth;

	// Token: 0x04001222 RID: 4642
	public Material material;

	// Token: 0x04001223 RID: 4643
	[NonSerialized]
	public Material materialInst;

	// Token: 0x04001224 RID: 4644
	public Texture2D gradientTexture;

	// Token: 0x04001225 RID: 4645
	public bool textureGradients;

	// Token: 0x04001226 RID: 4646
	public int gradientCount = 1;

	// Token: 0x04001227 RID: 4647
	public Vector2 texelSize;

	// Token: 0x04001228 RID: 4648
	[HideInInspector]
	public float invOrthoSize = 1f;

	// Token: 0x04001229 RID: 4649
	[HideInInspector]
	public float halfTargetHeight = 1f;
}
