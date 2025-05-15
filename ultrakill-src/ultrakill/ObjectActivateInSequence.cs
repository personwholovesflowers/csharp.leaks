using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000318 RID: 792
public class ObjectActivateInSequence : MonoBehaviour
{
	// Token: 0x06001243 RID: 4675 RVA: 0x00093334 File Offset: 0x00091534
	private void Awake()
	{
		this.aud = base.GetComponent<AudioSource>();
	}

	// Token: 0x06001244 RID: 4676 RVA: 0x00093344 File Offset: 0x00091544
	private void OnEnable()
	{
		foreach (GameObject gameObject in this.objectsToActivate)
		{
			if (!(gameObject == null))
			{
				gameObject.SetActive(false);
			}
		}
		this.coroutine = base.StartCoroutine(this.activationCoroutine());
	}

	// Token: 0x06001245 RID: 4677 RVA: 0x0009338C File Offset: 0x0009158C
	private void OnDisable()
	{
		if (this.coroutine != null)
		{
			base.StopCoroutine(this.coroutine);
		}
	}

	// Token: 0x06001246 RID: 4678 RVA: 0x000933A2 File Offset: 0x000915A2
	private IEnumerator activationCoroutine()
	{
		int i = 0;
		while (i < this.objectsToActivate.Length)
		{
			if (this.objectsToActivate[i] == null)
			{
				int num = i;
				i = num + 1;
			}
			else
			{
				this.objectsToActivate[i].SetActive(true);
				int num = i;
				i = num + 1;
				if (this.aud)
				{
					this.aud.Play();
				}
				yield return new WaitForSeconds(this.delay);
			}
		}
		yield break;
	}

	// Token: 0x0400193E RID: 6462
	public GameObject[] objectsToActivate;

	// Token: 0x0400193F RID: 6463
	private Coroutine coroutine;

	// Token: 0x04001940 RID: 6464
	public float delay;

	// Token: 0x04001941 RID: 6465
	private AudioSource aud;
}
