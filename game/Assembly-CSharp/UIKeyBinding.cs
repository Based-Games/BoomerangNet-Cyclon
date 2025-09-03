using System;
using UnityEngine;

// Token: 0x02000043 RID: 67
[AddComponentMenu("Game/UI/Key Binding")]
public class UIKeyBinding : MonoBehaviour
{
	// Token: 0x0600019D RID: 413 RVA: 0x000187C8 File Offset: 0x000169C8
	private void Start()
	{
		UIInput component = base.GetComponent<UIInput>();
		this.mIsInput = component != null;
		if (component != null)
		{
			EventDelegate.Add(component.onSubmit, new EventDelegate.Callback(this.OnSubmit));
		}
	}

	// Token: 0x0600019E RID: 414 RVA: 0x00004B00 File Offset: 0x00002D00
	private void OnSubmit()
	{
		if (UICamera.currentKey == this.keyCode && this.IsModifierActive())
		{
			this.mIgnoreUp = true;
		}
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0001880C File Offset: 0x00016A0C
	private bool IsModifierActive()
	{
		if (this.modifier == UIKeyBinding.Modifier.None)
		{
			return true;
		}
		if (this.modifier == UIKeyBinding.Modifier.Alt)
		{
			if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.Control)
		{
			if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.Shift && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
		{
			return true;
		}
		return false;
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x000188B8 File Offset: 0x00016AB8
	private void Update()
	{
		if (this.keyCode == KeyCode.None || !this.IsModifierActive())
		{
			return;
		}
		if (this.action == UIKeyBinding.Action.PressAndClick)
		{
			if (UICamera.inputHasFocus)
			{
				return;
			}
			if (Input.GetKeyDown(this.keyCode))
			{
				base.SendMessage("OnPress", true, SendMessageOptions.DontRequireReceiver);
			}
			if (Input.GetKeyUp(this.keyCode))
			{
				base.SendMessage("OnPress", false, SendMessageOptions.DontRequireReceiver);
				base.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
			}
		}
		else if (this.action == UIKeyBinding.Action.Select && Input.GetKeyUp(this.keyCode))
		{
			if (this.mIsInput)
			{
				if (!this.mIgnoreUp && !UICamera.inputHasFocus)
				{
					UICamera.selectedObject = base.gameObject;
				}
				this.mIgnoreUp = false;
			}
			else
			{
				UICamera.selectedObject = base.gameObject;
			}
		}
	}

	// Token: 0x04000186 RID: 390
	public KeyCode keyCode;

	// Token: 0x04000187 RID: 391
	public UIKeyBinding.Modifier modifier;

	// Token: 0x04000188 RID: 392
	public UIKeyBinding.Action action;

	// Token: 0x04000189 RID: 393
	private bool mIgnoreUp;

	// Token: 0x0400018A RID: 394
	private bool mIsInput;

	// Token: 0x02000044 RID: 68
	public enum Action
	{
		// Token: 0x0400018C RID: 396
		PressAndClick,
		// Token: 0x0400018D RID: 397
		Select
	}

	// Token: 0x02000045 RID: 69
	public enum Modifier
	{
		// Token: 0x0400018F RID: 399
		None,
		// Token: 0x04000190 RID: 400
		Shift,
		// Token: 0x04000191 RID: 401
		Control,
		// Token: 0x04000192 RID: 402
		Alt
	}
}
