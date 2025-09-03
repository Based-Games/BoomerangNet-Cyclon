using System;
using UnityEngine;

// Token: 0x0200024B RID: 587
public class tk2dSystem : ScriptableObject
{
	// Token: 0x060010C1 RID: 4289 RVA: 0x00077A38 File Offset: 0x00075C38
	private tk2dSystem()
	{
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x060010C3 RID: 4291 RVA: 0x00077A9C File Offset: 0x00075C9C
	public static tk2dSystem inst
	{
		get
		{
			if (tk2dSystem._inst == null)
			{
				tk2dSystem._inst = Resources.Load("tk2d/tk2dSystem", typeof(tk2dSystem)) as tk2dSystem;
				if (tk2dSystem._inst == null)
				{
					tk2dSystem._inst = ScriptableObject.CreateInstance<tk2dSystem>();
				}
				UnityEngine.Object.DontDestroyOnLoad(tk2dSystem._inst);
			}
			return tk2dSystem._inst;
		}
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x060010C4 RID: 4292 RVA: 0x0000E4BA File Offset: 0x0000C6BA
	public static tk2dSystem inst_NoCreate
	{
		get
		{
			if (tk2dSystem._inst == null)
			{
				tk2dSystem._inst = Resources.Load("tk2d/tk2dSystem", typeof(tk2dSystem)) as tk2dSystem;
			}
			return tk2dSystem._inst;
		}
	}

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x060010C5 RID: 4293 RVA: 0x0000E4EF File Offset: 0x0000C6EF
	// (set) Token: 0x060010C6 RID: 4294 RVA: 0x0000E4F6 File Offset: 0x0000C6F6
	public static string CurrentPlatform
	{
		get
		{
			return tk2dSystem.currentPlatform;
		}
		set
		{
			if (value != tk2dSystem.currentPlatform)
			{
				tk2dSystem.currentPlatform = value;
			}
		}
	}

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x060010C7 RID: 4295 RVA: 0x000090F9 File Offset: 0x000072F9
	public static bool OverrideBuildMaterial
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060010C8 RID: 4296 RVA: 0x00077B00 File Offset: 0x00075D00
	public static tk2dAssetPlatform GetAssetPlatform(string platform)
	{
		tk2dSystem inst_NoCreate = tk2dSystem.inst_NoCreate;
		if (inst_NoCreate == null)
		{
			return null;
		}
		for (int i = 0; i < inst_NoCreate.assetPlatforms.Length; i++)
		{
			if (inst_NoCreate.assetPlatforms[i].name == platform)
			{
				return inst_NoCreate.assetPlatforms[i];
			}
		}
		return null;
	}

	// Token: 0x060010C9 RID: 4297 RVA: 0x00077B5C File Offset: 0x00075D5C
	private T LoadResourceByGUIDImpl<T>(string guid) where T : UnityEngine.Object
	{
		tk2dResource tk2dResource = Resources.Load("tk2d/tk2d_" + guid, typeof(tk2dResource)) as tk2dResource;
		if (tk2dResource != null)
		{
			return tk2dResource.objectReference as T;
		}
		return (T)((object)null);
	}

	// Token: 0x060010CA RID: 4298 RVA: 0x00077BAC File Offset: 0x00075DAC
	private T LoadResourceByNameImpl<T>(string name) where T : UnityEngine.Object
	{
		for (int i = 0; i < this.allResourceEntries.Length; i++)
		{
			if (this.allResourceEntries[i] != null && this.allResourceEntries[i].assetName == name)
			{
				return this.LoadResourceByGUIDImpl<T>(this.allResourceEntries[i].assetGUID);
			}
		}
		return (T)((object)null);
	}

	// Token: 0x060010CB RID: 4299 RVA: 0x0000E50E File Offset: 0x0000C70E
	public static T LoadResourceByGUID<T>(string guid) where T : UnityEngine.Object
	{
		return tk2dSystem.inst.LoadResourceByGUIDImpl<T>(guid);
	}

	// Token: 0x060010CC RID: 4300 RVA: 0x0000E51B File Offset: 0x0000C71B
	public static T LoadResourceByName<T>(string guid) where T : UnityEngine.Object
	{
		return tk2dSystem.inst.LoadResourceByNameImpl<T>(guid);
	}

	// Token: 0x040012A2 RID: 4770
	public const string guidPrefix = "tk2d/tk2d_";

	// Token: 0x040012A3 RID: 4771
	public const string assetName = "tk2d/tk2dSystem";

	// Token: 0x040012A4 RID: 4772
	public const string assetFileName = "tk2dSystem.asset";

	// Token: 0x040012A5 RID: 4773
	[NonSerialized]
	public tk2dAssetPlatform[] assetPlatforms = new tk2dAssetPlatform[]
	{
		new tk2dAssetPlatform("1x", 1f),
		new tk2dAssetPlatform("2x", 2f),
		new tk2dAssetPlatform("4x", 4f)
	};

	// Token: 0x040012A6 RID: 4774
	private static tk2dSystem _inst;

	// Token: 0x040012A7 RID: 4775
	private static string currentPlatform = string.Empty;

	// Token: 0x040012A8 RID: 4776
	[SerializeField]
	private tk2dResourceTocEntry[] allResourceEntries = new tk2dResourceTocEntry[0];
}
