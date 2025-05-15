using System;
using TMPro;
using UnityEngine;

// Token: 0x02000184 RID: 388
public class SetBuildNumberText : MonoBehaviour
{
	// Token: 0x06000913 RID: 2323 RVA: 0x0002B1E1 File Offset: 0x000293E1
	private void Start()
	{
		this.UpdateText();
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x0002B1E9 File Offset: 0x000293E9
	private void UpdateText()
	{
		if (this.text != null)
		{
			this.text.text = Application.version ?? "";
		}
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x0002B1E1 File Offset: 0x000293E1
	private void OnValidate()
	{
		this.UpdateText();
	}

	// Token: 0x040008ED RID: 2285
	[InspectorButton("UpdateText", null)]
	[SerializeField]
	private TMP_Text text;
}
