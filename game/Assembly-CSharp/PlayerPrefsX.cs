using System;
using System.Text;
using UnityEngine;

// Token: 0x0200022B RID: 555
public static class PlayerPrefsX
{
	// Token: 0x06000FEE RID: 4078 RVA: 0x0000D9DF File Offset: 0x0000BBDF
	public static bool SetVector3(string key, Vector3 vector)
	{
		return PlayerPrefsX.SetFloatArray(key, new float[] { vector.x, vector.y, vector.z });
	}

	// Token: 0x06000FEF RID: 4079 RVA: 0x00072A48 File Offset: 0x00070C48
	public static Vector3 GetVector3(string key)
	{
		float[] floatArray = PlayerPrefsX.GetFloatArray(key);
		if (floatArray.Length < 3)
		{
			return Vector3.zero;
		}
		return new Vector3(floatArray[0], floatArray[1], floatArray[2]);
	}

	// Token: 0x06000FF0 RID: 4080 RVA: 0x00072A7C File Offset: 0x00070C7C
	public static bool SetBoolArray(string key, params bool[] boolArray)
	{
		if (boolArray.Length == 0)
		{
			return false;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < boolArray.Length - 1; i++)
		{
			stringBuilder.Append(boolArray[i]).Append("|");
		}
		stringBuilder.Append(boolArray[boolArray.Length - 1]);
		try
		{
			PlayerPrefs.SetString(key, stringBuilder.ToString());
		}
		catch (Exception ex)
		{
			return false;
		}
		return true;
	}

	// Token: 0x06000FF1 RID: 4081 RVA: 0x00072B04 File Offset: 0x00070D04
	public static bool[] GetBoolArray(string key)
	{
		if (PlayerPrefs.HasKey(key))
		{
			string[] array = PlayerPrefs.GetString(key).Split(new char[] { "|"[0] });
			bool[] array2 = new bool[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = Convert.ToBoolean(array[i]);
			}
			return array2;
		}
		return new bool[0];
	}

	// Token: 0x06000FF2 RID: 4082 RVA: 0x00072B6C File Offset: 0x00070D6C
	public static bool[] GetBoolArray(string key, bool defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefsX.GetBoolArray(key);
		}
		bool[] array = new bool[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	// Token: 0x06000FF3 RID: 4083 RVA: 0x00072BAC File Offset: 0x00070DAC
	public static bool SetIntArray(string key, params int[] intArray)
	{
		if (intArray.Length == 0)
		{
			return false;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < intArray.Length - 1; i++)
		{
			stringBuilder.Append(intArray[i]).Append("|");
		}
		stringBuilder.Append(intArray[intArray.Length - 1]);
		try
		{
			PlayerPrefs.SetString(key, stringBuilder.ToString());
		}
		catch (Exception ex)
		{
			return false;
		}
		return true;
	}

	// Token: 0x06000FF4 RID: 4084 RVA: 0x00072C34 File Offset: 0x00070E34
	public static int[] GetIntArray(string key)
	{
		if (PlayerPrefs.HasKey(key))
		{
			string[] array = PlayerPrefs.GetString(key).Split(new char[] { "|"[0] });
			int[] array2 = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = Convert.ToInt32(array[i]);
			}
			return array2;
		}
		return new int[0];
	}

	// Token: 0x06000FF5 RID: 4085 RVA: 0x00072C9C File Offset: 0x00070E9C
	public static int[] GetIntArray(string key, int defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			int num = PlayerPrefsX.GetIntArray(key).Length;
			int[] intArray = PlayerPrefsX.GetIntArray(key);
			if (num < defaultSize)
			{
				int[] array = new int[defaultSize];
				for (int i = 0; i < defaultSize; i++)
				{
					if (i < num)
					{
						array[i] = intArray[i];
					}
					else
					{
						array[i] = defaultValue;
					}
				}
				PlayerPrefsX.SetIntArray(key, array);
			}
			return PlayerPrefsX.GetIntArray(key);
		}
		int[] array2 = new int[defaultSize];
		for (int j = 0; j < defaultSize; j++)
		{
			array2[j] = defaultValue;
		}
		return array2;
	}

	// Token: 0x06000FF6 RID: 4086 RVA: 0x00072D30 File Offset: 0x00070F30
	public static bool SetFloatArray(string key, params float[] floatArray)
	{
		if (floatArray.Length == 0)
		{
			return false;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < floatArray.Length - 1; i++)
		{
			stringBuilder.Append(floatArray[i]).Append("|");
		}
		stringBuilder.Append(floatArray[floatArray.Length - 1]);
		try
		{
			PlayerPrefs.SetString(key, stringBuilder.ToString());
		}
		catch (Exception ex)
		{
			return false;
		}
		return true;
	}

	// Token: 0x06000FF7 RID: 4087 RVA: 0x00072DB8 File Offset: 0x00070FB8
	public static float[] GetFloatArray(string key)
	{
		if (PlayerPrefs.HasKey(key))
		{
			string[] array = PlayerPrefs.GetString(key).Split(new char[] { "|"[0] });
			float[] array2 = new float[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = Convert.ToSingle(array[i]);
			}
			return array2;
		}
		return new float[0];
	}

	// Token: 0x06000FF8 RID: 4088 RVA: 0x00072E20 File Offset: 0x00071020
	public static float[] GetFloatArray(string key, float defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefsX.GetFloatArray(key);
		}
		float[] array = new float[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	// Token: 0x06000FF9 RID: 4089 RVA: 0x00072E60 File Offset: 0x00071060
	public static bool SetStringArray(string key, char separator, params string[] stringArray)
	{
		if (stringArray.Length == 0)
		{
			return false;
		}
		try
		{
			PlayerPrefs.SetString(key, string.Join(separator.ToString(), stringArray));
		}
		catch (Exception ex)
		{
			return false;
		}
		return true;
	}

	// Token: 0x06000FFA RID: 4090 RVA: 0x0000DA0B File Offset: 0x0000BC0B
	public static bool SetStringArray(string key, params string[] stringArray)
	{
		return PlayerPrefsX.SetStringArray(key, "\n"[0], stringArray);
	}

	// Token: 0x06000FFB RID: 4091 RVA: 0x0000DA27 File Offset: 0x0000BC27
	public static string[] GetStringArray(string key, char separator)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefs.GetString(key).Split(new char[] { separator });
		}
		return new string[0];
	}

	// Token: 0x06000FFC RID: 4092 RVA: 0x0000DA50 File Offset: 0x0000BC50
	public static string[] GetStringArray(string key)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefs.GetString(key).Split(new char[] { "\n"[0] });
		}
		return new string[0];
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x00072EB0 File Offset: 0x000710B0
	public static string[] GetStringArray(string key, char separator, string defaultValue, int defaultSize)
	{
		if (PlayerPrefs.HasKey(key))
		{
			return PlayerPrefs.GetString(key).Split(new char[] { separator });
		}
		string[] array = new string[defaultSize];
		for (int i = 0; i < defaultSize; i++)
		{
			array[i] = defaultValue;
		}
		return array;
	}

	// Token: 0x06000FFE RID: 4094 RVA: 0x0000DA83 File Offset: 0x0000BC83
	public static string[] GetStringArray(string key, string defaultValue, int defaultSize)
	{
		return PlayerPrefsX.GetStringArray(key, "\n"[0], defaultValue, defaultSize);
	}
}
