using System;
using System.IO.Ports;
using System.Threading;

// Token: 0x02000104 RID: 260
public class SerialPortManager
{
	// Token: 0x06000945 RID: 2373 RVA: 0x00009A5A File Offset: 0x00007C5A
	protected SerialPortManager()
	{
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06000947 RID: 2375 RVA: 0x00044798 File Offset: 0x00042998
	public static SerialPortManager Instance
	{
		get
		{
			if (SerialPortManager.s_Instance == null)
			{
				object obj = SerialPortManager.padlock;
				lock (obj)
				{
					SerialPortManager.s_Instance = new SerialPortManager();
				}
			}
			return SerialPortManager.s_Instance;
		}
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06000948 RID: 2376 RVA: 0x00009A7F File Offset: 0x00007C7F
	public object Lock
	{
		get
		{
			return this.m_lock;
		}
	}

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x06000949 RID: 2377 RVA: 0x00009A87 File Offset: 0x00007C87
	public SerialPort SPort
	{
		get
		{
			return this.serialPort1;
		}
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x000447E8 File Offset: 0x000429E8
	public void Serial_Open()
	{
		if (this.serialPort1.IsOpen)
		{
			this.serialPort1.Close();
		}
		this.serialPort1.PortName = "COM7";
		this.serialPort1.BaudRate = 115200;
		this.serialPort1.DataBits = 8;
		this.serialPort1.Parity = Parity.None;
		this.serialPort1.StopBits = StopBits.One;
		this.serialPort1.ReadBufferSize = 128;
		this.serialPort1.Open();
		this.th = new Thread(new ThreadStart(this.ThreadOn));
		this.th.Start();
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x00044894 File Offset: 0x00042A94
	public void ThreadOn()
	{
		for (;;)
		{
			try
			{
				object @lock = SerialPortManager.Instance.Lock;
				lock (@lock)
				{
					byte b = (byte)this.serialPort1.ReadByte();
					Console.WriteLine("asdfasdfasdfasdfasdf");
					while (b != 255)
					{
						if (b == 24)
						{
							Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_TITLE_COIN_IN, false);
						}
						else if (b != 3)
						{
							if (b == 1)
							{
								Singleton<SoundSourceManager>.instance.Play(SOUNDINDEX.SFX_TITLE_COIN_IN, false);
							}
						}
						b = (byte)this.serialPort1.ReadByte();
					}
				}
			}
			catch (Exception ex)
			{
			}
			Thread.Sleep(1);
		}
	}

	// Token: 0x04000770 RID: 1904
	private const int MAX_IO_SENDBUF = 1024;

	// Token: 0x04000771 RID: 1905
	private const int IO_TIMEOUT = 5000;

	// Token: 0x04000772 RID: 1906
	private object m_lock;

	// Token: 0x04000773 RID: 1907
	private SerialPort serialPort1 = new SerialPort();

	// Token: 0x04000774 RID: 1908
	private static SerialPortManager s_Instance = null;

	// Token: 0x04000775 RID: 1909
	private static readonly object padlock = new object();

	// Token: 0x04000776 RID: 1910
	private int m_iSendBufferSize;

	// Token: 0x04000777 RID: 1911
	private Thread th;
}
