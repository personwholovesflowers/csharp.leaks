using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using plog;
using UnityEngine;

// Token: 0x02000357 RID: 855
[ConfigureSingleton(SingletonFlags.PersistAutoInstance | SingletonFlags.DestroyDuplicates)]
public class PrefsManager : MonoSingleton<PrefsManager>
{
	// Token: 0x17000176 RID: 374
	// (get) Token: 0x060013DA RID: 5082 RVA: 0x0009E911 File Offset: 0x0009CB11
	public static string PrefsPath
	{
		get
		{
			return Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Preferences");
		}
	}

	// Token: 0x060013DB RID: 5083 RVA: 0x0009E92C File Offset: 0x0009CB2C
	public bool HasKey(string key)
	{
		this.EnsureInitialized();
		return this.prefMap.ContainsKey(key) || this.localPrefMap.ContainsKey(key);
	}

	// Token: 0x060013DC RID: 5084 RVA: 0x0009E950 File Offset: 0x0009CB50
	public void DeleteKey(string key)
	{
		this.EnsureInitialized();
		if (this.prefMap.ContainsKey(key))
		{
			this.prefMap.Remove(key);
			if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.DirtySlowTick)
			{
				this.isDirty = true;
			}
			if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.Immediate)
			{
				this.CommitPrefs(false);
			}
		}
		if (this.localPrefMap.ContainsKey(key))
		{
			this.localPrefMap.Remove(key);
			if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.DirtySlowTick)
			{
				this.isLocalDirty = true;
			}
			if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.Immediate)
			{
				this.CommitPrefs(true);
			}
		}
	}

	// Token: 0x060013DD RID: 5085 RVA: 0x0009E9D4 File Offset: 0x0009CBD4
	public bool GetBoolLocal(string key, bool fallback = false)
	{
		this.EnsureInitialized();
		object obj;
		if (this.localPrefMap.TryGetValue(key, out obj) && obj is bool)
		{
			return (bool)obj;
		}
		object obj2;
		if (this.defaultValues.TryGetValue(key, out obj2) && obj2 is bool)
		{
			return (bool)obj2;
		}
		return fallback;
	}

	// Token: 0x060013DE RID: 5086 RVA: 0x0009EA2C File Offset: 0x0009CC2C
	public bool GetBool(string key, bool fallback = false)
	{
		this.EnsureInitialized();
		object obj;
		if (this.prefMap.TryGetValue(key, out obj) && obj is bool)
		{
			return (bool)obj;
		}
		if (this.defaultValues.ContainsKey(key))
		{
			object obj2 = this.defaultValues[key];
			if (obj2 is bool)
			{
				return (bool)obj2;
			}
		}
		return fallback;
	}

	// Token: 0x060013DF RID: 5087 RVA: 0x0009EA90 File Offset: 0x0009CC90
	public void SetBoolLocal(string key, bool content)
	{
		this.EnsureInitialized();
		if (this.localPrefMap.ContainsKey(key))
		{
			this.localPrefMap[key] = content;
		}
		else
		{
			this.localPrefMap.Add(key, content);
		}
		if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.Immediate)
		{
			this.CommitPrefs(true);
		}
		if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.DirtySlowTick)
		{
			this.isLocalDirty = true;
		}
		Action<string, object> action = PrefsManager.onPrefChanged;
		if (action == null)
		{
			return;
		}
		action(key, content);
	}

	// Token: 0x060013E0 RID: 5088 RVA: 0x0009EB0C File Offset: 0x0009CD0C
	public void SetBool(string key, bool content)
	{
		this.EnsureInitialized();
		if (this.prefMap.ContainsKey(key))
		{
			this.prefMap[key] = content;
		}
		else
		{
			this.prefMap.Add(key, content);
		}
		if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.Immediate)
		{
			this.CommitPrefs(false);
		}
		if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.DirtySlowTick)
		{
			this.isDirty = true;
		}
		Action<string, object> action = PrefsManager.onPrefChanged;
		if (action == null)
		{
			return;
		}
		action(key, content);
	}

	// Token: 0x060013E1 RID: 5089 RVA: 0x0009EB88 File Offset: 0x0009CD88
	public int GetIntLocal(string key, int fallback = 0)
	{
		this.EnsureInitialized();
		object obj;
		if (this.localPrefMap.TryGetValue(key, out obj))
		{
			if (obj is int)
			{
				int num = (int)obj;
				return (int)this.EnsureValid(key, num);
			}
			if (obj is long)
			{
				long num2 = (long)obj;
				return (int)this.EnsureValid(key, (int)num2);
			}
			if (obj is float)
			{
				float num3 = (float)obj;
				return (int)this.EnsureValid(key, (int)num3);
			}
			if (obj is double)
			{
				double num4 = (double)obj;
				return (int)this.EnsureValid(key, (int)num4);
			}
		}
		object obj2;
		if (this.defaultValues.TryGetValue(key, out obj2) && obj2 is int)
		{
			return (int)obj2;
		}
		return fallback;
	}

	// Token: 0x060013E2 RID: 5090 RVA: 0x0009EC6C File Offset: 0x0009CE6C
	public int GetInt(string key, int fallback = 0)
	{
		this.EnsureInitialized();
		object obj;
		if (this.prefMap.TryGetValue(key, out obj))
		{
			if (obj is int)
			{
				int num = (int)obj;
				return (int)this.EnsureValid(key, num);
			}
			if (obj is long)
			{
				long num2 = (long)obj;
				return (int)this.EnsureValid(key, (int)num2);
			}
			if (obj is float)
			{
				float num3 = (float)obj;
				return (int)this.EnsureValid(key, (int)num3);
			}
			if (obj is double)
			{
				double num4 = (double)obj;
				return (int)this.EnsureValid(key, (int)num4);
			}
		}
		object obj2;
		if (this.defaultValues.TryGetValue(key, out obj2) && obj2 is int)
		{
			return (int)obj2;
		}
		return fallback;
	}

	// Token: 0x060013E3 RID: 5091 RVA: 0x0009ED50 File Offset: 0x0009CF50
	public void SetIntLocal(string key, int content)
	{
		this.EnsureInitialized();
		content = (int)this.EnsureValid(key, content);
		if (this.localPrefMap.ContainsKey(key))
		{
			this.localPrefMap[key] = content;
		}
		else
		{
			this.localPrefMap.Add(key, content);
		}
		if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.Immediate)
		{
			this.CommitPrefs(true);
		}
		if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.DirtySlowTick)
		{
			this.isLocalDirty = true;
		}
		Action<string, object> action = PrefsManager.onPrefChanged;
		if (action == null)
		{
			return;
		}
		action(key, content);
	}

	// Token: 0x060013E4 RID: 5092 RVA: 0x0009EDE0 File Offset: 0x0009CFE0
	public void SetInt(string key, int content)
	{
		this.EnsureInitialized();
		content = (int)this.EnsureValid(key, content);
		if (this.prefMap.ContainsKey(key))
		{
			this.prefMap[key] = content;
		}
		else
		{
			this.prefMap.Add(key, content);
		}
		if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.Immediate)
		{
			this.CommitPrefs(false);
		}
		if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.DirtySlowTick)
		{
			this.isDirty = true;
		}
		Action<string, object> action = PrefsManager.onPrefChanged;
		if (action == null)
		{
			return;
		}
		action(key, content);
	}

	// Token: 0x060013E5 RID: 5093 RVA: 0x0009EE70 File Offset: 0x0009D070
	public float GetFloatLocal(string key, float fallback = 0f)
	{
		this.EnsureInitialized();
		if (this.localPrefMap.ContainsKey(key))
		{
			object obj = this.localPrefMap[key];
			if (obj is float)
			{
				float num = (float)obj;
				return (float)this.EnsureValid(key, num);
			}
			obj = this.localPrefMap[key];
			if (obj is int)
			{
				int num2 = (int)obj;
				return (float)((int)this.EnsureValid(key, num2));
			}
			obj = this.localPrefMap[key];
			if (obj is long)
			{
				long num3 = (long)obj;
				return (float)((long)this.EnsureValid(key, num3));
			}
			obj = this.localPrefMap[key];
			if (obj is double)
			{
				double num4 = (double)obj;
				return (float)this.EnsureValid(key, (float)num4);
			}
		}
		if (this.defaultValues.ContainsKey(key))
		{
			object obj = this.defaultValues[key];
			if (obj is float)
			{
				return (float)obj;
			}
		}
		return fallback;
	}

	// Token: 0x060013E6 RID: 5094 RVA: 0x0009EF94 File Offset: 0x0009D194
	public float GetFloat(string key, float fallback = 0f)
	{
		this.EnsureInitialized();
		if (this.prefMap.ContainsKey(key))
		{
			object obj = this.prefMap[key];
			if (obj is float)
			{
				float num = (float)obj;
				return (float)this.EnsureValid(key, num);
			}
			obj = this.prefMap[key];
			if (obj is int)
			{
				int num2 = (int)obj;
				return (float)((int)this.EnsureValid(key, num2));
			}
			obj = this.prefMap[key];
			if (obj is long)
			{
				long num3 = (long)obj;
				return (float)((long)this.EnsureValid(key, num3));
			}
			obj = this.prefMap[key];
			if (obj is double)
			{
				double num4 = (double)obj;
				return (float)this.EnsureValid(key, (float)num4);
			}
		}
		if (this.defaultValues.ContainsKey(key))
		{
			object obj = this.defaultValues[key];
			if (obj is float)
			{
				return (float)obj;
			}
		}
		return fallback;
	}

	// Token: 0x060013E7 RID: 5095 RVA: 0x0009F0B8 File Offset: 0x0009D2B8
	public void SetFloatLocal(string key, float content)
	{
		this.EnsureInitialized();
		content = (float)this.EnsureValid(key, content);
		if (this.localPrefMap.ContainsKey(key))
		{
			this.localPrefMap[key] = content;
		}
		else
		{
			this.localPrefMap.Add(key, content);
		}
		if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.Immediate)
		{
			this.CommitPrefs(true);
		}
		if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.DirtySlowTick)
		{
			this.isLocalDirty = true;
		}
		Action<string, object> action = PrefsManager.onPrefChanged;
		if (action == null)
		{
			return;
		}
		action(key, content);
	}

	// Token: 0x060013E8 RID: 5096 RVA: 0x0009F148 File Offset: 0x0009D348
	public void SetFloat(string key, float content)
	{
		this.EnsureInitialized();
		content = (float)this.EnsureValid(key, content);
		if (this.prefMap.ContainsKey(key))
		{
			this.prefMap[key] = content;
		}
		else
		{
			this.prefMap.Add(key, content);
		}
		if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.Immediate)
		{
			this.CommitPrefs(false);
		}
		if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.DirtySlowTick)
		{
			this.isDirty = true;
		}
		Action<string, object> action = PrefsManager.onPrefChanged;
		if (action == null)
		{
			return;
		}
		action(key, content);
	}

	// Token: 0x060013E9 RID: 5097 RVA: 0x0009F1D8 File Offset: 0x0009D3D8
	public string GetStringLocal(string key, string fallback = null)
	{
		this.EnsureInitialized();
		if (this.localPrefMap.ContainsKey(key))
		{
			string text = this.localPrefMap[key] as string;
			if (text != null)
			{
				return this.EnsureValid(key, text) as string;
			}
		}
		if (this.defaultValues.ContainsKey(key))
		{
			string text2 = this.defaultValues[key] as string;
			if (text2 != null)
			{
				return text2;
			}
		}
		return fallback;
	}

	// Token: 0x060013EA RID: 5098 RVA: 0x0009F244 File Offset: 0x0009D444
	public string GetString(string key, string fallback = null)
	{
		this.EnsureInitialized();
		if (this.prefMap.ContainsKey(key))
		{
			string text = this.prefMap[key] as string;
			if (text != null)
			{
				return this.EnsureValid(key, text) as string;
			}
		}
		if (this.defaultValues.ContainsKey(key))
		{
			string text2 = this.defaultValues[key] as string;
			if (text2 != null)
			{
				return text2;
			}
		}
		return fallback;
	}

	// Token: 0x060013EB RID: 5099 RVA: 0x0009F2B0 File Offset: 0x0009D4B0
	public void SetStringLocal(string key, string content)
	{
		this.EnsureInitialized();
		content = this.EnsureValid(key, content) as string;
		if (this.localPrefMap.ContainsKey(key))
		{
			this.localPrefMap[key] = content;
		}
		else
		{
			this.localPrefMap.Add(key, content);
		}
		if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.Immediate)
		{
			this.CommitPrefs(true);
		}
		if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.DirtySlowTick)
		{
			this.isLocalDirty = true;
		}
		Action<string, object> action = PrefsManager.onPrefChanged;
		if (action == null)
		{
			return;
		}
		action(key, content);
	}

	// Token: 0x060013EC RID: 5100 RVA: 0x0009F32C File Offset: 0x0009D52C
	public void SetString(string key, string content)
	{
		this.EnsureInitialized();
		content = this.EnsureValid(key, content) as string;
		if (this.prefMap.ContainsKey(key))
		{
			this.prefMap[key] = content;
		}
		else
		{
			this.prefMap.Add(key, content);
		}
		if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.Immediate)
		{
			this.CommitPrefs(false);
		}
		if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.DirtySlowTick)
		{
			this.isDirty = true;
		}
		Action<string, object> action = PrefsManager.onPrefChanged;
		if (action == null)
		{
			return;
		}
		action(key, content);
	}

	// Token: 0x060013ED RID: 5101 RVA: 0x0009F3A8 File Offset: 0x0009D5A8
	private void CommitPrefs(bool local)
	{
		if (local)
		{
			string text = JsonConvert.SerializeObject(this.localPrefMap, Formatting.Indented);
			this.localPrefsStream.SetLength(0L);
			StreamWriter streamWriter = new StreamWriter(this.localPrefsStream);
			streamWriter.Write(text);
			streamWriter.Flush();
			return;
		}
		string text2 = JsonConvert.SerializeObject(this.prefMap, Formatting.Indented);
		this.prefsStream.SetLength(0L);
		StreamWriter streamWriter2 = new StreamWriter(this.prefsStream);
		streamWriter2.Write(text2);
		streamWriter2.Flush();
	}

	// Token: 0x060013EE RID: 5102 RVA: 0x0009F41C File Offset: 0x0009D61C
	private void UpdateTimestamp()
	{
		this.EnsureInitialized();
		DateTime now = DateTime.Now;
		if (!this.prefMap.ContainsKey("lastTimePlayed.year"))
		{
			this.prefMap.Add("lastTimePlayed.year", now.Year);
			this.isDirty = true;
		}
		if (!this.prefMap.ContainsKey("lastTimePlayed.month"))
		{
			this.prefMap.Add("lastTimePlayed.month", now.Month);
			this.isDirty = true;
		}
		object obj = this.prefMap["lastTimePlayed.year"];
		if (obj is int)
		{
			int num = (int)obj;
			if (num != now.Year)
			{
				this.prefMap["lastTimePlayed.year"] = now.Year;
				this.prefMap["lastTimePlayed.month"] = now.Month;
				this.isDirty = true;
			}
			obj = this.prefMap["lastTimePlayed.month"];
			if (obj is int)
			{
				int num2 = (int)obj;
				if (num2 != now.Month)
				{
					this.prefMap["lastTimePlayed.month"] = now.Month;
					this.isDirty = true;
					return;
				}
			}
		}
		else
		{
			this.prefMap["lastTimePlayed.year"] = now.Year;
			this.prefMap["lastTimePlayed.month"] = now.Month;
			this.isDirty = true;
		}
	}

	// Token: 0x060013EF RID: 5103 RVA: 0x0009F598 File Offset: 0x0009D798
	private void EnsureInitialized()
	{
		if (this.prefMap == null || this.localPrefMap == null)
		{
			this.Initialize();
		}
	}

	// Token: 0x060013F0 RID: 5104 RVA: 0x0009F5B0 File Offset: 0x0009D7B0
	private void Initialize()
	{
		if (!Directory.Exists(PrefsManager.PrefsPath))
		{
			Directory.CreateDirectory(PrefsManager.PrefsPath);
		}
		this.timeSinceLastTick = 0f;
		if (this.prefsStream == null)
		{
			this.prefsStream = new FileStream(Path.Combine(PrefsManager.PrefsPath, "Prefs.json"), FileMode.OpenOrCreate);
		}
		if (this.localPrefsStream == null)
		{
			this.localPrefsStream = new FileStream(Path.Combine(PrefsManager.PrefsPath, "LocalPrefs.json"), FileMode.OpenOrCreate);
		}
		this.prefMap = this.LoadPrefs(this.prefsStream);
		this.localPrefMap = this.LoadPrefs(this.localPrefsStream);
		if (!File.Exists(Path.Combine(PrefsManager.PrefsPath, "NOTE.txt")))
		{
			File.WriteAllText(Path.Combine(PrefsManager.PrefsPath, "NOTE.txt"), PrefsManager.prefsNote);
			PrefsManager.Log.Warning("NOTE.txt created in prefs folder. Please read it.", null, null, null);
		}
		PrefsManager.Log.Info("PrefsManager initialized", null, null, null);
	}

	// Token: 0x060013F1 RID: 5105 RVA: 0x0009F6A1 File Offset: 0x0009D8A1
	private object EnsureValid(string key, object value)
	{
		if (!this.propertyValidators.ContainsKey(key))
		{
			return value;
		}
		return this.propertyValidators[key](value) ?? value;
	}

	// Token: 0x060013F2 RID: 5106 RVA: 0x0009F6CA File Offset: 0x0009D8CA
	private Dictionary<string, object> LoadPrefs(FileStream stream)
	{
		return JsonConvert.DeserializeObject<Dictionary<string, object>>(new StreamReader(stream).ReadToEnd()) ?? new Dictionary<string, object>();
	}

	// Token: 0x060013F3 RID: 5107 RVA: 0x0009F6E8 File Offset: 0x0009D8E8
	protected override void Awake()
	{
		base.Awake();
		if (MonoSingleton<PrefsManager>.Instance != this)
		{
			return;
		}
		this.Initialize();
		int @int = this.GetInt("lastTimePlayed.year", -1);
		int int2 = this.GetInt("lastTimePlayed.month", -1);
		if (@int == -1 || int2 == -1)
		{
			PrefsManager.monthsSinceLastPlayed = 0;
			return;
		}
		DateTime now = DateTime.Now;
		PrefsManager.monthsSinceLastPlayed = (now.Year - @int) * 12 + now.Month - int2;
	}

	// Token: 0x060013F4 RID: 5108 RVA: 0x0009F759 File Offset: 0x0009D959
	private void Start()
	{
		this.UpdateTimestamp();
	}

	// Token: 0x060013F5 RID: 5109 RVA: 0x0009F764 File Offset: 0x0009D964
	private void FixedUpdate()
	{
		if (!this.isDirty && !this.isLocalDirty)
		{
			return;
		}
		if (PrefsManager.CommitMode != PrefsManager.PrefsCommitMode.DirtySlowTick)
		{
			return;
		}
		if (this.timeSinceLastTick < 3f)
		{
			return;
		}
		this.timeSinceLastTick = 0f;
		if (this.isLocalDirty)
		{
			this.CommitPrefs(true);
		}
		if (this.isDirty)
		{
			this.CommitPrefs(false);
		}
		this.isLocalDirty = false;
		this.isDirty = false;
	}

	// Token: 0x060013F6 RID: 5110 RVA: 0x0009F7DC File Offset: 0x0009D9DC
	private void OnApplicationQuit()
	{
		this.UpdateTimestamp();
		if (PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.OnQuit || PrefsManager.CommitMode == PrefsManager.PrefsCommitMode.DirtySlowTick)
		{
			this.CommitPrefs(false);
			this.CommitPrefs(true);
		}
		FileStream fileStream = this.prefsStream;
		if (fileStream != null)
		{
			fileStream.Close();
		}
		FileStream fileStream2 = this.localPrefsStream;
		if (fileStream2 == null)
		{
			return;
		}
		fileStream2.Close();
	}

	// Token: 0x060013F7 RID: 5111 RVA: 0x0009F830 File Offset: 0x0009DA30
	public PrefsManager()
	{
		Dictionary<string, Func<object, object>> dictionary = new Dictionary<string, Func<object, object>>();
		dictionary.Add("difficulty", delegate(object value)
		{
			if (!(value is int))
			{
				PrefsManager.Log.Warning("Difficulty value is not an int", null, null, null);
				return 2;
			}
			int num = (int)value;
			if (num < 0 || num > 4)
			{
				PrefsManager.Log.Warning("Difficulty validation error", null, null, null);
				return 4;
			}
			return null;
		});
		dictionary.Add("cyberGrind.startingWave", delegate(object value)
		{
			PrefsManager.Log.Info("Validating CyberGrindStartingWave", null, null, null);
			if (!(value is int))
			{
				PrefsManager.Log.Warning("CyberGrindStartingWave value is not an int", null, null, null);
				return 0;
			}
			int num2 = (int)value;
			int safeStartingWave = WaveUtils.GetSafeStartingWave(num2);
			if (safeStartingWave != num2)
			{
				return safeStartingWave;
			}
			int? highestWaveForDifficulty = WaveUtils.GetHighestWaveForDifficulty(MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0));
			if (highestWaveForDifficulty == null)
			{
				PrefsManager.Log.Warning("No wave data found for difficulty level", null, null, null);
				return 0;
			}
			if (!WaveUtils.IsWaveSelectable(num2, highestWaveForDifficulty.Value))
			{
				PrefsManager.Log.Warning(string.Format("Wave {0} is not unlocked yet. Highest wave: {1}", num2, highestWaveForDifficulty.Value), null, null, null);
				return 0;
			}
			return null;
		});
		this.propertyValidators = dictionary;
		this.defaultValues = new Dictionary<string, object>
		{
			{ "difficulty", 2 },
			{ "scrollEnabled", true },
			{ "scrollWeapons", true },
			{ "scrollVariations", false },
			{ "scrollReversed", false },
			{ "mouseSensitivity", 50f },
			{ "discordIntegration", true },
			{ "levelLeaderboards", true },
			{ "subtitlesEnabled", false },
			{ "seasonalEvents", true },
			{ "majorAssist", false },
			{ "gameSpeed", 1f },
			{ "damageTaken", 1f },
			{ "infiniteStamina", false },
			{ "disableWhiplashHardDamage", false },
			{ "disableHardDamage", false },
			{ "disableWeaponFreshness", false },
			{ "autoAim", false },
			{ "autoAimAmount", 0.2f },
			{ "bossDifficultyOverride", 0 },
			{ "hideMajorAssistPopup", false },
			{ "outlineThickness", 1 },
			{ "disableHitParticles", false },
			{ "frameRateLimit", 1 },
			{ "fullscreen", true },
			{ "fieldOfView", 105f },
			{ "vSync", true },
			{ "totalRumbleIntensity", 1f },
			{ "musicVolume", 0.6f },
			{ "sfxVolume", 1f },
			{ "allVolume", 1f },
			{ "muffleMusic", true },
			{ "screenShake", 1f },
			{ "cameraTilt", true },
			{ "parryFlash", true },
			{ "dithering", 0.2f },
			{ "colorCompression", 2 },
			{ "vertexWarping", 0 },
			{ "textureWarping", 0f },
			{ "pixelization", 0 },
			{ "gamma", 1f },
			{ "crossHair", 1 },
			{ "crossHairColor", 1 },
			{ "crossHairHud", 2 },
			{ "hudType", 1 },
			{ "hudBackgroundOpacity", 50f },
			{ "WeaponRedrawBehaviour", 0 },
			{ "weaponHoldPosition", 0 },
			{ "variationMemory", false },
			{ "pauseMenuConfirmationDialogs", 0 },
			{ "sandboxSaveOverwriteWarnings", true },
			{
				"disabledComputeShaders",
				!SystemInfoEx.supportsComputeShaders
			},
			{ "bloodEnabled", true },
			{ "bloodStainChance", 0.5f },
			{ "bloodStainMax", 100000f },
			{ "maxGore", 3000f },
			{ "weaponIcons", true },
			{ "armIcons", true },
			{ "styleMeter", true },
			{ "styleInfo", true },
			{ "crossHairHudFade", true },
			{ "powerUpMeter", true },
			{ "railcannonMeter", true },
			{ "selectedSaveSlot", 0 }
		};
		base..ctor();
	}

	// Token: 0x04001B42 RID: 6978
	private static readonly global::plog.Logger Log = new global::plog.Logger("PrefsManager");

	// Token: 0x04001B43 RID: 6979
	private FileStream prefsStream;

	// Token: 0x04001B44 RID: 6980
	private FileStream localPrefsStream;

	// Token: 0x04001B45 RID: 6981
	private static PrefsManager.PrefsCommitMode CommitMode = (Application.isEditor ? PrefsManager.PrefsCommitMode.Immediate : PrefsManager.PrefsCommitMode.DirtySlowTick);

	// Token: 0x04001B46 RID: 6982
	private TimeSince timeSinceLastTick;

	// Token: 0x04001B47 RID: 6983
	private const float SlowTickCommitInterval = 3f;

	// Token: 0x04001B48 RID: 6984
	private const bool DebugLogging = false;

	// Token: 0x04001B49 RID: 6985
	private bool isDirty;

	// Token: 0x04001B4A RID: 6986
	private bool isLocalDirty;

	// Token: 0x04001B4B RID: 6987
	public static int monthsSinceLastPlayed = 0;

	// Token: 0x04001B4C RID: 6988
	public static Action<string, object> onPrefChanged;

	// Token: 0x04001B4D RID: 6989
	public Dictionary<string, object> prefMap;

	// Token: 0x04001B4E RID: 6990
	public Dictionary<string, object> localPrefMap;

	// Token: 0x04001B4F RID: 6991
	private readonly Dictionary<string, Func<object, object>> propertyValidators;

	// Token: 0x04001B50 RID: 6992
	public readonly Dictionary<string, object> defaultValues;

	// Token: 0x04001B51 RID: 6993
	private static string prefsNote = "LocalPrefs.json contains preferences local to this machine. It does NOT get backed up in any way.\nPrefs.json contains preferences that are synced across all of your machines via Steam Cloud.\n\nPlaylist.json contains the IDs of all the songs in the user's Cybergrind Playlist.All prefs files must be valid json.\nIf you edit them, make sure you don't break the format.\n\nIf a pref key is missing from the prefs files, the game will use the default value and save ONLY if overridden.\nAdditionally, you can NOT move things between Prefs.json and LocalPrefs.json, the game will ignore them if misplaced.";

	// Token: 0x02000358 RID: 856
	private enum PrefsCommitMode
	{
		// Token: 0x04001B53 RID: 6995
		Immediate,
		// Token: 0x04001B54 RID: 6996
		OnQuit,
		// Token: 0x04001B55 RID: 6997
		DirtySlowTick
	}
}
