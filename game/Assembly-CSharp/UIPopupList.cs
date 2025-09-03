using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004A RID: 74
[AddComponentMenu("NGUI/Interaction/Popup List")]
[ExecuteInEditMode]
public class UIPopupList : UIWidgetContainer
{
	// Token: 0x17000032 RID: 50
	// (get) Token: 0x060001C1 RID: 449 RVA: 0x00004D5C File Offset: 0x00002F5C
	// (set) Token: 0x060001C2 RID: 450 RVA: 0x000191B8 File Offset: 0x000173B8
	public UnityEngine.Object ambigiousFont
	{
		get
		{
			if (this.trueTypeFont != null)
			{
				return this.trueTypeFont;
			}
			if (this.bitmapFont != null)
			{
				return this.bitmapFont;
			}
			return this.font;
		}
		set
		{
			if (value is Font)
			{
				this.trueTypeFont = value as Font;
				this.bitmapFont = null;
				this.font = null;
			}
			else if (value is UIFont)
			{
				this.bitmapFont = value as UIFont;
				this.trueTypeFont = null;
				this.font = null;
			}
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x060001C3 RID: 451 RVA: 0x00004D94 File Offset: 0x00002F94
	// (set) Token: 0x060001C4 RID: 452 RVA: 0x00004D9C File Offset: 0x00002F9C
	[Obsolete("Use EventDelegate.Add(popup.onChange, YourCallback) instead, and UIPopupList.current.value to determine the state")]
	public UIPopupList.LegacyEvent onSelectionChange
	{
		get
		{
			return this.mLegacyEvent;
		}
		set
		{
			this.mLegacyEvent = value;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x060001C5 RID: 453 RVA: 0x00004DA5 File Offset: 0x00002FA5
	public bool isOpen
	{
		get
		{
			return this.mChild != null;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x060001C6 RID: 454 RVA: 0x00004DB3 File Offset: 0x00002FB3
	// (set) Token: 0x060001C7 RID: 455 RVA: 0x00004DBB File Offset: 0x00002FBB
	public string value
	{
		get
		{
			return this.mSelectedItem;
		}
		set
		{
			this.mSelectedItem = value;
			if (this.mSelectedItem == null)
			{
				return;
			}
			if (this.mSelectedItem != null)
			{
				this.TriggerCallbacks();
			}
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x060001C8 RID: 456 RVA: 0x00004DE1 File Offset: 0x00002FE1
	// (set) Token: 0x060001C9 RID: 457 RVA: 0x00004DE9 File Offset: 0x00002FE9
	[Obsolete("Use 'value' instead")]
	public string selection
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

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x060001CA RID: 458 RVA: 0x00019214 File Offset: 0x00017414
	// (set) Token: 0x060001CB RID: 459 RVA: 0x00019240 File Offset: 0x00017440
	private bool handleEvents
	{
		get
		{
			UIButtonKeys component = base.GetComponent<UIButtonKeys>();
			return component == null || !component.enabled;
		}
		set
		{
			UIButtonKeys component = base.GetComponent<UIButtonKeys>();
			if (component != null)
			{
				component.enabled = !value;
			}
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x060001CC RID: 460 RVA: 0x00004DF2 File Offset: 0x00002FF2
	private bool isValid
	{
		get
		{
			return this.bitmapFont != null || this.trueTypeFont != null;
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x060001CD RID: 461 RVA: 0x00004E14 File Offset: 0x00003014
	private int activeFontSize
	{
		get
		{
			return (!(this.trueTypeFont != null) && !(this.bitmapFont == null)) ? this.bitmapFont.defaultSize : this.fontSize;
		}
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x060001CE RID: 462 RVA: 0x0001926C File Offset: 0x0001746C
	private float activeFontScale
	{
		get
		{
			return (!(this.trueTypeFont != null) && !(this.bitmapFont == null)) ? ((float)this.fontSize / (float)this.bitmapFont.defaultSize) : 1f;
		}
	}

	// Token: 0x060001CF RID: 463 RVA: 0x000192BC File Offset: 0x000174BC
	protected void TriggerCallbacks()
	{
		UIPopupList.current = this;
		if (this.mLegacyEvent != null)
		{
			this.mLegacyEvent(this.mSelectedItem);
		}
		if (EventDelegate.IsValid(this.onChange))
		{
			EventDelegate.Execute(this.onChange);
		}
		else if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName))
		{
			this.eventReceiver.SendMessage(this.functionName, this.mSelectedItem, SendMessageOptions.DontRequireReceiver);
		}
		UIPopupList.current = null;
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x0001934C File Offset: 0x0001754C
	private void OnEnable()
	{
		if (EventDelegate.IsValid(this.onChange))
		{
			this.eventReceiver = null;
			this.functionName = null;
		}
		if (this.font != null)
		{
			if (this.font.isDynamic)
			{
				this.trueTypeFont = this.font.dynamicFont;
				this.fontStyle = this.font.dynamicFontStyle;
				this.mUseDynamicFont = true;
			}
			else if (this.bitmapFont == null)
			{
				this.bitmapFont = this.font;
				this.mUseDynamicFont = false;
			}
			this.font = null;
		}
		if (this.textScale != 0f)
		{
			this.fontSize = ((!(this.bitmapFont != null)) ? 16 : Mathf.RoundToInt((float)this.bitmapFont.defaultSize * this.textScale));
			this.textScale = 0f;
		}
		if (this.trueTypeFont == null && this.bitmapFont != null && this.bitmapFont.isDynamic)
		{
			this.trueTypeFont = this.bitmapFont.dynamicFont;
			this.bitmapFont = null;
		}
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x0001948C File Offset: 0x0001768C
	private void OnValidate()
	{
		Font font = this.trueTypeFont;
		UIFont uifont = this.bitmapFont;
		this.bitmapFont = null;
		this.trueTypeFont = null;
		if (font != null && (uifont == null || !this.mUseDynamicFont))
		{
			this.bitmapFont = null;
			this.trueTypeFont = font;
			this.mUseDynamicFont = true;
		}
		else if (uifont != null)
		{
			if (uifont.isDynamic)
			{
				this.trueTypeFont = uifont.dynamicFont;
				this.fontStyle = uifont.dynamicFontStyle;
				this.fontSize = uifont.defaultSize;
				this.mUseDynamicFont = true;
			}
			else
			{
				this.bitmapFont = uifont;
				this.mUseDynamicFont = false;
			}
		}
		else
		{
			this.trueTypeFont = font;
			this.mUseDynamicFont = true;
		}
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x0001955C File Offset: 0x0001775C
	private void Start()
	{
		if (this.textLabel != null)
		{
			EventDelegate.Add(this.onChange, new EventDelegate.Callback(this.textLabel.SetCurrentSelection));
			this.textLabel = null;
		}
		if (Application.isPlaying)
		{
			if (string.IsNullOrEmpty(this.mSelectedItem))
			{
				if (this.items.Count > 0)
				{
					this.value = this.items[0];
				}
			}
			else
			{
				string text = this.mSelectedItem;
				this.mSelectedItem = null;
				this.value = text;
			}
		}
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x00004E4E File Offset: 0x0000304E
	private void OnLocalize(Localization loc)
	{
		if (this.isLocalized)
		{
			this.TriggerCallbacks();
		}
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x000195F4 File Offset: 0x000177F4
	private void Highlight(UILabel lbl, bool instant)
	{
		if (this.mHighlight != null)
		{
			TweenPosition component = lbl.GetComponent<TweenPosition>();
			if (component != null && component.enabled)
			{
				return;
			}
			this.mHighlightedLabel = lbl;
			UISpriteData atlasSprite = this.mHighlight.GetAtlasSprite();
			if (atlasSprite == null)
			{
				return;
			}
			float pixelSize = this.atlas.pixelSize;
			float num = (float)atlasSprite.borderLeft * pixelSize;
			float num2 = (float)atlasSprite.borderTop * pixelSize;
			Vector3 vector = lbl.cachedTransform.localPosition + new Vector3(-num, num2, 1f);
			if (instant || !this.isAnimated)
			{
				this.mHighlight.cachedTransform.localPosition = vector;
			}
			else
			{
				TweenPosition.Begin(this.mHighlight.gameObject, 0.1f, vector).method = UITweener.Method.EaseOut;
			}
		}
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x000196D4 File Offset: 0x000178D4
	private void OnItemHover(GameObject go, bool isOver)
	{
		if (isOver)
		{
			UILabel component = go.GetComponent<UILabel>();
			this.Highlight(component, false);
		}
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x000196F8 File Offset: 0x000178F8
	private void Select(UILabel lbl, bool instant)
	{
		this.Highlight(lbl, instant);
		UIEventListener component = lbl.gameObject.GetComponent<UIEventListener>();
		this.value = component.parameter as string;
		UIPlaySound[] components = base.GetComponents<UIPlaySound>();
		int i = 0;
		int num = components.Length;
		while (i < num)
		{
			UIPlaySound uiplaySound = components[i];
			if (uiplaySound.trigger == UIPlaySound.Trigger.OnClick)
			{
				NGUITools.PlaySound(uiplaySound.audioClip, uiplaySound.volume, 1f);
			}
			i++;
		}
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x00004E61 File Offset: 0x00003061
	private void OnItemPress(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			this.Select(go.GetComponent<UILabel>(), true);
		}
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x00019774 File Offset: 0x00017974
	private void OnKey(KeyCode key)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.handleEvents)
		{
			int num = this.mLabelList.IndexOf(this.mHighlightedLabel);
			if (key == KeyCode.UpArrow)
			{
				if (num > 0)
				{
					this.Select(this.mLabelList[num - 1], false);
				}
			}
			else if (key == KeyCode.DownArrow)
			{
				if (num + 1 < this.mLabelList.Count)
				{
					this.Select(this.mLabelList[num + 1], false);
				}
			}
			else if (key == KeyCode.Escape)
			{
				this.OnSelect(false);
			}
		}
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x00019830 File Offset: 0x00017A30
	private void OnSelect(bool isSelected)
	{
		if (!isSelected && this.mChild != null)
		{
			this.mLabelList.Clear();
			this.handleEvents = false;
			if (this.isAnimated)
			{
				UIWidget[] componentsInChildren = this.mChild.GetComponentsInChildren<UIWidget>();
				int i = 0;
				int num = componentsInChildren.Length;
				while (i < num)
				{
					UIWidget uiwidget = componentsInChildren[i];
					Color color = uiwidget.color;
					color.a = 0f;
					TweenColor.Begin(uiwidget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
					i++;
				}
				Collider[] componentsInChildren2 = this.mChild.GetComponentsInChildren<Collider>();
				int j = 0;
				int num2 = componentsInChildren2.Length;
				while (j < num2)
				{
					componentsInChildren2[j].enabled = false;
					j++;
				}
				UnityEngine.Object.Destroy(this.mChild, 0.15f);
			}
			else
			{
				UnityEngine.Object.Destroy(this.mChild);
			}
			this.mBackground = null;
			this.mHighlight = null;
			this.mChild = null;
		}
	}

	// Token: 0x060001DA RID: 474 RVA: 0x00019930 File Offset: 0x00017B30
	private void AnimateColor(UIWidget widget)
	{
		Color color = widget.color;
		widget.color = new Color(color.r, color.g, color.b, 0f);
		TweenColor.Begin(widget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
	}

	// Token: 0x060001DB RID: 475 RVA: 0x00019980 File Offset: 0x00017B80
	private void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
	{
		Vector3 localPosition = widget.cachedTransform.localPosition;
		Vector3 vector = ((!placeAbove) ? new Vector3(localPosition.x, 0f, localPosition.z) : new Vector3(localPosition.x, bottom, localPosition.z));
		widget.cachedTransform.localPosition = vector;
		GameObject gameObject = widget.gameObject;
		TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
	}

	// Token: 0x060001DC RID: 476 RVA: 0x000199F8 File Offset: 0x00017BF8
	private void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
	{
		GameObject gameObject = widget.gameObject;
		Transform cachedTransform = widget.cachedTransform;
		float num = (float)this.activeFontSize * this.activeFontScale + this.mBgBorder * 2f;
		cachedTransform.localScale = new Vector3(1f, num / (float)widget.height, 1f);
		TweenScale.Begin(gameObject, 0.15f, Vector3.one).method = UITweener.Method.EaseOut;
		if (placeAbove)
		{
			Vector3 localPosition = cachedTransform.localPosition;
			cachedTransform.localPosition = new Vector3(localPosition.x, localPosition.y - (float)widget.height + num, localPosition.z);
			TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
		}
	}

	// Token: 0x060001DD RID: 477 RVA: 0x00004E76 File Offset: 0x00003076
	private void Animate(UIWidget widget, bool placeAbove, float bottom)
	{
		this.AnimateColor(widget);
		this.AnimatePosition(widget, placeAbove, bottom);
	}

	// Token: 0x060001DE RID: 478 RVA: 0x00019AAC File Offset: 0x00017CAC
	private void OnClick()
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.mChild == null && this.atlas != null && this.isValid && this.items.Count > 0)
		{
			this.mLabelList.Clear();
			if (this.mPanel == null)
			{
				this.mPanel = UIPanel.Find(base.transform);
				if (this.mPanel == null)
				{
					return;
				}
			}
			this.handleEvents = true;
			Transform transform = base.transform;
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform.parent, transform);
			this.mChild = new GameObject("Drop-down List");
			this.mChild.layer = base.gameObject.layer;
			Transform transform2 = this.mChild.transform;
			transform2.parent = transform.parent;
			transform2.localPosition = bounds.min;
			transform2.localRotation = Quaternion.identity;
			transform2.localScale = Vector3.one;
			this.mBackground = NGUITools.AddSprite(this.mChild, this.atlas, this.backgroundSprite);
			this.mBackground.pivot = UIWidget.Pivot.TopLeft;
			this.mBackground.depth = NGUITools.CalculateNextDepth(this.mPanel.gameObject);
			this.mBackground.color = this.backgroundColor;
			Vector4 border = this.mBackground.border;
			this.mBgBorder = border.y;
			this.mBackground.cachedTransform.localPosition = new Vector3(0f, border.y, 0f);
			this.mHighlight = NGUITools.AddSprite(this.mChild, this.atlas, this.highlightSprite);
			this.mHighlight.pivot = UIWidget.Pivot.TopLeft;
			this.mHighlight.color = this.highlightColor;
			UISpriteData atlasSprite = this.mHighlight.GetAtlasSprite();
			if (atlasSprite == null)
			{
				return;
			}
			float num = (float)atlasSprite.borderTop;
			float num2 = ((!(this.bitmapFont != null)) ? 1f : this.bitmapFont.pixelSize);
			float num3 = (float)this.activeFontSize * num2;
			float activeFontScale = this.activeFontScale;
			float num4 = num3 * activeFontScale;
			float num5 = 0f;
			float num6 = -this.padding.y;
			int num7 = ((!(this.bitmapFont != null)) ? this.fontSize : this.bitmapFont.defaultSize);
			List<UILabel> list = new List<UILabel>();
			int i = 0;
			int count = this.items.Count;
			while (i < count)
			{
				string text = this.items[i];
				UILabel uilabel = NGUITools.AddWidget<UILabel>(this.mChild);
				uilabel.pivot = UIWidget.Pivot.TopLeft;
				uilabel.bitmapFont = this.bitmapFont;
				uilabel.trueTypeFont = this.trueTypeFont;
				uilabel.fontSize = num7;
				uilabel.fontStyle = this.fontStyle;
				uilabel.text = ((!this.isLocalized || !(Localization.instance != null)) ? text : Localization.instance.Get(text));
				uilabel.color = this.textColor;
				uilabel.cachedTransform.localPosition = new Vector3(border.x + this.padding.x, num6, -1f);
				uilabel.overflowMethod = UILabel.Overflow.ResizeFreely;
				uilabel.MakePixelPerfect();
				if (activeFontScale != 1f)
				{
					uilabel.cachedTransform.localScale = Vector3.one * activeFontScale;
				}
				list.Add(uilabel);
				num6 -= num4;
				num6 -= this.padding.y;
				num5 = Mathf.Max(num5, uilabel.printedSize.x);
				UIEventListener uieventListener = UIEventListener.Get(uilabel.gameObject);
				uieventListener.onHover = new UIEventListener.BoolDelegate(this.OnItemHover);
				uieventListener.onPress = new UIEventListener.BoolDelegate(this.OnItemPress);
				uieventListener.parameter = text;
				if (this.mSelectedItem == text)
				{
					this.Highlight(uilabel, true);
				}
				this.mLabelList.Add(uilabel);
				i++;
			}
			num5 = Mathf.Max(num5, bounds.size.x * activeFontScale - (border.x + this.padding.x) * 2f);
			float num8 = num5 / activeFontScale;
			Vector3 vector = new Vector3(num8 * 0.5f, -num3 * 0.5f, 0f);
			Vector3 vector2 = new Vector3(num8, (num4 + this.padding.y) / activeFontScale, 1f);
			int j = 0;
			int count2 = list.Count;
			while (j < count2)
			{
				UILabel uilabel2 = list[j];
				BoxCollider boxCollider = NGUITools.AddWidgetCollider(uilabel2.gameObject);
				vector.z = boxCollider.center.z;
				boxCollider.center = vector;
				boxCollider.size = vector2;
				j++;
			}
			num5 += (border.x + this.padding.x) * 2f;
			num6 -= border.y;
			this.mBackground.width = Mathf.RoundToInt(num5);
			this.mBackground.height = Mathf.RoundToInt(-num6 + border.y);
			float num9 = 2f * this.atlas.pixelSize;
			float num10 = num5 - (border.x + this.padding.x) * 2f + (float)atlasSprite.borderLeft * num9;
			float num11 = num4 + num * num9;
			this.mHighlight.width = Mathf.RoundToInt(num10);
			this.mHighlight.height = Mathf.RoundToInt(num11);
			bool flag = this.position == UIPopupList.Position.Above;
			if (this.position == UIPopupList.Position.Auto)
			{
				UICamera uicamera = UICamera.FindCameraForLayer(base.gameObject.layer);
				if (uicamera != null)
				{
					flag = uicamera.cachedCamera.WorldToViewportPoint(transform.position).y < 0.5f;
				}
			}
			if (this.isAnimated)
			{
				float num12 = num6 + num4;
				this.Animate(this.mHighlight, flag, num12);
				int k = 0;
				int count3 = list.Count;
				while (k < count3)
				{
					this.Animate(list[k], flag, num12);
					k++;
				}
				this.AnimateColor(this.mBackground);
				this.AnimateScale(this.mBackground, flag, num12);
			}
			if (flag)
			{
				transform2.localPosition = new Vector3(bounds.min.x, bounds.max.y - num6 - border.y, bounds.min.z);
			}
		}
		else
		{
			this.OnSelect(false);
		}
	}

	// Token: 0x040001BA RID: 442
	private const float animSpeed = 0.15f;

	// Token: 0x040001BB RID: 443
	public static UIPopupList current;

	// Token: 0x040001BC RID: 444
	public UIAtlas atlas;

	// Token: 0x040001BD RID: 445
	public UIFont bitmapFont;

	// Token: 0x040001BE RID: 446
	public Font trueTypeFont;

	// Token: 0x040001BF RID: 447
	public int fontSize = 16;

	// Token: 0x040001C0 RID: 448
	public FontStyle fontStyle;

	// Token: 0x040001C1 RID: 449
	public string backgroundSprite;

	// Token: 0x040001C2 RID: 450
	public string highlightSprite;

	// Token: 0x040001C3 RID: 451
	public UIPopupList.Position position;

	// Token: 0x040001C4 RID: 452
	public List<string> items = new List<string>();

	// Token: 0x040001C5 RID: 453
	public Vector2 padding = new Vector3(4f, 4f);

	// Token: 0x040001C6 RID: 454
	public Color textColor = Color.white;

	// Token: 0x040001C7 RID: 455
	public Color backgroundColor = Color.white;

	// Token: 0x040001C8 RID: 456
	public Color highlightColor = new Color(0.88235295f, 0.78431374f, 0.5882353f, 1f);

	// Token: 0x040001C9 RID: 457
	public bool isAnimated = true;

	// Token: 0x040001CA RID: 458
	public bool isLocalized;

	// Token: 0x040001CB RID: 459
	public List<EventDelegate> onChange = new List<EventDelegate>();

	// Token: 0x040001CC RID: 460
	[SerializeField]
	[HideInInspector]
	private string mSelectedItem;

	// Token: 0x040001CD RID: 461
	private UIPanel mPanel;

	// Token: 0x040001CE RID: 462
	private GameObject mChild;

	// Token: 0x040001CF RID: 463
	private UISprite mBackground;

	// Token: 0x040001D0 RID: 464
	private UISprite mHighlight;

	// Token: 0x040001D1 RID: 465
	private UILabel mHighlightedLabel;

	// Token: 0x040001D2 RID: 466
	private List<UILabel> mLabelList = new List<UILabel>();

	// Token: 0x040001D3 RID: 467
	private float mBgBorder;

	// Token: 0x040001D4 RID: 468
	[HideInInspector]
	[SerializeField]
	private GameObject eventReceiver;

	// Token: 0x040001D5 RID: 469
	[SerializeField]
	[HideInInspector]
	private string functionName = "OnSelectionChange";

	// Token: 0x040001D6 RID: 470
	[HideInInspector]
	[SerializeField]
	private float textScale;

	// Token: 0x040001D7 RID: 471
	[SerializeField]
	[HideInInspector]
	private UIFont font;

	// Token: 0x040001D8 RID: 472
	[SerializeField]
	[HideInInspector]
	private UILabel textLabel;

	// Token: 0x040001D9 RID: 473
	private UIPopupList.LegacyEvent mLegacyEvent;

	// Token: 0x040001DA RID: 474
	private bool mUseDynamicFont;

	// Token: 0x0200004B RID: 75
	public enum Position
	{
		// Token: 0x040001DC RID: 476
		Auto,
		// Token: 0x040001DD RID: 477
		Above,
		// Token: 0x040001DE RID: 478
		Below
	}

	// Token: 0x0200004C RID: 76
	// (Invoke) Token: 0x060001E0 RID: 480
	public delegate void LegacyEvent(string val);
}
