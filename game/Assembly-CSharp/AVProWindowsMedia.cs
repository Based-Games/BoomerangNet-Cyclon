using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class AVProWindowsMedia : IDisposable
{
	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000047 RID: 71 RVA: 0x0000391F File Offset: 0x00001B1F
	public int Handle
	{
		get
		{
			return this._movieHandle;
		}
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000048 RID: 72 RVA: 0x00003927 File Offset: 0x00001B27
	// (set) Token: 0x06000049 RID: 73 RVA: 0x0000392F File Offset: 0x00001B2F
	public string Filename { get; private set; }

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x0600004A RID: 74 RVA: 0x00003938 File Offset: 0x00001B38
	// (set) Token: 0x0600004B RID: 75 RVA: 0x00003940 File Offset: 0x00001B40
	public bool CompleteLoad { get; set; }

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x0600004C RID: 76 RVA: 0x00003949 File Offset: 0x00001B49
	// (set) Token: 0x0600004D RID: 77 RVA: 0x00003951 File Offset: 0x00001B51
	public int Width { get; private set; }

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x0600004E RID: 78 RVA: 0x0000395A File Offset: 0x00001B5A
	// (set) Token: 0x0600004F RID: 79 RVA: 0x00003962 File Offset: 0x00001B62
	public int Height { get; private set; }

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000050 RID: 80 RVA: 0x0000396B File Offset: 0x00001B6B
	public float AspectRatio
	{
		get
		{
			return (float)this.Width / (float)this.Height;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000051 RID: 81 RVA: 0x0000397C File Offset: 0x00001B7C
	// (set) Token: 0x06000052 RID: 82 RVA: 0x00003984 File Offset: 0x00001B84
	public float FrameRate { get; private set; }

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000053 RID: 83 RVA: 0x0000398D File Offset: 0x00001B8D
	// (set) Token: 0x06000054 RID: 84 RVA: 0x00003995 File Offset: 0x00001B95
	public float DurationSeconds { get; private set; }

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x06000055 RID: 85 RVA: 0x0000399E File Offset: 0x00001B9E
	// (set) Token: 0x06000056 RID: 86 RVA: 0x000039A6 File Offset: 0x00001BA6
	public uint DurationFrames { get; private set; }

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000057 RID: 87 RVA: 0x000039AF File Offset: 0x00001BAF
	// (set) Token: 0x06000058 RID: 88 RVA: 0x000039B7 File Offset: 0x00001BB7
	public bool IsPlaying { get; private set; }

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x0600005A RID: 90 RVA: 0x000039D5 File Offset: 0x00001BD5
	// (set) Token: 0x06000059 RID: 89 RVA: 0x000039C0 File Offset: 0x00001BC0
	public bool Loop
	{
		get
		{
			return this._isLooping;
		}
		set
		{
			this._isLooping = value;
			AVProWindowsMediaPlugin.SetLooping(this._movieHandle, value);
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x0600005C RID: 92 RVA: 0x000039F7 File Offset: 0x00001BF7
	// (set) Token: 0x0600005B RID: 91 RVA: 0x000039DD File Offset: 0x00001BDD
	public int AudioDelay
	{
		get
		{
			return this._audioDelay;
		}
		set
		{
			this._audioDelay = value;
			AVProWindowsMediaPlugin.SetAudioDelay(this._movieHandle, this._audioDelay);
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x0600005E RID: 94 RVA: 0x00003A19 File Offset: 0x00001C19
	// (set) Token: 0x0600005D RID: 93 RVA: 0x000039FF File Offset: 0x00001BFF
	public float Volume
	{
		get
		{
			return this._volume;
		}
		set
		{
			this._volume = value;
			AVProWindowsMediaPlugin.SetVolume(this._movieHandle, this._volume);
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000060 RID: 96 RVA: 0x00003A2F File Offset: 0x00001C2F
	// (set) Token: 0x0600005F RID: 95 RVA: 0x00003A21 File Offset: 0x00001C21
	public float PlaybackRate
	{
		get
		{
			return AVProWindowsMediaPlugin.GetPlaybackRate(this._movieHandle);
		}
		set
		{
			AVProWindowsMediaPlugin.SetPlaybackRate(this._movieHandle, value);
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000061 RID: 97 RVA: 0x00003A3C File Offset: 0x00001C3C
	// (set) Token: 0x06000062 RID: 98 RVA: 0x00003A49 File Offset: 0x00001C49
	public float PositionSeconds
	{
		get
		{
			return AVProWindowsMediaPlugin.GetCurrentPositionSeconds(this._movieHandle);
		}
		set
		{
			AVProWindowsMediaPlugin.SeekSeconds(this._movieHandle, value);
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000063 RID: 99 RVA: 0x00003A57 File Offset: 0x00001C57
	// (set) Token: 0x06000064 RID: 100 RVA: 0x00003A5F File Offset: 0x00001C5F
	public uint PositionFrames
	{
		get
		{
			return (uint)this.DisplayFrame;
		}
		set
		{
			AVProWindowsMediaPlugin.SeekFrames(this._movieHandle, value);
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000065 RID: 101 RVA: 0x00003A6D File Offset: 0x00001C6D
	// (set) Token: 0x06000066 RID: 102 RVA: 0x00003A7A File Offset: 0x00001C7A
	public float AudioBalance
	{
		get
		{
			return AVProWindowsMediaPlugin.GetAudioBalance(this._movieHandle);
		}
		set
		{
			AVProWindowsMediaPlugin.SetAudioBalance(this._movieHandle, value);
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000067 RID: 103 RVA: 0x00003A88 File Offset: 0x00001C88
	public bool IsFinishedPlaying
	{
		get
		{
			return AVProWindowsMediaPlugin.IsFinishedPlaying(this._movieHandle);
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000068 RID: 104 RVA: 0x00003A95 File Offset: 0x00001C95
	public Texture OutputTexture
	{
		get
		{
			if (this._formatConverter != null && this._formatConverter.ValidPicture)
			{
				return this._formatConverter.OutputTexture;
			}
			return null;
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000069 RID: 105 RVA: 0x00003AB9 File Offset: 0x00001CB9
	public int DisplayFrame
	{
		get
		{
			if (this._formatConverter != null && this._formatConverter.ValidPicture)
			{
				return this._formatConverter.DisplayFrame;
			}
			return -1;
		}
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00013DE4 File Offset: 0x00011FE4
	public bool StartVideo(string filename, bool allowNativeFormat, bool useBT709, bool useAudioDelay, bool useAudioMixer, bool useDisplaySync)
	{
		this.Filename = filename;
		if (!string.IsNullOrEmpty(this.Filename))
		{
			if (this._movieHandle < 0)
			{
				this._movieHandle = AVProWindowsMediaPlugin.GetInstanceHandle();
			}
			IntPtr intPtr = Marshal.StringToHGlobalUni(this.Filename);
			if (AVProWindowsMediaPlugin.LoadMovie(this._movieHandle, intPtr, false, allowNativeFormat, useAudioDelay, useAudioMixer, useDisplaySync))
			{
				this.CompleteLoad = true;
				this.CompleteVideoLoad(useBT709);
			}
			else
			{
				Logger.Error("AVProWindowsMedia", "Movie failed to load", new object[0]);
				this.Close();
			}
			Marshal.FreeHGlobal(intPtr);
		}
		else
		{
			Logger.Error("AVProWindowsMedia", "No movie file specified", new object[0]);
			this.Close();
		}
		return this._movieHandle >= 0;
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00013E98 File Offset: 0x00012098
	public bool StartVideoFromMemory(string name, IntPtr moviePointer, uint movieLength, bool allowNativeFormat, bool useBT709, bool useAudioDelay, bool useAudioMixer, bool useDisplaySync)
	{
		this.Filename = name;
		if (moviePointer != IntPtr.Zero && movieLength > 0U)
		{
			if (this._movieHandle < 0)
			{
				this._movieHandle = AVProWindowsMediaPlugin.GetInstanceHandle();
			}
			if (AVProWindowsMediaPlugin.LoadMovieFromMemory(this._movieHandle, moviePointer, movieLength, allowNativeFormat, useAudioDelay, useAudioMixer, useDisplaySync))
			{
				this.CompleteVideoLoad(useBT709);
			}
			else
			{
				Logger.Error("AVProWindowsMedia", "Movie failed to load", new object[0]);
				this.Close();
			}
		}
		else
		{
			Logger.Error("AVProWindowsMedia", "No movie file specified", new object[0]);
			this.Close();
		}
		return this._movieHandle >= 0;
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00013F38 File Offset: 0x00012138
	private void CompleteVideoLoad(bool useBT709)
	{
		this.Loop = false;
		this.Volume = this._volume;
		this.Width = AVProWindowsMediaPlugin.GetWidth(this._movieHandle);
		this.Height = AVProWindowsMediaPlugin.GetHeight(this._movieHandle);
		this.FrameRate = AVProWindowsMediaPlugin.GetFrameRate(this._movieHandle);
		this.DurationSeconds = AVProWindowsMediaPlugin.GetDurationSeconds(this._movieHandle);
		this.DurationFrames = AVProWindowsMediaPlugin.GetDurationFrames(this._movieHandle);
		AVProWindowsMediaPlugin.VideoFrameFormat format = (AVProWindowsMediaPlugin.VideoFrameFormat)AVProWindowsMediaPlugin.GetFormat(this._movieHandle);
		if ((this.Width <= 0 || this.Width > 8192) && (this.Height <= 0 || this.Height > 8192))
		{
			int num = 0;
			this.Height = num;
			this.Width = num;
			if (this._formatConverter != null)
			{
				this._formatConverter.Dispose();
				this._formatConverter = null;
			}
		}
		else
		{
			bool flag = AVProWindowsMediaPlugin.IsOrientedTopDown(this._movieHandle);
			if (this._formatConverter == null)
			{
				this._formatConverter = new AVProWindowsMediaFormatConverter();
			}
			if (!this._formatConverter.Build(this._movieHandle, this.Width, this.Height, format, useBT709, false, flag))
			{
				Logger.Error("AVProWindowsMedia", "Unable to convert video format", new object[0]);
				int num2 = 0;
				this.Height = num2;
				this.Width = num2;
				if (this._formatConverter != null)
				{
					this._formatConverter.Dispose();
					this._formatConverter = null;
					this.Close();
				}
			}
		}
		Logger.Log("AVProWindowsMedia", "Movie loaded: " + this.Filename, new object[0]);
		this.PreRoll();
	}

	// Token: 0x0600006D RID: 109 RVA: 0x000140C4 File Offset: 0x000122C4
	public bool StartAudio(string filename)
	{
		this.Filename = filename;
		int num = 0;
		this.Height = num;
		this.Width = num;
		if (!string.IsNullOrEmpty(this.Filename))
		{
			if (this._movieHandle < 0)
			{
				this._movieHandle = AVProWindowsMediaPlugin.GetInstanceHandle();
			}
			if (this._formatConverter != null)
			{
				this._formatConverter.Dispose();
				this._formatConverter = null;
			}
			IntPtr intPtr = Marshal.StringToHGlobalUni(this.Filename);
			if (AVProWindowsMediaPlugin.LoadMovie(this._movieHandle, intPtr, false, false, false, false, false))
			{
				this.Volume = this._volume;
				this.DurationSeconds = AVProWindowsMediaPlugin.GetDurationSeconds(this._movieHandle);
				Logger.Log("AVProWindowsMedia", string.Concat(new string[]
				{
					"Loaded audio ",
					this.Filename,
					" ",
					this.DurationSeconds.ToString("F2"),
					" sec"
				}), new object[0]);
			}
			else
			{
				Logger.Error("AVProWindowsMedia", "Movie failed to load", new object[0]);
				this.Close();
			}
			Marshal.FreeHGlobal(intPtr);
		}
		else
		{
			Logger.Error("AVProWindowsMedia", "No movie file specified", new object[0]);
			this.Close();
		}
		return this._movieHandle >= 0;
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00003ADD File Offset: 0x00001CDD
	private void PreRoll()
	{
		int movieHandle = this._movieHandle;
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00014200 File Offset: 0x00012400
	public bool Update(bool force)
	{
		bool flag = false;
		if (this._movieHandle >= 0)
		{
			AVProWindowsMediaPlugin.Update(this._movieHandle);
			if (this._formatConverter != null)
			{
				bool flag2 = true;
				if (AVProWindowsMediaManager.Instance.TextureConversionMethod == AVProWindowsMediaManager.ConversionMethod.Unity35_OpenGL)
				{
					flag2 = true;
				}
				else if (!force)
				{
					flag2 = AVProWindowsMediaPlugin.IsNextFrameReadyForGrab(this._movieHandle);
				}
				if (flag2)
				{
					flag = this._formatConverter.Update();
				}
			}
			else
			{
				flag = false;
			}
		}
		return flag;
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00014264 File Offset: 0x00012464
	public int LastUpdateFrame()
	{
		int num = 0;
		if (this._movieHandle >= 0)
		{
			num = AVProWindowsMediaPlugin.GetLastFrameUploaded(this._movieHandle);
		}
		return num;
	}

	// Token: 0x06000071 RID: 113 RVA: 0x0001428C File Offset: 0x0001248C
	public int CurrentUpdateFrame()
	{
		int num = 0;
		if (this._movieHandle >= 0)
		{
			num = this.DisplayFrame;
		}
		return num;
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00003AE6 File Offset: 0x00001CE6
	public void Play()
	{
		if (this._movieHandle >= 0)
		{
			AVProWindowsMediaPlugin.Play(this._movieHandle);
			this.IsPlaying = true;
		}
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00003B03 File Offset: 0x00001D03
	public void Pause()
	{
		if (this._movieHandle >= 0)
		{
			AVProWindowsMediaPlugin.Pause(this._movieHandle);
			this.IsPlaying = false;
		}
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00003B20 File Offset: 0x00001D20
	public void Rewind()
	{
		if (this._movieHandle >= 0)
		{
			this.PositionSeconds = 0f;
		}
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00003B36 File Offset: 0x00001D36
	public void Dispose()
	{
		this.Close();
		if (this._formatConverter != null)
		{
			this._formatConverter.Dispose();
			this._formatConverter = null;
		}
	}

	// Token: 0x06000076 RID: 118 RVA: 0x000142AC File Offset: 0x000124AC
	private void Close()
	{
		this.CompleteLoad = false;
		this.Pause();
		AVProWindowsMediaPlugin.Stop(this._movieHandle);
		int num = 0;
		this.Height = num;
		this.Width = num;
		if (this._movieHandle >= 0)
		{
			AVProWindowsMediaPlugin.FreeInstanceHandle(this._movieHandle);
			this._movieHandle = -1;
		}
	}

	// Token: 0x04000049 RID: 73
	private int _movieHandle = -1;

	// Token: 0x0400004A RID: 74
	private AVProWindowsMediaFormatConverter _formatConverter;

	// Token: 0x0400004B RID: 75
	private bool _isLooping;

	// Token: 0x0400004C RID: 76
	private int _audioDelay;

	// Token: 0x0400004D RID: 77
	private float _volume = 1f;
}
