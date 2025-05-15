﻿using System;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002E7 RID: 743
	public static class GizmoUtil
	{
		// Token: 0x06001363 RID: 4963 RVA: 0x000673B0 File Offset: 0x000655B0
		public static void DrawWireCircle(Vector3 aPos, float aRadius)
		{
			GizmoUtil.DrawWireArc(aPos, aRadius, 0f, 360f);
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x000673C4 File Offset: 0x000655C4
		public static void DrawWireArc(Vector3 aPos, float aRadius, float aAngle, float aAngleWidth)
		{
			int num = (int)(6.2831855f * aRadius * (aAngleWidth / 360f) / 0.4f);
			float num2 = aAngleWidth * 0.017453292f / (float)num;
			float num3 = (aAngle - aAngleWidth / 2f) * 0.017453292f;
			for (int i = 0; i < num; i++)
			{
				Gizmos.DrawLine(aPos + new Vector3(Mathf.Cos(num3), 0f, Mathf.Sin(num3)) * aRadius, aPos + new Vector3(Mathf.Cos(num3 + num2), 0f, Mathf.Sin(num3 + num2)) * aRadius);
				num3 += num2;
			}
		}
	}
}
