using System;
using UnityEngine;

// Token: 0x020003D4 RID: 980
public class ScriptActivator : MonoBehaviour
{
	// Token: 0x06001635 RID: 5685 RVA: 0x000B30A0 File Offset: 0x000B12A0
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (this.pistons.Length != 0)
			{
				Piston[] array = this.pistons;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].off = false;
				}
			}
			if (this.lightpillars.Length != 0)
			{
				LightPillar[] array2 = this.lightpillars;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].ActivatePillar();
				}
			}
			Object.Destroy(this);
		}
	}

	// Token: 0x04001E8F RID: 7823
	public Piston[] pistons;

	// Token: 0x04001E90 RID: 7824
	public LightPillar[] lightpillars;
}
