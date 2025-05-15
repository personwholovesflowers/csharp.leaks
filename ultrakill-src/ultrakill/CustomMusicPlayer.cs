using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

// Token: 0x0200014C RID: 332
public class CustomMusicPlayer : MonoBehaviour
{
	// Token: 0x0600068C RID: 1676 RVA: 0x0002C5CB File Offset: 0x0002A7CB
	public void OnEnable()
	{
		this.StartPlaylist();
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x0002C5D3 File Offset: 0x0002A7D3
	public void StartPlaylist()
	{
		if (this.playlistEditor.playlist.Count < 1)
		{
			Debug.LogError("No songs in playlist, somehow. Not starting playlist routine...");
			return;
		}
		base.StartCoroutine(this.PlaylistRoutine());
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x0002C600 File Offset: 0x0002A800
	public void StopPlaylist()
	{
		this.stopped = true;
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x0002C609 File Offset: 0x0002A809
	private IEnumerator ShowPanelRoutine(Playlist.SongMetadata song)
	{
		this.panelText.text = song.displayName.ToUpper();
		this.panelIcon.sprite = ((song.icon != null) ? song.icon : this.defaultIcon);
		float time = 0f;
		while (time < this.panelApproachTime)
		{
			time += Time.deltaTime;
			this.panelGroup.alpha = time / this.panelApproachTime;
			yield return null;
		}
		this.panelGroup.alpha = 1f;
		yield return new WaitForSecondsRealtime(this.panelStayTime);
		time = this.panelApproachTime;
		while (time > 0f)
		{
			time -= Time.deltaTime;
			this.panelGroup.alpha = time / this.panelApproachTime;
			yield return null;
		}
		this.panelGroup.alpha = 0f;
		yield break;
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x0002C61F File Offset: 0x0002A81F
	private IEnumerator PlaylistRoutine()
	{
		WaitUntil songFinished = new WaitUntil(() => Application.isFocused && !this.source.isPlaying);
		Playlist.SongIdentifier lastSong = null;
		bool first = true;
		Playlist playlist = this.playlistEditor.playlist;
		IEnumerable<Playlist.SongIdentifier> currentOrder = (playlist.shuffled ? new DeckShuffled<Playlist.SongIdentifier>(playlist.ids).AsEnumerable<Playlist.SongIdentifier>() : playlist.ids.AsEnumerable<Playlist.SongIdentifier>());
		if (playlist.loopMode == Playlist.LoopMode.LoopOne)
		{
			currentOrder = currentOrder.Skip(playlist.selected).Take(1);
		}
		while (!this.stopped)
		{
			DeckShuffled<Playlist.SongIdentifier> deckShuffled = currentOrder as DeckShuffled<Playlist.SongIdentifier>;
			if (deckShuffled != null)
			{
				deckShuffled.Reshuffle();
			}
			foreach (Playlist.SongIdentifier id in currentOrder)
			{
				Playlist.SongMetadata songMetadata = this.playlistEditor.GetSongMetadata(id);
				if (id != lastSong)
				{
					base.StartCoroutine(this.ShowPanelRoutine(songMetadata));
				}
				lastSong = id;
				if (id.type == Playlist.SongIdentifier.IdentifierType.File)
				{
					FileInfo fileInfo = new FileInfo(id.path);
					AudioType audioType = CustomMusicFileBrowser.extensionTypeDict[fileInfo.Extension.ToLower()];
					using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(new Uri(id.path).AbsoluteUri, audioType))
					{
						DownloadHandlerAudioClip handler = request.downloadHandler as DownloadHandlerAudioClip;
						handler.streamAudio = true;
						request.SendWebRequest();
						yield return request;
						this.source.clip = handler.audioClip;
						this.source.Play();
						yield return songFinished;
						Object.Destroy(handler.audioClip);
						handler = null;
					}
					UnityWebRequest request = null;
				}
				if (id.type == Playlist.SongIdentifier.IdentifierType.Addressable)
				{
					AsyncOperationHandle<SoundtrackSong> handle = Addressables.LoadAssetAsync<SoundtrackSong>(id.path);
					yield return handle;
					SoundtrackSong song = handle.Result;
					if (first)
					{
						this.source.clip = song.introClip;
						this.source.Play();
						yield return songFinished;
					}
					int clipsPlayed = 0;
					foreach (AudioClip audioClip in song.clips)
					{
						this.source.clip = audioClip;
						this.source.Play();
						yield return songFinished;
						int num = clipsPlayed;
						clipsPlayed = num + 1;
						if (playlist.loopMode != Playlist.LoopMode.LoopOne && song.maxClipsIfNotRepeating > 0 && clipsPlayed >= song.maxClipsIfNotRepeating)
						{
							break;
						}
					}
					List<AudioClip>.Enumerator enumerator2 = default(List<AudioClip>.Enumerator);
					Addressables.Release<SoundtrackSong>(handle);
					handle = default(AsyncOperationHandle<SoundtrackSong>);
					song = null;
				}
				first = false;
				id = null;
			}
			IEnumerator<Playlist.SongIdentifier> enumerator = null;
		}
		yield break;
		yield break;
	}

	// Token: 0x040008A2 RID: 2210
	[SerializeField]
	private CanvasGroup panelGroup;

	// Token: 0x040008A3 RID: 2211
	[SerializeField]
	private Text panelText;

	// Token: 0x040008A4 RID: 2212
	[SerializeField]
	private Image panelIcon;

	// Token: 0x040008A5 RID: 2213
	[SerializeField]
	private CustomMusicPlaylistEditor playlistEditor;

	// Token: 0x040008A6 RID: 2214
	[SerializeField]
	private Sprite defaultIcon;

	// Token: 0x040008A7 RID: 2215
	public AudioSource source;

	// Token: 0x040008A8 RID: 2216
	public float panelApproachTime;

	// Token: 0x040008A9 RID: 2217
	public float panelStayTime;

	// Token: 0x040008AA RID: 2218
	private Random random = new Random();

	// Token: 0x040008AB RID: 2219
	private bool stopped;

	// Token: 0x040008AC RID: 2220
	public Dictionary<string, AudioClip> fileClipCache = new Dictionary<string, AudioClip>();
}
