using System;
using UnityEngine;

// Token: 0x020000C5 RID: 197
public class DemoUI : MonoBehaviour
{
	// Token: 0x060004A1 RID: 1185 RVA: 0x0001AA3F File Offset: 0x00018C3F
	private void Start()
	{
		this.m_SSAOPro = base.GetComponent<SSAOPro>();
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x0001AA50 File Offset: 0x00018C50
	private void OnGUI()
	{
		GUI.Box(new Rect(10f, 10f, 130f, 194f), "");
		GUI.BeginGroup(new Rect(20f, 15f, 200f, 200f));
		this.m_SSAOPro.enabled = GUILayout.Toggle(this.m_SSAOPro.enabled, "Enable SSAO", Array.Empty<GUILayoutOption>());
		this.m_SSAOPro.DebugAO = GUILayout.Toggle(this.m_SSAOPro.DebugAO, "Show AO Only", Array.Empty<GUILayoutOption>());
		bool flag = this.m_SSAOPro.Blur == SSAOPro.BlurMode.HighQualityBilateral;
		flag = GUILayout.Toggle(flag, "HQ Bilateral Blur", Array.Empty<GUILayoutOption>());
		this.m_SSAOPro.Blur = (flag ? SSAOPro.BlurMode.HighQualityBilateral : SSAOPro.BlurMode.None);
		GUILayout.Space(10f);
		bool flag2 = this.m_SSAOPro.Samples == SSAOPro.SampleCount.VeryLow;
		flag2 = GUILayout.Toggle(flag2, "4 samples", Array.Empty<GUILayoutOption>());
		this.m_SSAOPro.Samples = (flag2 ? SSAOPro.SampleCount.VeryLow : this.m_SSAOPro.Samples);
		flag2 = this.m_SSAOPro.Samples == SSAOPro.SampleCount.Low;
		flag2 = GUILayout.Toggle(flag2, "8 samples", Array.Empty<GUILayoutOption>());
		this.m_SSAOPro.Samples = (flag2 ? SSAOPro.SampleCount.Low : this.m_SSAOPro.Samples);
		flag2 = this.m_SSAOPro.Samples == SSAOPro.SampleCount.Medium;
		flag2 = GUILayout.Toggle(flag2, "12 samples", Array.Empty<GUILayoutOption>());
		this.m_SSAOPro.Samples = (flag2 ? SSAOPro.SampleCount.Medium : this.m_SSAOPro.Samples);
		flag2 = this.m_SSAOPro.Samples == SSAOPro.SampleCount.High;
		flag2 = GUILayout.Toggle(flag2, "16 samples", Array.Empty<GUILayoutOption>());
		this.m_SSAOPro.Samples = (flag2 ? SSAOPro.SampleCount.High : this.m_SSAOPro.Samples);
		flag2 = this.m_SSAOPro.Samples == SSAOPro.SampleCount.Ultra;
		flag2 = GUILayout.Toggle(flag2, "20 samples", Array.Empty<GUILayoutOption>());
		this.m_SSAOPro.Samples = (flag2 ? SSAOPro.SampleCount.Ultra : this.m_SSAOPro.Samples);
		GUI.EndGroup();
	}

	// Token: 0x0400046B RID: 1131
	protected SSAOPro m_SSAOPro;
}
