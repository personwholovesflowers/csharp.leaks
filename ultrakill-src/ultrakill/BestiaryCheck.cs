using System;
using UnityEngine;

// Token: 0x02000076 RID: 118
public class BestiaryCheck : MonoBehaviour
{
	// Token: 0x06000229 RID: 553 RVA: 0x0000B6C4 File Offset: 0x000098C4
	private void Start()
	{
		int[] bestiary = GameProgressSaver.GetBestiary();
		if ((EnemyType)bestiary.Length > this.enemy && bestiary[(int)this.enemy] >= (this.killRequired ? 2 : 1))
		{
			UltrakillEvent ultrakillEvent = this.onEnemyUnlocked;
			if (ultrakillEvent == null)
			{
				return;
			}
			ultrakillEvent.Invoke("");
		}
	}

	// Token: 0x04000274 RID: 628
	public EnemyType enemy;

	// Token: 0x04000275 RID: 629
	public bool killRequired;

	// Token: 0x04000276 RID: 630
	public UltrakillEvent onEnemyUnlocked;
}
