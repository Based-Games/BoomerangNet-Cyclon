using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200021D RID: 541
public class GlobalDefineManager
{
	// Token: 0x06000F89 RID: 3977 RVA: 0x0000D580 File Offset: 0x0000B780
	private GlobalDefineManager()
	{
	}

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06000F8A RID: 3978 RVA: 0x0000D59E File Offset: 0x0000B79E
	public static GlobalDefineManager GetInstance
	{
		get
		{
			if (GlobalDefineManager.instance == null)
			{
				GlobalDefineManager.instance = new GlobalDefineManager();
			}
			return GlobalDefineManager.instance;
		}
	}

	// Token: 0x06000F8B RID: 3979 RVA: 0x000707B0 File Offset: 0x0006E9B0
	public void Init()
	{
		List<string> list = null;
		if (!this.DoesSMCSExist())
		{
			FileStream fileStream = File.Create(Application.dataPath + "/gmcs.rsp");
			fileStream.Close();
		}
		else
		{
			list = this.GetDefinesFromSMCS();
		}
		if (!this.DoGlobalDefinesExist())
		{
			Directory.CreateDirectory(Application.dataPath + "/Scripts/Util/Global Defines/Sync Data");
			FileStream fileStream2 = File.Create(Application.dataPath + "/Scripts/Util/Global Defines/Sync Data/defines.txt");
			fileStream2.Close();
		}
		else
		{
			this.PopulateGlobalsFromDefinesTXT();
		}
		this.SyncWithSMCS(list);
		this.Refresh();
	}

	// Token: 0x06000F8C RID: 3980 RVA: 0x0000D5B9 File Offset: 0x0000B7B9
	public void Deactivate()
	{
		this.m_allDefines.Clear();
	}

	// Token: 0x06000F8D RID: 3981 RVA: 0x00070844 File Offset: 0x0006EA44
	public bool OnGUI()
	{
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		this.m_scrollPosition = GUILayout.BeginScrollView(this.m_scrollPosition, new GUILayoutOption[0]);
		if ((this.m_allDefines != null) & (this.m_allDefines.Count > 0))
		{
			foreach (GlobalDefineManager.GlobalDefine globalDefine in this.m_allDefines)
			{
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (GUILayout.Button("Remove", new GUILayoutOption[] { GUILayout.Width(75f) }))
				{
					this.RemoveDefine(globalDefine);
					this.Refresh();
					return true;
				}
				Color color = GUI.color;
				GUI.color = ((!globalDefine.m_isActive) ? Color.red : Color.green);
				if (GUILayout.Button(globalDefine.m_defineName, new GUILayoutOption[0]))
				{
					this.ToggleDefine(globalDefine, !globalDefine.m_isActive);
					this.Refresh();
					return true;
				}
				GUI.color = color;
				GUILayout.EndHorizontal();
				GUILayout.EndVertical();
			}
		}
		GUILayout.EndScrollView();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		this.m_newDefine = GUILayout.TextField(this.m_newDefine, new GUILayoutOption[] { GUILayout.Width(150f) });
		if (GUILayout.Button("Add New", new GUILayoutOption[0]))
		{
			this.AddDefine(this.m_newDefine, true);
			this.m_newDefine = string.Empty;
			this.Refresh();
			return true;
		}
		if (GUILayout.Button("Refresh", new GUILayoutOption[0]))
		{
			this.Refresh();
			return true;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		return false;
	}

	// Token: 0x06000F8E RID: 3982 RVA: 0x0000D5C6 File Offset: 0x0000B7C6
	private void Refresh()
	{
		this.AddDefinesToSMCS();
		this.WriteDefinesToFile();
	}

	// Token: 0x06000F8F RID: 3983 RVA: 0x00070A20 File Offset: 0x0006EC20
	private List<string> GetDefinesFromSMCS()
	{
		List<string> list = new List<string>();
		using (StreamReader streamReader = File.OpenText(Application.dataPath + "/gmcs.rsp"))
		{
			string text = string.Empty;
			while ((text = streamReader.ReadLine()) != null)
			{
				string[] array = text.Split(new char[] { ':' });
				if (array[0] == "-define")
				{
					list.Add(array[1]);
				}
			}
		}
		return list;
	}

	// Token: 0x06000F90 RID: 3984 RVA: 0x00070AB0 File Offset: 0x0006ECB0
	private void WriteDefinesToFile()
	{
		if (this.m_allDefines != null)
		{
			using (StreamWriter streamWriter = new StreamWriter(Application.dataPath + "/Scripts/Util/Global Defines/Sync Data/defines.txt", false))
			{
				foreach (GlobalDefineManager.GlobalDefine globalDefine in this.m_allDefines)
				{
					string text = ((!globalDefine.m_isActive) ? "0" : "1") + ":" + globalDefine.m_defineName;
					streamWriter.WriteLine(text);
				}
			}
		}
	}

	// Token: 0x06000F91 RID: 3985 RVA: 0x00070B74 File Offset: 0x0006ED74
	private void SyncWithSMCS(List<string> smcsDefines)
	{
		if (smcsDefines != null)
		{
			foreach (string text in smcsDefines)
			{
				this.AddDefine(text, true);
			}
		}
	}

	// Token: 0x06000F92 RID: 3986 RVA: 0x00070BD0 File Offset: 0x0006EDD0
	private List<string> GetActiveDefinesFromGlobalDefines()
	{
		List<string> list = new List<string>();
		using (StreamReader streamReader = File.OpenText(Application.dataPath + "/Scripts/Util/Global Defines/Sync Data/defines.txt"))
		{
			string text = string.Empty;
			while ((text = streamReader.ReadLine()) != null)
			{
				string[] array = text.Split(new char[] { ':' });
				if (array[0] == "1")
				{
					list.Add(array[1]);
				}
			}
		}
		return list;
	}

	// Token: 0x06000F93 RID: 3987 RVA: 0x00070C60 File Offset: 0x0006EE60
	private void PopulateGlobalsFromDefinesTXT()
	{
		StreamReader streamReader = File.OpenText(Application.dataPath + "/Scripts/Util/Global Defines/Sync Data/defines.txt");
		string text = string.Empty;
		while ((text = streamReader.ReadLine()) != null)
		{
			string[] array = text.Split(new char[] { ':' });
			if (array[0] == "1")
			{
				this.AddDefine(array[1], true);
			}
			else if (array[0] == "0")
			{
				this.AddDefine(array[1], false);
			}
		}
		streamReader.Close();
	}

	// Token: 0x06000F94 RID: 3988 RVA: 0x00070CF0 File Offset: 0x0006EEF0
	private void AddDefinesToSMCS()
	{
		if (this.m_allDefines != null)
		{
			using (StreamWriter streamWriter = new StreamWriter(Application.dataPath + "/gmcs.rsp", false))
			{
				foreach (GlobalDefineManager.GlobalDefine globalDefine in this.m_allDefines)
				{
					if (globalDefine.m_isActive)
					{
						string text = "-define:" + globalDefine.m_defineName;
						streamWriter.WriteLine(text);
					}
				}
			}
		}
	}

	// Token: 0x06000F95 RID: 3989 RVA: 0x0000D5D4 File Offset: 0x0000B7D4
	private void ToggleDefine(GlobalDefineManager.GlobalDefine define, bool isActive)
	{
		define.m_isActive = isActive;
	}

	// Token: 0x06000F96 RID: 3990 RVA: 0x00070DA8 File Offset: 0x0006EFA8
	private void AddDefine(string defineText, bool isActive)
	{
		if (!this.m_allDefines.Exists((GlobalDefineManager.GlobalDefine x) => x.m_defineName == defineText))
		{
			GlobalDefineManager.GlobalDefine globalDefine = new GlobalDefineManager.GlobalDefine(isActive, defineText);
			this.m_allDefines.Add(globalDefine);
		}
	}

	// Token: 0x06000F97 RID: 3991 RVA: 0x0000D5DD File Offset: 0x0000B7DD
	private void RemoveDefine(GlobalDefineManager.GlobalDefine define)
	{
		this.m_allDefines.Remove(define);
	}

	// Token: 0x06000F98 RID: 3992 RVA: 0x0000D5EC File Offset: 0x0000B7EC
	private bool DoesSMCSExist()
	{
		return File.Exists(Application.dataPath + "/gmcs.rsp");
	}

	// Token: 0x06000F99 RID: 3993 RVA: 0x0000D602 File Offset: 0x0000B802
	private bool DoGlobalDefinesExist()
	{
		return File.Exists(Application.dataPath + "/Scripts/Util/Global Defines/Sync Data/defines.txt");
	}

	// Token: 0x0400115A RID: 4442
	private const string DEFINES_TXT_PATH_EXT = "/Scripts/Util/Global Defines/Sync Data/defines.txt";

	// Token: 0x0400115B RID: 4443
	private const string SMCS_RSP_PATH_EXT = "/gmcs.rsp";

	// Token: 0x0400115C RID: 4444
	private const string DEFINES_DIR_EXT = "/Scripts/Util/Global Defines/Sync Data";

	// Token: 0x0400115D RID: 4445
	private const string REMOVE_TEXT = "Remove";

	// Token: 0x0400115E RID: 4446
	private const string ADD_NEW_TEXT = "Add New";

	// Token: 0x0400115F RID: 4447
	private const string REFRESH_TEXT = "Refresh";

	// Token: 0x04001160 RID: 4448
	private const string ZERO_TEXT = "0";

	// Token: 0x04001161 RID: 4449
	private const string ONE_TEXT = "1";

	// Token: 0x04001162 RID: 4450
	private const string COLON_TEXT = ":";

	// Token: 0x04001163 RID: 4451
	private const string SMCS_DEFINE_PREFIX = "-define:";

	// Token: 0x04001164 RID: 4452
	private const string SMCS_DEFINE_PREFIX_NO_COLON = "-define";

	// Token: 0x04001165 RID: 4453
	private static GlobalDefineManager instance;

	// Token: 0x04001166 RID: 4454
	private List<GlobalDefineManager.GlobalDefine> m_allDefines = new List<GlobalDefineManager.GlobalDefine>();

	// Token: 0x04001167 RID: 4455
	private Vector2 m_scrollPosition;

	// Token: 0x04001168 RID: 4456
	private string m_newDefine = string.Empty;

	// Token: 0x0200021E RID: 542
	public class GlobalDefine
	{
		// Token: 0x06000F9A RID: 3994 RVA: 0x0000D618 File Offset: 0x0000B818
		public GlobalDefine(bool isActive, string defineName)
		{
			this.m_isActive = isActive;
			this.m_defineName = defineName;
		}

		// Token: 0x04001169 RID: 4457
		public bool m_isActive;

		// Token: 0x0400116A RID: 4458
		public string m_defineName;
	}
}
