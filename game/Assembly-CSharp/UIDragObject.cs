using System;
using UnityEngine;

// Token: 0x02000039 RID: 57
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Drag Object")]
public class UIDragObject : MonoBehaviour
{
	// Token: 0x1700002E RID: 46
	// (get) Token: 0x0600015D RID: 349 RVA: 0x000046F6 File Offset: 0x000028F6
	// (set) Token: 0x0600015E RID: 350 RVA: 0x000046FE File Offset: 0x000028FE
	public Vector3 dragMovement
	{
		get
		{
			return this.scale;
		}
		set
		{
			this.scale = value;
		}
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00017388 File Offset: 0x00015588
	private void OnEnable()
	{
		if (this.scrollWheelFactor != 0f)
		{
			this.scrollMomentum = this.scale * this.scrollWheelFactor;
			this.scrollWheelFactor = 0f;
		}
		if (this.contentRect == null && this.target != null && Application.isPlaying)
		{
			UIWidget component = this.target.GetComponent<UIWidget>();
			if (component != null)
			{
				this.contentRect = component;
			}
		}
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00017414 File Offset: 0x00015614
	private void FindPanel()
	{
		this.mPanel = ((!(this.target != null)) ? null : UIPanel.Find(this.target.transform.parent));
		if (this.mPanel == null)
		{
			this.restrictWithinPanel = false;
		}
	}

	// Token: 0x06000161 RID: 353 RVA: 0x0001746C File Offset: 0x0001566C
	private void UpdateBounds()
	{
		if (this.contentRect)
		{
			Transform cachedTransform = this.mPanel.cachedTransform;
			Matrix4x4 worldToLocalMatrix = cachedTransform.worldToLocalMatrix;
			Vector3[] worldCorners = this.contentRect.worldCorners;
			for (int i = 0; i < 4; i++)
			{
				worldCorners[i] = worldToLocalMatrix.MultiplyPoint3x4(worldCorners[i]);
			}
			this.mBounds = new Bounds(worldCorners[0], Vector3.zero);
			for (int j = 1; j < 4; j++)
			{
				this.mBounds.Encapsulate(worldCorners[j]);
			}
		}
		else
		{
			this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mPanel.cachedTransform, this.target);
		}
	}

	// Token: 0x06000162 RID: 354 RVA: 0x00017544 File Offset: 0x00015744
	private void OnPress(bool pressed)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.target != null)
		{
			if (pressed)
			{
				if (!this.mPressed)
				{
					this.mTouchID = UICamera.currentTouchID;
					this.mPressed = true;
					this.mStarted = false;
					this.CancelMovement();
					if (this.restrictWithinPanel && this.mPanel == null)
					{
						this.FindPanel();
					}
					if (this.restrictWithinPanel)
					{
						this.UpdateBounds();
					}
					this.CancelSpring();
					Transform transform = UICamera.currentCamera.transform;
					this.mPlane = new Plane(((!(this.mPanel != null)) ? transform.rotation : this.mPanel.cachedTransform.rotation) * Vector3.back, UICamera.lastHit.point);
				}
			}
			else if (this.mPressed && this.mTouchID == UICamera.currentTouchID)
			{
				this.mPressed = false;
				if (this.restrictWithinPanel && this.dragEffect == UIDragObject.DragEffect.MomentumAndSpring && this.mPanel.ConstrainTargetToBounds(this.target, ref this.mBounds, false))
				{
					this.CancelMovement();
				}
			}
		}
	}

	// Token: 0x06000163 RID: 355 RVA: 0x000176A0 File Offset: 0x000158A0
	private void OnDrag(Vector2 delta)
	{
		if (this.mPressed && this.mTouchID == UICamera.currentTouchID && base.enabled && NGUITools.GetActive(base.gameObject) && this.target != null)
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			float num = 0f;
			if (this.mPlane.Raycast(ray, out num))
			{
				Vector3 point = ray.GetPoint(num);
				Vector3 vector = point - this.mLastPos;
				this.mLastPos = point;
				if (!this.mStarted)
				{
					this.mStarted = true;
					vector = Vector3.zero;
				}
				if (vector.x != 0f || vector.y != 0f)
				{
					vector = this.target.InverseTransformDirection(vector);
					vector.Scale(this.scale);
					vector = this.target.TransformDirection(vector);
				}
				if (this.dragEffect != UIDragObject.DragEffect.None)
				{
					this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + vector * (0.01f * this.momentumAmount), 0.67f);
				}
				Vector3 localPosition = this.target.localPosition;
				this.Move(vector);
				if (this.restrictWithinPanel)
				{
					this.mBounds.center = this.mBounds.center + (this.target.localPosition - localPosition);
					if (this.dragEffect != UIDragObject.DragEffect.MomentumAndSpring && this.mPanel.ConstrainTargetToBounds(this.target, ref this.mBounds, true))
					{
						this.CancelMovement();
					}
				}
			}
		}
	}

	// Token: 0x06000164 RID: 356 RVA: 0x0001786C File Offset: 0x00015A6C
	private void Move(Vector3 worldDelta)
	{
		if (this.mPanel != null)
		{
			this.mTargetPos += worldDelta;
			this.target.position = this.mTargetPos;
			Vector3 localPosition = this.target.localPosition;
			localPosition.x = Mathf.Round(localPosition.x);
			localPosition.y = Mathf.Round(localPosition.y);
			this.target.localPosition = localPosition;
		}
		else
		{
			this.target.position += worldDelta;
		}
	}

	// Token: 0x06000165 RID: 357 RVA: 0x00017908 File Offset: 0x00015B08
	private void LateUpdate()
	{
		if (this.target == null)
		{
			return;
		}
		float deltaTime = RealTime.deltaTime;
		this.mMomentum -= this.mScroll;
		this.mScroll = NGUIMath.SpringLerp(this.mScroll, Vector3.zero, 20f, deltaTime);
		if (!this.mPressed)
		{
			if (this.mMomentum.magnitude < 0.0001f)
			{
				return;
			}
			if (this.mPanel == null)
			{
				this.FindPanel();
			}
			this.Move(NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime));
			if (this.restrictWithinPanel && this.mPanel != null)
			{
				this.UpdateBounds();
				if (this.mPanel.ConstrainTargetToBounds(this.target, ref this.mBounds, this.dragEffect == UIDragObject.DragEffect.None))
				{
					this.CancelMovement();
				}
				else
				{
					this.CancelSpring();
				}
			}
		}
		else
		{
			this.mTargetPos = ((!(this.target != null)) ? Vector3.zero : this.target.position);
		}
		NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
	}

	// Token: 0x06000166 RID: 358 RVA: 0x00017A4C File Offset: 0x00015C4C
	public void CancelMovement()
	{
		this.mTargetPos = ((!(this.target != null)) ? Vector3.zero : this.target.position);
		this.mMomentum = Vector3.zero;
		this.mScroll = Vector3.zero;
	}

	// Token: 0x06000167 RID: 359 RVA: 0x00017A9C File Offset: 0x00015C9C
	public void CancelSpring()
	{
		SpringPosition component = this.target.GetComponent<SpringPosition>();
		if (component != null)
		{
			component.enabled = false;
		}
	}

	// Token: 0x06000168 RID: 360 RVA: 0x00004707 File Offset: 0x00002907
	private void OnScroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			this.mScroll -= this.scrollMomentum * (delta * 0.05f);
		}
	}

	// Token: 0x04000133 RID: 307
	public Transform target;

	// Token: 0x04000134 RID: 308
	public Vector3 scrollMomentum = Vector3.zero;

	// Token: 0x04000135 RID: 309
	public bool restrictWithinPanel;

	// Token: 0x04000136 RID: 310
	public UIRect contentRect;

	// Token: 0x04000137 RID: 311
	public UIDragObject.DragEffect dragEffect = UIDragObject.DragEffect.MomentumAndSpring;

	// Token: 0x04000138 RID: 312
	public float momentumAmount = 35f;

	// Token: 0x04000139 RID: 313
	[SerializeField]
	protected Vector3 scale = new Vector3(1f, 1f, 0f);

	// Token: 0x0400013A RID: 314
	[HideInInspector]
	[SerializeField]
	private float scrollWheelFactor;

	// Token: 0x0400013B RID: 315
	private Plane mPlane;

	// Token: 0x0400013C RID: 316
	private Vector3 mTargetPos;

	// Token: 0x0400013D RID: 317
	private Vector3 mLastPos;

	// Token: 0x0400013E RID: 318
	private UIPanel mPanel;

	// Token: 0x0400013F RID: 319
	private bool mPressed;

	// Token: 0x04000140 RID: 320
	private Vector3 mMomentum = Vector3.zero;

	// Token: 0x04000141 RID: 321
	private Vector3 mScroll = Vector3.zero;

	// Token: 0x04000142 RID: 322
	private Bounds mBounds;

	// Token: 0x04000143 RID: 323
	private int mTouchID;

	// Token: 0x04000144 RID: 324
	private bool mStarted;

	// Token: 0x0200003A RID: 58
	public enum DragEffect
	{
		// Token: 0x04000146 RID: 326
		None,
		// Token: 0x04000147 RID: 327
		Momentum,
		// Token: 0x04000148 RID: 328
		MomentumAndSpring
	}
}
