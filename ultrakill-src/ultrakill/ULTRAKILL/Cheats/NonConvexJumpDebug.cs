using System;
using System.Collections.Generic;
using NewBlood.Rendering;
using UnityEngine;

namespace ULTRAKILL.Cheats
{
	// Token: 0x0200060A RID: 1546
	public class NonConvexJumpDebug : ICheat
	{
		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x0600227C RID: 8828 RVA: 0x0010C59B File Offset: 0x0010A79B
		public static bool Active
		{
			get
			{
				return NonConvexJumpDebug._lastInstance != null && NonConvexJumpDebug._lastInstance.IsActive;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x0600227D RID: 8829 RVA: 0x0010C5B0 File Offset: 0x0010A7B0
		public string LongName
		{
			get
			{
				return "Non Convex Jump Debug";
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x0600227E RID: 8830 RVA: 0x0010C5B7 File Offset: 0x0010A7B7
		public string Identifier
		{
			get
			{
				return "ultrakill.debug.non-convex-jump-debug";
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x0600227F RID: 8831 RVA: 0x0010C5BE File Offset: 0x0010A7BE
		public string ButtonEnabledOverride { get; }

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06002280 RID: 8832 RVA: 0x0010C5C6 File Offset: 0x0010A7C6
		public string ButtonDisabledOverride { get; }

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06002281 RID: 8833 RVA: 0x000FF954 File Offset: 0x000FDB54
		public string Icon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06002282 RID: 8834 RVA: 0x0010C5CE File Offset: 0x0010A7CE
		// (set) Token: 0x06002283 RID: 8835 RVA: 0x0010C5D6 File Offset: 0x0010A7D6
		public bool IsActive { get; private set; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06002284 RID: 8836 RVA: 0x0010C5DF File Offset: 0x0010A7DF
		public bool DefaultState { get; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06002285 RID: 8837 RVA: 0x0002D245 File Offset: 0x0002B445
		public StatePersistenceMode PersistenceMode
		{
			get
			{
				return StatePersistenceMode.Persistent;
			}
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x0010C5E7 File Offset: 0x0010A7E7
		public void Enable(CheatsManager manager)
		{
			this.IsActive = true;
			NonConvexJumpDebug._lastInstance = this;
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x0010C5F6 File Offset: 0x0010A7F6
		public void Disable()
		{
			this.IsActive = false;
			NonConvexJumpDebug.Reset();
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x0010C604 File Offset: 0x0010A804
		public static void Reset()
		{
			if (NonConvexJumpDebug._lastInstance == null)
			{
				return;
			}
			foreach (GameObject gameObject in NonConvexJumpDebug._lastInstance._debugObjects)
			{
				Object.Destroy(gameObject);
			}
			NonConvexJumpDebug._lastInstance._debugObjects.Clear();
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x0010C670 File Offset: 0x0010A870
		public static void CreateBall(Color color, Vector3 position, float size = 1f)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			Object.DestroyImmediate(gameObject.GetComponent<Collider>());
			if (NonConvexJumpDebug._lastInstance == null)
			{
				NonConvexJumpDebug._lastInstance = new NonConvexJumpDebug();
			}
			NonConvexJumpDebug._lastInstance._debugObjects.Add(gameObject);
			gameObject.name = "jump debug ball";
			gameObject.transform.position = position;
			gameObject.transform.localScale = Vector3.one * size;
			MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
			Shader shader = Shader.Find("Unlit/Color");
			component.material = new Material(shader);
			component.material.color = color;
			gameObject.AddComponent<RemoveOnTime>().time = 3f;
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x0010C718 File Offset: 0x0010A918
		public static void CreateTri(Vector3 normal, Triangle<Vector3> triangle, Color color)
		{
			GameObject gameObject = new GameObject("jump debug tri");
			NonConvexJumpDebug._lastInstance._debugObjects.Add(gameObject);
			gameObject.transform.position += normal * 0.02f;
			MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
			gameObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Unlit/Color"))
			{
				color = color
			};
			meshFilter.sharedMesh = new Mesh
			{
				vertices = new Vector3[] { triangle.Index0, triangle.Index1, triangle.Index2 },
				triangles = new int[] { 0, 1, 2 }
			};
			gameObject.AddComponent<RemoveOnTime>().time = 3f;
		}

		// Token: 0x04002E1D RID: 11805
		private static NonConvexJumpDebug _lastInstance;

		// Token: 0x04002E22 RID: 11810
		private List<GameObject> _debugObjects = new List<GameObject>();
	}
}
