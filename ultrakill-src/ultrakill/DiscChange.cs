using System;
using UnityEngine;

// Token: 0x02000118 RID: 280
public class DiscChange : MonoBehaviour
{
	// Token: 0x0600052A RID: 1322 RVA: 0x00022604 File Offset: 0x00020804
	private void Update()
	{
		if (this.ready && !this.done)
		{
			this.done = true;
			PlayerPrefs.SetInt("DisCha", 1);
			this.discSound.transform.parent = null;
			Object.DontDestroyOnLoad(this.discSound);
			FadeOut fadeOut;
			if (this.discSound.TryGetComponent<FadeOut>(out fadeOut))
			{
				fadeOut.BeginFade();
			}
			SceneHelper.LoadScene(this.targetLevelName, true);
		}
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x00022670 File Offset: 0x00020870
	public void ReadyToChangeLevel()
	{
		this.ready = true;
	}

	// Token: 0x0400070F RID: 1807
	public string targetLevelName;

	// Token: 0x04000710 RID: 1808
	private bool ready;

	// Token: 0x04000711 RID: 1809
	private bool done;

	// Token: 0x04000712 RID: 1810
	[SerializeField]
	private GameObject discSound;
}
