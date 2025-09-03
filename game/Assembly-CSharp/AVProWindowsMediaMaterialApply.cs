using System;
using UnityEngine;

// Token: 0x0200000A RID: 10
[AddComponentMenu("AVPro Windows Media/Material Apply")]
public class AVProWindowsMediaMaterialApply : MonoBehaviour
{
	// Token: 0x06000027 RID: 39 RVA: 0x000139C4 File Offset: 0x00011BC4
	private static void CreateTexture()
	{
		AVProWindowsMediaMaterialApply._blackTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false, false);
		AVProWindowsMediaMaterialApply._blackTexture.name = "AVProWindowsMedia-BlackTexture";
		AVProWindowsMediaMaterialApply._blackTexture.filterMode = FilterMode.Point;
		AVProWindowsMediaMaterialApply._blackTexture.wrapMode = TextureWrapMode.Clamp;
		AVProWindowsMediaMaterialApply._blackTexture.hideFlags = HideFlags.HideAndDontSave;
		AVProWindowsMediaMaterialApply._blackTexture.SetPixel(0, 0, Color.black);
		AVProWindowsMediaMaterialApply._blackTexture.Apply(false, true);
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00003670 File Offset: 0x00001870
	private void OnDestroy()
	{
		this._defaultTexture = null;
		if (AVProWindowsMediaMaterialApply._blackTexture != null)
		{
			UnityEngine.Object.Destroy(AVProWindowsMediaMaterialApply._blackTexture);
			AVProWindowsMediaMaterialApply._blackTexture = null;
		}
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00003699 File Offset: 0x00001899
	private void Start()
	{
		if (AVProWindowsMediaMaterialApply._blackTexture == null)
		{
			AVProWindowsMediaMaterialApply.CreateTexture();
		}
		if (this._defaultTexture == null)
		{
			this._defaultTexture = AVProWindowsMediaMaterialApply._blackTexture;
		}
		this.Update();
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00013A30 File Offset: 0x00011C30
	private void Update()
	{
		if (this._movie != null)
		{
			if (this._movie.OutputTexture != null)
			{
				this.ApplyMapping(this._movie.OutputTexture);
			}
			else
			{
				this.ApplyMapping(this._defaultTexture);
			}
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x000036D2 File Offset: 0x000018D2
	private void ApplyMapping(Texture texture)
	{
		if (this._material != null)
		{
			this._material.mainTexture = texture;
		}
	}

	// Token: 0x0600002C RID: 44 RVA: 0x000036F1 File Offset: 0x000018F1
	public void OnDisable()
	{
		this.ApplyMapping(null);
	}

	// Token: 0x04000030 RID: 48
	public UITexture _material;

	// Token: 0x04000031 RID: 49
	public AVProWindowsMediaMovie _movie;

	// Token: 0x04000032 RID: 50
	public string _textureName;

	// Token: 0x04000033 RID: 51
	public Texture2D _defaultTexture;

	// Token: 0x04000034 RID: 52
	private static Texture2D _blackTexture;
}
