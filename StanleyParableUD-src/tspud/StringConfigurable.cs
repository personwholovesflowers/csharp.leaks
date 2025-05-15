using System;
using UnityEngine;

// Token: 0x02000046 RID: 70
[CreateAssetMenu(fileName = "New String Configurable", menuName = "Configurables/Configurable/String Configurable")]
[Serializable]
public class StringConfigurable : Configurable
{
	// Token: 0x06000179 RID: 377 RVA: 0x0000A2F1 File Offset: 0x000084F1
	public override LiveData CreateDefaultLiveData()
	{
		return new LiveData(this.key, ConfigurableTypes.String)
		{
			StringValue = this.defaultValue
		};
	}

	// Token: 0x0600017A RID: 378 RVA: 0x0000A30B File Offset: 0x0000850B
	public void SetValue(string value)
	{
		this.liveData.StringValue = value;
		this.SetNewConfiguredValue(this.liveData);
	}

	// Token: 0x040001C6 RID: 454
	[SerializeField]
	private string defaultValue = "";
}
