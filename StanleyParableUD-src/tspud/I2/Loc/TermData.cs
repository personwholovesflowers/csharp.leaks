using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002BD RID: 701
	[Serializable]
	public class TermData
	{
		// Token: 0x06001231 RID: 4657 RVA: 0x00062708 File Offset: 0x00060908
		public string GetTranslation(int idx, string specialization = null, bool editMode = false)
		{
			string text = this.Languages[idx];
			if (text != null)
			{
				text = SpecializationManager.GetSpecializedText(text, specialization);
				if (!editMode)
				{
					text = text.Replace("[i2nt]", "").Replace("[/i2nt]", "");
				}
			}
			return text;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x0006274D File Offset: 0x0006094D
		public void SetTranslation(int idx, string translation, string specialization = null)
		{
			this.Languages[idx] = SpecializationManager.SetSpecializedText(this.Languages[idx], translation, specialization);
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x00062768 File Offset: 0x00060968
		public void RemoveSpecialization(string specialization)
		{
			for (int i = 0; i < this.Languages.Length; i++)
			{
				this.RemoveSpecialization(i, specialization);
			}
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x00062790 File Offset: 0x00060990
		public void RemoveSpecialization(int idx, string specialization)
		{
			string text = this.Languages[idx];
			if (specialization == "Any" || !text.Contains("[i2s_" + specialization + "]"))
			{
				return;
			}
			Dictionary<string, string> specializations = SpecializationManager.GetSpecializations(text, null);
			specializations.Remove(specialization);
			this.Languages[idx] = SpecializationManager.SetSpecializedText(specializations);
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x000627EA File Offset: 0x000609EA
		public bool IsAutoTranslated(int idx, bool IsTouch)
		{
			return (this.Flags[idx] & 2) > 0;
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x000627FC File Offset: 0x000609FC
		public void Validate()
		{
			int num = Mathf.Max(this.Languages.Length, this.Flags.Length);
			if (this.Languages.Length != num)
			{
				Array.Resize<string>(ref this.Languages, num);
			}
			if (this.Flags.Length != num)
			{
				Array.Resize<byte>(ref this.Flags, num);
			}
			if (this.Languages_Touch != null)
			{
				for (int i = 0; i < Mathf.Min(this.Languages_Touch.Length, num); i++)
				{
					if (string.IsNullOrEmpty(this.Languages[i]) && !string.IsNullOrEmpty(this.Languages_Touch[i]))
					{
						this.Languages[i] = this.Languages_Touch[i];
						this.Languages_Touch[i] = null;
					}
				}
				this.Languages_Touch = null;
			}
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x000628AC File Offset: 0x00060AAC
		public bool IsTerm(string name, bool allowCategoryMistmatch)
		{
			if (!allowCategoryMistmatch)
			{
				return name == this.Term;
			}
			return name == LanguageSourceData.GetKeyFromFullTerm(this.Term, false);
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x000628D0 File Offset: 0x00060AD0
		public bool HasSpecializations()
		{
			for (int i = 0; i < this.Languages.Length; i++)
			{
				if (!string.IsNullOrEmpty(this.Languages[i]) && this.Languages[i].Contains("[i2s_"))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x00062918 File Offset: 0x00060B18
		public List<string> GetAllSpecializations()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this.Languages.Length; i++)
			{
				SpecializationManager.AppendSpecializations(this.Languages[i], list);
			}
			return list;
		}

		// Token: 0x04000E86 RID: 3718
		public string Term = string.Empty;

		// Token: 0x04000E87 RID: 3719
		public eTermType TermType;

		// Token: 0x04000E88 RID: 3720
		[NonSerialized]
		public string Description;

		// Token: 0x04000E89 RID: 3721
		public string[] Languages = new string[0];

		// Token: 0x04000E8A RID: 3722
		public byte[] Flags = new byte[0];

		// Token: 0x04000E8B RID: 3723
		[SerializeField]
		private string[] Languages_Touch;
	}
}
