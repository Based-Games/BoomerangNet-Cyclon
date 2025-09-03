using System;
using UnityEngine;

// Token: 0x02000050 RID: 80
[AddComponentMenu("NGUI/Interaction/Saved Option")]
public class UISavedOption : MonoBehaviour
{
	// Token: 0x17000044 RID: 68
	// (get) Token: 0x06000200 RID: 512 RVA: 0x00004FE9 File Offset: 0x000031E9
	private string key
	{
		get
		{
			return (!string.IsNullOrEmpty(this.keyName)) ? this.keyName : ("NGUI State: " + base.name);
		}
	}

	// Token: 0x06000201 RID: 513 RVA: 0x00005016 File Offset: 0x00003216
	private void Awake()
	{
		this.mList = base.GetComponent<UIPopupList>();
		this.mCheck = base.GetComponent<UIToggle>();
	}

	// Token: 0x06000202 RID: 514 RVA: 0x0001A91C File Offset: 0x00018B1C
	private void OnEnable()
	{
		if (this.mList != null)
		{
			EventDelegate.Add(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
		}
		if (this.mCheck != null)
		{
			EventDelegate.Add(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
		}
		if (this.mList != null)
		{
			string @string = PlayerPrefs.GetString(this.key);
			if (!string.IsNullOrEmpty(@string))
			{
				this.mList.value = @string;
			}
			return;
		}
		if (this.mCheck != null)
		{
			this.mCheck.value = PlayerPrefs.GetInt(this.key, 1) != 0;
		}
		else
		{
			string string2 = PlayerPrefs.GetString(this.key);
			UIToggle[] componentsInChildren = base.GetComponentsInChildren<UIToggle>(true);
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				UIToggle uitoggle = componentsInChildren[i];
				uitoggle.value = uitoggle.name == string2;
				i++;
			}
		}
	}

	// Token: 0x06000203 RID: 515 RVA: 0x0001AA30 File Offset: 0x00018C30
	private void OnDisable()
	{
		if (this.mCheck != null)
		{
			EventDelegate.Remove(this.mCheck.onChange, new EventDelegate.Callback(this.SaveState));
		}
		if (this.mList != null)
		{
			EventDelegate.Remove(this.mList.onChange, new EventDelegate.Callback(this.SaveSelection));
		}
		if (this.mCheck == null && this.mList == null)
		{
			UIToggle[] componentsInChildren = base.GetComponentsInChildren<UIToggle>(true);
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				UIToggle uitoggle = componentsInChildren[i];
				if (uitoggle.value)
				{
					PlayerPrefs.SetString(this.key, uitoggle.name);
					break;
				}
				i++;
			}
		}
	}

	// Token: 0x06000204 RID: 516 RVA: 0x00005030 File Offset: 0x00003230
	private void SaveSelection()
	{
		PlayerPrefs.SetString(this.key, UIPopupList.current.value);
	}

	// Token: 0x06000205 RID: 517 RVA: 0x00005047 File Offset: 0x00003247
	private void SaveState()
	{
		PlayerPrefs.SetInt(this.key, (!UIToggle.current.value) ? 0 : 1);
	}

	// Token: 0x040001F1 RID: 497
	public string keyName;

	// Token: 0x040001F2 RID: 498
	private UIPopupList mList;

	// Token: 0x040001F3 RID: 499
	private UIToggle mCheck;
}
