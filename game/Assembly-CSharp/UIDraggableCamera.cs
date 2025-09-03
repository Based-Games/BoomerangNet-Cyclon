using System;
using UnityEngine;

// Token: 0x0200003D RID: 61
[AddComponentMenu("NGUI/Interaction/Draggable Camera")]
[RequireComponent(typeof(Camera))]
public class UIDraggableCamera : MonoBehaviour
{
	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000175 RID: 373 RVA: 0x000047FF File Offset: 0x000029FF
	// (set) Token: 0x06000176 RID: 374 RVA: 0x00004807 File Offset: 0x00002A07
	public Vector2 currentMomentum
	{
		get
		{
			return this.mMomentum;
		}
		set
		{
			this.mMomentum = value;
		}
	}

	// Token: 0x06000177 RID: 375 RVA: 0x00017DE4 File Offset: 0x00015FE4
	private void Awake()
	{
		this.mCam = base.camera;
		this.mTrans = base.transform;
		if (this.rootForBounds == null)
		{
			Debug.LogError(NGUITools.GetHierarchy(base.gameObject) + " needs the 'Root For Bounds' parameter to be set", this);
			base.enabled = false;
		}
	}

	// Token: 0x06000178 RID: 376 RVA: 0x00004810 File Offset: 0x00002A10
	private void Start()
	{
		this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
	}

	// Token: 0x06000179 RID: 377 RVA: 0x00017E3C File Offset: 0x0001603C
	private Vector3 CalculateConstrainOffset()
	{
		if (this.rootForBounds == null || this.rootForBounds.childCount == 0)
		{
			return Vector3.zero;
		}
		Vector3 vector = new Vector3(this.mCam.rect.xMin * (float)Screen.width, this.mCam.rect.yMin * (float)Screen.height, 0f);
		Vector3 vector2 = new Vector3(this.mCam.rect.xMax * (float)Screen.width, this.mCam.rect.yMax * (float)Screen.height, 0f);
		vector = this.mCam.ScreenToWorldPoint(vector);
		vector2 = this.mCam.ScreenToWorldPoint(vector2);
		Vector2 vector3 = new Vector2(this.mBounds.min.x, this.mBounds.min.y);
		Vector2 vector4 = new Vector2(this.mBounds.max.x, this.mBounds.max.y);
		return NGUIMath.ConstrainRect(vector3, vector4, vector, vector2);
	}

	// Token: 0x0600017A RID: 378 RVA: 0x00017F84 File Offset: 0x00016184
	public bool ConstrainToBounds(bool immediate)
	{
		if (this.mTrans != null && this.rootForBounds != null)
		{
			Vector3 vector = this.CalculateConstrainOffset();
			if (vector.magnitude > 0f)
			{
				if (immediate)
				{
					this.mTrans.position -= vector;
				}
				else
				{
					SpringPosition springPosition = SpringPosition.Begin(base.gameObject, this.mTrans.position - vector, 13f);
					springPosition.ignoreTimeScale = true;
					springPosition.worldSpace = true;
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600017B RID: 379 RVA: 0x00018020 File Offset: 0x00016220
	public void Press(bool isPressed)
	{
		if (isPressed)
		{
			this.mDragStarted = false;
		}
		if (this.rootForBounds != null)
		{
			this.mPressed = isPressed;
			if (isPressed)
			{
				this.mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.rootForBounds);
				this.mMomentum = Vector2.zero;
				this.mScroll = 0f;
				SpringPosition component = base.GetComponent<SpringPosition>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
			else if (this.dragEffect == UIDragObject.DragEffect.MomentumAndSpring)
			{
				this.ConstrainToBounds(false);
			}
		}
	}

	// Token: 0x0600017C RID: 380 RVA: 0x000180B4 File Offset: 0x000162B4
	public void Drag(Vector2 delta)
	{
		if (this.smoothDragStart && !this.mDragStarted)
		{
			this.mDragStarted = true;
			return;
		}
		UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
		if (this.mRoot != null)
		{
			delta *= this.mRoot.pixelSizeAdjustment;
		}
		Vector2 vector = Vector2.Scale(delta, -this.scale);
		this.mTrans.localPosition += vector;
		this.mMomentum = Vector2.Lerp(this.mMomentum, this.mMomentum + vector * (0.01f * this.momentumAmount), 0.67f);
		if (this.dragEffect != UIDragObject.DragEffect.MomentumAndSpring && this.ConstrainToBounds(true))
		{
			this.mMomentum = Vector2.zero;
			this.mScroll = 0f;
		}
	}

	// Token: 0x0600017D RID: 381 RVA: 0x000181A0 File Offset: 0x000163A0
	public void Scroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			if (Mathf.Sign(this.mScroll) != Mathf.Sign(delta))
			{
				this.mScroll = 0f;
			}
			this.mScroll += delta * this.scrollWheelFactor;
		}
	}

	// Token: 0x0600017E RID: 382 RVA: 0x00018200 File Offset: 0x00016400
	private void Update()
	{
		float deltaTime = RealTime.deltaTime;
		if (this.mPressed)
		{
			SpringPosition component = base.GetComponent<SpringPosition>();
			if (component != null)
			{
				component.enabled = false;
			}
			this.mScroll = 0f;
		}
		else
		{
			this.mMomentum += this.scale * (this.mScroll * 20f);
			this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0f, 20f, deltaTime);
			if (this.mMomentum.magnitude > 0.01f)
			{
				this.mTrans.localPosition += NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
				this.mBounds = NGUIMath.CalculateAbsoluteWidgetBounds(this.rootForBounds);
				if (!this.ConstrainToBounds(this.dragEffect == UIDragObject.DragEffect.None))
				{
					SpringPosition component2 = base.GetComponent<SpringPosition>();
					if (component2 != null)
					{
						component2.enabled = false;
					}
				}
				return;
			}
			this.mScroll = 0f;
		}
		NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
	}

	// Token: 0x04000158 RID: 344
	public Transform rootForBounds;

	// Token: 0x04000159 RID: 345
	public Vector2 scale = Vector2.one;

	// Token: 0x0400015A RID: 346
	public float scrollWheelFactor;

	// Token: 0x0400015B RID: 347
	public UIDragObject.DragEffect dragEffect = UIDragObject.DragEffect.MomentumAndSpring;

	// Token: 0x0400015C RID: 348
	public bool smoothDragStart = true;

	// Token: 0x0400015D RID: 349
	public float momentumAmount = 35f;

	// Token: 0x0400015E RID: 350
	private Camera mCam;

	// Token: 0x0400015F RID: 351
	private Transform mTrans;

	// Token: 0x04000160 RID: 352
	private bool mPressed;

	// Token: 0x04000161 RID: 353
	private Vector2 mMomentum = Vector2.zero;

	// Token: 0x04000162 RID: 354
	private Bounds mBounds;

	// Token: 0x04000163 RID: 355
	private float mScroll;

	// Token: 0x04000164 RID: 356
	private UIRoot mRoot;

	// Token: 0x04000165 RID: 357
	private bool mDragStarted;
}
