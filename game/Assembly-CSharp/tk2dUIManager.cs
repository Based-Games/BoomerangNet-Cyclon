using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002B4 RID: 692
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUIManager")]
public class tk2dUIManager : MonoBehaviour
{
	// Token: 0x1400001E RID: 30
	// (add) Token: 0x06001457 RID: 5207 RVA: 0x000119DE File Offset: 0x0000FBDE
	// (remove) Token: 0x06001458 RID: 5208 RVA: 0x000119F7 File Offset: 0x0000FBF7
	public event Action OnAnyPress;

	// Token: 0x1400001F RID: 31
	// (add) Token: 0x06001459 RID: 5209 RVA: 0x00011A10 File Offset: 0x0000FC10
	// (remove) Token: 0x0600145A RID: 5210 RVA: 0x00011A29 File Offset: 0x0000FC29
	public event Action OnInputUpdate;

	// Token: 0x14000020 RID: 32
	// (add) Token: 0x0600145B RID: 5211 RVA: 0x00011A42 File Offset: 0x0000FC42
	// (remove) Token: 0x0600145C RID: 5212 RVA: 0x00011A5B File Offset: 0x0000FC5B
	public event Action<float> OnScrollWheelChange;

	// Token: 0x1700030E RID: 782
	// (get) Token: 0x0600145D RID: 5213 RVA: 0x0008970C File Offset: 0x0008790C
	public static tk2dUIManager Instance
	{
		get
		{
			if (tk2dUIManager.instance == null)
			{
				tk2dUIManager.instance = UnityEngine.Object.FindObjectOfType(typeof(tk2dUIManager)) as tk2dUIManager;
				if (tk2dUIManager.instance == null)
				{
					GameObject gameObject = new GameObject("tk2dUIManager");
					tk2dUIManager.instance = gameObject.AddComponent<tk2dUIManager>();
				}
			}
			return tk2dUIManager.instance;
		}
	}

	// Token: 0x1700030F RID: 783
	// (get) Token: 0x0600145E RID: 5214 RVA: 0x00011A74 File Offset: 0x0000FC74
	public static tk2dUIManager Instance__NoCreate
	{
		get
		{
			return tk2dUIManager.instance;
		}
	}

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x0600145F RID: 5215 RVA: 0x00011A7B File Offset: 0x0000FC7B
	// (set) Token: 0x06001460 RID: 5216 RVA: 0x00011A83 File Offset: 0x0000FC83
	public Camera UICamera
	{
		get
		{
			return this.uiCamera;
		}
		set
		{
			this.uiCamera = value;
		}
	}

	// Token: 0x06001461 RID: 5217 RVA: 0x00089770 File Offset: 0x00087970
	public Camera GetUICameraForControl(GameObject go)
	{
		int num = 1 << go.layer;
		int count = tk2dUIManager.allCameras.Count;
		for (int i = 0; i < count; i++)
		{
			tk2dUICamera tk2dUICamera = tk2dUIManager.allCameras[i];
			if ((tk2dUICamera.FilteredMask & num) != 0)
			{
				return tk2dUICamera.HostCamera;
			}
		}
		Debug.LogError("Unable to find UI camera for " + go.name);
		return null;
	}

	// Token: 0x06001462 RID: 5218 RVA: 0x00011A8C File Offset: 0x0000FC8C
	public static void RegisterCamera(tk2dUICamera cam)
	{
		tk2dUIManager.allCameras.Add(cam);
	}

	// Token: 0x06001463 RID: 5219 RVA: 0x00011A99 File Offset: 0x0000FC99
	public static void UnregisterCamera(tk2dUICamera cam)
	{
		tk2dUIManager.allCameras.Remove(cam);
	}

	// Token: 0x17000311 RID: 785
	// (get) Token: 0x06001464 RID: 5220 RVA: 0x00011AA7 File Offset: 0x0000FCA7
	// (set) Token: 0x06001465 RID: 5221 RVA: 0x000897E4 File Offset: 0x000879E4
	public bool InputEnabled
	{
		get
		{
			return this.inputEnabled;
		}
		set
		{
			if (this.inputEnabled && !value)
			{
				this.SortCameras();
				this.inputEnabled = value;
				if (this.useMultiTouch)
				{
					this.CheckMultiTouchInputs();
				}
				else
				{
					this.CheckInputs();
				}
			}
			else
			{
				this.inputEnabled = value;
			}
		}
	}

	// Token: 0x17000312 RID: 786
	// (get) Token: 0x06001466 RID: 5222 RVA: 0x00011AAF File Offset: 0x0000FCAF
	public tk2dUIItem PressedUIItem
	{
		get
		{
			if (!this.useMultiTouch)
			{
				return this.pressedUIItem;
			}
			if (this.pressedUIItems.Length > 0)
			{
				return this.pressedUIItems[this.pressedUIItems.Length - 1];
			}
			return null;
		}
	}

	// Token: 0x17000313 RID: 787
	// (get) Token: 0x06001467 RID: 5223 RVA: 0x00011AE4 File Offset: 0x0000FCE4
	public tk2dUIItem[] PressedUIItems
	{
		get
		{
			return this.pressedUIItems;
		}
	}

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x06001468 RID: 5224 RVA: 0x00011AEC File Offset: 0x0000FCEC
	// (set) Token: 0x06001469 RID: 5225 RVA: 0x00011AF4 File Offset: 0x0000FCF4
	public bool UseMultiTouch
	{
		get
		{
			return this.useMultiTouch;
		}
		set
		{
			if (this.useMultiTouch != value && this.inputEnabled)
			{
				this.InputEnabled = false;
				this.useMultiTouch = value;
				this.InputEnabled = true;
			}
			else
			{
				this.useMultiTouch = value;
			}
		}
	}

	// Token: 0x0600146A RID: 5226 RVA: 0x00089838 File Offset: 0x00087A38
	private void SortCameras()
	{
		this.sortedCameras.Clear();
		int count = tk2dUIManager.allCameras.Count;
		for (int i = 0; i < count; i++)
		{
			tk2dUICamera tk2dUICamera = tk2dUIManager.allCameras[i];
			if (tk2dUICamera != null)
			{
				this.sortedCameras.Add(tk2dUICamera);
			}
		}
		this.sortedCameras.Sort((tk2dUICamera a, tk2dUICamera b) => b.camera.depth.CompareTo(a.camera.depth));
	}

	// Token: 0x0600146B RID: 5227 RVA: 0x000898BC File Offset: 0x00087ABC
	private void Awake()
	{
		if (tk2dUIManager.instance == null)
		{
			tk2dUIManager.instance = this;
			if (tk2dUIManager.instance.transform.childCount != 0)
			{
				Debug.LogError("You should not attach anything to the tk2dUIManager object. The tk2dUIManager will not get destroyed between scene switches and any children will persist as well.");
			}
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}
		else if (tk2dUIManager.instance != this)
		{
			Debug.Log("Discarding unnecessary tk2dUIManager instance.");
			if (this.uiCamera != null)
			{
				this.HookUpLegacyCamera(this.uiCamera);
				this.uiCamera = null;
			}
			UnityEngine.Object.Destroy(this);
			return;
		}
		tk2dUITime.Init();
		this.Setup();
	}

	// Token: 0x0600146C RID: 5228 RVA: 0x00089968 File Offset: 0x00087B68
	private void HookUpLegacyCamera(Camera cam)
	{
		if (cam.GetComponent<tk2dUICamera>() == null)
		{
			tk2dUICamera tk2dUICamera = cam.gameObject.AddComponent<tk2dUICamera>();
			tk2dUICamera.AssignRaycastLayerMask(this.raycastLayerMask);
		}
	}

	// Token: 0x0600146D RID: 5229 RVA: 0x000899A0 File Offset: 0x00087BA0
	private void Start()
	{
		if (this.uiCamera != null)
		{
			Debug.Log("It is no longer necessary to hook up a camera to the tk2dUIManager. You can simply attach a tk2dUICamera script to the cameras that interact with UI.");
			this.HookUpLegacyCamera(this.uiCamera);
			this.uiCamera = null;
		}
		if (tk2dUIManager.allCameras.Count == 0)
		{
			Debug.LogError("Unable to find any tk2dUICameras, and no cameras are connected to the tk2dUIManager. You will not be able to interact with the UI.");
		}
	}

	// Token: 0x0600146E RID: 5230 RVA: 0x00011B2E File Offset: 0x0000FD2E
	private void Setup()
	{
		if (!this.areHoverEventsTracked)
		{
			this.checkForHovers = false;
		}
	}

	// Token: 0x0600146F RID: 5231 RVA: 0x000899F4 File Offset: 0x00087BF4
	private void Update()
	{
		tk2dUITime.Update();
		if (this.inputEnabled)
		{
			this.SortCameras();
			if (this.useMultiTouch)
			{
				this.CheckMultiTouchInputs();
			}
			else
			{
				this.CheckInputs();
			}
			if (this.OnInputUpdate != null)
			{
				this.OnInputUpdate();
			}
			if (this.OnScrollWheelChange != null)
			{
				float axis = Input.GetAxis("Mouse ScrollWheel");
				if (axis != 0f)
				{
					this.OnScrollWheelChange(axis);
				}
			}
		}
	}

	// Token: 0x06001470 RID: 5232 RVA: 0x00089A78 File Offset: 0x00087C78
	private void CheckInputs()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		this.primaryTouch = default(tk2dUITouch);
		this.secondaryTouch = default(tk2dUITouch);
		this.resultTouch = default(tk2dUITouch);
		this.hitUIItem = null;
		if (this.inputEnabled)
		{
			if (Input.touchCount > 0)
			{
				foreach (Touch touch in Input.touches)
				{
					if (touch.phase == TouchPhase.Began)
					{
						this.primaryTouch = new tk2dUITouch(touch);
						flag = true;
						flag3 = true;
					}
					else if (this.pressedUIItem != null && touch.fingerId == this.firstPressedUIItemTouch.fingerId)
					{
						this.secondaryTouch = new tk2dUITouch(touch);
						flag2 = true;
					}
				}
				this.checkForHovers = false;
			}
			else if (Input.GetMouseButtonDown(0))
			{
				this.primaryTouch = new tk2dUITouch(TouchPhase.Began, 9999, Input.mousePosition, Vector2.zero, 0f);
				flag = true;
				flag3 = true;
			}
			else if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
			{
				Vector2 vector = Vector2.zero;
				TouchPhase touchPhase = TouchPhase.Moved;
				if (this.pressedUIItem != null)
				{
					vector = this.firstPressedUIItemTouch.position - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				}
				if (Input.GetMouseButtonUp(0))
				{
					touchPhase = TouchPhase.Ended;
				}
				else if (vector == Vector2.zero)
				{
					touchPhase = TouchPhase.Stationary;
				}
				this.secondaryTouch = new tk2dUITouch(touchPhase, 9999, Input.mousePosition, vector, tk2dUITime.deltaTime);
				flag2 = true;
			}
		}
		if (flag)
		{
			this.resultTouch = this.primaryTouch;
		}
		else if (flag2)
		{
			this.resultTouch = this.secondaryTouch;
		}
		if (flag || flag2)
		{
			this.hitUIItem = this.RaycastForUIItem(this.resultTouch.position);
			if (this.resultTouch.phase == TouchPhase.Began)
			{
				if (this.pressedUIItem != null)
				{
					this.pressedUIItem.CurrentOverUIItem(this.hitUIItem);
					if (this.pressedUIItem != this.hitUIItem)
					{
						this.pressedUIItem.Release();
						this.pressedUIItem = null;
					}
					else
					{
						this.firstPressedUIItemTouch = this.resultTouch;
					}
				}
				if (this.hitUIItem != null)
				{
					this.hitUIItem.Press(this.resultTouch);
				}
				this.pressedUIItem = this.hitUIItem;
				this.firstPressedUIItemTouch = this.resultTouch;
			}
			else if (this.resultTouch.phase == TouchPhase.Ended)
			{
				if (this.pressedUIItem != null)
				{
					this.pressedUIItem.CurrentOverUIItem(this.hitUIItem);
					this.pressedUIItem.UpdateTouch(this.resultTouch);
					this.pressedUIItem.Release();
					this.pressedUIItem = null;
				}
			}
			else if (this.pressedUIItem != null)
			{
				this.pressedUIItem.CurrentOverUIItem(this.hitUIItem);
				this.pressedUIItem.UpdateTouch(this.resultTouch);
			}
		}
		else if (this.pressedUIItem != null)
		{
			this.pressedUIItem.CurrentOverUIItem(null);
			this.pressedUIItem.Release();
			this.pressedUIItem = null;
		}
		if (this.checkForHovers)
		{
			if (this.inputEnabled)
			{
				if (!flag && !flag2 && this.hitUIItem == null && !Input.GetMouseButton(0))
				{
					this.hitUIItem = this.RaycastForUIItem(Input.mousePosition);
				}
				else if (Input.GetMouseButton(0))
				{
					this.hitUIItem = null;
				}
			}
			if (this.hitUIItem != null)
			{
				if (this.hitUIItem.isHoverEnabled)
				{
					if (!this.hitUIItem.HoverOver(this.overUIItem) && this.overUIItem != null)
					{
						this.overUIItem.HoverOut(this.hitUIItem);
					}
					this.overUIItem = this.hitUIItem;
				}
				else if (this.overUIItem != null)
				{
					this.overUIItem.HoverOut(null);
				}
			}
			else if (this.overUIItem != null)
			{
				this.overUIItem.HoverOut(null);
			}
		}
		if (flag3 && this.OnAnyPress != null)
		{
			this.OnAnyPress();
		}
	}

	// Token: 0x06001471 RID: 5233 RVA: 0x00089F40 File Offset: 0x00088140
	private void CheckMultiTouchInputs()
	{
		bool flag = false;
		this.touchCounter = 0;
		if (this.inputEnabled)
		{
			if (Input.touchCount > 0)
			{
				foreach (Touch touch in Input.touches)
				{
					if (this.touchCounter >= 5)
					{
						break;
					}
					this.allTouches[this.touchCounter] = new tk2dUITouch(touch);
					this.touchCounter++;
				}
			}
			else if (Input.GetMouseButtonDown(0))
			{
				this.allTouches[this.touchCounter] = new tk2dUITouch(TouchPhase.Began, 9999, Input.mousePosition, Vector2.zero, 0f);
				this.mouseDownFirstPos = Input.mousePosition;
				this.touchCounter++;
			}
			else if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
			{
				Vector2 vector = this.mouseDownFirstPos - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				TouchPhase touchPhase = TouchPhase.Moved;
				if (Input.GetMouseButtonUp(0))
				{
					touchPhase = TouchPhase.Ended;
				}
				else if (vector == Vector2.zero)
				{
					touchPhase = TouchPhase.Stationary;
				}
				this.allTouches[this.touchCounter] = new tk2dUITouch(touchPhase, 9999, Input.mousePosition, vector, tk2dUITime.deltaTime);
				this.touchCounter++;
			}
		}
		for (int j = 0; j < this.touchCounter; j++)
		{
			this.pressedUIItems[j] = this.RaycastForUIItem(this.allTouches[j].position);
		}
		for (int k = 0; k < this.prevPressedUIItemList.Count; k++)
		{
			this.prevPressedItem = this.prevPressedUIItemList[k];
			if (this.prevPressedItem != null)
			{
				int fingerId = this.prevPressedItem.Touch.fingerId;
				bool flag2 = false;
				for (int l = 0; l < this.touchCounter; l++)
				{
					this.currTouch = this.allTouches[l];
					if (this.currTouch.fingerId == fingerId)
					{
						flag2 = true;
						this.currPressedItem = this.pressedUIItems[l];
						if (this.currTouch.phase == TouchPhase.Began)
						{
							this.prevPressedItem.CurrentOverUIItem(this.currPressedItem);
							if (this.prevPressedItem != this.currPressedItem)
							{
								this.prevPressedItem.Release();
								this.prevPressedUIItemList.RemoveAt(k);
								k--;
							}
						}
						else if (this.currTouch.phase == TouchPhase.Ended)
						{
							this.prevPressedItem.CurrentOverUIItem(this.currPressedItem);
							this.prevPressedItem.UpdateTouch(this.currTouch);
							this.prevPressedItem.Release();
							this.prevPressedUIItemList.RemoveAt(k);
							k--;
						}
						else
						{
							this.prevPressedItem.CurrentOverUIItem(this.currPressedItem);
							this.prevPressedItem.UpdateTouch(this.currTouch);
						}
						break;
					}
				}
				if (!flag2)
				{
					this.prevPressedItem.CurrentOverUIItem(null);
					this.prevPressedItem.Release();
					this.prevPressedUIItemList.RemoveAt(k);
					k--;
				}
			}
		}
		for (int m = 0; m < this.touchCounter; m++)
		{
			this.currPressedItem = this.pressedUIItems[m];
			this.currTouch = this.allTouches[m];
			if (this.currTouch.phase == TouchPhase.Began)
			{
				if (this.currPressedItem != null)
				{
					bool flag3 = this.currPressedItem.Press(this.currTouch);
					if (flag3)
					{
						this.prevPressedUIItemList.Add(this.currPressedItem);
					}
				}
				flag = true;
			}
		}
		if (flag && this.OnAnyPress != null)
		{
			this.OnAnyPress();
		}
	}

	// Token: 0x06001472 RID: 5234 RVA: 0x0008A398 File Offset: 0x00088598
	private tk2dUIItem RaycastForUIItem(Vector2 screenPos)
	{
		int count = this.sortedCameras.Count;
		for (int i = 0; i < count; i++)
		{
			tk2dUICamera tk2dUICamera = this.sortedCameras[0];
			this.ray = tk2dUICamera.HostCamera.ScreenPointToRay(screenPos);
			if (Physics.Raycast(this.ray, out this.hit, tk2dUICamera.HostCamera.farClipPlane, tk2dUICamera.FilteredMask))
			{
				return this.hit.collider.GetComponent<tk2dUIItem>();
			}
		}
		return null;
	}

	// Token: 0x06001473 RID: 5235 RVA: 0x0008A428 File Offset: 0x00088628
	public void OverrideClearAllChildrenPresses(tk2dUIItem item)
	{
		if (this.useMultiTouch)
		{
			for (int i = 0; i < this.pressedUIItems.Length; i++)
			{
				tk2dUIItem tk2dUIItem = this.pressedUIItems[i];
				if (tk2dUIItem != null && item.CheckIsUIItemChildOfMe(tk2dUIItem))
				{
					tk2dUIItem.CurrentOverUIItem(item);
				}
			}
		}
		else if (this.pressedUIItem != null && item.CheckIsUIItemChildOfMe(this.pressedUIItem))
		{
			this.pressedUIItem.CurrentOverUIItem(item);
		}
	}

	// Token: 0x040015B3 RID: 5555
	private const int MAX_MULTI_TOUCH_COUNT = 5;

	// Token: 0x040015B4 RID: 5556
	private const string MOUSE_WHEEL_AXES_NAME = "Mouse ScrollWheel";

	// Token: 0x040015B5 RID: 5557
	public static double version = 1.0;

	// Token: 0x040015B6 RID: 5558
	public static int releaseId = 0;

	// Token: 0x040015B7 RID: 5559
	private static tk2dUIManager instance;

	// Token: 0x040015B8 RID: 5560
	[SerializeField]
	private Camera uiCamera;

	// Token: 0x040015B9 RID: 5561
	private static List<tk2dUICamera> allCameras = new List<tk2dUICamera>();

	// Token: 0x040015BA RID: 5562
	private List<tk2dUICamera> sortedCameras = new List<tk2dUICamera>();

	// Token: 0x040015BB RID: 5563
	public LayerMask raycastLayerMask = -1;

	// Token: 0x040015BC RID: 5564
	private bool inputEnabled = true;

	// Token: 0x040015BD RID: 5565
	public bool areHoverEventsTracked = true;

	// Token: 0x040015BE RID: 5566
	private tk2dUIItem pressedUIItem;

	// Token: 0x040015BF RID: 5567
	private tk2dUIItem overUIItem;

	// Token: 0x040015C0 RID: 5568
	private tk2dUITouch firstPressedUIItemTouch;

	// Token: 0x040015C1 RID: 5569
	private bool checkForHovers = true;

	// Token: 0x040015C2 RID: 5570
	[SerializeField]
	private bool useMultiTouch;

	// Token: 0x040015C3 RID: 5571
	private tk2dUITouch[] allTouches = new tk2dUITouch[5];

	// Token: 0x040015C4 RID: 5572
	private List<tk2dUIItem> prevPressedUIItemList = new List<tk2dUIItem>();

	// Token: 0x040015C5 RID: 5573
	private tk2dUIItem[] pressedUIItems = new tk2dUIItem[5];

	// Token: 0x040015C6 RID: 5574
	private int touchCounter;

	// Token: 0x040015C7 RID: 5575
	private Vector2 mouseDownFirstPos = Vector2.zero;

	// Token: 0x040015C8 RID: 5576
	private tk2dUITouch primaryTouch = default(tk2dUITouch);

	// Token: 0x040015C9 RID: 5577
	private tk2dUITouch secondaryTouch = default(tk2dUITouch);

	// Token: 0x040015CA RID: 5578
	private tk2dUITouch resultTouch = default(tk2dUITouch);

	// Token: 0x040015CB RID: 5579
	private tk2dUIItem hitUIItem;

	// Token: 0x040015CC RID: 5580
	private RaycastHit hit;

	// Token: 0x040015CD RID: 5581
	private Ray ray;

	// Token: 0x040015CE RID: 5582
	private tk2dUITouch currTouch;

	// Token: 0x040015CF RID: 5583
	private tk2dUIItem currPressedItem;

	// Token: 0x040015D0 RID: 5584
	private tk2dUIItem prevPressedItem;
}
