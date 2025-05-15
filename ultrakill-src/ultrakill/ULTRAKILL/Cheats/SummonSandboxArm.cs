using System;
using System.Collections.Generic;
using Sandbox.Arm;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ULTRAKILL.Cheats
{
	// Token: 0x0200062E RID: 1582
	public class SummonSandboxArm : ICheat
	{
		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x0600241C RID: 9244 RVA: 0x0010DDC5 File Offset: 0x0010BFC5
		public static List<GameObject> armSlot
		{
			get
			{
				return MonoSingleton<GunControl>.Instance.slot6;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x0600241D RID: 9245 RVA: 0x0010DDD1 File Offset: 0x0010BFD1
		public string LongName
		{
			get
			{
				return "Spawner Arm";
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x0600241E RID: 9246 RVA: 0x0010DDD8 File Offset: 0x0010BFD8
		public string Identifier
		{
			get
			{
				return "ultrakill.spawner-arm";
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x0600241F RID: 9247 RVA: 0x0010DDDF File Offset: 0x0010BFDF
		public string ButtonEnabledOverride
		{
			get
			{
				return "REMOVE";
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06002420 RID: 9248 RVA: 0x0010DDE6 File Offset: 0x0010BFE6
		public string ButtonDisabledOverride
		{
			get
			{
				return "EQUIP";
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06002421 RID: 9249 RVA: 0x0010DDED File Offset: 0x0010BFED
		public string Icon
		{
			get
			{
				return "spawner-arm";
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06002422 RID: 9250 RVA: 0x0010DDF4 File Offset: 0x0010BFF4
		public bool IsActive
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06002423 RID: 9251 RVA: 0x0010DDFC File Offset: 0x0010BFFC
		public bool DefaultState { get; }

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06002424 RID: 9252 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.NotPersistent;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06002425 RID: 9253 RVA: 0x0010DE04 File Offset: 0x0010C004
		public static bool AnyArmActive
		{
			get
			{
				return MonoSingleton<GunControl>.Instance && MonoSingleton<GunControl>.Instance.currentWeapon && MonoSingleton<GunControl>.Instance.currentWeapon.GetComponent<SandboxArm>();
			}
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x0010DE3C File Offset: 0x0010C03C
		public void Enable(CheatsManager manager)
		{
			if (MonoSingleton<CheatsManager>.Instance.GetCheatState("ultrakill.clash-mode"))
			{
				return;
			}
			if (!this.active && this.createdArms.Count > 0)
			{
				this.DeleteAllArms();
			}
			this.active = true;
			SpawnMenu componentInChildren = MonoSingleton<CanvasController>.Instance.GetComponentInChildren<SpawnMenu>(true);
			foreach (SpawnableType[] array2 in this.mainArmTypes)
			{
				SandboxArm sandboxArm = this.CreateArm(array2[0]);
				this.createdArms.Add(sandboxArm);
				sandboxArm.menu = componentInChildren;
				foreach (SpawnableType spawnableType in array2)
				{
					this.spawnedArmMap[spawnableType] = sandboxArm;
				}
			}
			if (componentInChildren)
			{
				componentInChildren.armManager = this;
			}
			this.TryCreateArmType(SpawnableType.MoveHand);
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x0010DF08 File Offset: 0x0010C108
		public void TryCreateArmType(SpawnableType type)
		{
			SandboxArm sandboxArm;
			SandboxArm sandboxArm2;
			if (this.spawnedArmMap.TryGetValue(type, out sandboxArm))
			{
				sandboxArm2 = sandboxArm;
			}
			else
			{
				sandboxArm2 = this.CreateArm(type);
				this.createdArms.Add(sandboxArm2);
				this.spawnedArmMap[type] = sandboxArm2;
			}
			if (MonoSingleton<GunControl>.Instance)
			{
				MonoSingleton<GunControl>.Instance.ForceWeapon(sandboxArm2.gameObject, MonoSingleton<GunControl>.Instance.activated);
			}
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x0010DF70 File Offset: 0x0010C170
		private SandboxArm CreateArm(SpawnableType type)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(MonoSingleton<CheatsController>.Instance.spawnerArm, MonoSingleton<GunControl>.Instance.transform);
			GameObject gameObject2 = gameObject;
			gameObject2.name = gameObject2.name + " - " + type.ToString();
			SummonSandboxArm.armSlot.Add(gameObject);
			MonoSingleton<GunControl>.Instance.UpdateWeaponList(false);
			SandboxArm component = gameObject.GetComponent<SandboxArm>();
			component.cameraCtrl = MonoSingleton<CameraController>.Instance;
			component.onEnableType = type;
			component.SetArmMode(type);
			return component;
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x0010DFF0 File Offset: 0x0010C1F0
		public void SelectArm(SpawnableObject obj)
		{
			SandboxArm sandboxArm = null;
			SandboxArm sandboxArm2;
			if (this.spawnedArmMap.TryGetValue(obj.spawnableType, out sandboxArm2))
			{
				sandboxArm = sandboxArm2;
				if (sandboxArm)
				{
					MonoSingleton<GunControl>.Instance.ForceWeapon(sandboxArm.gameObject, true);
				}
			}
			if (sandboxArm == null)
			{
				sandboxArm = MonoSingleton<GunControl>.Instance.currentWeapon.GetComponent<SandboxArm>();
			}
			if (sandboxArm)
			{
				sandboxArm.SelectObject(obj);
			}
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x0010E058 File Offset: 0x0010C258
		public void Disable()
		{
			this.active = false;
			if (!SceneManager.GetActiveScene().isLoaded)
			{
				return;
			}
			this.DeleteAllArms();
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x0010E084 File Offset: 0x0010C284
		private void DeleteAllArms()
		{
			foreach (SandboxArm sandboxArm in this.createdArms)
			{
				if (MonoSingleton<GunControl>.Instance && SummonSandboxArm.armSlot.Contains(sandboxArm.gameObject))
				{
					SummonSandboxArm.armSlot.Remove(sandboxArm.gameObject);
				}
				Object.Destroy(sandboxArm.gameObject);
			}
			if (MonoSingleton<GunControl>.Instance)
			{
				MonoSingleton<GunControl>.Instance.UpdateWeaponList(false);
			}
			this.createdArms.Clear();
			this.spawnedArmMap.Clear();
		}

		// Token: 0x04002E99 RID: 11929
		private bool active;

		// Token: 0x04002E9A RID: 11930
		private readonly List<SandboxArm> createdArms = new List<SandboxArm>();

		// Token: 0x04002E9B RID: 11931
		private readonly Dictionary<SpawnableType, SandboxArm> spawnedArmMap = new Dictionary<SpawnableType, SandboxArm>();

		// Token: 0x04002E9C RID: 11932
		private readonly SpawnableType[][] mainArmTypes = new SpawnableType[][]
		{
			new SpawnableType[] { SpawnableType.MoveHand },
			new SpawnableType[] { SpawnableType.AlterHand },
			new SpawnableType[] { SpawnableType.DestroyHand },
			new SpawnableType[]
			{
				SpawnableType.Prop,
				SpawnableType.SimpleSpawn,
				SpawnableType.BuildHand
			}
		};
	}
}
