using System;
using System.Collections.Generic;
using plog;
using UnityEngine;
using UnityEngine.Events;

namespace Logic
{
	// Token: 0x02000598 RID: 1432
	[ConfigureSingleton(SingletonFlags.PersistAutoInstance)]
	public class MapVarManager : MonoSingleton<MapVarManager>
	{
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06002011 RID: 8209 RVA: 0x00103666 File Offset: 0x00101866
		public VarStore Store
		{
			get
			{
				return this.currentStore;
			}
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x0010366E File Offset: 0x0010186E
		private void Start()
		{
			if (Debug.isDebugBuild)
			{
				MapVarManager.LoggingEnabled = true;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06002013 RID: 8211 RVA: 0x0010367D File Offset: 0x0010187D
		public bool HasStashedStore
		{
			get
			{
				return this.stashedStore != null;
			}
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x00103688 File Offset: 0x00101888
		public void ReloadMapVars()
		{
			this.ResetStores();
			this.RestorePersistent();
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x00103698 File Offset: 0x00101898
		public void ResetStores()
		{
			if (MapVarManager.LoggingEnabled)
			{
				MapVarManager.Log.Info("Resetting MapVar stores", null, null, null);
			}
			this.currentStore.Clear();
			this.stashedStore = null;
			this.intSubscribers.Clear();
			this.boolSubscribers.Clear();
			this.floatSubscribers.Clear();
			this.stringSubscribers.Clear();
			MapVarManager.LoggingEnabled = false;
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x00103704 File Offset: 0x00101904
		private void RestorePersistent()
		{
			VarStore varStore = VarStore.LoadPersistentStore();
			if (varStore == null)
			{
				return;
			}
			this.currentStore = varStore;
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x00103724 File Offset: 0x00101924
		public void StashStore()
		{
			if (this.currentStore.intStore.Count == 0 && this.currentStore.boolStore.Count == 0 && this.currentStore.floatStore.Count == 0 && this.currentStore.stringStore.Count == 0)
			{
				this.stashedStore = null;
				return;
			}
			this.stashedStore = this.currentStore.DuplicateStore();
			if (MapVarManager.LoggingEnabled)
			{
				MapVarManager.Log.Info("Stashed MapVar stores", null, null, null);
			}
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x001037AC File Offset: 0x001019AC
		public void RestoreStashedStore()
		{
			if (this.stashedStore == null)
			{
				if (MapVarManager.LoggingEnabled)
				{
					MapVarManager.Log.Info("No stashed store to restore", null, null, null);
				}
				return;
			}
			this.currentStore = this.stashedStore.DuplicateStore();
			if (MapVarManager.LoggingEnabled)
			{
				MapVarManager.Log.Info("Restored MapVar stores", null, null, null);
			}
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x00103808 File Offset: 0x00101A08
		public void RegisterIntWatcher(string key, UnityAction<int> callback)
		{
			if (MapVarManager.LoggingEnabled)
			{
				MapVarManager.Log.Info("Registering int watcher for " + key, null, null, null);
			}
			if (!this.intSubscribers.ContainsKey(key))
			{
				this.intSubscribers.Add(key, new List<UnityAction<int>>());
			}
			this.intSubscribers[key].Add(callback);
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x00103868 File Offset: 0x00101A68
		public void RegisterBoolWatcher(string key, UnityAction<bool> callback)
		{
			if (MapVarManager.LoggingEnabled)
			{
				MapVarManager.Log.Info("Registering bool watcher for " + key, null, null, null);
			}
			if (!this.boolSubscribers.ContainsKey(key))
			{
				this.boolSubscribers.Add(key, new List<UnityAction<bool>>());
			}
			this.boolSubscribers[key].Add(callback);
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x001038C8 File Offset: 0x00101AC8
		public void RegisterFloatWatcher(string key, UnityAction<float> callback)
		{
			if (MapVarManager.LoggingEnabled)
			{
				MapVarManager.Log.Info("Registering float watcher for " + key, null, null, null);
			}
			if (!this.floatSubscribers.ContainsKey(key))
			{
				this.floatSubscribers.Add(key, new List<UnityAction<float>>());
			}
			this.floatSubscribers[key].Add(callback);
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x00103928 File Offset: 0x00101B28
		public void RegisterStringWatcher(string key, UnityAction<string> callback)
		{
			if (MapVarManager.LoggingEnabled)
			{
				MapVarManager.Log.Info("Registering string watcher for " + key, null, null, null);
			}
			if (!this.stringSubscribers.ContainsKey(key))
			{
				this.stringSubscribers.Add(key, new List<UnityAction<string>>());
			}
			this.stringSubscribers[key].Add(callback);
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x00103985 File Offset: 0x00101B85
		public void RegisterGlobalWatcher(UnityAction<string, object> callback)
		{
			if (MapVarManager.LoggingEnabled)
			{
				MapVarManager.Log.Info("Registering global watcher", null, null, null);
			}
			this.globalSubscribers.Add(callback);
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x001039AC File Offset: 0x00101BAC
		public void SetInt(string key, int value, bool persistent = false)
		{
			this.currentStore.intStore[key] = value;
			if (this.intSubscribers.ContainsKey(key))
			{
				if (MapVarManager.LoggingEnabled)
				{
					MapVarManager.Log.Info(string.Format("Notifying {0} int watcher{1} for {2}", this.intSubscribers[key].Count, (this.intSubscribers[key].Count == 1) ? "" : "s", key), null, null, null);
				}
				foreach (UnityAction<int> unityAction in this.intSubscribers[key])
				{
					if (unityAction != null)
					{
						unityAction(value);
					}
				}
			}
			if (this.globalSubscribers.Count > 0)
			{
				if (MapVarManager.LoggingEnabled)
				{
					MapVarManager.Log.Info(string.Format("Notifying {0} global watcher{1} for {2}", this.globalSubscribers.Count, (this.globalSubscribers.Count == 1) ? "" : "s", key), null, null, null);
				}
				foreach (UnityAction<string, object> unityAction2 in this.globalSubscribers)
				{
					if (unityAction2 != null)
					{
						unityAction2(key, value);
					}
				}
			}
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x00103B28 File Offset: 0x00101D28
		public void AddInt(string key, int value, bool persistent = false)
		{
			int valueOrDefault = this.GetInt(key).GetValueOrDefault();
			this.SetInt(key, valueOrDefault + value, persistent);
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x00103B50 File Offset: 0x00101D50
		public void SetBool(string key, bool value, bool persistent = false)
		{
			if (MapVarManager.LoggingEnabled)
			{
				MapVarManager.Log.Info(string.Format("SetBool: {0} - {1}", key, value), null, null, null);
			}
			this.currentStore.boolStore[key] = value;
			if (this.boolSubscribers.ContainsKey(key))
			{
				if (MapVarManager.LoggingEnabled)
				{
					MapVarManager.Log.Info(string.Format("Notifying {0} bool watchers for {1}", this.boolSubscribers[key].Count, key), null, null, null);
				}
				foreach (UnityAction<bool> unityAction in this.boolSubscribers[key])
				{
					if (unityAction != null)
					{
						unityAction(value);
					}
				}
			}
			if (this.globalSubscribers.Count > 0)
			{
				if (MapVarManager.LoggingEnabled)
				{
					MapVarManager.Log.Info(string.Format("Notifying {0} global watchers for {1}", this.globalSubscribers.Count, key), null, null, null);
				}
				foreach (UnityAction<string, object> unityAction2 in this.globalSubscribers)
				{
					if (unityAction2 != null)
					{
						unityAction2(key, value);
					}
				}
			}
			if (persistent)
			{
				this.currentStore.persistentKeys.Add(key);
				this.saver.WritePersistent(this.currentStore);
				return;
			}
			this.currentStore.persistentKeys.Remove(key);
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x00103CEC File Offset: 0x00101EEC
		public void SetFloat(string key, float value, bool persistent = false)
		{
			if (MapVarManager.LoggingEnabled)
			{
				MapVarManager.Log.Info(string.Format("SetFloat: {0} - {1}", key, value), null, null, null);
			}
			this.currentStore.floatStore[key] = value;
			if (this.floatSubscribers.ContainsKey(key))
			{
				if (MapVarManager.LoggingEnabled)
				{
					MapVarManager.Log.Info(string.Format("Notifying {0} float watchers for {1}", this.floatSubscribers[key].Count, key), null, null, null);
				}
				foreach (UnityAction<float> unityAction in this.floatSubscribers[key])
				{
					if (unityAction != null)
					{
						unityAction(value);
					}
				}
			}
			if (this.globalSubscribers.Count > 0)
			{
				if (MapVarManager.LoggingEnabled)
				{
					MapVarManager.Log.Info(string.Format("Notifying {0} global watchers for {1}", this.globalSubscribers.Count, key), null, null, null);
				}
				foreach (UnityAction<string, object> unityAction2 in this.globalSubscribers)
				{
					if (unityAction2 != null)
					{
						unityAction2(key, value);
					}
				}
			}
			if (persistent)
			{
				this.currentStore.persistentKeys.Add(key);
				this.saver.WritePersistent(this.currentStore);
				return;
			}
			this.currentStore.persistentKeys.Remove(key);
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x00103E88 File Offset: 0x00102088
		public void SetString(string key, string value, bool persistent = false)
		{
			if (MapVarManager.LoggingEnabled)
			{
				MapVarManager.Log.Info("SetString: " + key + " - " + value, null, null, null);
			}
			this.currentStore.stringStore[key] = value;
			if (this.stringSubscribers.ContainsKey(key))
			{
				if (MapVarManager.LoggingEnabled)
				{
					MapVarManager.Log.Info(string.Format("Notifying {0} string watchers for {1}", this.stringSubscribers[key].Count, key), null, null, null);
				}
				foreach (UnityAction<string> unityAction in this.stringSubscribers[key])
				{
					if (unityAction != null)
					{
						unityAction(value);
					}
				}
			}
			if (this.globalSubscribers.Count > 0)
			{
				if (MapVarManager.LoggingEnabled)
				{
					MapVarManager.Log.Info(string.Format("Notifying {0} global watchers for {1}", this.globalSubscribers.Count, key), null, null, null);
				}
				foreach (UnityAction<string, object> unityAction2 in this.globalSubscribers)
				{
					if (unityAction2 != null)
					{
						unityAction2(key, value);
					}
				}
			}
			if (persistent)
			{
				this.currentStore.persistentKeys.Add(key);
				this.saver.WritePersistent(this.currentStore);
				return;
			}
			this.currentStore.persistentKeys.Remove(key);
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x0010401C File Offset: 0x0010221C
		public int? GetInt(string key)
		{
			int num;
			if (this.currentStore.intStore.TryGetValue(key, out num))
			{
				return new int?(num);
			}
			return null;
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x00104050 File Offset: 0x00102250
		public bool? GetBool(string key)
		{
			bool flag;
			if (this.currentStore.boolStore.TryGetValue(key, out flag))
			{
				return new bool?(flag);
			}
			return null;
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x00104084 File Offset: 0x00102284
		public float? GetFloat(string key)
		{
			float num;
			if (this.currentStore.floatStore.TryGetValue(key, out num))
			{
				return new float?(num);
			}
			return null;
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x001040B8 File Offset: 0x001022B8
		public string GetString(string key)
		{
			string text;
			if (this.currentStore.stringStore.TryGetValue(key, out text))
			{
				return text;
			}
			return null;
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x001040E0 File Offset: 0x001022E0
		public List<VariableSnapshot> GetAllVariables()
		{
			List<VariableSnapshot> list = new List<VariableSnapshot>();
			foreach (KeyValuePair<string, int> keyValuePair in this.currentStore.intStore)
			{
				list.Add(new VariableSnapshot
				{
					type = typeof(int),
					name = keyValuePair.Key,
					value = keyValuePair.Value
				});
			}
			foreach (KeyValuePair<string, bool> keyValuePair2 in this.currentStore.boolStore)
			{
				list.Add(new VariableSnapshot
				{
					type = typeof(bool),
					name = keyValuePair2.Key,
					value = keyValuePair2.Value
				});
			}
			foreach (KeyValuePair<string, float> keyValuePair3 in this.currentStore.floatStore)
			{
				list.Add(new VariableSnapshot
				{
					type = typeof(float),
					name = keyValuePair3.Key,
					value = keyValuePair3.Value
				});
			}
			foreach (KeyValuePair<string, string> keyValuePair4 in this.currentStore.stringStore)
			{
				list.Add(new VariableSnapshot
				{
					type = typeof(string),
					name = keyValuePair4.Key,
					value = keyValuePair4.Value
				});
			}
			return list;
		}

		// Token: 0x04002C8A RID: 11402
		private static readonly global::plog.Logger Log = new global::plog.Logger("MapVarManager");

		// Token: 0x04002C8B RID: 11403
		private VarStore currentStore = new VarStore();

		// Token: 0x04002C8C RID: 11404
		private VarStore stashedStore;

		// Token: 0x04002C8D RID: 11405
		private MapVarSaver saver = new MapVarSaver();

		// Token: 0x04002C8E RID: 11406
		private readonly Dictionary<string, List<UnityAction<int>>> intSubscribers = new Dictionary<string, List<UnityAction<int>>>();

		// Token: 0x04002C8F RID: 11407
		private readonly Dictionary<string, List<UnityAction<bool>>> boolSubscribers = new Dictionary<string, List<UnityAction<bool>>>();

		// Token: 0x04002C90 RID: 11408
		private readonly Dictionary<string, List<UnityAction<float>>> floatSubscribers = new Dictionary<string, List<UnityAction<float>>>();

		// Token: 0x04002C91 RID: 11409
		private readonly Dictionary<string, List<UnityAction<string>>> stringSubscribers = new Dictionary<string, List<UnityAction<string>>>();

		// Token: 0x04002C92 RID: 11410
		private readonly List<UnityAction<string, object>> globalSubscribers = new List<UnityAction<string, object>>();

		// Token: 0x04002C93 RID: 11411
		public static bool LoggingEnabled = false;
	}
}
