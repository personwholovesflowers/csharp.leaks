using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using plog;
using Sandbox;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020003B5 RID: 949
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class SandboxSaver : MonoSingleton<SandboxSaver>
{
	// Token: 0x1700018E RID: 398
	// (get) Token: 0x06001595 RID: 5525 RVA: 0x000AEFEF File Offset: 0x000AD1EF
	public static string SavePath
	{
		get
		{
			return Path.Combine(GameProgressSaver.BaseSavePath, "Sandbox");
		}
	}

	// Token: 0x06001596 RID: 5526 RVA: 0x000AF000 File Offset: 0x000AD200
	private static void SetupDirs()
	{
		if (!Directory.Exists(SandboxSaver.SavePath))
		{
			Directory.CreateDirectory(SandboxSaver.SavePath);
		}
	}

	// Token: 0x06001597 RID: 5527 RVA: 0x000AF01C File Offset: 0x000AD21C
	public string[] ListSaves()
	{
		SandboxSaver.SetupDirs();
		return (from f in new DirectoryInfo(SandboxSaver.SavePath).GetFileSystemInfos()
			orderby f.LastWriteTime descending
			select Path.GetFileNameWithoutExtension(f.Name)).ToArray<string>();
	}

	// Token: 0x06001598 RID: 5528 RVA: 0x000AF08C File Offset: 0x000AD28C
	public void QuickSave()
	{
		this.Save(string.Format("{0}-{1}-{2} {3}-{4}-{5}", new object[]
		{
			DateTime.Now.Year,
			DateTime.Now.Month,
			DateTime.Now.Day,
			DateTime.Now.Hour,
			DateTime.Now.Minute,
			DateTime.Now.Second
		}));
	}

	// Token: 0x06001599 RID: 5529 RVA: 0x000AF130 File Offset: 0x000AD330
	public void QuickLoad()
	{
		string[] array = this.ListSaves();
		if (array.Length != 0)
		{
			this.Load(array[0]);
		}
	}

	// Token: 0x0600159A RID: 5530 RVA: 0x000AF154 File Offset: 0x000AD354
	public void Delete(string name)
	{
		SandboxSaver.SetupDirs();
		string text = Path.Combine(SandboxSaver.SavePath, name + ".pitr");
		if (File.Exists(text))
		{
			File.Delete(text);
		}
	}

	// Token: 0x0600159B RID: 5531 RVA: 0x000AF18A File Offset: 0x000AD38A
	public void Save(string name)
	{
		SandboxSaver.SetupDirs();
		this.activeSave = name;
		MonoSingleton<CheatsManager>.Instance.RefreshCheatStates();
		SandboxSaver.CreateSaveAndWrite(name);
	}

	// Token: 0x0600159C RID: 5532 RVA: 0x000AF1A8 File Offset: 0x000AD3A8
	public void Load(string name)
	{
		SandboxSaver.Log.Info("Loading save: " + name, null, null, null);
		SandboxSaver.SetupDirs();
		SandboxSaver.Clear();
		this.activeSave = name;
		MonoSingleton<CheatsManager>.Instance.RefreshCheatStates();
		this.RebuildObjectList();
		SandboxSaveData sandboxSaveData = JsonConvert.DeserializeObject<SandboxSaveData>(File.ReadAllText(Path.Combine(SandboxSaver.SavePath, name + ".pitr")));
		SandboxSaver.Log.Fine(string.Format("Loaded {0} blocks\nLoaded {1} props", sandboxSaveData.Blocks.Length, sandboxSaveData.Props.Length), null, null, null);
		SandboxSaver.Log.Fine("Save Version: " + sandboxSaveData.SaveVersion.ToString(), null, null, null);
		Vector3? vector = null;
		Vector3 position = MonoSingleton<NewMovement>.Instance.transform.position;
		foreach (SavedProp savedProp in sandboxSaveData.Props)
		{
			this.RecreateProp(savedProp, sandboxSaveData.SaveVersion > 1);
			if (!(savedProp.ObjectIdentifier != "ultrakill.spawn-point"))
			{
				if (vector == null)
				{
					vector = new Vector3?(savedProp.Position.ToVector3());
				}
				else if (Vector3.Distance(position, savedProp.Position.ToVector3()) < Vector3.Distance(position, vector.Value))
				{
					vector = new Vector3?(savedProp.Position.ToVector3());
				}
			}
		}
		if (vector != null)
		{
			MonoSingleton<NewMovement>.Instance.transform.position = vector.Value;
			MonoSingleton<NewMovement>.Instance.rb.velocity = Vector3.zero;
		}
		foreach (SavedBlock savedBlock in sandboxSaveData.Blocks)
		{
			this.RecreateBlock(savedBlock);
		}
		MonoSingleton<SandboxNavmesh>.Instance.Rebake();
		List<SandboxEnemy> list = new List<SandboxEnemy>();
		foreach (SavedEnemy savedEnemy in sandboxSaveData.Enemies)
		{
			SandboxEnemy sandboxEnemy = this.RecreateEnemy(savedEnemy, sandboxSaveData.SaveVersion > 1);
			sandboxEnemy.Pause(false);
			list.Add(sandboxEnemy);
		}
		base.StartCoroutine(this.PostLoadAndBake(list));
	}

	// Token: 0x0600159D RID: 5533 RVA: 0x000AF3D7 File Offset: 0x000AD5D7
	private IEnumerator PostLoadAndBake(List<SandboxEnemy> enemies)
	{
		yield return new WaitForEndOfFrame();
		List<SandboxEnemy> enemiesToFreezeBack = new List<SandboxEnemy>();
		foreach (SandboxEnemy sandboxEnemy in enemies)
		{
			bool frozen = sandboxEnemy.frozen;
			sandboxEnemy.Resume();
			if (frozen)
			{
				enemiesToFreezeBack.Add(sandboxEnemy);
			}
		}
		yield return new WaitForEndOfFrame();
		using (List<SandboxEnemy>.Enumerator enumerator = enemiesToFreezeBack.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SandboxEnemy sandboxEnemy2 = enumerator.Current;
				sandboxEnemy2.Pause(true);
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x0600159E RID: 5534 RVA: 0x000AF3E8 File Offset: 0x000AD5E8
	public SandboxEnemy RecreateEnemy(SavedGeneric genericObject, bool newSizing)
	{
		SpawnableObject spawnableObject;
		if (!this.registeredObjects.TryGetValue(genericObject.ObjectIdentifier, out spawnableObject))
		{
			SandboxSaver.Log.Error(genericObject.ObjectIdentifier + " missing from registered objects", null, null, null);
			return null;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(spawnableObject.gameObject);
		gameObject.transform.position = genericObject.Position.ToVector3();
		if (!newSizing)
		{
			gameObject.transform.localScale = genericObject.Scale.ToVector3();
		}
		KeepInBounds keepInBounds;
		if (gameObject.TryGetComponent<KeepInBounds>(out keepInBounds))
		{
			keepInBounds.ForceApproveNewPosition();
		}
		SandboxEnemy sandboxEnemy = gameObject.AddComponent<SandboxEnemy>();
		sandboxEnemy.sourceObject = this.registeredObjects[genericObject.ObjectIdentifier];
		sandboxEnemy.enemyId.checkingSpawnStatus = false;
		sandboxEnemy.RestoreRadiance(((SavedEnemy)genericObject).Radiance);
		SavedPhysical savedPhysical = genericObject as SavedPhysical;
		if (savedPhysical != null && savedPhysical.Kinematic)
		{
			sandboxEnemy.Pause(true);
		}
		if (newSizing)
		{
			sandboxEnemy.SetSize(genericObject.Scale.ToVector3());
		}
		sandboxEnemy.disallowManipulation = genericObject.DisallowManipulation;
		sandboxEnemy.disallowFreezing = genericObject.DisallowFreezing;
		this.ApplyData(gameObject, genericObject.Data);
		if (MonoSingleton<SandboxNavmesh>.Instance)
		{
			MonoSingleton<SandboxNavmesh>.Instance.EnsurePositionWithinBounds(gameObject.transform.position);
		}
		return sandboxEnemy;
	}

	// Token: 0x0600159F RID: 5535 RVA: 0x000AF528 File Offset: 0x000AD728
	private void RecreateProp(SavedProp prop, bool newSizing)
	{
		SpawnableObject spawnableObject;
		if (!this.registeredObjects.TryGetValue(prop.ObjectIdentifier, out spawnableObject))
		{
			SandboxSaver.Log.Error(prop.ObjectIdentifier + " missing from registered objects", null, null, null);
			return;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(spawnableObject.gameObject);
		gameObject.transform.SetPositionAndRotation(prop.Position.ToVector3(), prop.Rotation.ToQuaternion());
		if (!newSizing)
		{
			gameObject.transform.localScale = prop.Scale.ToVector3();
		}
		SandboxProp component = gameObject.GetComponent<SandboxProp>();
		component.sourceObject = this.registeredObjects[prop.ObjectIdentifier];
		if (newSizing)
		{
			component.SetSize(prop.Scale.ToVector3());
		}
		if (prop.Kinematic)
		{
			component.Pause(true);
		}
		else
		{
			component.Resume();
		}
		component.disallowManipulation = prop.DisallowManipulation;
		component.disallowFreezing = prop.DisallowFreezing;
		this.ApplyData(gameObject, prop.Data);
	}

	// Token: 0x060015A0 RID: 5536 RVA: 0x000AF61C File Offset: 0x000AD81C
	private void RecreateBlock(SavedBlock block)
	{
		SpawnableObject spawnableObject;
		if (!this.registeredObjects.TryGetValue(block.ObjectIdentifier, out spawnableObject))
		{
			SandboxSaver.Log.Error(block.ObjectIdentifier + " missing from registered objects", null, null, null);
			return;
		}
		GameObject gameObject = SandboxUtils.CreateFinalBlock(spawnableObject, block.Position.ToVector3(), block.BlockSize.ToVector3(), spawnableObject.isWater);
		gameObject.transform.rotation = block.Rotation.ToQuaternion();
		SandboxProp component = gameObject.GetComponent<SandboxProp>();
		component.sourceObject = this.registeredObjects[block.ObjectIdentifier];
		if (block.Kinematic)
		{
			component.Pause(true);
		}
		else
		{
			component.Resume();
		}
		component.disallowManipulation = block.DisallowManipulation;
		component.disallowFreezing = block.DisallowFreezing;
		this.ApplyData(gameObject, block.Data);
	}

	// Token: 0x060015A1 RID: 5537 RVA: 0x000AF6F0 File Offset: 0x000AD8F0
	private void ApplyData(GameObject go, SavedAlterData[] data)
	{
		if (data == null)
		{
			return;
		}
		IAlter[] componentsInChildren = go.GetComponentsInChildren<IAlter>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			IAlter alterComponent = componentsInChildren[i];
			if (alterComponent.alterKey != null)
			{
				if (!data.Select((SavedAlterData d) => d.Key).Contains(alterComponent.alterKey))
				{
					SandboxSaver.Log.Warning("No data for " + alterComponent.alterKey + " on " + go.name, null, null, null);
				}
				else
				{
					SavedAlterData savedAlterData = data.FirstOrDefault((SavedAlterData d) => d.Key == alterComponent.alterKey);
					if (savedAlterData != null)
					{
						SavedAlterOption[] options2 = savedAlterData.Options;
						int j = 0;
						while (j < options2.Length)
						{
							SavedAlterOption options = options2[j];
							if (options.BoolValue == null)
							{
								goto IL_013F;
							}
							IAlterOptions<bool> alterOptions = alterComponent as IAlterOptions<bool>;
							if (alterOptions == null)
							{
								goto IL_013F;
							}
							AlterOption<bool> alterOption = alterOptions.options.FirstOrDefault((AlterOption<bool> o) => o.key == options.Key);
							if (alterOption != null)
							{
								Action<bool> callback = alterOption.callback;
								if (callback == null)
								{
									goto IL_013F;
								}
								callback(options.BoolValue.Value);
								goto IL_013F;
							}
							IL_020E:
							j++;
							continue;
							IL_013F:
							if (options.FloatValue != null)
							{
								IAlterOptions<float> alterOptions2 = alterComponent as IAlterOptions<float>;
								if (alterOptions2 != null)
								{
									AlterOption<float> alterOption2 = alterOptions2.options.FirstOrDefault((AlterOption<float> o) => o.key == options.Key);
									if (alterOption2 == null)
									{
										goto IL_020E;
									}
									Action<float> callback2 = alterOption2.callback;
									if (callback2 != null)
									{
										callback2(options.FloatValue.Value);
									}
								}
							}
							if (options.IntValue == null)
							{
								goto IL_020E;
							}
							IAlterOptions<int> alterOptions3 = alterComponent as IAlterOptions<int>;
							if (alterOptions3 == null)
							{
								goto IL_020E;
							}
							AlterOption<int> alterOption3 = alterOptions3.options.FirstOrDefault((AlterOption<int> o) => o.key == options.Key);
							if (alterOption3 == null)
							{
								goto IL_020E;
							}
							Action<int> callback3 = alterOption3.callback;
							if (callback3 == null)
							{
								goto IL_020E;
							}
							callback3(options.IntValue.Value);
							goto IL_020E;
						}
					}
				}
			}
		}
	}

	// Token: 0x060015A2 RID: 5538 RVA: 0x000AF92C File Offset: 0x000ADB2C
	public void RebuildObjectList()
	{
		if (this.registeredObjects == null)
		{
			this.registeredObjects = new Dictionary<string, SpawnableObject>();
		}
		this.registeredObjects.Clear();
		this.RegisterObjects(this.objects.objects);
		this.RegisterObjects(this.objects.enemies);
		this.RegisterObjects(this.objects.sandboxTools);
		this.RegisterObjects(this.objects.sandboxObjects);
		this.RegisterObjects(this.objects.specialSandbox);
	}

	// Token: 0x060015A3 RID: 5539 RVA: 0x000AF9AC File Offset: 0x000ADBAC
	private void RegisterObjects(SpawnableObject[] objs)
	{
		foreach (SpawnableObject spawnableObject in objs)
		{
			if (!string.IsNullOrEmpty(spawnableObject.identifier) && !this.registeredObjects.ContainsKey(spawnableObject.identifier))
			{
				this.registeredObjects.Add(spawnableObject.identifier, spawnableObject);
			}
		}
	}

	// Token: 0x060015A4 RID: 5540 RVA: 0x000AFA00 File Offset: 0x000ADC00
	public static void Clear()
	{
		DefaultSandboxCheckpoint instance = MonoSingleton<DefaultSandboxCheckpoint>.Instance;
		if (instance == null)
		{
			MonoSingleton<StatsManager>.Instance.currentCheckPoint = null;
		}
		else
		{
			MonoSingleton<StatsManager>.Instance.currentCheckPoint = instance.checkpoint;
		}
		foreach (SandboxSpawnableInstance sandboxSpawnableInstance in Object.FindObjectsOfType<SandboxSpawnableInstance>())
		{
			if (sandboxSpawnableInstance.enabled)
			{
				Object.Destroy(sandboxSpawnableInstance.gameObject);
			}
		}
		Resources.UnloadUnusedAssets();
		MonoSingleton<SandboxNavmesh>.Instance.ResetSizeToDefault();
		MonoSingleton<SandboxSaver>.Instance.activeSave = null;
		MonoSingleton<CheatsManager>.Instance.RefreshCheatStates();
	}

	// Token: 0x060015A5 RID: 5541 RVA: 0x000AFA8C File Offset: 0x000ADC8C
	private static void CreateSaveAndWrite(string name)
	{
		SandboxSaver.Log.Info("Creating save", null, null, null);
		SandboxProp[] array = Object.FindObjectsOfType<SandboxProp>();
		SandboxSaver.Log.Fine(string.Format("{0} props found", array.Length), null, null, null);
		BrushBlock[] array2 = Object.FindObjectsOfType<BrushBlock>();
		SandboxSaver.Log.Fine(string.Format("{0} procedural blocks found", array2.Length), null, null, null);
		SandboxEnemy[] array3 = Object.FindObjectsOfType<SandboxEnemy>();
		SandboxSaver.Log.Fine(string.Format("{0} sandbox enemies found", array3.Length), null, null, null);
		List<SavedBlock> list = new List<SavedBlock>();
		foreach (BrushBlock brushBlock in array2)
		{
			if (brushBlock.enabled)
			{
				SandboxSaver.Log.Fine(string.Format("Position: {0}\nRotation: {1}\nSize: {2}\nType: {3}", new object[]
				{
					brushBlock.transform.position,
					brushBlock.transform.rotation,
					brushBlock.DataSize,
					brushBlock.Type
				}), null, null, null);
				list.Add(brushBlock.SaveBrushBlock());
			}
		}
		List<SavedProp> list2 = new List<SavedProp>();
		foreach (SandboxProp sandboxProp in array)
		{
			if (!sandboxProp.GetComponent<BrushBlock>() && sandboxProp.enabled)
			{
				SandboxSaver.Log.Fine(string.Format("Position: {0}\nRotation: {1}", sandboxProp.transform.position, sandboxProp.transform.rotation), null, null, null);
				list2.Add(sandboxProp.SaveProp());
			}
		}
		List<SavedEnemy> list3 = new List<SavedEnemy>();
		foreach (SandboxEnemy sandboxEnemy in array3)
		{
			if (sandboxEnemy.enabled)
			{
				SavedEnemy savedEnemy = sandboxEnemy.SaveEnemy();
				if (savedEnemy != null)
				{
					list3.Add(savedEnemy);
				}
			}
		}
		string text = JsonConvert.SerializeObject(new SandboxSaveData
		{
			MapName = SceneManager.GetActiveScene().name,
			Blocks = list.ToArray(),
			Props = list2.ToArray(),
			Enemies = list3.ToArray()
		});
		File.WriteAllText(Path.Combine(SandboxSaver.SavePath, name + ".pitr"), text);
	}

	// Token: 0x04001DE9 RID: 7657
	private static readonly global::plog.Logger Log = new global::plog.Logger("SandboxSaver");

	// Token: 0x04001DEA RID: 7658
	public const string SaveExtension = ".pitr";

	// Token: 0x04001DEB RID: 7659
	[SerializeField]
	private SpawnableObjectsDatabase objects;

	// Token: 0x04001DEC RID: 7660
	private Dictionary<string, SpawnableObject> registeredObjects;

	// Token: 0x04001DED RID: 7661
	public string activeSave;
}
