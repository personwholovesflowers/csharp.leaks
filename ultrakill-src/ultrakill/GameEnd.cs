using System;
using UnityEngine;

// Token: 0x0200020F RID: 527
public class GameEnd : MonoBehaviour
{
	// Token: 0x06000B1E RID: 2846 RVA: 0x0004FF6C File Offset: 0x0004E16C
	private void Awake()
	{
		this.endingSong = GameObject.FindWithTag("EndingSong").GetComponent<AudioSource>();
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x0004FF83 File Offset: 0x0004E183
	private void Update()
	{
		if (this.endingSong.volume < 1f)
		{
			this.endingSong.volume += Time.deltaTime / 2f;
		}
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x0004FFB4 File Offset: 0x0004E1B4
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.endingSong.volume = 0f;
			this.aud.volume = 0f;
			base.Invoke("EndGame", 0.1f);
		}
	}

	// Token: 0x06000B21 RID: 2849 RVA: 0x00050003 File Offset: 0x0004E203
	private void EndGame()
	{
		Application.Quit();
	}

	// Token: 0x04000EC9 RID: 3785
	public AudioSource aud;

	// Token: 0x04000ECA RID: 3786
	private AudioSource endingSong;
}
