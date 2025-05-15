using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SoftMasking.Samples
{
	// Token: 0x020001F0 RID: 496
	public class SoftMaskSampleChooser : MonoBehaviour
	{
		// Token: 0x06000B67 RID: 2919 RVA: 0x000347D8 File Offset: 0x000329D8
		public void Start()
		{
			string activeSceneName = SceneManager.GetActiveScene().name;
			int num = this.dropdown.options.FindIndex((Dropdown.OptionData x) => x.text == activeSceneName);
			if (num >= 0)
			{
				this.dropdown.value = num;
				this.dropdown.onValueChanged.AddListener(new UnityAction<int>(this.Choose));
				return;
			}
			this.Fallback(activeSceneName);
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x00034854 File Offset: 0x00032A54
		private void Fallback(string activeSceneName)
		{
			this.dropdown.gameObject.SetActive(false);
			this.fallbackLabel.gameObject.SetActive(true);
			this.fallbackLabel.text = activeSceneName;
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x00034884 File Offset: 0x00032A84
		public void Choose(int sampleIndex)
		{
			SceneManager.LoadScene(this.dropdown.options[sampleIndex].text);
		}

		// Token: 0x04000B22 RID: 2850
		public Dropdown dropdown;

		// Token: 0x04000B23 RID: 2851
		public Text fallbackLabel;
	}
}
