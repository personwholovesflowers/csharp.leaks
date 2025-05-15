﻿using System;
using UnityEngine;

// Token: 0x02000382 RID: 898
public class RaycastAssistant : MonoBehaviour
{
	// Token: 0x060014B8 RID: 5304 RVA: 0x000A76D4 File Offset: 0x000A58D4
	public static Collider[] TrueSphereCastAll(Vector3 position, float radius, Vector3 direction, float maxDistance, LayerMask layerMask)
	{
		Collider[] array = Physics.OverlapSphere(position, radius, layerMask);
		RaycastHit[] array2 = Physics.SphereCastAll(position, radius, direction, maxDistance, layerMask);
		if (array.Length == 0 && array2.Length == 0)
		{
			return null;
		}
		Collider[] array3 = new Collider[array.Length + array2.Length - ((array.Length != 0 && array2.Length != 0) ? 1 : 0)];
		for (int i = 0; i < array3.Length; i++)
		{
			if (i < array.Length)
			{
				array3[i] = array[i];
			}
			else
			{
				array3[i] = array2[i - array.Length].collider;
			}
		}
		return array3;
	}
}
