using System;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F5 RID: 245
public class Crosshair : MonoBehaviour
{
	// Token: 0x060004C5 RID: 1221 RVA: 0x00020B13 File Offset: 0x0001ED13
	private void Start()
	{
		this.mainch = base.GetComponent<Image>();
		MonoSingleton<StatsManager>.Instance.crosshair = base.gameObject;
		this.CheckCrossHair();
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x00020B37 File Offset: 0x0001ED37
	private void OnEnable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x00020B59 File Offset: 0x0001ED59
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x00020B7B File Offset: 0x0001ED7B
	private void OnPrefChanged(string key, object value)
	{
		if (key == "crossHair" || key == "crossHairColor" || key == "crossHairHud")
		{
			this.CheckCrossHair();
		}
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x00020BAC File Offset: 0x0001EDAC
	public void CheckCrossHair()
	{
		if (this.mainch == null)
		{
			this.mainch = base.GetComponent<Image>();
		}
		if (HideUI.Active)
		{
			this.mainch.enabled = false;
			Image[] array = this.altchs;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
			array = this.chuds;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
			return;
		}
		this.crossHairType = MonoSingleton<PrefsManager>.Instance.GetInt("crossHair", 0);
		switch (this.crossHairType)
		{
		case 0:
		{
			this.mainch.enabled = false;
			Image[] array = this.altchs;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].enabled = false;
			}
			break;
		}
		case 1:
		{
			this.mainch.enabled = true;
			Image[] array = this.altchs;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].enabled = false;
			}
			break;
		}
		case 2:
		{
			this.mainch.enabled = true;
			Image[] array = this.altchs;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].enabled = true;
			}
			break;
		}
		}
		Color color = Color.white;
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("crossHairColor", 0);
		switch (@int)
		{
		case 0:
		case 1:
			color = Color.white;
			break;
		case 2:
			color = Color.gray;
			break;
		case 3:
			color = Color.black;
			break;
		case 4:
			color = Color.red;
			break;
		case 5:
			color = Color.green;
			break;
		case 6:
			color = Color.blue;
			break;
		case 7:
			color = Color.cyan;
			break;
		case 8:
			color = Color.yellow;
			break;
		case 9:
			color = Color.magenta;
			break;
		}
		if (@int == 0)
		{
			this.mainch.material = this.invertMaterial;
		}
		else
		{
			this.mainch.material = null;
		}
		this.mainch.color = color;
		foreach (Image image in this.altchs)
		{
			image.color = color;
			if (@int == 0)
			{
				image.material = this.invertMaterial;
			}
			else
			{
				image.material = null;
			}
		}
		int int2 = MonoSingleton<PrefsManager>.Instance.GetInt("crossHairHud", 0);
		if (int2 == 0)
		{
			Image[] array = this.chuds;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
			return;
		}
		foreach (Image image2 in this.chuds)
		{
			image2.enabled = true;
			image2.sprite = this.circles[int2 - 1];
		}
	}

	// Token: 0x04000679 RID: 1657
	private int crossHairType;

	// Token: 0x0400067A RID: 1658
	private Image mainch;

	// Token: 0x0400067B RID: 1659
	public Image[] altchs;

	// Token: 0x0400067C RID: 1660
	public Image[] chuds;

	// Token: 0x0400067D RID: 1661
	public Sprite[] circles;

	// Token: 0x0400067E RID: 1662
	public Material invertMaterial;
}
