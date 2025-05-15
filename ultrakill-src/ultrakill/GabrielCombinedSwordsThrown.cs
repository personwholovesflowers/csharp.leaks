using System;
using UnityEngine;

// Token: 0x02000203 RID: 515
public class GabrielCombinedSwordsThrown : MonoBehaviour
{
	// Token: 0x06000AAB RID: 2731 RVA: 0x0004C3F4 File Offset: 0x0004A5F4
	private void Start()
	{
		if (!this.gabe)
		{
			this.gabe = Object.FindObjectOfType<GabrielSecond>();
		}
		this.parent = base.transform.parent;
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x0004C420 File Offset: 0x0004A620
	private void OnDestroy()
	{
		if (!base.gameObject.scene.isLoaded)
		{
			return;
		}
		if (!this.parent || !this.parent.gameObject.activeSelf)
		{
			return;
		}
		if (!this.gabe)
		{
			return;
		}
		if (this.gabe.swordsCombined)
		{
			this.gabe.UnGattai(false);
		}
		this.CreateTrail(this.justice, this.gabe.leftHand);
		this.CreateTrail(this.splendor, this.gabe.rightHand);
		AudioSource audioSource;
		if (Object.Instantiate<GameObject>(this.gabe.teleportSound, this.gabe.transform.position, Quaternion.identity).TryGetComponent<AudioSource>(out audioSource))
		{
			audioSource.pitch = 1.5f;
		}
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0004C4F4 File Offset: 0x0004A6F4
	private void CreateTrail(Transform start, Transform target)
	{
		if (!start || !target)
		{
			return;
		}
		int num = Mathf.RoundToInt(Vector3.Distance(start.position, target.position) / 2.5f);
		for (int i = 0; i < num; i++)
		{
			MindflayerDecoy[] componentsInChildren = Object.Instantiate<GameObject>(this.teleportSword, Vector3.Lerp(start.position, target.position, (float)i / (float)num), Quaternion.Lerp(start.rotation, target.rotation, (float)i / (float)num)).GetComponentsInChildren<MindflayerDecoy>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].fadeOverride = (float)i / (float)num + 0.1f;
			}
		}
	}

	// Token: 0x04000E33 RID: 3635
	public Transform justice;

	// Token: 0x04000E34 RID: 3636
	public Transform splendor;

	// Token: 0x04000E35 RID: 3637
	public GameObject teleportSword;

	// Token: 0x04000E36 RID: 3638
	[HideInInspector]
	public GabrielSecond gabe;

	// Token: 0x04000E37 RID: 3639
	private Transform parent;
}
