using System;
using UnityEngine;

// Token: 0x020000C3 RID: 195
public class CullForSwitchController : MonoBehaviour
{
	// Token: 0x06000494 RID: 1172 RVA: 0x0001A848 File Offset: 0x00018A48
	private void Start()
	{
		if (Application.isPlaying)
		{
			this.DoCulling();
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06000495 RID: 1173 RVA: 0x0001A857 File Offset: 0x00018A57
	public static bool IsSwitchEnvironment
	{
		get
		{
			return PlatformSettings.Instance.isSwitch.GetBooleanValue();
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x06000496 RID: 1174 RVA: 0x0001A868 File Offset: 0x00018A68
	private bool ShouldCull
	{
		get
		{
			if (this.itemsAreCulled == CullForSwitchController.CullMode.CullForSwitchAsExpected)
			{
				return CullForSwitchController.IsSwitchEnvironment;
			}
			if (this.itemsAreCulled == CullForSwitchController.CullMode.ForceCull)
			{
				return true;
			}
			if (this.itemsAreCulled == CullForSwitchController.CullMode.ForceDoNotCull)
			{
				return false;
			}
			Debug.LogWarning("CullMode invalid");
			return false;
		}
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x0001A899 File Offset: 0x00018A99
	[ContextMenu("Set To Cull")]
	private void SetCull()
	{
		this.itemsAreCulled = CullForSwitchController.CullMode.ForceCull;
		this.DoCulling();
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x0001A8A8 File Offset: 0x00018AA8
	[ContextMenu("Set To Not Cull")]
	private void SetNotCull()
	{
		this.itemsAreCulled = CullForSwitchController.CullMode.ForceDoNotCull;
		this.DoCulling();
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x0001A8B7 File Offset: 0x00018AB7
	[ContextMenu("Set To Cull On Switch")]
	private void SetCullOnSwitch()
	{
		this.itemsAreCulled = CullForSwitchController.CullMode.CullForSwitchAsExpected;
		this.DoCulling();
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x0001A8C8 File Offset: 0x00018AC8
	[ContextMenu("Do Culling Executions")]
	private void DoCulling()
	{
		bool shouldCull = this.ShouldCull;
		foreach (CullForSwitch cullForSwitch in Resources.FindObjectsOfTypeAll<CullForSwitch>())
		{
			cullForSwitch.gameObject.SetActive(shouldCull == cullForSwitch.invertCull);
		}
		if (this.doFarZSwitch)
		{
			this.farZVolume.farZ = (shouldCull ? this.culledFarZVolume : this.unculledFarZVolume);
		}
	}

	// Token: 0x04000460 RID: 1120
	public CullForSwitchController.CullMode itemsAreCulled;

	// Token: 0x04000461 RID: 1121
	public bool doFarZSwitch = true;

	// Token: 0x04000462 RID: 1122
	public FarZVolume farZVolume;

	// Token: 0x04000463 RID: 1123
	public float culledFarZVolume = 1000f;

	// Token: 0x04000464 RID: 1124
	public float unculledFarZVolume = 5000f;

	// Token: 0x04000465 RID: 1125
	public Camera mainCamera_AffectsEditorOnly;

	// Token: 0x0200039F RID: 927
	public enum CullMode
	{
		// Token: 0x0400134C RID: 4940
		CullForSwitchAsExpected,
		// Token: 0x0400134D RID: 4941
		ForceCull,
		// Token: 0x0400134E RID: 4942
		ForceDoNotCull
	}
}
