using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewBlood.IK
{
	// Token: 0x020005FF RID: 1535
	public sealed class IKManager3D : MonoBehaviour
	{
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x0600220E RID: 8718 RVA: 0x0010BA18 File Offset: 0x00109C18
		// (set) Token: 0x0600220F RID: 8719 RVA: 0x0010BA20 File Offset: 0x00109C20
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

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06002210 RID: 8720 RVA: 0x0010BA2E File Offset: 0x00109C2E
		public List<Solver3D> solvers
		{
			get
			{
				return this.m_Solvers;
			}
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x0010BA36 File Offset: 0x00109C36
		public void AddSolver(Solver3D solver)
		{
			if (this.m_Solvers.Contains(solver))
			{
				return;
			}
			this.m_Solvers.Add(solver);
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x0010BA53 File Offset: 0x00109C53
		public void RemoveSolver(Solver3D solver)
		{
			this.m_Solvers.Remove(solver);
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x0010BA64 File Offset: 0x00109C64
		public void UpdateManager()
		{
			foreach (Solver3D solver3D in this.m_Solvers)
			{
				if (!(solver3D == null) && solver3D.isActiveAndEnabled)
				{
					if (!solver3D.isValid)
					{
						solver3D.Initialize();
					}
					solver3D.UpdateIK(this.m_Weight);
				}
			}
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x0010BADC File Offset: 0x00109CDC
		private void LateUpdate()
		{
			this.UpdateManager();
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x0010BAE4 File Offset: 0x00109CE4
		private void FindChildSolvers()
		{
			this.m_Solvers.Clear();
			base.transform.GetComponentsInChildren<Solver3D>(true, this.m_Solvers);
			for (int i = this.m_Solvers.Count - 1; i >= 0; i--)
			{
				if (this.m_Solvers[i].GetComponentInParent<IKManager3D>() != this)
				{
					this.m_Solvers.RemoveAt(i);
				}
			}
		}

		// Token: 0x04002DF9 RID: 11769
		[SerializeField]
		private List<Solver3D> m_Solvers = new List<Solver3D>();

		// Token: 0x04002DFA RID: 11770
		[Range(0f, 1f)]
		[SerializeField]
		private float m_Weight = 1f;
	}
}
