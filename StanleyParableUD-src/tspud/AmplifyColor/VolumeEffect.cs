using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AmplifyColor
{
	// Token: 0x0200031F RID: 799
	[Serializable]
	public class VolumeEffect
	{
		// Token: 0x06001436 RID: 5174 RVA: 0x0006BF12 File Offset: 0x0006A112
		public VolumeEffect(AmplifyColorBase effect)
		{
			this.gameObject = effect;
			this.components = new List<VolumeEffectComponent>();
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0006BF2C File Offset: 0x0006A12C
		public static VolumeEffect BlendValuesToVolumeEffect(VolumeEffectFlags flags, VolumeEffect volume1, VolumeEffect volume2, float blend)
		{
			VolumeEffect volumeEffect = new VolumeEffect(volume1.gameObject);
			foreach (VolumeEffectComponentFlags volumeEffectComponentFlags in flags.components)
			{
				if (volumeEffectComponentFlags.blendFlag)
				{
					VolumeEffectComponent volumeEffectComponent = volume1.FindEffectComponent(volumeEffectComponentFlags.componentName);
					VolumeEffectComponent volumeEffectComponent2 = volume2.FindEffectComponent(volumeEffectComponentFlags.componentName);
					if (volumeEffectComponent != null && volumeEffectComponent2 != null)
					{
						VolumeEffectComponent volumeEffectComponent3 = new VolumeEffectComponent(volumeEffectComponent.componentName);
						foreach (VolumeEffectFieldFlags volumeEffectFieldFlags in volumeEffectComponentFlags.componentFields)
						{
							if (volumeEffectFieldFlags.blendFlag)
							{
								VolumeEffectField volumeEffectField = volumeEffectComponent.FindEffectField(volumeEffectFieldFlags.fieldName);
								VolumeEffectField volumeEffectField2 = volumeEffectComponent2.FindEffectField(volumeEffectFieldFlags.fieldName);
								if (volumeEffectField != null && volumeEffectField2 != null)
								{
									VolumeEffectField volumeEffectField3 = new VolumeEffectField(volumeEffectField.fieldName, volumeEffectField.fieldType);
									string fieldType = volumeEffectField3.fieldType;
									if (!(fieldType == "System.Single"))
									{
										if (!(fieldType == "System.Boolean"))
										{
											if (!(fieldType == "UnityEngine.Vector2"))
											{
												if (!(fieldType == "UnityEngine.Vector3"))
												{
													if (!(fieldType == "UnityEngine.Vector4"))
													{
														if (fieldType == "UnityEngine.Color")
														{
															volumeEffectField3.valueColor = Color.Lerp(volumeEffectField.valueColor, volumeEffectField2.valueColor, blend);
														}
													}
													else
													{
														volumeEffectField3.valueVector4 = Vector4.Lerp(volumeEffectField.valueVector4, volumeEffectField2.valueVector4, blend);
													}
												}
												else
												{
													volumeEffectField3.valueVector3 = Vector3.Lerp(volumeEffectField.valueVector3, volumeEffectField2.valueVector3, blend);
												}
											}
											else
											{
												volumeEffectField3.valueVector2 = Vector2.Lerp(volumeEffectField.valueVector2, volumeEffectField2.valueVector2, blend);
											}
										}
										else
										{
											volumeEffectField3.valueBoolean = volumeEffectField2.valueBoolean;
										}
									}
									else
									{
										volumeEffectField3.valueSingle = Mathf.Lerp(volumeEffectField.valueSingle, volumeEffectField2.valueSingle, blend);
									}
									volumeEffectComponent3.fields.Add(volumeEffectField3);
								}
							}
						}
						volumeEffect.components.Add(volumeEffectComponent3);
					}
				}
			}
			return volumeEffect;
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x0006C19C File Offset: 0x0006A39C
		public VolumeEffectComponent AddComponent(Component c, VolumeEffectComponentFlags compFlags)
		{
			if (compFlags == null)
			{
				VolumeEffectComponent volumeEffectComponent = new VolumeEffectComponent(string.Concat(c.GetType()));
				this.components.Add(volumeEffectComponent);
				return volumeEffectComponent;
			}
			VolumeEffectComponent volumeEffectComponent2;
			if ((volumeEffectComponent2 = this.FindEffectComponent(string.Concat(c.GetType()))) != null)
			{
				volumeEffectComponent2.UpdateComponent(c, compFlags);
				return volumeEffectComponent2;
			}
			VolumeEffectComponent volumeEffectComponent3 = new VolumeEffectComponent(c, compFlags);
			this.components.Add(volumeEffectComponent3);
			return volumeEffectComponent3;
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x0006C1FF File Offset: 0x0006A3FF
		public void RemoveEffectComponent(VolumeEffectComponent comp)
		{
			this.components.Remove(comp);
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0006C210 File Offset: 0x0006A410
		public void UpdateVolume()
		{
			if (this.gameObject == null)
			{
				return;
			}
			foreach (VolumeEffectComponentFlags volumeEffectComponentFlags in this.gameObject.EffectFlags.components)
			{
				if (volumeEffectComponentFlags.blendFlag)
				{
					Component component = this.gameObject.GetComponent(volumeEffectComponentFlags.componentName);
					if (component != null)
					{
						this.AddComponent(component, volumeEffectComponentFlags);
					}
				}
			}
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x0006C2A4 File Offset: 0x0006A4A4
		public void SetValues(AmplifyColorBase targetColor)
		{
			VolumeEffectFlags effectFlags = targetColor.EffectFlags;
			GameObject gameObject = targetColor.gameObject;
			foreach (VolumeEffectComponentFlags volumeEffectComponentFlags in effectFlags.components)
			{
				if (volumeEffectComponentFlags.blendFlag)
				{
					Component component = gameObject.GetComponent(volumeEffectComponentFlags.componentName);
					VolumeEffectComponent volumeEffectComponent = this.FindEffectComponent(volumeEffectComponentFlags.componentName);
					if (!(component == null) && volumeEffectComponent != null)
					{
						foreach (VolumeEffectFieldFlags volumeEffectFieldFlags in volumeEffectComponentFlags.componentFields)
						{
							if (volumeEffectFieldFlags.blendFlag)
							{
								FieldInfo field = component.GetType().GetField(volumeEffectFieldFlags.fieldName);
								VolumeEffectField volumeEffectField = volumeEffectComponent.FindEffectField(volumeEffectFieldFlags.fieldName);
								if (!(field == null) && volumeEffectField != null)
								{
									string fullName = field.FieldType.FullName;
									if (!(fullName == "System.Single"))
									{
										if (!(fullName == "System.Boolean"))
										{
											if (!(fullName == "UnityEngine.Vector2"))
											{
												if (!(fullName == "UnityEngine.Vector3"))
												{
													if (!(fullName == "UnityEngine.Vector4"))
													{
														if (fullName == "UnityEngine.Color")
														{
															field.SetValue(component, volumeEffectField.valueColor);
														}
													}
													else
													{
														field.SetValue(component, volumeEffectField.valueVector4);
													}
												}
												else
												{
													field.SetValue(component, volumeEffectField.valueVector3);
												}
											}
											else
											{
												field.SetValue(component, volumeEffectField.valueVector2);
											}
										}
										else
										{
											field.SetValue(component, volumeEffectField.valueBoolean);
										}
									}
									else
									{
										field.SetValue(component, volumeEffectField.valueSingle);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x0006C4C4 File Offset: 0x0006A6C4
		public void BlendValues(AmplifyColorBase targetColor, VolumeEffect other, float blendAmount)
		{
			VolumeEffectFlags effectFlags = targetColor.EffectFlags;
			GameObject gameObject = targetColor.gameObject;
			for (int i = 0; i < effectFlags.components.Count; i++)
			{
				VolumeEffectComponentFlags volumeEffectComponentFlags = effectFlags.components[i];
				if (volumeEffectComponentFlags.blendFlag)
				{
					Component component = gameObject.GetComponent(volumeEffectComponentFlags.componentName);
					VolumeEffectComponent volumeEffectComponent = this.FindEffectComponent(volumeEffectComponentFlags.componentName);
					VolumeEffectComponent volumeEffectComponent2 = other.FindEffectComponent(volumeEffectComponentFlags.componentName);
					if (!(component == null) && volumeEffectComponent != null && volumeEffectComponent2 != null)
					{
						for (int j = 0; j < volumeEffectComponentFlags.componentFields.Count; j++)
						{
							VolumeEffectFieldFlags volumeEffectFieldFlags = volumeEffectComponentFlags.componentFields[j];
							if (volumeEffectFieldFlags.blendFlag)
							{
								FieldInfo field = component.GetType().GetField(volumeEffectFieldFlags.fieldName);
								VolumeEffectField volumeEffectField = volumeEffectComponent.FindEffectField(volumeEffectFieldFlags.fieldName);
								VolumeEffectField volumeEffectField2 = volumeEffectComponent2.FindEffectField(volumeEffectFieldFlags.fieldName);
								if (!(field == null) && volumeEffectField != null && volumeEffectField2 != null)
								{
									string fullName = field.FieldType.FullName;
									if (!(fullName == "System.Single"))
									{
										if (!(fullName == "System.Boolean"))
										{
											if (!(fullName == "UnityEngine.Vector2"))
											{
												if (!(fullName == "UnityEngine.Vector3"))
												{
													if (!(fullName == "UnityEngine.Vector4"))
													{
														if (fullName == "UnityEngine.Color")
														{
															field.SetValue(component, Color.Lerp(volumeEffectField.valueColor, volumeEffectField2.valueColor, blendAmount));
														}
													}
													else
													{
														field.SetValue(component, Vector4.Lerp(volumeEffectField.valueVector4, volumeEffectField2.valueVector4, blendAmount));
													}
												}
												else
												{
													field.SetValue(component, Vector3.Lerp(volumeEffectField.valueVector3, volumeEffectField2.valueVector3, blendAmount));
												}
											}
											else
											{
												field.SetValue(component, Vector2.Lerp(volumeEffectField.valueVector2, volumeEffectField2.valueVector2, blendAmount));
											}
										}
										else
										{
											field.SetValue(component, volumeEffectField2.valueBoolean);
										}
									}
									else
									{
										field.SetValue(component, Mathf.Lerp(volumeEffectField.valueSingle, volumeEffectField2.valueSingle, blendAmount));
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x0006C724 File Offset: 0x0006A924
		public VolumeEffectComponent FindEffectComponent(string compName)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (this.components[i].componentName == compName)
				{
					return this.components[i];
				}
			}
			return null;
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x0006C770 File Offset: 0x0006A970
		public static Component[] ListAcceptableComponents(AmplifyColorBase go)
		{
			if (go == null)
			{
				return new Component[0];
			}
			return (from comp in go.GetComponents(typeof(Component))
				where comp != null && !string.Concat(comp.GetType()).StartsWith("UnityEngine.") && !(comp.GetType() == typeof(AmplifyColorBase))
				select comp).ToArray<Component>();
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0006C7C6 File Offset: 0x0006A9C6
		public string[] GetComponentNames()
		{
			return this.components.Select((VolumeEffectComponent r) => r.componentName).ToArray<string>();
		}

		// Token: 0x0400104E RID: 4174
		public AmplifyColorBase gameObject;

		// Token: 0x0400104F RID: 4175
		public List<VolumeEffectComponent> components;
	}
}
