using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	// Token: 0x020002D0 RID: 720
	public class ResourceManager : MonoBehaviour
	{
		// Token: 0x17000195 RID: 405
		// (get) Token: 0x0600127C RID: 4732 RVA: 0x00064878 File Offset: 0x00062A78
		public static ResourceManager pInstance
		{
			get
			{
				bool flag = ResourceManager.mInstance == null;
				if (ResourceManager.mInstance == null)
				{
					ResourceManager.mInstance = (ResourceManager)Object.FindObjectOfType(typeof(ResourceManager));
				}
				if (ResourceManager.mInstance == null)
				{
					GameObject gameObject = new GameObject("I2ResourceManager", new Type[] { typeof(ResourceManager) });
					gameObject.hideFlags |= HideFlags.HideAndDontSave;
					ResourceManager.mInstance = gameObject.GetComponent<ResourceManager>();
					SceneManager.sceneLoaded += ResourceManager.MyOnLevelWasLoaded;
				}
				if (flag && Application.isPlaying)
				{
					Object.DontDestroyOnLoad(ResourceManager.mInstance.gameObject);
				}
				return ResourceManager.mInstance;
			}
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x00064929 File Offset: 0x00062B29
		public static void MyOnLevelWasLoaded(Scene scene, LoadSceneMode mode)
		{
			ResourceManager.pInstance.CleanResourceCache();
			LocalizationManager.UpdateSources();
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x0006493C File Offset: 0x00062B3C
		public T GetAsset<T>(string Name) where T : Object
		{
			T t = this.FindAsset(Name) as T;
			if (t != null)
			{
				return t;
			}
			return this.LoadFromResources<T>(Name);
		}

		// Token: 0x0600127F RID: 4735 RVA: 0x00064974 File Offset: 0x00062B74
		private Object FindAsset(string Name)
		{
			if (this.Assets != null)
			{
				int i = 0;
				int num = this.Assets.Length;
				while (i < num)
				{
					if (this.Assets[i] != null && this.Assets[i].name == Name)
					{
						return this.Assets[i];
					}
					i++;
				}
			}
			return null;
		}

		// Token: 0x06001280 RID: 4736 RVA: 0x000649CD File Offset: 0x00062BCD
		public bool HasAsset(Object Obj)
		{
			return this.Assets != null && Array.IndexOf<Object>(this.Assets, Obj) >= 0;
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x000649EC File Offset: 0x00062BEC
		public T LoadFromResources<T>(string Path) where T : Object
		{
			T t;
			try
			{
				Object @object;
				if (string.IsNullOrEmpty(Path))
				{
					t = default(T);
					t = t;
				}
				else if (this.mResourcesCache.TryGetValue(Path, out @object) && @object != null)
				{
					t = @object as T;
				}
				else
				{
					T t2 = default(T);
					if (Path.EndsWith("]", StringComparison.OrdinalIgnoreCase))
					{
						int num = Path.LastIndexOf("[", StringComparison.OrdinalIgnoreCase);
						int num2 = Path.Length - num - 2;
						string text = Path.Substring(num + 1, num2);
						Path = Path.Substring(0, num);
						T[] array = Resources.LoadAll<T>(Path);
						int i = 0;
						int num3 = array.Length;
						while (i < num3)
						{
							if (array[i].name.Equals(text))
							{
								t2 = array[i];
								break;
							}
							i++;
						}
					}
					else
					{
						t2 = Resources.Load(Path, typeof(T)) as T;
					}
					if (t2 == null)
					{
						t2 = this.LoadFromBundle<T>(Path);
					}
					if (t2 != null)
					{
						this.mResourcesCache[Path] = t2;
					}
					t = t2;
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Unable to load {0} '{1}'\nERROR: {2}", new object[]
				{
					typeof(T),
					Path,
					ex.ToString()
				});
				t = default(T);
			}
			return t;
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00064B78 File Offset: 0x00062D78
		public T LoadFromBundle<T>(string path) where T : Object
		{
			int i = 0;
			int count = this.mBundleManagers.Count;
			while (i < count)
			{
				if (this.mBundleManagers[i] != null)
				{
					T t = this.mBundleManagers[i].LoadFromBundle(path, typeof(T)) as T;
					if (t != null)
					{
						return t;
					}
				}
				i++;
			}
			return default(T);
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x00064BEB File Offset: 0x00062DEB
		public void CleanResourceCache()
		{
			this.mResourcesCache.Clear();
			Resources.UnloadUnusedAssets();
			base.CancelInvoke();
		}

		// Token: 0x04000EF5 RID: 3829
		private static ResourceManager mInstance;

		// Token: 0x04000EF6 RID: 3830
		public List<IResourceManager_Bundles> mBundleManagers = new List<IResourceManager_Bundles>();

		// Token: 0x04000EF7 RID: 3831
		public Object[] Assets;

		// Token: 0x04000EF8 RID: 3832
		private readonly Dictionary<string, Object> mResourcesCache = new Dictionary<string, Object>(StringComparer.Ordinal);
	}
}
