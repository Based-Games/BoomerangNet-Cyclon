using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
[AddComponentMenu("AVPro Windows Media/GUI Display")]
public class AVProWindowsMediaGUIDisplay : MonoBehaviour
{
	// Token: 0x0600000D RID: 13 RVA: 0x00013598 File Offset: 0x00011798
	public void OnGUI()
	{
		if (this._movie == null)
		{
			return;
		}
		if (this._movie.OutputTexture != null)
		{
			GUI.depth = this._depth;
			GUI.color = this._color;
			Rect rect = this.GetRect();
			GUI.DrawTexture(rect, this._movie.OutputTexture, this._scaleMode, this._alphaBlend);
		}
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00013608 File Offset: 0x00011808
	public Rect GetRect()
	{
		Rect rect;
		if (this._fullScreen)
		{
			rect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		}
		else
		{
			rect = new Rect(this._x * (float)(Screen.width - 1), this._y * (float)(Screen.height - 1), this._width * (float)Screen.width, this._height * (float)Screen.height);
		}
		return rect;
	}

	// Token: 0x04000010 RID: 16
	public AVProWindowsMediaMovie _movie;

	// Token: 0x04000011 RID: 17
	public ScaleMode _scaleMode = ScaleMode.ScaleToFit;

	// Token: 0x04000012 RID: 18
	public Color _color = Color.white;

	// Token: 0x04000013 RID: 19
	public bool _alphaBlend;

	// Token: 0x04000014 RID: 20
	public bool _fullScreen = true;

	// Token: 0x04000015 RID: 21
	public int _depth;

	// Token: 0x04000016 RID: 22
	public float _x;

	// Token: 0x04000017 RID: 23
	public float _y;

	// Token: 0x04000018 RID: 24
	public float _width = 1f;

	// Token: 0x04000019 RID: 25
	public float _height = 1f;
}
