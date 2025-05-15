using System;
using UnityEngine;

// Token: 0x02000383 RID: 899
public class RaycastHelper
{
	// Token: 0x060014BA RID: 5306 RVA: 0x000A7758 File Offset: 0x000A5958
	public static bool RaycastAndDebugDraw(Vector3 origin, Vector3 direction, float maxDistance, int layerMask)
	{
		RaycastHit raycastHit;
		bool flag = Physics.Raycast(origin, direction, out raycastHit, maxDistance, layerMask);
		if (Application.isEditor)
		{
			if (flag)
			{
				Debug.DrawRay(origin, direction.normalized * raycastHit.distance, Color.green, 15f);
				Debug.DrawRay(origin + direction.normalized * raycastHit.distance, direction.normalized * (maxDistance - raycastHit.distance), Color.red, 15f);
			}
			else
			{
				Debug.DrawRay(origin, direction.normalized * maxDistance, Color.green, 15f);
			}
		}
		return flag;
	}

	// Token: 0x04001C8A RID: 7306
	private const float Duration = 15f;
}
