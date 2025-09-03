using System;
using UnityEngine;

// Token: 0x02000014 RID: 20
[RequireComponent(typeof(UISprite))]
[AddComponentMenu("NGUI/Examples/UI Cursor")]
public class UICursor : MonoBehaviour
{
	// Token: 0x060000B7 RID: 183 RVA: 0x00003BA0 File Offset: 0x00001DA0
	private void Awake()
	{
		UICursor.mInstance = this;
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00003BA8 File Offset: 0x00001DA8
	private void OnDestroy()
	{
		UICursor.mInstance = null;
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x000149FC File Offset: 0x00012BFC
	private void Start()
	{
		this.mTrans = base.transform;
		this.mSprite = base.GetComponentInChildren<UISprite>();
		this.mAtlas = this.mSprite.atlas;
		this.mSpriteName = this.mSprite.spriteName;
		this.mSprite.depth = 100;
		if (this.uiCamera == null)
		{
			this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
		}
	}

	// Token: 0x060000BA RID: 186 RVA: 0x00014A78 File Offset: 0x00012C78
	private void Update()
	{
		if (this.mSprite.atlas != null)
		{
			Vector3 mousePosition = Input.mousePosition;
			if (this.uiCamera != null)
			{
				mousePosition.x = Mathf.Clamp01(mousePosition.x / (float)Screen.width);
				mousePosition.y = Mathf.Clamp01(mousePosition.y / (float)Screen.height);
				this.mTrans.position = this.uiCamera.ViewportToWorldPoint(mousePosition);
				if (this.uiCamera.isOrthoGraphic)
				{
					Vector3 localPosition = this.mTrans.localPosition;
					localPosition.x = Mathf.Round(localPosition.x);
					localPosition.y = Mathf.Round(localPosition.y);
					this.mTrans.localPosition = localPosition;
				}
			}
			else
			{
				mousePosition.x -= (float)Screen.width * 0.5f;
				mousePosition.y -= (float)Screen.height * 0.5f;
				mousePosition.x = Mathf.Round(mousePosition.x);
				mousePosition.y = Mathf.Round(mousePosition.y);
				this.mTrans.localPosition = mousePosition;
			}
		}
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00003BB0 File Offset: 0x00001DB0
	public static void Clear()
	{
		UICursor.Set(UICursor.mInstance.mAtlas, UICursor.mInstance.mSpriteName);
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00014BB8 File Offset: 0x00012DB8
	public static void Set(UIAtlas atlas, string sprite)
	{
		if (UICursor.mInstance != null)
		{
			UICursor.mInstance.mSprite.atlas = atlas;
			UICursor.mInstance.mSprite.spriteName = sprite;
			UICursor.mInstance.mSprite.MakePixelPerfect();
			UICursor.mInstance.Update();
		}
	}

	// Token: 0x04000072 RID: 114
	private static UICursor mInstance;

	// Token: 0x04000073 RID: 115
	public Camera uiCamera;

	// Token: 0x04000074 RID: 116
	private Transform mTrans;

	// Token: 0x04000075 RID: 117
	private UISprite mSprite;

	// Token: 0x04000076 RID: 118
	private UIAtlas mAtlas;

	// Token: 0x04000077 RID: 119
	private string mSpriteName;
}
