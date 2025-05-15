using System;
using UnityEngine;

// Token: 0x02000165 RID: 357
public class CustomMusicPlaceholder : MonoBehaviour
{
	// Token: 0x060006EA RID: 1770 RVA: 0x0002D626 File Offset: 0x0002B826
	private void Awake()
	{
		Random.Range(0, 1);
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x0002D630 File Offset: 0x0002B830
	private void OnEnable()
	{
		this.sinceEnabled = this.offset;
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x0002D643 File Offset: 0x0002B843
	private void Update()
	{
		base.transform.localRotation = Quaternion.Euler(0f, 0f, this.sinceEnabled * -360f);
	}

	// Token: 0x040008F5 RID: 2293
	private float offset;

	// Token: 0x040008F6 RID: 2294
	private TimeSince sinceEnabled;
}
