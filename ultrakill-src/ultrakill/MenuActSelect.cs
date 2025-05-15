using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002EE RID: 750
public class MenuActSelect : MonoBehaviour
{
	// Token: 0x06001088 RID: 4232 RVA: 0x0007ECD8 File Offset: 0x0007CED8
	private void OnEnable()
	{
		int @int = MonoSingleton<PrefsManager>.Instance.GetInt("difficulty", 0);
		if (this.text == null)
		{
			this.text = base.transform.GetChild(0).GetComponent<TMP_Text>();
			this.originalName = this.text.text;
		}
		bool flag = false;
		if (this.primeLevels)
		{
			for (int i = 1; i < 4; i++)
			{
				if (GameProgressSaver.GetPrime(@int, i) > 0)
				{
					Debug.Log("Found Primes");
					flag = true;
					break;
				}
			}
		}
		if (this.forceOff || (GameProgressSaver.GetProgress(@int) <= this.requiredLevels && !flag))
		{
			base.GetComponent<Button>().interactable = false;
			this.text.color = new Color(0.3f, 0.3f, 0.3f);
			if (this.nameWhenDisabled != "")
			{
				this.text.text = this.nameWhenDisabled;
			}
			base.transform.GetChild(1).gameObject.SetActive(false);
			if (this.hideWhenOff)
			{
				base.gameObject.SetActive(false);
				return;
			}
		}
		else
		{
			base.GetComponent<Button>().interactable = true;
			this.text.color = new Color(1f, 1f, 1f);
			this.text.text = this.originalName;
			base.transform.GetChild(1).gameObject.SetActive(true);
		}
	}

	// Token: 0x04001675 RID: 5749
	public int requiredLevels;

	// Token: 0x04001676 RID: 5750
	public bool forceOff;

	// Token: 0x04001677 RID: 5751
	public bool hideWhenOff;

	// Token: 0x04001678 RID: 5752
	public bool primeLevels;

	// Token: 0x04001679 RID: 5753
	private Transform[] children;

	// Token: 0x0400167A RID: 5754
	private Image img;

	// Token: 0x0400167B RID: 5755
	private TMP_Text text;

	// Token: 0x0400167C RID: 5756
	private string originalName;

	// Token: 0x0400167D RID: 5757
	public string nameWhenDisabled;
}
