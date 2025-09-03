using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x0200048E RID: 1166
public static class Logger
{
	// Token: 0x0600156F RID: 5487 RVA: 0x0008E62C File Offset: 0x0008C82C
	static Logger()
	{
		Application.RegisterLogCallback(new Application.LogCallback(Logger.LogCallback));
		if (Logger.EnableLogging)
		{
			string text = Logger.FormatMessage("BNCyclon", "Logger initialized", LogType.Log);
			Debug.Log(text);
			if (Logger.LogToFile)
			{
				try
				{
					File.WriteAllText(Logger.LogFilePath, string.Empty);
					File.AppendAllText(Logger.LogFilePath, text + "\n");
				}
				catch (Exception ex)
				{
					Debug.LogError("BNCyclon: Failed to write to log file: " + ex.Message);
				}
			}
		}
		ConfigManager.Initialize("config.yaml");
		if (ConfigManager.Instance.Get<bool>("game.debug", false))
		{
			Logger.AllocConsole();
			Console.SetOut(new StreamWriter(Console.OpenStandardOutput())
			{
				AutoFlush = true
			});
		}
		Logger.Log("BNCyclon", "Version: " + GameData.XYCLON_VERSION + " Lang: " + GameData.LANGUAGE, new object[0]);
	}

	// Token: 0x06001570 RID: 5488 RVA: 0x0008E734 File Offset: 0x0008C934
	private static void LogCallback(string logString, string stackTrace, LogType type)
	{
		if (!Logger.EnableLogging || !Logger.LogToFile)
		{
			return;
		}
		string text = string.Concat(new string[]
		{
			"[",
			DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
			"] [",
			type.ToString(),
			"] ",
			logString
		});
		if (type == LogType.Error || type == LogType.Exception)
		{
			text = text + "\nStackTrace: " + stackTrace;
		}
		try
		{
			Console.WriteLine(text);
			File.AppendAllText(Logger.LogFilePath, text + "\n");
		}
		catch (Exception ex)
		{
			Debug.LogError("BNCyclon: Failed to write to log file: " + ex.Message);
		}
	}

	// Token: 0x06001571 RID: 5489 RVA: 0x00012290 File Offset: 0x00010490
	public static void Log(string context, string message, params object[] args)
	{
		if (!Logger.EnableLogging)
		{
			return;
		}
		Debug.Log(Logger.FormatMessage(context, string.Format(message, args), LogType.Log));
	}

	// Token: 0x06001572 RID: 5490 RVA: 0x000122AD File Offset: 0x000104AD
	public static void Warn(string context, string message, params object[] args)
	{
		if (!Logger.EnableLogging)
		{
			return;
		}
		Debug.LogWarning(Logger.FormatMessage(context, string.Format(message, args), LogType.Warning));
	}

	// Token: 0x06001573 RID: 5491 RVA: 0x000122CA File Offset: 0x000104CA
	public static void Error(string context, string message, params object[] args)
	{
		if (!Logger.EnableLogging)
		{
			return;
		}
		Debug.LogError(Logger.FormatMessage(context, string.Format(message, args), LogType.Error));
	}

	// Token: 0x06001574 RID: 5492 RVA: 0x000122E7 File Offset: 0x000104E7
	private static string FormatMessage(string context, string message, LogType type)
	{
		return string.Concat(new string[] { "[", context, "] ", message });
	}

	// Token: 0x06001575 RID: 5493
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool AllocConsole();

	// Token: 0x06001576 RID: 5494
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool FreeConsole();

	// Token: 0x04001CDD RID: 7389
	private static readonly string LogFilePath = "log.txt";

	// Token: 0x04001CDE RID: 7390
	private static readonly bool EnableLogging = true;

	// Token: 0x04001CDF RID: 7391
	private static readonly bool LogToFile = true;
}
