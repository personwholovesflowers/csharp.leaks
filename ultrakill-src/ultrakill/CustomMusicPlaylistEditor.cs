using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

// Token: 0x02000166 RID: 358
public class CustomMusicPlaylistEditor : DirectoryTreeBrowser<Playlist.SongIdentifier>
{
	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x060006EE RID: 1774 RVA: 0x0002D670 File Offset: 0x0002B870
	protected override int maxPageLength
	{
		get
		{
			return this.anchors.Count;
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x060006EF RID: 1775 RVA: 0x0002D67D File Offset: 0x0002B87D
	protected override IDirectoryTree<Playlist.SongIdentifier> baseDirectory
	{
		get
		{
			return new FakeDirectoryTree<Playlist.SongIdentifier>("Songs", this.playlist.ids, null, null);
		}
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x060006F0 RID: 1776 RVA: 0x0002D696 File Offset: 0x0002B896
	private Playlist.SongIdentifier selectedSongId
	{
		get
		{
			return this.playlist.ids[this.playlist.selected];
		}
	}

	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0002D6B3 File Offset: 0x0002B8B3
	private CustomContentButton currentButton
	{
		get
		{
			Transform transform = this.buttons.ElementAtOrDefault(this.playlist.selected % this.maxPageLength);
			if (transform == null)
			{
				return null;
			}
			return transform.GetComponent<CustomContentButton>();
		}
	}

	// Token: 0x060006F2 RID: 1778 RVA: 0x0002D6E0 File Offset: 0x0002B8E0
	public Playlist.SongMetadata GetSongMetadata(Playlist.SongIdentifier id)
	{
		Playlist.SongMetadata songMetadata;
		if (this.metadataDict.TryGetValue(id, out songMetadata))
		{
			return songMetadata;
		}
		Playlist.SongIdentifier.IdentifierType type = id.type;
		Playlist.SongMetadata songMetadata2;
		if (type != Playlist.SongIdentifier.IdentifierType.Addressable)
		{
			if (type != Playlist.SongIdentifier.IdentifierType.File)
			{
				throw new ArgumentException(string.Format("Could not fetch matadata: SongIdentifier '{0}' has invalid type '{1}'.", id.path, id.type));
			}
			songMetadata2 = this.GetSongMetadataFromFilepath(id);
		}
		else
		{
			songMetadata2 = this.GetSongMetadataFromAddressable(id);
		}
		this.metadataDict.Add(id, songMetadata2);
		return songMetadata2;
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x0002D754 File Offset: 0x0002B954
	private Playlist.SongMetadata GetSongMetadataFromAddressable(Playlist.SongIdentifier id)
	{
		AsyncOperationHandle<SoundtrackSong> asyncOperationHandle = new AssetReferenceSoundtrackSong(id.path).LoadAssetAsync();
		asyncOperationHandle.WaitForCompletion();
		return new Playlist.SongMetadata(asyncOperationHandle.Result.songName, asyncOperationHandle.Result.icon, asyncOperationHandle.Result.maxClipsIfNotRepeating);
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x0002D7A3 File Offset: 0x0002B9A3
	private Playlist.SongMetadata GetSongMetadataFromFilepath(Playlist.SongIdentifier id)
	{
		return new Playlist.SongMetadata(new FileInfo(id.path).Name, this.defaultIcon, 1);
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x0002D7C1 File Offset: 0x0002B9C1
	public void SavePlaylist()
	{
		File.WriteAllText(Playlist.currentPath, JsonConvert.SerializeObject(this.playlist));
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x0002D7D8 File Offset: 0x0002B9D8
	public void LoadPlaylist()
	{
		Debug.Log("Loading Playlist");
		Playlist playlist = null;
		using (StreamReader streamReader = new StreamReader(File.Open(Playlist.currentPath, FileMode.OpenOrCreate)))
		{
			playlist = JsonConvert.DeserializeObject<Playlist>(streamReader.ReadToEnd());
		}
		if (playlist == null)
		{
			Debug.Log("No saved playlist found at " + Playlist.currentPath + ". Creating default...");
			using (List<AssetReferenceSoundtrackSong>.Enumerator enumerator = this.browser.rootFolder.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AssetReferenceSoundtrackSong assetReferenceSoundtrackSong = enumerator.Current;
					this.playlist.Add(new Playlist.SongIdentifier(assetReferenceSoundtrackSong.AssetGUID, Playlist.SongIdentifier.IdentifierType.Addressable));
				}
				goto IL_00B6;
			}
		}
		this.playlist = playlist;
		this.currentDirectory = this.baseDirectory;
		this.Rebuild(true);
		IL_00B6:
		this.Rebuild(true);
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x0002D8C0 File Offset: 0x0002BAC0
	public void Remove()
	{
		this.playlist.Remove(this.playlist.selected);
		if (this.playlist.selected >= this.playlist.ids.Count)
		{
			this.Select(this.playlist.Count - 1, true);
		}
		this.Rebuild(false);
	}

	// Token: 0x060006F8 RID: 1784 RVA: 0x0002D91B File Offset: 0x0002BB1B
	public void MoveUp()
	{
		this.Move(-1);
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x0002D924 File Offset: 0x0002BB24
	public void MoveDown()
	{
		this.Move(1);
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x0002D930 File Offset: 0x0002BB30
	public void Move(int amount)
	{
		int num = this.playlist.selected % this.maxPageLength;
		int num2 = num + amount;
		bool flag = base.PageOf(this.playlist.selected) == base.PageOf(this.playlist.selected + amount);
		if (this.playlist.selected + amount >= 0 && this.playlist.selected + amount < this.playlist.ids.Count)
		{
			this.playlist.Swap(this.playlist.selected, this.playlist.selected + amount);
			if (flag)
			{
				this.ChangeAnchorOf(this.buttons[num], this.anchors[num2], 0.15f);
				this.ChangeAnchorOf(this.selectedControls, this.anchors[num2], 0.15f);
				this.ChangeAnchorOf(this.buttons[num2], this.anchors[num], 0.15f);
				CustomContentButton currentButton = this.currentButton;
				this.buttons.RemoveAt(num);
				this.buttons.Insert(num2, currentButton.transform);
				this.Select(this.playlist.selected + amount, false);
				return;
			}
			this.selectedControls.gameObject.SetActive(false);
			this.Select(this.playlist.selected + amount, true);
		}
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x0002DA9C File Offset: 0x0002BC9C
	public void ChangeAnchorOf(Transform obj, Transform anchor, float time = 0.15f)
	{
		CustomMusicPlaylistEditor.<>c__DisplayClass30_0 CS$<>8__locals1 = new CustomMusicPlaylistEditor.<>c__DisplayClass30_0();
		CS$<>8__locals1.obj = obj;
		CS$<>8__locals1.anchor = anchor;
		CS$<>8__locals1.time = time;
		if (this.changeAnchorRoutines.ContainsKey(CS$<>8__locals1.obj))
		{
			if (this.changeAnchorRoutines[CS$<>8__locals1.obj] != null)
			{
				base.StopCoroutine(this.changeAnchorRoutines[CS$<>8__locals1.obj]);
			}
			this.changeAnchorRoutines.Remove(CS$<>8__locals1.obj);
		}
		this.changeAnchorRoutines.Add(CS$<>8__locals1.obj, base.StartCoroutine(CS$<>8__locals1.<ChangeAnchorOf>g__ChangeAnchorOverTime|0()));
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x0002DB30 File Offset: 0x0002BD30
	public void ToggleLoopMode()
	{
		this.SetLoopMode((this.playlist.loopMode == Playlist.LoopMode.Loop) ? Playlist.LoopMode.LoopOne : Playlist.LoopMode.Loop);
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x0002DB49 File Offset: 0x0002BD49
	private void SetLoopMode(Playlist.LoopMode mode)
	{
		this.playlist.loopMode = mode;
		this.loopModeImage.sprite = ((this.playlist.loopMode == Playlist.LoopMode.Loop) ? this.loopSprite : this.loopOnceSprite);
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x0002DB7D File Offset: 0x0002BD7D
	public void ToggleShuffle()
	{
		this.SetShuffle(!this.playlist.shuffled);
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x0002DB93 File Offset: 0x0002BD93
	private void SetShuffle(bool shuffle)
	{
		this.playlist.shuffled = shuffle;
		this.shuffleImage.color = (shuffle ? Color.white : Color.gray);
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x0002DBBC File Offset: 0x0002BDBC
	public void Select(int newIndex, bool rebuild = true)
	{
		if (newIndex < 0 || newIndex >= this.playlist.Count)
		{
			Debug.LogWarning("Attempted to set current index outside bounds of playlist");
			return;
		}
		bool flag = base.PageOf(newIndex) == this.currentPage;
		if (this.currentButton)
		{
			this.currentButton.border.color = Color.white;
			if (this.currentButton.iconInset != null)
			{
				this.currentButton.iconInset.color = Color.white;
			}
		}
		int selected = this.playlist.selected;
		this.playlist.selected = newIndex;
		if (base.PageOf(selected) < base.PageOf(newIndex))
		{
			this.ChangeAnchorOf(this.selectedControls, this.anchors.First<Transform>(), 0f);
		}
		else if (base.PageOf(selected) > base.PageOf(newIndex))
		{
			this.ChangeAnchorOf(this.selectedControls, this.anchors.Last<Transform>(), 0f);
		}
		if (this.currentButton)
		{
			this.currentButton.border.color = Color.red;
			if (this.currentButton.iconInset != null)
			{
				this.currentButton.iconInset.color = Color.red;
			}
		}
		Transform transform = this.anchors[this.playlist.selected % this.maxPageLength];
		if (flag)
		{
			this.selectedControls.gameObject.SetActive(true);
			this.ChangeAnchorOf(this.selectedControls, transform, 0.15f);
		}
		else
		{
			this.selectedControls.gameObject.SetActive(false);
			this.selectedControls.transform.position = transform.position;
		}
		if (rebuild)
		{
			this.Rebuild(false);
		}
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x0002DD74 File Offset: 0x0002BF74
	public override void Rebuild(bool setToPageZero = true)
	{
		foreach (KeyValuePair<Transform, Coroutine> keyValuePair in this.changeAnchorRoutines)
		{
			if (keyValuePair.Value != null)
			{
				base.StopCoroutine(keyValuePair.Value);
			}
		}
		this.changeAnchorRoutines.Clear();
		this.buttons.Clear();
		base.Rebuild(setToPageZero);
		if (this.buttons.Count < this.maxPageLength)
		{
			this.ChangeAnchorOf(this.plusButton.transform, this.anchors[this.buttons.Count], 0f);
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.itemParent as RectTransform);
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x0002DE44 File Offset: 0x0002C044
	protected override Action BuildLeaf(Playlist.SongIdentifier id, int currentIndex)
	{
		Playlist.SongMetadata songMetadata = this.GetSongMetadata(id);
		GameObject go = Object.Instantiate<GameObject>(this.itemButtonTemplate, this.itemButtonTemplate.transform.parent);
		CustomContentButton contentButton = go.GetComponent<CustomContentButton>();
		contentButton.text.text = songMetadata.displayName;
		contentButton.icon.sprite = ((songMetadata.icon != null) ? songMetadata.icon : this.defaultIcon);
		go.SetActive(true);
		this.ChangeAnchorOf(go.transform, this.anchors[currentIndex], 0f);
		this.buttons.Add(go.transform);
		if (base.PageOf(this.playlist.selected) == this.currentPage && contentButton == this.currentButton)
		{
			contentButton.border.color = Color.red;
			if (this.currentButton.iconInset != null)
			{
				this.currentButton.iconInset.color = Color.red;
			}
			this.selectedControls.gameObject.SetActive(true);
			this.ChangeAnchorOf(this.selectedControls, this.anchors[currentIndex], 0.15f);
			return delegate
			{
				this.selectedControls.gameObject.SetActive(false);
				Object.Destroy(go);
			};
		}
		contentButton.button.onClick.AddListener(delegate
		{
			this.Select(this.buttons.IndexOf(contentButton.transform) + this.currentPage * this.maxPageLength, true);
		});
		return delegate
		{
			Object.Destroy(go);
		};
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x0002DFF4 File Offset: 0x0002C1F4
	private void Start()
	{
		try
		{
			this.LoadPlaylist();
		}
		catch (JsonReaderException ex)
		{
			Debug.LogError("Error loading Playlist.json: '" + ex.Message + "'. Recreating file.");
			File.Delete(Playlist.currentPath);
			this.LoadPlaylist();
		}
		this.Select(this.playlist.selected, true);
		this.SetLoopMode(this.playlist.loopMode);
		this.SetShuffle(this.playlist.shuffled);
		this.playlist.OnChanged += this.SavePlaylist;
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x0002E094 File Offset: 0x0002C294
	private void OnDestroy()
	{
		this.playlist.OnChanged -= this.SavePlaylist;
	}

	// Token: 0x040008F7 RID: 2295
	[SerializeField]
	private CustomMusicSoundtrackBrowser browser;

	// Token: 0x040008F8 RID: 2296
	[SerializeField]
	private Sprite defaultIcon;

	// Token: 0x040008F9 RID: 2297
	[SerializeField]
	private Sprite loopSprite;

	// Token: 0x040008FA RID: 2298
	[SerializeField]
	private Sprite loopOnceSprite;

	// Token: 0x040008FB RID: 2299
	[Header("UI Elements")]
	[SerializeField]
	private Image loopModeImage;

	// Token: 0x040008FC RID: 2300
	[SerializeField]
	private Image shuffleImage;

	// Token: 0x040008FD RID: 2301
	[SerializeField]
	private RectTransform selectedControls;

	// Token: 0x040008FE RID: 2302
	[SerializeField]
	private List<Transform> anchors;

	// Token: 0x040008FF RID: 2303
	public Playlist playlist = new Playlist();

	// Token: 0x04000900 RID: 2304
	private Coroutine moveControlsRoutine;

	// Token: 0x04000901 RID: 2305
	private Dictionary<Transform, Coroutine> changeAnchorRoutines = new Dictionary<Transform, Coroutine>();

	// Token: 0x04000902 RID: 2306
	private List<Transform> buttons = new List<Transform>();

	// Token: 0x04000903 RID: 2307
	private Dictionary<Playlist.SongIdentifier, Playlist.SongMetadata> metadataDict = new Dictionary<Playlist.SongIdentifier, Playlist.SongMetadata>();
}
