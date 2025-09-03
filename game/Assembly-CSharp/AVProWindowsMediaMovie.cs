using System;
using System.IO;
using UnityEngine;

// Token: 0x0200000C RID: 12
[AddComponentMenu("AVPro Windows Media/Movie")]
public class AVProWindowsMediaMovie : MonoBehaviour
{
	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000033 RID: 51 RVA: 0x0000377B File Offset: 0x0000197B
	public Texture OutputTexture
	{
		get
		{
			if (this._moviePlayer != null)
			{
				return this._moviePlayer.OutputTexture;
			}
			return null;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000034 RID: 52 RVA: 0x00003792 File Offset: 0x00001992
	public AVProWindowsMedia MovieInstance
	{
		get
		{
			return this._moviePlayer;
		}
	}

	// Token: 0x06000035 RID: 53 RVA: 0x0000379A File Offset: 0x0000199A
	public virtual void Start()
	{
		if (null == AVProWindowsMediaManager.Instance)
		{
			throw new Exception("You need to add AVProWindowsMediaManager component to your scene.");
		}
		if (this._loadOnStart)
		{
			this.LoadMovie(this._playOnStart);
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000036 RID: 54 RVA: 0x000037C9 File Offset: 0x000019C9
	public bool CompleteLoad
	{
		get
		{
			return this._moviePlayer.CompleteLoad;
		}
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00013AD4 File Offset: 0x00011CD4
	public bool LoadMovie(bool autoPlay)
	{
		bool flag = true;
		if (this._moviePlayer == null)
		{
			this._moviePlayer = new AVProWindowsMedia();
		}
		bool flag2 = this._colourFormat > AVProWindowsMediaMovie.ColourFormat.RGBA32;
		this._moviePlayer.CompleteLoad = false;
		string text = Path.Combine(this._folder, this._filename);
		if (!Application.isEditor && !Path.IsPathRooted(text))
		{
			text = Path.Combine(Path.GetFullPath(Path.Combine(Application.dataPath, "..")), text);
		}
		if (this._moviePlayer.StartVideo(text, flag2, this._colourFormat == AVProWindowsMediaMovie.ColourFormat.YCbCr_HD, this._useAudioDelay, this._useAudioMixer, this._useDisplaySync))
		{
			this._moviePlayer.Volume = this._volume;
			this._moviePlayer.Loop = this._loop;
			if (autoPlay)
			{
				this._moviePlayer.Play();
			}
		}
		else
		{
			Logger.Error("AVProWindowsMedia", "Couldn't load movie " + this._filename, new object[0]);
			this.UnloadMovie();
			flag = false;
		}
		return flag;
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00013BD0 File Offset: 0x00011DD0
	public bool LoadMovieFromMemory(bool autoPlay, string name, IntPtr moviePointer, uint movieLength)
	{
		bool flag = true;
		if (this._moviePlayer == null)
		{
			this._moviePlayer = new AVProWindowsMedia();
		}
		bool flag2 = this._colourFormat > AVProWindowsMediaMovie.ColourFormat.RGBA32;
		if (this._moviePlayer.StartVideoFromMemory(name, moviePointer, movieLength, flag2, this._colourFormat == AVProWindowsMediaMovie.ColourFormat.YCbCr_HD, this._useAudioDelay, this._useAudioMixer, this._useDisplaySync))
		{
			this._moviePlayer.Volume = this._volume;
			if (autoPlay)
			{
				this._moviePlayer.Play();
			}
		}
		else
		{
			Logger.Error("AVProWindowsMedia", "Couldn't load movie " + this._filename, new object[0]);
			this.UnloadMovie();
			flag = false;
		}
		return flag;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00013C74 File Offset: 0x00011E74
	public void Update()
	{
		if (this._moviePlayer != null)
		{
			this._volume = Mathf.Clamp01(this._volume);
			if (this._volume != this._moviePlayer.Volume)
			{
				this._moviePlayer.Volume = this._volume;
			}
			if (this._loop != this._moviePlayer.Loop)
			{
				this._moviePlayer.Loop = this._loop;
			}
			this._moviePlayer.Update(false);
			if (!this._moviePlayer.Loop && this._moviePlayer.IsPlaying && this._moviePlayer.IsFinishedPlaying)
			{
				this._moviePlayer.Pause();
				base.SendMessage("MovieFinished", this, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	// Token: 0x0600003A RID: 58 RVA: 0x000037D6 File Offset: 0x000019D6
	public void Play()
	{
		if (this._moviePlayer != null)
		{
			this._moviePlayer.Play();
		}
	}

	// Token: 0x0600003B RID: 59 RVA: 0x000037EB File Offset: 0x000019EB
	public void Pause()
	{
		if (this._moviePlayer != null)
		{
			this._moviePlayer.Pause();
		}
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00003800 File Offset: 0x00001A00
	public float GetElapsedTime()
	{
		if (this._moviePlayer != null)
		{
			return this._moviePlayer.PositionSeconds;
		}
		return 0f;
	}

	// Token: 0x0600003D RID: 61 RVA: 0x0000381B File Offset: 0x00001A1B
	public void SetElapsedTime(float fTime)
	{
		if (this._moviePlayer != null)
		{
			this._moviePlayer.PositionSeconds = fTime;
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00003831 File Offset: 0x00001A31
	public virtual void UnloadMovie()
	{
		if (this._moviePlayer != null)
		{
			this._moviePlayer.Dispose();
			this._moviePlayer = null;
		}
	}

	// Token: 0x0600003F RID: 63 RVA: 0x0000384D File Offset: 0x00001A4D
	public void OnDestroy()
	{
		this.UnloadMovie();
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000040 RID: 64 RVA: 0x00003855 File Offset: 0x00001A55
	public bool IsFinishedPlaying
	{
		get
		{
			return this._moviePlayer != null && this._moviePlayer.IsFinishedPlaying;
		}
	}

	// Token: 0x04000037 RID: 55
	protected AVProWindowsMedia _moviePlayer;

	// Token: 0x04000038 RID: 56
	public string _folder = "./";

	// Token: 0x04000039 RID: 57
	public string _filename = "movie.avi";

	// Token: 0x0400003A RID: 58
	public bool _loop;

	// Token: 0x0400003B RID: 59
	public AVProWindowsMediaMovie.ColourFormat _colourFormat = AVProWindowsMediaMovie.ColourFormat.YCbCr_HD;

	// Token: 0x0400003C RID: 60
	public bool _useAudioDelay;

	// Token: 0x0400003D RID: 61
	public bool _useAudioMixer;

	// Token: 0x0400003E RID: 62
	public bool _useDisplaySync;

	// Token: 0x0400003F RID: 63
	public bool _loadOnStart = true;

	// Token: 0x04000040 RID: 64
	public bool _playOnStart = true;

	// Token: 0x04000041 RID: 65
	public bool _editorPreview;

	// Token: 0x04000042 RID: 66
	public float _volume = 1f;

	// Token: 0x0200000D RID: 13
	public enum ColourFormat
	{
		// Token: 0x04000044 RID: 68
		RGBA32,
		// Token: 0x04000045 RID: 69
		YCbCr_SD,
		// Token: 0x04000046 RID: 70
		YCbCr_HD
	}
}
