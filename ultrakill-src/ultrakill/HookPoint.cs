using System;
using Sandbox;
using UnityEngine;

// Token: 0x0200024F RID: 591
public class HookPoint : MonoBehaviour, IAlter, IAlterOptions<float>
{
	// Token: 0x06000CEF RID: 3311 RVA: 0x00062AC8 File Offset: 0x00060CC8
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
		if (!this.valuesSet)
		{
			this.SetValues();
		}
		if (!this.active)
		{
			this.TurnOff();
			return;
		}
		if (this.activeParticle != null)
		{
			this.activeParticle.Play();
		}
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x00062B18 File Offset: 0x00060D18
	private void Update()
	{
		if (this.timer > 0f)
		{
			this.timer = Mathf.MoveTowards(this.timer, 0f, Time.deltaTime);
			this.tickTimer = Mathf.MoveTowards(this.tickTimer, 0f, Time.deltaTime);
			if (this.tickTimer == 0f)
			{
				Object.Instantiate<AudioSource>(this.reactivationTick, base.transform.position, Quaternion.identity);
				for (int i = 0; i < this.spins.Length; i++)
				{
					this.spins[i].transform.localRotation = Quaternion.Lerp(this.spinDefaultRotations[i], this.spinDefaultRotations[i] * Quaternion.AngleAxis(-90f, this.spins[i].spinDirection), this.timer / this.reactivationTime);
				}
				if (this.timer > 3f)
				{
					this.tickTimer = 1f;
				}
				else if (this.timer > 1f)
				{
					this.tickTimer = 0.5f;
				}
				else
				{
					this.tickTimer = 0.25f;
				}
			}
			if (this.timer <= 0f)
			{
				this.TimerStop();
			}
		}
		Vector3 vector;
		Vector3 zero;
		if (this.active && Vector3.Distance(MonoSingleton<HookArm>.Instance.transform.position, base.transform.position) < 5f && !this.hooked)
		{
			vector = Vector3.one * 2.5f;
			zero = Vector3.zero;
		}
		else if (this.active && this.hooked)
		{
			vector = Vector3.zero;
			zero = this.innerOriginalScale;
		}
		else
		{
			vector = Vector3.one * 5f;
			zero = this.innerOriginalScale;
		}
		if (this.outerOrb.localScale != vector)
		{
			this.outerOrb.localScale = Vector3.MoveTowards(this.outerOrb.localScale, vector, Time.deltaTime * 50f);
		}
		if (this.innerOrb.localScale != zero)
		{
			this.innerOrb.localScale = Vector3.MoveTowards(this.innerOrb.localScale, zero, Time.deltaTime * 50f);
		}
		if (this.hooked)
		{
			this.aud.pitch = 0.75f;
			return;
		}
		this.aud.pitch = Mathf.Max(0.5f, this.outerOrb.localScale.x / 5f) / 2f;
	}

	// Token: 0x06000CF1 RID: 3313 RVA: 0x00062D98 File Offset: 0x00060F98
	public void Hooked()
	{
		if (!this.valuesSet)
		{
			this.SetValues();
		}
		this.hooked = true;
		Spin[] array = this.spins;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].speed = 450f;
		}
		this.lit.range = 20f;
		Object.Instantiate<GameObject>(this.grabParticle, base.transform.position, Quaternion.identity);
		this.onHook.Invoke("");
	}

	// Token: 0x06000CF2 RID: 3314 RVA: 0x00062E18 File Offset: 0x00061018
	public void Unhooked()
	{
		this.hooked = false;
		Spin[] array = this.spins;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].speed = 100f;
		}
		this.lit.range = 10f;
		this.onUnhook.Invoke("");
	}

	// Token: 0x06000CF3 RID: 3315 RVA: 0x00062E6E File Offset: 0x0006106E
	public void Reached()
	{
		this.Reached(Vector3.zero);
	}

	// Token: 0x06000CF4 RID: 3316 RVA: 0x00062E7C File Offset: 0x0006107C
	public void Reached(Vector3 direction)
	{
		UltrakillEvent ultrakillEvent = this.onReach;
		if (ultrakillEvent != null)
		{
			ultrakillEvent.Invoke("");
		}
		if (this.reachParticle)
		{
			if (direction == Vector3.zero)
			{
				direction = base.transform.position - MonoSingleton<CameraController>.Instance.transform.position;
			}
			Object.Instantiate<GameObject>(this.reachParticle, base.transform.position, Quaternion.LookRotation(direction));
		}
	}

	// Token: 0x06000CF5 RID: 3317 RVA: 0x00062EF7 File Offset: 0x000610F7
	public void SwitchPulled()
	{
		this.onReach.Invoke("");
		this.Deactivate();
		this.timer = this.reactivationTime;
		this.tickTimer = 0f;
	}

	// Token: 0x06000CF6 RID: 3318 RVA: 0x00062F26 File Offset: 0x00061126
	public void Activate()
	{
		if (!this.valuesSet)
		{
			this.SetValues();
		}
		if (!this.active)
		{
			this.TurnOn();
		}
	}

	// Token: 0x06000CF7 RID: 3319 RVA: 0x00062F44 File Offset: 0x00061144
	public void Deactivate()
	{
		if (!this.valuesSet)
		{
			this.SetValues();
		}
		if (this.active)
		{
			this.TurnOff();
		}
	}

	// Token: 0x06000CF8 RID: 3320 RVA: 0x00062F64 File Offset: 0x00061164
	private void TurnOn()
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].material = this.origMats[i];
		}
		this.activeParticle.Play();
		this.lit.enabled = true;
		this.aud.Play();
		Spin[] array = this.spins;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].enabled = true;
		}
		GameObject[] array2 = this.disableOnInactive;
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j].SetActive(true);
		}
		Object.Instantiate<GameObject>(this.grabParticle, base.transform.position, Quaternion.identity);
		this.active = true;
	}

	// Token: 0x06000CF9 RID: 3321 RVA: 0x0006301C File Offset: 0x0006121C
	private void TurnOff()
	{
		MeshRenderer[] array = this.renderers;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].material = this.disabledMaterial;
		}
		this.activeParticle.Stop();
		this.activeParticle.Clear();
		this.lit.enabled = false;
		this.aud.Stop();
		for (int j = 0; j < this.spins.Length; j++)
		{
			this.spins[j].enabled = false;
			this.spins[j].transform.localRotation = this.spinDefaultRotations[j];
		}
		GameObject[] array2 = this.disableOnInactive;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].SetActive(false);
		}
		this.active = false;
	}

	// Token: 0x06000CFA RID: 3322 RVA: 0x000630E0 File Offset: 0x000612E0
	private void SetValues()
	{
		this.origMats = new Material[this.renderers.Length];
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.origMats[i] = new Material(this.renderers[i].material);
		}
		this.spins = base.GetComponentsInChildren<Spin>();
		this.spinDefaultRotations = new Quaternion[this.spins.Length];
		for (int j = 0; j < this.spins.Length; j++)
		{
			this.spinDefaultRotations[j] = this.spins[j].transform.localRotation;
		}
		this.innerOriginalScale = this.innerOrb.localScale;
		this.lit = base.GetComponent<Light>();
		this.valuesSet = true;
	}

	// Token: 0x06000CFB RID: 3323 RVA: 0x000631A4 File Offset: 0x000613A4
	public void TimerStop()
	{
		this.timer = 0f;
		Object.Instantiate<AudioSource>(this.reactivationTick, base.transform.position, Quaternion.identity).pitch = 3f;
		this.onReach.Revert();
		this.Activate();
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06000CFC RID: 3324 RVA: 0x000631F2 File Offset: 0x000613F2
	public string alterKey
	{
		get
		{
			return "hook-point";
		}
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x06000CFD RID: 3325 RVA: 0x000631F9 File Offset: 0x000613F9
	public string alterCategoryName
	{
		get
		{
			return "Hook Point";
		}
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06000CFE RID: 3326 RVA: 0x00063200 File Offset: 0x00061400
	public AlterOption<float>[] options
	{
		get
		{
			if (this.type != hookPointType.Slingshot)
			{
				return null;
			}
			return new AlterOption<float>[]
			{
				new AlterOption<float>
				{
					key = "force",
					name = "Force",
					value = this.slingShotForce,
					callback = delegate(float value)
					{
						this.slingShotForce = value;
					},
					constraints = new SliderConstraints
					{
						step = 5f,
						min = -50f,
						max = 200f
					}
				}
			};
		}
	}

	// Token: 0x0400115D RID: 4445
	public bool active = true;

	// Token: 0x0400115E RID: 4446
	public hookPointType type;

	// Token: 0x0400115F RID: 4447
	public float slingShotForce;

	// Token: 0x04001160 RID: 4448
	[HideInInspector]
	public bool valuesSet;

	// Token: 0x04001161 RID: 4449
	public MeshRenderer[] renderers;

	// Token: 0x04001162 RID: 4450
	[HideInInspector]
	public Material[] origMats;

	// Token: 0x04001163 RID: 4451
	[HideInInspector]
	public Spin[] spins;

	// Token: 0x04001164 RID: 4452
	[HideInInspector]
	public Quaternion[] spinDefaultRotations;

	// Token: 0x04001165 RID: 4453
	[HideInInspector]
	public Light lit;

	// Token: 0x04001166 RID: 4454
	public Transform outerOrb;

	// Token: 0x04001167 RID: 4455
	public Transform innerOrb;

	// Token: 0x04001168 RID: 4456
	[HideInInspector]
	public Vector3 innerOriginalScale;

	// Token: 0x04001169 RID: 4457
	public Material disabledMaterial;

	// Token: 0x0400116A RID: 4458
	public ParticleSystem activeParticle;

	// Token: 0x0400116B RID: 4459
	public GameObject[] disableOnInactive;

	// Token: 0x0400116C RID: 4460
	private bool hooked;

	// Token: 0x0400116D RID: 4461
	private AudioSource aud;

	// Token: 0x0400116E RID: 4462
	public GameObject grabParticle;

	// Token: 0x0400116F RID: 4463
	public GameObject reachParticle;

	// Token: 0x04001170 RID: 4464
	public float reactivationTime = 6f;

	// Token: 0x04001171 RID: 4465
	[HideInInspector]
	public float timer;

	// Token: 0x04001172 RID: 4466
	private float tickTimer;

	// Token: 0x04001173 RID: 4467
	public AudioSource reactivationTick;

	// Token: 0x04001174 RID: 4468
	[Header("Events")]
	public UltrakillEvent onHook;

	// Token: 0x04001175 RID: 4469
	public UltrakillEvent onUnhook;

	// Token: 0x04001176 RID: 4470
	public UltrakillEvent onReach;
}
