using System;
using Nest.Util;
using UnityEngine;

namespace Nest.Integrations
{
	// Token: 0x02000245 RID: 581
	[AddComponentMenu("Cast/Integrations/Transform Movement")]
	public class TransformMovement : BaseIntegration
	{
		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x0003D646 File Offset: 0x0003B846
		// (set) Token: 0x06000DAD RID: 3501 RVA: 0x0003D64E File Offset: 0x0003B84E
		public Transform TargetTransform
		{
			get
			{
				return this._targetTransform;
			}
			set
			{
				if (this._targetTransform != null)
				{
					this.OnDisable();
				}
				this._targetTransform = value;
				if (this._targetTransform != null)
				{
					this.OnEnable();
				}
			}
		}

		// Token: 0x17000143 RID: 323
		// (set) Token: 0x06000DAE RID: 3502 RVA: 0x0003D680 File Offset: 0x0003B880
		public override float InputValue
		{
			set
			{
				if (base.enabled && this._targetTransform != null)
				{
					if (this._translationEnabled)
					{
						this.UpdatePosition(value);
					}
					if (this._rotationEnabled)
					{
						this.UpdateRotation(value);
					}
					if (this._scaleEnabled)
					{
						this.UpdateScale(value);
					}
				}
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000DAF RID: 3503 RVA: 0x0003D6D0 File Offset: 0x0003B8D0
		private Vector3 TranslationVector
		{
			get
			{
				switch (this._translationMode)
				{
				case TransformMovement.Translation.XAxis:
					return Vector3.right;
				case TransformMovement.Translation.YAxis:
					return Vector3.up;
				case TransformMovement.Translation.ZAxis:
					return Vector3.forward;
				case TransformMovement.Translation.Vector:
					return this._translationVector;
				}
				return this._randomVectorT;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x0003D720 File Offset: 0x0003B920
		private Vector3 RotationAxis
		{
			get
			{
				switch (this._rotationMode)
				{
				case TransformMovement.Rotation.XAxis:
					return Vector3.right;
				case TransformMovement.Rotation.YAxis:
					return Vector3.up;
				case TransformMovement.Rotation.ZAxis:
					return Vector3.forward;
				case TransformMovement.Rotation.Vector:
					return this._rotationAxis;
				}
				return this._randomVectorR;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x0003D773 File Offset: 0x0003B973
		private Vector3 ScaleVector
		{
			get
			{
				if (this._scaleMode == TransformMovement.Scale.Uniform)
				{
					return Vector3.one;
				}
				if (this._scaleMode == TransformMovement.Scale.Vector)
				{
					return this._scaleVector;
				}
				return this._randomVectorS;
			}
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0003D79C File Offset: 0x0003B99C
		private void UpdatePosition(float value)
		{
			float num = Mathf.Lerp(this._translationAmount0, this._translationAmount1, value);
			Vector3 vector = this.TranslationVector * num;
			if (this._addToOriginal)
			{
				vector += this._originalPosition;
			}
			this._targetTransform.localPosition = vector;
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0003D7EC File Offset: 0x0003B9EC
		private void UpdateRotation(float value)
		{
			Quaternion quaternion = Quaternion.AngleAxis(Mathf.Lerp(this._rotationAngle0, this._rotationAngle1, value), this.RotationAxis);
			if (this._addToOriginal)
			{
				quaternion = this._originalRotation * quaternion;
			}
			this._targetTransform.localRotation = quaternion;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0003D838 File Offset: 0x0003BA38
		private void UpdateScale(float value)
		{
			float num = Mathf.Lerp(this._scaleAmount0, this._scaleAmount1, value);
			Vector3 vector = this.ScaleVector * num;
			if (this._addToOriginal)
			{
				vector += this._originalScale;
			}
			this._targetTransform.localScale = vector;
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0003D888 File Offset: 0x0003BA88
		private void OnEnable()
		{
			if (this._targetTransform == null)
			{
				this._targetTransform = base.transform;
			}
			this._originalPosition = this._targetTransform.localPosition;
			this._originalRotation = this._targetTransform.localRotation;
			this._originalScale = this._targetTransform.localScale;
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0003D8E4 File Offset: 0x0003BAE4
		private void OnDisable()
		{
			if (this._targetTransform != null)
			{
				this._targetTransform.localPosition = this._originalPosition;
				this._targetTransform.localRotation = this._originalRotation;
				this._targetTransform.localScale = this._originalScale;
			}
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0003D934 File Offset: 0x0003BB34
		private void Start()
		{
			this._randomVectorT = Random.onUnitSphere;
			this._randomVectorR = Random.onUnitSphere;
			this._randomVectorS = new Vector3(Random.value, Random.value, Random.value);
			this._rigidBody = this._targetTransform.GetComponent<Rigidbody>();
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x0003D984 File Offset: 0x0003BB84
		private void FixedUpdate()
		{
			if (this._referenceTransform == null)
			{
				return;
			}
			TransformMovement.Rotation rotationMode = this._rotationMode;
			if (rotationMode != TransformMovement.Rotation.Mirror)
			{
				if (rotationMode == TransformMovement.Rotation.LookTowards)
				{
					if (this._rigidBody != null)
					{
						Vector3 vector = this._rigidBody.angularVelocity * -1f;
						Debug.DrawRay(base.transform.position, this._rigidBody.angularVelocity * 10f, Color.black);
						Vector3 vector2 = this._angularVelocityController.Update(vector, Time.deltaTime);
						Debug.DrawRay(base.transform.position, vector2, Color.green);
						this._rigidBody.AddTorque(vector2);
						Vector3 vector3 = this._referenceTransform.position - base.transform.position;
						Debug.DrawRay(base.transform.position, vector3, Color.magenta);
						Vector3 forward = base.transform.forward;
						Debug.DrawRay(base.transform.position, forward * 15f, Color.blue);
						Vector3 vector4 = Vector3.Cross(forward, vector3);
						Vector3 vector5 = this._headingController.Update(vector4, Time.deltaTime);
						this._rigidBody.AddTorque(vector5);
					}
					else
					{
						base.transform.LookAt(this._referenceTransform.position);
					}
				}
			}
			else if (this._rigidBody != null)
			{
				this._rigidBody.rotation = this._referenceTransform.rotation;
			}
			else
			{
				base.transform.rotation = this._referenceTransform.rotation;
			}
			TransformMovement.Translation translationMode = this._translationMode;
			if (translationMode == TransformMovement.Translation.Mirror)
			{
				base.transform.localPosition = this._referenceTransform.localPosition;
			}
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0003DB43 File Offset: 0x0003BD43
		public void ResetPosition()
		{
			if (this._rigidBody != null)
			{
				this._rigidBody.position = this._originalPosition;
				return;
			}
			this._targetTransform.localPosition = this._originalPosition;
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x0003DB76 File Offset: 0x0003BD76
		public void ResetPosition(Transform tr)
		{
			this._originalPosition = tr.position;
			this.ResetPosition();
		}

		// Token: 0x04000C31 RID: 3121
		[SerializeField]
		private bool _translationEnabled;

		// Token: 0x04000C32 RID: 3122
		[SerializeField]
		private TransformMovement.Translation _translationMode;

		// Token: 0x04000C33 RID: 3123
		[SerializeField]
		private Vector3 _translationVector = Vector3.forward;

		// Token: 0x04000C34 RID: 3124
		[SerializeField]
		private float _translationAmount0;

		// Token: 0x04000C35 RID: 3125
		[SerializeField]
		private float _translationAmount1 = 10f;

		// Token: 0x04000C36 RID: 3126
		[SerializeField]
		private bool _rotationEnabled;

		// Token: 0x04000C37 RID: 3127
		[SerializeField]
		private TransformMovement.Rotation _rotationMode;

		// Token: 0x04000C38 RID: 3128
		[SerializeField]
		private Vector3 _rotationAxis = Vector3.up;

		// Token: 0x04000C39 RID: 3129
		[SerializeField]
		private float _rotationAngle0;

		// Token: 0x04000C3A RID: 3130
		[SerializeField]
		private float _rotationAngle1 = 90f;

		// Token: 0x04000C3B RID: 3131
		private readonly VectorPid _angularVelocityController = new VectorPid(33.7766f, 0f, 0.2553191f);

		// Token: 0x04000C3C RID: 3132
		private readonly VectorPid _headingController = new VectorPid(9.244681f, 0f, 0.06382979f);

		// Token: 0x04000C3D RID: 3133
		[SerializeField]
		private TransformMovement.Scale _scaleMode;

		// Token: 0x04000C3E RID: 3134
		[SerializeField]
		private Vector3 _scaleVector = Vector3.one;

		// Token: 0x04000C3F RID: 3135
		[SerializeField]
		private float _scaleAmount0;

		// Token: 0x04000C40 RID: 3136
		[SerializeField]
		private float _scaleAmount1 = 1f;

		// Token: 0x04000C41 RID: 3137
		[SerializeField]
		private bool _scaleEnabled;

		// Token: 0x04000C42 RID: 3138
		[SerializeField]
		private Transform _targetTransform;

		// Token: 0x04000C43 RID: 3139
		[SerializeField]
		private Transform _referenceTransform;

		// Token: 0x04000C44 RID: 3140
		[SerializeField]
		private bool _addToOriginal = true;

		// Token: 0x04000C45 RID: 3141
		private Vector3 _originalPosition;

		// Token: 0x04000C46 RID: 3142
		private Quaternion _originalRotation;

		// Token: 0x04000C47 RID: 3143
		private Vector3 _originalScale;

		// Token: 0x04000C48 RID: 3144
		private Vector3 _randomVectorT;

		// Token: 0x04000C49 RID: 3145
		private Vector3 _randomVectorR;

		// Token: 0x04000C4A RID: 3146
		private Vector3 _randomVectorS;

		// Token: 0x04000C4B RID: 3147
		private Rigidbody _rigidBody;

		// Token: 0x04000C4C RID: 3148
		private float _rotationSpeed;

		// Token: 0x02000438 RID: 1080
		public enum Translation
		{
			// Token: 0x040015B6 RID: 5558
			XAxis,
			// Token: 0x040015B7 RID: 5559
			YAxis,
			// Token: 0x040015B8 RID: 5560
			ZAxis,
			// Token: 0x040015B9 RID: 5561
			Mirror,
			// Token: 0x040015BA RID: 5562
			Vector,
			// Token: 0x040015BB RID: 5563
			Random
		}

		// Token: 0x02000439 RID: 1081
		public enum Rotation
		{
			// Token: 0x040015BD RID: 5565
			XAxis,
			// Token: 0x040015BE RID: 5566
			YAxis,
			// Token: 0x040015BF RID: 5567
			ZAxis,
			// Token: 0x040015C0 RID: 5568
			Mirror,
			// Token: 0x040015C1 RID: 5569
			LookTowards,
			// Token: 0x040015C2 RID: 5570
			Vector,
			// Token: 0x040015C3 RID: 5571
			Random
		}

		// Token: 0x0200043A RID: 1082
		public enum Scale
		{
			// Token: 0x040015C5 RID: 5573
			Uniform,
			// Token: 0x040015C6 RID: 5574
			Vector,
			// Token: 0x040015C7 RID: 5575
			Random
		}
	}
}
