using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200006A RID: 106
public class BetterList<T>
{
	// Token: 0x06000288 RID: 648 RVA: 0x0001DAF8 File Offset: 0x0001BCF8
	public IEnumerator<T> GetEnumerator()
	{
		if (this.buffer != null)
		{
			for (int i = 0; i < this.size; i++)
			{
				yield return this.buffer[i];
			}
		}
		yield break;
	}

	// Token: 0x17000064 RID: 100
	[DebuggerHidden]
	public T this[int i]
	{
		get
		{
			return this.buffer[i];
		}
		set
		{
			this.buffer[i] = value;
		}
	}

	// Token: 0x0600028B RID: 651 RVA: 0x0001DB14 File Offset: 0x0001BD14
	private void AllocateMore()
	{
		T[] array = ((this.buffer == null) ? new T[32] : new T[Mathf.Max(this.buffer.Length << 1, 32)]);
		if (this.buffer != null && this.size > 0)
		{
			this.buffer.CopyTo(array, 0);
		}
		this.buffer = array;
	}

	// Token: 0x0600028C RID: 652 RVA: 0x0001DB7C File Offset: 0x0001BD7C
	private void Trim()
	{
		if (this.size > 0)
		{
			if (this.size < this.buffer.Length)
			{
				T[] array = new T[this.size];
				for (int i = 0; i < this.size; i++)
				{
					array[i] = this.buffer[i];
				}
				this.buffer = array;
			}
		}
		else
		{
			this.buffer = null;
		}
	}

	// Token: 0x0600028D RID: 653 RVA: 0x0000552D File Offset: 0x0000372D
	public void Clear()
	{
		this.size = 0;
	}

	// Token: 0x0600028E RID: 654 RVA: 0x00005536 File Offset: 0x00003736
	public void Release()
	{
		this.size = 0;
		this.buffer = null;
	}

	// Token: 0x0600028F RID: 655 RVA: 0x0001DBF4 File Offset: 0x0001BDF4
	public void Add(T item)
	{
		if (this.buffer == null || this.size == this.buffer.Length)
		{
			this.AllocateMore();
		}
		this.buffer[this.size++] = item;
	}

	// Token: 0x06000290 RID: 656 RVA: 0x0001DC44 File Offset: 0x0001BE44
	public void Insert(int index, T item)
	{
		if (this.buffer == null || this.size == this.buffer.Length)
		{
			this.AllocateMore();
		}
		if (index < this.size)
		{
			for (int i = this.size; i > index; i--)
			{
				this.buffer[i] = this.buffer[i - 1];
			}
			this.buffer[index] = item;
			this.size++;
		}
		else
		{
			this.Add(item);
		}
	}

	// Token: 0x06000291 RID: 657 RVA: 0x0001DCDC File Offset: 0x0001BEDC
	public bool Contains(T item)
	{
		if (this.buffer == null)
		{
			return false;
		}
		for (int i = 0; i < this.size; i++)
		{
			if (this.buffer[i].Equals(item))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000292 RID: 658 RVA: 0x0001DD34 File Offset: 0x0001BF34
	public bool Remove(T item)
	{
		if (this.buffer != null)
		{
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int i = 0; i < this.size; i++)
			{
				if (@default.Equals(this.buffer[i], item))
				{
					this.size--;
					this.buffer[i] = default(T);
					for (int j = i; j < this.size; j++)
					{
						this.buffer[j] = this.buffer[j + 1];
					}
					this.buffer[this.size] = default(T);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000293 RID: 659 RVA: 0x0001DDF4 File Offset: 0x0001BFF4
	public void RemoveAt(int index)
	{
		if (this.buffer != null && index < this.size)
		{
			this.size--;
			this.buffer[index] = default(T);
			for (int i = index; i < this.size; i++)
			{
				this.buffer[i] = this.buffer[i + 1];
			}
			this.buffer[this.size] = default(T);
		}
	}

	// Token: 0x06000294 RID: 660 RVA: 0x0001DE88 File Offset: 0x0001C088
	public T Pop()
	{
		if (this.buffer != null && this.size != 0)
		{
			T t = this.buffer[--this.size];
			this.buffer[this.size] = default(T);
			return t;
		}
		return default(T);
	}

	// Token: 0x06000295 RID: 661 RVA: 0x00005546 File Offset: 0x00003746
	public T[] ToArray()
	{
		this.Trim();
		return this.buffer;
	}

	// Token: 0x06000296 RID: 662 RVA: 0x0001DEF0 File Offset: 0x0001C0F0
	[DebuggerStepThrough]
	[DebuggerHidden]
	public void Sort(BetterList<T>.CompareFunc comparer)
	{
		int num = 0;
		int num2 = this.size - 1;
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int i = num; i < num2; i++)
			{
				if (comparer(this.buffer[i], this.buffer[i + 1]) > 0)
				{
					T t = this.buffer[i];
					this.buffer[i] = this.buffer[i + 1];
					this.buffer[i + 1] = t;
					flag = true;
				}
				else if (!flag)
				{
					num = ((i != 0) ? (i - 1) : 0);
				}
			}
		}
	}

	// Token: 0x04000294 RID: 660
	public T[] buffer;

	// Token: 0x04000295 RID: 661
	public int size;

	// Token: 0x0200006B RID: 107
	// (Invoke) Token: 0x06000298 RID: 664
	public delegate int CompareFunc(T left, T right);
}
