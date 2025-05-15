using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000140 RID: 320
public class MoviePlayer : HammerEntity
{
	// Token: 0x0600077A RID: 1914 RVA: 0x000265B5 File Offset: 0x000247B5
	public void SetMoviePath(string newMovie)
	{
		this.MoviePath = newMovie;
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x00005444 File Offset: 0x00003644
	private void Awake()
	{
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x000265C0 File Offset: 0x000247C0
	public void Input_PlayMovie()
	{
		string text = this.MoviePath;
		if (PlatformManager.UseLowerFPSVideos && MoviePlayer.sixtyFPSToThirtyFPS.ContainsKey(this.MoviePath))
		{
			text = MoviePlayer.sixtyFPSToThirtyFPS[this.MoviePath];
		}
		string text2 = Path.Combine(Application.streamingAssetsPath, "video", text);
		GameMaster.MoviePlaybackContext moviePlaybackContext = Singleton<GameMaster>.Instance.StartMovie(this.skippable, this, this.CameraObjectName, text2, this.isFullScreenMovie);
		if (this.autoCameraActivation == MoviePlayer.AutoCameraActivation.AutoEnableOnStart || this.autoCameraActivation == MoviePlayer.AutoCameraActivation.AutoEnableOnStartAndDisableOnStop)
		{
			moviePlaybackContext.CameraEnabled = true;
		}
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x00026647 File Offset: 0x00024847
	public void Ended(GameMaster.MoviePlaybackContext context)
	{
		if (this.autoCameraActivation == MoviePlayer.AutoCameraActivation.AutoEnableOnStartAndDisableOnStop)
		{
			context.CameraEnabled = false;
		}
		base.FireOutput(Outputs.OnPlaybackFinished);
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x00026660 File Offset: 0x00024860
	public void Skipped()
	{
		base.FireOutput(Outputs.OnSkip);
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x00026669 File Offset: 0x00024869
	public override void Input_Enable()
	{
		base.Input_Enable();
		if (this.child != null)
		{
			this.child.SetActive(true);
		}
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x0002668B File Offset: 0x0002488B
	public override void Input_Disable()
	{
		base.Input_Disable();
		if (this.child != null)
		{
			this.child.SetActive(false);
		}
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x000266BC File Offset: 0x000248BC
	// Note: this type is marked as 'beforefieldinit'.
	static MoviePlayer()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["bucketchoice1.mp4"] = "bucketchoice1_30fps.mp4";
		dictionary["bucketchoice2.mp4"] = "bucketchoice2_30fps.mp4";
		dictionary["intro.mp4"] = "intro_30fps.mp4";
		dictionary["intro_balloon1.mp4"] = "intro_balloon1_30fps.mp4";
		dictionary["intro_balloon2.mp4"] = "intro_balloon2_30fps.mp4";
		dictionary["intro_reverse.mp4"] = "intro_reverse_30fps.mp4";
		dictionary["sillybirds.mp4"] = "sillybirds_30fps_5mbps.mp4";
		MoviePlayer.sixtyFPSToThirtyFPS = dictionary;
	}

	// Token: 0x0400079F RID: 1951
	public bool skippable;

	// Token: 0x040007A0 RID: 1952
	public bool loop;

	// Token: 0x040007A1 RID: 1953
	public string MoviePath;

	// Token: 0x040007A2 RID: 1954
	public string CameraObjectName;

	// Token: 0x040007A3 RID: 1955
	[HideInInspector]
	public GameObject child;

	// Token: 0x040007A4 RID: 1956
	public bool isFullScreenMovie = true;

	// Token: 0x040007A5 RID: 1957
	public MoviePlayer.AutoCameraActivation autoCameraActivation;

	// Token: 0x040007A6 RID: 1958
	private static readonly Dictionary<string, string> sixtyFPSToThirtyFPS;

	// Token: 0x020003DB RID: 987
	public enum AutoCameraActivation
	{
		// Token: 0x0400143D RID: 5181
		Manual,
		// Token: 0x0400143E RID: 5182
		AutoEnableOnStart,
		// Token: 0x0400143F RID: 5183
		AutoEnableOnStartAndDisableOnStop
	}
}
