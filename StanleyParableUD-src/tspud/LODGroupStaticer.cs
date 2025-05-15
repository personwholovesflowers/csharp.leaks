using System;
using UnityEngine;

// Token: 0x020000C7 RID: 199
[ExecuteInEditMode]
public class LODGroupStaticer : MonoBehaviour
{
	// Token: 0x060004A7 RID: 1191 RVA: 0x0001AF50 File Offset: 0x00019150
	private void Start()
	{
		if (Application.isPlaying)
		{
			this.refresh = this.runOnceOnStart;
			return;
		}
		this.refresh = false;
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x0001AF70 File Offset: 0x00019170
	private void OnGUI()
	{
		if (!this.showDebugButtons)
		{
			return;
		}
		GUILayout.Space(120f);
		GUILayout.Label("LOD bias = " + QualitySettings.lodBias, Array.Empty<GUILayoutOption>());
		foreach (float num in this.lodBiases)
		{
			if (GUILayout.Button("LOD bias = " + num, Array.Empty<GUILayoutOption>()))
			{
				QualitySettings.lodBias = num;
				this.refresh = true;
			}
		}
		if (GUILayout.Button(this.useBiasMultipliers ? "Using Bias Multiplier" : "NOT Using Bias Multiplier", Array.Empty<GUILayoutOption>()))
		{
			this.useBiasMultipliers = !this.useBiasMultipliers;
			this.refresh = true;
		}
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x0001B028 File Offset: 0x00019228
	private void Update()
	{
		if (this.calculateStaticLODLevel != null)
		{
			bool flag = this.verboseMode;
			this.verboseMode = true;
			this.CalculateStaticLODLevel(this.calculateStaticLODLevel);
			this.verboseMode = flag;
			this.calculateStaticLODLevel = null;
		}
		if (this.old_staticifyLODGroups != this.staticifyLODGroups)
		{
			this.refresh = true;
			this.old_staticifyLODGroups = this.staticifyLODGroups;
		}
		if (this.refresh)
		{
			if (this.staticifyLODGroups)
			{
				this.StaticifyLODGroups();
			}
			else
			{
				this.ResetLODGoups();
			}
			this.refresh = false;
		}
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x0001B0B4 File Offset: 0x000192B4
	private int CalculateStaticLODLevel(LODGroup lg)
	{
		if (this.verboseMode)
		{
			Debug.Log(lg, lg);
		}
		float num = Vector3.Distance(lg.transform.TransformPoint(lg.localReferencePoint), base.transform.position);
		bool flag = this.verboseMode;
		LODGroupStaticerBiasMultiplier component = lg.GetComponent<LODGroupStaticerBiasMultiplier>();
		float num2 = ((component == null || !this.useBiasMultipliers) ? 1f : component.GetBias());
		float num3 = lg.size / num * QualitySettings.lodBias * num2;
		if (this.verboseMode)
		{
			Debug.Log("LOD dist: " + num3);
		}
		LOD[] lods = lg.GetLODs();
		int num4 = 0;
		for (int i = 0; i < lods.Length; i++)
		{
			if (num3 < lods[i].screenRelativeTransitionHeight)
			{
				num4 = i + 1;
			}
		}
		if (this.verboseMode)
		{
			Debug.Log("LOD level: " + num4);
		}
		return num4;
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x0001B1A4 File Offset: 0x000193A4
	private void ResetLODGoups()
	{
		foreach (LODGroup lodgroup in Object.FindObjectsOfType<LODGroup>())
		{
			lodgroup.enabled = true;
			LOD[] lods = lodgroup.GetLODs();
			for (int j = 0; j < lods.Length; j++)
			{
				foreach (Renderer renderer in lods[j].renderers)
				{
					if (renderer != null)
					{
						renderer.gameObject.SetActive(true);
					}
				}
			}
		}
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x0001B224 File Offset: 0x00019424
	private void StaticifyLODGroups()
	{
		foreach (LODGroup lodgroup in Object.FindObjectsOfType<LODGroup>())
		{
			if (!(lodgroup.GetComponent<LODGroupStaticerIgnore>() != null))
			{
				LOD[] lods = lodgroup.GetLODs();
				int num = this.CalculateStaticLODLevel(lodgroup);
				lodgroup.enabled = false;
				for (int j = 0; j < lods.Length; j++)
				{
					if (this.verboseMode)
					{
						Debug.Log(j + ": " + lods[j].screenRelativeTransitionHeight);
					}
					foreach (Renderer renderer in lods[j].renderers)
					{
						if (this.verboseMode)
						{
							Debug.Log("Disabled: " + renderer);
						}
						if (renderer != null)
						{
							renderer.gameObject.SetActive(false);
						}
					}
				}
				if (this.verboseMode)
				{
					Debug.Log("lod level: " + num);
				}
				if (num != lods.Length)
				{
					foreach (Renderer renderer2 in lods[num].renderers)
					{
						if (this.verboseMode)
						{
							Debug.Log("Enabled: " + renderer2);
						}
						if (renderer2 != null)
						{
							renderer2.gameObject.SetActive(true);
						}
					}
				}
			}
		}
	}

	// Token: 0x04000473 RID: 1139
	private bool old_staticifyLODGroups;

	// Token: 0x04000474 RID: 1140
	public bool staticifyLODGroups;

	// Token: 0x04000475 RID: 1141
	public bool refresh;

	// Token: 0x04000476 RID: 1142
	public bool runOnceOnStart = true;

	// Token: 0x04000477 RID: 1143
	public bool useBiasMultipliers = true;

	// Token: 0x04000478 RID: 1144
	public bool showDebugButtons;

	// Token: 0x04000479 RID: 1145
	public LODGroup calculateStaticLODLevel;

	// Token: 0x0400047A RID: 1146
	private float[] lodBiases = new float[]
	{
		2f, 1.5f, 1f, 0.75f, 0.7f, 0.65f, 0.6f, 0.55f, 0.5f, 0.25f,
		0.1f, 0.01f
	};

	// Token: 0x0400047B RID: 1147
	private bool verboseMode;
}
