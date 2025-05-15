using System;
using UnityEngine;

// Token: 0x0200022D RID: 557
public class GoreSplatter : MonoBehaviour
{
	// Token: 0x06000BE7 RID: 3047 RVA: 0x000539B0 File Offset: 0x00051BB0
	private void OnEnable()
	{
		base.Invoke("SlowUpdate", 1f);
		if (this.beenFlung)
		{
			return;
		}
		this.beenFlung = true;
		if (!this.rb)
		{
			base.TryGetComponent<Rigidbody>(out this.rb);
		}
		this.StopGore();
		if (!this.detectCollisions)
		{
			this.rb.detectCollisions = true;
		}
		this.defaultScale = base.transform.localScale;
		this.direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		this.force = (float)Random.Range(20, 60);
		this.startPos = base.transform.position;
		this.bloodAbsorberCount = 0;
		if (StockMapInfo.Instance.continuousGibCollisions)
		{
			this.rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
		}
		this.rb.AddForce(this.direction * this.force, ForceMode.VelocityChange);
		this.rb.rotation = Random.rotation;
		this.freezeGore = !MonoSingleton<BloodsplatterManager>.Instance.neverFreezeGibs && MonoSingleton<PrefsManager>.Instance.GetBoolLocal("freezeGore", false);
		if (this.freezeGore)
		{
			base.Invoke("Repool", 5f);
		}
		if (StockMapInfo.Instance.removeGibsWithoutAbsorbers)
		{
			base.Invoke("RepoolIfNoAbsorber", StockMapInfo.Instance.gibRemoveTime);
		}
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x00053B25 File Offset: 0x00051D25
	private void OnDisable()
	{
		base.CancelInvoke("SlowUpdate");
	}

	// Token: 0x06000BE9 RID: 3049 RVA: 0x00053B32 File Offset: 0x00051D32
	private void RepoolIfNoAbsorber()
	{
		if (this.bloodAbsorberCount <= 0)
		{
			this.Repool();
			return;
		}
		if (StockMapInfo.Instance.removeGibsWithoutAbsorbers)
		{
			base.Invoke("RepoolIfNoAbsorber", StockMapInfo.Instance.gibRemoveTime);
		}
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x00053B65 File Offset: 0x00051D65
	public void Repool()
	{
		this.beenFlung = false;
		MonoSingleton<BloodsplatterManager>.Instance.RepoolGore(base.gameObject, this.bloodSplatterType);
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x00053B84 File Offset: 0x00051D84
	private void SlowUpdate()
	{
		if (this.freezeGore && this.goreOver && this.rb.velocity.y > -0.1f && this.rb.velocity.y < 0.1f)
		{
			this.StopGore();
		}
		if (Vector3.Distance(base.transform.position, this.startPos) > 1000f)
		{
			this.Repool();
		}
		if (this.rb && base.transform.position.y > 666f && this.rb.velocity.y > 0f)
		{
			this.rb.velocity = new Vector3(this.rb.velocity.x, 0f, this.rb.velocity.z);
		}
		base.Invoke("SlowUpdate", 1f);
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x00053C78 File Offset: 0x00051E78
	private void OnCollisionEnter(Collision other)
	{
		if (this.freezeGore && LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment) && (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Floor")))
		{
			this.touchedCollisions++;
			this.goreOver = true;
		}
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x00053CDC File Offset: 0x00051EDC
	private void OnCollisionExit(Collision other)
	{
		if (this.freezeGore && LayerMaskDefaults.IsMatchingLayer(other.gameObject.layer, LMD.Environment) && (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Floor")))
		{
			this.touchedCollisions--;
			if (this.touchedCollisions <= 0)
			{
				this.goreOver = false;
			}
		}
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x00053D46 File Offset: 0x00051F46
	private void StopGore()
	{
		this.detectCollisions = false;
		this.rb.velocity = Vector3.zero;
		this.rb.detectCollisions = false;
	}

	// Token: 0x04000F7F RID: 3967
	public int bloodAbsorberCount;

	// Token: 0x04000F80 RID: 3968
	public BSType bloodSplatterType;

	// Token: 0x04000F81 RID: 3969
	private Rigidbody rb;

	// Token: 0x04000F82 RID: 3970
	private Vector3 direction;

	// Token: 0x04000F83 RID: 3971
	private float force;

	// Token: 0x04000F84 RID: 3972
	private bool goreOver;

	// Token: 0x04000F85 RID: 3973
	private int touchedCollisions;

	// Token: 0x04000F86 RID: 3974
	private Vector3 defaultScale;

	// Token: 0x04000F87 RID: 3975
	private bool freezeGore;

	// Token: 0x04000F88 RID: 3976
	private bool foundParent;

	// Token: 0x04000F89 RID: 3977
	private Vector3 startPos;

	// Token: 0x04000F8A RID: 3978
	private bool detectCollisions = true;

	// Token: 0x04000F8B RID: 3979
	[HideInInspector]
	public bool beenFlung;
}
