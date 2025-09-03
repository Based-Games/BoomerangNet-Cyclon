using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000AB RID: 171
[AddComponentMenu("NGUI/UI/Input Field")]
public class UIInput : MonoBehaviour
{
	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06000562 RID: 1378 RVA: 0x00007577 File Offset: 0x00005777
	// (set) Token: 0x06000563 RID: 1379 RVA: 0x0000757F File Offset: 0x0000577F
	public string defaultText
	{
		get
		{
			return this.mDefaultText;
		}
		set
		{
			this.mDefaultText = value;
		}
	}

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06000564 RID: 1380 RVA: 0x00007588 File Offset: 0x00005788
	// (set) Token: 0x06000565 RID: 1381 RVA: 0x00007590 File Offset: 0x00005790
	[Obsolete("Use UIInput.value instead")]
	public string text
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06000566 RID: 1382 RVA: 0x00007599 File Offset: 0x00005799
	// (set) Token: 0x06000567 RID: 1383 RVA: 0x0002BBC0 File Offset: 0x00029DC0
	public string value
	{
		get
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			if (this.isSelected && UIInput.mEditor != null)
			{
				return UIInput.mEditor.content.text;
			}
			return this.mValue;
		}
		set
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			UIInput.mDrawStart = 0;
			UIInput.mDrawEnd = 0;
			if (this.isSelected && UIInput.mEditor != null && UIInput.mEditor.content.text != value)
			{
				UIInput.mEditor.content.text = value;
				this.UpdateLabel();
				this.ExecuteOnChange();
				return;
			}
			if (this.value != value)
			{
				this.mValue = value;
				if (this.isSelected && UIInput.mEditor != null)
				{
					UIInput.mEditor.content.text = value;
					UIInput.mEditor.OnLostFocus();
					UIInput.mEditor.OnFocus();
					UIInput.mEditor.MoveTextEnd();
				}
				this.SaveToPlayerPrefs(this.mValue);
				this.UpdateLabel();
				this.ExecuteOnChange();
			}
		}
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x06000568 RID: 1384 RVA: 0x000075D7 File Offset: 0x000057D7
	protected bool needsTextCursor
	{
		get
		{
			return this.isSelected;
		}
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06000569 RID: 1385 RVA: 0x000075D7 File Offset: 0x000057D7
	// (set) Token: 0x0600056A RID: 1386 RVA: 0x000075DF File Offset: 0x000057DF
	[Obsolete("Use UIInput.isSelected instead")]
	public bool selected
	{
		get
		{
			return this.isSelected;
		}
		set
		{
			this.isSelected = value;
		}
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x0600056B RID: 1387 RVA: 0x000075E8 File Offset: 0x000057E8
	// (set) Token: 0x0600056C RID: 1388 RVA: 0x000075F5 File Offset: 0x000057F5
	public bool isSelected
	{
		get
		{
			return UIInput.selection == this;
		}
		set
		{
			if (!value)
			{
				if (this.isSelected)
				{
					UICamera.selectedObject = null;
				}
			}
			else
			{
				UICamera.selectedObject = base.gameObject;
			}
		}
	}

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x0600056D RID: 1389 RVA: 0x0000761E File Offset: 0x0000581E
	protected int cursorPosition
	{
		get
		{
			return (!this.isSelected || UIInput.mEditor == null) ? this.value.Length : UIInput.mEditor.selectPos;
		}
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x0002BCAC File Offset: 0x00029EAC
	private void Start()
	{
		if (string.IsNullOrEmpty(this.mValue))
		{
			if (!string.IsNullOrEmpty(this.savedAs) && PlayerPrefs.HasKey(this.savedAs))
			{
				this.value = PlayerPrefs.GetString(this.savedAs);
			}
		}
		else
		{
			this.value = this.mValue;
		}
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x0002BD0C File Offset: 0x00029F0C
	protected void Init()
	{
		if (this.mDoInit && this.label != null)
		{
			this.mDoInit = false;
			this.mDefaultText = this.label.text;
			this.mDefaultColor = this.label.color;
			this.label.supportEncoding = false;
			this.mPivot = this.label.pivot;
			this.mPosition = this.label.cachedTransform.localPosition.x;
			this.UpdateLabel();
		}
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x0000764F File Offset: 0x0000584F
	protected void SaveToPlayerPrefs(string val)
	{
		if (!string.IsNullOrEmpty(this.savedAs))
		{
			if (string.IsNullOrEmpty(val))
			{
				PlayerPrefs.DeleteKey(this.savedAs);
			}
			else
			{
				PlayerPrefs.SetString(this.savedAs, val);
			}
		}
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x00007688 File Offset: 0x00005888
	protected virtual void OnSelect(bool isSelected)
	{
		if (isSelected)
		{
			this.OnSelectEvent();
		}
		else
		{
			this.OnDeselectEvent();
		}
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x0002BDA0 File Offset: 0x00029FA0
	protected void OnSelectEvent()
	{
		UIInput.selection = this;
		if (this.mDoInit)
		{
			this.Init();
		}
		if (this.label != null && NGUITools.GetActive(this))
		{
			this.label.color = this.activeTextColor;
			Input.imeCompositionMode = IMECompositionMode.On;
			Input.compositionCursorPos = ((!(UICamera.current != null) || !(UICamera.current.cachedCamera != null)) ? this.label.worldCorners[0] : UICamera.current.cachedCamera.WorldToScreenPoint(this.label.worldCorners[0]));
			UIInput.mEditor = new TextEditor();
			UIInput.mEditor.content = new GUIContent(this.mValue);
			UIInput.mEditor.OnFocus();
			UIInput.mEditor.MoveTextEnd();
			UIInput.mDrawStart = 0;
			UIInput.mDrawEnd = 0;
			this.UpdateLabel();
		}
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x0002BEAC File Offset: 0x0002A0AC
	protected void OnDeselectEvent()
	{
		if (this.mDoInit)
		{
			this.Init();
		}
		if (this.label != null && NGUITools.GetActive(this))
		{
			this.mValue = this.value;
			UIInput.mEditor = null;
			if (string.IsNullOrEmpty(this.mValue))
			{
				this.label.text = this.mDefaultText;
				this.label.color = this.mDefaultColor;
			}
			else
			{
				this.label.text = this.mValue;
			}
			Input.imeCompositionMode = IMECompositionMode.Off;
			this.RestoreLabelPivot();
		}
		UIInput.selection = null;
		this.UpdateLabel();
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x0002BF58 File Offset: 0x0002A158
	private void Update()
	{
		if (this.isSelected && NGUITools.GetActive(this))
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			if (this.selectOnTab != null && Input.GetKeyDown(KeyCode.Tab))
			{
				UICamera.selectedObject = this.selectOnTab;
				return;
			}
			this.Append(Input.inputString);
			if (UIInput.mLastIME != Input.compositionString)
			{
				UIInput.mLastIME = Input.compositionString;
				this.UpdateLabel();
				this.ExecuteOnChange();
			}
		}
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x000076A1 File Offset: 0x000058A1
	private void OnGUI()
	{
		if (this.isSelected && Event.current.rawType == EventType.KeyDown)
		{
			this.ProcessEvent(Event.current);
		}
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0002BFEC File Offset: 0x0002A1EC
	private bool ProcessEvent(Event ev)
	{
		RuntimePlatform platform = Application.platform;
		bool flag = platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer || platform == RuntimePlatform.OSXWebPlayer;
		bool flag2 = ((!flag) ? (ev.modifiers == EventModifiers.Control) : (ev.modifiers == EventModifiers.Command));
		KeyCode keyCode = ev.keyCode;
		switch (keyCode)
		{
		case KeyCode.KeypadEnter:
			break;
		default:
			switch (keyCode)
			{
			case KeyCode.V:
				if (flag2)
				{
					ev.Use();
					this.Append(NGUITools.clipboard);
				}
				return true;
			default:
				if (keyCode == KeyCode.Backspace)
				{
					ev.Use();
					UIInput.mEditor.Backspace();
					this.UpdateLabel();
					this.ExecuteOnChange();
					return true;
				}
				if (keyCode != KeyCode.Return)
				{
					if (keyCode == KeyCode.C)
					{
						if (flag2)
						{
							ev.Use();
							NGUITools.clipboard = this.value;
						}
						return true;
					}
					if (keyCode != KeyCode.Delete)
					{
						return false;
					}
					ev.Use();
					UIInput.mEditor.Delete();
					this.UpdateLabel();
					this.ExecuteOnChange();
					return true;
				}
				break;
			case KeyCode.X:
				if (flag2)
				{
					ev.Use();
					NGUITools.clipboard = this.value;
					this.value = string.Empty;
				}
				return true;
			}
			break;
		case KeyCode.UpArrow:
		case KeyCode.Home:
			ev.Use();
			UIInput.mEditor.MoveTextStart();
			this.UpdateLabel();
			return true;
		case KeyCode.DownArrow:
		case KeyCode.End:
			ev.Use();
			UIInput.mEditor.MoveTextEnd();
			this.UpdateLabel();
			return true;
		case KeyCode.RightArrow:
			ev.Use();
			UIInput.mEditor.MoveRight();
			this.UpdateLabel();
			return true;
		case KeyCode.LeftArrow:
			ev.Use();
			UIInput.mEditor.MoveLeft();
			this.UpdateLabel();
			return true;
		}
		ev.Use();
		if (flag2 && this.label != null && this.label.overflowMethod != UILabel.Overflow.ClampContent)
		{
			char c = '\n';
			if (this.onValidate != null)
			{
				c = this.onValidate(UIInput.mEditor.content.text, UIInput.mEditor.selectPos, c);
			}
			else if (this.validation != UIInput.Validation.None)
			{
				c = this.Validate(UIInput.mEditor.content.text, UIInput.mEditor.selectPos, c);
			}
			if (c != '\0')
			{
				UIInput.mEditor.Insert(c);
				this.UpdateLabel();
				this.ExecuteOnChange();
			}
		}
		else
		{
			UICamera.currentKey = ev.keyCode;
			this.Submit();
			UICamera.currentKey = KeyCode.None;
			this.isSelected = false;
			this.UpdateLabel();
			this.ExecuteOnChange();
		}
		return true;
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x000076CA File Offset: 0x000058CA
	protected void Submit()
	{
		if (NGUITools.GetActive(this))
		{
			UIInput.current = this;
			this.mValue = this.value;
			EventDelegate.Execute(this.onSubmit);
			this.SaveToPlayerPrefs(this.mValue);
			UIInput.current = null;
		}
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x0002C280 File Offset: 0x0002A480
	protected virtual void Append(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return;
		}
		int i = 0;
		int length = input.Length;
		while (i < length)
		{
			char c = input[i];
			if (c >= ' ')
			{
				if (this.characterLimit <= 0 || UIInput.mEditor.content.text.Length < this.characterLimit)
				{
					if (this.onValidate != null)
					{
						c = this.onValidate(UIInput.mEditor.content.text, UIInput.mEditor.selectPos, c);
					}
					else if (this.validation != UIInput.Validation.None)
					{
						c = this.Validate(UIInput.mEditor.content.text, UIInput.mEditor.selectPos, c);
					}
					if (c != '\0')
					{
						UIInput.mEditor.Insert(c);
					}
				}
			}
			i++;
		}
		this.UpdateLabel();
		this.ExecuteOnChange();
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x0002C378 File Offset: 0x0002A578
	protected void UpdateLabel()
	{
		if (this.label != null)
		{
			if (this.mDoInit)
			{
				this.Init();
			}
			bool isSelected = this.isSelected;
			string value = this.value;
			bool flag = string.IsNullOrEmpty(value);
			string text;
			if (flag)
			{
				text = ((!isSelected) ? this.mDefaultText : ((!this.needsTextCursor) ? string.Empty : "|"));
				this.RestoreLabelPivot();
			}
			else
			{
				if (this.inputType == UIInput.InputType.Password)
				{
					text = string.Empty;
					int i = 0;
					int length = value.Length;
					while (i < length)
					{
						text += "*";
						i++;
					}
				}
				else
				{
					text = value;
				}
				int num = ((!isSelected) ? 0 : Mathf.Min(text.Length, this.cursorPosition));
				string text2 = text.Substring(0, num);
				if (isSelected)
				{
					text2 += Input.compositionString;
					if (this.needsTextCursor)
					{
						text2 += "|";
					}
				}
				text = text2 + text.Substring(num, text.Length - num);
				if (this.label.overflowMethod == UILabel.Overflow.ClampContent)
				{
					if (isSelected)
					{
						if (UIInput.mDrawEnd == 0)
						{
							UIInput.mDrawEnd = num;
						}
						string text3 = text.Substring(0, Mathf.Min(UIInput.mDrawEnd, text.Length));
						int num2 = this.label.CalculateOffsetToFit(text3);
						if (num < num2 || num >= UIInput.mDrawEnd)
						{
							num2 = this.label.CalculateOffsetToFit(text2);
							UIInput.mDrawStart = num2;
							UIInput.mDrawEnd = text2.Length;
						}
						else if (num2 != UIInput.mDrawStart)
						{
							UIInput.mDrawStart = num2;
						}
					}
					if (UIInput.mDrawStart != 0)
					{
						text = text.Substring(UIInput.mDrawStart, text.Length - UIInput.mDrawStart);
						if (this.mPivot == UIWidget.Pivot.Left)
						{
							this.label.pivot = UIWidget.Pivot.Right;
						}
						else if (this.mPivot == UIWidget.Pivot.TopLeft)
						{
							this.label.pivot = UIWidget.Pivot.TopRight;
						}
						else if (this.mPivot == UIWidget.Pivot.BottomLeft)
						{
							this.label.pivot = UIWidget.Pivot.BottomRight;
						}
					}
					else
					{
						this.RestoreLabelPivot();
					}
				}
				else
				{
					this.RestoreLabelPivot();
				}
			}
			this.label.text = text;
			this.label.color = ((!flag || isSelected) ? this.activeTextColor : this.mDefaultColor);
		}
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x00007706 File Offset: 0x00005906
	protected void RestoreLabelPivot()
	{
		if (this.label != null && this.label.pivot != this.mPivot)
		{
			this.label.pivot = this.mPivot;
		}
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x0002C60C File Offset: 0x0002A80C
	protected char Validate(string text, int pos, char ch)
	{
		if (this.validation == UIInput.Validation.None || !base.enabled)
		{
			return ch;
		}
		if (this.validation == UIInput.Validation.Integer)
		{
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
			if (ch == '-' && pos == 0 && !text.Contains("-"))
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Float)
		{
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
			if (ch == '-' && pos == 0 && !text.Contains("-"))
			{
				return ch;
			}
			if (ch == '.' && !text.Contains("."))
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Alphanumeric)
		{
			if (ch >= 'A' && ch <= 'Z')
			{
				return ch;
			}
			if (ch >= 'a' && ch <= 'z')
			{
				return ch;
			}
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Username)
		{
			if (ch >= 'A' && ch <= 'Z')
			{
				return ch - 'A' + 'a';
			}
			if (ch >= 'a' && ch <= 'z')
			{
				return ch;
			}
			if (ch >= '0' && ch <= '9')
			{
				return ch;
			}
		}
		else if (this.validation == UIInput.Validation.Name)
		{
			char c = ((text.Length <= 0) ? ' ' : text[Mathf.Clamp(pos, 0, text.Length - 1)]);
			char c2 = ((text.Length <= 0) ? '\n' : text[Mathf.Clamp(pos + 1, 0, text.Length - 1)]);
			if (ch >= 'a' && ch <= 'z')
			{
				if (c == ' ')
				{
					return ch - 'a' + 'A';
				}
				return ch;
			}
			else if (ch >= 'A' && ch <= 'Z')
			{
				if (c != ' ' && c != '\'')
				{
					return ch - 'A' + 'a';
				}
				return ch;
			}
			else if (ch == '\'')
			{
				if (c != ' ' && c != '\'' && c2 != '\'' && !text.Contains("'"))
				{
					return ch;
				}
			}
			else if (ch == ' ' && c != ' ' && c != '\'' && c2 != ' ' && c2 != '\'')
			{
				return ch;
			}
		}
		return '\0';
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x00007740 File Offset: 0x00005940
	protected void ExecuteOnChange()
	{
		if (EventDelegate.IsValid(this.onChange))
		{
			UIInput.current = this;
			EventDelegate.Execute(this.onChange);
			UIInput.current = null;
		}
	}

	// Token: 0x04000426 RID: 1062
	public static UIInput current;

	// Token: 0x04000427 RID: 1063
	public static UIInput selection;

	// Token: 0x04000428 RID: 1064
	public UILabel label;

	// Token: 0x04000429 RID: 1065
	public UIInput.InputType inputType;

	// Token: 0x0400042A RID: 1066
	public UIInput.KeyboardType keyboardType;

	// Token: 0x0400042B RID: 1067
	public UIInput.Validation validation;

	// Token: 0x0400042C RID: 1068
	public int characterLimit;

	// Token: 0x0400042D RID: 1069
	public string savedAs;

	// Token: 0x0400042E RID: 1070
	public GameObject selectOnTab;

	// Token: 0x0400042F RID: 1071
	public Color activeTextColor = Color.white;

	// Token: 0x04000430 RID: 1072
	public List<EventDelegate> onSubmit = new List<EventDelegate>();

	// Token: 0x04000431 RID: 1073
	public List<EventDelegate> onChange = new List<EventDelegate>();

	// Token: 0x04000432 RID: 1074
	public UIInput.OnValidate onValidate;

	// Token: 0x04000433 RID: 1075
	[HideInInspector]
	[SerializeField]
	protected string mValue;

	// Token: 0x04000434 RID: 1076
	protected string mDefaultText = string.Empty;

	// Token: 0x04000435 RID: 1077
	protected Color mDefaultColor = Color.white;

	// Token: 0x04000436 RID: 1078
	protected float mPosition;

	// Token: 0x04000437 RID: 1079
	protected bool mDoInit = true;

	// Token: 0x04000438 RID: 1080
	protected UIWidget.Pivot mPivot;

	// Token: 0x04000439 RID: 1081
	protected static int mDrawStart;

	// Token: 0x0400043A RID: 1082
	protected static int mDrawEnd;

	// Token: 0x0400043B RID: 1083
	protected static string mLastIME = string.Empty;

	// Token: 0x0400043C RID: 1084
	protected static TextEditor mEditor;

	// Token: 0x020000AC RID: 172
	public enum InputType
	{
		// Token: 0x0400043E RID: 1086
		Standard,
		// Token: 0x0400043F RID: 1087
		AutoCorrect,
		// Token: 0x04000440 RID: 1088
		Password
	}

	// Token: 0x020000AD RID: 173
	public enum Validation
	{
		// Token: 0x04000442 RID: 1090
		None,
		// Token: 0x04000443 RID: 1091
		Integer,
		// Token: 0x04000444 RID: 1092
		Float,
		// Token: 0x04000445 RID: 1093
		Alphanumeric,
		// Token: 0x04000446 RID: 1094
		Username,
		// Token: 0x04000447 RID: 1095
		Name
	}

	// Token: 0x020000AE RID: 174
	public enum KeyboardType
	{
		// Token: 0x04000449 RID: 1097
		Default,
		// Token: 0x0400044A RID: 1098
		ASCIICapable,
		// Token: 0x0400044B RID: 1099
		NumbersAndPunctuation,
		// Token: 0x0400044C RID: 1100
		URL,
		// Token: 0x0400044D RID: 1101
		NumberPad,
		// Token: 0x0400044E RID: 1102
		PhonePad,
		// Token: 0x0400044F RID: 1103
		NamePhonePad,
		// Token: 0x04000450 RID: 1104
		EmailAddress
	}

	// Token: 0x020000AF RID: 175
	// (Invoke) Token: 0x0600057E RID: 1406
	public delegate char OnValidate(string text, int charIndex, char addedChar);
}
