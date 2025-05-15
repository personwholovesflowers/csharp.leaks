using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000307 RID: 775
public class MovementParticleTrigger : MonoBehaviour
{
	// Token: 0x06001195 RID: 4501 RVA: 0x000892B0 File Offset: 0x000874B0
	private void Awake()
	{
		this.colliders = base.GetComponentsInChildren<Collider>(true);
	}

	// Token: 0x06001196 RID: 4502 RVA: 0x000892BF File Offset: 0x000874BF
	private void OnCollisionEnter(Collision collision)
	{
		this.Enter(collision.collider);
	}

	// Token: 0x06001197 RID: 4503 RVA: 0x000892CD File Offset: 0x000874CD
	private void OnTriggerEnter(Collider other)
	{
		this.Enter(other);
	}

	// Token: 0x06001198 RID: 4504 RVA: 0x000892D8 File Offset: 0x000874D8
	private void Enter(Collider other)
	{
		if (other.gameObject == MonoSingleton<NewMovement>.Instance.gameObject || other.gameObject.layer == 12)
		{
			EntererTracker entererTracker = this.IsTracked(other.gameObject);
			if (entererTracker == null)
			{
				entererTracker = new EntererTracker(other.gameObject, other.gameObject.transform.position);
				this.enterers.Add(entererTracker);
				Object.Instantiate<GameObject>(this.particle, this.GetClosestPointOnTrigger(other.gameObject.transform.position), Quaternion.identity);
			}
			entererTracker.amount++;
		}
	}

	// Token: 0x06001199 RID: 4505 RVA: 0x00089378 File Offset: 0x00087578
	private void OnCollisionExit(Collision collision)
	{
		this.Exit(collision.collider);
	}

	// Token: 0x0600119A RID: 4506 RVA: 0x00089386 File Offset: 0x00087586
	private void OnTriggerExit(Collider other)
	{
		this.Exit(other);
	}

	// Token: 0x0600119B RID: 4507 RVA: 0x00089390 File Offset: 0x00087590
	private void Exit(Collider other)
	{
		EntererTracker entererTracker = this.IsTracked(other.gameObject);
		if (entererTracker == null)
		{
			return;
		}
		entererTracker.amount--;
		if (entererTracker.amount == 0)
		{
			this.enterers.Remove(entererTracker);
		}
	}

	// Token: 0x0600119C RID: 4508 RVA: 0x000893D1 File Offset: 0x000875D1
	private void OnDisable()
	{
		this.enterers.Clear();
	}

	// Token: 0x0600119D RID: 4509 RVA: 0x000893E0 File Offset: 0x000875E0
	private void Update()
	{
		if (this.enterers == null || this.enterers.Count == 0)
		{
			return;
		}
		for (int i = this.enterers.Count - 1; i >= 0; i--)
		{
			if (this.enterers[i] == null || this.enterers[i].target == null || !this.enterers[i].target.activeInHierarchy)
			{
				this.enterers.RemoveAt(i);
			}
			else
			{
				Transform transform = this.enterers[i].target.transform;
				float num = Vector3.Distance(this.enterers[i].position, transform.position);
				if (num > this.distancePerParticle)
				{
					Object.Instantiate<GameObject>(this.particle, this.GetClosestPointOnTrigger(transform.position), Quaternion.identity);
					if (num > this.distancePerParticle * 2f)
					{
						Vector3 normalized = (this.enterers[i].position - transform.position).normalized;
						while (num > this.distancePerParticle)
						{
							num -= this.distancePerParticle;
							transform.position + normalized * num;
						}
					}
					this.enterers[i].position = transform.position;
				}
			}
		}
	}

	// Token: 0x0600119E RID: 4510 RVA: 0x00089544 File Offset: 0x00087744
	private EntererTracker IsTracked(GameObject gob)
	{
		EntererTracker entererTracker = null;
		for (int i = 0; i < this.enterers.Count; i++)
		{
			if (this.enterers[i].target == gob)
			{
				entererTracker = this.enterers[i];
			}
		}
		return entererTracker;
	}

	// Token: 0x0600119F RID: 4511 RVA: 0x00089590 File Offset: 0x00087790
	private Vector3 GetClosestPointOnTrigger(Vector3 position)
	{
		if (this.colliders.Length == 0)
		{
			return Vector3.zero;
		}
		if (this.colliders.Length == 1)
		{
			if (!(this.colliders[0] == null))
			{
				return this.colliders[0].ClosestPoint(position);
			}
			return Vector3.zero;
		}
		else
		{
			Vector3 vector = position + Vector3.one * 100f;
			for (int i = 0; i < this.colliders.Length; i++)
			{
				if (!(this.colliders[i] == null) && this.colliders[i].enabled && this.colliders[i].gameObject.activeInHierarchy)
				{
					Vector3 vector2 = this.colliders[i].ClosestPoint(position);
					if (vector2 == position)
					{
						return vector2;
					}
					if (Vector3.Distance(position, vector) > Vector3.Distance(position, vector2))
					{
						vector = vector2;
					}
				}
			}
			if (vector == position + Vector3.one * 100f)
			{
				return position;
			}
			return vector;
		}
	}

	// Token: 0x040017FA RID: 6138
	public GameObject particle;

	// Token: 0x040017FB RID: 6139
	public float distancePerParticle = 3f;

	// Token: 0x040017FC RID: 6140
	private Collider[] colliders;

	// Token: 0x040017FD RID: 6141
	[HideInInspector]
	public List<EntererTracker> enterers = new List<EntererTracker>();
}
