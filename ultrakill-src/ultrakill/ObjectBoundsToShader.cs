using System;
using UnityEngine;

// Token: 0x020004E3 RID: 1251
[ExecuteInEditMode]
public class ObjectBoundsToShader : MonoBehaviour
{
	// Token: 0x06001CAF RID: 7343 RVA: 0x000F0BA8 File Offset: 0x000EEDA8
	private void OnValidate()
	{
		this.manager = MonoSingleton<CausticVolumeManager>.Instance;
		if (this.manager == null)
		{
			this.manager = Object.FindAnyObjectByType<CausticVolumeManager>(FindObjectsInactive.Include);
		}
		if (this.manager == null)
		{
			GameObject gameObject = new GameObject("CausticVolumeManager");
			this.manager = gameObject.AddComponent<CausticVolumeManager>();
		}
		this.manager.AddObject(this);
	}

	// Token: 0x06001CB0 RID: 7344 RVA: 0x000F0C0B File Offset: 0x000EEE0B
	private void OnEnable()
	{
		this.manager.AddObject(this);
	}

	// Token: 0x06001CB1 RID: 7345 RVA: 0x000F0C19 File Offset: 0x000EEE19
	private void OnDisable()
	{
		this.manager.RemoveObject(this);
	}

	// Token: 0x06001CB2 RID: 7346 RVA: 0x000F0C27 File Offset: 0x000EEE27
	private void OnDestroy()
	{
		if (this.manager == null)
		{
			return;
		}
		this.manager.RemoveObject(this);
	}

	// Token: 0x06001CB3 RID: 7347 RVA: 0x000F0C44 File Offset: 0x000EEE44
	private void Update()
	{
		if (base.transform.hasChanged)
		{
			base.transform.hasChanged = false;
			if (this.manager != null)
			{
				this.manager.isDirty = true;
				return;
			}
		}
		else if (this.customCenter != this.lastCustomBounds)
		{
			this.lastCustomBounds = this.customCenter;
			if (this.manager != null)
			{
				this.manager.isDirty = true;
			}
		}
	}

	// Token: 0x06001CB4 RID: 7348 RVA: 0x000F0CC0 File Offset: 0x000EEEC0
	public void UpdateRendererBounds()
	{
		this.rend = base.GetComponent<MeshRenderer>();
		if (this.rend == null)
		{
			return;
		}
		if (!this.useCustomCenter)
		{
			this.customCenter = this.rend.bounds.center;
		}
		Vector4 vector = (this.useCustomCenter ? this.customCenter : this.rend.bounds.center);
		vector.w = (float)this.manager.causticVolumes.Count;
		if (this.propertyBlock == null)
		{
			this.propertyBlock = new MaterialPropertyBlock();
		}
		this.rend.GetPropertyBlock(this.propertyBlock);
		this.propertyBlock.SetVector("_BoundsCenter_VolumeCount", vector);
		this.rend.SetPropertyBlock(this.propertyBlock);
	}

	// Token: 0x040028B5 RID: 10421
	public bool useCustomCenter;

	// Token: 0x040028B6 RID: 10422
	public Vector3 customCenter;

	// Token: 0x040028B7 RID: 10423
	private MeshRenderer rend;

	// Token: 0x040028B8 RID: 10424
	private MaterialPropertyBlock propertyBlock;

	// Token: 0x040028B9 RID: 10425
	[HideInInspector]
	public CausticVolumeManager manager;

	// Token: 0x040028BA RID: 10426
	private Vector3 lastCustomBounds;
}
