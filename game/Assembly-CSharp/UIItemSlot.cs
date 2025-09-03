using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000015 RID: 21
public abstract class UIItemSlot : MonoBehaviour
{
	// Token: 0x17000022 RID: 34
	// (get) Token: 0x060000BE RID: 190
	protected abstract InvGameItem observedItem { get; }

	// Token: 0x060000BF RID: 191
	protected abstract InvGameItem Replace(InvGameItem item);

	// Token: 0x060000C0 RID: 192 RVA: 0x00014C10 File Offset: 0x00012E10
	private void OnTooltip(bool show)
	{
		InvGameItem invGameItem = ((!show) ? null : this.mItem);
		if (invGameItem != null)
		{
			InvBaseItem baseItem = invGameItem.baseItem;
			if (baseItem != null)
			{
				string text = string.Concat(new string[]
				{
					"[",
					NGUIText.EncodeColor(invGameItem.color),
					"]",
					invGameItem.name,
					"[-]\n"
				});
				string text2 = text;
				text = string.Concat(new object[] { text2, "[AFAFAF]Level ", invGameItem.itemLevel, " ", baseItem.slot });
				List<InvStat> list = invGameItem.CalculateStats();
				int i = 0;
				int count = list.Count;
				while (i < count)
				{
					InvStat invStat = list[i];
					if (invStat.amount != 0)
					{
						if (invStat.amount < 0)
						{
							text = text + "\n[FF0000]" + invStat.amount;
						}
						else
						{
							text = text + "\n[00FF00]+" + invStat.amount;
						}
						if (invStat.modifier == InvStat.Modifier.Percent)
						{
							text += "%";
						}
						text = text + " " + invStat.id;
						text += "[-]";
					}
					i++;
				}
				if (!string.IsNullOrEmpty(baseItem.description))
				{
					text = text + "\n[FF9900]" + baseItem.description;
				}
				UITooltip.ShowText(text);
				return;
			}
		}
		UITooltip.ShowText(null);
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00014DB0 File Offset: 0x00012FB0
	private void OnClick()
	{
		if (UIItemSlot.mDraggedItem != null)
		{
			this.OnDrop(null);
		}
		else if (this.mItem != null)
		{
			UIItemSlot.mDraggedItem = this.Replace(null);
			if (UIItemSlot.mDraggedItem != null)
			{
				NGUITools.PlaySound(this.grabSound);
			}
			this.UpdateCursor();
		}
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00003BDE File Offset: 0x00001DDE
	private void OnDrag(Vector2 delta)
	{
		if (UIItemSlot.mDraggedItem == null && this.mItem != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			UIItemSlot.mDraggedItem = this.Replace(null);
			NGUITools.PlaySound(this.grabSound);
			this.UpdateCursor();
		}
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00014E08 File Offset: 0x00013008
	private void OnDrop(GameObject go)
	{
		InvGameItem invGameItem = this.Replace(UIItemSlot.mDraggedItem);
		if (UIItemSlot.mDraggedItem == invGameItem)
		{
			NGUITools.PlaySound(this.errorSound);
		}
		else if (invGameItem != null)
		{
			NGUITools.PlaySound(this.grabSound);
		}
		else
		{
			NGUITools.PlaySound(this.placeSound);
		}
		UIItemSlot.mDraggedItem = invGameItem;
		this.UpdateCursor();
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00014E6C File Offset: 0x0001306C
	private void UpdateCursor()
	{
		if (UIItemSlot.mDraggedItem != null && UIItemSlot.mDraggedItem.baseItem != null)
		{
			UICursor.Set(UIItemSlot.mDraggedItem.baseItem.iconAtlas, UIItemSlot.mDraggedItem.baseItem.iconName);
		}
		else
		{
			UICursor.Clear();
		}
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00014EC0 File Offset: 0x000130C0
	private void Update()
	{
		InvGameItem observedItem = this.observedItem;
		if (this.mItem != observedItem)
		{
			this.mItem = observedItem;
			InvBaseItem invBaseItem = ((observedItem == null) ? null : observedItem.baseItem);
			if (this.label != null)
			{
				string text = ((observedItem == null) ? null : observedItem.name);
				if (string.IsNullOrEmpty(this.mText))
				{
					this.mText = this.label.text;
				}
				this.label.text = ((text == null) ? this.mText : text);
			}
			if (this.icon != null)
			{
				if (invBaseItem == null || invBaseItem.iconAtlas == null)
				{
					this.icon.enabled = false;
				}
				else
				{
					this.icon.atlas = invBaseItem.iconAtlas;
					this.icon.spriteName = invBaseItem.iconName;
					this.icon.enabled = true;
					this.icon.MakePixelPerfect();
				}
			}
			if (this.background != null)
			{
				this.background.color = ((observedItem == null) ? Color.white : observedItem.color);
			}
		}
	}

	// Token: 0x04000078 RID: 120
	public UISprite icon;

	// Token: 0x04000079 RID: 121
	public UIWidget background;

	// Token: 0x0400007A RID: 122
	public UILabel label;

	// Token: 0x0400007B RID: 123
	public AudioClip grabSound;

	// Token: 0x0400007C RID: 124
	public AudioClip placeSound;

	// Token: 0x0400007D RID: 125
	public AudioClip errorSound;

	// Token: 0x0400007E RID: 126
	private InvGameItem mItem;

	// Token: 0x0400007F RID: 127
	private string mText = string.Empty;

	// Token: 0x04000080 RID: 128
	private static InvGameItem mDraggedItem;
}
