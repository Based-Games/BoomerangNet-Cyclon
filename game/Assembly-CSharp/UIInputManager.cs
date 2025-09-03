using System;
using UnityEngine;

// Token: 0x02000212 RID: 530
public class UIInputManager : MonoBehaviour
{
	// Token: 0x1700023E RID: 574
	// (get) Token: 0x06000F5C RID: 3932 RVA: 0x0000D37A File Offset: 0x0000B57A
	public UIInputManager.MouseState_e isMouseState
	{
		get
		{
			return this.m_isMouseState;
		}
	}

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06000F5D RID: 3933 RVA: 0x0006E198 File Offset: 0x0006C398
	public static UIInputManager instance
	{
		get
		{
			if (null == UIInputManager.s_Instance)
			{
				UIInputManager.s_Instance = UnityEngine.Object.FindObjectOfType(typeof(UIInputManager)) as UIInputManager;
				if (null == UIInputManager.s_Instance)
				{
					GameObject gameObject = new GameObject("UIInputManager");
					UIInputManager.s_Instance = gameObject.AddComponent(typeof(UIInputManager)) as UIInputManager;
					Debug.Log("Could not locate an UIInputManager object. \n GameManager was Generated Automaticly.");
				}
			}
			return UIInputManager.s_Instance;
		}
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x0000D382 File Offset: 0x0000B582
	private void Start()
	{
		this.m_isMouseState = UIInputManager.MouseState_e.None;
	}

	// Token: 0x06000F5F RID: 3935 RVA: 0x0000D38B File Offset: 0x0000B58B
	private void Update()
	{
		this.InputProcess();
	}

	// Token: 0x06000F60 RID: 3936 RVA: 0x0006E214 File Offset: 0x0006C414
	private void InputProcess()
	{
		if (Input.GetMouseButtonDown(0) && this.m_isMouseState == UIInputManager.MouseState_e.None)
		{
			this.m_fLimitClickTime = 0f;
			this.m_fLimitClickValue = 0f;
			this.m_isMouseState = UIInputManager.MouseState_e.None;
			Ray ray = Camera.main.ScreenPointToRay(iPhoneToMouse.instance.GetTouch(0).position);
			if (Physics.Raycast(ray, out this.m_Hit, 1000f))
			{
				this.m_v2OldPosMove = (this.m_v2isMovePosition = (this.m_v2PressMousePos = (this.m_v2OldMousePos = iPhoneToMouse.instance.GetTouch(0).position)));
				this.m_v2DragMoveValue = Vector2.zero;
				this.m_isMouseState = UIInputManager.MouseState_e.Press;
				this.m_Hit.transform.SendMessage("UIPress");
			}
		}
		else
		{
			if (this.m_isMouseState == UIInputManager.MouseState_e.None)
			{
				return;
			}
			if (this.m_isMouseState == UIInputManager.MouseState_e.Click)
			{
				this.m_isMouseState = UIInputManager.MouseState_e.None;
			}
			if (this.m_isMouseState == UIInputManager.MouseState_e.Drag)
			{
				float num = iPhoneToMouse.instance.GetTouch(0).position.x - this.m_v2OldMousePos.x;
				float num2 = iPhoneToMouse.instance.GetTouch(0).position.y - this.m_v2OldMousePos.y;
				this.m_v2DragMoveValue = new Vector2(num, num2);
				if (this.m_v2DragMoveValue.x != 0f && this.m_v2DragMoveValue.y != 0f)
				{
					if (this.m_v2DragMoveValue.x > 0f)
					{
						this.m_v2DirMoveValue.x = 1f;
					}
					else if (this.m_v2DragMoveValue.x < 0f)
					{
						this.m_v2DirMoveValue.x = -1f;
					}
					if (this.m_v2DragMoveValue.y > 0f)
					{
						this.m_v2DirMoveValue.y = 1f;
					}
					else if (this.m_v2DragMoveValue.y < 0f)
					{
						this.m_v2DirMoveValue.y = -1f;
					}
				}
			}
			UIScroll.ScrollKind_e scrollKind_e = this.isPickObjScroll;
			if (scrollKind_e != UIScroll.ScrollKind_e.Horizontal)
			{
				if (scrollKind_e == UIScroll.ScrollKind_e.Vertical)
				{
					if ((this.m_v2PressMousePos.x + 1000f < iPhoneToMouse.instance.GetTouch(0).position.x || this.m_v2PressMousePos.x - 1000f > iPhoneToMouse.instance.GetTouch(0).position.x || this.m_v2PressMousePos.y + this.m_fDragSensitivity < iPhoneToMouse.instance.GetTouch(0).position.y || this.m_v2PressMousePos.y - this.m_fDragSensitivity > iPhoneToMouse.instance.GetTouch(0).position.y) && this.m_isMouseState != UIInputManager.MouseState_e.Drag)
					{
						this.m_isMouseState = UIInputManager.MouseState_e.Drag;
						this.m_Hit.transform.SendMessage("UIDrag");
					}
				}
			}
			else if ((this.m_v2PressMousePos.x + this.m_fDragSensitivity < iPhoneToMouse.instance.GetTouch(0).position.x || this.m_v2PressMousePos.x - this.m_fDragSensitivity > iPhoneToMouse.instance.GetTouch(0).position.x || this.m_v2PressMousePos.y + 1000f < iPhoneToMouse.instance.GetTouch(0).position.y || this.m_v2PressMousePos.y - 1000f > iPhoneToMouse.instance.GetTouch(0).position.y) && this.m_isMouseState != UIInputManager.MouseState_e.Drag)
			{
				this.m_isMouseState = UIInputManager.MouseState_e.Drag;
				this.m_Hit.transform.SendMessage("UIDrag");
			}
			this.m_v2isMovePosition = new Vector2(iPhoneToMouse.instance.GetTouch(0).position.x - this.m_v2OldPosMove.x, iPhoneToMouse.instance.GetTouch(0).position.y - this.m_v2OldPosMove.y);
			this.m_v2OldPosMove = this.m_v2OldMousePos;
			this.m_v2OldMousePos = iPhoneToMouse.instance.GetTouch(0).position;
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (this.m_Hit.transform == null)
			{
				this.m_isMouseState = UIInputManager.MouseState_e.None;
				return;
			}
			if (this.m_isMouseState != UIInputManager.MouseState_e.Drag)
			{
				if (this.m_isMouseState == UIInputManager.MouseState_e.None)
				{
					return;
				}
				this.m_isMouseState = UIInputManager.MouseState_e.Click;
				this.m_Hit.transform.SendMessage("UIClick");
				this.m_isMouseState = UIInputManager.MouseState_e.None;
			}
			else
			{
				this.m_Hit.transform.SendMessage("UIDragEnd");
				this.m_isMouseState = UIInputManager.MouseState_e.None;
			}
		}
	}

	// Token: 0x06000F61 RID: 3937 RVA: 0x0000D393 File Offset: 0x0000B593
	public Vector2 getMovePositionValue()
	{
		return this.m_v2isMovePosition;
	}

	// Token: 0x06000F62 RID: 3938 RVA: 0x0000D39B File Offset: 0x0000B59B
	public Vector2 getDragMoveValue()
	{
		return this.m_v2DragMoveValue;
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x0000D3A3 File Offset: 0x0000B5A3
	public Vector2 getDirMoveValue()
	{
		return this.m_v2DirMoveValue;
	}

	// Token: 0x04001108 RID: 4360
	private UIInputManager.MouseState_e m_isMouseState;

	// Token: 0x04001109 RID: 4361
	private bool m_bPress;

	// Token: 0x0400110A RID: 4362
	private bool m_bUnPress;

	// Token: 0x0400110B RID: 4363
	private bool m_bDrag;

	// Token: 0x0400110C RID: 4364
	private Vector2 m_v2OldMousePos;

	// Token: 0x0400110D RID: 4365
	private Vector2 m_v2OldPosMove;

	// Token: 0x0400110E RID: 4366
	private Vector2 m_v2PressMousePos;

	// Token: 0x0400110F RID: 4367
	private Vector2 m_v2DragMoveValue;

	// Token: 0x04001110 RID: 4368
	private Vector2 m_v2DirMoveValue;

	// Token: 0x04001111 RID: 4369
	private Vector2 m_v2isMovePosition;

	// Token: 0x04001112 RID: 4370
	private float m_flimitClickMaxValue = 25f;

	// Token: 0x04001113 RID: 4371
	private float m_fLimitClickValue;

	// Token: 0x04001114 RID: 4372
	private float m_fLimitClickTime;

	// Token: 0x04001115 RID: 4373
	private float m_fDragSensitivity = 25f;

	// Token: 0x04001116 RID: 4374
	private RaycastHit m_Hit;

	// Token: 0x04001117 RID: 4375
	private static UIInputManager s_Instance;

	// Token: 0x04001118 RID: 4376
	[HideInInspector]
	public UIScroll.ScrollKind_e isPickObjScroll;

	// Token: 0x02000213 RID: 531
	public enum MouseState_e
	{
		// Token: 0x0400111A RID: 4378
		None,
		// Token: 0x0400111B RID: 4379
		Press,
		// Token: 0x0400111C RID: 4380
		Click,
		// Token: 0x0400111D RID: 4381
		Drag,
		// Token: 0x0400111E RID: 4382
		DragEnd
	}
}
