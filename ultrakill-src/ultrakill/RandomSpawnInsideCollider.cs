using System;
using UnityEngine;

// Token: 0x0200037C RID: 892
public class RandomSpawnInsideCollider : MonoBehaviour
{
	// Token: 0x060014A9 RID: 5289 RVA: 0x000A7137 File Offset: 0x000A5337
	private void Start()
	{
		this.boxCollider = base.GetComponent<BoxCollider>();
	}

	// Token: 0x060014AA RID: 5290 RVA: 0x000A7145 File Offset: 0x000A5345
	private void Update()
	{
		this.cooldown = Mathf.MoveTowards(this.cooldown, 0f, Time.deltaTime);
		if (this.cooldown <= 0f)
		{
			this.Spawn();
		}
	}

	// Token: 0x060014AB RID: 5291 RVA: 0x000A7178 File Offset: 0x000A5378
	private void Spawn()
	{
		if (this.activated)
		{
			if (this.oneTime)
			{
				base.enabled = false;
				return;
			}
		}
		else
		{
			this.activated = true;
		}
		Vector3 vector = this.boxCollider.size / 2f;
		Vector3 vector2 = new Vector3(Random.Range(-vector.x, vector.x), Random.Range(-vector.y, vector.y), Random.Range(-vector.z, vector.z)) + this.boxCollider.center;
		Object.Instantiate<GameObject>(this.spawnedObject, this.boxCollider.transform.TransformPoint(vector2), Random.rotation);
		this.cooldown = this.delay;
		if (this.oneTime)
		{
			base.enabled = false;
		}
	}

	// Token: 0x04001C75 RID: 7285
	public GameObject spawnedObject;

	// Token: 0x04001C76 RID: 7286
	private BoxCollider boxCollider;

	// Token: 0x04001C77 RID: 7287
	public float delay;

	// Token: 0x04001C78 RID: 7288
	private float cooldown;

	// Token: 0x04001C79 RID: 7289
	public bool oneTime;

	// Token: 0x04001C7A RID: 7290
	private bool activated;
}
