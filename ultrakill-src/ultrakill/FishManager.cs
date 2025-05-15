using System;
using System.Collections.Generic;
using System.Linq;
using Logic;
using UnityEngine;

// Token: 0x020001EC RID: 492
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class FishManager : MonoSingleton<FishManager>
{
	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x06000A00 RID: 2560 RVA: 0x00045752 File Offset: 0x00043952
	public int RemainingFishes
	{
		get
		{
			return this.recognizedFishes.Count((KeyValuePair<FishObject, bool> f) => !f.Value);
		}
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x00045780 File Offset: 0x00043980
	protected override void Awake()
	{
		FishDB[] array = this.fishDbs;
		for (int i = 0; i < array.Length; i++)
		{
			FishDescriptor[] foundFishes = array[i].foundFishes;
			for (int j = 0; j < foundFishes.Length; j++)
			{
				FishObject fish = foundFishes[j].fish;
				if (!this.recognizedFishes.ContainsKey(fish))
				{
					this.recognizedFishes.Add(fish, false);
				}
			}
		}
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x000457E0 File Offset: 0x000439E0
	public bool UnlockFish(FishObject fish)
	{
		if (!this.recognizedFishes.ContainsKey(fish))
		{
			return false;
		}
		if (this.recognizedFishes[fish])
		{
			return false;
		}
		this.recognizedFishes[fish] = true;
		this.newFoundFishes++;
		Action<FishObject> action = this.onFishUnlocked;
		if (action != null)
		{
			action(fish);
		}
		return true;
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x0004583B File Offset: 0x00043A3B
	public void UpdateFishCount()
	{
		if (this.newFoundFishes == 0)
		{
			return;
		}
		MonoSingleton<MapVarManager>.Instance.AddInt("UniqueFishCaught", this.newFoundFishes, false);
		this.newFoundFishes = 0;
	}

	// Token: 0x04000D11 RID: 3345
	[SerializeField]
	private FishDB[] fishDbs;

	// Token: 0x04000D12 RID: 3346
	public Dictionary<FishObject, bool> recognizedFishes = new Dictionary<FishObject, bool>();

	// Token: 0x04000D13 RID: 3347
	public Action<FishObject> onFishUnlocked;

	// Token: 0x04000D14 RID: 3348
	private int newFoundFishes;
}
