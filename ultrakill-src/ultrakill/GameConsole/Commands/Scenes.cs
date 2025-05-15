using System;
using plog;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace GameConsole.Commands
{
	// Token: 0x020005D7 RID: 1495
	public class Scenes : ICommand, IConsoleLogger
	{
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06002190 RID: 8592 RVA: 0x0010A199 File Offset: 0x00108399
		public Logger Log { get; } = new Logger("Scenes");

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06002191 RID: 8593 RVA: 0x0010A1A1 File Offset: 0x001083A1
		public string Name
		{
			get
			{
				return "Scenes";
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06002192 RID: 8594 RVA: 0x0010A1A8 File Offset: 0x001083A8
		public string Description
		{
			get
			{
				return "Lists all scenes.";
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06002193 RID: 8595 RVA: 0x0010A1AF File Offset: 0x001083AF
		public string Command
		{
			get
			{
				return "scenes";
			}
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x0010A1B8 File Offset: 0x001083B8
		public void Execute(Console con, string[] args)
		{
			if (con.CheatBlocker())
			{
				return;
			}
			this.Log.Info("<b>Available Scenes:</b>", null, null, null);
			foreach (IResourceLocation resourceLocation in Addressables.LoadResourceLocationsAsync("Assets/Scenes", null).WaitForCompletion())
			{
				string text = resourceLocation.InternalId;
				if (resourceLocation.InternalId.StartsWith("Assets/Scenes/"))
				{
					text = resourceLocation.InternalId.Substring(14);
				}
				if (resourceLocation.InternalId.EndsWith(".unity"))
				{
					text = text.Substring(0, text.Length - 6);
				}
				this.Log.Info(text + " [<color=orange>" + resourceLocation.InternalId + "</color>]", null, null, null);
			}
		}
	}
}
