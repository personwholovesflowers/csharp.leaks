using System;
using plog;
using Sandbox;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020003B2 RID: 946
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class SandboxNavmesh : MonoSingleton<SandboxNavmesh>
{
	// Token: 0x06001588 RID: 5512 RVA: 0x000AE8A0 File Offset: 0x000ACAA0
	protected override void Awake()
	{
		base.Awake();
		this.defaultCenter = this.surface.center;
		this.defaultSize = this.surface.size;
	}

	// Token: 0x06001589 RID: 5513 RVA: 0x000AE8CC File Offset: 0x000ACACC
	public void MarkAsDirty(SandboxSpawnableInstance instance)
	{
		if (this.isDirty)
		{
			return;
		}
		if (instance)
		{
			if (!instance.frozen)
			{
				return;
			}
			if (instance.sourceObject != null && (instance.sourceObject.isWater || instance.sourceObject.triggerOnly || instance.sourceObject.spawnableObjectType == SpawnableObject.SpawnableObjectDataType.Enemy))
			{
				return;
			}
		}
		MonoSingleton<SandboxHud>.Instance.NavmeshDirty();
		this.isDirty = true;
		MonoSingleton<CheatsManager>.Instance.RenderCheatsInfo();
	}

	// Token: 0x0600158A RID: 5514 RVA: 0x000AE948 File Offset: 0x000ACB48
	public void Rebake()
	{
		this.surface.BuildNavMesh();
		SandboxNavmesh.Log.Info("Navmesh built", null, null, null);
		this.isDirty = false;
		MonoSingleton<SandboxHud>.Instance.HideNavmeshNotice();
		if (this.navmeshBuilt != null)
		{
			this.navmeshBuilt();
		}
		MonoSingleton<CheatsManager>.Instance.RenderCheatsInfo();
	}

	// Token: 0x0600158B RID: 5515 RVA: 0x000AE9A0 File Offset: 0x000ACBA0
	private void OnDrawGizmos()
	{
		if (this.surface == null)
		{
			return;
		}
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(this.surface.center + base.transform.position, this.surface.size);
	}

	// Token: 0x0600158C RID: 5516 RVA: 0x000AE9F1 File Offset: 0x000ACBF1
	public void ResetSizeToDefault()
	{
		this.surface.center = this.defaultCenter;
		this.surface.size = this.defaultSize;
	}

	// Token: 0x0600158D RID: 5517 RVA: 0x000AEA18 File Offset: 0x000ACC18
	public void EnsurePositionWithinBounds(Vector3 worldPosition)
	{
		Vector3 vector = this.surface.center + base.transform.position;
		float num = 1f;
		if (worldPosition.x < vector.x - this.surface.size.x / 2f)
		{
			float num2 = vector.x - this.surface.size.x / 2f - worldPosition.x + num;
			this.surface.center = new Vector3(this.surface.center.x - num2 / 2f, this.surface.center.y, this.surface.center.z);
			this.surface.size = new Vector3(this.surface.size.x + num2, this.surface.size.y, this.surface.size.z);
		}
		else if (worldPosition.x > vector.x + this.surface.size.x / 2f)
		{
			float num3 = worldPosition.x - (vector.x + this.surface.size.x / 2f) + num;
			this.surface.center = new Vector3(this.surface.center.x + num3 / 2f, this.surface.center.y, this.surface.center.z);
			this.surface.size = new Vector3(this.surface.size.x + num3, this.surface.size.y, this.surface.size.z);
		}
		if (worldPosition.y < vector.y - this.surface.size.y / 2f)
		{
			float num4 = vector.y - this.surface.size.y / 2f - worldPosition.y + num;
			this.surface.center = new Vector3(this.surface.center.x, this.surface.center.y - num4 / 2f, this.surface.center.z);
			this.surface.size = new Vector3(this.surface.size.x, this.surface.size.y + num4, this.surface.size.z);
		}
		else if (worldPosition.y > vector.y + this.surface.size.y / 2f)
		{
			float num5 = worldPosition.y - (vector.y + this.surface.size.y / 2f) + num;
			this.surface.center = new Vector3(this.surface.center.x, this.surface.center.y + num5 / 2f, this.surface.center.z);
			this.surface.size = new Vector3(this.surface.size.x, this.surface.size.y + num5, this.surface.size.z);
		}
		if (worldPosition.z < vector.z - this.surface.size.z / 2f)
		{
			float num6 = vector.z - this.surface.size.z / 2f - worldPosition.z + num;
			this.surface.center = new Vector3(this.surface.center.x, this.surface.center.y, this.surface.center.z - num6 / 2f);
			this.surface.size = new Vector3(this.surface.size.x, this.surface.size.y, this.surface.size.z + num6);
			return;
		}
		if (worldPosition.z > vector.z + this.surface.size.z / 2f)
		{
			float num7 = worldPosition.z - (vector.z + this.surface.size.z / 2f) + num;
			this.surface.center = new Vector3(this.surface.center.x, this.surface.center.y, this.surface.center.z + num7 / 2f);
			this.surface.size = new Vector3(this.surface.size.x, this.surface.size.y, this.surface.size.z + num7);
		}
	}

	// Token: 0x04001DE0 RID: 7648
	private static readonly global::plog.Logger Log = new global::plog.Logger("SandboxNavmesh");

	// Token: 0x04001DE1 RID: 7649
	[SerializeField]
	private NavMeshSurface surface;

	// Token: 0x04001DE2 RID: 7650
	public bool isDirty;

	// Token: 0x04001DE3 RID: 7651
	public UnityAction navmeshBuilt;

	// Token: 0x04001DE4 RID: 7652
	private Vector3 defaultCenter;

	// Token: 0x04001DE5 RID: 7653
	private Vector3 defaultSize;
}
