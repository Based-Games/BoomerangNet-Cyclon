using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class AVProWindowsMediaMaterialMappingDemo : MonoBehaviour
{
	// Token: 0x06000002 RID: 2 RVA: 0x0001245C File Offset: 0x0001065C
	public void OnGUI()
	{
		GUI.skin = this._skin;
		if (this._visible)
		{
			GUI.color = new Color(1f, 1f, 1f, this._alpha);
			GUILayout.BeginArea(new Rect(0f, 0f, 740f, 300f), GUI.skin.box);
			this.ControlWindow(0);
			GUILayout.EndArea();
		}
		GUI.color = new Color(1f, 1f, 1f, 1f - this._alpha);
		GUI.Box(new Rect(0f, 0f, 128f, 32f), "Demo Controls");
	}

	// Token: 0x06000003 RID: 3 RVA: 0x0001251C File Offset: 0x0001071C
	private void Update()
	{
		Rect rect = new Rect(0f, 0f, 740f, 310f);
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

	// Token: 0x06000004 RID: 4 RVA: 0x000125C8 File Offset: 0x000107C8
	public void ControlWindow(int id)
	{
		if (this._movie == null)
		{
			return;
		}
		GUILayout.Space(16f);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("Folder: ", new GUILayoutOption[] { GUILayout.Width(80f) });
		this._movie._folder = GUILayout.TextField(this._movie._folder, 192, new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.Label("File: ", new GUILayoutOption[] { GUILayout.Width(80f) });
		this._movie._filename = GUILayout.TextField(this._movie._filename, 128, new GUILayoutOption[] { GUILayout.Width(440f) });
		if (GUILayout.Button("Load File", new GUILayoutOption[] { GUILayout.Width(90f) }))
		{
			this._movie.LoadMovie(true);
		}
		GUILayout.EndHorizontal();
		bool flag = this._movie._colourFormat == AVProWindowsMediaMovie.ColourFormat.RGBA32;
		if (flag)
		{
			flag = GUILayout.Toggle(flag, "Render with Transparency (requires movie reload)", new GUILayoutOption[0]);
		}
		else
		{
			flag = GUILayout.Toggle(flag, "Render without Transparency", new GUILayoutOption[0]);
		}
		if (flag)
		{
			this._movie._colourFormat = AVProWindowsMediaMovie.ColourFormat.RGBA32;
		}
		else
		{
			this._movie._colourFormat = AVProWindowsMediaMovie.ColourFormat.YCbCr_HD;
		}
		AVProWindowsMedia movieInstance = this._movie.MovieInstance;
		if (movieInstance != null)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Info:", new GUILayoutOption[] { GUILayout.Width(80f) });
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
			GUILayout.Label("Volume ", new GUILayoutOption[] { GUILayout.Width(80f) });
			float volume = this._movie._volume;
			float num = GUILayout.HorizontalSlider(volume, 0f, 1f, new GUILayoutOption[] { GUILayout.Width(200f) });
			if (volume != num)
			{
				this._movie._volume = num;
			}
			GUILayout.Label(this._movie._volume.ToString("F1"), new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Balance ", new GUILayoutOption[] { GUILayout.Width(80f) });
			float audioBalance = movieInstance.AudioBalance;
			float num2 = GUILayout.HorizontalSlider(audioBalance, -1f, 1f, new GUILayoutOption[] { GUILayout.Width(200f) });
			if (audioBalance != num2)
			{
				movieInstance.AudioBalance = num2;
			}
			GUILayout.Label(movieInstance.AudioBalance.ToString("F1"), new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Time ", new GUILayoutOption[] { GUILayout.Width(80f) });
			float positionSeconds = movieInstance.PositionSeconds;
			float num3 = GUILayout.HorizontalSlider(positionSeconds, 0f, movieInstance.DurationSeconds, new GUILayoutOption[] { GUILayout.Width(200f) });
			if (positionSeconds != num3)
			{
				movieInstance.PositionSeconds = num3;
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
			GUILayout.Label("Frame", new GUILayoutOption[] { GUILayout.Width(80f) });
			GUILayout.Label(movieInstance.PositionFrames.ToString() + " / " + movieInstance.DurationFrames.ToString(), new GUILayoutOption[0]);
			if (GUILayout.Button("<", new GUILayoutOption[] { GUILayout.Width(50f) }))
			{
				movieInstance.Pause();
				if (movieInstance.PositionFrames > 0U)
				{
					movieInstance.PositionFrames -= 1U;
				}
			}
			if (GUILayout.Button(">", new GUILayoutOption[] { GUILayout.Width(50f) }))
			{
				movieInstance.Pause();
				if (movieInstance.PositionFrames < movieInstance.DurationFrames)
				{
					movieInstance.PositionFrames += 1U;
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Rate ", new GUILayoutOption[] { GUILayout.Width(80f) });
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

	// Token: 0x04000001 RID: 1
	public GUISkin _skin;

	// Token: 0x04000002 RID: 2
	public AVProWindowsMediaMovie _movie;

	// Token: 0x04000003 RID: 3
	private bool _visible = true;

	// Token: 0x04000004 RID: 4
	private float _alpha = 1f;
}
