using System;
using System.Text;
using UnityEngine;

// Token: 0x020000C3 RID: 195
[AddComponentMenu("NGUI/UI/Text List")]
public class UITextList : MonoBehaviour
{
	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06000661 RID: 1633 RVA: 0x00008226 File Offset: 0x00006426
	public bool isValid
	{
		get
		{
			return this.textLabel != null && this.textLabel.ambigiousFont != null;
		}
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06000662 RID: 1634 RVA: 0x0000824D File Offset: 0x0000644D
	// (set) Token: 0x06000663 RID: 1635 RVA: 0x000329B8 File Offset: 0x00030BB8
	public float scrollValue
	{
		get
		{
			return this.mScroll;
		}
		set
		{
			value = Mathf.Clamp01(value);
			if (this.isValid && this.mScroll != value)
			{
				if (this.scrollBar != null)
				{
					this.scrollBar.value = value;
				}
				else
				{
					this.mScroll = value;
					this.UpdateVisibleText();
				}
			}
		}
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06000664 RID: 1636 RVA: 0x00008255 File Offset: 0x00006455
	protected float lineHeight
	{
		get
		{
			return (!(this.textLabel != null)) ? 20f : ((float)this.textLabel.fontSize);
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x06000665 RID: 1637 RVA: 0x00032A14 File Offset: 0x00030C14
	protected int scrollHeight
	{
		get
		{
			if (!this.isValid)
			{
				return 0;
			}
			int num = Mathf.FloorToInt((float)this.textLabel.height / (float)this.textLabel.fontSize);
			return Mathf.Max(0, this.mTotalLines - num);
		}
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x0000827E File Offset: 0x0000647E
	public void Clear()
	{
		this.mParagraphs.Clear();
		this.UpdateVisibleText();
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x00032A5C File Offset: 0x00030C5C
	private void Start()
	{
		if (this.textLabel == null)
		{
			this.textLabel = base.GetComponentInChildren<UILabel>();
		}
		if (this.scrollBar != null)
		{
			EventDelegate.Add(this.scrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
		}
		this.textLabel.overflowMethod = UILabel.Overflow.ClampContent;
		if (this.style == UITextList.Style.Chat)
		{
			this.textLabel.pivot = UIWidget.Pivot.BottomLeft;
			this.scrollValue = 1f;
		}
		else
		{
			this.textLabel.pivot = UIWidget.Pivot.TopLeft;
			this.scrollValue = 0f;
		}
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x00032B00 File Offset: 0x00030D00
	private void Update()
	{
		if (this.isValid && (this.textLabel.width != this.mLastWidth || this.textLabel.height != this.mLastHeight))
		{
			this.mLastWidth = this.textLabel.width;
			this.mLastHeight = this.textLabel.height;
			this.Rebuild();
		}
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x00032B6C File Offset: 0x00030D6C
	private void OnScroll(float val)
	{
		int scrollHeight = this.scrollHeight;
		if (scrollHeight != 0)
		{
			val *= this.lineHeight;
			this.scrollValue = this.mScroll - val / (float)scrollHeight;
		}
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x00032BA4 File Offset: 0x00030DA4
	private void OnDrag(Vector2 delta)
	{
		int scrollHeight = this.scrollHeight;
		if (scrollHeight != 0)
		{
			float num = delta.y / this.lineHeight;
			this.scrollValue = this.mScroll + num / (float)scrollHeight;
		}
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x00008291 File Offset: 0x00006491
	private void OnScrollBar()
	{
		this.mScroll = UIProgressBar.current.value;
		this.UpdateVisibleText();
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x000082A9 File Offset: 0x000064A9
	public void Add(string text)
	{
		this.Add(text, true);
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x00032BE0 File Offset: 0x00030DE0
	protected void Add(string text, bool updateVisible)
	{
		UITextList.Paragraph paragraph;
		if (this.mParagraphs.size < this.paragraphHistory)
		{
			paragraph = new UITextList.Paragraph();
		}
		else
		{
			paragraph = this.mParagraphs[0];
			this.mParagraphs.RemoveAt(0);
		}
		paragraph.text = text;
		this.mParagraphs.Add(paragraph);
		this.Rebuild();
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x00032C44 File Offset: 0x00030E44
	protected void Rebuild()
	{
		if (this.isValid)
		{
			this.textLabel.UpdateNGUIText();
			NGUIText.current.lineHeight = 1000000;
			UIFont bitmapFont = this.textLabel.bitmapFont;
			this.mTotalLines = 0;
			for (int i = 0; i < this.mParagraphs.size; i++)
			{
				UITextList.Paragraph paragraph = this.mParagraphs.buffer[i];
				string text;
				if (bitmapFont != null)
				{
					if (bitmapFont.WrapText(paragraph.text, out text))
					{
						paragraph.lines = text.Split(new char[] { '\n' });
						this.mTotalLines += paragraph.lines.Length;
					}
				}
				else if (NGUIText.WrapText(this.textLabel.trueTypeFont, paragraph.text, out text))
				{
					paragraph.lines = text.Split(new char[] { '\n' });
					this.mTotalLines += paragraph.lines.Length;
				}
			}
			this.mTotalLines = 0;
			int j = 0;
			int size = this.mParagraphs.size;
			while (j < size)
			{
				this.mTotalLines += this.mParagraphs.buffer[j].lines.Length;
				j++;
			}
			if (this.scrollBar != null)
			{
				UIScrollBar uiscrollBar = this.scrollBar as UIScrollBar;
				if (uiscrollBar != null)
				{
					uiscrollBar.barSize = 1f - (float)this.scrollHeight / (float)this.mTotalLines;
				}
			}
			this.UpdateVisibleText();
		}
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x00032DE8 File Offset: 0x00030FE8
	protected void UpdateVisibleText()
	{
		if (this.isValid)
		{
			int num = Mathf.FloorToInt((float)this.textLabel.height / (float)this.textLabel.fontSize);
			int num2 = Mathf.Max(0, this.mTotalLines - num);
			int num3 = Mathf.RoundToInt(this.mScroll * (float)num2);
			if (num3 < 0)
			{
				num3 = 0;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num4 = 0;
			int size = this.mParagraphs.size;
			while (num > 0 && num4 < size)
			{
				UITextList.Paragraph paragraph = this.mParagraphs.buffer[num4];
				int num5 = 0;
				int num6 = paragraph.lines.Length;
				while (num > 0 && num5 < num6)
				{
					string text = paragraph.lines[num5];
					if (num3 > 0)
					{
						num3--;
					}
					else
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append("\n");
						}
						stringBuilder.Append(text);
						num--;
					}
					num5++;
				}
				num4++;
			}
			this.textLabel.text = stringBuilder.ToString();
		}
	}

	// Token: 0x040004FC RID: 1276
	public UILabel textLabel;

	// Token: 0x040004FD RID: 1277
	public UIProgressBar scrollBar;

	// Token: 0x040004FE RID: 1278
	public UITextList.Style style;

	// Token: 0x040004FF RID: 1279
	public int paragraphHistory = 50;

	// Token: 0x04000500 RID: 1280
	protected char[] mSeparator = new char[] { '\n' };

	// Token: 0x04000501 RID: 1281
	protected BetterList<UITextList.Paragraph> mParagraphs = new BetterList<UITextList.Paragraph>();

	// Token: 0x04000502 RID: 1282
	protected float mScroll;

	// Token: 0x04000503 RID: 1283
	protected int mTotalLines;

	// Token: 0x04000504 RID: 1284
	protected int mLastWidth;

	// Token: 0x04000505 RID: 1285
	protected int mLastHeight;

	// Token: 0x020000C4 RID: 196
	public enum Style
	{
		// Token: 0x04000507 RID: 1287
		Text,
		// Token: 0x04000508 RID: 1288
		Chat
	}

	// Token: 0x020000C5 RID: 197
	protected class Paragraph
	{
		// Token: 0x04000509 RID: 1289
		public string text;

		// Token: 0x0400050A RID: 1290
		public string[] lines;
	}
}
