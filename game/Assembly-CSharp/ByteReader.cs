using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x0200006D RID: 109
public class ByteReader
{
	// Token: 0x060002A1 RID: 673 RVA: 0x00005572 File Offset: 0x00003772
	public ByteReader(byte[] bytes)
	{
		this.mBuffer = bytes;
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x00005581 File Offset: 0x00003781
	public ByteReader(TextAsset asset)
	{
		this.mBuffer = asset.bytes;
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x060002A3 RID: 675 RVA: 0x00005595 File Offset: 0x00003795
	public bool canRead
	{
		get
		{
			return this.mBuffer != null && this.mOffset < this.mBuffer.Length;
		}
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x000055B5 File Offset: 0x000037B5
	private static string ReadLine(byte[] buffer, int start, int count)
	{
		return Encoding.UTF8.GetString(buffer, start, count);
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x0001E048 File Offset: 0x0001C248
	public string ReadLine()
	{
		int num = this.mBuffer.Length;
		while (this.mOffset < num && this.mBuffer[this.mOffset] < 32)
		{
			this.mOffset++;
		}
		int i = this.mOffset;
		if (i < num)
		{
			while (i < num)
			{
				int num2 = (int)this.mBuffer[i++];
				if (num2 == 10 || num2 == 13)
				{
					IL_81:
					string text = ByteReader.ReadLine(this.mBuffer, this.mOffset, i - this.mOffset - 1);
					this.mOffset = i;
					return text;
				}
			}
			i++;
			goto IL_81;
		}
		this.mOffset = num;
		return null;
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x0001E108 File Offset: 0x0001C308
	public Dictionary<string, string> ReadDictionary()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		char[] array = new char[] { '=' };
		while (this.canRead)
		{
			string text = this.ReadLine();
			if (text == null)
			{
				break;
			}
			if (!text.StartsWith("//"))
			{
				string[] array2 = text.Split(array, 2, StringSplitOptions.RemoveEmptyEntries);
				if (array2.Length == 2)
				{
					string text2 = array2[0].Trim();
					string text3 = array2[1].Trim().Replace("\\n", "\n");
					dictionary[text2] = text3;
				}
			}
		}
		return dictionary;
	}

	// Token: 0x0400029A RID: 666
	private byte[] mBuffer;

	// Token: 0x0400029B RID: 667
	private int mOffset;
}
