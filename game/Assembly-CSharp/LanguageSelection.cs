using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
[AddComponentMenu("NGUI/Interaction/Language Selection")]
[RequireComponent(typeof(UIPopupList))]
public class LanguageSelection : MonoBehaviour
{
	// Token: 0x06000100 RID: 256 RVA: 0x00015E64 File Offset: 0x00014064
	private void Start()
	{
		this.mList = base.GetComponent<UIPopupList>();
		if (Localization.instance != null && Localization.instance.languages != null && Localization.instance.languages.Length > 0)
		{
			this.mList.items.Clear();
			int i = 0;
			int num = Localization.instance.languages.Length;
			while (i < num)
			{
				TextAsset textAsset = Localization.instance.languages[i];
				if (textAsset != null)
				{
					this.mList.items.Add(textAsset.name);
				}
				i++;
			}
			this.mList.value = Localization.instance.currentLanguage;
		}
		EventDelegate.Add(this.mList.onChange, new EventDelegate.Callback(this.OnChange));
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00003FFE File Offset: 0x000021FE
	private void OnChange()
	{
		if (Localization.instance != null)
		{
			Localization.instance.currentLanguage = UIPopupList.current.value;
		}
	}

	// Token: 0x040000E7 RID: 231
	private UIPopupList mList;
}
