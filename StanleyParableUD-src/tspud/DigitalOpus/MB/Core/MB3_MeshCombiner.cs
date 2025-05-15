using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200027D RID: 637
	[Serializable]
	public abstract class MB3_MeshCombiner
	{
		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x0001A562 File Offset: 0x00018762
		public static bool EVAL_VERSION
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000F1E RID: 3870 RVA: 0x0004A93E File Offset: 0x00048B3E
		// (set) Token: 0x06000F1F RID: 3871 RVA: 0x0004A946 File Offset: 0x00048B46
		public virtual MB2_LogLevel LOG_LEVEL
		{
			get
			{
				return this._LOG_LEVEL;
			}
			set
			{
				this._LOG_LEVEL = value;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000F20 RID: 3872 RVA: 0x0004A94F File Offset: 0x00048B4F
		// (set) Token: 0x06000F21 RID: 3873 RVA: 0x0004A957 File Offset: 0x00048B57
		public virtual MB2_ValidationLevel validationLevel
		{
			get
			{
				return this._validationLevel;
			}
			set
			{
				this._validationLevel = value;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000F22 RID: 3874 RVA: 0x0004A960 File Offset: 0x00048B60
		// (set) Token: 0x06000F23 RID: 3875 RVA: 0x0004A968 File Offset: 0x00048B68
		public string name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x0004A971 File Offset: 0x00048B71
		// (set) Token: 0x06000F25 RID: 3877 RVA: 0x0004A979 File Offset: 0x00048B79
		public virtual MB2_TextureBakeResults textureBakeResults
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

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000F26 RID: 3878 RVA: 0x0004A982 File Offset: 0x00048B82
		// (set) Token: 0x06000F27 RID: 3879 RVA: 0x0004A98A File Offset: 0x00048B8A
		public virtual GameObject resultSceneObject
		{
			get
			{
				return this._resultSceneObject;
			}
			set
			{
				this._resultSceneObject = value;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000F28 RID: 3880 RVA: 0x0004A993 File Offset: 0x00048B93
		// (set) Token: 0x06000F29 RID: 3881 RVA: 0x0004A99B File Offset: 0x00048B9B
		public virtual Renderer targetRenderer
		{
			get
			{
				return this._targetRenderer;
			}
			set
			{
				if (this._targetRenderer != null && this._targetRenderer != value)
				{
					Debug.LogWarning("Previous targetRenderer was not null. Combined mesh may be being used by more than one Renderer");
				}
				this._targetRenderer = value;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000F2A RID: 3882 RVA: 0x0004A9CA File Offset: 0x00048BCA
		// (set) Token: 0x06000F2B RID: 3883 RVA: 0x0004A9D2 File Offset: 0x00048BD2
		public virtual MB_RenderType renderType
		{
			get
			{
				return this._renderType;
			}
			set
			{
				this._renderType = value;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000F2C RID: 3884 RVA: 0x0004A9DB File Offset: 0x00048BDB
		// (set) Token: 0x06000F2D RID: 3885 RVA: 0x0004A9E3 File Offset: 0x00048BE3
		public virtual MB2_OutputOptions outputOption
		{
			get
			{
				return this._outputOption;
			}
			set
			{
				this._outputOption = value;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000F2E RID: 3886 RVA: 0x0004A9EC File Offset: 0x00048BEC
		// (set) Token: 0x06000F2F RID: 3887 RVA: 0x0004A9F4 File Offset: 0x00048BF4
		public virtual MB2_LightmapOptions lightmapOption
		{
			get
			{
				return this._lightmapOption;
			}
			set
			{
				this._lightmapOption = value;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000F30 RID: 3888 RVA: 0x0004A9FD File Offset: 0x00048BFD
		// (set) Token: 0x06000F31 RID: 3889 RVA: 0x0004AA05 File Offset: 0x00048C05
		public virtual bool doNorm
		{
			get
			{
				return this._doNorm;
			}
			set
			{
				this._doNorm = value;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000F32 RID: 3890 RVA: 0x0004AA0E File Offset: 0x00048C0E
		// (set) Token: 0x06000F33 RID: 3891 RVA: 0x0004AA16 File Offset: 0x00048C16
		public virtual bool doTan
		{
			get
			{
				return this._doTan;
			}
			set
			{
				this._doTan = value;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000F34 RID: 3892 RVA: 0x0004AA1F File Offset: 0x00048C1F
		// (set) Token: 0x06000F35 RID: 3893 RVA: 0x0004AA27 File Offset: 0x00048C27
		public virtual bool doCol
		{
			get
			{
				return this._doCol;
			}
			set
			{
				this._doCol = value;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000F36 RID: 3894 RVA: 0x0004AA30 File Offset: 0x00048C30
		// (set) Token: 0x06000F37 RID: 3895 RVA: 0x0004AA38 File Offset: 0x00048C38
		public virtual bool doUV
		{
			get
			{
				return this._doUV;
			}
			set
			{
				this._doUV = value;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000F38 RID: 3896 RVA: 0x0001A562 File Offset: 0x00018762
		// (set) Token: 0x06000F39 RID: 3897 RVA: 0x00005444 File Offset: 0x00003644
		public virtual bool doUV1
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x0004AA41 File Offset: 0x00048C41
		public virtual bool doUV2()
		{
			return this._lightmapOption == MB2_LightmapOptions.copy_UV2_unchanged || this._lightmapOption == MB2_LightmapOptions.preserve_current_lightmapping || this._lightmapOption == MB2_LightmapOptions.copy_UV2_unchanged_to_separate_rects;
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000F3B RID: 3899 RVA: 0x0004AA5F File Offset: 0x00048C5F
		// (set) Token: 0x06000F3C RID: 3900 RVA: 0x0004AA67 File Offset: 0x00048C67
		public virtual bool doUV3
		{
			get
			{
				return this._doUV3;
			}
			set
			{
				this._doUV3 = value;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000F3D RID: 3901 RVA: 0x0004AA70 File Offset: 0x00048C70
		// (set) Token: 0x06000F3E RID: 3902 RVA: 0x0004AA78 File Offset: 0x00048C78
		public virtual bool doUV4
		{
			get
			{
				return this._doUV4;
			}
			set
			{
				this._doUV4 = value;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000F3F RID: 3903 RVA: 0x0004AA81 File Offset: 0x00048C81
		// (set) Token: 0x06000F40 RID: 3904 RVA: 0x0004AA89 File Offset: 0x00048C89
		public virtual bool doBlendShapes
		{
			get
			{
				return this._doBlendShapes;
			}
			set
			{
				this._doBlendShapes = value;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000F41 RID: 3905 RVA: 0x0004AA92 File Offset: 0x00048C92
		// (set) Token: 0x06000F42 RID: 3906 RVA: 0x0004AA9A File Offset: 0x00048C9A
		public virtual bool recenterVertsToBoundsCenter
		{
			get
			{
				return this._recenterVertsToBoundsCenter;
			}
			set
			{
				this._recenterVertsToBoundsCenter = value;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000F43 RID: 3907 RVA: 0x0004AAA3 File Offset: 0x00048CA3
		// (set) Token: 0x06000F44 RID: 3908 RVA: 0x0004AAAB File Offset: 0x00048CAB
		public bool optimizeAfterBake
		{
			get
			{
				return this._optimizeAfterBake;
			}
			set
			{
				this._optimizeAfterBake = value;
			}
		}

		// Token: 0x06000F45 RID: 3909
		public abstract int GetLightmapIndex();

		// Token: 0x06000F46 RID: 3910
		public abstract void ClearBuffers();

		// Token: 0x06000F47 RID: 3911
		public abstract void ClearMesh();

		// Token: 0x06000F48 RID: 3912
		public abstract void DestroyMesh();

		// Token: 0x06000F49 RID: 3913
		public abstract void DestroyMeshEditor(MB2_EditorMethodsInterface editorMethods);

		// Token: 0x06000F4A RID: 3914
		public abstract List<GameObject> GetObjectsInCombined();

		// Token: 0x06000F4B RID: 3915
		public abstract int GetNumObjectsInCombined();

		// Token: 0x06000F4C RID: 3916
		public abstract int GetNumVerticesFor(GameObject go);

		// Token: 0x06000F4D RID: 3917
		public abstract int GetNumVerticesFor(int instanceID);

		// Token: 0x06000F4E RID: 3918
		public abstract Dictionary<MB3_MeshCombiner.MBBlendShapeKey, MB3_MeshCombiner.MBBlendShapeValue> BuildSourceBlendShapeToCombinedIndexMap();

		// Token: 0x06000F4F RID: 3919 RVA: 0x0004AAB4 File Offset: 0x00048CB4
		public virtual void Apply()
		{
			this.Apply(null);
		}

		// Token: 0x06000F50 RID: 3920
		public abstract void Apply(MB3_MeshCombiner.GenerateUV2Delegate uv2GenerationMethod);

		// Token: 0x06000F51 RID: 3921
		public abstract void Apply(bool triangles, bool vertices, bool normals, bool tangents, bool uvs, bool uv2, bool uv3, bool uv4, bool colors, bool bones = false, bool blendShapeFlag = false, MB3_MeshCombiner.GenerateUV2Delegate uv2GenerationMethod = null);

		// Token: 0x06000F52 RID: 3922
		public abstract void UpdateGameObjects(GameObject[] gos, bool recalcBounds = true, bool updateVertices = true, bool updateNormals = true, bool updateTangents = true, bool updateUV = false, bool updateUV2 = false, bool updateUV3 = false, bool updateUV4 = false, bool updateColors = false, bool updateSkinningInfo = false);

		// Token: 0x06000F53 RID: 3923
		public abstract bool AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource = true);

		// Token: 0x06000F54 RID: 3924
		public abstract bool AddDeleteGameObjectsByID(GameObject[] gos, int[] deleteGOinstanceIDs, bool disableRendererInSource);

		// Token: 0x06000F55 RID: 3925
		public abstract bool CombinedMeshContains(GameObject go);

		// Token: 0x06000F56 RID: 3926
		public abstract void UpdateSkinnedMeshApproximateBounds();

		// Token: 0x06000F57 RID: 3927
		public abstract void UpdateSkinnedMeshApproximateBoundsFromBones();

		// Token: 0x06000F58 RID: 3928
		public abstract void CheckIntegrity();

		// Token: 0x06000F59 RID: 3929
		public abstract void UpdateSkinnedMeshApproximateBoundsFromBounds();

		// Token: 0x06000F5A RID: 3930 RVA: 0x0004AAC0 File Offset: 0x00048CC0
		public static void UpdateSkinnedMeshApproximateBoundsFromBonesStatic(Transform[] bs, SkinnedMeshRenderer smr)
		{
			Vector3 position = bs[0].position;
			Vector3 position2 = bs[0].position;
			for (int i = 1; i < bs.Length; i++)
			{
				Vector3 position3 = bs[i].position;
				if (position3.x < position2.x)
				{
					position2.x = position3.x;
				}
				if (position3.y < position2.y)
				{
					position2.y = position3.y;
				}
				if (position3.z < position2.z)
				{
					position2.z = position3.z;
				}
				if (position3.x > position.x)
				{
					position.x = position3.x;
				}
				if (position3.y > position.y)
				{
					position.y = position3.y;
				}
				if (position3.z > position.z)
				{
					position.z = position3.z;
				}
			}
			Vector3 vector = (position + position2) / 2f;
			Vector3 vector2 = position - position2;
			Matrix4x4 worldToLocalMatrix = smr.worldToLocalMatrix;
			Bounds bounds = new Bounds(worldToLocalMatrix * vector, worldToLocalMatrix * vector2);
			smr.localBounds = bounds;
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x0004AC08 File Offset: 0x00048E08
		public static void UpdateSkinnedMeshApproximateBoundsFromBoundsStatic(List<GameObject> objectsInCombined, SkinnedMeshRenderer smr)
		{
			Bounds bounds = default(Bounds);
			Bounds bounds2 = default(Bounds);
			if (MB_Utility.GetBounds(objectsInCombined[0], out bounds))
			{
				bounds2 = bounds;
				for (int i = 1; i < objectsInCombined.Count; i++)
				{
					if (!MB_Utility.GetBounds(objectsInCombined[i], out bounds))
					{
						Debug.LogError("Could not get bounds. Not updating skinned mesh bounds");
						return;
					}
					bounds2.Encapsulate(bounds);
				}
				smr.localBounds = bounds2;
				return;
			}
			Debug.LogError("Could not get bounds. Not updating skinned mesh bounds");
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x0004AC81 File Offset: 0x00048E81
		protected virtual bool _CreateTemporaryTextrueBakeResult(GameObject[] gos, List<Material> matsOnTargetRenderer)
		{
			if (this.GetNumObjectsInCombined() > 0)
			{
				Debug.LogError("Can't add objects if there are already objects in combined mesh when 'Texture Bake Result' is not set. Perhaps enable 'Clear Buffers After Bake'");
				return false;
			}
			this._usingTemporaryTextureBakeResult = true;
			this._textureBakeResults = MB2_TextureBakeResults.CreateForMaterialsOnRenderer(gos, matsOnTargetRenderer);
			return true;
		}

		// Token: 0x06000F5D RID: 3933
		public abstract List<Material> GetMaterialsOnTargetRenderer();

		// Token: 0x04000D79 RID: 3449
		[SerializeField]
		protected MB2_LogLevel _LOG_LEVEL = MB2_LogLevel.info;

		// Token: 0x04000D7A RID: 3450
		[SerializeField]
		protected MB2_ValidationLevel _validationLevel = MB2_ValidationLevel.robust;

		// Token: 0x04000D7B RID: 3451
		[SerializeField]
		protected string _name;

		// Token: 0x04000D7C RID: 3452
		[SerializeField]
		protected MB2_TextureBakeResults _textureBakeResults;

		// Token: 0x04000D7D RID: 3453
		[SerializeField]
		protected GameObject _resultSceneObject;

		// Token: 0x04000D7E RID: 3454
		[SerializeField]
		protected Renderer _targetRenderer;

		// Token: 0x04000D7F RID: 3455
		[SerializeField]
		protected MB_RenderType _renderType;

		// Token: 0x04000D80 RID: 3456
		[SerializeField]
		protected MB2_OutputOptions _outputOption;

		// Token: 0x04000D81 RID: 3457
		[SerializeField]
		protected MB2_LightmapOptions _lightmapOption = MB2_LightmapOptions.ignore_UV2;

		// Token: 0x04000D82 RID: 3458
		[SerializeField]
		protected bool _doNorm = true;

		// Token: 0x04000D83 RID: 3459
		[SerializeField]
		protected bool _doTan = true;

		// Token: 0x04000D84 RID: 3460
		[SerializeField]
		protected bool _doCol;

		// Token: 0x04000D85 RID: 3461
		[SerializeField]
		protected bool _doUV = true;

		// Token: 0x04000D86 RID: 3462
		[SerializeField]
		protected bool _doUV3;

		// Token: 0x04000D87 RID: 3463
		[SerializeField]
		protected bool _doUV4;

		// Token: 0x04000D88 RID: 3464
		[SerializeField]
		protected bool _doBlendShapes;

		// Token: 0x04000D89 RID: 3465
		[SerializeField]
		protected bool _recenterVertsToBoundsCenter;

		// Token: 0x04000D8A RID: 3466
		[SerializeField]
		public bool _optimizeAfterBake = true;

		// Token: 0x04000D8B RID: 3467
		[SerializeField]
		public float uv2UnwrappingParamsHardAngle = 60f;

		// Token: 0x04000D8C RID: 3468
		[SerializeField]
		public float uv2UnwrappingParamsPackMargin = 0.005f;

		// Token: 0x04000D8D RID: 3469
		protected bool _usingTemporaryTextureBakeResult;

		// Token: 0x0200045D RID: 1117
		// (Invoke) Token: 0x06001900 RID: 6400
		public delegate void GenerateUV2Delegate(Mesh m, float hardAngle, float packMargin);

		// Token: 0x0200045E RID: 1118
		public class MBBlendShapeKey
		{
			// Token: 0x06001903 RID: 6403 RVA: 0x0007CFA9 File Offset: 0x0007B1A9
			public MBBlendShapeKey(int srcSkinnedMeshRenderGameObjectID, int blendShapeIndexInSource)
			{
				this.gameObjecID = srcSkinnedMeshRenderGameObjectID;
				this.blendShapeIndexInSrc = blendShapeIndexInSource;
			}

			// Token: 0x06001904 RID: 6404 RVA: 0x0007CFC0 File Offset: 0x0007B1C0
			public override bool Equals(object obj)
			{
				if (!(obj is MB3_MeshCombiner.MBBlendShapeKey) || obj == null)
				{
					return false;
				}
				MB3_MeshCombiner.MBBlendShapeKey mbblendShapeKey = (MB3_MeshCombiner.MBBlendShapeKey)obj;
				return this.gameObjecID == mbblendShapeKey.gameObjecID && this.blendShapeIndexInSrc == mbblendShapeKey.blendShapeIndexInSrc;
			}

			// Token: 0x06001905 RID: 6405 RVA: 0x0007CFFF File Offset: 0x0007B1FF
			public override int GetHashCode()
			{
				return (23 * 31 + this.gameObjecID) * 31 + this.blendShapeIndexInSrc;
			}

			// Token: 0x04001636 RID: 5686
			public int gameObjecID;

			// Token: 0x04001637 RID: 5687
			public int blendShapeIndexInSrc;
		}

		// Token: 0x0200045F RID: 1119
		public class MBBlendShapeValue
		{
			// Token: 0x04001638 RID: 5688
			public GameObject combinedMeshGameObject;

			// Token: 0x04001639 RID: 5689
			public int blendShapeIndex;
		}
	}
}
