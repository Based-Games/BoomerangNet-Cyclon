using System;
using UnityEngine;

// Token: 0x020000B4 RID: 180
[AddComponentMenu("NGUI/UI/Localize")]
[RequireComponent(typeof(UIWidget))]
public class UILocalize : MonoBehaviour
{
	// Token: 0x060005D3 RID: 1491 RVA: 0x00007CCA File Offset: 0x00005ECA
	private void OnLocalize(Localization loc)
	{
		if (this.mLanguage != loc.currentLanguage)
		{
			this.Localize();
		}
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x00007CE8 File Offset: 0x00005EE8
	private void OnEnable()
	{
		if (this.mStarted && Localization.instance != null)
		{
			this.Localize();
		}
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x00007D0B File Offset: 0x00005F0B
	private void Start()
	{
		this.mStarted = true;
		if (Localization.instance != null)
		{
			this.Localize();
		}
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x0002DA74 File Offset: 0x0002BC74
	public void Localize()
	{
		Localization instance = Localization.instance;
		UIWidget component = base.GetComponent<UIWidget>();
		UILabel uilabel = component as UILabel;
		UISprite uisprite = component as UISprite;
		if (string.IsNullOrEmpty(this.mLanguage) && string.IsNullOrEmpty(this.key) && uilabel != null)
		{
			this.key = uilabel.text;
		}
		string text = ((!string.IsNullOrEmpty(this.key)) ? instance.Get(this.key) : string.Empty);
		if (uilabel != null)
		{
			UIInput uiinput = NGUITools.FindInParents<UIInput>(uilabel.gameObject);
			if (uiinput != null && uiinput.label == uilabel)
			{
				uiinput.defaultText = text;
			}
			else
			{
				uilabel.text = text;
			}
		}
		else if (uisprite != null)
		{
			uisprite.spriteName = text;
			uisprite.MakePixelPerfect();
		}
		this.mLanguage = instance.currentLanguage;
	}

	// Token: 0x0400047F RID: 1151
	public string key;

	// Token: 0x04000480 RID: 1152
	private string mLanguage;

	// Token: 0x04000481 RID: 1153
	private bool mStarted;
}
