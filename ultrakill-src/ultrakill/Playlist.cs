using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

// Token: 0x0200014F RID: 335
[JsonObject(MemberSerialization.OptIn)]
public class Playlist
{
	// Token: 0x17000099 RID: 153
	// (get) Token: 0x060006A2 RID: 1698 RVA: 0x0002CD70 File Offset: 0x0002AF70
	public static DirectoryInfo directory
	{
		get
		{
			return Directory.CreateDirectory(Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Preferences", "Playlists"));
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x060006A3 RID: 1699 RVA: 0x0002CD98 File Offset: 0x0002AF98
	public static string currentPath
	{
		get
		{
			string text = Path.Combine(Playlist.directory.Parent.FullName, "Playlist.json");
			string text2 = Path.Combine(Playlist.directory.FullName, string.Format("slot{0}.json", GameProgressSaver.currentSlot + 1));
			if (File.Exists(text) && !File.Exists(text2))
			{
				File.Move(text, text2);
			}
			return text2;
		}
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x060006A4 RID: 1700 RVA: 0x0002CDFD File Offset: 0x0002AFFD
	public List<Playlist.SongIdentifier> ids
	{
		get
		{
			return this._ids;
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x060006A5 RID: 1701 RVA: 0x0002CE05 File Offset: 0x0002B005
	// (set) Token: 0x060006A6 RID: 1702 RVA: 0x0002CE0D File Offset: 0x0002B00D
	public Playlist.LoopMode loopMode
	{
		get
		{
			return this._loopMode;
		}
		set
		{
			this._loopMode = value;
			Action onChanged = this.OnChanged;
			if (onChanged == null)
			{
				return;
			}
			onChanged();
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x060006A7 RID: 1703 RVA: 0x0002CE26 File Offset: 0x0002B026
	// (set) Token: 0x060006A8 RID: 1704 RVA: 0x0002CE2E File Offset: 0x0002B02E
	public int selected
	{
		get
		{
			return this._selected;
		}
		set
		{
			this._selected = value;
			Action onChanged = this.OnChanged;
			if (onChanged == null)
			{
				return;
			}
			onChanged();
		}
	}

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x060006A9 RID: 1705 RVA: 0x0002CE47 File Offset: 0x0002B047
	// (set) Token: 0x060006AA RID: 1706 RVA: 0x0002CE4F File Offset: 0x0002B04F
	public bool shuffled
	{
		get
		{
			return this._shuffled;
		}
		set
		{
			this._shuffled = value;
			Action onChanged = this.OnChanged;
			if (onChanged == null)
			{
				return;
			}
			onChanged();
		}
	}

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x060006AB RID: 1707 RVA: 0x0002CE68 File Offset: 0x0002B068
	// (remove) Token: 0x060006AC RID: 1708 RVA: 0x0002CEA0 File Offset: 0x0002B0A0
	public event Action OnChanged;

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x060006AD RID: 1709 RVA: 0x0002CED5 File Offset: 0x0002B0D5
	public int Count
	{
		get
		{
			return this._ids.Count;
		}
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x0002CEE2 File Offset: 0x0002B0E2
	public Playlist()
	{
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x0002CF03 File Offset: 0x0002B103
	public Playlist(IEnumerable<Playlist.SongIdentifier> passedIds)
	{
		this._ids.AddRange(passedIds);
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x0002CF30 File Offset: 0x0002B130
	public void Add(Playlist.SongIdentifier id)
	{
		this._ids.Add(id);
		Action onChanged = this.OnChanged;
		if (onChanged == null)
		{
			return;
		}
		onChanged();
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x0002CF50 File Offset: 0x0002B150
	public void Remove(int index)
	{
		if (this._ids.Count <= 1)
		{
			Debug.LogWarning("Attempted to remove last song from playlist!");
			return;
		}
		if (index < 0 && index > this._ids.Count - 1)
		{
			Debug.LogError(string.Format("Attempted to remove index '{0}' from playlist, which is out of bounds. (0..{1})", index, this._ids.Count - 1));
			return;
		}
		this._ids.RemoveAt(index);
		Action onChanged = this.OnChanged;
		if (onChanged == null)
		{
			return;
		}
		onChanged();
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x0002CFD0 File Offset: 0x0002B1D0
	public void Swap(int index1, int index2)
	{
		Playlist.SongIdentifier songIdentifier = this._ids[index1];
		this._ids[index1] = this._ids[index2];
		this._ids[index2] = songIdentifier;
		Action onChanged = this.OnChanged;
		if (onChanged == null)
		{
			return;
		}
		onChanged();
	}

	// Token: 0x040008C3 RID: 2243
	[JsonProperty]
	private List<Playlist.SongIdentifier> _ids = new List<Playlist.SongIdentifier>();

	// Token: 0x040008C4 RID: 2244
	[JsonProperty]
	private Playlist.LoopMode _loopMode = Playlist.LoopMode.LoopOne;

	// Token: 0x040008C5 RID: 2245
	[JsonProperty]
	private int _selected;

	// Token: 0x040008C6 RID: 2246
	[JsonProperty]
	private bool _shuffled = true;

	// Token: 0x02000150 RID: 336
	public enum LoopMode
	{
		// Token: 0x040008C8 RID: 2248
		Loop,
		// Token: 0x040008C9 RID: 2249
		LoopOne
	}

	// Token: 0x02000151 RID: 337
	public class SongMetadata
	{
		// Token: 0x060006B3 RID: 1715 RVA: 0x0002D01F File Offset: 0x0002B21F
		public SongMetadata(string displayName, Sprite icon, int maxClips = 1)
		{
			this.displayName = displayName;
			this.icon = icon;
			this.maxClips = maxClips;
		}

		// Token: 0x040008CA RID: 2250
		public string displayName;

		// Token: 0x040008CB RID: 2251
		public Sprite icon;

		// Token: 0x040008CC RID: 2252
		public int maxClips;
	}

	// Token: 0x02000152 RID: 338
	public class SongIdentifier
	{
		// Token: 0x060006B4 RID: 1716 RVA: 0x0002D03C File Offset: 0x0002B23C
		public SongIdentifier(string id, Playlist.SongIdentifier.IdentifierType type)
		{
			this.path = id;
			this.type = type;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0002D052 File Offset: 0x0002B252
		public static implicit operator Playlist.SongIdentifier(string id)
		{
			return new Playlist.SongIdentifier(id, Playlist.SongIdentifier.IdentifierType.Addressable);
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0002D05C File Offset: 0x0002B25C
		public override bool Equals(object obj)
		{
			Playlist.SongIdentifier songIdentifier = obj as Playlist.SongIdentifier;
			if (((songIdentifier != null) ? songIdentifier.path : null) == this.path)
			{
				Playlist.SongIdentifier.IdentifierType? identifierType = ((songIdentifier != null) ? new Playlist.SongIdentifier.IdentifierType?(songIdentifier.type) : null);
				Playlist.SongIdentifier.IdentifierType identifierType2 = this.type;
				return (identifierType.GetValueOrDefault() == identifierType2) & (identifierType != null);
			}
			return false;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0002D0BE File Offset: 0x0002B2BE
		public override int GetHashCode()
		{
			return (-1056084179 * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.path)) * -1521134295 + this.type.GetHashCode();
		}

		// Token: 0x040008CD RID: 2253
		public string path;

		// Token: 0x040008CE RID: 2254
		public Playlist.SongIdentifier.IdentifierType type;

		// Token: 0x02000153 RID: 339
		public enum IdentifierType
		{
			// Token: 0x040008D0 RID: 2256
			Addressable,
			// Token: 0x040008D1 RID: 2257
			File
		}
	}
}
