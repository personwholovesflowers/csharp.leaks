using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ferr
{
	// Token: 0x020002EC RID: 748
	public class Juice : MonoBehaviour
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600136C RID: 4972 RVA: 0x0006776C File Offset: 0x0006596C
		private static Juice Instance
		{
			get
			{
				if (Juice.instance == null)
				{
					Juice.instance = Juice.Create();
				}
				return Juice.instance;
			}
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x0006778A File Offset: 0x0006598A
		private Juice()
		{
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x000677A8 File Offset: 0x000659A8
		private static Juice Create()
		{
			return new GameObject("JuiceDriver").AddComponent<Juice>();
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600136F RID: 4975 RVA: 0x000677BC File Offset: 0x000659BC
		public static AnimationCurve SproingIn
		{
			get
			{
				return new AnimationCurve(new Keyframe[]
				{
					new Keyframe(0f, 0f),
					new Keyframe(0.7f, 1.1f),
					new Keyframe(0.85f, 0.9f),
					new Keyframe(1f, 1f)
				});
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06001370 RID: 4976 RVA: 0x0006782C File Offset: 0x00065A2C
		public static AnimationCurve FastFalloff
		{
			get
			{
				return new AnimationCurve(new Keyframe[]
				{
					new Keyframe(0f, 0f, 1f, 1f),
					new Keyframe(0.25f, 0.8f, 1f, 1f),
					new Keyframe(1f, 1f)
				});
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x0006789C File Offset: 0x00065A9C
		public static AnimationCurve LateFalloff
		{
			get
			{
				return new AnimationCurve(new Keyframe[]
				{
					new Keyframe(0f, 0f),
					new Keyframe(0.75f, 0.25f),
					new Keyframe(1f, 1f)
				});
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06001372 RID: 4978 RVA: 0x000678F8 File Offset: 0x00065AF8
		public static AnimationCurve Wobble
		{
			get
			{
				return new AnimationCurve(new Keyframe[]
				{
					new Keyframe(0f, 0f),
					new Keyframe(0.25f, 1f),
					new Keyframe(0.75f, -1f),
					new Keyframe(1f, 0f)
				});
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x00067968 File Offset: 0x00065B68
		public static AnimationCurve Linear
		{
			get
			{
				return new AnimationCurve(new Keyframe[]
				{
					new Keyframe(0f, 0f, 1f, 1f),
					new Keyframe(1f, 1f, 1f, 1f)
				});
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06001374 RID: 4980 RVA: 0x000679C0 File Offset: 0x00065BC0
		public static AnimationCurve Hop
		{
			get
			{
				return new AnimationCurve(new Keyframe[]
				{
					new Keyframe(0f, 0f),
					new Keyframe(0.5f, 1f),
					new Keyframe(1f, 0f)
				});
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06001375 RID: 4981 RVA: 0x00067A1A File Offset: 0x00065C1A
		public static AnimationCurve SharpHop
		{
			get
			{
				return new AnimationCurve(new Keyframe[]
				{
					new Keyframe(0f, 1f),
					new Keyframe(1f, 0f)
				});
			}
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x00067A54 File Offset: 0x00065C54
		private void Update()
		{
			for (int i = 0; i < this.list.Count; i++)
			{
				if (this.list[i].Update())
				{
					if (this.list[i].callback != null)
					{
						this.list[i].callback();
					}
					this.list.RemoveAt(i);
					i--;
				}
			}
			for (int j = 0; j < this.listColor.Count; j++)
			{
				if (this.listColor[j].Update())
				{
					if (this.listColor[j].callback != null)
					{
						this.listColor[j].callback();
					}
					this.listColor.RemoveAt(j);
					j--;
				}
			}
			if (this.sleep && Time.realtimeSinceStartup > this.sleepTimer)
			{
				this.sleep = false;
				Time.timeScale = this.savedTimescale;
			}
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x00067B4C File Offset: 0x00065D4C
		public static void Add(Transform aTransform, JuiceType aType, AnimationCurve aCurve, float aStart = 0f, float aEnd = 1f, float aDuration = 1f, bool aRelative = false, Action aCallback = null)
		{
			JuiceData juiceData = new JuiceData();
			juiceData.transform = aTransform;
			juiceData.type = aType;
			juiceData.curve = aCurve;
			juiceData.start = aStart;
			juiceData.duration = aDuration;
			juiceData.startTime = Time.time;
			juiceData.end = aEnd;
			juiceData.relative = aRelative;
			juiceData.callback = aCallback;
			Juice.Instance.list.Add(juiceData);
			juiceData.Update();
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x00067BBD File Offset: 0x00065DBD
		public static void Scale(Transform aTransform, AnimationCurve aCurve, float aStart = 0f, float aEnd = 1f, float aDuration = 1f, bool aRelative = false, Action aCallback = null)
		{
			Juice.Add(aTransform, JuiceType.ScaleXYZ, aCurve, aStart, aEnd, aDuration, aRelative, aCallback);
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x00067BD0 File Offset: 0x00065DD0
		public static void Scale(Transform aTransform, AnimationCurve aCurve, Vector3 aStart, Vector3 aEnd, float aDuration = 1f, bool aRelative = false, Action aCallback = null)
		{
			Juice.Add(aTransform, JuiceType.ScaleX, aCurve, aStart.x, aEnd.x, aDuration, aRelative, aCallback);
			Juice.Add(aTransform, JuiceType.ScaleY, aCurve, aStart.y, aEnd.y, aDuration, aRelative, aCallback);
			Juice.Add(aTransform, JuiceType.ScaleZ, aCurve, aStart.z, aEnd.z, aDuration, aRelative, aCallback);
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x00067C2C File Offset: 0x00065E2C
		public static void Rotate(Transform aTransform, AnimationCurve aCurve, Vector3 aStart, Vector3 aEnd, float aDuration = 1f, bool aRelative = false, Action aCallback = null)
		{
			Juice.Add(aTransform, JuiceType.RotationX, aCurve, aStart.x, aEnd.x, aDuration, aRelative, aCallback);
			Juice.Add(aTransform, JuiceType.RotationY, aCurve, aStart.y, aEnd.y, aDuration, aRelative, aCallback);
			Juice.Add(aTransform, JuiceType.RotationZ, aCurve, aStart.z, aEnd.z, aDuration, aRelative, aCallback);
		}

		// Token: 0x0600137B RID: 4987 RVA: 0x00067C90 File Offset: 0x00065E90
		public static void Translate(Transform aTransform, AnimationCurve aCurve, Vector3 aStart, Vector3 aEnd, float aDuration, bool aRelative = false, Action aCallback = null)
		{
			Vector3 vector = Vector3.zero;
			if (aTransform.parent != null)
			{
				vector = aTransform.parent.position;
			}
			Juice.Add(aTransform, JuiceType.TranslateX, aCurve, aStart.x - vector.x, aEnd.x - vector.x, aDuration, aRelative, null);
			Juice.Add(aTransform, JuiceType.TranslateY, aCurve, aStart.y - vector.y, aEnd.y - vector.y, aDuration, aRelative, null);
			Juice.Add(aTransform, JuiceType.TranslateZ, aCurve, aStart.z - vector.z, aEnd.z - vector.z, aDuration, aRelative, aCallback);
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x00067D38 File Offset: 0x00065F38
		public static void TranslateLocal(Transform aTransform, AnimationCurve aCurve, Vector3 aStart, Vector3 aEnd, float aDuration, bool aRelative = false, Action aCallback = null)
		{
			Juice.Add(aTransform, JuiceType.TranslateX, aCurve, aStart.x, aEnd.x, aDuration, aRelative, null);
			Juice.Add(aTransform, JuiceType.TranslateY, aCurve, aStart.y, aEnd.y, aDuration, aRelative, null);
			Juice.Add(aTransform, JuiceType.TranslateZ, aCurve, aStart.z, aEnd.z, aDuration, aRelative, aCallback);
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x00067D94 File Offset: 0x00065F94
		public static void Color(Material aRenderer, AnimationCurve aCurve, Color aStart, Color aEnd, float aDuration, Action aCallback = null)
		{
			JuiceDataColor juiceDataColor = new JuiceDataColor();
			juiceDataColor.renderer = aRenderer;
			juiceDataColor.curve = aCurve;
			juiceDataColor.start = aStart;
			juiceDataColor.duration = aDuration;
			juiceDataColor.startTime = Time.time;
			juiceDataColor.end = aEnd;
			juiceDataColor.callback = aCallback;
			Juice.Instance.listColor.Add(juiceDataColor);
			juiceDataColor.Update();
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x00067DF8 File Offset: 0x00065FF8
		public static void Cancel(Transform aTransform, bool aFinishEffect = true)
		{
			for (int i = 0; i < Juice.Instance.list.Count; i++)
			{
				if (Juice.Instance.list[i].transform == aTransform)
				{
					if (aFinishEffect)
					{
						Juice.Instance.list[i].Cancel();
					}
					Juice.Instance.list.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x00067E68 File Offset: 0x00066068
		public static void Cancel(Renderer aRenderer, bool aFinishEffect = true)
		{
			for (int i = 0; i < Juice.Instance.listColor.Count; i++)
			{
				if (Juice.Instance.listColor[i].renderer == aRenderer)
				{
					if (aFinishEffect)
					{
						Juice.Instance.listColor[i].Cancel();
					}
					Juice.Instance.listColor.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x00067ED8 File Offset: 0x000660D8
		public static void SlowMo(float aSpeed)
		{
			Time.timeScale = aSpeed;
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x00067EF0 File Offset: 0x000660F0
		public static void Sleep(float aDuration)
		{
			Juice.Instance.savedTimescale = ((Time.timeScale == 0f) ? Juice.Instance.savedTimescale : Time.timeScale);
			Time.timeScale = 0f;
			Juice.Instance.sleep = true;
			Juice.Instance.sleepTimer = Time.realtimeSinceStartup + aDuration;
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x00067F4A File Offset: 0x0006614A
		public static void SleepMS(int aMilliseconds)
		{
			Juice.Sleep((float)aMilliseconds * 0.001f);
		}

		// Token: 0x04000F3A RID: 3898
		private static Juice instance;

		// Token: 0x04000F3B RID: 3899
		public List<JuiceData> list = new List<JuiceData>();

		// Token: 0x04000F3C RID: 3900
		public List<JuiceDataColor> listColor = new List<JuiceDataColor>();

		// Token: 0x04000F3D RID: 3901
		private float savedTimescale;

		// Token: 0x04000F3E RID: 3902
		private float sleepTimer;

		// Token: 0x04000F3F RID: 3903
		private bool sleep;
	}
}
