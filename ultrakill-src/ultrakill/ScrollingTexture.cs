using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003D9 RID: 985
public class ScrollingTexture : MonoBehaviour
{
	// Token: 0x0600164B RID: 5707 RVA: 0x000B3618 File Offset: 0x000B1818
	private void Start()
	{
		if (this.scrollAttachedObjects)
		{
			GameObject gameObject = new GameObject("Scrolling Texture Parent");
			this.parent = gameObject.AddComponent<BloodstainParent>();
		}
		if (ScrollingTexture._propertyBlock == null)
		{
			ScrollingTexture._propertyBlock = new MaterialPropertyBlock();
		}
		this.mr = base.GetComponent<MeshRenderer>();
		this.propertyNames = new Dictionary<int, int[]>();
		this.mr.GetMaterials(this.materials);
		this.usesMasterShader = new bool[this.materials.Count];
		Shader masterShader = MonoSingleton<DefaultReferenceManager>.Instance.masterShader;
		this.scrollOffsetID = Shader.PropertyToID("_ScrollOffset");
		for (int i = 0; i < this.materials.Count; i++)
		{
			Material material = this.materials[i];
			bool flag = material.shader == masterShader;
			this.usesMasterShader[i] = flag;
			if (flag)
			{
				material.EnableKeyword("SCROLLING");
			}
		}
		this.mr.SetMaterials(this.materials);
		if (this.scrollAttachedObjects && !this.valuesSet)
		{
			this.valuesSet = true;
			MonoSingleton<ComponentsDatabase>.Instance.scrollers.Add(base.transform);
			Collider collider;
			Rigidbody rigidbody;
			if (base.TryGetComponent<Collider>(out collider))
			{
				this.bounds = collider.bounds;
			}
			else if (base.TryGetComponent<Rigidbody>(out rigidbody))
			{
				Collider[] componentsInChildren = base.GetComponentsInChildren<Collider>();
				bool flag2 = false;
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					if (componentsInChildren[j].attachedRigidbody == rigidbody)
					{
						if (!flag2)
						{
							this.bounds = componentsInChildren[j].bounds;
							flag2 = true;
						}
						else
						{
							this.bounds.Encapsulate(componentsInChildren[j].bounds);
						}
					}
				}
			}
			base.Invoke("SlowUpdate", 5f);
		}
	}

	// Token: 0x0600164C RID: 5708 RVA: 0x000B37D0 File Offset: 0x000B19D0
	private void SlowUpdate()
	{
		foreach (GameObject gameObject in this.cleanUp)
		{
			Object.Destroy(gameObject);
		}
		this.cleanUp.Clear();
		base.Invoke("SlowUpdate", 5f);
	}

	// Token: 0x0600164D RID: 5709 RVA: 0x000B383C File Offset: 0x000B1A3C
	private void OnDestroy()
	{
		if (this.parent)
		{
			Object.Destroy(this.parent.gameObject);
		}
		if (!base.gameObject.scene.isLoaded)
		{
			return;
		}
		ComponentsDatabase instance = MonoSingleton<ComponentsDatabase>.Instance;
		if (instance)
		{
			instance.scrollers.Remove(base.transform);
		}
	}

	// Token: 0x0600164E RID: 5710 RVA: 0x000B389C File Offset: 0x000B1A9C
	private void Update()
	{
		this.offset += new Vector2(this.scrollSpeedX * Time.deltaTime, this.scrollSpeedY * Time.deltaTime);
		this.mr.GetPropertyBlock(ScrollingTexture._propertyBlock);
		ScrollingTexture._propertyBlock.SetVector(this.scrollOffsetID, new Vector4(this.offset.x, this.offset.y, 0f, 0f));
		this.mr.SetPropertyBlock(ScrollingTexture._propertyBlock);
		Vector3 vector = this.force;
		if (this.relativeDirection)
		{
			vector = new Vector3(this.force.x * base.transform.forward.x, this.force.y * base.transform.forward.y, this.force.z * base.transform.forward.z);
		}
		if (this.parent)
		{
			this.parent.transform.position += vector * Time.deltaTime;
		}
		if (this.scrollAttachedObjects && this.attachedObjects.Count > 0)
		{
			for (int i = this.attachedObjects.Count - 1; i >= 0; i--)
			{
				if (this.attachedObjects[i] != null)
				{
					this.attachedObjects[i].position = this.attachedObjects[i].position + vector * Time.deltaTime;
					int num = -1;
					if (this.specialScrollers.Count != 0)
					{
						for (int j = this.specialScrollers.Count - 1; j >= 0; j--)
						{
							if (this.specialScrollers[j].transform == null)
							{
								this.specialScrollers.RemoveAt(j);
							}
							else if (this.specialScrollers[j].transform == this.attachedObjects[i])
							{
								num = j;
								break;
							}
						}
					}
					if ((num < 0 && Vector3.Distance(this.attachedObjects[i].position, this.bounds.ClosestPoint(this.attachedObjects[i].position)) > 1f) || (num >= 0 && Vector3.Distance(this.attachedObjects[i].position + this.specialScrollers[num].closestPosition, this.bounds.ClosestPoint(this.attachedObjects[i].position + this.specialScrollers[num].closestPosition)) > 1f))
					{
						if (num >= 0)
						{
							this.specialScrollers.RemoveAt(num);
						}
						this.cleanUp.Add(this.attachedObjects[i].gameObject);
						this.attachedObjects[i].gameObject.SetActive(false);
						this.attachedObjects.RemoveAt(i);
					}
				}
				else
				{
					this.attachedObjects.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x0600164F RID: 5711 RVA: 0x000B3BC8 File Offset: 0x000B1DC8
	private void FixedUpdate()
	{
		if (this.touchingRbs.Count <= 0)
		{
			return;
		}
		Vector3 vector = this.force;
		if (this.relativeDirection)
		{
			vector = new Vector3(this.force.x * base.transform.forward.x, this.force.y * base.transform.forward.y, this.force.z * base.transform.forward.z);
		}
		for (int i = this.touchingRbs.Count - 1; i >= 0; i--)
		{
			if (this.touchingRbs[i] == null)
			{
				this.touchingRbs.RemoveAt(i);
			}
			else
			{
				this.touchingRbs[i].AddForce(vector * Time.fixedDeltaTime, ForceMode.VelocityChange);
			}
		}
	}

	// Token: 0x04001EAC RID: 7852
	private static MaterialPropertyBlock _propertyBlock;

	// Token: 0x04001EAD RID: 7853
	public float scrollSpeedX;

	// Token: 0x04001EAE RID: 7854
	public float scrollSpeedY;

	// Token: 0x04001EAF RID: 7855
	private int scrollOffsetID;

	// Token: 0x04001EB0 RID: 7856
	private bool[] usesMasterShader;

	// Token: 0x04001EB1 RID: 7857
	private Dictionary<int, int[]> propertyNames;

	// Token: 0x04001EB2 RID: 7858
	private List<Material> materials = new List<Material>();

	// Token: 0x04001EB3 RID: 7859
	private MeshRenderer mr;

	// Token: 0x04001EB4 RID: 7860
	private Vector2 offset;

	// Token: 0x04001EB5 RID: 7861
	public bool scrollAttachedObjects;

	// Token: 0x04001EB6 RID: 7862
	public Vector3 force;

	// Token: 0x04001EB7 RID: 7863
	public bool relativeDirection;

	// Token: 0x04001EB8 RID: 7864
	public List<Transform> attachedObjects = new List<Transform>();

	// Token: 0x04001EB9 RID: 7865
	[HideInInspector]
	public Bounds bounds;

	// Token: 0x04001EBA RID: 7866
	[HideInInspector]
	public bool valuesSet;

	// Token: 0x04001EBB RID: 7867
	[HideInInspector]
	public List<GameObject> cleanUp = new List<GameObject>();

	// Token: 0x04001EBC RID: 7868
	[HideInInspector]
	public List<WaterDryTracker> specialScrollers = new List<WaterDryTracker>();

	// Token: 0x04001EBD RID: 7869
	[HideInInspector]
	public List<Rigidbody> touchingRbs = new List<Rigidbody>();

	// Token: 0x04001EBE RID: 7870
	public BloodstainParent parent;
}
