using System;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x020003EE RID: 1006
public class SetVideoFilePath : MonoBehaviour
{
	// Token: 0x0600169F RID: 5791 RVA: 0x000B59AF File Offset: 0x000B3BAF
	private void OnEnable()
	{
		if (SceneHelper.IsPlayingCustom)
		{
			return;
		}
		base.GetComponent<VideoPlayer>().url = "file://" + Path.Combine(Application.streamingAssetsPath, "Videos", this.videoName);
	}

	// Token: 0x04001F3B RID: 7995
	[SerializeField]
	private string videoName;
}
