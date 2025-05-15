using System;
using System.Collections.Generic;
using GameConsole.CommandTree;
using plog;

namespace GameConsole.Commands
{
	// Token: 0x020005D3 RID: 1491
	public class Prefs : CommandRoot, IConsoleLogger
	{
		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06002163 RID: 8547 RVA: 0x001096A9 File Offset: 0x001078A9
		public Logger Log { get; } = new Logger("Prefs");

		// Token: 0x06002164 RID: 8548 RVA: 0x001096B1 File Offset: 0x001078B1
		public Prefs(Console con)
			: base(con)
		{
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06002165 RID: 8549 RVA: 0x001096CA File Offset: 0x001078CA
		public override string Name
		{
			get
			{
				return "Prefs";
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06002166 RID: 8550 RVA: 0x001096D1 File Offset: 0x001078D1
		public override string Description
		{
			get
			{
				return "Interfaces with the PrefsManager.";
			}
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x001096D8 File Offset: 0x001078D8
		protected override Branch BuildTree(Console con)
		{
			return CommandRoot.Branch("prefs", new Node[]
			{
				CommandRoot.Branch("get", new Node[]
				{
					CommandRoot.Leaf<string>("bool", delegate(string key)
					{
						this.Log.Info(string.Format("{0} = {1}", key, MonoSingleton<PrefsManager>.Instance.GetBool(key, false)), null, null, null);
					}, false),
					CommandRoot.Leaf<string>("int", delegate(string key)
					{
						this.Log.Info(string.Format("{0} = {1}", key, MonoSingleton<PrefsManager>.Instance.GetInt(key, 0)), null, null, null);
					}, false),
					CommandRoot.Leaf<string>("float", delegate(string key)
					{
						this.Log.Info(string.Format("{0} = {1}", key, MonoSingleton<PrefsManager>.Instance.GetFloat(key, 0f)), null, null, null);
					}, false),
					CommandRoot.Leaf<string>("string", delegate(string key)
					{
						this.Log.Info(key + " = " + MonoSingleton<PrefsManager>.Instance.GetString(key, null), null, null, null);
					}, false)
				}),
				CommandRoot.Branch("set", new Node[]
				{
					CommandRoot.Leaf<string, bool>("bool", delegate(string key, bool value)
					{
						this.Log.Info(string.Format("Set {0} to {1}", key, value), null, null, null);
						MonoSingleton<PrefsManager>.Instance.SetBool(key, value);
					}, false),
					CommandRoot.Leaf<string, int>("int", delegate(string key, int value)
					{
						this.Log.Info(string.Format("Set {0} to {1}", key, value), null, null, null);
						MonoSingleton<PrefsManager>.Instance.SetInt(key, value);
					}, false),
					CommandRoot.Leaf<string, float>("float", delegate(string key, float value)
					{
						this.Log.Info(string.Format("Set {0} to {1}", key, value), null, null, null);
						MonoSingleton<PrefsManager>.Instance.SetFloat(key, value);
					}, false),
					CommandRoot.Leaf<string, string>("string", delegate(string key, string value)
					{
						this.Log.Info("Set " + key + " to " + value, null, null, null);
						MonoSingleton<PrefsManager>.Instance.SetString(key, value);
					}, false)
				}),
				CommandRoot.Branch("get_local", new Node[]
				{
					CommandRoot.Leaf<string>("bool", delegate(string key)
					{
						this.Log.Info(string.Format("{0} = {1}", key, MonoSingleton<PrefsManager>.Instance.GetBoolLocal(key, false)), null, null, null);
					}, false),
					CommandRoot.Leaf<string>("int", delegate(string key)
					{
						this.Log.Info(string.Format("{0} = {1}", key, MonoSingleton<PrefsManager>.Instance.GetIntLocal(key, 0)), null, null, null);
					}, false),
					CommandRoot.Leaf<string>("float", delegate(string key)
					{
						this.Log.Info(string.Format("{0} = {1}", key, MonoSingleton<PrefsManager>.Instance.GetFloatLocal(key, 0f)), null, null, null);
					}, false),
					CommandRoot.Leaf<string>("string", delegate(string key)
					{
						this.Log.Info(key + " = " + MonoSingleton<PrefsManager>.Instance.GetStringLocal(key, null), null, null, null);
					}, false)
				}),
				CommandRoot.Branch("set_local", new Node[]
				{
					CommandRoot.Leaf<string, bool>("bool", delegate(string key, bool value)
					{
						this.Log.Info(string.Format("Set {0} to {1}", key, value), null, null, null);
						MonoSingleton<PrefsManager>.Instance.SetBoolLocal(key, value);
					}, false),
					CommandRoot.Leaf<string, int>("int", delegate(string key, int value)
					{
						this.Log.Info(string.Format("Set {0} to {1}", key, value), null, null, null);
						MonoSingleton<PrefsManager>.Instance.SetIntLocal(key, value);
					}, false),
					CommandRoot.Leaf<string, float>("float", delegate(string key, float value)
					{
						this.Log.Info(string.Format("Set {0} to {1}", key, value), null, null, null);
						MonoSingleton<PrefsManager>.Instance.SetFloatLocal(key, value);
					}, false),
					CommandRoot.Leaf<string, string>("string", delegate(string key, string value)
					{
						this.Log.Info("Set " + key + " to " + value, null, null, null);
						MonoSingleton<PrefsManager>.Instance.SetStringLocal(key, value);
					}, false)
				}),
				CommandRoot.Leaf<string>("delete", delegate(string key)
				{
					this.Log.Info("Deleted " + key, null, null, null);
					MonoSingleton<PrefsManager>.Instance.DeleteKey(key);
				}, false),
				CommandRoot.Leaf("list_defaults", delegate
				{
					this.Log.Info("<b>Default Prefs:</b>", null, null, null);
					foreach (KeyValuePair<string, object> keyValuePair in MonoSingleton<PrefsManager>.Instance.defaultValues)
					{
						this.Log.Info(string.Format("{0} = {1}", keyValuePair.Key, keyValuePair.Value), null, null, null);
					}
				}, false),
				CommandRoot.Leaf("list_cached", delegate
				{
					this.Log.Info("<b>Cached Prefs:</b>", null, null, null);
					foreach (KeyValuePair<string, object> keyValuePair2 in MonoSingleton<PrefsManager>.Instance.prefMap)
					{
						this.Log.Info(string.Format("{0} = {1}", keyValuePair2.Key, keyValuePair2.Value), null, null, null);
					}
				}, false),
				CommandRoot.Leaf("list_cached_local", delegate
				{
					this.Log.Info("<b>Local Cached Prefs:</b>", null, null, null);
					foreach (KeyValuePair<string, object> keyValuePair3 in MonoSingleton<PrefsManager>.Instance.localPrefMap)
					{
						this.Log.Info(string.Format("{0} = {1}", keyValuePair3.Key, keyValuePair3.Value), null, null, null);
					}
				}, false),
				CommandRoot.Leaf("last_played", delegate
				{
					this.Log.Info(string.Format("The game has been played {0} months ago last.\nThis is only valid per session.", PrefsManager.monthsSinceLastPlayed), null, null, null);
				}, false)
			});
		}
	}
}
