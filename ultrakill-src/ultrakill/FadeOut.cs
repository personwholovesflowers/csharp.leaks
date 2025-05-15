using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public class FadeOut : MonoBehaviour
{
	// Token: 0x0600090A RID: 2314 RVA: 0x0003BFD4 File Offset: 0x0003A1D4
	private void Start()
	{
		if (this.auds == null || this.auds.Length == 0)
		{
			this.auds = base.GetComponents<AudioSource>();
		}
		if (this.fadeIn)
		{
			foreach (AudioSource audioSource in this.auds)
			{
				this.origVol.Add(audioSource.volume);
				audioSource.volume = 0f;
			}
		}
		this.player = MonoSingleton<NewMovement>.Instance.gameObject;
		if (this.activateOnEnable)
		{
			this.BeginFade();
		}
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x0003C05C File Offset: 0x0003A25C
	private void Update()
	{
		if (!this.fading)
		{
			if (this.distance)
			{
				if (this.fadeIn)
				{
					for (int i = 0; i < this.auds.Length; i++)
					{
						if (Vector3.Distance(base.transform.position, this.player.transform.position) > this.maxDistance)
						{
							this.auds[i].volume = 0f;
						}
						else
						{
							this.auds[i].volume = Mathf.Pow((Mathf.Sqrt(this.maxDistance) - Mathf.Sqrt(Vector3.Distance(base.transform.position, this.player.transform.position))) / Mathf.Sqrt(this.maxDistance), 2f) * this.origVol[i];
						}
					}
					return;
				}
				for (int j = 0; j < this.auds.Length; j++)
				{
					this.auds[j].volume = Vector3.Distance(base.transform.position, this.player.transform.position) / this.maxDistance * this.origVol[j];
				}
			}
			return;
		}
		if (this.fadeIn)
		{
			for (int k = 0; k < this.auds.Length; k++)
			{
				if (this.auds[k].isPlaying)
				{
					if (this.auds[k].volume == this.origVol[k])
					{
						this.fading = false;
					}
					else
					{
						this.auds[k].volume = Mathf.MoveTowards(this.auds[k].volume, this.origVol[k], Time.deltaTime * this.speed);
					}
				}
			}
			return;
		}
		foreach (AudioSource audioSource in this.auds)
		{
			if (audioSource.isPlaying || this.fadeEvenIfNotPlaying)
			{
				if (audioSource.volume <= 0f)
				{
					if (!this.dontStopOnZero)
					{
						audioSource.Stop();
					}
					else
					{
						this.fading = false;
					}
				}
				else
				{
					audioSource.volume -= Time.deltaTime * this.speed;
				}
			}
		}
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0003C291 File Offset: 0x0003A491
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			this.BeginFade();
		}
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x0003C2AC File Offset: 0x0003A4AC
	public void BeginFade()
	{
		this.fading = true;
		AudioSource[] array = this.auds;
		for (int i = 0; i < array.Length; i++)
		{
			GetMusicVolume component = array[i].GetComponent<GetMusicVolume>();
			if (component)
			{
				Object.Destroy(component);
			}
		}
	}

	// Token: 0x04000B7C RID: 2940
	public bool fadeIn;

	// Token: 0x04000B7D RID: 2941
	public bool distance;

	// Token: 0x04000B7E RID: 2942
	private List<float> origVol = new List<float>();

	// Token: 0x04000B7F RID: 2943
	public AudioSource[] auds;

	// Token: 0x04000B80 RID: 2944
	private bool fading;

	// Token: 0x04000B81 RID: 2945
	public float speed;

	// Token: 0x04000B82 RID: 2946
	public float maxDistance;

	// Token: 0x04000B83 RID: 2947
	public bool activateOnEnable;

	// Token: 0x04000B84 RID: 2948
	public bool dontStopOnZero;

	// Token: 0x04000B85 RID: 2949
	private GameObject player;

	// Token: 0x04000B86 RID: 2950
	public bool fadeEvenIfNotPlaying;
}
