using System;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002EA RID: 746
	[Serializable]
	public class JuiceData
	{
		// Token: 0x06001366 RID: 4966 RVA: 0x00067470 File Offset: 0x00065670
		public bool Update()
		{
			if (this.transform == null)
			{
				return true;
			}
			float num = Mathf.Min((Time.time - this.startTime) / this.duration, 1f);
			float num2 = this.start + this.curve.Evaluate(num) * (this.end - this.start);
			if (this.relative)
			{
				float num3 = Mathf.Max(0f, Mathf.Min((Time.time - Time.deltaTime - this.startTime) / this.duration, 1f));
				num2 -= this.start + this.curve.Evaluate(num3) * (this.end - this.start);
			}
			JuiceType juiceType = this.type;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			Vector3 vector = (this.relative ? Vector3.zero : this.transform.localPosition);
			Vector3 vector2 = (this.relative ? Vector3.zero : this.transform.localScale);
			Vector3 vector3 = (this.relative ? Vector3.zero : this.transform.eulerAngles);
			if ((juiceType & JuiceType.TranslateX) > (JuiceType)0)
			{
				vector.x = num2;
				flag = true;
			}
			if ((juiceType & JuiceType.TranslateY) > (JuiceType)0)
			{
				vector.y = num2;
				flag = true;
			}
			if ((juiceType & JuiceType.TranslateZ) > (JuiceType)0)
			{
				vector.z = num2;
				flag = true;
			}
			if ((juiceType & JuiceType.ScaleX) > (JuiceType)0)
			{
				vector2.x = num2;
				flag2 = true;
			}
			if ((juiceType & JuiceType.ScaleY) > (JuiceType)0)
			{
				vector2.y = num2;
				flag2 = true;
			}
			if ((juiceType & JuiceType.ScaleZ) > (JuiceType)0)
			{
				vector2.z = num2;
				flag2 = true;
			}
			if ((juiceType & JuiceType.RotationX) > (JuiceType)0)
			{
				vector3.x = num2;
				flag3 = true;
			}
			if ((juiceType & JuiceType.RotationY) > (JuiceType)0)
			{
				vector3.y = num2;
				flag3 = true;
			}
			if ((juiceType & JuiceType.RotationZ) > (JuiceType)0)
			{
				vector3.z = num2;
				flag3 = true;
			}
			if (flag && this.relative)
			{
				this.transform.localPosition += vector;
			}
			else if (flag)
			{
				this.transform.localPosition = vector;
			}
			if (flag2 && this.relative)
			{
				this.transform.localScale += vector2;
			}
			else if (flag2)
			{
				this.transform.localScale = vector2;
			}
			if (flag3 && this.relative)
			{
				this.transform.localEulerAngles += vector3;
			}
			else if (flag3)
			{
				this.transform.localEulerAngles = vector3;
			}
			return num >= 1f;
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x000676D3 File Offset: 0x000658D3
		public void Cancel()
		{
			this.startTime = -10000f;
			this.Update();
		}

		// Token: 0x04000F2A RID: 3882
		public JuiceType type;

		// Token: 0x04000F2B RID: 3883
		public Transform transform;

		// Token: 0x04000F2C RID: 3884
		public float start;

		// Token: 0x04000F2D RID: 3885
		public float end;

		// Token: 0x04000F2E RID: 3886
		public float duration;

		// Token: 0x04000F2F RID: 3887
		public float startTime;

		// Token: 0x04000F30 RID: 3888
		public bool relative;

		// Token: 0x04000F31 RID: 3889
		public AnimationCurve curve;

		// Token: 0x04000F32 RID: 3890
		public Action callback;
	}
}
