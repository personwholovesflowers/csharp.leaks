using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004E1 RID: 1249
[ExecuteInEditMode]
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class CausticVolumeManager : MonoSingleton<CausticVolumeManager>
{
	// Token: 0x06001CA5 RID: 7333 RVA: 0x000F088C File Offset: 0x000EEA8C
	private void OnValidate()
	{
		this.isDirty = true;
	}

	// Token: 0x06001CA6 RID: 7334 RVA: 0x000F0895 File Offset: 0x000EEA95
	private new void OnDestroy()
	{
		if (this.causticVolumeBuffer != null)
		{
			this.causticVolumeBuffer.Dispose();
		}
	}

	// Token: 0x06001CA7 RID: 7335 RVA: 0x000F08AA File Offset: 0x000EEAAA
	public void AddObject(ObjectBoundsToShader rendObj)
	{
		if (!this.objects.Contains(rendObj))
		{
			this.objects.Add(rendObj);
		}
		this.isDirty = true;
	}

	// Token: 0x06001CA8 RID: 7336 RVA: 0x000F08CD File Offset: 0x000EEACD
	public void RemoveObject(ObjectBoundsToShader rendObj)
	{
		this.objects.Remove(rendObj);
		this.isDirty = true;
	}

	// Token: 0x06001CA9 RID: 7337 RVA: 0x000F08E3 File Offset: 0x000EEAE3
	public void AddVolume(CausticVolume volume)
	{
		if (!this.causticVolumes.Contains(volume))
		{
			this.causticVolumes.Add(volume);
		}
		this.isDirty = true;
	}

	// Token: 0x06001CAA RID: 7338 RVA: 0x000F0906 File Offset: 0x000EEB06
	public void RemoveVolume(CausticVolume volume)
	{
		this.causticVolumes.Remove(volume);
		this.isDirty = true;
	}

	// Token: 0x06001CAB RID: 7339 RVA: 0x000F091C File Offset: 0x000EEB1C
	private void LateUpdate()
	{
		if (!this.isDirty)
		{
			return;
		}
		this.UpdateCausticData();
		this.isDirty = false;
	}

	// Token: 0x06001CAC RID: 7340 RVA: 0x000F0934 File Offset: 0x000EEB34
	private void Start()
	{
		if (this.causticVolumeBuffer == null || !this.causticVolumeBuffer.IsValid() || this.causticVolumeBuffer.count < this.causticVolumes.Count)
		{
			if (this.causticVolumeBuffer != null)
			{
				this.causticVolumeBuffer.Release();
			}
			this.causticVolumeBuffer = new ComputeBuffer(1, 32);
		}
		Shader.SetGlobalBuffer("_CausticVolumeData", this.causticVolumeBuffer);
	}

	// Token: 0x06001CAD RID: 7341 RVA: 0x000F09A0 File Offset: 0x000EEBA0
	private void UpdateCausticData()
	{
		for (int i = this.causticVolumes.Count - 1; i >= 0; i--)
		{
			if (this.causticVolumes[i] == null || !this.causticVolumes[i].isActiveAndEnabled)
			{
				this.RemoveVolume(this.causticVolumes[i]);
			}
		}
		foreach (ObjectBoundsToShader objectBoundsToShader in this.objects)
		{
			objectBoundsToShader.UpdateRendererBounds();
		}
		if (this.causticVolumeBuffer != null)
		{
			this.causticVolumeBuffer.Release();
		}
		this.causticDataArray.Clear();
		foreach (CausticVolume causticVolume in this.causticVolumes)
		{
			Vector4 vector = causticVolume.transform.position;
			vector.w = causticVolume.nearRadius;
			CausticVolumeManager.CausticData causticData;
			causticData.position_nearRadius = vector;
			Vector4 vector2 = causticVolume.color * causticVolume.intensity;
			vector2.w = causticVolume.farRadius;
			causticData.color_farRadius = vector2;
			this.causticDataArray.Add(causticData);
		}
		if (this.causticVolumeBuffer == null || !this.causticVolumeBuffer.IsValid() || this.causticVolumeBuffer.count < this.causticVolumes.Count)
		{
			if (this.causticVolumeBuffer != null)
			{
				this.causticVolumeBuffer.Release();
			}
			this.causticVolumeBuffer = new ComputeBuffer(Mathf.Max(1, this.causticVolumes.Count), 32);
		}
		this.causticVolumeBuffer.SetData<CausticVolumeManager.CausticData>(this.causticDataArray);
		Shader.SetGlobalBuffer("_CausticVolumeData", this.causticVolumeBuffer);
	}

	// Token: 0x040028AE RID: 10414
	public List<CausticVolume> causticVolumes = new List<CausticVolume>();

	// Token: 0x040028AF RID: 10415
	private List<ObjectBoundsToShader> objects = new List<ObjectBoundsToShader>();

	// Token: 0x040028B0 RID: 10416
	private List<CausticVolumeManager.CausticData> causticDataArray = new List<CausticVolumeManager.CausticData>();

	// Token: 0x040028B1 RID: 10417
	private ComputeBuffer causticVolumeBuffer;

	// Token: 0x040028B2 RID: 10418
	public bool isDirty;

	// Token: 0x020004E2 RID: 1250
	private struct CausticData
	{
		// Token: 0x040028B3 RID: 10419
		public Vector4 position_nearRadius;

		// Token: 0x040028B4 RID: 10420
		public Vector4 color_farRadius;
	}
}
