using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000517 RID: 1303
public class TerribleTasteBook : MonoBehaviour
{
	// Token: 0x06001DB9 RID: 7609 RVA: 0x000F7E38 File Offset: 0x000F6038
	private void Start()
	{
		base.transform.GetChild(0).gameObject.SetActive(false);
	}

	// Token: 0x06001DBA RID: 7610 RVA: 0x000F7E51 File Offset: 0x000F6051
	public void ActivateBookShelf()
	{
		if (this.crt == null && this.otherSideBook.GetComponent<TerribleTasteBook>().crt == null)
		{
			this.crt = base.StartCoroutine(this.SpinShelf());
		}
	}

	// Token: 0x06001DBB RID: 7611 RVA: 0x000F7E7F File Offset: 0x000F607F
	private IEnumerator SpinShelf()
	{
		this.otherSideBook.enabled = false;
		Renderer rend = base.GetComponent<Renderer>();
		rend.enabled = false;
		MeshCollider col = rend.GetComponent<MeshCollider>();
		col.enabled = false;
		base.transform.GetChild(0).gameObject.SetActive(true);
		Transform parent = base.transform.parent;
		Quaternion startRot = parent.rotation;
		Quaternion endRot = parent.rotation * Quaternion.AngleAxis(180f, Vector3.forward);
		float progress = 0f;
		while (progress <= this.spinTime)
		{
			progress += Time.deltaTime;
			parent.rotation = Quaternion.Slerp(startRot, endRot, progress / this.spinTime);
			yield return null;
		}
		rend.enabled = true;
		col.enabled = true;
		base.transform.GetChild(0).gameObject.SetActive(false);
		yield return null;
		this.crt = null;
		this.otherSideBook.enabled = true;
		yield break;
	}

	// Token: 0x04002A19 RID: 10777
	public float spinTime = 2f;

	// Token: 0x04002A1A RID: 10778
	public Coroutine crt;

	// Token: 0x04002A1B RID: 10779
	public TerribleTasteBook otherSideBook;
}
