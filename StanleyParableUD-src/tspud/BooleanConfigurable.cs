using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
[CreateAssetMenu(fileName = "New Boolean Configurable", menuName = "Configurables/Configurable/Boolean Configurable")]
[Serializable]
public class BooleanConfigurable : Configurable
{
	// Token: 0x060000F3 RID: 243 RVA: 0x000088C9 File Offset: 0x00006AC9
	public override LiveData CreateDefaultLiveData()
	{
		return new LiveData(this.key, ConfigurableTypes.Boolean)
		{
			BooleanValue = this.defaultValue
		};
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x000088E3 File Offset: 0x00006AE3
	public override void SetNewConfiguredValue(LiveData argument)
	{
		base.SetNewConfiguredValue(argument);
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x000088EC File Offset: 0x00006AEC
	public void SetValue(bool value)
	{
		this.liveData.BooleanValue = value;
		this.SetNewConfiguredValue(this.liveData);
	}

	// Token: 0x04000166 RID: 358
	[SerializeField]
	private bool defaultValue;
}
