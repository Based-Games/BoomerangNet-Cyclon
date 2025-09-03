using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

// Token: 0x020000E6 RID: 230
public class NewMultiTouch
{
	// Token: 0x060007B1 RID: 1969
	[DllImport("PulseIRLib")]
	private static extern bool isDllStart();

	// Token: 0x060007B2 RID: 1970 RVA: 0x00008E61 File Offset: 0x00007061
	public static bool OnisDllStart()
	{
		return NewMultiTouch.isDllStart();
	}

	// Token: 0x060007B3 RID: 1971
	[DllImport("PulseIRLib")]
	private static extern int PulseIRInit(NewMultiTouch.CallBackDelegate ptr);

	// Token: 0x060007B4 RID: 1972 RVA: 0x00008E68 File Offset: 0x00007068
	public static int OnTouchPulseIRInit(NewMultiTouch.CallBackDelegate ptr)
	{
		return NewMultiTouch.PulseIRInit(ptr);
	}

	// Token: 0x060007B5 RID: 1973
	[DllImport("PulseIRLib")]
	private static extern int getDeviceinfo(int iUsbIdx, StringBuilder strDeviceName, StringBuilder strFwVer, out int iDeviceType, out int iUsbIndexNo);

	// Token: 0x060007B6 RID: 1974 RVA: 0x00008E70 File Offset: 0x00007070
	public static int OnDeviceInfo(int iUsbIdx, StringBuilder strDeviceName, StringBuilder strFwVer, out int iDeviceType, out int iUsbIndexNo)
	{
		return NewMultiTouch.getDeviceinfo(iUsbIdx, strDeviceName, strFwVer, out iDeviceType, out iUsbIndexNo);
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x00039BDC File Offset: 0x00037DDC
	public static void UpdateCount()
	{
		NewMultiTouch.m_iCount = 0;
		for (int i = 0; i < 10; i++)
		{
			NewMultiTouch.ArrTouch[i].m_ePhase = TouchPhase.Ended;
		}
		for (int j = 0; j < NewMultiTouch.m_iTotalCount; j++)
		{
			if (NewMultiTouch.ArrTouch[j].Point.type == 2 || NewMultiTouch.ArrTouch[j].Point.type == 3)
			{
				NewMultiTouch.m_iCount++;
			}
			else if (NewMultiTouch.ArrTouch[j].Point.type == 1 && (NewMultiTouch.ArrTouch[j].m_iPreType == 2 || NewMultiTouch.ArrTouch[j].m_iPreType == 3))
			{
				NewMultiTouch.m_iCount++;
			}
			NewMultiTouch.ArrTouch[j].m_iPreType = NewMultiTouch.ArrTouch[j].Point.type;
		}
		Singleton<GameManager>.instance.UpdateTouch();
	}

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x060007B8 RID: 1976 RVA: 0x00008E7D File Offset: 0x0000707D
	public static int Count
	{
		get
		{
			return NewMultiTouch.m_iCount;
		}
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x00039CD4 File Offset: 0x00037ED4
	public static void OnPulseIREvent(int id, int type, int iprm, IntPtr pPt)
	{
		if (type == 2)
		{
			int num = 0;
			NewMultiTouch.m_iTotalCount = iprm;
			for (int i = 0; i < iprm; i++)
			{
				PULSEIR_POINT pulseir_POINT = (PULSEIR_POINT)Marshal.PtrToStructure(new IntPtr(pPt.ToInt32() + num), NewMultiTouch.pType.GetType());
				num += Marshal.SizeOf(NewMultiTouch.pType.GetType());
				if (10 > i)
				{
					NewMultiTouch.ArrTouch[i].Point = pulseir_POINT;
				}
			}
		}
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x00008E84 File Offset: 0x00007084
	public static TouchInfo GetTouch(int id)
	{
		return NewMultiTouch.ArrTouch[id];
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x00039D58 File Offset: 0x00037F58
	public static void OnStart()
	{
		NewMultiTouch.CallBackDelegate callBackDelegate = new NewMultiTouch.CallBackDelegate(NewMultiTouch.OnPulseIREvent);
		NewMultiTouch.OnTouchPulseIRInit(callBackDelegate);
	}

	// Token: 0x04000631 RID: 1585
	private const int MAXPOINT = 10;

	// Token: 0x04000632 RID: 1586
	public const int TOUCH_UP = 1;

	// Token: 0x04000633 RID: 1587
	public const int TOUCH_DOWN = 2;

	// Token: 0x04000634 RID: 1588
	public const int TOUCH_MOVE = 3;

	// Token: 0x04000635 RID: 1589
	public static TouchInfo[] ArrTouch = new TouchInfo[]
	{
		new TouchInfo(),
		new TouchInfo(),
		new TouchInfo(),
		new TouchInfo(),
		new TouchInfo(),
		new TouchInfo(),
		new TouchInfo(),
		new TouchInfo(),
		new TouchInfo(),
		new TouchInfo()
	};

	// Token: 0x04000636 RID: 1590
	private static int m_iCount = 0;

	// Token: 0x04000637 RID: 1591
	private static int m_iTotalCount = 0;

	// Token: 0x04000638 RID: 1592
	public static float MONITOR_WIDTH = (float)Screen.width;

	// Token: 0x04000639 RID: 1593
	public static float MONITOR_HEIGHT = (float)Screen.height;

	// Token: 0x0400063A RID: 1594
	public static PULSEIR_POINT pType = default(PULSEIR_POINT);

	// Token: 0x0400063B RID: 1595
	public static bool ONEVENT = false;

	// Token: 0x020000E7 RID: 231
	// (Invoke) Token: 0x060007BD RID: 1981
	public delegate void CallBackDelegate(int id, int type, int iprm, IntPtr pPt);
}
