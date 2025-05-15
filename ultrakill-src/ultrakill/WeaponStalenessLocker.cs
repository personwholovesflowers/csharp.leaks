using System;
using UnityEngine;

// Token: 0x020004C6 RID: 1222
public class WeaponStalenessLocker : MonoBehaviour
{
	// Token: 0x06001BFC RID: 7164 RVA: 0x000E86EB File Offset: 0x000E68EB
	private void Start()
	{
		if (base.GetComponent<Collider>() == null && base.GetComponent<Rigidbody>() == null)
		{
			this.colliderless = true;
			this.Activate();
		}
	}

	// Token: 0x06001BFD RID: 7165 RVA: 0x000E8716 File Offset: 0x000E6916
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == MonoSingleton<NewMovement>.Instance.gameObject)
		{
			this.Activate();
		}
	}

	// Token: 0x06001BFE RID: 7166 RVA: 0x000E8738 File Offset: 0x000E6938
	public void Activate()
	{
		if (this.beenActivated && this.oneTime)
		{
			return;
		}
		this.beenActivated = true;
		switch (this.type)
		{
		case LockerType.State:
			MonoSingleton<StyleHUD>.Instance.LockFreshness(this.slot, new StyleFreshnessState?(this.minState), new StyleFreshnessState?(this.maxState));
			return;
		case LockerType.Value:
			MonoSingleton<StyleHUD>.Instance.LockFreshness(this.slot, new float?(this.minValue), new float?(this.maxValue));
			return;
		case LockerType.Unlocker:
			MonoSingleton<StyleHUD>.Instance.UnlockFreshness(this.slot);
			return;
		default:
			return;
		}
	}

	// Token: 0x0400277E RID: 10110
	public LockerType type;

	// Token: 0x0400277F RID: 10111
	public int slot;

	// Token: 0x04002780 RID: 10112
	public float minValue;

	// Token: 0x04002781 RID: 10113
	public float maxValue;

	// Token: 0x04002782 RID: 10114
	public StyleFreshnessState minState;

	// Token: 0x04002783 RID: 10115
	public StyleFreshnessState maxState;

	// Token: 0x04002784 RID: 10116
	public bool oneTime;

	// Token: 0x04002785 RID: 10117
	private bool beenActivated;

	// Token: 0x04002786 RID: 10118
	private bool colliderless;
}
