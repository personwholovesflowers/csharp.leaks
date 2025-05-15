using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002DF RID: 735
public class MainMenuBG : MonoBehaviour
{
	// Token: 0x06000FFC RID: 4092 RVA: 0x00079E7E File Offset: 0x0007807E
	private void OnEnable()
	{
		this.img = base.GetComponent<RawImage>();
		this.coroutine = base.StartCoroutine(this.Animate());
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x00079E9E File Offset: 0x0007809E
	private void OnDisable()
	{
		if (this.coroutine != null)
		{
			base.StopCoroutine(this.coroutine);
		}
		this.coroutine = null;
	}

	// Token: 0x06000FFE RID: 4094 RVA: 0x00079EBB File Offset: 0x000780BB
	private IEnumerator Animate()
	{
		for (;;)
		{
			this.img.enabled = true;
			this.img.uvRect = new Rect(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), this.img.uvRect.size);
			yield return new WaitForSeconds(0.5f);
			this.img.enabled = false;
			yield return new WaitForSeconds(0.1f);
		}
		yield break;
	}

	// Token: 0x040015CD RID: 5581
	private RawImage img;

	// Token: 0x040015CE RID: 5582
	private Coroutine coroutine;
}
