using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewBlood.IK
{
	// Token: 0x020005FD RID: 1533
	public sealed class FabrikSolver3D : Solver3D
	{
		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x060021F4 RID: 8692 RVA: 0x0010B3B8 File Offset: 0x001095B8
		// (set) Token: 0x060021F5 RID: 8693 RVA: 0x0010B3C0 File Offset: 0x001095C0
		public int iterations
		{
			get
			{
				return this.m_Iterations;
			}
			set
			{
				this.m_Iterations = Mathf.Max(value, 1);
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x060021F6 RID: 8694 RVA: 0x0010B3CF File Offset: 0x001095CF
		// (set) Token: 0x060021F7 RID: 8695 RVA: 0x0010B3D7 File Offset: 0x001095D7
		public float tolerance
		{
			get
			{
				return this.m_Tolerance;
			}
			set
			{
				this.m_Tolerance = Mathf.Max(value, 0.001f);
			}
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x0010B3EA File Offset: 0x001095EA
		public override IKChain3D GetChain(int index)
		{
			return this.m_Chain;
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x0002D245 File Offset: 0x0002B445
		protected override int GetChainCount()
		{
			return 1;
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x0010B3F2 File Offset: 0x001095F2
		protected override void DoPrepare()
		{
			base.DoPrepare();
			if (this.m_Positions == null || this.m_Positions.Length != this.m_Chain.transformCount)
			{
				this.m_Positions = new Vector3[this.m_Chain.transformCount];
			}
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x0010B430 File Offset: 0x00109630
		protected override void DoUpdateIK(List<Vector3> effectorPositions)
		{
			float[] lengths = this.m_Chain.lengths;
			for (int i = 0; i < this.m_Positions.Length; i++)
			{
				this.m_Positions[i] = this.m_Chain.transforms[i].position;
			}
			Vector3 vector = this.m_Positions[0];
			Vector3 vector2 = effectorPositions[0];
			float num = Vector3.Magnitude(vector2 - this.m_Positions[this.m_Positions.Length - 1]);
			int num2 = 0;
			while (num2 < this.iterations && num > this.tolerance)
			{
				this.Forward(vector2, lengths, this.m_Positions);
				this.Backward(vector, lengths, this.m_Positions);
				num = Vector3.Magnitude(vector2 - this.m_Positions[this.m_Positions.Length - 1]);
				num2++;
			}
			for (int j = 0; j < this.m_Chain.transformCount - 1; j++)
			{
				Vector3 localPosition = this.m_Chain.transforms[j + 1].localPosition;
				Vector3 vector3 = this.m_Chain.transforms[j].InverseTransformPoint(this.m_Positions[j + 1]);
				this.m_Chain.transforms[j].localRotation *= Quaternion.FromToRotation(localPosition, vector3);
			}
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x0010B590 File Offset: 0x00109790
		private void Forward(Vector3 targetPosition, float[] lengths, Vector3[] positions)
		{
			int num = positions.Length - 1;
			positions[num] = targetPosition;
			for (int i = num - 1; i >= 0; i--)
			{
				Vector3 vector = positions[i + 1] - positions[i];
				float num2 = lengths[i] / vector.magnitude;
				Vector3 vector2 = (1f - num2) * positions[i + 1] + num2 * positions[i];
				positions[i] = vector2;
			}
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x0010B610 File Offset: 0x00109810
		private void Backward(Vector3 originPosition, float[] lengths, Vector3[] positions)
		{
			positions[0] = originPosition;
			int num = positions.Length - 1;
			for (int i = 0; i < num; i++)
			{
				Vector3 vector = positions[i + 1] - positions[i];
				float num2 = lengths[i] / vector.magnitude;
				Vector3 vector2 = (1f - num2) * positions[i] + num2 * positions[i + 1];
				positions[i + 1] = vector2;
			}
		}

		// Token: 0x04002DEC RID: 11756
		public const float MinTolerance = 0.001f;

		// Token: 0x04002DED RID: 11757
		public const int MinIterations = 1;

		// Token: 0x04002DEE RID: 11758
		[SerializeField]
		private IKChain3D m_Chain = new IKChain3D();

		// Token: 0x04002DEF RID: 11759
		[SerializeField]
		[Range(1f, 50f)]
		private int m_Iterations = 10;

		// Token: 0x04002DF0 RID: 11760
		[SerializeField]
		[Range(0.001f, 0.1f)]
		private float m_Tolerance = 0.01f;

		// Token: 0x04002DF1 RID: 11761
		private Vector3[] m_Positions;
	}
}
