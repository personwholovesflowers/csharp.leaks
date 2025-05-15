using System;
using System.Collections.Generic;
using ULTRAKILL.Cheats.UnityEditor;
using UnityEngine;

// Token: 0x02000447 RID: 1095
public class VoxelProxy : MonoBehaviour
{
	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x060018C2 RID: 6338 RVA: 0x000C8BFB File Offset: 0x000C6DFB
	public bool isBurning
	{
		get
		{
			return this.burningVoxel != null || this.exploded;
		}
	}

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x060018C3 RID: 6339 RVA: 0x000C8C13 File Offset: 0x000C6E13
	public List<GasolineStain> stains { get; } = new List<GasolineStain>();

	// Token: 0x060018C4 RID: 6340 RVA: 0x000C8C1B File Offset: 0x000C6E1B
	private void Awake()
	{
		if (this.burningVoxel == null)
		{
			this.burningVoxel = base.GetComponent<BurningVoxel>();
		}
		if (this.debug == null)
		{
			this.debug = base.GetComponent<VoxelProxyDebug>();
		}
	}

	// Token: 0x060018C5 RID: 6341 RVA: 0x000C8C54 File Offset: 0x000C6E54
	public void SetParent(Transform parent, bool isStatic)
	{
		this.isStatic = isStatic;
		this.parent = parent;
		Vector3 vector = this.ComputeCombinedHierarchyScale(parent);
		base.transform.SetParent(parent, true);
		base.transform.localScale = new Vector3(1f / vector.x, 1f / vector.y, 1f / vector.z);
		base.transform.localRotation = Quaternion.identity;
	}

	// Token: 0x060018C6 RID: 6342 RVA: 0x000C8CC8 File Offset: 0x000C6EC8
	private Vector3 ComputeCombinedHierarchyScale(Transform parent)
	{
		Vector3 vector = Vector3.one;
		Transform transform = parent;
		while (transform != null)
		{
			vector = Vector3.Scale(vector, transform.localScale);
			transform = transform.parent;
		}
		return vector;
	}

	// Token: 0x060018C7 RID: 6343 RVA: 0x000C8CFD File Offset: 0x000C6EFD
	public void Add(GasolineStain stain)
	{
		this.stains.Add(stain);
		stain.transform.SetParent(base.transform, true);
	}

	// Token: 0x060018C8 RID: 6344 RVA: 0x000C8D20 File Offset: 0x000C6F20
	public bool IsMatch(ProxySearchMode searchMode)
	{
		if (this.stains.Count == 0)
		{
			return false;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			return false;
		}
		if (!searchMode.HasFlag(ProxySearchMode.IncludeStatic) && this.isStatic)
		{
			return false;
		}
		if (!searchMode.HasFlag(ProxySearchMode.IncludeDynamic) && !this.isStatic)
		{
			return false;
		}
		if (!searchMode.HasFlag(ProxySearchMode.IncludeBurning) && this.isBurning)
		{
			return false;
		}
		if (!searchMode.HasFlag(ProxySearchMode.IncludeNotBurning) && !this.isBurning)
		{
			return false;
		}
		if (searchMode.HasFlag(ProxySearchMode.FloorOnly))
		{
			using (List<GasolineStain>.Enumerator enumerator = this.stains.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsFloor)
					{
						return true;
					}
				}
			}
			return false;
		}
		return true;
	}

	// Token: 0x060018C9 RID: 6345 RVA: 0x0000A719 File Offset: 0x00008919
	public void DestroySelf()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060018CA RID: 6346 RVA: 0x000C8E24 File Offset: 0x000C7024
	public void StartBurningOrRefuel()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.burningVoxel != null)
		{
			this.burningVoxel.Refuel();
			return;
		}
		this.burningVoxel = base.gameObject.AddComponent<BurningVoxel>();
		this.burningVoxel.Initialize(this);
	}

	// Token: 0x060018CB RID: 6347 RVA: 0x000C8E78 File Offset: 0x000C7078
	public void ExplodeAndDestroy()
	{
		this.exploded = true;
		this.DestroySelf();
		GameObject gameObject = Object.Instantiate<GameObject>(MonoSingleton<DefaultReferenceManager>.Instance.explosion, base.transform.position, Quaternion.identity);
		ExplosionController explosionController;
		if (gameObject.TryGetComponent<ExplosionController>(out explosionController))
		{
			explosionController.tryIgniteGasoline = false;
			explosionController.forceSimple = true;
			foreach (Explosion explosion in gameObject.GetComponentsInChildren<Explosion>())
			{
				explosion.lowQuality = true;
				explosion.HurtCooldownCollection = MonoSingleton<StainVoxelManager>.Instance.SharedHurtCooldownCollection;
			}
		}
	}

	// Token: 0x060018CC RID: 6348 RVA: 0x000C8EF8 File Offset: 0x000C70F8
	private void Update()
	{
		if (this.debug == null && NapalmDebugVoxels.Enabled)
		{
			this.debug = base.gameObject.AddComponent<VoxelProxyDebug>();
		}
		if (this.isStatic)
		{
			return;
		}
		Vector3Int vector3Int = StainVoxelManager.WorldToVoxelPosition(base.transform.position);
		if (vector3Int != this.voxel.VoxelPosition)
		{
			MonoSingleton<StainVoxelManager>.Instance.UpdateProxyPosition(this, vector3Int);
		}
	}

	// Token: 0x060018CD RID: 6349 RVA: 0x000C8F64 File Offset: 0x000C7164
	private void OnDestroy()
	{
		if (this.debug != null)
		{
			Object.Destroy(this.debug);
		}
		if (!base.gameObject.scene.isLoaded)
		{
			return;
		}
		this.voxel.RemoveProxy(this, true);
	}

	// Token: 0x060018CE RID: 6350 RVA: 0x000C8FB0 File Offset: 0x000C71B0
	public void SetStainSize(float size)
	{
		foreach (GasolineStain gasolineStain in this.stains)
		{
			gasolineStain.SetSize(size);
		}
	}

	// Token: 0x040022A0 RID: 8864
	[HideInInspector]
	public bool isStatic;

	// Token: 0x040022A1 RID: 8865
	public StainVoxel voxel;

	// Token: 0x040022A2 RID: 8866
	[HideInInspector]
	public Transform parent;

	// Token: 0x040022A4 RID: 8868
	private BurningVoxel burningVoxel;

	// Token: 0x040022A5 RID: 8869
	private VoxelProxyDebug debug;

	// Token: 0x040022A6 RID: 8870
	private bool exploded;
}
