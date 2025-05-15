using System;
using System.Collections.Generic;

// Token: 0x02000077 RID: 119
[ConfigureSingleton(SingletonFlags.PersistAutoInstance)]
public class BestiaryData : MonoSingleton<BestiaryData>
{
	// Token: 0x0600022B RID: 555 RVA: 0x0000B710 File Offset: 0x00009910
	private void InitDictionary()
	{
		this.foundEnemies.Clear();
		foreach (object obj in Enum.GetValues(typeof(EnemyType)))
		{
			this.foundEnemies.Add((EnemyType)obj, 0);
		}
	}

	// Token: 0x0600022C RID: 556 RVA: 0x0000B784 File Offset: 0x00009984
	protected override void Awake()
	{
		base.Awake();
		base.gameObject.AddComponent<UnlockablesData>();
	}

	// Token: 0x0600022D RID: 557 RVA: 0x0000B798 File Offset: 0x00009998
	private void Start()
	{
		if (!this.checkedSave)
		{
			this.CheckSave();
		}
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0000B7A8 File Offset: 0x000099A8
	public int GetEnemy(EnemyType enemy)
	{
		if (!this.checkedSave)
		{
			this.CheckSave();
		}
		return this.foundEnemies[enemy];
	}

	// Token: 0x0600022F RID: 559 RVA: 0x0000B7C4 File Offset: 0x000099C4
	public void SetEnemy(EnemyType enemy, int newState = 2)
	{
		if (!this.checkedSave)
		{
			this.CheckSave();
		}
		if (this.foundEnemies[enemy] < newState)
		{
			this.foundEnemies[enemy] = newState;
			GameProgressSaver.SetBestiary(enemy, newState);
			foreach (EnemyInfoPage enemyInfoPage in ListComponent<EnemyInfoPage>.InstanceList)
			{
				enemyInfoPage.UpdateInfo();
				enemyInfoPage.DisplayInfo();
			}
		}
	}

	// Token: 0x06000230 RID: 560 RVA: 0x0000B84C File Offset: 0x00009A4C
	public void CheckSave()
	{
		this.checkedSave = true;
		this.InitDictionary();
		int[] bestiary = GameProgressSaver.GetBestiary();
		for (int i = 0; i < bestiary.Length; i++)
		{
			int num = bestiary[i];
			this.foundEnemies[(EnemyType)i] = num;
		}
	}

	// Token: 0x04000277 RID: 631
	private bool checkedSave;

	// Token: 0x04000278 RID: 632
	private Dictionary<EnemyType, int> foundEnemies = new Dictionary<EnemyType, int>();
}
