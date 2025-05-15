using System;
using System.Collections.Generic;
using ULTRAKILL.Cheats;
using UnityEngine;

// Token: 0x0200019C RID: 412
[ConfigureSingleton(SingletonFlags.None)]
public class EnemyTracker : MonoSingleton<EnemyTracker>
{
	// Token: 0x0600085A RID: 2138 RVA: 0x00039C90 File Offset: 0x00037E90
	private void Update()
	{
		if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.F9))
		{
			foreach (EnemyIdentifier enemyIdentifier in this.GetCurrentEnemies())
			{
				enemyIdentifier.gameObject.SetActive(false);
				enemyIdentifier.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x00039D08 File Offset: 0x00037F08
	private void OnGUI()
	{
		if (!EnemyIdentifierDebug.Active)
		{
			return;
		}
		List<EnemyIdentifier> currentEnemies = this.GetCurrentEnemies();
		GUI.color = Color.white;
		GUIStyle guistyle = new GUIStyle(GUI.skin.label)
		{
			fontStyle = FontStyle.Bold
		};
		GUILayout.Label("[Enemy Tracker]", guistyle, Array.Empty<GUILayoutOption>());
		GUILayout.Space(12f);
		GUILayout.Label("Active Enemies: " + currentEnemies.Count.ToString(), guistyle, Array.Empty<GUILayoutOption>());
		for (int i = 0; i < currentEnemies.Count; i++)
		{
			GUILayout.Label("Enemy " + i.ToString() + ": " + currentEnemies[i].name, guistyle, Array.Empty<GUILayoutOption>());
			GUILayout.Label("Enemy Type: " + currentEnemies[i].enemyType.ToString(), guistyle, Array.Empty<GUILayoutOption>());
			GUILayout.Label("Enemy Rank: " + this.enemyRanks[i].ToString(), guistyle, Array.Empty<GUILayoutOption>());
			GUILayout.Space(12f);
		}
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x00039E28 File Offset: 0x00038028
	public List<EnemyIdentifier> GetCurrentEnemies()
	{
		List<EnemyIdentifier> list = new List<EnemyIdentifier>();
		if (this.enemies != null && this.enemies.Count > 0)
		{
			for (int i = this.enemies.Count - 1; i >= 0; i--)
			{
				if (this.enemies[i].dead || this.enemies[i] == null || this.enemies[i].gameObject == null)
				{
					this.enemies.RemoveAt(i);
					this.enemyRanks.RemoveAt(i);
				}
				else if (this.enemies[i].gameObject.activeInHierarchy)
				{
					list.Add(this.enemies[i]);
				}
			}
		}
		return list;
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x00039EFC File Offset: 0x000380FC
	public void UpdateIdolsNow()
	{
		foreach (EnemyIdentifier enemyIdentifier in this.GetCurrentEnemies())
		{
			if (enemyIdentifier.enemyType == EnemyType.Idol && enemyIdentifier.idol != null)
			{
				enemyIdentifier.idol.PickNewTarget(true);
			}
		}
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x00039F6C File Offset: 0x0003816C
	public List<EnemyIdentifier> GetEnemiesOfType(EnemyType type)
	{
		List<EnemyIdentifier> currentEnemies = this.GetCurrentEnemies();
		if (currentEnemies.Count > 0)
		{
			for (int i = currentEnemies.Count - 1; i >= 0; i--)
			{
				if (currentEnemies[i].enemyType != type)
				{
					currentEnemies.RemoveAt(i);
				}
			}
		}
		return currentEnemies;
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x00039FB3 File Offset: 0x000381B3
	public void AddEnemy(EnemyIdentifier eid)
	{
		if (!this.enemies.Contains(eid))
		{
			this.enemies.Add(eid);
			this.enemyRanks.Add(this.GetEnemyRank(eid));
		}
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x00039FE4 File Offset: 0x000381E4
	public int GetEnemyRank(EnemyIdentifier eid)
	{
		switch (eid.enemyType)
		{
		case EnemyType.Cerberus:
			return 3;
		case EnemyType.Drone:
			return 1;
		case EnemyType.HideousMass:
			return 6;
		case EnemyType.Filth:
			return 0;
		case EnemyType.MaliciousFace:
			return 3;
		case EnemyType.Mindflayer:
			return 5;
		case EnemyType.Streetcleaner:
			return 2;
		case EnemyType.Swordsmachine:
			return 3;
		case EnemyType.V2:
			return 6;
		case EnemyType.Virtue:
			return 3;
		case EnemyType.Wicked:
			return 6;
		case EnemyType.Minos:
			return 6;
		case EnemyType.Stalker:
			return 4;
		case EnemyType.Stray:
			return 0;
		case EnemyType.Schism:
			return 1;
		case EnemyType.Soldier:
			return 1;
		case EnemyType.Gabriel:
			return 6;
		case EnemyType.MinosPrime:
			return 7;
		case EnemyType.Sisyphus:
			return 6;
		case EnemyType.Turret:
			return 3;
		case EnemyType.V2Second:
			return 6;
		case EnemyType.Mandalore:
			return 5;
		case EnemyType.Ferryman:
			return 5;
		case EnemyType.GabrielSecond:
			return 6;
		case EnemyType.SisyphusPrime:
			return 7;
		case EnemyType.Mannequin:
			return 2;
		case EnemyType.Minotaur:
			return 6;
		case EnemyType.Gutterman:
			return 4;
		case EnemyType.Guttertank:
			return 4;
		case EnemyType.Puppet:
			return 0;
		}
		return -1;
	}

	// Token: 0x04000B29 RID: 2857
	public List<EnemyIdentifier> enemies = new List<EnemyIdentifier>();

	// Token: 0x04000B2A RID: 2858
	public List<int> enemyRanks = new List<int>();

	// Token: 0x04000B2B RID: 2859
	public List<Drone> drones = new List<Drone>();
}
