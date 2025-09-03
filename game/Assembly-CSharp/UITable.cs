using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200005B RID: 91
[AddComponentMenu("NGUI/Interaction/Table")]
public class UITable : UIWidgetContainer
{
	// Token: 0x17000051 RID: 81
	// (set) Token: 0x0600023F RID: 575 RVA: 0x0000522F File Offset: 0x0000342F
	public bool repositionNow
	{
		set
		{
			if (value)
			{
				this.mReposition = true;
				base.enabled = true;
			}
		}
	}

	// Token: 0x06000240 RID: 576 RVA: 0x00004A99 File Offset: 0x00002C99
	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(a.name, b.name);
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x06000241 RID: 577 RVA: 0x0001C7B0 File Offset: 0x0001A9B0
	public List<Transform> children
	{
		get
		{
			if (this.mChildren.Count == 0)
			{
				Transform transform = base.transform;
				this.mChildren.Clear();
				for (int i = 0; i < transform.childCount; i++)
				{
					Transform child = transform.GetChild(i);
					if (child && child.gameObject && (!this.hideInactive || NGUITools.GetActive(child.gameObject)))
					{
						this.mChildren.Add(child);
					}
				}
				if (this.sorted)
				{
					this.mChildren.Sort(new Comparison<Transform>(UITable.SortByName));
				}
			}
			return this.mChildren;
		}
	}

	// Token: 0x06000242 RID: 578 RVA: 0x0001C868 File Offset: 0x0001AA68
	private void RepositionVariableSize(List<Transform> children)
	{
		float num = 0f;
		float num2 = 0f;
		int num3 = ((this.columns <= 0) ? 1 : (children.Count / this.columns + 1));
		int num4 = ((this.columns <= 0) ? children.Count : this.columns);
		Bounds[,] array = new Bounds[num3, num4];
		Bounds[] array2 = new Bounds[num4];
		Bounds[] array3 = new Bounds[num3];
		int num5 = 0;
		int num6 = 0;
		int i = 0;
		int count = children.Count;
		while (i < count)
		{
			Transform transform = children[i];
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform);
			Vector3 localScale = transform.localScale;
			bounds.min = Vector3.Scale(bounds.min, localScale);
			bounds.max = Vector3.Scale(bounds.max, localScale);
			array[num6, num5] = bounds;
			array2[num5].Encapsulate(bounds);
			array3[num6].Encapsulate(bounds);
			if (++num5 >= this.columns && this.columns > 0)
			{
				num5 = 0;
				num6++;
			}
			i++;
		}
		num5 = 0;
		num6 = 0;
		int j = 0;
		int count2 = children.Count;
		while (j < count2)
		{
			Transform transform2 = children[j];
			Bounds bounds2 = array[num6, num5];
			Bounds bounds3 = array2[num5];
			Bounds bounds4 = array3[num6];
			Vector3 localPosition = transform2.localPosition;
			localPosition.x = num + bounds2.extents.x - bounds2.center.x;
			localPosition.x += bounds2.min.x - bounds3.min.x + this.padding.x;
			if (this.direction == UITable.Direction.Down)
			{
				localPosition.y = -num2 - bounds2.extents.y - bounds2.center.y;
				localPosition.y += (bounds2.max.y - bounds2.min.y - bounds4.max.y + bounds4.min.y) * 0.5f - this.padding.y;
			}
			else
			{
				localPosition.y = num2 + (bounds2.extents.y - bounds2.center.y);
				localPosition.y -= (bounds2.max.y - bounds2.min.y - bounds4.max.y + bounds4.min.y) * 0.5f - this.padding.y;
			}
			num += bounds3.max.x - bounds3.min.x + this.padding.x * 2f;
			transform2.localPosition = localPosition;
			if (++num5 >= this.columns && this.columns > 0)
			{
				num5 = 0;
				num6++;
				num = 0f;
				num2 += bounds4.size.y + this.padding.y * 2f;
			}
			j++;
		}
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0001CC20 File Offset: 0x0001AE20
	[ContextMenu("Execute")]
	public void Reposition()
	{
		if (Application.isPlaying && !this.mInitDone)
		{
			this.mReposition = true;
			return;
		}
		if (!this.mInitDone)
		{
			this.Init();
		}
		this.mReposition = false;
		Transform transform = base.transform;
		this.mChildren.Clear();
		List<Transform> children = this.children;
		if (children.Count > 0)
		{
			this.RepositionVariableSize(children);
		}
		if (this.mDrag != null)
		{
			this.mDrag.UpdateScrollbars(true);
			this.mDrag.RestrictWithinBounds(true);
		}
		else if (this.mPanel != null)
		{
			this.mPanel.ConstrainTargetToBounds(transform, true);
		}
		if (this.onReposition != null)
		{
			this.onReposition();
		}
	}

	// Token: 0x06000244 RID: 580 RVA: 0x00005245 File Offset: 0x00003445
	private void Start()
	{
		this.Init();
		this.Reposition();
		base.enabled = false;
	}

	// Token: 0x06000245 RID: 581 RVA: 0x0000525A File Offset: 0x0000345A
	private void Init()
	{
		this.mInitDone = true;
		if (this.keepWithinPanel)
		{
			this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
			this.mDrag = NGUITools.FindInParents<UIScrollView>(base.gameObject);
		}
	}

	// Token: 0x06000246 RID: 582 RVA: 0x00005290 File Offset: 0x00003490
	private void LateUpdate()
	{
		if (this.mReposition)
		{
			this.Reposition();
		}
		base.enabled = false;
	}

	// Token: 0x0400022E RID: 558
	public int columns;

	// Token: 0x0400022F RID: 559
	public UITable.Direction direction;

	// Token: 0x04000230 RID: 560
	public bool sorted;

	// Token: 0x04000231 RID: 561
	public bool hideInactive = true;

	// Token: 0x04000232 RID: 562
	public bool keepWithinPanel;

	// Token: 0x04000233 RID: 563
	public Vector2 padding = Vector2.zero;

	// Token: 0x04000234 RID: 564
	public UITable.OnReposition onReposition;

	// Token: 0x04000235 RID: 565
	private UIPanel mPanel;

	// Token: 0x04000236 RID: 566
	private UIScrollView mDrag;

	// Token: 0x04000237 RID: 567
	private bool mInitDone;

	// Token: 0x04000238 RID: 568
	private bool mReposition;

	// Token: 0x04000239 RID: 569
	private List<Transform> mChildren = new List<Transform>();

	// Token: 0x0200005C RID: 92
	public enum Direction
	{
		// Token: 0x0400023B RID: 571
		Down,
		// Token: 0x0400023C RID: 572
		Up
	}

	// Token: 0x0200005D RID: 93
	// (Invoke) Token: 0x06000248 RID: 584
	public delegate void OnReposition();
}
