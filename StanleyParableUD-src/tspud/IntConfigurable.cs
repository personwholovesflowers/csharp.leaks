using System;
using UnityEngine;

// Token: 0x02000043 RID: 67
[CreateAssetMenu(fileName = "New Int Configurable", menuName = "Configurables/Configurable/Int Configurable")]
[Serializable]
public class IntConfigurable : Configurable
{
	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000161 RID: 353 RVA: 0x00009EDD File Offset: 0x000080DD
	public int MinValue
	{
		get
		{
			return this.minimumValue;
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000162 RID: 354 RVA: 0x00009EE5 File Offset: 0x000080E5
	public int MaxValue
	{
		get
		{
			return this.maximumValue;
		}
	}

	// Token: 0x06000163 RID: 355 RVA: 0x00009EED File Offset: 0x000080ED
	public override LiveData CreateDefaultLiveData()
	{
		return new LiveData(this.key, ConfigurableTypes.Int)
		{
			IntValue = this.defaultValue
		};
	}

	// Token: 0x06000164 RID: 356 RVA: 0x00009F08 File Offset: 0x00008108
	public void IncreaseValue()
	{
		this.liveData.IntValue++;
		if (!this.clampOnOverflow && this.liveData.IntValue > this.maximumValue)
		{
			this.liveData.IntValue = this.minimumValue;
		}
		this.SetNewConfiguredValue(this.liveData);
	}

	// Token: 0x06000165 RID: 357 RVA: 0x00009F60 File Offset: 0x00008160
	public void DecreaseValue()
	{
		this.liveData.IntValue--;
		if (!this.clampOnOverflow && this.liveData.IntValue < this.minimumValue)
		{
			this.liveData.IntValue = this.maximumValue;
		}
		this.SetNewConfiguredValue(this.liveData);
	}

	// Token: 0x06000166 RID: 358 RVA: 0x00009FB8 File Offset: 0x000081B8
	public void SetValue(int value)
	{
		this.liveData.IntValue = value;
		this.SetNewConfiguredValue(this.liveData);
	}

	// Token: 0x06000167 RID: 359 RVA: 0x00009FD2 File Offset: 0x000081D2
	public void SetNewMaxValue(int newMaxValue)
	{
		this.maximumValue = newMaxValue;
	}

	// Token: 0x06000168 RID: 360 RVA: 0x00009FDB File Offset: 0x000081DB
	public void SetNewMinValue(int newMinValue)
	{
		this.minimumValue = newMinValue;
	}

	// Token: 0x06000169 RID: 361 RVA: 0x00009FE4 File Offset: 0x000081E4
	public override void SetNewConfiguredValue(LiveData argument)
	{
		if (argument.IntValue > this.maximumValue)
		{
			argument.IntValue = this.maximumValue;
		}
		if (argument.IntValue < this.minimumValue)
		{
			argument.IntValue = this.minimumValue;
		}
		base.SetNewConfiguredValue(argument);
	}

	// Token: 0x040001BD RID: 445
	[SerializeField]
	private int defaultValue;

	// Token: 0x040001BE RID: 446
	[SerializeField]
	private int minimumValue;

	// Token: 0x040001BF RID: 447
	[SerializeField]
	private int maximumValue = 1;

	// Token: 0x040001C0 RID: 448
	[SerializeField]
	private bool clampOnOverflow;
}
