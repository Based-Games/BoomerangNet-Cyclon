using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000071 RID: 113
[AddComponentMenu("NGUI/Internal/Debug")]
public class NGUIDebug : MonoBehaviour
{
	// Token: 0x1700006F RID: 111
	// (get) Token: 0x060002D5 RID: 725 RVA: 0x00005807 File Offset: 0x00003A07
	// (set) Token: 0x060002D6 RID: 726 RVA: 0x0000580E File Offset: 0x00003A0E
	public static bool debugRaycast
	{
		get
		{
			return NGUIDebug.mRayDebug;
		}
		set
		{
			if (Application.isPlaying)
			{
				NGUIDebug.mRayDebug = value;
				if (value)
				{
					NGUIDebug.CreateInstance();
				}
			}
		}
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x0001E828 File Offset: 0x0001CA28
	public static void CreateInstance()
	{
		if (NGUIDebug.mInstance == null)
		{
			GameObject gameObject = new GameObject("_NGUI Debug");
			NGUIDebug.mInstance = gameObject.AddComponent<NGUIDebug>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x0001E864 File Offset: 0x0001CA64
	private static void LogString(string text)
	{
		if (Application.isPlaying)
		{
			if (NGUIDebug.mLines.Count > 20)
			{
				NGUIDebug.mLines.RemoveAt(0);
			}
			NGUIDebug.mLines.Add(text);
			NGUIDebug.CreateInstance();
		}
		else
		{
			Debug.Log(text);
		}
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x0001E8B4 File Offset: 0x0001CAB4
	public static void Log(params object[] objs)
	{
		string text = string.Empty;
		for (int i = 0; i < objs.Length; i++)
		{
			if (i == 0)
			{
				text += objs[i].ToString();
			}
			else
			{
				text = text + ", " + objs[i].ToString();
			}
		}
		NGUIDebug.LogString(text);
	}

	// Token: 0x060002DA RID: 730 RVA: 0x0001E910 File Offset: 0x0001CB10
	public static void DrawBounds(Bounds b)
	{
		Vector3 center = b.center;
		Vector3 vector = b.center - b.extents;
		Vector3 vector2 = b.center + b.extents;
		Debug.DrawLine(new Vector3(vector.x, vector.y, center.z), new Vector3(vector2.x, vector.y, center.z), Color.red);
		Debug.DrawLine(new Vector3(vector.x, vector.y, center.z), new Vector3(vector.x, vector2.y, center.z), Color.red);
		Debug.DrawLine(new Vector3(vector2.x, vector.y, center.z), new Vector3(vector2.x, vector2.y, center.z), Color.red);
		Debug.DrawLine(new Vector3(vector.x, vector2.y, center.z), new Vector3(vector2.x, vector2.y, center.z), Color.red);
	}

	// Token: 0x060002DB RID: 731 RVA: 0x0001EA48 File Offset: 0x0001CC48
	private void OnGUI()
	{
		if (NGUIDebug.mLines.Count == 0)
		{
			if (NGUIDebug.mRayDebug && UICamera.hoveredObject != null && Application.isPlaying)
			{
				GUILayout.Label("Last Hit: " + NGUITools.GetHierarchy(UICamera.hoveredObject).Replace("\"", string.Empty), new GUILayoutOption[0]);
			}
		}
		else
		{
			int i = 0;
			int count = NGUIDebug.mLines.Count;
			while (i < count)
			{
				GUILayout.Label(NGUIDebug.mLines[i], new GUILayoutOption[0]);
				i++;
			}
		}
	}

	// Token: 0x040002A7 RID: 679
	private static bool mRayDebug = false;

	// Token: 0x040002A8 RID: 680
	private static List<string> mLines = new List<string>();

	// Token: 0x040002A9 RID: 681
	private static NGUIDebug mInstance = null;
}
