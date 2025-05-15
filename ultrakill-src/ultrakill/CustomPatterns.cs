using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200013C RID: 316
public class CustomPatterns : MonoBehaviour
{
	// Token: 0x17000081 RID: 129
	// (get) Token: 0x0600061A RID: 1562 RVA: 0x0002A28B File Offset: 0x0002848B
	private string PatternsPath
	{
		get
		{
			return Path.Combine(Directory.GetParent(Application.dataPath).FullName, "CyberGrind", "Patterns");
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x0600061B RID: 1563 RVA: 0x0002A2AC File Offset: 0x000284AC
	private ArenaPattern[] AllEnabledPatterns
	{
		get
		{
			List<ArenaPattern> list = new List<ArenaPattern>();
			list.AddRange(this.enabledPatterns.Values.ToArray<ArenaPattern>());
			foreach (ArenaPattern[] array in this.enabledPatternPacks.Values)
			{
				list.AddRange(array);
			}
			return list.ToArray();
		}
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x0002A328 File Offset: 0x00028528
	private void Awake()
	{
		Directory.CreateDirectory(Path.GetDirectoryName(this.PatternsPath));
		Directory.CreateDirectory(this.PatternsPath);
		this.LoadEnabledPatterns();
		this.BuildButtons();
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x0002A354 File Offset: 0x00028554
	public void Toggle()
	{
		Debug.Log("Toggling custom patterns");
		bool customPatternMode = MonoSingleton<EndlessGrid>.Instance.customPatternMode;
		MonoSingleton<EndlessGrid>.Instance.customPatternMode = !customPatternMode;
		this.stateButtonText.text = (customPatternMode ? "Enable" : "Disable");
		GameObject gameObject = this.enableWhenCustom;
		if (gameObject != null)
		{
			gameObject.SetActive(!customPatternMode);
		}
		GameObject gameObject2 = this.disableWhenCustom;
		if (gameObject2 != null)
		{
			gameObject2.SetActive(customPatternMode);
		}
		MonoSingleton<PrefsManager>.Instance.SetBoolLocal("cyberGrind.customPool", MonoSingleton<EndlessGrid>.Instance.customPatternMode);
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x0002A3E0 File Offset: 0x000285E0
	public void SaveEnabledPatterns()
	{
		ActivePatterns activePatterns = new ActivePatterns
		{
			enabledPatterns = this.enabledPatterns.Keys.ToArray<string>(),
			enabledPatternPacks = this.enabledPatternPacks.Keys.ToArray<string>()
		};
		MonoSingleton<PrefsManager>.Instance.SetStringLocal("cyberGrind.enabledPatterns", JsonUtility.ToJson(activePatterns));
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x0002A434 File Offset: 0x00028634
	public void LoadEnabledPatterns()
	{
		string stringLocal = MonoSingleton<PrefsManager>.Instance.GetStringLocal("cyberGrind.enabledPatterns", null);
		if (!string.IsNullOrEmpty(stringLocal))
		{
			ActivePatterns activePatterns = JsonUtility.FromJson<ActivePatterns>(stringLocal);
			this.enabledPatterns = new Dictionary<string, ArenaPattern>();
			this.enabledPatternPacks = new Dictionary<string, ArenaPattern[]>();
			if (activePatterns.enabledPatterns != null)
			{
				foreach (string text in activePatterns.enabledPatterns)
				{
					string text2 = text;
					if (Path.GetFileName(text) != text)
					{
						text2 = Path.GetFileName(text);
					}
					ArenaPattern arenaPattern = this.LoadPattern(text2);
					if (arenaPattern)
					{
						this.enabledPatterns.Add(text2, arenaPattern);
					}
				}
			}
			if (activePatterns.enabledPatternPacks != null)
			{
				foreach (string text3 in activePatterns.enabledPatternPacks)
				{
					if (Directory.Exists(Path.Combine(this.PatternsPath, text3)))
					{
						string[] array2 = (from f in Directory.GetFiles(Path.Combine(this.PatternsPath, text3))
							select Path.GetFileName(f)).ToArray<string>();
						List<ArenaPattern> list = new List<ArenaPattern>();
						foreach (string text4 in array2)
						{
							ArenaPattern arenaPattern2 = this.LoadPattern(Path.Combine(text3, text4));
							list.Add(arenaPattern2);
						}
						this.enabledPatternPacks.Add(text3, list.ToArray());
					}
				}
			}
			MonoSingleton<EndlessGrid>.Instance.customPatterns = this.AllEnabledPatterns;
		}
		MonoSingleton<EndlessGrid>.Instance.customPatterns = this.AllEnabledPatterns;
		MonoSingleton<EndlessGrid>.Instance.customPatternMode = MonoSingleton<PrefsManager>.Instance.GetBoolLocal("cyberGrind.customPool", false);
		bool flag = !MonoSingleton<EndlessGrid>.Instance.customPatternMode;
		this.stateButtonText.text = (flag ? "Enable" : "Disable");
		this.enableWhenCustom.SetActive(!flag);
		GameObject gameObject = this.disableWhenCustom;
		if (gameObject == null)
		{
			return;
		}
		gameObject.SetActive(flag);
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x0002A638 File Offset: 0x00028838
	public void BuildButtons()
	{
		GridTile[] array = (from f in Directory.GetDirectories(this.PatternsPath, "*", SearchOption.TopDirectoryOnly)
			select new GridTile
			{
				path = Path.GetFileName(f),
				folder = true
			}).ToArray<GridTile>();
		GridTile[] array2 = (from f in Directory.GetFiles(this.PatternsPath, "*.cgp", SearchOption.TopDirectoryOnly)
			select new GridTile
			{
				path = Path.GetFileName(f),
				folder = false
			}).ToArray<GridTile>();
		List<GridTile> list = new List<GridTile>();
		list.AddRange(array);
		list.AddRange(array2);
		for (int i = 2; i < this.grid.childCount; i++)
		{
			Object.Destroy(this.grid.GetChild(i).gameObject);
		}
		this.maxPages = Mathf.CeilToInt((float)list.Count / (float)this.maxItemsPerPage);
		int num = (this.currentPage - 1) * this.maxItemsPerPage;
		while (num < list.Count && num < this.currentPage * this.maxItemsPerPage)
		{
			GridTile tile = list[num];
			if (tile.folder)
			{
				string[] array3 = (from f in Directory.GetFiles(Path.Combine(this.PatternsPath, tile.path), "*.cgp")
					select Path.GetFileName(f)).ToArray<string>();
				Texture2D texture2D = new Texture2D(48, 48);
				for (int j = 0; j < 48; j++)
				{
					for (int k = 0; k < 48; k++)
					{
						texture2D.SetPixel(j, k, Color.black);
					}
				}
				int num2 = 0;
				while (num2 < array3.Length && num2 < 7)
				{
					ArenaPattern arenaPattern = this.LoadPattern(Path.Combine(tile.path, array3[num2]));
					Vector2Int vector2Int = new Vector2Int(num2 % 3, (num2 > 2) ? 1 : 0);
					this.GeneratePatternPreview(arenaPattern, vector2Int, ref texture2D);
					num2++;
				}
				texture2D.Apply();
				GameObject gameObject = Object.Instantiate<GameObject>(this.packButtonTemplate, this.grid, false);
				Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, 48f, 48f), new Vector2(0.5f, 0.5f), 100f);
				sprite.texture.filterMode = FilterMode.Point;
				gameObject.GetComponentInChildren<TMP_Text>(true).text = tile.path;
				gameObject.GetComponent<Image>().sprite = sprite;
				gameObject.GetComponent<ControllerPointer>().OnPressed.AddListener(delegate
				{
					this.TogglePattern(tile.path, true);
				});
				this.patternPackActiveIndicators[tile.path] = gameObject.transform.GetChild(0).gameObject;
				this.patternPackActiveIndicators[tile.path].SetActive(this.enabledPatternPacks.ContainsKey(tile.path));
				gameObject.SetActive(true);
			}
			else
			{
				string path = list[num].path;
				ArenaPattern arenaPattern2 = this.LoadPattern(path);
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.buttonTemplate, this.grid, false);
				if (arenaPattern2 == null)
				{
					Sprite sprite2 = Sprite.Create(this.parsingErrorTexture, new Rect(0f, 0f, 16f, 16f), new Vector2(0.5f, 0.5f), 100f);
					sprite2.texture.filterMode = FilterMode.Point;
					gameObject2.GetComponent<Image>().sprite = sprite2;
					gameObject2.transform.GetChild(0).gameObject.SetActive(false);
					gameObject2.SetActive(true);
				}
				else
				{
					Texture2D texture2D2 = new Texture2D(16, 16);
					bool flag = this.GeneratePatternPreview(arenaPattern2, Vector2Int.zero, ref texture2D2);
					texture2D2.Apply();
					if (!flag)
					{
						Sprite sprite3 = Sprite.Create(this.parsingErrorTexture, new Rect(0f, 0f, 16f, 16f), new Vector2(0.5f, 0.5f), 100f);
						sprite3.texture.filterMode = FilterMode.Point;
						gameObject2.GetComponent<Image>().sprite = sprite3;
						gameObject2.transform.GetChild(0).gameObject.SetActive(false);
						gameObject2.SetActive(true);
					}
					else
					{
						Sprite sprite4 = Sprite.Create(texture2D2, new Rect(0f, 0f, 16f, 16f), new Vector2(0.5f, 0.5f), 100f);
						sprite4.texture.filterMode = FilterMode.Point;
						gameObject2.GetComponent<Image>().sprite = sprite4;
						gameObject2.SetActive(true);
						string key = Path.GetFileName(path);
						gameObject2.GetComponent<ControllerPointer>().OnPressed.AddListener(delegate
						{
							this.TogglePattern(key, false);
						});
						this.patternActiveIndicators[key] = gameObject2.transform.GetChild(0).gameObject;
						this.patternActiveIndicators[key].SetActive(this.enabledPatterns.ContainsKey(key));
					}
				}
			}
			num++;
		}
		this.pageText.text = string.Format("{0}/{1}", this.currentPage, this.maxPages);
		this.buttonTemplate.SetActive(false);
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x0002ABD4 File Offset: 0x00028DD4
	private bool GeneratePatternPreview(ArenaPattern pattern, Vector2Int offset, ref Texture2D target)
	{
		string[] array = pattern.heights.Split('\n', StringSplitOptions.None);
		if (array.Length != 16)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"[Heights] Pattern \"",
				pattern.name,
				"\" has ",
				array.Length.ToString(),
				" rows instead of ",
				16.ToString()
			}));
			Debug.Log(pattern.heights);
			return false;
		}
		for (int i = 0; i < array.Length; i++)
		{
			int[] array2 = new int[16];
			if (array[i].Length != 16)
			{
				if (array[i].Length < 16)
				{
					Debug.LogError(string.Concat(new string[]
					{
						"[Heights] Pattern \"",
						pattern.name,
						"\" has ",
						array[i].Length.ToString(),
						" elements in row ",
						i.ToString(),
						" instead of ",
						16.ToString()
					}));
					return false;
				}
				int num = 0;
				bool flag = false;
				string text = "";
				int j = 0;
				while (j < array[i].Length)
				{
					int num2;
					if (!int.TryParse(array[i][j].ToString(), out num2) && array[i][j] != '-')
					{
						goto IL_016D;
					}
					if (flag)
					{
						text += array[i][j].ToString();
						goto IL_016D;
					}
					array2[num] = num2;
					num++;
					IL_01E7:
					j++;
					continue;
					IL_016D:
					if (array[i][j] == '(')
					{
						if (flag)
						{
							Debug.LogError("[Heights] Pattern \"" + pattern.name + "\", Error while parsing extended numbers!");
							return false;
						}
						flag = true;
					}
					if (array[i][j] != ')')
					{
						goto IL_01E7;
					}
					if (!flag)
					{
						Debug.LogError("[Heights] Pattern \"" + pattern.name + "\", Error while parsing extended numbers!");
						return false;
					}
					array2[num] = int.Parse(text);
					flag = false;
					text = "";
					num++;
					goto IL_01E7;
				}
				if (num != 16)
				{
					Debug.LogError(string.Concat(new string[]
					{
						"[Heights] Pattern \"",
						pattern.name,
						"\" has ",
						array[i].Length.ToString(),
						" elements in row ",
						num.ToString(),
						" instead of ",
						16.ToString()
					}));
					return false;
				}
			}
			else
			{
				for (int k = 0; k < array[i].Length; k++)
				{
					array2[k] = int.Parse(array[i][k].ToString());
				}
			}
			for (int l = 0; l < array2.Length; l++)
			{
				float num3 = this.colorCurve.Evaluate((float)array2[l]);
				target.SetPixel(offset.x * 16 + l, offset.y * 16 + i, new Color(num3, num3, num3));
			}
		}
		return true;
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x0002AEDC File Offset: 0x000290DC
	private void TogglePattern(string key, bool isPack)
	{
		if (isPack)
		{
			if (this.enabledPatternPacks.ContainsKey(key))
			{
				this.enabledPatternPacks.Remove(key);
				this.patternPackActiveIndicators[key].SetActive(false);
			}
			else if (Directory.Exists(Path.Combine(this.PatternsPath, key)))
			{
				string[] array = (from f in Directory.GetFiles(Path.Combine(this.PatternsPath, key))
					select Path.GetFileName(f)).ToArray<string>();
				List<ArenaPattern> list = new List<ArenaPattern>();
				foreach (string text in array)
				{
					ArenaPattern arenaPattern = this.LoadPattern(Path.Combine(key, text));
					list.Add(arenaPattern);
				}
				this.enabledPatternPacks.Add(key, list.ToArray());
				this.enabledPatternPacks[key] = list.ToArray();
				this.patternPackActiveIndicators[key].SetActive(true);
			}
			if (!MonoSingleton<EndlessGrid>.Instance.customPatternMode)
			{
				this.Toggle();
			}
		}
		else
		{
			if (this.enabledPatterns.ContainsKey(key))
			{
				this.enabledPatterns.Remove(key);
				this.patternActiveIndicators[key].SetActive(false);
			}
			else
			{
				this.enabledPatterns[key] = this.LoadPattern(key);
				this.patternActiveIndicators[key].SetActive(true);
			}
			if (!MonoSingleton<EndlessGrid>.Instance.customPatternMode)
			{
				this.Toggle();
			}
		}
		MonoSingleton<EndlessGrid>.Instance.customPatterns = this.AllEnabledPatterns;
		this.SaveEnabledPatterns();
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0002B068 File Offset: 0x00029268
	private ArenaPattern LoadPattern(string relativePath)
	{
		if (this.patternCache.ContainsKey(relativePath))
		{
			return this.patternCache[relativePath];
		}
		if (!File.Exists(Path.Combine(this.PatternsPath, relativePath)))
		{
			return null;
		}
		string[] array = File.ReadAllLines(Path.Combine(this.PatternsPath, relativePath));
		if (array.Length != 33)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < 16; i++)
		{
			stringBuilder.Append(array[i]);
			if (i != 15)
			{
				stringBuilder.Append('\n');
			}
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		for (int j = 17; j < 33; j++)
		{
			stringBuilder2.Append(array[j]);
			if (j != 32)
			{
				stringBuilder2.Append('\n');
			}
		}
		ArenaPattern arenaPattern = ScriptableObject.CreateInstance<ArenaPattern>();
		arenaPattern.heights = stringBuilder.ToString();
		arenaPattern.prefabs = stringBuilder2.ToString();
		arenaPattern.name = relativePath;
		this.patternCache[relativePath] = arenaPattern;
		return arenaPattern;
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x0002B156 File Offset: 0x00029356
	public void NextPage()
	{
		if (this.currentPage == this.maxPages)
		{
			return;
		}
		this.currentPage++;
		this.BuildButtons();
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0002B17B File Offset: 0x0002937B
	public void PreviousPage()
	{
		if (this.currentPage == 1)
		{
			return;
		}
		this.currentPage--;
		this.BuildButtons();
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x0002B19C File Offset: 0x0002939C
	public void OpenEditor()
	{
		try
		{
			new Process
			{
				StartInfo = 
				{
					FileName = Path.Combine(Application.streamingAssetsPath, "cgef", "cgef.exe")
				}
			}.Start();
		}
		catch
		{
			Application.OpenURL("https://cyber.pitr.dev/");
		}
	}

	// Token: 0x04000847 RID: 2119
	private Dictionary<string, ArenaPattern> patternCache = new Dictionary<string, ArenaPattern>();

	// Token: 0x04000848 RID: 2120
	private Dictionary<string, ArenaPattern> enabledPatterns = new Dictionary<string, ArenaPattern>();

	// Token: 0x04000849 RID: 2121
	private Dictionary<string, ArenaPattern[]> enabledPatternPacks = new Dictionary<string, ArenaPattern[]>();

	// Token: 0x0400084A RID: 2122
	private Dictionary<string, GameObject> patternActiveIndicators = new Dictionary<string, GameObject>();

	// Token: 0x0400084B RID: 2123
	private Dictionary<string, GameObject> patternPackActiveIndicators = new Dictionary<string, GameObject>();

	// Token: 0x0400084C RID: 2124
	private int currentPage = 1;

	// Token: 0x0400084D RID: 2125
	private int maxPages = 1;

	// Token: 0x0400084E RID: 2126
	private int maxItemsPerPage = 12;

	// Token: 0x0400084F RID: 2127
	[SerializeField]
	private AnimationCurve colorCurve;

	// Token: 0x04000850 RID: 2128
	[SerializeField]
	private Texture2D parsingErrorTexture;

	// Token: 0x04000851 RID: 2129
	[SerializeField]
	private GameObject buttonTemplate;

	// Token: 0x04000852 RID: 2130
	[SerializeField]
	private GameObject packButtonTemplate;

	// Token: 0x04000853 RID: 2131
	[SerializeField]
	private Transform grid;

	// Token: 0x04000854 RID: 2132
	[SerializeField]
	private TMP_Text stateButtonText;

	// Token: 0x04000855 RID: 2133
	[SerializeField]
	private TMP_Text pageText;

	// Token: 0x04000856 RID: 2134
	public GameObject enableWhenCustom;

	// Token: 0x04000857 RID: 2135
	public GameObject disableWhenCustom;
}
