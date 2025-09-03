using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000060 RID: 96
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Toggled Objects")]
public class UIToggledObjects : MonoBehaviour
{
	// Token: 0x0600025B RID: 603 RVA: 0x0001D078 File Offset: 0x0001B278
	private void Awake()
	{
		if (this.target != null)
		{
			if (this.activate.Count == 0 && this.deactivate.Count == 0)
			{
				if (this.inverse)
				{
					this.deactivate.Add(this.target);
				}
				else
				{
					this.activate.Add(this.target);
				}
			}
			else
			{
				this.target = null;
			}
		}
		UIToggle component = base.GetComponent<UIToggle>();
		EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.Toggle));
	}

	// Token: 0x0600025C RID: 604 RVA: 0x0001D114 File Offset: 0x0001B314
	public void Toggle()
	{
		bool value = UIToggle.current.value;
		if (base.enabled)
		{
			for (int i = 0; i < this.activate.Count; i++)
			{
				this.Set(this.activate[i], value);
			}
			for (int j = 0; j < this.deactivate.Count; j++)
			{
				this.Set(this.deactivate[j], !value);
			}
		}
	}

	// Token: 0x0600025D RID: 605 RVA: 0x0000537F File Offset: 0x0000357F
	private void Set(GameObject go, bool state)
	{
		if (go != null)
		{
			NGUITools.SetActive(go, state);
		}
	}

	// Token: 0x04000252 RID: 594
	public List<GameObject> activate;

	// Token: 0x04000253 RID: 595
	public List<GameObject> deactivate;

	// Token: 0x04000254 RID: 596
	[HideInInspector]
	[SerializeField]
	private GameObject target;

	// Token: 0x04000255 RID: 597
	[SerializeField]
	[HideInInspector]
	private bool inverse;
}
