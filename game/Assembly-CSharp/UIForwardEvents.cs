using System;
using UnityEngine;

// Token: 0x0200003E RID: 62
[AddComponentMenu("NGUI/Interaction/Forward Events")]
public class UIForwardEvents : MonoBehaviour
{
	// Token: 0x06000180 RID: 384 RVA: 0x00004823 File Offset: 0x00002A23
	private void OnHover(bool isOver)
	{
		if (this.onHover && this.target != null)
		{
			this.target.SendMessage("OnHover", isOver, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06000181 RID: 385 RVA: 0x00004858 File Offset: 0x00002A58
	private void OnPress(bool pressed)
	{
		if (this.onPress && this.target != null)
		{
			this.target.SendMessage("OnPress", pressed, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000488D File Offset: 0x00002A8D
	private void OnClick()
	{
		if (this.onClick && this.target != null)
		{
			this.target.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06000183 RID: 387 RVA: 0x000048BC File Offset: 0x00002ABC
	private void OnDoubleClick()
	{
		if (this.onDoubleClick && this.target != null)
		{
			this.target.SendMessage("OnDoubleClick", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06000184 RID: 388 RVA: 0x000048EB File Offset: 0x00002AEB
	private void OnSelect(bool selected)
	{
		if (this.onSelect && this.target != null)
		{
			this.target.SendMessage("OnSelect", selected, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06000185 RID: 389 RVA: 0x00004920 File Offset: 0x00002B20
	private void OnDrag(Vector2 delta)
	{
		if (this.onDrag && this.target != null)
		{
			this.target.SendMessage("OnDrag", delta, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06000186 RID: 390 RVA: 0x00004955 File Offset: 0x00002B55
	private void OnDrop(GameObject go)
	{
		if (this.onDrop && this.target != null)
		{
			this.target.SendMessage("OnDrop", go, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06000187 RID: 391 RVA: 0x00004985 File Offset: 0x00002B85
	private void OnInput(string text)
	{
		if (this.onInput && this.target != null)
		{
			this.target.SendMessage("OnInput", text, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06000188 RID: 392 RVA: 0x000049B5 File Offset: 0x00002BB5
	private void OnSubmit()
	{
		if (this.onSubmit && this.target != null)
		{
			this.target.SendMessage("OnSubmit", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06000189 RID: 393 RVA: 0x000049E4 File Offset: 0x00002BE4
	private void OnScroll(float delta)
	{
		if (this.onScroll && this.target != null)
		{
			this.target.SendMessage("OnScroll", delta, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x04000166 RID: 358
	public GameObject target;

	// Token: 0x04000167 RID: 359
	public bool onHover;

	// Token: 0x04000168 RID: 360
	public bool onPress;

	// Token: 0x04000169 RID: 361
	public bool onClick;

	// Token: 0x0400016A RID: 362
	public bool onDoubleClick;

	// Token: 0x0400016B RID: 363
	public bool onSelect;

	// Token: 0x0400016C RID: 364
	public bool onDrag;

	// Token: 0x0400016D RID: 365
	public bool onDrop;

	// Token: 0x0400016E RID: 366
	public bool onInput;

	// Token: 0x0400016F RID: 367
	public bool onSubmit;

	// Token: 0x04000170 RID: 368
	public bool onScroll;
}
