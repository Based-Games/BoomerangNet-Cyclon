using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class AVProWindowsMediaPlayVideoDemo : MonoBehaviour
{
	// Token: 0x06000006 RID: 6 RVA: 0x0000359C File Offset: 0x0000179C
	private void ReleaseMemoryFile()
	{
		if (this._bytesHandle.IsAllocated)
		{
			this._bytesHandle.Free();
		}
		this._moviePtr = IntPtr.Zero;
		this._movieLength = 0U;
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00012B8C File Offset: 0x00010D8C
	private void LoadFileToMemory(string folder, string filename)
	{
		string text = Path.Combine(folder, filename);
		if (!Application.isEditor && !Path.IsPathRooted(text))
		{
			string fullPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
			text = Path.Combine(fullPath, text);
		}
		this.ReleaseMemoryFile();
		if (File.Exists(text))
		{
			byte[] array = File.ReadAllBytes(text);
			if (array.Length > 0)
			{
				this._bytesHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
				this._moviePtr = this._bytesHandle.AddrOfPinnedObject();
				this._movieLength = (uint)array.Length;
				this._movie.LoadMovieFromMemory(true, filename, this._moviePtr, this._movieLength);
			}
		}
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00012C38 File Offset: 0x00010E38
	public void OnGUI()
	{
		GUI.skin = this._skin;
		if (this._visible)
		{
			GUI.color = new Color(1f, 1f, 1f, this._alpha);
			GUILayout.BeginArea(new Rect(0f, 0f, 740f, 330f), GUI.skin.box);
			this.ControlWindow(0);
			GUILayout.EndArea();
		}
		GUI.color = new Color(1f, 1f, 1f, 1f - this._alpha);
		GUI.Box(new Rect(0f, 0f, 128f, 32f), "Demo Controls");
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00012CF8 File Offset: 0x00010EF8
	private void Update()
	{
		Rect rect = new Rect(0f, 0f, 740f, 330f);
		if (rect.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)))
		{
			this._visible = true;
			this._alpha = 1f;
		}
		else
		{
			this._alpha -= Time.deltaTime * 4f;
			if (this._alpha <= 0f)
			{
				this._alpha = 0f;
				this._visible = false;
			}
		}
	}

	// Token: 0x0600000A RID: 10 RVA: 0x00012DA4 File Offset: 0x00010FA4
	public void ControlWindow(int id)
	{
		if (this._movie == null)
		{
			return;
		}
		GUILayout.Space(16f);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Folder: ", new GUILayoutOption[] { GUILayout.Width(100f) });
		this._movie._folder = GUILayout.TextField(this._movie._folder, 192, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("File: ", new GUILayoutOption[] { GUILayout.Width(100f) });
		this._movie._filename = GUILayout.TextField(this._movie._filename, 128, new GUILayoutOption[] { GUILayout.Width(440f) });
		if (GUILayout.Button("Load File", new GUILayoutOption[] { GUILayout.Width(90f) }))
		{
			if (!this._playFromMemory)
			{
				this._movie.LoadMovie(true);
			}
			else
			{
				this.LoadFileToMemory(this._movie._folder, this._movie._filename);
			}
		}
		GUILayout.EndHorizontal();
		if (this._display != null)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Space(100f);
			if (this._display._alphaBlend != GUILayout.Toggle(this._display._alphaBlend, "Render with Transparency", new GUILayoutOption[0]))
			{
				this._display._alphaBlend = !this._display._alphaBlend;
				if (this._display._alphaBlend)
				{
					this._movie._colourFormat = AVProWindowsMediaMovie.ColourFormat.RGBA32;
				}
				else
				{
					this._movie._colourFormat = AVProWindowsMediaMovie.ColourFormat.YCbCr_HD;
				}
				if (!this._playFromMemory)
				{
					this._movie.LoadMovie(true);
				}
				else
				{
					this.LoadFileToMemory(this._movie._folder, this._movie._filename);
				}
			}
			if (this._playFromMemory != GUILayout.Toggle(this._playFromMemory, "Play from Memory", new GUILayoutOption[0]))
			{
				this._playFromMemory = !this._playFromMemory;
				if (this._movie.MovieInstance != null)
				{
					if (!this._playFromMemory)
					{
						this._movie.LoadMovie(true);
					}
					else
					{
						this.LoadFileToMemory(this._movie._folder, this._movie._filename);
					}
				}
			}
			GUILayout.EndHorizontal();
		}
		AVProWindowsMedia movieInstance = this._movie.MovieInstance;
		if (movieInstance != null)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Info:", new GUILayoutOption[] { GUILayout.Width(100f) });
			GUILayout.Label(string.Concat(new object[]
			{
				movieInstance.Width,
				"x",
				movieInstance.Height,
				" @ ",
				movieInstance.FrameRate.ToString("F2"),
				" FPS"
			}), new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Volume ", new GUILayoutOption[] { GUILayout.Width(100f) });
			float volume = this._movie._volume;
			float num = GUILayout.HorizontalSlider(volume, 0f, 1f, new GUILayoutOption[] { GUILayout.Width(200f) });
			if (volume != num)
			{
				this._movie._volume = num;
			}
			GUILayout.Label(this._movie._volume.ToString("F1"), new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Balance ", new GUILayoutOption[] { GUILayout.Width(100f) });
			float audioBalance = movieInstance.AudioBalance;
			float num2 = GUILayout.HorizontalSlider(audioBalance, -1f, 1f, new GUILayoutOption[] { GUILayout.Width(200f) });
			if (audioBalance != num2)
			{
				movieInstance.AudioBalance = num2;
			}
			GUILayout.Label(movieInstance.AudioBalance.ToString("F1"), new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Audio Delay", new GUILayoutOption[] { GUILayout.Width(100f) });
			int audioDelay = movieInstance.AudioDelay;
			int num3 = Mathf.FloorToInt(GUILayout.HorizontalSlider((float)audioDelay, -1000f, 1000f, new GUILayoutOption[] { GUILayout.Width(200f) }));
			if (audioDelay != num3)
			{
				movieInstance.AudioDelay = num3;
			}
			float num4 = 1000f / movieInstance.FrameRate;
			int num5 = Mathf.FloorToInt((float)num3 / num4);
			GUILayout.Label(string.Concat(new object[]
			{
				movieInstance.AudioDelay.ToString(),
				"ms (",
				num5,
				" frames)"
			}), new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Time ", new GUILayoutOption[] { GUILayout.Width(100f) });
			float positionSeconds = movieInstance.PositionSeconds;
			float num6 = GUILayout.HorizontalSlider(positionSeconds, 0f, movieInstance.DurationSeconds, new GUILayoutOption[] { GUILayout.Width(200f) });
			if (positionSeconds != num6)
			{
				movieInstance.PositionSeconds = num6;
			}
			GUILayout.Label(movieInstance.PositionSeconds.ToString("F1") + " / " + movieInstance.DurationSeconds.ToString("F1") + "s", new GUILayoutOption[0]);
			if (GUILayout.Button("Play", new GUILayoutOption[0]))
			{
				movieInstance.Play();
			}
			if (GUILayout.Button("Pause", new GUILayoutOption[0]))
			{
				movieInstance.Pause();
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Frame", new GUILayoutOption[] { GUILayout.Width(100f) });
			uint positionFrames = movieInstance.PositionFrames;
			if (positionFrames != 4294967295U)
			{
				uint num7 = (uint)GUILayout.HorizontalSlider(positionFrames, 0f, movieInstance.DurationFrames - 1f, new GUILayoutOption[] { GUILayout.Width(200f) });
				if (positionFrames != num7)
				{
					movieInstance.PositionFrames = num7;
				}
				GUILayout.Label(movieInstance.PositionFrames.ToString() + " / " + (movieInstance.DurationFrames - 1U).ToString(), new GUILayoutOption[0]);
				if (GUILayout.RepeatButton("<", new GUILayoutOption[] { GUILayout.Width(50f) }) && movieInstance.PositionFrames > 0U)
				{
					movieInstance.PositionFrames -= 1U;
				}
				if (GUILayout.RepeatButton(">", new GUILayoutOption[] { GUILayout.Width(50f) }) && movieInstance.PositionFrames < movieInstance.DurationFrames - 1U)
				{
					movieInstance.PositionFrames += 1U;
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Rate ", new GUILayoutOption[] { GUILayout.Width(100f) });
			GUILayout.Label(movieInstance.PlaybackRate.ToString("F2") + "x", new GUILayoutOption[0]);
			if (GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(50f) }))
			{
				movieInstance.PlaybackRate *= 0.5f;
			}
			if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(50f) }))
			{
				movieInstance.PlaybackRate *= 2f;
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}

	// Token: 0x04000005 RID: 5
	public GUISkin _skin;

	// Token: 0x04000006 RID: 6
	public AVProWindowsMediaMovie _movie;

	// Token: 0x04000007 RID: 7
	public AVProWindowsMediaGUIDisplay _display;

	// Token: 0x04000008 RID: 8
	private bool _visible = true;

	// Token: 0x04000009 RID: 9
	private float _alpha = 1f;

	// Token: 0x0400000A RID: 10
	private bool _playFromMemory;

	// Token: 0x0400000B RID: 11
	private GCHandle _bytesHandle;

	// Token: 0x0400000C RID: 12
	private IntPtr _moviePtr;

	// Token: 0x0400000D RID: 13
	private uint _movieLength;
}
