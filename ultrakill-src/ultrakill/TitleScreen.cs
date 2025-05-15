using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002BE RID: 702
public class TitleScreen : MonoBehaviour
{
	// Token: 0x06000F26 RID: 3878 RVA: 0x0006FEDC File Offset: 0x0006E0DC
	private void OnTriggerEnter(Collider other)
	{
		if (!this.sequenceStarted && other.gameObject.CompareTag("Player"))
		{
			this.sequenceStarted = true;
			this.RevolverPickedUp();
			GameObject[] array = this.hud;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
			this.mman.StartMusic();
			this.mman.ArenaMusicStart();
			this.titleScreen.SetActive(true);
			this.title = this.titleScreen.GetComponent<Text>();
			this.title.resizeTextMaxSize = 1000;
			base.Invoke("HideTitle", 4.5f);
		}
	}

	// Token: 0x06000F27 RID: 3879 RVA: 0x0006FF84 File Offset: 0x0006E184
	private void Update()
	{
		if (this.sequenceStarted && this.titleScreen.activeSelf)
		{
			base.transform.parent.position += Vector3.down * Time.deltaTime * 10f;
		}
	}

	// Token: 0x06000F28 RID: 3880 RVA: 0x0006FFDA File Offset: 0x0006E1DA
	private void HideTitle()
	{
		this.titleScreen.SetActive(false);
		this.arenaActivator.SetActive(true);
	}

	// Token: 0x06000F29 RID: 3881 RVA: 0x0006FFF4 File Offset: 0x0006E1F4
	private void RevolverPickedUp()
	{
		this.worldRevolver.SetActive(false);
		this.revolver.SetActive(true);
	}

	// Token: 0x0400144C RID: 5196
	private bool sequenceStarted;

	// Token: 0x0400144D RID: 5197
	public MusicManager mman;

	// Token: 0x0400144E RID: 5198
	public GameObject[] hud;

	// Token: 0x0400144F RID: 5199
	public GameObject titleScreen;

	// Token: 0x04001450 RID: 5200
	private Text title;

	// Token: 0x04001451 RID: 5201
	public GameObject worldRevolver;

	// Token: 0x04001452 RID: 5202
	public GameObject revolver;

	// Token: 0x04001453 RID: 5203
	public GameObject arenaActivator;
}
