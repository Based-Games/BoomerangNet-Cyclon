using System;
using UnityEngine;

// Token: 0x02000214 RID: 532
public class UIScroll : MonoBehaviour
{
	// Token: 0x06000F65 RID: 3941 RVA: 0x0006E78C File Offset: 0x0006C98C
	private void Awake()
	{
		this.SpeedSetting();
		this.m_UIInputManager = UIInputManager.instance;
		this.m_TweenPos = base.GetComponent<TweenPosition>();
		if (this.m_TweenPos == null)
		{
			base.gameObject.AddComponent("TweenPosition");
			this.m_TweenPos = base.GetComponent<TweenPosition>();
			this.m_TweenPos.duration = 1f;
			Keyframe[] array = new Keyframe[]
			{
				new Keyframe(0f, 0f, 1f, 2f),
				new Keyframe(1f, 1f, 0f, 0f)
			};
			this.m_TweenPos.animationCurve = new AnimationCurve(array);
			this.m_TweenPos.enabled = false;
		}
		this.m_v2PanelSize = this.m_Panel.GetViewSize();
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x0000D3AB File Offset: 0x0000B5AB
	private void Start()
	{
		this.m_v2SoftClipSize = Vector2.zero;
		if (this.m_Panel.clipping == UIDrawCall.Clipping.SoftClip)
		{
			this.m_v2SoftClipSize = this.m_Panel.clipSoftness;
		}
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x0000D3DA File Offset: 0x0000B5DA
	public void SpeedSetting()
	{
		this.m_fDragSpeed *= this.isDragSpeed;
		this.m_fSmoothSpeed *= this.isSmoothSpeed;
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x0006E874 File Offset: 0x0006CA74
	public void CellsSetting(int tabCount = 0)
	{
		this.m_ftabCount = (float)tabCount;
		this.m_gCells = new GameObject[this.Grid.transform.childCount];
		for (int i = 0; i < this.Grid.transform.childCount; i++)
		{
			this.m_gCells[i] = this.Grid.transform.GetChild(i).gameObject;
		}
		int num = this.m_gCells.Length;
		this.m_iLeftCellCount = (num - 1) / 2;
		this.m_iRightCellCount = num - 1 - this.m_iLeftCellCount;
		float num2 = 0f;
		if (this.m_ScrollKind == UIScroll.ScrollKind_e.Horizontal)
		{
			num2 = this.CellSize.x;
		}
		else if (this.m_ScrollKind == UIScroll.ScrollKind_e.Vertical)
		{
			num2 = this.CellSize.y;
		}
		this.m_fLimitLeftPos = (float)this.m_iLeftCellCount * -num2 - num2 * 0.5f;
		this.m_fLimitRightPos = (float)this.m_iRightCellCount * num2 + num2 * 0.5f;
		float num3 = (float)this.m_gCells.Length * -this.CellSize.x + this.m_v2PanelSize.x + this.m_ftabCount * 120f + -this.m_v2SoftClipSize.x;
		if (base.transform.localPosition.x <= num3 && this.m_ScrollKind == UIScroll.ScrollKind_e.Horizontal)
		{
			base.transform.localPosition = new Vector3(num3, base.transform.localPosition.y, base.transform.localPosition.z);
		}
		this.m_v2PanelSize = this.m_Panel.GetViewSize();
		this.SetProgressBar();
	}

	// Token: 0x06000F69 RID: 3945 RVA: 0x0006EA24 File Offset: 0x0006CC24
	private void SetProgressBar()
	{
		if (this.m_ProgressBar == null)
		{
			return;
		}
		if (this.m_ProgressBar.foregroundWidget == null)
		{
			return;
		}
		float num = 0f;
		float num2 = 0f;
		if (this.m_ScrollKind == UIScroll.ScrollKind_e.Horizontal)
		{
			num = this.CellSize.x;
			num2 = this.m_v2PanelSize.x;
		}
		else if (this.m_ScrollKind == UIScroll.ScrollKind_e.Vertical)
		{
			num = this.CellSize.y;
			num2 = this.m_v2PanelSize.y;
		}
		if ((float)this.m_gCells.Length * num2 <= num2)
		{
			this.m_ProgressBar.value = 1f;
		}
		if (num2 / ((float)this.m_gCells.Length * num) >= 1f)
		{
			this.m_ProgressBar.value = 1f;
		}
		else
		{
			this.m_ProgressBar.value = num2 / ((float)this.m_gCells.Length * num);
		}
		if (this.m_gCells.Length > 0)
		{
			this.m_bProgressState = true;
		}
		else
		{
			this.m_bProgressState = false;
		}
		this.m_fProgressBarValue = this.m_ProgressBar.value;
		this.m_v3ProgressBarStartPos = this.m_ProgressBar.foregroundWidget.transform.localPosition;
		this.m_iProgressBarStartHeightSize = this.m_ProgressBar.foregroundWidget.GetComponent<UISprite>().height;
	}

	// Token: 0x06000F6A RID: 3946 RVA: 0x0006EB80 File Offset: 0x0006CD80
	private void ProgressBarUpdate()
	{
		if (this.m_ProgressBar == null)
		{
			return;
		}
		if (this.m_ProgressBar.foregroundWidget == null)
		{
			return;
		}
		if (!this.m_bProgressState)
		{
			return;
		}
		Vector3 localPosition = this.m_ProgressBar.foregroundWidget.transform.localPosition;
		float num = (float)this.m_ProgressBar.foregroundWidget.GetComponent<UISprite>().height * this.m_ProgressBar.foregroundWidget.transform.localScale.y;
		float num2 = (num - num * this.m_ProgressBar.value) / (this.CellSize.y * (float)this.m_gCells.Length - this.m_v2PanelSize.y);
		float num3 = this.m_v3ProgressBarStartPos.y - base.transform.localPosition.y * num2;
		this.m_ProgressBar.fillDirection = UIProgressBar.FillDirection.TopToBottom;
		if (num3 > this.m_v3ProgressBarStartPos.y)
		{
			if (this.m_iProgressBarStartHeightSize + (int)base.transform.localPosition.y > 80)
			{
				this.m_ProgressBar.foregroundWidget.GetComponent<UISprite>().height = this.m_iProgressBarStartHeightSize + (int)base.transform.localPosition.y;
			}
		}
		else if (base.transform.localPosition.y > this.CellSize.y * (float)this.m_gCells.Length - this.m_v2PanelSize.y)
		{
			float num4 = (this.CellSize.y * (float)this.m_gCells.Length - this.m_v2PanelSize.y) / base.transform.localPosition.y;
			float num5 = 1f - num4;
			this.m_ProgressBar.fillDirection = UIProgressBar.FillDirection.BottomToTop;
			if (this.m_fProgressBarValue - num5 > 0.03f)
			{
				this.m_ProgressBar.value = this.m_fProgressBarValue - num5;
			}
			this.m_ProgressBar.foregroundWidget.transform.localPosition = new Vector3(localPosition.x, this.m_v3ProgressBarStartPos.y, localPosition.z);
		}
		else
		{
			this.m_ProgressBar.foregroundWidget.transform.localPosition = new Vector3(localPosition.x, num3, 0f);
		}
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x0000D402 File Offset: 0x0000B602
	public void ClickProcess()
	{
		this.m_bIsDrag = false;
		this.m_v2MouseUpPosition = this.m_UIInputManager.getMovePositionValue();
		this.SetSmoothPosMove();
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x0000D422 File Offset: 0x0000B622
	public void DragProcess()
	{
		this.m_bIsDrag = true;
		this.m_TweenPos.enabled = false;
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x0006EDE8 File Offset: 0x0006CFE8
	private void DragUpdate()
	{
		if (!this.m_bIsDrag)
		{
			return;
		}
		Vector3 localPosition = base.transform.localPosition;
		Vector3 zero = Vector3.zero;
		if (this.m_ScrollKind == UIScroll.ScrollKind_e.Horizontal)
		{
			if (this.m_UIInputManager.getDragMoveValue().x < 0f)
			{
				base.transform.localPosition = new Vector3(localPosition.x + this.m_UIInputManager.getDragMoveValue().x * this.m_fDragSpeed, localPosition.y, localPosition.z);
			}
			else if (this.m_UIInputManager.getDragMoveValue().x > 0f)
			{
				base.transform.localPosition = new Vector3(localPosition.x + this.m_UIInputManager.getDragMoveValue().x * this.m_fDragSpeed, localPosition.y, localPosition.z);
			}
		}
		else if (this.m_ScrollKind == UIScroll.ScrollKind_e.Vertical)
		{
			if (this.m_UIInputManager.getDragMoveValue().y < 0f)
			{
				base.transform.localPosition = new Vector3(localPosition.x, localPosition.y + this.m_UIInputManager.getDragMoveValue().y * this.m_fDragSpeed, localPosition.z);
			}
			else if (this.m_UIInputManager.getDragMoveValue().y > 0f)
			{
				base.transform.localPosition = new Vector3(localPosition.x, localPosition.y + this.m_UIInputManager.getDragMoveValue().y * this.m_fDragSpeed, localPosition.z);
			}
		}
		this.m_v2MouseUpPosition = this.m_UIInputManager.getMovePositionValue();
	}

	// Token: 0x06000F6E RID: 3950 RVA: 0x0006EFC4 File Offset: 0x0006D1C4
	public void SetSmoothPosMove()
	{
		Vector3 localPosition = base.transform.localPosition;
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		if (this.m_ScrollOption == UIScroll.ScrollOption_e.Normal)
		{
			UIScroll.ScrollKind_e scrollKind_e = this.m_ScrollKind;
			if (scrollKind_e != UIScroll.ScrollKind_e.Horizontal)
			{
				if (scrollKind_e == UIScroll.ScrollKind_e.Vertical)
				{
					float num = this.m_v2MouseUpPosition.y;
					if (num == 0f)
					{
						num = 0.2f * this.m_UIInputManager.getDirMoveValue().y;
					}
					else if (num > this.m_fTouchUplimtValue)
					{
						num = this.m_fTouchUplimtValue;
					}
					else if (num < -this.m_fTouchUplimtValue)
					{
						num = -this.m_fTouchUplimtValue;
					}
					float num2 = localPosition.y + num * this.m_fSmoothSpeed;
					if (num2 < 0f)
					{
						num2 = 0f;
					}
					if (num2 > (float)this.m_gCells.Length * this.CellSize.y - this.m_v2PanelSize.y)
					{
						num2 = (float)this.m_gCells.Length * this.CellSize.y - this.m_v2PanelSize.y;
					}
					float num3 = 1f;
					if (num2 < 0f)
					{
						num3 = -1f;
					}
					float num4 = (float)((int)(num2 / this.CellSize.y + 0.5f * num3)) * this.CellSize.y;
					int num5 = (int)(num4 / (this.CellSize.y * (float)this.m_gCells.Length));
					int num6 = (int)((num4 - this.CellSize.y * (float)(this.m_gCells.Length * num5)) / this.CellSize.y);
					if (localPosition.y > 0f)
					{
						this.m_iCenterIndex = this.m_gCells.Length - num6;
					}
					else
					{
						this.m_iCenterIndex = num6 * -1;
					}
					if (this.m_iCenterIndex == this.m_gCells.Length)
					{
						this.m_iCenterIndex = 0;
					}
					else if (this.m_iCenterIndex > this.m_gCells.Length)
					{
						this.m_iCenterIndex -= this.m_gCells.Length;
					}
					else if (this.m_iCenterIndex < 0)
					{
						this.m_iCenterIndex *= -1;
					}
					zero = new Vector3(localPosition.x, localPosition.y, localPosition.z);
					zero2 = new Vector3(localPosition.x, num2, localPosition.z);
				}
			}
			else
			{
				float num = this.m_v2MouseUpPosition.x;
				if (num == 0f)
				{
					num = 0.2f * this.m_UIInputManager.getDirMoveValue().x;
				}
				else if (num > this.m_fTouchUplimtValue)
				{
					num = this.m_fTouchUplimtValue;
				}
				else if (num < -this.m_fTouchUplimtValue)
				{
					num = -this.m_fTouchUplimtValue;
				}
				float num2 = localPosition.x + num * this.m_fSmoothSpeed;
				float num7 = ((float)this.m_gCells.Length - this.m_ftabCount) * -this.CellSize.x + this.m_v2PanelSize.x + this.m_ftabCount * -120f + -this.m_v2SoftClipSize.x;
				if (num2 > 0f)
				{
					num2 = 0f;
				}
				else if (num2 < num7)
				{
					num2 = num7;
				}
				zero = new Vector3(localPosition.x, localPosition.y, localPosition.z);
				zero2 = new Vector3(num2, localPosition.y, localPosition.z);
			}
		}
		else if (this.m_ScrollOption == UIScroll.ScrollOption_e.NormalCenter)
		{
			UIScroll.ScrollKind_e scrollKind_e = this.m_ScrollKind;
			if (scrollKind_e != UIScroll.ScrollKind_e.Horizontal)
			{
				if (scrollKind_e == UIScroll.ScrollKind_e.Vertical)
				{
					float num = this.m_v2MouseUpPosition.y;
					if (num == 0f)
					{
						num = 0.2f * this.m_UIInputManager.getDirMoveValue().y;
					}
					else if (num > this.m_fTouchUplimtValue)
					{
						num = this.m_fTouchUplimtValue;
					}
					else if (num < -this.m_fTouchUplimtValue)
					{
						num = -this.m_fTouchUplimtValue;
					}
					float num2 = localPosition.y + num * this.m_fSmoothSpeed;
					if (num2 > 0f)
					{
						num2 = 0f;
					}
					if (num2 < (float)(this.m_gCells.Length / 2) * -this.CellSize.y + this.CellSize.y * 0.5f)
					{
						num2 = (float)this.m_gCells.Length * -this.CellSize.y;
					}
					float num8 = 1f;
					float num9 = (float)((int)(num2 / this.CellSize.y + 0.5f * num8)) * this.CellSize.y;
					int num10 = (int)(num9 / (this.CellSize.y * (float)this.m_gCells.Length));
					int num11 = (int)((num9 - this.CellSize.y * (float)(this.m_gCells.Length * num10)) / this.CellSize.y);
					if (localPosition.y > 0f)
					{
						this.m_iCenterIndex = this.m_gCells.Length - num11;
					}
					else
					{
						this.m_iCenterIndex = num11 * -1;
					}
					if (this.m_iCenterIndex == this.m_gCells.Length)
					{
						this.m_iCenterIndex = 0;
					}
					else if (this.m_iCenterIndex > this.m_gCells.Length)
					{
						this.m_iCenterIndex -= this.m_gCells.Length;
					}
					else if (this.m_iCenterIndex < 0)
					{
						this.m_iCenterIndex *= -1;
					}
					zero = new Vector3(localPosition.x, localPosition.y, localPosition.z);
					zero2 = new Vector3(localPosition.x, num9, localPosition.z);
				}
			}
			else
			{
				float num = this.m_v2MouseUpPosition.y;
				if (num == 0f)
				{
					num = 0.2f * this.m_UIInputManager.getDirMoveValue().y;
				}
				else if (num > this.m_fTouchUplimtValue)
				{
					num = this.m_fTouchUplimtValue;
				}
				else if (num < -this.m_fTouchUplimtValue)
				{
					num = -this.m_fTouchUplimtValue;
				}
				float num2 = localPosition.y + num * this.m_fSmoothSpeed;
				float num9 = (float)((int)(num2 / this.CellSize.x + 0.5f)) * this.CellSize.x;
				int num10 = (int)(num9 / (this.CellSize.x * (float)this.m_gCells.Length));
				int num11 = (int)((num9 - this.CellSize.x * (float)(this.m_gCells.Length * num10)) / this.CellSize.x);
				if (localPosition.x > 0f)
				{
					this.m_iCenterIndex = this.m_gCells.Length - num11;
				}
				else
				{
					this.m_iCenterIndex = num11 * -1;
				}
				if (this.m_iCenterIndex == this.m_gCells.Length)
				{
					this.m_iCenterIndex = 0;
				}
				else if (this.m_iCenterIndex > this.m_gCells.Length)
				{
					this.m_iCenterIndex -= this.m_gCells.Length;
				}
				else if (this.m_iCenterIndex < 0)
				{
					this.m_iCenterIndex *= -1;
				}
				zero = new Vector3(localPosition.x, localPosition.y, localPosition.z);
				zero2 = new Vector3(num9, localPosition.y, localPosition.z);
			}
		}
		else if (this.m_ScrollOption == UIScroll.ScrollOption_e.Rolling)
		{
			UIScroll.ScrollKind_e scrollKind_e = this.m_ScrollKind;
			if (scrollKind_e != UIScroll.ScrollKind_e.Horizontal)
			{
				if (scrollKind_e == UIScroll.ScrollKind_e.Vertical)
				{
					float num = this.m_v2MouseUpPosition.y;
					if (num == 0f)
					{
						num = 0.2f * this.m_UIInputManager.getDirMoveValue().y;
					}
					else if (num > this.m_fTouchUplimtValue)
					{
						num = this.m_fTouchUplimtValue;
					}
					else if (num < -this.m_fTouchUplimtValue)
					{
						num = -this.m_fTouchUplimtValue;
					}
					float num2 = localPosition.y + num * this.m_fSmoothSpeed;
					zero = new Vector3(localPosition.x, localPosition.y, localPosition.z);
					zero2 = new Vector3(localPosition.x, num2, localPosition.z);
				}
			}
			else
			{
				float num = this.m_v2MouseUpPosition.x;
				if (num == 0f)
				{
					num = 0.2f * this.m_UIInputManager.getDirMoveValue().x;
				}
				else if (num > this.m_fTouchUplimtValue)
				{
					num = this.m_fTouchUplimtValue;
				}
				else if (num < -this.m_fTouchUplimtValue)
				{
					num = -this.m_fTouchUplimtValue;
				}
				float num2 = localPosition.x + num * this.m_fSmoothSpeed;
				zero = new Vector3(localPosition.x, localPosition.y, localPosition.z);
				zero2 = new Vector3(num2, localPosition.y, localPosition.z);
			}
		}
		else if (this.m_ScrollOption == UIScroll.ScrollOption_e.RollingAndCenter)
		{
			UIScroll.ScrollKind_e scrollKind_e = this.m_ScrollKind;
			if (scrollKind_e != UIScroll.ScrollKind_e.Horizontal)
			{
				if (scrollKind_e == UIScroll.ScrollKind_e.Vertical)
				{
					float num = this.m_v2MouseUpPosition.y;
					if (num == 0f)
					{
						num = 0.2f * this.m_UIInputManager.getDirMoveValue().y;
					}
					else if (num > this.m_fTouchUplimtValue)
					{
						num = this.m_fTouchUplimtValue;
					}
					else if (num < -this.m_fTouchUplimtValue)
					{
						num = -this.m_fTouchUplimtValue;
					}
					float num2 = localPosition.y + num * this.m_fSmoothSpeed;
					float num12 = 1f;
					if (num2 < 0f)
					{
						num12 = -1f;
					}
					float num13 = (float)((int)(num2 / this.CellSize.y + 0.5f * num12)) * this.CellSize.y;
					int num14 = (int)(num13 / (this.CellSize.y * (float)this.m_gCells.Length));
					int num15 = (int)((num13 - this.CellSize.y * (float)(this.m_gCells.Length * num14)) / this.CellSize.y);
					if (localPosition.y > 0f)
					{
						this.m_iCenterIndex = this.m_gCells.Length - num15;
					}
					else
					{
						this.m_iCenterIndex = num15 * -1;
					}
					if (this.m_iCenterIndex == this.m_gCells.Length)
					{
						this.m_iCenterIndex = 0;
					}
					else if (this.m_iCenterIndex > this.m_gCells.Length)
					{
						this.m_iCenterIndex -= this.m_gCells.Length;
					}
					else if (this.m_iCenterIndex < 0)
					{
						this.m_iCenterIndex *= -1;
					}
					zero = new Vector3(localPosition.x, localPosition.y, localPosition.z);
					zero2 = new Vector3(localPosition.x, num13, localPosition.z);
				}
			}
			else
			{
				float num = this.m_v2MouseUpPosition.y;
				if (num == 0f)
				{
					num = 0.2f * this.m_UIInputManager.getDirMoveValue().y;
				}
				else if (num > this.m_fTouchUplimtValue)
				{
					num = this.m_fTouchUplimtValue;
				}
				else if (num < -this.m_fTouchUplimtValue)
				{
					num = -this.m_fTouchUplimtValue;
				}
				float num2 = localPosition.y + num * this.m_fSmoothSpeed;
				float num13 = (float)((int)(num2 / this.CellSize.x + 0.5f)) * this.CellSize.x;
				int num14 = (int)(num13 / (this.CellSize.x * (float)this.m_gCells.Length));
				int num15 = (int)((num13 - this.CellSize.x * (float)(this.m_gCells.Length * num14)) / this.CellSize.x);
				if (localPosition.x > 0f)
				{
					this.m_iCenterIndex = this.m_gCells.Length - num15;
				}
				else
				{
					this.m_iCenterIndex = num15 * -1;
				}
				if (this.m_iCenterIndex == this.m_gCells.Length)
				{
					this.m_iCenterIndex = 0;
				}
				else if (this.m_iCenterIndex > this.m_gCells.Length)
				{
					this.m_iCenterIndex -= this.m_gCells.Length;
				}
				else if (this.m_iCenterIndex < 0)
				{
					this.m_iCenterIndex *= -1;
				}
				zero = new Vector3(localPosition.x, localPosition.y, localPosition.z);
				zero2 = new Vector3(num13, localPosition.y, localPosition.z);
			}
		}
		this.m_TweenPos.from = zero;
		this.m_TweenPos.to = zero2;
		this.m_TweenPos.ResetToBeginning();
		this.m_TweenPos.Play(true);
		this.m_bIsDrag = false;
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x0006FCB0 File Offset: 0x0006DEB0
	private void RollingMoveCheck()
	{
		if (this.m_ScrollOption == UIScroll.ScrollOption_e.Normal || this.m_ScrollOption == UIScroll.ScrollOption_e.NormalCenter)
		{
			return;
		}
		Vector3 localPosition = base.transform.localPosition;
		UIScroll.ScrollKind_e scrollKind = this.m_ScrollKind;
		if (scrollKind != UIScroll.ScrollKind_e.Horizontal)
		{
			if (scrollKind == UIScroll.ScrollKind_e.Vertical)
			{
				for (int i = 0; i < this.Grid.transform.childCount; i++)
				{
					if (this.m_gCells.Length > i)
					{
						Vector3 vector = this.m_gCells[i].transform.localPosition + localPosition;
						if (vector.y < this.m_fLimitLeftPos)
						{
							int num = i - 1;
							if (i == 0)
							{
								num = this.m_gCells.Length - 1;
							}
							this.m_gCells[i].transform.localPosition = new Vector3(this.m_gCells[i].transform.localPosition.x, this.m_gCells[num].transform.localPosition.y + this.CellSize.y, this.m_gCells[i].transform.localPosition.z);
						}
						else if (vector.y > this.m_fLimitRightPos)
						{
							int num2 = i + 1;
							if (i == this.m_gCells.Length - 1)
							{
								num2 = 0;
							}
							this.m_gCells[i].transform.localPosition = new Vector3(this.m_gCells[i].transform.localPosition.x, this.m_gCells[num2].transform.localPosition.y - this.CellSize.y, this.m_gCells[i].transform.localPosition.z);
						}
					}
				}
			}
		}
		else
		{
			for (int j = 0; j < this.Grid.transform.childCount; j++)
			{
				Vector3 vector2 = this.m_gCells[j].transform.localPosition + localPosition;
				if (vector2.x < this.m_fLimitLeftPos)
				{
					int num3 = j - 1;
					if (j == 0)
					{
						num3 = this.m_gCells.Length - 1;
					}
					this.m_gCells[j].transform.localPosition = new Vector3(this.m_gCells[num3].transform.localPosition.x + this.CellSize.x, this.m_gCells[j].transform.localPosition.y, this.m_gCells[j].transform.localPosition.z);
				}
				else if (vector2.x > this.m_fLimitRightPos)
				{
					int num4 = j + 1;
					if (j == this.m_gCells.Length - 1)
					{
						num4 = 0;
					}
					this.m_gCells[j].transform.localPosition = new Vector3(this.m_gCells[num4].transform.localPosition.x - this.CellSize.x, this.m_gCells[j].transform.localPosition.y, this.m_gCells[j].transform.localPosition.z);
				}
			}
		}
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x0007001C File Offset: 0x0006E21C
	private void ViewOptimizationCheck()
	{
		if (!this.CellOptimization)
		{
			return;
		}
		if (UIInputManager.instance.isMouseState != UIInputManager.MouseState_e.None)
		{
			return;
		}
		if (this.m_ScrollKind == UIScroll.ScrollKind_e.Vertical)
		{
			for (int i = 0; i < this.m_gCells.Length; i++)
			{
				if (!(this.m_gCells[i] == null))
				{
					if (this.m_gCells[i].transform.localPosition.y * -1f < base.transform.localPosition.y - this.m_v2PanelSize.y)
					{
						this.m_gCells[i].SetActive(false);
					}
					else if (this.m_gCells[i].transform.localPosition.y * -1f > base.transform.localPosition.y + this.m_v2PanelSize.y)
					{
						this.m_gCells[i].SetActive(false);
					}
					else
					{
						this.m_gCells[i].SetActive(true);
					}
				}
			}
		}
		else if (this.m_ScrollKind == UIScroll.ScrollKind_e.Horizontal)
		{
			for (int j = 0; j < this.m_gCells.Length; j++)
			{
				if (!(this.m_gCells[j] == null))
				{
					if (this.m_gCells[j].transform.localPosition.x * -1f < base.transform.localPosition.x - this.m_v2PanelSize.x)
					{
						this.m_gCells[j].SetActive(false);
					}
					else if (this.m_gCells[j].transform.localPosition.x * -1f > base.transform.localPosition.x + this.m_v2PanelSize.x)
					{
						this.m_gCells[j].SetActive(false);
					}
					else
					{
						this.m_gCells[j].SetActive(true);
					}
				}
			}
		}
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x0000D437 File Offset: 0x0000B637
	private void Update()
	{
		this.DragUpdate();
		this.RollingMoveCheck();
		this.ProgressBarUpdate();
		this.ViewOptimizationCheck();
	}

	// Token: 0x0400111F RID: 4383
	public UIScroll.ScrollKind_e m_ScrollKind;

	// Token: 0x04001120 RID: 4384
	public UIScroll.ScrollOption_e m_ScrollOption;

	// Token: 0x04001121 RID: 4385
	public UIPanel m_Panel;

	// Token: 0x04001122 RID: 4386
	public GameObject Grid;

	// Token: 0x04001123 RID: 4387
	public UIProgressBar m_ProgressBar;

	// Token: 0x04001124 RID: 4388
	public Vector3 CellSize;

	// Token: 0x04001125 RID: 4389
	public float isSmoothSpeed = 1f;

	// Token: 0x04001126 RID: 4390
	public float isDragSpeed = 1f;

	// Token: 0x04001127 RID: 4391
	public bool CellOptimization;

	// Token: 0x04001128 RID: 4392
	[HideInInspector]
	public GameObject[] m_gCells;

	// Token: 0x04001129 RID: 4393
	[HideInInspector]
	public bool m_bIsDrag;

	// Token: 0x0400112A RID: 4394
	[HideInInspector]
	public int m_iCenterIndex;

	// Token: 0x0400112B RID: 4395
	[HideInInspector]
	public Vector2 m_v2MouseUpPosition = Vector2.zero;

	// Token: 0x0400112C RID: 4396
	[HideInInspector]
	public float m_fSmoothSpeed = 100f;

	// Token: 0x0400112D RID: 4397
	[HideInInspector]
	public float m_fTouchUplimtValue = 50f;

	// Token: 0x0400112E RID: 4398
	private UIInputManager m_UIInputManager;

	// Token: 0x0400112F RID: 4399
	private TweenPosition m_TweenPos;

	// Token: 0x04001130 RID: 4400
	private Vector2 m_v2PanelSize;

	// Token: 0x04001131 RID: 4401
	private Vector3 m_v3ProgressBarStartPos;

	// Token: 0x04001132 RID: 4402
	private int m_iProgressBarStartHeightSize;

	// Token: 0x04001133 RID: 4403
	private float m_fProgressBarValue;

	// Token: 0x04001134 RID: 4404
	private bool m_bProgressState;

	// Token: 0x04001135 RID: 4405
	private int m_iLeftCellCount;

	// Token: 0x04001136 RID: 4406
	private int m_iRightCellCount;

	// Token: 0x04001137 RID: 4407
	private float m_fLimitLeftPos;

	// Token: 0x04001138 RID: 4408
	private float m_fLimitRightPos;

	// Token: 0x04001139 RID: 4409
	private float m_ftabCount;

	// Token: 0x0400113A RID: 4410
	private float m_fDragSpeed = 2f;

	// Token: 0x0400113B RID: 4411
	private Vector2 m_v2SoftClipSize;

	// Token: 0x02000215 RID: 533
	public enum ScrollKind_e
	{
		// Token: 0x0400113D RID: 4413
		Horizontal,
		// Token: 0x0400113E RID: 4414
		Vertical,
		// Token: 0x0400113F RID: 4415
		None
	}

	// Token: 0x02000216 RID: 534
	public enum ScrollOption_e
	{
		// Token: 0x04001141 RID: 4417
		Normal,
		// Token: 0x04001142 RID: 4418
		NormalCenter,
		// Token: 0x04001143 RID: 4419
		Rolling,
		// Token: 0x04001144 RID: 4420
		RollingAndCenter
	}
}
