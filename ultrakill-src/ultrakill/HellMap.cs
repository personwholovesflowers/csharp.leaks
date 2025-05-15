using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000248 RID: 584
public class HellMap : MonoBehaviour
{
	// Token: 0x06000CD5 RID: 3285 RVA: 0x0005F950 File Offset: 0x0005DB50
	private void Start()
	{
		if (MonoSingleton<StatsManager>.Instance.levelNumber < this.firstLevelNumber)
		{
			base.gameObject.SetActive(false);
			return;
		}
		Transform child = base.transform.GetChild(0).GetChild(MonoSingleton<StatsManager>.Instance.levelNumber - this.firstLevelNumber);
		this.targetPos = new Vector3(this.arrow.transform.localPosition.x, child.transform.localPosition.y - 25f, this.arrow.transform.localPosition.z);
		this.arrow.transform.localPosition = this.targetPos + Vector3.up * 50f;
		this.targetImage = this.arrow.GetComponentInChildren<Image>();
		this.aud = base.GetComponent<AudioSource>();
		base.Invoke("FlashImage", 0.075f);
	}

	// Token: 0x06000CD6 RID: 3286 RVA: 0x0005FA44 File Offset: 0x0005DC44
	private void Update()
	{
		this.arrow.transform.localPosition = Vector3.MoveTowards(this.arrow.transform.localPosition, this.targetPos, Time.deltaTime * 4f * Vector3.Distance(this.arrow.transform.localPosition, this.targetPos));
	}

	// Token: 0x06000CD7 RID: 3287 RVA: 0x0005FAA4 File Offset: 0x0005DCA4
	private void FlashImage()
	{
		if (this.white)
		{
			this.white = false;
			this.targetImage.color = new Color(0f, 0f, 0f, 0f);
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			this.aud.Play();
		}
		else
		{
			this.white = true;
			this.targetImage.color = Color.white;
		}
		if (base.gameObject.activeSelf)
		{
			base.Invoke("FlashImage", 0.075f);
		}
	}

	// Token: 0x04001118 RID: 4376
	private Vector3 targetPos;

	// Token: 0x04001119 RID: 4377
	private Image targetImage;

	// Token: 0x0400111A RID: 4378
	public GameObject arrow;

	// Token: 0x0400111B RID: 4379
	public int firstLevelNumber;

	// Token: 0x0400111C RID: 4380
	private bool white = true;

	// Token: 0x0400111D RID: 4381
	private AudioSource aud;
}
