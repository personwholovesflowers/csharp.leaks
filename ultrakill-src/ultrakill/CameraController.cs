using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

// Token: 0x0200009E RID: 158
[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
public class CameraController : MonoSingleton<CameraController>
{
	// Token: 0x06000301 RID: 769 RVA: 0x00011AEC File Offset: 0x0000FCEC
	protected override void Awake()
	{
		this.audmix = new AudioMixer[]
		{
			MonoSingleton<AudioMixerController>.Instance.allSound,
			MonoSingleton<AudioMixerController>.Instance.goreSound,
			MonoSingleton<AudioMixerController>.Instance.musicSound,
			MonoSingleton<AudioMixerController>.Instance.doorSound,
			MonoSingleton<AudioMixerController>.Instance.unfreezeableSound
		};
		base.Awake();
		this.pm = MonoSingleton<NewMovement>.Instance;
		this.player = this.pm.gameObject;
	}

	// Token: 0x06000302 RID: 770 RVA: 0x00011B68 File Offset: 0x0000FD68
	private void Start()
	{
		this.cam = base.GetComponent<Camera>();
		if (MonoSingleton<StatsManager>.Instance)
		{
			this.asscon = MonoSingleton<AssistController>.Instance;
		}
		this.originalPos = base.transform.localPosition;
		this.defaultPos = base.transform.localPosition;
		this.defaultTarget = base.transform.localPosition;
		this.targetPos = new Vector3(this.defaultPos.x, this.defaultPos.y - 0.2f, this.defaultPos.z);
		float num = MonoSingleton<PrefsManager>.Instance.GetFloat("fieldOfView", 0f);
		if (this.platformerCamera)
		{
			num = 105f;
		}
		this.cam.fieldOfView = num;
		this.defaultFov = this.cam.fieldOfView;
		this.tilt = MonoSingleton<PrefsManager>.Instance.GetBool("cameraTilt", false);
		if (this.opm == null && MonoSingleton<StatsManager>.Instance && MonoSingleton<OptionsManager>.Instance)
		{
			this.opm = MonoSingleton<OptionsManager>.Instance;
		}
		AudioMixer[] array = this.audmix;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetFloat("allPitch", 1f);
		}
		this.CheckAspectRatio();
		this.CheckMouseReverse();
		this.environmentMask = LayerMaskDefaults.Get(LMD.Environment);
	}

	// Token: 0x06000303 RID: 771 RVA: 0x00011CC4 File Offset: 0x0000FEC4
	protected override void OnEnable()
	{
		if (MonoSingleton<OptionsManager>.Instance.frozen || MonoSingleton<OptionsManager>.Instance.paused)
		{
			MonoSingleton<CameraController>.Instance.activated = true;
			this.activated = false;
		}
		base.OnEnable();
		this.CheckAspectRatio();
		this.CheckMouseReverse();
		float num = MonoSingleton<PrefsManager>.Instance.GetFloat("fieldOfView", 0f);
		if (this.platformerCamera)
		{
			num = 105f;
		}
		this.cam.fieldOfView = num;
		this.defaultFov = this.cam.fieldOfView;
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Combine(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000304 RID: 772 RVA: 0x00011D6D File Offset: 0x0000FF6D
	private void OnDisable()
	{
		PrefsManager.onPrefChanged = (Action<string, object>)Delegate.Remove(PrefsManager.onPrefChanged, new Action<string, object>(this.OnPrefChanged));
	}

	// Token: 0x06000305 RID: 773 RVA: 0x00011D90 File Offset: 0x0000FF90
	private void OnPrefChanged(string key, object value)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(key);
		if (num <= 1345518424U)
		{
			if (num != 505642131U)
			{
				if (num != 884218541U)
				{
					if (num != 1345518424U)
					{
						return;
					}
					if (!(key == "fullscreen"))
					{
						return;
					}
				}
				else
				{
					if (!(key == "cameraTilt"))
					{
						return;
					}
					if (value is bool)
					{
						bool flag = (bool)value;
						this.tilt = flag;
						return;
					}
					return;
				}
			}
			else
			{
				if (!(key == "fieldOfView"))
				{
					return;
				}
				if (value is float)
				{
					float num2 = (float)value;
					this.defaultFov = num2;
					this.cam.fieldOfView = num2;
					return;
				}
				return;
			}
		}
		else
		{
			if (num <= 4052293553U)
			{
				if (num != 4035515934U)
				{
					if (num != 4052293553U)
					{
						return;
					}
					if (!(key == "mouseReverseY"))
					{
						return;
					}
				}
				else if (!(key == "mouseReverseX"))
				{
					return;
				}
				this.CheckMouseReverse();
				return;
			}
			if (num != 4173721633U)
			{
				if (num != 4210291440U)
				{
					return;
				}
				if (!(key == "resolutionHeight"))
				{
					return;
				}
			}
			else if (!(key == "resolutionWidth"))
			{
				return;
			}
		}
		this.CheckAspectRatio();
	}

	// Token: 0x06000306 RID: 774 RVA: 0x00011E9C File Offset: 0x0001009C
	private void Update()
	{
		this.CheckAspectRatio();
		if (Input.GetKeyDown(KeyCode.F1) && Debug.isDebugBuild)
		{
			if (Cursor.lockState != CursorLockMode.Locked)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
		if (this.cameraShaking > 0f)
		{
			if (MonoSingleton<OptionsManager>.Instance && MonoSingleton<OptionsManager>.Instance.paused)
			{
				base.transform.localPosition = this.defaultPos;
			}
			else
			{
				Vector3 vector = base.transform.parent.position + this.defaultPos;
				Vector3 vector2 = vector;
				if (this.cameraShaking > 1f)
				{
					vector2 += base.transform.right * (float)Random.Range(-1, 2);
					vector2 += base.transform.up * (float)Random.Range(-1, 2);
				}
				else
				{
					vector2 += base.transform.right * (this.cameraShaking * Random.Range(-1f, 1f));
					vector2 += base.transform.up * (this.cameraShaking * Random.Range(-1f, 1f));
				}
				RaycastHit raycastHit;
				if (Physics.Raycast(vector, vector2 - vector, out raycastHit, Vector3.Distance(vector2, vector) + 0.4f, this.environmentMask))
				{
					base.transform.position = raycastHit.point - (vector2 - vector).normalized * 0.5f;
				}
				else
				{
					base.transform.position = vector2;
				}
				this.cameraShaking -= Time.unscaledDeltaTime * 3f;
			}
		}
		if (this.platformerCamera)
		{
			return;
		}
		if (this.player == null)
		{
			this.player = this.pm.gameObject;
		}
		this.scroll = Input.GetAxis("Mouse ScrollWheel");
		bool flag = this.activated;
		if (MonoSingleton<InputManager>.Instance.LastButtonDevice is Gamepad && this.gamepadFreezeCount > 0)
		{
			flag = false;
		}
		if (GameStateManager.Instance.CameraLocked)
		{
			flag = false;
		}
		if (flag)
		{
			float num = 1f;
			Vector2 vector3 = MonoSingleton<InputManager>.Instance.InputSource.Look.ReadValue<Vector2>();
			if (this.zooming)
			{
				num = this.cam.fieldOfView / this.defaultFov;
			}
			if (!this.reverseY)
			{
				this.rotationX += vector3.y * (this.opm.mouseSensitivity / 10f) * num;
			}
			else
			{
				this.rotationX -= vector3.y * (this.opm.mouseSensitivity / 10f) * num;
			}
			if (!this.reverseX)
			{
				this.rotationY += vector3.x * (this.opm.mouseSensitivity / 10f) * num;
			}
			else
			{
				this.rotationY -= vector3.x * (this.opm.mouseSensitivity / 10f) * num;
			}
		}
		if (this.rotationY > 180f)
		{
			this.rotationY -= 360f;
		}
		else if (this.rotationY < -180f)
		{
			this.rotationY += 360f;
		}
		this.rotationX = Mathf.Clamp(this.rotationX, this.minimumX, this.maximumX);
		if (this.zooming)
		{
			this.cam.fieldOfView = Mathf.MoveTowards(this.cam.fieldOfView, this.zoomTarget, Time.deltaTime * 300f);
		}
		else if (this.pm.boost)
		{
			if (this.dodgeDirection == 0)
			{
				this.cam.fieldOfView = this.defaultFov - this.defaultFov / 20f;
			}
			else if (this.dodgeDirection == 1)
			{
				this.cam.fieldOfView = this.defaultFov + this.defaultFov / 10f;
			}
		}
		else
		{
			this.cam.fieldOfView = Mathf.MoveTowards(this.cam.fieldOfView, this.defaultFov, Time.deltaTime * 300f);
		}
		if (this.hudCamera)
		{
			if (this.zooming)
			{
				this.hudCamera.fieldOfView = Mathf.MoveTowards(this.hudCamera.fieldOfView, this.zoomTarget, Time.deltaTime * 300f);
			}
			else if (this.hudCamera.fieldOfView != 90f)
			{
				this.hudCamera.fieldOfView = Mathf.MoveTowards(this.hudCamera.fieldOfView, 90f, Time.deltaTime * 300f);
			}
		}
		if (flag)
		{
			this.player.transform.localEulerAngles = new Vector3(0f, this.rotationY, 0f);
		}
		float num2 = this.movementHor * -1f;
		float num3 = base.transform.localEulerAngles.z;
		if (num3 > 180f)
		{
			num3 -= 360f;
		}
		float num4;
		if (this.tilt)
		{
			if (!this.pm.boost)
			{
				num4 = Mathf.MoveTowards(num3, num2, Time.deltaTime * 25f * (Mathf.Abs(num3 - num2) + 0.01f));
			}
			else
			{
				num4 = Mathf.MoveTowards(num3, num2 * 5f, Time.deltaTime * 100f * (Mathf.Abs(num3 - num2 * 5f) + 0.01f));
			}
		}
		else
		{
			num4 = Mathf.MoveTowards(num3, 0f, Time.deltaTime * 25f * (Mathf.Abs(num3) + 0.01f));
		}
		if (flag)
		{
			base.transform.localEulerAngles = new Vector3(-this.rotationX, 0f, num4);
		}
		if (this.defaultPos != this.defaultTarget)
		{
			this.defaultPos = Vector3.MoveTowards(this.defaultPos, this.defaultTarget, ((this.defaultTarget - this.defaultPos).magnitude + 0.5f) * Time.deltaTime * 10f);
		}
		if (this.pm.activated && this.cameraShaking <= 0f)
		{
			if (this.pm.walking && this.pm.standing && this.defaultPos == this.defaultTarget)
			{
				base.transform.localPosition = new Vector3(Mathf.MoveTowards(base.transform.localPosition.x, this.targetPos.x, Time.deltaTime * 0.5f), Mathf.MoveTowards(base.transform.localPosition.y, this.targetPos.y, Time.deltaTime * 0.5f * (Mathf.Min(this.pm.rb.velocity.magnitude, 15f) / 15f)), Mathf.MoveTowards(base.transform.localPosition.z, this.targetPos.z, Time.deltaTime * 0.5f));
				if (base.transform.localPosition == this.targetPos && this.targetPos != this.defaultPos)
				{
					this.targetPos = this.defaultPos;
					return;
				}
				if (base.transform.localPosition == this.targetPos && this.targetPos == this.defaultPos)
				{
					this.targetPos = new Vector3(this.defaultPos.x, this.defaultPos.y - 0.1f, this.defaultPos.z);
					return;
				}
			}
			else
			{
				base.transform.localPosition = this.defaultPos;
				this.targetPos = new Vector3(this.defaultPos.x, this.defaultPos.y - 0.1f, this.defaultPos.z);
			}
		}
	}

	// Token: 0x06000307 RID: 775 RVA: 0x000126C6 File Offset: 0x000108C6
	private void FixedUpdate()
	{
		bool flag = this.activated;
	}

	// Token: 0x06000308 RID: 776 RVA: 0x000126D0 File Offset: 0x000108D0
	public void CameraShake(float shakeAmount)
	{
		float @float = MonoSingleton<PrefsManager>.Instance.GetFloat("screenShake", 0f);
		if (@float != 0f && this.cameraShaking < shakeAmount * @float)
		{
			this.cameraShaking = shakeAmount * @float;
		}
	}

	// Token: 0x06000309 RID: 777 RVA: 0x0001270E File Offset: 0x0001090E
	public void StopShake()
	{
		this.cameraShaking = 0f;
		base.transform.localPosition = this.defaultPos;
	}

	// Token: 0x0600030A RID: 778 RVA: 0x0001272C File Offset: 0x0001092C
	public void ResetCamera(float degreesY, float degreesX = 0f)
	{
		this.rotationY = degreesY;
		this.rotationX = degreesX;
	}

	// Token: 0x0600030B RID: 779 RVA: 0x0001273C File Offset: 0x0001093C
	public void Zoom(float amount)
	{
		this.zooming = true;
		this.zoomTarget = amount;
	}

	// Token: 0x0600030C RID: 780 RVA: 0x0001274C File Offset: 0x0001094C
	public void StopZoom()
	{
		this.zooming = false;
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00012758 File Offset: 0x00010958
	public void ResetToDefaultPos()
	{
		base.transform.localPosition = this.defaultPos;
		this.targetPos = new Vector3(this.defaultPos.x, this.defaultPos.y - 0.1f, this.defaultPos.z);
	}

	// Token: 0x0600030E RID: 782 RVA: 0x000127A8 File Offset: 0x000109A8
	public Vector3 GetDefaultPos()
	{
		return base.transform.parent.position + this.defaultPos;
	}

	// Token: 0x0600030F RID: 783 RVA: 0x000127C8 File Offset: 0x000109C8
	public void CheckAspectRatio()
	{
		if (!this.cam)
		{
			this.cam = base.GetComponent<Camera>();
		}
		if (!Mathf.Approximately(this.aspectRatio, this.cam.aspect))
		{
			this.aspectRatio = this.cam.aspect;
			float num = Mathf.Min(this.aspectRatio / 1.778f, 1f);
			if (this.hudCamera)
			{
				this.hudCamera.transform.localScale = new Vector3(num, 1f, 1f);
			}
		}
	}

	// Token: 0x06000310 RID: 784 RVA: 0x0001285B File Offset: 0x00010A5B
	public void CheckMouseReverse()
	{
		this.reverseX = MonoSingleton<PrefsManager>.Instance.GetBool("mouseReverseX", false);
		this.reverseY = MonoSingleton<PrefsManager>.Instance.GetBool("mouseReverseY", false);
	}

	// Token: 0x040003A7 RID: 935
	public bool invert;

	// Token: 0x040003A8 RID: 936
	public float minimumX = -89f;

	// Token: 0x040003A9 RID: 937
	public float maximumX = 89f;

	// Token: 0x040003AA RID: 938
	public float minimumY = -360f;

	// Token: 0x040003AB RID: 939
	public float maximumY = 360f;

	// Token: 0x040003AC RID: 940
	public OptionsManager opm;

	// Token: 0x040003AD RID: 941
	public float scroll;

	// Token: 0x040003AE RID: 942
	public Vector3 defaultTarget;

	// Token: 0x040003AF RID: 943
	public Vector3 originalPos;

	// Token: 0x040003B0 RID: 944
	public Vector3 defaultPos;

	// Token: 0x040003B1 RID: 945
	private Vector3 targetPos;

	// Token: 0x040003B2 RID: 946
	public GameObject player;

	// Token: 0x040003B3 RID: 947
	public NewMovement pm;

	// Token: 0x040003B4 RID: 948
	[HideInInspector]
	public Camera cam;

	// Token: 0x040003B5 RID: 949
	public bool activated;

	// Token: 0x040003B6 RID: 950
	public int gamepadFreezeCount;

	// Token: 0x040003B7 RID: 951
	public float rotationY;

	// Token: 0x040003B8 RID: 952
	public float rotationX;

	// Token: 0x040003B9 RID: 953
	public bool reverseX;

	// Token: 0x040003BA RID: 954
	public bool reverseY;

	// Token: 0x040003BB RID: 955
	public float cameraShaking;

	// Token: 0x040003BC RID: 956
	public float movementHor;

	// Token: 0x040003BD RID: 957
	public float movementVer;

	// Token: 0x040003BE RID: 958
	public int dodgeDirection;

	// Token: 0x040003BF RID: 959
	public float defaultFov;

	// Token: 0x040003C0 RID: 960
	private AudioMixer[] audmix;

	// Token: 0x040003C1 RID: 961
	private bool mouseUnlocked;

	// Token: 0x040003C2 RID: 962
	public bool slide;

	// Token: 0x040003C3 RID: 963
	private AssistController asscon;

	// Token: 0x040003C4 RID: 964
	[SerializeField]
	private GameObject parryLight;

	// Token: 0x040003C5 RID: 965
	[SerializeField]
	private GameObject parryFlash;

	// Token: 0x040003C6 RID: 966
	[SerializeField]
	private Camera hudCamera;

	// Token: 0x040003C7 RID: 967
	private float aspectRatio;

	// Token: 0x040003C8 RID: 968
	private bool pixeled;

	// Token: 0x040003C9 RID: 969
	private bool tilt;

	// Token: 0x040003CA RID: 970
	private float currentStop;

	// Token: 0x040003CB RID: 971
	private bool zooming;

	// Token: 0x040003CC RID: 972
	private float zoomTarget;

	// Token: 0x040003CD RID: 973
	private LayerMask environmentMask;

	// Token: 0x040003CE RID: 974
	public bool platformerCamera;
}
