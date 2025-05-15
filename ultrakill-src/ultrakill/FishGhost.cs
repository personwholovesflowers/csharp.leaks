using System;
using UnityEngine;

// Token: 0x020001E1 RID: 481
public class FishGhost : MonoBehaviour
{
	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x060009BE RID: 2494 RVA: 0x00043887 File Offset: 0x00041A87
	public float tiredness
	{
		get
		{
			return 1f - Mathf.Clamp01(this.timeSinceDifficultAction);
		}
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x000438A0 File Offset: 0x00041AA0
	private void RollTheDice()
	{
		this.speed = Random.Range(0.1f, 7f);
		this.turnSpeed = Random.Range(4f, 50f);
		this.direction = ((Random.value > 0.5f) ? 1f : (-1f));
		this.directionChangeTendency = Random.Range(0.0001f, 0.02f);
		this.jitter = Random.Range(0.1f, 1.5f);
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x00043920 File Offset: 0x00041B20
	private void Start()
	{
		base.transform.rotation = Quaternion.Euler(0f, (float)Random.Range(0, 360), 0f);
		this.indecisiveness = Random.Range(0.003f, 0.1f);
		this.timeSinceLogicTick = 0f;
		this.RollTheDice();
		this.constraints = base.GetComponent<FishConstraints>();
		if (this.constraints == null && base.transform.parent != null)
		{
			this.constraints = base.transform.parent.GetComponent<FishConstraints>();
		}
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x000439C4 File Offset: 0x00041BC4
	private void FixedUpdate()
	{
		if (Random.value < this.indecisiveness)
		{
			this.RollTheDice();
		}
		if (Random.value < this.directionChangeTendency)
		{
			this.direction *= -1f;
		}
		if (this.scared && this.timeSinceSpook > 3f)
		{
			this.scared = false;
		}
		if (this.timeSinceLogicTick > 3f + this.jitter)
		{
			RaycastHit raycastHit;
			bool flag = Physics.Raycast(base.transform.position, -base.transform.forward, out raycastHit, 10f, LayerMaskDefaults.Get(LMD.Environment) | 65536, QueryTriggerInteraction.Collide);
			Vector3 position = MonoSingleton<NewMovement>.Instance.transform.position;
			bool flag2 = this.constraints == null || this.constraints.area.Contains(base.transform.position);
			if ((this.tiredness < 0.5f && Vector3.Distance(position, base.transform.position) < 15f) || flag || !flag2)
			{
				if (flag)
				{
					base.transform.rotation = Quaternion.LookRotation(raycastHit.point - base.transform.position);
				}
				if (!flag2)
				{
					Vector3 center = this.constraints.area.center;
					base.transform.rotation = Quaternion.LookRotation(base.transform.position - center);
				}
				base.transform.rotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
				this.scared = true;
				this.timeSinceSpook = 0f;
				this.timeSinceDifficultAction = 0f;
			}
		}
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x00043BA4 File Offset: 0x00041DA4
	private void Update()
	{
		base.transform.Translate(-Vector3.forward * Time.deltaTime * this.speed * (this.scared ? 4f : (1f * (1f - this.tiredness))));
		base.transform.Rotate(Vector3.up * Time.deltaTime * this.turnSpeed * this.direction * (1f - this.tiredness));
	}

	// Token: 0x04000CAC RID: 3244
	private float speed = 1f;

	// Token: 0x04000CAD RID: 3245
	private float turnSpeed = 1f;

	// Token: 0x04000CAE RID: 3246
	private float direction = 1f;

	// Token: 0x04000CAF RID: 3247
	private float directionChangeTendency;

	// Token: 0x04000CB0 RID: 3248
	private float jitter;

	// Token: 0x04000CB1 RID: 3249
	private float indecisiveness;

	// Token: 0x04000CB2 RID: 3250
	private TimeSince timeSinceLogicTick;

	// Token: 0x04000CB3 RID: 3251
	private bool scared;

	// Token: 0x04000CB4 RID: 3252
	private TimeSince timeSinceSpook;

	// Token: 0x04000CB5 RID: 3253
	private TimeSince timeSinceDifficultAction;

	// Token: 0x04000CB6 RID: 3254
	private FishConstraints constraints;
}
