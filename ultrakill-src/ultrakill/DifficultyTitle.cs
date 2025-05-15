using System;
using TMPro;
using UnityEngine;

// Token: 0x02000110 RID: 272
public class DifficultyTitle : MonoBehaviour
{
	// Token: 0x06000516 RID: 1302 RVA: 0x000222B7 File Offset: 0x000204B7
	private void Start()
	{
		this.Check();
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x000222B7 File Offset: 0x000204B7
	private void OnEnable()
	{
		this.Check();
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x000222C0 File Offset: 0x000204C0
	private void Check()
	{
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		string text = "";
		if (this.lines)
		{
			text += "-- ";
		}
		switch (@int)
		{
		case 0:
			text += "HARMLESS";
			break;
		case 1:
			text += "LENIENT";
			break;
		case 2:
			text += "STANDARD";
			break;
		case 3:
			text += "VIOLENT";
			break;
		case 4:
			text += "BRUTAL";
			break;
		case 5:
			text += "ULTRAKILL MUST DIE";
			break;
		}
		if (this.lines)
		{
			text += " --";
		}
		if (!this.txt2)
		{
			this.txt2 = base.GetComponent<TMP_Text>();
		}
		if (this.txt2)
		{
			this.txt2.text = text;
			return;
		}
	}

	// Token: 0x04000703 RID: 1795
	public bool lines;

	// Token: 0x04000704 RID: 1796
	private TMP_Text txt2;
}
