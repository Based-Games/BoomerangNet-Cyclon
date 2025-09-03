using System;
using System.Runtime.InteropServices;
using System.Text;

// Token: 0x02000495 RID: 1173
public class CardManagerR1
{
	// Token: 0x06001579 RID: 5497
	[DllImport("CRT_R1")]
	private static extern IntPtr CommOpen([MarshalAs(UnmanagedType.LPStr)] string Port);

	// Token: 0x0600157A RID: 5498
	[DllImport("CRT_R1")]
	private static extern int CommSetting(IntPtr ComHandle, [MarshalAs(UnmanagedType.LPStr)] string ComSeting);

	// Token: 0x0600157B RID: 5499
	[DllImport("CRT_R1")]
	private static extern int CRT286_GetStatus(IntPtr ComHandle, ref byte bAtr);

	// Token: 0x0600157C RID: 5500
	[DllImport("CRT_R1")]
	private static extern int CRT286_Eject(IntPtr ComHandle);

	// Token: 0x0600157D RID: 5501
	[DllImport("CRT_R1")]
	private static extern int CommClose(IntPtr ComHandle);

	// Token: 0x0600157E RID: 5502
	[DllImport("CRT_R1")]
	private static extern int RF_DetectCard(IntPtr ComHandle);

	// Token: 0x0600157F RID: 5503
	[DllImport("CRT_R1")]
	private static extern int RF_GetCardID(IntPtr ComHandle, byte[] _CardID);

	// Token: 0x06001580 RID: 5504
	[DllImport("CRT_R1")]
	private static extern int RF_LoadSecKey(IntPtr ComHandle, byte _Sec, byte _KEYType, byte[] _KEY);

	// Token: 0x06001581 RID: 5505
	[DllImport("CRT_R1")]
	private static extern int RF_ReadBlock(IntPtr ComHandle, byte _Sec, byte _Block, byte[] _BlockData);

	// Token: 0x06001582 RID: 5506 RVA: 0x0008E938 File Offset: 0x0008CB38
	public static void Init()
	{
		new ASCIIEncoding();
		string text = "Com6";
		Logger.Log("CardManagerR1", "Opening CRT_R1 at: " + text, new object[0]);
		CardManagerR1.m_Comm = CardManagerR1.CommOpen(text);
		text = "38400,n,8,1";
		if (CardManagerR1.CommSetting(CardManagerR1.m_Comm, text) != 0)
		{
			CardManagerR1.terminateCard();
			Logger.Warn("CardManagerR1", "Failed to open CRT_R1 at: " + text, new object[0]);
			return;
		}
		Singleton<GameManager>.instance.READER_ACTIVE = CardManagerR1.IsConnected();
		CardManagerR1.m_bInit = true;
		CardManagerR1.eject();
		Logger.Log("CardManagerR1", "CRT_R1 opened!", new object[0]);
	}

	// Token: 0x06001583 RID: 5507 RVA: 0x0001230C File Offset: 0x0001050C
	public static bool read(byte sector, byte block, byte[] pBuffer)
	{
		return CardManagerR1.RF_ReadBlock(CardManagerR1.m_Comm, sector, block, pBuffer) == 0;
	}

	// Token: 0x06001584 RID: 5508 RVA: 0x0008E9DC File Offset: 0x0008CBDC
	public static bool loadBase()
	{
		byte[] array = new byte[32];
		byte[] array2 = new byte[32];
		if (!CardManagerR1.read(0, 1, array))
		{
			return false;
		}
		if (!CardManagerR1.read(0, 2, array2))
		{
			return false;
		}
		string text = Encoding.Default.GetString(array).TrimEnd(new char[1]);
		string text2 = Encoding.Default.GetString(array2).TrimEnd(new char[1]);
		CardManagerR1.m_strId = text + text2;
		return true;
	}

	// Token: 0x06001585 RID: 5509 RVA: 0x0008EA4C File Offset: 0x0008CC4C
	public static bool isInsertCard()
	{
		if (CardManagerR1.RF_DetectCard(CardManagerR1.m_Comm) != 0)
		{
			return false;
		}
		byte[] array = new byte[4];
		return CardManagerR1.RF_GetCardID(CardManagerR1.m_Comm, array) == 0;
	}

	// Token: 0x06001586 RID: 5510 RVA: 0x0008EA7C File Offset: 0x0008CC7C
	public static bool isDefaultPassword()
	{
		foreach (byte[] array2 in new byte[][]
		{
			new byte[] { 114, 101, 116, 115, 97, 109 },
			new byte[] { 55, 33, 83, 106, 114, 64 },
			new byte[] { 35, 87, 110, 106, 48, 33 },
			new byte[] { 68, 77, 84, 35, 48, 49 },
			new byte[] { 33, 68, 77, 84, 49, 33 },
			new byte[] { 94, 78, 33, 112, 64, 106 }
		})
		{
			if (CardManagerR1.verifySecureKey(0, array2))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001587 RID: 5511 RVA: 0x0001231E File Offset: 0x0001051E
	public static bool verifySecureKey(byte sector, byte[] pBuffer)
	{
		return CardManagerR1.RF_LoadSecKey(CardManagerR1.m_Comm, sector, 0, pBuffer) == 0;
	}

	// Token: 0x06001588 RID: 5512 RVA: 0x00012330 File Offset: 0x00010530
	public static bool eject()
	{
		return CardManagerR1.m_bInit && CardManagerR1.CRT286_Eject(CardManagerR1.m_Comm) == 0;
	}

	// Token: 0x06001589 RID: 5513 RVA: 0x00012348 File Offset: 0x00010548
	private static void terminateCard()
	{
		CardManagerR1.eject();
		CardManagerR1.CommClose(CardManagerR1.m_Comm);
	}

	// Token: 0x0600158A RID: 5514 RVA: 0x0008EB28 File Offset: 0x0008CD28
	public static bool IsConnected()
	{
		byte b = 0;
		return CardManagerR1.CRT286_GetStatus(CardManagerR1.m_Comm, ref b) == 0;
	}

	// Token: 0x04001CFF RID: 7423
	private static IntPtr m_Comm;

	// Token: 0x04001D00 RID: 7424
	public static bool m_bInit;

	// Token: 0x04001D01 RID: 7425
	public static string m_strId = string.Empty;
}
