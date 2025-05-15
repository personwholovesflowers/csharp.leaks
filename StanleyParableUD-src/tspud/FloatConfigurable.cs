using System;
using UnityEngine;

// Token: 0x02000040 RID: 64
[CreateAssetMenu(fileName = "New Float Configurable", menuName = "Configurables/Configurable/Float Configurable")]
[Serializable]
public class FloatConfigurable : Configurable
{
	// Token: 0x17000013 RID: 19
	// (get) Token: 0x06000150 RID: 336 RVA: 0x00009C65 File Offset: 0x00007E65
	public float MinValue
	{
		get
		{
			return this.minimumValue;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000151 RID: 337 RVA: 0x00009C6D File Offset: 0x00007E6D
	public float MaxValue
	{
		get
		{
			return this.maximumValue;
		}
	}

	// Token: 0x06000152 RID: 338 RVA: 0x00009C75 File Offset: 0x00007E75
	public override LiveData CreateDefaultLiveData()
	{
		return new LiveData(this.key, ConfigurableTypes.Float)
		{
			FloatValue = this.defaultValue
		};
	}

	// Token: 0x06000153 RID: 339 RVA: 0x00009C8F File Offset: 0x00007E8F
	public override void SetNewConfiguredValue(LiveData argument)
	{
		if (argument.FloatValue > this.maximumValue)
		{
			argument.FloatValue = this.maximumValue;
		}
		if (argument.FloatValue < this.minimumValue)
		{
			argument.FloatValue = this.minimumValue;
		}
		base.SetNewConfiguredValue(argument);
	}

	// Token: 0x06000154 RID: 340 RVA: 0x00009CCC File Offset: 0x00007ECC
	public void SetValue(float value)
	{
		this.liveData.FloatValue = value;
		this.SetNewConfiguredValue(this.liveData);
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00009CE6 File Offset: 0x00007EE6
	public float GetNormalizedFloatValue()
	{
		return Mathf.InverseLerp(this.MinValue, this.maximumValue, base.GetFloatValue());
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00009CFF File Offset: 0x00007EFF
	public override string PrintValue()
	{
		return this.liveData.FloatValue.ToString("F0");
	}

	// Token: 0x040001B6 RID: 438
	[SerializeField]
	private float defaultValue;

	// Token: 0x040001B7 RID: 439
	[SerializeField]
	private float minimumValue;

	// Token: 0x040001B8 RID: 440
	[SerializeField]
	private float maximumValue = 1f;
}
