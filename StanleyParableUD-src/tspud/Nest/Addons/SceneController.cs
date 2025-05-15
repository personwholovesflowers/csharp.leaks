using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nest.Addons
{
	// Token: 0x0200024E RID: 590
	public class SceneController : Singleton<SceneController>
	{
		// Token: 0x06000DEF RID: 3567 RVA: 0x0003E801 File Offset: 0x0003CA01
		private void OnEnable()
		{
			SceneManager.sceneUnloaded += this.SceneUnloaded;
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0003E814 File Offset: 0x0003CA14
		private void OnDisable()
		{
			SceneManager.sceneUnloaded -= this.SceneUnloaded;
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0003E828 File Offset: 0x0003CA28
		public void RegisterPostbox(ScenePostbox postbox)
		{
			if (postbox.gameObject.scene.name == this.PauseScene)
			{
				this._pausePostbox = postbox;
			}
			this._postboxes.Add(postbox.name, new SceneController.PostboxProperty
			{
				Postbox = postbox,
				Scene = postbox.gameObject.scene
			});
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x00005444 File Offset: 0x00003644
		public void RemovePostbox(string postboxName)
		{
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0003E88C File Offset: 0x0003CA8C
		public Scene FindPostboxSceneFromName(string postboxName)
		{
			SceneController.PostboxProperty postboxProperty;
			if (!this._postboxes.TryGetValue(postboxName, out postboxProperty))
			{
				return default(Scene);
			}
			return postboxProperty.Scene;
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x0003E8B9 File Offset: 0x0003CAB9
		public void LoadScene(string scene)
		{
			base.StartCoroutine(this.LoadSceneOperation(scene, LoadSceneMode.Additive));
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0003E8CA File Offset: 0x0003CACA
		public void LoadSceneSingle(string scene)
		{
			base.StartCoroutine(this.LoadSceneOperation(scene, LoadSceneMode.Single));
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0003E8DB File Offset: 0x0003CADB
		public void UnloadScene(string scene)
		{
			SceneManager.UnloadScene(scene);
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0003E8E4 File Offset: 0x0003CAE4
		public void SendSceneMessage(string message)
		{
			foreach (KeyValuePair<string, SceneController.PostboxProperty> keyValuePair in this._postboxes)
			{
				if (keyValuePair.Value.Scene.isLoaded && keyValuePair.Value.Postbox.gameObject.activeInHierarchy)
				{
					keyValuePair.Value.Postbox.RecieveEvent(message);
				}
			}
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0003E970 File Offset: 0x0003CB70
		public void SendSceneMessage(string recipient, string message)
		{
			SceneController.PostboxProperty postboxProperty;
			if (this._postboxes.TryGetValue(recipient, out postboxProperty))
			{
				if (postboxProperty.Scene.isLoaded && postboxProperty.Postbox.gameObject.activeInHierarchy)
				{
					postboxProperty.Postbox.RecieveEvent(message);
					return;
				}
			}
			else
			{
				Debug.LogError(string.Format("Could not find the recipient {0}", recipient));
			}
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0003E9C9 File Offset: 0x0003CBC9
		public void SendPauseEvent(string message)
		{
			if (this._pausePostbox == null)
			{
				Debug.LogWarning("Pause postbox not initialised.");
				return;
			}
			this._pausePostbox.RecieveEvent(message);
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0003E9F0 File Offset: 0x0003CBF0
		public void DisplayPause()
		{
			if (this._pauseScene == default(Scene) && (this._pauseScene = SceneManager.GetSceneByName(this.PauseScene)) == default(Scene))
			{
				base.StartCoroutine(this.LoadPause(this.PauseScene));
				return;
			}
			if (this._pausePostbox != null)
			{
				this._pausePostbox.RecieveEvent("Pause");
			}
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0003EA69 File Offset: 0x0003CC69
		private IEnumerator LoadPause(string scene)
		{
			yield return this.LoadSceneOperation(scene, LoadSceneMode.Additive);
			this._pauseScene = SceneManager.GetSceneByName(scene);
			while (!this._pauseScene.isLoaded)
			{
				yield return null;
			}
			this.DisplayPause();
			yield break;
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0003EA7F File Offset: 0x0003CC7F
		private IEnumerator LoadSceneOperation(string scene, LoadSceneMode sceneMode = LoadSceneMode.Additive)
		{
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene, sceneMode);
			if (sceneMode == LoadSceneMode.Single)
			{
				asyncOperation.allowSceneActivation = true;
			}
			if (!asyncOperation.isDone)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0003EA95 File Offset: 0x0003CC95
		private void SceneUnloaded(Scene scene)
		{
			if (!scene.isLoaded)
			{
				Resources.UnloadUnusedAssets();
			}
		}

		// Token: 0x04000C7D RID: 3197
		public string PauseScene = "Pause";

		// Token: 0x04000C7E RID: 3198
		private ScenePostbox _pausePostbox;

		// Token: 0x04000C7F RID: 3199
		private Scene _pauseScene;

		// Token: 0x04000C80 RID: 3200
		private readonly Dictionary<string, SceneController.PostboxProperty> _postboxes = new Dictionary<string, SceneController.PostboxProperty>();

		// Token: 0x02000444 RID: 1092
		public class PostboxProperty
		{
			// Token: 0x040015E7 RID: 5607
			public Scene Scene;

			// Token: 0x040015E8 RID: 5608
			public ScenePostbox Postbox;
		}

		// Token: 0x02000445 RID: 1093
		[Serializable]
		public class Message
		{
			// Token: 0x040015E9 RID: 5609
			public string Recipient;

			// Token: 0x040015EA RID: 5610
			public string Contents;
		}
	}
}
