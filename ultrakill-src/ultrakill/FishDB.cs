using System;
using System.Linq;
using UnityEngine;

// Token: 0x020001DB RID: 475
[CreateAssetMenu(fileName = "New Fish Database", menuName = "ULTRAKILL/FishDB")]
public class FishDB : ScriptableObject
{
	// Token: 0x060009AE RID: 2478 RVA: 0x00043310 File Offset: 0x00041510
	public void SetupWater(Water water)
	{
		if (!this.fishGhostPrefab)
		{
			return;
		}
		Bounds bounds = water.GetComponent<Collider>().bounds;
		int num = (int)(bounds.size.x * bounds.size.y / 100f);
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.fishGhostPrefab, water.transform, true);
			gameObject.transform.position = new Vector3(Random.Range(-bounds.size.x / 4f, bounds.size.x / 4f) + bounds.center.x, 0f, Random.Range(-bounds.size.z / 4f, bounds.size.z / 4f) + bounds.center.z);
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, bounds.center.y + Random.Range(-1f, 1f) * (bounds.size.y / 2f - 0.2f), gameObject.transform.position.z);
			gameObject.transform.localRotation = Quaternion.Euler(0f, (float)Random.Range(0, 360), 0f);
		}
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x0004348C File Offset: 0x0004168C
	public FishDescriptor GetRandomFish(FishObject[] attractFish)
	{
		if (attractFish != null && attractFish.Length != 0)
		{
			FishDescriptor fishDescriptor = this.foundFishes.FirstOrDefault((FishDescriptor f) => attractFish.Any((FishObject a) => a == f.fish));
			if (fishDescriptor != null)
			{
				return fishDescriptor;
			}
		}
		int num = 0;
		FishDescriptor[] array = this.foundFishes;
		for (int i = 0; i < array.Length; i++)
		{
			int chance = array[i].chance;
			num += chance;
		}
		if (num == 0)
		{
			return null;
		}
		int num2 = Random.Range(0, num);
		int num3 = 0;
		foreach (FishDescriptor fishDescriptor2 in this.foundFishes)
		{
			int chance2 = fishDescriptor2.chance;
			num3 += chance2;
			if (num2 < num3)
			{
				return fishDescriptor2;
			}
		}
		return this.foundFishes.Last<FishDescriptor>();
	}

	// Token: 0x04000C99 RID: 3225
	public string fullName;

	// Token: 0x04000C9A RID: 3226
	public Color symbolColor = Color.white;

	// Token: 0x04000C9B RID: 3227
	public GameObject fishGhostPrefab;

	// Token: 0x04000C9C RID: 3228
	public FishDescriptor[] foundFishes;
}
