using System;
using UnityEngine;

// Token: 0x0200002B RID: 43
public class BasicEnemyDataRelay : MonoBehaviour, IPlaceholdableComponent
{
	// Token: 0x0600010E RID: 270 RVA: 0x000063E8 File Offset: 0x000045E8
	public void WillReplace(GameObject oldObject, GameObject newObject, bool isSelfBeingReplaced)
	{
		if (!isSelfBeingReplaced)
		{
			return;
		}
		BasicEnemyDataRelay component = newObject.GetComponent<BasicEnemyDataRelay>();
		if (!component)
		{
			return;
		}
		component.Apply(this);
	}

	// Token: 0x0600010F RID: 271 RVA: 0x00006410 File Offset: 0x00004610
	private void Apply(BasicEnemyDataRelay source)
	{
		Zombie zombie;
		if (base.TryGetComponent<Zombie>(out zombie))
		{
			zombie.health = source.health;
		}
		Drone drone;
		if (base.TryGetComponent<Drone>(out drone))
		{
			drone.health = source.health;
		}
		Machine machine;
		if (base.TryGetComponent<Machine>(out machine))
		{
			machine.health = source.health;
		}
		Statue componentInChildren = base.GetComponentInChildren<Statue>();
		if (componentInChildren)
		{
			componentInChildren.health = source.health;
		}
		SpiderBody componentInChildren2 = base.GetComponentInChildren<SpiderBody>();
		if (componentInChildren2)
		{
			componentInChildren2.health = source.health;
		}
	}

	// Token: 0x040000C7 RID: 199
	[HideInInspector]
	public EnemyType enemyType;

	// Token: 0x040000C8 RID: 200
	[HideInInspector]
	public float health = 1f;
}
