using System;
using UnityEngine;

// Token: 0x02000220 RID: 544
public class iPhoneToMouse : MonoBehaviour
{
	// Token: 0x17000241 RID: 577
	// (get) Token: 0x06000F9E RID: 3998 RVA: 0x0000D654 File Offset: 0x0000B854
	// (set) Token: 0x06000F9F RID: 3999 RVA: 0x0000D65C File Offset: 0x0000B85C
	public bool Arcade { get; set; }

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x00070DF8 File Offset: 0x0006EFF8
	public static iPhoneToMouse instance
	{
		get
		{
			if (null == iPhoneToMouse.s_Instance)
			{
				iPhoneToMouse.s_Instance = UnityEngine.Object.FindObjectOfType(typeof(iPhoneToMouse)) as iPhoneToMouse;
				if (null == iPhoneToMouse.s_Instance)
				{
					iPhoneToMouse.s_Instance = new GameObject("iPhoneToMouse").AddComponent(typeof(iPhoneToMouse)) as iPhoneToMouse;
				}
			}
			return iPhoneToMouse.s_Instance;
		}
	}

	// Token: 0x06000FA1 RID: 4001 RVA: 0x0000D665 File Offset: 0x0000B865
	private void Awake()
	{
		Logger.Log("iPhoneToMouse", "Initalized iPhoneToMouse", new object[0]);
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x0000D682 File Offset: 0x0000B882
	public int touchCount
	{
		get
		{
			if (this.bMobilePlatform)
			{
				return Input.touchCount;
			}
			if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
			{
				return 1;
			}
			return 0;
		}
	}

	// Token: 0x06000FA3 RID: 4003 RVA: 0x00070E60 File Offset: 0x0006F060
	public iPhoneToMouse.pos GetTouch(int ID)
	{
		iPhoneToMouse.pos pos = default(iPhoneToMouse.pos);
		if (this.bMobilePlatform)
		{
			pos.deltaPosition = Input.GetTouch(ID).deltaPosition;
			pos.position = Input.GetTouch(ID).position;
			pos.phase = Input.GetTouch(ID).phase;
			pos.deltatime = Input.GetTouch(ID).deltaTime;
			return pos;
		}
		pos.position = Input.mousePosition;
		if (ConfigManager.Instance.Get<string>("game.input", false).Equals("real_io") && GameData.FLIP)
		{
			pos.position.x = (float)Screen.width - pos.position.x;
			pos.position.y = (float)Screen.height - pos.position.y;
		}
		if (Input.GetMouseButtonDown(ID))
		{
			pos.phase = TouchPhase.Began;
			this.BeforePos = pos.position;
			this.ButtonDown = true;
			pos.deltaPosition.x = 0f;
			pos.deltaPosition.y = 0f;
		}
		else if (Input.GetMouseButton(ID) && this.ButtonDown)
		{
			pos.deltaPosition = (pos.position - this.BeforePos) / 5f;
			pos.phase = TouchPhase.Moved;
		}
		if (Input.GetMouseButtonUp(ID))
		{
			pos.deltaPosition.x = 0f;
			pos.deltaPosition.y = 0f;
			pos.phase = TouchPhase.Ended;
			this.ButtonDown = false;
		}
		return pos;
	}

	// Token: 0x06000FA4 RID: 4004 RVA: 0x0000D6B5 File Offset: 0x0000B8B5
	public Vector3 GetAcceration()
	{
		return Input.acceleration;
	}

	// Token: 0x06000FA5 RID: 4005 RVA: 0x0000D6BC File Offset: 0x0000B8BC
	public float GetAxis(string strAxisName)
	{
		return Input.GetAxis(strAxisName);
	}

	// Token: 0x06000FA6 RID: 4006 RVA: 0x00003648 File Offset: 0x00001848
	public void LateUpdate()
	{
	}

	// Token: 0x0400116C RID: 4460
	private static iPhoneToMouse s_Instance;

	// Token: 0x0400116D RID: 4461
	private bool bMobilePlatform;

	// Token: 0x0400116E RID: 4462
	private Vector2 BeforePos = Vector2.zero;

	// Token: 0x04001170 RID: 4464
	private bool ButtonDown;

	// Token: 0x02000221 RID: 545
	public struct pos
	{
		// Token: 0x04001171 RID: 4465
		public Vector2 deltaPosition;

		// Token: 0x04001172 RID: 4466
		public Vector2 position;

		// Token: 0x04001173 RID: 4467
		public TouchPhase phase;

		// Token: 0x04001174 RID: 4468
		public float deltatime;
	}
}
