using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000006 RID: 6
[AddComponentMenu("AVPro Windows Media/Manager (required)")]
public class AVProWindowsMediaManager : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000010 RID: 16 RVA: 0x00013684 File Offset: 0x00011884
	public static AVProWindowsMediaManager Instance
	{
		get
		{
			if (AVProWindowsMediaManager._instance == null)
			{
				AVProWindowsMediaManager._instance = (AVProWindowsMediaManager)UnityEngine.Object.FindObjectOfType(typeof(AVProWindowsMediaManager));
				if (AVProWindowsMediaManager._instance == null)
				{
					Logger.Error("AVProWindowsMedia", "component required", new object[0]);
					return null;
				}
				if (!AVProWindowsMediaManager._instance._isInitialised)
				{
					AVProWindowsMediaManager._instance.Init();
				}
			}
			return AVProWindowsMediaManager._instance;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000011 RID: 17 RVA: 0x0000360A File Offset: 0x0000180A
	public AVProWindowsMediaManager.ConversionMethod TextureConversionMethod
	{
		get
		{
			return this._conversionMethod;
		}
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00003612 File Offset: 0x00001812
	private void Start()
	{
		if (!this._isInitialised)
		{
			AVProWindowsMediaManager._instance = this;
			this.Init();
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00003629 File Offset: 0x00001829
	private void OnDestroy()
	{
		this.Deinit();
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000136F8 File Offset: 0x000118F8
	protected bool Init()
	{
		try
		{
			if (!AVProWindowsMediaPlugin.Init())
			{
				base.enabled = false;
				this.Deinit();
				return false;
			}
		}
		catch (DllNotFoundException ex)
		{
			throw ex;
		}
		this.GetConversionMethod();
		this.SetUnityFeatures();
		base.StartCoroutine("FinalRenderCapture");
		this._isInitialised = true;
		return this._isInitialised;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00013758 File Offset: 0x00011958
	private void SetUnityFeatures()
	{
		bool flag = false;
		bool flag2 = false;
		AVProWindowsMediaPlugin.SetUnityFeatures(flag, flag2);
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00013770 File Offset: 0x00011970
	private void GetConversionMethod()
	{
		bool flag = false;
		this._conversionMethod = AVProWindowsMediaManager.ConversionMethod.UnityScript;
		this._conversionMethod = AVProWindowsMediaManager.ConversionMethod.Unity4;
		if (SystemInfo.graphicsDeviceVersion.StartsWith("Direct3D 11"))
		{
			flag = true;
		}
		if (flag)
		{
			Shader.DisableKeyword("SWAP_RED_BLUE_OFF");
			Shader.EnableKeyword("SWAP_RED_BLUE_ON");
		}
		else
		{
			Shader.DisableKeyword("SWAP_RED_BLUE_ON");
			Shader.EnableKeyword("SWAP_RED_BLUE_OFF");
		}
		Shader.DisableKeyword("AVPRO_GAMMACORRECTION");
		Shader.EnableKeyword("AVPRO_GAMMACORRECTION_OFF");
		if (QualitySettings.activeColorSpace == ColorSpace.Linear)
		{
			Shader.DisableKeyword("AVPRO_GAMMACORRECTION_OFF");
			Shader.EnableKeyword("AVPRO_GAMMACORRECTION");
		}
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00003631 File Offset: 0x00001831
	private IEnumerator FinalRenderCapture()
	{
		while (Application.isPlaying)
		{
			GL.IssuePluginEvent(262209536);
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00013800 File Offset: 0x00011A00
	public void Deinit()
	{
		AVProWindowsMediaMovie[] array = (AVProWindowsMediaMovie[])UnityEngine.Object.FindObjectsOfType(typeof(AVProWindowsMediaMovie));
		if (array != null && array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i].UnloadMovie();
			}
		}
		AVProWindowsMediaManager._instance = null;
		this._isInitialised = false;
		AVProWindowsMediaPlugin.Deinit();
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00013854 File Offset: 0x00011A54
	public Shader GetPixelConversionShader(AVProWindowsMediaPlugin.VideoFrameFormat format, bool useBT709)
	{
		Shader shader = null;
		switch (format)
		{
		case AVProWindowsMediaPlugin.VideoFrameFormat.RAW_BGRA32:
			return this._shaderBGRA32;
		case AVProWindowsMediaPlugin.VideoFrameFormat.YUV_422_YUY2:
			shader = this._shaderYUY2;
			if (useBT709)
			{
				shader = this._shaderYUY2_709;
			}
			return shader;
		case AVProWindowsMediaPlugin.VideoFrameFormat.YUV_422_UYVY:
			shader = this._shaderUYVY;
			if (useBT709)
			{
				shader = this._shaderHDYC;
			}
			return shader;
		case AVProWindowsMediaPlugin.VideoFrameFormat.YUV_422_YVYU:
			return this._shaderYVYU;
		case AVProWindowsMediaPlugin.VideoFrameFormat.YUV_422_HDYC:
			return this._shaderHDYC;
		case AVProWindowsMediaPlugin.VideoFrameFormat.YUV_420_NV12:
			return this._shaderNV12;
		case AVProWindowsMediaPlugin.VideoFrameFormat.Hap_RGB:
			return this._shaderCopy;
		case AVProWindowsMediaPlugin.VideoFrameFormat.Hap_RGBA:
			return this._shaderCopy;
		case AVProWindowsMediaPlugin.VideoFrameFormat.Hap_RGB_HQ:
			return this._shaderHap_YCoCg;
		}
		Logger.Error("AVProWindowsMedia", "Unknown pixel format '" + format.ToString(), new object[0]);
		return shader;
	}

	// Token: 0x0400001A RID: 26
	private static AVProWindowsMediaManager _instance;

	// Token: 0x0400001B RID: 27
	public Shader _shaderBGRA32;

	// Token: 0x0400001C RID: 28
	public Shader _shaderYUY2;

	// Token: 0x0400001D RID: 29
	public Shader _shaderYUY2_709;

	// Token: 0x0400001E RID: 30
	public Shader _shaderUYVY;

	// Token: 0x0400001F RID: 31
	public Shader _shaderYVYU;

	// Token: 0x04000020 RID: 32
	public Shader _shaderHDYC;

	// Token: 0x04000021 RID: 33
	public Shader _shaderNV12;

	// Token: 0x04000022 RID: 34
	public Shader _shaderCopy;

	// Token: 0x04000023 RID: 35
	public Shader _shaderHap_YCoCg;

	// Token: 0x04000024 RID: 36
	private bool _isInitialised;

	// Token: 0x04000025 RID: 37
	private AVProWindowsMediaManager.ConversionMethod _conversionMethod;

	// Token: 0x02000007 RID: 7
	public enum ConversionMethod
	{
		// Token: 0x04000027 RID: 39
		Unknown,
		// Token: 0x04000028 RID: 40
		Unity4,
		// Token: 0x04000029 RID: 41
		Unity35_OpenGL,
		// Token: 0x0400002A RID: 42
		Unity34_OpenGL,
		// Token: 0x0400002B RID: 43
		UnityScript
	}
}
