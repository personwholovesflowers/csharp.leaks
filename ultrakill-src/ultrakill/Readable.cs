using System;
using UnityEngine;

// Token: 0x02000384 RID: 900
public class Readable : MonoBehaviour
{
	// Token: 0x060014BC RID: 5308 RVA: 0x000A77FE File Offset: 0x000A59FE
	private void Awake()
	{
		this.gameObjectInstanceId = base.gameObject.GetInstanceID();
	}

	// Token: 0x060014BD RID: 5309 RVA: 0x000A7811 File Offset: 0x000A5A11
	public void PickUp()
	{
		this.pickedUp = true;
		MonoSingleton<ScanningStuff>.Instance.oldWeaponState = !MonoSingleton<GunControl>.Instance.noWeapons;
		base.Invoke("StartScan", 0.5f);
	}

	// Token: 0x060014BE RID: 5310 RVA: 0x000A7841 File Offset: 0x000A5A41
	public void PutDown()
	{
		this.pickedUp = false;
		base.CancelInvoke("StartScan");
		MonoSingleton<ScanningStuff>.Instance.ResetState();
	}

	// Token: 0x060014BF RID: 5311 RVA: 0x000A785F File Offset: 0x000A5A5F
	private void StartScan()
	{
		MonoSingleton<ScanningStuff>.Instance.ScanBook(this.content, this.instantScan, base.gameObject.GetInstanceID());
	}

	// Token: 0x060014C0 RID: 5312 RVA: 0x000A7882 File Offset: 0x000A5A82
	private void OnDestroy()
	{
		if (MonoSingleton<ScanningStuff>.Instance)
		{
			MonoSingleton<ScanningStuff>.Instance.ReleaseScroll(this.gameObjectInstanceId);
		}
	}

	// Token: 0x04001C8B RID: 7307
	[SerializeField]
	[TextArea(3, 12)]
	private string content;

	// Token: 0x04001C8C RID: 7308
	[SerializeField]
	private bool instantScan;

	// Token: 0x04001C8D RID: 7309
	private bool pickedUp;

	// Token: 0x04001C8E RID: 7310
	private int gameObjectInstanceId;
}
