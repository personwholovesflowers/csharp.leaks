using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace AmplifyImpostors
{
	// Token: 0x02000304 RID: 772
	public class AmplifyImpostor : MonoBehaviour
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x00069436 File Offset: 0x00067636
		// (set) Token: 0x060013EA RID: 5098 RVA: 0x0006943E File Offset: 0x0006763E
		public AmplifyImpostorAsset Data
		{
			get
			{
				return this.m_data;
			}
			set
			{
				this.m_data = value;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060013EB RID: 5099 RVA: 0x00069447 File Offset: 0x00067647
		// (set) Token: 0x060013EC RID: 5100 RVA: 0x0006944F File Offset: 0x0006764F
		public Transform RootTransform
		{
			get
			{
				return this.m_rootTransform;
			}
			set
			{
				this.m_rootTransform = value;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060013ED RID: 5101 RVA: 0x00069458 File Offset: 0x00067658
		// (set) Token: 0x060013EE RID: 5102 RVA: 0x00069460 File Offset: 0x00067660
		public LODGroup LodGroup
		{
			get
			{
				return this.m_lodGroup;
			}
			set
			{
				this.m_lodGroup = value;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060013EF RID: 5103 RVA: 0x00069469 File Offset: 0x00067669
		// (set) Token: 0x060013F0 RID: 5104 RVA: 0x00069471 File Offset: 0x00067671
		public Renderer[] Renderers
		{
			get
			{
				return this.m_renderers;
			}
			set
			{
				this.m_renderers = value;
			}
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x0006947C File Offset: 0x0006767C
		private void GenerateTextures(List<TextureOutput> outputList, bool standardRendering)
		{
			this.m_rtGBuffers = new RenderTexture[outputList.Count];
			if (standardRendering && this.m_renderPipelineInUse == RenderPipelineInUse.HD)
			{
				GraphicsFormat graphicsFormat = GraphicsFormat.R8G8B8A8_SRGB;
				GraphicsFormat graphicsFormat2 = GraphicsFormat.R8G8B8A8_UNorm;
				GraphicsFormat graphicsFormat3 = GraphicsFormat.R16G16B16A16_SFloat;
				this.m_rtGBuffers[0] = new RenderTexture((int)this.m_data.TexSize.x, (int)this.m_data.TexSize.y, 16, graphicsFormat);
				this.m_rtGBuffers[0].Create();
				this.m_rtGBuffers[1] = new RenderTexture((int)this.m_data.TexSize.x, (int)this.m_data.TexSize.y, 16, graphicsFormat2);
				this.m_rtGBuffers[1].Create();
				this.m_rtGBuffers[2] = new RenderTexture((int)this.m_data.TexSize.x, (int)this.m_data.TexSize.y, 16, graphicsFormat2);
				this.m_rtGBuffers[2].Create();
				this.m_rtGBuffers[3] = new RenderTexture((int)this.m_data.TexSize.x, (int)this.m_data.TexSize.y, 16, graphicsFormat3);
				this.m_rtGBuffers[3].Create();
				this.m_rtGBuffers[4] = new RenderTexture((int)this.m_data.TexSize.x, (int)this.m_data.TexSize.y, 16, graphicsFormat2);
				this.m_rtGBuffers[4].Create();
			}
			else
			{
				for (int i = 0; i < this.m_rtGBuffers.Length; i++)
				{
					this.m_rtGBuffers[i] = new RenderTexture((int)this.m_data.TexSize.x, (int)this.m_data.TexSize.y, 16, outputList[i].SRGB ? RenderTextureFormat.ARGB32 : RenderTextureFormat.ARGBHalf);
					this.m_rtGBuffers[i].Create();
				}
			}
			this.m_trueDepth = new RenderTexture((int)this.m_data.TexSize.x, (int)this.m_data.TexSize.y, 16, RenderTextureFormat.Depth);
			this.m_trueDepth.Create();
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x00069698 File Offset: 0x00067898
		private void GenerateAlphaTextures(List<TextureOutput> outputList)
		{
			this.m_alphaGBuffers = new RenderTexture[outputList.Count];
			for (int i = 0; i < this.m_alphaGBuffers.Length; i++)
			{
				this.m_alphaGBuffers[i] = new RenderTexture(256, 256, 16, outputList[i].SRGB ? RenderTextureFormat.ARGB32 : RenderTextureFormat.ARGBHalf);
				this.m_alphaGBuffers[i].Create();
			}
			this.m_trueDepth = new RenderTexture(256, 256, 16, RenderTextureFormat.Depth);
			this.m_trueDepth.Create();
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x00069728 File Offset: 0x00067928
		private void ClearBuffers()
		{
			RenderTexture.active = null;
			RenderTexture[] rtGBuffers = this.m_rtGBuffers;
			for (int i = 0; i < rtGBuffers.Length; i++)
			{
				rtGBuffers[i].Release();
			}
			this.m_rtGBuffers = null;
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x00069760 File Offset: 0x00067960
		private void ClearAlphaBuffers()
		{
			RenderTexture.active = null;
			RenderTexture[] alphaGBuffers = this.m_alphaGBuffers;
			for (int i = 0; i < alphaGBuffers.Length; i++)
			{
				alphaGBuffers[i].Release();
			}
			this.m_alphaGBuffers = null;
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x00069798 File Offset: 0x00067998
		public void RenderImpostor(ImpostorType impostorType, int targetAmount, bool impostorMaps = true, bool combinedAlphas = false, bool useMinResolution = false, Shader customShader = null)
		{
			if (!impostorMaps && !combinedAlphas)
			{
				return;
			}
			if (targetAmount <= 0)
			{
				return;
			}
			bool flag = customShader == null;
			Dictionary<Material, Material> dictionary = new Dictionary<Material, Material>();
			CommandBuffer commandBuffer = new CommandBuffer();
			if (impostorMaps)
			{
				commandBuffer.name = "GBufferCatcher";
				RenderTargetIdentifier[] array = new RenderTargetIdentifier[targetAmount];
				for (int i = 0; i < targetAmount; i++)
				{
					array[i] = this.m_rtGBuffers[i];
				}
				commandBuffer.SetRenderTarget(array, this.m_trueDepth);
				commandBuffer.ClearRenderTarget(true, true, Color.clear, 1f);
			}
			CommandBuffer commandBuffer2 = new CommandBuffer();
			if (combinedAlphas)
			{
				commandBuffer2.name = "DepthAlphaCatcher";
				RenderTargetIdentifier[] array2 = new RenderTargetIdentifier[targetAmount];
				for (int j = 0; j < targetAmount; j++)
				{
					array2[j] = this.m_alphaGBuffers[j];
				}
				commandBuffer2.SetRenderTarget(array2, this.m_trueDepth);
				commandBuffer2.ClearRenderTarget(true, true, Color.clear, 1f);
			}
			int horizontalFrames = this.m_data.HorizontalFrames;
			int num = this.m_data.HorizontalFrames;
			if (impostorType == ImpostorType.Spherical)
			{
				num = this.m_data.HorizontalFrames - 1;
				if (this.m_data.DecoupleAxisFrames)
				{
					num = this.m_data.VerticalFrames - 1;
				}
			}
			List<MeshFilter> list = new List<MeshFilter>();
			for (int k = 0; k < this.Renderers.Length; k++)
			{
				if (this.Renderers[k] == null || !this.Renderers[k].enabled || this.Renderers[k].shadowCastingMode == ShadowCastingMode.ShadowsOnly)
				{
					list.Add(null);
				}
				else
				{
					MeshFilter component = this.Renderers[k].GetComponent<MeshFilter>();
					if (component == null || component.sharedMesh == null)
					{
						list.Add(null);
					}
					else
					{
						list.Add(component);
					}
				}
			}
			int count = list.Count;
			for (int l = 0; l < horizontalFrames; l++)
			{
				for (int m = 0; m <= num; m++)
				{
					Bounds bounds = default(Bounds);
					Matrix4x4 cameraRotationMatrix = this.GetCameraRotationMatrix(impostorType, horizontalFrames, num, l, m);
					for (int n = 0; n < count; n++)
					{
						if (!(list[n] == null))
						{
							if (bounds.size == Vector3.zero)
							{
								bounds = list[n].sharedMesh.bounds.Transform(this.m_rootTransform.worldToLocalMatrix * this.Renderers[n].localToWorldMatrix);
							}
							else
							{
								bounds.Encapsulate(list[n].sharedMesh.bounds.Transform(this.m_rootTransform.worldToLocalMatrix * this.Renderers[n].localToWorldMatrix));
							}
						}
					}
					if (l == 0 && m == 0)
					{
						this.m_originalBound = bounds;
					}
					bounds = bounds.Transform(cameraRotationMatrix);
					Matrix4x4 matrix4x = cameraRotationMatrix.inverse * Matrix4x4.LookAt(bounds.center - new Vector3(0f, 0f, this.m_depthFitSize * 0.5f), bounds.center, Vector3.up);
					float num2 = this.m_xyFitSize * 0.5f;
					Matrix4x4 matrix4x2 = Matrix4x4.Ortho(-num2 + this.m_pixelOffset.x, num2 + this.m_pixelOffset.x, -num2 + this.m_pixelOffset.y, num2 + this.m_pixelOffset.y, 0f, -this.m_depthFitSize);
					matrix4x = matrix4x.inverse * this.m_rootTransform.worldToLocalMatrix;
					if (flag && this.m_renderPipelineInUse == RenderPipelineInUse.HD)
					{
						matrix4x2 = GL.GetGPUProjectionMatrix(matrix4x2, true);
					}
					if (impostorMaps)
					{
						commandBuffer.SetViewProjectionMatrices(matrix4x, matrix4x2);
						commandBuffer.SetViewport(new Rect(this.m_data.TexSize.x / (float)horizontalFrames * (float)l, this.m_data.TexSize.y / (float)(num + ((impostorType == ImpostorType.Spherical) ? 1 : 0)) * (float)m, this.m_data.TexSize.x / (float)this.m_data.HorizontalFrames, this.m_data.TexSize.y / (float)this.m_data.VerticalFrames));
						if (flag && this.m_renderPipelineInUse == RenderPipelineInUse.HD)
						{
							commandBuffer.SetGlobalMatrix("_ViewMatrix", matrix4x);
							commandBuffer.SetGlobalMatrix("_InvViewMatrix", matrix4x.inverse);
							commandBuffer.SetGlobalMatrix("_ProjMatrix", matrix4x2);
							commandBuffer.SetGlobalMatrix("_ViewProjMatrix", matrix4x2 * matrix4x);
							commandBuffer.SetGlobalVector("_WorldSpaceCameraPos", Vector4.zero);
						}
					}
					if (combinedAlphas)
					{
						commandBuffer2.SetViewProjectionMatrices(matrix4x, matrix4x2);
						commandBuffer2.SetViewport(new Rect(0f, 0f, 256f, 256f));
						if (flag && this.m_renderPipelineInUse == RenderPipelineInUse.HD)
						{
							commandBuffer2.SetGlobalMatrix("_ViewMatrix", matrix4x);
							commandBuffer2.SetGlobalMatrix("_InvViewMatrix", matrix4x.inverse);
							commandBuffer2.SetGlobalMatrix("_ProjMatrix", matrix4x2);
							commandBuffer2.SetGlobalMatrix("_ViewProjMatrix", matrix4x2 * matrix4x);
							commandBuffer2.SetGlobalVector("_WorldSpaceCameraPos", Vector4.zero);
						}
					}
					for (int num3 = 0; num3 < count; num3++)
					{
						if (!(list[num3] == null))
						{
							Material[] sharedMaterials = this.Renderers[num3].sharedMaterials;
							for (int num4 = 0; num4 < sharedMaterials.Length; num4++)
							{
								Material material = null;
								Mesh sharedMesh = list[num3].sharedMesh;
								int num5 = 0;
								int num6;
								if (flag)
								{
									material = sharedMaterials[num4];
									num5 = material.FindPass("DEFERRED");
									if (num5 == -1)
									{
										num5 = material.FindPass("Deferred");
									}
									if (num5 == -1)
									{
										num5 = material.FindPass("GBuffer");
									}
									num6 = material.FindPass("DepthOnly");
									if (num5 == -1)
									{
										num5 = 0;
										for (int num7 = 0; num7 < material.passCount; num7++)
										{
											if (material.GetTag("LightMode", true).Equals("Deferred"))
											{
												num5 = num7;
												break;
											}
										}
									}
									commandBuffer.EnableShaderKeyword("UNITY_HDR_ON");
								}
								else
								{
									num6 = -1;
									if (!dictionary.TryGetValue(sharedMaterials[num4], out material))
									{
										material = new Material(customShader)
										{
											hideFlags = HideFlags.HideAndDontSave
										};
										dictionary.Add(sharedMaterials[num4], material);
									}
								}
								bool flag2 = this.Renderers[num3].lightmapIndex > -1;
								bool flag3 = this.Renderers[num3].realtimeLightmapIndex > -1;
								if ((flag2 || flag3) && !flag)
								{
									commandBuffer.EnableShaderKeyword("LIGHTMAP_ON");
									if (flag2)
									{
										commandBuffer.SetGlobalVector("unity_LightmapST", this.Renderers[num3].lightmapScaleOffset);
									}
									if (flag3)
									{
										commandBuffer.EnableShaderKeyword("DYNAMICLIGHTMAP_ON");
										commandBuffer.SetGlobalVector("unity_DynamicLightmapST", this.Renderers[num3].realtimeLightmapScaleOffset);
									}
									else
									{
										commandBuffer.DisableShaderKeyword("DYNAMICLIGHTMAP_ON");
									}
									if (flag2 && flag3)
									{
										commandBuffer.EnableShaderKeyword("DIRLIGHTMAP_COMBINED");
									}
									else
									{
										commandBuffer.DisableShaderKeyword("DIRLIGHTMAP_COMBINED");
									}
								}
								else
								{
									commandBuffer.DisableShaderKeyword("LIGHTMAP_ON");
									commandBuffer.DisableShaderKeyword("DYNAMICLIGHTMAP_ON");
									commandBuffer.DisableShaderKeyword("DIRLIGHTMAP_COMBINED");
								}
								commandBuffer.DisableShaderKeyword("LIGHTPROBE_SH");
								if (impostorMaps)
								{
									if (num6 > -1)
									{
										commandBuffer.DrawRenderer(this.Renderers[num3], material, num4, num6);
									}
									commandBuffer.DrawRenderer(this.Renderers[num3], material, num4, num5);
								}
								if (combinedAlphas)
								{
									if (num6 > -1)
									{
										commandBuffer2.DrawRenderer(this.Renderers[num3], material, num4, num6);
									}
									commandBuffer2.DrawRenderer(this.Renderers[num3], material, num4, num5);
								}
							}
						}
					}
					if (impostorMaps)
					{
						Graphics.ExecuteCommandBuffer(commandBuffer);
					}
					if (combinedAlphas)
					{
						Graphics.ExecuteCommandBuffer(commandBuffer2);
					}
				}
			}
			list.Clear();
			foreach (KeyValuePair<Material, Material> keyValuePair in dictionary)
			{
				Material value = keyValuePair.Value;
				if (value != null)
				{
					if (!Application.isPlaying)
					{
						Object.DestroyImmediate(value);
					}
				}
			}
			dictionary.Clear();
			commandBuffer.Release();
			commandBuffer = null;
			commandBuffer2.Release();
			commandBuffer2 = null;
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x00069FE0 File Offset: 0x000681E0
		private Matrix4x4 GetCameraRotationMatrix(ImpostorType impostorType, int hframes, int vframes, int x, int y)
		{
			Matrix4x4 matrix4x = Matrix4x4.identity;
			if (impostorType == ImpostorType.Spherical)
			{
				float num = 0f;
				if (vframes > 0)
				{
					num = -(180f / (float)vframes);
				}
				Quaternion quaternion = Quaternion.Euler(num * (float)y + 90f, 0f, 0f);
				Quaternion quaternion2 = Quaternion.Euler(0f, 360f / (float)hframes * (float)x + -90f, 0f);
				matrix4x = Matrix4x4.Rotate(quaternion * quaternion2);
			}
			else if (impostorType == ImpostorType.Octahedron)
			{
				Vector3 vector = this.OctahedronToVector((float)x / ((float)hframes - 1f) * 2f - 1f, (float)y / ((float)vframes - 1f) * 2f - 1f);
				matrix4x = Matrix4x4.Rotate(Quaternion.LookRotation(new Vector3(vector.x * -1f, vector.z * -1f, vector.y * -1f), Vector3.up)).inverse;
			}
			else if (impostorType == ImpostorType.HemiOctahedron)
			{
				Vector3 vector2 = this.HemiOctahedronToVector((float)x / ((float)hframes - 1f) * 2f - 1f, (float)y / ((float)vframes - 1f) * 2f - 1f);
				matrix4x = Matrix4x4.Rotate(Quaternion.LookRotation(new Vector3(vector2.x * -1f, vector2.z * -1f, vector2.y * -1f), Vector3.up)).inverse;
			}
			return matrix4x;
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0006A15C File Offset: 0x0006835C
		private Vector3 OctahedronToVector(Vector2 oct)
		{
			Vector3 vector = new Vector3(oct.x, oct.y, 1f - Mathf.Abs(oct.x) - Mathf.Abs(oct.y));
			float num = Mathf.Clamp01(-vector.z);
			vector.Set(vector.x + ((vector.x >= 0f) ? (-num) : num), vector.y + ((vector.y >= 0f) ? (-num) : num), vector.z);
			vector = Vector3.Normalize(vector);
			return vector;
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0006A1F0 File Offset: 0x000683F0
		private Vector3 OctahedronToVector(float x, float y)
		{
			Vector3 vector = new Vector3(x, y, 1f - Mathf.Abs(x) - Mathf.Abs(y));
			float num = Mathf.Clamp01(-vector.z);
			vector.Set(vector.x + ((vector.x >= 0f) ? (-num) : num), vector.y + ((vector.y >= 0f) ? (-num) : num), vector.z);
			vector = Vector3.Normalize(vector);
			return vector;
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x0006A270 File Offset: 0x00068470
		private Vector3 HemiOctahedronToVector(float x, float y)
		{
			float num = x;
			float num2 = y;
			x = (num + num2) * 0.5f;
			y = (num - num2) * 0.5f;
			return Vector3.Normalize(new Vector3(x, y, 1f - Mathf.Abs(x) - Mathf.Abs(y)));
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0006A2B4 File Offset: 0x000684B4
		public void GenerateAutomaticMesh(AmplifyImpostorAsset data)
		{
			Rect rect = new Rect(0f, 0f, (float)this.m_alphaTex.width, (float)this.m_alphaTex.height);
			Vector2[][] array;
			SpriteUtilityEx.GenerateOutline(this.m_alphaTex, rect, data.Tolerance, 254, false, out array);
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				num += array[i].Length;
			}
			data.ShapePoints = new Vector2[num];
			int num2 = 0;
			for (int j = 0; j < array.Length; j++)
			{
				for (int k = 0; k < array[j].Length; k++)
				{
					data.ShapePoints[num2] = array[j][k] + new Vector2((float)this.m_alphaTex.width * 0.5f, (float)this.m_alphaTex.height * 0.5f);
					data.ShapePoints[num2] = Vector2.Scale(data.ShapePoints[num2], new Vector2(1f / (float)this.m_alphaTex.width, 1f / (float)this.m_alphaTex.height));
					num2++;
				}
			}
			data.ShapePoints = Vector2Ex.ConvexHull(data.ShapePoints);
			data.ShapePoints = Vector2Ex.ReduceVertices(data.ShapePoints, data.MaxVertices);
			data.ShapePoints = Vector2Ex.ScaleAlongNormals(data.ShapePoints, data.NormalScale);
			for (int l = 0; l < data.ShapePoints.Length; l++)
			{
				data.ShapePoints[l].x = Mathf.Clamp01(data.ShapePoints[l].x);
				data.ShapePoints[l].y = Mathf.Clamp01(data.ShapePoints[l].y);
			}
			data.ShapePoints = Vector2Ex.ConvexHull(data.ShapePoints);
			for (int m = 0; m < data.ShapePoints.Length; m++)
			{
				data.ShapePoints[m] = new Vector2(data.ShapePoints[m].x, 1f - data.ShapePoints[m].y);
			}
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x0006A500 File Offset: 0x00068700
		public Mesh GenerateMesh(Vector2[] points, Vector3 offset, float width = 1f, float height = 1f, bool invertY = true)
		{
			Vector2[] array = new Vector2[points.Length];
			Vector2[] array2 = new Vector2[points.Length];
			Array.Copy(points, array, points.Length);
			float num = width * 0.5f;
			float num2 = height * 0.5f;
			if (invertY)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new Vector2(array[i].x, 1f - array[i].y);
				}
			}
			Array.Copy(array, array2, array.Length);
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = new Vector2(array[j].x * width - num + this.m_pixelOffset.x, array[j].y * height - num2 + this.m_pixelOffset.y);
			}
			Triangulator triangulator = new Triangulator(array);
			int[] array3 = triangulator.Triangulate();
			Vector3[] array4 = new Vector3[triangulator.Points.Count];
			for (int k = 0; k < array4.Length; k++)
			{
				array4[k] = new Vector3(triangulator.Points[k].x, triangulator.Points[k].y, 0f);
			}
			Mesh mesh = new Mesh();
			mesh.vertices = array4;
			mesh.uv = array2;
			mesh.triangles = array3;
			mesh.RecalculateNormals();
			mesh.bounds = new Bounds(offset, this.m_originalBound.size);
			return mesh;
		}

		// Token: 0x04000FA6 RID: 4006
		private const string ShaderGUID = "e82933f4c0eb9ba42aab0739f48efe21";

		// Token: 0x04000FA7 RID: 4007
		private const string DilateGUID = "57c23892d43bc9f458360024c5985405";

		// Token: 0x04000FA8 RID: 4008
		private const string PackerGUID = "31bd3cd74692f384a916d9d7ea87710d";

		// Token: 0x04000FA9 RID: 4009
		private const string ShaderOctaGUID = "572f9be5706148142b8da6e9de53acdb";

		// Token: 0x04000FAA RID: 4010
		private const string StandardPreset = "e4786beb7716da54dbb02a632681cc37";

		// Token: 0x04000FAB RID: 4011
		private const string LWPreset = "089f3a2f6b5f48348a48c755f8d9a7a2";

		// Token: 0x04000FAC RID: 4012
		private const string LWShaderOctaGUID = "94e2ddcdfb3257a43872042f97e2fb01";

		// Token: 0x04000FAD RID: 4013
		private const string LWShaderGUID = "990451a2073f6994ebf9fd6f90a842b3";

		// Token: 0x04000FAE RID: 4014
		private const string HDPreset = "47b6b3dcefe0eaf4997acf89caf8c75e";

		// Token: 0x04000FAF RID: 4015
		private const string HDShaderOctaGUID = "56236dc63ad9b7949b63a27f0ad180b3";

		// Token: 0x04000FB0 RID: 4016
		private const string HDShaderGUID = "175c951fec709c44fa2f26b8ab78b8dd";

		// Token: 0x04000FB1 RID: 4017
		private const string UPreset = "0403878495ffa3c4e9d4bcb3eac9b559";

		// Token: 0x04000FB2 RID: 4018
		private const string UShaderOctaGUID = "83dd8de9a5c14874884f9012def4fdcc";

		// Token: 0x04000FB3 RID: 4019
		private const string UShaderGUID = "da79d698f4bf0164e910ad798d07efdf";

		// Token: 0x04000FB4 RID: 4020
		[SerializeField]
		private AmplifyImpostorAsset m_data;

		// Token: 0x04000FB5 RID: 4021
		[SerializeField]
		private Transform m_rootTransform;

		// Token: 0x04000FB6 RID: 4022
		[SerializeField]
		private LODGroup m_lodGroup;

		// Token: 0x04000FB7 RID: 4023
		[SerializeField]
		private Renderer[] m_renderers;

		// Token: 0x04000FB8 RID: 4024
		public LODReplacement m_lodReplacement = LODReplacement.ReplaceLast;

		// Token: 0x04000FB9 RID: 4025
		[SerializeField]
		public RenderPipelineInUse m_renderPipelineInUse;

		// Token: 0x04000FBA RID: 4026
		public int m_insertIndex = 1;

		// Token: 0x04000FBB RID: 4027
		[SerializeField]
		public GameObject m_lastImpostor;

		// Token: 0x04000FBC RID: 4028
		[SerializeField]
		public string m_folderPath;

		// Token: 0x04000FBD RID: 4029
		[NonSerialized]
		public string m_impostorName = string.Empty;

		// Token: 0x04000FBE RID: 4030
		[SerializeField]
		public CutMode m_cutMode;

		// Token: 0x04000FBF RID: 4031
		[NonSerialized]
		private const float StartXRotation = -90f;

		// Token: 0x04000FC0 RID: 4032
		[NonSerialized]
		private const float StartYRotation = 90f;

		// Token: 0x04000FC1 RID: 4033
		[NonSerialized]
		private const int MinAlphaResolution = 256;

		// Token: 0x04000FC2 RID: 4034
		[NonSerialized]
		private RenderTexture[] m_rtGBuffers;

		// Token: 0x04000FC3 RID: 4035
		[NonSerialized]
		private RenderTexture[] m_alphaGBuffers;

		// Token: 0x04000FC4 RID: 4036
		[NonSerialized]
		private RenderTexture m_trueDepth;

		// Token: 0x04000FC5 RID: 4037
		[NonSerialized]
		public Texture2D m_alphaTex;

		// Token: 0x04000FC6 RID: 4038
		[NonSerialized]
		private float m_xyFitSize;

		// Token: 0x04000FC7 RID: 4039
		[NonSerialized]
		private float m_depthFitSize;

		// Token: 0x04000FC8 RID: 4040
		[NonSerialized]
		private Vector2 m_pixelOffset = Vector2.zero;

		// Token: 0x04000FC9 RID: 4041
		[NonSerialized]
		private Bounds m_originalBound;

		// Token: 0x04000FCA RID: 4042
		[NonSerialized]
		private Vector3 m_oriPos = Vector3.zero;

		// Token: 0x04000FCB RID: 4043
		[NonSerialized]
		private Quaternion m_oriRot = Quaternion.identity;

		// Token: 0x04000FCC RID: 4044
		[NonSerialized]
		private Vector3 m_oriSca = Vector3.one;

		// Token: 0x04000FCD RID: 4045
		[NonSerialized]
		private const int BlockSize = 65536;
	}
}
