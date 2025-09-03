using System;
using UnityEngine;

// Token: 0x02000087 RID: 135
[AddComponentMenu("NGUI/UI/NGUI Widget")]
[ExecuteInEditMode]
public class UIWidget : UIRect
{
	// Token: 0x17000096 RID: 150
	// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00006332 File Offset: 0x00004532
	// (set) Token: 0x060003F7 RID: 1015 RVA: 0x0000633A File Offset: 0x0000453A
	public Vector4 drawRegion
	{
		get
		{
			return this.mDrawRegion;
		}
		set
		{
			if (this.mDrawRegion != value)
			{
				this.mDrawRegion = value;
				if (this.autoResizeBoxCollider)
				{
					this.ResizeCollider();
				}
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0000636B File Offset: 0x0000456B
	public Vector2 pivotOffset
	{
		get
		{
			return NGUIMath.GetPivotOffset(this.pivot);
		}
	}

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00006378 File Offset: 0x00004578
	// (set) Token: 0x060003FA RID: 1018 RVA: 0x000242D4 File Offset: 0x000224D4
	public int width
	{
		get
		{
			return this.mWidth;
		}
		set
		{
			int minWidth = this.minWidth;
			if (value < minWidth)
			{
				value = minWidth;
			}
			if (this.mWidth != value)
			{
				this.mWidth = value;
				if (this.autoResizeBoxCollider)
				{
					this.ResizeCollider();
				}
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x060003FB RID: 1019 RVA: 0x00006380 File Offset: 0x00004580
	// (set) Token: 0x060003FC RID: 1020 RVA: 0x0002431C File Offset: 0x0002251C
	public int height
	{
		get
		{
			return this.mHeight;
		}
		set
		{
			int minHeight = this.minHeight;
			if (value < minHeight)
			{
				value = minHeight;
			}
			if (this.mHeight != value)
			{
				this.mHeight = value;
				if (this.autoResizeBoxCollider)
				{
					this.ResizeCollider();
				}
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x060003FD RID: 1021 RVA: 0x00006388 File Offset: 0x00004588
	// (set) Token: 0x060003FE RID: 1022 RVA: 0x00024364 File Offset: 0x00022564
	public Color color
	{
		get
		{
			return this.mColor;
		}
		set
		{
			if (this.mColor != value)
			{
				bool flag = this.mColor.a != value.a;
				this.mColor = value;
				base.Invalidate(flag);
			}
		}
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x060003FF RID: 1023 RVA: 0x00006390 File Offset: 0x00004590
	// (set) Token: 0x06000400 RID: 1024 RVA: 0x0000639D File Offset: 0x0000459D
	public override float alpha
	{
		get
		{
			return this.mColor.a;
		}
		set
		{
			if (this.mColor.a != value)
			{
				this.mColor.a = value;
				base.Invalidate(true);
			}
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x06000401 RID: 1025 RVA: 0x000243A8 File Offset: 0x000225A8
	public override float finalAlpha
	{
		get
		{
			if (!this.mIsVisible || !this.mIsInFront)
			{
				return 0f;
			}
			UIRect parent = base.parent;
			return (!(base.parent != null)) ? this.mColor.a : (parent.finalAlpha * this.mColor.a);
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x06000402 RID: 1026 RVA: 0x0002440C File Offset: 0x0002260C
	public float cumulativeAlpha
	{
		get
		{
			UIRect parent = base.parent;
			return (!(parent != null)) ? this.mColor.a : (parent.finalAlpha * this.mColor.a);
		}
	}

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x06000403 RID: 1027 RVA: 0x000063C3 File Offset: 0x000045C3
	public bool isVisible
	{
		get
		{
			return this.mIsVisible && this.mIsInFront;
		}
	}

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x06000404 RID: 1028 RVA: 0x000063D9 File Offset: 0x000045D9
	public bool hasVertices
	{
		get
		{
			return this.mGeom != null && this.mGeom.hasVertices;
		}
	}

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x06000405 RID: 1029 RVA: 0x000063F4 File Offset: 0x000045F4
	// (set) Token: 0x06000406 RID: 1030 RVA: 0x000063FC File Offset: 0x000045FC
	public UIWidget.Pivot rawPivot
	{
		get
		{
			return this.mPivot;
		}
		set
		{
			if (this.mPivot != value)
			{
				this.mPivot = value;
				if (this.autoResizeBoxCollider)
				{
					this.ResizeCollider();
				}
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x06000407 RID: 1031 RVA: 0x000063F4 File Offset: 0x000045F4
	// (set) Token: 0x06000408 RID: 1032 RVA: 0x00024450 File Offset: 0x00022650
	public UIWidget.Pivot pivot
	{
		get
		{
			return this.mPivot;
		}
		set
		{
			if (this.mPivot != value)
			{
				Vector3 vector = this.worldCorners[0];
				this.mPivot = value;
				this.mChanged = true;
				Vector3 vector2 = this.worldCorners[0];
				Transform cachedTransform = base.cachedTransform;
				Vector3 vector3 = cachedTransform.position;
				float z = cachedTransform.localPosition.z;
				vector3.x += vector.x - vector2.x;
				vector3.y += vector.y - vector2.y;
				base.cachedTransform.position = vector3;
				vector3 = base.cachedTransform.localPosition;
				vector3.x = Mathf.Round(vector3.x);
				vector3.y = Mathf.Round(vector3.y);
				vector3.z = z;
				base.cachedTransform.localPosition = vector3;
			}
		}
	}

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x06000409 RID: 1033 RVA: 0x00006428 File Offset: 0x00004628
	// (set) Token: 0x0600040A RID: 1034 RVA: 0x00006430 File Offset: 0x00004630
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
				this.RemoveFromPanel();
				this.mDepth = value;
				UIPanel.RebuildAllDrawCalls(true);
			}
		}
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x0600040B RID: 1035 RVA: 0x00024548 File Offset: 0x00022748
	public int raycastDepth
	{
		get
		{
			if (this.mPanel == null)
			{
				this.CreatePanel();
			}
			return (!(this.mPanel != null)) ? this.mDepth : (this.mDepth + this.mPanel.depth * 1000);
		}
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x0600040C RID: 1036 RVA: 0x000245A0 File Offset: 0x000227A0
	public override Vector3[] localCorners
	{
		get
		{
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			this.mCorners[0] = new Vector3(num, num2);
			this.mCorners[1] = new Vector3(num, num4);
			this.mCorners[2] = new Vector3(num3, num4);
			this.mCorners[3] = new Vector3(num3, num2);
			return this.mCorners;
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x0600040D RID: 1037 RVA: 0x00024654 File Offset: 0x00022854
	public virtual Vector2 localSize
	{
		get
		{
			Vector3[] localCorners = this.localCorners;
			return localCorners[2] - localCorners[0];
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x0600040E RID: 1038 RVA: 0x0002468C File Offset: 0x0002288C
	public override Vector3[] worldCorners
	{
		get
		{
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			Transform cachedTransform = base.cachedTransform;
			this.mCorners[0] = cachedTransform.TransformPoint(num, num2, 0f);
			this.mCorners[1] = cachedTransform.TransformPoint(num, num4, 0f);
			this.mCorners[2] = cachedTransform.TransformPoint(num3, num4, 0f);
			this.mCorners[3] = cachedTransform.TransformPoint(num3, num2, 0f);
			return this.mCorners;
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x0600040F RID: 1039 RVA: 0x00024764 File Offset: 0x00022964
	public virtual Vector4 drawingDimensions
	{
		get
		{
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			return new Vector4((this.mDrawRegion.x != 0f) ? Mathf.Lerp(num, num3, this.mDrawRegion.x) : num, (this.mDrawRegion.y != 0f) ? Mathf.Lerp(num2, num4, this.mDrawRegion.y) : num2, (this.mDrawRegion.z != 1f) ? Mathf.Lerp(num, num3, this.mDrawRegion.z) : num3, (this.mDrawRegion.w != 1f) ? Mathf.Lerp(num2, num4, this.mDrawRegion.w) : num4);
		}
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x06000410 RID: 1040 RVA: 0x00006451 File Offset: 0x00004651
	// (set) Token: 0x06000411 RID: 1041 RVA: 0x00006454 File Offset: 0x00004654
	public virtual Material material
	{
		get
		{
			return null;
		}
		set
		{
			throw new NotImplementedException(base.GetType() + " has no material setter");
		}
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x06000412 RID: 1042 RVA: 0x0002486C File Offset: 0x00022A6C
	// (set) Token: 0x06000413 RID: 1043 RVA: 0x0000646B File Offset: 0x0000466B
	public virtual Texture mainTexture
	{
		get
		{
			Material material = this.material;
			return (!(material != null)) ? null : material.mainTexture;
		}
		set
		{
			throw new NotImplementedException(base.GetType() + " has no mainTexture setter");
		}
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x06000414 RID: 1044 RVA: 0x00024898 File Offset: 0x00022A98
	// (set) Token: 0x06000415 RID: 1045 RVA: 0x00006482 File Offset: 0x00004682
	public virtual Shader shader
	{
		get
		{
			Material material = this.material;
			return (!(material != null)) ? null : material.shader;
		}
		set
		{
			throw new NotImplementedException(base.GetType() + " has no shader setter");
		}
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x06000416 RID: 1046 RVA: 0x00006499 File Offset: 0x00004699
	// (set) Token: 0x06000417 RID: 1047 RVA: 0x000064B8 File Offset: 0x000046B8
	public UIPanel panel
	{
		get
		{
			if (this.mPanel == null)
			{
				this.CreatePanel();
			}
			return this.mPanel;
		}
		set
		{
			this.mPanel = value;
		}
	}

	// Token: 0x170000AC RID: 172
	// (get) Token: 0x06000418 RID: 1048 RVA: 0x000064C1 File Offset: 0x000046C1
	[Obsolete("There is no relative scale anymore. Widgets now have width and height instead")]
	public Vector2 relativeSize
	{
		get
		{
			return Vector2.one;
		}
	}

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x06000419 RID: 1049 RVA: 0x000248C4 File Offset: 0x00022AC4
	public bool hasBoxCollider
	{
		get
		{
			BoxCollider boxCollider = base.collider as BoxCollider;
			return boxCollider != null;
		}
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x000248E4 File Offset: 0x00022AE4
	public override Vector3[] GetSides(Transform relativeTo)
	{
		Vector2 pivotOffset = this.pivotOffset;
		float num = -pivotOffset.x * (float)this.mWidth;
		float num2 = -pivotOffset.y * (float)this.mHeight;
		float num3 = num + (float)this.mWidth;
		float num4 = num2 + (float)this.mHeight;
		float num5 = (num + num3) * 0.5f;
		float num6 = (num2 + num4) * 0.5f;
		Transform cachedTransform = base.cachedTransform;
		this.mCorners[0] = cachedTransform.TransformPoint(num, num6, 0f);
		this.mCorners[1] = cachedTransform.TransformPoint(num5, num4, 0f);
		this.mCorners[2] = cachedTransform.TransformPoint(num3, num6, 0f);
		this.mCorners[3] = cachedTransform.TransformPoint(num5, num2, 0f);
		if (relativeTo != null)
		{
			for (int i = 0; i < 4; i++)
			{
				this.mCorners[i] = relativeTo.InverseTransformPoint(this.mCorners[i]);
			}
		}
		return this.mCorners;
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x00024A24 File Offset: 0x00022C24
	public void SetRect(float x, float y, float width, float height)
	{
		Vector2 pivotOffset = this.pivotOffset;
		float num = Mathf.Lerp(x, x + width, pivotOffset.x);
		float num2 = Mathf.Lerp(y, y + height, pivotOffset.y);
		int num3 = Mathf.FloorToInt(width + 0.5f);
		int num4 = Mathf.FloorToInt(height + 0.5f);
		if (pivotOffset.x == 0.5f)
		{
			num3 = num3 >> 1 << 1;
		}
		if (pivotOffset.y == 0.5f)
		{
			num4 = num4 >> 1 << 1;
		}
		Transform transform = base.cachedTransform;
		Vector3 localPosition = transform.localPosition;
		localPosition.x = Mathf.Floor(num + 0.5f);
		localPosition.y = Mathf.Floor(num2 + 0.5f);
		if (num3 < this.minWidth)
		{
			num3 = this.minWidth;
		}
		if (num4 < this.minHeight)
		{
			num4 = this.minHeight;
		}
		transform.localPosition = localPosition;
		width = (float)num3;
		height = (float)num4;
		if (base.isAnchored)
		{
			transform = transform.parent;
			if (this.leftAnchor.target)
			{
				this.leftAnchor.SetHorizontal(transform, x);
			}
			if (this.rightAnchor.target)
			{
				this.rightAnchor.SetHorizontal(transform, x + width);
			}
			if (this.bottomAnchor.target)
			{
				this.bottomAnchor.SetVertical(transform, y);
			}
			if (this.topAnchor.target)
			{
				this.topAnchor.SetVertical(transform, y + height);
			}
		}
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x00024BC0 File Offset: 0x00022DC0
	public void ResizeCollider()
	{
		if (NGUITools.GetActive(this))
		{
			BoxCollider boxCollider = base.collider as BoxCollider;
			if (boxCollider != null)
			{
				NGUITools.UpdateWidgetCollider(boxCollider, true);
			}
		}
	}

	// Token: 0x0600041D RID: 1053 RVA: 0x00024BF8 File Offset: 0x00022DF8
	public static int CompareFunc(UIWidget left, UIWidget right)
	{
		int num = UIPanel.CompareFunc(left.mPanel, right.mPanel);
		if (num != 0)
		{
			return num;
		}
		if (left.mDepth < right.mDepth)
		{
			return -1;
		}
		if (left.mDepth > right.mDepth)
		{
			return 1;
		}
		Material material = left.material;
		Material material2 = right.material;
		if (material == material2)
		{
			return 0;
		}
		if (material != null)
		{
			return -1;
		}
		if (material2 != null)
		{
			return 1;
		}
		return (material.GetInstanceID() >= material2.GetInstanceID()) ? 1 : (-1);
	}

	// Token: 0x0600041E RID: 1054 RVA: 0x000064C8 File Offset: 0x000046C8
	public Bounds CalculateBounds()
	{
		return this.CalculateBounds(null);
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x00024C98 File Offset: 0x00022E98
	public Bounds CalculateBounds(Transform relativeParent)
	{
		if (relativeParent == null)
		{
			Vector3[] localCorners = this.localCorners;
			Bounds bounds = new Bounds(localCorners[0], Vector3.zero);
			for (int i = 1; i < 4; i++)
			{
				bounds.Encapsulate(localCorners[i]);
			}
			return bounds;
		}
		Matrix4x4 worldToLocalMatrix = relativeParent.worldToLocalMatrix;
		Vector3[] worldCorners = this.worldCorners;
		Bounds bounds2 = new Bounds(worldToLocalMatrix.MultiplyPoint3x4(worldCorners[0]), Vector3.zero);
		for (int j = 1; j < 4; j++)
		{
			bounds2.Encapsulate(worldToLocalMatrix.MultiplyPoint3x4(worldCorners[j]));
		}
		return bounds2;
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x00024D5C File Offset: 0x00022F5C
	private void SetDirty()
	{
		if (this.drawCall != null)
		{
			this.drawCall.isDirty = true;
		}
		else if (this.isVisible && this.hasVertices)
		{
			this.drawCall = UIPanel.InsertWidget(this);
		}
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x000064D1 File Offset: 0x000046D1
	protected void RemoveFromPanel()
	{
		UIPanel.RemoveWidget(this);
		this.mPanel = null;
		UIWidget.list.Remove(this);
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x00024DB0 File Offset: 0x00022FB0
	public virtual void MarkAsChanged()
	{
		if (this == null)
		{
			return;
		}
		this.mChanged = true;
		if (this.mPanel != null && base.enabled && NGUITools.GetActive(base.gameObject) && !this.mPlayMode)
		{
			this.SetDirty();
			this.CheckLayer();
		}
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x00024E14 File Offset: 0x00023014
	public void CreatePanel()
	{
		if (this.mStarted && this.mPanel == null && base.enabled && NGUITools.GetActive(base.gameObject))
		{
			this.mPanel = UIPanel.Find(base.cachedTransform, this.mStarted, base.cachedGameObject.layer);
			if (this.mPanel != null)
			{
				int raycastDepth = this.raycastDepth;
				bool flag = false;
				for (int i = 0; i < UIWidget.list.size; i++)
				{
					if (UIWidget.list[i].raycastDepth > raycastDepth)
					{
						UIWidget.list.Insert(i, this);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					UIWidget.list.Add(this);
				}
				this.CheckLayer();
				base.Invalidate(true);
				this.drawCall = UIPanel.InsertWidget(this);
			}
		}
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x00024F04 File Offset: 0x00023104
	public void CheckLayer()
	{
		if (this.mPanel != null && this.mPanel.gameObject.layer != base.gameObject.layer)
		{
			Debug.LogWarning("You can't place widgets on a layer different than the UIPanel that manages them.\nIf you want to move widgets to a different layer, parent them to a new panel instead.", this);
			base.gameObject.layer = this.mPanel.gameObject.layer;
		}
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x00024F68 File Offset: 0x00023168
	public override void ParentHasChanged()
	{
		base.ParentHasChanged();
		if (this.mPanel != null)
		{
			UIPanel uipanel = UIPanel.Find(base.cachedTransform, true, base.cachedGameObject.layer);
			if (this.mPanel != uipanel)
			{
				this.RemoveFromPanel();
				this.CreatePanel();
			}
		}
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x000064EC File Offset: 0x000046EC
	protected virtual void Awake()
	{
		this.mGo = base.gameObject;
		this.mPlayMode = Application.isPlaying;
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x00024FC4 File Offset: 0x000231C4
	protected override void OnEnable()
	{
		base.OnEnable();
		this.RemoveFromPanel();
		if (this.mWidth == 100 && this.mHeight == 100 && base.cachedTransform.localScale.magnitude > 8f)
		{
			this.UpgradeFrom265();
			base.cachedTransform.localScale = Vector3.one;
		}
		base.Update();
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x00025030 File Offset: 0x00023230
	protected virtual void UpgradeFrom265()
	{
		Vector3 localScale = base.cachedTransform.localScale;
		this.mWidth = Mathf.Abs(Mathf.RoundToInt(localScale.x));
		this.mHeight = Mathf.Abs(Mathf.RoundToInt(localScale.y));
		if (base.GetComponent<BoxCollider>() != null)
		{
			NGUITools.AddWidgetCollider(base.gameObject, true);
		}
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x00006505 File Offset: 0x00004705
	protected override void OnStart()
	{
		this.mStarted = true;
		this.CreatePanel();
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x00025098 File Offset: 0x00023298
	protected override void OnAnchor()
	{
		Transform cachedTransform = base.cachedTransform;
		Transform parent = cachedTransform.parent;
		Vector3 localPosition = cachedTransform.localPosition;
		Vector2 pivotOffset = this.pivotOffset;
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
				this.mIsInFront = true;
			}
			else
			{
				Vector3 localPos = base.GetLocalPos(this.leftAnchor, parent);
				num = localPos.x + (float)this.leftAnchor.absolute;
				num3 = localPos.y + (float)this.bottomAnchor.absolute;
				num2 = localPos.x + (float)this.rightAnchor.absolute;
				num4 = localPos.y + (float)this.topAnchor.absolute;
				this.mIsInFront = !this.hideIfOffScreen || localPos.z >= 0f;
			}
		}
		else
		{
			this.mIsInFront = true;
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
				num = localPosition.x - pivotOffset.x * (float)this.mWidth;
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
				num2 = localPosition.x - pivotOffset.x * (float)this.mWidth + (float)this.mWidth;
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
				num3 = localPosition.y - pivotOffset.y * (float)this.mHeight;
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
				num4 = localPosition.y - pivotOffset.y * (float)this.mHeight + (float)this.mHeight;
			}
		}
		Vector3 vector = new Vector3(Mathf.Lerp(num, num2, pivotOffset.x), Mathf.Lerp(num3, num4, pivotOffset.y), localPosition.z);
		int num5 = Mathf.FloorToInt(num2 - num + 0.5f);
		int num6 = Mathf.FloorToInt(num4 - num3 + 0.5f);
		if (num5 < this.minWidth)
		{
			num5 = this.minWidth;
		}
		if (num6 < this.minHeight)
		{
			num6 = this.minHeight;
		}
		if (Vector3.SqrMagnitude(localPosition - vector) > 0.001f)
		{
			base.cachedTransform.localPosition = vector;
			if (this.mIsInFront)
			{
				this.mChanged = true;
			}
		}
		if (this.mWidth != num5 || this.mHeight != num6)
		{
			this.mWidth = num5;
			this.mHeight = num6;
			if (this.mIsInFront)
			{
				this.mChanged = true;
			}
			if (this.autoResizeBoxCollider)
			{
				this.ResizeCollider();
			}
		}
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x00006514 File Offset: 0x00004714
	protected override void OnUpdate()
	{
		if (this.mPanel == null)
		{
			this.CreatePanel();
		}
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x0000652D File Offset: 0x0000472D
	private void OnApplicationPause(bool paused)
	{
		if (!paused)
		{
			this.MarkAsChanged();
		}
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x0000653B File Offset: 0x0000473B
	protected override void OnDisable()
	{
		this.RemoveFromPanel();
		base.OnDisable();
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x00006549 File Offset: 0x00004749
	private void OnDestroy()
	{
		this.RemoveFromPanel();
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x00006551 File Offset: 0x00004751
	private bool HasTransformChanged()
	{
		if (base.cachedTransform.hasChanged)
		{
			this.mTrans.hasChanged = false;
			return true;
		}
		return false;
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x00025670 File Offset: 0x00023870
	public bool UpdateGeometry(bool visible)
	{
		bool flag = false;
		float finalAlpha = this.finalAlpha;
		bool flag2 = false;
		if (this.mIsVisible != visible)
		{
			this.mChanged = true;
			this.mIsVisible = visible;
		}
		if (this.HasTransformChanged() && !this.mPanel.widgetsAreStatic)
		{
			this.mLocalToPanel = this.mPanel.worldToLocal * base.cachedTransform.localToWorldMatrix;
			flag = true;
			Vector2 pivotOffset = this.pivotOffset;
			float num = -pivotOffset.x * (float)this.mWidth;
			float num2 = -pivotOffset.y * (float)this.mHeight;
			float num3 = num + (float)this.mWidth;
			float num4 = num2 + (float)this.mHeight;
			Transform cachedTransform = base.cachedTransform;
			Vector3 vector = cachedTransform.TransformPoint(num, num2, 0f);
			Vector3 vector2 = cachedTransform.TransformPoint(num3, num4, 0f);
			vector = this.mPanel.worldToLocal.MultiplyPoint3x4(vector);
			vector2 = this.mPanel.worldToLocal.MultiplyPoint3x4(vector2);
			if (Vector3.SqrMagnitude(this.mOldV0 - vector) > 1E-06f || Vector3.SqrMagnitude(this.mOldV1 - vector2) > 1E-06f)
			{
				flag2 = true;
				this.mOldV0 = vector;
				this.mOldV1 = vector2;
			}
		}
		if (visible && this.mLastAlpha != finalAlpha)
		{
			this.mChanged = true;
		}
		this.mLastAlpha = finalAlpha;
		if (this.mChanged)
		{
			this.mChanged = false;
			if (this.mIsVisible && this.finalAlpha > 0.001f && this.shader != null)
			{
				bool hasVertices = this.mGeom.hasVertices;
				this.mGeom.Clear();
				this.OnFill(this.mGeom.verts, this.mGeom.uvs, this.mGeom.cols);
				if (this.mGeom.hasVertices)
				{
					if (!flag)
					{
						this.mLocalToPanel = this.mPanel.worldToLocal * base.cachedTransform.localToWorldMatrix;
					}
					this.mGeom.ApplyTransform(this.mLocalToPanel);
					return true;
				}
				return hasVertices;
			}
			else if (this.mGeom.hasVertices)
			{
				this.mGeom.Clear();
				return true;
			}
		}
		else if (flag2 && this.mGeom.hasVertices)
		{
			if (!flag)
			{
				this.mLocalToPanel = this.mPanel.worldToLocal * base.cachedTransform.localToWorldMatrix;
			}
			this.mGeom.ApplyTransform(this.mLocalToPanel);
			return true;
		}
		return false;
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x00006572 File Offset: 0x00004772
	public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
	{
		this.mGeom.WriteToBuffers(v, u, c, n, t);
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x00025920 File Offset: 0x00023B20
	public virtual void MakePixelPerfect()
	{
		Vector3 localPosition = base.cachedTransform.localPosition;
		localPosition.z = Mathf.Round(localPosition.z);
		localPosition.x = Mathf.Round(localPosition.x);
		localPosition.y = Mathf.Round(localPosition.y);
		base.cachedTransform.localPosition = localPosition;
		Vector3 localScale = base.cachedTransform.localScale;
		base.cachedTransform.localScale = new Vector3(Mathf.Sign(localScale.x), Mathf.Sign(localScale.y), 1f);
	}

	// Token: 0x170000AE RID: 174
	// (get) Token: 0x06000433 RID: 1075 RVA: 0x00006586 File Offset: 0x00004786
	public virtual int minWidth
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x06000434 RID: 1076 RVA: 0x00006586 File Offset: 0x00004786
	public virtual int minHeight
	{
		get
		{
			return 2;
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x06000435 RID: 1077 RVA: 0x00006589 File Offset: 0x00004789
	public virtual Vector4 border
	{
		get
		{
			return Vector4.zero;
		}
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x00003648 File Offset: 0x00001848
	public virtual void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
	}

	// Token: 0x0400031A RID: 794
	public static BetterList<UIWidget> list = new BetterList<UIWidget>();

	// Token: 0x0400031B RID: 795
	[HideInInspector]
	[SerializeField]
	protected Color mColor = Color.white;

	// Token: 0x0400031C RID: 796
	[HideInInspector]
	[SerializeField]
	protected UIWidget.Pivot mPivot = UIWidget.Pivot.Center;

	// Token: 0x0400031D RID: 797
	[HideInInspector]
	[SerializeField]
	protected int mWidth = 100;

	// Token: 0x0400031E RID: 798
	[SerializeField]
	[HideInInspector]
	protected int mHeight = 100;

	// Token: 0x0400031F RID: 799
	[HideInInspector]
	[SerializeField]
	protected int mDepth;

	// Token: 0x04000320 RID: 800
	public bool autoResizeBoxCollider;

	// Token: 0x04000321 RID: 801
	public bool hideIfOffScreen;

	// Token: 0x04000322 RID: 802
	protected UIPanel mPanel;

	// Token: 0x04000323 RID: 803
	protected bool mPlayMode = true;

	// Token: 0x04000324 RID: 804
	protected Vector4 mDrawRegion = new Vector4(0f, 0f, 1f, 1f);

	// Token: 0x04000325 RID: 805
	private bool mStarted;

	// Token: 0x04000326 RID: 806
	private Matrix4x4 mLocalToPanel;

	// Token: 0x04000327 RID: 807
	private bool mIsVisible = true;

	// Token: 0x04000328 RID: 808
	private bool mIsInFront = true;

	// Token: 0x04000329 RID: 809
	private float mLastAlpha;

	// Token: 0x0400032A RID: 810
	[HideInInspector]
	[NonSerialized]
	public UIDrawCall drawCall;

	// Token: 0x0400032B RID: 811
	protected UIGeometry mGeom = new UIGeometry();

	// Token: 0x0400032C RID: 812
	protected Vector3[] mCorners = new Vector3[4];

	// Token: 0x0400032D RID: 813
	private Vector3 mOldV0;

	// Token: 0x0400032E RID: 814
	private Vector3 mOldV1;

	// Token: 0x02000088 RID: 136
	public enum Pivot
	{
		// Token: 0x04000330 RID: 816
		TopLeft,
		// Token: 0x04000331 RID: 817
		Top,
		// Token: 0x04000332 RID: 818
		TopRight,
		// Token: 0x04000333 RID: 819
		Left,
		// Token: 0x04000334 RID: 820
		Center,
		// Token: 0x04000335 RID: 821
		Right,
		// Token: 0x04000336 RID: 822
		BottomLeft,
		// Token: 0x04000337 RID: 823
		Bottom,
		// Token: 0x04000338 RID: 824
		BottomRight
	}
}
