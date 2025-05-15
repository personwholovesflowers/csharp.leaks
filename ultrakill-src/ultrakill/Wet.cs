using System;
using UnityEngine;

// Token: 0x020004CC RID: 1228
public class Wet : MonoBehaviour
{
	// Token: 0x06001C13 RID: 7187 RVA: 0x000E9248 File Offset: 0x000E7448
	private void Start()
	{
		this.wetness = 5f;
	}

	// Token: 0x06001C14 RID: 7188 RVA: 0x000E9258 File Offset: 0x000E7458
	private void Update()
	{
		if (this.drying)
		{
			if (this.wetness > 0f)
			{
				this.wetness = Mathf.MoveTowards(this.wetness, 0f, Time.deltaTime);
				return;
			}
			Flammable[] componentsInChildren = base.GetComponentsInChildren<Flammable>();
			if (componentsInChildren != null)
			{
				Flammable[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].wet = false;
				}
			}
			this.ReturnSoon();
		}
	}

	// Token: 0x06001C15 RID: 7189 RVA: 0x000E92C0 File Offset: 0x000E74C0
	public void Dry(Vector3 position)
	{
		this.drying = true;
		if (!this.wetParticle)
		{
			this.wetParticle = MonoSingleton<PooledWaterStore>.Instance.GetFromQueue(Water.WaterGOType.wetparticle).GetComponent<ParticleSystem>();
			this.wetParticle.transform.SetParent(base.transform, true);
			this.wetParticle.transform.localPosition = position;
		}
		this.wetParticle.Play();
	}

	// Token: 0x06001C16 RID: 7190 RVA: 0x000E932A File Offset: 0x000E752A
	public void Refill()
	{
		this.drying = false;
		this.wetness = 5f;
		if (this.wetParticle)
		{
			this.wetParticle.Stop();
		}
	}

	// Token: 0x06001C17 RID: 7191 RVA: 0x000E9356 File Offset: 0x000E7556
	private void ReturnSoon()
	{
		if (this.wetParticle)
		{
			this.wetParticle.Stop();
		}
		base.Invoke("ReturnWetParticle", 1f);
	}

	// Token: 0x06001C18 RID: 7192 RVA: 0x000E9380 File Offset: 0x000E7580
	private void ReturnWetParticle()
	{
		if (this.wetParticle)
		{
			MonoSingleton<PooledWaterStore>.Instance.ReturnToQueue(this.wetParticle.gameObject, Water.WaterGOType.wetparticle);
			this.wetParticle = null;
		}
	}

	// Token: 0x0400279C RID: 10140
	private ParticleSystem wetParticle;

	// Token: 0x0400279D RID: 10141
	public float wetness;

	// Token: 0x0400279E RID: 10142
	private bool drying;
}
