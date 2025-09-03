using System;
using UnityEngine;

// Token: 0x020000B6 RID: 182
[AddComponentMenu("NGUI/UI/NGUI Panel")]
[ExecuteInEditMode]
public class UIPanel : UIRect
{
	// Token: 0x17000128 RID: 296
	// (get) Token: 0x060005DC RID: 1500 RVA: 0x0002DD08 File Offset: 0x0002BF08
	public static int nextUnusedDepth
	{
		get
		{
			int num = int.MinValue;
			for (int i = 0; i < UIPanel.list.size; i++)
			{
				num = Mathf.Max(num, UIPanel.list[i].depth);
			}
			return (num != int.MinValue) ? (num + 1) : 0;
		}
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x060005DD RID: 1501 RVA: 0x00007D50 File Offset: 0x00005F50
	// (set) Token: 0x060005DE RID: 1502 RVA: 0x0002DD64 File Offset: 0x0002BF64
	public override float alpha
	{
		get
		{
			return this.mAlpha;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mAlpha != num)
			{
				this.mAlpha = num;
				this.SetDirty();
			}
		}
	}

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x060005DF RID: 1503 RVA: 0x0002DD94 File Offset: 0x0002BF94
	public override float finalAlpha
	{
		get
		{
			UIRect parent = base.parent;
			return (!(parent != null)) ? this.mAlpha : (parent.finalAlpha * this.mAlpha);
		}
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x060005E0 RID: 1504 RVA: 0x00007D58 File Offset: 0x00005F58
	// (set) Token: 0x060005E1 RID: 1505 RVA: 0x0002DDCC File Offset: 0x0002BFCC
	public int depth
	{
		get
		{
			return this.mDepth;
		}
		set
		{
			if (this.mDepth != value)
			{
				this.mDepth = value;
				UIPanel.mRebuild = true;
				UIDrawCall.SetDirty();
				for (int i = 0; i < UIWidget.list.size; i++)
				{
					UIWidget.list[i].Invalidate(false);
				}
				UIPanel.list.Sort(new BetterList<UIPanel>.CompareFunc(UIPanel.CompareFunc));
				UIWidget.list.Sort(new BetterList<UIWidget>.CompareFunc(UIWidget.CompareFunc));
			}
		}
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0002DE50 File Offset: 0x0002C050
	public static int CompareFunc(UIPanel a, UIPanel b)
	{
		if (!(a != b) || !(a != null) || !(b != null))
		{
			return 0;
		}
		if (a.mDepth < b.mDepth)
		{
			return -1;
		}
		if (a.mDepth > b.mDepth)
		{
			return 1;
		}
		return (a.GetInstanceID() >= b.GetInstanceID()) ? 1 : (-1);
	}

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0002DEC4 File Offset: 0x0002C0C4
	public float width
	{
		get
		{
			return this.GetViewSize().x;
		}
	}

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x060005E4 RID: 1508 RVA: 0x0002DEE0 File Offset: 0x0002C0E0
	public float height
	{
		get
		{
			return this.GetViewSize().y;
		}
	}

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x060005E5 RID: 1509 RVA: 0x00007D60 File Offset: 0x00005F60
	public bool halfPixelOffset
	{
		get
		{
			return this.mHalfPixelOffset;
		}
	}

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x060005E6 RID: 1510 RVA: 0x00007D68 File Offset: 0x00005F68
	public bool usedForUI
	{
		get
		{
			return this.mCam != null && this.mCam.isOrthoGraphic;
		}
	}

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x060005E7 RID: 1511 RVA: 0x0002DEFC File Offset: 0x0002C0FC
	public Vector3 drawCallOffset
	{
		get
		{
			if (this.mHalfPixelOffset && this.mCam != null && this.mCam.isOrthoGraphic)
			{
				float num = 1f / this.GetWindowSize().y / this.mCam.orthographicSize;
				return new Vector3(-num, num);
			}
			return Vector3.zero;
		}
	}

	// Token: 0x17000131 RID: 305
	// (get) Token: 0x060005E8 RID: 1512 RVA: 0x00007D89 File Offset: 0x00005F89
	public int drawCallCount
	{
		get
		{
			return UIDrawCall.Count(this);
		}
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x060005E9 RID: 1513 RVA: 0x00007D91 File Offset: 0x00005F91
	// (set) Token: 0x060005EA RID: 1514 RVA: 0x00007D99 File Offset: 0x00005F99
	public UIDrawCall.Clipping clipping
	{
		get
		{
			return this.mClipping;
		}
		set
		{
			if (this.mClipping != value)
			{
				this.mClipping = value;
				this.mMatrixFrame = -1;
			}
		}
	}

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x060005EB RID: 1515 RVA: 0x00007DB5 File Offset: 0x00005FB5
	// (set) Token: 0x060005EC RID: 1516 RVA: 0x0002DF64 File Offset: 0x0002C164
	public Vector2 clipOffset
	{
		get
		{
			return this.mClipOffset;
		}
		set
		{
			if (Mathf.Abs(this.mClipOffset.x - value.x) > 0.001f || Mathf.Abs(this.mClipOffset.y - value.y) > 0.001f)
			{
				this.mCullTime = ((this.mCullTime != 0f) ? (RealTime.time + 0.15f) : 0.001f);
				this.mClipOffset = value;
				this.mMatrixFrame = -1;
			}
		}
	}

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x060005ED RID: 1517 RVA: 0x00007DBD File Offset: 0x00005FBD
	// (set) Token: 0x060005EE RID: 1518 RVA: 0x00007DC5 File Offset: 0x00005FC5
	[Obsolete("Use 'finalClipRegion' or 'baseClipRegion' instead")]
	public Vector4 clipRange
	{
		get
		{
			return this.baseClipRegion;
		}
		set
		{
			this.baseClipRegion = value;
		}
	}

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x060005EF RID: 1519 RVA: 0x00007DCE File Offset: 0x00005FCE
	// (set) Token: 0x060005F0 RID: 1520 RVA: 0x0002DFF0 File Offset: 0x0002C1F0
	public Vector4 baseClipRegion
	{
		get
		{
			return this.mClipRange;
		}
		set
		{
			if (Mathf.Abs(this.mClipRange.x - value.x) > 0.001f || Mathf.Abs(this.mClipRange.y - value.y) > 0.001f || Mathf.Abs(this.mClipRange.z - value.z) > 0.001f || Mathf.Abs(this.mClipRange.w - value.w) > 0.001f)
			{
				this.mCullTime = ((this.mCullTime != 0f) ? (RealTime.time + 0.15f) : 0.001f);
				this.mClipRange = value;
				this.mMatrixFrame = -1;
			}
		}
	}

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x060005F1 RID: 1521 RVA: 0x0002E0C0 File Offset: 0x0002C2C0
	public Vector4 finalClipRegion
	{
		get
		{
			Vector2 viewSize = this.GetViewSize();
			if (this.mClipping != UIDrawCall.Clipping.None)
			{
				return new Vector4(this.mClipRange.x + this.mClipOffset.x, this.mClipRange.y + this.mClipOffset.y, viewSize.x, viewSize.y);
			}
			return new Vector4(0f, 0f, viewSize.x, viewSize.y);
		}
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x060005F2 RID: 1522 RVA: 0x00007DD6 File Offset: 0x00005FD6
	// (set) Token: 0x060005F3 RID: 1523 RVA: 0x00007DDE File Offset: 0x00005FDE
	public Vector2 clipSoftness
	{
		get
		{
			return this.mClipSoftness;
		}
		set
		{
			if (this.mClipSoftness != value)
			{
				this.mClipSoftness = value;
			}
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x060005F4 RID: 1524 RVA: 0x0002E140 File Offset: 0x0002C340
	public override Vector3[] localCorners
	{
		get
		{
			if (this.mClipping == UIDrawCall.Clipping.None)
			{
				Vector2 viewSize = this.GetViewSize();
				float num = -0.5f * viewSize.x;
				float num2 = -0.5f * viewSize.y;
				float num3 = num + viewSize.x;
				float num4 = num2 + viewSize.y;
				Transform transform = ((!(this.mCam != null)) ? null : this.mCam.transform);
				if (transform != null)
				{
					UIPanel.mCorners[0] = transform.TransformPoint(num, num2, 0f);
					UIPanel.mCorners[1] = transform.TransformPoint(num, num4, 0f);
					UIPanel.mCorners[2] = transform.TransformPoint(num3, num4, 0f);
					UIPanel.mCorners[3] = transform.TransformPoint(num3, num2, 0f);
					transform = base.cachedTransform;
					for (int i = 0; i < 4; i++)
					{
						UIPanel.mCorners[i] = transform.InverseTransformPoint(UIPanel.mCorners[i]);
					}
				}
				else
				{
					UIPanel.mCorners[0] = new Vector3(num, num2);
					UIPanel.mCorners[1] = new Vector3(num, num4);
					UIPanel.mCorners[2] = new Vector3(num3, num4);
					UIPanel.mCorners[3] = new Vector3(num3, num2);
				}
			}
			else
			{
				float num5 = this.mClipOffset.x + this.mClipRange.x - 0.5f * this.mClipRange.z;
				float num6 = this.mClipOffset.y + this.mClipRange.y - 0.5f * this.mClipRange.w;
				float num7 = num5 + this.mClipRange.z;
				float num8 = num6 + this.mClipRange.w;
				UIPanel.mCorners[0] = new Vector3(num5, num6);
				UIPanel.mCorners[1] = new Vector3(num5, num8);
				UIPanel.mCorners[2] = new Vector3(num7, num8);
				UIPanel.mCorners[3] = new Vector3(num7, num6);
			}
			return UIPanel.mCorners;
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x060005F5 RID: 1525 RVA: 0x0002E3CC File Offset: 0x0002C5CC
	public override Vector3[] worldCorners
	{
		get
		{
			if (this.mClipping == UIDrawCall.Clipping.None)
			{
				Vector2 viewSize = this.GetViewSize();
				float num = -0.5f * viewSize.x;
				float num2 = -0.5f * viewSize.y;
				float num3 = num + viewSize.x;
				float num4 = num2 + viewSize.y;
				Transform transform = ((!(this.mCam != null)) ? null : this.mCam.transform);
				if (transform != null)
				{
					UIPanel.mCorners[0] = transform.TransformPoint(num, num2, 0f);
					UIPanel.mCorners[1] = transform.TransformPoint(num, num4, 0f);
					UIPanel.mCorners[2] = transform.TransformPoint(num3, num4, 0f);
					UIPanel.mCorners[3] = transform.TransformPoint(num3, num2, 0f);
				}
			}
			else
			{
				float num5 = this.mClipOffset.x + this.mClipRange.x - 0.5f * this.mClipRange.z;
				float num6 = this.mClipOffset.y + this.mClipRange.y - 0.5f * this.mClipRange.w;
				float num7 = num5 + this.mClipRange.z;
				float num8 = num6 + this.mClipRange.w;
				Transform cachedTransform = base.cachedTransform;
				UIPanel.mCorners[0] = cachedTransform.TransformPoint(num5, num6, 0f);
				UIPanel.mCorners[1] = cachedTransform.TransformPoint(num5, num8, 0f);
				UIPanel.mCorners[2] = cachedTransform.TransformPoint(num7, num8, 0f);
				UIPanel.mCorners[3] = cachedTransform.TransformPoint(num7, num6, 0f);
			}
			return UIPanel.mCorners;
		}
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x0002E5D0 File Offset: 0x0002C7D0
	public override Vector3[] GetSides(Transform relativeTo)
	{
		if (this.mClipping != UIDrawCall.Clipping.None || this.anchorOffset)
		{
			Vector2 viewSize = this.GetViewSize();
			Vector2 vector = ((this.mClipping == UIDrawCall.Clipping.None) ? Vector2.zero : (this.mClipRange + this.mClipOffset));
			float num = vector.x - 0.5f * viewSize.x;
			float num2 = vector.y - 0.5f * viewSize.y;
			float num3 = num + viewSize.x;
			float num4 = num2 + viewSize.y;
			float num5 = (num + num3) * 0.5f;
			float num6 = (num2 + num4) * 0.5f;
			Matrix4x4 localToWorldMatrix = base.cachedTransform.localToWorldMatrix;
			UIPanel.mCorners[0] = localToWorldMatrix.MultiplyPoint3x4(new Vector3(num, num6));
			UIPanel.mCorners[1] = localToWorldMatrix.MultiplyPoint3x4(new Vector3(num5, num4));
			UIPanel.mCorners[2] = localToWorldMatrix.MultiplyPoint3x4(new Vector3(num3, num6));
			UIPanel.mCorners[3] = localToWorldMatrix.MultiplyPoint3x4(new Vector3(num5, num2));
			if (relativeTo != null)
			{
				for (int i = 0; i < 4; i++)
				{
					UIPanel.mCorners[i] = relativeTo.InverseTransformPoint(UIPanel.mCorners[i]);
				}
			}
			return UIPanel.mCorners;
		}
		return base.GetSides(relativeTo);
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x0002E764 File Offset: 0x0002C964
	private bool IsVisible(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{
		this.UpdateTransformMatrix();
		a = this.worldToLocal.MultiplyPoint3x4(a);
		b = this.worldToLocal.MultiplyPoint3x4(b);
		c = this.worldToLocal.MultiplyPoint3x4(c);
		d = this.worldToLocal.MultiplyPoint3x4(d);
		UIPanel.mTemp[0] = a.x;
		UIPanel.mTemp[1] = b.x;
		UIPanel.mTemp[2] = c.x;
		UIPanel.mTemp[3] = d.x;
		float num = Mathf.Min(UIPanel.mTemp);
		float num2 = Mathf.Max(UIPanel.mTemp);
		UIPanel.mTemp[0] = a.y;
		UIPanel.mTemp[1] = b.y;
		UIPanel.mTemp[2] = c.y;
		UIPanel.mTemp[3] = d.y;
		float num3 = Mathf.Min(UIPanel.mTemp);
		float num4 = Mathf.Max(UIPanel.mTemp);
		return num2 >= this.mMin.x && num4 >= this.mMin.y && num <= this.mMax.x && num3 <= this.mMax.y;
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0002E89C File Offset: 0x0002CA9C
	public bool IsVisible(Vector3 worldPos)
	{
		if (this.mAlpha < 0.001f)
		{
			return false;
		}
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			return true;
		}
		this.UpdateTransformMatrix();
		Vector3 vector = this.worldToLocal.MultiplyPoint3x4(worldPos);
		return vector.x >= this.mMin.x && vector.y >= this.mMin.y && vector.x <= this.mMax.x && vector.y <= this.mMax.y;
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0002E940 File Offset: 0x0002CB40
	public bool IsVisible(UIWidget w)
	{
		Vector3[] worldCorners = w.worldCorners;
		return this.IsVisible(worldCorners[0], worldCorners[1], worldCorners[2], worldCorners[3]);
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x00007DF8 File Offset: 0x00005FF8
	public static void RebuildAllDrawCalls(bool sort)
	{
		UIPanel.mRebuild = true;
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x00007E00 File Offset: 0x00006000
	public void SetDirty()
	{
		UIDrawCall.SetDirty(this);
		base.Invalidate(true);
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x0002E98C File Offset: 0x0002CB8C
	private void Awake()
	{
		this.mGo = base.gameObject;
		this.mTrans = base.transform;
		this.mHalfPixelOffset = Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.XBOX360 || Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.WindowsEditor;
		if (this.mHalfPixelOffset)
		{
			this.mHalfPixelOffset = SystemInfo.graphicsShaderLevel < 40;
		}
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x0002EA00 File Offset: 0x0002CC00
	protected override void OnStart()
	{
		this.mLayer = this.mGo.layer;
		UICamera uicamera = UICamera.FindCameraForLayer(this.mLayer);
		this.mCam = ((!(uicamera != null)) ? NGUITools.FindCameraForLayer(this.mLayer) : uicamera.cachedCamera);
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x0002EA54 File Offset: 0x0002CC54
	protected override void OnEnable()
	{
		base.OnEnable();
		if (base.rigidbody == null)
		{
			Rigidbody rigidbody = base.gameObject.AddComponent<Rigidbody>();
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;
		}
		UIPanel.mRebuild = true;
		UIPanel.list.Add(this);
		UIPanel.list.Sort(new BetterList<UIPanel>.CompareFunc(UIPanel.CompareFunc));
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x00007E0F File Offset: 0x0000600F
	protected override void OnDisable()
	{
		UIDrawCall.Destroy(this);
		UIPanel.list.Remove(this);
		if (UIPanel.list.size == 0)
		{
			UIDrawCall.ReleaseAll();
		}
		base.OnDisable();
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x0002EABC File Offset: 0x0002CCBC
	private void UpdateTransformMatrix()
	{
		int frameCount = Time.frameCount;
		if (this.mMatrixFrame != frameCount)
		{
			this.mMatrixFrame = frameCount;
			this.worldToLocal = base.cachedTransform.worldToLocalMatrix;
			Vector2 vector = this.GetViewSize() * 0.5f;
			float num = this.mClipOffset.x + this.mClipRange.x;
			float num2 = this.mClipOffset.y + this.mClipRange.y;
			this.mMin.x = num - vector.x;
			this.mMin.y = num2 - vector.y;
			this.mMax.x = num + vector.x;
			this.mMax.y = num2 + vector.y;
		}
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0002EB84 File Offset: 0x0002CD84
	protected override void OnAnchor()
	{
		if (this.mClipping == UIDrawCall.Clipping.None)
		{
			return;
		}
		Transform cachedTransform = base.cachedTransform;
		Transform parent = cachedTransform.parent;
		Vector2 viewSize = this.GetViewSize();
		Vector2 vector = cachedTransform.localPosition;
		float num;
		float num2;
		float num3;
		float num4;
		if (this.leftAnchor.target == this.bottomAnchor.target && this.leftAnchor.target == this.rightAnchor.target && this.leftAnchor.target == this.topAnchor.target)
		{
			Vector3[] sides = this.leftAnchor.GetSides(parent);
			if (sides != null)
			{
				num = NGUIMath.Lerp(sides[0].x, sides[2].x, this.leftAnchor.relative) + (float)this.leftAnchor.absolute;
				num2 = NGUIMath.Lerp(sides[0].x, sides[2].x, this.rightAnchor.relative) + (float)this.rightAnchor.absolute;
				num3 = NGUIMath.Lerp(sides[3].y, sides[1].y, this.bottomAnchor.relative) + (float)this.bottomAnchor.absolute;
				num4 = NGUIMath.Lerp(sides[3].y, sides[1].y, this.topAnchor.relative) + (float)this.topAnchor.absolute;
			}
			else
			{
				Vector2 vector2 = base.GetLocalPos(this.leftAnchor, parent);
				num = vector2.x + (float)this.leftAnchor.absolute;
				num3 = vector2.y + (float)this.bottomAnchor.absolute;
				num2 = vector2.x + (float)this.rightAnchor.absolute;
				num4 = vector2.y + (float)this.topAnchor.absolute;
			}
		}
		else
		{
			if (this.leftAnchor.target)
			{
				Vector3[] sides2 = this.leftAnchor.GetSides(parent);
				if (sides2 != null)
				{
					num = NGUIMath.Lerp(sides2[0].x, sides2[2].x, this.leftAnchor.relative) + (float)this.leftAnchor.absolute;
				}
				else
				{
					num = base.GetLocalPos(this.leftAnchor, parent).x + (float)this.leftAnchor.absolute;
				}
			}
			else
			{
				num = this.mClipRange.x - 0.5f * viewSize.x;
			}
			if (this.rightAnchor.target)
			{
				Vector3[] sides3 = this.rightAnchor.GetSides(parent);
				if (sides3 != null)
				{
					num2 = NGUIMath.Lerp(sides3[0].x, sides3[2].x, this.rightAnchor.relative) + (float)this.rightAnchor.absolute;
				}
				else
				{
					num2 = base.GetLocalPos(this.rightAnchor, parent).x + (float)this.rightAnchor.absolute;
				}
			}
			else
			{
				num2 = this.mClipRange.x + 0.5f * viewSize.x;
			}
			if (this.bottomAnchor.target)
			{
				Vector3[] sides4 = this.bottomAnchor.GetSides(parent);
				if (sides4 != null)
				{
					num3 = NGUIMath.Lerp(sides4[3].y, sides4[1].y, this.bottomAnchor.relative) + (float)this.bottomAnchor.absolute;
				}
				else
				{
					num3 = base.GetLocalPos(this.bottomAnchor, parent).y + (float)this.bottomAnchor.absolute;
				}
			}
			else
			{
				num3 = this.mClipRange.y - 0.5f * viewSize.y;
			}
			if (this.topAnchor.target)
			{
				Vector3[] sides5 = this.topAnchor.GetSides(parent);
				if (sides5 != null)
				{
					num4 = NGUIMath.Lerp(sides5[3].y, sides5[1].y, this.topAnchor.relative) + (float)this.topAnchor.absolute;
				}
				else
				{
					num4 = base.GetLocalPos(this.topAnchor, parent).y + (float)this.topAnchor.absolute;
				}
			}
			else
			{
				num4 = this.mClipRange.y + 0.5f * viewSize.y;
			}
		}
		num -= vector.x + this.mClipOffset.x;
		num2 -= vector.x + this.mClipOffset.x;
		num3 -= vector.y + this.mClipOffset.y;
		num4 -= vector.y + this.mClipOffset.y;
		float num5 = Mathf.Lerp(num, num2, 0.5f);
		float num6 = Mathf.Lerp(num3, num4, 0.5f);
		float num7 = num2 - num;
		float num8 = num4 - num3;
		float num9 = Mathf.Max(20f, this.mClipSoftness.x);
		float num10 = Mathf.Max(20f, this.mClipSoftness.y);
		if (num7 < num9)
		{
			num7 = num9;
		}
		if (num8 < num10)
		{
			num8 = num10;
		}
		this.baseClipRegion = new Vector4(num5, num6, num7, num8);
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0002F12C File Offset: 0x0002D32C
	private void LateUpdate()
	{
		if (UIPanel.list[0] != this)
		{
			return;
		}
		for (int i = 0; i < UIPanel.list.size; i++)
		{
			UIPanel uipanel = UIPanel.list[i];
			uipanel.mUpdateTime = RealTime.time;
			uipanel.UpdateTransformMatrix();
			uipanel.UpdateLayers();
			uipanel.UpdateWidgets();
		}
		if (UIPanel.mRebuild)
		{
			UIPanel.Fill();
		}
		else
		{
			BetterList<UIDrawCall> activeList = UIDrawCall.activeList;
			int j = 0;
			while (j < activeList.size)
			{
				UIDrawCall uidrawCall = activeList.buffer[j];
				if (uidrawCall.isDirty && !UIPanel.Fill(uidrawCall))
				{
					UIDrawCall.Destroy(uidrawCall);
				}
				else
				{
					j++;
				}
			}
		}
		for (int k = 0; k < UIPanel.list.size; k++)
		{
			UIDrawCall.Update(UIPanel.list[k]);
		}
		UIPanel.mRebuild = false;
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0002F22C File Offset: 0x0002D42C
	private void UpdateLayers()
	{
		if (this.mLayer != base.cachedGameObject.layer)
		{
			this.mLayer = this.mGo.layer;
			UICamera uicamera = UICamera.FindCameraForLayer(this.mLayer);
			this.mCam = ((!(uicamera != null)) ? NGUITools.FindCameraForLayer(this.mLayer) : uicamera.cachedCamera);
			NGUITools.SetChildLayer(base.cachedTransform, this.mLayer);
			UIDrawCall.UpdateLayer(this);
		}
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0002F2AC File Offset: 0x0002D4AC
	private void UpdateWidgets()
	{
		bool flag = !this.cullWhileDragging && this.mCullTime > this.mUpdateTime;
		bool flag2 = false;
		int i = 0;
		int size = UIWidget.list.size;
		while (i < size)
		{
			UIWidget uiwidget = UIWidget.list[i];
			if (uiwidget.panel == this && uiwidget.enabled)
			{
				bool flag3 = flag || (this.mClipping == UIDrawCall.Clipping.None && !uiwidget.hideIfOffScreen) || (uiwidget.cumulativeAlpha > 0.001f && this.IsVisible(uiwidget));
				if (uiwidget.UpdateGeometry(flag3))
				{
					flag2 = true;
					if (!UIPanel.mRebuild)
					{
						if (uiwidget.drawCall != null)
						{
							uiwidget.drawCall.isDirty = true;
						}
						else
						{
							uiwidget.drawCall = UIPanel.InsertWidget(uiwidget);
							if (uiwidget.drawCall == null)
							{
								UIPanel.mRebuild = true;
							}
						}
					}
				}
			}
			i++;
		}
		if (flag2 && this.onChange != null)
		{
			this.onChange();
		}
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0002F3E8 File Offset: 0x0002D5E8
	public static UIDrawCall InsertWidget(UIWidget w)
	{
		UIPanel panel = w.panel;
		if (panel == null)
		{
			return null;
		}
		Material material = w.material;
		Texture mainTexture = w.mainTexture;
		int raycastDepth = w.raycastDepth;
		BetterList<UIDrawCall> activeList = UIDrawCall.activeList;
		for (int i = 0; i < activeList.size; i++)
		{
			UIDrawCall uidrawCall = activeList.buffer[i];
			if (!(uidrawCall.manager != panel))
			{
				int num = ((i != 0) ? (activeList.buffer[i - 1].depthEnd + 1) : int.MinValue);
				int num2 = ((i + 1 != activeList.size) ? (activeList.buffer[i + 1].depthStart - 1) : int.MaxValue);
				if (num <= raycastDepth && num2 >= raycastDepth)
				{
					if (uidrawCall.baseMaterial == material && uidrawCall.mainTexture == mainTexture)
					{
						if (w.isVisible)
						{
							w.drawCall = uidrawCall;
							if (w.hasVertices)
							{
								uidrawCall.isDirty = true;
							}
							return uidrawCall;
						}
					}
					else
					{
						UIPanel.mRebuild = true;
					}
					return null;
				}
			}
		}
		UIPanel.mRebuild = true;
		return null;
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x0002F52C File Offset: 0x0002D72C
	public static void RemoveWidget(UIWidget w)
	{
		if (w.drawCall != null)
		{
			int raycastDepth = w.raycastDepth;
			if (raycastDepth == w.drawCall.depthStart || raycastDepth == w.drawCall.depthEnd)
			{
				UIPanel.mRebuild = true;
			}
			w.drawCall.isDirty = true;
			w.drawCall = null;
		}
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x00007E3D File Offset: 0x0000603D
	public void Refresh()
	{
		UIPanel.mRebuild = true;
		if (UIPanel.list.size > 0)
		{
			UIPanel.list[0].LateUpdate();
		}
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x0002F58C File Offset: 0x0002D78C
	public virtual Vector3 CalculateConstrainOffset(Vector2 min, Vector2 max)
	{
		Vector4 finalClipRegion = this.finalClipRegion;
		float num = finalClipRegion.z * 0.5f;
		float num2 = finalClipRegion.w * 0.5f;
		Vector2 vector = new Vector2(min.x, min.y);
		Vector2 vector2 = new Vector2(max.x, max.y);
		Vector2 vector3 = new Vector2(finalClipRegion.x - num, finalClipRegion.y - num2);
		Vector2 vector4 = new Vector2(finalClipRegion.x + num, finalClipRegion.y + num2);
		if (this.clipping == UIDrawCall.Clipping.SoftClip)
		{
			vector3.x += this.clipSoftness.x;
			vector3.y += this.clipSoftness.y;
			vector4.x -= this.clipSoftness.x;
			vector4.y -= this.clipSoftness.y;
		}
		return NGUIMath.ConstrainRect(vector, vector2, vector3, vector4);
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x0002F6AC File Offset: 0x0002D8AC
	public bool ConstrainTargetToBounds(Transform target, ref Bounds targetBounds, bool immediate)
	{
		Vector3 vector = this.CalculateConstrainOffset(targetBounds.min, targetBounds.max);
		if (vector.magnitude > 0f)
		{
			if (immediate)
			{
				target.localPosition += vector;
				targetBounds.center += vector;
				SpringPosition component = target.GetComponent<SpringPosition>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
			else
			{
				SpringPosition springPosition = SpringPosition.Begin(target.gameObject, target.localPosition + vector, 13f);
				springPosition.ignoreTimeScale = true;
				springPosition.worldSpace = false;
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x0002F760 File Offset: 0x0002D960
	public bool ConstrainTargetToBounds(Transform target, bool immediate)
	{
		Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(base.cachedTransform, target);
		return this.ConstrainTargetToBounds(target, ref bounds, immediate);
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x00007E65 File Offset: 0x00006065
	public static UIPanel Find(Transform trans)
	{
		return UIPanel.Find(trans, false, -1);
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x00007E6F File Offset: 0x0000606F
	public static UIPanel Find(Transform trans, bool createIfMissing)
	{
		return UIPanel.Find(trans, createIfMissing, -1);
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x0002F784 File Offset: 0x0002D984
	public static UIPanel Find(Transform trans, bool createIfMissing, int layer)
	{
		UIPanel uipanel = null;
		while (uipanel == null && trans != null)
		{
			uipanel = trans.GetComponent<UIPanel>();
			if (uipanel != null)
			{
				return uipanel;
			}
			if (trans.parent == null)
			{
				break;
			}
			trans = trans.parent;
		}
		return (!createIfMissing) ? null : NGUITools.CreateUI(trans, false, layer);
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x0002F7F8 File Offset: 0x0002D9F8
	public static void Fill()
	{
		UIDrawCall.ClearAll();
		int num = 0;
		UIPanel uipanel = null;
		Material material = null;
		Texture texture = null;
		Shader shader = null;
		UIDrawCall uidrawCall = null;
		int i = 0;
		while (i < UIWidget.list.size)
		{
			UIWidget uiwidget = UIWidget.list[i];
			if (uiwidget == null)
			{
				UIWidget.list.RemoveAt(i);
			}
			else
			{
				if (uiwidget.isVisible && uiwidget.hasVertices)
				{
					UIPanel panel = uiwidget.panel;
					Material material2 = uiwidget.material;
					Texture mainTexture = uiwidget.mainTexture;
					Shader shader2 = uiwidget.shader;
					if (uipanel != panel || material != material2 || texture != mainTexture || shader != shader2)
					{
						if (uipanel != null && UIPanel.mVerts.size != 0)
						{
							uipanel.SubmitDrawCall(uidrawCall);
							uidrawCall = null;
						}
						uipanel = panel;
						material = material2;
						texture = mainTexture;
						shader = shader2;
					}
					if (uipanel != null && (material != null || shader != null || texture != null))
					{
						if (uidrawCall == null)
						{
							uidrawCall = UIDrawCall.Create(num++, uipanel, material, texture, shader);
							uidrawCall.depthStart = uiwidget.raycastDepth;
							uidrawCall.depthEnd = uidrawCall.depthStart;
							uidrawCall.panel = uipanel;
						}
						else
						{
							int raycastDepth = uiwidget.raycastDepth;
							if (raycastDepth < uidrawCall.depthStart)
							{
								uidrawCall.depthStart = raycastDepth;
							}
							if (raycastDepth > uidrawCall.depthEnd)
							{
								uidrawCall.depthEnd = raycastDepth;
							}
						}
						uiwidget.drawCall = uidrawCall;
						if (uipanel.generateNormals)
						{
							uiwidget.WriteToBuffers(UIPanel.mVerts, UIPanel.mUvs, UIPanel.mCols, UIPanel.mNorms, UIPanel.mTans);
						}
						else
						{
							uiwidget.WriteToBuffers(UIPanel.mVerts, UIPanel.mUvs, UIPanel.mCols, null, null);
						}
					}
				}
				else
				{
					uiwidget.drawCall = null;
				}
				i++;
			}
		}
		if (UIPanel.mVerts.size != 0)
		{
			uipanel.SubmitDrawCall(uidrawCall);
		}
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x0002FA2C File Offset: 0x0002DC2C
	private void SubmitDrawCall(UIDrawCall dc)
	{
		dc.clipping = this.clipping;
		dc.alwaysOnScreen = this.alwaysOnScreen && (this.clipping == UIDrawCall.Clipping.None || this.clipping == UIDrawCall.Clipping.ConstrainButDontClip);
		dc.Set(UIPanel.mVerts, (!this.generateNormals) ? null : UIPanel.mNorms, (!this.generateNormals) ? null : UIPanel.mTans, UIPanel.mUvs, UIPanel.mCols);
		UIPanel.mVerts.Clear();
		UIPanel.mNorms.Clear();
		UIPanel.mTans.Clear();
		UIPanel.mUvs.Clear();
		UIPanel.mCols.Clear();
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x0002FAE4 File Offset: 0x0002DCE4
	private static bool Fill(UIDrawCall dc)
	{
		if (dc != null)
		{
			dc.isDirty = false;
			int i = 0;
			while (i < UIWidget.list.size)
			{
				UIWidget uiwidget = UIWidget.list[i];
				if (uiwidget == null)
				{
					UIWidget.list.RemoveAt(i);
				}
				else
				{
					if (uiwidget.drawCall == dc)
					{
						if (uiwidget.isVisible && uiwidget.hasVertices)
						{
							if (dc.manager.generateNormals)
							{
								uiwidget.WriteToBuffers(UIPanel.mVerts, UIPanel.mUvs, UIPanel.mCols, UIPanel.mNorms, UIPanel.mTans);
							}
							else
							{
								uiwidget.WriteToBuffers(UIPanel.mVerts, UIPanel.mUvs, UIPanel.mCols, null, null);
							}
						}
						else
						{
							uiwidget.drawCall = null;
						}
					}
					i++;
				}
			}
			if (UIPanel.mVerts.size != 0)
			{
				dc.Set(UIPanel.mVerts, (!dc.manager.generateNormals) ? null : UIPanel.mNorms, (!dc.manager.generateNormals) ? null : UIPanel.mTans, UIPanel.mUvs, UIPanel.mCols);
				UIPanel.mVerts.Clear();
				UIPanel.mNorms.Clear();
				UIPanel.mTans.Clear();
				UIPanel.mUvs.Clear();
				UIPanel.mCols.Clear();
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x0002FC54 File Offset: 0x0002DE54
	private Vector2 GetWindowSize()
	{
		UIRoot root = base.root;
		Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
		if (root != null)
		{
			vector *= root.GetPixelSizeAdjustment(Screen.height);
		}
		return vector;
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x0002FC9C File Offset: 0x0002DE9C
	public Vector2 GetViewSize()
	{
		bool flag = this.mClipping != UIDrawCall.Clipping.None;
		Vector2 vector = ((!flag) ? new Vector2((float)Screen.width, (float)Screen.height) : new Vector2(this.mClipRange.z, this.mClipRange.w));
		if (!flag)
		{
			UIRoot root = base.root;
			if (root != null)
			{
				vector *= root.GetPixelSizeAdjustment(Screen.height);
			}
		}
		return vector;
	}

	// Token: 0x04000484 RID: 1156
	public static BetterList<UIPanel> list = new BetterList<UIPanel>();

	// Token: 0x04000485 RID: 1157
	public UIPanel.OnChangeDelegate onChange;

	// Token: 0x04000486 RID: 1158
	public bool showInPanelTool = true;

	// Token: 0x04000487 RID: 1159
	public bool generateNormals;

	// Token: 0x04000488 RID: 1160
	public bool widgetsAreStatic;

	// Token: 0x04000489 RID: 1161
	public bool cullWhileDragging;

	// Token: 0x0400048A RID: 1162
	public bool alwaysOnScreen;

	// Token: 0x0400048B RID: 1163
	public bool anchorOffset;

	// Token: 0x0400048C RID: 1164
	public UIPanel.RenderQueue renderQueue;

	// Token: 0x0400048D RID: 1165
	public int startingRenderQueue = 3000;

	// Token: 0x0400048E RID: 1166
	[HideInInspector]
	public Matrix4x4 worldToLocal = Matrix4x4.identity;

	// Token: 0x0400048F RID: 1167
	[SerializeField]
	[HideInInspector]
	private float mAlpha = 1f;

	// Token: 0x04000490 RID: 1168
	[HideInInspector]
	[SerializeField]
	private UIDrawCall.Clipping mClipping;

	// Token: 0x04000491 RID: 1169
	[HideInInspector]
	[SerializeField]
	private Vector4 mClipRange = new Vector4(0f, 0f, 300f, 200f);

	// Token: 0x04000492 RID: 1170
	[SerializeField]
	[HideInInspector]
	private Vector2 mClipSoftness = new Vector2(4f, 4f);

	// Token: 0x04000493 RID: 1171
	[HideInInspector]
	[SerializeField]
	private int mDepth;

	// Token: 0x04000494 RID: 1172
	private static bool mRebuild = false;

	// Token: 0x04000495 RID: 1173
	private static BetterList<Vector3> mVerts = new BetterList<Vector3>();

	// Token: 0x04000496 RID: 1174
	private static BetterList<Vector3> mNorms = new BetterList<Vector3>();

	// Token: 0x04000497 RID: 1175
	private static BetterList<Vector4> mTans = new BetterList<Vector4>();

	// Token: 0x04000498 RID: 1176
	private static BetterList<Vector2> mUvs = new BetterList<Vector2>();

	// Token: 0x04000499 RID: 1177
	private static BetterList<Color32> mCols = new BetterList<Color32>();

	// Token: 0x0400049A RID: 1178
	private Camera mCam;

	// Token: 0x0400049B RID: 1179
	private float mCullTime;

	// Token: 0x0400049C RID: 1180
	private float mUpdateTime;

	// Token: 0x0400049D RID: 1181
	private int mMatrixFrame = -1;

	// Token: 0x0400049E RID: 1182
	private int mLayer = -1;

	// Token: 0x0400049F RID: 1183
	private Vector2 mClipOffset = Vector2.zero;

	// Token: 0x040004A0 RID: 1184
	private static float[] mTemp = new float[4];

	// Token: 0x040004A1 RID: 1185
	private Vector2 mMin = Vector2.zero;

	// Token: 0x040004A2 RID: 1186
	private Vector2 mMax = Vector2.zero;

	// Token: 0x040004A3 RID: 1187
	private bool mHalfPixelOffset;

	// Token: 0x040004A4 RID: 1188
	private static Vector3[] mCorners = new Vector3[4];

	// Token: 0x020000B7 RID: 183
	public enum DebugInfo
	{
		// Token: 0x040004A6 RID: 1190
		None,
		// Token: 0x040004A7 RID: 1191
		Gizmos,
		// Token: 0x040004A8 RID: 1192
		Geometry
	}

	// Token: 0x020000B8 RID: 184
	public enum RenderQueue
	{
		// Token: 0x040004AA RID: 1194
		Automatic,
		// Token: 0x040004AB RID: 1195
		StartAt,
		// Token: 0x040004AC RID: 1196
		Explicit
	}

	// Token: 0x020000B9 RID: 185
	// (Invoke) Token: 0x06000614 RID: 1556
	public delegate void OnChangeDelegate();
}
