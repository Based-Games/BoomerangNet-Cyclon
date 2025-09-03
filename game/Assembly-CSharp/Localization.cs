using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000070 RID: 112
[AddComponentMenu("NGUI/Internal/Localization")]
public class Localization : MonoBehaviour
{
	// Token: 0x1700006C RID: 108
	// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000575C File Offset: 0x0000395C
	public static bool isActive
	{
		get
		{
			return Localization.mInstance != null;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x060002C9 RID: 713 RVA: 0x0001E61C File Offset: 0x0001C81C
	public static Localization instance
	{
		get
		{
			if (Localization.mInstance == null)
			{
				Localization.mInstance = UnityEngine.Object.FindObjectOfType(typeof(Localization)) as Localization;
				if (Localization.mInstance == null)
				{
					GameObject gameObject = new GameObject("_Localization");
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
					Localization.mInstance = gameObject.AddComponent<Localization>();
				}
			}
			return Localization.mInstance;
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x060002CA RID: 714 RVA: 0x00005769 File Offset: 0x00003969
	// (set) Token: 0x060002CB RID: 715 RVA: 0x0001E684 File Offset: 0x0001C884
	public string currentLanguage
	{
		get
		{
			return this.mLanguage;
		}
		set
		{
			if (this.mLanguage != value)
			{
				this.startingLanguage = value;
				if (!string.IsNullOrEmpty(value))
				{
					if (this.languages != null)
					{
						int i = 0;
						int num = this.languages.Length;
						while (i < num)
						{
							TextAsset textAsset = this.languages[i];
							if (textAsset != null && textAsset.name == value)
							{
								this.Load(textAsset);
								return;
							}
							i++;
						}
					}
					TextAsset textAsset2 = Resources.Load(value, typeof(TextAsset)) as TextAsset;
					if (textAsset2 != null)
					{
						this.Load(textAsset2);
						return;
					}
				}
				this.mDictionary.Clear();
				PlayerPrefs.DeleteKey("Language");
			}
		}
	}

	// Token: 0x060002CC RID: 716 RVA: 0x0001E748 File Offset: 0x0001C948
	private void Awake()
	{
		if (Localization.mInstance == null)
		{
			Localization.mInstance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			this.currentLanguage = PlayerPrefs.GetString("Language", this.startingLanguage);
			if (string.IsNullOrEmpty(this.mLanguage) && this.languages != null && this.languages.Length > 0)
			{
				this.currentLanguage = this.languages[0].name;
			}
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060002CD RID: 717 RVA: 0x00005771 File Offset: 0x00003971
	private void OnEnable()
	{
		if (Localization.mInstance == null)
		{
			Localization.mInstance = this;
		}
	}

	// Token: 0x060002CE RID: 718 RVA: 0x00005789 File Offset: 0x00003989
	private void OnDestroy()
	{
		if (Localization.mInstance == this)
		{
			Localization.mInstance = null;
		}
	}

	// Token: 0x060002CF RID: 719 RVA: 0x0001E7D8 File Offset: 0x0001C9D8
	public void Load(TextAsset asset)
	{
		ByteReader byteReader = new ByteReader(asset);
		this.Set(asset.name, byteReader.ReadDictionary());
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x000057A1 File Offset: 0x000039A1
	public void Set(string languageName, Dictionary<string, string> dictionary)
	{
		this.mLanguage = languageName;
		PlayerPrefs.SetString("Language", this.mLanguage);
		this.mDictionary = dictionary;
		UIRoot.Broadcast("OnLocalize", this);
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x0001E800 File Offset: 0x0001CA00
	public string Get(string key)
	{
		string text;
		return (!this.mDictionary.TryGetValue(key, out text)) ? key : text;
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x000057CC File Offset: 0x000039CC
	public static string Localize(string key)
	{
		return (!(Localization.instance != null)) ? key : Localization.instance.Get(key);
	}

	// Token: 0x040002A2 RID: 674
	private static Localization mInstance;

	// Token: 0x040002A3 RID: 675
	public string startingLanguage = "English";

	// Token: 0x040002A4 RID: 676
	public TextAsset[] languages;

	// Token: 0x040002A5 RID: 677
	private Dictionary<string, string> mDictionary = new Dictionary<string, string>();

	// Token: 0x040002A6 RID: 678
	private string mLanguage;
}
