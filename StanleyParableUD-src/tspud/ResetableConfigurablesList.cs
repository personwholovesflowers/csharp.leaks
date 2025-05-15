using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000171 RID: 369
[CreateAssetMenu(fileName = "ResetableConfigurablesList.asset", menuName = "Data/Resetable Configurables List")]
public class ResetableConfigurablesList : ScriptableObject
{
	// Token: 0x06000896 RID: 2198 RVA: 0x00028930 File Offset: 0x00026B30
	public void ResetAllConfigurables()
	{
		foreach (Configurable configurable in this.allConfigurables)
		{
			configurable.Init();
			configurable.SetNewConfiguredValue(configurable.CreateDefaultLiveData());
			configurable.SaveToDiskAll();
		}
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x00028994 File Offset: 0x00026B94
	public void ResetAll()
	{
		this.ResetAllConfigurables();
		this.DestroyAllGameObjects();
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x000289A4 File Offset: 0x00026BA4
	private void DestroyAllGameObjects()
	{
		Singleton<ChoreoMaster>.Instance.DropAll();
		StanleyController.Instance.gameObject.SetActive(false);
		Singleton<GameMaster>.Instance.ClosePauseMenu(true);
		Singleton<GameMaster>.Instance.ReInit();
		Singleton<GameMaster>.Instance.ChangeLevel("Settings_UD_MASTER", true);
	}

	// Token: 0x04000865 RID: 2149
	[Header("Right-Click and Reimport any configurables that do not show up on the list to allow to find them")]
	[InspectorButton("FindAllConfigurables", null)]
	public bool findAll;

	// Token: 0x04000866 RID: 2150
	public List<Configurable> allConfigurables;

	// Token: 0x04000867 RID: 2151
	public List<Configurable> excludeTheseConfigurablesInFind;
}
