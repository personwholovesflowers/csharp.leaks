using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200017C RID: 380
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class EndlessGrid : MonoSingleton<EndlessGrid>
{
	// Token: 0x170000BF RID: 191
	// (get) Token: 0x0600074E RID: 1870 RVA: 0x0002F39B File Offset: 0x0002D59B
	private ArenaPattern[] CurrentPatternPool
	{
		get
		{
			if (!this.customPatternMode)
			{
				return this.patterns;
			}
			return this.customPatterns;
		}
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x0002F3B2 File Offset: 0x0002D5B2
	public void TrySetupStaticGridMesh()
	{
		if (this.incompleteBlocks != 0 || this.incompletePrefabs != 0)
		{
			return;
		}
		this.SetupStaticGridMesh();
	}

	// Token: 0x06000750 RID: 1872 RVA: 0x0002F3CC File Offset: 0x0002D5CC
	public void SetupStaticGridMesh()
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		if (this.combinedGridStaticObject == null)
		{
			this.combinedGridStaticObject = new GameObject("Combined Static Mesh");
			this.combinedGridStaticObject.transform.parent = base.transform;
			this.combinedGridStaticObject.layer = LayerMask.NameToLayer("Outdoors");
			this.combinedGridStaticMeshRenderer = this.combinedGridStaticObject.AddComponent<MeshRenderer>();
			this.combinedGridStaticMeshFilter = this.combinedGridStaticObject.AddComponent<MeshFilter>();
		}
		this.combinedGridStaticObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
		this.combinedGridStaticObject.transform.localScale = Vector3.one;
		if (this.combinedGridStaticMesh == null)
		{
			this.combinedGridStaticMesh = new Mesh();
		}
		this.combinedGridStaticMesh.Clear();
		List<Mesh> list = new List<Mesh>();
		List<Material> list2 = new List<Material>();
		bool flag = false;
		for (int i = 0; i < this.cubes[0][0].MeshRenderer.sharedMaterials.Length; i++)
		{
			Material material = this.cubes[0][0].MeshRenderer.sharedMaterials[i];
			if (!list2.Contains(material))
			{
				list2.Add(material);
			}
		}
		for (int j = 0; j < list2.Count; j++)
		{
			Mesh mesh = new Mesh();
			List<CombineInstance> list3 = new List<CombineInstance>();
			for (int k = 0; k < 16; k++)
			{
				for (int l = 0; l < 16; l++)
				{
					EndlessCube endlessCube = this.cubes[k][l];
					if (!(endlessCube == null))
					{
						list3.Add(new CombineInstance
						{
							transform = endlessCube.MeshRenderer.localToWorldMatrix,
							mesh = endlessCube.MeshFilter.sharedMesh,
							subMeshIndex = j
						});
						endlessCube.MeshRenderer.enabled = false;
					}
				}
			}
			if (j == 1)
			{
				using (List<GameObject>.Enumerator enumerator = this.spawnedPrefabs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EndlessStairs endlessStairs;
						if (enumerator.Current.TryGetComponent<EndlessStairs>(out endlessStairs))
						{
							if (endlessStairs.ActivateFirst)
							{
								if (!flag)
								{
									foreach (Material material2 in endlessStairs.PrimaryMeshRenderer.sharedMaterials)
									{
										if (!list2.Contains(material2))
										{
											list2.Add(material2);
										}
									}
									flag = true;
								}
								list3.Add(new CombineInstance
								{
									transform = endlessStairs.PrimaryMeshRenderer.localToWorldMatrix,
									mesh = endlessStairs.PrimaryMeshFilter.sharedMesh
								});
								endlessStairs.PrimaryMeshRenderer.enabled = false;
							}
							if (endlessStairs.ActivateSecond)
							{
								if (!flag)
								{
									foreach (Material material3 in endlessStairs.SecondaryMeshRenderer.sharedMaterials)
									{
										if (!list2.Contains(material3))
										{
											list2.Add(material3);
										}
									}
									flag = true;
								}
								list3.Add(new CombineInstance
								{
									transform = endlessStairs.SecondaryMeshRenderer.localToWorldMatrix,
									mesh = endlessStairs.SecondaryMeshFilter.sharedMesh
								});
								endlessStairs.SecondaryMeshRenderer.enabled = false;
							}
						}
					}
				}
			}
			mesh.CombineMeshes(list3.ToArray(), true, true);
			list.Add(mesh);
		}
		CombineInstance[] array2 = new CombineInstance[list.Count];
		for (int n = 0; n < list.Count; n++)
		{
			array2[n] = new CombineInstance
			{
				mesh = list[n]
			};
		}
		this.combinedGridStaticMesh.CombineMeshes(array2, false, false);
		this.combinedGridStaticMesh.Optimize();
		this.combinedGridStaticMesh.RecalculateBounds();
		this.combinedGridStaticMesh.RecalculateNormals();
		this.combinedGridStaticMesh.UploadMeshData(false);
		Material[] array3 = list2.ToArray();
		this.combinedGridStaticMeshRenderer.sharedMaterials = array3;
		this.combinedGridStaticObject.SetActive(true);
		this.combinedGridStaticMeshFilter.sharedMesh = this.combinedGridStaticMesh;
		for (int num = 0; num < list.Count; num++)
		{
			Object.Destroy(list[num]);
		}
		list.Clear();
		stopwatch.Stop();
		Debug.Log(string.Format("Combined arena mesh in {0} ms", stopwatch.ElapsedMilliseconds));
		PhysicsSceneStateEnforcer physicsSceneStateEnforcer;
		if (this.combinedGridStaticObject.TryGetComponent<PhysicsSceneStateEnforcer>(out physicsSceneStateEnforcer))
		{
			physicsSceneStateEnforcer.ForceUpdate();
			return;
		}
		Transform transform = this.combinedGridStaticObject.transform;
		MonoSingleton<SceneHelper>.Instance.AddMeshToPhysicsScene(this.combinedGridStaticMesh, array3, transform.position, transform.rotation, transform.lossyScale, this.combinedGridStaticObject.layer, this.combinedGridStaticObject);
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x0002F8A8 File Offset: 0x0002DAA8
	private void Start()
	{
		this.nms = base.GetComponent<NavMeshSurface>();
		this.anw = base.GetComponent<ActivateNextWave>();
		this.gz = GoreZone.ResolveGoreZone(base.transform);
		this.cubes = new EndlessCube[16][];
		for (int i = 0; i < 16; i++)
		{
			this.cubes[i] = new EndlessCube[16];
			for (int j = 0; j < 16; j++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.gridCube, base.transform, true);
				gameObject.SetActive(true);
				gameObject.transform.localPosition = new Vector3((float)i * this.offset, 0f, (float)j * this.offset);
				this.cubes[i][j] = gameObject.GetComponent<EndlessCube>();
				this.cubes[i][j].positionOnGrid = new Vector2Int(i, j);
			}
		}
		for (int k = 0; k < this.CurrentPatternPool.Length; k++)
		{
			ArenaPattern arenaPattern = this.CurrentPatternPool[k];
			int num = Random.Range(k, this.CurrentPatternPool.Length);
			this.CurrentPatternPool[k] = this.CurrentPatternPool[num];
			this.CurrentPatternPool[num] = arenaPattern;
		}
		this.crorea = MonoSingleton<CrowdReactions>.Instance;
		if (this.crorea != null)
		{
			this.crowdReactions = true;
		}
		this.ShuffleDecks();
		PresenceController.UpdateCyberGrindWave(0);
		this.mats = base.GetComponentInChildren<MeshRenderer>().sharedMaterials;
		foreach (Material material in this.mats)
		{
			material.SetColor(UKShaderProperties.EmissiveColor, Color.blue);
			material.SetFloat(UKShaderProperties.EmissiveIntensity, 0.2f * this.glowMultiplier);
			material.SetFloat("_PCGamerMode", 0f);
			material.SetFloat("_GradientScale", 2f);
			material.SetFloat("_GradientFalloff", 5f);
			material.SetFloat("_GradientSpeed", 10f);
			material.SetVector("_WorldOffset", new Vector4(0f, 0f, 62.5f, 0f));
			this.targetColor = Color.blue;
		}
		this.TrySetupStaticGridMesh();
		int? highestWaveForDifficulty = WaveUtils.GetHighestWaveForDifficulty(MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0));
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("cyberGrind.startingWave", 0);
		this.startWave = (WaveUtils.IsWaveSelectable(@int, highestWaveForDifficulty.GetValueOrDefault()) ? @int : 0);
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x0002FB14 File Offset: 0x0002DD14
	private void LastEnemyMode()
	{
		this.lastEnemyMode = true;
		EnemyIdentifier[] componentsInChildren = base.GetComponentsInChildren<EnemyIdentifier>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!componentsInChildren[i].dead)
			{
				this.enemyToTrack = componentsInChildren[i].transform;
				break;
			}
		}
		foreach (Material material in this.mats)
		{
			if (this.currentWave < 20)
			{
				this.currentGlow = 0.5f;
			}
			else
			{
				this.currentGlow = 1f;
			}
			material.SetFloat(UKShaderProperties.EmissiveIntensity, this.currentGlow * this.glowMultiplier);
			material.SetFloat(EndlessGrid.GradientScale, 0.5f);
			material.SetFloat(EndlessGrid.GradientSpeed, 25f);
		}
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x0002FBC8 File Offset: 0x0002DDC8
	private void NormalMode()
	{
		this.lastEnemyMode = false;
		foreach (Material material in this.mats)
		{
			if (this.currentWave < 20)
			{
				this.currentGlow = 0.2f;
			}
			else
			{
				this.currentGlow = 0.5f;
			}
			material.SetFloat(UKShaderProperties.EmissiveIntensity, this.currentGlow * this.glowMultiplier);
			material.SetFloat(EndlessGrid.GradientScale, 2f);
			material.SetFloat(EndlessGrid.GradientFalloff, 5f);
			material.SetFloat(EndlessGrid.GradientSpeed, 10f);
			material.SetVector(EndlessGrid.WorldOffset, new Vector4(0f, 0f, 62.5f, 0f));
		}
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x0002FC88 File Offset: 0x0002DE88
	public void UpdateGlow()
	{
		Material[] array = this.mats;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetFloat(UKShaderProperties.EmissiveIntensity, this.currentGlow * this.glowMultiplier);
		}
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x0002FCC4 File Offset: 0x0002DEC4
	private void Update()
	{
		if (this.lastEnemyMode)
		{
			if (this.anw.deadEnemies != this.enemyAmount - 1)
			{
				this.NormalMode();
			}
			else if (this.enemyToTrack)
			{
				Material[] array = this.mats;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetVector(EndlessGrid.WorldOffset, new Vector4(this.enemyToTrack.position.x, this.enemyToTrack.position.y, this.enemyToTrack.position.z, 0f));
				}
			}
		}
		else if (!this.lastEnemyMode && this.anw.deadEnemies == this.enemyAmount - 1)
		{
			this.LastEnemyMode();
		}
		if (this.anw.deadEnemies >= this.enemyAmount && !this.testMode)
		{
			this.anw.deadEnemies = 0;
			this.enemyAmount = 999;
			this.tempEnemyAmount = 0;
			base.Invoke("NextWave", 1f);
			if (this.crowdReactions)
			{
				if (this.crorea == null)
				{
					this.crorea = MonoSingleton<CrowdReactions>.Instance;
				}
				if (this.crorea.enabled)
				{
					this.crorea.React(this.crorea.cheerLong);
				}
				else
				{
					this.crowdReactions = false;
				}
			}
		}
		foreach (Material material in this.mats)
		{
			if (material.GetColor(UKShaderProperties.EmissiveColor) != this.targetColor)
			{
				material.SetColor(UKShaderProperties.EmissiveColor, Color.Lerp(material.GetColor(UKShaderProperties.EmissiveColor), this.targetColor, Time.deltaTime));
			}
		}
		if (this.anw.deadEnemies > this.tempEnemyAmount)
		{
			this.anw.deadEnemies = this.tempEnemyAmount;
		}
		this.waveNumberText.text = this.currentWave.ToString() ?? "";
		this.enemiesLeftText.text = (this.tempEnemyAmount - this.anw.deadEnemies).ToString() ?? "";
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x0002FEF0 File Offset: 0x0002E0F0
	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player"))
		{
			return;
		}
		base.GetComponent<Collider>().enabled = false;
		if (this.startWave > 0)
		{
			this.currentWave = this.startWave - 1;
			for (int i = 1; i <= this.currentWave; i++)
			{
				this.maxPoints += 3 + i / 3;
			}
		}
		this.SetGlowColor(true);
		this.waveNumberText.transform.parent.parent.gameObject.SetActive(true);
		this.ShuffleDecks();
		this.NextWave();
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x0002FF84 File Offset: 0x0002E184
	private void NextWave()
	{
		this.currentPatternNum++;
		this.currentWave++;
		this.maxPoints += 3 + this.currentWave / 3;
		this.points = this.maxPoints;
		if (!this.nmov)
		{
			this.nmov = MonoSingleton<NewMovement>.Instance;
		}
		if (this.nmov.hp > 0)
		{
			this.nmov.ResetHardDamage();
			this.nmov.exploded = false;
			this.nmov.GetHealth(999, true, false, true);
			this.nmov.FullStamina();
		}
		if (!this.wc)
		{
			this.wc = MonoSingleton<WeaponCharges>.Instance;
		}
		this.wc.MaxCharges();
		if (this.gz)
		{
			this.gz.ResetGibs();
		}
		if (MonoSingleton<ObjectTracker>.Instance && MonoSingleton<ObjectTracker>.Instance.landmineList.Count > 0)
		{
			for (int i = MonoSingleton<ObjectTracker>.Instance.landmineList.Count - 1; i >= 0; i--)
			{
				if (!(MonoSingleton<ObjectTracker>.Instance.landmineList[i] == null))
				{
					Object.Destroy(MonoSingleton<ObjectTracker>.Instance.landmineList[i].gameObject);
				}
			}
		}
		Projectile[] array = Object.FindObjectsOfType<Projectile>();
		if (array.Length != 0)
		{
			foreach (Projectile projectile in array)
			{
				if (projectile != null && !projectile.friendly && !projectile.playerBullet)
				{
					Object.Destroy(projectile.gameObject);
				}
			}
		}
		this.SetGlowColor(false);
		if (this.currentPatternNum >= this.CurrentPatternPool.Length)
		{
			this.currentPatternNum = 0;
			this.ShuffleDecks();
		}
		foreach (GameObject gameObject in this.spawnedPrefabs)
		{
			gameObject.GetComponent<EndlessPrefabAnimator>().reverse = true;
		}
		this.spawnedPrefabs.Clear();
		this.incompletePrefabs = 0;
		PresenceController.UpdateCyberGrindWave(this.currentWave);
		if (this.CurrentPatternPool.Length == 0)
		{
			base.Invoke("DisplayNoPatternWarning", 2f);
		}
		if (this.CurrentPatternPool.Length <= this.currentPatternNum)
		{
			return;
		}
		this.LoadPattern(this.CurrentPatternPool[this.currentPatternNum]);
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x000301E4 File Offset: 0x0002E3E4
	private void DisplayNoPatternWarning()
	{
		MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("NO PATTERNS SELECTED.", "", "", 0, false, false, true);
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x00030204 File Offset: 0x0002E404
	private void LoadPattern(ArenaPattern pattern)
	{
		if (this.customPatternMode)
		{
			string[] array = pattern.name.Split('\\', StringSplitOptions.None);
			string text = array[array.Length - 1];
			text = text.Substring(0, text.Length - 4);
			text = text.Replace("CG_", "");
			text = text.Replace("Cg_", "");
			text = text.Replace("cg_", "");
			text = text.Replace('_', ' ');
			text = this.SplitCamelCase(text);
			text = text.Replace("  ", " ");
			text = text.ToUpper();
			MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage(text, "", "", 0, true, false, true);
		}
		string[] array2 = pattern.heights.Split('\n', StringSplitOptions.None);
		if (array2.Length != 16)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"[Heights] Pattern \"",
				pattern.name,
				"\" has ",
				array2.Length.ToString(),
				" rows instead of ",
				16.ToString()
			}));
			return;
		}
		for (int i = 0; i < array2.Length; i++)
		{
			int[] array3 = new int[16];
			if (array2[i].Length != 16)
			{
				if (array2[i].Length < 16)
				{
					Debug.LogError(string.Concat(new string[]
					{
						"[Heights] Pattern \"",
						pattern.name,
						"\" has ",
						array2[i].Length.ToString(),
						" elements in row ",
						i.ToString(),
						" instead of ",
						16.ToString()
					}));
					return;
				}
				int num = 0;
				bool flag = false;
				string text2 = "";
				int j = 0;
				while (j < array2[i].Length)
				{
					int num2;
					if (!int.TryParse(array2[i][j].ToString(), out num2) && array2[i][j] != '-')
					{
						goto IL_025D;
					}
					if (flag)
					{
						text2 += array2[i][j].ToString();
						goto IL_025D;
					}
					if (array3.Length <= num)
					{
						throw new Exception(string.Concat(new string[]
						{
							"Unable to parse pattern: ",
							pattern.name,
							" at row ",
							i.ToString(),
							" and column ",
							j.ToString()
						}));
					}
					array3[num] = num2;
					num++;
					IL_0327:
					j++;
					continue;
					IL_025D:
					if (array2[i][j] == '(')
					{
						if (flag)
						{
							Debug.LogError("[Heights] Pattern \"" + pattern.name + "\", Error while parsing extended numbers!");
							return;
						}
						flag = true;
					}
					if (array2[i][j] != ')')
					{
						goto IL_0327;
					}
					if (!flag)
					{
						Debug.LogError("[Heights] Pattern \"" + pattern.name + "\", Error while parsing extended numbers!");
						return;
					}
					if (array3.Length <= num)
					{
						throw new Exception(string.Concat(new string[]
						{
							"Unable to parse pattern: ",
							pattern.name,
							" at row ",
							i.ToString(),
							" and column ",
							j.ToString()
						}));
					}
					array3[num] = int.Parse(text2);
					flag = false;
					text2 = "";
					num++;
					goto IL_0327;
				}
				if (num != 16)
				{
					Debug.LogError(string.Concat(new string[]
					{
						"[Heights] Pattern \"",
						pattern.name,
						"\" has ",
						array2[i].Length.ToString(),
						" elements in row ",
						num.ToString(),
						" instead of ",
						16.ToString()
					}));
					return;
				}
			}
			else
			{
				for (int k = 0; k < array2[i].Length; k++)
				{
					array3[k] = int.Parse(array2[i][k].ToString());
				}
			}
			for (int l = 0; l < array3.Length; l++)
			{
				this.cubes[i][l].SetTarget((float)array3[l] * this.offset / 2f);
				this.cubes[i][l].blockedByPrefab = false;
				this.incompleteBlocks++;
			}
		}
		this.currentPattern = pattern;
		this.MakeGridDynamic();
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x00030660 File Offset: 0x0002E860
	public void MakeGridDynamic()
	{
		for (int i = 0; i < 16; i++)
		{
			for (int j = 0; j < 16; j++)
			{
				EndlessCube endlessCube = this.cubes[i][j];
				if (!(endlessCube == null))
				{
					endlessCube.MeshRenderer.enabled = true;
				}
			}
		}
		using (List<GameObject>.Enumerator enumerator = this.spawnedPrefabs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				EndlessStairs endlessStairs;
				if (enumerator.Current.TryGetComponent<EndlessStairs>(out endlessStairs))
				{
					if (endlessStairs.ActivateFirst)
					{
						endlessStairs.PrimaryMeshRenderer.enabled = true;
					}
					if (endlessStairs.ActivateSecond)
					{
						endlessStairs.SecondaryMeshRenderer.enabled = true;
					}
				}
			}
		}
		this.combinedGridStaticObject.SetActive(false);
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x00030728 File Offset: 0x0002E928
	private GameObject SpawnOnGrid(GameObject obj, Vector2 position, bool prefab = false, bool enemy = false, CyberPooledType poolType = CyberPooledType.None, bool radiant = false)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position + new Vector3(position.x * this.offset, 200f, position.y * this.offset), Vector3.down, out raycastHit, float.PositiveInfinity, 16777216))
		{
			float y = obj.transform.position.y;
			GameObject gameObject = null;
			bool flag = false;
			if (poolType != CyberPooledType.None && poolType == CyberPooledType.JumpPad && this.jumpPadSelector < this.jumpPadPool.Count)
			{
				CyberPooledPrefab cyberPooledPrefab = this.jumpPadPool[this.jumpPadSelector];
				gameObject = cyberPooledPrefab.gameObject;
				gameObject.transform.position = raycastHit.point + Vector3.up * y;
				cyberPooledPrefab.Animator.Start();
				cyberPooledPrefab.Animator.reverse = false;
				this.jumpPadSelector++;
				flag = true;
				gameObject.SetActive(true);
			}
			if (!flag)
			{
				gameObject = Object.Instantiate<GameObject>(obj, raycastHit.point + Vector3.up * y, obj.transform.rotation, base.transform);
			}
			if (prefab)
			{
				if (!flag && poolType == CyberPooledType.JumpPad)
				{
					CyberPooledPrefab cyberPooledPrefab2 = gameObject.AddComponent<CyberPooledPrefab>();
					this.jumpPadPool.Add(cyberPooledPrefab2);
					this.jumpPadSelector++;
					cyberPooledPrefab2.Index = this.jumpPadPool.Count - 1;
					cyberPooledPrefab2.Type = CyberPooledType.JumpPad;
					cyberPooledPrefab2.Animator = gameObject.GetComponent<EndlessPrefabAnimator>();
				}
				this.spawnedPrefabs.Add(gameObject);
				this.incompletePrefabs++;
			}
			if (enemy)
			{
				if (radiant && gameObject)
				{
					EnemyIdentifier componentInChildren = gameObject.GetComponentInChildren<EnemyIdentifier>(true);
					if (componentInChildren)
					{
						componentInChildren.HealthBuff();
						componentInChildren.SpeedBuff();
					}
				}
				this.spawnedEnemies.Add(gameObject);
			}
			return gameObject;
		}
		return null;
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x000308FE File Offset: 0x0002EAFE
	public GameObject[] GetSpawnedEnemies()
	{
		return this.spawnedEnemies.ToArray();
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x0003090C File Offset: 0x0002EB0C
	public void OneDone()
	{
		this.jumpPadSelector = 0;
		this.incompleteBlocks--;
		if (this.incompleteBlocks == 0)
		{
			this.projectilePositions.Clear();
			this.meleePositions.Clear();
			foreach (GameObject gameObject in this.spawnedEnemies)
			{
				if (gameObject != null)
				{
					gameObject.transform.SetParent(this.gz.gibZone, true);
					EnemyIdentifier component = gameObject.GetComponent<EnemyIdentifier>();
					if (component != null && !component.dead)
					{
						Object.Destroy(gameObject);
					}
				}
			}
			if (this.gz)
			{
				this.gz.ResetGibs();
			}
			this.spawnedEnemies.Clear();
			string[] array = this.currentPattern.prefabs.Split('\n', StringSplitOptions.None);
			if (array.Length != 16)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"[Prefabs] Pattern \"",
					this.currentPattern.name,
					"\" has ",
					array.Length.ToString(),
					" rows instead of ",
					16.ToString()
				}));
				return;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Length != 16)
				{
					Debug.LogError(string.Concat(new string[]
					{
						"[Prefabs] Pattern \"",
						this.currentPattern.name,
						"\" has ",
						array[i].Length.ToString(),
						" elements in row ",
						i.ToString(),
						" instead of ",
						16.ToString()
					}));
					return;
				}
				for (int j = 0; j < array[i].Length; j++)
				{
					if (array[i][j] != '0')
					{
						char c = array[i][j];
						if (c <= 'J')
						{
							if (c != 'H')
							{
								if (c == 'J')
								{
									this.cubes[i][j].blockedByPrefab = true;
									this.SpawnOnGrid(this.prefabs.jumpPad, new Vector2((float)i, (float)j), true, false, CyberPooledType.JumpPad, false);
								}
							}
							else if (this.massAntiBuffer == 0 && this.currentWave >= (this.hideousMasses + 1) * 10 && this.points > 70)
							{
								this.hideousMasses++;
								this.SpawnOnGrid(this.prefabs.hideousMass, new Vector2((float)i, (float)j), false, true, CyberPooledType.None, false);
								this.points -= 45;
							}
						}
						else if (c != 'n')
						{
							if (c != 'p')
							{
								if (c == 's')
								{
									this.cubes[i][j].blockedByPrefab = true;
									EndlessStairs endlessStairs;
									if (this.SpawnOnGrid(this.prefabs.stairs, new Vector2((float)i, (float)j), true, false, CyberPooledType.None, false).TryGetComponent<EndlessStairs>(out endlessStairs))
									{
										if (endlessStairs.PrimaryMeshRenderer != null && endlessStairs.ActivateFirst)
										{
											endlessStairs.PrimaryMeshRenderer.enabled = true;
										}
										if (endlessStairs.SecondaryMeshRenderer != null && endlessStairs.ActivateSecond)
										{
											endlessStairs.SecondaryMeshRenderer.enabled = true;
										}
									}
								}
							}
							else
							{
								this.projectilePositions.Add(new Vector2((float)i, (float)j));
							}
						}
						else
						{
							this.meleePositions.Add(new Vector2((float)i, (float)j));
						}
					}
				}
			}
			if (this.hideousMasses > 0)
			{
				this.massAntiBuffer += this.hideousMasses * 2;
			}
			else if (this.massAntiBuffer > 0)
			{
				this.massAntiBuffer--;
			}
			if (this.spawnedPrefabs.Count == 0)
			{
				this.GetEnemies();
			}
		}
		this.TrySetupStaticGridMesh();
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x00030D08 File Offset: 0x0002EF08
	public void OnePrefabDone()
	{
		this.incompletePrefabs--;
		if (this.incompletePrefabs == 0)
		{
			this.GetEnemies();
		}
		this.TrySetupStaticGridMesh();
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x00030D2C File Offset: 0x0002EF2C
	private void GetEnemies()
	{
		this.nms.BuildNavMesh();
		this.nvmhlpr.GenerateLinks(this.cubes);
		for (int i = 0; i < this.meleePositions.Count; i++)
		{
			Vector2 vector = this.meleePositions[i];
			int num = Random.Range(i, this.meleePositions.Count);
			this.meleePositions[i] = this.meleePositions[num];
			this.meleePositions[num] = vector;
		}
		for (int j = 0; j < this.projectilePositions.Count; j++)
		{
			Vector2 vector2 = this.projectilePositions[j];
			int num2 = Random.Range(j, this.projectilePositions.Count);
			this.projectilePositions[j] = this.projectilePositions[num2];
			this.projectilePositions[num2] = vector2;
		}
		this.tempEnemyAmount = 0;
		this.usedMeleePositions = 0;
		this.usedProjectilePositions = 0;
		this.spawnedEnemyTypes.Clear();
		this.tempEnemyAmount += this.hideousMasses;
		this.hideousMasses = 0;
		if (this.currentWave > 11)
		{
			int k = this.currentWave;
			int num3 = 0;
			while (k >= 10)
			{
				k -= 10;
				num3++;
			}
			if (this.tempEnemyAmount > 0)
			{
				num3 -= this.tempEnemyAmount;
			}
			if (this.uncommonAntiBuffer < 1f && num3 > 0)
			{
				int num4 = Random.Range(0, this.currentWave / 10 + 1);
				if (this.uncommonAntiBuffer <= -0.5f && num4 < 1)
				{
					num4 = 1;
				}
				if (num4 > 0 && this.meleePositions.Count > 0)
				{
					int l = Random.Range(0, this.prefabs.uncommonEnemies.Length);
					int num5 = Random.Range(0, this.prefabs.uncommonEnemies.Length);
					int num6 = 0;
					while (l >= 0)
					{
						if (this.currentWave < this.prefabs.uncommonEnemies[l].spawnWave)
						{
							l--;
						}
						else
						{
							IL_0230:
							while (num5 >= 0 && (this.currentWave < this.prefabs.uncommonEnemies[num5].spawnWave || num5 == l))
							{
								if (num5 == 0)
								{
									num6 = -1;
									break;
								}
								num5--;
							}
							if (l < 0)
							{
								goto IL_0361;
							}
							if (this.currentWave > 16)
							{
								if (this.currentWave < 25)
								{
									num4++;
								}
								else if (num6 != -1)
								{
									num6 = num4;
								}
							}
							bool flag = false;
							bool flag2 = this.SpawnUncommons(l, num4);
							if (num6 > 0)
							{
								flag = this.SpawnUncommons(num5, num6);
							}
							if (flag2 || flag)
							{
								if (this.uncommonAntiBuffer < 0f)
								{
									this.uncommonAntiBuffer = 0f;
								}
								if (flag2)
								{
									this.uncommonAntiBuffer += ((this.prefabs.uncommonEnemies[l].enemyType == EnemyType.Stalker || this.prefabs.uncommonEnemies[l].enemyType == EnemyType.Idol) ? 1f : 0.5f);
								}
								if (flag)
								{
									this.uncommonAntiBuffer += ((this.prefabs.uncommonEnemies[num5].enemyType == EnemyType.Stalker || this.prefabs.uncommonEnemies[num5].enemyType == EnemyType.Idol) ? 1f : 0.5f);
								}
								num3 -= ((flag2 && flag) ? 2 : 1);
								goto IL_0361;
							}
							goto IL_0361;
						}
					}
					goto IL_0230;
				}
			}
			else
			{
				this.uncommonAntiBuffer -= 1f;
			}
			IL_0361:
			if (this.currentWave > 15)
			{
				bool flag3 = false;
				if (this.specialAntiBuffer <= 0 && num3 > 0)
				{
					int num7 = Random.Range(0, num3 + 1);
					if (this.specialAntiBuffer <= -2 && num7 < 1)
					{
						num7 = 1;
					}
					if (num7 > 0 && this.meleePositions.Count > 0)
					{
						for (int m = 0; m < num7; m++)
						{
							int num8 = Random.Range(0, this.prefabs.specialEnemies.Length);
							int num9 = this.GetIndexOfEnemyType(this.prefabs.specialEnemies[num8].enemyType);
							float num10 = 0f;
							while (num8 >= 0 && this.usedMeleePositions < this.meleePositions.Count - 1)
							{
								if (this.currentWave >= this.prefabs.specialEnemies[num8].spawnWave && (float)this.points >= (float)this.prefabs.specialEnemies[num8].spawnCost + num10)
								{
									bool flag4 = this.SpawnRadiant(this.prefabs.specialEnemies[num8], num9);
									this.SpawnOnGrid(this.prefabs.specialEnemies[num8].prefab, this.meleePositions[this.usedMeleePositions], false, true, CyberPooledType.None, flag4);
									this.points -= Mathf.RoundToInt((float)(this.prefabs.specialEnemies[num8].spawnCost * (flag4 ? 3 : 1)) + num10);
									num10 += (float)(this.prefabs.specialEnemies[num8].costIncreasePerSpawn * (flag4 ? 3 : 1));
									this.spawnedEnemyTypes[num9].amount++;
									this.usedMeleePositions++;
									this.tempEnemyAmount++;
									if (this.specialAntiBuffer < 0)
									{
										this.specialAntiBuffer = 0;
									}
									this.specialAntiBuffer++;
									flag3 = true;
									break;
								}
								num8--;
								if (num8 >= 0)
								{
									num9 = this.GetIndexOfEnemyType(this.prefabs.specialEnemies[num8].enemyType);
								}
							}
						}
					}
				}
				if (!flag3)
				{
					this.specialAntiBuffer--;
				}
			}
		}
		this.GetNextEnemy();
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x000312DC File Offset: 0x0002F4DC
	private int CapUncommonsAmount(int target, int amount)
	{
		EnemyType enemyType = this.prefabs.uncommonEnemies[target].enemyType;
		if (enemyType <= EnemyType.Turret)
		{
			if (enemyType != EnemyType.Virtue)
			{
				if (enemyType != EnemyType.Stalker)
				{
					if (enemyType == EnemyType.Turret)
					{
						if (amount > this.currentWave / 5 || amount > 6)
						{
							return Mathf.Min(this.currentWave / 5, 6);
						}
					}
				}
				else if (amount > this.currentWave / 8 || amount > 3)
				{
					return Mathf.Min(this.currentWave / 8, 3);
				}
			}
			else if (amount > this.currentWave / 5 || amount > 8)
			{
				return Mathf.Min(this.currentWave / 5, 8);
			}
		}
		else if (enemyType != EnemyType.Idol)
		{
			if (enemyType != EnemyType.Gutterman)
			{
				if (enemyType == EnemyType.Guttertank)
				{
					if (amount > this.currentWave / 20 || amount > 3)
					{
						return Mathf.Min(this.currentWave / 20, 3);
					}
				}
			}
			else if (amount > this.currentWave / 15 || amount > 5)
			{
				return Mathf.Min(this.currentWave / 15, 5);
			}
		}
		else if (amount > this.currentWave / 8 || amount > 5)
		{
			return Mathf.Min(this.currentWave / 8, 5);
		}
		return amount;
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x000313F0 File Offset: 0x0002F5F0
	private int GetIndexOfEnemyType(EnemyType target)
	{
		if (this.spawnedEnemyTypes.Count > 0)
		{
			for (int i = 0; i < this.spawnedEnemyTypes.Count; i++)
			{
				if (this.spawnedEnemyTypes[i].type == target)
				{
					return i;
				}
			}
		}
		this.spawnedEnemyTypes.Add(new EnemyTypeTracker(target));
		return this.spawnedEnemyTypes.Count - 1;
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x00031458 File Offset: 0x0002F658
	private bool SpawnRadiant(EndlessEnemy target, int indexOf)
	{
		float num = (float)(target.spawnWave * 2 + 25);
		float num2 = (float)target.spawnCost;
		if (target.spawnCost < 10)
		{
			num2 += 1f;
		}
		if (target.spawnCost > 10)
		{
			num2 = num2 / 2f + 5f;
		}
		return (float)this.currentWave >= num + (float)this.spawnedEnemyTypes[indexOf].amount * num2;
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x000314C8 File Offset: 0x0002F6C8
	private bool SpawnUncommons(int target, int amount)
	{
		amount = this.CapUncommonsAmount(target, amount);
		bool flag = false;
		for (int i = 0; i < amount; i++)
		{
			EndlessEnemy endlessEnemy = this.prefabs.uncommonEnemies[target];
			bool flag2 = endlessEnemy.enemyType != EnemyType.Stalker && endlessEnemy.enemyType != EnemyType.Guttertank && Random.Range(0f, 1f) > 0.5f;
			if (flag2 && this.usedProjectilePositions >= this.projectilePositions.Count - 1)
			{
				flag2 = false;
			}
			if (this.usedMeleePositions >= this.meleePositions.Count - 1)
			{
				break;
			}
			int indexOfEnemyType = this.GetIndexOfEnemyType(endlessEnemy.enemyType);
			int num = endlessEnemy.costIncreasePerSpawn * this.spawnedEnemyTypes[indexOfEnemyType].amount;
			int spawnCost = endlessEnemy.spawnCost;
			if (this.currentWave < endlessEnemy.spawnWave || this.points / 2 < endlessEnemy.spawnCost + num)
			{
				break;
			}
			bool flag3 = this.SpawnRadiant(endlessEnemy, indexOfEnemyType);
			this.SpawnOnGrid(endlessEnemy.prefab, flag2 ? this.projectilePositions[this.usedProjectilePositions] : this.meleePositions[this.usedMeleePositions], false, true, CyberPooledType.None, flag3);
			this.points -= endlessEnemy.spawnCost * (flag3 ? 3 : 1) + num;
			this.spawnedEnemyTypes[indexOfEnemyType].amount++;
			if (flag2)
			{
				this.usedProjectilePositions++;
			}
			else
			{
				this.usedMeleePositions++;
			}
			this.tempEnemyAmount++;
			flag = true;
			if (flag3)
			{
				amount -= 2;
			}
		}
		return flag;
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x00031670 File Offset: 0x0002F870
	private void GetNextEnemy()
	{
		if (!base.gameObject.scene.isLoaded)
		{
			return;
		}
		if ((this.points > 0 && this.usedMeleePositions < this.meleePositions.Count) || (this.points > 1 && this.usedProjectilePositions < this.projectilePositions.Count))
		{
			if ((Random.Range(0f, 1f) < 0.5f || this.usedProjectilePositions >= this.projectilePositions.Count) && this.usedMeleePositions < this.meleePositions.Count)
			{
				int num = Random.Range(0, this.prefabs.meleeEnemies.Length);
				bool flag = false;
				for (int i = num; i >= 0; i--)
				{
					EndlessEnemy endlessEnemy = this.prefabs.meleeEnemies[i];
					int indexOfEnemyType = this.GetIndexOfEnemyType(endlessEnemy.enemyType);
					int num2 = endlessEnemy.costIncreasePerSpawn * this.spawnedEnemyTypes[indexOfEnemyType].amount;
					int num3 = endlessEnemy.spawnCost + num2;
					if (((float)this.points >= (float)num3 * 1.5f || (i == 0 && this.points >= num3)) && this.currentWave >= endlessEnemy.spawnWave)
					{
						bool flag2 = this.SpawnRadiant(endlessEnemy, indexOfEnemyType);
						flag = true;
						this.SpawnOnGrid(endlessEnemy.prefab, this.meleePositions[this.usedMeleePositions], false, true, CyberPooledType.None, flag2);
						this.points -= endlessEnemy.spawnCost * (flag2 ? 3 : 1) + num2;
						this.spawnedEnemyTypes[indexOfEnemyType].amount++;
						this.usedMeleePositions++;
						this.tempEnemyAmount++;
						break;
					}
				}
				if (!flag)
				{
					this.usedMeleePositions = this.meleePositions.Count;
				}
			}
			else if (this.usedProjectilePositions < this.projectilePositions.Count)
			{
				int num4 = Random.Range(0, this.prefabs.projectileEnemies.Length);
				bool flag3 = false;
				for (int j = num4; j >= 0; j--)
				{
					EndlessEnemy endlessEnemy2 = this.prefabs.projectileEnemies[j];
					int indexOfEnemyType2 = this.GetIndexOfEnemyType(endlessEnemy2.enemyType);
					int num5 = endlessEnemy2.costIncreasePerSpawn * this.spawnedEnemyTypes[indexOfEnemyType2].amount;
					int num6 = endlessEnemy2.spawnCost + num5;
					if (((float)this.points >= (float)num6 * 1.5f || (j == 0 && this.points >= num6)) && this.currentWave >= endlessEnemy2.spawnWave)
					{
						bool flag4 = this.SpawnRadiant(endlessEnemy2, indexOfEnemyType2);
						flag3 = true;
						this.SpawnOnGrid(endlessEnemy2.prefab, this.projectilePositions[this.usedProjectilePositions], false, true, CyberPooledType.None, flag4);
						this.points -= endlessEnemy2.spawnCost * (flag4 ? 3 : 1) + num5;
						this.spawnedEnemyTypes[indexOfEnemyType2].amount++;
						this.usedProjectilePositions++;
						this.tempEnemyAmount++;
						break;
					}
				}
				if (!flag3)
				{
					this.usedProjectilePositions = this.projectilePositions.Count;
				}
			}
			base.Invoke("GetNextEnemy", 0.1f);
			return;
		}
		this.enemyAmount = this.tempEnemyAmount;
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x000319D4 File Offset: 0x0002FBD4
	private void ShuffleDecks()
	{
		int num = Mathf.FloorToInt((float)(this.CurrentPatternPool.Length / 2));
		for (int i = 0; i < num; i++)
		{
			ArenaPattern arenaPattern = this.CurrentPatternPool[i];
			int num2 = Random.Range(i, num);
			this.CurrentPatternPool[i] = this.CurrentPatternPool[num2];
			this.CurrentPatternPool[num2] = arenaPattern;
		}
		for (int j = num; j < this.CurrentPatternPool.Length; j++)
		{
			ArenaPattern arenaPattern2 = this.CurrentPatternPool[j];
			int num3 = Random.Range(j, this.CurrentPatternPool.Length);
			this.CurrentPatternPool[j] = this.CurrentPatternPool[num3];
			this.CurrentPatternPool[num3] = arenaPattern2;
		}
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x00031A78 File Offset: 0x0002FC78
	private string SplitCamelCase(string str)
	{
		return Regex.Replace(Regex.Replace(str, "(\\P{Ll})(\\P{Ll}\\p{Ll})", "$1 $2"), "(\\p{Ll})(\\P{Ll})", "$1 $2");
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x00031A9C File Offset: 0x0002FC9C
	public void SetGlowColor(bool roundDown = false)
	{
		if (this.currentWave < 10)
		{
			return;
		}
		int num = this.currentWave;
		if (roundDown)
		{
			if (num > 25)
			{
				num = 25;
			}
			else if (num > 20)
			{
				num = 20;
			}
			else if (num > 15)
			{
				num = 15;
			}
			else
			{
				num = 10;
			}
		}
		if (num <= 15)
		{
			if (num == 10)
			{
				this.targetColor = Color.green;
				return;
			}
			if (num != 15)
			{
				return;
			}
			this.targetColor = Color.yellow;
			return;
		}
		else
		{
			if (num == 20)
			{
				this.targetColor = Color.red;
				this.currentGlow = 0.35f;
				Material[] array = this.mats;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetFloat(UKShaderProperties.EmissiveIntensity, this.currentGlow * this.glowMultiplier);
				}
				return;
			}
			if (num != 25)
			{
				return;
			}
			this.currentGlow = 0.5f;
			foreach (Material material in this.mats)
			{
				material.SetFloat(EndlessGrid.PcGamerMode, 1f);
				material.SetFloat(UKShaderProperties.EmissiveIntensity, this.currentGlow * this.glowMultiplier);
			}
			return;
		}
	}

	// Token: 0x0400095F RID: 2399
	public bool customPatternMode;

	// Token: 0x04000960 RID: 2400
	public ArenaPattern[] customPatterns;

	// Token: 0x04000961 RID: 2401
	public const int ArenaSize = 16;

	// Token: 0x04000962 RID: 2402
	[SerializeField]
	private ArenaPattern[] patterns;

	// Token: 0x04000963 RID: 2403
	private int[] usedPatterns;

	// Token: 0x04000964 RID: 2404
	[SerializeField]
	private List<CyberPooledPrefab> jumpPadPool;

	// Token: 0x04000965 RID: 2405
	private int jumpPadSelector;

	// Token: 0x04000966 RID: 2406
	[SerializeField]
	private CyberGrindNavHelper nvmhlpr;

	// Token: 0x04000967 RID: 2407
	[SerializeField]
	private PrefabDatabase prefabs;

	// Token: 0x04000968 RID: 2408
	[SerializeField]
	private GameObject gridCube;

	// Token: 0x04000969 RID: 2409
	[SerializeField]
	private float offset = 5f;

	// Token: 0x0400096A RID: 2410
	[HideInInspector]
	public EndlessCube[][] cubes;

	// Token: 0x0400096B RID: 2411
	private int incompleteBlocks;

	// Token: 0x0400096C RID: 2412
	private ArenaPattern currentPattern;

	// Token: 0x0400096D RID: 2413
	public NavMeshSurface nms;

	// Token: 0x0400096E RID: 2414
	private ActivateNextWave anw;

	// Token: 0x0400096F RID: 2415
	public int enemyAmount = 999;

	// Token: 0x04000970 RID: 2416
	public int tempEnemyAmount;

	// Token: 0x04000971 RID: 2417
	private int points;

	// Token: 0x04000972 RID: 2418
	private int maxPoints = 10;

	// Token: 0x04000973 RID: 2419
	public int currentWave;

	// Token: 0x04000974 RID: 2420
	private int currentPatternNum = -1;

	// Token: 0x04000975 RID: 2421
	private List<Vector2> meleePositions = new List<Vector2>();

	// Token: 0x04000976 RID: 2422
	private int usedMeleePositions;

	// Token: 0x04000977 RID: 2423
	private List<Vector2> projectilePositions = new List<Vector2>();

	// Token: 0x04000978 RID: 2424
	private int usedProjectilePositions;

	// Token: 0x04000979 RID: 2425
	private List<GameObject> spawnedEnemies = new List<GameObject>();

	// Token: 0x0400097A RID: 2426
	private List<GameObject> spawnedPrefabs = new List<GameObject>();

	// Token: 0x0400097B RID: 2427
	private List<EnemyTypeTracker> spawnedEnemyTypes = new List<EnemyTypeTracker>();

	// Token: 0x0400097C RID: 2428
	private int incompletePrefabs;

	// Token: 0x0400097D RID: 2429
	private GoreZone gz;

	// Token: 0x0400097E RID: 2430
	private int specialAntiBuffer;

	// Token: 0x0400097F RID: 2431
	private int massAntiBuffer;

	// Token: 0x04000980 RID: 2432
	private float uncommonAntiBuffer;

	// Token: 0x04000981 RID: 2433
	public Text waveNumberText;

	// Token: 0x04000982 RID: 2434
	public Text enemiesLeftText;

	// Token: 0x04000983 RID: 2435
	public bool crowdReactions;

	// Token: 0x04000984 RID: 2436
	private CrowdReactions crorea;

	// Token: 0x04000985 RID: 2437
	private int hideousMasses;

	// Token: 0x04000986 RID: 2438
	private NewMovement nmov;

	// Token: 0x04000987 RID: 2439
	private WeaponCharges wc;

	// Token: 0x04000988 RID: 2440
	private Material[] mats;

	// Token: 0x04000989 RID: 2441
	private Color targetColor;

	// Token: 0x0400098A RID: 2442
	private bool testMode;

	// Token: 0x0400098B RID: 2443
	private bool lastEnemyMode;

	// Token: 0x0400098C RID: 2444
	public Transform enemyToTrack;

	// Token: 0x0400098D RID: 2445
	private float currentGlow = 0.2f;

	// Token: 0x0400098E RID: 2446
	public float glowMultiplier = 1f;

	// Token: 0x0400098F RID: 2447
	private GameObject combinedGridStaticObject;

	// Token: 0x04000990 RID: 2448
	private MeshRenderer combinedGridStaticMeshRenderer;

	// Token: 0x04000991 RID: 2449
	private MeshFilter combinedGridStaticMeshFilter;

	// Token: 0x04000992 RID: 2450
	private Mesh combinedGridStaticMesh;

	// Token: 0x04000993 RID: 2451
	private static readonly int WorldOffset = Shader.PropertyToID("_WorldOffset");

	// Token: 0x04000994 RID: 2452
	private static readonly int GradientSpeed = Shader.PropertyToID("_GradientSpeed");

	// Token: 0x04000995 RID: 2453
	private static readonly int GradientFalloff = Shader.PropertyToID("_GradientFalloff");

	// Token: 0x04000996 RID: 2454
	private static readonly int GradientScale = Shader.PropertyToID("_GradientScale");

	// Token: 0x04000997 RID: 2455
	private static readonly int PcGamerMode = Shader.PropertyToID("_PCGamerMode");

	// Token: 0x04000998 RID: 2456
	public int startWave;
}
