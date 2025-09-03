using System;
using System.Collections.Generic;
using tk2dRuntime;
using UnityEngine;

// Token: 0x02000276 RID: 630
[AddComponentMenu("2D Toolkit/Backend/tk2dSpriteCollectionData")]
public class tk2dSpriteCollectionData : MonoBehaviour
{
	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06001205 RID: 4613 RVA: 0x0000F6D1 File Offset: 0x0000D8D1
	// (set) Token: 0x06001206 RID: 4614 RVA: 0x0000F6D9 File Offset: 0x0000D8D9
	public bool Transient { get; set; }

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x06001207 RID: 4615 RVA: 0x0000F6E2 File Offset: 0x0000D8E2
	public int Count
	{
		get
		{
			return this.inst.spriteDefinitions.Length;
		}
	}

	// Token: 0x06001208 RID: 4616 RVA: 0x0000F6F1 File Offset: 0x0000D8F1
	public int GetSpriteIdByName(string name)
	{
		return this.GetSpriteIdByName(name, 0);
	}

	// Token: 0x06001209 RID: 4617 RVA: 0x0007CCE4 File Offset: 0x0007AEE4
	public int GetSpriteIdByName(string name, int defaultValue)
	{
		this.inst.InitDictionary();
		int num = defaultValue;
		if (!this.inst.spriteNameLookupDict.TryGetValue(name, out num))
		{
			return defaultValue;
		}
		return num;
	}

	// Token: 0x0600120A RID: 4618 RVA: 0x0007CD1C File Offset: 0x0007AF1C
	public tk2dSpriteDefinition GetSpriteDefinition(string name)
	{
		int spriteIdByName = this.GetSpriteIdByName(name, -1);
		if (spriteIdByName == -1)
		{
			return null;
		}
		return this.spriteDefinitions[spriteIdByName];
	}

	// Token: 0x0600120B RID: 4619 RVA: 0x0007CD44 File Offset: 0x0007AF44
	public void InitDictionary()
	{
		if (this.spriteNameLookupDict == null)
		{
			this.spriteNameLookupDict = new Dictionary<string, int>(this.spriteDefinitions.Length);
			for (int i = 0; i < this.spriteDefinitions.Length; i++)
			{
				this.spriteNameLookupDict[this.spriteDefinitions[i].name] = i;
			}
		}
	}

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x0600120C RID: 4620 RVA: 0x0007CDA4 File Offset: 0x0007AFA4
	public tk2dSpriteDefinition FirstValidDefinition
	{
		get
		{
			foreach (tk2dSpriteDefinition tk2dSpriteDefinition in this.inst.spriteDefinitions)
			{
				if (tk2dSpriteDefinition.Valid)
				{
					return tk2dSpriteDefinition;
				}
			}
			return null;
		}
	}

	// Token: 0x0600120D RID: 4621 RVA: 0x0000F6FB File Offset: 0x0000D8FB
	public bool IsValidSpriteId(int id)
	{
		return id >= 0 && id < this.inst.spriteDefinitions.Length && this.inst.spriteDefinitions[id].Valid;
	}

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x0600120E RID: 4622 RVA: 0x0007CDE4 File Offset: 0x0007AFE4
	public int FirstValidDefinitionIndex
	{
		get
		{
			tk2dSpriteCollectionData inst = this.inst;
			for (int i = 0; i < inst.spriteDefinitions.Length; i++)
			{
				if (inst.spriteDefinitions[i].Valid)
				{
					return i;
				}
			}
			return -1;
		}
	}

	// Token: 0x0600120F RID: 4623 RVA: 0x0007CE28 File Offset: 0x0007B028
	public void InitMaterialIds()
	{
		if (this.inst.materialIdsValid)
		{
			return;
		}
		int num = -1;
		Dictionary<Material, int> dictionary = new Dictionary<Material, int>();
		for (int i = 0; i < this.inst.materials.Length; i++)
		{
			if (num == -1 && this.inst.materials[i] != null)
			{
				num = i;
			}
			dictionary[this.materials[i]] = i;
		}
		if (num == -1)
		{
			Debug.LogError("Init material ids failed.");
		}
		else
		{
			foreach (tk2dSpriteDefinition tk2dSpriteDefinition in this.inst.spriteDefinitions)
			{
				if (!dictionary.TryGetValue(tk2dSpriteDefinition.material, out tk2dSpriteDefinition.materialId))
				{
					tk2dSpriteDefinition.materialId = num;
				}
			}
			this.inst.materialIdsValid = true;
		}
	}

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06001210 RID: 4624 RVA: 0x0007CF08 File Offset: 0x0007B108
	public tk2dSpriteCollectionData inst
	{
		get
		{
			if (this.platformSpecificData == null)
			{
				if (this.hasPlatformData)
				{
					string currentPlatform = tk2dSystem.CurrentPlatform;
					string text = string.Empty;
					for (int i = 0; i < this.spriteCollectionPlatforms.Length; i++)
					{
						if (this.spriteCollectionPlatforms[i] == currentPlatform)
						{
							text = this.spriteCollectionPlatformGUIDs[i];
							break;
						}
					}
					if (text.Length == 0)
					{
						text = this.spriteCollectionPlatformGUIDs[0];
					}
					this.platformSpecificData = tk2dSystem.LoadResourceByGUID<tk2dSpriteCollectionData>(text);
				}
				else
				{
					this.platformSpecificData = this;
				}
			}
			this.platformSpecificData.Init();
			return this.platformSpecificData;
		}
	}

	// Token: 0x06001211 RID: 4625 RVA: 0x0007CFB4 File Offset: 0x0007B1B4
	private void Init()
	{
		if (this.materialInsts != null)
		{
			return;
		}
		if (this.spriteDefinitions == null)
		{
			this.spriteDefinitions = new tk2dSpriteDefinition[0];
		}
		if (this.materials == null)
		{
			this.materials = new Material[0];
		}
		this.materialInsts = new Material[this.materials.Length];
		if (this.needMaterialInstance)
		{
			if (tk2dSystem.OverrideBuildMaterial)
			{
				for (int i = 0; i < this.materials.Length; i++)
				{
					this.materialInsts[i] = new Material(Shader.Find("tk2d/BlendVertexColor"));
				}
			}
			else
			{
				for (int j = 0; j < this.materials.Length; j++)
				{
					this.materialInsts[j] = UnityEngine.Object.Instantiate(this.materials[j]) as Material;
				}
			}
			for (int k = 0; k < this.spriteDefinitions.Length; k++)
			{
				tk2dSpriteDefinition tk2dSpriteDefinition = this.spriteDefinitions[k];
				tk2dSpriteDefinition.materialInst = this.materialInsts[tk2dSpriteDefinition.materialId];
			}
		}
		else
		{
			for (int l = 0; l < this.spriteDefinitions.Length; l++)
			{
				tk2dSpriteDefinition tk2dSpriteDefinition2 = this.spriteDefinitions[l];
				tk2dSpriteDefinition2.materialInst = tk2dSpriteDefinition2.material;
			}
		}
	}

	// Token: 0x06001212 RID: 4626 RVA: 0x0000F72B File Offset: 0x0000D92B
	public static tk2dSpriteCollectionData CreateFromTexture(Texture texture, tk2dSpriteCollectionSize size, string[] names, Rect[] regions, Vector2[] anchors)
	{
		return SpriteCollectionGenerator.CreateFromTexture(texture, size, names, regions, anchors);
	}

	// Token: 0x06001213 RID: 4627 RVA: 0x0000F738 File Offset: 0x0000D938
	public static tk2dSpriteCollectionData CreateFromTexturePacker(tk2dSpriteCollectionSize size, string texturePackerData, Texture texture)
	{
		return SpriteCollectionGenerator.CreateFromTexturePacker(size, texturePackerData, texture);
	}

	// Token: 0x06001214 RID: 4628 RVA: 0x0000F742 File Offset: 0x0000D942
	public void ResetPlatformData()
	{
		if (this.hasPlatformData && this.platformSpecificData)
		{
			this.platformSpecificData = null;
		}
		this.materialInsts = null;
	}

	// Token: 0x06001215 RID: 4629 RVA: 0x0007D0FC File Offset: 0x0007B2FC
	public void UnloadTextures()
	{
		tk2dSpriteCollectionData inst = this.inst;
		foreach (Texture2D texture2D in inst.textures)
		{
			Resources.UnloadAsset(texture2D);
		}
	}

	// Token: 0x06001216 RID: 4630 RVA: 0x0007D13C File Offset: 0x0007B33C
	private void OnDestroy()
	{
		if (this.Transient)
		{
			foreach (Material material in this.materials)
			{
				UnityEngine.Object.DestroyImmediate(material);
			}
		}
		else if (this.needMaterialInstance)
		{
			foreach (Material material2 in this.materialInsts)
			{
				UnityEngine.Object.DestroyImmediate(material2);
			}
		}
		this.ResetPlatformData();
	}

	// Token: 0x04001405 RID: 5125
	public const int CURRENT_VERSION = 3;

	// Token: 0x04001406 RID: 5126
	public int version;

	// Token: 0x04001407 RID: 5127
	public bool materialIdsValid;

	// Token: 0x04001408 RID: 5128
	public bool needMaterialInstance;

	// Token: 0x04001409 RID: 5129
	public tk2dSpriteDefinition[] spriteDefinitions;

	// Token: 0x0400140A RID: 5130
	private Dictionary<string, int> spriteNameLookupDict;

	// Token: 0x0400140B RID: 5131
	public bool premultipliedAlpha;

	// Token: 0x0400140C RID: 5132
	public Material material;

	// Token: 0x0400140D RID: 5133
	public Material[] materials;

	// Token: 0x0400140E RID: 5134
	[NonSerialized]
	public Material[] materialInsts;

	// Token: 0x0400140F RID: 5135
	public Texture[] textures;

	// Token: 0x04001410 RID: 5136
	public bool allowMultipleAtlases;

	// Token: 0x04001411 RID: 5137
	public string spriteCollectionGUID;

	// Token: 0x04001412 RID: 5138
	public string spriteCollectionName;

	// Token: 0x04001413 RID: 5139
	public string assetName = string.Empty;

	// Token: 0x04001414 RID: 5140
	public bool loadable;

	// Token: 0x04001415 RID: 5141
	public float invOrthoSize = 1f;

	// Token: 0x04001416 RID: 5142
	public float halfTargetHeight = 1f;

	// Token: 0x04001417 RID: 5143
	public int buildKey;

	// Token: 0x04001418 RID: 5144
	public string dataGuid = string.Empty;

	// Token: 0x04001419 RID: 5145
	public bool managedSpriteCollection;

	// Token: 0x0400141A RID: 5146
	public bool hasPlatformData;

	// Token: 0x0400141B RID: 5147
	public string[] spriteCollectionPlatforms;

	// Token: 0x0400141C RID: 5148
	public string[] spriteCollectionPlatformGUIDs;

	// Token: 0x0400141D RID: 5149
	private tk2dSpriteCollectionData platformSpecificData;
}
