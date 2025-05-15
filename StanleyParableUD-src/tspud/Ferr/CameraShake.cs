using System;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002DF RID: 735
	public class CameraShake : MonoBehaviour
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06001328 RID: 4904 RVA: 0x0006615B File Offset: 0x0006435B
		private static CameraShake Instance
		{
			get
			{
				if (CameraShake.instance == null)
				{
					CameraShake.instance = CameraShake.Create();
				}
				return CameraShake.instance;
			}
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x00003C1D File Offset: 0x00001E1D
		private CameraShake()
		{
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x00066179 File Offset: 0x00064379
		private static CameraShake Create()
		{
			return Camera.main.gameObject.AddComponent<CameraShake>();
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x0006618C File Offset: 0x0006438C
		private void LateUpdate()
		{
			float num = (Time.time - this.start) / this.duration;
			if (num <= 1f)
			{
				base.transform.position -= this.offset;
				this.offset = new Vector3(Random.Range(-this.magnitude.x, this.magnitude.x), Random.Range(-this.magnitude.y, this.magnitude.y), Random.Range(-this.magnitude.z, this.magnitude.z)) * this.curve.Evaluate(num);
				base.transform.position += this.offset;
				return;
			}
			base.transform.position -= this.offset;
			this.offset = Vector2.zero;
			base.enabled = false;
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x00066294 File Offset: 0x00064494
		public static void Shake(Vector3 aMagnitude, float aDuration)
		{
			CameraShake.Instance.magnitude = aMagnitude;
			CameraShake.Instance.duration = aDuration;
			CameraShake.Instance.start = Time.time;
			CameraShake.Instance.transform.position -= CameraShake.Instance.offset;
			CameraShake.Instance.offset = Vector3.zero;
			CameraShake.Instance.enabled = true;
			CameraShake.Instance.curve = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 1f),
				new Keyframe(1f, 0f)
			});
		}

		// Token: 0x04000F08 RID: 3848
		private static CameraShake instance;

		// Token: 0x04000F09 RID: 3849
		private Vector3 magnitude;

		// Token: 0x04000F0A RID: 3850
		private float duration;

		// Token: 0x04000F0B RID: 3851
		private float start;

		// Token: 0x04000F0C RID: 3852
		private Vector3 offset;

		// Token: 0x04000F0D RID: 3853
		private AnimationCurve curve;
	}
}
