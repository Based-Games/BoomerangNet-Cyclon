using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003F RID: 63
[AddComponentMenu("NGUI/Interaction/Grid")]
public class UIGrid : UIWidgetContainer
{
	// Token: 0x17000030 RID: 48
	// (set) Token: 0x0600018B RID: 395 RVA: 0x00004A3E File Offset: 0x00002C3E
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

	// Token: 0x0600018C RID: 396 RVA: 0x00004A54 File Offset: 0x00002C54
	private void Init()
	{
		this.mInitDone = true;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
		this.mDrag = NGUITools.FindInParents<UIScrollView>(base.gameObject);
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0001832C File Offset: 0x0001652C
	private void Start()
	{
		if (!this.mInitDone)
		{
			this.Init();
		}
		this.mStarted = true;
		bool flag = this.animateSmoothly;
		this.animateSmoothly = false;
		this.Reposition();
		this.animateSmoothly = flag;
		base.enabled = false;
	}

	// Token: 0x0600018E RID: 398 RVA: 0x00004A7F File Offset: 0x00002C7F
	private void Update()
	{
		if (this.mReposition)
		{
			this.Reposition();
		}
		base.enabled = false;
	}

	// Token: 0x0600018F RID: 399 RVA: 0x00004A99 File Offset: 0x00002C99
	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(a.name, b.name);
	}

	// Token: 0x06000190 RID: 400 RVA: 0x00018374 File Offset: 0x00016574
	[ContextMenu("Execute")]
	public void Reposition()
	{
		if (Application.isPlaying && !this.mStarted)
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
		int num = 0;
		int num2 = 0;
		if (this.sorted)
		{
			List<Transform> list = new List<Transform>();
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				if (child && (!this.hideInactive || NGUITools.GetActive(child.gameObject)))
				{
					list.Add(child);
				}
			}
			list.Sort(new Comparison<Transform>(UIGrid.SortByName));
			int j = 0;
			int count = list.Count;
			while (j < count)
			{
				Transform transform2 = list[j];
				if (NGUITools.GetActive(transform2.gameObject) || !this.hideInactive)
				{
					float z = transform2.localPosition.z;
					Vector3 vector = ((this.arrangement != UIGrid.Arrangement.Horizontal) ? new Vector3(this.cellWidth * (float)num2, -this.cellHeight * (float)num, z) : new Vector3(this.cellWidth * (float)num, -this.cellHeight * (float)num2, z));
					if (this.animateSmoothly && Application.isPlaying)
					{
						SpringPosition.Begin(transform2.gameObject, vector, 15f);
					}
					else
					{
						transform2.localPosition = vector;
					}
					if (++num >= this.maxPerLine && this.maxPerLine > 0)
					{
						num = 0;
						num2++;
					}
				}
				j++;
			}
		}
		else
		{
			for (int k = 0; k < transform.childCount; k++)
			{
				Transform child2 = transform.GetChild(k);
				if (NGUITools.GetActive(child2.gameObject) || !this.hideInactive)
				{
					float z2 = child2.localPosition.z;
					Vector3 vector2 = ((this.arrangement != UIGrid.Arrangement.Horizontal) ? new Vector3(this.cellWidth * (float)num2, -this.cellHeight * (float)num, z2) : new Vector3(this.cellWidth * (float)num, -this.cellHeight * (float)num2, z2));
					if (this.animateSmoothly && Application.isPlaying)
					{
						SpringPosition.Begin(child2.gameObject, vector2, 15f);
					}
					else
					{
						child2.localPosition = vector2;
					}
					if (++num >= this.maxPerLine && this.maxPerLine > 0)
					{
						num = 0;
						num2++;
					}
				}
			}
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

	// Token: 0x04000171 RID: 369
	public UIGrid.Arrangement arrangement;

	// Token: 0x04000172 RID: 370
	public int maxPerLine;

	// Token: 0x04000173 RID: 371
	public float cellWidth = 200f;

	// Token: 0x04000174 RID: 372
	public float cellHeight = 200f;

	// Token: 0x04000175 RID: 373
	public bool animateSmoothly;

	// Token: 0x04000176 RID: 374
	public bool sorted;

	// Token: 0x04000177 RID: 375
	public bool hideInactive = true;

	// Token: 0x04000178 RID: 376
	public UIGrid.OnReposition onReposition;

	// Token: 0x04000179 RID: 377
	private bool mStarted;

	// Token: 0x0400017A RID: 378
	private bool mReposition;

	// Token: 0x0400017B RID: 379
	private UIPanel mPanel;

	// Token: 0x0400017C RID: 380
	private UIScrollView mDrag;

	// Token: 0x0400017D RID: 381
	private bool mInitDone;

	// Token: 0x02000040 RID: 64
	public enum Arrangement
	{
		// Token: 0x0400017F RID: 383
		Horizontal,
		// Token: 0x04000180 RID: 384
		Vertical
	}

	// Token: 0x02000041 RID: 65
	// (Invoke) Token: 0x06000192 RID: 402
	public delegate void OnReposition();
}
