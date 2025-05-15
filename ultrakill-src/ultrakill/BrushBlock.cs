using System;
using Sandbox;
using UnityEngine;

// Token: 0x020003A0 RID: 928
public class BrushBlock : SandboxProp, IAlter, IAlterOptions<Vector3>
{
	// Token: 0x0600154D RID: 5453 RVA: 0x000ADF28 File Offset: 0x000AC128
	public SavedBlock SaveBrushBlock()
	{
		SavedBlock savedBlock = new SavedBlock();
		savedBlock.BlockSize = new SavedVector3(this.DataSize);
		savedBlock.BlockType = this.Type;
		SavedGeneric savedGeneric = savedBlock;
		base.BaseSave(ref savedGeneric);
		return savedBlock;
	}

	// Token: 0x0600154E RID: 5454 RVA: 0x000ADF64 File Offset: 0x000AC164
	public void RegenerateMesh()
	{
		Mesh mesh = SandboxUtils.GenerateProceduralMesh(this.DataSize, false);
		base.GetComponent<MeshFilter>().mesh = mesh;
		BoxCollider boxCollider = (this.OverrideCollider ? this.OverrideCollider : base.GetComponent<BoxCollider>());
		boxCollider.size = this.DataSize;
		boxCollider.size /= 2f;
		if (this.WaterTrigger)
		{
			this.WaterTrigger.size = this.DataSize;
			this.WaterTrigger.center = this.WaterTrigger.size / 2f;
		}
		if (MonoSingleton<SandboxNavmesh>.Instance != null && this.frozen)
		{
			MonoSingleton<SandboxNavmesh>.Instance.MarkAsDirty(this);
		}
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x0600154F RID: 5455 RVA: 0x000AE024 File Offset: 0x000AC224
	public string alterKey
	{
		get
		{
			return "block";
		}
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x06001550 RID: 5456 RVA: 0x000AE02B File Offset: 0x000AC22B
	public string alterCategoryName
	{
		get
		{
			return "Material Block";
		}
	}

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x06001551 RID: 5457 RVA: 0x000AE032 File Offset: 0x000AC232
	public AlterOption<Vector3>[] options
	{
		get
		{
			return new AlterOption<Vector3>[]
			{
				new AlterOption<Vector3>
				{
					name = "Size",
					key = null,
					value = this.DataSize,
					callback = delegate(Vector3 value)
					{
						this.DataSize = value;
						float num = 10000f;
						this.DataSize.x = Mathf.Min(this.DataSize.x, num);
						this.DataSize.y = Mathf.Min(this.DataSize.y, num);
						this.DataSize.z = Mathf.Min(this.DataSize.z, num);
						this.RegenerateMesh();
					}
				}
			};
		}
	}

	// Token: 0x04001D9C RID: 7580
	public Vector3 DataSize;

	// Token: 0x04001D9D RID: 7581
	public BlockType Type;

	// Token: 0x04001D9E RID: 7582
	public BoxCollider OverrideCollider;

	// Token: 0x04001D9F RID: 7583
	public BoxCollider WaterTrigger;
}
