using System;
using UnityEngine;

namespace NewBlood.IK
{
	// Token: 0x02000600 RID: 1536
	internal static class IKUtility
	{
		// Token: 0x06002217 RID: 8727 RVA: 0x0010BB69 File Offset: 0x00109D69
		public static bool IsDescendantOf(Transform transform, Transform ancestor)
		{
			transform = transform.parent;
			while (transform != null)
			{
				if (transform == ancestor)
				{
					return true;
				}
				transform = transform.parent;
			}
			return false;
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x0010BB94 File Offset: 0x00109D94
		public static bool IsDescendantOf(Transform transform, Transform ancestor, int ancestorCount)
		{
			transform = transform.parent;
			int num = 0;
			while (num < ancestorCount && !(transform == null))
			{
				if (transform == ancestor)
				{
					return true;
				}
				transform = transform.parent;
				num++;
			}
			return false;
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x0010BBD4 File Offset: 0x00109DD4
		public static bool AncestorCountAtLeast(Transform transform, int count)
		{
			for (int i = 0; i < count; i++)
			{
				if (transform.parent == null)
				{
					return false;
				}
				transform = transform.parent;
			}
			return true;
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x0010BC08 File Offset: 0x00109E08
		public static int GetAncestorCount(Transform transform)
		{
			int num = 0;
			while (transform.parent != null)
			{
				num++;
				transform = transform.parent;
			}
			return num;
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x0010BC34 File Offset: 0x00109E34
		public static int GetMaxChainCount(IKChain3D chain)
		{
			if (chain.effector != null)
			{
				return IKUtility.GetAncestorCount(chain.effector) + 1;
			}
			return 0;
		}
	}
}
