using System;
using UnityEngine;

// Token: 0x020002A2 RID: 674
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/UI/tk2dUIScrollbar")]
public class tk2dUIScrollbar : MonoBehaviour
{
	// Token: 0x1400000C RID: 12
	// (add) Token: 0x0600137F RID: 4991 RVA: 0x00010C73 File Offset: 0x0000EE73
	// (remove) Token: 0x06001380 RID: 4992 RVA: 0x00010C8C File Offset: 0x0000EE8C
	public event Action<tk2dUIScrollbar> OnScroll;

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x06001381 RID: 4993 RVA: 0x00010CA5 File Offset: 0x0000EEA5
	// (set) Token: 0x06001382 RID: 4994 RVA: 0x00086778 File Offset: 0x00084978
	public tk2dUILayout BarLayoutItem
	{
		get
		{
			return this.barLayoutItem;
		}
		set
		{
			if (this.barLayoutItem != value)
			{
				if (this.barLayoutItem != null)
				{
					this.barLayoutItem.OnReshape -= this.LayoutReshaped;
				}
				this.barLayoutItem = value;
				if (this.barLayoutItem != null)
				{
					this.barLayoutItem.OnReshape += this.LayoutReshaped;
				}
			}
		}
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x06001383 RID: 4995 RVA: 0x00010CAD File Offset: 0x0000EEAD
	// (set) Token: 0x06001384 RID: 4996 RVA: 0x00010CCD File Offset: 0x0000EECD
	public GameObject SendMessageTarget
	{
		get
		{
			if (this.barUIItem != null)
			{
				return this.barUIItem.sendMessageTarget;
			}
			return null;
		}
		set
		{
			if (this.barUIItem != null && this.barUIItem.sendMessageTarget != value)
			{
				this.barUIItem.sendMessageTarget = value;
			}
		}
	}

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x06001385 RID: 4997 RVA: 0x00010D02 File Offset: 0x0000EF02
	// (set) Token: 0x06001386 RID: 4998 RVA: 0x000867F0 File Offset: 0x000849F0
	public float Value
	{
		get
		{
			return this.percent;
		}
		set
		{
			this.percent = Mathf.Clamp(value, 0f, 1f);
			if (this.OnScroll != null)
			{
				this.OnScroll(this);
			}
			this.SetScrollThumbPosition();
			if (this.SendMessageTarget != null && this.SendMessageOnScrollMethodName.Length > 0)
			{
				this.SendMessageTarget.SendMessage(this.SendMessageOnScrollMethodName, this, SendMessageOptions.RequireReceiver);
			}
		}
	}

	// Token: 0x06001387 RID: 4999 RVA: 0x00010D0A File Offset: 0x0000EF0A
	public void SetScrollPercentWithoutEvent(float newScrollPercent)
	{
		this.percent = Mathf.Clamp(newScrollPercent, 0f, 1f);
		this.SetScrollThumbPosition();
	}

	// Token: 0x06001388 RID: 5000 RVA: 0x00086868 File Offset: 0x00084A68
	private void OnEnable()
	{
		if (this.barUIItem != null)
		{
			this.barUIItem.OnDown += this.ScrollTrackButtonDown;
			this.barUIItem.OnHoverOver += this.ScrollTrackButtonHoverOver;
			this.barUIItem.OnHoverOut += this.ScrollTrackButtonHoverOut;
		}
		if (this.thumbBtn != null)
		{
			this.thumbBtn.OnDown += this.ScrollThumbButtonDown;
			this.thumbBtn.OnRelease += this.ScrollThumbButtonRelease;
		}
		if (this.upButton != null)
		{
			this.upButton.OnDown += this.ScrollUpButtonDown;
			this.upButton.OnUp += this.ScrollUpButtonUp;
		}
		if (this.downButton != null)
		{
			this.downButton.OnDown += this.ScrollDownButtonDown;
			this.downButton.OnUp += this.ScrollDownButtonUp;
		}
		if (this.barLayoutItem != null)
		{
			this.barLayoutItem.OnReshape += this.LayoutReshaped;
		}
	}

	// Token: 0x06001389 RID: 5001 RVA: 0x000869B0 File Offset: 0x00084BB0
	private void OnDisable()
	{
		if (this.barUIItem != null)
		{
			this.barUIItem.OnDown -= this.ScrollTrackButtonDown;
			this.barUIItem.OnHoverOver -= this.ScrollTrackButtonHoverOver;
			this.barUIItem.OnHoverOut -= this.ScrollTrackButtonHoverOut;
		}
		if (this.thumbBtn != null)
		{
			this.thumbBtn.OnDown -= this.ScrollThumbButtonDown;
			this.thumbBtn.OnRelease -= this.ScrollThumbButtonRelease;
		}
		if (this.upButton != null)
		{
			this.upButton.OnDown -= this.ScrollUpButtonDown;
			this.upButton.OnUp -= this.ScrollUpButtonUp;
		}
		if (this.downButton != null)
		{
			this.downButton.OnDown -= this.ScrollDownButtonDown;
			this.downButton.OnUp -= this.ScrollDownButtonUp;
		}
		if (this.isScrollThumbButtonDown)
		{
			if (tk2dUIManager.Instance__NoCreate != null)
			{
				tk2dUIManager.Instance.OnInputUpdate -= this.MoveScrollThumbButton;
			}
			this.isScrollThumbButtonDown = false;
		}
		if (this.isTrackHoverOver)
		{
			if (tk2dUIManager.Instance__NoCreate != null)
			{
				tk2dUIManager.Instance.OnScrollWheelChange -= this.TrackHoverScrollWheelChange;
			}
			this.isTrackHoverOver = false;
		}
		if (this.scrollUpDownButtonState != 0)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.CheckRepeatScrollUpDownButton;
			this.scrollUpDownButtonState = 0;
		}
		if (this.barLayoutItem != null)
		{
			this.barLayoutItem.OnReshape -= this.LayoutReshaped;
		}
	}

	// Token: 0x0600138A RID: 5002 RVA: 0x00086B90 File Offset: 0x00084D90
	private void Awake()
	{
		if (this.upButton != null)
		{
			this.hoverUpButton = this.upButton.GetComponent<tk2dUIHoverItem>();
		}
		if (this.downButton != null)
		{
			this.hoverDownButton = this.downButton.GetComponent<tk2dUIHoverItem>();
		}
	}

	// Token: 0x0600138B RID: 5003 RVA: 0x00010D28 File Offset: 0x0000EF28
	private void Start()
	{
		this.SetScrollThumbPosition();
	}

	// Token: 0x0600138C RID: 5004 RVA: 0x00010D30 File Offset: 0x0000EF30
	private void TrackHoverScrollWheelChange(float mouseWheelChange)
	{
		if (mouseWheelChange > 0f)
		{
			this.ScrollUpFixed();
		}
		else if (mouseWheelChange < 0f)
		{
			this.ScrollDownFixed();
		}
	}

	// Token: 0x0600138D RID: 5005 RVA: 0x00086BE4 File Offset: 0x00084DE4
	private void SetScrollThumbPosition()
	{
		if (this.thumbTransform != null)
		{
			float num = -((this.scrollBarLength - this.thumbLength) * this.Value);
			Vector3 localPosition = this.thumbTransform.localPosition;
			if (this.scrollAxes == tk2dUIScrollbar.Axes.XAxis)
			{
				localPosition.x = -num;
			}
			else if (this.scrollAxes == tk2dUIScrollbar.Axes.YAxis)
			{
				localPosition.y = num;
			}
			this.thumbTransform.localPosition = localPosition;
		}
		if (this.highlightProgressBar != null)
		{
			this.highlightProgressBar.Value = this.Value;
		}
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x00010D59 File Offset: 0x0000EF59
	private void MoveScrollThumbButton()
	{
		this.ScrollToPosition(this.CalculateClickWorldPos(this.thumbBtn) + this.moveThumbBtnOffset);
	}

	// Token: 0x0600138F RID: 5007 RVA: 0x00086C80 File Offset: 0x00084E80
	private Vector3 CalculateClickWorldPos(tk2dUIItem btn)
	{
		Camera uicameraForControl = tk2dUIManager.Instance.GetUICameraForControl(base.gameObject);
		Vector2 position = btn.Touch.position;
		Vector3 vector = uicameraForControl.ScreenToWorldPoint(new Vector3(position.x, position.y, btn.transform.position.z - uicameraForControl.transform.position.z));
		vector.z = btn.transform.position.z;
		return vector;
	}

	// Token: 0x06001390 RID: 5008 RVA: 0x00086D0C File Offset: 0x00084F0C
	private void ScrollToPosition(Vector3 worldPos)
	{
		Vector3 vector = this.thumbTransform.parent.InverseTransformPoint(worldPos);
		float num = 0f;
		if (this.scrollAxes == tk2dUIScrollbar.Axes.XAxis)
		{
			num = vector.x;
		}
		else if (this.scrollAxes == tk2dUIScrollbar.Axes.YAxis)
		{
			num = -vector.y;
		}
		this.Value = num / (this.scrollBarLength - this.thumbLength);
	}

	// Token: 0x06001391 RID: 5009 RVA: 0x00010D78 File Offset: 0x0000EF78
	private void ScrollTrackButtonDown()
	{
		this.ScrollToPosition(this.CalculateClickWorldPos(this.barUIItem));
	}

	// Token: 0x06001392 RID: 5010 RVA: 0x00010D8C File Offset: 0x0000EF8C
	private void ScrollTrackButtonHoverOver()
	{
		if (this.allowScrollWheel)
		{
			if (!this.isTrackHoverOver)
			{
				tk2dUIManager.Instance.OnScrollWheelChange += this.TrackHoverScrollWheelChange;
			}
			this.isTrackHoverOver = true;
		}
	}

	// Token: 0x06001393 RID: 5011 RVA: 0x00010DC1 File Offset: 0x0000EFC1
	private void ScrollTrackButtonHoverOut()
	{
		if (this.isTrackHoverOver)
		{
			tk2dUIManager.Instance.OnScrollWheelChange -= this.TrackHoverScrollWheelChange;
		}
		this.isTrackHoverOver = false;
	}

	// Token: 0x06001394 RID: 5012 RVA: 0x00086D74 File Offset: 0x00084F74
	private void ScrollThumbButtonDown()
	{
		if (!this.isScrollThumbButtonDown)
		{
			tk2dUIManager.Instance.OnInputUpdate += this.MoveScrollThumbButton;
		}
		this.isScrollThumbButtonDown = true;
		Vector3 vector = this.CalculateClickWorldPos(this.thumbBtn);
		this.moveThumbBtnOffset = this.thumbBtn.transform.position - vector;
		this.moveThumbBtnOffset.z = 0f;
		if (this.hoverUpButton != null)
		{
			this.hoverUpButton.IsOver = true;
		}
		if (this.hoverDownButton != null)
		{
			this.hoverDownButton.IsOver = true;
		}
	}

	// Token: 0x06001395 RID: 5013 RVA: 0x00086E1C File Offset: 0x0008501C
	private void ScrollThumbButtonRelease()
	{
		if (this.isScrollThumbButtonDown)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.MoveScrollThumbButton;
		}
		this.isScrollThumbButtonDown = false;
		if (this.hoverUpButton != null)
		{
			this.hoverUpButton.IsOver = false;
		}
		if (this.hoverDownButton != null)
		{
			this.hoverDownButton.IsOver = false;
		}
	}

	// Token: 0x06001396 RID: 5014 RVA: 0x00086E8C File Offset: 0x0008508C
	private void ScrollUpButtonDown()
	{
		this.timeOfUpDownButtonPressStart = Time.realtimeSinceStartup;
		this.repeatUpDownButtonHoldCounter = 0f;
		if (this.scrollUpDownButtonState == 0)
		{
			tk2dUIManager.Instance.OnInputUpdate += this.CheckRepeatScrollUpDownButton;
		}
		this.scrollUpDownButtonState = -1;
		this.ScrollUpFixed();
	}

	// Token: 0x06001397 RID: 5015 RVA: 0x00010DEB File Offset: 0x0000EFEB
	private void ScrollUpButtonUp()
	{
		if (this.scrollUpDownButtonState != 0)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.CheckRepeatScrollUpDownButton;
		}
		this.scrollUpDownButtonState = 0;
	}

	// Token: 0x06001398 RID: 5016 RVA: 0x00086EE0 File Offset: 0x000850E0
	private void ScrollDownButtonDown()
	{
		this.timeOfUpDownButtonPressStart = Time.realtimeSinceStartup;
		this.repeatUpDownButtonHoldCounter = 0f;
		if (this.scrollUpDownButtonState == 0)
		{
			tk2dUIManager.Instance.OnInputUpdate += this.CheckRepeatScrollUpDownButton;
		}
		this.scrollUpDownButtonState = 1;
		this.ScrollDownFixed();
	}

	// Token: 0x06001399 RID: 5017 RVA: 0x00010DEB File Offset: 0x0000EFEB
	private void ScrollDownButtonUp()
	{
		if (this.scrollUpDownButtonState != 0)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.CheckRepeatScrollUpDownButton;
		}
		this.scrollUpDownButtonState = 0;
	}

	// Token: 0x0600139A RID: 5018 RVA: 0x00010E15 File Offset: 0x0000F015
	public void ScrollUpFixed()
	{
		this.ScrollDirection(-1);
	}

	// Token: 0x0600139B RID: 5019 RVA: 0x00010E1E File Offset: 0x0000F01E
	public void ScrollDownFixed()
	{
		this.ScrollDirection(1);
	}

	// Token: 0x0600139C RID: 5020 RVA: 0x00086F34 File Offset: 0x00085134
	private void CheckRepeatScrollUpDownButton()
	{
		if (this.scrollUpDownButtonState != 0)
		{
			float num = Time.realtimeSinceStartup - this.timeOfUpDownButtonPressStart;
			if (this.repeatUpDownButtonHoldCounter == 0f)
			{
				if (num > 0.55f)
				{
					this.repeatUpDownButtonHoldCounter += 1f;
					num -= 0.55f;
					this.ScrollDirection(this.scrollUpDownButtonState);
				}
			}
			else if (num > 0.45f)
			{
				this.repeatUpDownButtonHoldCounter += 1f;
				num -= 0.45f;
				this.ScrollDirection(this.scrollUpDownButtonState);
			}
		}
	}

	// Token: 0x0600139D RID: 5021 RVA: 0x00086FD0 File Offset: 0x000851D0
	public void ScrollDirection(int dir)
	{
		if (this.scrollAxes == tk2dUIScrollbar.Axes.XAxis)
		{
			this.Value -= this.CalcScrollPercentOffsetButtonScrollDistance() * (float)dir * this.buttonUpDownScrollDistance;
		}
		else
		{
			this.Value += this.CalcScrollPercentOffsetButtonScrollDistance() * (float)dir * this.buttonUpDownScrollDistance;
		}
	}

	// Token: 0x0600139E RID: 5022 RVA: 0x00010E27 File Offset: 0x0000F027
	private float CalcScrollPercentOffsetButtonScrollDistance()
	{
		return 0.1f;
	}

	// Token: 0x0600139F RID: 5023 RVA: 0x00010E2E File Offset: 0x0000F02E
	private void LayoutReshaped(Vector3 dMin, Vector3 dMax)
	{
		this.scrollBarLength += ((this.scrollAxes != tk2dUIScrollbar.Axes.XAxis) ? (dMax.y - dMin.y) : (dMax.x - dMin.x));
	}

	// Token: 0x04001522 RID: 5410
	private const float WITHOUT_SCROLLBAR_FIXED_SCROLL_WHEEL_PERCENT = 0.1f;

	// Token: 0x04001523 RID: 5411
	private const float INITIAL_TIME_TO_REPEAT_UP_DOWN_SCROLL_BUTTON_SCROLLING_ON_HOLD = 0.55f;

	// Token: 0x04001524 RID: 5412
	private const float TIME_TO_REPEAT_UP_DOWN_SCROLL_BUTTON_SCROLLING_ON_HOLD = 0.45f;

	// Token: 0x04001525 RID: 5413
	public tk2dUIItem barUIItem;

	// Token: 0x04001526 RID: 5414
	public float scrollBarLength;

	// Token: 0x04001527 RID: 5415
	public tk2dUIItem thumbBtn;

	// Token: 0x04001528 RID: 5416
	public Transform thumbTransform;

	// Token: 0x04001529 RID: 5417
	public float thumbLength;

	// Token: 0x0400152A RID: 5418
	public tk2dUIItem upButton;

	// Token: 0x0400152B RID: 5419
	private tk2dUIHoverItem hoverUpButton;

	// Token: 0x0400152C RID: 5420
	public tk2dUIItem downButton;

	// Token: 0x0400152D RID: 5421
	private tk2dUIHoverItem hoverDownButton;

	// Token: 0x0400152E RID: 5422
	public float buttonUpDownScrollDistance = 1f;

	// Token: 0x0400152F RID: 5423
	public bool allowScrollWheel = true;

	// Token: 0x04001530 RID: 5424
	public tk2dUIScrollbar.Axes scrollAxes = tk2dUIScrollbar.Axes.YAxis;

	// Token: 0x04001531 RID: 5425
	public tk2dUIProgressBar highlightProgressBar;

	// Token: 0x04001532 RID: 5426
	[SerializeField]
	[HideInInspector]
	private tk2dUILayout barLayoutItem;

	// Token: 0x04001533 RID: 5427
	private bool isScrollThumbButtonDown;

	// Token: 0x04001534 RID: 5428
	private bool isTrackHoverOver;

	// Token: 0x04001535 RID: 5429
	private float percent;

	// Token: 0x04001536 RID: 5430
	private Vector3 moveThumbBtnOffset = Vector3.zero;

	// Token: 0x04001537 RID: 5431
	private int scrollUpDownButtonState;

	// Token: 0x04001538 RID: 5432
	private float timeOfUpDownButtonPressStart;

	// Token: 0x04001539 RID: 5433
	private float repeatUpDownButtonHoldCounter;

	// Token: 0x0400153A RID: 5434
	public string SendMessageOnScrollMethodName = string.Empty;

	// Token: 0x020002A3 RID: 675
	public enum Axes
	{
		// Token: 0x0400153D RID: 5437
		XAxis,
		// Token: 0x0400153E RID: 5438
		YAxis
	}
}
