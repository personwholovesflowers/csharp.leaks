using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000064 RID: 100
public class Arena : MonoBehaviour
{
	// Token: 0x060001E0 RID: 480 RVA: 0x00009C5C File Offset: 0x00007E5C
	private void Awake()
	{
		ActivateNextWave[] waves = this.GetWaves();
		this.AutoSetupWaves(waves);
		ActivateNextWave[] array = waves;
		for (int i = 0; i < array.Length; i++)
		{
			GameObject[] enemies = Arena.GetEnemies(array[i].transform);
			for (int j = 0; j < enemies.Length; j++)
			{
				enemies[j].SetActive(false);
			}
		}
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x00009CB4 File Offset: 0x00007EB4
	public ActivateNextWave[] GetWaves()
	{
		List<ActivateNextWave> list = new List<ActivateNextWave>();
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.GetComponent<ActivateNextWave>())
			{
				list.Add(transform.GetComponent<ActivateNextWave>());
			}
		}
		return list.ToArray();
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x00009D2C File Offset: 0x00007F2C
	public static GameObject[] GetEnemies(Transform target)
	{
		List<GameObject> list = new List<GameObject>();
		foreach (object obj in target.transform)
		{
			Transform transform = (Transform)obj;
			list.Add(transform.gameObject);
		}
		return list.ToArray();
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x00009D98 File Offset: 0x00007F98
	public ActivateArena GetActivateArena()
	{
		return base.GetComponentInChildren<ActivateArena>();
	}

	// Token: 0x060001E4 RID: 484 RVA: 0x00009DA0 File Offset: 0x00007FA0
	private void ConfigureActivateArena(ActivateArena aa)
	{
		aa.doors = this.doors;
		ActivateNextWave[] waves = this.GetWaves();
		if (waves.Length == 0)
		{
			return;
		}
		aa.enemies = Arena.GetEnemies(waves[0].transform);
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x00009DD8 File Offset: 0x00007FD8
	private void ConfigureWaves(ActivateNextWave[] waves)
	{
		if (waves == null || waves.Length == 0)
		{
			return;
		}
		for (int i = 0; i < waves.Length; i++)
		{
			waves[i].CountEnemies();
			if (i < waves.Length - 1)
			{
				waves[i].nextEnemies = Arena.GetEnemies(waves[i + 1].transform);
			}
			waves[i].doors = new Door[0];
			waves[i].lastWave = false;
		}
		waves[waves.Length - 1].doors = this.doors;
		waves[waves.Length - 1].lastWave = true;
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x00009E58 File Offset: 0x00008058
	public void AutoSetupWaves(ActivateNextWave[] waves)
	{
		this.ConfigureWaves(waves);
		this.ConfigureActivateArena(this.GetActivateArena());
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x00009E70 File Offset: 0x00008070
	private void OnDrawGizmosSelected()
	{
		foreach (Door door in this.doors)
		{
			Gizmos.color = new Color(1f, 1f, 0f, 0.5f);
			Vector3? vector = null;
			Vector3? vector2 = null;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			GameObject gameObject;
			if (door.doorType == DoorType.Normal)
			{
				gameObject = door.transform.parent.gameObject;
			}
			else
			{
				gameObject = door.gameObject;
			}
			Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				Bounds bounds = componentsInChildren[j].bounds;
				float num = bounds.center.x - bounds.size.x;
				float num2 = bounds.center.x + bounds.size.x;
				float num3 = bounds.center.y - bounds.size.y;
				float num4 = bounds.center.y + bounds.size.y;
				float num5 = bounds.center.z - bounds.size.z;
				float num6 = bounds.center.z + bounds.size.z;
				if (vector == null)
				{
					zero2 = new Vector3(num, num3, num5);
					zero = new Vector3(num2, num4, num6);
					vector2 = new Vector3?(new Vector3(zero.x - zero2.x, zero.y - zero2.y, zero.z - zero2.z));
					vector = new Vector3?(new Vector3(num + vector2.Value.x / 2f, num3 + vector2.Value.y / 2f, num5 + vector2.Value.z / 2f));
				}
				else
				{
					if (num < zero2.x)
					{
						zero2.x = num;
					}
					if (zero.x < num2)
					{
						zero.x = num2;
					}
					if (num3 < zero2.y)
					{
						zero2.y = num3;
					}
					if (zero.y < num4)
					{
						zero.y = num4;
					}
					if (num5 < zero2.z)
					{
						zero2.z = num5;
					}
					if (zero.z < num6)
					{
						zero.z = num6;
					}
				}
			}
			vector2 = new Vector3?(new Vector3(zero.x - zero2.x, zero.y - zero2.y, zero.z - zero2.z));
			vector = new Vector3?(new Vector3(zero2.x + vector2.Value.x / 2f, zero2.y + vector2.Value.y / 2f, zero2.z + vector2.Value.z / 2f));
			Gizmos.color = new Color(1f, 1f, 0f, 1f);
			Gizmos.DrawWireCube(vector.Value, vector2.Value * 0.75f);
			Gizmos.color = new Color(1f, 1f, 0f, 0.15f);
			Gizmos.DrawCube(vector.Value, vector2.Value * 0.75f);
		}
	}

	// Token: 0x04000208 RID: 520
	public Door[] doors;
}
