using System;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class ChangeMaterials : MonoBehaviour
{
	// Token: 0x06000350 RID: 848 RVA: 0x00014CA4 File Offset: 0x00012EA4
	public void Activate()
	{
		if (!this.smr)
		{
			this.smr = base.GetComponentInParent<SkinnedMeshRenderer>();
		}
		if (this.smr)
		{
			if (this.oldMaterials == null || this.oldMaterials.Length == 0)
			{
				this.oldMaterials = this.smr.materials;
			}
			this.smr.materials = this.materials;
			EnemySimplifier enemySimplifier;
			if (this.enemySimplifierOverride && base.TryGetComponent<EnemySimplifier>(out enemySimplifier))
			{
				EnemySimplifier.MaterialState materialState = (this.enraged ? EnemySimplifier.MaterialState.enraged : EnemySimplifier.MaterialState.normal);
				for (int i = 0; i < this.smr.materials.Length; i++)
				{
					if (this.oldMaterials[i] != this.smr.materials[i])
					{
						enemySimplifier.ChangeMaterialNew(materialState, this.materials[i]);
					}
				}
			}
			return;
		}
		if (!this.mr)
		{
			this.mr = base.GetComponentInParent<MeshRenderer>();
		}
		if (this.mr)
		{
			if (this.oldMaterials == null || this.oldMaterials.Length == 0)
			{
				this.oldMaterials = this.mr.materials;
			}
			this.mr.materials = this.materials;
			EnemySimplifier enemySimplifier2;
			if (this.enemySimplifierOverride && base.TryGetComponent<EnemySimplifier>(out enemySimplifier2))
			{
				EnemySimplifier.MaterialState materialState2 = (this.enraged ? EnemySimplifier.MaterialState.enraged : EnemySimplifier.MaterialState.normal);
				for (int j = 0; j < this.mr.materials.Length; j++)
				{
					if (this.oldMaterials[j] != this.mr.materials[j])
					{
						enemySimplifier2.ChangeMaterialNew(materialState2, this.materials[j]);
					}
				}
			}
		}
	}

	// Token: 0x06000351 RID: 849 RVA: 0x00014E38 File Offset: 0x00013038
	public void Reverse()
	{
		if (this.oldMaterials == null || this.oldMaterials.Length == 0)
		{
			return;
		}
		if (this.smr)
		{
			this.smr.materials = this.oldMaterials;
			EnemySimplifier enemySimplifier;
			if (this.enemySimplifierOverride && base.TryGetComponent<EnemySimplifier>(out enemySimplifier))
			{
				for (int i = 0; i < this.smr.materials.Length; i++)
				{
					if (this.materials[i] != this.smr.materials[i])
					{
						enemySimplifier.ChangeMaterial(this.materials[i], this.oldMaterials[i]);
					}
				}
				return;
			}
		}
		else if (this.mr)
		{
			this.mr.materials = this.oldMaterials;
			EnemySimplifier enemySimplifier2;
			if (this.enemySimplifierOverride && base.TryGetComponent<EnemySimplifier>(out enemySimplifier2))
			{
				for (int j = 0; j < this.mr.materials.Length; j++)
				{
					if (this.materials[j] != this.mr.materials[j])
					{
						enemySimplifier2.ChangeMaterial(this.materials[j], this.oldMaterials[j]);
					}
				}
			}
		}
	}

	// Token: 0x0400042E RID: 1070
	public Material[] materials;

	// Token: 0x0400042F RID: 1071
	private SkinnedMeshRenderer smr;

	// Token: 0x04000430 RID: 1072
	private MeshRenderer mr;

	// Token: 0x04000431 RID: 1073
	private Material[] oldMaterials;

	// Token: 0x04000432 RID: 1074
	public bool enemySimplifierOverride;

	// Token: 0x04000433 RID: 1075
	public bool enraged;
}
