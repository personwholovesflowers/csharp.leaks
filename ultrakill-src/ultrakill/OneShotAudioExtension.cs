using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000321 RID: 801
public static class OneShotAudioExtension
{
	// Token: 0x0600127B RID: 4731 RVA: 0x00094204 File Offset: 0x00092404
	public static AudioSource PlayClipAtPoint(this AudioClip clip, AudioMixerGroup mixGroup, Vector3 position, int priority = 128, float spatialBlend = 0f, float volume = 1f, float pitch = 1f, AudioRolloffMode rolloffMode = AudioRolloffMode.Linear, float minimumDistance = 1f, float maximumDistance = 100f)
	{
		GameObject gameObject = new GameObject("TempAudio");
		gameObject.transform.position = position;
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.outputAudioMixerGroup = mixGroup;
		audioSource.priority = priority;
		audioSource.volume = volume;
		audioSource.pitch = pitch;
		audioSource.spatialBlend = spatialBlend;
		audioSource.rolloffMode = rolloffMode;
		audioSource.minDistance = minimumDistance;
		audioSource.maxDistance = maximumDistance;
		audioSource.Play();
		Object.Destroy(gameObject, clip.length / pitch);
		return audioSource;
	}
}
