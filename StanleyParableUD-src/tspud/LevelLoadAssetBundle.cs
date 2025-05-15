using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000FA RID: 250
public class LevelLoadAssetBundle : MonoBehaviour
{
	// Token: 0x0600060D RID: 1549 RVA: 0x00021900 File Offset: 0x0001FB00
	private void Awake()
	{
		Object.DontDestroyOnLoad(this);
		AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "maps/" + this.mapName));
		if (assetBundle.isStreamedSceneAssetBundle)
		{
			SceneManager.LoadScene(Path.GetFileNameWithoutExtension(assetBundle.GetAllScenePaths()[0]));
		}
	}

	// Token: 0x04000661 RID: 1633
	[SerializeField]
	private string mapName = "map1_ud_master";
}
