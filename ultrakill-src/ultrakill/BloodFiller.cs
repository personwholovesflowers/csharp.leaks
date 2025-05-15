using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004A5 RID: 1189
[RequireComponent(typeof(Rigidbody))]
public class BloodFiller : MonoBehaviour
{
	// Token: 0x06001B60 RID: 7008 RVA: 0x000E363C File Offset: 0x000E183C
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
		this.rend = base.GetComponent<Renderer>();
		this.propBlock = new MaterialPropertyBlock();
		this.mf = base.GetComponent<MeshFilter>();
		this.col = base.GetComponent<Collider>();
		this.meshBounds = this.mf.mesh.bounds;
		Vector4 vector = this.meshBounds.size;
		Vector4 vector2 = this.meshBounds.center;
		vector2.w = 1f;
		this.propBlock.SetVector("_MeshScale", vector);
		this.propBlock.SetVector("_MeshCenter", vector2);
		this.propBlock.SetFloat("_FillAmount", this.fillAmount);
		this.rend.SetPropertyBlock(this.propBlock);
	}

	// Token: 0x06001B61 RID: 7009 RVA: 0x000E3711 File Offset: 0x000E1911
	private void OnEnable()
	{
		MonoSingleton<BloodsplatterManager>.Instance.bloodFillers.Add(base.gameObject);
		MonoSingleton<BloodsplatterManager>.Instance.hasBloodFillers = true;
	}

	// Token: 0x06001B62 RID: 7010 RVA: 0x000E3734 File Offset: 0x000E1934
	private void OnDisable()
	{
		if (!base.gameObject.scene.isLoaded)
		{
			return;
		}
		MonoSingleton<BloodsplatterManager>.Instance.bloodFillers.Remove(base.gameObject);
		if (MonoSingleton<BloodsplatterManager>.Instance.bloodFillers.Count == 0)
		{
			MonoSingleton<BloodsplatterManager>.Instance.hasBloodFillers = false;
		}
	}

	// Token: 0x06001B63 RID: 7011 RVA: 0x000E378C File Offset: 0x000E198C
	private void Update()
	{
		if (this.fillAmount > 0f && !this.fullyFilled)
		{
			this.heartBeatCooldown = Mathf.MoveTowards(this.heartBeatCooldown, 0f, Time.deltaTime * Mathf.Max(0.25f, 3f * this.fillAmount));
			if (this.heartBeatCooldown <= 0f)
			{
				this.aud.Play();
				this.aud.pitch = Mathf.Lerp(1f, 1.5f, this.fillAmount);
				this.heartBeatCooldown = 1f;
			}
		}
		if (this.eidCooldowns.Count > 0)
		{
			for (int i = this.eidCooldowns.Count - 1; i >= 0; i--)
			{
				this.eidCooldowns[i] = Mathf.MoveTowards(this.eidCooldowns[i], 0f, Time.deltaTime);
				if (this.eidCooldowns[i] == 0f)
				{
					this.eidCooldowns.RemoveAt(i);
					this.eidAmounts.RemoveAt(i);
					this.eids.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x06001B64 RID: 7012 RVA: 0x000E38A8 File Offset: 0x000E1AA8
	public void FillBloodSlider(float amount, Vector3 position, int eidID = 0)
	{
		if (!this.fullyFilled && (eidID == 0 || !this.eids.Contains(eidID) || this.eidAmounts[this.eids.IndexOf(eidID)] < amount))
		{
			if (this.bloodIngestParticle)
			{
				Vector3 vector = new Vector3(base.transform.position.x, position.y, base.transform.position.z);
				GameObject gameObject = Object.Instantiate<GameObject>(this.bloodIngestParticle, vector, Quaternion.LookRotation(position - vector));
				ParticleSystem componentInChildren = gameObject.GetComponentInChildren<ParticleSystem>();
				if (componentInChildren && componentInChildren.emission.burstCount > 0)
				{
					ParticleSystem.Burst burst = componentInChildren.emission.GetBurst(0);
					burst.count = new ParticleSystem.MinMaxCurve(Mathf.Max(3f, burst.count.constantMin * (amount / 50f)), Mathf.Max(3f, burst.count.constantMax * (amount / 50f)));
					componentInChildren.emission.SetBurst(0, burst);
				}
				AudioSource component = gameObject.GetComponent<AudioSource>();
				if (component)
				{
					component.pitch = Mathf.Lerp(3f, 2f, amount / 50f);
					component.volume = Mathf.Lerp(0.5f, 1f, amount / 50f);
				}
			}
			if (eidID != 0)
			{
				if (!this.eids.Contains(eidID))
				{
					this.eids.Add(eidID);
					this.eidCooldowns.Add(0.5f);
					this.eidAmounts.Add(amount);
				}
				else
				{
					int num = this.eids.IndexOf(eidID);
					amount -= this.eidAmounts[num];
					List<float> list = this.eidAmounts;
					int num2 = num;
					list[num2] += amount;
					this.eidCooldowns[num] = 0.5f;
				}
			}
			base.StartCoroutine(this.FillBlood(amount));
		}
	}

	// Token: 0x06001B65 RID: 7013 RVA: 0x000E3ABB File Offset: 0x000E1CBB
	private IEnumerator FillBlood(float amount)
	{
		Vector4 vector = this.meshBounds.size;
		Vector4 vector2 = this.meshBounds.center;
		this.propBlock.SetVector("_MeshScale", vector);
		this.propBlock.SetVector("_MeshCenter", vector2);
		this.rend.SetPropertyBlock(this.propBlock);
		float timer = 0f;
		float initialFillAmount = this.fillAmount;
		amount *= this.fillSpeed;
		while (timer <= this.fillTimePerHit)
		{
			float num = timer / this.fillTimePerHit;
			float num2 = this.fillAmount;
			float num3 = Mathf.Lerp(initialFillAmount, initialFillAmount + amount * 0.01f, num);
			this.fillAmount += Mathf.Clamp01(num3 - num2);
			this.propBlock.SetFloat("_FillAmount", this.fillAmount);
			this.rend.SetPropertyBlock(this.propBlock);
			timer += Time.deltaTime;
			if (this.fillAmount >= this.fullFillThreshold)
			{
				this.FullyFilled();
				timer = this.fillTimePerHit + 1f;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001B66 RID: 7014 RVA: 0x000E3AD4 File Offset: 0x000E1CD4
	private void FullyFilled()
	{
		this.fullyFilled = true;
		this.fillAmount = 1f;
		UltrakillEvent ultrakillEvent = this.onFullyFilled;
		if (ultrakillEvent != null)
		{
			ultrakillEvent.Invoke("");
		}
		MonoSingleton<CameraController>.Instance.CameraShake(1f);
		this.propBlock.SetFloat("_FillAmount", this.fillAmount);
		this.rend.SetPropertyBlock(this.propBlock);
	}

	// Token: 0x06001B67 RID: 7015 RVA: 0x000E3B3F File Offset: 0x000E1D3F
	public void InstaFill()
	{
		base.StartCoroutine(this.FillBlood(9999f));
	}

	// Token: 0x04002692 RID: 9874
	public float fullFillThreshold = 1f;

	// Token: 0x04002693 RID: 9875
	public float fillSpeed = 1f;

	// Token: 0x04002694 RID: 9876
	public float fillTimePerHit = 0.5f;

	// Token: 0x04002695 RID: 9877
	public float fillAmount;

	// Token: 0x04002696 RID: 9878
	[HideInInspector]
	public bool fullyFilled;

	// Token: 0x04002697 RID: 9879
	private AudioSource aud;

	// Token: 0x04002698 RID: 9880
	private Bounds meshBounds;

	// Token: 0x04002699 RID: 9881
	private Renderer rend;

	// Token: 0x0400269A RID: 9882
	private MaterialPropertyBlock propBlock;

	// Token: 0x0400269B RID: 9883
	private MeshFilter mf;

	// Token: 0x0400269C RID: 9884
	private Collider col;

	// Token: 0x0400269D RID: 9885
	public GameObject bloodIngestParticle;

	// Token: 0x0400269E RID: 9886
	private float heartBeatCooldown;

	// Token: 0x0400269F RID: 9887
	public UltrakillEvent onFullyFilled;

	// Token: 0x040026A0 RID: 9888
	private List<int> eids = new List<int>();

	// Token: 0x040026A1 RID: 9889
	private List<float> eidCooldowns = new List<float>();

	// Token: 0x040026A2 RID: 9890
	private List<float> eidAmounts = new List<float>();
}
