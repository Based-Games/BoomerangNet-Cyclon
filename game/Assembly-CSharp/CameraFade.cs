using System;
using UnityEngine;

// Token: 0x02000219 RID: 537
public class CameraFade : MonoBehaviour
{
	// Token: 0x06000F7A RID: 3962 RVA: 0x0000D479 File Offset: 0x0000B679
	private void Awake()
	{
		this.m_FadeTexture = new Texture2D(1, 1);
		this.m_BackgroundStyle.normal.background = this.m_FadeTexture;
		this.SetScreenOverlayColor(this.m_CurrentScreenOverlayColor);
	}

	// Token: 0x06000F7B RID: 3963 RVA: 0x000705B8 File Offset: 0x0006E7B8
	private void OnGUI()
	{
		if (this.m_CurrentScreenOverlayColor != this.m_TargetScreenOverlayColor)
		{
			if (Mathf.Abs(this.m_CurrentScreenOverlayColor.a - this.m_TargetScreenOverlayColor.a) < Mathf.Abs(this.m_DeltaColor.a) * Time.deltaTime)
			{
				this.m_CurrentScreenOverlayColor = this.m_TargetScreenOverlayColor;
				this.SetScreenOverlayColor(this.m_CurrentScreenOverlayColor);
				this.m_DeltaColor = new Color(0f, 0f, 0f, 0f);
			}
			else
			{
				this.SetScreenOverlayColor(this.m_CurrentScreenOverlayColor + this.m_DeltaColor * Time.deltaTime);
			}
		}
		if (this.m_CurrentScreenOverlayColor.a > 0f)
		{
			GUI.depth = this.m_FadeGUIDepth;
			GUI.Label(new Rect(-10f, -10f, (float)(Screen.width + 10), (float)(Screen.height + 10)), this.m_FadeTexture, this.m_BackgroundStyle);
		}
	}

	// Token: 0x06000F7C RID: 3964 RVA: 0x0000D4AA File Offset: 0x0000B6AA
	public void SetScreenOverlayColor(Color newScreenOverlayColor)
	{
		this.m_CurrentScreenOverlayColor = newScreenOverlayColor;
		this.m_FadeTexture.SetPixel(0, 0, this.m_CurrentScreenOverlayColor);
		this.m_FadeTexture.Apply();
	}

	// Token: 0x06000F7D RID: 3965 RVA: 0x0000D4D1 File Offset: 0x0000B6D1
	public void StartFade(Color newScreenOverlayColor, float fadeDuration)
	{
		if (fadeDuration <= 0f)
		{
			this.SetScreenOverlayColor(newScreenOverlayColor);
		}
		else
		{
			this.m_TargetScreenOverlayColor = newScreenOverlayColor;
			this.m_DeltaColor = (this.m_TargetScreenOverlayColor - this.m_CurrentScreenOverlayColor) / fadeDuration;
		}
	}

	// Token: 0x0400114C RID: 4428
	private GUIStyle m_BackgroundStyle = new GUIStyle();

	// Token: 0x0400114D RID: 4429
	private Texture2D m_FadeTexture;

	// Token: 0x0400114E RID: 4430
	private Color m_CurrentScreenOverlayColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x0400114F RID: 4431
	private Color m_TargetScreenOverlayColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x04001150 RID: 4432
	private Color m_DeltaColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x04001151 RID: 4433
	private int m_FadeGUIDepth = -1000;
}
