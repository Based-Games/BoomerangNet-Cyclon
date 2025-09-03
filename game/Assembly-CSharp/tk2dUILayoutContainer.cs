using System;
using UnityEngine;

// Token: 0x020002B2 RID: 690
public abstract class tk2dUILayoutContainer : tk2dUILayout
{
	// Token: 0x1400001D RID: 29
	// (add) Token: 0x0600144B RID: 5195 RVA: 0x000118EF File Offset: 0x0000FAEF
	// (remove) Token: 0x0600144C RID: 5196 RVA: 0x00011908 File Offset: 0x0000FB08
	public event Action OnChangeContent;

	// Token: 0x0600144D RID: 5197 RVA: 0x00011921 File Offset: 0x0000FB21
	public Vector2 GetInnerSize()
	{
		return this.innerSize;
	}

	// Token: 0x0600144E RID: 5198
	protected abstract void DoChildLayout();

	// Token: 0x0600144F RID: 5199 RVA: 0x0008904C File Offset: 0x0008724C
	public override void Reshape(Vector3 dMin, Vector3 dMax, bool updateChildren)
	{
		this.bMin += dMin;
		this.bMax += dMax;
		Vector3 vector = new Vector3(this.bMin.x, this.bMax.y);
		base.transform.position += vector;
		this.bMin -= vector;
		this.bMax -= vector;
		this.DoChildLayout();
		if (this.OnChangeContent != null)
		{
			this.OnChangeContent();
		}
	}

	// Token: 0x06001450 RID: 5200 RVA: 0x00011929 File Offset: 0x0000FB29
	public void AddLayout(tk2dUILayout layout, tk2dUILayoutItem item)
	{
		item.gameObj = layout.gameObject;
		item.layout = layout;
		this.layoutItems.Add(item);
		layout.gameObject.transform.parent = base.transform;
		base.Refresh();
	}

	// Token: 0x06001451 RID: 5201 RVA: 0x00011966 File Offset: 0x0000FB66
	public void AddLayoutAtIndex(tk2dUILayout layout, tk2dUILayoutItem item, int index)
	{
		item.gameObj = layout.gameObject;
		item.layout = layout;
		this.layoutItems.Insert(index, item);
		layout.gameObject.transform.parent = base.transform;
		base.Refresh();
	}

	// Token: 0x06001452 RID: 5202 RVA: 0x000890F4 File Offset: 0x000872F4
	public void RemoveLayout(tk2dUILayout layout)
	{
		foreach (tk2dUILayoutItem tk2dUILayoutItem in this.layoutItems)
		{
			if (tk2dUILayoutItem.layout == layout)
			{
				this.layoutItems.Remove(tk2dUILayoutItem);
				layout.gameObject.transform.parent = null;
				break;
			}
		}
		base.Refresh();
	}

	// Token: 0x040015AD RID: 5549
	protected Vector2 innerSize = Vector2.zero;
}
