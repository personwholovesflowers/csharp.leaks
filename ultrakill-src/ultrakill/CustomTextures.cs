using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000140 RID: 320
public class CustomTextures : DirectoryTreeBrowser<FileInfo>
{
	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06000633 RID: 1587 RVA: 0x0002B2CE File Offset: 0x000294CE
	private string TexturesPath
	{
		get
		{
			return Path.Combine(Directory.GetParent(Application.dataPath).FullName, "CyberGrind", "Textures");
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000634 RID: 1588 RVA: 0x0002B2EE File Offset: 0x000294EE
	protected override int maxPageLength
	{
		get
		{
			return 14;
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x06000635 RID: 1589 RVA: 0x0002B2F2 File Offset: 0x000294F2
	protected override IDirectoryTree<FileInfo> baseDirectory
	{
		get
		{
			return new FileDirectoryTree(this.TexturesPath, null);
		}
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x0002B300 File Offset: 0x00029500
	public bool TryToLoad(string key)
	{
		if (!File.Exists(Path.Combine(this.TexturesPath, key)))
		{
			Debug.LogError("Tried to load an invalid texture! " + key);
			return false;
		}
		this.LoadTexture(key);
		return true;
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x0002B330 File Offset: 0x00029530
	public void SetEditMode(int m)
	{
		base.GoToBase();
		this.currentEditMode = (CustomTextures.EditMode)m;
		if (this.gridBtn)
		{
			this.gridBtn.interactable = m != 1;
			this.gridBtn.GetComponent<ShopButton>().deactivated = m == 1;
			this.skyboxBtn.interactable = m != 2;
			this.skyboxBtn.GetComponent<ShopButton>().deactivated = m == 2;
			this.emissionBtn.interactable = m != 3;
			this.emissionBtn.GetComponent<ShopButton>().deactivated = m == 3;
			this.fogBtn.interactable = m != 4;
			this.fogBtn.GetComponent<ShopButton>().deactivated = m == 4;
		}
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x0002B3F4 File Offset: 0x000295F4
	public void SetGridEditMode(int num)
	{
		switch (num)
		{
		case 0:
			if (this.editBase)
			{
				this.editBase = false;
			}
			else
			{
				this.editBase = true;
			}
			break;
		case 1:
			if (this.editTop)
			{
				this.editTop = false;
			}
			else
			{
				this.editTop = true;
			}
			break;
		case 2:
			if (this.editTopRow)
			{
				this.editTopRow = false;
			}
			else
			{
				this.editTopRow = true;
			}
			break;
		}
		if (this.editBase)
		{
			this.baseBtnFrame.color = Color.red;
		}
		else
		{
			this.baseBtnFrame.color = Color.white;
		}
		if (this.editTop)
		{
			this.topBtnFrame.color = Color.red;
		}
		else
		{
			this.topBtnFrame.color = Color.white;
		}
		if (this.editTopRow)
		{
			this.topRowBtnFrame.color = Color.red;
			return;
		}
		this.topRowBtnFrame.color = Color.white;
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x0002B4E0 File Offset: 0x000296E0
	public void SetTexture(string key)
	{
		switch (this.currentEditMode)
		{
		case CustomTextures.EditMode.Grid:
			if (this.editBase)
			{
				MonoSingleton<PrefsManager>.Instance.SetStringLocal("cyberGrind.customGrid_" + 0.ToString(), key);
				this.gridMaterials[0].mainTexture = this.imageCache[key];
			}
			if (this.editTop)
			{
				MonoSingleton<PrefsManager>.Instance.SetStringLocal("cyberGrind.customGrid_" + 1.ToString(), key);
				this.gridMaterials[1].mainTexture = this.imageCache[key];
			}
			if (this.editTopRow)
			{
				MonoSingleton<PrefsManager>.Instance.SetStringLocal("cyberGrind.customGrid_" + 2.ToString(), key);
				this.gridMaterials[2].mainTexture = this.imageCache[key];
				return;
			}
			break;
		case CustomTextures.EditMode.Skybox:
		{
			MonoSingleton<PrefsManager>.Instance.SetStringLocal("cyberGrind.customSkybox", key);
			this.skyMaterial.mainTexture = this.imageCache[key];
			OutdoorLightMaster outdoorLightMaster = this.olm;
			if (outdoorLightMaster == null)
			{
				return;
			}
			outdoorLightMaster.UpdateSkyboxMaterial();
			break;
		}
		case CustomTextures.EditMode.Emission:
			if (this.editBase)
			{
				MonoSingleton<PrefsManager>.Instance.SetStringLocal("cyberGrind.customGlow_" + 0.ToString(), key);
				this.gridMaterials[0].SetTexture(CustomTextures.EmissiveTex, this.imageCache[key]);
			}
			if (this.editTop)
			{
				MonoSingleton<PrefsManager>.Instance.SetStringLocal("cyberGrind.customGlow_" + 1.ToString(), key);
				this.gridMaterials[1].SetTexture(CustomTextures.EmissiveTex, this.imageCache[key]);
			}
			if (this.editTopRow)
			{
				MonoSingleton<PrefsManager>.Instance.SetStringLocal("cyberGrind.customGlow_" + 2.ToString(), key);
				this.gridMaterials[2].SetTexture(CustomTextures.EmissiveTex, this.imageCache[key]);
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x0002B6CE File Offset: 0x000298CE
	public void SetGlowIntensity()
	{
		MonoSingleton<EndlessGrid>.Instance.glowMultiplier = this.glowSlider.value;
		MonoSingleton<EndlessGrid>.Instance.UpdateGlow();
		MonoSingleton<PrefsManager>.Instance.SetFloatLocal("cyberGrind.glowIntensity", this.glowSlider.value);
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x0002B70C File Offset: 0x0002990C
	private void Start()
	{
		string[] array = new int[] { 0, 1, 2 }.Select((int i) => MonoSingleton<PrefsManager>.Instance.GetStringLocal("cyberGrind.customGrid_" + i.ToString(), null)).ToArray<string>();
		string[] array2 = new int[] { 0, 1, 2 }.Select((int i) => MonoSingleton<PrefsManager>.Instance.GetStringLocal("cyberGrind.customGlow_" + i.ToString(), null)).ToArray<string>();
		for (int k = 0; k < array.Length; k++)
		{
			if (!string.IsNullOrEmpty(array[k]) && this.TryToLoad(array[k]))
			{
				this.gridMaterials[k].mainTexture = this.imageCache[array[k]];
			}
			else
			{
				MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.customGrid_" + k.ToString());
			}
		}
		for (int j = 0; j < array2.Length; j++)
		{
			if (!string.IsNullOrEmpty(array2[j]) && this.TryToLoad(array2[j]))
			{
				this.gridMaterials[j].SetTexture(CustomTextures.EmissiveTex, this.imageCache[array2[j]]);
			}
			else
			{
				MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.customGlow_" + j.ToString());
				this.gridMaterials[j].SetTexture("_EmissiveTex", this.defaultGlow);
			}
		}
		string stringLocal = MonoSingleton<PrefsManager>.Instance.GetStringLocal("cyberGrind.customSkybox", null);
		if (!string.IsNullOrEmpty(stringLocal) && this.TryToLoad(stringLocal))
		{
			this.skyMaterial.mainTexture = this.imageCache[stringLocal];
			OutdoorLightMaster outdoorLightMaster = this.olm;
			if (outdoorLightMaster != null)
			{
				outdoorLightMaster.UpdateSkyboxMaterial();
			}
		}
		float floatLocal = MonoSingleton<PrefsManager>.Instance.GetFloatLocal("cyberGrind.glowIntensity", -1f);
		if (floatLocal != -1f)
		{
			this.glowSlider.SetValueWithoutNotify(floatLocal);
			MonoSingleton<EndlessGrid>.Instance.glowMultiplier = this.glowSlider.value;
			MonoSingleton<EndlessGrid>.Instance.UpdateGlow();
		}
		if (this.gridBtn)
		{
			this.gridBtn.GetComponent<ShopButton>().PointerClickSuccess += delegate
			{
				this.SetEditMode(1);
			};
			this.emissionBtn.GetComponent<ShopButton>().PointerClickSuccess += delegate
			{
				this.SetEditMode(3);
			};
			this.skyboxBtn.GetComponent<ShopButton>().PointerClickSuccess += delegate
			{
				this.SetEditMode(2);
			};
			this.fogBtn.GetComponent<ShopButton>().PointerClickSuccess += delegate
			{
				this.SetEditMode(4);
			};
			this.baseBtn.GetComponent<ShopButton>().PointerClickSuccess += delegate
			{
				this.SetGridEditMode(0);
			};
			this.topRowBtn.GetComponent<ShopButton>().PointerClickSuccess += delegate
			{
				this.SetGridEditMode(2);
			};
			this.topBtn.GetComponent<ShopButton>().PointerClickSuccess += delegate
			{
				this.SetGridEditMode(1);
			};
		}
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x0002B9D4 File Offset: 0x00029BD4
	protected override Action BuildLeaf(FileInfo file, int indexInPage)
	{
		Texture2D texture2D = this.LoadTexture(file.FullName);
		GameObject btn = Object.Instantiate<GameObject>(this.itemButtonTemplate, this.itemParent, false);
		Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f), 100f);
		sprite.texture.filterMode = FilterMode.Point;
		btn.GetComponent<Button>().onClick.RemoveAllListeners();
		btn.GetComponent<Button>().onClick.AddListener(delegate
		{
			this.SetTexture(file.FullName);
		});
		btn.GetComponent<Image>().sprite = sprite;
		btn.SetActive(true);
		return delegate
		{
			Object.Destroy(btn);
		};
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x0002BAC4 File Offset: 0x00029CC4
	private Texture2D LoadTexture(string name)
	{
		if (this.imageCache.ContainsKey(name))
		{
			return this.imageCache[name];
		}
		byte[] array = File.ReadAllBytes(Path.Combine(this.TexturesPath, name));
		Texture2D texture2D = new Texture2D(0, 0, TextureFormat.RGBA32, false);
		texture2D.filterMode = FilterMode.Point;
		texture2D.LoadImage(array);
		this.imageCache[name] = texture2D;
		return texture2D;
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x0002BB28 File Offset: 0x00029D28
	public void RemoveCustomPrefs()
	{
		for (int i = 0; i < 3; i++)
		{
			MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.customGlow_" + i.ToString());
			MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.customGrid_" + i.ToString());
		}
		MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.customSkybox");
		MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.glowIntensity");
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x0002BB98 File Offset: 0x00029D98
	public void ResetTexture()
	{
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("cyberGrind.theme", 0);
		switch (this.currentEditMode)
		{
		case CustomTextures.EditMode.Grid:
			if (this.editBase)
			{
				MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.customGrid_" + 0.ToString());
				this.gridMaterials[0].mainTexture = this.defaultGridTextures[@int];
			}
			if (this.editTop)
			{
				MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.customGrid_" + 1.ToString());
				this.gridMaterials[1].mainTexture = this.defaultGridTextures[@int];
			}
			if (this.editTopRow)
			{
				MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.customGrid_" + 2.ToString());
				this.gridMaterials[2].mainTexture = this.defaultGridTextures[@int];
				return;
			}
			break;
		case CustomTextures.EditMode.Skybox:
		{
			this.skyMaterial.mainTexture = this.defaultSkyboxes[@int];
			OutdoorLightMaster outdoorLightMaster = this.olm;
			if (outdoorLightMaster != null)
			{
				outdoorLightMaster.UpdateSkyboxMaterial();
			}
			MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.customSkybox");
			break;
		}
		case CustomTextures.EditMode.Emission:
			if (this.editBase)
			{
				MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.customGlow_" + 0.ToString());
				this.gridMaterials[0].SetTexture(CustomTextures.EmissiveTex, this.defaultEmission);
			}
			if (this.editTop)
			{
				MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.customGlow_" + 1.ToString());
				this.gridMaterials[1].SetTexture(CustomTextures.EmissiveTex, this.defaultEmission);
			}
			if (this.editTopRow)
			{
				MonoSingleton<PrefsManager>.Instance.DeleteKey("cyberGrind.customGlow_" + 2.ToString());
				this.gridMaterials[2].SetTexture(CustomTextures.EmissiveTex, this.defaultEmission);
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x04000862 RID: 2146
	[SerializeField]
	private Material[] gridMaterials;

	// Token: 0x04000863 RID: 2147
	[SerializeField]
	private OutdoorLightMaster olm;

	// Token: 0x04000864 RID: 2148
	[SerializeField]
	private Material skyMaterial;

	// Token: 0x04000865 RID: 2149
	[SerializeField]
	private Texture defaultGlow;

	// Token: 0x04000866 RID: 2150
	[SerializeField]
	private Texture[] defaultGridTextures;

	// Token: 0x04000867 RID: 2151
	[SerializeField]
	private Texture defaultEmission;

	// Token: 0x04000868 RID: 2152
	[SerializeField]
	private Texture[] defaultSkyboxes;

	// Token: 0x04000869 RID: 2153
	[SerializeField]
	private GameObject gridWrapper;

	// Token: 0x0400086A RID: 2154
	[SerializeField]
	private Button gridBtn;

	// Token: 0x0400086B RID: 2155
	[SerializeField]
	private Button skyboxBtn;

	// Token: 0x0400086C RID: 2156
	[SerializeField]
	private Button emissionBtn;

	// Token: 0x0400086D RID: 2157
	[SerializeField]
	private Button fogBtn;

	// Token: 0x0400086E RID: 2158
	private Dictionary<string, Texture2D> imageCache = new Dictionary<string, Texture2D>();

	// Token: 0x0400086F RID: 2159
	private CustomTextures.EditMode currentEditMode;

	// Token: 0x04000870 RID: 2160
	private bool editBase = true;

	// Token: 0x04000871 RID: 2161
	private bool editTop = true;

	// Token: 0x04000872 RID: 2162
	private bool editTopRow = true;

	// Token: 0x04000873 RID: 2163
	[SerializeField]
	private Button baseBtn;

	// Token: 0x04000874 RID: 2164
	[SerializeField]
	private Image baseBtnFrame;

	// Token: 0x04000875 RID: 2165
	[SerializeField]
	private Button topBtn;

	// Token: 0x04000876 RID: 2166
	[SerializeField]
	private Image topBtnFrame;

	// Token: 0x04000877 RID: 2167
	[SerializeField]
	private Button topRowBtn;

	// Token: 0x04000878 RID: 2168
	[SerializeField]
	private Image topRowBtnFrame;

	// Token: 0x04000879 RID: 2169
	[SerializeField]
	private Slider glowSlider;

	// Token: 0x0400087A RID: 2170
	private static readonly int EmissiveTex = Shader.PropertyToID("_EmissiveTex");

	// Token: 0x0400087B RID: 2171
	public static readonly string[] AllowedExtensions = new string[] { ".png", ".jpg", ".jpeg" };

	// Token: 0x02000141 RID: 321
	private enum EditMode
	{
		// Token: 0x0400087D RID: 2173
		None,
		// Token: 0x0400087E RID: 2174
		Grid,
		// Token: 0x0400087F RID: 2175
		Skybox,
		// Token: 0x04000880 RID: 2176
		Emission,
		// Token: 0x04000881 RID: 2177
		Fog
	}
}
