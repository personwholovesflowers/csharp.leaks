using System;
using UnityEngine;

// Token: 0x02000071 RID: 113
public class Beamgun : MonoBehaviour
{
	// Token: 0x0600021A RID: 538 RVA: 0x00004AE3 File Offset: 0x00002CE3
	private void Start()
	{
	}

	// Token: 0x0600021B RID: 539 RVA: 0x0000AE58 File Offset: 0x00009058
	private void Update()
	{
		if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed)
		{
			if (!this.beam.gameObject.activeSelf)
			{
				this.beam.gameObject.SetActive(true);
			}
			this.beam.fakeStartPoint = this.shootPoint.position;
			float num = Mathf.Clamp(MonoSingleton<NewMovement>.Instance.rb.velocity.magnitude / 5f, 0.1f, 0.25f);
			if (this.beam.beamWidth > num)
			{
				this.tempWidthCooldown = Mathf.MoveTowards(this.tempWidthCooldown, 1f, Time.deltaTime * 50f);
				if (this.tempWidthCooldown >= 1f)
				{
					this.beam.beamWidth = num;
				}
			}
			else
			{
				this.tempWidthCooldown = Mathf.MoveTowards(this.tempWidthCooldown, 0f, Time.deltaTime * 50f);
				this.beam.beamWidth = num;
			}
			this.beam.beamCheckSpeed = 1f + (num - 0.1f) * 5f;
		}
		else if (this.beam.gameObject.activeSelf)
		{
			this.beam.gameObject.SetActive(false);
		}
		if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame)
		{
			if (!this.currentBeamDrone)
			{
				this.currentBeamDrone = Object.Instantiate<GameObject>(this.beamDrone, base.transform.position + base.transform.forward, base.transform.rotation);
				return;
			}
			Object.Destroy(this.currentBeamDrone);
		}
	}

	// Token: 0x04000250 RID: 592
	[SerializeField]
	private Transform shootPoint;

	// Token: 0x04000251 RID: 593
	[SerializeField]
	private BeamgunBeam beam;

	// Token: 0x04000252 RID: 594
	[SerializeField]
	private GameObject beamDrone;

	// Token: 0x04000253 RID: 595
	private GameObject currentBeamDrone;

	// Token: 0x04000254 RID: 596
	private float tempWidthCooldown;
}
