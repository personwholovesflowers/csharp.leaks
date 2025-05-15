using System;
using System.Linq;
using plog;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Sandbox.Arm
{
	// Token: 0x02000572 RID: 1394
	[ConfigureSingleton(SingletonFlags.NoAutoInstance)]
	public class SandboxArm : MonoSingleton<SandboxArm>
	{
		// Token: 0x06001FAE RID: 8110 RVA: 0x00101728 File Offset: 0x000FF928
		protected override void Awake()
		{
			base.Awake();
			this.localIcon = base.GetComponent<WeaponIcon>();
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x0010173C File Offset: 0x000FF93C
		public void SetArmMode(ISandboxArmMode mode)
		{
			this.ResetMode();
			this.currentMode = mode;
			this.currentMode.OnEnable(this);
			if (this.selectSound.isActiveAndEnabled)
			{
				this.selectSound.Play();
			}
			this.ReloadIcon();
			this.ReloadHudIconColor();
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x0010177C File Offset: 0x000FF97C
		public void ReloadIcon()
		{
			if (this.currentMode == null || string.IsNullOrEmpty(this.currentMode.Icon))
			{
				this.holoIconContainer.SetActive(false);
				return;
			}
			this.holoIconContainer.SetActive(true);
			if (MonoSingleton<IconManager>.Instance.CurrentIcons.sandboxArmHoloIcons.Select((CheatAssetObject.KeyIcon e) => e.key).Contains(this.currentMode.Icon))
			{
				this.holoIcon.sprite = MonoSingleton<IconManager>.Instance.CurrentIcons.sandboxArmHoloIcons.First((CheatAssetObject.KeyIcon e) => e.key == this.currentMode.Icon).sprite;
				return;
			}
			this.holoIcon.sprite = MonoSingleton<IconManager>.Instance.CurrentIcons.genericSandboxToolIcon;
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x0010184C File Offset: 0x000FFA4C
		private void ReloadHudIconColor()
		{
			if (this.localIcon == null)
			{
				this.localIcon = base.GetComponent<WeaponIcon>();
			}
			if (this.currentMode is AlterMode)
			{
				this.localIcon.weaponDescriptor = this.alterDescriptor;
			}
			else if (this.currentMode is DestroyMode)
			{
				this.localIcon.weaponDescriptor = this.destroyDescriptor;
			}
			else if (this.currentMode is PlaceMode || this.currentMode is BuildMode)
			{
				this.localIcon.weaponDescriptor = this.buildOrPlaceDescriptor;
			}
			else
			{
				this.localIcon.weaponDescriptor = this.genericDescriptor;
			}
			this.localIcon.UpdateIcon();
		}

		// Token: 0x06001FB2 RID: 8114 RVA: 0x001018FC File Offset: 0x000FFAFC
		public void ResetAnimator()
		{
			if (!this.animator.isActiveAndEnabled)
			{
				return;
			}
			this.animator.SetBool(SandboxArm.Holding, false);
			this.animator.SetBool(SandboxArm.Punch, false);
			this.animator.SetBool(SandboxArm.Manipulating, false);
			this.animator.SetBool(SandboxArm.Pinched, false);
			this.animator.SetBool(SandboxArm.Crush, false);
			this.animator.SetBool(SandboxArm.Point, false);
			this.animator.SetBool(SandboxArm.Tap, false);
		}

		// Token: 0x06001FB3 RID: 8115 RVA: 0x0010198E File Offset: 0x000FFB8E
		public GoreZone GetGoreZone()
		{
			if (!this.goreZone)
			{
				this.goreZone = new GameObject("Debug Gore Zone").AddComponent<GoreZone>();
				SandboxArm.debugZone = this.goreZone;
			}
			return this.goreZone;
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x001019C4 File Offset: 0x000FFBC4
		public void SelectObject(SpawnableObject obj)
		{
			this.ResetMode();
			this.menu.gameObject.SetActive(false);
			MonoSingleton<OptionsManager>.Instance.UnFreeze();
			ArmModeWithHeldPreview armModeWithHeldPreview = this.SetArmMode(obj.spawnableType) as ArmModeWithHeldPreview;
			if (armModeWithHeldPreview != null)
			{
				armModeWithHeldPreview.SetPreview(obj);
			}
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x00101A10 File Offset: 0x000FFC10
		public ISandboxArmMode SetArmMode(SpawnableType type)
		{
			ISandboxArmMode sandboxArmMode = null;
			switch (type)
			{
			case SpawnableType.SimpleSpawn:
			case SpawnableType.Prop:
				sandboxArmMode = new PlaceMode();
				break;
			case SpawnableType.BuildHand:
				sandboxArmMode = new BuildMode();
				break;
			case SpawnableType.MoveHand:
				sandboxArmMode = new MoveMode();
				break;
			case SpawnableType.DestroyHand:
				sandboxArmMode = new DestroyMode();
				break;
			case SpawnableType.AlterHand:
				sandboxArmMode = new AlterMode();
				break;
			}
			this.SetArmMode(sandboxArmMode);
			return sandboxArmMode;
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x00101A6D File Offset: 0x000FFC6D
		protected override void OnEnable()
		{
			base.OnEnable();
			if (this.currentMode == null)
			{
				this.ResetMode();
				this.SetArmMode(this.onEnableType);
				return;
			}
			this.currentMode.OnEnable(this);
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x00101A9D File Offset: 0x000FFC9D
		private void OnDisable()
		{
			ISandboxArmMode sandboxArmMode = this.currentMode;
			if (sandboxArmMode == null)
			{
				return;
			}
			sandboxArmMode.OnDisable();
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x00101AAF File Offset: 0x000FFCAF
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this.ResetMode();
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x00101ABD File Offset: 0x000FFCBD
		public void ResetMode()
		{
			ISandboxArmMode sandboxArmMode = this.currentMode;
			if (sandboxArmMode != null)
			{
				sandboxArmMode.OnDestroy();
			}
			this.currentMode = null;
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x00101AD7 File Offset: 0x000FFCD7
		private void FixedUpdate()
		{
			ISandboxArmMode sandboxArmMode = this.currentMode;
			if (sandboxArmMode == null)
			{
				return;
			}
			sandboxArmMode.FixedUpdate();
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x00101AEC File Offset: 0x000FFCEC
		private void Update()
		{
			if (Time.timeScale == 0f)
			{
				return;
			}
			if (this.menu != null && MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame && (this.currentMode == null || this.currentMode.CanOpenMenu))
			{
				this.menu.gameObject.SetActive(true);
				MonoSingleton<OptionsManager>.Instance.Freeze();
				return;
			}
			if (this.menu == null || !this.menu.gameObject.activeSelf)
			{
				if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame)
				{
					ISandboxArmMode sandboxArmMode = this.currentMode;
					if (sandboxArmMode != null)
					{
						sandboxArmMode.OnPrimaryDown();
					}
				}
				if (MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasCanceledThisFrame)
				{
					ISandboxArmMode sandboxArmMode2 = this.currentMode;
					if (sandboxArmMode2 != null)
					{
						sandboxArmMode2.OnPrimaryUp();
					}
				}
				if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame)
				{
					ISandboxArmMode sandboxArmMode3 = this.currentMode;
					if (sandboxArmMode3 != null)
					{
						sandboxArmMode3.OnSecondaryDown();
					}
				}
				if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasCanceledThisFrame)
				{
					ISandboxArmMode sandboxArmMode4 = this.currentMode;
					if (sandboxArmMode4 != null)
					{
						sandboxArmMode4.OnSecondaryUp();
					}
				}
			}
			ISandboxArmMode sandboxArmMode5 = this.currentMode;
			if (sandboxArmMode5 != null && sandboxArmMode5.Raycast)
			{
				this.hitSomething = Physics.Raycast(this.cameraCtrl.transform.position, this.cameraCtrl.transform.forward, out this.hit, 75f, this.raycastLayers);
			}
			ISandboxArmMode sandboxArmMode6 = this.currentMode;
			if (sandboxArmMode6 == null)
			{
				return;
			}
			sandboxArmMode6.Update();
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x00101C98 File Offset: 0x000FFE98
		public Vector2? GetHolderScreenPosition()
		{
			Vector3 vector = MonoSingleton<CameraController>.Instance.cam.WorldToScreenPoint(this.holder.position);
			if (vector.z < 0f)
			{
				return null;
			}
			return new Vector2?(vector);
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x00101CE4 File Offset: 0x000FFEE4
		private void OnGUI()
		{
			if (!SandboxArmDebug.DebugActive)
			{
				return;
			}
			ISandboxArmDebugGUI sandboxArmDebugGUI = this.currentMode as ISandboxArmDebugGUI;
			if (sandboxArmDebugGUI == null)
			{
				return;
			}
			Vector2? holderScreenPosition = this.GetHolderScreenPosition();
			if (holderScreenPosition == null)
			{
				return;
			}
			float num = holderScreenPosition.Value.x - 150f;
			float num2 = (float)Screen.height - holderScreenPosition.Value.y - 150f;
			GUILayout.BeginArea(new Rect(num, num2, Mathf.Min(300f, (float)Screen.width - num), Mathf.Min(250f, (float)Screen.height - num2)), GUI.skin.box);
			GUI.color = ((this.currentMode == null) ? Color.red : Color.green);
			GUILayout.Label((this.currentMode == null) ? "No mode is active" : this.currentMode.ToString(), Array.Empty<GUILayoutOption>());
			GUI.color = Color.white;
			if (!sandboxArmDebugGUI.OnGUI())
			{
				GUILayout.Label("No debug info is available right now.", Array.Empty<GUILayoutOption>());
			}
			GUILayout.EndArea();
		}

		// Token: 0x04002BD1 RID: 11217
		private const bool DebugLogging = false;

		// Token: 0x04002BD2 RID: 11218
		private static readonly global::plog.Logger Log = new global::plog.Logger("SandboxArm");

		// Token: 0x04002BD3 RID: 11219
		[FormerlySerializedAs("onEnableMode")]
		public SpawnableType onEnableType;

		// Token: 0x04002BD4 RID: 11220
		[HideInInspector]
		public CameraController cameraCtrl;

		// Token: 0x04002BD5 RID: 11221
		public LayerMask raycastLayers;

		// Token: 0x04002BD6 RID: 11222
		public GameObject axisPoint;

		// Token: 0x04002BD7 RID: 11223
		[SerializeField]
		private GameObject spawnEffect;

		// Token: 0x04002BD8 RID: 11224
		public Material previewMaterial;

		// Token: 0x04002BD9 RID: 11225
		public Transform holder;

		// Token: 0x04002BDA RID: 11226
		[FormerlySerializedAs("armAnimator")]
		public Animator animator;

		// Token: 0x04002BDB RID: 11227
		[Space]
		[SerializeField]
		private WeaponDescriptor genericDescriptor;

		// Token: 0x04002BDC RID: 11228
		[SerializeField]
		private WeaponDescriptor alterDescriptor;

		// Token: 0x04002BDD RID: 11229
		[SerializeField]
		private WeaponDescriptor destroyDescriptor;

		// Token: 0x04002BDE RID: 11230
		[SerializeField]
		private WeaponDescriptor buildOrPlaceDescriptor;

		// Token: 0x04002BDF RID: 11231
		[Space]
		public AudioSource tickSound;

		// Token: 0x04002BE0 RID: 11232
		public AudioSource jabSound;

		// Token: 0x04002BE1 RID: 11233
		public AudioSource selectSound;

		// Token: 0x04002BE2 RID: 11234
		public AudioSource freezeSound;

		// Token: 0x04002BE3 RID: 11235
		public AudioSource unfreezeSound;

		// Token: 0x04002BE4 RID: 11236
		public AudioSource destroySound;

		// Token: 0x04002BE5 RID: 11237
		public GameObject genericBreakParticles;

		// Token: 0x04002BE6 RID: 11238
		public GameObject manipulateEffect;

		// Token: 0x04002BE7 RID: 11239
		[Space]
		[SerializeField]
		private Image holoIcon;

		// Token: 0x04002BE8 RID: 11240
		[SerializeField]
		private GameObject holoIconContainer;

		// Token: 0x04002BE9 RID: 11241
		[NonSerialized]
		public SpawnMenu menu;

		// Token: 0x04002BEA RID: 11242
		private GoreZone goreZone;

		// Token: 0x04002BEB RID: 11243
		private bool debugStarted;

		// Token: 0x04002BEC RID: 11244
		private TimeSince timeSinceDebug;

		// Token: 0x04002BED RID: 11245
		[NonSerialized]
		public bool hitSomething;

		// Token: 0x04002BEE RID: 11246
		[NonSerialized]
		public RaycastHit hit;

		// Token: 0x04002BEF RID: 11247
		private WeaponIcon localIcon;

		// Token: 0x04002BF0 RID: 11248
		private bool firstBrushPositionSet;

		// Token: 0x04002BF1 RID: 11249
		private Vector3 firstBlockPos;

		// Token: 0x04002BF2 RID: 11250
		private Vector3 secondBlockPos;

		// Token: 0x04002BF3 RID: 11251
		private Vector3 previousSecondBlockPos;

		// Token: 0x04002BF4 RID: 11252
		public static GoreZone debugZone;

		// Token: 0x04002BF5 RID: 11253
		private ISandboxArmMode currentMode;

		// Token: 0x04002BF6 RID: 11254
		private static readonly int Holding = Animator.StringToHash("Holding");

		// Token: 0x04002BF7 RID: 11255
		private static readonly int Punch = Animator.StringToHash("Punch");

		// Token: 0x04002BF8 RID: 11256
		private static readonly int Manipulating = Animator.StringToHash("Manipulating");

		// Token: 0x04002BF9 RID: 11257
		private static readonly int Pinched = Animator.StringToHash("Pinched");

		// Token: 0x04002BFA RID: 11258
		private static readonly int Crush = Animator.StringToHash("Crush");

		// Token: 0x04002BFB RID: 11259
		private static readonly int PushZ = Animator.StringToHash("PushZ");

		// Token: 0x04002BFC RID: 11260
		private static readonly int Point = Animator.StringToHash("Point");

		// Token: 0x04002BFD RID: 11261
		private static readonly int Tap = Animator.StringToHash("Tap");
	}
}
