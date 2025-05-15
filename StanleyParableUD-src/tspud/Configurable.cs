using System;
using UnityEngine;

// Token: 0x02000032 RID: 50
public class Configurable : ScriptableObject
{
	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000103 RID: 259 RVA: 0x00008B4D File Offset: 0x00006D4D
	public string Key
	{
		get
		{
			return this.key;
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000104 RID: 260 RVA: 0x00008B55 File Offset: 0x00006D55
	public bool DeviatesFromSavedValue
	{
		get
		{
			return this.deviatesFromSavedValue;
		}
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00008B60 File Offset: 0x00006D60
	public void Init()
	{
		if (this.initialized)
		{
			return;
		}
		if (Configurable.ConfigurableDataContainer == null)
		{
			Configurable.ConfigurableDataContainer = ConfigurableDataContainer.LoadContainer("data");
		}
		this.liveData = this.LoadOrCreateSaveData();
		this.UpdateSaveDataCache();
		ConfigurableDataContainer.OnSaveValues = (Action)Delegate.Combine(ConfigurableDataContainer.OnSaveValues, new Action(this.OnSaveValues));
		ConfigurableDataContainer.OnResetValues = (Action)Delegate.Combine(ConfigurableDataContainer.OnResetValues, new Action(this.OnResetValues));
		this.initialized = true;
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00008BE5 File Offset: 0x00006DE5
	public void ForceUpdate()
	{
		if (this.OnValueChanged != null)
		{
			this.OnValueChanged(this.liveData);
		}
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00008C00 File Offset: 0x00006E00
	private void OnDestroy()
	{
		ConfigurableDataContainer.OnSaveValues = (Action)Delegate.Remove(ConfigurableDataContainer.OnSaveValues, new Action(this.OnSaveValues));
		ConfigurableDataContainer.OnResetValues = (Action)Delegate.Remove(ConfigurableDataContainer.OnResetValues, new Action(this.OnResetValues));
	}

	// Token: 0x06000108 RID: 264 RVA: 0x00008C50 File Offset: 0x00006E50
	private void UpdateSaveDataCache()
	{
		if (this.persistent && this.saveDataCache == null)
		{
			this.saveDataCache = new LiveData(this.liveData.key, this.liveData.configureableType);
		}
		if (this.saveDataCache != null)
		{
			this.saveDataCache.CopyValuesFrom(this.liveData);
		}
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00008CA8 File Offset: 0x00006EA8
	public virtual void SetNewConfiguredValue(LiveData data)
	{
		if (!this.initialized)
		{
			this.Init();
		}
		if (this.saveDataCache == null)
		{
			this.deviatesFromSavedValue = true;
			switch (data.configureableType)
			{
			case ConfigurableTypes.Int:
				this.liveData.IntValue = data.IntValue;
				break;
			case ConfigurableTypes.Float:
				this.liveData.FloatValue = data.FloatValue;
				break;
			case ConfigurableTypes.Boolean:
				this.liveData.BooleanValue = data.BooleanValue;
				break;
			case ConfigurableTypes.String:
				this.liveData.StringValue = data.StringValue;
				break;
			}
			if (this.OnValueChanged != null)
			{
				this.OnValueChanged(this.liveData);
			}
			return;
		}
		switch (data.configureableType)
		{
		case ConfigurableTypes.Int:
			this.deviatesFromSavedValue = this.saveDataCache.IntValue != data.IntValue;
			this.liveData.IntValue = data.IntValue;
			break;
		case ConfigurableTypes.Float:
			this.deviatesFromSavedValue = this.saveDataCache.FloatValue != data.FloatValue;
			this.liveData.FloatValue = data.FloatValue;
			break;
		case ConfigurableTypes.Boolean:
			this.deviatesFromSavedValue = this.saveDataCache.BooleanValue != data.BooleanValue;
			this.liveData.BooleanValue = data.BooleanValue;
			break;
		case ConfigurableTypes.String:
			this.deviatesFromSavedValue = !this.saveDataCache.StringValue.Equals(data.StringValue);
			this.liveData.StringValue = data.StringValue;
			break;
		}
		if (this.OnValueChanged != null)
		{
			this.OnValueChanged(this.liveData);
		}
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00008E54 File Offset: 0x00007054
	public virtual LiveData LoadOrCreateSaveData()
	{
		LiveData savedDataFromContainer = Configurable.ConfigurableDataContainer.GetSavedDataFromContainer(this.key);
		if (this.persistent && savedDataFromContainer != null && this.IsValidSaveData(savedDataFromContainer))
		{
			return savedDataFromContainer;
		}
		return this.CreateDefaultLiveData();
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00008E90 File Offset: 0x00007090
	public virtual bool IsValidSaveData(LiveData data)
	{
		bool flag;
		switch (data.configureableType)
		{
		case ConfigurableTypes.Int:
			flag = this is IntConfigurable;
			break;
		case ConfigurableTypes.Float:
			flag = this is FloatConfigurable;
			break;
		case ConfigurableTypes.Boolean:
			flag = this is BooleanConfigurable;
			break;
		case ConfigurableTypes.String:
			flag = this is StringConfigurable;
			break;
		default:
			flag = false;
			break;
		}
		return flag;
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00008EF4 File Offset: 0x000070F4
	public virtual LiveData LoadOrCreateSaveData(out bool saveDataExists)
	{
		LiveData savedDataFromContainer = Configurable.ConfigurableDataContainer.GetSavedDataFromContainer(this.key);
		if (this.persistent && savedDataFromContainer != null && this.IsValidSaveData(savedDataFromContainer))
		{
			saveDataExists = true;
			return savedDataFromContainer;
		}
		saveDataExists = false;
		return this.CreateDefaultLiveData();
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00008F34 File Offset: 0x00007134
	private void OnResetValues()
	{
		if (this.deviatesFromSavedValue)
		{
			this.liveData.CopyValuesFrom(this.saveDataCache);
			if (this.OnValueChanged != null)
			{
				this.OnValueChanged(this.liveData);
			}
			this.deviatesFromSavedValue = false;
		}
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00008F70 File Offset: 0x00007170
	private void OnSaveValues()
	{
		if (this.deviatesFromSavedValue)
		{
			if (this.persistent)
			{
				Configurable.ConfigurableDataContainer.UpdateSaveDataCache(this.liveData);
			}
			this.deviatesFromSavedValue = false;
			if (this.persistent)
			{
				this.UpdateSaveDataCache();
			}
			if (this.OnValueChanged != null)
			{
				this.OnValueChanged(this.liveData);
			}
			if (this.persistent)
			{
				Configurable.Dirty = true;
			}
		}
	}

	// Token: 0x0600010F RID: 271 RVA: 0x00008FD9 File Offset: 0x000071D9
	public virtual LiveData CreateDefaultLiveData()
	{
		return null;
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00008FDC File Offset: 0x000071DC
	public virtual void SaveToDiskAll()
	{
		if (Configurable.ConfigurableDataContainer == null)
		{
			Configurable.ConfigurableDataContainer = ConfigurableDataContainer.LoadContainer("data");
		}
		Configurable.ConfigurableDataContainer.SaveToPlatformPrefs("data", false);
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00009004 File Offset: 0x00007204
	public virtual string PrintValue()
	{
		switch (this.liveData.configureableType)
		{
		case ConfigurableTypes.Int:
			return this.liveData.IntValue.ToString();
		case ConfigurableTypes.Float:
			return this.liveData.FloatValue.ToString();
		case ConfigurableTypes.Boolean:
			return this.liveData.BooleanValue.ToString();
		case ConfigurableTypes.String:
			return this.liveData.StringValue;
		default:
			return string.Empty;
		}
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00009079 File Offset: 0x00007279
	public bool GetBooleanValue()
	{
		if (!this.initialized)
		{
			this.Init();
		}
		return this.liveData.BooleanValue;
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00009094 File Offset: 0x00007294
	public int GetIntValue()
	{
		if (!this.initialized)
		{
			this.Init();
		}
		return this.liveData.IntValue;
	}

	// Token: 0x06000114 RID: 276 RVA: 0x000090AF File Offset: 0x000072AF
	public float GetFloatValue()
	{
		if (!this.initialized)
		{
			this.Init();
		}
		return this.liveData.FloatValue;
	}

	// Token: 0x06000115 RID: 277 RVA: 0x000090CA File Offset: 0x000072CA
	public string GetStringValue()
	{
		if (!this.initialized)
		{
			this.Init();
		}
		return this.liveData.StringValue;
	}

	// Token: 0x06000116 RID: 278 RVA: 0x000090E5 File Offset: 0x000072E5
	public ConfigurableTypes GetConfigurableType()
	{
		if (!this.initialized)
		{
			this.Init();
		}
		return this.liveData.configureableType;
	}

	// Token: 0x04000177 RID: 375
	public static ConfigurableDataContainer ConfigurableDataContainer;

	// Token: 0x04000178 RID: 376
	public static bool Dirty;

	// Token: 0x04000179 RID: 377
	public Action<LiveData> OnValueChanged;

	// Token: 0x0400017A RID: 378
	[NonSerialized]
	private bool initialized;

	// Token: 0x0400017B RID: 379
	[NonSerialized]
	private bool deviatesFromSavedValue;

	// Token: 0x0400017C RID: 380
	[Header("Configuration")]
	[SerializeField]
	protected string key = "ConfigurableKey";

	// Token: 0x0400017D RID: 381
	[SerializeField]
	private string description = "Say something about this configurable";

	// Token: 0x0400017E RID: 382
	[SerializeField]
	private bool persistent = true;

	// Token: 0x0400017F RID: 383
	[Space]
	[Header("Data")]
	[NonSerialized]
	private LiveData saveDataCache;

	// Token: 0x04000180 RID: 384
	[SerializeField]
	[HideInInspector]
	protected LiveData liveData;

	// Token: 0x04000181 RID: 385
	private bool triedToInitBeforePlayerPrefs;
}
