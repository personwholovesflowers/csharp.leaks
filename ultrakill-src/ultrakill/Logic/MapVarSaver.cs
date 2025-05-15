using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Logic
{
	// Token: 0x0200059A RID: 1434
	public class MapVarSaver
	{
		// Token: 0x1700024A RID: 586
		// (get) Token: 0x0600202A RID: 8234 RVA: 0x00104377 File Offset: 0x00102577
		public static string MapVarDirectory
		{
			get
			{
				return Path.Combine(GameProgressSaver.SavePath, "MapVars");
			}
		}

		// Token: 0x0600202B RID: 8235 RVA: 0x00104388 File Offset: 0x00102588
		public static string AssembleCurrentFilePath()
		{
			if (!Directory.Exists(MapVarSaver.MapVarDirectory))
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (!SceneHelper.IsPlayingCustom)
			{
				stringBuilder.Append("ultrakill_");
				stringBuilder.Append(SceneHelper.CurrentScene);
			}
			else
			{
				MapInfo instance = MapInfo.Instance;
				if (instance == null)
				{
					return null;
				}
				if (instance.uniqueId == null)
				{
					return null;
				}
				if (instance.uniqueId.StartsWith("ultrakill_"))
				{
					return null;
				}
				stringBuilder.Append(instance.uniqueId);
			}
			stringBuilder.Append(".vars.json");
			return Path.Combine(MapVarSaver.MapVarDirectory, stringBuilder.ToString());
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x00104424 File Offset: 0x00102624
		public void WritePersistent(VarStore store)
		{
			List<SavedVariable> list = new List<SavedVariable>();
			using (HashSet<string>.Enumerator enumerator = store.persistentKeys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string key = enumerator.Current;
					if (list.Any((SavedVariable var) => var.name == key))
					{
						return;
					}
					int num;
					SavedValue savedValue;
					bool flag;
					float num2;
					if (store.intStore.TryGetValue(key, out num))
					{
						savedValue = new SavedValue
						{
							type = typeof(int).FullName,
							value = num.ToString()
						};
					}
					else if (store.boolStore.TryGetValue(key, out flag))
					{
						savedValue = new SavedValue
						{
							type = typeof(bool).FullName,
							value = flag
						};
					}
					else if (store.floatStore.TryGetValue(key, out num2))
					{
						savedValue = new SavedValue
						{
							type = typeof(float).FullName,
							value = num2.ToString(CultureInfo.InvariantCulture)
						};
					}
					else
					{
						string text;
						if (!store.stringStore.TryGetValue(key, out text))
						{
							continue;
						}
						savedValue = new SavedValue
						{
							type = typeof(string).FullName,
							value = text
						};
					}
					list.Add(new SavedVariable
					{
						name = key,
						value = savedValue
					});
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			if (!Directory.Exists(MapVarSaver.MapVarDirectory))
			{
				Directory.CreateDirectory(MapVarSaver.MapVarDirectory);
			}
			string text2 = MapVarSaver.AssembleCurrentFilePath();
			if (text2 == null)
			{
				return;
			}
			string text3 = JsonConvert.SerializeObject(new PersistentSavedStore
			{
				variables = list
			});
			File.WriteAllText(text2, text3);
		}

		// Token: 0x04002C97 RID: 11415
		public const string BuiltInPrefix = "ultrakill_";
	}
}
