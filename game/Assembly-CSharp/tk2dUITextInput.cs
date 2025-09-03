using System;
using UnityEngine;

// Token: 0x020002A5 RID: 677
[AddComponentMenu("2D Toolkit/UI/tk2dUITextInput")]
[ExecuteInEditMode]
public class tk2dUITextInput : MonoBehaviour
{
	// Token: 0x170002F8 RID: 760
	// (get) Token: 0x060013A9 RID: 5033 RVA: 0x00010EEC File Offset: 0x0000F0EC
	// (set) Token: 0x060013AA RID: 5034 RVA: 0x000871A8 File Offset: 0x000853A8
	public tk2dUILayout LayoutItem
	{
		get
		{
			return this.layoutItem;
		}
		set
		{
			if (this.layoutItem != value)
			{
				if (this.layoutItem != null)
				{
					this.layoutItem.OnReshape -= this.LayoutReshaped;
				}
				this.layoutItem = value;
				if (this.layoutItem != null)
				{
					this.layoutItem.OnReshape += this.LayoutReshaped;
				}
			}
		}
	}

	// Token: 0x170002F9 RID: 761
	// (get) Token: 0x060013AB RID: 5035 RVA: 0x00010EF4 File Offset: 0x0000F0F4
	// (set) Token: 0x060013AC RID: 5036 RVA: 0x00010F14 File Offset: 0x0000F114
	public GameObject SendMessageTarget
	{
		get
		{
			if (this.selectionBtn != null)
			{
				return this.selectionBtn.sendMessageTarget;
			}
			return null;
		}
		set
		{
			if (this.selectionBtn != null && this.selectionBtn.sendMessageTarget != value)
			{
				this.selectionBtn.sendMessageTarget = value;
			}
		}
	}

	// Token: 0x170002FA RID: 762
	// (get) Token: 0x060013AD RID: 5037 RVA: 0x00010F49 File Offset: 0x0000F149
	public bool IsFocus
	{
		get
		{
			return this.isSelected;
		}
	}

	// Token: 0x170002FB RID: 763
	// (get) Token: 0x060013AE RID: 5038 RVA: 0x00010F51 File Offset: 0x0000F151
	// (set) Token: 0x060013AF RID: 5039 RVA: 0x00087220 File Offset: 0x00085420
	public string Text
	{
		get
		{
			return this.text;
		}
		set
		{
			if (this.text != value)
			{
				this.text = value;
				if (this.text.Length > this.maxCharacterLength)
				{
					this.text = this.text.Substring(0, this.maxCharacterLength);
				}
				this.FormatTextForDisplay(this.text);
				if (this.isSelected)
				{
					this.SetCursorPosition();
				}
			}
		}
	}

	// Token: 0x060013B0 RID: 5040 RVA: 0x00010F59 File Offset: 0x0000F159
	private void Awake()
	{
		this.SetState();
		this.ShowDisplayText();
	}

	// Token: 0x060013B1 RID: 5041 RVA: 0x00010F67 File Offset: 0x0000F167
	private void Start()
	{
		this.wasStartedCalled = true;
		if (tk2dUIManager.Instance__NoCreate != null)
		{
			tk2dUIManager.Instance.OnAnyPress += this.AnyPress;
		}
		this.wasOnAnyPressEventAttached = true;
	}

	// Token: 0x060013B2 RID: 5042 RVA: 0x00087290 File Offset: 0x00085490
	private void OnEnable()
	{
		if (this.wasStartedCalled && !this.wasOnAnyPressEventAttached && tk2dUIManager.Instance__NoCreate != null)
		{
			tk2dUIManager.Instance.OnAnyPress += this.AnyPress;
		}
		if (this.layoutItem != null)
		{
			this.layoutItem.OnReshape += this.LayoutReshaped;
		}
		this.selectionBtn.OnClick += this.InputSelected;
	}

	// Token: 0x060013B3 RID: 5043 RVA: 0x00087318 File Offset: 0x00085518
	private void OnDisable()
	{
		if (tk2dUIManager.Instance__NoCreate != null)
		{
			tk2dUIManager.Instance.OnAnyPress -= this.AnyPress;
			if (this.listenForKeyboardText)
			{
				tk2dUIManager.Instance.OnInputUpdate -= this.ListenForKeyboardTextUpdate;
			}
		}
		this.wasOnAnyPressEventAttached = false;
		this.selectionBtn.OnClick -= this.InputSelected;
		this.listenForKeyboardText = false;
		if (this.layoutItem != null)
		{
			this.layoutItem.OnReshape -= this.LayoutReshaped;
		}
	}

	// Token: 0x060013B4 RID: 5044 RVA: 0x00010F9D File Offset: 0x0000F19D
	public void SetFocus()
	{
		if (!this.IsFocus)
		{
			this.InputSelected();
		}
	}

	// Token: 0x060013B5 RID: 5045 RVA: 0x000873BC File Offset: 0x000855BC
	private void FormatTextForDisplay(string modifiedText)
	{
		if (this.isPasswordField)
		{
			int length = modifiedText.Length;
			char c = ((this.passwordChar.Length <= 0) ? '*' : this.passwordChar[0]);
			modifiedText = string.Empty;
			modifiedText = modifiedText.PadRight(length, c);
		}
		this.inputLabel.text = modifiedText;
		this.inputLabel.Commit();
		while (this.inputLabel.renderer.bounds.extents.x * 2f > this.fieldLength)
		{
			modifiedText = modifiedText.Substring(1, modifiedText.Length - 1);
			this.inputLabel.text = modifiedText;
			this.inputLabel.Commit();
		}
		if (modifiedText.Length == 0 && !this.listenForKeyboardText)
		{
			this.ShowDisplayText();
		}
		else
		{
			this.HideDisplayText();
		}
	}

	// Token: 0x060013B6 RID: 5046 RVA: 0x000874B0 File Offset: 0x000856B0
	private void ListenForKeyboardTextUpdate()
	{
		bool flag = false;
		string text = this.text;
		foreach (char c in Input.inputString)
		{
			if (c == "\b"[0])
			{
				if (this.text.Length != 0)
				{
					text = this.text.Substring(0, this.text.Length - 1);
					flag = true;
				}
			}
			else if (c != "\n"[0] && c != "\r"[0])
			{
				if (c != '\t' && c != '\u001b')
				{
					text += c;
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.Text = text;
			if (this.OnTextChange != null)
			{
				this.OnTextChange(this);
			}
			if (this.SendMessageTarget != null && this.SendMessageOnTextChangeMethodName.Length > 0)
			{
				this.SendMessageTarget.SendMessage(this.SendMessageOnTextChangeMethodName, this, SendMessageOptions.RequireReceiver);
			}
		}
	}

	// Token: 0x060013B7 RID: 5047 RVA: 0x000875D4 File Offset: 0x000857D4
	private void InputSelected()
	{
		if (this.text.Length == 0)
		{
			this.HideDisplayText();
		}
		this.isSelected = true;
		if (!this.listenForKeyboardText)
		{
			tk2dUIManager.Instance.OnInputUpdate += this.ListenForKeyboardTextUpdate;
		}
		this.listenForKeyboardText = true;
		this.SetState();
		this.SetCursorPosition();
	}

	// Token: 0x060013B8 RID: 5048 RVA: 0x00087634 File Offset: 0x00085834
	private void InputDeselected()
	{
		if (this.text.Length == 0)
		{
			this.ShowDisplayText();
		}
		this.isSelected = false;
		if (this.listenForKeyboardText)
		{
			tk2dUIManager.Instance.OnInputUpdate -= this.ListenForKeyboardTextUpdate;
		}
		this.listenForKeyboardText = false;
		this.SetState();
	}

	// Token: 0x060013B9 RID: 5049 RVA: 0x00010FB0 File Offset: 0x0000F1B0
	private void AnyPress()
	{
		if (this.isSelected && tk2dUIManager.Instance.PressedUIItem != this.selectionBtn)
		{
			this.InputDeselected();
		}
	}

	// Token: 0x060013BA RID: 5050 RVA: 0x00010FDD File Offset: 0x0000F1DD
	private void SetState()
	{
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.unSelectedStateGO, !this.isSelected);
		tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.selectedStateGO, this.isSelected);
		tk2dUIBaseItemControl.ChangeGameObjectActiveState(this.cursor, this.isSelected);
	}

	// Token: 0x060013BB RID: 5051 RVA: 0x0008768C File Offset: 0x0008588C
	private void SetCursorPosition()
	{
		float num = 1f;
		float num2 = 0.002f;
		if (this.inputLabel.anchor == TextAnchor.MiddleLeft || this.inputLabel.anchor == TextAnchor.LowerLeft || this.inputLabel.anchor == TextAnchor.UpperLeft)
		{
			num = 2f;
		}
		else if (this.inputLabel.anchor == TextAnchor.MiddleRight || this.inputLabel.anchor == TextAnchor.LowerRight || this.inputLabel.anchor == TextAnchor.UpperRight)
		{
			num = -2f;
			num2 = 0.012f;
		}
		if (this.text.EndsWith(" "))
		{
			tk2dFontChar tk2dFontChar;
			if (this.inputLabel.font.useDictionary)
			{
				tk2dFontChar = this.inputLabel.font.charDict[32];
			}
			else
			{
				tk2dFontChar = this.inputLabel.font.chars[32];
			}
			num2 += tk2dFontChar.advance * this.inputLabel.scale.x / 2f;
		}
		this.cursor.transform.localPosition = new Vector3(this.inputLabel.transform.localPosition.x + (this.inputLabel.renderer.bounds.extents.x + num2) * num, this.cursor.transform.localPosition.y, this.cursor.transform.localPosition.z);
	}

	// Token: 0x060013BC RID: 5052 RVA: 0x00087828 File Offset: 0x00085A28
	private void ShowDisplayText()
	{
		if (!this.isDisplayTextShown)
		{
			this.isDisplayTextShown = true;
			if (this.emptyDisplayLabel != null)
			{
				this.emptyDisplayLabel.text = this.emptyDisplayText;
				this.emptyDisplayLabel.Commit();
				tk2dUIBaseItemControl.ChangeGameObjectActiveState(this.emptyDisplayLabel.gameObject, true);
			}
			tk2dUIBaseItemControl.ChangeGameObjectActiveState(this.inputLabel.gameObject, false);
		}
	}

	// Token: 0x060013BD RID: 5053 RVA: 0x00011015 File Offset: 0x0000F215
	private void HideDisplayText()
	{
		if (this.isDisplayTextShown)
		{
			this.isDisplayTextShown = false;
			tk2dUIBaseItemControl.ChangeGameObjectActiveStateWithNullCheck(this.emptyDisplayLabel.gameObject, false);
			tk2dUIBaseItemControl.ChangeGameObjectActiveState(this.inputLabel.gameObject, true);
		}
	}

	// Token: 0x060013BE RID: 5054 RVA: 0x00087898 File Offset: 0x00085A98
	private void LayoutReshaped(Vector3 dMin, Vector3 dMax)
	{
		this.fieldLength += dMax.x - dMin.x;
		string text = this.text;
		this.text = string.Empty;
		this.Text = text;
	}

	// Token: 0x04001543 RID: 5443
	public tk2dUIItem selectionBtn;

	// Token: 0x04001544 RID: 5444
	public tk2dTextMesh inputLabel;

	// Token: 0x04001545 RID: 5445
	public tk2dTextMesh emptyDisplayLabel;

	// Token: 0x04001546 RID: 5446
	public GameObject unSelectedStateGO;

	// Token: 0x04001547 RID: 5447
	public GameObject selectedStateGO;

	// Token: 0x04001548 RID: 5448
	public GameObject cursor;

	// Token: 0x04001549 RID: 5449
	public float fieldLength = 1f;

	// Token: 0x0400154A RID: 5450
	public int maxCharacterLength = 30;

	// Token: 0x0400154B RID: 5451
	public string emptyDisplayText;

	// Token: 0x0400154C RID: 5452
	public bool isPasswordField;

	// Token: 0x0400154D RID: 5453
	public string passwordChar = "*";

	// Token: 0x0400154E RID: 5454
	[HideInInspector]
	[SerializeField]
	private tk2dUILayout layoutItem;

	// Token: 0x0400154F RID: 5455
	private bool isSelected;

	// Token: 0x04001550 RID: 5456
	private bool wasStartedCalled;

	// Token: 0x04001551 RID: 5457
	private bool wasOnAnyPressEventAttached;

	// Token: 0x04001552 RID: 5458
	private bool listenForKeyboardText;

	// Token: 0x04001553 RID: 5459
	private bool isDisplayTextShown;

	// Token: 0x04001554 RID: 5460
	public Action<tk2dUITextInput> OnTextChange;

	// Token: 0x04001555 RID: 5461
	public string SendMessageOnTextChangeMethodName = string.Empty;

	// Token: 0x04001556 RID: 5462
	private string text = string.Empty;
}
