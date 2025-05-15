using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200037D RID: 893
public class RandomTextString : MonoBehaviour
{
	// Token: 0x060014AD RID: 5293 RVA: 0x000A7244 File Offset: 0x000A5444
	private void Start()
	{
		Text component = base.GetComponent<Text>();
		if (!component)
		{
			return;
		}
		component.text = this.strings[Random.Range(0, this.strings.Length)];
	}

	// Token: 0x04001C7B RID: 7291
	[SerializeField]
	private string[] strings;
}
