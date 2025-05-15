using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020004AD RID: 1197
public class ShadowCamera
{
	// Token: 0x06001B88 RID: 7048 RVA: 0x000E4338 File Offset: 0x000E2538
	[return: TupleElementNames(new string[] { "viewMatrix", "projectionMatrix" })]
	public static ValueTuple<Matrix4x4, Matrix4x4> RenderShadowMap(Light light, Bounds groupBounds, ref RenderTexture shadowMap, int shadowIndex)
	{
		Camera camera = new GameObject("Shadow Camera").AddComponent<Camera>();
		camera.transform.SetPositionAndRotation(light.transform.position, light.transform.rotation);
		camera.nearClipPlane = 0.01f;
		camera.cullingMask = light.cullingMask;
		camera.clearFlags = CameraClearFlags.Color;
		camera.backgroundColor = Color.black;
		RenderTexture renderTexture = new RenderTexture(shadowMap.width, shadowMap.height, 24, shadowMap.format);
		if (light.type == LightType.Directional)
		{
			renderTexture.dimension = TextureDimension.Tex2D;
			renderTexture.useMipMap = false;
			ValueTuple<Vector3, Vector3> valueTuple = ShadowCamera.CalculateCameraParams(light.transform, groupBounds);
			Vector3 item = valueTuple.Item1;
			Vector3 item2 = valueTuple.Item2;
			camera.transform.position = item;
			camera.orthographic = true;
			camera.orthographicSize = item2.x;
			camera.nearClipPlane = item2.y;
			camera.farClipPlane = item2.z;
			camera.targetTexture = renderTexture;
			camera.backgroundColor = new Color(-9999f, -9999f, -9999f);
			Shader shader = Shader.Find("ULTRAKILL/Shadowmap_Directional");
			camera.SetReplacementShader(shader, "RenderType");
			camera.Render();
			Graphics.CopyTexture(renderTexture, 0, shadowMap, shadowIndex);
		}
		else
		{
			renderTexture.dimension = TextureDimension.Cube;
			renderTexture.useMipMap = false;
			camera.farClipPlane = light.range * 2f;
			camera.backgroundColor = new Color(9999f, 9999f, 9999f);
			Shader shader2 = Shader.Find("ULTRAKILL/Shadowmap_PointSpot");
			camera.SetReplacementShader(shader2, "RenderType");
			Shader.SetGlobalVector("_LightPos", camera.transform.position);
			camera.RenderToCubemap(renderTexture);
			for (int i = 0; i < 6; i++)
			{
				int num = 6 * shadowIndex + i;
				Graphics.CopyTexture(renderTexture, i, 0, shadowMap, num, 0);
			}
		}
		renderTexture.Release();
		Matrix4x4 worldToCameraMatrix = camera.worldToCameraMatrix;
		Matrix4x4 projectionMatrix = camera.projectionMatrix;
		camera.targetTexture = null;
		camera.ResetReplacementShader();
		Object.DestroyImmediate(camera.gameObject);
		return new ValueTuple<Matrix4x4, Matrix4x4>(worldToCameraMatrix, projectionMatrix);
	}

	// Token: 0x06001B89 RID: 7049 RVA: 0x000E4548 File Offset: 0x000E2748
	public static Bounds CalculateGroupBounds(Renderer[] rends)
	{
		Bounds bounds = default(Bounds);
		foreach (Renderer renderer in rends)
		{
			bounds.Encapsulate(renderer.bounds);
		}
		return bounds;
	}

	// Token: 0x06001B8A RID: 7050 RVA: 0x000E4580 File Offset: 0x000E2780
	public static ValueTuple<Vector3, Vector3> CalculateCameraParams(Transform lightTransform, Bounds groupBounds)
	{
		Vector3[] boundsVertices = ShadowCamera.GetBoundsVertices(groupBounds);
		Bounds bounds = new Bounds(lightTransform.InverseTransformPoint(boundsVertices[0]), Vector3.zero);
		foreach (Vector3 vector in boundsVertices)
		{
			Vector3 vector2 = lightTransform.InverseTransformPoint(vector);
			bounds.Encapsulate(vector2);
		}
		Vector3 vector3 = lightTransform.TransformPoint(bounds.center);
		float num = Mathf.Max(bounds.extents.x, bounds.extents.y);
		float num2 = -bounds.extents.z;
		float z = bounds.extents.z;
		Vector3 vector4 = new Vector3(num, num2, z);
		return new ValueTuple<Vector3, Vector3>(vector3, vector4);
	}

	// Token: 0x06001B8B RID: 7051 RVA: 0x000E463C File Offset: 0x000E283C
	private static Vector3[] GetBoundsVertices(Bounds bounds)
	{
		return new Vector3[]
		{
			bounds.min,
			new Vector3(bounds.min.x, bounds.min.y, bounds.max.z),
			new Vector3(bounds.min.x, bounds.max.y, bounds.min.z),
			new Vector3(bounds.min.x, bounds.max.y, bounds.max.z),
			new Vector3(bounds.max.x, bounds.min.y, bounds.min.z),
			new Vector3(bounds.max.x, bounds.min.y, bounds.max.z),
			new Vector3(bounds.max.x, bounds.max.y, bounds.min.z),
			bounds.max
		};
	}
}
