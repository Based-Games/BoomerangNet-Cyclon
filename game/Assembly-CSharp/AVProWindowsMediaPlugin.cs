using System;
using System.Runtime.InteropServices;

// Token: 0x02000011 RID: 17
public class AVProWindowsMediaPlugin
{
	// Token: 0x06000088 RID: 136
	[DllImport("AVProWindowsMedia")]
	public static extern bool Init();

	// Token: 0x06000089 RID: 137
	[DllImport("AVProWindowsMedia")]
	public static extern void Deinit();

	// Token: 0x0600008A RID: 138
	[DllImport("AVProWindowsMedia")]
	public static extern void SetUnityFeatures(bool supportExternalTextures, bool supportUnity35OpenGL);

	// Token: 0x0600008B RID: 139
	[DllImport("AVProWindowsMedia")]
	public static extern float GetPluginVersion();

	// Token: 0x0600008C RID: 140
	[DllImport("AVProWindowsMedia")]
	public static extern int GetInstanceHandle();

	// Token: 0x0600008D RID: 141
	[DllImport("AVProWindowsMedia")]
	public static extern void FreeInstanceHandle(int handle);

	// Token: 0x0600008E RID: 142
	[DllImport("AVProWindowsMedia")]
	public static extern bool LoadMovie(int handle, IntPtr filename, bool playFromMemory, bool allowNativeFormat, bool useAudioDelay, bool useAudioMixer, bool useDisplaySync);

	// Token: 0x0600008F RID: 143
	[DllImport("AVProWindowsMedia")]
	public static extern bool LoadMovieFromMemory(int handle, IntPtr moviePointer, uint movieLength, bool allowNativeFormat, bool useAudioDelay, bool useAudioMixer, bool useDisplaySync);

	// Token: 0x06000090 RID: 144
	[DllImport("AVProWindowsMedia")]
	public static extern int GetWidth(int handle);

	// Token: 0x06000091 RID: 145
	[DllImport("AVProWindowsMedia")]
	public static extern int GetHeight(int handle);

	// Token: 0x06000092 RID: 146
	[DllImport("AVProWindowsMedia")]
	public static extern float GetFrameRate(int handle);

	// Token: 0x06000093 RID: 147
	[DllImport("AVProWindowsMedia")]
	public static extern long GetFrameDuration(int handle);

	// Token: 0x06000094 RID: 148
	[DllImport("AVProWindowsMedia")]
	public static extern int GetFormat(int handle);

	// Token: 0x06000095 RID: 149
	[DllImport("AVProWindowsMedia")]
	public static extern float GetDurationSeconds(int handle);

	// Token: 0x06000096 RID: 150
	[DllImport("AVProWindowsMedia")]
	public static extern uint GetDurationFrames(int handle);

	// Token: 0x06000097 RID: 151
	[DllImport("AVProWindowsMedia")]
	public static extern bool IsOrientedTopDown(int handle);

	// Token: 0x06000098 RID: 152
	[DllImport("AVProWindowsMedia")]
	public static extern void Play(int handle);

	// Token: 0x06000099 RID: 153
	[DllImport("AVProWindowsMedia")]
	public static extern void Pause(int handle);

	// Token: 0x0600009A RID: 154
	[DllImport("AVProWindowsMedia")]
	public static extern void Stop(int handle);

	// Token: 0x0600009B RID: 155
	[DllImport("AVProWindowsMedia")]
	public static extern void SeekUnit(int handle, float position);

	// Token: 0x0600009C RID: 156
	[DllImport("AVProWindowsMedia")]
	public static extern void SeekSeconds(int handle, float position);

	// Token: 0x0600009D RID: 157
	[DllImport("AVProWindowsMedia")]
	public static extern void SeekFrames(int handle, uint position);

	// Token: 0x0600009E RID: 158
	[DllImport("AVProWindowsMedia")]
	public static extern float GetCurrentPositionSeconds(int handle);

	// Token: 0x0600009F RID: 159
	[DllImport("AVProWindowsMedia")]
	public static extern uint GetCurrentPositionFrames(int handle);

	// Token: 0x060000A0 RID: 160
	[DllImport("AVProWindowsMedia")]
	public static extern bool IsLooping(int handle);

	// Token: 0x060000A1 RID: 161
	[DllImport("AVProWindowsMedia")]
	public static extern float GetPlaybackRate(int handle);

	// Token: 0x060000A2 RID: 162
	[DllImport("AVProWindowsMedia")]
	public static extern float GetAudioBalance(int handle);

	// Token: 0x060000A3 RID: 163
	[DllImport("AVProWindowsMedia")]
	public static extern bool IsFinishedPlaying(int handle);

	// Token: 0x060000A4 RID: 164
	[DllImport("AVProWindowsMedia")]
	public static extern void SetVolume(int handle, float volume);

	// Token: 0x060000A5 RID: 165
	[DllImport("AVProWindowsMedia")]
	public static extern void SetLooping(int handle, bool loop);

	// Token: 0x060000A6 RID: 166
	[DllImport("AVProWindowsMedia")]
	public static extern void SetPlaybackRate(int handle, float rate);

	// Token: 0x060000A7 RID: 167
	[DllImport("AVProWindowsMedia")]
	public static extern void SetAudioBalance(int handle, float balance);

	// Token: 0x060000A8 RID: 168
	[DllImport("AVProWindowsMedia")]
	public static extern void SetAudioChannelMatrix(int handle, float[] values, int numValues);

	// Token: 0x060000A9 RID: 169
	[DllImport("AVProWindowsMedia")]
	public static extern void SetAudioDelay(int handle, int ms);

	// Token: 0x060000AA RID: 170
	[DllImport("AVProWindowsMedia")]
	public static extern bool Update(int handle);

	// Token: 0x060000AB RID: 171
	[DllImport("AVProWindowsMedia")]
	public static extern bool IsNextFrameReadyForGrab(int handle);

	// Token: 0x060000AC RID: 172
	[DllImport("AVProWindowsMedia")]
	public static extern int GetLastFrameUploaded(int handle);

	// Token: 0x060000AD RID: 173
	[DllImport("AVProWindowsMedia")]
	public static extern bool UpdateTextureGL(int handle, int textureID, ref int frameNumber);

	// Token: 0x060000AE RID: 174
	[DllImport("AVProWindowsMedia")]
	public static extern bool GetFramePixels(int handle, IntPtr data, int bufferWidth, int bufferHeight, ref int frameNumber);

	// Token: 0x060000AF RID: 175
	[DllImport("AVProWindowsMedia")]
	public static extern bool SetTexturePointer(int handle, IntPtr data);

	// Token: 0x060000B0 RID: 176
	[DllImport("AVProWindowsMedia")]
	public static extern IntPtr GetTexturePointer(int handle);

	// Token: 0x060000B1 RID: 177
	[DllImport("AVProWindowsMedia")]
	public static extern float GetCaptureFrameRate(int handle);

	// Token: 0x060000B2 RID: 178
	[DllImport("AVProWindowsMedia")]
	public static extern void SetFrameBufferSize(int handle, int read, int write);

	// Token: 0x060000B3 RID: 179
	[DllImport("AVProWindowsMedia")]
	public static extern long GetLastFrameBufferedTime(int handle);

	// Token: 0x060000B4 RID: 180
	[DllImport("AVProWindowsMedia")]
	public static extern IntPtr GetLastFrameBuffered(int handle);

	// Token: 0x060000B5 RID: 181
	[DllImport("AVProWindowsMedia")]
	public static extern IntPtr GetFrameFromBufferAtTime(int handle, long time);

	// Token: 0x04000065 RID: 101
	public const int PluginID = 262209536;

	// Token: 0x02000012 RID: 18
	public enum VideoFrameFormat
	{
		// Token: 0x04000067 RID: 103
		RAW_BGRA32,
		// Token: 0x04000068 RID: 104
		YUV_422_YUY2,
		// Token: 0x04000069 RID: 105
		YUV_422_UYVY,
		// Token: 0x0400006A RID: 106
		YUV_422_YVYU,
		// Token: 0x0400006B RID: 107
		YUV_422_HDYC,
		// Token: 0x0400006C RID: 108
		YUV_420_NV12 = 7,
		// Token: 0x0400006D RID: 109
		Hap_RGB = 9,
		// Token: 0x0400006E RID: 110
		Hap_RGBA,
		// Token: 0x0400006F RID: 111
		Hap_RGB_HQ
	}

	// Token: 0x02000013 RID: 19
	public enum PluginEvent
	{
		// Token: 0x04000071 RID: 113
		UpdateAllTextures
	}
}
