using System;
using UnityEngine;

namespace NewBlood.IK
{
	// Token: 0x020005FE RID: 1534
	[Serializable]
	public sealed class IKChain3D
	{
		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x060021FF RID: 8703 RVA: 0x0010B6B4 File Offset: 0x001098B4
		public bool isValid
		{
			get
			{
				return !(this.m_EffectorTransform == null) && this.m_TransformCount != 0 && this.m_Transforms != null && this.m_Transforms.Length == this.m_TransformCount && this.m_DefaultLocalRotations != null && this.m_DefaultLocalRotations.Length == this.m_TransformCount && this.m_StoredLocalRotations != null && this.m_StoredLocalRotations.Length == this.m_TransformCount && !(this.m_Transforms[0] == null) && !(this.m_Transforms[this.m_TransformCount - 1] != this.m_EffectorTransform) && (!(this.m_TargetTransform != null) || !IKUtility.IsDescendantOf(this.m_TargetTransform, this.m_Transforms[0], this.m_TransformCount));
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06002200 RID: 8704 RVA: 0x0010B785 File Offset: 0x00109985
		public int transformCount
		{
			get
			{
				return this.m_TransformCount;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06002201 RID: 8705 RVA: 0x0010B78D File Offset: 0x0010998D
		// (set) Token: 0x06002202 RID: 8706 RVA: 0x0010B795 File Offset: 0x00109995
		public Transform effector
		{
			get
			{
				return this.m_EffectorTransform;
			}
			set
			{
				this.m_EffectorTransform = value;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06002203 RID: 8707 RVA: 0x0010B79E File Offset: 0x0010999E
		// (set) Token: 0x06002204 RID: 8708 RVA: 0x0010B7A6 File Offset: 0x001099A6
		public Transform target
		{
			get
			{
				return this.m_TargetTransform;
			}
			set
			{
				this.m_TargetTransform = value;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06002205 RID: 8709 RVA: 0x0010B7AF File Offset: 0x001099AF
		public Transform[] transforms
		{
			get
			{
				return this.m_Transforms;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06002206 RID: 8710 RVA: 0x0010B7B7 File Offset: 0x001099B7
		public Transform rootTransform
		{
			get
			{
				if (this.m_TransformCount == 0)
				{
					return null;
				}
				if (this.m_Transforms == null || this.m_Transforms.Length != this.m_TransformCount)
				{
					return null;
				}
				return this.m_Transforms[0];
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06002207 RID: 8711 RVA: 0x0010B7E5 File Offset: 0x001099E5
		public float[] lengths
		{
			get
			{
				if (this.isValid)
				{
					this.PrepareLengths();
					return this.m_Lengths;
				}
				return null;
			}
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x0010B800 File Offset: 0x00109A00
		public void Initialize()
		{
			if (this.m_EffectorTransform == null || this.m_TransformCount == 0)
			{
				return;
			}
			if (!IKUtility.AncestorCountAtLeast(this.m_EffectorTransform, this.m_TransformCount - 1))
			{
				return;
			}
			this.m_Transforms = new Transform[this.m_TransformCount];
			this.m_DefaultLocalRotations = new Quaternion[this.m_TransformCount];
			this.m_StoredLocalRotations = new Quaternion[this.m_TransformCount];
			Transform transform = this.m_EffectorTransform;
			int num = this.m_TransformCount - 1;
			while (transform != null && num >= 0)
			{
				this.m_Transforms[num] = transform;
				this.m_DefaultLocalRotations[num] = transform.localRotation;
				transform = transform.parent;
				num--;
			}
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x0010B8B4 File Offset: 0x00109AB4
		public void RestoreDefaultPose(bool targetRotationIsConstrained)
		{
			int num = this.m_TransformCount;
			if (!targetRotationIsConstrained)
			{
				num--;
			}
			for (int i = 0; i < num; i++)
			{
				this.m_Transforms[i].localRotation = this.m_DefaultLocalRotations[i];
			}
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x0010B8F4 File Offset: 0x00109AF4
		public void StoreLocalRotations()
		{
			for (int i = 0; i < this.m_Transforms.Length; i++)
			{
				this.m_StoredLocalRotations[i] = this.m_Transforms[i].localRotation;
			}
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x0010B930 File Offset: 0x00109B30
		public void BlendFKToIK(float finalWeight, bool targetRotationIsConstrained)
		{
			int num = this.m_TransformCount;
			if (!targetRotationIsConstrained)
			{
				num--;
			}
			for (int i = 0; i < num; i++)
			{
				this.m_Transforms[i].localRotation = Quaternion.Slerp(this.m_StoredLocalRotations[i], this.m_Transforms[i].localRotation, finalWeight);
			}
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x0010B984 File Offset: 0x00109B84
		private void PrepareLengths()
		{
			Transform transform = this.m_EffectorTransform;
			if (this.m_Lengths == null || this.m_Lengths.Length != this.m_TransformCount - 1)
			{
				this.m_Lengths = new float[this.m_TransformCount - 1];
			}
			int num = this.m_Lengths.Length - 1;
			while (num > 0 && !(transform == null) && !(transform.parent == null))
			{
				this.m_Lengths[num - 1] = Vector3.Distance(transform.position, transform.parent.position);
				transform = transform.parent;
				num--;
			}
		}

		// Token: 0x04002DF2 RID: 11762
		[SerializeField]
		private Transform m_EffectorTransform;

		// Token: 0x04002DF3 RID: 11763
		[SerializeField]
		private Transform m_TargetTransform;

		// Token: 0x04002DF4 RID: 11764
		[SerializeField]
		private int m_TransformCount;

		// Token: 0x04002DF5 RID: 11765
		[SerializeField]
		private Transform[] m_Transforms;

		// Token: 0x04002DF6 RID: 11766
		[SerializeField]
		private Quaternion[] m_DefaultLocalRotations;

		// Token: 0x04002DF7 RID: 11767
		[SerializeField]
		private Quaternion[] m_StoredLocalRotations;

		// Token: 0x04002DF8 RID: 11768
		private float[] m_Lengths;
	}
}
