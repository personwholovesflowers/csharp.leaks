using System;
using UnityEngine;

// Token: 0x02000329 RID: 809
public class OutdoorsChecker : MonoBehaviour
{
	// Token: 0x060012C2 RID: 4802 RVA: 0x00095874 File Offset: 0x00093A74
	private void Start()
	{
		if (!MonoSingleton<OutdoorLightMaster>.Instance)
		{
			base.enabled = false;
			return;
		}
		if (this.targets.Length == 0)
		{
			this.targets = new GameObject[1];
			this.targets[0] = base.gameObject;
		}
		this.boxCol = base.GetComponent<BoxCollider>();
		this.SlowUpdate();
	}

	// Token: 0x060012C3 RID: 4803 RVA: 0x000958CC File Offset: 0x00093ACC
	public void SlowUpdate()
	{
		if (!this.oneTime)
		{
			base.Invoke("SlowUpdate", 0.5f);
		}
		if (OutdoorsChecker.CheckIfPositionOutdoors(this.boxCol ? this.boxCol.bounds.center : base.transform.position))
		{
			foreach (GameObject gameObject in this.targets)
			{
				if (gameObject && gameObject.layer != 13)
				{
					gameObject.layer = (this.nonSolid ? 25 : 24);
				}
			}
			UltrakillEvent ultrakillEvent = this.onOutdoors;
			if (ultrakillEvent == null)
			{
				return;
			}
			ultrakillEvent.Invoke("");
			return;
		}
		else
		{
			foreach (GameObject gameObject2 in this.targets)
			{
				if (gameObject2 && gameObject2.layer != 13)
				{
					gameObject2.layer = (this.nonSolid ? 27 : 8);
				}
			}
			UltrakillEvent ultrakillEvent2 = this.onIndoors;
			if (ultrakillEvent2 == null)
			{
				return;
			}
			ultrakillEvent2.Invoke("");
			return;
		}
	}

	// Token: 0x060012C4 RID: 4804 RVA: 0x000959D4 File Offset: 0x00093BD4
	public static bool CheckIfPositionOutdoors(Vector3 position)
	{
		if (!MonoSingleton<OutdoorLightMaster>.Instance)
		{
			return false;
		}
		Collider[] array = Physics.OverlapSphere(position, 0.1f, 262144, QueryTriggerInteraction.Collide);
		if (array != null && array.Length != 0)
		{
			foreach (Collider collider in array)
			{
				if (!(collider == null) && MonoSingleton<OutdoorLightMaster>.Instance.outdoorsZonesCheckerable.Contains(collider))
				{
					return !MonoSingleton<OutdoorLightMaster>.Instance.inverse;
				}
			}
		}
		return MonoSingleton<OutdoorLightMaster>.Instance.inverse;
	}

	// Token: 0x040019B3 RID: 6579
	public bool nonSolid = true;

	// Token: 0x040019B4 RID: 6580
	public bool oneTime;

	// Token: 0x040019B5 RID: 6581
	public GameObject[] targets;

	// Token: 0x040019B6 RID: 6582
	[Header("Additional Events")]
	public UltrakillEvent onIndoors;

	// Token: 0x040019B7 RID: 6583
	public UltrakillEvent onOutdoors;

	// Token: 0x040019B8 RID: 6584
	private BoxCollider boxCol;
}
