using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class AVProWindowsMediaFormatConverter : IDisposable
{
	// Token: 0x1700001F RID: 31
	// (get) Token: 0x06000078 RID: 120 RVA: 0x00003B67 File Offset: 0x00001D67
	public Texture OutputTexture
	{
		get
		{
			return this._finalTexture;
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x06000079 RID: 121 RVA: 0x00003B6F File Offset: 0x00001D6F
	public int DisplayFrame
	{
		get
		{
			return this._lastFrameUploaded;
		}
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x0600007A RID: 122 RVA: 0x00003B77 File Offset: 0x00001D77
	// (set) Token: 0x0600007B RID: 123 RVA: 0x00003B7F File Offset: 0x00001D7F
	public bool ValidPicture { get; private set; }

	// Token: 0x0600007C RID: 124 RVA: 0x00003B88 File Offset: 0x00001D88
	public void Reset()
	{
		this.ValidPicture = false;
		this._lastFrameUploaded = -1;
	}

	// Token: 0x0600007D RID: 125 RVA: 0x000142FC File Offset: 0x000124FC
	public bool Build(int movieHandle, int width, int height, AVProWindowsMediaPlugin.VideoFrameFormat format, bool useBT709, bool flipX, bool flipY)
	{
		this.Reset();
		this._movieHandle = movieHandle;
		this._width = width;
		this._height = height;
		this._sourceVideoFormat = format;
		this._flipX = flipX;
		this._flipY = flipY;
		this._useBT709 = useBT709;
		if (this.CreateMaterial())
		{
			this.CreateTexture();
			if (this._rawTexture != null)
			{
				this.CreateUVs(this._flipX, this._flipY);
				switch (AVProWindowsMediaManager.Instance.TextureConversionMethod)
				{
				case AVProWindowsMediaManager.ConversionMethod.Unity4:
					AVProWindowsMediaPlugin.SetTexturePointer(this._movieHandle, this._rawTexture.GetNativeTexturePtr());
					break;
				}
				this.CreateRenderTexture();
				this._conversionMaterial.mainTexture = this._rawTexture;
				bool flag = this._sourceVideoFormat != AVProWindowsMediaPlugin.VideoFrameFormat.RAW_BGRA32;
				if (flag)
				{
					this._conversionMaterial.SetFloat("_TextureWidth", (float)this._finalTexture.width);
				}
			}
		}
		return this._conversionMaterial != null;
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00014410 File Offset: 0x00012610
	public bool Update()
	{
		bool flag = this.UpdateTexture();
		if (flag)
		{
			this.DoFormatConversion();
		}
		return flag;
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00014434 File Offset: 0x00012634
	private bool UpdateTexture()
	{
		bool flag = false;
		AVProWindowsMediaManager.ConversionMethod textureConversionMethod = AVProWindowsMediaManager.Instance.TextureConversionMethod;
		if (textureConversionMethod == AVProWindowsMediaManager.ConversionMethod.Unity4 || textureConversionMethod == AVProWindowsMediaManager.ConversionMethod.Unity35_OpenGL)
		{
			int lastFrameUploaded = AVProWindowsMediaPlugin.GetLastFrameUploaded(this._movieHandle);
			if (this._lastFrameUploaded != lastFrameUploaded)
			{
				this._lastFrameUploaded = lastFrameUploaded;
				flag = true;
			}
			return flag;
		}
		return flag;
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00014480 File Offset: 0x00012680
	public void Dispose()
	{
		this.ValidPicture = false;
		this._width = (this._height = 0);
		if (this._conversionMaterial != null)
		{
			this._conversionMaterial.mainTexture = null;
			UnityEngine.Object.Destroy(this._conversionMaterial);
			this._conversionMaterial = null;
		}
		if (this._finalTexture != null)
		{
			RenderTexture.ReleaseTemporary(this._finalTexture);
			this._finalTexture = null;
		}
		if (this._rawTexture != null)
		{
			UnityEngine.Object.Destroy(this._rawTexture);
			this._rawTexture = null;
		}
	}

	// Token: 0x06000081 RID: 129 RVA: 0x0001451C File Offset: 0x0001271C
	private bool CreateMaterial()
	{
		Shader pixelConversionShader = AVProWindowsMediaManager.Instance.GetPixelConversionShader(this._sourceVideoFormat, this._useBT709);
		if (pixelConversionShader)
		{
			if (this._conversionMaterial != null && this._conversionMaterial.shader != pixelConversionShader)
			{
				UnityEngine.Object.Destroy(this._conversionMaterial);
				this._conversionMaterial = null;
			}
			if (this._conversionMaterial == null)
			{
				this._conversionMaterial = new Material(pixelConversionShader);
				this._conversionMaterial.name = "AVProWindowsMedia-Material";
			}
		}
		return this._conversionMaterial != null;
	}

	// Token: 0x06000082 RID: 130 RVA: 0x000145C0 File Offset: 0x000127C0
	private void CreateTexture()
	{
		this._usedTextureWidth = this._width;
		this._usedTextureHeight = this._height;
		int num = this._usedTextureWidth;
		int num2 = this._usedTextureHeight;
		TextureFormat textureFormat = TextureFormat.RGBA32;
		switch (this._sourceVideoFormat)
		{
		case AVProWindowsMediaPlugin.VideoFrameFormat.YUV_422_YUY2:
		case AVProWindowsMediaPlugin.VideoFrameFormat.YUV_422_UYVY:
		case AVProWindowsMediaPlugin.VideoFrameFormat.YUV_422_YVYU:
		case AVProWindowsMediaPlugin.VideoFrameFormat.YUV_422_HDYC:
		case AVProWindowsMediaPlugin.VideoFrameFormat.YUV_420_NV12:
			textureFormat = TextureFormat.RGBA32;
			this._usedTextureWidth /= 2;
			num = this._usedTextureWidth;
			break;
		case AVProWindowsMediaPlugin.VideoFrameFormat.Hap_RGB:
			textureFormat = TextureFormat.DXT1;
			break;
		case AVProWindowsMediaPlugin.VideoFrameFormat.Hap_RGBA:
		case AVProWindowsMediaPlugin.VideoFrameFormat.Hap_RGB_HQ:
			textureFormat = TextureFormat.DXT5;
			break;
		}
		bool flag = SystemInfo.npotSupport == NPOTSupport.None;
		if (flag && (!Mathf.IsPowerOfTwo(this._width) || !Mathf.IsPowerOfTwo(this._height)))
		{
			num = Mathf.NextPowerOfTwo(num);
			num2 = Mathf.NextPowerOfTwo(num2);
		}
		if (this._rawTexture != null && (this._rawTexture.width != num || this._rawTexture.height != num2 || this._rawTexture.format != textureFormat))
		{
			UnityEngine.Object.Destroy(this._rawTexture);
			this._rawTexture = null;
		}
		if (this._rawTexture == null)
		{
			this._rawTexture = new Texture2D(num, num2, textureFormat, false, true);
			this._rawTexture.wrapMode = TextureWrapMode.Clamp;
			this._rawTexture.filterMode = FilterMode.Point;
			this._rawTexture.name = "AVProWindowsMedia-RawTexture";
		}
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00014744 File Offset: 0x00012944
	private void CreateRenderTexture()
	{
		if (this._finalTexture != null && (this._finalTexture.width != this._width || this._finalTexture.height != this._height))
		{
			RenderTexture.ReleaseTemporary(this._finalTexture);
			this._finalTexture = null;
		}
		if (this._finalTexture == null)
		{
			this.ValidPicture = false;
			this._finalTexture = RenderTexture.GetTemporary(this._width, this._height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
			this._finalTexture.wrapMode = TextureWrapMode.Clamp;
			this._finalTexture.filterMode = FilterMode.Bilinear;
			this._finalTexture.useMipMap = false;
			this._finalTexture.name = "AVProWindowsMedia-FinalTexture";
			this._finalTexture.Create();
		}
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00014814 File Offset: 0x00012A14
	private void DoFormatConversion()
	{
		if (this._finalTexture == null)
		{
			return;
		}
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = this._finalTexture;
		this._conversionMaterial.SetPass(0);
		GL.PushMatrix();
		GL.LoadOrtho();
		AVProWindowsMediaFormatConverter.DrawQuad(this._uv);
		GL.PopMatrix();
		this.ValidPicture = true;
		RenderTexture.active = active;
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00014878 File Offset: 0x00012A78
	private void CreateUVs(bool invertX, bool invertY)
	{
		float num;
		float num2;
		if (invertX)
		{
			num = 1f;
			num2 = 0f;
		}
		else
		{
			num = 0f;
			num2 = 1f;
		}
		float num3;
		float num4;
		if (invertY)
		{
			num3 = 1f;
			num4 = 0f;
		}
		else
		{
			num3 = 0f;
			num4 = 1f;
		}
		if (this._usedTextureWidth != this._rawTexture.width)
		{
			float num5 = (float)this._usedTextureWidth / (float)this._rawTexture.width;
			num *= num5;
			num2 *= num5;
		}
		if (this._usedTextureHeight != this._rawTexture.height)
		{
			float num6 = (float)this._usedTextureHeight / (float)this._rawTexture.height;
			num3 *= num6;
			num4 *= num6;
		}
		this._uv = new Vector4(num, num3, num2, num4);
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00014948 File Offset: 0x00012B48
	private static void DrawQuad(Vector4 uv)
	{
		GL.Begin(7);
		GL.TexCoord2(uv.x, uv.y);
		GL.Vertex3(0f, 0f, 0.1f);
		GL.TexCoord2(uv.z, uv.y);
		GL.Vertex3(1f, 0f, 0.1f);
		GL.TexCoord2(uv.z, uv.w);
		GL.Vertex3(1f, 1f, 0.1f);
		GL.TexCoord2(uv.x, uv.w);
		GL.Vertex3(0f, 1f, 0.1f);
		GL.End();
	}

	// Token: 0x04000056 RID: 86
	private int _movieHandle;

	// Token: 0x04000057 RID: 87
	private Texture2D _rawTexture;

	// Token: 0x04000058 RID: 88
	private RenderTexture _finalTexture;

	// Token: 0x04000059 RID: 89
	private Material _conversionMaterial;

	// Token: 0x0400005A RID: 90
	private int _usedTextureWidth;

	// Token: 0x0400005B RID: 91
	private int _usedTextureHeight;

	// Token: 0x0400005C RID: 92
	private Vector4 _uv;

	// Token: 0x0400005D RID: 93
	private int _lastFrameUploaded = -1;

	// Token: 0x0400005E RID: 94
	private int _width;

	// Token: 0x0400005F RID: 95
	private int _height;

	// Token: 0x04000060 RID: 96
	private bool _flipX;

	// Token: 0x04000061 RID: 97
	private bool _flipY;

	// Token: 0x04000062 RID: 98
	private AVProWindowsMediaPlugin.VideoFrameFormat _sourceVideoFormat;

	// Token: 0x04000063 RID: 99
	private bool _useBT709;
}
