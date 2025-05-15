using System;
using UnityEngine;

// Token: 0x0200015B RID: 347
[QuickReference(typeof(PCMoviePlayer))]
public interface IMoviePlayer
{
	// Token: 0x14000009 RID: 9
	// (add) Token: 0x0600082A RID: 2090
	// (remove) Token: 0x0600082B RID: 2091
	event Action OnMovieLoopPointReached;

	// Token: 0x0600082C RID: 2092
	GameObject Play(string cameraName, string moviePath);

	// Token: 0x0600082D RID: 2093
	void Pause();

	// Token: 0x0600082E RID: 2094
	void Unpause();

	// Token: 0x0600082F RID: 2095
	void Stop();

	// Token: 0x06000830 RID: 2096
	void SetSpeed(float speed);
}
