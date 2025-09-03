using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x0200000E RID: 14
[AddComponentMenu("AVPro Windows Media/Movie From Resource")]
public class AVProWindowsMediaMovieFromResource : AVProWindowsMediaMovie
{
	// Token: 0x06000042 RID: 66 RVA: 0x00003874 File Offset: 0x00001A74
	public override void Start()
	{
		if (null == AVProWindowsMediaManager.Instance)
		{
			throw new Exception("You need to add AVProWindowsMediaManager component to your scene.");
		}
		if (this._loadOnStart)
		{
			this.LoadMovieFromResource(this._playOnStart, this._filename);
		}
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00013D34 File Offset: 0x00011F34
	public bool LoadMovieFromResource(bool autoPlay, string path)
	{
		bool flag = false;
		this.UnloadMovie();
		this._textAsset = Resources.Load(path, typeof(TextAsset)) as TextAsset;
		if (this._textAsset != null && this._textAsset.bytes != null && this._textAsset.bytes.Length != 0)
		{
			this._bytesHandle = GCHandle.Alloc(this._textAsset.bytes, GCHandleType.Pinned);
			flag = base.LoadMovieFromMemory(autoPlay, path, this._bytesHandle.AddrOfPinnedObject(), (uint)this._textAsset.bytes.Length);
		}
		if (!flag)
		{
			Logger.Error("AVProWindowsMedia", "Unable to load resource " + path, new object[0]);
		}
		return flag;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x000038A9 File Offset: 0x00001AA9
	public override void UnloadMovie()
	{
		if (this._moviePlayer != null)
		{
			this._moviePlayer.Dispose();
			this._moviePlayer = null;
		}
		this.UnloadResource();
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000038CB File Offset: 0x00001ACB
	private void UnloadResource()
	{
		if (this._bytesHandle.IsAllocated)
		{
			this._bytesHandle.Free();
		}
		if (this._textAsset != null)
		{
			Resources.UnloadAsset(this._textAsset);
			this._textAsset = null;
		}
	}

	// Token: 0x04000047 RID: 71
	private TextAsset _textAsset;

	// Token: 0x04000048 RID: 72
	private GCHandle _bytesHandle;
}
