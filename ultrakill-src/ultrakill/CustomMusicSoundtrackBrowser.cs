using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

// Token: 0x0200016B RID: 363
public class CustomMusicSoundtrackBrowser : DirectoryTreeBrowser<AssetReferenceSoundtrackSong>
{
	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x06000713 RID: 1811 RVA: 0x0002D42B File Offset: 0x0002B62B
	protected override int maxPageLength
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x06000714 RID: 1812 RVA: 0x0002E284 File Offset: 0x0002C484
	protected override IDirectoryTree<AssetReferenceSoundtrackSong> baseDirectory
	{
		get
		{
			if (this._baseDirectory == null)
			{
				this.levelFolders.Add(new SoundtrackFolder("Secret Levels", this.secretLevelFolder));
				for (int i = 1; i <= 3; i++)
				{
					if (GameProgressSaver.GetPrime(0, i) > 0)
					{
						this.levelFolders.Add(new SoundtrackFolder("Prime Sanctums", this.primeFolder));
						break;
					}
				}
				if (GameProgressSaver.GetEncoreProgress(0) > 0)
				{
					this.levelFolders.Add(new SoundtrackFolder("Encores", this.encoreFolder));
				}
				this.levelFolders.Add(new SoundtrackFolder("Miscellaneous Tracks", this.miscFolder));
				this.levelFolders.Insert(0, new SoundtrackFolder("The Cyber Grind", this.rootFolder));
				IEnumerable<FakeDirectoryTree<AssetReferenceSoundtrackSong>> enumerable = this.levelFolders.Select((SoundtrackFolder f) => new FakeDirectoryTree<AssetReferenceSoundtrackSong>(f.name, f.songs, null, null));
				this._baseDirectory = DirectoryTreeBrowser<AssetReferenceSoundtrackSong>.Folder("OST", null, enumerable.Cast<IDirectoryTree<AssetReferenceSoundtrackSong>>().ToList<IDirectoryTree<AssetReferenceSoundtrackSong>>(), null);
			}
			return this._baseDirectory;
		}
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x00004AE3 File Offset: 0x00002CE3
	private void Start()
	{
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x0002E393 File Offset: 0x0002C593
	private void OnEnable()
	{
		this.Rebuild(true);
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x0002E39C File Offset: 0x0002C59C
	private void SelectSong(string id, SoundtrackSong song)
	{
		if (song.clips.Count > 0)
		{
			int count = this.playlistEditorLogic.playlist.Count;
			int num = this.playlistEditorLogic.PageOf(count);
			this.playlistEditorLogic.playlist.Add(new Playlist.SongIdentifier(id, Playlist.SongIdentifier.IdentifierType.Addressable));
			this.playlistEditorLogic.SetPage(num);
			this.playlistEditorLogic.Select(count, true);
			this.navigator.GoToNoMenu(this.playlistEditorPanel);
			return;
		}
		Debug.LogWarning("Attempted to add song with no clips to playlist.");
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x00004AE3 File Offset: 0x00002CE3
	private void OnDestroy()
	{
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x0002E421 File Offset: 0x0002C621
	public IEnumerator LoadSongButton(AssetReferenceSoundtrackSong reference, GameObject btn)
	{
		CustomMusicSoundtrackBrowser.<>c__DisplayClass26_0 CS$<>8__locals1 = new CustomMusicSoundtrackBrowser.<>c__DisplayClass26_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.reference = reference;
		CS$<>8__locals1.btn = btn;
		GameObject placeholder = Object.Instantiate<GameObject>(this.loadingPrefab, this.itemParent, false);
		placeholder.SetActive(true);
		if (this.referenceCache.ContainsKey(CS$<>8__locals1.reference))
		{
			WaitUntil waitUntil = new WaitUntil(() => CS$<>8__locals1.<>4__this.referenceCache[CS$<>8__locals1.reference] != null || CS$<>8__locals1.btn == null);
			yield return waitUntil;
			if (CS$<>8__locals1.btn == null)
			{
				Object.Destroy(placeholder);
				yield break;
			}
			CS$<>8__locals1.song = this.referenceCache[CS$<>8__locals1.reference];
		}
		else
		{
			CustomMusicSoundtrackBrowser.<>c__DisplayClass26_1 CS$<>8__locals2 = new CustomMusicSoundtrackBrowser.<>c__DisplayClass26_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			CS$<>8__locals2.handle = CS$<>8__locals2.CS$<>8__locals1.reference.LoadAssetAsync();
			this.referenceCache.Add(CS$<>8__locals2.CS$<>8__locals1.reference, null);
			WaitUntil waitUntil2 = new WaitUntil(() => CS$<>8__locals2.handle.IsDone || CS$<>8__locals2.CS$<>8__locals1.btn == null);
			yield return waitUntil2;
			if (CS$<>8__locals2.CS$<>8__locals1.btn == null)
			{
				Object.Destroy(placeholder);
				yield return CS$<>8__locals2.handle;
			}
			CS$<>8__locals2.CS$<>8__locals1.song = CS$<>8__locals2.handle.Result;
			this.referenceCache[CS$<>8__locals2.CS$<>8__locals1.reference] = CS$<>8__locals2.CS$<>8__locals1.song;
			Addressables.Release<SoundtrackSong>(CS$<>8__locals2.handle);
			if (CS$<>8__locals2.CS$<>8__locals1.btn == null)
			{
				yield break;
			}
			CS$<>8__locals2 = null;
		}
		Object.Destroy(placeholder);
		CustomContentButton componentInChildren = CS$<>8__locals1.btn.GetComponentInChildren<CustomContentButton>();
		componentInChildren.button.onClick.RemoveAllListeners();
		if (CS$<>8__locals1.song.conditions.AllMet())
		{
			componentInChildren.icon.sprite = ((CS$<>8__locals1.song.icon != null) ? CS$<>8__locals1.song.icon : this.defaultIcon);
			componentInChildren.text.text = CS$<>8__locals1.song.songName + " <color=grey>" + CS$<>8__locals1.song.extraLevelBit + "</color>";
			componentInChildren.costText.text = "Unlocked";
			componentInChildren.button.onClick.AddListener(delegate
			{
				CS$<>8__locals1.<>4__this.SelectSong(CS$<>8__locals1.reference.AssetGUID, CS$<>8__locals1.song);
			});
			this.SetActiveAll(componentInChildren.objectsToActivateIfAvailable, true);
			CS$<>8__locals1.btn.SetActive(true);
		}
		else
		{
			this.SetActiveAll(componentInChildren.objectsToActivateIfAvailable, false);
			componentInChildren.text.text = "????????? " + CS$<>8__locals1.song.extraLevelBit;
			componentInChildren.costText.text = CS$<>8__locals1.song.conditions.DescribeAll();
			componentInChildren.icon.sprite = this.lockedLevelSprite;
			componentInChildren.border.color = Color.grey;
			if (componentInChildren.iconInset != null)
			{
				componentInChildren.iconInset.color = Color.grey;
			}
			componentInChildren.text.color = Color.grey;
			componentInChildren.costText.color = Color.grey;
			CS$<>8__locals1.btn.SetActive(true);
		}
		yield break;
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x0002E440 File Offset: 0x0002C640
	protected override Action BuildLeaf(AssetReferenceSoundtrackSong reference, int indexInPage)
	{
		GameObject btn = Object.Instantiate<GameObject>(this.itemButtonTemplate, this.itemParent, false);
		base.StartCoroutine(this.LoadSongButton(reference, btn));
		return delegate
		{
			Object.Destroy(btn);
		};
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x0002E48C File Offset: 0x0002C68C
	private void SetActiveAll(List<GameObject> objects, bool active)
	{
		foreach (GameObject gameObject in objects)
		{
			gameObject.SetActive(active);
		}
	}

	// Token: 0x04000910 RID: 2320
	[Header("References")]
	[SerializeField]
	private CustomMusicPlaylistEditor playlistEditorLogic;

	// Token: 0x04000911 RID: 2321
	[SerializeField]
	private GameObject playlistEditorPanel;

	// Token: 0x04000912 RID: 2322
	[SerializeField]
	private CyberGrindSettingsNavigator navigator;

	// Token: 0x04000913 RID: 2323
	[SerializeField]
	private TMP_Text songName;

	// Token: 0x04000914 RID: 2324
	[SerializeField]
	private Image songIcon;

	// Token: 0x04000915 RID: 2325
	[Header("Assets")]
	[SerializeField]
	private GameObject loadingPrefab;

	// Token: 0x04000916 RID: 2326
	[SerializeField]
	private Sprite lockedLevelSprite;

	// Token: 0x04000917 RID: 2327
	[SerializeField]
	private Sprite defaultIcon;

	// Token: 0x04000918 RID: 2328
	[SerializeField]
	private GameObject buySound;

	// Token: 0x04000919 RID: 2329
	public List<AssetReferenceSoundtrackSong> rootFolder = new List<AssetReferenceSoundtrackSong>();

	// Token: 0x0400091A RID: 2330
	public List<SoundtrackFolder> levelFolders = new List<SoundtrackFolder>();

	// Token: 0x0400091B RID: 2331
	public List<AssetReferenceSoundtrackSong> secretLevelFolder = new List<AssetReferenceSoundtrackSong>();

	// Token: 0x0400091C RID: 2332
	public List<AssetReferenceSoundtrackSong> primeFolder = new List<AssetReferenceSoundtrackSong>();

	// Token: 0x0400091D RID: 2333
	public List<AssetReferenceSoundtrackSong> encoreFolder = new List<AssetReferenceSoundtrackSong>();

	// Token: 0x0400091E RID: 2334
	public List<AssetReferenceSoundtrackSong> miscFolder = new List<AssetReferenceSoundtrackSong>();

	// Token: 0x0400091F RID: 2335
	private FakeDirectoryTree<AssetReferenceSoundtrackSong> _baseDirectory;

	// Token: 0x04000920 RID: 2336
	private Dictionary<AssetReferenceSoundtrackSong, SoundtrackSong> referenceCache = new Dictionary<AssetReferenceSoundtrackSong, SoundtrackSong>();

	// Token: 0x04000921 RID: 2337
	private SoundtrackSong songBeingBought;
}
