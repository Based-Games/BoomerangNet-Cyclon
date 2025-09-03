using System;
using UnityEngine;

// Token: 0x0200003C RID: 60
[AddComponentMenu("NGUI/Interaction/Drag Scroll View")]
public class UIDragScrollView : MonoBehaviour
{
	// Token: 0x0600016E RID: 366 RVA: 0x00017C84 File Offset: 0x00015E84
	private void OnEnable()
	{
		this.mTrans = base.transform;
		if (this.scrollView == null && this.draggablePanel != null)
		{
			this.scrollView = this.draggablePanel;
			this.draggablePanel = null;
		}
		this.FindScrollView();
	}

	// Token: 0x0600016F RID: 367 RVA: 0x00017CD8 File Offset: 0x00015ED8
	private void FindScrollView()
	{
		UIScrollView uiscrollView = NGUITools.FindInParents<UIScrollView>(this.mTrans);
		if (this.scrollView == null)
		{
			this.scrollView = uiscrollView;
			this.mAutoFind = true;
		}
		else if (this.scrollView == uiscrollView)
		{
			this.mAutoFind = true;
		}
		this.mScroll = this.scrollView;
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0000476F File Offset: 0x0000296F
	private void Start()
	{
		this.FindScrollView();
	}

	// Token: 0x06000171 RID: 369 RVA: 0x00017D3C File Offset: 0x00015F3C
	private void OnPress(bool pressed)
	{
		if (this.mAutoFind && this.mScroll != this.scrollView)
		{
			this.mScroll = this.scrollView;
			this.mAutoFind = false;
		}
		if (this.scrollView && base.enabled && NGUITools.GetActive(base.gameObject))
		{
			this.scrollView.Press(pressed);
			if (!pressed && this.mAutoFind)
			{
				this.scrollView = NGUITools.FindInParents<UIScrollView>(this.mTrans);
				this.mScroll = this.scrollView;
			}
		}
	}

	// Token: 0x06000172 RID: 370 RVA: 0x00004777 File Offset: 0x00002977
	private void OnDrag(Vector2 delta)
	{
		if (this.scrollView && NGUITools.GetActive(this))
		{
			this.scrollView.Drag();
		}
	}

	// Token: 0x06000173 RID: 371 RVA: 0x0000479F File Offset: 0x0000299F
	private void OnScroll(float delta)
	{
		if (this.scrollView && NGUITools.GetActive(this))
		{
			this.scrollView.Scroll(delta);
		}
	}

	// Token: 0x04000153 RID: 339
	public UIScrollView scrollView;

	// Token: 0x04000154 RID: 340
	[HideInInspector]
	[SerializeField]
	private UIScrollView draggablePanel;

	// Token: 0x04000155 RID: 341
	private Transform mTrans;

	// Token: 0x04000156 RID: 342
	private UIScrollView mScroll;

	// Token: 0x04000157 RID: 343
	private bool mAutoFind;
}
