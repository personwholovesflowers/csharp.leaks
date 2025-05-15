using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// Token: 0x020000C1 RID: 193
public class FigleyOverlayController : MonoBehaviour
{
	// Token: 0x1700004B RID: 75
	// (set) Token: 0x0600047A RID: 1146 RVA: 0x0001A548 File Offset: 0x00018748
	private bool CameraAndCanvasEnabled
	{
		set
		{
			this.figleyCamera.enabled = value;
			this.figleyCanvas.enabled = value;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x0600047B RID: 1147 RVA: 0x0001A562 File Offset: 0x00018762
	private bool UseBlurredBackground
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x0600047C RID: 1148 RVA: 0x0001A565 File Offset: 0x00018765
	// (set) Token: 0x0600047D RID: 1149 RVA: 0x0001A56C File Offset: 0x0001876C
	public static FigleyOverlayController Instance { get; private set; }

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x0600047E RID: 1150 RVA: 0x0001A574 File Offset: 0x00018774
	public int FiglysFound
	{
		get
		{
			int num = 0;
			IntConfigurable[] array = this.figleyCountCounfigurable;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].GetIntValue() != -1)
				{
					num++;
				}
			}
			return num;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x0600047F RID: 1151 RVA: 0x0001A5A8 File Offset: 0x000187A8
	public int PostFiglysFound
	{
		get
		{
			int num = 0;
			StringConfigurable[] array = this.figleyPostCollectionCountConfigurables;
			for (int i = 0; i < array.Length; i++)
			{
				char[] postCollectableCharArray = FigleyCollectable.GetPostCollectableCharArray(array[i], 0);
				for (int j = 0; j < postCollectableCharArray.Length; j++)
				{
					if (postCollectableCharArray[j] == FigleyCollectable.CollectedChar)
					{
						num++;
					}
				}
			}
			return num;
		}
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x0001A5F8 File Offset: 0x000187F8
	private void Awake()
	{
		FigleyOverlayController.Instance = this;
		this.CameraAndCanvasEnabled = false;
		IntConfigurable[] array = this.figleyCountCounfigurable;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Init();
		}
		this.figleyInitiatorFoundCounfigurable.Init();
		StringConfigurable[] array2 = this.figleyPostCollectionCountConfigurables;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Init();
		}
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x0001A657 File Offset: 0x00018857
	private void Start()
	{
		GameMaster.OnPrepareLoadingLevel += this.ForceStop;
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x0001A66A File Offset: 0x0001886A
	private void ForceStop()
	{
		base.StopAllCoroutines();
		this.FigleyHideFinished();
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x0001A678 File Offset: 0x00018878
	public void StartFigleyCollectionRoutine()
	{
		this.simpleRotate.enabled = true;
		this.blurredBackgroundImage.enabled = false;
		this.count = this.FiglysFound;
		this.count %= 6;
		this.ShowCount(this.count);
		this.collectionNarrations[this.count].Input_Start();
		StanleyController.Instance.viewFrozen = true;
		StanleyController.Instance.motionFrozen = true;
		StanleyController.Instance.ResetVelocity();
		base.StartCoroutine(this.SetAndBlurBackground(this.UseBlurredBackground));
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x0001A708 File Offset: 0x00018908
	public void StartFigleyPostCollection()
	{
		this.simpleRotate.ResetToStartRotation();
		this.CameraAndCanvasEnabled = true;
		this.blurredBackgroundImage.enabled = false;
		this.count = 6 + this.PostFiglysFound;
		this.ShowCount(this.count);
		this.pickupSFX.Play();
		this.figleyAnimator.SetTrigger("Instant Reveal");
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x0001A768 File Offset: 0x00018968
	private IEnumerator SetAndBlurBackground(bool useBlurredBackground)
	{
		yield return null;
		this.figleyRotation.ResetToStartRotation();
		this.figleyAnimator.SetTrigger("Reveal");
		if (useBlurredBackground)
		{
			yield return new WaitForEndOfFrame();
			this.renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
			ScreenCapture.CaptureScreenshotIntoRenderTexture(this.renderTexture);
			AsyncGPUReadback.Request(this.renderTexture, 0, TextureFormat.RGB24, new Action<AsyncGPUReadbackRequest>(this.ReadbackCompleted));
		}
		else
		{
			this.CameraAndCanvasEnabled = true;
			this.blurredBackgroundImage.enabled = true;
		}
		yield break;
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x0001A77E File Offset: 0x0001897E
	private void ReadbackCompleted(AsyncGPUReadbackRequest request)
	{
		this.bgMaterial.mainTexture = this.renderTexture;
		base.StartCoroutine(this.BlurBackground());
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x0001A79E File Offset: 0x0001899E
	private IEnumerator BlurBackground()
	{
		this.CameraAndCanvasEnabled = true;
		this.blurredBackgroundImage.enabled = true;
		float startTime = Time.realtimeSinceStartup;
		float endTime = startTime + 2.5f;
		while (Time.realtimeSinceStartup < endTime)
		{
			float num = Mathf.InverseLerp(startTime, endTime, Time.realtimeSinceStartup);
			num += 0.2f;
			num /= 1.2f;
			this.bgMaterial.SetFloat("_Strength", num * 10f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x0001A7AD File Offset: 0x000189AD
	[ContextMenu("Complete Narration")]
	public void NarrationComplete()
	{
		this.figleyAnimator.SetTrigger("Hide");
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x0001A7BF File Offset: 0x000189BF
	private void ShowCount(int c)
	{
		this.countUI.text = c + "/6";
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x0001A7DC File Offset: 0x000189DC
	public void FigleyPop()
	{
		this.ShowCount(this.count + 1);
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x0001A7EC File Offset: 0x000189EC
	public void FigleySFX()
	{
		this.obnoxiousSFX.Play();
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x0001A7F9 File Offset: 0x000189F9
	public void ImpactSFX()
	{
		this.impactSFX.Play();
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x0001A806 File Offset: 0x00018A06
	public void SwishSFX()
	{
		this.swishSFX.Play();
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x0001A813 File Offset: 0x00018A13
	public void PickupSmallSFX()
	{
		this.pickupSFX.Play();
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x0001A820 File Offset: 0x00018A20
	public void FigleyDisableCamera()
	{
		this.CameraAndCanvasEnabled = false;
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x0001A829 File Offset: 0x00018A29
	public void FigleyHideFinished()
	{
		StanleyController.Instance.viewFrozen = false;
		StanleyController.Instance.motionFrozen = false;
		this.CameraAndCanvasEnabled = false;
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x0001A820 File Offset: 0x00018A20
	public void FigleyInstantHideFinished()
	{
		this.CameraAndCanvasEnabled = false;
	}

	// Token: 0x0400044C RID: 1100
	[InspectorButton("StartFigleyCollectionRoutine", null)]
	[SerializeField]
	private Animator figleyAnimator;

	// Token: 0x0400044D RID: 1101
	[SerializeField]
	private RawImage blurredBackgroundImage;

	// Token: 0x0400044E RID: 1102
	[InspectorButton("ForceStop", null)]
	[SerializeField]
	private Camera figleyCamera;

	// Token: 0x0400044F RID: 1103
	[SerializeField]
	private TextMeshProUGUI countUI;

	// Token: 0x04000450 RID: 1104
	[SerializeField]
	private ChoreographedScene[] collectionNarrations;

	// Token: 0x04000451 RID: 1105
	[SerializeField]
	private IntConfigurable[] figleyCountCounfigurable;

	// Token: 0x04000452 RID: 1106
	[SerializeField]
	private StringConfigurable[] figleyPostCollectionCountConfigurables;

	// Token: 0x04000453 RID: 1107
	[InspectorButton("StartFigleyPostCollection", null)]
	[SerializeField]
	private BooleanConfigurable figleyInitiatorFoundCounfigurable;

	// Token: 0x04000454 RID: 1108
	[SerializeField]
	private AudioSource pickupSFX;

	// Token: 0x04000455 RID: 1109
	[SerializeField]
	private AudioSource obnoxiousSFX;

	// Token: 0x04000456 RID: 1110
	[SerializeField]
	private AudioSource impactSFX;

	// Token: 0x04000457 RID: 1111
	[SerializeField]
	private AudioSource swishSFX;

	// Token: 0x04000458 RID: 1112
	[SerializeField]
	private SimpleRotate simpleRotate;

	// Token: 0x04000459 RID: 1113
	private RenderTexture renderTexture;

	// Token: 0x0400045A RID: 1114
	[SerializeField]
	private Material bgMaterial;

	// Token: 0x0400045B RID: 1115
	[SerializeField]
	private SimpleRotate figleyRotation;

	// Token: 0x0400045C RID: 1116
	[SerializeField]
	private Canvas figleyCanvas;

	// Token: 0x0400045D RID: 1117
	private int count;
}
