using System;
using SettingsMenu.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SettingsMenu.Components
{
	// Token: 0x02000544 RID: 1348
	[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
	public class SettingsMenu : MonoSingleton<SettingsMenu>
	{
		// Token: 0x06001E67 RID: 7783 RVA: 0x000FB529 File Offset: 0x000F9729
		private void Start()
		{
			this.Initialize();
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x000FB534 File Offset: 0x000F9734
		public void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			this.pageGameObjects = new GameObject[this.pageContainer.childCount];
			for (int i = 0; i < this.pageContainer.childCount; i++)
			{
				this.pageGameObjects[i] = this.pageContainer.GetChild(i).gameObject;
			}
			this.pageBuilders = this.pageContainer.GetComponentsInChildren<SettingsPageBuilder>(true);
			this.settingsLogic = this.pageContainer.GetComponentsInChildren<SettingsLogicBase>(true);
			SettingsLogicBase[] array = this.settingsLogic;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].Initialize(this);
			}
			this.initialized = true;
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x000FB5D8 File Offset: 0x000F97D8
		public void OnPrefChanged(string key, object value)
		{
			SettingsLogicBase[] array = this.settingsLogic;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnPrefChanged(key, value);
			}
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x000FB604 File Offset: 0x000F9804
		public void SetActivePage(GameObject targetPage)
		{
			foreach (GameObject gameObject in this.pageGameObjects)
			{
				gameObject.gameObject.SetActive(gameObject == targetPage);
			}
			SettingsPageBuilder settingsPageBuilder;
			if (targetPage.TryGetComponent<SettingsPageBuilder>(out settingsPageBuilder))
			{
				settingsPageBuilder.SetSelected();
			}
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x000FB64C File Offset: 0x000F984C
		public static void SetSelected(Selectable selectable)
		{
			SettingsMenu.selectedSomethingThisFrame = true;
			EventSystem.current.SetSelectedGameObject(selectable.gameObject);
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x000FB664 File Offset: 0x000F9864
		private void LateUpdate()
		{
			SettingsMenu.selectedSomethingThisFrame = false;
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x000FB66C File Offset: 0x000F986C
		public bool TryGetItemBuilderInstance<T>(SettingsItem item, out T builder) where T : SettingsBuilderBase
		{
			builder = default(T);
			SettingsPageBuilder[] array = this.pageBuilders;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].TryGetItemBuilderInstance<T>(item, out builder))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04002AEF RID: 10991
		public static bool selectedSomethingThisFrame;

		// Token: 0x04002AF0 RID: 10992
		[SerializeField]
		private Transform navigationRail;

		// Token: 0x04002AF1 RID: 10993
		[SerializeField]
		private Transform pageContainer;

		// Token: 0x04002AF2 RID: 10994
		private GameObject[] pageGameObjects;

		// Token: 0x04002AF3 RID: 10995
		private SettingsPageBuilder[] pageBuilders;

		// Token: 0x04002AF4 RID: 10996
		private SettingsLogicBase[] settingsLogic;

		// Token: 0x04002AF5 RID: 10997
		private bool initialized;
	}
}
