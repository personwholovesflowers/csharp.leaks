using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// Token: 0x02000218 RID: 536
public static class GameProgressSaver
{
	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06000B43 RID: 2883 RVA: 0x000509C7 File Offset: 0x0004EBC7
	public static string BaseSavePath
	{
		get
		{
			return Path.Combine((SystemInfo.deviceType == DeviceType.Desktop) ? Directory.GetParent(Application.dataPath).FullName : Application.persistentDataPath, "Saves");
		}
	}

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06000B44 RID: 2884 RVA: 0x000509F1 File Offset: 0x0004EBF1
	public static string SavePath
	{
		get
		{
			return Path.Combine(GameProgressSaver.BaseSavePath, string.Format("Slot{0}", GameProgressSaver.currentSlot + 1));
		}
	}

	// Token: 0x06000B45 RID: 2885 RVA: 0x00050A14 File Offset: 0x0004EC14
	private static string DifficultySavePath(int diff)
	{
		if (!SceneHelper.IsPlayingCustom)
		{
			return Path.Combine(GameProgressSaver.SavePath, string.Format("difficulty{0}progress.bepis", diff));
		}
		if (!string.IsNullOrEmpty(GameStateManager.Instance.currentCustomGame.campaignId))
		{
			return Path.Combine(GameProgressSaver.customCampaignsDir, GameStateManager.Instance.currentCustomGame.campaignId, string.Format("difficulty{0}progress.bepis", diff));
		}
		return Path.Combine(GameProgressSaver.customCampaignsDir, string.Format("difficulty{0}progress.bepis", diff));
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x06000B46 RID: 2886 RVA: 0x00050A9D File Offset: 0x0004EC9D
	private static string currentDifficultyPath
	{
		get
		{
			return GameProgressSaver.DifficultySavePath(MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0));
		}
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06000B47 RID: 2887 RVA: 0x00050AB4 File Offset: 0x0004ECB4
	private static string customSavesDir
	{
		get
		{
			return Path.Combine(GameProgressSaver.SavePath, "Custom");
		}
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x06000B48 RID: 2888 RVA: 0x00050AC5 File Offset: 0x0004ECC5
	private static string customCampaignsDir
	{
		get
		{
			return Path.Combine(GameProgressSaver.customSavesDir, "Campaigns");
		}
	}

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x06000B49 RID: 2889 RVA: 0x00050AD6 File Offset: 0x0004ECD6
	private static string generalProgressPath
	{
		get
		{
			return Path.Combine(GameProgressSaver.SavePath, "generalprogress.bepis");
		}
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06000B4A RID: 2890 RVA: 0x00050AE7 File Offset: 0x0004ECE7
	private static string cyberGrindHighScorePath
	{
		get
		{
			return Path.Combine(GameProgressSaver.SavePath, "cybergrindhighscore.bepis");
		}
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x06000B4B RID: 2891 RVA: 0x00050AF8 File Offset: 0x0004ECF8
	private static string currentCustomLevelProgressPath
	{
		get
		{
			return GameProgressSaver.CustomLevelProgressPath(GameStateManager.Instance.currentCustomGame.uniqueIdentifier);
		}
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x00050B0E File Offset: 0x0004ED0E
	private static string CustomLevelProgressPath(string uuid)
	{
		return Path.Combine(GameProgressSaver.customSavesDir, uuid + ".bepis");
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x06000B4D RID: 2893 RVA: 0x00050B25 File Offset: 0x0004ED25
	public static string customMapsPath
	{
		get
		{
			return Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Mods", "Maps");
		}
	}

	// Token: 0x06000B4E RID: 2894 RVA: 0x00050B45 File Offset: 0x0004ED45
	private static string LevelProgressPath(int lvl)
	{
		return Path.Combine(GameProgressSaver.SavePath, string.Format("lvl{0}progress.bepis", lvl));
	}

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x06000B4F RID: 2895 RVA: 0x00050B61 File Offset: 0x0004ED61
	private static string resolveCurrentLevelPath
	{
		get
		{
			if (!SceneHelper.IsPlayingCustom)
			{
				return GameProgressSaver.LevelProgressPath(MonoSingleton<StatsManager>.Instance.levelNumber);
			}
			return GameProgressSaver.currentCustomLevelProgressPath;
		}
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x00050B7F File Offset: 0x0004ED7F
	public static void SetSlot(int slot)
	{
		GameProgressSaver.currentSlot = slot;
		GameProgressSaver.lastTotalSecrets = -1;
		MonoSingleton<PrefsManager>.Instance.SetInt("selectedSaveSlot", slot);
		MonoSingleton<BestiaryData>.Instance.CheckSave();
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x00050BA8 File Offset: 0x0004EDA8
	public static void CreateSaveDirs(bool forceCustom = false)
	{
		if (!Directory.Exists(GameProgressSaver.SavePath))
		{
			Directory.CreateDirectory(GameProgressSaver.SavePath);
		}
		if (SceneHelper.IsPlayingCustom || forceCustom)
		{
			if (!Directory.Exists(GameProgressSaver.customSavesDir))
			{
				Directory.CreateDirectory(GameProgressSaver.customSavesDir);
			}
			if (!Directory.Exists(GameProgressSaver.customCampaignsDir))
			{
				Directory.CreateDirectory(GameProgressSaver.customCampaignsDir);
			}
			if (!Directory.Exists(GameProgressSaver.customMapsPath))
			{
				Directory.CreateDirectory(GameProgressSaver.customMapsPath);
			}
		}
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x00050C1C File Offset: 0x0004EE1C
	public static void WipeSlot(int slot)
	{
		int num = GameProgressSaver.currentSlot;
		GameProgressSaver.currentSlot = slot;
		try
		{
			if (Directory.Exists(GameProgressSaver.SavePath))
			{
				string[] files = Directory.GetFiles(GameProgressSaver.SavePath, "*.bepis", SearchOption.TopDirectoryOnly);
				for (int i = 0; i < files.Length; i++)
				{
					File.Delete(files[i]);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
		}
		GameProgressSaver.currentSlot = num;
	}

	// Token: 0x06000B53 RID: 2899 RVA: 0x00050C88 File Offset: 0x0004EE88
	private static SaveSlotMenu.SlotData GetDirectorySlotData(string path)
	{
		Debug.Log("Generating SlotData for " + path);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < 6; i++)
		{
			GameProgressData gameProgressData = GameProgressSaver.ReadFile(Path.Combine(path, string.Format("difficulty{0}progress.bepis", i))) as GameProgressData;
			if (gameProgressData != null && (gameProgressData.levelNum > num || (gameProgressData.levelNum == num && gameProgressData.difficulty > num2)))
			{
				num = gameProgressData.levelNum;
				num2 = gameProgressData.difficulty;
			}
		}
		return new SaveSlotMenu.SlotData
		{
			exists = true,
			highestDifficulty = num2,
			highestLvlNumber = num
		};
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x00050D1C File Offset: 0x0004EF1C
	public static SaveSlotMenu.SlotData[] GetSlots()
	{
		int num = GameProgressSaver.currentSlot;
		List<SaveSlotMenu.SlotData> list = new List<SaveSlotMenu.SlotData>();
		for (int i = 0; i < 5; i++)
		{
			GameProgressSaver.currentSlot = i;
			if (!Directory.Exists(GameProgressSaver.SavePath))
			{
				list.Add(new SaveSlotMenu.SlotData
				{
					exists = false
				});
			}
			else
			{
				try
				{
					if (!(GameProgressSaver.ReadFile(GameProgressSaver.generalProgressPath) is GameProgressMoneyAndGear))
					{
						list.Add(new SaveSlotMenu.SlotData
						{
							exists = false
						});
						goto IL_0077;
					}
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
					goto IL_0077;
				}
				SaveSlotMenu.SlotData directorySlotData = GameProgressSaver.GetDirectorySlotData(GameProgressSaver.SavePath);
				list.Add(directorySlotData);
			}
			IL_0077:;
		}
		GameProgressSaver.currentSlot = num;
		return list.ToArray();
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x00050DC4 File Offset: 0x0004EFC4
	private static void PrepareFs()
	{
		GameProgressSaver.CreateSaveDirs(false);
		if (GameProgressSaver.initialized)
		{
			return;
		}
		GameProgressSaver.initialized = true;
		if ((from a in Directory.GetFiles(GameProgressSaver.BaseSavePath, "*.bepis", SearchOption.TopDirectoryOnly)
			where !GameProgressSaver.SlotIgnoreFiles.Contains(Path.GetFileNameWithoutExtension(a))
			select a).ToArray<string>().Length != 0)
		{
			Debug.Log("Old saves found");
			int num = GameProgressSaver.currentSlot;
			GameProgressSaver.currentSlot = 0;
			try
			{
				GameProgressSaver.CreateSaveDirs(false);
				if (Directory.GetFiles(GameProgressSaver.SavePath, "*.bepis").Length != 0)
				{
					Debug.Log("Slot 1 is populated while old saves also exist. Showing consent screen.");
					SaveSlotMenu.SlotData directorySlotData = GameProgressSaver.GetDirectorySlotData(GameProgressSaver.BaseSavePath);
					SaveSlotMenu.SlotData directorySlotData2 = GameProgressSaver.GetDirectorySlotData(GameProgressSaver.SavePath);
					SaveLoadFailMessage.DisplayMergeConsent(directorySlotData, directorySlotData2);
				}
				else
				{
					GameProgressSaver.MergeRootWithSlotOne(true);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			GameProgressSaver.currentSlot = num;
		}
		if (Directory.Exists(Path.Combine(GameProgressSaver.BaseSavePath, "Custom")))
		{
			string[] files = Directory.GetFiles(Path.Combine(GameProgressSaver.BaseSavePath, "Custom"), "*.bepis", SearchOption.TopDirectoryOnly);
			if (files.Length != 0)
			{
				int num2 = GameProgressSaver.currentSlot;
				GameProgressSaver.currentSlot = 0;
				try
				{
					GameProgressSaver.CreateSaveDirs(true);
					foreach (string text in files)
					{
						GameProgressSaver.SafeMove(text, Path.Combine(GameProgressSaver.customSavesDir, Path.GetFileName(text)));
					}
				}
				catch (Exception ex2)
				{
					Debug.LogException(ex2);
				}
				GameProgressSaver.currentSlot = num2;
			}
		}
		GameProgressSaver.currentSlot = MonoSingleton<PrefsManager>.Instance.GetInt("selectedSaveSlot", 0);
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x00050F48 File Offset: 0x0004F148
	public static void MergeRootWithSlotOne(bool keepRoot)
	{
		foreach (string text in (from a in Directory.GetFiles(GameProgressSaver.BaseSavePath, "*.bepis", SearchOption.TopDirectoryOnly)
			where !GameProgressSaver.SlotIgnoreFiles.Contains(Path.GetFileNameWithoutExtension(a))
			select a).ToArray<string>())
		{
			Debug.Log(text);
			if (!keepRoot)
			{
				File.Delete(text);
			}
			else
			{
				GameProgressSaver.SafeMove(text, Path.Combine(GameProgressSaver.SavePath, Path.GetFileName(text)));
			}
		}
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x00050FC8 File Offset: 0x0004F1C8
	private static void SafeMove(string source, string target)
	{
		if (File.Exists(target))
		{
			File.Delete(target);
		}
		File.Move(source, target);
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x00050FE0 File Offset: 0x0004F1E0
	private static object ReadFile(string path)
	{
		GameProgressSaver.PrepareFs();
		if (!File.Exists(path))
		{
			return null;
		}
		object obj;
		using (FileStream fileStream = new FileStream(path, FileMode.Open))
		{
			if (fileStream.Length == 0L)
			{
				throw new Exception("Stream Length 0");
			}
			obj = new BinaryFormatter
			{
				Binder = new RestrictedSerializationBinder
				{
					AllowedTypes = 
					{
						typeof(RankData),
						typeof(CyberRankData),
						typeof(RankScoreData),
						typeof(GameProgressData),
						typeof(GameProgressMoneyAndGear)
					}
				}
			}.Deserialize(fileStream);
		}
		return obj;
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x000510BC File Offset: 0x0004F2BC
	private static void WriteFile(string path, object data)
	{
		GameProgressSaver.PrepareFs();
		Debug.Log("[FS] Writing To " + path);
		Directory.CreateDirectory(Path.GetDirectoryName(path));
		using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			try
			{
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x00051138 File Offset: 0x0004F338
	private static RankData GetRankData(bool returnNull, int lvl = -1)
	{
		string text;
		return GameProgressSaver.GetRankData(out text, lvl, returnNull);
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x00051150 File Offset: 0x0004F350
	private static RankData GetRankData(int lvl = -1)
	{
		string text;
		return GameProgressSaver.GetRankData(out text, lvl, false);
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x00051168 File Offset: 0x0004F368
	private static RankData GetRankData(out string path, int lvl = -1, bool returnNull = false)
	{
		GameProgressSaver.PrepareFs();
		path = ((lvl < 0) ? GameProgressSaver.resolveCurrentLevelPath : GameProgressSaver.LevelProgressPath(lvl));
		RankData rankData = GameProgressSaver.ReadFile(path) as RankData;
		if (rankData == null)
		{
			rankData = (returnNull ? null : new RankData(MonoSingleton<StatsManager>.Instance));
		}
		return rankData;
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x000511B0 File Offset: 0x0004F3B0
	public static RankData GetCustomRankData(string uuid)
	{
		string text = GameProgressSaver.CustomLevelProgressPath(uuid);
		Debug.Log(text);
		RankData rankData = GameProgressSaver.ReadFile(text) as RankData;
		if (rankData == null)
		{
			return null;
		}
		return rankData;
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x000511DC File Offset: 0x0004F3DC
	private static GameProgressData GetGameProgress(int difficulty = -1)
	{
		string text;
		return GameProgressSaver.GetGameProgress(out text, difficulty);
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x000511F4 File Offset: 0x0004F3F4
	private static GameProgressData GetGameProgress(out string path, int difficulty = -1)
	{
		path = ((difficulty < 0) ? GameProgressSaver.currentDifficultyPath : GameProgressSaver.DifficultySavePath(difficulty));
		GameProgressData gameProgressData = GameProgressSaver.ReadFile(path) as GameProgressData;
		if (gameProgressData == null)
		{
			gameProgressData = new GameProgressData();
		}
		if (gameProgressData.primeLevels == null || gameProgressData.primeLevels.Length == 0)
		{
			gameProgressData.primeLevels = new int[3];
		}
		return gameProgressData;
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x00051248 File Offset: 0x0004F448
	public static void ChallengeComplete()
	{
		RankData rankData = GameProgressSaver.GetRankData(-1);
		if (!rankData.challenge && (rankData.levelNumber == MonoSingleton<StatsManager>.Instance.levelNumber || SceneHelper.IsPlayingCustom))
		{
			rankData.challenge = true;
			GameProgressSaver.WriteFile(GameProgressSaver.resolveCurrentLevelPath, rankData);
		}
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x00051290 File Offset: 0x0004F490
	public static void SaveProgress(int levelNum)
	{
		Debug.Log(string.Format("[FS] Saving Progress for Level {0}", levelNum));
		string text;
		GameProgressData gameProgress = GameProgressSaver.GetGameProgress(out text, -1);
		if (gameProgress.levelNum < levelNum || gameProgress.difficulty != MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0))
		{
			gameProgress.levelNum = levelNum;
			GameProgressSaver.WriteFile(text, gameProgress);
		}
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x000512EA File Offset: 0x0004F4EA
	public static void SaveRank()
	{
		GameProgressSaver.WriteFile(GameProgressSaver.resolveCurrentLevelPath, new RankData(MonoSingleton<StatsManager>.Instance));
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x00051300 File Offset: 0x0004F500
	public static RankData GetRank(bool returnNull, int lvl = -1)
	{
		return GameProgressSaver.GetRankData(returnNull, lvl);
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x0005130C File Offset: 0x0004F50C
	public static void SecretFound(int secretNum)
	{
		GameProgressSaver.lastTotalSecrets = -1;
		string text;
		RankData rankData = GameProgressSaver.GetRankData(out text, -1, false);
		if (rankData.levelNumber != MonoSingleton<StatsManager>.Instance.levelNumber && !SceneHelper.IsPlayingCustom)
		{
			return;
		}
		if (rankData.secretsFound.Length <= secretNum)
		{
			bool[] array = new bool[MonoSingleton<StatsManager>.Instance.secretObjects.Length];
			for (int i = 0; i < rankData.secretsFound.Length; i++)
			{
				array[i] = rankData.secretsFound[i];
			}
			rankData.secretsFound = array;
		}
		else if (rankData.secretsFound[secretNum])
		{
			return;
		}
		rankData.secretsFound[secretNum] = true;
		GameProgressSaver.WriteFile(text, rankData);
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x000513A4 File Offset: 0x0004F5A4
	public static int GetProgress(int difficulty)
	{
		int num = 1;
		for (int i = difficulty; i <= 5; i++)
		{
			GameProgressData gameProgress = GameProgressSaver.GetGameProgress(i);
			if (gameProgress != null && gameProgress.difficulty == i && gameProgress.levelNum > num)
			{
				num = gameProgress.levelNum;
			}
		}
		return num;
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x000513E4 File Offset: 0x0004F5E4
	public static RankData GetRank(int levelNumber, bool returnNull = false)
	{
		string text;
		return GameProgressSaver.GetRankData(out text, levelNumber, returnNull);
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x000513FC File Offset: 0x0004F5FC
	public static void SetPrime(int level, int state)
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return;
		}
		level--;
		string text;
		GameProgressData gameProgress = GameProgressSaver.GetGameProgress(out text, -1);
		if (level < gameProgress.primeLevels.Length)
		{
			if (state <= gameProgress.primeLevels[level])
			{
				return;
			}
			gameProgress.primeLevels[level] = state;
		}
		else
		{
			int[] array = new int[3];
			for (int i = 0; i < gameProgress.primeLevels.Length; i++)
			{
				array[i] = gameProgress.primeLevels[i];
			}
			for (int j = gameProgress.primeLevels.Length; j < array.Length; j++)
			{
				if (j == level)
				{
					array[j] = state;
				}
				else
				{
					array[j] = 0;
				}
			}
			gameProgress.primeLevels = array;
		}
		GameProgressSaver.WriteFile(text, gameProgress);
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x000514A0 File Offset: 0x0004F6A0
	public static int GetPrime(int difficulty, int level)
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return 0;
		}
		level--;
		int num = 0;
		for (int i = difficulty; i <= 5; i++)
		{
			GameProgressData gameProgress = GameProgressSaver.GetGameProgress(i);
			if (gameProgress != null && gameProgress.difficulty == i && gameProgress.primeLevels != null && gameProgress.primeLevels.Length > level && gameProgress.primeLevels[level] > num)
			{
				Debug.Log("Highest: . Data: " + gameProgress.primeLevels[level].ToString());
				if (gameProgress.primeLevels[level] >= 2)
				{
					return 2;
				}
				num = gameProgress.primeLevels[level];
			}
		}
		return num;
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x00051534 File Offset: 0x0004F734
	public static void SetEncoreProgress(int levelNum)
	{
		Debug.Log(string.Format("[FS] Saving Encore Progress for Level {0}", levelNum));
		string text;
		GameProgressData gameProgress = GameProgressSaver.GetGameProgress(out text, -1);
		if (gameProgress.encores < levelNum || gameProgress.difficulty != MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0))
		{
			gameProgress.encores = levelNum;
			GameProgressSaver.WriteFile(text, gameProgress);
		}
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x00051590 File Offset: 0x0004F790
	public static int GetEncoreProgress(int difficulty)
	{
		int num = 0;
		for (int i = difficulty; i <= 5; i++)
		{
			GameProgressData gameProgress = GameProgressSaver.GetGameProgress(i);
			if (gameProgress != null && gameProgress.difficulty == i && gameProgress.encores > num)
			{
				num = gameProgress.encores;
			}
		}
		return num;
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x000515D0 File Offset: 0x0004F7D0
	public static GameProgressMoneyAndGear GetGeneralProgress()
	{
		GameProgressSaver.PrepareFs();
		GameProgressMoneyAndGear gameProgressMoneyAndGear = GameProgressSaver.ReadFile(GameProgressSaver.generalProgressPath) as GameProgressMoneyAndGear;
		if (gameProgressMoneyAndGear == null)
		{
			gameProgressMoneyAndGear = new GameProgressMoneyAndGear();
		}
		if (gameProgressMoneyAndGear.secretMissions == null || gameProgressMoneyAndGear.secretMissions.Length == 0)
		{
			gameProgressMoneyAndGear.secretMissions = new int[10];
		}
		if (gameProgressMoneyAndGear.limboSwitches == null || gameProgressMoneyAndGear.limboSwitches.Length == 0)
		{
			gameProgressMoneyAndGear.limboSwitches = new bool[4];
		}
		if (gameProgressMoneyAndGear.shotgunSwitches == null || gameProgressMoneyAndGear.shotgunSwitches.Length == 0)
		{
			gameProgressMoneyAndGear.shotgunSwitches = new bool[3];
		}
		if (gameProgressMoneyAndGear.newEnemiesFound == null)
		{
			gameProgressMoneyAndGear.newEnemiesFound = new int[Enum.GetValues(typeof(EnemyType)).Length];
		}
		if (gameProgressMoneyAndGear.unlockablesFound == null)
		{
			gameProgressMoneyAndGear.unlockablesFound = new bool[Enum.GetValues(typeof(UnlockableType)).Length];
		}
		return gameProgressMoneyAndGear;
	}

	// Token: 0x06000B6C RID: 2924 RVA: 0x000516A4 File Offset: 0x0004F8A4
	public static void AddGear(string gear)
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return;
		}
		GameProgressMoneyAndGear generalProgress = GameProgressSaver.GetGeneralProgress();
		FieldInfo field = typeof(GameProgressMoneyAndGear).GetField(gear, BindingFlags.Instance | BindingFlags.Public);
		if (field == null)
		{
			return;
		}
		field.SetValue(generalProgress, 1);
		GameProgressSaver.WriteFile(GameProgressSaver.generalProgressPath, generalProgress);
	}

	// Token: 0x06000B6D RID: 2925 RVA: 0x000516F4 File Offset: 0x0004F8F4
	public static int CheckGear(string gear)
	{
		GameProgressMoneyAndGear generalProgress = GameProgressSaver.GetGeneralProgress();
		FieldInfo field = typeof(GameProgressMoneyAndGear).GetField(gear, BindingFlags.Instance | BindingFlags.Public);
		if (field == null)
		{
			return 0;
		}
		object value = field.GetValue(generalProgress);
		if (value == null)
		{
			return 0;
		}
		return (int)value;
	}

	// Token: 0x06000B6E RID: 2926 RVA: 0x00051738 File Offset: 0x0004F938
	public static void AddMoney(int money)
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return;
		}
		GameProgressMoneyAndGear generalProgress = GameProgressSaver.GetGeneralProgress();
		if (generalProgress.money + money >= 0)
		{
			generalProgress.money += money;
		}
		else
		{
			generalProgress.money = 0;
		}
		GameProgressSaver.WriteFile(GameProgressSaver.generalProgressPath, generalProgress);
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x00051780 File Offset: 0x0004F980
	public static void UnlockWeaponCustomization(GameProgressSaver.WeaponCustomizationType weapon)
	{
		GameProgressMoneyAndGear generalProgress = GameProgressSaver.GetGeneralProgress();
		switch (weapon)
		{
		case GameProgressSaver.WeaponCustomizationType.Revolver:
			generalProgress.revCustomizationUnlocked = true;
			break;
		case GameProgressSaver.WeaponCustomizationType.Shotgun:
			generalProgress.shoCustomizationUnlocked = true;
			break;
		case GameProgressSaver.WeaponCustomizationType.Nailgun:
			generalProgress.naiCustomizationUnlocked = true;
			break;
		case GameProgressSaver.WeaponCustomizationType.Railcannon:
			generalProgress.raiCustomizationUnlocked = true;
			break;
		case GameProgressSaver.WeaponCustomizationType.RocketLauncher:
			generalProgress.rockCustomizationUnlocked = true;
			break;
		}
		GameProgressSaver.WriteFile(GameProgressSaver.generalProgressPath, generalProgress);
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x000517E8 File Offset: 0x0004F9E8
	public static bool HasWeaponCustomization(GameProgressSaver.WeaponCustomizationType weapon)
	{
		GameProgressMoneyAndGear generalProgress = GameProgressSaver.GetGeneralProgress();
		switch (weapon)
		{
		case GameProgressSaver.WeaponCustomizationType.Revolver:
			return generalProgress.revCustomizationUnlocked;
		case GameProgressSaver.WeaponCustomizationType.Shotgun:
			return generalProgress.shoCustomizationUnlocked;
		case GameProgressSaver.WeaponCustomizationType.Nailgun:
			return generalProgress.naiCustomizationUnlocked;
		case GameProgressSaver.WeaponCustomizationType.Railcannon:
			return generalProgress.raiCustomizationUnlocked;
		case GameProgressSaver.WeaponCustomizationType.RocketLauncher:
			return generalProgress.rockCustomizationUnlocked;
		default:
			return false;
		}
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x0005183B File Offset: 0x0004FA3B
	public static int GetMoney()
	{
		return GameProgressSaver.GetGeneralProgress().money;
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x00051848 File Offset: 0x0004FA48
	public static void SetTutorial(bool beat)
	{
		GameProgressMoneyAndGear generalProgress = GameProgressSaver.GetGeneralProgress();
		generalProgress.tutorialBeat = beat;
		GameProgressSaver.WriteFile(GameProgressSaver.generalProgressPath, generalProgress);
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x0005186D File Offset: 0x0004FA6D
	public static bool GetTutorial()
	{
		return SceneHelper.IsPlayingCustom || GameProgressSaver.GetGeneralProgress().tutorialBeat;
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x00051884 File Offset: 0x0004FA84
	public static void SetIntro(bool seen)
	{
		GameProgressMoneyAndGear generalProgress = GameProgressSaver.GetGeneralProgress();
		generalProgress.introSeen = seen;
		GameProgressSaver.WriteFile(GameProgressSaver.generalProgressPath, generalProgress);
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x000518A9 File Offset: 0x0004FAA9
	public static bool GetIntro()
	{
		return SceneHelper.IsPlayingCustom || GameProgressSaver.GetGeneralProgress().introSeen;
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x000518C0 File Offset: 0x0004FAC0
	public static void SetClashModeUnlocked(bool unlocked)
	{
		GameProgressMoneyAndGear generalProgress = GameProgressSaver.GetGeneralProgress();
		generalProgress.clashModeUnlocked = unlocked;
		GameProgressSaver.WriteFile(GameProgressSaver.generalProgressPath, generalProgress);
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x000518E5 File Offset: 0x0004FAE5
	public static bool GetClashModeUnlocked()
	{
		return SceneHelper.IsPlayingCustom || GameProgressSaver.GetGeneralProgress().clashModeUnlocked;
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x000518FC File Offset: 0x0004FAFC
	public static void SetGhostDroneModeUnlocked(bool unlocked)
	{
		GameProgressMoneyAndGear generalProgress = GameProgressSaver.GetGeneralProgress();
		generalProgress.ghostDroneModeUnlocked = unlocked;
		GameProgressSaver.WriteFile(GameProgressSaver.generalProgressPath, generalProgress);
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x00051921 File Offset: 0x0004FB21
	public static bool GetGhostDroneModeUnlocked()
	{
		return SceneHelper.IsPlayingCustom || GameProgressSaver.GetGeneralProgress().ghostDroneModeUnlocked;
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x00051938 File Offset: 0x0004FB38
	public static void SetUnlockable(UnlockableType unlockable, bool state)
	{
		GameProgressMoneyAndGear generalProgress = GameProgressSaver.GetGeneralProgress();
		bool[] array = new bool[Enum.GetValues(typeof(UnlockableType)).Length];
		for (int i = 0; i < generalProgress.unlockablesFound.Length; i++)
		{
			array[i] = generalProgress.unlockablesFound[i];
		}
		for (int j = generalProgress.newEnemiesFound.Length; j < array.Length; j++)
		{
			array[j] = false;
		}
		array[(int)unlockable] = state;
		generalProgress.unlockablesFound = array;
		GameProgressSaver.WriteFile(GameProgressSaver.generalProgressPath, generalProgress);
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x000519BC File Offset: 0x0004FBBC
	public static UnlockableType[] GetUnlockables()
	{
		GameProgressMoneyAndGear generalProgress = GameProgressSaver.GetGeneralProgress();
		List<UnlockableType> list = new List<UnlockableType>();
		for (int i = 0; i < generalProgress.unlockablesFound.Length; i++)
		{
			if (generalProgress.unlockablesFound[i])
			{
				list.Add((UnlockableType)i);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x00051A00 File Offset: 0x0004FC00
	public static void SetBestiary(EnemyType enemy, int state)
	{
		GameProgressMoneyAndGear generalProgress = GameProgressSaver.GetGeneralProgress();
		int[] array = new int[Enum.GetValues(typeof(EnemyType)).Length];
		for (int i = 0; i < generalProgress.newEnemiesFound.Length; i++)
		{
			array[i] = generalProgress.newEnemiesFound[i];
		}
		for (int j = generalProgress.newEnemiesFound.Length; j < array.Length; j++)
		{
			array[j] = 0;
		}
		array[(int)enemy] = state;
		generalProgress.newEnemiesFound = array;
		GameProgressSaver.WriteFile(GameProgressSaver.generalProgressPath, generalProgress);
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x00051A81 File Offset: 0x0004FC81
	public static int[] GetBestiary()
	{
		return GameProgressSaver.GetGeneralProgress().newEnemiesFound;
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x00051A90 File Offset: 0x0004FC90
	public static void SetLimboSwitch(int switchNum)
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return;
		}
		GameProgressMoneyAndGear generalProgress = GameProgressSaver.GetGeneralProgress();
		if (switchNum < generalProgress.limboSwitches.Length)
		{
			generalProgress.limboSwitches[switchNum] = true;
		}
		GameProgressSaver.WriteFile(GameProgressSaver.generalProgressPath, generalProgress);
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x00051ACA File Offset: 0x0004FCCA
	public static bool GetLimboSwitch(int switchNum)
	{
		return GameProgressSaver.GetGeneralProgress().limboSwitches[switchNum];
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x00051AD8 File Offset: 0x0004FCD8
	public static void SetShotgunSwitch(int switchNum)
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return;
		}
		GameProgressMoneyAndGear generalProgress = GameProgressSaver.GetGeneralProgress();
		if (switchNum < generalProgress.shotgunSwitches.Length)
		{
			generalProgress.shotgunSwitches[switchNum] = true;
		}
		GameProgressSaver.WriteFile(GameProgressSaver.generalProgressPath, generalProgress);
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x00051B12 File Offset: 0x0004FD12
	public static bool GetShotgunSwitch(int switchNum)
	{
		return GameProgressSaver.GetGeneralProgress().shotgunSwitches[switchNum];
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x00051B20 File Offset: 0x0004FD20
	public static int GetSecretMission(int missionNumber)
	{
		GameProgressMoneyAndGear generalProgress = GameProgressSaver.GetGeneralProgress();
		if (generalProgress.secretMissions.Length > missionNumber)
		{
			return generalProgress.secretMissions[missionNumber];
		}
		return 0;
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x00051B48 File Offset: 0x0004FD48
	public static void FoundSecretMission(int missionNumber)
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return;
		}
		GameProgressMoneyAndGear generalProgress = GameProgressSaver.GetGeneralProgress();
		if (generalProgress.secretMissions[missionNumber] != 2)
		{
			generalProgress.secretMissions[missionNumber] = 1;
		}
		GameProgressSaver.WriteFile(GameProgressSaver.generalProgressPath, generalProgress);
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x00051B84 File Offset: 0x0004FD84
	public static void SetSecretMission(int missionNumber)
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return;
		}
		GameProgressMoneyAndGear generalProgress = GameProgressSaver.GetGeneralProgress();
		generalProgress.secretMissions[missionNumber] = 2;
		GameProgressSaver.WriteFile(GameProgressSaver.generalProgressPath, generalProgress);
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x00051BB4 File Offset: 0x0004FDB4
	public static int GetTotalSecretsFound()
	{
		if (GameProgressSaver.lastTotalSecrets != -1)
		{
			return GameProgressSaver.lastTotalSecrets;
		}
		FileInfo[] files = new DirectoryInfo(GameProgressSaver.SavePath).GetFiles("lvl*progress.bepis");
		int num = 0;
		FileInfo[] array = files;
		for (int i = 0; i < array.Length; i++)
		{
			RankData rankData = GameProgressSaver.ReadFile(array[i].FullName) as RankData;
			if (rankData != null)
			{
				num += rankData.secretsFound.Count((bool a) => a);
			}
		}
		GameProgressSaver.lastTotalSecrets = num;
		return num;
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x00051C40 File Offset: 0x0004FE40
	private static CyberRankData GetCyberRankData()
	{
		CyberRankData cyberRankData = GameProgressSaver.ReadFile(GameProgressSaver.cyberGrindHighScorePath) as CyberRankData;
		if (cyberRankData == null)
		{
			cyberRankData = new CyberRankData();
		}
		if (cyberRankData.preciseWavesByDifficulty == null || cyberRankData.preciseWavesByDifficulty.Length != 6)
		{
			cyberRankData.preciseWavesByDifficulty = new float[6];
		}
		if (cyberRankData.style == null || cyberRankData.style.Length != 6)
		{
			cyberRankData.style = new int[6];
		}
		if (cyberRankData.kills == null || cyberRankData.kills.Length != 6)
		{
			cyberRankData.kills = new int[6];
		}
		if (cyberRankData.time == null || cyberRankData.time.Length != 6)
		{
			cyberRankData.time = new float[6];
		}
		return cyberRankData;
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x00051CE3 File Offset: 0x0004FEE3
	public static CyberRankData GetBestCyber()
	{
		return GameProgressSaver.GetCyberRankData();
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x00051CEC File Offset: 0x0004FEEC
	public static void SetBestCyber(FinalCyberRank fcr)
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return;
		}
		CyberRankData cyberRankData = GameProgressSaver.GetCyberRankData();
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		cyberRankData.preciseWavesByDifficulty[@int] = fcr.savedWaves;
		cyberRankData.kills[@int] = fcr.savedKills;
		cyberRankData.style[@int] = fcr.savedStyle;
		cyberRankData.time[@int] = fcr.savedTime;
		GameProgressSaver.WriteFile(GameProgressSaver.cyberGrindHighScorePath, cyberRankData);
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x00051D5B File Offset: 0x0004FF5B
	public static void ResetBestCyber()
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return;
		}
		if (File.Exists(GameProgressSaver.cyberGrindHighScorePath))
		{
			File.Delete(GameProgressSaver.cyberGrindHighScorePath);
		}
	}

	// Token: 0x04000F11 RID: 3857
	public static int currentSlot = 0;

	// Token: 0x04000F12 RID: 3858
	private static int lastTotalSecrets = -1;

	// Token: 0x04000F13 RID: 3859
	private static bool initialized;

	// Token: 0x04000F14 RID: 3860
	private static readonly string[] SlotIgnoreFiles = new string[] { "prefs" };

	// Token: 0x02000219 RID: 537
	public enum WeaponCustomizationType
	{
		// Token: 0x04000F16 RID: 3862
		Revolver,
		// Token: 0x04000F17 RID: 3863
		Shotgun,
		// Token: 0x04000F18 RID: 3864
		Nailgun,
		// Token: 0x04000F19 RID: 3865
		Railcannon,
		// Token: 0x04000F1A RID: 3866
		RocketLauncher
	}
}
