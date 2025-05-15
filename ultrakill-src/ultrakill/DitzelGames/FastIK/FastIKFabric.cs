using System;
using UnityEngine;

namespace DitzelGames.FastIK
{
	// Token: 0x020005E1 RID: 1505
	[DefaultExecutionOrder(2147483647)]
	public class FastIKFabric : MonoBehaviour
	{
		// Token: 0x060021B9 RID: 8633 RVA: 0x0010A753 File Offset: 0x00108953
		private void Awake()
		{
			this.Init();
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x0010A75C File Offset: 0x0010895C
		private void Init()
		{
			this.bones = new Transform[this.chainLength + 1];
			this.positions = new Vector3[this.chainLength + 1];
			this.bonesLength = new float[this.chainLength];
			this.startDirectionSucc = new Vector3[this.chainLength + 1];
			this.startRotationBone = new Quaternion[this.chainLength + 1];
			this.root = base.transform;
			for (int i = 0; i <= this.chainLength; i++)
			{
				if (this.root == null)
				{
					throw new UnityException("The chain value is longer than the ancestor chain!");
				}
				this.root = this.root.parent;
			}
			if (this.target == null)
			{
				this.target = new GameObject(base.gameObject.name + " Target").transform;
				this.SetPositionRootSpace(this.target, this.GetPositionRootSpace(base.transform));
			}
			this.startRotationTarget = this.GetRotationRootSpace(this.target);
			Transform transform = base.transform;
			this.completeLength = 0f;
			for (int j = this.bones.Length - 1; j >= 0; j--)
			{
				this.bones[j] = transform;
				this.startRotationBone[j] = this.GetRotationRootSpace(transform);
				if (j == this.bones.Length - 1)
				{
					this.startDirectionSucc[j] = this.GetPositionRootSpace(this.target) - this.GetPositionRootSpace(transform);
				}
				else
				{
					this.startDirectionSucc[j] = this.GetPositionRootSpace(this.bones[j + 1]) - this.GetPositionRootSpace(transform);
					this.bonesLength[j] = this.startDirectionSucc[j].magnitude;
					this.completeLength += this.bonesLength[j];
				}
				transform = transform.parent;
			}
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x0010A943 File Offset: 0x00108B43
		private void LateUpdate()
		{
			this.ResolveIK();
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x0010A94C File Offset: 0x00108B4C
		private void ResolveIK()
		{
			if (this.target == null)
			{
				return;
			}
			if (this.bonesLength.Length != this.chainLength)
			{
				this.Init();
			}
			for (int i = 0; i < this.bones.Length; i++)
			{
				this.positions[i] = this.GetPositionRootSpace(this.bones[i]);
			}
			Vector3 positionRootSpace = this.GetPositionRootSpace(this.target);
			this.GetRotationRootSpace(this.target);
			float num = Vector3.Distance(positionRootSpace, this.GetPositionRootSpace(this.bones[0]));
			if ((positionRootSpace - this.GetPositionRootSpace(this.bones[0])).sqrMagnitude >= this.completeLength * this.completeLength)
			{
				Vector3 normalized = (positionRootSpace - this.positions[0]).normalized;
				for (int j = 1; j < this.positions.Length; j++)
				{
					this.positions[j] = this.positions[j - 1] + normalized * this.bonesLength[j - 1];
				}
			}
			else
			{
				for (int k = 0; k < this.positions.Length - 1; k++)
				{
					this.positions[k + 1] = Vector3.Lerp(this.positions[k + 1], this.positions[k] + this.startDirectionSucc[k], this.snapBackStrength);
				}
				for (int l = 0; l < this.iterations; l++)
				{
					for (int m = this.positions.Length - 1; m > 0; m--)
					{
						if (m == this.positions.Length - 1)
						{
							this.positions[m] = positionRootSpace;
						}
						else
						{
							this.positions[m] = this.positions[m + 1] + (this.positions[m] - this.positions[m + 1]).normalized * (num / (float)this.positions.Length);
						}
					}
					for (int n = 1; n < this.positions.Length; n++)
					{
						this.positions[n] = this.positions[n - 1] + (this.positions[n] - this.positions[n - 1]).normalized * (num / (float)this.positions.Length);
					}
					if ((this.positions[this.positions.Length - 1] - positionRootSpace).sqrMagnitude < this.delta * this.delta)
					{
						break;
					}
				}
			}
			if (this.pole != null)
			{
				Vector3 positionRootSpace2 = this.GetPositionRootSpace(this.pole);
				for (int num2 = 1; num2 < this.positions.Length - 1; num2++)
				{
					Plane plane = new Plane(this.positions[num2 + 1] - this.positions[num2 - 1], this.positions[num2 - 1]);
					Vector3 vector = plane.ClosestPointOnPlane(positionRootSpace2);
					float num3 = Vector3.SignedAngle(plane.ClosestPointOnPlane(this.positions[num2]) - this.positions[num2 - 1], vector - this.positions[num2 - 1], plane.normal);
					this.positions[num2] = Quaternion.AngleAxis(num3, plane.normal) * (this.positions[num2] - this.positions[num2 - 1]) + this.positions[num2 - 1];
				}
			}
			for (int num4 = 0; num4 < this.positions.Length; num4++)
			{
				this.SetPositionRootSpace(this.bones[num4], this.positions[num4]);
			}
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x0010AD7C File Offset: 0x00108F7C
		private Vector3 GetPositionRootSpace(Transform current)
		{
			if (this.root == null)
			{
				return current.position;
			}
			return Quaternion.Inverse(this.root.rotation) * (current.position - this.root.position);
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x0010ADC9 File Offset: 0x00108FC9
		private void SetPositionRootSpace(Transform current, Vector3 position)
		{
			if (this.root == null)
			{
				current.position = position;
				return;
			}
			current.position = this.root.rotation * position + this.root.position;
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x0010AE08 File Offset: 0x00109008
		private Quaternion GetRotationRootSpace(Transform current)
		{
			if (this.root == null)
			{
				return current.rotation;
			}
			return Quaternion.Inverse(current.rotation) * this.root.rotation;
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x0010AE3A File Offset: 0x0010903A
		private void SetRotationRootSpace(Transform current, Quaternion rotation)
		{
			if (this.root == null)
			{
				current.rotation = rotation;
				return;
			}
			current.rotation = this.root.rotation * rotation;
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x00004AE3 File Offset: 0x00002CE3
		private void OnDrawGizmos()
		{
		}

		// Token: 0x04002D86 RID: 11654
		public int chainLength = 2;

		// Token: 0x04002D87 RID: 11655
		public Transform target;

		// Token: 0x04002D88 RID: 11656
		public Transform pole;

		// Token: 0x04002D89 RID: 11657
		[Header("Solver Parameters")]
		public int iterations = 10;

		// Token: 0x04002D8A RID: 11658
		public float delta = 0.001f;

		// Token: 0x04002D8B RID: 11659
		[Range(0f, 1f)]
		public float snapBackStrength = 1f;

		// Token: 0x04002D8C RID: 11660
		protected float[] bonesLength;

		// Token: 0x04002D8D RID: 11661
		protected float completeLength;

		// Token: 0x04002D8E RID: 11662
		protected Transform[] bones;

		// Token: 0x04002D8F RID: 11663
		protected Vector3[] positions;

		// Token: 0x04002D90 RID: 11664
		protected Vector3[] startDirectionSucc;

		// Token: 0x04002D91 RID: 11665
		protected Quaternion[] startRotationBone;

		// Token: 0x04002D92 RID: 11666
		protected Quaternion startRotationTarget;

		// Token: 0x04002D93 RID: 11667
		protected Transform root;
	}
}
