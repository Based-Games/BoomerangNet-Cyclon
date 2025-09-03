using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200005F RID: 95
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Toggled Components")]
[RequireComponent(typeof(UIToggle))]
public class UIToggledComponents : MonoBehaviour
{
	// Token: 0x06000258 RID: 600 RVA: 0x0001CF4C File Offset: 0x0001B14C
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

	// Token: 0x06000259 RID: 601 RVA: 0x0001CFE8 File Offset: 0x0001B1E8
	public void Toggle()
	{
		if (base.enabled)
		{
			for (int i = 0; i < this.activate.Count; i++)
			{
				MonoBehaviour monoBehaviour = this.activate[i];
				monoBehaviour.enabled = UIToggle.current.value;
			}
			for (int j = 0; j < this.deactivate.Count; j++)
			{
				MonoBehaviour monoBehaviour2 = this.deactivate[j];
				monoBehaviour2.enabled = !UIToggle.current.value;
			}
		}
	}

	// Token: 0x0400024E RID: 590
	public List<MonoBehaviour> activate;

	// Token: 0x0400024F RID: 591
	public List<MonoBehaviour> deactivate;

	// Token: 0x04000250 RID: 592
	[HideInInspector]
	[SerializeField]
	private MonoBehaviour target;

	// Token: 0x04000251 RID: 593
	[HideInInspector]
	[SerializeField]
	private bool inverse;
}
