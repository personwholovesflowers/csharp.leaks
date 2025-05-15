using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x020002C1 RID: 705
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class LevelNamePopup : MonoSingleton<LevelNamePopup>
{
	// Token: 0x06000F31 RID: 3889 RVA: 0x000701D8 File Offset: 0x0006E3D8
	private void Start()
	{
		MapInfoBase instance = MapInfoBase.Instance;
		if (instance)
		{
			this.layerString = instance.layerName;
			this.nameString = instance.levelName;
		}
		this.aud = base.GetComponent<AudioSource>();
		this.layerText.text = "";
		this.nameText.text = "";
	}

	// Token: 0x06000F32 RID: 3890 RVA: 0x00070238 File Offset: 0x0006E438
	private void Update()
	{
		if (this.countTime)
		{
			this.textTimer += Time.deltaTime;
		}
		if (this.fadingOut)
		{
			Color color = this.layerText.color;
			color.a = Mathf.MoveTowards(color.a, 0f, Time.deltaTime);
			this.layerText.color = color;
			this.nameText.color = color;
			if (color.a <= 0f)
			{
				this.fadingOut = false;
			}
		}
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x000702BB File Offset: 0x0006E4BB
	public void NameAppearDelayed(float delay)
	{
		base.Invoke("NameAppear", delay);
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x000702C9 File Offset: 0x0006E4C9
	public void NameAppear()
	{
		if (!this.activated)
		{
			this.activated = true;
			base.StartCoroutine(this.ShowLayerText());
		}
	}

	// Token: 0x06000F35 RID: 3893 RVA: 0x000702E7 File Offset: 0x0006E4E7
	private IEnumerator ShowLayerText()
	{
		this.countTime = true;
		this.currentLetter = 0;
		this.aud.Play();
		while (this.currentLetter <= this.layerString.Length)
		{
			while (this.textTimer >= 0.01f && this.currentLetter <= this.layerString.Length)
			{
				this.textTimer -= 0.01f;
				this.layerText.text = this.layerString.Substring(0, this.currentLetter);
				this.currentLetter++;
			}
			yield return new WaitForSeconds(0.01f);
		}
		this.countTime = false;
		this.aud.Stop();
		yield return new WaitForSeconds(0.5f);
		base.StartCoroutine(this.ShowNameText());
		yield break;
	}

	// Token: 0x06000F36 RID: 3894 RVA: 0x000702F6 File Offset: 0x0006E4F6
	private IEnumerator ShowNameText()
	{
		this.countTime = true;
		this.currentLetter = 0;
		this.aud.Play();
		while (this.currentLetter <= this.nameString.Length)
		{
			while (this.textTimer >= 0.01f && this.currentLetter <= this.nameString.Length)
			{
				this.textTimer -= 0.01f;
				this.nameText.text = this.nameString.Substring(0, this.currentLetter);
				this.currentLetter++;
			}
			yield return new WaitForSeconds(0.01f);
		}
		this.countTime = false;
		this.aud.Stop();
		yield return new WaitForSeconds(3f);
		this.fadingOut = true;
		yield break;
	}

	// Token: 0x0400145E RID: 5214
	public TMP_Text layerText;

	// Token: 0x0400145F RID: 5215
	private string layerString;

	// Token: 0x04001460 RID: 5216
	public TMP_Text nameText;

	// Token: 0x04001461 RID: 5217
	private string nameString;

	// Token: 0x04001462 RID: 5218
	private bool activated;

	// Token: 0x04001463 RID: 5219
	private bool fadingOut;

	// Token: 0x04001464 RID: 5220
	private AudioSource aud;

	// Token: 0x04001465 RID: 5221
	private float textTimer;

	// Token: 0x04001466 RID: 5222
	private int currentLetter;

	// Token: 0x04001467 RID: 5223
	private bool countTime;
}
