using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020000D0 RID: 208
[ExecuteInEditMode]
public abstract class vgCommandBufferBase : MonoBehaviour
{
	// Token: 0x060004CB RID: 1227
	protected abstract void RefreshCommandBufferInfo(CommandBuffer buf, Camera cam);

	// Token: 0x060004CC RID: 1228
	protected abstract string GetPassCommandBufferName();

	// Token: 0x060004CD RID: 1229
	protected abstract CameraEvent GetPassCameraEvent();

	// Token: 0x060004CE RID: 1230
	protected abstract int GetPassSortingIndex();

	// Token: 0x060004CF RID: 1231 RVA: 0x0001BF10 File Offset: 0x0001A110
	public virtual void VerifyResources()
	{
		if (vgCommandBufferBase.fullScreenQuadMesh == null)
		{
			vgCommandBufferBase.fullScreenQuadMesh = new Mesh();
			Vector3[] array = new Vector3[]
			{
				new Vector3(-1f, -1f, 3f),
				new Vector3(1f, -1f, 2f),
				new Vector3(1f, 1f, 1f),
				new Vector3(-1f, 1f, 0f)
			};
			Vector2[] array2 = new Vector2[]
			{
				new Vector2(0f, 0f),
				new Vector2(1f, 0f),
				new Vector2(1f, 1f),
				new Vector2(0f, 1f)
			};
			int[] array3 = new int[] { 0, 1, 2, 0, 2, 3 };
			vgCommandBufferBase.fullScreenQuadMesh.vertices = array;
			vgCommandBufferBase.fullScreenQuadMesh.uv = array2;
			vgCommandBufferBase.fullScreenQuadMesh.triangles = array3;
		}
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x0001C04C File Offset: 0x0001A24C
	public void AddToCommandListIfNeededAndSort(vgCommandBufferBase.CommandBuffersAndEvents cbce)
	{
		for (int i = 0; i < vgCommandBufferBase.cameraCbs.Count; i++)
		{
			if (cbce.cb.name.Equals(vgCommandBufferBase.cameraCbs[i].cb.name))
			{
				return;
			}
		}
		if (vgCommandBufferBase.mainCamera)
		{
			vgCommandBufferBase.mainCamera.RemoveAllCommandBuffers();
		}
		vgCommandBufferBase.cameraCbs.Add(cbce);
		vgCommandBufferBase.cameraCbs.Sort(new vgCommandBufferBase.CBSorter());
		if (vgCommandBufferBase.mainCamera)
		{
			for (int j = 0; j < vgCommandBufferBase.cameraCbs.Count; j++)
			{
				vgCommandBufferBase.mainCamera.AddCommandBuffer(vgCommandBufferBase.cameraCbs[j].ce, vgCommandBufferBase.cameraCbs[j].cb);
			}
		}
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x0001C114 File Offset: 0x0001A314
	public void OnDisable()
	{
		if (!vgCommandBufferBase.mainCamera)
		{
			return;
		}
		string passCommandBufferName = this.GetPassCommandBufferName();
		int num = -1;
		int num2 = 0;
		while (num2 < vgCommandBufferBase.cameraCbs.Count && num == -1)
		{
			if (vgCommandBufferBase.cameraCbs[num2].cb.name.Equals(passCommandBufferName))
			{
				num = num2;
			}
			num2++;
		}
		if (num == -1)
		{
			return;
		}
		CameraEvent ce = vgCommandBufferBase.cameraCbs[num].ce;
		CommandBuffer[] commandBuffers = vgCommandBufferBase.mainCamera.GetCommandBuffers(ce);
		for (int i = 0; i < commandBuffers.Length; i++)
		{
			if (commandBuffers[i].name.Equals(passCommandBufferName))
			{
				vgCommandBufferBase.mainCamera.RemoveCommandBuffer(ce, commandBuffers[i]);
			}
		}
		vgCommandBufferBase.cameraCbs.RemoveAt(num);
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x0001C1D4 File Offset: 0x0001A3D4
	public void OnEnable()
	{
		if (vgCommandBufferBase.cameraCbs == null)
		{
			vgCommandBufferBase.cameraCbs = new List<vgCommandBufferBase.CommandBuffersAndEvents>();
		}
		vgCommandBufferBase.mainCamera = base.GetComponent<Camera>();
		this.AddToCommandListIfNeededAndSort(new vgCommandBufferBase.CommandBuffersAndEvents
		{
			cb = new CommandBuffer(),
			cb = 
			{
				name = this.GetPassCommandBufferName()
			},
			ce = this.GetPassCameraEvent(),
			index = this.GetPassSortingIndex()
		});
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x0001C240 File Offset: 0x0001A440
	private void AddCommandBufferToCameraIfNeeded(vgCommandBufferBase.CommandBuffersAndEvents cbce)
	{
		if (!vgCommandBufferBase.mainCamera)
		{
			return;
		}
		if (!cbce.addedToCamera)
		{
			cbce.addedToCamera = true;
			CommandBuffer[] commandBuffers = vgCommandBufferBase.mainCamera.GetCommandBuffers(cbce.ce);
			for (int i = 0; i < commandBuffers.Length; i++)
			{
				if (commandBuffers[i].name.Equals(cbce.cb.name))
				{
					return;
				}
			}
			vgCommandBufferBase.mainCamera.AddCommandBuffer(cbce.ce, cbce.cb);
		}
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x0001C2BC File Offset: 0x0001A4BC
	private void Update()
	{
		if (!base.gameObject.activeInHierarchy || !base.enabled)
		{
			this.OnDisable();
			return;
		}
		if (!vgCommandBufferBase.mainCamera)
		{
			return;
		}
		for (int i = 0; i < vgCommandBufferBase.cameraCbs.Count; i++)
		{
			if (vgCommandBufferBase.cameraCbs[i].cb.name.Equals(this.GetPassCommandBufferName()))
			{
				vgCommandBufferBase.cameraCbs[i].cb.Clear();
				this.RefreshCommandBufferInfo(vgCommandBufferBase.cameraCbs[i].cb, vgCommandBufferBase.mainCamera);
				this.AddCommandBufferToCameraIfNeeded(vgCommandBufferBase.cameraCbs[i]);
			}
		}
	}

	// Token: 0x0400049F RID: 1183
	protected static Mesh fullScreenQuadMesh;

	// Token: 0x040004A0 RID: 1184
	protected static List<vgCommandBufferBase.CommandBuffersAndEvents> cameraCbs;

	// Token: 0x040004A1 RID: 1185
	protected static Camera mainCamera;

	// Token: 0x020003A6 RID: 934
	public class CommandBuffersAndEvents
	{
		// Token: 0x04001366 RID: 4966
		public CommandBuffer cb;

		// Token: 0x04001367 RID: 4967
		public CameraEvent ce;

		// Token: 0x04001368 RID: 4968
		public bool addedToCamera;

		// Token: 0x04001369 RID: 4969
		public int index;
	}

	// Token: 0x020003A7 RID: 935
	public class CBSorter : IComparer<vgCommandBufferBase.CommandBuffersAndEvents>
	{
		// Token: 0x060016AB RID: 5803 RVA: 0x000775D3 File Offset: 0x000757D3
		public int Compare(vgCommandBufferBase.CommandBuffersAndEvents x, vgCommandBufferBase.CommandBuffersAndEvents y)
		{
			return x.index - y.index;
		}
	}
}
