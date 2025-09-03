using System;
using System.Runtime.InteropServices;
using System.Text;

// Token: 0x020004A6 RID: 1190
public class CardManagerHID
{
	// Token: 0x06001599 RID: 5529
	[DllImport("hid.dll", SetLastError = true)]
	private static extern void HidD_GetHidGuid(ref Guid hidGuid);

	// Token: 0x0600159A RID: 5530
	[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, uint Flags);

	// Token: 0x0600159B RID: 5531
	[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool SetupDiEnumDeviceInterfaces(IntPtr DeviceInfoSet, IntPtr DeviceInfoData, ref Guid InterfaceClassGuid, uint MemberIndex, ref CardManagerHID.SP_DEVICE_INTERFACE_DATA DeviceInterfaceData);

	// Token: 0x0600159C RID: 5532
	[DllImport("setupapi.dll", SetLastError = true)]
	private static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

	// Token: 0x0600159D RID: 5533
	[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr DeviceInfoSet, ref CardManagerHID.SP_DEVICE_INTERFACE_DATA DeviceInterfaceData, IntPtr DeviceInterfaceDetailData, uint DeviceInterfaceDetailDataSize, ref uint RequiredSize, IntPtr DeviceInfoData);

	// Token: 0x0600159E RID: 5534
	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr SecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

	// Token: 0x0600159F RID: 5535
	[DllImport("hid.dll", SetLastError = true)]
	private static extern bool HidD_GetAttributes(IntPtr HidDeviceObject, ref CardManagerHID.HIDD_ATTRIBUTES Attributes);

	// Token: 0x060015A0 RID: 5536
	[DllImport("hid.dll", SetLastError = true)]
	private static extern bool HidD_GetPreparsedData(IntPtr HidDeviceObject, ref IntPtr PreparsedData);

	// Token: 0x060015A1 RID: 5537
	[DllImport("hid.dll", SetLastError = true)]
	private static extern bool HidD_FreePreparsedData(IntPtr PreparsedData);

	// Token: 0x060015A2 RID: 5538
	[DllImport("hid.dll", SetLastError = true)]
	private static extern bool HidD_SetFeature(IntPtr HidDeviceObject, byte[] lpReportBuffer, uint ReportBufferLength);

	// Token: 0x060015A3 RID: 5539
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool ReadFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, ref uint lpNumberOfBytesRead, IntPtr lpOverlapped);

	// Token: 0x060015A4 RID: 5540
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, ref uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

	// Token: 0x060015A5 RID: 5541
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool CloseHandle(IntPtr hObject);

	// Token: 0x060015A6 RID: 5542 RVA: 0x0008F1D4 File Offset: 0x0008D3D4
	public static void Init()
	{
		Logger.Log("CardManagerHID", "Initializing HID reader", new object[0]);
		string text = CardManagerHID.FindHidDevice(4660, 43981);
		if (string.IsNullOrEmpty(text))
		{
			Logger.Warn("CardManagerHID", "Failed to find HID device with VID:0x" + 4660.ToString("X4") + " PID:0x" + 43981.ToString("X4"), new object[0]);
			return;
		}
		CardManagerHID.m_Handle = CardManagerHID.CreateFile(text, 3221225472U, 3U, IntPtr.Zero, 3U, 1073741952U, IntPtr.Zero);
		if (CardManagerHID.m_Handle == IntPtr.Zero)
		{
			Logger.Warn("CardManagerHID", "Failed to open HID device at: " + text, new object[0]);
			return;
		}
		CardManagerHID.HIDD_ATTRIBUTES hidd_ATTRIBUTES = default(CardManagerHID.HIDD_ATTRIBUTES);
		hidd_ATTRIBUTES.Size = Marshal.SizeOf(hidd_ATTRIBUTES);
		if (!CardManagerHID.HidD_GetAttributes(CardManagerHID.m_Handle, ref hidd_ATTRIBUTES))
		{
			CardManagerHID.terminateCard();
			Logger.Warn("CardManagerHID", "Failed to get HID attributes", new object[0]);
			return;
		}
		if (hidd_ATTRIBUTES.VendorID != 4660 || hidd_ATTRIBUTES.ProductID != 43981)
		{
			CardManagerHID.terminateCard();
			Logger.Warn("CardManagerHID", "HID device VID/PID mismatch", new object[0]);
			return;
		}
		Singleton<GameManager>.instance.READER_ACTIVE = CardManagerHID.IsConnected();
		CardManagerHID.m_bInit = true;
		CardManagerHID.eject();
		Logger.Log("CardManagerHID", "HID reader opened!", new object[0]);
	}

	// Token: 0x060015A7 RID: 5543 RVA: 0x0008F348 File Offset: 0x0008D548
	private static string FindHidDevice(ushort vid, ushort pid)
	{
		Guid empty = Guid.Empty;
		CardManagerHID.HidD_GetHidGuid(ref empty);
		IntPtr intPtr = CardManagerHID.SetupDiGetClassDevs(ref empty, IntPtr.Zero, IntPtr.Zero, 18U);
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		CardManagerHID.SP_DEVICE_INTERFACE_DATA sp_DEVICE_INTERFACE_DATA = default(CardManagerHID.SP_DEVICE_INTERFACE_DATA);
		sp_DEVICE_INTERFACE_DATA.cbSize = (uint)Marshal.SizeOf(sp_DEVICE_INTERFACE_DATA);
		uint num = 0U;
		while (CardManagerHID.SetupDiEnumDeviceInterfaces(intPtr, IntPtr.Zero, ref empty, num, ref sp_DEVICE_INTERFACE_DATA))
		{
			uint num2 = 0U;
			CardManagerHID.SetupDiGetDeviceInterfaceDetail(intPtr, ref sp_DEVICE_INTERFACE_DATA, IntPtr.Zero, 0U, ref num2, IntPtr.Zero);
			IntPtr intPtr2 = Marshal.AllocHGlobal((int)num2);
			Marshal.StructureToPtr(new CardManagerHID.SP_DEVICE_INTERFACE_DETAIL_DATA
			{
				cbSize = (uint)(4 + Marshal.SystemDefaultCharSize)
			}, intPtr2, false);
			if (CardManagerHID.SetupDiGetDeviceInterfaceDetail(intPtr, ref sp_DEVICE_INTERFACE_DATA, intPtr2, num2, ref num2, IntPtr.Zero))
			{
				CardManagerHID.SP_DEVICE_INTERFACE_DETAIL_DATA sp_DEVICE_INTERFACE_DETAIL_DATA = (CardManagerHID.SP_DEVICE_INTERFACE_DETAIL_DATA)Marshal.PtrToStructure(intPtr2, typeof(CardManagerHID.SP_DEVICE_INTERFACE_DETAIL_DATA));
				string devicePath = sp_DEVICE_INTERFACE_DETAIL_DATA.DevicePath;
				IntPtr intPtr3 = CardManagerHID.CreateFile(devicePath, 3221225472U, 3U, IntPtr.Zero, 3U, 128U, IntPtr.Zero);
				if (intPtr3 != IntPtr.Zero)
				{
					CardManagerHID.HIDD_ATTRIBUTES hidd_ATTRIBUTES = default(CardManagerHID.HIDD_ATTRIBUTES);
					hidd_ATTRIBUTES.Size = Marshal.SizeOf(hidd_ATTRIBUTES);
					if (CardManagerHID.HidD_GetAttributes(intPtr3, ref hidd_ATTRIBUTES) && hidd_ATTRIBUTES.VendorID == vid && hidd_ATTRIBUTES.ProductID == pid)
					{
						CardManagerHID.CloseHandle(intPtr3);
						Marshal.FreeHGlobal(intPtr2);
						CardManagerHID.SetupDiDestroyDeviceInfoList(intPtr);
						return devicePath;
					}
					CardManagerHID.CloseHandle(intPtr3);
				}
			}
			Marshal.FreeHGlobal(intPtr2);
			num += 1U;
		}
		CardManagerHID.SetupDiDestroyDeviceInfoList(intPtr);
		return null;
	}

	// Token: 0x060015A8 RID: 5544 RVA: 0x0008F4D8 File Offset: 0x0008D6D8
	public static bool read(byte sector, byte block, byte[] pBuffer)
	{
		if (!CardManagerHID.m_bInit)
		{
			return false;
		}
		byte[] array = new byte[64];
		array[0] = 0;
		array[1] = 1;
		array[2] = sector;
		array[3] = block;
		uint num = 0U;
		if (!CardManagerHID.WriteFile(CardManagerHID.m_Handle, array, (uint)array.Length, ref num, IntPtr.Zero) || (ulong)num != (ulong)((long)array.Length))
		{
			return false;
		}
		byte[] array2 = new byte[64];
		uint num2 = 0U;
		if (!CardManagerHID.ReadFile(CardManagerHID.m_Handle, array2, (uint)array2.Length, ref num2, IntPtr.Zero) || (ulong)num2 < (ulong)((long)(pBuffer.Length + 1)))
		{
			return false;
		}
		if (array2[0] != 0)
		{
			return false;
		}
		Array.Copy(array2, 1, pBuffer, 0, pBuffer.Length);
		return true;
	}

	// Token: 0x060015A9 RID: 5545 RVA: 0x0008F56C File Offset: 0x0008D76C
	public static bool loadBase()
	{
		byte[] array = new byte[32];
		byte[] array2 = new byte[32];
		if (!CardManagerHID.read(0, 1, array))
		{
			return false;
		}
		if (!CardManagerHID.read(0, 2, array2))
		{
			return false;
		}
		string text = Encoding.Default.GetString(array).TrimEnd(new char[1]);
		string text2 = Encoding.Default.GetString(array2).TrimEnd(new char[1]);
		CardManagerHID.m_strId = text + text2;
		return true;
	}

	// Token: 0x060015AA RID: 5546 RVA: 0x0008F5DC File Offset: 0x0008D7DC
	public static bool isInsertCard()
	{
		if (!CardManagerHID.m_bInit)
		{
			return false;
		}
		byte[] array = new byte[64];
		array[0] = 0;
		array[1] = 2;
		uint num = 0U;
		if (!CardManagerHID.WriteFile(CardManagerHID.m_Handle, array, (uint)array.Length, ref num, IntPtr.Zero))
		{
			return false;
		}
		byte[] array2 = new byte[64];
		uint num2 = 0U;
		return CardManagerHID.ReadFile(CardManagerHID.m_Handle, array2, (uint)array2.Length, ref num2, IntPtr.Zero) && array2[0] == 1;
	}

	// Token: 0x060015AB RID: 5547 RVA: 0x000070C1 File Offset: 0x000052C1
	public static bool isDefaultPassword()
	{
		return true;
	}

	// Token: 0x060015AC RID: 5548 RVA: 0x0008F648 File Offset: 0x0008D848
	public static bool verifySecureKey(byte sector, byte[] pBuffer)
	{
		if (!CardManagerHID.m_bInit)
		{
			return false;
		}
		byte[] array = new byte[64];
		array[0] = 0;
		array[1] = 3;
		array[2] = sector;
		Array.Copy(pBuffer, 0, array, 3, Math.Min(pBuffer.Length, array.Length - 3));
		uint num = 0U;
		return CardManagerHID.WriteFile(CardManagerHID.m_Handle, array, (uint)array.Length, ref num, IntPtr.Zero) && (ulong)num == (ulong)((long)array.Length);
	}

	// Token: 0x060015AD RID: 5549 RVA: 0x0008F6AC File Offset: 0x0008D8AC
	public static bool eject()
	{
		if (!CardManagerHID.m_bInit)
		{
			return false;
		}
		byte[] array = new byte[64];
		array[0] = 0;
		array[1] = 4;
		uint num = 0U;
		return CardManagerHID.WriteFile(CardManagerHID.m_Handle, array, (uint)array.Length, ref num, IntPtr.Zero) && (ulong)num == (ulong)((long)array.Length);
	}

	// Token: 0x060015AE RID: 5550 RVA: 0x000123F2 File Offset: 0x000105F2
	public static bool IsConnected()
	{
		return CardManagerHID.m_Handle != IntPtr.Zero;
	}

	// Token: 0x060015AF RID: 5551 RVA: 0x00012403 File Offset: 0x00010603
	private static void terminateCard()
	{
		CardManagerHID.eject();
		if (CardManagerHID.m_Handle != IntPtr.Zero)
		{
			CardManagerHID.CloseHandle(CardManagerHID.m_Handle);
			CardManagerHID.m_Handle = IntPtr.Zero;
		}
	}

	// Token: 0x04001D60 RID: 7520
	private const int HID_REPORT_ID = 0;

	// Token: 0x04001D61 RID: 7521
	private const int HID_INPUT_REPORT_SIZE = 64;

	// Token: 0x04001D62 RID: 7522
	private const int HID_OUTPUT_REPORT_SIZE = 64;

	// Token: 0x04001D63 RID: 7523
	private const ushort VID = 4660;

	// Token: 0x04001D64 RID: 7524
	private const ushort PID = 43981;

	// Token: 0x04001D65 RID: 7525
	private static readonly Guid HID_GUID = new Guid("4D1E55B2-F16F-11CF-88CB-001111000030");

	// Token: 0x04001D66 RID: 7526
	private const uint DIGCF_PRESENT = 2U;

	// Token: 0x04001D67 RID: 7527
	private const uint DIGCF_DEVICEINTERFACE = 16U;

	// Token: 0x04001D68 RID: 7528
	private const uint FILE_ATTRIBUTE_NORMAL = 128U;

	// Token: 0x04001D69 RID: 7529
	private const uint FILE_FLAG_OVERLAPPED = 1073741824U;

	// Token: 0x04001D6A RID: 7530
	private const uint OPEN_EXISTING = 3U;

	// Token: 0x04001D6B RID: 7531
	private const uint GENERIC_READ = 2147483648U;

	// Token: 0x04001D6C RID: 7532
	private const uint GENERIC_WRITE = 1073741824U;

	// Token: 0x04001D6D RID: 7533
	private const uint FILE_SHARE_READ = 1U;

	// Token: 0x04001D6E RID: 7534
	private const uint FILE_SHARE_WRITE = 2U;

	// Token: 0x04001D6F RID: 7535
	private static IntPtr m_Handle = IntPtr.Zero;

	// Token: 0x04001D70 RID: 7536
	private static bool m_bInit = false;

	// Token: 0x04001D71 RID: 7537
	public static string m_strId = string.Empty;

	// Token: 0x020004A7 RID: 1191
	private struct HIDD_ATTRIBUTES
	{
		// Token: 0x04001D72 RID: 7538
		public int Size;

		// Token: 0x04001D73 RID: 7539
		public ushort VendorID;

		// Token: 0x04001D74 RID: 7540
		public ushort ProductID;

		// Token: 0x04001D75 RID: 7541
		public ushort VersionNumber;
	}

	// Token: 0x020004A8 RID: 1192
	private struct SP_DEVICE_INTERFACE_DATA
	{
		// Token: 0x04001D76 RID: 7542
		public uint cbSize;

		// Token: 0x04001D77 RID: 7543
		public Guid InterfaceClassGuid;

		// Token: 0x04001D78 RID: 7544
		public uint Flags;

		// Token: 0x04001D79 RID: 7545
		public IntPtr Reserved;
	}

	// Token: 0x020004A9 RID: 1193
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	private struct SP_DEVICE_INTERFACE_DETAIL_DATA
	{
		// Token: 0x04001D7A RID: 7546
		public uint cbSize;

		// Token: 0x04001D7B RID: 7547
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string DevicePath;
	}
}
