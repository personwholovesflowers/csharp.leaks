using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000145 RID: 325
public class PauseEvent : MonoBehaviour
{
	// Token: 0x06000795 RID: 1941 RVA: 0x00026A20 File Offset: 0x00024C20
	private void Awake()
	{
		GameMaster.OnPause += this.OnPause;
		GameMaster.OnResume += this.OnResume;
		SceneManager.sceneLoaded += this.OnSceneLoaded;
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x00026A55 File Offset: 0x00024C55
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (this.ResumeEventOnSceneLoad)
		{
			this.OnResume();
		}
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x00026A65 File Offset: 0x00024C65
	private void OnDestroy()
	{
		GameMaster.OnPause -= this.OnPause;
		GameMaster.OnResume -= this.OnResume;
		SceneManager.sceneLoaded -= this.OnSceneLoaded;
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x00026A9A File Offset: 0x00024C9A
	private void OnPause()
	{
		if (!this.lowendOnly && this.defaultConfigConfigurable.GetBooleanValue())
		{
			return;
		}
		this.OnPauseEvent.Invoke(true);
		this.OnPauseInverseEvent.Invoke(false);
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x00026ACA File Offset: 0x00024CCA
	private void OnResume()
	{
		if (!this.lowendOnly && this.defaultConfigConfigurable.GetBooleanValue())
		{
			return;
		}
		this.OnPauseEvent.Invoke(false);
		this.OnPauseInverseEvent.Invoke(true);
	}

	// Token: 0x040007B9 RID: 1977
	[SerializeField]
	private bool lowendOnly = true;

	// Token: 0x040007BA RID: 1978
	[SerializeField]
	private bool ResumeEventOnSceneLoad;

	// Token: 0x040007BB RID: 1979
	[SerializeField]
	private BooleanConfigurable defaultConfigConfigurable;

	// Token: 0x040007BC RID: 1980
	[SerializeField]
	private BooleanValueChangedEvent OnPauseEvent;

	// Token: 0x040007BD RID: 1981
	[SerializeField]
	private BooleanValueChangedEvent OnPauseInverseEvent;
}
