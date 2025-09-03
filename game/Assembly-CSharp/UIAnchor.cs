using System;
using UnityEngine;

// Token: 0x0200009C RID: 156
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Anchor")]
public class UIAnchor : MonoBehaviour
{
	// Token: 0x060004DF RID: 1247 RVA: 0x00006EDB File Offset: 0x000050DB
	private void Awake()
	{
		this.mTrans = base.transform;
		this.mAnim = base.animation;
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Combine(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x00006F15 File Offset: 0x00005115
	private void OnDestroy()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Remove(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x00006F37 File Offset: 0x00005137
	private void ScreenSizeChanged()
	{
		if (this.mStarted && this.runOnlyOnce)
		{
			this.Update();
		}
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x00027058 File Offset: 0x00025258
	private void Start()
	{
		if (this.container == null && this.widgetContainer != null)
		{
			this.container = this.widgetContainer.gameObject;
			this.widgetContainer = null;
		}
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.Update();
		this.mStarted = true;
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x000270E4 File Offset: 0x000252E4
	private void Update()
	{
		if (this.mAnim != null && this.mAnim.enabled && this.mAnim.isPlaying)
		{
			return;
		}
		bool flag = false;
		UIWidget uiwidget = ((!(this.container == null)) ? this.container.GetComponent<UIWidget>() : null);
		UIPanel uipanel = ((!(this.container == null) || !(uiwidget == null)) ? this.container.GetComponent<UIPanel>() : null);
		if (uiwidget != null)
		{
			Bounds bounds = uiwidget.CalculateBounds(this.container.transform.parent);
			this.mRect.x = bounds.min.x;
			this.mRect.y = bounds.min.y;
			this.mRect.width = bounds.size.x;
			this.mRect.height = bounds.size.y;
		}
		else if (uipanel != null)
		{
			if (uipanel.clipping == UIDrawCall.Clipping.None)
			{
				float num = ((!(this.mRoot != null)) ? 0.5f : ((float)this.mRoot.activeHeight / (float)Screen.height * 0.5f));
				this.mRect.xMin = (float)(-(float)Screen.width) * num;
				this.mRect.yMin = (float)(-(float)Screen.height) * num;
				this.mRect.xMax = -this.mRect.xMin;
				this.mRect.yMax = -this.mRect.yMin;
			}
			else
			{
				Vector4 finalClipRegion = uipanel.finalClipRegion;
				this.mRect.x = finalClipRegion.x - finalClipRegion.z * 0.5f;
				this.mRect.y = finalClipRegion.y - finalClipRegion.w * 0.5f;
				this.mRect.width = finalClipRegion.z;
				this.mRect.height = finalClipRegion.w;
			}
		}
		else if (this.container != null)
		{
			Transform parent = this.container.transform.parent;
			Bounds bounds2 = ((!(parent != null)) ? NGUIMath.CalculateRelativeWidgetBounds(this.container.transform) : NGUIMath.CalculateRelativeWidgetBounds(parent, this.container.transform));
			this.mRect.x = bounds2.min.x;
			this.mRect.y = bounds2.min.y;
			this.mRect.width = bounds2.size.x;
			this.mRect.height = bounds2.size.y;
		}
		else
		{
			if (!(this.uiCamera != null))
			{
				return;
			}
			flag = true;
			this.mRect = this.uiCamera.pixelRect;
		}
		float num2 = (this.mRect.xMin + this.mRect.xMax) * 0.5f;
		float num3 = (this.mRect.yMin + this.mRect.yMax) * 0.5f;
		Vector3 vector = new Vector3(num2, num3, 0f);
		if (this.side != UIAnchor.Side.Center)
		{
			if (this.side == UIAnchor.Side.Right || this.side == UIAnchor.Side.TopRight || this.side == UIAnchor.Side.BottomRight)
			{
				vector.x = this.mRect.xMax;
			}
			else if (this.side == UIAnchor.Side.Top || this.side == UIAnchor.Side.Center || this.side == UIAnchor.Side.Bottom)
			{
				vector.x = num2;
			}
			else
			{
				vector.x = this.mRect.xMin;
			}
			if (this.side == UIAnchor.Side.Top || this.side == UIAnchor.Side.TopRight || this.side == UIAnchor.Side.TopLeft)
			{
				vector.y = this.mRect.yMax;
			}
			else if (this.side == UIAnchor.Side.Left || this.side == UIAnchor.Side.Center || this.side == UIAnchor.Side.Right)
			{
				vector.y = num3;
			}
			else
			{
				vector.y = this.mRect.yMin;
			}
		}
		float width = this.mRect.width;
		float height = this.mRect.height;
		vector.x += this.pixelOffset.x + this.relativeOffset.x * width;
		vector.y += this.pixelOffset.y + this.relativeOffset.y * height;
		if (flag)
		{
			if (this.uiCamera.orthographic)
			{
				vector.x = Mathf.Round(vector.x);
				vector.y = Mathf.Round(vector.y);
			}
			vector.z = this.uiCamera.WorldToScreenPoint(this.mTrans.position).z;
			vector = this.uiCamera.ScreenToWorldPoint(vector);
		}
		else
		{
			vector.x = Mathf.Round(vector.x);
			vector.y = Mathf.Round(vector.y);
			if (uipanel != null)
			{
				vector = uipanel.cachedTransform.TransformPoint(vector);
			}
			else if (this.container != null)
			{
				Transform parent2 = this.container.transform.parent;
				if (parent2 != null)
				{
					vector = parent2.TransformPoint(vector);
				}
			}
			vector.z = this.mTrans.position.z;
		}
		if (this.mTrans.position != vector)
		{
			this.mTrans.position = vector;
		}
		if (this.runOnlyOnce && Application.isPlaying)
		{
			base.enabled = false;
		}
	}

	// Token: 0x04000396 RID: 918
	public Camera uiCamera;

	// Token: 0x04000397 RID: 919
	public GameObject container;

	// Token: 0x04000398 RID: 920
	public UIAnchor.Side side = UIAnchor.Side.Center;

	// Token: 0x04000399 RID: 921
	public bool runOnlyOnce = true;

	// Token: 0x0400039A RID: 922
	public Vector2 relativeOffset = Vector2.zero;

	// Token: 0x0400039B RID: 923
	public Vector2 pixelOffset = Vector2.zero;

	// Token: 0x0400039C RID: 924
	[SerializeField]
	[HideInInspector]
	private UIWidget widgetContainer;

	// Token: 0x0400039D RID: 925
	private Transform mTrans;

	// Token: 0x0400039E RID: 926
	private Animation mAnim;

	// Token: 0x0400039F RID: 927
	private Rect mRect = default(Rect);

	// Token: 0x040003A0 RID: 928
	private UIRoot mRoot;

	// Token: 0x040003A1 RID: 929
	private bool mStarted;

	// Token: 0x0200009D RID: 157
	public enum Side
	{
		// Token: 0x040003A3 RID: 931
		BottomLeft,
		// Token: 0x040003A4 RID: 932
		Left,
		// Token: 0x040003A5 RID: 933
		TopLeft,
		// Token: 0x040003A6 RID: 934
		Top,
		// Token: 0x040003A7 RID: 935
		TopRight,
		// Token: 0x040003A8 RID: 936
		Right,
		// Token: 0x040003A9 RID: 937
		BottomRight,
		// Token: 0x040003AA RID: 938
		Bottom,
		// Token: 0x040003AB RID: 939
		Center
	}
}
