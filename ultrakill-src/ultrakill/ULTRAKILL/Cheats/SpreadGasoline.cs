using System;
using UnityEngine;

namespace ULTRAKILL.Cheats
{
	// Token: 0x0200060E RID: 1550
	public class SpreadGasoline : ICheat
	{
		// Token: 0x17000301 RID: 769
		// (get) Token: 0x060022AD RID: 8877 RVA: 0x0010CA4A File Offset: 0x0010AC4A
		public string LongName
		{
			get
			{
				return "Spread Gasoline";
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x060022AE RID: 8878 RVA: 0x0010CA51 File Offset: 0x0010AC51
		public string Identifier
		{
			get
			{
				return "ultrakill.debug.spread-gasoline";
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x060022AF RID: 8879 RVA: 0x0010CA58 File Offset: 0x0010AC58
		public string ButtonEnabledOverride { get; }

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x060022B0 RID: 8880 RVA: 0x0010CA60 File Offset: 0x0010AC60
		public string ButtonDisabledOverride
		{
			get
			{
				return "Spawn Gasoline Projectiles";
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x060022B1 RID: 8881 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x060022B2 RID: 8882 RVA: 0x0010CA67 File Offset: 0x0010AC67
		public bool IsActive
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x060022B3 RID: 8883 RVA: 0x0010CA6F File Offset: 0x0010AC6F
		public bool DefaultState { get; }

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x060022B4 RID: 8884 RVA: 0x000C9FC2 File Offset: 0x000C81C2
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.NotPersistent;
			}
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x0010CA78 File Offset: 0x0010AC78
		public void Enable(CheatsManager manager)
		{
			if (this.asset == null)
			{
				GameObject gameObject = AssetHelper.LoadPrefab("Assets/Prefabs/Attacks and Projectiles/GasolineProjectile.prefab");
				if (gameObject == null || gameObject.Equals(null))
				{
					Debug.LogWarning("Failed to load projectile asset.\nRuntime key: Assets/Prefabs/Attacks and Projectiles/GasolineProjectile.prefab");
					MonoSingleton<HudMessageReceiver>.Instance.SendHudMessage("Failed to load gasoline projectile asset", "", "", 0, false, false, true);
					return;
				}
				this.asset = gameObject;
			}
			this.SpawnProjectiles(this.asset);
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x0010CAEC File Offset: 0x0010ACEC
		private void SpawnProjectiles(GameObject projectilePrefab)
		{
			Vector3 vector = MonoSingleton<PlayerTracker>.Instance.GetPlayer().position + Vector3.up * 4f;
			int num = 256;
			for (int i = 0; i < num; i++)
			{
				Quaternion quaternion = Random.rotation;
				if (quaternion.eulerAngles.x < 180f)
				{
					quaternion = Quaternion.Euler(180f, quaternion.eulerAngles.y, quaternion.eulerAngles.z);
				}
				GameObject gameObject = Object.Instantiate<GameObject>(projectilePrefab, vector, quaternion);
				gameObject.SetActive(true);
				Rigidbody rigidbody;
				if (gameObject.TryGetComponent<Rigidbody>(out rigidbody))
				{
					rigidbody.AddForce(quaternion * Vector3.forward * Random.Range(10f, 100f), ForceMode.Impulse);
				}
			}
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x00004AE3 File Offset: 0x00002CE3
		public void Disable()
		{
		}

		// Token: 0x04002E33 RID: 11827
		private bool active;

		// Token: 0x04002E34 RID: 11828
		private GameObject asset;
	}
}
