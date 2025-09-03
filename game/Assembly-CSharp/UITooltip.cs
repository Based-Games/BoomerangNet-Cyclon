using System;
using UnityEngine;

// Token: 0x020000C7 RID: 199
[AddComponentMenu("NGUI/UI/Tooltip")]
public class UITooltip : MonoBehaviour
{
	// Token: 0x1700015A RID: 346
	// (get) Token: 0x0600067F RID: 1663 RVA: 0x000083C9 File Offset: 0x000065C9
	public static bool isVisible
	{
		get
		{
			return UITooltip.mInstance != null && UITooltip.mInstance.mTarget == 1f;
		}
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x000083EF File Offset: 0x000065EF
	private void Awake()
	{
		UITooltip.mInstance = this;
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x000083F7 File Offset: 0x000065F7
	private void OnDestroy()
	{
		UITooltip.mInstance = null;
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x00033300 File Offset: 0x00031500
	protected virtual void Start()
	{
		this.mTrans = base.transform;
		this.mWidgets = base.GetComponentsInChildren<UIWidget>();
		this.mPos = this.mTrans.localPosition;
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
		this.SetAlpha(0f);
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x00033368 File Offset: 0x00031568
	protected virtual void Update()
	{
		if (this.mCurrent != this.mTarget)
		{
			this.mCurrent = Mathf.Lerp(this.mCurrent, this.mTarget, RealTime.deltaTime * this.appearSpeed);
			if (Mathf.Abs(this.mCurrent - this.mTarget) < 0.001f)
			{
				this.mCurrent = this.mTarget;
			}
			this.SetAlpha(this.mCurrent * this.mCurrent);
			if (this.scalingTransitions)
			{
				Vector3 vector = this.mSize * 0.25f;
				vector.y = -vector.y;
				Vector3 vector2 = Vector3.one * (1.5f - this.mCurrent * 0.5f);
				Vector3 vector3 = Vector3.Lerp(this.mPos - vector, this.mPos, this.mCurrent);
				this.mTrans.localPosition = vector3;
				this.mTrans.localScale = vector2;
			}
		}
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x00033464 File Offset: 0x00031664
	protected virtual void SetAlpha(float val)
	{
		int i = 0;
		int num = this.mWidgets.Length;
		while (i < num)
		{
			UIWidget uiwidget = this.mWidgets[i];
			Color color = uiwidget.color;
			color.a = val;
			uiwidget.color = color;
			i++;
		}
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x000334AC File Offset: 0x000316AC
	protected virtual void SetText(string tooltipText)
	{
		if (this.text != null && !string.IsNullOrEmpty(tooltipText))
		{
			this.mTarget = 1f;
			if (this.text != null)
			{
				this.text.text = tooltipText;
			}
			this.mPos = Input.mousePosition;
			if (this.background != null)
			{
				Transform transform = this.text.transform;
				Vector3 localPosition = transform.localPosition;
				Vector3 localScale = transform.localScale;
				this.mSize = this.text.printedSize;
				this.mSize.x = this.mSize.x * localScale.x;
				this.mSize.y = this.mSize.y * localScale.y;
				Vector4 border = this.background.border;
				this.mSize.x = this.mSize.x + (border.x + border.z + (localPosition.x - border.x) * 2f);
				this.mSize.y = this.mSize.y + (border.y + border.w + (-localPosition.y - border.y) * 2f);
				this.background.width = Mathf.RoundToInt(this.mSize.x);
				this.background.height = Mathf.RoundToInt(this.mSize.y);
			}
			if (this.uiCamera != null)
			{
				this.mPos.x = Mathf.Clamp01(this.mPos.x / (float)Screen.width);
				this.mPos.y = Mathf.Clamp01(this.mPos.y / (float)Screen.height);
				float num = this.uiCamera.orthographicSize / this.mTrans.parent.lossyScale.y;
				float num2 = (float)Screen.height * 0.5f / num;
				Vector2 vector = new Vector2(num2 * this.mSize.x / (float)Screen.width, num2 * this.mSize.y / (float)Screen.height);
				this.mPos.x = Mathf.Min(this.mPos.x, 1f - vector.x);
				this.mPos.y = Mathf.Max(this.mPos.y, vector.y);
				this.mTrans.position = this.uiCamera.ViewportToWorldPoint(this.mPos);
				this.mPos = this.mTrans.localPosition;
				this.mPos.x = Mathf.Round(this.mPos.x);
				this.mPos.y = Mathf.Round(this.mPos.y);
				this.mTrans.localPosition = this.mPos;
			}
			else
			{
				if (this.mPos.x + this.mSize.x > (float)Screen.width)
				{
					this.mPos.x = (float)Screen.width - this.mSize.x;
				}
				if (this.mPos.y - this.mSize.y < 0f)
				{
					this.mPos.y = this.mSize.y;
				}
				this.mPos.x = this.mPos.x - (float)Screen.width * 0.5f;
				this.mPos.y = this.mPos.y - (float)Screen.height * 0.5f;
			}
		}
		else
		{
			this.mTarget = 0f;
		}
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x000083FF File Offset: 0x000065FF
	public static void ShowText(string tooltipText)
	{
		if (UITooltip.mInstance != null)
		{
			UITooltip.mInstance.SetText(tooltipText);
		}
	}

	// Token: 0x04000510 RID: 1296
	protected static UITooltip mInstance;

	// Token: 0x04000511 RID: 1297
	public Camera uiCamera;

	// Token: 0x04000512 RID: 1298
	public UILabel text;

	// Token: 0x04000513 RID: 1299
	public UISprite background;

	// Token: 0x04000514 RID: 1300
	public float appearSpeed = 10f;

	// Token: 0x04000515 RID: 1301
	public bool scalingTransitions = true;

	// Token: 0x04000516 RID: 1302
	protected Transform mTrans;

	// Token: 0x04000517 RID: 1303
	protected float mTarget;

	// Token: 0x04000518 RID: 1304
	protected float mCurrent;

	// Token: 0x04000519 RID: 1305
	protected Vector3 mPos;

	// Token: 0x0400051A RID: 1306
	protected Vector3 mSize = Vector3.zero;

	// Token: 0x0400051B RID: 1307
	protected UIWidget[] mWidgets;
}
