using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000094 RID: 148
[CreateAssetMenu(fileName = "New Audio Collection", menuName = "Data/Audio Collection")]
public class AudioCollection : ScriptableObject
{
	// Token: 0x17000043 RID: 67
	// (get) Token: 0x0600039C RID: 924 RVA: 0x00017C96 File Offset: 0x00015E96
	public float AverageDuration
	{
		get
		{
			return this.averageDuration;
		}
	}

	// Token: 0x0600039D RID: 925 RVA: 0x00017C9E File Offset: 0x00015E9E
	public int GetIndex()
	{
		return this.index;
	}

	// Token: 0x0600039E RID: 926 RVA: 0x00017CA8 File Offset: 0x00015EA8
	public bool SetVolumeAndPitchAndPlayClip(AudioSource audioSource)
	{
		AudioEntry audioEntry;
		AudioClip audioClip;
		if (this.GetRandomEntry(out audioEntry) && audioSource != null && audioEntry.GetClip(out audioClip))
		{
			audioSource.clip = audioClip;
			audioSource.volume = audioEntry.GetVolume() * this.masterVolume;
			audioSource.pitch = audioEntry.GetPitch();
			if (audioSource.gameObject.activeInHierarchy)
			{
				audioSource.Play();
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600039F RID: 927 RVA: 0x00017D10 File Offset: 0x00015F10
	private bool GetRandomEntry(out AudioEntry entry)
	{
		if (this.usePlaylistSorting && this.AudioEntries.Count > 0)
		{
			if (!this.setup)
			{
				this.UpdatePlaylist();
				entry = this.GetNextEntryFromPlaylist();
				this.setup = true;
				return true;
			}
			entry = this.GetNextEntryFromPlaylist();
			return true;
		}
		else
		{
			if (this.AudioEntries.Count > 0)
			{
				entry = this.AudioEntries[Random.Range(0, this.AudioEntries.Count)];
				return true;
			}
			entry = null;
			return false;
		}
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x00017D90 File Offset: 0x00015F90
	private void UpdatePlaylist()
	{
		this.entryPlaylist = new AudioEntry[this.AudioEntries.Count];
		Array.Copy(this.AudioEntries.ToArray(), 0, this.entryPlaylist, 0, this.AudioEntries.Count);
		if (!this.sequentialPlaylist)
		{
			new Random().Shuffle(this.entryPlaylist);
		}
		this.playlistIndex = 0;
		this.playlistLength = this.entryPlaylist.Length;
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x00017E03 File Offset: 0x00016003
	private AudioEntry GetNextEntryFromPlaylist()
	{
		if (this.playlistIndex >= this.playlistLength)
		{
			this.UpdatePlaylist();
		}
		AudioEntry audioEntry = this.entryPlaylist[this.playlistIndex];
		this.playlistIndex++;
		return audioEntry;
	}

	// Token: 0x0400038D RID: 909
	[SerializeField]
	[Range(0f, 2f)]
	private float masterVolume = 1f;

	// Token: 0x0400038E RID: 910
	[SerializeField]
	private int index;

	// Token: 0x0400038F RID: 911
	[SerializeField]
	[Range(0f, 3f)]
	private float averageDuration = 0.25f;

	// Token: 0x04000390 RID: 912
	[SerializeField]
	private bool usePlaylistSorting;

	// Token: 0x04000391 RID: 913
	[SerializeField]
	private bool sequentialPlaylist;

	// Token: 0x04000392 RID: 914
	[Space]
	[SerializeField]
	public List<AudioEntry> AudioEntries;

	// Token: 0x04000393 RID: 915
	[NonSerialized]
	private AudioEntry[] entryPlaylist;

	// Token: 0x04000394 RID: 916
	[NonSerialized]
	private int playlistLength;

	// Token: 0x04000395 RID: 917
	[NonSerialized]
	private int playlistIndex;

	// Token: 0x04000396 RID: 918
	[NonSerialized]
	private bool setup;

	// Token: 0x04000397 RID: 919
	public const bool SHOW_DEBUG = false;
}
