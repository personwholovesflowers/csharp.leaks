using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

// Token: 0x02000030 RID: 48
public class Magenta
{
	// Token: 0x0600011B RID: 283 RVA: 0x00006574 File Offset: 0x00004774
	public Magenta(string path, bool loadUnhardened = true)
	{
		this.path = path;
		if (!Directory.Exists(path))
		{
			throw new DirectoryNotFoundException();
		}
		if (!File.Exists(Path.Combine(path, "magenta.json")))
		{
			throw new Exception("magenta.json not found");
		}
		Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(Path.Combine(path, "magenta.json")));
		this.version = dictionary["version"];
		string text = File.ReadAllText(Path.Combine(path, "bundles.json"));
		this.bundles = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(text);
		this.assetsToBundles = new Dictionary<string, string>();
		foreach (KeyValuePair<string, List<string>> keyValuePair in this.bundles)
		{
			foreach (string text2 in keyValuePair.Value)
			{
				if (!this.assetsToBundles.ContainsKey(text2))
				{
					this.assetsToBundles.Add(text2, keyValuePair.Key);
				}
			}
		}
		if (loadUnhardened)
		{
			Dictionary<string, List<string>> unhardenedBundles = this.GetUnhardenedBundles();
			if (unhardenedBundles != null)
			{
				foreach (KeyValuePair<string, List<string>> keyValuePair2 in unhardenedBundles)
				{
					if (!this.bundles.ContainsKey(keyValuePair2.Key))
					{
						this.bundles.Add(keyValuePair2.Key, keyValuePair2.Value);
						foreach (string text3 in keyValuePair2.Value.Where((string asset) => !this.assetsToBundles.ContainsKey(asset)))
						{
							this.assetsToBundles.Add(text3, keyValuePair2.Key);
						}
					}
				}
			}
		}
		string text4 = Path.Combine(path, "sceneDependencies.json");
		if (File.Exists(text4))
		{
			string text5 = File.ReadAllText(text4);
			this.sceneDependencies = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(text5);
		}
		string text6 = Path.Combine(path, "agonyDependencies.json");
		if (File.Exists(text6))
		{
			string text7 = File.ReadAllText(text6);
			this.agonyDependencies = JsonConvert.DeserializeObject<List<string>>(text7);
		}
	}

	// Token: 0x0600011C RID: 284 RVA: 0x000067E0 File Offset: 0x000049E0
	public Dictionary<string, List<string>> GetUnhardenedBundles()
	{
		string text = Path.Combine(this.path, "unhardenedBundles.json");
		if (!File.Exists(text))
		{
			return null;
		}
		return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(text));
	}

	// Token: 0x040000DE RID: 222
	public string path;

	// Token: 0x040000DF RID: 223
	public string version;

	// Token: 0x040000E0 RID: 224
	public readonly Dictionary<string, List<string>> bundles;

	// Token: 0x040000E1 RID: 225
	public readonly Dictionary<string, string> assetsToBundles;

	// Token: 0x040000E2 RID: 226
	public readonly Dictionary<string, List<string>> sceneDependencies;

	// Token: 0x040000E3 RID: 227
	public readonly List<string> agonyDependencies;
}
