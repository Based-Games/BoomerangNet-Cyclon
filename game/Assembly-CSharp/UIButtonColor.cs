using System;
using UnityEngine;

// Token: 0x0200002B RID: 43
[AddComponentMenu("NGUI/Interaction/Button Color")]
[ExecuteInEditMode]
public class UIButtonColor : UIWidgetContainer
{
	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000110 RID: 272 RVA: 0x00004145 File Offset: 0x00002345
	// (set) Token: 0x06000111 RID: 273 RVA: 0x00004153 File Offset: 0x00002353
	public Color defaultColor
	{
		get
		{
			this.Start();
			return this.mColor;
		}
		set
		{
			this.Start();
			this.mColor = value;
		}
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00004162 File Offset: 0x00002362
	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			this.Init();
		}
	}

	// Token: 0x06000113 RID: 275 RVA: 0x0000417C File Offset: 0x0000237C
	protected virtual void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.gameObject));
		}
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00016114 File Offset: 0x00014314
	protected virtual void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenColor component = this.tweenTarget.GetComponent<TweenColor>();
			if (component != null)
			{
				component.value = this.mColor;
				component.enabled = false;
			}
		}
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00016168 File Offset: 0x00014368
	protected void Init()
	{
		if (this.tweenTarget == null)
		{
			this.tweenTarget = base.gameObject;
		}
		this.mWidget = this.tweenTarget.GetComponent<UIWidget>();
		if (this.mWidget != null)
		{
			this.mColor = this.mWidget.color;
		}
		else
		{
			Renderer renderer = this.tweenTarget.renderer;
			if (renderer != null)
			{
				this.mColor = renderer.material.color;
			}
			else
			{
				Light light = this.tweenTarget.light;
				if (light != null)
				{
					this.mColor = light.color;
				}
				else
				{
					this.tweenTarget = null;
					if (Application.isPlaying)
					{
						Debug.LogWarning(NGUITools.GetHierarchy(base.gameObject) + " has nothing for UIButtonColor to color", this);
						base.enabled = false;
					}
				}
			}
		}
		this.OnEnable();
	}

	// Token: 0x06000116 RID: 278 RVA: 0x0001625C File Offset: 0x0001445C
	protected virtual void OnPress(bool isPressed)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			if (isPressed)
			{
				TweenColor.Begin(this.tweenTarget, this.duration, this.pressed);
			}
			else if (UICamera.currentTouch.current == base.gameObject && UICamera.currentScheme == UICamera.ControlScheme.Controller)
			{
				TweenColor.Begin(this.tweenTarget, this.duration, this.hover);
			}
			else
			{
				TweenColor.Begin(this.tweenTarget, this.duration, this.mColor);
			}
		}
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00016304 File Offset: 0x00014504
	protected virtual void OnHover(bool isOver)
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenColor.Begin(this.tweenTarget, this.duration, (!isOver) ? this.mColor : this.hover);
		}
	}

	// Token: 0x06000118 RID: 280 RVA: 0x0000419A File Offset: 0x0000239A
	protected virtual void OnDragOver()
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenColor.Begin(this.tweenTarget, this.duration, this.pressed);
		}
	}

	// Token: 0x06000119 RID: 281 RVA: 0x000041D0 File Offset: 0x000023D0
	protected virtual void OnDragOut()
	{
		if (base.enabled)
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenColor.Begin(this.tweenTarget, this.duration, this.mColor);
		}
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00004206 File Offset: 0x00002406
	protected virtual void OnSelect(bool isSelected)
	{
		if (base.enabled && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	// Token: 0x040000EE RID: 238
	public GameObject tweenTarget;

	// Token: 0x040000EF RID: 239
	public Color hover = new Color(0.88235295f, 0.78431374f, 0.5882353f, 1f);

	// Token: 0x040000F0 RID: 240
	public Color pressed = new Color(0.7176471f, 0.6392157f, 0.48235294f, 1f);

	// Token: 0x040000F1 RID: 241
	public float duration = 0.2f;

	// Token: 0x040000F2 RID: 242
	protected Color mColor;

	// Token: 0x040000F3 RID: 243
	protected bool mStarted;

	// Token: 0x040000F4 RID: 244
	protected UIWidget mWidget;
}
