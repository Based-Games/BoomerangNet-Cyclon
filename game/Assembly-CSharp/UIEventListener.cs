using System;
using UnityEngine;

// Token: 0x0200007C RID: 124
[AddComponentMenu("NGUI/Internal/Event Listener")]
public class UIEventListener : MonoBehaviour
{
	// Token: 0x060003A3 RID: 931 RVA: 0x00005FB1 File Offset: 0x000041B1
	private void OnSubmit()
	{
		if (this.onSubmit != null)
		{
			this.onSubmit(base.gameObject);
		}
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x00005FCF File Offset: 0x000041CF
	private void OnClick()
	{
		if (this.onClick != null)
		{
			this.onClick(base.gameObject);
		}
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x00005FED File Offset: 0x000041ED
	private void OnDoubleClick()
	{
		if (this.onDoubleClick != null)
		{
			this.onDoubleClick(base.gameObject);
		}
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x0000600B File Offset: 0x0000420B
	private void OnHover(bool isOver)
	{
		if (this.onHover != null)
		{
			this.onHover(base.gameObject, isOver);
		}
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x0000602A File Offset: 0x0000422A
	private void OnPress(bool isPressed)
	{
		if (this.onPress != null)
		{
			this.onPress(base.gameObject, isPressed);
		}
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x00006049 File Offset: 0x00004249
	private void OnSelect(bool selected)
	{
		if (this.onSelect != null)
		{
			this.onSelect(base.gameObject, selected);
		}
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x00006068 File Offset: 0x00004268
	private void OnScroll(float delta)
	{
		if (this.onScroll != null)
		{
			this.onScroll(base.gameObject, delta);
		}
	}

	// Token: 0x060003AA RID: 938 RVA: 0x00006087 File Offset: 0x00004287
	private void OnDrag(Vector2 delta)
	{
		if (this.onDrag != null)
		{
			this.onDrag(base.gameObject, delta);
		}
	}

	// Token: 0x060003AB RID: 939 RVA: 0x000060A6 File Offset: 0x000042A6
	private void OnDrop(GameObject go)
	{
		if (this.onDrop != null)
		{
			this.onDrop(base.gameObject, go);
		}
	}

	// Token: 0x060003AC RID: 940 RVA: 0x000060C5 File Offset: 0x000042C5
	private void OnInput(string text)
	{
		if (this.onInput != null)
		{
			this.onInput(base.gameObject, text);
		}
	}

	// Token: 0x060003AD RID: 941 RVA: 0x000060E4 File Offset: 0x000042E4
	private void OnKey(KeyCode key)
	{
		if (this.onKey != null)
		{
			this.onKey(base.gameObject, key);
		}
	}

	// Token: 0x060003AE RID: 942 RVA: 0x000237B0 File Offset: 0x000219B0
	public static UIEventListener Get(GameObject go)
	{
		UIEventListener uieventListener = go.GetComponent<UIEventListener>();
		if (uieventListener == null)
		{
			uieventListener = go.AddComponent<UIEventListener>();
		}
		return uieventListener;
	}

	// Token: 0x040002F2 RID: 754
	public object parameter;

	// Token: 0x040002F3 RID: 755
	public UIEventListener.VoidDelegate onSubmit;

	// Token: 0x040002F4 RID: 756
	public UIEventListener.VoidDelegate onClick;

	// Token: 0x040002F5 RID: 757
	public UIEventListener.VoidDelegate onDoubleClick;

	// Token: 0x040002F6 RID: 758
	public UIEventListener.BoolDelegate onHover;

	// Token: 0x040002F7 RID: 759
	public UIEventListener.BoolDelegate onPress;

	// Token: 0x040002F8 RID: 760
	public UIEventListener.BoolDelegate onSelect;

	// Token: 0x040002F9 RID: 761
	public UIEventListener.FloatDelegate onScroll;

	// Token: 0x040002FA RID: 762
	public UIEventListener.VectorDelegate onDrag;

	// Token: 0x040002FB RID: 763
	public UIEventListener.ObjectDelegate onDrop;

	// Token: 0x040002FC RID: 764
	public UIEventListener.StringDelegate onInput;

	// Token: 0x040002FD RID: 765
	public UIEventListener.KeyCodeDelegate onKey;

	// Token: 0x0200007D RID: 125
	// (Invoke) Token: 0x060003B0 RID: 944
	public delegate void VoidDelegate(GameObject go);

	// Token: 0x0200007E RID: 126
	// (Invoke) Token: 0x060003B4 RID: 948
	public delegate void BoolDelegate(GameObject go, bool state);

	// Token: 0x0200007F RID: 127
	// (Invoke) Token: 0x060003B8 RID: 952
	public delegate void FloatDelegate(GameObject go, float delta);

	// Token: 0x02000080 RID: 128
	// (Invoke) Token: 0x060003BC RID: 956
	public delegate void VectorDelegate(GameObject go, Vector2 delta);

	// Token: 0x02000081 RID: 129
	// (Invoke) Token: 0x060003C0 RID: 960
	public delegate void StringDelegate(GameObject go, string text);

	// Token: 0x02000082 RID: 130
	// (Invoke) Token: 0x060003C4 RID: 964
	public delegate void ObjectDelegate(GameObject go, GameObject draggedObject);

	// Token: 0x02000083 RID: 131
	// (Invoke) Token: 0x060003C8 RID: 968
	public delegate void KeyCodeDelegate(GameObject go, KeyCode key);
}
