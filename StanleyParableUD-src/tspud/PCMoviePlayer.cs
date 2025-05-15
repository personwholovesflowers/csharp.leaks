using System;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x02000157 RID: 343
public class PCMoviePlayer : IMoviePlayer
{
	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06000810 RID: 2064 RVA: 0x00027A48 File Offset: 0x00025C48
	// (remove) Token: 0x06000811 RID: 2065 RVA: 0x00027A80 File Offset: 0x00025C80
	public event Action OnMovieLoopPointReached;

	// Token: 0x06000812 RID: 2066 RVA: 0x00027AB5 File Offset: 0x00025CB5
	public void SetSpeed(float speed)
	{
		if (this.videoPlayer != null && this.videoPlayer.canSetPlaybackSpeed)
		{
			this.videoPlayer.playbackSpeed = speed;
		}
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x00027AE0 File Offset: 0x00025CE0
	public GameObject Play(string cameraName, string path)
	{
		this.videoPlayerGameObject = GameObject.Find(cameraName);
		if (this.videoPlayerGameObject == null)
		{
			return null;
		}
		this.videoPlayer = this.videoPlayerGameObject.GetComponent<VideoPlayer>();
		if (this.videoPlayer == null)
		{
			this.videoPlayer = this.videoPlayerGameObject.AddComponent<VideoPlayer>();
			this.videoPlayer.aspectRatio = VideoAspectRatio.FitInside;
			this.videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
		}
		if (path == this.videoPlayer.url)
		{
			Debug.Log("[PCMoviePlayer] Tried playing same video again. Restarting it." + path);
			this.videoPlayer.Play();
			return this.videoPlayerGameObject;
		}
		this.videoPlayer.loopPointReached -= this.BroadcastLoopPointReached;
		this.videoPlayer.loopPointReached += this.BroadcastLoopPointReached;
		Debug.Log("PRE LINUX BLOCK");
		Debug.Log("video->video_webm path=" + path);
		Debug.Log("videoPlayer.url = path=" + path);
		this.videoPlayer.url = path;
		this.videoPlayer.Prepare();
		this.videoPlayer.Play();
		return this.videoPlayerGameObject;
	}

	// Token: 0x06000814 RID: 2068 RVA: 0x00027C06 File Offset: 0x00025E06
	public void Pause()
	{
		if (this.videoPlayerGameObject != null)
		{
			this.videoPlayer.Pause();
		}
	}

	// Token: 0x06000815 RID: 2069 RVA: 0x00027C21 File Offset: 0x00025E21
	public void Unpause()
	{
		if (this.videoPlayerGameObject != null)
		{
			this.videoPlayer.Play();
		}
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x00027C3C File Offset: 0x00025E3C
	public void Stop()
	{
		if (this.videoPlayerGameObject != null)
		{
			this.videoPlayer.Stop();
		}
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x00027C57 File Offset: 0x00025E57
	private void BroadcastLoopPointReached(VideoPlayer source)
	{
		Action onMovieLoopPointReached = this.OnMovieLoopPointReached;
		if (onMovieLoopPointReached == null)
		{
			return;
		}
		onMovieLoopPointReached();
	}

	// Token: 0x04000822 RID: 2082
	private VideoPlayer videoPlayer;

	// Token: 0x04000823 RID: 2083
	private GameObject videoPlayerGameObject;
}
