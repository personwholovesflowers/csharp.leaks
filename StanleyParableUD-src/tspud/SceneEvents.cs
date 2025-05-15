using System;
using Nest.Addons;
using UnityEngine;

// Token: 0x0200007A RID: 122
public class SceneEvents : MonoBehaviour
{
	// Token: 0x060002F0 RID: 752 RVA: 0x00014851 File Offset: 0x00012A51
	public void Send(string message)
	{
		Nest.Addons.Singleton<SceneController>.Instance.SendSceneMessage(message);
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x0001485E File Offset: 0x00012A5E
	public void EnableScene(string sceneName)
	{
		Nest.Addons.Singleton<SceneController>.Instance.LoadScene(sceneName);
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x0001486B File Offset: 0x00012A6B
	public void DisableScene(string sceneName)
	{
		Nest.Addons.Singleton<SceneController>.Instance.UnloadScene(sceneName);
	}
}
