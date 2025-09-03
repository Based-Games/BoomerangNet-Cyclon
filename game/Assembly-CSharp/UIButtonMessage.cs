using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
[AddComponentMenu("NGUI/Interaction/Button Message")]
public class UIButtonMessage : MonoBehaviour
{
	// Token: 0x06000120 RID: 288 RVA: 0x00004296 File Offset: 0x00002496
	private void Start()
	{
		this.mStarted = true;
	}

	// Token: 0x06000121 RID: 289 RVA: 0x0000429F File Offset: 0x0000249F
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x06000122 RID: 290 RVA: 0x000042BD File Offset: 0x000024BD
	private void OnHover(bool isOver)
	{
		if (base.enabled && ((isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOver) || (!isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOut)))
		{
			this.Send();
		}
	}

	// Token: 0x06000123 RID: 291 RVA: 0x000042F4 File Offset: 0x000024F4
	private void OnPress(bool isPressed)
	{
		if (base.enabled && ((isPressed && this.trigger == UIButtonMessage.Trigger.OnPress) || (!isPressed && this.trigger == UIButtonMessage.Trigger.OnRelease)))
		{
			this.Send();
		}
	}

	// Token: 0x06000124 RID: 292 RVA: 0x0000432B File Offset: 0x0000252B
	private void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00004350 File Offset: 0x00002550
	private void OnClick()
	{
		if (base.enabled && this.trigger == UIButtonMessage.Trigger.OnClick)
		{
			this.Send();
		}
	}

	// Token: 0x06000126 RID: 294 RVA: 0x0000436E File Offset: 0x0000256E
	private void OnDoubleClick()
	{
		if (base.enabled && this.trigger == UIButtonMessage.Trigger.OnDoubleClick)
		{
			this.Send();
		}
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00016594 File Offset: 0x00014794
	private void Send()
	{
		if (string.IsNullOrEmpty(this.functionName))
		{
			return;
		}
		if (this.target == null)
		{
			this.target = base.gameObject;
		}
		if (this.includeChildren)
		{
			Transform[] componentsInChildren = this.target.GetComponentsInChildren<Transform>();
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				Transform transform = componentsInChildren[i];
				transform.gameObject.SendMessage(this.functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
				i++;
			}
		}
		else
		{
			this.target.SendMessage(this.functionName, base.gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x040000FB RID: 251
	public GameObject target;

	// Token: 0x040000FC RID: 252
	public string functionName;

	// Token: 0x040000FD RID: 253
	public UIButtonMessage.Trigger trigger;

	// Token: 0x040000FE RID: 254
	public bool includeChildren;

	// Token: 0x040000FF RID: 255
	private bool mStarted;

	// Token: 0x0200002E RID: 46
	public enum Trigger
	{
		// Token: 0x04000101 RID: 257
		OnClick,
		// Token: 0x04000102 RID: 258
		OnMouseOver,
		// Token: 0x04000103 RID: 259
		OnMouseOut,
		// Token: 0x04000104 RID: 260
		OnPress,
		// Token: 0x04000105 RID: 261
		OnRelease,
		// Token: 0x04000106 RID: 262
		OnDoubleClick
	}
}
