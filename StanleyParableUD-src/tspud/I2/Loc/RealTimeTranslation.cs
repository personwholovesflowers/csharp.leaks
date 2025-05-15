using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200028B RID: 651
	public class RealTimeTranslation : MonoBehaviour
	{
		// Token: 0x06001056 RID: 4182 RVA: 0x00055CC0 File Offset: 0x00053EC0
		public void OnGUI()
		{
			GUILayout.Label("Translate:", Array.Empty<GUILayoutOption>());
			this.OriginalText = GUILayout.TextArea(this.OriginalText, new GUILayoutOption[] { GUILayout.Width((float)Screen.width) });
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("English -> Español", new GUILayoutOption[] { GUILayout.Height(100f) }))
			{
				this.StartTranslating("en", "es");
			}
			if (GUILayout.Button("Español -> English", new GUILayoutOption[] { GUILayout.Height(100f) }))
			{
				this.StartTranslating("es", "en");
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.TextArea("Multiple Translation with 1 call:\n'This is an example' -> en,zh\n'Hola' -> en", Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Multi Translate", new GUILayoutOption[] { GUILayout.ExpandHeight(true) }))
			{
				this.ExampleMultiTranslations_Async();
			}
			GUILayout.EndHorizontal();
			GUILayout.TextArea(this.TranslatedText, new GUILayoutOption[] { GUILayout.Width((float)Screen.width) });
			GUILayout.Space(10f);
			if (this.IsTranslating)
			{
				GUILayout.Label("Contacting Google....", Array.Empty<GUILayoutOption>());
			}
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00055E03 File Offset: 0x00054003
		public void StartTranslating(string fromCode, string toCode)
		{
			this.IsTranslating = true;
			GoogleTranslation.Translate(this.OriginalText, fromCode, toCode, new GoogleTranslation.fnOnTranslated(this.OnTranslationReady));
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x00055E25 File Offset: 0x00054025
		private void OnTranslationReady(string Translation, string errorMsg)
		{
			this.IsTranslating = false;
			if (errorMsg != null)
			{
				Debug.LogError(errorMsg);
				return;
			}
			this.TranslatedText = Translation;
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00055E40 File Offset: 0x00054040
		public void ExampleMultiTranslations_Blocking()
		{
			Dictionary<string, TranslationQuery> dictionary = new Dictionary<string, TranslationQuery>();
			GoogleTranslation.AddQuery("This is an example", "en", "es", dictionary);
			GoogleTranslation.AddQuery("This is an example", "auto", "zh", dictionary);
			GoogleTranslation.AddQuery("Hola", "es", "en", dictionary);
			if (!GoogleTranslation.ForceTranslate(dictionary, true))
			{
				return;
			}
			Debug.Log(GoogleTranslation.GetQueryResult("This is an example", "en", dictionary));
			Debug.Log(GoogleTranslation.GetQueryResult("This is an example", "zh", dictionary));
			Debug.Log(GoogleTranslation.GetQueryResult("This is an example", "", dictionary));
			Debug.Log(dictionary["Hola"].Results[0]);
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x00055EF4 File Offset: 0x000540F4
		public void ExampleMultiTranslations_Async()
		{
			this.IsTranslating = true;
			Dictionary<string, TranslationQuery> dictionary = new Dictionary<string, TranslationQuery>();
			GoogleTranslation.AddQuery("This is an example", "en", "es", dictionary);
			GoogleTranslation.AddQuery("This is an example", "auto", "zh", dictionary);
			GoogleTranslation.AddQuery("Hola", "es", "en", dictionary);
			GoogleTranslation.Translate(dictionary, new GoogleTranslation.fnOnTranslationReady(this.OnMultitranslationReady), true);
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x00055F60 File Offset: 0x00054160
		private void OnMultitranslationReady(Dictionary<string, TranslationQuery> dict, string errorMsg)
		{
			if (!string.IsNullOrEmpty(errorMsg))
			{
				Debug.LogError(errorMsg);
				return;
			}
			this.IsTranslating = false;
			this.TranslatedText = "";
			this.TranslatedText = this.TranslatedText + GoogleTranslation.GetQueryResult("This is an example", "es", dict) + "\n";
			this.TranslatedText = this.TranslatedText + GoogleTranslation.GetQueryResult("This is an example", "zh", dict) + "\n";
			this.TranslatedText = this.TranslatedText + GoogleTranslation.GetQueryResult("This is an example", "", dict) + "\n";
			this.TranslatedText += dict["Hola"].Results[0];
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x00056023 File Offset: 0x00054223
		public bool IsWaitingForTranslation()
		{
			return this.IsTranslating;
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x0005602B File Offset: 0x0005422B
		public string GetTranslatedText()
		{
			return this.TranslatedText;
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x00056033 File Offset: 0x00054233
		public void SetOriginalText(string text)
		{
			this.OriginalText = text;
		}

		// Token: 0x04000DC8 RID: 3528
		private string OriginalText = "This is an example showing how to use the google translator to translate chat messages within the game.\nIt also supports multiline translations.";

		// Token: 0x04000DC9 RID: 3529
		private string TranslatedText = string.Empty;

		// Token: 0x04000DCA RID: 3530
		private bool IsTranslating;
	}
}
