using System;
using UnityEngine;

// Token: 0x02000299 RID: 665
public static class LayerMaskDefaults
{
	// Token: 0x06000EA5 RID: 3749 RVA: 0x0006C750 File Offset: 0x0006A950
	public static bool IsMatchingLayer(int otherLayer, LMD layerMask)
	{
		return (LayerMaskDefaults.Get(layerMask) & (1 << otherLayer)) != 0;
	}

	// Token: 0x06000EA6 RID: 3750 RVA: 0x0006C768 File Offset: 0x0006A968
	public static LayerMask Get(LMD lmd)
	{
		LayerMask layerMask = default(LayerMask);
		switch (lmd)
		{
		case LMD.Enemies:
			layerMask |= 1024;
			layerMask |= 2048;
			break;
		case LMD.Environment:
			layerMask |= 256;
			layerMask |= 16777216;
			layerMask |= 64;
			layerMask |= 128;
			break;
		case LMD.EnvironmentAndBigEnemies:
			layerMask |= 256;
			layerMask |= 16777216;
			layerMask |= 64;
			layerMask |= 128;
			layerMask |= 2048;
			break;
		case LMD.EnemiesAndEnvironment:
			layerMask |= 256;
			layerMask |= 16777216;
			layerMask |= 64;
			layerMask |= 128;
			layerMask |= 1024;
			layerMask |= 2048;
			break;
		case LMD.EnemiesAndPlayer:
			layerMask |= 4;
			layerMask |= 1024;
			layerMask |= 2048;
			break;
		case LMD.EnvironmentAndPlayer:
			layerMask |= 4;
			layerMask |= 256;
			layerMask |= 16777216;
			layerMask |= 64;
			layerMask |= 128;
			break;
		case LMD.EnemiesEnvironmentAndPlayer:
			layerMask |= 4;
			layerMask |= 256;
			layerMask |= 16777216;
			layerMask |= 64;
			layerMask |= 128;
			layerMask |= 1024;
			layerMask |= 2048;
			break;
		case LMD.BigEnemiesEnvironmentAndPlayer:
			layerMask |= 4;
			layerMask |= 256;
			layerMask |= 16777216;
			layerMask |= 64;
			layerMask |= 128;
			layerMask |= 2048;
			break;
		case LMD.Player:
			layerMask |= 4;
			break;
		}
		return layerMask;
	}
}
