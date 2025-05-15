using System;
using UnityEngine;

// Token: 0x0200030A RID: 778
public class MusicChanger : MonoBehaviour
{
	// Token: 0x060011B3 RID: 4531 RVA: 0x00089FCD File Offset: 0x000881CD
	private void OnEnable()
	{
		if (this.onEnable)
		{
			this.Change();
		}
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x00089FDD File Offset: 0x000881DD
	private void OnTriggerEnter(Collider other)
	{
		if (!this.onEnable && other.gameObject.CompareTag("Player"))
		{
			this.Change();
		}
	}

	// Token: 0x060011B5 RID: 4533 RVA: 0x00089FFF File Offset: 0x000881FF
	public void ChangeTo(AudioClip clip)
	{
		this.clean = clip;
		this.battle = clip;
		this.boss = clip;
		this.Change();
	}

	// Token: 0x060011B6 RID: 4534 RVA: 0x0008A01C File Offset: 0x0008821C
	public void Change()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.muman == null)
		{
			this.muman = MonoSingleton<MusicManager>.Instance;
		}
		if (this.oneTime || this.muman.cleanTheme.clip != this.clean || this.muman.battleTheme.clip != this.battle || (this.muman.off && (!this.muman.forcedOff || this.forceOn)))
		{
			float num = 0f;
			bool off = this.muman.off;
			if (this.match)
			{
				num = this.muman.cleanTheme.time;
			}
			else
			{
				this.muman.cleanTheme.time = 0f;
				this.muman.battleTheme.time = 0f;
				this.muman.bossTheme.time = 0f;
			}
			this.muman.StopMusic();
			this.muman.cleanTheme.pitch = this.pitch;
			this.muman.battleTheme.pitch = this.pitch;
			this.muman.bossTheme.pitch = this.pitch;
			this.muman.cleanTheme.clip = this.clean;
			this.muman.battleTheme.clip = this.battle;
			this.muman.bossTheme.clip = this.boss;
			if (this.forceOn)
			{
				this.muman.forcedOff = false;
			}
			if (!this.dontStart || !off)
			{
				this.muman.StartMusic();
			}
			if (this.match)
			{
				this.muman.cleanTheme.time = num;
				this.muman.battleTheme.time = num;
				this.muman.bossTheme.time = num;
			}
			if (this.oneTime)
			{
				Object.Destroy(this);
			}
		}
	}

	// Token: 0x060011B7 RID: 4535 RVA: 0x0008A228 File Offset: 0x00088428
	public void ChangePitch(float newPitch)
	{
		this.pitch = newPitch;
	}

	// Token: 0x04001822 RID: 6178
	public bool match;

	// Token: 0x04001823 RID: 6179
	public bool oneTime;

	// Token: 0x04001824 RID: 6180
	public bool onEnable;

	// Token: 0x04001825 RID: 6181
	public bool dontStart;

	// Token: 0x04001826 RID: 6182
	public bool forceOn;

	// Token: 0x04001827 RID: 6183
	public float pitch = 1f;

	// Token: 0x04001828 RID: 6184
	public AudioClip clean;

	// Token: 0x04001829 RID: 6185
	public AudioClip battle;

	// Token: 0x0400182A RID: 6186
	public AudioClip boss;

	// Token: 0x0400182B RID: 6187
	private MusicManager muman;
}
