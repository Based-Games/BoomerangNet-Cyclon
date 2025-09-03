using System;
using UnityEngine;

// Token: 0x0200005A RID: 90
[RequireComponent(typeof(UISlider))]
[AddComponentMenu("NGUI/Interaction/Sound Volume")]
public class UISoundVolume : MonoBehaviour
{
	// Token: 0x0600023C RID: 572 RVA: 0x000051BF File Offset: 0x000033BF
	private void Awake()
	{
		this.mSlider = base.GetComponent<UISlider>();
		this.mSlider.value = NGUITools.soundVolume;
		EventDelegate.Add(this.mSlider.onChange, new EventDelegate.Callback(this.OnChange));
	}

	// Token: 0x0600023D RID: 573 RVA: 0x000051F9 File Offset: 0x000033F9
	private void OnChange()
	{
		NGUITools.soundVolume = UIProgressBar.current.value;
	}

	// Token: 0x0400022D RID: 557
	private UISlider mSlider;
}
