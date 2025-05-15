using System;
using JigglePhysics;
using UnityEngine;

// Token: 0x02000376 RID: 886
public class RandomizeWind : MonoBehaviour
{
	// Token: 0x06001494 RID: 5268 RVA: 0x000A6B76 File Offset: 0x000A4D76
	private void Start()
	{
		this.rig = base.GetComponent<JiggleRigBuilder>();
		this.initialDirection = this.rig.wind.normalized;
		this.initialStrength = this.rig.wind.magnitude;
	}

	// Token: 0x06001495 RID: 5269 RVA: 0x000A6BB0 File Offset: 0x000A4DB0
	private void Update()
	{
		this.elapsedTime += Time.deltaTime;
		if (this.elapsedTime >= this.waitTime)
		{
			this.Randomize();
		}
	}

	// Token: 0x06001496 RID: 5270 RVA: 0x000A6BD8 File Offset: 0x000A4DD8
	private void Randomize()
	{
		this.elapsedTime = 0f;
		this.waitTime = Random.Range(1f, 5f);
		Vector3 vector = (Random.rotation.eulerAngles - new Vector3(180f, 180f, 180f)).normalized;
		MonoBehaviour.print(vector);
		vector = this.initialDirection + Vector3.Scale(vector, this.randomizeDirectionStrength) * this.initialStrength;
		this.rig.wind = vector;
	}

	// Token: 0x04001C56 RID: 7254
	public Vector3 randomizeDirectionStrength = Vector3.one;

	// Token: 0x04001C57 RID: 7255
	private float initialStrength;

	// Token: 0x04001C58 RID: 7256
	private Vector3 initialDirection;

	// Token: 0x04001C59 RID: 7257
	private JiggleRigBuilder rig;

	// Token: 0x04001C5A RID: 7258
	private float waitTime;

	// Token: 0x04001C5B RID: 7259
	private float elapsedTime;
}
