using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000016 RID: 22
[AddComponentMenu("NGUI/Examples/UI Item Storage")]
public class UIItemStorage : MonoBehaviour
{
	// Token: 0x17000023 RID: 35
	// (get) Token: 0x060000C7 RID: 199 RVA: 0x00003C59 File Offset: 0x00001E59
	public List<InvGameItem> items
	{
		get
		{
			while (this.mItems.Count < this.maxItemCount)
			{
				this.mItems.Add(null);
			}
			return this.mItems;
		}
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00003C88 File Offset: 0x00001E88
	public InvGameItem GetItem(int slot)
	{
		return (slot >= this.items.Count) ? null : this.mItems[slot];
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00015000 File Offset: 0x00013200
	public InvGameItem Replace(int slot, InvGameItem item)
	{
		if (slot < this.maxItemCount)
		{
			InvGameItem invGameItem = this.items[slot];
			this.mItems[slot] = item;
			return invGameItem;
		}
		return item;
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00015038 File Offset: 0x00013238
	private void Start()
	{
		if (this.template != null)
		{
			int num = 0;
			Bounds bounds = default(Bounds);
			for (int i = 0; i < this.maxRows; i++)
			{
				for (int j = 0; j < this.maxColumns; j++)
				{
					GameObject gameObject = NGUITools.AddChild(base.gameObject, this.template);
					Transform transform = gameObject.transform;
					transform.localPosition = new Vector3((float)this.padding + ((float)j + 0.5f) * (float)this.spacing, (float)(-(float)this.padding) - ((float)i + 0.5f) * (float)this.spacing, 0f);
					UIStorageSlot component = gameObject.GetComponent<UIStorageSlot>();
					if (component != null)
					{
						component.storage = this;
						component.slot = num;
					}
					bounds.Encapsulate(new Vector3((float)this.padding * 2f + (float)((j + 1) * this.spacing), (float)(-(float)this.padding) * 2f - (float)((i + 1) * this.spacing), 0f));
					if (++num >= this.maxItemCount)
					{
						if (this.background != null)
						{
							this.background.transform.localScale = bounds.size;
						}
						return;
					}
				}
			}
			if (this.background != null)
			{
				this.background.transform.localScale = bounds.size;
			}
		}
	}

	// Token: 0x04000081 RID: 129
	public int maxItemCount = 8;

	// Token: 0x04000082 RID: 130
	public int maxRows = 4;

	// Token: 0x04000083 RID: 131
	public int maxColumns = 4;

	// Token: 0x04000084 RID: 132
	public GameObject template;

	// Token: 0x04000085 RID: 133
	public UIWidget background;

	// Token: 0x04000086 RID: 134
	public int spacing = 128;

	// Token: 0x04000087 RID: 135
	public int padding = 10;

	// Token: 0x04000088 RID: 136
	private List<InvGameItem> mItems = new List<InvGameItem>();
}
