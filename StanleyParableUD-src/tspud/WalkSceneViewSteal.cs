using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001CF RID: 463
public class WalkSceneViewSteal : MonoBehaviour
{
	// Token: 0x06000A9F RID: 2719 RVA: 0x00005444 File Offset: 0x00003644
	private void Start()
	{
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x00031908 File Offset: 0x0002FB08
	public void StealView()
	{
		StanleyController.Instance.FreezeMotionAndView();
		if (this.stolen)
		{
			return;
		}
		this.stolen = true;
		this.anim.SetTrigger("WalkStart");
		StanleyController.Instance.currentCam.GetComponentInChildren<BucketController>();
		this.childCam = StanleyController.Instance.currentCam.gameObject;
		this.childCam.transform.position = StanleyController.Instance.currentCam.gameObject.transform.position;
		this.childCam.transform.rotation = StanleyController.Instance.currentCam.gameObject.transform.rotation;
		StanleyController.Instance.FreezeMotionAndView();
		base.StartCoroutine(this.SlowWalkingSpeedMultiplier(0.8f));
		base.StartCoroutine(this.LerpCamera());
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x000319DF File Offset: 0x0002FBDF
	private IEnumerator SlowWalkingSpeedMultiplier(float time)
	{
		float minMovementSpeed = 0.2f;
		float timeRemaining = time;
		while (timeRemaining > 0f)
		{
			float num = timeRemaining / time;
			num = Mathf.InverseLerp(minMovementSpeed, 1f, num);
			StanleyController.Instance.SetMovementSpeedMultiplier(1f / num);
			StanleyController.Instance.Bucket.SetWalkingSpeed(num);
			StanleyController.Instance.Bucket.SetAnimationSpeed(num);
			timeRemaining -= Singleton<GameMaster>.Instance.GameDeltaTime;
			yield return null;
		}
		float num2 = minMovementSpeed;
		StanleyController.Instance.SetMovementSpeedMultiplier(1f / num2);
		StanleyController.Instance.Bucket.SetWalkingSpeed(num2);
		StanleyController.Instance.Bucket.SetAnimationSpeed(num2);
		yield break;
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x000319EE File Offset: 0x0002FBEE
	private IEnumerator LerpCamera()
	{
		Vector3 startPos = this.childCam.transform.position;
		Quaternion startRot = this.childCam.transform.rotation;
		float startTime = Singleton<GameMaster>.Instance.GameTime;
		float endTime = startTime + 2.5f;
		while (Singleton<GameMaster>.Instance.GameTime < endTime)
		{
			float num = Mathf.InverseLerp(startTime, endTime, Singleton<GameMaster>.Instance.GameTime);
			num *= num;
			this.childCam.transform.position = Vector3.Lerp(startPos, base.transform.TransformPoint(Vector3.zero), num);
			this.childCam.transform.rotation = Quaternion.Slerp(startRot, base.transform.rotation, num);
			yield return null;
		}
		base.StartCoroutine(this.FollowCam());
		yield break;
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x000319FD File Offset: 0x0002FBFD
	private IEnumerator FollowCam()
	{
		for (;;)
		{
			this.childCam.transform.position = base.transform.position;
			this.childCam.transform.rotation = base.transform.rotation;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x0001924F File Offset: 0x0001744F
	private void OnDestroy()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x04000A87 RID: 2695
	public Animator anim;

	// Token: 0x04000A88 RID: 2696
	private bool stolen;

	// Token: 0x04000A89 RID: 2697
	private GameObject childCam;
}
