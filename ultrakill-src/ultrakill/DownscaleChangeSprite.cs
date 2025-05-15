using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000124 RID: 292
public class DownscaleChangeSprite : MonoBehaviour
{
	// Token: 0x06000563 RID: 1379 RVA: 0x00024434 File Offset: 0x00022634
	private void Start()
	{
		this.CheckScale();
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0002443C File Offset: 0x0002263C
	private void OnEnable()
	{
		this.CheckScale();
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x00024464 File Offset: 0x00022664
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x00024486 File Offset: 0x00022686
	private void OnPrefChanged(string key, object value)
	{
		if (key == "pixelization")
		{
			this.CheckScale();
		}
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x0002449C File Offset: 0x0002269C
	public void CheckScale()
	{
		if (this.img == null)
		{
			this.img = base.GetComponent<Image>();
		}
		if (MonoSingleton<PrefsManager>.Instance.GetInt("pixelization", 0) == 1)
		{
			this.img.sprite = this.downscaled;
			return;
		}
		this.img.sprite = this.normal;
	}

	// Token: 0x04000771 RID: 1905
	public Sprite normal;

	// Token: 0x04000772 RID: 1906
	public Sprite downscaled;

	// Token: 0x04000773 RID: 1907
	private Image img;
}
