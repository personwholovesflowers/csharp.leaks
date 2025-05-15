using System;
using System.Collections.Generic;
using System.Linq;
using plog;
using UnityEngine;

namespace Sandbox
{
	// Token: 0x02000565 RID: 1381
	public class SandboxSpawnableInstance : MonoBehaviour
	{
		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06001F1E RID: 7966 RVA: 0x000FEBE4 File Offset: 0x000FCDE4
		public bool uniformSize
		{
			get
			{
				Vector3 normalizedSize = this.normalizedSize;
				return normalizedSize.x == normalizedSize.y && normalizedSize.y == normalizedSize.z;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06001F1F RID: 7967 RVA: 0x000FEC16 File Offset: 0x000FCE16
		// (set) Token: 0x06001F20 RID: 7968 RVA: 0x000FEC1E File Offset: 0x000FCE1E
		public Vector3 defaultSize { get; private set; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06001F21 RID: 7969 RVA: 0x000FEC28 File Offset: 0x000FCE28
		public Vector3 normalizedSize
		{
			get
			{
				return new Vector3(base.transform.localScale.x / this.defaultSize.x, base.transform.localScale.y / this.defaultSize.y, base.transform.localScale.z / this.defaultSize.z);
			}
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x000FEC90 File Offset: 0x000FCE90
		public virtual void Awake()
		{
			this.defaultSize = base.transform.localScale;
			this.rigidbody = base.GetComponent<Rigidbody>();
			if (this.collider == null)
			{
				this.collider = base.GetComponent<Collider>();
			}
			if (this.collider == null)
			{
				this.collider = base.transform.GetComponentInChildren<Collider>();
			}
			SandboxPropPart[] componentsInChildren = base.GetComponentsInChildren<SandboxPropPart>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].parent = this;
			}
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x000FED14 File Offset: 0x000FCF14
		public virtual void SetSize(Vector3 size)
		{
			base.transform.localScale = new Vector3(size.x * this.defaultSize.x, size.y * this.defaultSize.y, size.z * this.defaultSize.z);
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x000FED67 File Offset: 0x000FCF67
		public void SetSizeUniform(float size)
		{
			this.SetSize(Vector3.one * size);
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x000FED7C File Offset: 0x000FCF7C
		public void BaseSave(ref SavedGeneric saveObject)
		{
			if (saveObject != null)
			{
				saveObject.ObjectIdentifier = this.sourceObject.identifier;
				saveObject.Position = new SavedVector3(base.transform.position);
				saveObject.Rotation = new SavedQuaternion(base.transform.rotation);
				saveObject.Scale = new SavedVector3(this.normalizedSize);
				SavedPhysical savedPhysical = saveObject as SavedPhysical;
				if (savedPhysical != null)
				{
					savedPhysical.Kinematic = this.frozen;
				}
				saveObject.DisallowManipulation = this.disallowManipulation;
				saveObject.DisallowFreezing = this.disallowFreezing;
			}
			else
			{
				saveObject = new SavedGeneric
				{
					ObjectIdentifier = this.sourceObject.identifier,
					Position = new SavedVector3(base.transform.position),
					Rotation = new SavedQuaternion(base.transform.rotation),
					Scale = new SavedVector3(this.normalizedSize),
					DisallowManipulation = this.disallowManipulation,
					DisallowFreezing = this.disallowFreezing
				};
			}
			IAlter[] array = (from c in base.GetComponentsInChildren<IAlter>()
				where c.alterKey != null
				select c).ToArray<IAlter>();
			if (array.Length != 0)
			{
				saveObject.Data = new SavedAlterData[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					IAlter alter = array[i];
					List<SavedAlterOption> list = new List<SavedAlterOption>();
					IAlterOptions<bool> alterOptions = alter as IAlterOptions<bool>;
					if (alterOptions != null)
					{
						if (alterOptions.options != null)
						{
							list.AddRange(from o in alterOptions.options
								where o.key != null
								select o into b
								select new SavedAlterOption
								{
									BoolValue = new bool?(b.value),
									Key = b.key
								});
						}
						SandboxSpawnableInstance.Log.Fine("Saving <b>bool</b> data for <b>" + alter.alterKey + "</b>", null, null, null);
						global::plog.Logger log = SandboxSpawnableInstance.Log;
						string text;
						if (alterOptions.options != null)
						{
							text = "Bool Options: " + string.Join(", ", alterOptions.options.Select((AlterOption<bool> o) => o.key ?? "(missing key!)"));
						}
						else
						{
							text = "No options available!";
						}
						log.Fine(text, null, null, null);
					}
					IAlterOptions<float> alterOptions2 = alter as IAlterOptions<float>;
					if (alterOptions2 != null)
					{
						if (alterOptions2.options != null)
						{
							list.AddRange(from o in alterOptions2.options
								where o.key != null
								select o into b
								select new SavedAlterOption
								{
									FloatValue = new float?(b.value),
									Key = b.key
								});
						}
						SandboxSpawnableInstance.Log.Fine("Saving <b>float</b> data for <b>" + alter.alterKey + "</b>", null, null, null);
						global::plog.Logger log2 = SandboxSpawnableInstance.Log;
						string text2;
						if (alterOptions2.options != null)
						{
							text2 = "Float Options: " + string.Join(", ", alterOptions2.options.Select((AlterOption<float> o) => o.key ?? "(missing key!)"));
						}
						else
						{
							text2 = "No options available!";
						}
						log2.Fine(text2, null, null, null);
					}
					IAlterOptions<Vector3> alterOptions3 = alter as IAlterOptions<Vector3>;
					if (alterOptions3 != null)
					{
						if (alterOptions3.options != null)
						{
							list.AddRange(from o in alterOptions3.options
								where o.key != null
								select o into b
								select new SavedAlterOption
								{
									VectorData = new Vector3?(b.value),
									Key = b.key
								});
						}
						if (Debug.isDebugBuild)
						{
							SandboxSpawnableInstance.Log.Fine("Saving <b>vector</b> data for <b>" + alter.alterKey + "</b>", null, null, null);
							global::plog.Logger log3 = SandboxSpawnableInstance.Log;
							string text3;
							if (alterOptions3.options != null)
							{
								text3 = "Vector Options: " + string.Join(", ", alterOptions3.options.Select((AlterOption<Vector3> o) => o.key ?? "(missing key!)"));
							}
							else
							{
								text3 = "No options available!";
							}
							log3.Fine(text3, null, null, null);
						}
					}
					IAlterOptions<int> alterOptions4 = alter as IAlterOptions<int>;
					if (alterOptions4 != null)
					{
						if (alterOptions4.options != null)
						{
							list.AddRange(from o in alterOptions4.options
								where o.key != null
								select o into b
								select new SavedAlterOption
								{
									IntValue = new int?(b.value),
									Key = b.key
								});
						}
						SandboxSpawnableInstance.Log.Fine("Saving <b>float</b> data for <b>" + alter.alterKey + "</b>", null, null, null);
						global::plog.Logger log4 = SandboxSpawnableInstance.Log;
						string text4;
						if (alterOptions4.options != null)
						{
							text4 = "Float Options: " + string.Join(", ", alterOptions4.options.Select((AlterOption<int> o) => o.key ?? "(missing key!)"));
						}
						else
						{
							text4 = "No options available!";
						}
						log4.Fine(text4, null, null, null);
					}
					saveObject.Data[i] = new SavedAlterData
					{
						Key = array[i].alterKey,
						Options = list.ToArray()
					};
				}
			}
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x000FF2C7 File Offset: 0x000FD4C7
		public virtual void Pause(bool freeze = true)
		{
			if (freeze)
			{
				this.frozen = true;
			}
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x000FF2D3 File Offset: 0x000FD4D3
		public virtual void Resume()
		{
			this.frozen = false;
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x000FF2DC File Offset: 0x000FD4DC
		public void ApplyAlterOptions(IEnumerable<AlterOption> requestedOptions)
		{
			IAlterOptions<bool>[] array = null;
			IAlterOptions<int>[] array2 = null;
			IAlterOptions<float>[] array3 = null;
			IAlterOptions<Vector3>[] array4 = null;
			foreach (AlterOption alterOption in requestedOptions)
			{
				if (alterOption.useBool)
				{
					if (array == null)
					{
						array = base.GetComponentsInChildren<IAlterOptions<bool>>(true);
					}
					IAlterOptions<bool>[] array5 = array;
					for (int i = 0; i < array5.Length; i++)
					{
						foreach (AlterOption<bool> alterOption2 in array5[i].options)
						{
							if (!(alterOption2.key != alterOption.targetKey))
							{
								alterOption2.callback(alterOption.boolValue);
							}
						}
					}
				}
				if (alterOption.useInt)
				{
					if (array2 == null)
					{
						array2 = base.GetComponentsInChildren<IAlterOptions<int>>(true);
					}
					IAlterOptions<int>[] array6 = array2;
					for (int i = 0; i < array6.Length; i++)
					{
						foreach (AlterOption<int> alterOption3 in array6[i].options)
						{
							if (!(alterOption3.key != alterOption.targetKey))
							{
								alterOption3.callback(alterOption.intValue);
							}
						}
					}
				}
				if (alterOption.useFloat)
				{
					if (array3 == null)
					{
						array3 = base.GetComponentsInChildren<IAlterOptions<float>>(true);
					}
					IAlterOptions<float>[] array7 = array3;
					for (int i = 0; i < array7.Length; i++)
					{
						foreach (AlterOption<float> alterOption4 in array7[i].options)
						{
							if (!(alterOption4.key != alterOption.targetKey))
							{
								alterOption4.callback(alterOption.floatValue);
							}
						}
					}
				}
				if (alterOption.useVector)
				{
					if (array4 == null)
					{
						array4 = base.GetComponentsInChildren<IAlterOptions<Vector3>>(true);
					}
					IAlterOptions<Vector3>[] array8 = array4;
					for (int i = 0; i < array8.Length; i++)
					{
						foreach (AlterOption<Vector3> alterOption5 in array8[i].options)
						{
							if (!(alterOption5.key != alterOption.targetKey))
							{
								alterOption5.callback(alterOption.vectorValue);
							}
						}
					}
				}
			}
		}

		// Token: 0x04002B85 RID: 11141
		private static readonly global::plog.Logger Log = new global::plog.Logger("SandboxSpawnableInstance");

		// Token: 0x04002B86 RID: 11142
		public SpawnableObject sourceObject;

		// Token: 0x04002B87 RID: 11143
		public GameObject attachedParticles;

		// Token: 0x04002B88 RID: 11144
		public Collider collider;

		// Token: 0x04002B8A RID: 11146
		[NonSerialized]
		public bool alwaysFrozen;

		// Token: 0x04002B8B RID: 11147
		[NonSerialized]
		public Rigidbody rigidbody;

		// Token: 0x04002B8C RID: 11148
		public bool frozen;

		// Token: 0x04002B8D RID: 11149
		public bool disallowManipulation;

		// Token: 0x04002B8E RID: 11150
		public bool disallowFreezing;
	}
}
