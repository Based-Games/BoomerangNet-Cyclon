using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200024C RID: 588
[AddComponentMenu("")]
public class tk2dUpdateManager : MonoBehaviour
{
	// Token: 0x17000284 RID: 644
	// (get) Token: 0x060010CE RID: 4302 RVA: 0x00077C14 File Offset: 0x00075E14
	private static tk2dUpdateManager Instance
	{
		get
		{
			if (tk2dUpdateManager.inst == null)
			{
				tk2dUpdateManager.inst = UnityEngine.Object.FindObjectOfType(typeof(tk2dUpdateManager)) as tk2dUpdateManager;
				if (tk2dUpdateManager.inst == null)
				{
					GameObject gameObject = new GameObject("@tk2dUpdateManager");
					gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
					tk2dUpdateManager.inst = gameObject.AddComponent<tk2dUpdateManager>();
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
			}
			return tk2dUpdateManager.inst;
		}
	}

	// Token: 0x060010CF RID: 4303 RVA: 0x0000E53D File Offset: 0x0000C73D
	public static void QueueCommit(tk2dTextMesh textMesh)
	{
		tk2dUpdateManager.Instance.QueueCommitInternal(textMesh);
	}

	// Token: 0x060010D0 RID: 4304 RVA: 0x0000E54A File Offset: 0x0000C74A
	public static void FlushQueues()
	{
		tk2dUpdateManager.Instance.FlushQueuesInternal();
	}

	// Token: 0x060010D1 RID: 4305 RVA: 0x0000E556 File Offset: 0x0000C756
	private void OnEnable()
	{
		base.StartCoroutine(this.coSuperLateUpdate());
	}

	// Token: 0x060010D2 RID: 4306 RVA: 0x0000E565 File Offset: 0x0000C765
	private void LateUpdate()
	{
		this.FlushQueuesInternal();
	}

	// Token: 0x060010D3 RID: 4307 RVA: 0x00077C84 File Offset: 0x00075E84
	private IEnumerator coSuperLateUpdate()
	{
		this.FlushQueuesInternal();
		yield break;
	}

	// Token: 0x060010D4 RID: 4308 RVA: 0x0000E56D File Offset: 0x0000C76D
	private void QueueCommitInternal(tk2dTextMesh textMesh)
	{
		this.textMeshes.Add(textMesh);
	}

	// Token: 0x060010D5 RID: 4309 RVA: 0x00077CA0 File Offset: 0x00075EA0
	private void FlushQueuesInternal()
	{
		int count = this.textMeshes.Count;
		for (int i = 0; i < count; i++)
		{
			tk2dTextMesh tk2dTextMesh = this.textMeshes[i];
			if (tk2dTextMesh != null)
			{
				tk2dTextMesh.DoNotUse__CommitInternal();
			}
		}
		this.textMeshes.Clear();
	}

	// Token: 0x040012A9 RID: 4777
	private static tk2dUpdateManager inst;

	// Token: 0x040012AA RID: 4778
	[SerializeField]
	private List<tk2dTextMesh> textMeshes = new List<tk2dTextMesh>(64);
}
