using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000437 RID: 1079
public class SplashContinuous : MonoBehaviour
{
	// Token: 0x0600184E RID: 6222 RVA: 0x000C6860 File Offset: 0x000C4A60
	private void Start()
	{
		this.waterStore = MonoSingleton<PooledWaterStore>.Instance;
	}

	// Token: 0x0600184F RID: 6223 RVA: 0x000C686D File Offset: 0x000C4A6D
	private void OnEnable()
	{
		this.active = true;
	}

	// Token: 0x06001850 RID: 6224 RVA: 0x000C6878 File Offset: 0x000C4A78
	private void FixedUpdate()
	{
		if (!this.active)
		{
			return;
		}
		ParticleSystem.EmissionModule emission = this.particles.emission;
		if ((this.nma && this.nma.velocity.magnitude > 4f) || Vector3.Distance(base.transform.position, this.previousPosition) > 0.05f)
		{
			emission.rateOverTime = this.movingEmissionRate;
			if (this.cooldown == 0f)
			{
				AudioSource audioSource;
				if (Object.Instantiate<GameObject>(this.wadingSound, base.transform).TryGetComponent<AudioSource>(out audioSource))
				{
					audioSource.clip = this.wadingSounds[Random.Range(0, this.wadingSounds.Length)];
					audioSource.pitch = Random.Range(this.wadingSoundPitch - 0.05f, this.wadingSoundPitch + 0.05f);
					audioSource.Play();
				}
				this.cooldown = 0.75f;
			}
		}
		else
		{
			emission.rateOverTime = this.stillEmissionRate;
		}
		this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.fixedDeltaTime * (1f + Vector3.Distance(base.transform.position, this.previousPosition) * 5f));
		this.previousPosition = base.transform.position;
	}

	// Token: 0x06001851 RID: 6225 RVA: 0x000C69CC File Offset: 0x000C4BCC
	public void ReturnSoon()
	{
		this.particles.Stop();
		this.active = false;
		base.Invoke("ReturnToQueue", 2f);
	}

	// Token: 0x06001852 RID: 6226 RVA: 0x000C69F0 File Offset: 0x000C4BF0
	private void ReturnToQueue()
	{
		if (this.waterGOType == Water.WaterGOType.continuous && this.waterStore)
		{
			this.waterStore.ReturnToQueue(base.gameObject, this.waterGOType);
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0400221A RID: 8730
	private bool active = true;

	// Token: 0x0400221B RID: 8731
	private float cooldown;

	// Token: 0x0400221C RID: 8732
	[SerializeField]
	private Water.WaterGOType waterGOType;

	// Token: 0x0400221D RID: 8733
	[SerializeField]
	private ParticleSystem particles;

	// Token: 0x0400221E RID: 8734
	[SerializeField]
	private GameObject wadingSound;

	// Token: 0x0400221F RID: 8735
	[SerializeField]
	private AudioClip[] wadingSounds;

	// Token: 0x04002220 RID: 8736
	[SerializeField]
	private float wadingSoundPitch = 0.8f;

	// Token: 0x04002221 RID: 8737
	private Vector3 previousPosition;

	// Token: 0x04002222 RID: 8738
	[SerializeField]
	private float movingEmissionRate = 20f;

	// Token: 0x04002223 RID: 8739
	[SerializeField]
	private float stillEmissionRate = 2f;

	// Token: 0x04002224 RID: 8740
	[HideInInspector]
	public NavMeshAgent nma;

	// Token: 0x04002225 RID: 8741
	private PooledWaterStore waterStore;
}
