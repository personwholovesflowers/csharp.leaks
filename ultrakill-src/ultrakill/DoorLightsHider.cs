using System;
using UnityEngine;

// Token: 0x02000120 RID: 288
public class DoorLightsHider : MonoBehaviour
{
	// Token: 0x06000555 RID: 1365 RVA: 0x00024118 File Offset: 0x00022318
	private void Start()
	{
		this.parentDoor = base.GetComponentInParent<Door>();
		this.SlowUpdate();
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x0002412C File Offset: 0x0002232C
	private void SlowUpdate()
	{
		if (this.parentDoor && this.parentDoor.open)
		{
			GameObject[] array = this.sideA;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
			array = this.sideB;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
			this.overridePreviousSide = true;
			base.Invoke("SlowUpdate", 0.025f);
			return;
		}
		if (Vector3.Distance(base.transform.position, MonoSingleton<PlayerTracker>.Instance.GetTarget().position) > 200f)
		{
			GameObject[] array = this.sideA;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			array = this.sideB;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			this.overridePreviousSide = true;
			base.Invoke("SlowUpdate", 0.5f);
			return;
		}
		Vector3 vector = Vector3.zero;
		Vector3 vector2 = Vector3.zero;
		foreach (GameObject gameObject in this.sideA)
		{
			vector += gameObject.transform.position;
		}
		foreach (GameObject gameObject2 in this.sideB)
		{
			vector2 += gameObject2.transform.position;
		}
		vector /= (float)this.sideA.Length;
		vector2 /= (float)this.sideB.Length;
		if (Vector3.Distance(vector, MonoSingleton<PlayerTracker>.Instance.GetTarget().position) <= Vector3.Distance(vector2, MonoSingleton<PlayerTracker>.Instance.GetTarget().position))
		{
			this.SetSide(true);
		}
		else
		{
			this.SetSide(false);
		}
		base.Invoke("SlowUpdate", 0.1f);
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x000242F4 File Offset: 0x000224F4
	public void SetSide(bool targetSideIsA)
	{
		if (!this.overridePreviousSide && this.currentSideIsA == targetSideIsA)
		{
			return;
		}
		GameObject[] array = this.sideA;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(targetSideIsA);
		}
		array = this.sideB;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(!targetSideIsA);
		}
		this.overridePreviousSide = false;
		this.currentSideIsA = targetSideIsA;
	}

	// Token: 0x04000766 RID: 1894
	public GameObject[] sideA;

	// Token: 0x04000767 RID: 1895
	public GameObject[] sideB;

	// Token: 0x04000768 RID: 1896
	private Door parentDoor;

	// Token: 0x04000769 RID: 1897
	private bool currentSideIsA;

	// Token: 0x0400076A RID: 1898
	private bool overridePreviousSide = true;
}
