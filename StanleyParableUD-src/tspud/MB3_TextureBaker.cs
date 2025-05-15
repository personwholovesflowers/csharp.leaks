using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DigitalOpus.MB.Core;
using UnityEngine;

// Token: 0x0200006E RID: 110
public class MB3_TextureBaker : MB3_MeshBakerRoot
{
	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000296 RID: 662 RVA: 0x0001149C File Offset: 0x0000F69C
	// (set) Token: 0x06000297 RID: 663 RVA: 0x000114A4 File Offset: 0x0000F6A4
	public override MB2_TextureBakeResults textureBakeResults
	{
		get
		{
			return this._textureBakeResults;
		}
		set
		{
			this._textureBakeResults = value;
		}
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000298 RID: 664 RVA: 0x000114AD File Offset: 0x0000F6AD
	// (set) Token: 0x06000299 RID: 665 RVA: 0x000114B5 File Offset: 0x0000F6B5
	public virtual int atlasPadding
	{
		get
		{
			return this._atlasPadding;
		}
		set
		{
			this._atlasPadding = value;
		}
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x0600029A RID: 666 RVA: 0x000114BE File Offset: 0x0000F6BE
	// (set) Token: 0x0600029B RID: 667 RVA: 0x000114C6 File Offset: 0x0000F6C6
	public virtual int maxAtlasSize
	{
		get
		{
			return this._maxAtlasSize;
		}
		set
		{
			this._maxAtlasSize = value;
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x0600029C RID: 668 RVA: 0x000114CF File Offset: 0x0000F6CF
	// (set) Token: 0x0600029D RID: 669 RVA: 0x000114D7 File Offset: 0x0000F6D7
	public virtual bool resizePowerOfTwoTextures
	{
		get
		{
			return this._resizePowerOfTwoTextures;
		}
		set
		{
			this._resizePowerOfTwoTextures = value;
		}
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x0600029E RID: 670 RVA: 0x000114E0 File Offset: 0x0000F6E0
	// (set) Token: 0x0600029F RID: 671 RVA: 0x000114E8 File Offset: 0x0000F6E8
	public virtual bool fixOutOfBoundsUVs
	{
		get
		{
			return this._fixOutOfBoundsUVs;
		}
		set
		{
			this._fixOutOfBoundsUVs = value;
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x060002A0 RID: 672 RVA: 0x000114F1 File Offset: 0x0000F6F1
	// (set) Token: 0x060002A1 RID: 673 RVA: 0x000114F9 File Offset: 0x0000F6F9
	public virtual int maxTilingBakeSize
	{
		get
		{
			return this._maxTilingBakeSize;
		}
		set
		{
			this._maxTilingBakeSize = value;
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x060002A2 RID: 674 RVA: 0x00011502 File Offset: 0x0000F702
	// (set) Token: 0x060002A3 RID: 675 RVA: 0x0001150A File Offset: 0x0000F70A
	public virtual MB2_PackingAlgorithmEnum packingAlgorithm
	{
		get
		{
			return this._packingAlgorithm;
		}
		set
		{
			this._packingAlgorithm = value;
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x060002A4 RID: 676 RVA: 0x00011513 File Offset: 0x0000F713
	// (set) Token: 0x060002A5 RID: 677 RVA: 0x0001151B File Offset: 0x0000F71B
	public bool meshBakerTexturePackerForcePowerOfTwo
	{
		get
		{
			return this._meshBakerTexturePackerForcePowerOfTwo;
		}
		set
		{
			this._meshBakerTexturePackerForcePowerOfTwo = value;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x060002A6 RID: 678 RVA: 0x00011524 File Offset: 0x0000F724
	// (set) Token: 0x060002A7 RID: 679 RVA: 0x0001152C File Offset: 0x0000F72C
	public virtual List<ShaderTextureProperty> customShaderProperties
	{
		get
		{
			return this._customShaderProperties;
		}
		set
		{
			this._customShaderProperties = value;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x060002A8 RID: 680 RVA: 0x00011535 File Offset: 0x0000F735
	// (set) Token: 0x060002A9 RID: 681 RVA: 0x0001153D File Offset: 0x0000F73D
	public virtual List<string> customShaderPropNames
	{
		get
		{
			return this._customShaderPropNames_Depricated;
		}
		set
		{
			this._customShaderPropNames_Depricated = value;
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x060002AA RID: 682 RVA: 0x00011546 File Offset: 0x0000F746
	// (set) Token: 0x060002AB RID: 683 RVA: 0x0001154E File Offset: 0x0000F74E
	public virtual bool doMultiMaterial
	{
		get
		{
			return this._doMultiMaterial;
		}
		set
		{
			this._doMultiMaterial = value;
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x060002AC RID: 684 RVA: 0x00011557 File Offset: 0x0000F757
	// (set) Token: 0x060002AD RID: 685 RVA: 0x0001155F File Offset: 0x0000F75F
	public virtual bool doMultiMaterialSplitAtlasesIfTooBig
	{
		get
		{
			return this._doMultiMaterialSplitAtlasesIfTooBig;
		}
		set
		{
			this._doMultiMaterialSplitAtlasesIfTooBig = value;
		}
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x060002AE RID: 686 RVA: 0x00011568 File Offset: 0x0000F768
	// (set) Token: 0x060002AF RID: 687 RVA: 0x00011570 File Offset: 0x0000F770
	public virtual bool doMultiMaterialSplitAtlasesIfOBUVs
	{
		get
		{
			return this._doMultiMaterialSplitAtlasesIfOBUVs;
		}
		set
		{
			this._doMultiMaterialSplitAtlasesIfOBUVs = value;
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x060002B0 RID: 688 RVA: 0x00011579 File Offset: 0x0000F779
	// (set) Token: 0x060002B1 RID: 689 RVA: 0x00011581 File Offset: 0x0000F781
	public virtual Material resultMaterial
	{
		get
		{
			return this._resultMaterial;
		}
		set
		{
			this._resultMaterial = value;
		}
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x060002B2 RID: 690 RVA: 0x0001158A File Offset: 0x0000F78A
	// (set) Token: 0x060002B3 RID: 691 RVA: 0x00011592 File Offset: 0x0000F792
	public bool considerNonTextureProperties
	{
		get
		{
			return this._considerNonTextureProperties;
		}
		set
		{
			this._considerNonTextureProperties = value;
		}
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x060002B4 RID: 692 RVA: 0x0001159B File Offset: 0x0000F79B
	// (set) Token: 0x060002B5 RID: 693 RVA: 0x000115A3 File Offset: 0x0000F7A3
	public bool doSuggestTreatment
	{
		get
		{
			return this._doSuggestTreatment;
		}
		set
		{
			this._doSuggestTreatment = value;
		}
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x000115AC File Offset: 0x0000F7AC
	public override List<GameObject> GetObjectsToCombine()
	{
		if (this.objsToMesh == null)
		{
			this.objsToMesh = new List<GameObject>();
		}
		return this.objsToMesh;
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x000115C7 File Offset: 0x0000F7C7
	public MB_AtlasesAndRects[] CreateAtlases()
	{
		return this.CreateAtlases(null, false, null);
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x000115D2 File Offset: 0x0000F7D2
	public IEnumerator CreateAtlasesCoroutine(ProgressUpdateDelegate progressInfo, MB3_TextureBaker.CreateAtlasesCoroutineResult coroutineResult, bool saveAtlasesAsAssets = false, MB2_EditorMethodsInterface editorMethods = null, float maxTimePerFrame = 0.01f)
	{
		MBVersionConcrete mbversionConcrete = new MBVersionConcrete();
		if (!MB3_TextureCombiner._RunCorutineWithoutPauseIsRunning && (mbversionConcrete.GetMajorVersion() < 5 || (mbversionConcrete.GetMajorVersion() == 5 && mbversionConcrete.GetMinorVersion() < 3)))
		{
			Debug.LogError("Running the texture combiner as a coroutine only works in Unity 5.3 and higher");
			coroutineResult.success = false;
			yield break;
		}
		this.OnCombinedTexturesCoroutineAtlasesAndRects = null;
		if (maxTimePerFrame <= 0f)
		{
			Debug.LogError("maxTimePerFrame must be a value greater than zero");
			coroutineResult.isFinished = true;
			yield break;
		}
		MB2_ValidationLevel mb2_ValidationLevel = (Application.isPlaying ? MB2_ValidationLevel.quick : MB2_ValidationLevel.robust);
		if (!MB3_MeshBakerRoot.DoCombinedValidate(this, MB_ObjsToCombineTypes.dontCare, null, mb2_ValidationLevel))
		{
			coroutineResult.isFinished = true;
			yield break;
		}
		if (this._doMultiMaterial && !this._ValidateResultMaterials())
		{
			coroutineResult.isFinished = true;
			yield break;
		}
		if (!this._doMultiMaterial)
		{
			if (this._resultMaterial == null)
			{
				Debug.LogError("Combined Material is null please create and assign a result material.");
				coroutineResult.isFinished = true;
				yield break;
			}
			Shader shader = this._resultMaterial.shader;
			for (int j = 0; j < this.objsToMesh.Count; j++)
			{
				foreach (Material material in MB_Utility.GetGOMaterials(this.objsToMesh[j]))
				{
					if (material != null && material.shader != shader)
					{
						Debug.LogWarning(string.Concat(new object[]
						{
							"Game object ",
							this.objsToMesh[j],
							" does not use shader ",
							shader,
							" it may not have the required textures. If not small solid color textures will be generated."
						}));
					}
				}
			}
		}
		MB3_TextureCombiner combiner = this.CreateAndConfigureTextureCombiner();
		combiner.saveAtlasesAsAssets = saveAtlasesAsAssets;
		int num = 1;
		if (this._doMultiMaterial)
		{
			num = this.resultMaterials.Length;
		}
		this.OnCombinedTexturesCoroutineAtlasesAndRects = new MB_AtlasesAndRects[num];
		for (int l = 0; l < this.OnCombinedTexturesCoroutineAtlasesAndRects.Length; l++)
		{
			this.OnCombinedTexturesCoroutineAtlasesAndRects[l] = new MB_AtlasesAndRects();
		}
		int num2;
		for (int i = 0; i < this.OnCombinedTexturesCoroutineAtlasesAndRects.Length; i = num2 + 1)
		{
			List<Material> list = null;
			Material material2;
			if (this._doMultiMaterial)
			{
				list = this.resultMaterials[i].sourceMaterials;
				material2 = this.resultMaterials[i].combinedMaterial;
				combiner.fixOutOfBoundsUVs = this.resultMaterials[i].considerMeshUVs;
			}
			else
			{
				material2 = this._resultMaterial;
			}
			Debug.Log(string.Format("Creating atlases for result material {0} using shader {1}", material2, material2.shader));
			MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult coroutineResult2 = new MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult();
			yield return combiner.CombineTexturesIntoAtlasesCoroutine(progressInfo, this.OnCombinedTexturesCoroutineAtlasesAndRects[i], material2, this.objsToMesh, list, editorMethods, coroutineResult2, maxTimePerFrame, null, false);
			coroutineResult.success = coroutineResult2.success;
			if (!coroutineResult.success)
			{
				coroutineResult.isFinished = true;
				yield break;
			}
			coroutineResult2 = null;
			num2 = i;
		}
		this.unpackMat2RectMap(this.textureBakeResults);
		this.textureBakeResults.doMultiMaterial = this._doMultiMaterial;
		if (this._doMultiMaterial)
		{
			this.textureBakeResults.resultMaterials = this.resultMaterials;
		}
		else
		{
			MB_MultiMaterial[] array = new MB_MultiMaterial[]
			{
				new MB_MultiMaterial()
			};
			array[0].combinedMaterial = this._resultMaterial;
			array[0].considerMeshUVs = this._fixOutOfBoundsUVs;
			array[0].sourceMaterials = new List<Material>();
			array[0].sourceMaterials.AddRange(this.textureBakeResults.materials);
			this.textureBakeResults.resultMaterials = array;
		}
		MB3_MeshBakerCommon[] componentsInChildren = base.GetComponentsInChildren<MB3_MeshBakerCommon>();
		for (int m = 0; m < componentsInChildren.Length; m++)
		{
			componentsInChildren[m].textureBakeResults = this.textureBakeResults;
		}
		if (this.LOG_LEVEL >= MB2_LogLevel.info)
		{
			Debug.Log("Created Atlases");
		}
		coroutineResult.isFinished = true;
		if (coroutineResult.success && this.onBuiltAtlasesSuccess != null)
		{
			this.onBuiltAtlasesSuccess();
		}
		if (!coroutineResult.success && this.onBuiltAtlasesFail != null)
		{
			this.onBuiltAtlasesFail();
		}
		yield break;
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00011608 File Offset: 0x0000F808
	public MB_AtlasesAndRects[] CreateAtlases(ProgressUpdateDelegate progressInfo, bool saveAtlasesAsAssets = false, MB2_EditorMethodsInterface editorMethods = null)
	{
		MB_AtlasesAndRects[] array = null;
		try
		{
			MB3_TextureBaker.CreateAtlasesCoroutineResult createAtlasesCoroutineResult = new MB3_TextureBaker.CreateAtlasesCoroutineResult();
			MB3_TextureCombiner.RunCorutineWithoutPause(this.CreateAtlasesCoroutine(progressInfo, createAtlasesCoroutineResult, saveAtlasesAsAssets, editorMethods, 1000f), 0);
			if (createAtlasesCoroutineResult.success && this.textureBakeResults != null)
			{
				array = this.OnCombinedTexturesCoroutineAtlasesAndRects;
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
		}
		finally
		{
			if (saveAtlasesAsAssets && array != null)
			{
				foreach (MB_AtlasesAndRects mb_AtlasesAndRects in array)
				{
					if (mb_AtlasesAndRects != null && mb_AtlasesAndRects.atlases != null)
					{
						for (int j = 0; j < mb_AtlasesAndRects.atlases.Length; j++)
						{
							if (mb_AtlasesAndRects.atlases[j] != null)
							{
								if (editorMethods != null)
								{
									editorMethods.Destroy(mb_AtlasesAndRects.atlases[j]);
								}
								else
								{
									MB_Utility.Destroy(mb_AtlasesAndRects.atlases[j]);
								}
							}
						}
					}
				}
			}
		}
		return array;
	}

	// Token: 0x060002BA RID: 698 RVA: 0x000116E4 File Offset: 0x0000F8E4
	private void unpackMat2RectMap(MB2_TextureBakeResults tbr)
	{
		List<Material> list = new List<Material>();
		List<MB_MaterialAndUVRect> list2 = new List<MB_MaterialAndUVRect>();
		List<Rect> list3 = new List<Rect>();
		for (int i = 0; i < this.OnCombinedTexturesCoroutineAtlasesAndRects.Length; i++)
		{
			List<MB_MaterialAndUVRect> mat2rect_map = this.OnCombinedTexturesCoroutineAtlasesAndRects[i].mat2rect_map;
			if (mat2rect_map != null)
			{
				for (int j = 0; j < mat2rect_map.Count; j++)
				{
					list2.Add(mat2rect_map[j]);
					list.Add(mat2rect_map[j].material);
					list3.Add(mat2rect_map[j].atlasRect);
				}
			}
		}
		tbr.materials = list.ToArray();
		tbr.materialsAndUVRects = list2.ToArray();
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00011790 File Offset: 0x0000F990
	public MB3_TextureCombiner CreateAndConfigureTextureCombiner()
	{
		return new MB3_TextureCombiner
		{
			LOG_LEVEL = this.LOG_LEVEL,
			atlasPadding = this._atlasPadding,
			maxAtlasSize = this._maxAtlasSize,
			customShaderPropNames = this._customShaderProperties,
			fixOutOfBoundsUVs = this._fixOutOfBoundsUVs,
			maxTilingBakeSize = this._maxTilingBakeSize,
			packingAlgorithm = this._packingAlgorithm,
			meshBakerTexturePackerForcePowerOfTwo = this._meshBakerTexturePackerForcePowerOfTwo,
			resizePowerOfTwoTextures = this._resizePowerOfTwoTextures,
			considerNonTextureProperties = this._considerNonTextureProperties
		};
	}

	// Token: 0x060002BC RID: 700 RVA: 0x0001181C File Offset: 0x0000FA1C
	public static void ConfigureNewMaterialToMatchOld(Material newMat, Material original)
	{
		if (original == null)
		{
			Debug.LogWarning(string.Concat(new object[] { "Original material is null, could not copy properties to ", newMat, ". Setting shader to ", newMat.shader }));
			return;
		}
		newMat.shader = original.shader;
		newMat.CopyPropertiesFromMaterial(original);
		ShaderTextureProperty[] shaderTexPropertyNames = MB3_TextureCombiner.shaderTexPropertyNames;
		for (int i = 0; i < shaderTexPropertyNames.Length; i++)
		{
			Vector2 one = Vector2.one;
			Vector2 zero = Vector2.zero;
			if (newMat.HasProperty(shaderTexPropertyNames[i].name))
			{
				newMat.SetTextureOffset(shaderTexPropertyNames[i].name, zero);
				newMat.SetTextureScale(shaderTexPropertyNames[i].name, one);
			}
		}
	}

	// Token: 0x060002BD RID: 701 RVA: 0x000118C4 File Offset: 0x0000FAC4
	private string PrintSet(HashSet<Material> s)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (Material material in s)
		{
			stringBuilder.Append(material + ",");
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060002BE RID: 702 RVA: 0x0001192C File Offset: 0x0000FB2C
	private bool _ValidateResultMaterials()
	{
		HashSet<Material> hashSet = new HashSet<Material>();
		for (int i = 0; i < this.objsToMesh.Count; i++)
		{
			if (this.objsToMesh[i] != null)
			{
				Material[] gomaterials = MB_Utility.GetGOMaterials(this.objsToMesh[i]);
				for (int j = 0; j < gomaterials.Length; j++)
				{
					if (gomaterials[j] != null)
					{
						hashSet.Add(gomaterials[j]);
					}
				}
			}
		}
		HashSet<Material> hashSet2 = new HashSet<Material>();
		for (int k = 0; k < this.resultMaterials.Length; k++)
		{
			MB_MultiMaterial mb_MultiMaterial = this.resultMaterials[k];
			if (mb_MultiMaterial.combinedMaterial == null)
			{
				Debug.LogError("Combined Material is null please create and assign a result material.");
				return false;
			}
			Shader shader = mb_MultiMaterial.combinedMaterial.shader;
			for (int l = 0; l < mb_MultiMaterial.sourceMaterials.Count; l++)
			{
				if (mb_MultiMaterial.sourceMaterials[l] == null)
				{
					Debug.LogError("There are null entries in the list of Source Materials");
					return false;
				}
				if (shader != mb_MultiMaterial.sourceMaterials[l].shader)
				{
					Debug.LogWarning(string.Concat(new object[]
					{
						"Source material ",
						mb_MultiMaterial.sourceMaterials[l],
						" does not use shader ",
						shader,
						" it may not have the required textures. If not empty textures will be generated."
					}));
				}
				if (hashSet2.Contains(mb_MultiMaterial.sourceMaterials[l]))
				{
					Debug.LogError("A Material " + mb_MultiMaterial.sourceMaterials[l] + " appears more than once in the list of source materials in the source material to combined mapping. Each source material must be unique.");
					return false;
				}
				hashSet2.Add(mb_MultiMaterial.sourceMaterials[l]);
			}
		}
		if (hashSet.IsProperSubsetOf(hashSet2))
		{
			hashSet2.ExceptWith(hashSet);
			Debug.LogWarning("There are materials in the mapping that are not used on your source objects: " + this.PrintSet(hashSet2));
		}
		if (this.resultMaterials != null && this.resultMaterials.Length != 0 && hashSet2.IsProperSubsetOf(hashSet))
		{
			hashSet.ExceptWith(hashSet2);
			Debug.LogError("There are materials on the objects to combine that are not in the mapping: " + this.PrintSet(hashSet));
			return false;
		}
		return true;
	}

	// Token: 0x04000292 RID: 658
	public MB2_LogLevel LOG_LEVEL = MB2_LogLevel.info;

	// Token: 0x04000293 RID: 659
	[SerializeField]
	protected MB2_TextureBakeResults _textureBakeResults;

	// Token: 0x04000294 RID: 660
	[SerializeField]
	protected int _atlasPadding = 1;

	// Token: 0x04000295 RID: 661
	[SerializeField]
	protected int _maxAtlasSize = 4096;

	// Token: 0x04000296 RID: 662
	[SerializeField]
	protected bool _resizePowerOfTwoTextures;

	// Token: 0x04000297 RID: 663
	[SerializeField]
	protected bool _fixOutOfBoundsUVs;

	// Token: 0x04000298 RID: 664
	[SerializeField]
	protected int _maxTilingBakeSize = 1024;

	// Token: 0x04000299 RID: 665
	[SerializeField]
	protected MB2_PackingAlgorithmEnum _packingAlgorithm = MB2_PackingAlgorithmEnum.MeshBakerTexturePacker;

	// Token: 0x0400029A RID: 666
	[SerializeField]
	protected bool _meshBakerTexturePackerForcePowerOfTwo = true;

	// Token: 0x0400029B RID: 667
	[SerializeField]
	protected List<ShaderTextureProperty> _customShaderProperties = new List<ShaderTextureProperty>();

	// Token: 0x0400029C RID: 668
	[SerializeField]
	protected List<string> _customShaderPropNames_Depricated = new List<string>();

	// Token: 0x0400029D RID: 669
	[SerializeField]
	protected bool _doMultiMaterial;

	// Token: 0x0400029E RID: 670
	[SerializeField]
	protected bool _doMultiMaterialSplitAtlasesIfTooBig = true;

	// Token: 0x0400029F RID: 671
	[SerializeField]
	protected bool _doMultiMaterialSplitAtlasesIfOBUVs = true;

	// Token: 0x040002A0 RID: 672
	[SerializeField]
	protected Material _resultMaterial;

	// Token: 0x040002A1 RID: 673
	[SerializeField]
	protected bool _considerNonTextureProperties;

	// Token: 0x040002A2 RID: 674
	[SerializeField]
	protected bool _doSuggestTreatment = true;

	// Token: 0x040002A3 RID: 675
	public MB_MultiMaterial[] resultMaterials = new MB_MultiMaterial[0];

	// Token: 0x040002A4 RID: 676
	public List<GameObject> objsToMesh;

	// Token: 0x040002A5 RID: 677
	public MB3_TextureBaker.OnCombinedTexturesCoroutineSuccess onBuiltAtlasesSuccess;

	// Token: 0x040002A6 RID: 678
	public MB3_TextureBaker.OnCombinedTexturesCoroutineFail onBuiltAtlasesFail;

	// Token: 0x040002A7 RID: 679
	public MB_AtlasesAndRects[] OnCombinedTexturesCoroutineAtlasesAndRects;

	// Token: 0x02000378 RID: 888
	// (Invoke) Token: 0x06001615 RID: 5653
	public delegate void OnCombinedTexturesCoroutineSuccess();

	// Token: 0x02000379 RID: 889
	// (Invoke) Token: 0x06001619 RID: 5657
	public delegate void OnCombinedTexturesCoroutineFail();

	// Token: 0x0200037A RID: 890
	public class CreateAtlasesCoroutineResult
	{
		// Token: 0x04001291 RID: 4753
		public bool success = true;

		// Token: 0x04001292 RID: 4754
		public bool isFinished;
	}
}
