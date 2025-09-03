using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000230 RID: 560
[AddComponentMenu("2D Toolkit/Camera/tk2dCamera")]
[ExecuteInEditMode]
public class tk2dCamera : MonoBehaviour
{
	// Token: 0x1700024F RID: 591
	// (get) Token: 0x0600100E RID: 4110 RVA: 0x0000DB2A File Offset: 0x0000BD2A
	public tk2dCameraSettings CameraSettings
	{
		get
		{
			return this.cameraSettings;
		}
	}

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x0600100F RID: 4111 RVA: 0x00073424 File Offset: 0x00071624
	public tk2dCameraResolutionOverride CurrentResolutionOverride
	{
		get
		{
			tk2dCamera settingsRoot = this.SettingsRoot;
			Camera screenCamera = this.ScreenCamera;
			float pixelWidth = screenCamera.pixelWidth;
			float pixelHeight = screenCamera.pixelHeight;
			tk2dCameraResolutionOverride tk2dCameraResolutionOverride = null;
			if (tk2dCameraResolutionOverride == null || (tk2dCameraResolutionOverride != null && ((float)tk2dCameraResolutionOverride.width != pixelWidth || (float)tk2dCameraResolutionOverride.height != pixelHeight)))
			{
				tk2dCameraResolutionOverride = null;
				if (settingsRoot.resolutionOverride != null)
				{
					foreach (tk2dCameraResolutionOverride tk2dCameraResolutionOverride2 in settingsRoot.resolutionOverride)
					{
						if (tk2dCameraResolutionOverride2.Match((int)pixelWidth, (int)pixelHeight))
						{
							tk2dCameraResolutionOverride = tk2dCameraResolutionOverride2;
							break;
						}
					}
				}
			}
			return tk2dCameraResolutionOverride;
		}
	}

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x06001010 RID: 4112 RVA: 0x0000DB32 File Offset: 0x0000BD32
	// (set) Token: 0x06001011 RID: 4113 RVA: 0x0000DB3A File Offset: 0x0000BD3A
	public tk2dCamera InheritConfig
	{
		get
		{
			return this.inheritSettings;
		}
		set
		{
			if (this.inheritSettings != value)
			{
				this.inheritSettings = value;
				this._settingsRoot = null;
			}
		}
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06001012 RID: 4114 RVA: 0x0000DB5B File Offset: 0x0000BD5B
	private Camera UnityCamera
	{
		get
		{
			if (this._unityCamera == null)
			{
				this._unityCamera = base.camera;
				if (this._unityCamera == null)
				{
					Debug.LogError("A unity camera must be attached to the tk2dCamera script");
				}
			}
			return this._unityCamera;
		}
	}

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06001013 RID: 4115 RVA: 0x0000DB9B File Offset: 0x0000BD9B
	public static tk2dCamera Instance
	{
		get
		{
			return tk2dCamera.inst;
		}
	}

	// Token: 0x06001014 RID: 4116 RVA: 0x000734CC File Offset: 0x000716CC
	public static tk2dCamera CameraForLayer(int layer)
	{
		int num = 1 << layer;
		int count = tk2dCamera.allCameras.Count;
		for (int i = 0; i < count; i++)
		{
			tk2dCamera tk2dCamera = tk2dCamera.allCameras[i];
			if ((tk2dCamera.UnityCamera.cullingMask & num) == num)
			{
				return tk2dCamera;
			}
		}
		return null;
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06001015 RID: 4117 RVA: 0x0000DBA2 File Offset: 0x0000BDA2
	public Rect ScreenExtents
	{
		get
		{
			return this._screenExtents;
		}
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x06001016 RID: 4118 RVA: 0x0000DBAA File Offset: 0x0000BDAA
	public Rect NativeScreenExtents
	{
		get
		{
			return this._nativeScreenExtents;
		}
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06001017 RID: 4119 RVA: 0x0000DBB2 File Offset: 0x0000BDB2
	public Vector2 TargetResolution
	{
		get
		{
			return this._targetResolution;
		}
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06001018 RID: 4120 RVA: 0x0000DBBA File Offset: 0x0000BDBA
	public Vector2 NativeResolution
	{
		get
		{
			return new Vector2((float)this.nativeResolutionWidth, (float)this.nativeResolutionHeight);
		}
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06001019 RID: 4121 RVA: 0x00073520 File Offset: 0x00071720
	[Obsolete]
	public Vector2 ScreenOffset
	{
		get
		{
			return new Vector2(this.ScreenExtents.xMin - this.NativeScreenExtents.xMin, this.ScreenExtents.yMin - this.NativeScreenExtents.yMin);
		}
	}

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x0600101A RID: 4122 RVA: 0x0007356C File Offset: 0x0007176C
	[Obsolete]
	public Vector2 resolution
	{
		get
		{
			return new Vector2(this.ScreenExtents.xMax, this.ScreenExtents.yMax);
		}
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x0600101B RID: 4123 RVA: 0x0007356C File Offset: 0x0007176C
	[Obsolete]
	public Vector2 ScreenResolution
	{
		get
		{
			return new Vector2(this.ScreenExtents.xMax, this.ScreenExtents.yMax);
		}
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x0600101C RID: 4124 RVA: 0x0007359C File Offset: 0x0007179C
	[Obsolete]
	public Vector2 ScaledResolution
	{
		get
		{
			return new Vector2(this.ScreenExtents.width, this.ScreenExtents.height);
		}
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x0600101D RID: 4125 RVA: 0x0000DBCF File Offset: 0x0000BDCF
	// (set) Token: 0x0600101E RID: 4126 RVA: 0x0000DBD7 File Offset: 0x0000BDD7
	public float ZoomFactor
	{
		get
		{
			return this.zoomFactor;
		}
		set
		{
			this.zoomFactor = Mathf.Max(0.01f, value);
		}
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x0600101F RID: 4127 RVA: 0x0000DBEA File Offset: 0x0000BDEA
	[Obsolete]
	public float zoomScale
	{
		get
		{
			return 1f / Mathf.Max(0.01f, this.zoomFactor);
		}
	}

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06001020 RID: 4128 RVA: 0x000735CC File Offset: 0x000717CC
	public Camera ScreenCamera
	{
		get
		{
			bool flag = this.viewportClippingEnabled && this.inheritSettings != null && this.inheritSettings.UnityCamera.rect == this.unitRect;
			return (!flag) ? this.UnityCamera : this.inheritSettings.UnityCamera;
		}
	}

	// Token: 0x06001021 RID: 4129 RVA: 0x0000DC02 File Offset: 0x0000BE02
	private void Awake()
	{
		this.Upgrade();
		if (tk2dCamera.allCameras.IndexOf(this) == -1)
		{
			tk2dCamera.allCameras.Add(this);
		}
	}

	// Token: 0x06001022 RID: 4130 RVA: 0x00073630 File Offset: 0x00071830
	private void OnEnable()
	{
		if (this.UnityCamera != null)
		{
			this.UpdateCameraMatrix();
		}
		else
		{
			base.camera.enabled = false;
		}
		if (!this.viewportClippingEnabled)
		{
			tk2dCamera.inst = this;
		}
		if (tk2dCamera.allCameras.IndexOf(this) == -1)
		{
			tk2dCamera.allCameras.Add(this);
		}
	}

	// Token: 0x06001023 RID: 4131 RVA: 0x00073694 File Offset: 0x00071894
	private void OnDestroy()
	{
		int num = tk2dCamera.allCameras.IndexOf(this);
		if (num != -1)
		{
			tk2dCamera.allCameras.RemoveAt(num);
		}
	}

	// Token: 0x06001024 RID: 4132 RVA: 0x0000DC26 File Offset: 0x0000BE26
	private void OnPreCull()
	{
		tk2dUpdateManager.FlushQueues();
		this.UpdateCameraMatrix();
	}

	// Token: 0x06001025 RID: 4133 RVA: 0x000736C0 File Offset: 0x000718C0
	public float GetSizeAtDistance(float distance)
	{
		tk2dCameraSettings tk2dCameraSettings = this.SettingsRoot.CameraSettings;
		tk2dCameraSettings.ProjectionType projection = tk2dCameraSettings.projection;
		if (projection != tk2dCameraSettings.ProjectionType.Orthographic)
		{
			if (projection != tk2dCameraSettings.ProjectionType.Perspective)
			{
				return 1f;
			}
			return Mathf.Tan(this.CameraSettings.fieldOfView * 0.017453292f * 0.5f) * distance * 2f / (float)this.SettingsRoot.nativeResolutionHeight;
		}
		else
		{
			if (tk2dCameraSettings.orthographicType == tk2dCameraSettings.OrthographicType.PixelsPerMeter)
			{
				return 1f / tk2dCameraSettings.orthographicPixelsPerMeter;
			}
			return 2f * tk2dCameraSettings.orthographicSize / (float)this.SettingsRoot.nativeResolutionHeight;
		}
	}

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x06001026 RID: 4134 RVA: 0x0007375C File Offset: 0x0007195C
	public tk2dCamera SettingsRoot
	{
		get
		{
			if (this._settingsRoot == null)
			{
				this._settingsRoot = ((!(this.inheritSettings == null) && !(this.inheritSettings == this)) ? this.inheritSettings.SettingsRoot : this);
			}
			return this._settingsRoot;
		}
	}

	// Token: 0x06001027 RID: 4135 RVA: 0x000737BC File Offset: 0x000719BC
	public Matrix4x4 OrthoOffCenter(Vector2 scale, float left, float right, float bottom, float top, float near, float far)
	{
		float num = 2f / (right - left) * scale.x;
		float num2 = 2f / (top - bottom) * scale.y;
		float num3 = -2f / (far - near);
		float num4 = -(right + left) / (right - left);
		float num5 = -(bottom + top) / (top - bottom);
		float num6 = -(far + near) / (far - near);
		Matrix4x4 matrix4x = default(Matrix4x4);
		matrix4x[0, 0] = num;
		matrix4x[0, 1] = 0f;
		matrix4x[0, 2] = 0f;
		matrix4x[0, 3] = num4;
		matrix4x[1, 0] = 0f;
		matrix4x[1, 1] = num2;
		matrix4x[1, 2] = 0f;
		matrix4x[1, 3] = num5;
		matrix4x[2, 0] = 0f;
		matrix4x[2, 1] = 0f;
		matrix4x[2, 2] = num3;
		matrix4x[2, 3] = num6;
		matrix4x[3, 0] = 0f;
		matrix4x[3, 1] = 0f;
		matrix4x[3, 2] = 0f;
		matrix4x[3, 3] = 1f;
		return matrix4x;
	}

	// Token: 0x06001028 RID: 4136 RVA: 0x000738F4 File Offset: 0x00071AF4
	private Vector2 GetScaleForOverride(tk2dCamera settings, tk2dCameraResolutionOverride currentOverride, float width, float height)
	{
		Vector2 one = Vector2.one;
		if (currentOverride == null)
		{
			return one;
		}
		float num;
		switch (currentOverride.autoScaleMode)
		{
		case tk2dCameraResolutionOverride.AutoScaleMode.FitWidth:
			num = width / (float)settings.nativeResolutionWidth;
			one.Set(num, num);
			return one;
		case tk2dCameraResolutionOverride.AutoScaleMode.FitHeight:
			num = height / (float)settings.nativeResolutionHeight;
			one.Set(num, num);
			return one;
		case tk2dCameraResolutionOverride.AutoScaleMode.FitVisible:
		case tk2dCameraResolutionOverride.AutoScaleMode.ClosestMultipleOfTwo:
		{
			float num2 = (float)settings.nativeResolutionWidth / (float)settings.nativeResolutionHeight;
			float num3 = width / height;
			if (num3 < num2)
			{
				num = width / (float)settings.nativeResolutionWidth;
			}
			else
			{
				num = height / (float)settings.nativeResolutionHeight;
			}
			if (currentOverride.autoScaleMode == tk2dCameraResolutionOverride.AutoScaleMode.ClosestMultipleOfTwo)
			{
				if (num > 1f)
				{
					num = Mathf.Floor(num);
				}
				else
				{
					num = Mathf.Pow(2f, Mathf.Floor(Mathf.Log(num, 2f)));
				}
			}
			one.Set(num, num);
			return one;
		}
		case tk2dCameraResolutionOverride.AutoScaleMode.StretchToFit:
			one.Set(width / (float)settings.nativeResolutionWidth, height / (float)settings.nativeResolutionHeight);
			return one;
		case tk2dCameraResolutionOverride.AutoScaleMode.PixelPerfect:
			num = 1f;
			one.Set(num, num);
			return one;
		}
		num = currentOverride.scale;
		one.Set(num, num);
		return one;
	}

	// Token: 0x06001029 RID: 4137 RVA: 0x00073A44 File Offset: 0x00071C44
	private Vector2 GetOffsetForOverride(tk2dCamera settings, tk2dCameraResolutionOverride currentOverride, Vector2 scale, float width, float height)
	{
		Vector2 vector = Vector2.zero;
		if (currentOverride == null)
		{
			return vector;
		}
		tk2dCameraResolutionOverride.FitMode fitMode = currentOverride.fitMode;
		if (fitMode != tk2dCameraResolutionOverride.FitMode.Constant)
		{
			if (fitMode == tk2dCameraResolutionOverride.FitMode.Center)
			{
				if (settings.cameraSettings.orthographicOrigin == tk2dCameraSettings.OrthographicOrigin.BottomLeft)
				{
					vector = new Vector2(Mathf.Round(((float)settings.nativeResolutionWidth * scale.x - width) / 2f), Mathf.Round(((float)settings.nativeResolutionHeight * scale.y - height) / 2f));
				}
				return vector;
			}
		}
		vector = -currentOverride.offsetPixels;
		return vector;
	}

	// Token: 0x0600102A RID: 4138 RVA: 0x00073AE0 File Offset: 0x00071CE0
	private Matrix4x4 GetProjectionMatrixForOverride(tk2dCamera settings, tk2dCameraResolutionOverride currentOverride, float pixelWidth, float pixelHeight, bool halfTexelOffset, out Rect screenExtents, out Rect unscaledScreenExtents)
	{
		Vector2 scaleForOverride = this.GetScaleForOverride(settings, currentOverride, pixelWidth, pixelHeight);
		Vector2 offsetForOverride = this.GetOffsetForOverride(settings, currentOverride, scaleForOverride, pixelWidth, pixelHeight);
		float num = offsetForOverride.x;
		float num2 = offsetForOverride.y;
		float num3 = pixelWidth + offsetForOverride.x;
		float num4 = pixelHeight + offsetForOverride.y;
		Vector2 zero = Vector2.zero;
		if (this.viewportClippingEnabled && this.InheritConfig != null)
		{
			float num5 = (num3 - num) / scaleForOverride.x;
			float num6 = (num4 - num2) / scaleForOverride.y;
			Vector4 vector = new Vector4((float)((int)this.viewportRegion.x), (float)((int)this.viewportRegion.y), (float)((int)this.viewportRegion.z), (float)((int)this.viewportRegion.w));
			float num7 = -offsetForOverride.x / pixelWidth + vector.x / num5;
			float num8 = -offsetForOverride.y / pixelHeight + vector.y / num6;
			float num9 = vector.z / num5;
			float num10 = vector.w / num6;
			Rect rect = new Rect(num7, num8, num9, num10);
			if (this.UnityCamera.rect.x != num7 || this.UnityCamera.rect.y != num8 || this.UnityCamera.rect.width != num9 || this.UnityCamera.rect.height != num10)
			{
				this.UnityCamera.rect = rect;
			}
			float num11 = Mathf.Min(1f - rect.x, rect.width);
			float num12 = Mathf.Min(1f - rect.y, rect.height);
			float num13 = vector.x * scaleForOverride.x - offsetForOverride.x;
			float num14 = vector.y * scaleForOverride.y - offsetForOverride.y;
			if (rect.x < 0f)
			{
				num13 += -rect.x * pixelWidth;
				num11 = rect.x + rect.width;
			}
			if (rect.y < 0f)
			{
				num14 += -rect.y * pixelHeight;
				num12 = rect.y + rect.height;
			}
			num += num13;
			num2 += num14;
			num3 = pixelWidth * num11 + offsetForOverride.x + num13;
			num4 = pixelHeight * num12 + offsetForOverride.y + num14;
		}
		else if (this.UnityCamera.rect != this.CameraSettings.rect)
		{
			this.UnityCamera.rect = this.CameraSettings.rect;
		}
		if (settings.cameraSettings.orthographicOrigin == tk2dCameraSettings.OrthographicOrigin.Center)
		{
			float num15 = (num3 - num) * 0.5f;
			num -= num15;
			num3 -= num15;
			float num16 = (num4 - num2) * 0.5f;
			num4 -= num16;
			num2 -= num16;
			zero.Set((float)(-(float)this.nativeResolutionWidth) / 2f, (float)(-(float)this.nativeResolutionHeight) / 2f);
		}
		float num17 = settings.cameraSettings.orthographicSize;
		tk2dCameraSettings.OrthographicType orthographicType = settings.cameraSettings.orthographicType;
		if (orthographicType != tk2dCameraSettings.OrthographicType.PixelsPerMeter)
		{
			if (orthographicType == tk2dCameraSettings.OrthographicType.OrthographicSize)
			{
				num17 = 2f * settings.cameraSettings.orthographicSize / (float)settings.nativeResolutionHeight;
			}
		}
		else
		{
			num17 = 1f / settings.cameraSettings.orthographicPixelsPerMeter;
		}
		float num18 = 1f / this.ZoomFactor;
		bool flag = Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.WindowsEditor;
		float num19 = ((!halfTexelOffset || !flag) ? 0f : 0.5f);
		float num20 = num17 * num18;
		screenExtents = new Rect(num * num20 / scaleForOverride.x, num2 * num20 / scaleForOverride.y, (num3 - num) * num20 / scaleForOverride.x, (num4 - num2) * num20 / scaleForOverride.y);
		unscaledScreenExtents = new Rect(zero.x * num20, zero.y * num20, (float)this.nativeResolutionWidth * num20, (float)this.nativeResolutionHeight * num20);
		return this.OrthoOffCenter(scaleForOverride, num17 * (num + num19) * num18, num17 * (num3 + num19) * num18, num17 * (num2 - num19) * num18, num17 * (num4 - num19) * num18, this.UnityCamera.nearClipPlane, this.UnityCamera.farClipPlane);
	}

	// Token: 0x0600102B RID: 4139 RVA: 0x00073F80 File Offset: 0x00072180
	private Vector2 GetScreenPixelDimensions(tk2dCamera settings)
	{
		Vector2 vector = new Vector2(this.ScreenCamera.pixelWidth, this.ScreenCamera.pixelHeight);
		return vector;
	}

	// Token: 0x0600102C RID: 4140 RVA: 0x00073FAC File Offset: 0x000721AC
	private void Upgrade()
	{
		if (this.version != tk2dCamera.CURRENT_VERSION)
		{
			if (this.version == 0)
			{
				this.cameraSettings.orthographicPixelsPerMeter = 1f;
				this.cameraSettings.orthographicType = tk2dCameraSettings.OrthographicType.PixelsPerMeter;
				this.cameraSettings.orthographicOrigin = tk2dCameraSettings.OrthographicOrigin.BottomLeft;
				this.cameraSettings.projection = tk2dCameraSettings.ProjectionType.Orthographic;
				foreach (tk2dCameraResolutionOverride tk2dCameraResolutionOverride in this.resolutionOverride)
				{
					tk2dCameraResolutionOverride.Upgrade(this.version);
				}
				Camera camera = base.camera;
				if (camera != null)
				{
					this.cameraSettings.rect = camera.rect;
					if (!camera.isOrthoGraphic)
					{
						this.cameraSettings.projection = tk2dCameraSettings.ProjectionType.Perspective;
						this.cameraSettings.fieldOfView = camera.fieldOfView * this.ZoomFactor;
					}
					camera.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
				}
			}
			Debug.Log("tk2dCamera '" + base.name + "' - Upgraded from version " + this.version.ToString());
			this.version = tk2dCamera.CURRENT_VERSION;
		}
	}

	// Token: 0x0600102D RID: 4141 RVA: 0x000740C0 File Offset: 0x000722C0
	public void UpdateCameraMatrix()
	{
		this.Upgrade();
		if (!this.viewportClippingEnabled)
		{
			tk2dCamera.inst = this;
		}
		Camera unityCamera = this.UnityCamera;
		tk2dCamera settingsRoot = this.SettingsRoot;
		tk2dCameraSettings tk2dCameraSettings = settingsRoot.CameraSettings;
		if (unityCamera.rect != this.cameraSettings.rect)
		{
			unityCamera.rect = this.cameraSettings.rect;
		}
		this._targetResolution = this.GetScreenPixelDimensions(settingsRoot);
		if (tk2dCameraSettings.projection == tk2dCameraSettings.ProjectionType.Perspective)
		{
			if (unityCamera.orthographic)
			{
				unityCamera.orthographic = false;
			}
			float num = Mathf.Min(179.9f, tk2dCameraSettings.fieldOfView / Mathf.Max(0.001f, this.ZoomFactor));
			if (unityCamera.fieldOfView != num)
			{
				unityCamera.fieldOfView = num;
			}
			this._screenExtents.Set(-unityCamera.aspect, -1f, unityCamera.aspect * 2f, 2f);
			this._nativeScreenExtents = this._screenExtents;
			unityCamera.ResetProjectionMatrix();
		}
		else
		{
			if (!unityCamera.orthographic)
			{
				unityCamera.orthographic = true;
			}
			Matrix4x4 matrix4x = this.GetProjectionMatrixForOverride(settingsRoot, settingsRoot.CurrentResolutionOverride, this._targetResolution.x, this._targetResolution.y, true, out this._screenExtents, out this._nativeScreenExtents);
			if (Application.platform == RuntimePlatform.WP8Player && (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight))
			{
				float num2 = ((Screen.orientation != ScreenOrientation.LandscapeRight) ? (-90f) : 90f);
				Matrix4x4 matrix4x2 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, num2), Vector3.one);
				matrix4x = matrix4x2 * matrix4x;
			}
			if (unityCamera.projectionMatrix != matrix4x)
			{
				unityCamera.projectionMatrix = matrix4x;
			}
		}
	}

	// Token: 0x040011AA RID: 4522
	private static int CURRENT_VERSION = 1;

	// Token: 0x040011AB RID: 4523
	public int version;

	// Token: 0x040011AC RID: 4524
	[SerializeField]
	private tk2dCameraSettings cameraSettings = new tk2dCameraSettings();

	// Token: 0x040011AD RID: 4525
	public tk2dCameraResolutionOverride[] resolutionOverride = new tk2dCameraResolutionOverride[] { tk2dCameraResolutionOverride.DefaultOverride };

	// Token: 0x040011AE RID: 4526
	[SerializeField]
	private tk2dCamera inheritSettings;

	// Token: 0x040011AF RID: 4527
	public int nativeResolutionWidth = 960;

	// Token: 0x040011B0 RID: 4528
	public int nativeResolutionHeight = 640;

	// Token: 0x040011B1 RID: 4529
	[SerializeField]
	private Camera _unityCamera;

	// Token: 0x040011B2 RID: 4530
	private static tk2dCamera inst;

	// Token: 0x040011B3 RID: 4531
	private static List<tk2dCamera> allCameras = new List<tk2dCamera>();

	// Token: 0x040011B4 RID: 4532
	public bool viewportClippingEnabled;

	// Token: 0x040011B5 RID: 4533
	public Vector4 viewportRegion = new Vector4(0f, 0f, 100f, 100f);

	// Token: 0x040011B6 RID: 4534
	private Vector2 _targetResolution = Vector2.zero;

	// Token: 0x040011B7 RID: 4535
	[SerializeField]
	private float zoomFactor = 1f;

	// Token: 0x040011B8 RID: 4536
	[HideInInspector]
	public bool forceResolutionInEditor;

	// Token: 0x040011B9 RID: 4537
	private bool useGameWindowResolutionInEditor;

	// Token: 0x040011BA RID: 4538
	[HideInInspector]
	public Vector2 forceResolution = new Vector2(960f, 640f);

	// Token: 0x040011BB RID: 4539
	private Vector2 gameWindowResolution = new Vector2(960f, 640f);

	// Token: 0x040011BC RID: 4540
	private Rect _screenExtents;

	// Token: 0x040011BD RID: 4541
	private Rect _nativeScreenExtents;

	// Token: 0x040011BE RID: 4542
	private Rect unitRect = new Rect(0f, 0f, 1f, 1f);

	// Token: 0x040011BF RID: 4543
	private tk2dCamera _settingsRoot;
}
