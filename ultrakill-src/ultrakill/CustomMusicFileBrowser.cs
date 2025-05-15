using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000162 RID: 354
public class CustomMusicFileBrowser : DirectoryTreeBrowser<FileInfo>
{
	// Token: 0x170000AF RID: 175
	// (get) Token: 0x060006E1 RID: 1761 RVA: 0x0002D42B File Offset: 0x0002B62B
	protected override int maxPageLength
	{
		get
		{
			return 4;
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x060006E2 RID: 1762 RVA: 0x0002D42E File Offset: 0x0002B62E
	protected override IDirectoryTree<FileInfo> baseDirectory
	{
		get
		{
			return new FileDirectoryTree(Path.Combine(Directory.GetParent(Application.dataPath).FullName, "CyberGrind", "Music"), null);
		}
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x0002D454 File Offset: 0x0002B654
	protected override Action BuildLeaf(FileInfo file, int indexInPage)
	{
		if (CustomMusicFileBrowser.extensionTypeDict.ContainsKey(file.Extension.ToLower()))
		{
			GameObject go = Object.Instantiate<GameObject>(this.itemButtonTemplate, this.itemParent, false);
			CustomContentButton component = go.GetComponent<CustomContentButton>();
			component.button.onClick.AddListener(delegate
			{
				int count = this.playlistEditorLogic.playlist.Count;
				int num = this.playlistEditorLogic.PageOf(count);
				this.playlistEditorLogic.playlist.Add(new Playlist.SongIdentifier(file.FullName, Playlist.SongIdentifier.IdentifierType.File));
				this.playlistEditorLogic.SetPage(num);
				this.playlistEditorLogic.Select(count, true);
				this.navigator.GoToNoMenu(this.playlistEditor);
			});
			component.text.text = file.Name;
			component.icon.sprite = this.defaultIcon;
			if (component.costText)
			{
				component.costText.text = "";
			}
			go.SetActive(true);
			return delegate
			{
				Object.Destroy(go);
			};
		}
		return null;
	}

	// Token: 0x040008EA RID: 2282
	[SerializeField]
	private CyberGrindSettingsNavigator navigator;

	// Token: 0x040008EB RID: 2283
	[SerializeField]
	private CustomMusicPlaylistEditor playlistEditorLogic;

	// Token: 0x040008EC RID: 2284
	[SerializeField]
	private GameObject playlistEditor;

	// Token: 0x040008ED RID: 2285
	[SerializeField]
	private GameObject loadingPrefab;

	// Token: 0x040008EE RID: 2286
	[SerializeField]
	private Sprite defaultIcon;

	// Token: 0x040008EF RID: 2287
	private AudioClip selectedClip;

	// Token: 0x040008F0 RID: 2288
	public static Dictionary<string, AudioType> extensionTypeDict = new Dictionary<string, AudioType>
	{
		{
			".wav",
			AudioType.WAV
		},
		{
			".mp3",
			AudioType.MPEG
		},
		{
			".ogg",
			AudioType.OGGVORBIS
		},
		{
			".flac",
			(AudioType)7
		}
	};

	// Token: 0x040008F1 RID: 2289
	private AudioClip currentSong;
}
