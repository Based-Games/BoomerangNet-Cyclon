using System;
using UnityEngine;

// Token: 0x02000017 RID: 23
[AddComponentMenu("NGUI/Examples/UI Storage Slot")]
public class UIStorageSlot : UIItemSlot
{
	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060000CC RID: 204 RVA: 0x00003CB5 File Offset: 0x00001EB5
	protected override InvGameItem observedItem
	{
		get
		{
			return (!(this.storage != null)) ? null : this.storage.GetItem(this.slot);
		}
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00003CDF File Offset: 0x00001EDF
	protected override InvGameItem Replace(InvGameItem item)
	{
		return (!(this.storage != null)) ? item : this.storage.Replace(this.slot, item);
	}

	// Token: 0x04000089 RID: 137
	public UIItemStorage storage;

	// Token: 0x0400008A RID: 138
	public int slot;
}
