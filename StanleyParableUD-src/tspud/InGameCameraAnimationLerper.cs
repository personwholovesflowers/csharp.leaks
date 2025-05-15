using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000E8 RID: 232
[ExecuteInEditMode]
public class InGameCameraAnimationLerper : MonoBehaviour
{
	// Token: 0x17000070 RID: 112
	// (get) Token: 0x0600059C RID: 1436 RVA: 0x0001F7A9 File Offset: 0x0001D9A9
	private Transform Parent
	{
		get
		{
			if (!Application.isPlaying)
			{
				return this.editorTestCameraParent;
			}
			return StanleyController.Instance.camParent;
		}
	}

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x0600059D RID: 1437 RVA: 0x0001F7C3 File Offset: 0x0001D9C3
	private Camera Camera
	{
		get
		{
			if (!Application.isPlaying)
			{
				return this.editorTestCamera;
			}
			return StanleyController.Instance.cam;
		}
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x0600059E RID: 1438 RVA: 0x0001F7DD File Offset: 0x0001D9DD
	private float OriginalFOV
	{
		get
		{
			if (!Application.isPlaying)
			{
				return this.editorOriginalFOV;
			}
			return StanleyController.Instance.FieldOfViewBase;
		}
	}

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x0600059F RID: 1439 RVA: 0x0001F7F7 File Offset: 0x0001D9F7
	// (set) Token: 0x060005A0 RID: 1440 RVA: 0x0001F816 File Offset: 0x0001DA16
	private float FOV
	{
		get
		{
			if (!Application.isPlaying)
			{
				return this.editorTestCamera.fieldOfView;
			}
			return StanleyController.Instance.FieldOfView;
		}
		set
		{
			if (Application.isPlaying)
			{
				StanleyController.Instance.FieldOfView = value;
				return;
			}
			this.editorTestCamera.fieldOfView = value;
		}
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x0001F837 File Offset: 0x0001DA37
	private void Start()
	{
		if (Application.isPlaying)
		{
			this.isLerping = false;
			this.editorTestCameraParent.gameObject.SetActive(false);
			this.editorTestCamera.gameObject.SetActive(false);
		}
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x0001F86C File Offset: 0x0001DA6C
	private void Update()
	{
		if (this.isLerping)
		{
			this.Camera.transform.position = Vector3.Lerp(this.Parent.TransformPoint(Vector3.zero), this.cameraAnimationTarget.position, this.lerpValue);
			this.Camera.transform.rotation = Quaternion.Slerp(this.Parent.rotation, this.cameraAnimationTarget.rotation, this.lerpValue);
			this.FOV = Mathf.Lerp(this.OriginalFOV, this.targetFOV, this.lerpValue);
		}
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x0001F908 File Offset: 0x0001DB08
	public void StartLerping()
	{
		this.isLerping = true;
		this.lerpValue = 0f;
		StanleyController.Instance.FreezeMotionAndView();
		this.animator.SetTrigger("Do Transition");
		StanleyController.Instance.Bucket.SetAnimationSpeedImmediate(0.25f);
		StanleyController.Instance.Bucket.SetBucket(false);
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x0001F965 File Offset: 0x0001DB65
	public void AnimationEndedEvent()
	{
		UnityEvent unityEvent = this.onAnimationFinished;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x040005DC RID: 1500
	[Range(0f, 1f)]
	public float lerpValue;

	// Token: 0x040005DD RID: 1501
	public bool isLerping;

	// Token: 0x040005DE RID: 1502
	public Animator animator;

	// Token: 0x040005DF RID: 1503
	public Transform cameraAnimationTarget;

	// Token: 0x040005E0 RID: 1504
	public float targetFOV = 72f;

	// Token: 0x040005E1 RID: 1505
	public Camera editorTestCamera;

	// Token: 0x040005E2 RID: 1506
	public Transform editorTestCameraParent;

	// Token: 0x040005E3 RID: 1507
	public float editorOriginalFOV = 72f;

	// Token: 0x040005E4 RID: 1508
	public UnityEvent onAnimationFinished;
}
