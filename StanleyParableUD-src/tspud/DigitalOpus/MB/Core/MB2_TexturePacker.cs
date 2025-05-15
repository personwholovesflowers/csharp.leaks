﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000279 RID: 633
	public class MB2_TexturePacker
	{
		// Token: 0x06000EFE RID: 3838 RVA: 0x000485F4 File Offset: 0x000467F4
		private static void printTree(MB2_TexturePacker.Node r, string spc)
		{
			Debug.Log(string.Concat(new object[]
			{
				spc,
				"Nd img=",
				(r.img != null).ToString(),
				" r=",
				r.r
			}));
			if (r.child[0] != null)
			{
				MB2_TexturePacker.printTree(r.child[0], spc + "      ");
			}
			if (r.child[1] != null)
			{
				MB2_TexturePacker.printTree(r.child[1], spc + "      ");
			}
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x00048688 File Offset: 0x00046888
		private static void flattenTree(MB2_TexturePacker.Node r, List<MB2_TexturePacker.Image> putHere)
		{
			if (r.img != null)
			{
				r.img.x = r.r.x;
				r.img.y = r.r.y;
				putHere.Add(r.img);
			}
			if (r.child[0] != null)
			{
				MB2_TexturePacker.flattenTree(r.child[0], putHere);
			}
			if (r.child[1] != null)
			{
				MB2_TexturePacker.flattenTree(r.child[1], putHere);
			}
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00048708 File Offset: 0x00046908
		private static void drawGizmosNode(MB2_TexturePacker.Node r)
		{
			Vector3 vector = new Vector3((float)r.r.w, (float)r.r.h, 0f);
			Vector3 vector2 = new Vector3((float)r.r.x + vector.x / 2f, (float)(-(float)r.r.y) - vector.y / 2f, 0f);
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube(vector2, vector);
			if (r.img != null)
			{
				Gizmos.color = new Color(Random.value, Random.value, Random.value);
				vector = new Vector3((float)r.img.w, (float)r.img.h, 0f);
				Gizmos.DrawCube(new Vector3((float)r.r.x + vector.x / 2f, (float)(-(float)r.r.y) - vector.y / 2f, 0f), vector);
			}
			if (r.child[0] != null)
			{
				Gizmos.color = Color.red;
				MB2_TexturePacker.drawGizmosNode(r.child[0]);
			}
			if (r.child[1] != null)
			{
				Gizmos.color = Color.green;
				MB2_TexturePacker.drawGizmosNode(r.child[1]);
			}
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x00048854 File Offset: 0x00046A54
		private static Texture2D createFilledTex(Color c, int w, int h)
		{
			Texture2D texture2D = new Texture2D(w, h);
			for (int i = 0; i < w; i++)
			{
				for (int j = 0; j < h; j++)
				{
					texture2D.SetPixel(i, j, c);
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00048894 File Offset: 0x00046A94
		public void DrawGizmos()
		{
			if (this.bestRoot != null)
			{
				MB2_TexturePacker.drawGizmosNode(this.bestRoot.root);
				Gizmos.color = Color.yellow;
				Vector3 vector = new Vector3((float)this.bestRoot.outW, (float)(-(float)this.bestRoot.outH), 0f);
				Gizmos.DrawWireCube(new Vector3(vector.x / 2f, vector.y / 2f, 0f), vector);
			}
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00048910 File Offset: 0x00046B10
		private bool ProbeSingleAtlas(MB2_TexturePacker.Image[] imgsToAdd, int idealAtlasW, int idealAtlasH, float imgArea, int maxAtlasDim, MB2_TexturePacker.ProbeResult pr)
		{
			MB2_TexturePacker.Node node = new MB2_TexturePacker.Node(MB2_TexturePacker.NodeType.maxDim);
			node.r = new MB2_TexturePacker.PixRect(0, 0, idealAtlasW, idealAtlasH);
			for (int i = 0; i < imgsToAdd.Length; i++)
			{
				if (node.Insert(imgsToAdd[i], false) == null)
				{
					return false;
				}
				if (i == imgsToAdd.Length - 1)
				{
					int num = 0;
					int num2 = 0;
					this.GetExtent(node, ref num, ref num2);
					int num3 = num;
					int num4 = num2;
					bool flag;
					float num8;
					float num9;
					if (this.doPowerOfTwoTextures)
					{
						num3 = Mathf.Min(MB2_TexturePacker.CeilToNearestPowerOfTwo(num), maxAtlasDim);
						num4 = Mathf.Min(MB2_TexturePacker.CeilToNearestPowerOfTwo(num2), maxAtlasDim);
						if (num4 < num3 / 2)
						{
							num4 = num3 / 2;
						}
						if (num3 < num4 / 2)
						{
							num3 = num4 / 2;
						}
						flag = num <= maxAtlasDim && num2 <= maxAtlasDim;
						float num5 = Mathf.Max(1f, (float)num / (float)maxAtlasDim);
						float num6 = Mathf.Max(1f, (float)num2 / (float)maxAtlasDim);
						float num7 = (float)num3 * num5 * (float)num4 * num6;
						num8 = 1f - (num7 - imgArea) / num7;
						num9 = 1f;
					}
					else
					{
						num8 = 1f - ((float)(num * num2) - imgArea) / (float)(num * num2);
						if (num < num2)
						{
							num9 = (float)num / (float)num2;
						}
						else
						{
							num9 = (float)num2 / (float)num;
						}
						flag = num <= maxAtlasDim && num2 <= maxAtlasDim;
					}
					pr.Set(num, num2, num3, num4, node, flag, num8, num9);
					if (this.LOG_LEVEL >= MB2_LogLevel.debug)
					{
						MB2_Log.LogDebug(string.Concat(new object[]
						{
							"Probe success efficiency w=",
							num,
							" h=",
							num2,
							" e=",
							num8,
							" sq=",
							num9,
							" fits=",
							flag.ToString()
						}), Array.Empty<object>());
					}
					return true;
				}
			}
			Debug.LogError("Should never get here.");
			return false;
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x00048AEC File Offset: 0x00046CEC
		private bool ProbeMultiAtlas(MB2_TexturePacker.Image[] imgsToAdd, int idealAtlasW, int idealAtlasH, float imgArea, int maxAtlasDim, MB2_TexturePacker.ProbeResult pr)
		{
			int num = 0;
			MB2_TexturePacker.Node node = new MB2_TexturePacker.Node(MB2_TexturePacker.NodeType.maxDim);
			node.r = new MB2_TexturePacker.PixRect(0, 0, idealAtlasW, idealAtlasH);
			for (int i = 0; i < imgsToAdd.Length; i++)
			{
				if (node.Insert(imgsToAdd[i], false) == null)
				{
					if (imgsToAdd[i].x > idealAtlasW && imgsToAdd[i].y > idealAtlasH)
					{
						return false;
					}
					MB2_TexturePacker.Node node2 = new MB2_TexturePacker.Node(MB2_TexturePacker.NodeType.Container);
					node2.r = new MB2_TexturePacker.PixRect(0, 0, node.r.w + idealAtlasW, idealAtlasH);
					MB2_TexturePacker.Node node3 = new MB2_TexturePacker.Node(MB2_TexturePacker.NodeType.maxDim);
					node3.r = new MB2_TexturePacker.PixRect(node.r.w, 0, idealAtlasW, idealAtlasH);
					node2.child[1] = node3;
					node2.child[0] = node;
					node = node2;
					node.Insert(imgsToAdd[i], false);
					num++;
				}
			}
			pr.numAtlases = num;
			pr.root = node;
			pr.totalAtlasArea = (float)(num * maxAtlasDim * maxAtlasDim);
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				MB2_Log.LogDebug(string.Concat(new object[] { "Probe success efficiency numAtlases=", num, " totalArea=", pr.totalAtlasArea }), Array.Empty<object>());
			}
			return true;
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x00048C14 File Offset: 0x00046E14
		private void GetExtent(MB2_TexturePacker.Node r, ref int x, ref int y)
		{
			if (r.img != null)
			{
				if (r.r.x + r.img.w > x)
				{
					x = r.r.x + r.img.w;
				}
				if (r.r.y + r.img.h > y)
				{
					y = r.r.y + r.img.h;
				}
			}
			if (r.child[0] != null)
			{
				this.GetExtent(r.child[0], ref x, ref y);
			}
			if (r.child[1] != null)
			{
				this.GetExtent(r.child[1], ref x, ref y);
			}
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x00048CC8 File Offset: 0x00046EC8
		private int StepWidthHeight(int oldVal, int step, int maxDim)
		{
			if (this.doPowerOfTwoTextures && oldVal < maxDim)
			{
				return oldVal * 2;
			}
			int num = oldVal + step;
			if (num > maxDim && oldVal < maxDim)
			{
				num = maxDim;
			}
			return num;
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x00048CF4 File Offset: 0x00046EF4
		public static int RoundToNearestPositivePowerOfTwo(int x)
		{
			int num = (int)Mathf.Pow(2f, (float)Mathf.RoundToInt(Mathf.Log((float)x) / Mathf.Log(2f)));
			if (num == 0 || num == 1)
			{
				num = 2;
			}
			return num;
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00048D30 File Offset: 0x00046F30
		public static int CeilToNearestPowerOfTwo(int x)
		{
			int num = (int)Mathf.Pow(2f, Mathf.Ceil(Mathf.Log((float)x) / Mathf.Log(2f)));
			if (num == 0 || num == 1)
			{
				num = 2;
			}
			return num;
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x00048D6A File Offset: 0x00046F6A
		public AtlasPackingResult[] GetRects(List<Vector2> imgWidthHeights, int maxDimension, int padding)
		{
			return this.GetRects(imgWidthHeights, maxDimension, padding, false);
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x00048D78 File Offset: 0x00046F78
		public AtlasPackingResult[] GetRects(List<Vector2> imgWidthHeights, int maxDimension, int padding, bool doMultiAtlas)
		{
			if (doMultiAtlas)
			{
				return this._GetRectsMultiAtlas(imgWidthHeights, maxDimension, padding, 2 + padding * 2, 2 + padding * 2, 2 + padding * 2, 2 + padding * 2);
			}
			AtlasPackingResult atlasPackingResult = this._GetRectsSingleAtlas(imgWidthHeights, maxDimension, padding, 2 + padding * 2, 2 + padding * 2, 2 + padding * 2, 2 + padding * 2, 0);
			if (atlasPackingResult == null)
			{
				return null;
			}
			return new AtlasPackingResult[] { atlasPackingResult };
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x00048DD8 File Offset: 0x00046FD8
		private AtlasPackingResult _GetRectsSingleAtlas(List<Vector2> imgWidthHeights, int maxDimension, int padding, int minImageSizeX, int minImageSizeY, int masterImageSizeX, int masterImageSizeY, int recursionDepth)
		{
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				Debug.Log(string.Format("_GetRects numImages={0}, maxDimension={1}, padding={2}, minImageSizeX={3}, minImageSizeY={4}, masterImageSizeX={5}, masterImageSizeY={6}, recursionDepth={7}", new object[] { imgWidthHeights.Count, maxDimension, padding, minImageSizeX, minImageSizeY, masterImageSizeX, masterImageSizeY, recursionDepth }));
			}
			if (recursionDepth > 10)
			{
				if (this.LOG_LEVEL >= MB2_LogLevel.error)
				{
					Debug.LogError("Maximum recursion depth reached. Couldn't find packing for these textures.");
				}
				return null;
			}
			float num = 0f;
			int num2 = 0;
			int num3 = 0;
			MB2_TexturePacker.Image[] array = new MB2_TexturePacker.Image[imgWidthHeights.Count];
			for (int i = 0; i < array.Length; i++)
			{
				int num4 = (int)imgWidthHeights[i].x;
				int num5 = (int)imgWidthHeights[i].y;
				MB2_TexturePacker.Image image = (array[i] = new MB2_TexturePacker.Image(i, num4, num5, padding, minImageSizeX, minImageSizeY));
				num += (float)(image.w * image.h);
				num2 = Mathf.Max(num2, image.w);
				num3 = Mathf.Max(num3, image.h);
			}
			if ((float)num3 / (float)num2 > 2f)
			{
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					MB2_Log.LogDebug("Using height Comparer", Array.Empty<object>());
				}
				Array.Sort<MB2_TexturePacker.Image>(array, new MB2_TexturePacker.ImageHeightComparer());
			}
			else if ((double)((float)num3 / (float)num2) < 0.5)
			{
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					MB2_Log.LogDebug("Using width Comparer", Array.Empty<object>());
				}
				Array.Sort<MB2_TexturePacker.Image>(array, new MB2_TexturePacker.ImageWidthComparer());
			}
			else
			{
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					MB2_Log.LogDebug("Using area Comparer", Array.Empty<object>());
				}
				Array.Sort<MB2_TexturePacker.Image>(array, new MB2_TexturePacker.ImageAreaComparer());
			}
			int num6 = (int)Mathf.Sqrt(num);
			int num8;
			int num7;
			if (this.doPowerOfTwoTextures)
			{
				num7 = (num8 = MB2_TexturePacker.RoundToNearestPositivePowerOfTwo(num6));
				if (num2 > num8)
				{
					num8 = MB2_TexturePacker.CeilToNearestPowerOfTwo(num8);
				}
				if (num3 > num7)
				{
					num7 = MB2_TexturePacker.CeilToNearestPowerOfTwo(num7);
				}
			}
			else
			{
				num8 = num6;
				num7 = num6;
				if (num2 > num6)
				{
					num8 = num2;
					num7 = Mathf.Max(Mathf.CeilToInt(num / (float)num2), num3);
				}
				if (num3 > num6)
				{
					num8 = Mathf.Max(Mathf.CeilToInt(num / (float)num3), num2);
					num7 = num3;
				}
			}
			if (num8 == 0)
			{
				num8 = 4;
			}
			if (num7 == 0)
			{
				num7 = 4;
			}
			int num9 = (int)((float)num8 * 0.15f);
			int num10 = (int)((float)num7 * 0.15f);
			if (num9 == 0)
			{
				num9 = 1;
			}
			if (num10 == 0)
			{
				num10 = 1;
			}
			int num11 = 2;
			int num12 = num7;
			while (num11 >= 1 && num12 < num6 * 1000)
			{
				bool flag = false;
				num11 = 0;
				int num13 = num8;
				while (!flag && num13 < num6 * 1000)
				{
					MB2_TexturePacker.ProbeResult probeResult = new MB2_TexturePacker.ProbeResult();
					if (this.LOG_LEVEL >= MB2_LogLevel.trace)
					{
						Debug.Log(string.Concat(new object[] { "Probing h=", num12, " w=", num13 }));
					}
					if (this.ProbeSingleAtlas(array, num13, num12, num, maxDimension, probeResult))
					{
						flag = true;
						if (this.bestRoot == null)
						{
							this.bestRoot = probeResult;
						}
						else if (probeResult.GetScore(this.doPowerOfTwoTextures) > this.bestRoot.GetScore(this.doPowerOfTwoTextures))
						{
							this.bestRoot = probeResult;
						}
					}
					else
					{
						num11++;
						num13 = this.StepWidthHeight(num13, num9, maxDimension);
						if (this.LOG_LEVEL >= MB2_LogLevel.trace)
						{
							MB2_Log.LogDebug(string.Concat(new object[] { "increasing Width h=", num12, " w=", num13 }), Array.Empty<object>());
						}
					}
				}
				num12 = this.StepWidthHeight(num12, num10, maxDimension);
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					MB2_Log.LogDebug(string.Concat(new object[] { "increasing Height h=", num12, " w=", num13 }), Array.Empty<object>());
				}
			}
			if (this.bestRoot == null)
			{
				return null;
			}
			int num14;
			int num15;
			if (this.doPowerOfTwoTextures)
			{
				num14 = Mathf.Min(MB2_TexturePacker.CeilToNearestPowerOfTwo(this.bestRoot.w), maxDimension);
				num15 = Mathf.Min(MB2_TexturePacker.CeilToNearestPowerOfTwo(this.bestRoot.h), maxDimension);
				if (num15 < num14 / 2)
				{
					num15 = num14 / 2;
				}
				if (num14 < num15 / 2)
				{
					num14 = num15 / 2;
				}
			}
			else
			{
				num14 = Mathf.Min(this.bestRoot.w, maxDimension);
				num15 = Mathf.Min(this.bestRoot.h, maxDimension);
			}
			this.bestRoot.outW = num14;
			this.bestRoot.outH = num15;
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				Debug.Log(string.Concat(new object[]
				{
					"Best fit found: atlasW=",
					num14,
					" atlasH",
					num15,
					" w=",
					this.bestRoot.w,
					" h=",
					this.bestRoot.h,
					" efficiency=",
					this.bestRoot.efficiency,
					" squareness=",
					this.bestRoot.squareness,
					" fits in max dimension=",
					this.bestRoot.largerOrEqualToMaxDim.ToString()
				}));
			}
			List<MB2_TexturePacker.Image> list = new List<MB2_TexturePacker.Image>();
			MB2_TexturePacker.flattenTree(this.bestRoot.root, list);
			list.Sort(new MB2_TexturePacker.ImgIDComparer());
			AtlasPackingResult atlasPackingResult = this.ScaleAtlasToFitMaxDim(this.bestRoot, imgWidthHeights, list, maxDimension, padding, minImageSizeX, minImageSizeY, masterImageSizeX, masterImageSizeY, num14, num15, recursionDepth);
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				MB2_Log.LogDebug(string.Format("Done GetRects atlasW={0} atlasH={1}", this.bestRoot.w, this.bestRoot.h), Array.Empty<object>());
			}
			return atlasPackingResult;
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x000493B0 File Offset: 0x000475B0
		private AtlasPackingResult ScaleAtlasToFitMaxDim(MB2_TexturePacker.ProbeResult root, List<Vector2> imgWidthHeights, List<MB2_TexturePacker.Image> images, int maxDimension, int padding, int minImageSizeX, int minImageSizeY, int masterImageSizeX, int masterImageSizeY, int outW, int outH, int recursionDepth)
		{
			int num = minImageSizeX;
			int num2 = minImageSizeY;
			bool flag = false;
			float num3 = (float)padding / (float)outW;
			if (root.w > maxDimension)
			{
				num3 = (float)padding / (float)maxDimension;
				float num4 = (float)maxDimension / (float)root.w;
				if (this.LOG_LEVEL >= MB2_LogLevel.warn)
				{
					Debug.LogWarning("Packing exceeded atlas width shrinking to " + num4);
				}
				for (int i = 0; i < images.Count; i++)
				{
					MB2_TexturePacker.Image image = images[i];
					if ((float)image.w * num4 < (float)masterImageSizeX)
					{
						if (this.LOG_LEVEL >= MB2_LogLevel.debug)
						{
							Debug.Log("Small images are being scaled to zero. Will need to redo packing with larger minTexSizeX.");
						}
						flag = true;
						num = Mathf.CeilToInt((float)minImageSizeX / num4);
					}
					int num5 = (int)((float)(image.x + image.w) * num4);
					image.x = (int)(num4 * (float)image.x);
					image.w = num5 - image.x;
				}
				outW = maxDimension;
			}
			float num6 = (float)padding / (float)outH;
			if (root.h > maxDimension)
			{
				num6 = (float)padding / (float)maxDimension;
				float num7 = (float)maxDimension / (float)root.h;
				if (this.LOG_LEVEL >= MB2_LogLevel.warn)
				{
					Debug.LogWarning("Packing exceeded atlas height shrinking to " + num7);
				}
				for (int j = 0; j < images.Count; j++)
				{
					MB2_TexturePacker.Image image2 = images[j];
					if ((float)image2.h * num7 < (float)masterImageSizeY)
					{
						if (this.LOG_LEVEL >= MB2_LogLevel.debug)
						{
							Debug.Log("Small images are being scaled to zero. Will need to redo packing with larger minTexSizeY.");
						}
						flag = true;
						num2 = Mathf.CeilToInt((float)minImageSizeY / num7);
					}
					int num8 = (int)((float)(image2.y + image2.h) * num7);
					image2.y = (int)(num7 * (float)image2.y);
					image2.h = num8 - image2.y;
				}
				outH = maxDimension;
			}
			if (!flag)
			{
				AtlasPackingResult atlasPackingResult = new AtlasPackingResult();
				atlasPackingResult.rects = new Rect[images.Count];
				atlasPackingResult.srcImgIdxs = new int[images.Count];
				atlasPackingResult.atlasX = outW;
				atlasPackingResult.atlasY = outH;
				atlasPackingResult.usedW = -1;
				atlasPackingResult.usedH = -1;
				for (int k = 0; k < images.Count; k++)
				{
					MB2_TexturePacker.Image image3 = images[k];
					Rect rect = (atlasPackingResult.rects[k] = new Rect((float)image3.x / (float)outW + num3, (float)image3.y / (float)outH + num6, (float)image3.w / (float)outW - num3 * 2f, (float)image3.h / (float)outH - num6 * 2f));
					atlasPackingResult.srcImgIdxs[k] = image3.imgId;
					if (this.LOG_LEVEL >= MB2_LogLevel.debug)
					{
						MB2_Log.LogDebug(string.Concat(new object[]
						{
							"Image: ",
							k,
							" imgID=",
							image3.imgId,
							" x=",
							rect.x * (float)outW,
							" y=",
							rect.y * (float)outH,
							" w=",
							rect.width * (float)outW,
							" h=",
							rect.height * (float)outH,
							" padding=",
							padding
						}), Array.Empty<object>());
					}
				}
				return atlasPackingResult;
			}
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				Debug.Log("==================== REDOING PACKING ================");
			}
			root = null;
			return this._GetRectsSingleAtlas(imgWidthHeights, maxDimension, padding, num, num2, masterImageSizeX, masterImageSizeY, recursionDepth + 1);
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x00049764 File Offset: 0x00047964
		private AtlasPackingResult[] _GetRectsMultiAtlas(List<Vector2> imgWidthHeights, int maxDimensionPassed, int padding, int minImageSizeX, int minImageSizeY, int masterImageSizeX, int masterImageSizeY)
		{
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				Debug.Log(string.Format("_GetRects numImages={0}, maxDimension={1}, padding={2}, minImageSizeX={3}, minImageSizeY={4}, masterImageSizeX={5}, masterImageSizeY={6}", new object[] { imgWidthHeights.Count, maxDimensionPassed, padding, minImageSizeX, minImageSizeY, masterImageSizeX, masterImageSizeY }));
			}
			float num = 0f;
			int num2 = 0;
			int num3 = 0;
			MB2_TexturePacker.Image[] array = new MB2_TexturePacker.Image[imgWidthHeights.Count];
			int num4 = maxDimensionPassed;
			if (this.doPowerOfTwoTextures)
			{
				num4 = MB2_TexturePacker.RoundToNearestPositivePowerOfTwo(num4);
			}
			for (int i = 0; i < array.Length; i++)
			{
				int num5 = (int)imgWidthHeights[i].x;
				int num6 = (int)imgWidthHeights[i].y;
				num5 = Mathf.Min(num5, num4 - padding * 2);
				num6 = Mathf.Min(num6, num4 - padding * 2);
				MB2_TexturePacker.Image image = (array[i] = new MB2_TexturePacker.Image(i, num5, num6, padding, minImageSizeX, minImageSizeY));
				num += (float)(image.w * image.h);
				num2 = Mathf.Max(num2, image.w);
				num3 = Mathf.Max(num3, image.h);
			}
			int num7;
			int num8;
			if (this.doPowerOfTwoTextures)
			{
				num7 = MB2_TexturePacker.RoundToNearestPositivePowerOfTwo(num4);
				num8 = MB2_TexturePacker.RoundToNearestPositivePowerOfTwo(num4);
			}
			else
			{
				num7 = num4;
				num8 = num4;
			}
			if (num8 == 0)
			{
				num8 = 4;
			}
			if (num7 == 0)
			{
				num7 = 4;
			}
			MB2_TexturePacker.ProbeResult probeResult = new MB2_TexturePacker.ProbeResult();
			Array.Sort<MB2_TexturePacker.Image>(array, new MB2_TexturePacker.ImageHeightComparer());
			if (this.ProbeMultiAtlas(array, num8, num7, num, num4, probeResult))
			{
				this.bestRoot = probeResult;
			}
			Array.Sort<MB2_TexturePacker.Image>(array, new MB2_TexturePacker.ImageWidthComparer());
			if (this.ProbeMultiAtlas(array, num8, num7, num, num4, probeResult) && probeResult.totalAtlasArea < this.bestRoot.totalAtlasArea)
			{
				this.bestRoot = probeResult;
			}
			Array.Sort<MB2_TexturePacker.Image>(array, new MB2_TexturePacker.ImageAreaComparer());
			if (this.ProbeMultiAtlas(array, num8, num7, num, num4, probeResult) && probeResult.totalAtlasArea < this.bestRoot.totalAtlasArea)
			{
				this.bestRoot = probeResult;
			}
			if (this.bestRoot == null)
			{
				return null;
			}
			if (this.LOG_LEVEL >= MB2_LogLevel.debug)
			{
				Debug.Log(string.Concat(new object[]
				{
					"Best fit found: w=",
					this.bestRoot.w,
					" h=",
					this.bestRoot.h,
					" efficiency=",
					this.bestRoot.efficiency,
					" squareness=",
					this.bestRoot.squareness,
					" fits in max dimension=",
					this.bestRoot.largerOrEqualToMaxDim.ToString()
				}));
			}
			List<AtlasPackingResult> list = new List<AtlasPackingResult>();
			List<MB2_TexturePacker.Node> list2 = new List<MB2_TexturePacker.Node>();
			Stack<MB2_TexturePacker.Node> stack = new Stack<MB2_TexturePacker.Node>();
			for (MB2_TexturePacker.Node node = this.bestRoot.root; node != null; node = node.child[0])
			{
				stack.Push(node);
			}
			while (stack.Count > 0)
			{
				MB2_TexturePacker.Node node = stack.Pop();
				if (node.isFullAtlas == MB2_TexturePacker.NodeType.maxDim)
				{
					list2.Add(node);
				}
				if (node.child[1] != null)
				{
					for (node = node.child[1]; node != null; node = node.child[0])
					{
						stack.Push(node);
					}
				}
			}
			for (int j = 0; j < list2.Count; j++)
			{
				List<MB2_TexturePacker.Image> list3 = new List<MB2_TexturePacker.Image>();
				MB2_TexturePacker.flattenTree(list2[j], list3);
				Rect[] array2 = new Rect[list3.Count];
				int[] array3 = new int[list3.Count];
				for (int k = 0; k < list3.Count; k++)
				{
					array2[k] = new Rect((float)(list3[k].x - list2[j].r.x), (float)list3[k].y, (float)list3[k].w, (float)list3[k].h);
					array3[k] = list3[k].imgId;
				}
				AtlasPackingResult atlasPackingResult = new AtlasPackingResult();
				this.GetExtent(list2[j], ref atlasPackingResult.usedW, ref atlasPackingResult.usedH);
				atlasPackingResult.usedW -= list2[j].r.x;
				int num9 = list2[j].r.w;
				int num10 = list2[j].r.h;
				if (this.doPowerOfTwoTextures)
				{
					num9 = Mathf.Min(MB2_TexturePacker.CeilToNearestPowerOfTwo(atlasPackingResult.usedW), list2[j].r.w);
					num10 = Mathf.Min(MB2_TexturePacker.CeilToNearestPowerOfTwo(atlasPackingResult.usedH), list2[j].r.h);
					if (num10 < num9 / 2)
					{
						num10 = num9 / 2;
					}
					if (num9 < num10 / 2)
					{
						num9 = num10 / 2;
					}
				}
				else
				{
					num9 = atlasPackingResult.usedW;
					num10 = atlasPackingResult.usedH;
				}
				atlasPackingResult.atlasY = num10;
				atlasPackingResult.atlasX = num9;
				atlasPackingResult.rects = array2;
				atlasPackingResult.srcImgIdxs = array3;
				list.Add(atlasPackingResult);
				this.normalizeRects(atlasPackingResult, padding);
				if (this.LOG_LEVEL >= MB2_LogLevel.debug)
				{
					MB2_Log.LogDebug(string.Format("Done GetRects ", Array.Empty<object>()), Array.Empty<object>());
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x00049CE0 File Offset: 0x00047EE0
		private void normalizeRects(AtlasPackingResult rr, int padding)
		{
			for (int i = 0; i < rr.rects.Length; i++)
			{
				rr.rects[i].x = (rr.rects[i].x + (float)padding) / (float)rr.atlasX;
				rr.rects[i].y = (rr.rects[i].y + (float)padding) / (float)rr.atlasY;
				rr.rects[i].width = (rr.rects[i].width - (float)(padding * 2)) / (float)rr.atlasX;
				rr.rects[i].height = (rr.rects[i].height - (float)(padding * 2)) / (float)rr.atlasY;
			}
		}

		// Token: 0x04000D6A RID: 3434
		public MB2_LogLevel LOG_LEVEL = MB2_LogLevel.info;

		// Token: 0x04000D6B RID: 3435
		private MB2_TexturePacker.ProbeResult bestRoot;

		// Token: 0x04000D6C RID: 3436
		public int atlasY;

		// Token: 0x04000D6D RID: 3437
		public bool doPowerOfTwoTextures = true;

		// Token: 0x02000451 RID: 1105
		private enum NodeType
		{
			// Token: 0x0400160F RID: 5647
			Container,
			// Token: 0x04001610 RID: 5648
			maxDim,
			// Token: 0x04001611 RID: 5649
			regular
		}

		// Token: 0x02000452 RID: 1106
		private class PixRect
		{
			// Token: 0x060018E7 RID: 6375 RVA: 0x0000883F File Offset: 0x00006A3F
			public PixRect()
			{
			}

			// Token: 0x060018E8 RID: 6376 RVA: 0x0007C9E5 File Offset: 0x0007ABE5
			public PixRect(int xx, int yy, int ww, int hh)
			{
				this.x = xx;
				this.y = yy;
				this.w = ww;
				this.h = hh;
			}

			// Token: 0x060018E9 RID: 6377 RVA: 0x0007CA0C File Offset: 0x0007AC0C
			public override string ToString()
			{
				return string.Format("x={0},y={1},w={2},h={3}", new object[] { this.x, this.y, this.w, this.h });
			}

			// Token: 0x04001612 RID: 5650
			public int x;

			// Token: 0x04001613 RID: 5651
			public int y;

			// Token: 0x04001614 RID: 5652
			public int w;

			// Token: 0x04001615 RID: 5653
			public int h;
		}

		// Token: 0x02000453 RID: 1107
		private class Image
		{
			// Token: 0x060018EA RID: 6378 RVA: 0x0007CA61 File Offset: 0x0007AC61
			public Image(int id, int tw, int th, int padding, int minImageSizeX, int minImageSizeY)
			{
				this.imgId = id;
				this.w = Mathf.Max(tw + padding * 2, minImageSizeX);
				this.h = Mathf.Max(th + padding * 2, minImageSizeY);
			}

			// Token: 0x060018EB RID: 6379 RVA: 0x0007CA98 File Offset: 0x0007AC98
			public Image(MB2_TexturePacker.Image im)
			{
				this.imgId = im.imgId;
				this.w = im.w;
				this.h = im.h;
				this.x = im.x;
				this.y = im.y;
			}

			// Token: 0x04001616 RID: 5654
			public int imgId;

			// Token: 0x04001617 RID: 5655
			public int w;

			// Token: 0x04001618 RID: 5656
			public int h;

			// Token: 0x04001619 RID: 5657
			public int x;

			// Token: 0x0400161A RID: 5658
			public int y;
		}

		// Token: 0x02000454 RID: 1108
		private class ImgIDComparer : IComparer<MB2_TexturePacker.Image>
		{
			// Token: 0x060018EC RID: 6380 RVA: 0x0007CAE7 File Offset: 0x0007ACE7
			public int Compare(MB2_TexturePacker.Image x, MB2_TexturePacker.Image y)
			{
				if (x.imgId > y.imgId)
				{
					return 1;
				}
				if (x.imgId == y.imgId)
				{
					return 0;
				}
				return -1;
			}
		}

		// Token: 0x02000455 RID: 1109
		private class ImageHeightComparer : IComparer<MB2_TexturePacker.Image>
		{
			// Token: 0x060018EE RID: 6382 RVA: 0x0007CB0A File Offset: 0x0007AD0A
			public int Compare(MB2_TexturePacker.Image x, MB2_TexturePacker.Image y)
			{
				if (x.h > y.h)
				{
					return -1;
				}
				if (x.h == y.h)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x02000456 RID: 1110
		private class ImageWidthComparer : IComparer<MB2_TexturePacker.Image>
		{
			// Token: 0x060018F0 RID: 6384 RVA: 0x0007CB2D File Offset: 0x0007AD2D
			public int Compare(MB2_TexturePacker.Image x, MB2_TexturePacker.Image y)
			{
				if (x.w > y.w)
				{
					return -1;
				}
				if (x.w == y.w)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x02000457 RID: 1111
		private class ImageAreaComparer : IComparer<MB2_TexturePacker.Image>
		{
			// Token: 0x060018F2 RID: 6386 RVA: 0x0007CB50 File Offset: 0x0007AD50
			public int Compare(MB2_TexturePacker.Image x, MB2_TexturePacker.Image y)
			{
				int num = x.w * x.h;
				int num2 = y.w * y.h;
				if (num > num2)
				{
					return -1;
				}
				if (num == num2)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x02000458 RID: 1112
		private class ProbeResult
		{
			// Token: 0x060018F4 RID: 6388 RVA: 0x0007CB86 File Offset: 0x0007AD86
			public void Set(int ww, int hh, int outw, int outh, MB2_TexturePacker.Node r, bool fits, float e, float sq)
			{
				this.w = ww;
				this.h = hh;
				this.outW = outw;
				this.outH = outh;
				this.root = r;
				this.largerOrEqualToMaxDim = fits;
				this.efficiency = e;
				this.squareness = sq;
			}

			// Token: 0x060018F5 RID: 6389 RVA: 0x0007CBC8 File Offset: 0x0007ADC8
			public float GetScore(bool doPowerOfTwoScore)
			{
				float num = (this.largerOrEqualToMaxDim ? 1f : 0f);
				if (doPowerOfTwoScore)
				{
					return num * 2f + this.efficiency;
				}
				return this.squareness + 2f * this.efficiency + num;
			}

			// Token: 0x060018F6 RID: 6390 RVA: 0x0007CC11 File Offset: 0x0007AE11
			public void PrintTree()
			{
				MB2_TexturePacker.printTree(this.root, "  ");
			}

			// Token: 0x0400161B RID: 5659
			public int w;

			// Token: 0x0400161C RID: 5660
			public int h;

			// Token: 0x0400161D RID: 5661
			public int outW;

			// Token: 0x0400161E RID: 5662
			public int outH;

			// Token: 0x0400161F RID: 5663
			public MB2_TexturePacker.Node root;

			// Token: 0x04001620 RID: 5664
			public bool largerOrEqualToMaxDim;

			// Token: 0x04001621 RID: 5665
			public float efficiency;

			// Token: 0x04001622 RID: 5666
			public float squareness;

			// Token: 0x04001623 RID: 5667
			public float totalAtlasArea;

			// Token: 0x04001624 RID: 5668
			public int numAtlases;
		}

		// Token: 0x02000459 RID: 1113
		private class Node
		{
			// Token: 0x060018F8 RID: 6392 RVA: 0x0007CC23 File Offset: 0x0007AE23
			public Node(MB2_TexturePacker.NodeType rootType)
			{
				this.isFullAtlas = rootType;
			}

			// Token: 0x060018F9 RID: 6393 RVA: 0x0007CC3E File Offset: 0x0007AE3E
			private bool isLeaf()
			{
				return this.child[0] == null || this.child[1] == null;
			}

			// Token: 0x060018FA RID: 6394 RVA: 0x0007CC58 File Offset: 0x0007AE58
			public MB2_TexturePacker.Node Insert(MB2_TexturePacker.Image im, bool handed)
			{
				int num;
				int num2;
				if (handed)
				{
					num = 0;
					num2 = 1;
				}
				else
				{
					num = 1;
					num2 = 0;
				}
				if (!this.isLeaf())
				{
					MB2_TexturePacker.Node node = this.child[num].Insert(im, handed);
					if (node != null)
					{
						return node;
					}
					return this.child[num2].Insert(im, handed);
				}
				else
				{
					if (this.img != null)
					{
						return null;
					}
					if (this.r.w < im.w || this.r.h < im.h)
					{
						return null;
					}
					if (this.r.w == im.w && this.r.h == im.h)
					{
						this.img = im;
						return this;
					}
					this.child[num] = new MB2_TexturePacker.Node(MB2_TexturePacker.NodeType.regular);
					this.child[num2] = new MB2_TexturePacker.Node(MB2_TexturePacker.NodeType.regular);
					int num3 = this.r.w - im.w;
					int num4 = this.r.h - im.h;
					if (num3 > num4)
					{
						this.child[num].r = new MB2_TexturePacker.PixRect(this.r.x, this.r.y, im.w, this.r.h);
						this.child[num2].r = new MB2_TexturePacker.PixRect(this.r.x + im.w, this.r.y, this.r.w - im.w, this.r.h);
					}
					else
					{
						this.child[num].r = new MB2_TexturePacker.PixRect(this.r.x, this.r.y, this.r.w, im.h);
						this.child[num2].r = new MB2_TexturePacker.PixRect(this.r.x, this.r.y + im.h, this.r.w, this.r.h - im.h);
					}
					return this.child[num].Insert(im, handed);
				}
			}

			// Token: 0x04001625 RID: 5669
			public MB2_TexturePacker.NodeType isFullAtlas;

			// Token: 0x04001626 RID: 5670
			public MB2_TexturePacker.Node[] child = new MB2_TexturePacker.Node[2];

			// Token: 0x04001627 RID: 5671
			public MB2_TexturePacker.PixRect r;

			// Token: 0x04001628 RID: 5672
			public MB2_TexturePacker.Image img;
		}
	}
}
