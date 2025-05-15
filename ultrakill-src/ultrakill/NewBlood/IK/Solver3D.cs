using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewBlood.IK
{
	// Token: 0x02000601 RID: 1537
	public abstract class Solver3D : MonoBehaviour
	{
		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x0600221C RID: 8732 RVA: 0x0010BC53 File Offset: 0x00109E53
		public int chainCount
		{
			get
			{
				return this.GetChainCount();
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x0600221D RID: 8733 RVA: 0x0010BC5B File Offset: 0x00109E5B
		// (set) Token: 0x0600221E RID: 8734 RVA: 0x0010BC63 File Offset: 0x00109E63
		public bool constrainRotation
		{
			get
			{
				return this.m_ConstrainRotation;
			}
			set
			{
				this.m_ConstrainRotation = value;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x0600221F RID: 8735 RVA: 0x0010BC6C File Offset: 0x00109E6C
		// (set) Token: 0x06002220 RID: 8736 RVA: 0x0010BC74 File Offset: 0x00109E74
		public bool solveFromDefaultPose
		{
			get
			{
				return this.m_SolveFromDefaultPose;
			}
			set
			{
				this.m_SolveFromDefaultPose = value;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06002221 RID: 8737 RVA: 0x0010BC80 File Offset: 0x00109E80
		public bool isValid
		{
			get
			{
				for (int i = 0; i < this.chainCount; i++)
				{
					if (!this.GetChain(i).isValid)
					{
						return false;
					}
				}
				return this.DoValidate();
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06002222 RID: 8738 RVA: 0x0010BCB4 File Offset: 0x00109EB4
		public bool allChainsHaveTargets
		{
			get
			{
				for (int i = 0; i < this.chainCount; i++)
				{
					if (this.GetChain(i).target == null)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06002223 RID: 8739 RVA: 0x0010BCE9 File Offset: 0x00109EE9
		// (set) Token: 0x06002224 RID: 8740 RVA: 0x0010BCF1 File Offset: 0x00109EF1
		public float weight
		{
			get
			{
				return this.m_Weight;
			}
			set
			{
				this.m_Weight = Mathf.Clamp01(value);
			}
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x0010BCFF File Offset: 0x00109EFF
		public void UpdateIK(float globalWeight)
		{
			if (this.allChainsHaveTargets)
			{
				this.PrepareEffectorPositions();
				this.UpdateIK(this.m_TargetPositions, globalWeight);
			}
		}

		// Token: 0x06002226 RID: 8742 RVA: 0x0010BD1C File Offset: 0x00109F1C
		public void UpdateIK(List<Vector3> positions, float globalWeight)
		{
			if (positions.Count != this.chainCount)
			{
				return;
			}
			float num = globalWeight * this.m_Weight;
			if (Mathf.Approximately(num, 0f))
			{
				return;
			}
			if (!this.isValid)
			{
				return;
			}
			this.Prepare();
			if (num < 1f)
			{
				this.StoreLocalRotations();
			}
			this.DoUpdateIK(positions);
			if (this.constrainRotation)
			{
				for (int i = 0; i < this.chainCount; i++)
				{
					IKChain3D chain = this.GetChain(i);
					if (!(chain.target == null))
					{
						chain.effector.rotation = chain.target.rotation;
					}
				}
			}
			if (num < 1f)
			{
				this.BlendFKToIK(num);
			}
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x0010BDC8 File Offset: 0x00109FC8
		public void Initialize()
		{
			this.DoInitialize();
			for (int i = 0; i < this.chainCount; i++)
			{
				this.GetChain(i).Initialize();
			}
		}

		// Token: 0x06002228 RID: 8744
		public abstract IKChain3D GetChain(int index);

		// Token: 0x06002229 RID: 8745
		protected abstract int GetChainCount();

		// Token: 0x0600222A RID: 8746
		protected abstract void DoUpdateIK(List<Vector3> effectorPositions);

		// Token: 0x0600222B RID: 8747 RVA: 0x0002D245 File Offset: 0x0002B445
		protected virtual bool DoValidate()
		{
			return true;
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x00004AE3 File Offset: 0x00002CE3
		protected virtual void DoInitialize()
		{
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x00004AE3 File Offset: 0x00002CE3
		protected virtual void DoPrepare()
		{
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x0010BDF8 File Offset: 0x00109FF8
		protected virtual Transform GetRootTransform()
		{
			if (this.chainCount > 0)
			{
				return this.GetChain(0).rootTransform;
			}
			return null;
		}

		// Token: 0x0600222F RID: 8751 RVA: 0x0010BE11 File Offset: 0x0010A011
		protected virtual void OnValidate()
		{
			this.m_Weight = Mathf.Clamp01(this.m_Weight);
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x0010BE24 File Offset: 0x0010A024
		private void Prepare()
		{
			if (this.solveFromDefaultPose)
			{
				for (int i = 0; i < this.chainCount; i++)
				{
					IKChain3D chain = this.GetChain(i);
					bool flag = this.constrainRotation && chain.target != null;
					chain.RestoreDefaultPose(flag);
				}
			}
			this.DoPrepare();
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x0010BE78 File Offset: 0x0010A078
		private void PrepareEffectorPositions()
		{
			this.m_TargetPositions.Clear();
			for (int i = 0; i < this.chainCount; i++)
			{
				IKChain3D chain = this.GetChain(i);
				if (!(chain.target == null))
				{
					this.m_TargetPositions.Add(chain.target.position);
				}
			}
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x0010BED0 File Offset: 0x0010A0D0
		private void StoreLocalRotations()
		{
			for (int i = 0; i < this.chainCount; i++)
			{
				this.GetChain(i).StoreLocalRotations();
			}
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x0010BEFC File Offset: 0x0010A0FC
		private void BlendFKToIK(float finalWeight)
		{
			for (int i = 0; i < this.chainCount; i++)
			{
				IKChain3D chain = this.GetChain(i);
				bool flag = this.constrainRotation && chain.target != null;
				chain.BlendFKToIK(finalWeight, flag);
			}
		}

		// Token: 0x04002DFB RID: 11771
		[SerializeField]
		private bool m_ConstrainRotation = true;

		// Token: 0x04002DFC RID: 11772
		[SerializeField]
		private bool m_SolveFromDefaultPose = true;

		// Token: 0x04002DFD RID: 11773
		[Range(0f, 1f)]
		[SerializeField]
		private float m_Weight = 1f;

		// Token: 0x04002DFE RID: 11774
		private List<Vector3> m_TargetPositions = new List<Vector3>();
	}
}
