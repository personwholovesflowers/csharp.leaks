using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

// Token: 0x02000208 RID: 520
[Serializable]
public class GameBuildSettings
{
	// Token: 0x06000B00 RID: 2816 RVA: 0x0004F90C File Offset: 0x0004DB0C
	public static GameBuildSettings GetInstance()
	{
		if (GameBuildSettings._instance == null)
		{
			string text = Path.Combine(Application.streamingAssetsPath, "GameBuildSettings.json");
			if (File.Exists(text))
			{
				try
				{
					GameBuildSettings._instance = JsonConvert.DeserializeObject<GameBuildSettings>(File.ReadAllText(text));
					goto IL_0058;
				}
				catch (Exception ex)
				{
					Debug.LogError(string.Format("Failed to load GameBuildSettings: {0}", ex));
					GameBuildSettings._instance = GameBuildSettings.Default;
					goto IL_0058;
				}
			}
			GameBuildSettings._instance = GameBuildSettings.Default;
		}
		IL_0058:
		return GameBuildSettings._instance;
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06000B01 RID: 2817 RVA: 0x0004F988 File Offset: 0x0004DB88
	public static GameBuildSettings Default
	{
		get
		{
			return new GameBuildSettings
			{
				startScene = null,
				noTutorial = false
			};
		}
	}

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x06000B02 RID: 2818 RVA: 0x0004F99D File Offset: 0x0004DB9D
	public static GameBuildSettings Agony
	{
		get
		{
			return new GameBuildSettings
			{
				startScene = "Main Menu",
				noTutorial = true
			};
		}
	}

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x06000B03 RID: 2819 RVA: 0x0004F9B6 File Offset: 0x0004DBB6
	public static GameBuildSettings SandboxOnly
	{
		get
		{
			return new GameBuildSettings
			{
				startScene = "uk_construct",
				noTutorial = true
			};
		}
	}

	// Token: 0x04000EAE RID: 3758
	public string startScene;

	// Token: 0x04000EAF RID: 3759
	public bool noTutorial;

	// Token: 0x04000EB0 RID: 3760
	private static GameBuildSettings _instance;
}
