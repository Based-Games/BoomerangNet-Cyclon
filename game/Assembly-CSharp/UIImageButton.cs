using System;
using UnityEngine;

// Token: 0x02000042 RID: 66
[AddComponentMenu("NGUI/UI/Image Button")]
public class UIImageButton : MonoBehaviour
{
	// Token: 0x17000031 RID: 49
	// (get) Token: 0x06000196 RID: 406 RVA: 0x00018694 File Offset: 0x00016894
	// (set) Token: 0x06000197 RID: 407 RVA: 0x000186BC File Offset: 0x000168BC
	public bool isEnabled
	{
		get
		{
			Collider collider = base.collider;
			return collider && collider.enabled;
		}
		set
		{
			Collider collider = base.collider;
			if (!collider)
			{
				return;
			}
			if (collider.enabled != value)
			{
				collider.enabled = value;
				this.UpdateImage();
			}
		}
	}

	// Token: 0x06000198 RID: 408 RVA: 0x00004AAC File Offset: 0x00002CAC
	private void OnEnable()
	{
		if (this.target == null)
		{
			this.target = base.GetComponentInChildren<UISprite>();
		}
		this.UpdateImage();
	}

	// Token: 0x06000199 RID: 409 RVA: 0x000186F8 File Offset: 0x000168F8
	private void UpdateImage()
	{
		if (this.target != null)
		{
			if (this.isEnabled)
			{
				this.target.spriteName = ((!UICamera.IsHighlighted(base.gameObject)) ? this.normalSprite : this.hoverSprite);
			}
			else
			{
				this.target.spriteName = this.disabledSprite;
			}
			this.target.MakePixelPerfect();
		}
	}

	// Token: 0x0600019A RID: 410 RVA: 0x00018770 File Offset: 0x00016970
	private void OnHover(bool isOver)
	{
		if (this.isEnabled && this.target != null)
		{
			this.target.spriteName = ((!isOver) ? this.normalSprite : this.hoverSprite);
			this.target.MakePixelPerfect();
		}
	}

	// Token: 0x0600019B RID: 411 RVA: 0x00004AD1 File Offset: 0x00002CD1
	private void OnPress(bool pressed)
	{
		if (pressed)
		{
			this.target.spriteName = this.pressedSprite;
			this.target.MakePixelPerfect();
		}
		else
		{
			this.UpdateImage();
		}
	}

	// Token: 0x04000181 RID: 385
	public UISprite target;

	// Token: 0x04000182 RID: 386
	public string normalSprite;

	// Token: 0x04000183 RID: 387
	public string hoverSprite;

	// Token: 0x04000184 RID: 388
	public string pressedSprite;

	// Token: 0x04000185 RID: 389
	public string disabledSprite;
}
