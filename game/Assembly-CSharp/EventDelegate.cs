using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200006E RID: 110
[Serializable]
public class EventDelegate
{
	// Token: 0x060002A7 RID: 679 RVA: 0x00003B98 File Offset: 0x00001D98
	public EventDelegate()
	{
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x000055C4 File Offset: 0x000037C4
	public EventDelegate(EventDelegate.Callback call)
	{
		this.Set(call);
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x000055D3 File Offset: 0x000037D3
	public EventDelegate(MonoBehaviour target, string methodName)
	{
		this.Set(target, methodName);
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x060002AB RID: 683 RVA: 0x000055F4 File Offset: 0x000037F4
	// (set) Token: 0x060002AC RID: 684 RVA: 0x000055FC File Offset: 0x000037FC
	public MonoBehaviour target
	{
		get
		{
			return this.mTarget;
		}
		set
		{
			this.mTarget = value;
			this.mCachedCallback = null;
			this.mRawDelegate = false;
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x060002AD RID: 685 RVA: 0x00005613 File Offset: 0x00003813
	// (set) Token: 0x060002AE RID: 686 RVA: 0x0000561B File Offset: 0x0000381B
	public string methodName
	{
		get
		{
			return this.mMethodName;
		}
		set
		{
			this.mMethodName = value;
			this.mCachedCallback = null;
			this.mRawDelegate = false;
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x060002AF RID: 687 RVA: 0x00005632 File Offset: 0x00003832
	public bool isValid
	{
		get
		{
			return (this.mRawDelegate && this.mCachedCallback != null) || (this.mTarget != null && !string.IsNullOrEmpty(this.mMethodName));
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000566F File Offset: 0x0000386F
	public bool isEnabled
	{
		get
		{
			return (this.mRawDelegate && this.mCachedCallback != null) || (this.mTarget != null && this.mTarget.enabled);
		}
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x000056A9 File Offset: 0x000038A9
	private static string GetMethodName(EventDelegate.Callback callback)
	{
		return callback.Method.Name;
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x000056B6 File Offset: 0x000038B6
	private static bool IsValid(EventDelegate.Callback callback)
	{
		return callback != null && callback.Method != null;
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x0001E1A0 File Offset: 0x0001C3A0
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return !this.isValid;
		}
		if (obj is EventDelegate.Callback)
		{
			EventDelegate.Callback callback = obj as EventDelegate.Callback;
			return callback.Equals(this.mCachedCallback) || (this.mTarget == (MonoBehaviour)callback.Target && string.Equals(this.mMethodName, EventDelegate.GetMethodName(callback)));
		}
		if (obj is EventDelegate)
		{
			EventDelegate eventDelegate = obj as EventDelegate;
			return this.mTarget == eventDelegate.mTarget && string.Equals(this.mMethodName, eventDelegate.mMethodName);
		}
		return false;
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x000056CD File Offset: 0x000038CD
	public override int GetHashCode()
	{
		return EventDelegate.s_Hash;
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x0001E250 File Offset: 0x0001C450
	private EventDelegate.Callback Get()
	{
		if (!this.mRawDelegate && (this.mCachedCallback == null || (MonoBehaviour)this.mCachedCallback.Target != this.mTarget || EventDelegate.GetMethodName(this.mCachedCallback) != this.mMethodName))
		{
			if (!(this.mTarget != null) || string.IsNullOrEmpty(this.mMethodName))
			{
				return null;
			}
			this.mCachedCallback = (EventDelegate.Callback)Delegate.CreateDelegate(typeof(EventDelegate.Callback), this.mTarget, this.mMethodName);
		}
		return this.mCachedCallback;
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x0001E304 File Offset: 0x0001C504
	private void Set(EventDelegate.Callback call)
	{
		if (call == null || !EventDelegate.IsValid(call))
		{
			this.mTarget = null;
			this.mMethodName = null;
			this.mCachedCallback = null;
			this.mRawDelegate = false;
		}
		else
		{
			this.mTarget = call.Target as MonoBehaviour;
			if (this.mTarget == null)
			{
				this.mRawDelegate = true;
				this.mCachedCallback = call;
				this.mMethodName = null;
			}
			else
			{
				this.mMethodName = EventDelegate.GetMethodName(call);
				this.mRawDelegate = false;
			}
		}
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x000056D4 File Offset: 0x000038D4
	public void Set(MonoBehaviour target, string methodName)
	{
		this.mTarget = target;
		this.mMethodName = methodName;
		this.mCachedCallback = null;
		this.mRawDelegate = false;
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x0001E394 File Offset: 0x0001C594
	public bool Execute()
	{
		EventDelegate.Callback callback = this.Get();
		if (callback != null)
		{
			callback();
			return true;
		}
		return false;
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x000056F2 File Offset: 0x000038F2
	public void Clear()
	{
		this.mTarget = null;
		this.mMethodName = null;
		this.mRawDelegate = false;
		this.mCachedCallback = null;
	}

	// Token: 0x060002BA RID: 698 RVA: 0x0001E3B8 File Offset: 0x0001C5B8
	public override string ToString()
	{
		if (!(this.mTarget != null))
		{
			return (!this.mRawDelegate) ? null : "[delegate]";
		}
		string text = this.mTarget.GetType().ToString();
		int num = text.LastIndexOf('.');
		if (num > 0)
		{
			text = text.Substring(num + 1);
		}
		if (!string.IsNullOrEmpty(this.methodName))
		{
			return text + "." + this.methodName;
		}
		return text + ".[delegate]";
	}

	// Token: 0x060002BB RID: 699 RVA: 0x0001E448 File Offset: 0x0001C648
	public static void Execute(List<EventDelegate> list)
	{
		if (list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null)
				{
					eventDelegate.Execute();
					if (eventDelegate.oneShot)
					{
						list.RemoveAt(i);
						continue;
					}
				}
			}
		}
	}

	// Token: 0x060002BC RID: 700 RVA: 0x0001E4A0 File Offset: 0x0001C6A0
	public static bool IsValid(List<EventDelegate> list)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.isValid)
				{
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x060002BD RID: 701 RVA: 0x00005710 File Offset: 0x00003910
	public static void Set(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		if (list != null)
		{
			list.Clear();
			list.Add(new EventDelegate(callback));
		}
	}

	// Token: 0x060002BE RID: 702 RVA: 0x0000572A File Offset: 0x0000392A
	public static void Add(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		EventDelegate.Add(list, callback, false);
	}

	// Token: 0x060002BF RID: 703 RVA: 0x0001E4E8 File Offset: 0x0001C6E8
	public static void Add(List<EventDelegate> list, EventDelegate.Callback callback, bool oneShot)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(callback))
				{
					return;
				}
				i++;
			}
			list.Add(new EventDelegate(callback)
			{
				oneShot = oneShot
			});
		}
		else
		{
			Debug.LogWarning("Attempting to add a callback to a list that's null");
		}
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x00005734 File Offset: 0x00003934
	public static void Add(List<EventDelegate> list, EventDelegate ev)
	{
		EventDelegate.Add(list, ev, false);
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x0001E554 File Offset: 0x0001C754
	public static void Add(List<EventDelegate> list, EventDelegate ev, bool oneShot)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(ev))
				{
					return;
				}
				i++;
			}
			list.Add(new EventDelegate(ev.target, ev.methodName)
			{
				oneShot = oneShot
			});
		}
		else
		{
			Debug.LogWarning("Attempting to add a callback to a list that's null");
		}
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x0001E5CC File Offset: 0x0001C7CC
	public static bool Remove(List<EventDelegate> list, EventDelegate.Callback callback)
	{
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				EventDelegate eventDelegate = list[i];
				if (eventDelegate != null && eventDelegate.Equals(callback))
				{
					list.RemoveAt(i);
					return true;
				}
				i++;
			}
		}
		return false;
	}

	// Token: 0x0400029C RID: 668
	[SerializeField]
	private MonoBehaviour mTarget;

	// Token: 0x0400029D RID: 669
	[SerializeField]
	private string mMethodName;

	// Token: 0x0400029E RID: 670
	public bool oneShot;

	// Token: 0x0400029F RID: 671
	private EventDelegate.Callback mCachedCallback;

	// Token: 0x040002A0 RID: 672
	private bool mRawDelegate;

	// Token: 0x040002A1 RID: 673
	private static int s_Hash = "EventDelegate".GetHashCode();

	// Token: 0x0200006F RID: 111
	// (Invoke) Token: 0x060002C4 RID: 708
	public delegate void Callback();
}
