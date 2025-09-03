using System;
using System.Security.Cryptography;
using System.Text;

namespace AESWithJavaCS
{
	// Token: 0x02000217 RID: 535
	internal class AESWithJava
	{
		// Token: 0x06000F73 RID: 3955 RVA: 0x00070244 File Offset: 0x0006E444
		public static string Decrypt(string textToDecrypt, string key)
		{
			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			rijndaelManaged.Mode = CipherMode.CBC;
			rijndaelManaged.Padding = PaddingMode.PKCS7;
			rijndaelManaged.KeySize = 128;
			rijndaelManaged.BlockSize = 128;
			byte[] array = Convert.FromBase64String(textToDecrypt);
			byte[] bytes = Encoding.UTF8.GetBytes(key);
			byte[] array2 = new byte[16];
			int num = bytes.Length;
			if (num > array2.Length)
			{
				num = array2.Length;
			}
			Array.Copy(bytes, array2, num);
			rijndaelManaged.Key = array2;
			rijndaelManaged.IV = array2;
			byte[] array3 = rijndaelManaged.CreateDecryptor().TransformFinalBlock(array, 0, array.Length);
			return Encoding.UTF8.GetString(array3);
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x0000D451 File Offset: 0x0000B651
		public static string Encrypt(string textToEncrypt, string key)
		{
			return textToEncrypt;
		}
	}
}
