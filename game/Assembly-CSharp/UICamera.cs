using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A1 RID: 161
[RequireComponent(typeof(Camera))]
[AddComponentMenu("NGUI/UI/NGUI Event System (UICamera)")]
[ExecuteInEditMode]
public class UICamera : MonoBehaviour
{
	// Token: 0x170000DF RID: 223
	// (get) Token: 0x060004FC RID: 1276 RVA: 0x000070C1 File Offset: 0x000052C1
	[Obsolete("Use new OnDragStart / OnDragOver / OnDragOut / OnDragEnd events instead")]
	public bool stickyPress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x060004FD RID: 1277 RVA: 0x0002826C File Offset: 0x0002646C
	public static Ray currentRay
	{
		get
		{
			return (!(UICamera.currentCamera != null) || UICamera.currentTouch == null) ? default(Ray) : UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x060004FE RID: 1278 RVA: 0x000070C4 File Offset: 0x000052C4
	private bool handlesEvents
	{
		get
		{
			return UICamera.eventHandler == this;
		}
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x060004FF RID: 1279 RVA: 0x000070D1 File Offset: 0x000052D1
	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = base.camera;
			}
			return this.mCam;
		}
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x06000500 RID: 1280 RVA: 0x000070F6 File Offset: 0x000052F6
	// (set) Token: 0x06000501 RID: 1281 RVA: 0x000070FD File Offset: 0x000052FD
	public static GameObject selectedObject
	{
		get
		{
			return UICamera.mCurrentSelection;
		}
		set
		{
			UICamera.SetSelection(value, UICamera.currentScheme);
		}
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x000282BC File Offset: 0x000264BC
	protected static void SetSelection(GameObject go, UICamera.ControlScheme scheme)
	{
		if (UICamera.mNextSelection != null)
		{
			UICamera.mNextSelection = go;
		}
		else if (UICamera.mCurrentSelection != go)
		{
			if (UICamera.mCurrentSelection != null)
			{
				UICamera uicamera = UICamera.FindCameraForLayer(UICamera.mCurrentSelection.layer);
				if (uicamera != null)
				{
					UICamera.current = uicamera;
					UICamera.currentCamera = uicamera.mCam;
					UICamera.currentScheme = scheme;
					UICamera.Notify(UICamera.mCurrentSelection, "OnSelect", false);
					UICamera.current = null;
				}
			}
			UICamera.mCurrentSelection = null;
			UICamera.mNextSelection = go;
			UICamera.mNextScheme = scheme;
			if (UICamera.list.size > 0)
			{
				UICamera uicamera2 = ((!(UICamera.mNextSelection != null)) ? UICamera.list[0] : UICamera.FindCameraForLayer(UICamera.mNextSelection.layer));
				if (uicamera2 != null)
				{
					uicamera2.StartCoroutine(uicamera2.ChangeSelection());
				}
			}
		}
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x000283C0 File Offset: 0x000265C0
	private IEnumerator ChangeSelection()
	{
		yield return new WaitForEndOfFrame();
		UICamera.mCurrentSelection = UICamera.mNextSelection;
		UICamera.mNextSelection = null;
		if (UICamera.mCurrentSelection != null)
		{
			UICamera.current = this;
			UICamera.currentCamera = this.mCam;
			UICamera.currentScheme = UICamera.mNextScheme;
			UICamera.Notify(UICamera.mCurrentSelection, "OnSelect", true);
			UICamera.current = null;
		}
		yield break;
	}

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x06000504 RID: 1284 RVA: 0x000283DC File Offset: 0x000265DC
	public static int touchCount
	{
		get
		{
			int num = 0;
			foreach (KeyValuePair<int, UICamera.MouseOrTouch> keyValuePair in UICamera.mTouches)
			{
				if (keyValuePair.Value.pressed != null)
				{
					num++;
				}
			}
			for (int i = 0; i < UICamera.mMouse.Length; i++)
			{
				if (UICamera.mMouse[i].pressed != null)
				{
					num++;
				}
			}
			if (UICamera.mController.pressed != null)
			{
				num++;
			}
			return num;
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000505 RID: 1285 RVA: 0x00028498 File Offset: 0x00026698
	public static int dragCount
	{
		get
		{
			int num = 0;
			foreach (KeyValuePair<int, UICamera.MouseOrTouch> keyValuePair in UICamera.mTouches)
			{
				if (keyValuePair.Value.dragged != null)
				{
					num++;
				}
			}
			for (int i = 0; i < UICamera.mMouse.Length; i++)
			{
				if (UICamera.mMouse[i].dragged != null)
				{
					num++;
				}
			}
			if (UICamera.mController.dragged != null)
			{
				num++;
			}
			return num;
		}
	}

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06000506 RID: 1286 RVA: 0x00028554 File Offset: 0x00026754
	public static Camera mainCamera
	{
		get
		{
			UICamera eventHandler = UICamera.eventHandler;
			return (!(eventHandler != null)) ? null : eventHandler.cachedCamera;
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000507 RID: 1287 RVA: 0x00028580 File Offset: 0x00026780
	public static UICamera eventHandler
	{
		get
		{
			for (int i = 0; i < UICamera.list.size; i++)
			{
				UICamera uicamera = UICamera.list.buffer[i];
				if (!(uicamera == null) && uicamera.enabled && NGUITools.GetActive(uicamera.gameObject))
				{
					return uicamera;
				}
			}
			return null;
		}
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x0000710A File Offset: 0x0000530A
	private static int CompareFunc(UICamera a, UICamera b)
	{
		if (a.cachedCamera.depth < b.cachedCamera.depth)
		{
			return 1;
		}
		if (a.cachedCamera.depth > b.cachedCamera.depth)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x000285E4 File Offset: 0x000267E4
	public static bool Raycast(Vector3 inPos, out RaycastHit hit)
	{
		for (int i = 0; i < UICamera.list.size; i++)
		{
			UICamera uicamera = UICamera.list.buffer[i];
			if (uicamera.enabled && NGUITools.GetActive(uicamera.gameObject))
			{
				UICamera.currentCamera = uicamera.cachedCamera;
				Vector3 vector = UICamera.currentCamera.ScreenToViewportPoint(inPos);
				if (!float.IsNaN(vector.x) && !float.IsNaN(vector.y))
				{
					if (vector.x >= 0f && vector.x <= 1f && vector.y >= 0f && vector.y <= 1f)
					{
						Ray ray = UICamera.currentCamera.ScreenPointToRay(inPos);
						int num = UICamera.currentCamera.cullingMask & uicamera.eventReceiverMask;
						float num2 = ((uicamera.rangeDistance <= 0f) ? (UICamera.currentCamera.farClipPlane - UICamera.currentCamera.nearClipPlane) : uicamera.rangeDistance);
						if (uicamera.eventType == UICamera.EventType.World)
						{
							if (Physics.Raycast(ray, out hit, num2, num))
							{
								UICamera.hoveredObject = hit.collider.gameObject;
								return true;
							}
						}
						else if (uicamera.eventType == UICamera.EventType.UI)
						{
							RaycastHit[] array = Physics.RaycastAll(ray, num2, num);
							if (array.Length > 1)
							{
								for (int j = 0; j < array.Length; j++)
								{
									GameObject gameObject = array[j].collider.gameObject;
									UICamera.mHit.depth = NGUITools.CalculateRaycastDepth(gameObject);
									if (UICamera.mHit.depth != 2147483647)
									{
										UICamera.mHit.hit = array[j];
										UICamera.mHits.Add(UICamera.mHit);
									}
								}
								UICamera.mHits.Sort((UICamera.DepthEntry r1, UICamera.DepthEntry r2) => r2.depth.CompareTo(r1.depth));
								for (int k = 0; k < UICamera.mHits.size; k++)
								{
									if (UICamera.IsVisible(ref UICamera.mHits.buffer[k]))
									{
										hit = UICamera.mHits[k].hit;
										UICamera.hoveredObject = hit.collider.gameObject;
										UICamera.mHits.Clear();
										return true;
									}
								}
								UICamera.mHits.Clear();
							}
							else if (array.Length == 1 && UICamera.IsVisible(ref array[0]))
							{
								hit = array[0];
								UICamera.hoveredObject = hit.collider.gameObject;
								return true;
							}
						}
					}
				}
			}
		}
		hit = UICamera.mEmpty;
		return false;
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x000288E0 File Offset: 0x00026AE0
	private static bool IsVisible(ref RaycastHit hit)
	{
		UIPanel uipanel = NGUITools.FindInParents<UIPanel>(hit.collider.gameObject);
		return uipanel == null || uipanel.IsVisible(hit.point);
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x00028920 File Offset: 0x00026B20
	private static bool IsVisible(ref UICamera.DepthEntry de)
	{
		UIPanel uipanel = NGUITools.FindInParents<UIPanel>(de.hit.collider.gameObject);
		return uipanel == null || uipanel.IsVisible(de.hit.point);
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x00007147 File Offset: 0x00005347
	public static bool IsHighlighted(GameObject go)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Mouse)
		{
			return UICamera.hoveredObject == go;
		}
		return UICamera.currentScheme == UICamera.ControlScheme.Controller && UICamera.selectedObject == go;
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x00028964 File Offset: 0x00026B64
	public static UICamera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		for (int i = 0; i < UICamera.list.size; i++)
		{
			UICamera uicamera = UICamera.list.buffer[i];
			Camera cachedCamera = uicamera.cachedCamera;
			if (cachedCamera != null && (cachedCamera.cullingMask & num) != 0)
			{
				return uicamera;
			}
		}
		return null;
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x00007177 File Offset: 0x00005377
	private static int GetDirection(KeyCode up, KeyCode down)
	{
		if (Input.GetKeyDown(up))
		{
			return 1;
		}
		if (Input.GetKeyDown(down))
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x00007194 File Offset: 0x00005394
	private static int GetDirection(KeyCode up0, KeyCode up1, KeyCode down0, KeyCode down1)
	{
		if (Input.GetKeyDown(up0) || Input.GetKeyDown(up1))
		{
			return 1;
		}
		if (Input.GetKeyDown(down0) || Input.GetKeyDown(down1))
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x000289C4 File Offset: 0x00026BC4
	private static int GetDirection(string axis)
	{
		float time = RealTime.time;
		if (UICamera.mNextEvent < time)
		{
			float axis2 = Input.GetAxis(axis);
			if (axis2 > 0.75f)
			{
				UICamera.mNextEvent = time + 0.25f;
				return 1;
			}
			if (axis2 < -0.75f)
			{
				UICamera.mNextEvent = time + 0.25f;
				return -1;
			}
		}
		return 0;
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x00028A1C File Offset: 0x00026C1C
	public static void Notify(GameObject go, string funcName, object obj)
	{
		if (go != null)
		{
			go.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
			if (UICamera.genericEventHandler != null && UICamera.genericEventHandler != go)
			{
				UICamera.genericEventHandler.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x000071C7 File Offset: 0x000053C7
	public static UICamera.MouseOrTouch GetMouse(int button)
	{
		return UICamera.mMouse[button];
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x00028A6C File Offset: 0x00026C6C
	public static UICamera.MouseOrTouch GetTouch(int id)
	{
		UICamera.MouseOrTouch mouseOrTouch = null;
		if (id < 0)
		{
			return UICamera.GetMouse(-id - 1);
		}
		if (!UICamera.mTouches.TryGetValue(id, out mouseOrTouch))
		{
			mouseOrTouch = new UICamera.MouseOrTouch();
			mouseOrTouch.touchBegan = true;
			UICamera.mTouches.Add(id, mouseOrTouch);
		}
		return mouseOrTouch;
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x000071D0 File Offset: 0x000053D0
	public static void RemoveTouch(int id)
	{
		UICamera.mTouches.Remove(id);
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x00028AB8 File Offset: 0x00026CB8
	private void Awake()
	{
		UICamera.mWidth = Screen.width;
		UICamera.mHeight = Screen.height;
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WP8Player || Application.platform == RuntimePlatform.BB10Player)
		{
			this.useMouse = false;
			this.useTouch = true;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				this.useKeyboard = false;
				this.useController = false;
			}
		}
		else if (Application.platform == RuntimePlatform.PS3 || Application.platform == RuntimePlatform.XBOX360)
		{
			this.useMouse = false;
			this.useTouch = false;
			this.useKeyboard = false;
			this.useController = true;
		}
		UICamera.mMouse[0].pos.x = Input.mousePosition.x;
		UICamera.mMouse[0].pos.y = Input.mousePosition.y;
		for (int i = 1; i < 3; i++)
		{
			UICamera.mMouse[i].pos = UICamera.mMouse[0].pos;
			UICamera.mMouse[i].lastPos = UICamera.mMouse[0].pos;
		}
		UICamera.lastTouchPosition = UICamera.mMouse[0].pos;
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x000071DE File Offset: 0x000053DE
	private void OnEnable()
	{
		UICamera.list.Add(this);
		UICamera.list.Sort(new BetterList<UICamera>.CompareFunc(UICamera.CompareFunc));
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x00007201 File Offset: 0x00005401
	private void OnDisable()
	{
		UICamera.list.Remove(this);
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x0000720F File Offset: 0x0000540F
	private void Start()
	{
		this.cachedCamera.eventMask = 0;
		this.cachedCamera.transparencySortMode = TransparencySortMode.Orthographic;
		if (this.debug)
		{
			NGUIDebug.debugRaycast = true;
		}
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x00028BF8 File Offset: 0x00026DF8
	private void Update()
	{
		if (!Application.isPlaying || !this.handlesEvents)
		{
			return;
		}
		UICamera.current = this;
		int width = Screen.width;
		int height = Screen.height;
		if (width != UICamera.mWidth || height != UICamera.mHeight)
		{
			UICamera.mWidth = width;
			UICamera.mHeight = height;
			if (UICamera.onScreenResize != null)
			{
				UICamera.onScreenResize();
			}
		}
		if (UICamera.onCustomInput != null)
		{
			UICamera.onCustomInput();
		}
		if (this.useMouse && UICamera.mCurrentSelection != null)
		{
			if (this.cancelKey0 != KeyCode.None && Input.GetKeyDown(this.cancelKey0))
			{
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.currentKey = this.cancelKey0;
				UICamera.selectedObject = null;
			}
			else if (this.cancelKey1 != KeyCode.None && Input.GetKeyDown(this.cancelKey1))
			{
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.currentKey = this.cancelKey1;
				UICamera.selectedObject = null;
			}
		}
		if (UICamera.mCurrentSelection != null)
		{
			string text = Input.inputString;
			if (this.useKeyboard && Input.GetKeyDown(KeyCode.Delete))
			{
				text += "\b";
			}
			if (text.Length > 0)
			{
				if (!this.stickyTooltip && this.mTooltip != null)
				{
					this.ShowTooltip(false);
				}
				UICamera.Notify(UICamera.mCurrentSelection, "OnInput", text);
			}
		}
		else
		{
			UICamera.inputHasFocus = false;
		}
		if (UICamera.mCurrentSelection != null)
		{
			this.ProcessOthers();
		}
		if (this.useMouse && UICamera.mHover != null)
		{
			float axis = Input.GetAxis(this.scrollAxisName);
			if (axis != 0f)
			{
				UICamera.Notify(UICamera.mHover, "OnScroll", axis);
			}
			if (UICamera.showTooltips && this.mTooltipTime != 0f && (this.mTooltipTime < RealTime.time || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
			{
				this.mTooltip = UICamera.mHover;
				this.ShowTooltip(true);
			}
		}
		UICamera.current = null;
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00028E3C File Offset: 0x0002703C
	public void ProcessMouse()
	{
		if (this.mNextRaycast < RealTime.time)
		{
			this.mNextRaycast = RealTime.time + 0.02f;
			if (!UICamera.Raycast(Input.mousePosition, out UICamera.lastHit))
			{
				UICamera.hoveredObject = UICamera.fallThrough;
			}
			if (UICamera.hoveredObject == null)
			{
				UICamera.hoveredObject = UICamera.genericEventHandler;
			}
			for (int i = 0; i < 3; i++)
			{
				UICamera.mMouse[i].current = UICamera.hoveredObject;
			}
		}
		UICamera.lastTouchPosition = Input.mousePosition;
		bool flag = UICamera.mMouse[0].last != UICamera.mMouse[0].current;
		if (flag)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Mouse;
		}
		UICamera.mMouse[0].delta = UICamera.lastTouchPosition - UICamera.mMouse[0].pos;
		UICamera.mMouse[0].pos = UICamera.lastTouchPosition;
		bool flag2 = UICamera.mMouse[0].delta.sqrMagnitude > 0.001f;
		for (int j = 1; j < 3; j++)
		{
			UICamera.mMouse[j].pos = UICamera.mMouse[0].pos;
			UICamera.mMouse[j].delta = UICamera.mMouse[0].delta;
		}
		bool flag3 = false;
		for (int k = 0; k < 3; k++)
		{
			if (Input.GetMouseButton(k))
			{
				UICamera.currentScheme = UICamera.ControlScheme.Mouse;
				flag3 = true;
				break;
			}
		}
		if (flag3)
		{
			this.mTooltipTime = 0f;
		}
		else if (flag2 && (!this.stickyTooltip || flag))
		{
			if (this.mTooltipTime != 0f)
			{
				this.mTooltipTime = RealTime.time + this.tooltipDelay;
			}
			else if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
		}
		if (!flag3 && UICamera.mHover != null && flag)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Mouse;
			if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
			UICamera.Notify(UICamera.mHover, "OnHover", false);
			UICamera.mHover = null;
		}
		for (int l = 0; l < 3; l++)
		{
			bool mouseButtonDown = Input.GetMouseButtonDown(l);
			bool mouseButtonUp = Input.GetMouseButtonUp(l);
			if (mouseButtonDown || mouseButtonUp)
			{
				UICamera.currentScheme = UICamera.ControlScheme.Mouse;
			}
			UICamera.currentTouch = UICamera.mMouse[l];
			UICamera.currentTouchID = -1 - l;
			UICamera.currentKey = KeyCode.Mouse0 + l;
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			this.ProcessTouch(mouseButtonDown, mouseButtonUp);
			UICamera.currentKey = KeyCode.None;
		}
		UICamera.currentTouch = null;
		if (!flag3 && flag)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Mouse;
			this.mTooltipTime = RealTime.time + this.tooltipDelay;
			UICamera.mHover = UICamera.mMouse[0].current;
			UICamera.Notify(UICamera.mHover, "OnHover", true);
		}
		UICamera.mMouse[0].last = UICamera.mMouse[0].current;
		for (int m = 1; m < 3; m++)
		{
			UICamera.mMouse[m].last = UICamera.mMouse[0].last;
		}
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x000291C8 File Offset: 0x000273C8
	public void ProcessTouches()
	{
		UICamera.currentScheme = UICamera.ControlScheme.Touch;
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			UICamera.currentTouchID = ((!this.allowMultiTouch) ? 1 : touch.fingerId);
			UICamera.currentTouch = UICamera.GetTouch(UICamera.currentTouchID);
			bool flag = touch.phase == TouchPhase.Began || UICamera.currentTouch.touchBegan;
			bool flag2 = touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended;
			UICamera.currentTouch.touchBegan = false;
			UICamera.currentTouch.delta = ((!flag) ? (touch.position - UICamera.currentTouch.pos) : Vector2.zero);
			UICamera.currentTouch.pos = touch.position;
			if (!UICamera.Raycast(UICamera.currentTouch.pos, out UICamera.lastHit))
			{
				UICamera.hoveredObject = UICamera.fallThrough;
			}
			if (UICamera.hoveredObject == null)
			{
				UICamera.hoveredObject = UICamera.genericEventHandler;
			}
			UICamera.currentTouch.last = UICamera.currentTouch.current;
			UICamera.currentTouch.current = UICamera.hoveredObject;
			UICamera.lastTouchPosition = UICamera.currentTouch.pos;
			if (flag)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			if (touch.tapCount > 1)
			{
				UICamera.currentTouch.clickTime = RealTime.time;
			}
			this.ProcessTouch(flag, flag2);
			if (flag2)
			{
				UICamera.RemoveTouch(UICamera.currentTouchID);
			}
			UICamera.currentTouch.last = null;
			UICamera.currentTouch = null;
			if (!this.allowMultiTouch)
			{
				break;
			}
		}
		if (Input.touchCount == 0 && this.useMouse)
		{
			this.ProcessMouse();
		}
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x000293C8 File Offset: 0x000275C8
	private void ProcessFakeTouches()
	{
		bool mouseButtonDown = Input.GetMouseButtonDown(0);
		bool mouseButtonUp = Input.GetMouseButtonUp(0);
		bool mouseButton = Input.GetMouseButton(0);
		if (mouseButtonDown || mouseButtonUp || mouseButton)
		{
			UICamera.currentTouchID = 1;
			UICamera.currentTouch = UICamera.mMouse[0];
			UICamera.currentTouch.touchBegan = mouseButtonDown;
			Vector2 vector = Input.mousePosition;
			UICamera.currentTouch.delta = ((!mouseButtonDown) ? (vector - UICamera.currentTouch.pos) : Vector2.zero);
			UICamera.currentTouch.pos = vector;
			if (!UICamera.Raycast(UICamera.currentTouch.pos, out UICamera.lastHit))
			{
				UICamera.hoveredObject = UICamera.fallThrough;
			}
			if (UICamera.hoveredObject == null)
			{
				UICamera.hoveredObject = UICamera.genericEventHandler;
			}
			UICamera.currentTouch.last = UICamera.currentTouch.current;
			UICamera.currentTouch.current = UICamera.hoveredObject;
			UICamera.lastTouchPosition = UICamera.currentTouch.pos;
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			this.ProcessTouch(mouseButtonDown, mouseButtonUp);
			if (mouseButtonUp)
			{
				UICamera.RemoveTouch(UICamera.currentTouchID);
			}
			UICamera.currentTouch.last = null;
			UICamera.currentTouch = null;
		}
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x00029534 File Offset: 0x00027734
	public void ProcessOthers()
	{
		UICamera.currentTouchID = -100;
		UICamera.currentTouch = UICamera.mController;
		UICamera.inputHasFocus = UICamera.mCurrentSelection != null && UICamera.mCurrentSelection.GetComponent<UIInput>() != null;
		bool flag = false;
		bool flag2 = false;
		if (this.submitKey0 != KeyCode.None && Input.GetKeyDown(this.submitKey0))
		{
			UICamera.currentKey = this.submitKey0;
			flag = true;
		}
		if (this.submitKey1 != KeyCode.None && Input.GetKeyDown(this.submitKey1))
		{
			UICamera.currentKey = this.submitKey1;
			flag = true;
		}
		if (this.submitKey0 != KeyCode.None && Input.GetKeyUp(this.submitKey0))
		{
			UICamera.currentKey = this.submitKey0;
			flag2 = true;
		}
		if (this.submitKey1 != KeyCode.None && Input.GetKeyUp(this.submitKey1))
		{
			UICamera.currentKey = this.submitKey1;
			flag2 = true;
		}
		if (flag || flag2)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.currentTouch.last = UICamera.currentTouch.current;
			UICamera.currentTouch.current = UICamera.mCurrentSelection;
			this.ProcessTouch(flag, flag2);
			UICamera.currentTouch.last = null;
		}
		int num = 0;
		int num2 = 0;
		if (this.useKeyboard)
		{
			if (UICamera.inputHasFocus)
			{
				num += UICamera.GetDirection(KeyCode.UpArrow, KeyCode.DownArrow);
				num2 += UICamera.GetDirection(KeyCode.RightArrow, KeyCode.LeftArrow);
			}
			else
			{
				num += UICamera.GetDirection(KeyCode.W, KeyCode.UpArrow, KeyCode.S, KeyCode.DownArrow);
				num2 += UICamera.GetDirection(KeyCode.D, KeyCode.RightArrow, KeyCode.A, KeyCode.LeftArrow);
			}
		}
		if (this.useController)
		{
			if (!string.IsNullOrEmpty(this.verticalAxisName))
			{
				num += UICamera.GetDirection(this.verticalAxisName);
			}
			if (!string.IsNullOrEmpty(this.horizontalAxisName))
			{
				num2 += UICamera.GetDirection(this.horizontalAxisName);
			}
		}
		if (num != 0)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", (num <= 0) ? KeyCode.DownArrow : KeyCode.UpArrow);
		}
		if (num2 != 0)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", (num2 <= 0) ? KeyCode.LeftArrow : KeyCode.RightArrow);
		}
		if (this.useKeyboard && Input.GetKeyDown(KeyCode.Tab))
		{
			UICamera.currentKey = KeyCode.Tab;
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", KeyCode.Tab);
		}
		if (this.cancelKey0 != KeyCode.None && Input.GetKeyDown(this.cancelKey0))
		{
			UICamera.currentKey = this.cancelKey0;
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", KeyCode.Escape);
		}
		if (this.cancelKey1 != KeyCode.None && Input.GetKeyDown(this.cancelKey1))
		{
			UICamera.currentKey = this.cancelKey1;
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", KeyCode.Escape);
		}
		UICamera.currentTouch = null;
		UICamera.currentKey = KeyCode.None;
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x00029854 File Offset: 0x00027A54
	public void ProcessTouch(bool pressed, bool unpressed)
	{
		bool flag = UICamera.currentScheme == UICamera.ControlScheme.Mouse;
		float num = ((!flag) ? this.touchDragThreshold : this.mouseDragThreshold);
		float num2 = ((!flag) ? this.touchClickThreshold : this.mouseClickThreshold);
		if (pressed)
		{
			if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
			UICamera.currentTouch.pressStarted = true;
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
			UICamera.currentTouch.pressed = UICamera.currentTouch.current;
			UICamera.currentTouch.dragged = UICamera.currentTouch.current;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			UICamera.currentTouch.totalDelta = Vector2.zero;
			UICamera.currentTouch.dragStarted = false;
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", true);
			if (UICamera.currentTouch.pressed != UICamera.mCurrentSelection)
			{
				if (this.mTooltip != null)
				{
					this.ShowTooltip(false);
				}
				UICamera.currentScheme = UICamera.ControlScheme.Touch;
				UICamera.selectedObject = null;
			}
		}
		else if (UICamera.currentTouch.pressed != null && (UICamera.currentTouch.delta.magnitude != 0f || UICamera.currentTouch.current != UICamera.currentTouch.last))
		{
			UICamera.currentTouch.totalDelta += UICamera.currentTouch.delta;
			float magnitude = UICamera.currentTouch.totalDelta.magnitude;
			bool flag2 = false;
			if (!UICamera.currentTouch.dragStarted && UICamera.currentTouch.last != UICamera.currentTouch.current)
			{
				UICamera.currentTouch.dragStarted = true;
				UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDragStart", null);
				UICamera.Notify(UICamera.currentTouch.last, "OnDragOver", UICamera.currentTouch.dragged);
			}
			else if (!UICamera.currentTouch.dragStarted && num < magnitude)
			{
				flag2 = true;
				UICamera.currentTouch.dragStarted = true;
				UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
			}
			if (UICamera.currentTouch.dragStarted)
			{
				if (this.mTooltip != null)
				{
					this.ShowTooltip(false);
				}
				UICamera.isDragging = true;
				bool flag3 = UICamera.currentTouch.clickNotification == UICamera.ClickNotification.None;
				if (flag2)
				{
					UICamera.Notify(UICamera.currentTouch.dragged, "OnDragStart", null);
					UICamera.Notify(UICamera.currentTouch.current, "OnDragOver", UICamera.currentTouch.dragged);
				}
				else if (UICamera.currentTouch.last != UICamera.currentTouch.current)
				{
					UICamera.Notify(UICamera.currentTouch.last, "OnDragOut", UICamera.currentTouch.dragged);
					UICamera.Notify(UICamera.currentTouch.current, "OnDragOver", UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDrag", UICamera.currentTouch.delta);
				UICamera.currentTouch.last = UICamera.currentTouch.current;
				UICamera.isDragging = false;
				if (flag3)
				{
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				}
				else if (UICamera.currentTouch.clickNotification == UICamera.ClickNotification.BasedOnDelta && num2 < magnitude)
				{
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				}
			}
		}
		if (unpressed)
		{
			UICamera.currentTouch.pressStarted = false;
			if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
			if (UICamera.currentTouch.pressed != null)
			{
				if (UICamera.currentTouch.dragStarted)
				{
					UICamera.Notify(UICamera.currentTouch.last, "OnDragOut", UICamera.currentTouch.dragged);
					UICamera.Notify(UICamera.currentTouch.dragged, "OnDragEnd", null);
				}
				UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
				if (flag)
				{
					UICamera.Notify(UICamera.currentTouch.current, "OnHover", true);
				}
				UICamera.mHover = UICamera.currentTouch.current;
				if (UICamera.currentTouch.dragged == UICamera.currentTouch.current || (UICamera.currentScheme != UICamera.ControlScheme.Controller && UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && UICamera.currentTouch.totalDelta.magnitude < num))
				{
					if (UICamera.currentTouch.pressed != UICamera.mCurrentSelection)
					{
						UICamera.mNextSelection = null;
						UICamera.mCurrentSelection = UICamera.currentTouch.pressed;
						UICamera.Notify(UICamera.currentTouch.pressed, "OnSelect", true);
					}
					else
					{
						UICamera.mNextSelection = null;
						UICamera.mCurrentSelection = UICamera.currentTouch.pressed;
					}
					if (UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None)
					{
						float time = RealTime.time;
						UICamera.Notify(UICamera.currentTouch.pressed, "OnClick", null);
						if (UICamera.currentTouch.clickTime + 0.35f > time)
						{
							UICamera.Notify(UICamera.currentTouch.pressed, "OnDoubleClick", null);
						}
						UICamera.currentTouch.clickTime = time;
					}
				}
				else if (UICamera.currentTouch.dragStarted)
				{
					UICamera.Notify(UICamera.currentTouch.current, "OnDrop", UICamera.currentTouch.dragged);
				}
			}
			UICamera.currentTouch.dragStarted = false;
			UICamera.currentTouch.pressed = null;
			UICamera.currentTouch.dragged = null;
		}
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x0000723A File Offset: 0x0000543A
	public void ShowTooltip(bool val)
	{
		this.mTooltipTime = 0f;
		UICamera.Notify(this.mTooltip, "OnTooltip", val);
		if (!val)
		{
			this.mTooltip = null;
		}
	}

	// Token: 0x040003BF RID: 959
	public static BetterList<UICamera> list = new BetterList<UICamera>();

	// Token: 0x040003C0 RID: 960
	public static UICamera.OnScreenResize onScreenResize;

	// Token: 0x040003C1 RID: 961
	public UICamera.EventType eventType = UICamera.EventType.UI;

	// Token: 0x040003C2 RID: 962
	public LayerMask eventReceiverMask = -1;

	// Token: 0x040003C3 RID: 963
	public bool debug;

	// Token: 0x040003C4 RID: 964
	public bool useMouse = true;

	// Token: 0x040003C5 RID: 965
	public bool useTouch = true;

	// Token: 0x040003C6 RID: 966
	public bool allowMultiTouch = true;

	// Token: 0x040003C7 RID: 967
	public bool useKeyboard = true;

	// Token: 0x040003C8 RID: 968
	public bool useController = true;

	// Token: 0x040003C9 RID: 969
	public bool stickyTooltip = true;

	// Token: 0x040003CA RID: 970
	public float tooltipDelay = 1f;

	// Token: 0x040003CB RID: 971
	public float mouseDragThreshold = 4f;

	// Token: 0x040003CC RID: 972
	public float mouseClickThreshold = 10f;

	// Token: 0x040003CD RID: 973
	public float touchDragThreshold = 40f;

	// Token: 0x040003CE RID: 974
	public float touchClickThreshold = 40f;

	// Token: 0x040003CF RID: 975
	public float rangeDistance = -1f;

	// Token: 0x040003D0 RID: 976
	public string scrollAxisName = "Mouse ScrollWheel";

	// Token: 0x040003D1 RID: 977
	public string verticalAxisName = "Vertical";

	// Token: 0x040003D2 RID: 978
	public string horizontalAxisName = "Horizontal";

	// Token: 0x040003D3 RID: 979
	public KeyCode submitKey0 = KeyCode.Return;

	// Token: 0x040003D4 RID: 980
	public KeyCode submitKey1 = KeyCode.JoystickButton0;

	// Token: 0x040003D5 RID: 981
	public KeyCode cancelKey0 = KeyCode.Escape;

	// Token: 0x040003D6 RID: 982
	public KeyCode cancelKey1 = KeyCode.JoystickButton1;

	// Token: 0x040003D7 RID: 983
	public static UICamera.OnCustomInput onCustomInput;

	// Token: 0x040003D8 RID: 984
	public static bool showTooltips = true;

	// Token: 0x040003D9 RID: 985
	public static Vector2 lastTouchPosition = Vector2.zero;

	// Token: 0x040003DA RID: 986
	public static RaycastHit lastHit;

	// Token: 0x040003DB RID: 987
	public static UICamera current = null;

	// Token: 0x040003DC RID: 988
	public static Camera currentCamera = null;

	// Token: 0x040003DD RID: 989
	public static UICamera.ControlScheme currentScheme = UICamera.ControlScheme.Mouse;

	// Token: 0x040003DE RID: 990
	public static int currentTouchID = -1;

	// Token: 0x040003DF RID: 991
	public static KeyCode currentKey = KeyCode.None;

	// Token: 0x040003E0 RID: 992
	public static UICamera.MouseOrTouch currentTouch = null;

	// Token: 0x040003E1 RID: 993
	public static bool inputHasFocus = false;

	// Token: 0x040003E2 RID: 994
	public static GameObject genericEventHandler;

	// Token: 0x040003E3 RID: 995
	public static GameObject fallThrough;

	// Token: 0x040003E4 RID: 996
	private static GameObject mCurrentSelection = null;

	// Token: 0x040003E5 RID: 997
	private static GameObject mNextSelection = null;

	// Token: 0x040003E6 RID: 998
	private static UICamera.ControlScheme mNextScheme = UICamera.ControlScheme.Controller;

	// Token: 0x040003E7 RID: 999
	private static UICamera.MouseOrTouch[] mMouse = new UICamera.MouseOrTouch[]
	{
		new UICamera.MouseOrTouch(),
		new UICamera.MouseOrTouch(),
		new UICamera.MouseOrTouch()
	};

	// Token: 0x040003E8 RID: 1000
	private static GameObject mHover;

	// Token: 0x040003E9 RID: 1001
	private static UICamera.MouseOrTouch mController = new UICamera.MouseOrTouch();

	// Token: 0x040003EA RID: 1002
	private static float mNextEvent = 0f;

	// Token: 0x040003EB RID: 1003
	private static Dictionary<int, UICamera.MouseOrTouch> mTouches = new Dictionary<int, UICamera.MouseOrTouch>();

	// Token: 0x040003EC RID: 1004
	private static int mWidth = 0;

	// Token: 0x040003ED RID: 1005
	private static int mHeight = 0;

	// Token: 0x040003EE RID: 1006
	private GameObject mTooltip;

	// Token: 0x040003EF RID: 1007
	private Camera mCam;

	// Token: 0x040003F0 RID: 1008
	private float mTooltipTime;

	// Token: 0x040003F1 RID: 1009
	private float mNextRaycast;

	// Token: 0x040003F2 RID: 1010
	public static bool isDragging = false;

	// Token: 0x040003F3 RID: 1011
	public static GameObject hoveredObject;

	// Token: 0x040003F4 RID: 1012
	private static UICamera.DepthEntry mHit = default(UICamera.DepthEntry);

	// Token: 0x040003F5 RID: 1013
	private static BetterList<UICamera.DepthEntry> mHits = new BetterList<UICamera.DepthEntry>();

	// Token: 0x040003F6 RID: 1014
	private static RaycastHit mEmpty = default(RaycastHit);

	// Token: 0x020000A2 RID: 162
	public enum ControlScheme
	{
		// Token: 0x040003F9 RID: 1017
		Mouse,
		// Token: 0x040003FA RID: 1018
		Touch,
		// Token: 0x040003FB RID: 1019
		Controller
	}

	// Token: 0x020000A3 RID: 163
	public enum ClickNotification
	{
		// Token: 0x040003FD RID: 1021
		None,
		// Token: 0x040003FE RID: 1022
		Always,
		// Token: 0x040003FF RID: 1023
		BasedOnDelta
	}

	// Token: 0x020000A4 RID: 164
	public class MouseOrTouch
	{
		// Token: 0x04000400 RID: 1024
		public Vector2 pos;

		// Token: 0x04000401 RID: 1025
		public Vector2 lastPos;

		// Token: 0x04000402 RID: 1026
		public Vector2 delta;

		// Token: 0x04000403 RID: 1027
		public Vector2 totalDelta;

		// Token: 0x04000404 RID: 1028
		public Camera pressedCam;

		// Token: 0x04000405 RID: 1029
		public GameObject last;

		// Token: 0x04000406 RID: 1030
		public GameObject current;

		// Token: 0x04000407 RID: 1031
		public GameObject pressed;

		// Token: 0x04000408 RID: 1032
		public GameObject dragged;

		// Token: 0x04000409 RID: 1033
		public float clickTime;

		// Token: 0x0400040A RID: 1034
		public UICamera.ClickNotification clickNotification = UICamera.ClickNotification.Always;

		// Token: 0x0400040B RID: 1035
		public bool touchBegan = true;

		// Token: 0x0400040C RID: 1036
		public bool pressStarted;

		// Token: 0x0400040D RID: 1037
		public bool dragStarted;
	}

	// Token: 0x020000A5 RID: 165
	public enum EventType
	{
		// Token: 0x0400040F RID: 1039
		World,
		// Token: 0x04000410 RID: 1040
		UI
	}

	// Token: 0x020000A6 RID: 166
	private struct DepthEntry
	{
		// Token: 0x04000411 RID: 1041
		public int depth;

		// Token: 0x04000412 RID: 1042
		public RaycastHit hit;
	}

	// Token: 0x020000A7 RID: 167
	// (Invoke) Token: 0x06000523 RID: 1315
	public delegate void OnScreenResize();

	// Token: 0x020000A8 RID: 168
	// (Invoke) Token: 0x06000527 RID: 1319
	public delegate void OnCustomInput();
}
