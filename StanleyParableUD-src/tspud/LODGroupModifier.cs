using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C6 RID: 198
[ExecuteInEditMode]
public class LODGroupModifier : MonoBehaviour
{
	// Token: 0x060004A4 RID: 1188 RVA: 0x0001AC58 File Offset: 0x00018E58
	private void Update()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (this.runSetEveryFrame || this.runOneVerify || this.runOneFind || this.runOneCopy)
		{
			this.runSetOnce = true;
		}
		if (!this.runSetOnce)
		{
			return;
		}
		if (this.runOneFind)
		{
			this.foundLODGroups = null;
		}
		this.runSetOnce = false;
		this.runOneVerify = false;
		this.runOneFind = false;
		bool flag = this.runOneCopy;
		this.runOneCopy = false;
		this.defintion.levels.Verify();
		if (this.runOneVerify)
		{
			return;
		}
		if (this.foundLODGroups == null)
		{
			this.foundLODGroups = new List<LODGroup>();
			foreach (LODGroup lodgroup in Object.FindObjectsOfType<LODGroup>())
			{
				foreach (string text in this.defintion.gameObjectNames)
				{
					if (lodgroup.name.Contains(text))
					{
						this.foundLODGroups.Add(lodgroup);
					}
				}
			}
		}
		if (flag && this.foundLODGroups.Count > 0)
		{
			LOD[] lods = this.foundLODGroups[0].GetLODs();
			this.defintion.levels.lod0 = lods[0].screenRelativeTransitionHeight;
			this.defintion.levels.lod1 = lods[1].screenRelativeTransitionHeight;
			this.defintion.levels.lod2 = lods[2].screenRelativeTransitionHeight;
			this.defintion.levels.lod3 = lods[3].screenRelativeTransitionHeight;
			this.defintion.levels.lod4 = lods[4].screenRelativeTransitionHeight;
			if (lods.Length > 5)
			{
				this.defintion.levels.cull = lods[5].screenRelativeTransitionHeight;
			}
		}
		if (this.runSetOnce)
		{
			foreach (LODGroup lodgroup2 in this.foundLODGroups)
			{
				LOD[] lods2 = lodgroup2.GetLODs();
				lods2[0].screenRelativeTransitionHeight = this.defintion.levels.lod0;
				lods2[1].screenRelativeTransitionHeight = this.defintion.levels.lod1;
				lods2[2].screenRelativeTransitionHeight = this.defintion.levels.lod2;
				lods2[3].screenRelativeTransitionHeight = this.defintion.levels.lod3;
				lods2[4].screenRelativeTransitionHeight = this.defintion.levels.lod4;
				if (lods2.Length > 5)
				{
					lods2[5].screenRelativeTransitionHeight = this.defintion.levels.cull;
				}
				lodgroup2.SetLODs(lods2);
				lodgroup2.RecalculateBounds();
			}
		}
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x00005444 File Offset: 0x00003644
	private void OnValidate()
	{
	}

	// Token: 0x0400046C RID: 1132
	public LODGroupModifier.LODObjectsDefinition defintion;

	// Token: 0x0400046D RID: 1133
	public bool runSetEveryFrame;

	// Token: 0x0400046E RID: 1134
	public bool runSetOnce;

	// Token: 0x0400046F RID: 1135
	public bool runOneVerify;

	// Token: 0x04000470 RID: 1136
	public bool runOneFind;

	// Token: 0x04000471 RID: 1137
	public bool runOneCopy;

	// Token: 0x04000472 RID: 1138
	public List<LODGroup> foundLODGroups;

	// Token: 0x020003A0 RID: 928
	[Serializable]
	public class LODObjectsDefinition
	{
		// Token: 0x0400134F RID: 4943
		public string[] gameObjectNames;

		// Token: 0x04001350 RID: 4944
		public LODGroupModifier.LODObjectsDefinition.LODLevels levels;

		// Token: 0x020004D4 RID: 1236
		[Serializable]
		public class LODLevels
		{
			// Token: 0x06001A60 RID: 6752 RVA: 0x0008307C File Offset: 0x0008127C
			public void Verify()
			{
				if (this.lod4 < this.cull)
				{
					this.lod4 = this.cull;
				}
				if (this.lod3 < this.lod4)
				{
					this.lod3 = this.lod4;
				}
				if (this.lod2 < this.lod3)
				{
					this.lod2 = this.lod3;
				}
				if (this.lod1 < this.lod2)
				{
					this.lod1 = this.lod2;
				}
				if (this.lod0 < this.lod1)
				{
					this.lod0 = this.lod1;
				}
			}

			// Token: 0x040017E7 RID: 6119
			[Range(0f, 1f)]
			public float lod0 = 1f;

			// Token: 0x040017E8 RID: 6120
			[Range(0f, 1f)]
			public float lod1 = 0.7f;

			// Token: 0x040017E9 RID: 6121
			[Range(0f, 1f)]
			public float lod2 = 0.5f;

			// Token: 0x040017EA RID: 6122
			[Range(0f, 1f)]
			public float lod3 = 0.25f;

			// Token: 0x040017EB RID: 6123
			[Range(0f, 1f)]
			public float lod4 = 0.125f;

			// Token: 0x040017EC RID: 6124
			[Range(0f, 1f)]
			public float cull = 0.05f;
		}
	}
}
