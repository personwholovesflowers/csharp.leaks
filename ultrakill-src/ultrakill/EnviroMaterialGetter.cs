using System;
using UnityEngine;

// Token: 0x020001A0 RID: 416
public class EnviroMaterialGetter : MonoBehaviour
{
	// Token: 0x06000867 RID: 2151 RVA: 0x0003A1C0 File Offset: 0x000383C0
	private void Start()
	{
		if (this.oneTime)
		{
			this.Activate();
		}
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x0003A1D0 File Offset: 0x000383D0
	private void OnEnable()
	{
		if (!this.oneTime)
		{
			this.Activate();
		}
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x0003A1E0 File Offset: 0x000383E0
	private void Activate()
	{
		if (Vector3.Distance(base.transform.position, this.previousActivationPosition) < 1f)
		{
			return;
		}
		this.previousActivationPosition = base.transform.position;
		Vector3 vector = this.getRayDirection.normalized;
		if (this.relative)
		{
			vector = base.transform.InverseTransformDirection(vector);
		}
		SceneHelper.HitSurfaceData hitSurfaceData;
		if (!MonoSingleton<SceneHelper>.Instance.TryGetSurfaceData(base.transform.position - vector, vector, 5f, out hitSurfaceData))
		{
			if (this.oneTime)
			{
				base.enabled = false;
			}
			return;
		}
		if (this.targets == null || this.targets.Length == 0)
		{
			this.targets = base.GetComponentsInChildren<MeshRenderer>();
		}
		MeshRenderer[] array = this.targets;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].material = hitSurfaceData.material;
		}
		if (this.oneTime)
		{
			base.enabled = false;
		}
	}

	// Token: 0x04000B33 RID: 2867
	public bool oneTime = true;

	// Token: 0x04000B34 RID: 2868
	[HideInInspector]
	public Vector3 previousActivationPosition = Vector3.one * -9999f;

	// Token: 0x04000B35 RID: 2869
	public Vector3 getRayDirection = Vector3.down;

	// Token: 0x04000B36 RID: 2870
	public bool relative = true;

	// Token: 0x04000B37 RID: 2871
	[Space]
	public MeshRenderer[] targets;
}
