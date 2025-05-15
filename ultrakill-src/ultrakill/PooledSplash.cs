using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000350 RID: 848
public class PooledSplash : MonoBehaviour
{
	// Token: 0x0600139B RID: 5019 RVA: 0x0009CD82 File Offset: 0x0009AF82
	private void Start()
	{
		this.waterStore = MonoSingleton<PooledWaterStore>.Instance;
	}

	// Token: 0x0600139C RID: 5020 RVA: 0x0009CD8F File Offset: 0x0009AF8F
	private void OnEnable()
	{
		this.RandomizePitch();
		this.ScheduleRemove();
		this.InitializeScaleNFade();
	}

	// Token: 0x0600139D RID: 5021 RVA: 0x0004C7E6 File Offset: 0x0004A9E6
	private void OnDisable()
	{
		base.CancelInvoke();
	}

	// Token: 0x0600139E RID: 5022 RVA: 0x0009CDA4 File Offset: 0x0009AFA4
	private void RandomizePitch()
	{
		if (!this.aud)
		{
			this.aud = base.GetComponent<AudioSource>();
		}
		if (this.aud != null)
		{
			if (this.pitchVariation == 0f)
			{
				this.aud.pitch = Random.Range(0.8f, 1.2f);
			}
			else
			{
				this.aud.pitch = Random.Range(this.defaultPitch - this.pitchVariation, this.defaultPitch + this.pitchVariation);
			}
			this.aud.Play();
		}
	}

	// Token: 0x0600139F RID: 5023 RVA: 0x0009CE36 File Offset: 0x0009B036
	private void ScheduleRemove()
	{
		base.CancelInvoke();
		base.Invoke("ReturnToQueue", this.time + Random.Range(-this.randomizer, this.randomizer));
	}

	// Token: 0x060013A0 RID: 5024 RVA: 0x0009CE62 File Offset: 0x0009B062
	private void ReturnToQueue()
	{
		this.waterStore.ReturnToQueue(base.gameObject, this.splashType);
	}

	// Token: 0x060013A1 RID: 5025 RVA: 0x0009CE7C File Offset: 0x0009B07C
	private void InitializeScaleNFade()
	{
		if (this.fade)
		{
			switch (this.ft)
			{
			case FadeType.Sprite:
				this.sr = base.GetComponent<SpriteRenderer>();
				break;
			case FadeType.Line:
				this.lr = base.GetComponent<LineRenderer>();
				break;
			case FadeType.Light:
				this.lght = base.GetComponent<Light>();
				break;
			case FadeType.Renderer:
				this.rend = base.GetComponent<Renderer>();
				if (this.rend == null)
				{
					this.rend = base.GetComponentInChildren<Renderer>();
				}
				break;
			case FadeType.UiImage:
				this.img = base.GetComponent<Image>();
				break;
			}
		}
		if (this.rend != null)
		{
			this.hasOpacScale = this.rend.material.HasProperty("_OpacScale");
			this.hasTint = this.rend.material.HasProperty("_Tint");
			this.hasColor = this.rend.material.HasProperty("_Color");
		}
		this.scaleAmt = base.transform.localScale;
	}

	// Token: 0x060013A2 RID: 5026 RVA: 0x0009CF88 File Offset: 0x0009B188
	private void Update()
	{
		if (this.scale)
		{
			this.scaleAmt += Vector3.one * Time.deltaTime * this.scaleSpeed;
			base.transform.localScale = this.scaleAmt;
		}
		if (!this.fade)
		{
			return;
		}
		switch (this.ft)
		{
		case FadeType.Sprite:
			this.sr.color = this.FadeColor(this.sr.color);
			return;
		case FadeType.Line:
		case FadeType.Light:
			break;
		case FadeType.Renderer:
			this.FadeRenderer();
			break;
		case FadeType.UiImage:
			this.img.color = this.FadeColor(this.img.color);
			return;
		default:
			return;
		}
	}

	// Token: 0x060013A3 RID: 5027 RVA: 0x0009D044 File Offset: 0x0009B244
	private void FixedUpdate()
	{
		if (this.fade && this.ft == FadeType.Line && this.lr)
		{
			Color startColor = this.lr.startColor;
			startColor.a -= this.fadeSpeed * Time.deltaTime;
			this.lr.startColor = startColor;
			Color endColor = this.lr.endColor;
			endColor.a -= this.fadeSpeed * Time.deltaTime;
			this.lr.endColor = endColor;
		}
	}

	// Token: 0x060013A4 RID: 5028 RVA: 0x0009D0CD File Offset: 0x0009B2CD
	private Color FadeColor(Color c)
	{
		if (c.a <= 0f && this.fadeSpeed > 0f)
		{
			return c;
		}
		c.a -= this.fadeSpeed * Time.deltaTime;
		return c;
	}

	// Token: 0x060013A5 RID: 5029 RVA: 0x0009D104 File Offset: 0x0009B304
	private void FadeRenderer()
	{
		if (this.hasOpacScale)
		{
			float num = this.rend.material.GetFloat("_OpacScale");
			num -= this.fadeSpeed * Time.deltaTime;
			this.rend.material.SetFloat("_OpacScale", num);
			return;
		}
		if (this.hasTint || this.hasColor)
		{
			string text = (this.hasTint ? "_Tint" : "_Color");
			Color color = this.rend.material.GetColor(text);
			color.a -= this.fadeSpeed * Time.deltaTime;
			this.rend.material.SetColor(text, color);
		}
	}

	// Token: 0x060013A6 RID: 5030 RVA: 0x0009D1B5 File Offset: 0x0009B3B5
	public void ChangeFadeSpeed(float newSpeed)
	{
		this.fadeSpeed = newSpeed;
	}

	// Token: 0x060013A7 RID: 5031 RVA: 0x0009D1BE File Offset: 0x0009B3BE
	public void ChangeScaleSpeed(float newSpeed)
	{
		this.scaleSpeed = newSpeed;
	}

	// Token: 0x04001AE2 RID: 6882
	public float defaultPitch = 1f;

	// Token: 0x04001AE3 RID: 6883
	public float pitchVariation = 0.1f;

	// Token: 0x04001AE4 RID: 6884
	public AudioSource aud;

	// Token: 0x04001AE5 RID: 6885
	public float time = 1f;

	// Token: 0x04001AE6 RID: 6886
	public float randomizer;

	// Token: 0x04001AE7 RID: 6887
	public bool scale;

	// Token: 0x04001AE8 RID: 6888
	public bool fade;

	// Token: 0x04001AE9 RID: 6889
	public FadeType ft;

	// Token: 0x04001AEA RID: 6890
	public float scaleSpeed;

	// Token: 0x04001AEB RID: 6891
	public float fadeSpeed;

	// Token: 0x04001AEC RID: 6892
	private SpriteRenderer sr;

	// Token: 0x04001AED RID: 6893
	private LineRenderer lr;

	// Token: 0x04001AEE RID: 6894
	private Light lght;

	// Token: 0x04001AEF RID: 6895
	private Renderer rend;

	// Token: 0x04001AF0 RID: 6896
	private Image img;

	// Token: 0x04001AF1 RID: 6897
	private bool hasOpacScale;

	// Token: 0x04001AF2 RID: 6898
	private bool hasTint;

	// Token: 0x04001AF3 RID: 6899
	private bool hasColor;

	// Token: 0x04001AF4 RID: 6900
	private Vector3 scaleAmt = Vector3.one;

	// Token: 0x04001AF5 RID: 6901
	public Water.WaterGOType splashType;

	// Token: 0x04001AF6 RID: 6902
	[HideInInspector]
	public PooledWaterStore waterStore;
}
