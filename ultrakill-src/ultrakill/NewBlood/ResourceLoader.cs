using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace NewBlood
{
	// Token: 0x020005E3 RID: 1507
	internal static class ResourceLoader
	{
		// Token: 0x060021C3 RID: 8643 RVA: 0x0010AE98 File Offset: 0x00109098
		public static IEnumerator LoadAudioClip(string path, AudioClipLoadType loadType, Action<AudioClip> onCompleted)
		{
			return ResourceLoader.LoadAudioClip<Action<AudioClip>>(path, loadType, AudioType.UNKNOWN, onCompleted, delegate(Action<AudioClip> _, AudioClip clip)
			{
				onCompleted(clip);
			});
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x0010AECC File Offset: 0x001090CC
		public static IEnumerator LoadAudioClip(string path, AudioClipLoadType loadType, AudioType audioType, Action<AudioClip> onCompleted)
		{
			return ResourceLoader.LoadAudioClip<Action<AudioClip>>(path, loadType, audioType, onCompleted, delegate(Action<AudioClip> _, AudioClip clip)
			{
				onCompleted(clip);
			});
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x0010AF00 File Offset: 0x00109100
		public static IEnumerator LoadAudioClip<TState>(string path, AudioClipLoadType loadType, TState state, Action<TState, AudioClip> onCompleted)
		{
			return ResourceLoader.LoadAudioClip<TState>(path, loadType, AudioType.UNKNOWN, state, onCompleted);
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x0010AF0C File Offset: 0x0010910C
		public static IEnumerator LoadAudioClip<TState>(string path, AudioClipLoadType loadType, AudioType audioType, TState state, Action<TState, AudioClip> onCompleted)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (onCompleted == null)
			{
				throw new ArgumentNullException("onCompleted");
			}
			Uri fileUri = ResourceLoader.GetFileUri(path);
			UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(fileUri, audioType);
			DownloadHandlerAudioClip handler = (DownloadHandlerAudioClip)request.downloadHandler;
			if (loadType != AudioClipLoadType.CompressedInMemory)
			{
				if (loadType == AudioClipLoadType.Streaming)
				{
					handler.streamAudio = true;
				}
			}
			else
			{
				handler.compressed = true;
			}
			UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = request.SendWebRequest();
			ResourceLoader.DisposeAndThrowIfRequestFailed(request);
			if (loadType == AudioClipLoadType.Streaming)
			{
				try
				{
					onCompleted(state, handler.audioClip);
				}
				catch
				{
					request.Dispose();
					throw;
				}
			}
			yield return unityWebRequestAsyncOperation;
			ResourceLoader.DisposeAndThrowIfRequestFailed(request);
			if (loadType == AudioClipLoadType.Streaming)
			{
				request.Dispose();
			}
			else
			{
				using (request)
				{
					onCompleted(state, handler.audioClip);
					yield break;
				}
			}
			yield break;
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x0010AF38 File Offset: 0x00109138
		private static void DisposeAndThrowIfRequestFailed(UnityWebRequest request)
		{
			if (!request.isHttpError || !request.isNetworkError || !request.isDone)
			{
				return;
			}
			object exceptionForWebRequest = ResourceLoader.GetExceptionForWebRequest(request);
			request.Dispose();
			throw exceptionForWebRequest;
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x0010AF5F File Offset: 0x0010915F
		private static Exception GetExceptionForWebRequest(UnityWebRequest request)
		{
			if (request.responseCode == 404L)
			{
				return new FileNotFoundException(null, request.uri.LocalPath);
			}
			return new Exception(request.error);
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x0010AF8C File Offset: 0x0010918C
		private static Uri GetFileUri(string path)
		{
			path = Path.GetFullPath(path);
			path = path.Replace('\\', '/');
			path = Uri.EscapeUriString(path);
			return new UriBuilder(Uri.UriSchemeFile, string.Empty, 0, path).Uri;
		}
	}
}
