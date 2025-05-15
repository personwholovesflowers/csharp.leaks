using System;
using UnityEngine;

// Token: 0x02000390 RID: 912
public class RevolverCylinder : MonoBehaviour
{
	// Token: 0x060014FA RID: 5370 RVA: 0x000AB520 File Offset: 0x000A9720
	private void Start()
	{
		this.aud = base.GetComponent<AudioSource>();
		this.currentRotation = base.transform.localRotation;
		this.allRotations = new Quaternion[this.bulletAmount];
		for (int i = 0; i < this.bulletAmount; i++)
		{
			this.allRotations[i] = base.transform.localRotation * Quaternion.Euler(this.rotationAxis * (float)(360 / this.bulletAmount) * (float)i);
		}
	}

	// Token: 0x060014FB RID: 5371 RVA: 0x000AB5AC File Offset: 0x000A97AC
	private void LateUpdate()
	{
		if (this.spinSpeed * 10f > this.speed)
		{
			this.freeSpinning = true;
			this.currentRotation *= Quaternion.Euler(this.rotationAxis * Time.deltaTime * this.spinSpeed * 10f);
		}
		else if (this.freeSpinning)
		{
			this.freeSpinning = false;
			this.target = this.GetClosestTarget();
		}
		if (!this.freeSpinning && Quaternion.Angle(this.currentRotation, this.allRotations[this.target]) > 0.1f)
		{
			this.currentRotation = Quaternion.RotateTowards(this.currentRotation, this.allRotations[this.target], Time.deltaTime * this.speed);
			if (Quaternion.Angle(this.currentRotation, this.allRotations[this.target]) <= 0.1f)
			{
				this.currentRotation = this.allRotations[this.target];
				AudioSource audioSource = this.aud;
				if (audioSource != null)
				{
					audioSource.Play();
				}
			}
		}
		base.transform.localRotation = this.currentRotation;
	}

	// Token: 0x060014FC RID: 5372 RVA: 0x000AB6E4 File Offset: 0x000A98E4
	public void DoTurn()
	{
		this.target++;
		if (this.target >= this.allRotations.Length)
		{
			this.target = 0;
		}
	}

	// Token: 0x060014FD RID: 5373 RVA: 0x000AB70C File Offset: 0x000A990C
	private int GetClosestTarget()
	{
		int num = 0;
		float num2 = Quaternion.Angle(this.currentRotation, this.allRotations[0]);
		for (int i = 1; i < this.bulletAmount; i++)
		{
			float num3 = Quaternion.Angle(this.currentRotation, this.allRotations[i]);
			if (num3 < num2)
			{
				num2 = num3;
				num = i;
			}
		}
		return num;
	}

	// Token: 0x04001D1A RID: 7450
	public int bulletAmount = 6;

	// Token: 0x04001D1B RID: 7451
	public Vector3 rotationAxis;

	// Token: 0x04001D1C RID: 7452
	public float speed;

	// Token: 0x04001D1D RID: 7453
	private AudioSource aud;

	// Token: 0x04001D1E RID: 7454
	private int target;

	// Token: 0x04001D1F RID: 7455
	private Quaternion currentRotation;

	// Token: 0x04001D20 RID: 7456
	private Quaternion[] allRotations;

	// Token: 0x04001D21 RID: 7457
	private bool freeSpinning;

	// Token: 0x04001D22 RID: 7458
	[HideInInspector]
	public float spinSpeed;
}
