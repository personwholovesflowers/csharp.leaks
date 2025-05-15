using System;
using UnityEngine;

namespace TFHC_ForceShield_Shader_Sample
{
	// Token: 0x020002FD RID: 765
	public class ForceShieldDestroyBall : MonoBehaviour
	{
		// Token: 0x060013E1 RID: 5089 RVA: 0x00069280 File Offset: 0x00067480
		private void Start()
		{
			Object.Destroy(base.gameObject, this.lifetime);
		}

		// Token: 0x04000F8B RID: 3979
		public float lifetime = 5f;
	}
}
