using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020003D2 RID: 978
[CreateAssetMenu(fileName = "Weapon Descriptor", menuName = "ULTRAKILL/Weapon Descriptor")]
public class WeaponDescriptor : ScriptableObject
{
	// Token: 0x04001E86 RID: 7814
	public string weaponName;

	// Token: 0x04001E87 RID: 7815
	[FormerlySerializedAs("weaponIcon")]
	public Sprite icon;

	// Token: 0x04001E88 RID: 7816
	public Sprite glowIcon;

	// Token: 0x04001E89 RID: 7817
	public WeaponVariant variationColor;
}
