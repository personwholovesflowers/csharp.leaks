using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SettingsMenu.Models
{
	// Token: 0x0200052B RID: 1323
	[CreateAssetMenu(fileName = "SettingsCategory", menuName = "ULTRAKILL/Settings/Category")]
	public class SettingsCategory : ScriptableObject
	{
		// Token: 0x06001E25 RID: 7717 RVA: 0x000FA5CC File Offset: 0x000F87CC
		public string GetLabel(bool capitalize = true)
		{
			if (string.IsNullOrEmpty(this.title))
			{
				return string.Empty;
			}
			string text = this.title;
			if (capitalize)
			{
				text = text.ToUpper();
			}
			if (string.IsNullOrEmpty(this.titleDecorator))
			{
				return text;
			}
			return string.Concat(new string[] { this.titleDecorator, " ", text, " ", this.titleDecorator });
		}

		// Token: 0x04002A8F RID: 10895
		[FormerlySerializedAs("label")]
		public string title;

		// Token: 0x04002A90 RID: 10896
		public string titleDecorator = "--";

		// Token: 0x04002A91 RID: 10897
		public string description;

		// Token: 0x04002A92 RID: 10898
		public SettingsGroup group;

		// Token: 0x04002A93 RID: 10899
		public List<SettingsItem> items;

		// Token: 0x04002A94 RID: 10900
		[HideInInspector]
		public List<SettingsItem> unusedItems;
	}
}
