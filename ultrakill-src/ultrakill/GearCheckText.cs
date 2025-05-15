using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000226 RID: 550
public class GearCheckText : MonoBehaviour
{
	// Token: 0x06000BD3 RID: 3027 RVA: 0x00052D78 File Offset: 0x00050F78
	private void OnEnable()
	{
		if (!this.target && !this.target2)
		{
			this.target = base.GetComponent<Text>();
			if (this.target)
			{
				this.originalName = this.target.text;
			}
			else
			{
				this.target2 = base.GetComponent<TMP_Text>();
				if (this.target2)
				{
					this.originalName = this.target2.text;
				}
			}
		}
		if (GameProgressSaver.CheckGear(this.gearName) == 0)
		{
			if (this.target)
			{
				this.target.text = "???";
				return;
			}
			this.target2.text = "???";
			return;
		}
		else
		{
			if (this.target)
			{
				this.target.text = this.originalName;
				return;
			}
			this.target2.text = this.originalName;
			return;
		}
	}

	// Token: 0x04000F6D RID: 3949
	public string gearName;

	// Token: 0x04000F6E RID: 3950
	private Text target;

	// Token: 0x04000F6F RID: 3951
	private TMP_Text target2;

	// Token: 0x04000F70 RID: 3952
	private string originalName;
}
