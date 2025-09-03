using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004D RID: 77
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/NGUI Progress Bar")]
public class UIProgressBar : UIWidgetContainer
{
	// Token: 0x1700003B RID: 59
	// (get) Token: 0x060001E4 RID: 484 RVA: 0x00004EA6 File Offset: 0x000030A6
	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x060001E5 RID: 485 RVA: 0x00004ECB File Offset: 0x000030CB
	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = NGUITools.FindCameraForLayer(base.gameObject.layer);
			}
			return this.mCam;
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x060001E6 RID: 486 RVA: 0x00004EFA File Offset: 0x000030FA
	// (set) Token: 0x060001E7 RID: 487 RVA: 0x00004F02 File Offset: 0x00003102
	public UIWidget foregroundWidget
	{
		get
		{
			return this.mFG;
		}
		set
		{
			if (this.mFG != value)
			{
				this.mFG = value;
				this.mIsDirty = true;
			}
		}
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x060001E8 RID: 488 RVA: 0x00004F23 File Offset: 0x00003123
	// (set) Token: 0x060001E9 RID: 489 RVA: 0x00004F2B File Offset: 0x0000312B
	public UIWidget backgroundWidget
	{
		get
		{
			return this.mBG;
		}
		set
		{
			if (this.mBG != value)
			{
				this.mBG = value;
				this.mIsDirty = true;
			}
		}
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x060001EA RID: 490 RVA: 0x00004F4C File Offset: 0x0000314C
	// (set) Token: 0x060001EB RID: 491 RVA: 0x00004F54 File Offset: 0x00003154
	public UIProgressBar.FillDirection fillDirection
	{
		get
		{
			return this.mFill;
		}
		set
		{
			if (this.mFill != value)
			{
				this.mFill = value;
				this.ForceUpdate();
			}
		}
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x060001EC RID: 492 RVA: 0x00004F6F File Offset: 0x0000316F
	// (set) Token: 0x060001ED RID: 493 RVA: 0x0001A1B8 File Offset: 0x000183B8
	public float value
	{
		get
		{
			if (this.numberOfSteps > 1)
			{
				return Mathf.Round(this.mValue * (float)(this.numberOfSteps - 1)) / (float)(this.numberOfSteps - 1);
			}
			return this.mValue;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mValue != num)
			{
				this.mValue = num;
				if (EventDelegate.IsValid(this.onChange))
				{
					UIProgressBar.current = this;
					EventDelegate.Execute(this.onChange);
					UIProgressBar.current = null;
				}
				this.ForceUpdate();
			}
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x060001EE RID: 494 RVA: 0x0001A20C File Offset: 0x0001840C
	// (set) Token: 0x060001EF RID: 495 RVA: 0x0001A258 File Offset: 0x00018458
	public float alpha
	{
		get
		{
			if (this.mFG != null)
			{
				return this.mFG.alpha;
			}
			if (this.mBG != null)
			{
				return this.mBG.alpha;
			}
			return 1f;
		}
		set
		{
			if (this.mFG != null)
			{
				this.mFG.alpha = value;
				if (this.mFG.collider != null)
				{
					this.mFG.collider.enabled = this.mFG.alpha > 0.001f;
				}
			}
			if (this.mBG != null)
			{
				this.mBG.alpha = value;
				if (this.mBG.collider != null)
				{
					this.mBG.collider.enabled = this.mBG.alpha > 0.001f;
				}
			}
			if (this.thumb != null)
			{
				UIWidget component = this.thumb.GetComponent<UIWidget>();
				if (component != null)
				{
					component.alpha = value;
					if (component.collider != null)
					{
						component.collider.enabled = component.alpha > 0.001f;
					}
				}
			}
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x060001F0 RID: 496 RVA: 0x00004FA3 File Offset: 0x000031A3
	protected bool isHorizontal
	{
		get
		{
			return this.mFill == UIProgressBar.FillDirection.LeftToRight || this.mFill == UIProgressBar.FillDirection.RightToLeft;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x060001F1 RID: 497 RVA: 0x00004FBC File Offset: 0x000031BC
	protected bool isInverted
	{
		get
		{
			return this.mFill == UIProgressBar.FillDirection.RightToLeft || this.mFill == UIProgressBar.FillDirection.TopToBottom;
		}
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x0001A368 File Offset: 0x00018568
	protected void Start()
	{
		this.Upgrade();
		if (Application.isPlaying)
		{
			if (this.mFG == null)
			{
				Debug.LogWarning("Progress bar needs a foreground widget to work with", this);
				base.enabled = false;
				return;
			}
			if (this.mBG != null)
			{
				this.mBG.autoResizeBoxCollider = true;
			}
			this.OnStart();
			if (this.onChange != null)
			{
				UIProgressBar.current = this;
				EventDelegate.Execute(this.onChange);
				UIProgressBar.current = null;
			}
		}
		this.ForceUpdate();
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x00003648 File Offset: 0x00001848
	protected virtual void Upgrade()
	{
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x00003648 File Offset: 0x00001848
	protected virtual void OnStart()
	{
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x00004FD6 File Offset: 0x000031D6
	protected void Update()
	{
		if (this.mIsDirty)
		{
			this.ForceUpdate();
		}
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x0001A3F4 File Offset: 0x000185F4
	protected void OnValidate()
	{
		this.Upgrade();
		this.mIsDirty = true;
		float num = Mathf.Clamp01(this.mValue);
		if (this.mValue != num)
		{
			this.mValue = num;
		}
		if (this.numberOfSteps < 0)
		{
			this.numberOfSteps = 0;
		}
		else if (this.numberOfSteps > 20)
		{
			this.numberOfSteps = 20;
		}
		this.ForceUpdate();
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0001A460 File Offset: 0x00018660
	protected float ScreenToValue(Vector2 screenPos)
	{
		Transform cachedTransform = this.cachedTransform;
		Plane plane = new Plane(cachedTransform.rotation * Vector3.back, cachedTransform.position);
		Ray ray = this.cachedCamera.ScreenPointToRay(screenPos);
		float num;
		if (!plane.Raycast(ray, out num))
		{
			return this.value;
		}
		return this.LocalToValue(cachedTransform.InverseTransformPoint(ray.GetPoint(num)));
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x0001A4D4 File Offset: 0x000186D4
	protected virtual float LocalToValue(Vector2 localPos)
	{
		if (!(this.mFG != null))
		{
			return this.value;
		}
		Vector3[] localCorners = this.mFG.localCorners;
		Vector3 vector = localCorners[2] - localCorners[0];
		if (this.isHorizontal)
		{
			float num = (localPos.x - localCorners[0].x) / vector.x;
			return Mathf.Clamp01((!this.isInverted) ? num : (1f - num));
		}
		float num2 = (localPos.y - localCorners[0].y) / vector.y;
		return Mathf.Clamp01((!this.isInverted) ? num2 : (1f - num2));
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0001A5A4 File Offset: 0x000187A4
	public virtual void ForceUpdate()
	{
		this.mIsDirty = false;
		if (this.mFG != null)
		{
			UISprite uisprite = this.mFG as UISprite;
			if (this.isHorizontal)
			{
				if (uisprite != null && uisprite.type == UISprite.Type.Filled)
				{
					uisprite.fillDirection = UISprite.FillDirection.Horizontal;
					uisprite.invert = this.isInverted;
					uisprite.fillAmount = this.value;
				}
				else
				{
					this.mFG.drawRegion = ((!this.isInverted) ? new Vector4(0f, 0f, this.value, 1f) : new Vector4(1f - this.value, 0f, 1f, 1f));
				}
			}
			else if (uisprite != null && uisprite.type == UISprite.Type.Filled)
			{
				uisprite.fillDirection = UISprite.FillDirection.Vertical;
				uisprite.invert = this.isInverted;
				uisprite.fillAmount = this.value;
			}
			else
			{
				this.mFG.drawRegion = ((!this.isInverted) ? new Vector4(0f, 0f, 1f, this.value) : new Vector4(0f, 1f - this.value, 1f, 1f));
			}
		}
		if (this.thumb != null && (this.mFG != null || this.mBG != null))
		{
			Vector3[] array = ((!(this.mFG != null)) ? this.mBG.worldCorners : this.mFG.worldCorners);
			if (this.isHorizontal)
			{
				Vector3 vector = Vector3.Lerp(array[0], array[1], 0.5f);
				Vector3 vector2 = Vector3.Lerp(array[2], array[3], 0.5f);
				this.SetThumbPosition(Vector3.Lerp(vector, vector2, (!this.isInverted) ? this.value : (1f - this.value)));
			}
			else
			{
				Vector3 vector3 = Vector3.Lerp(array[0], array[3], 0.5f);
				Vector3 vector4 = Vector3.Lerp(array[1], array[2], 0.5f);
				this.SetThumbPosition(Vector3.Lerp(vector3, vector4, (!this.isInverted) ? this.value : (1f - this.value)));
			}
		}
	}

	// Token: 0x060001FA RID: 506 RVA: 0x0001A868 File Offset: 0x00018A68
	protected void SetThumbPosition(Vector3 worldPos)
	{
		Transform parent = this.thumb.parent;
		if (parent != null)
		{
			worldPos = parent.InverseTransformPoint(worldPos);
			worldPos.x = Mathf.Round(worldPos.x);
			worldPos.y = Mathf.Round(worldPos.y);
			worldPos.z = 0f;
			if (Vector3.Distance(this.thumb.localPosition, worldPos) > 0.001f)
			{
				this.thumb.localPosition = worldPos;
			}
		}
		else if (Vector3.Distance(this.thumb.position, worldPos) > 1E-05f)
		{
			this.thumb.position = worldPos;
		}
	}

	// Token: 0x040001DF RID: 479
	public static UIProgressBar current;

	// Token: 0x040001E0 RID: 480
	public UIProgressBar.OnDragFinished onDragFinished;

	// Token: 0x040001E1 RID: 481
	public Transform thumb;

	// Token: 0x040001E2 RID: 482
	[SerializeField]
	[HideInInspector]
	protected UIWidget mBG;

	// Token: 0x040001E3 RID: 483
	[HideInInspector]
	[SerializeField]
	protected UIWidget mFG;

	// Token: 0x040001E4 RID: 484
	[SerializeField]
	[HideInInspector]
	protected float mValue = 1f;

	// Token: 0x040001E5 RID: 485
	[SerializeField]
	[HideInInspector]
	protected UIProgressBar.FillDirection mFill;

	// Token: 0x040001E6 RID: 486
	protected Transform mTrans;

	// Token: 0x040001E7 RID: 487
	protected bool mIsDirty;

	// Token: 0x040001E8 RID: 488
	protected Camera mCam;

	// Token: 0x040001E9 RID: 489
	protected float mOffset;

	// Token: 0x040001EA RID: 490
	public int numberOfSteps;

	// Token: 0x040001EB RID: 491
	public List<EventDelegate> onChange = new List<EventDelegate>();

	// Token: 0x0200004E RID: 78
	public enum FillDirection
	{
		// Token: 0x040001ED RID: 493
		LeftToRight,
		// Token: 0x040001EE RID: 494
		RightToLeft,
		// Token: 0x040001EF RID: 495
		BottomToTop,
		// Token: 0x040001F0 RID: 496
		TopToBottom
	}

	// Token: 0x0200004F RID: 79
	// (Invoke) Token: 0x060001FC RID: 508
	public delegate void OnDragFinished();
}
