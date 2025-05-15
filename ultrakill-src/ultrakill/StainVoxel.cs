using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200043D RID: 1085
[Serializable]
public class StainVoxel
{
	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x0600185F RID: 6239 RVA: 0x000C6DB8 File Offset: 0x000C4FB8
	public bool isEmpty
	{
		get
		{
			return this.staticProxy == null && (this.dynamicProxies == null || this.dynamicProxies.Count == 0);
		}
	}

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06001860 RID: 6240 RVA: 0x000C6DE2 File Offset: 0x000C4FE2
	public bool isBurning
	{
		get
		{
			return this.HasBurningProxies();
		}
	}

	// Token: 0x06001861 RID: 6241 RVA: 0x000C6DEC File Offset: 0x000C4FEC
	public StainVoxel(Vector3Int voxelPosition)
	{
		this.VoxelPosition = voxelPosition;
		this.HashCode = this.VoxelPosition.GetHashCode();
		this.RoundedWorldPosition = StainVoxelManager.VoxelToWorldPosition(voxelPosition);
		this.staticProxy = null;
		this.dynamicProxies = null;
	}

	// Token: 0x06001862 RID: 6242 RVA: 0x000C6E44 File Offset: 0x000C5044
	public VoxelProxy CreateOrGetProxyFor(GasolineStain stain)
	{
		if (stain.IsStatic)
		{
			if (this.staticProxy == null)
			{
				this.staticProxy = this.CreateNewProxy(stain, true);
			}
			this.staticProxy.Add(stain);
			return this.staticProxy;
		}
		if (this.dynamicProxies == null)
		{
			this.dynamicProxies = new Dictionary<Transform, List<VoxelProxy>>();
		}
		if (!this.dynamicProxies.ContainsKey(stain.Parent))
		{
			this.dynamicProxies[stain.Parent] = new List<VoxelProxy>();
		}
		if (this.dynamicProxies[stain.Parent].Count == 0)
		{
			VoxelProxy voxelProxy = this.CreateNewProxy(stain, false);
			this.dynamicProxies[stain.Parent].Add(voxelProxy);
		}
		this.dynamicProxies[stain.Parent][0].Add(stain);
		return this.dynamicProxies[stain.Parent][0];
	}

	// Token: 0x06001863 RID: 6243 RVA: 0x000C6F30 File Offset: 0x000C5130
	public void AcknowledgeNewStain()
	{
		if (!this.isBurning)
		{
			return;
		}
		foreach (VoxelProxy voxelProxy in this.GetProxies(ProxySearchMode.Any).ToList<VoxelProxy>())
		{
			voxelProxy.StartBurningOrRefuel();
		}
	}

	// Token: 0x06001864 RID: 6244 RVA: 0x000C6F90 File Offset: 0x000C5190
	public void AddProxy(VoxelProxy existingProxy)
	{
		if (existingProxy.isBurning)
		{
			this.TryIgnite();
		}
		else if (this.isBurning)
		{
			existingProxy.StartBurningOrRefuel();
		}
		if (existingProxy.isStatic)
		{
			this.staticProxy = existingProxy;
		}
		else
		{
			if (this.dynamicProxies == null)
			{
				this.dynamicProxies = new Dictionary<Transform, List<VoxelProxy>>();
			}
			if (!this.dynamicProxies.ContainsKey(existingProxy.parent))
			{
				this.dynamicProxies[existingProxy.parent] = new List<VoxelProxy>();
			}
			this.dynamicProxies[existingProxy.parent].Add(existingProxy);
		}
		existingProxy.voxel = this;
	}

	// Token: 0x06001865 RID: 6245 RVA: 0x000C7028 File Offset: 0x000C5228
	public void RemoveProxy(VoxelProxy proxy, bool destroy = true)
	{
		if (proxy.isStatic)
		{
			if (this.staticProxy == proxy)
			{
				this.staticProxy = null;
			}
		}
		else
		{
			if (this.dynamicProxies == null)
			{
				return;
			}
			if (this.dynamicProxies.ContainsKey(proxy.parent))
			{
				this.dynamicProxies[proxy.parent].Remove(proxy);
				if (this.dynamicProxies[proxy.parent].Count == 0)
				{
					this.dynamicProxies.Remove(proxy.parent);
				}
			}
			if (this.dynamicProxies.Count == 0)
			{
				this.dynamicProxies = null;
			}
		}
		if (destroy)
		{
			proxy.DestroySelf();
		}
		MonoSingleton<StainVoxelManager>.Instance.RefreshVoxel(this);
	}

	// Token: 0x06001866 RID: 6246 RVA: 0x000C70DC File Offset: 0x000C52DC
	public void DestroySelf()
	{
		if (this.staticProxy != null)
		{
			this.staticProxy.DestroySelf();
		}
		if (this.dynamicProxies != null)
		{
			foreach (List<VoxelProxy> list in this.dynamicProxies.Values)
			{
				foreach (VoxelProxy voxelProxy in list)
				{
					voxelProxy.DestroySelf();
				}
			}
		}
		this.staticProxy = null;
		this.dynamicProxies = null;
	}

	// Token: 0x06001867 RID: 6247 RVA: 0x000C7194 File Offset: 0x000C5394
	public IEnumerable<VoxelProxy> GetProxies(ProxySearchMode mode)
	{
		if (mode.HasFlag(ProxySearchMode.IncludeStatic) && this.staticProxy != null && this.staticProxy.IsMatch(mode))
		{
			yield return this.staticProxy;
		}
		if (mode.HasFlag(ProxySearchMode.IncludeDynamic) && this.dynamicProxies != null)
		{
			foreach (List<VoxelProxy> list in this.dynamicProxies.Values)
			{
				foreach (VoxelProxy voxelProxy in list)
				{
					if (voxelProxy.IsMatch(mode))
					{
						yield return voxelProxy;
					}
				}
				List<VoxelProxy>.Enumerator enumerator2 = default(List<VoxelProxy>.Enumerator);
			}
			Dictionary<Transform, List<VoxelProxy>>.ValueCollection.Enumerator enumerator = default(Dictionary<Transform, List<VoxelProxy>>.ValueCollection.Enumerator);
		}
		yield break;
		yield break;
	}

	// Token: 0x06001868 RID: 6248 RVA: 0x000C71AB File Offset: 0x000C53AB
	public bool HasFloorStains()
	{
		return this.GetProxies(ProxySearchMode.FloorOnly).Any<VoxelProxy>();
	}

	// Token: 0x06001869 RID: 6249 RVA: 0x000C71B9 File Offset: 0x000C53B9
	public bool HasBurningProxies()
	{
		return this.GetProxies(ProxySearchMode.AnyBurning).Any<VoxelProxy>();
	}

	// Token: 0x0600186A RID: 6250 RVA: 0x000C71C8 File Offset: 0x000C53C8
	public bool HasStains(ProxySearchMode mode)
	{
		return this.GetProxies(mode).Any<VoxelProxy>();
	}

	// Token: 0x0600186B RID: 6251 RVA: 0x000C71D8 File Offset: 0x000C53D8
	public bool TryIgnite()
	{
		if (this.isBurning)
		{
			return false;
		}
		List<VoxelProxy> list = this.GetProxies(ProxySearchMode.AnyNotBurning).ToList<VoxelProxy>();
		if (list.Count == 0)
		{
			return false;
		}
		StainVoxelManager instance = MonoSingleton<StainVoxelManager>.Instance;
		bool flag = true;
		if (Physics.OverlapSphereNonAlloc(this.RoundedWorldPosition, 1.375f, this.waterOverlapResult, 16, QueryTriggerInteraction.Collide) > 0)
		{
			flag = false;
			int num = 65536;
			num |= 262144;
			int num2 = Physics.OverlapSphereNonAlloc(this.RoundedWorldPosition, 1.375f, this.waterOverlapResult, num, QueryTriggerInteraction.Collide);
			using (IEnumerator<Collider> enumerator = this.waterOverlapResult.Take(num2).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DryZone dryZone;
					if (enumerator.Current.TryGetComponent<DryZone>(out dryZone))
					{
						flag = true;
						break;
					}
				}
			}
		}
		if (flag)
		{
			using (List<VoxelProxy>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					VoxelProxy voxelProxy = enumerator2.Current;
					voxelProxy.StartBurningOrRefuel();
				}
				goto IL_014F;
			}
		}
		if (instance.ShouldExplodeAt(this.VoxelPosition))
		{
			using (List<VoxelProxy>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					VoxelProxy voxelProxy2 = enumerator2.Current;
					voxelProxy2.ExplodeAndDestroy();
				}
				goto IL_014F;
			}
		}
		foreach (VoxelProxy voxelProxy3 in list)
		{
			voxelProxy3.DestroySelf();
		}
		IL_014F:
		MonoSingleton<StainVoxelManager>.Instance.ScheduleFirePropagation(this);
		return true;
	}

	// Token: 0x0600186C RID: 6252 RVA: 0x000C7374 File Offset: 0x000C5574
	private VoxelProxy CreateNewProxy(GasolineStain stain, bool isStatic)
	{
		VoxelProxy voxelProxy = new GameObject(this.GetProxyName())
		{
			transform = 
			{
				position = this.RoundedWorldPosition
			}
		}.AddComponent<VoxelProxy>();
		voxelProxy.gameObject.AddComponent<DestroyOnCheckpointRestart>();
		Transform parent = stain.transform.parent;
		voxelProxy.SetParent(parent, isStatic);
		voxelProxy.voxel = this;
		return voxelProxy;
	}

	// Token: 0x0600186D RID: 6253 RVA: 0x000C73C9 File Offset: 0x000C55C9
	public string GetProxyName()
	{
		return "VoxelProxy";
	}

	// Token: 0x0600186E RID: 6254 RVA: 0x000C73D0 File Offset: 0x000C55D0
	public override int GetHashCode()
	{
		return this.HashCode;
	}

	// Token: 0x0600186F RID: 6255 RVA: 0x000C73D8 File Offset: 0x000C55D8
	public override bool Equals(object obj)
	{
		StainVoxel stainVoxel = obj as StainVoxel;
		return stainVoxel != null && this.HashCode == stainVoxel.HashCode;
	}

	// Token: 0x04002240 RID: 8768
	public readonly Vector3Int VoxelPosition;

	// Token: 0x04002241 RID: 8769
	public readonly Vector3 RoundedWorldPosition;

	// Token: 0x04002242 RID: 8770
	public readonly int HashCode;

	// Token: 0x04002243 RID: 8771
	public VoxelProxy staticProxy;

	// Token: 0x04002244 RID: 8772
	public Dictionary<Transform, List<VoxelProxy>> dynamicProxies;

	// Token: 0x04002245 RID: 8773
	private readonly Collider[] waterOverlapResult = new Collider[3];

	// Token: 0x04002246 RID: 8774
	private const string StaticVoxelName = "VoxelProxy";
}
