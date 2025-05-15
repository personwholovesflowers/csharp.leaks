using System;
using System.Collections.Generic;
using UnityEngine.Events;

// Token: 0x02000492 RID: 1170
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class UnlockablesData : MonoSingleton<UnlockablesData>
{
	// Token: 0x06001AD9 RID: 6873 RVA: 0x000DCFB5 File Offset: 0x000DB1B5
	private void InitDictionary()
	{
		this.unlocked.Clear();
	}

	// Token: 0x06001ADA RID: 6874 RVA: 0x000DCFC2 File Offset: 0x000DB1C2
	private void Start()
	{
		if (!this.checkedSave)
		{
			this.CheckSave();
		}
	}

	// Token: 0x06001ADB RID: 6875 RVA: 0x000DCFD2 File Offset: 0x000DB1D2
	public bool IsUnlocked(UnlockableType unlockable)
	{
		if (!this.checkedSave)
		{
			this.CheckSave();
		}
		return this.unlocked.Contains(unlockable);
	}

	// Token: 0x06001ADC RID: 6876 RVA: 0x000DCFF0 File Offset: 0x000DB1F0
	public void SetUnlocked(UnlockableType unlockable, bool unlocked)
	{
		if (!this.checkedSave)
		{
			this.CheckSave();
		}
		if (unlocked && !this.unlocked.Contains(unlockable))
		{
			this.unlocked.Add(unlockable);
			GameProgressSaver.SetUnlockable(unlockable, true);
			this.unlockableFound();
			return;
		}
		if (!unlocked && this.unlocked.Contains(unlockable))
		{
			this.unlocked.Remove(unlockable);
			GameProgressSaver.SetUnlockable(unlockable, false);
			this.unlockableFound();
		}
	}

	// Token: 0x06001ADD RID: 6877 RVA: 0x000DD06C File Offset: 0x000DB26C
	public void CheckSave()
	{
		this.checkedSave = true;
		this.InitDictionary();
		UnlockableType[] unlockables = GameProgressSaver.GetUnlockables();
		for (int i = 0; i < unlockables.Length; i++)
		{
			this.unlocked.Add(unlockables[i]);
		}
	}

	// Token: 0x040025D4 RID: 9684
	public UnityAction unlockableFound = delegate
	{
	};

	// Token: 0x040025D5 RID: 9685
	private bool checkedSave;

	// Token: 0x040025D6 RID: 9686
	private readonly HashSet<UnlockableType> unlocked = new HashSet<UnlockableType>();
}
