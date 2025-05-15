using System;
using UnityEngine;

namespace TFHC_ForceShield_Shader_Sample
{
	// Token: 0x020002FE RID: 766
	public class ForceShieldImpactDetection : MonoBehaviour
	{
		// Token: 0x060013E3 RID: 5091 RVA: 0x000692A6 File Offset: 0x000674A6
		private void Start()
		{
			this.mat = base.GetComponent<Renderer>().material;
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x000692BC File Offset: 0x000674BC
		private void Update()
		{
			if (this.hitTime > 0f)
			{
				this.hitTime -= Time.deltaTime * 1000f;
				if (this.hitTime < 0f)
				{
					this.hitTime = 0f;
				}
				this.mat.SetFloat("_HitTime", this.hitTime);
			}
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0006931C File Offset: 0x0006751C
		private void OnCollisionEnter(Collision collision)
		{
			foreach (ContactPoint contactPoint in collision.contacts)
			{
				this.mat.SetVector("_HitPosition", base.transform.InverseTransformPoint(contactPoint.point));
				this.hitTime = 500f;
				this.mat.SetFloat("_HitTime", this.hitTime);
			}
		}

		// Token: 0x04000F8C RID: 3980
		private float hitTime;

		// Token: 0x04000F8D RID: 3981
		private Material mat;
	}
}
