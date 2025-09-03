using System;
using UnityEngine;

// Token: 0x0200002C RID: 44
[RequireComponent(typeof(Collider))]
[AddComponentMenu("NGUI/Interaction/Button Keys")]
public class UIButtonKeys : MonoBehaviour
{
	// Token: 0x0600011C RID: 284 RVA: 0x0000422B File Offset: 0x0000242B
	private void OnEnable()
	{
		if (this.startsSelected && (UICamera.selectedObject == null || !NGUITools.GetActive(UICamera.selectedObject)))
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.selectedObject = base.gameObject;
		}
	}

	// Token: 0x0600011D RID: 285 RVA: 0x00016358 File Offset: 0x00014558
	private void OnKey(KeyCode key)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject))
		{
			switch (key)
			{
			case KeyCode.UpArrow:
				if (this.selectOnUp != null)
				{
					UICamera.selectedObject = this.selectOnUp.gameObject;
				}
				break;
			case KeyCode.DownArrow:
				if (this.selectOnDown != null)
				{
					UICamera.selectedObject = this.selectOnDown.gameObject;
				}
				break;
			case KeyCode.RightArrow:
				if (this.selectOnRight != null)
				{
					UICamera.selectedObject = this.selectOnRight.gameObject;
				}
				break;
			case KeyCode.LeftArrow:
				if (this.selectOnLeft != null)
				{
					UICamera.selectedObject = this.selectOnLeft.gameObject;
				}
				break;
			default:
				if (key == KeyCode.Tab)
				{
					if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
					{
						if (this.selectOnLeft != null)
						{
							UICamera.selectedObject = this.selectOnLeft.gameObject;
						}
						else if (this.selectOnUp != null)
						{
							UICamera.selectedObject = this.selectOnUp.gameObject;
						}
						else if (this.selectOnDown != null)
						{
							UICamera.selectedObject = this.selectOnDown.gameObject;
						}
						else if (this.selectOnRight != null)
						{
							UICamera.selectedObject = this.selectOnRight.gameObject;
						}
					}
					else if (this.selectOnRight != null)
					{
						UICamera.selectedObject = this.selectOnRight.gameObject;
					}
					else if (this.selectOnDown != null)
					{
						UICamera.selectedObject = this.selectOnDown.gameObject;
					}
					else if (this.selectOnUp != null)
					{
						UICamera.selectedObject = this.selectOnUp.gameObject;
					}
					else if (this.selectOnLeft != null)
					{
						UICamera.selectedObject = this.selectOnLeft.gameObject;
					}
				}
				break;
			}
		}
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00004268 File Offset: 0x00002468
	private void OnClick()
	{
		if (base.enabled && this.selectOnClick != null)
		{
			UICamera.selectedObject = this.selectOnClick.gameObject;
		}
	}

	// Token: 0x040000F5 RID: 245
	public bool startsSelected;

	// Token: 0x040000F6 RID: 246
	public UIButtonKeys selectOnClick;

	// Token: 0x040000F7 RID: 247
	public UIButtonKeys selectOnUp;

	// Token: 0x040000F8 RID: 248
	public UIButtonKeys selectOnDown;

	// Token: 0x040000F9 RID: 249
	public UIButtonKeys selectOnLeft;

	// Token: 0x040000FA RID: 250
	public UIButtonKeys selectOnRight;
}
