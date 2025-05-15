using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F9 RID: 249
public class CustomPaletteSelector : MonoBehaviour
{
	// Token: 0x17000073 RID: 115
	// (get) Token: 0x060004D0 RID: 1232 RVA: 0x00020F41 File Offset: 0x0001F141
	private static string PalettePath
	{
		get
		{
			return Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Palettes");
		}
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x00020F5C File Offset: 0x0001F15C
	private static void Init()
	{
		if (!Directory.Exists(CustomPaletteSelector.PalettePath))
		{
			Directory.CreateDirectory(CustomPaletteSelector.PalettePath);
		}
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x00020F78 File Offset: 0x0001F178
	private void ResetMenu()
	{
		for (int i = 1; i < this.container.childCount; i++)
		{
			Object.Destroy(this.container.GetChild(i).gameObject);
		}
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x00020FB4 File Offset: 0x0001F1B4
	private void RefreshCurrentPreview()
	{
		Texture currentTexture = MonoSingleton<PostProcessV2_Handler>.Instance.CurrentTexture;
		if (currentTexture == null)
		{
			return;
		}
		Sprite sprite = Sprite.Create((Texture2D)currentTexture, new Rect(0f, 0f, (float)currentTexture.width, (float)currentTexture.height), new Vector2(0.5f, 0.5f), 100f);
		sprite.texture.filterMode = FilterMode.Point;
		this.previewImage.sprite = sprite;
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x0002102B File Offset: 0x0001F22B
	public void ShowMenu()
	{
		this.BuildMenu();
		this.RefreshCurrentPreview();
		this.menu.SetActive(true);
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x00021048 File Offset: 0x0001F248
	private void BuildMenu()
	{
		this.ResetMenu();
		this.buttonTemplate.gameObject.SetActive(false);
		using (IEnumerator<string> enumerator = (from a in Directory.GetFiles(CustomPaletteSelector.PalettePath, "*", SearchOption.TopDirectoryOnly)
			where CustomTextures.AllowedExtensions.Contains(Path.GetExtension(a))
			select a).Select(new Func<string, string>(Path.GetFileName)).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string palette = enumerator.Current;
				Texture2D txt = CustomPaletteSelector.LoadPalette(palette);
				Sprite sprite = Sprite.Create(txt, new Rect(0f, 0f, (float)txt.width, (float)txt.height), new Vector2(0.5f, 0.5f), 100f);
				sprite.texture.filterMode = FilterMode.Point;
				this.templatePreviewImage.sprite = sprite;
				this.templateFileName.text = Path.GetFileNameWithoutExtension(palette);
				Button button = Object.Instantiate<Button>(this.buttonTemplate, this.container, false);
				button.gameObject.SetActive(true);
				button.onClick.AddListener(delegate
				{
					this.SetGamePalette(txt, palette);
				});
			}
		}
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x000211B8 File Offset: 0x0001F3B8
	private void SetGamePalette(Texture2D txt, string name)
	{
		MonoSingleton<PrefsManager>.Instance.SetStringLocal("colorPaletteTexture", name);
		this.RefreshCurrentPreview();
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x000211D0 File Offset: 0x0001F3D0
	private static Texture2D LoadPalette(string name)
	{
		if (!File.Exists(Path.Combine(CustomPaletteSelector.PalettePath, name)))
		{
			return null;
		}
		byte[] array = File.ReadAllBytes(Path.Combine(CustomPaletteSelector.PalettePath, name));
		Texture2D texture2D = new Texture2D(0, 0, TextureFormat.RGBA32, false);
		texture2D.filterMode = FilterMode.Point;
		texture2D.LoadImage(array);
		texture2D.name = name;
		return texture2D;
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x00021224 File Offset: 0x0001F424
	public static Texture2D LoadSavedPalette()
	{
		CustomPaletteSelector.Init();
		string stringLocal = MonoSingleton<PrefsManager>.Instance.GetStringLocal("colorPaletteTexture", null);
		if (!string.IsNullOrEmpty(stringLocal))
		{
			return CustomPaletteSelector.LoadPalette(stringLocal);
		}
		return null;
	}

	// Token: 0x04000699 RID: 1689
	[Space]
	[SerializeField]
	private GameObject menu;

	// Token: 0x0400069A RID: 1690
	[SerializeField]
	private Transform container;

	// Token: 0x0400069B RID: 1691
	[Space]
	[SerializeField]
	private Image templatePreviewImage;

	// Token: 0x0400069C RID: 1692
	[SerializeField]
	private Text templateFileName;

	// Token: 0x0400069D RID: 1693
	[SerializeField]
	private Button buttonTemplate;

	// Token: 0x0400069E RID: 1694
	[Space]
	[SerializeField]
	private Image previewImage;
}
