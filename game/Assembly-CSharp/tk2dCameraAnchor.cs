using System;
using UnityEngine;

// Token: 0x02000231 RID: 561
[AddComponentMenu("2D Toolkit/Camera/tk2dCameraAnchor")]
[ExecuteInEditMode]
public class tk2dCameraAnchor : MonoBehaviour
{
	// Token: 0x17000260 RID: 608
	// (get) Token: 0x0600102F RID: 4143 RVA: 0x00074290 File Offset: 0x00072490
	// (set) Token: 0x06001030 RID: 4144 RVA: 0x0000DC54 File Offset: 0x0000BE54
	public tk2dBaseSprite.Anchor AnchorPoint
	{
		get
		{
			if (this.anchor != -1)
			{
				if (this.anchor >= 0 && this.anchor <= 2)
				{
					this._anchorPoint = this.anchor + tk2dBaseSprite.Anchor.UpperLeft;
				}
				else if (this.anchor >= 6 && this.anchor <= 8)
				{
					this._anchorPoint = (tk2dBaseSprite.Anchor)(this.anchor - 6);
				}
				else
				{
					this._anchorPoint = (tk2dBaseSprite.Anchor)this.anchor;
				}
				this.anchor = -1;
			}
			return this._anchorPoint;
		}
		set
		{
			this._anchorPoint = value;
		}
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06001031 RID: 4145 RVA: 0x0000DC5D File Offset: 0x0000BE5D
	// (set) Token: 0x06001032 RID: 4146 RVA: 0x0000DC65 File Offset: 0x0000BE65
	public Vector2 AnchorOffsetPixels
	{
		get
		{
			return this.offset;
		}
		set
		{
			this.offset = value;
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06001033 RID: 4147 RVA: 0x0000DC6E File Offset: 0x0000BE6E
	// (set) Token: 0x06001034 RID: 4148 RVA: 0x0000DC76 File Offset: 0x0000BE76
	public bool AnchorToNativeBounds
	{
		get
		{
			return this.anchorToNativeBounds;
		}
		set
		{
			this.anchorToNativeBounds = value;
		}
	}

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06001035 RID: 4149 RVA: 0x0000DC7F File Offset: 0x0000BE7F
	// (set) Token: 0x06001036 RID: 4150 RVA: 0x0000DCB0 File Offset: 0x0000BEB0
	public Camera AnchorCamera
	{
		get
		{
			if (this.tk2dCamera != null)
			{
				this._anchorCamera = this.tk2dCamera.camera;
				this.tk2dCamera = null;
			}
			return this._anchorCamera;
		}
		set
		{
			this._anchorCamera = value;
			this._anchorCameraCached = null;
		}
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06001037 RID: 4151 RVA: 0x0000DCC0 File Offset: 0x0000BEC0
	private tk2dCamera AnchorTk2dCamera
	{
		get
		{
			if (this._anchorCameraCached != this._anchorCamera)
			{
				this._anchorTk2dCamera = this._anchorCamera.GetComponent<tk2dCamera>();
				this._anchorCameraCached = this._anchorCamera;
			}
			return this._anchorTk2dCamera;
		}
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06001038 RID: 4152 RVA: 0x0000DCFB File Offset: 0x0000BEFB
	private Transform myTransform
	{
		get
		{
			if (this._myTransform == null)
			{
				this._myTransform = base.transform;
			}
			return this._myTransform;
		}
	}

	// Token: 0x06001039 RID: 4153 RVA: 0x0000DD20 File Offset: 0x0000BF20
	private void Start()
	{
		this.UpdateTransform();
	}

	// Token: 0x0600103A RID: 4154 RVA: 0x00074318 File Offset: 0x00072518
	private void UpdateTransform()
	{
		if (this.AnchorCamera == null)
		{
			return;
		}
		float num = 1f;
		Vector3 localPosition = this.myTransform.localPosition;
		this.tk2dCamera = ((!(this.AnchorTk2dCamera != null) || this.AnchorTk2dCamera.CameraSettings.projection == tk2dCameraSettings.ProjectionType.Perspective) ? null : this.AnchorTk2dCamera);
		Rect rect = default(Rect);
		if (this.tk2dCamera != null)
		{
			rect = ((!this.anchorToNativeBounds) ? this.tk2dCamera.ScreenExtents : this.tk2dCamera.NativeScreenExtents);
			num = this.tk2dCamera.GetSizeAtDistance(1f);
		}
		else
		{
			rect.Set(0f, 0f, this.AnchorCamera.pixelWidth, this.AnchorCamera.pixelHeight);
		}
		float yMin = rect.yMin;
		float yMax = rect.yMax;
		float num2 = (yMin + yMax) * 0.5f;
		float xMin = rect.xMin;
		float xMax = rect.xMax;
		float num3 = (xMin + xMax) * 0.5f;
		Vector3 zero = Vector3.zero;
		switch (this.AnchorPoint)
		{
		case tk2dBaseSprite.Anchor.LowerLeft:
			zero = new Vector3(xMin, yMin, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.LowerCenter:
			zero = new Vector3(num3, yMin, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.LowerRight:
			zero = new Vector3(xMax, yMin, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.MiddleLeft:
			zero = new Vector3(xMin, num2, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.MiddleCenter:
			zero = new Vector3(num3, num2, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.MiddleRight:
			zero = new Vector3(xMax, num2, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.UpperLeft:
			zero = new Vector3(xMin, yMax, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.UpperCenter:
			zero = new Vector3(num3, yMax, localPosition.z);
			break;
		case tk2dBaseSprite.Anchor.UpperRight:
			zero = new Vector3(xMax, yMax, localPosition.z);
			break;
		}
		Vector3 vector = zero + new Vector3(num * this.offset.x, num * this.offset.y, 0f);
		if (this.tk2dCamera == null)
		{
			Vector3 vector2 = this.AnchorCamera.ScreenToWorldPoint(vector);
			if (this.myTransform.position != vector2)
			{
				this.myTransform.position = vector2;
			}
		}
		else
		{
			Vector3 localPosition2 = this.myTransform.localPosition;
			if (localPosition2 != vector)
			{
				this.myTransform.localPosition = vector;
			}
		}
	}

	// Token: 0x0600103B RID: 4155 RVA: 0x0000DD20 File Offset: 0x0000BF20
	public void ForceUpdateTransform()
	{
		this.UpdateTransform();
	}

	// Token: 0x0600103C RID: 4156 RVA: 0x0000DD20 File Offset: 0x0000BF20
	private void LateUpdate()
	{
		this.UpdateTransform();
	}

	// Token: 0x040011C0 RID: 4544
	[SerializeField]
	private int anchor = -1;

	// Token: 0x040011C1 RID: 4545
	[SerializeField]
	private tk2dBaseSprite.Anchor _anchorPoint = tk2dBaseSprite.Anchor.UpperLeft;

	// Token: 0x040011C2 RID: 4546
	[SerializeField]
	private bool anchorToNativeBounds;

	// Token: 0x040011C3 RID: 4547
	[SerializeField]
	private Vector2 offset = Vector2.zero;

	// Token: 0x040011C4 RID: 4548
	[SerializeField]
	private tk2dCamera tk2dCamera;

	// Token: 0x040011C5 RID: 4549
	[SerializeField]
	private Camera _anchorCamera;

	// Token: 0x040011C6 RID: 4550
	private Camera _anchorCameraCached;

	// Token: 0x040011C7 RID: 4551
	private tk2dCamera _anchorTk2dCamera;

	// Token: 0x040011C8 RID: 4552
	private Transform _myTransform;
}
