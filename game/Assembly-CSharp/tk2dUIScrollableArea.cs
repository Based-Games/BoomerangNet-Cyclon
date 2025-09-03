using System;
using UnityEngine;

// Token: 0x020002A0 RID: 672
[AddComponentMenu("2D Toolkit/UI/tk2dUIScrollableArea")]
[ExecuteInEditMode]
public class tk2dUIScrollableArea : MonoBehaviour
{
	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06001358 RID: 4952 RVA: 0x000109BA File Offset: 0x0000EBBA
	// (remove) Token: 0x06001359 RID: 4953 RVA: 0x000109D3 File Offset: 0x0000EBD3
	public event Action<tk2dUIScrollableArea> OnScroll;

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x0600135A RID: 4954 RVA: 0x000109EC File Offset: 0x0000EBEC
	// (set) Token: 0x0600135B RID: 4955 RVA: 0x000109F4 File Offset: 0x0000EBF4
	public float ContentLength
	{
		get
		{
			return this.contentLength;
		}
		set
		{
			this.ContentLengthVisibleAreaLengthChange(this.contentLength, value, this.visibleAreaLength, this.visibleAreaLength);
		}
	}

	// Token: 0x170002EF RID: 751
	// (get) Token: 0x0600135C RID: 4956 RVA: 0x00010A0F File Offset: 0x0000EC0F
	// (set) Token: 0x0600135D RID: 4957 RVA: 0x00010A17 File Offset: 0x0000EC17
	public float VisibleAreaLength
	{
		get
		{
			return this.visibleAreaLength;
		}
		set
		{
			this.ContentLengthVisibleAreaLengthChange(this.contentLength, this.contentLength, this.visibleAreaLength, value);
		}
	}

	// Token: 0x170002F0 RID: 752
	// (get) Token: 0x0600135E RID: 4958 RVA: 0x00010A32 File Offset: 0x0000EC32
	// (set) Token: 0x0600135F RID: 4959 RVA: 0x000858D4 File Offset: 0x00083AD4
	public tk2dUILayout BackgroundLayoutItem
	{
		get
		{
			return this.backgroundLayoutItem;
		}
		set
		{
			if (this.backgroundLayoutItem != value)
			{
				if (this.backgroundLayoutItem != null)
				{
					this.backgroundLayoutItem.OnReshape -= this.LayoutReshaped;
				}
				this.backgroundLayoutItem = value;
				if (this.backgroundLayoutItem != null)
				{
					this.backgroundLayoutItem.OnReshape += this.LayoutReshaped;
				}
			}
		}
	}

	// Token: 0x170002F1 RID: 753
	// (get) Token: 0x06001360 RID: 4960 RVA: 0x00010A3A File Offset: 0x0000EC3A
	// (set) Token: 0x06001361 RID: 4961 RVA: 0x0008594C File Offset: 0x00083B4C
	public tk2dUILayoutContainer ContentLayoutContainer
	{
		get
		{
			return this.contentLayoutContainer;
		}
		set
		{
			if (this.contentLayoutContainer != value)
			{
				if (this.contentLayoutContainer != null)
				{
					this.contentLayoutContainer.OnChangeContent -= this.ContentLayoutChangeCallback;
				}
				this.contentLayoutContainer = value;
				if (this.contentLayoutContainer != null)
				{
					this.contentLayoutContainer.OnChangeContent += this.ContentLayoutChangeCallback;
				}
			}
		}
	}

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06001362 RID: 4962 RVA: 0x00010A42 File Offset: 0x0000EC42
	// (set) Token: 0x06001363 RID: 4963 RVA: 0x00010A62 File Offset: 0x0000EC62
	public GameObject SendMessageTarget
	{
		get
		{
			if (this.backgroundUIItem != null)
			{
				return this.backgroundUIItem.sendMessageTarget;
			}
			return null;
		}
		set
		{
			if (this.backgroundUIItem != null && this.backgroundUIItem.sendMessageTarget != value)
			{
				this.backgroundUIItem.sendMessageTarget = value;
			}
		}
	}

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06001364 RID: 4964 RVA: 0x00010A97 File Offset: 0x0000EC97
	// (set) Token: 0x06001365 RID: 4965 RVA: 0x000859C4 File Offset: 0x00083BC4
	public float Value
	{
		get
		{
			return Mathf.Clamp01(this.percent);
		}
		set
		{
			value = Mathf.Clamp(value, 0f, 1f);
			if (value != this.percent)
			{
				this.UnpressAllUIItemChildren();
				this.percent = value;
				if (this.OnScroll != null)
				{
					this.OnScroll(this);
				}
				if (this.isBackgroundButtonDown || this.isSwipeScrollingInProgress)
				{
					if (tk2dUIManager.Instance__NoCreate != null)
					{
						tk2dUIManager.Instance.OnInputUpdate -= this.BackgroundOverUpdate;
					}
					this.isBackgroundButtonDown = false;
					this.isSwipeScrollingInProgress = false;
				}
				this.TargetOnScrollCallback();
			}
			if (this.scrollBar != null)
			{
				this.scrollBar.SetScrollPercentWithoutEvent(this.percent);
			}
			this.SetContentPosition();
		}
	}

	// Token: 0x06001366 RID: 4966 RVA: 0x00085A8C File Offset: 0x00083C8C
	public void SetScrollPercentWithoutEvent(float newScrollPercent)
	{
		this.percent = Mathf.Clamp(newScrollPercent, 0f, 1f);
		this.UnpressAllUIItemChildren();
		if (this.scrollBar != null)
		{
			this.scrollBar.SetScrollPercentWithoutEvent(this.percent);
		}
		this.SetContentPosition();
	}

	// Token: 0x06001367 RID: 4967 RVA: 0x00085AE0 File Offset: 0x00083CE0
	public float MeasureContentLength()
	{
		Vector3 vector = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		Vector3 vector2 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3[] array = new Vector3[] { vector2, vector };
		Transform transform = this.contentContainer.transform;
		tk2dUIScrollableArea.GetRendererBoundsInChildren(transform.worldToLocalMatrix, array, transform);
		if (array[0] != vector2 && array[1] != vector)
		{
			array[0] = Vector3.Min(array[0], Vector3.zero);
			array[1] = Vector3.Max(array[1], Vector3.zero);
			return (this.scrollAxes != tk2dUIScrollableArea.Axes.YAxis) ? (array[1].x - array[0].x) : (array[1].y - array[0].y);
		}
		Debug.LogError("Unable to measure content length");
		return this.VisibleAreaLength * 0.9f;
	}

	// Token: 0x06001368 RID: 4968 RVA: 0x00085C20 File Offset: 0x00083E20
	private void OnEnable()
	{
		if (this.scrollBar != null)
		{
			this.scrollBar.OnScroll += this.ScrollBarMove;
		}
		if (this.backgroundUIItem != null)
		{
			this.backgroundUIItem.OnDown += this.BackgroundButtonDown;
			this.backgroundUIItem.OnRelease += this.BackgroundButtonRelease;
			this.backgroundUIItem.OnHoverOver += this.BackgroundButtonHoverOver;
			this.backgroundUIItem.OnHoverOut += this.BackgroundButtonHoverOut;
		}
		if (this.backgroundLayoutItem != null)
		{
			this.backgroundLayoutItem.OnReshape += this.LayoutReshaped;
		}
		if (this.contentLayoutContainer != null)
		{
			this.contentLayoutContainer.OnChangeContent += this.ContentLayoutChangeCallback;
		}
	}

	// Token: 0x06001369 RID: 4969 RVA: 0x00085D14 File Offset: 0x00083F14
	private void OnDisable()
	{
		if (this.scrollBar != null)
		{
			this.scrollBar.OnScroll -= this.ScrollBarMove;
		}
		if (this.backgroundUIItem != null)
		{
			this.backgroundUIItem.OnDown -= this.BackgroundButtonDown;
			this.backgroundUIItem.OnRelease -= this.BackgroundButtonRelease;
			this.backgroundUIItem.OnHoverOver -= this.BackgroundButtonHoverOver;
			this.backgroundUIItem.OnHoverOut -= this.BackgroundButtonHoverOut;
		}
		if (this.isBackgroundButtonOver)
		{
			if (tk2dUIManager.Instance__NoCreate != null)
			{
				tk2dUIManager.Instance.OnScrollWheelChange -= this.BackgroundHoverOverScrollWheelChange;
			}
			this.isBackgroundButtonOver = false;
		}
		if (this.isBackgroundButtonDown || this.isSwipeScrollingInProgress)
		{
			if (tk2dUIManager.Instance__NoCreate != null)
			{
				tk2dUIManager.Instance.OnInputUpdate -= this.BackgroundOverUpdate;
			}
			this.isBackgroundButtonDown = false;
			this.isSwipeScrollingInProgress = false;
		}
		if (this.backgroundLayoutItem != null)
		{
			this.backgroundLayoutItem.OnReshape -= this.LayoutReshaped;
		}
		if (this.contentLayoutContainer != null)
		{
			this.contentLayoutContainer.OnChangeContent -= this.ContentLayoutChangeCallback;
		}
		this.swipeCurrVelocity = 0f;
	}

	// Token: 0x0600136A RID: 4970 RVA: 0x00010AA4 File Offset: 0x0000ECA4
	private void Start()
	{
		this.UpdateScrollbarActiveState();
	}

	// Token: 0x0600136B RID: 4971 RVA: 0x00085E94 File Offset: 0x00084094
	private void BackgroundHoverOverScrollWheelChange(float mouseWheelChange)
	{
		if (mouseWheelChange > 0f)
		{
			if (this.scrollBar)
			{
				this.scrollBar.ScrollUpFixed();
			}
			else
			{
				this.Value -= 0.1f;
			}
		}
		else if (mouseWheelChange < 0f)
		{
			if (this.scrollBar)
			{
				this.scrollBar.ScrollDownFixed();
			}
			else
			{
				this.Value += 0.1f;
			}
		}
	}

	// Token: 0x0600136C RID: 4972 RVA: 0x00010AAC File Offset: 0x0000ECAC
	private void ScrollBarMove(tk2dUIScrollbar scrollBar)
	{
		this.Value = scrollBar.Value;
		this.isSwipeScrollingInProgress = false;
		if (this.isBackgroundButtonDown)
		{
			this.BackgroundButtonRelease();
		}
	}

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x0600136D RID: 4973 RVA: 0x00010AD2 File Offset: 0x0000ECD2
	// (set) Token: 0x0600136E RID: 4974 RVA: 0x00010AFD File Offset: 0x0000ECFD
	private Vector3 ContentContainerOffset
	{
		get
		{
			return Vector3.Scale(new Vector3(-1f, 1f, 1f), this.contentContainer.transform.localPosition);
		}
		set
		{
			this.contentContainer.transform.localPosition = Vector3.Scale(new Vector3(-1f, 1f, 1f), value);
		}
	}

	// Token: 0x0600136F RID: 4975 RVA: 0x00085F20 File Offset: 0x00084120
	private void SetContentPosition()
	{
		Vector3 contentContainerOffset = this.ContentContainerOffset;
		float num = (this.contentLength - this.visibleAreaLength) * this.Value;
		if (num < 0f)
		{
			num = 0f;
		}
		if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
		{
			contentContainerOffset.x = num;
		}
		else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
		{
			contentContainerOffset.y = num;
		}
		this.ContentContainerOffset = contentContainerOffset;
	}

	// Token: 0x06001370 RID: 4976 RVA: 0x00085F90 File Offset: 0x00084190
	private void BackgroundButtonDown()
	{
		if (this.allowSwipeScrolling && this.contentLength > this.visibleAreaLength)
		{
			if (!this.isBackgroundButtonDown && !this.isSwipeScrollingInProgress)
			{
				tk2dUIManager.Instance.OnInputUpdate += this.BackgroundOverUpdate;
			}
			this.swipeScrollingPressDownStartLocalPos = base.transform.InverseTransformPoint(this.CalculateClickWorldPos(this.backgroundUIItem));
			this.swipePrevScrollingContentPressLocalPos = this.swipeScrollingPressDownStartLocalPos;
			this.swipeScrollingContentStartLocalPos = this.ContentContainerOffset;
			this.swipeScrollingContentDestLocalPos = this.swipeScrollingContentStartLocalPos;
			this.isBackgroundButtonDown = true;
			this.swipeCurrVelocity = 0f;
		}
	}

	// Token: 0x06001371 RID: 4977 RVA: 0x00086038 File Offset: 0x00084238
	private void BackgroundOverUpdate()
	{
		if (this.isBackgroundButtonDown)
		{
			this.UpdateSwipeScrollDestintationPosition();
		}
		if (this.isSwipeScrollingInProgress)
		{
			float num = this.percent;
			float num2 = 0f;
			if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
			{
				num2 = this.swipeScrollingContentDestLocalPos.x;
			}
			else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
			{
				num2 = this.swipeScrollingContentDestLocalPos.y;
			}
			float num3 = 0f;
			float num4 = this.contentLength - this.visibleAreaLength;
			if (this.isBackgroundButtonDown)
			{
				if (num2 < num3)
				{
					num2 += -num2 / this.visibleAreaLength / 2f;
					if (num2 > num3)
					{
						num2 = num3;
					}
				}
				else if (num2 > num4)
				{
					num2 -= (num2 - num4) / this.visibleAreaLength / 2f;
					if (num2 < num4)
					{
						num2 = num4;
					}
				}
				if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
				{
					this.swipeScrollingContentDestLocalPos.x = num2;
				}
				else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
				{
					this.swipeScrollingContentDestLocalPos.y = num2;
				}
				num = num2 / (this.contentLength - this.visibleAreaLength);
			}
			else
			{
				float num5 = this.visibleAreaLength * 0.001f;
				if (num2 < num3 || num2 > num4)
				{
					float num6 = ((num2 >= num3) ? num4 : num3);
					num2 = Mathf.SmoothDamp(num2, num6, ref this.snapBackVelocity, 0.05f, float.PositiveInfinity, tk2dUITime.deltaTime);
					if (Mathf.Abs(this.snapBackVelocity) < num5)
					{
						num2 = num6;
						this.snapBackVelocity = 0f;
					}
					this.swipeCurrVelocity = 0f;
				}
				else if (this.swipeCurrVelocity != 0f)
				{
					num2 += this.swipeCurrVelocity * tk2dUITime.deltaTime * 20f;
					if (this.swipeCurrVelocity > num5 || this.swipeCurrVelocity < -num5)
					{
						this.swipeCurrVelocity = Mathf.Lerp(this.swipeCurrVelocity, 0f, tk2dUITime.deltaTime * 2.5f);
					}
					else
					{
						this.swipeCurrVelocity = 0f;
					}
				}
				else
				{
					this.isSwipeScrollingInProgress = false;
					tk2dUIManager.Instance.OnInputUpdate -= this.BackgroundOverUpdate;
				}
				if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
				{
					this.swipeScrollingContentDestLocalPos.x = num2;
				}
				else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
				{
					this.swipeScrollingContentDestLocalPos.y = num2;
				}
				num = num2 / (this.contentLength - this.visibleAreaLength);
			}
			if (num != this.percent)
			{
				this.percent = num;
				this.ContentContainerOffset = this.swipeScrollingContentDestLocalPos;
				if (this.OnScroll != null)
				{
					this.OnScroll(this);
				}
				this.TargetOnScrollCallback();
			}
			if (this.scrollBar != null)
			{
				float num7 = this.percent;
				if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
				{
					num7 = this.ContentContainerOffset.x / (this.contentLength - this.visibleAreaLength);
				}
				else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
				{
					num7 = this.ContentContainerOffset.y / (this.contentLength - this.visibleAreaLength);
				}
				this.scrollBar.SetScrollPercentWithoutEvent(num7);
			}
		}
	}

	// Token: 0x06001372 RID: 4978 RVA: 0x00086364 File Offset: 0x00084564
	private void UpdateSwipeScrollDestintationPosition()
	{
		Vector3 vector = base.transform.InverseTransformPoint(this.CalculateClickWorldPos(this.backgroundUIItem));
		Vector3 vector2 = vector - this.swipeScrollingPressDownStartLocalPos;
		vector2.x *= -1f;
		float num = 0f;
		if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
		{
			num = vector2.x;
			this.swipeCurrVelocity = -(vector.x - this.swipePrevScrollingContentPressLocalPos.x);
		}
		else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
		{
			num = vector2.y;
			this.swipeCurrVelocity = vector.y - this.swipePrevScrollingContentPressLocalPos.y;
		}
		if (!this.isSwipeScrollingInProgress && Mathf.Abs(num) > 0.02f)
		{
			this.isSwipeScrollingInProgress = true;
			tk2dUIManager.Instance.OverrideClearAllChildrenPresses(this.backgroundUIItem);
		}
		if (this.isSwipeScrollingInProgress)
		{
			Vector3 vector3 = this.swipeScrollingContentStartLocalPos + vector2;
			vector3.z = this.ContentContainerOffset.z;
			if (this.scrollAxes == tk2dUIScrollableArea.Axes.XAxis)
			{
				vector3.y = this.ContentContainerOffset.y;
			}
			else if (this.scrollAxes == tk2dUIScrollableArea.Axes.YAxis)
			{
				vector3.x = this.ContentContainerOffset.x;
			}
			vector3.z = this.ContentContainerOffset.z;
			this.swipeScrollingContentDestLocalPos = vector3;
			this.swipePrevScrollingContentPressLocalPos = vector;
		}
	}

	// Token: 0x06001373 RID: 4979 RVA: 0x00010B29 File Offset: 0x0000ED29
	private void BackgroundButtonRelease()
	{
		if (this.allowSwipeScrolling)
		{
			if (this.isBackgroundButtonDown && !this.isSwipeScrollingInProgress)
			{
				tk2dUIManager.Instance.OnInputUpdate -= this.BackgroundOverUpdate;
			}
			this.isBackgroundButtonDown = false;
		}
	}

	// Token: 0x06001374 RID: 4980 RVA: 0x00010B69 File Offset: 0x0000ED69
	private void BackgroundButtonHoverOver()
	{
		if (this.allowScrollWheel)
		{
			if (!this.isBackgroundButtonOver)
			{
				tk2dUIManager.Instance.OnScrollWheelChange += this.BackgroundHoverOverScrollWheelChange;
			}
			this.isBackgroundButtonOver = true;
		}
	}

	// Token: 0x06001375 RID: 4981 RVA: 0x00010B9E File Offset: 0x0000ED9E
	private void BackgroundButtonHoverOut()
	{
		if (this.isBackgroundButtonOver)
		{
			tk2dUIManager.Instance.OnScrollWheelChange -= this.BackgroundHoverOverScrollWheelChange;
		}
		this.isBackgroundButtonOver = false;
	}

	// Token: 0x06001376 RID: 4982 RVA: 0x000864DC File Offset: 0x000846DC
	private Vector3 CalculateClickWorldPos(tk2dUIItem btn)
	{
		Vector2 position = btn.Touch.position;
		Camera uicameraForControl = tk2dUIManager.Instance.GetUICameraForControl(base.gameObject);
		Vector3 vector = uicameraForControl.ScreenToWorldPoint(new Vector3(position.x, position.y, btn.transform.position.z - uicameraForControl.transform.position.z));
		vector.z = btn.transform.position.z;
		return vector;
	}

	// Token: 0x06001377 RID: 4983 RVA: 0x00086568 File Offset: 0x00084768
	private void UpdateScrollbarActiveState()
	{
		bool flag = this.contentLength > this.visibleAreaLength;
		if (this.scrollBar != null && this.scrollBar.gameObject.activeSelf != flag)
		{
			tk2dUIBaseItemControl.ChangeGameObjectActiveState(this.scrollBar.gameObject, flag);
		}
	}

	// Token: 0x06001378 RID: 4984 RVA: 0x000865BC File Offset: 0x000847BC
	private void ContentLengthVisibleAreaLengthChange(float prevContentLength, float newContentLength, float prevVisibleAreaLength, float newVisibleAreaLength)
	{
		float num;
		if (newContentLength - this.visibleAreaLength != 0f)
		{
			num = (prevContentLength - prevVisibleAreaLength) * this.Value / (newContentLength - newVisibleAreaLength);
		}
		else
		{
			num = 0f;
		}
		this.contentLength = newContentLength;
		this.visibleAreaLength = newVisibleAreaLength;
		this.UpdateScrollbarActiveState();
		this.Value = num;
	}

	// Token: 0x06001379 RID: 4985 RVA: 0x00003648 File Offset: 0x00001848
	private void UnpressAllUIItemChildren()
	{
	}

	// Token: 0x0600137A RID: 4986 RVA: 0x00010BC8 File Offset: 0x0000EDC8
	private void TargetOnScrollCallback()
	{
		if (this.SendMessageTarget != null && this.SendMessageOnScrollMethodName.Length > 0)
		{
			this.SendMessageTarget.SendMessage(this.SendMessageOnScrollMethodName, this, SendMessageOptions.RequireReceiver);
		}
	}

	// Token: 0x0600137B RID: 4987 RVA: 0x00086614 File Offset: 0x00084814
	private static void GetRendererBoundsInChildren(Matrix4x4 rootWorldToLocal, Vector3[] minMax, Transform t)
	{
		MeshFilter component = t.GetComponent<MeshFilter>();
		if (component != null && component.sharedMesh != null)
		{
			Bounds bounds = component.sharedMesh.bounds;
			Matrix4x4 matrix4x = rootWorldToLocal * t.localToWorldMatrix;
			for (int i = 0; i < 8; i++)
			{
				Vector3 vector = bounds.center + Vector3.Scale(bounds.extents, tk2dUIScrollableArea.boxExtents[i]);
				Vector3 vector2 = matrix4x.MultiplyPoint(vector);
				minMax[0] = Vector3.Min(minMax[0], vector2);
				minMax[1] = Vector3.Max(minMax[1], vector2);
			}
		}
		int childCount = t.childCount;
		for (int j = 0; j < childCount; j++)
		{
			Transform child = t.GetChild(j);
			if (t.gameObject.activeSelf)
			{
				tk2dUIScrollableArea.GetRendererBoundsInChildren(rootWorldToLocal, minMax, child);
			}
		}
	}

	// Token: 0x0600137C RID: 4988 RVA: 0x00010BFF File Offset: 0x0000EDFF
	private void LayoutReshaped(Vector3 dMin, Vector3 dMax)
	{
		this.VisibleAreaLength += ((this.scrollAxes != tk2dUIScrollableArea.Axes.XAxis) ? (dMax.y - dMin.y) : (dMax.x - dMin.x));
	}

	// Token: 0x0600137D RID: 4989 RVA: 0x00086728 File Offset: 0x00084928
	private void ContentLayoutChangeCallback()
	{
		if (this.contentLayoutContainer != null)
		{
			Vector2 innerSize = this.contentLayoutContainer.GetInnerSize();
			this.ContentLength = ((this.scrollAxes != tk2dUIScrollableArea.Axes.XAxis) ? innerSize.y : innerSize.x);
		}
	}

	// Token: 0x04001506 RID: 5382
	private const float SWIPE_SCROLLING_FIRST_SCROLL_THRESHOLD = 0.02f;

	// Token: 0x04001507 RID: 5383
	private const float WITHOUT_SCROLLBAR_FIXED_SCROLL_WHEEL_PERCENT = 0.1f;

	// Token: 0x04001508 RID: 5384
	[SerializeField]
	private float contentLength = 1f;

	// Token: 0x04001509 RID: 5385
	[SerializeField]
	private float visibleAreaLength = 1f;

	// Token: 0x0400150A RID: 5386
	public GameObject contentContainer;

	// Token: 0x0400150B RID: 5387
	public tk2dUIScrollbar scrollBar;

	// Token: 0x0400150C RID: 5388
	public tk2dUIItem backgroundUIItem;

	// Token: 0x0400150D RID: 5389
	public tk2dUIScrollableArea.Axes scrollAxes = tk2dUIScrollableArea.Axes.YAxis;

	// Token: 0x0400150E RID: 5390
	public bool allowSwipeScrolling = true;

	// Token: 0x0400150F RID: 5391
	public bool allowScrollWheel = true;

	// Token: 0x04001510 RID: 5392
	[HideInInspector]
	[SerializeField]
	private tk2dUILayout backgroundLayoutItem;

	// Token: 0x04001511 RID: 5393
	[HideInInspector]
	[SerializeField]
	private tk2dUILayoutContainer contentLayoutContainer;

	// Token: 0x04001512 RID: 5394
	private bool isBackgroundButtonDown;

	// Token: 0x04001513 RID: 5395
	private bool isBackgroundButtonOver;

	// Token: 0x04001514 RID: 5396
	private Vector3 swipeScrollingPressDownStartLocalPos = Vector3.zero;

	// Token: 0x04001515 RID: 5397
	private Vector3 swipeScrollingContentStartLocalPos = Vector3.zero;

	// Token: 0x04001516 RID: 5398
	private Vector3 swipeScrollingContentDestLocalPos = Vector3.zero;

	// Token: 0x04001517 RID: 5399
	private bool isSwipeScrollingInProgress;

	// Token: 0x04001518 RID: 5400
	private Vector3 swipePrevScrollingContentPressLocalPos = Vector3.zero;

	// Token: 0x04001519 RID: 5401
	private float swipeCurrVelocity;

	// Token: 0x0400151A RID: 5402
	private float snapBackVelocity;

	// Token: 0x0400151B RID: 5403
	public string SendMessageOnScrollMethodName = string.Empty;

	// Token: 0x0400151C RID: 5404
	private float percent;

	// Token: 0x0400151D RID: 5405
	private static readonly Vector3[] boxExtents = new Vector3[]
	{
		new Vector3(-1f, -1f, -1f),
		new Vector3(1f, -1f, -1f),
		new Vector3(-1f, 1f, -1f),
		new Vector3(1f, 1f, -1f),
		new Vector3(-1f, -1f, 1f),
		new Vector3(1f, -1f, 1f),
		new Vector3(-1f, 1f, 1f),
		new Vector3(1f, 1f, 1f)
	};

	// Token: 0x020002A1 RID: 673
	public enum Axes
	{
		// Token: 0x04001520 RID: 5408
		XAxis,
		// Token: 0x04001521 RID: 5409
		YAxis
	}
}
