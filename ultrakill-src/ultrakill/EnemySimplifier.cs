using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000198 RID: 408
[DefaultExecutionOrder(200)]
public class EnemySimplifier : MonoBehaviour
{
	// Token: 0x06000835 RID: 2101 RVA: 0x00038C20 File Offset: 0x00036E20
	private void Awake()
	{
		this.oman = MonoSingleton<OptionsManager>.Instance;
		this.player = MonoSingleton<NewMovement>.Instance.gameObject;
		if (this.enemyRootTransform == null)
		{
			this.enemyRootTransform = base.transform;
		}
		this.meshrenderer = base.GetComponent<Renderer>();
		this.currentMaterial = this.meshrenderer.material;
		this.originalMaterial = this.currentMaterial;
		if (this.enragedSimplifiedMaterial)
		{
			this.enragedSimplifiedMaterial = new Material(this.enragedSimplifiedMaterial);
		}
		if (this.simplifiedMaterial)
		{
			this.simplifiedMaterial = new Material(this.simplifiedMaterial);
		}
		if (this.simplifiedMaterial2)
		{
			this.simplifiedMaterial2 = new Material(this.simplifiedMaterial2);
		}
		if (this.simplifiedMaterial3)
		{
			this.simplifiedMaterial3 = new Material(this.simplifiedMaterial3);
		}
		this.matList = this.meshrenderer.materials;
		if (this.simplifiedMaterial2 != null && this.matList.Length > 1)
		{
			Material material = this.matList[1];
			this.originalMaterial2 = material;
		}
		if (this.simplifiedMaterial3 != null && this.matList.Length > 2)
		{
			Material material2 = this.matList[2];
			this.originalMaterial3 = material2;
		}
		if (!this.enragedMaterial)
		{
			this.enragedMaterial = this.originalMaterial;
		}
		this.materialDict = new Dictionary<EnemySimplifier.MaterialState, Material>
		{
			{
				EnemySimplifier.MaterialState.normal,
				this.originalMaterial
			},
			{
				EnemySimplifier.MaterialState.simplified,
				this.simplifiedMaterial
			},
			{
				EnemySimplifier.MaterialState.enraged,
				this.enragedMaterial
			},
			{
				EnemySimplifier.MaterialState.enragedSimplified,
				this.enragedSimplifiedMaterial
			}
		};
		this.eid = base.GetComponentInParent<EnemyIdentifier>();
		if (this.eid && this.enemyColorType == EnemyType.Cerberus)
		{
			this.enemyColorType = this.eid.enemyType;
		}
		if (base.GetComponentInParent<SeasonalHats>())
		{
			this.isHat = true;
		}
		this.propBlock = new MaterialPropertyBlock();
		if (this.isHat && !this.eid)
		{
			this.Begone();
		}
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x00038E2C File Offset: 0x0003702C
	private void Start()
	{
		this.oman = MonoSingleton<OptionsManager>.Instance;
		this.player = MonoSingleton<NewMovement>.Instance.gameObject;
		this.TryRemoveSimplifier();
		if (this.enemyRootTransform == null)
		{
			this.enemyRootTransform = base.transform;
		}
		this.hasSimplifiedMaterial = this.simplifiedMaterial != null;
		this.hasEnragedSimplified = this.enragedSimplifiedMaterial != null;
		this.UpdateColors();
		this.SetOutline(true);
		if (this.neverOutlineAndRemoveSimplifier)
		{
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			materialPropertyBlock.SetFloat("_Outline", 0f);
			materialPropertyBlock.SetFloat("_ForceOutline", 0f);
			this.meshrenderer.SetPropertyBlock(materialPropertyBlock);
			this.active = false;
			Object.Destroy(this);
			return;
		}
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x00038EF0 File Offset: 0x000370F0
	private void TryRemoveSimplifier()
	{
		if (this.isHat)
		{
			bool flag = true;
			EnemySimplifier[] componentsInChildren = this.eid.GetComponentsInChildren<EnemySimplifier>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (!componentsInChildren[i].isHat)
				{
					flag = false;
				}
			}
			if (flag)
			{
				this.Begone();
			}
		}
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x00038F36 File Offset: 0x00037136
	private void OnEnable()
	{
		this.hasSimplifiedMaterial = this.simplifiedMaterial != null;
		this.UpdateColors();
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x00038F50 File Offset: 0x00037150
	private void Update()
	{
		if (this.active)
		{
			this.SetOutline(false);
		}
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x00038F64 File Offset: 0x00037164
	public void SetOutline(bool forceUpdate)
	{
		this.playerDistCheck = Vector3.Distance(this.enemyRootTransform.position, this.player.transform.position) > this.oman.simplifiedDistance;
		if (!this.enemyScriptHandlesEnrage)
		{
			if (this.oman.simplifyEnemies && this.hasSimplifiedMaterial && this.playerDistCheck && !this.oman.outlinesOnly)
			{
				this.currentState = ((this.enraged && this.hasEnragedSimplified) ? EnemySimplifier.MaterialState.enragedSimplified : EnemySimplifier.MaterialState.simplified);
			}
			else
			{
				this.currentState = (this.enraged ? EnemySimplifier.MaterialState.enraged : EnemySimplifier.MaterialState.normal);
			}
		}
		bool flag = this.currentState != this.lastState;
		bool flag2 = false;
		bool flag3 = false;
		if (this.eid && !this.eid.dead)
		{
			flag3 = this.eid.blessed;
			if (this.eid.enemyType != EnemyType.Stalker && this.eid.sandified)
			{
				flag2 = true;
			}
		}
		if (flag2 != this.lastSandified || flag3 != this.lastBlessed)
		{
			flag = true;
		}
		this.lastSandified = flag2;
		this.lastBlessed = flag3;
		if (forceUpdate)
		{
			flag = true;
		}
		if (flag && !this.enemyScriptHandlesEnrage)
		{
			this.matList[0] = this.materialDict[this.currentState];
			if (this.simplifiedMaterial2 != null)
			{
				this.matList[1] = ((this.currentState == EnemySimplifier.MaterialState.normal) ? this.originalMaterial2 : this.simplifiedMaterial2);
			}
			if (this.simplifiedMaterial3 != null)
			{
				this.matList[2] = ((this.currentState == EnemySimplifier.MaterialState.normal) ? this.originalMaterial3 : this.simplifiedMaterial3);
			}
			this.meshrenderer.materials = this.matList;
		}
		this.shouldBeOutlined = ((this.oman.simplifyEnemies && this.playerDistCheck) ? 1 : 0);
		if (this.shouldBeOutlined != this.lastOutlineState || flag)
		{
			this.meshrenderer.GetPropertyBlock(this.propBlock);
			this.propBlock.SetFloat(EnemySimplifier.HasSandBuff, (float)(flag2 ? 1 : 0));
			this.propBlock.SetFloat(EnemySimplifier.NewSanded, (float)(flag2 ? 1 : 0));
			this.propBlock.SetFloat(EnemySimplifier.Blessed, (float)(flag3 ? 1 : 0));
			this.propBlock.SetFloat(EnemySimplifier.Outline, (float)this.shouldBeOutlined);
			this.propBlock.SetFloat(EnemySimplifier.ForceOutline, 1f);
			this.meshrenderer.SetPropertyBlock(this.propBlock);
		}
		this.lastState = this.currentState;
		this.lastOutlineState = this.shouldBeOutlined;
		if (this.eid && (this.eid.damageBuff || this.eid.speedBuff || this.eid.healthBuff || OptionsManager.forceRadiance))
		{
			if (this.radianceEffect == null)
			{
				this.radianceEffect = this.meshrenderer.gameObject.AddComponent<DoubleRender>();
				this.radianceEffect.subMeshesToIgnore = this.radiantSubmeshesToIgnore;
			}
			this.radianceEffect.SetOutline((this.oman.simplifyEnemies && this.playerDistCheck) ? 1 : 0);
			return;
		}
		if (this.radianceEffect)
		{
			this.radianceEffect.RemoveEffect();
		}
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x000392B0 File Offset: 0x000374B0
	public void UpdateColors()
	{
		if (!this.ignoreCustomColor)
		{
			if (this.hasSimplifiedMaterial)
			{
				this.simplifiedMaterial.color = MonoSingleton<ColorBlindSettings>.Instance.GetEnemyColor(this.enemyColorType);
			}
			if (this.enragedSimplifiedMaterial)
			{
				this.enragedSimplifiedMaterial.color = MonoSingleton<ColorBlindSettings>.Instance.enrageColor;
			}
		}
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x0003930C File Offset: 0x0003750C
	public void Begone()
	{
		this.active = false;
		if (this.meshrenderer)
		{
			this.matList[0] = this.originalMaterial;
			if (this.simplifiedMaterial2 != null)
			{
				this.matList[1] = this.originalMaterial2;
			}
			if (this.simplifiedMaterial3 != null)
			{
				this.matList[2] = this.originalMaterial3;
			}
			this.meshrenderer.materials = this.matList;
			this.propBlock.SetFloat("_Outline", 0f);
			this.propBlock.SetFloat("_ForceOutline", 0f);
			this.propBlock.SetFloat(EnemySimplifier.HasSandBuff, 0f);
			this.propBlock.SetFloat(EnemySimplifier.NewSanded, 0f);
			this.propBlock.SetFloat(EnemySimplifier.Blessed, 0f);
			this.meshrenderer.SetPropertyBlock(this.propBlock);
		}
		if (this.radianceEffect)
		{
			this.radianceEffect.RemoveEffect();
		}
		Object.Destroy(this);
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x0003941F File Offset: 0x0003761F
	public void ChangeTexture(EnemySimplifier.MaterialState stateToTarget, Texture newTexture)
	{
		this.materialDict[stateToTarget].SetTexture("_MainTex", newTexture);
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x00039438 File Offset: 0x00037638
	public void ChangeMaterialNew(EnemySimplifier.MaterialState stateToTarget, Material newMaterial)
	{
		this.materialDict[stateToTarget] = newMaterial;
		this.SetOutline(true);
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x00039450 File Offset: 0x00037650
	public void ChangeMaterial(Material oldMaterial, Material newMaterial)
	{
		bool flag = false;
		if (this.currentMaterial == oldMaterial)
		{
			flag = true;
		}
		newMaterial = new Material(newMaterial);
		if (oldMaterial == this.originalMaterial)
		{
			this.originalMaterial = newMaterial;
		}
		else if (oldMaterial == this.simplifiedMaterial)
		{
			this.simplifiedMaterial = newMaterial;
		}
		else if (oldMaterial == this.enragedMaterial)
		{
			this.enragedMaterial = newMaterial;
		}
		else if (oldMaterial == this.enragedSimplifiedMaterial)
		{
			this.enragedSimplifiedMaterial = newMaterial;
		}
		if (flag)
		{
			this.meshrenderer.material = newMaterial;
			this.currentMaterial = newMaterial;
		}
	}

	// Token: 0x04000AED RID: 2797
	public bool neverOutlineAndRemoveSimplifier;

	// Token: 0x04000AEE RID: 2798
	public bool enemyScriptHandlesEnrage;

	// Token: 0x04000AEF RID: 2799
	public Transform enemyRootTransform;

	// Token: 0x04000AF0 RID: 2800
	public List<int> radiantSubmeshesToIgnore = new List<int>();

	// Token: 0x04000AF1 RID: 2801
	private Material currentMaterial;

	// Token: 0x04000AF2 RID: 2802
	public Material enragedMaterial;

	// Token: 0x04000AF3 RID: 2803
	[HideInInspector]
	public Material originalMaterial;

	// Token: 0x04000AF4 RID: 2804
	[HideInInspector]
	public Material originalMaterial2;

	// Token: 0x04000AF5 RID: 2805
	[HideInInspector]
	public Material originalMaterial3;

	// Token: 0x04000AF6 RID: 2806
	public Material simplifiedMaterial;

	// Token: 0x04000AF7 RID: 2807
	public Material simplifiedMaterial2;

	// Token: 0x04000AF8 RID: 2808
	public Material simplifiedMaterial3;

	// Token: 0x04000AF9 RID: 2809
	public Material enragedSimplifiedMaterial;

	// Token: 0x04000AFA RID: 2810
	private Renderer meshrenderer;

	// Token: 0x04000AFB RID: 2811
	[HideInInspector]
	public bool enraged;

	// Token: 0x04000AFC RID: 2812
	private OptionsManager oman;

	// Token: 0x04000AFD RID: 2813
	private GameObject player;

	// Token: 0x04000AFE RID: 2814
	private bool active = true;

	// Token: 0x04000AFF RID: 2815
	private bool simplify;

	// Token: 0x04000B00 RID: 2816
	private bool playerDistCheck;

	// Token: 0x04000B01 RID: 2817
	[HideInInspector]
	public EnemyType enemyColorType;

	// Token: 0x04000B02 RID: 2818
	public bool ignoreCustomColor;

	// Token: 0x04000B03 RID: 2819
	[HideInInspector]
	public EnemyIdentifier eid;

	// Token: 0x04000B04 RID: 2820
	[HideInInspector]
	public bool isHat;

	// Token: 0x04000B05 RID: 2821
	[HideInInspector]
	public DoubleRender radianceEffect;

	// Token: 0x04000B06 RID: 2822
	public Material[] matList;

	// Token: 0x04000B07 RID: 2823
	private bool hasSimplifiedMaterial;

	// Token: 0x04000B08 RID: 2824
	private int shouldBeOutlined;

	// Token: 0x04000B09 RID: 2825
	private int lastOutlineState;

	// Token: 0x04000B0A RID: 2826
	private EnemySimplifier.MaterialState currentState;

	// Token: 0x04000B0B RID: 2827
	private EnemySimplifier.MaterialState lastState;

	// Token: 0x04000B0C RID: 2828
	private Dictionary<EnemySimplifier.MaterialState, Material> materialDict;

	// Token: 0x04000B0D RID: 2829
	private bool hasEnragedSimplified;

	// Token: 0x04000B0E RID: 2830
	private bool lastSandified;

	// Token: 0x04000B0F RID: 2831
	private bool lastBlessed;

	// Token: 0x04000B10 RID: 2832
	private MaterialPropertyBlock propBlock;

	// Token: 0x04000B11 RID: 2833
	private static readonly int HasSandBuff = Shader.PropertyToID("_HasSandBuff");

	// Token: 0x04000B12 RID: 2834
	private static readonly int NewSanded = Shader.PropertyToID("_Sanded");

	// Token: 0x04000B13 RID: 2835
	private static readonly int Blessed = Shader.PropertyToID("_Blessed");

	// Token: 0x04000B14 RID: 2836
	private static readonly int Outline = Shader.PropertyToID("_Outline");

	// Token: 0x04000B15 RID: 2837
	private static readonly int ForceOutline = Shader.PropertyToID("_ForceOutline");

	// Token: 0x02000199 RID: 409
	public enum MaterialState
	{
		// Token: 0x04000B17 RID: 2839
		normal,
		// Token: 0x04000B18 RID: 2840
		simplified,
		// Token: 0x04000B19 RID: 2841
		enraged,
		// Token: 0x04000B1A RID: 2842
		enragedSimplified
	}
}
