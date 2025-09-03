using System;
using System.Collections.Generic;
using System.IO;
using MiniJSON;

// Token: 0x0200049E RID: 1182
public class ConfigManager
{
	// Token: 0x0600158D RID: 5517 RVA: 0x00012367 File Offset: 0x00010567
	private ConfigManager(string configPath)
	{
		this._configPath = configPath;
		this._config = new Dictionary<string, object>();
		this.FullLoad();
	}

	// Token: 0x0600158E RID: 5518 RVA: 0x0008EB48 File Offset: 0x0008CD48
	private void FullLoad()
	{
		if (!File.Exists(this._configPath) || string.IsNullOrEmpty(File.ReadAllText(this._configPath)))
		{
			Logger.Warn("ConfigManager", "Config file not found or empty at " + this._configPath + ", attempting to migrate from JSON.", new object[0]);
			if (File.Exists(this._oldJsonPath))
			{
				try
				{
					string text = File.ReadAllText(this._oldJsonPath);
					this._config = Json.Deserialize(text) as Dictionary<string, object>;
					if (this._config == null)
					{
						Logger.Warn("ConfigManager", "Failed to deserialize JSON config from " + this._oldJsonPath, new object[0]);
						this._config = new Dictionary<string, object>();
					}
					else
					{
						Logger.Log("ConfigManager", "Migrated JSON config to YAML structure.", new object[0]);
					}
					goto IL_129;
				}
				catch (Exception ex)
				{
					Logger.Error("ConfigManager", "Failed to load JSON config from " + this._oldJsonPath + ": " + ex.Message, new object[0]);
					this._config = new Dictionary<string, object>();
					goto IL_129;
				}
			}
			Logger.Warn("ConfigManager", "No JSON config found at " + this._oldJsonPath + ", creating default YAML config.", new object[0]);
			this._config = new Dictionary<string, object>();
			IL_129:
			this.Save();
			return;
		}
		try
		{
			string[] array = File.ReadAllLines(this._configPath);
			this._config = this.ParseYaml(array);
			Logger.Log("ConfigManager", "Config loaded successfully from " + this._configPath, new object[0]);
		}
		catch (Exception ex2)
		{
			Logger.Error("ConfigManager", "Failed to load config from " + this._configPath + ": " + ex2.Message, new object[0]);
			throw;
		}
	}

	// Token: 0x0600158F RID: 5519 RVA: 0x00012392 File Offset: 0x00010592
	public Dictionary<string, object> SoftLoad()
	{
		return this._config;
	}

	// Token: 0x06001590 RID: 5520 RVA: 0x0008ED0C File Offset: 0x0008CF0C
	public void Set(string path, object value)
	{
		string[] array = path.Split(new char[] { '.' });
		Dictionary<string, object> dictionary = this._config;
		for (int i = 0; i < array.Length - 1; i++)
		{
			string text = array[i];
			if (!dictionary.ContainsKey(text))
			{
				dictionary[text] = new Dictionary<string, object>();
			}
			dictionary = dictionary[text] as Dictionary<string, object>;
			if (dictionary == null)
			{
				Logger.Warn("ConfigManager", "Invalid config structure for path: " + path, new object[0]);
				throw new InvalidOperationException("Invalid config structure.");
			}
		}
		dictionary[array[array.Length - 1]] = value;
		this.Save();
	}

	// Token: 0x06001591 RID: 5521 RVA: 0x0008EDA8 File Offset: 0x0008CFA8
	private void Save()
	{
		try
		{
			string text = this.SerializeToYaml(this._config, 0);
			File.WriteAllText(this._configPath, text);
			Logger.Log("ConfigManager", "Config saved to " + this._configPath, new object[0]);
		}
		catch (Exception ex)
		{
			Logger.Warn("ConfigManager", "Failed to save config to " + this._configPath + ": " + ex.Message, new object[0]);
			throw;
		}
		this.FullLoad();
	}

	// Token: 0x06001592 RID: 5522 RVA: 0x0001239A File Offset: 0x0001059A
	public static void Initialize(string configPath)
	{
		if (ConfigManager.instance != null)
		{
			Logger.Warn("ConfigManager", "Already initialized, ignoring new initialization.", new object[0]);
			return;
		}
		ConfigManager.instance = new ConfigManager(configPath);
	}

	// Token: 0x1700035F RID: 863
	// (get) Token: 0x06001593 RID: 5523 RVA: 0x000123C4 File Offset: 0x000105C4
	public static ConfigManager Instance
	{
		get
		{
			if (ConfigManager.instance == null)
			{
				Logger.Warn("ConfigManager", "Not initialized. Call Initialize first.", new object[0]);
				throw new InvalidOperationException("ConfigManager not initialized.");
			}
			return ConfigManager.instance;
		}
	}

	// Token: 0x06001594 RID: 5524 RVA: 0x0008EE38 File Offset: 0x0008D038
	private Dictionary<string, object> ParseYaml(string[] lines)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Stack<Dictionary<string, object>> stack = new Stack<Dictionary<string, object>>();
		stack.Push(dictionary);
		int num = 0;
		foreach (string text in lines)
		{
			string text2 = text.Trim();
			if (!string.IsNullOrEmpty(text2) && !text2.StartsWith("#"))
			{
				int num2 = text.Length - text.TrimStart(new char[0]).Length;
				string text3 = text2;
				while (num2 < num && stack.Count > 1)
				{
					stack.Pop();
					num -= 2;
				}
				if (text3.Contains(":"))
				{
					string[] array = text3.Split(new char[] { ':' }, 2);
					string text4 = array[0].Trim();
					string text5 = ((array.Length > 1) ? array[1].Trim() : "");
					if (string.IsNullOrEmpty(text5))
					{
						Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
						stack.Peek()[text4] = dictionary2;
						stack.Push(dictionary2);
						num = num2 + 2;
					}
					else
					{
						stack.Peek()[text4] = this.ParseValue(text5);
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06001595 RID: 5525 RVA: 0x0008EF68 File Offset: 0x0008D168
	private object ParseValue(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return "";
		}
		bool flag;
		if (bool.TryParse(value, out flag))
		{
			return flag;
		}
		int num;
		if (int.TryParse(value, out num))
		{
			return num;
		}
		float num2;
		if (float.TryParse(value, out num2))
		{
			return num2;
		}
		return value;
	}

	// Token: 0x06001596 RID: 5526 RVA: 0x0008EFB8 File Offset: 0x0008D1B8
	private string SerializeToYaml(Dictionary<string, object> dict, int indent)
	{
		string text = "";
		string text2 = new string(' ', indent);
		foreach (KeyValuePair<string, object> keyValuePair in dict)
		{
			Dictionary<string, object> dictionary = keyValuePair.Value as Dictionary<string, object>;
			if (dictionary != null)
			{
				text = text + text2 + keyValuePair.Key + ":\n";
				text += this.SerializeToYaml(dictionary, indent + 2);
			}
			else
			{
				string[] array = new string[6];
				array[0] = text;
				array[1] = text2;
				array[2] = keyValuePair.Key;
				array[3] = ": ";
				int num = 4;
				object value = keyValuePair.Value;
				array[num] = ((value != null) ? value.ToString() : null);
				array[5] = "\n";
				text = string.Concat(array);
			}
		}
		return text;
	}

	// Token: 0x06001597 RID: 5527 RVA: 0x0008F09C File Offset: 0x0008D29C
	public T Get<T>(string path, bool fullLoad = false)
	{
		if (fullLoad)
		{
			this.FullLoad();
		}
		string[] array = path.Split(new char[] { '.' });
		object obj = this._config;
		foreach (string text in array)
		{
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null || !dictionary.ContainsKey(text))
			{
				Logger.Warn("ConfigManager", "Config key '{0}' not found.", new object[] { path });
				return ConfigManager.GetDefault<T>();
			}
			obj = dictionary[text];
		}
		T t;
		try
		{
			t = (t = (T)((object)Convert.ChangeType(obj, typeof(T))));
		}
		catch (Exception ex)
		{
			Type typeFromHandle = typeof(T);
			string text2 = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
			Logger.Warn("ConfigManager", "Failed to convert key {0} to type {1}: {2}", new object[] { path, text2, ex.Message });
			return ConfigManager.GetDefault<T>();
		}
		return t;
	}

	// Token: 0x06001598 RID: 5528 RVA: 0x0008F19C File Offset: 0x0008D39C
	private static T GetDefault<T>()
	{
		if (typeof(T) == typeof(string))
		{
			return (T)((object)string.Empty);
		}
		return default(T);
	}

	// Token: 0x04001D24 RID: 7460
	private readonly string _configPath;

	// Token: 0x04001D25 RID: 7461
	private Dictionary<string, object> _config;

	// Token: 0x04001D26 RID: 7462
	private static ConfigManager instance;

	// Token: 0x04001D27 RID: 7463
	private readonly string _oldJsonPath = "../Data/System/JSON/config.json";
}
