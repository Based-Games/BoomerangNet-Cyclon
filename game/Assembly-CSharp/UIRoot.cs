using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000BA RID: 186
[AddComponentMenu("NGUI/UI/Root")]
[ExecuteInEditMode]
public class UIRoot : MonoBehaviour
{
	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06000619 RID: 1561 RVA: 0x0002FD1C File Offset: 0x0002DF1C
	public int activeHeight
	{
		get
		{
			int num = Mathf.Max(2, Screen.height);
			if (this.scalingStyle == UIRoot.Scaling.FixedSize)
			{
				return this.manualHeight;
			}
			if (num < this.minimumHeight)
			{
				return this.minimumHeight;
			}
			if (num > this.maximumHeight)
			{
				return this.maximumHeight;
			}
			return num;
		}
	}

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x0600061A RID: 1562 RVA: 0x00007EAE File Offset: 0x000060AE
	public float pixelSizeAdjustment
	{
		get
		{
			return this.GetPixelSizeAdjustment(Screen.height);
		}
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x0002FD70 File Offset: 0x0002DF70
	public static float GetPixelSizeAdjustment(GameObject go)
	{
		UIRoot uiroot = NGUITools.FindInParents<UIRoot>(go);
		return (!(uiroot != null)) ? 1f : uiroot.pixelSizeAdjustment;
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x0002FDA0 File Offset: 0x0002DFA0
	public float GetPixelSizeAdjustment(int height)
	{
		height = Mathf.Max(2, height);
		if (this.scalingStyle == UIRoot.Scaling.FixedSize)
		{
			return (float)this.manualHeight / (float)height;
		}
		if (height < this.minimumHeight)
		{
			return (float)this.minimumHeight / (float)height;
		}
		if (height > this.maximumHeight)
		{
			return (float)this.maximumHeight / (float)height;
		}
		return 1f;
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x00007EBB File Offset: 0x000060BB
	protected virtual void Awake()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x00007EC9 File Offset: 0x000060C9
	protected virtual void OnEnable()
	{
		UIRoot.list.Add(this);
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x00007ED6 File Offset: 0x000060D6
	protected virtual void OnDisable()
	{
		UIRoot.list.Remove(this);
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x0002FE00 File Offset: 0x0002E000
	protected virtual void Start()
	{
		UIOrthoCamera componentInChildren = base.GetComponentInChildren<UIOrthoCamera>();
		if (componentInChildren != null)
		{
			Debug.LogWarning("UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", componentInChildren);
			Camera component = componentInChildren.gameObject.GetComponent<Camera>();
			componentInChildren.enabled = false;
			if (component != null)
			{
				component.orthographicSize = 1f;
			}
		}
		else
		{
			this.Update();
		}
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x0002FE60 File Offset: 0x0002E060
	private void Update()
	{
		if (this.mTrans != null)
		{
			float num = (float)this.activeHeight;
			if (num > 0f)
			{
				float num2 = 2f / num;
				Vector3 localScale = this.mTrans.localScale;
				if (Mathf.Abs(localScale.x - num2) > 1E-45f || Mathf.Abs(localScale.y - num2) > 1E-45f || Mathf.Abs(localScale.z - num2) > 1E-45f)
				{
					this.mTrans.localScale = new Vector3(num2, num2, num2);
				}
			}
		}
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x0002FF00 File Offset: 0x0002E100
	public static void Broadcast(string funcName)
	{
		int i = 0;
		int count = UIRoot.list.Count;
		while (i < count)
		{
			UIRoot uiroot = UIRoot.list[i];
			if (uiroot != null)
			{
				uiroot.BroadcastMessage(funcName, SendMessageOptions.DontRequireReceiver);
			}
			i++;
		}
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0002FF4C File Offset: 0x0002E14C
	public static void Broadcast(string funcName, object param)
	{
		if (param == null)
		{
			Debug.LogError("SendMessage is bugged when you try to pass 'null' in the parameter field. It behaves as if no parameter was specified.");
		}
		else
		{
			int i = 0;
			int count = UIRoot.list.Count;
			while (i < count)
			{
				UIRoot uiroot = UIRoot.list[i];
				if (uiroot != null)
				{
					uiroot.BroadcastMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
				}
				i++;
			}
		}
	}

	// Token: 0x040004AD RID: 1197
	public static List<UIRoot> list = new List<UIRoot>();

	// Token: 0x040004AE RID: 1198
	public UIRoot.Scaling scalingStyle;

	// Token: 0x040004AF RID: 1199
	public int manualHeight = 720;

	// Token: 0x040004B0 RID: 1200
	public int minimumHeight = 320;

	// Token: 0x040004B1 RID: 1201
	public int maximumHeight = 1536;

	// Token: 0x040004B2 RID: 1202
	private Transform mTrans;

	// Token: 0x020000BB RID: 187
	public enum Scaling
	{
		// Token: 0x040004B4 RID: 1204
		PixelPerfect,
		// Token: 0x040004B5 RID: 1205
		FixedSize,
		// Token: 0x040004B6 RID: 1206
		FixedSizeOnMobiles
	}
}
