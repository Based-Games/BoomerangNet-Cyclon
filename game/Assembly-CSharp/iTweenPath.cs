using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C9 RID: 201
[AddComponentMenu("Pixelplacement/iTweenPath")]
public class iTweenPath : MonoBehaviour
{
	// Token: 0x0600068C RID: 1676 RVA: 0x00008465 File Offset: 0x00006665
	private void OnEnable()
	{
		if (!iTweenPath.paths.ContainsKey(this.pathName))
		{
			iTweenPath.paths.Add(this.pathName.ToLower(), this);
		}
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x00008492 File Offset: 0x00006692
	private void OnDisable()
	{
		iTweenPath.paths.Remove(this.pathName.ToLower());
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x000084AA File Offset: 0x000066AA
	private void OnDrawGizmosSelected()
	{
		if (this.pathVisible && this.nodes.Count > 0)
		{
			iTween.DrawPath(this.nodes.ToArray(), this.pathColor);
		}
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x000339CC File Offset: 0x00031BCC
	public static Vector3[] GetPath(string requestedName)
	{
		requestedName = requestedName.ToLower();
		if (iTweenPath.paths.ContainsKey(requestedName))
		{
			return iTweenPath.paths[requestedName].nodes.ToArray();
		}
		Debug.Log("No path with that name (" + requestedName + ") exists! Are you sure you wrote it correctly?");
		return null;
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x00033A20 File Offset: 0x00031C20
	public static Vector3[] GetPathReversed(string requestedName)
	{
		requestedName = requestedName.ToLower();
		if (iTweenPath.paths.ContainsKey(requestedName))
		{
			List<Vector3> range = iTweenPath.paths[requestedName].nodes.GetRange(0, iTweenPath.paths[requestedName].nodes.Count);
			range.Reverse();
			return range.ToArray();
		}
		Debug.Log("No path with that name (" + requestedName + ") exists! Are you sure you wrote it correctly?");
		return null;
	}

	// Token: 0x04000521 RID: 1313
	public string pathName = string.Empty;

	// Token: 0x04000522 RID: 1314
	public Color pathColor = Color.cyan;

	// Token: 0x04000523 RID: 1315
	public List<Vector3> nodes = new List<Vector3>
	{
		Vector3.zero,
		Vector3.zero
	};

	// Token: 0x04000524 RID: 1316
	public int nodeCount;

	// Token: 0x04000525 RID: 1317
	public static Dictionary<string, iTweenPath> paths = new Dictionary<string, iTweenPath>();

	// Token: 0x04000526 RID: 1318
	public bool initialized;

	// Token: 0x04000527 RID: 1319
	public string initialName = string.Empty;

	// Token: 0x04000528 RID: 1320
	public bool pathVisible = true;
}
