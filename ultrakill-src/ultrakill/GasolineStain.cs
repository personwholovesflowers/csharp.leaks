using System;
using UnityEngine;

// Token: 0x02000223 RID: 547
public class GasolineStain : MonoBehaviour
{
	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x00052A52 File Offset: 0x00050C52
	// (set) Token: 0x06000BC2 RID: 3010 RVA: 0x00052A5A File Offset: 0x00050C5A
	public Transform Parent { get; private set; }

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x00052A63 File Offset: 0x00050C63
	// (set) Token: 0x06000BC4 RID: 3012 RVA: 0x00052A6B File Offset: 0x00050C6B
	public bool IsStatic { get; private set; } = true;

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x00052A74 File Offset: 0x00050C74
	// (set) Token: 0x06000BC6 RID: 3014 RVA: 0x00052A7C File Offset: 0x00050C7C
	public bool IsFloor { get; private set; }

	// Token: 0x06000BC7 RID: 3015 RVA: 0x00052A85 File Offset: 0x00050C85
	private void Awake()
	{
		this.initialSize = base.transform.localScale;
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x00052A98 File Offset: 0x00050C98
	private void Start()
	{
		this.IsFloor = this.CalculateDot() > 0.25f;
		this.initialSize = base.transform.localScale;
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x00052ABE File Offset: 0x00050CBE
	private float CalculateDot()
	{
		return Vector3.Dot(-base.transform.forward, Vector3.up);
	}

	// Token: 0x06000BCA RID: 3018 RVA: 0x00052ADC File Offset: 0x00050CDC
	public void AttachTo(Collider other, bool clipToSurface)
	{
		Transform transform = other.transform;
		GameObject gameObject = other.gameObject;
		base.transform.SetParent(transform, true);
		this.Parent = transform;
		Rigidbody rigidbody;
		if (gameObject.CompareTag("Moving") || (MonoSingleton<ComponentsDatabase>.Instance && MonoSingleton<ComponentsDatabase>.Instance.scrollers.Contains(transform)) || (gameObject.TryGetComponent<Rigidbody>(out rigidbody) && !rigidbody.isKinematic))
		{
			this.IsStatic = false;
		}
		else
		{
			this.IsStatic = true;
		}
		StainVoxelManager instance = MonoSingleton<StainVoxelManager>.Instance;
		if (!instance.usedComputeShadersAtStart)
		{
			this.mRend = base.GetComponent<MeshRenderer>();
			this.mRend.enabled = true;
			this.propBlock = new MaterialPropertyBlock();
			this.propBlock.SetInteger("_Index", Random.Range(0, 5));
			this.propBlock.SetFloat("_ClipToSurface", (float)(clipToSurface ? 1 : 0));
			this.mRend.SetPropertyBlock(this.propBlock);
		}
		else
		{
			instance.AddGasolineStain(base.transform, clipToSurface);
		}
		Vector3 forward = base.transform.forward;
		Vector3 vector = base.transform.position + forward * -0.5f;
		StainVoxel stainVoxel = instance.CreateOrGetVoxel(vector, false);
		VoxelProxy voxelProxy = stainVoxel.CreateOrGetProxyFor(this);
		instance.AcknowledgeNewStain(stainVoxel);
		ScrollingTexture scrollingTexture;
		if (!this.IsStatic && MonoSingleton<ComponentsDatabase>.Instance && MonoSingleton<ComponentsDatabase>.Instance.scrollers.Contains(transform) && transform.TryGetComponent<ScrollingTexture>(out scrollingTexture) && !scrollingTexture.attachedObjects.Contains(voxelProxy.transform))
		{
			scrollingTexture.attachedObjects.Add(voxelProxy.transform);
		}
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x00052A85 File Offset: 0x00050C85
	public void OnTransformParentChanged()
	{
		this.initialSize = base.transform.localScale;
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x00052C78 File Offset: 0x00050E78
	public void SetSize(float size)
	{
		base.transform.localScale = this.initialSize * size;
	}

	// Token: 0x04000F61 RID: 3937
	private Vector3 initialSize;

	// Token: 0x04000F62 RID: 3938
	private int index;

	// Token: 0x04000F63 RID: 3939
	private MeshRenderer mRend;

	// Token: 0x04000F64 RID: 3940
	private MaterialPropertyBlock propBlock;
}
