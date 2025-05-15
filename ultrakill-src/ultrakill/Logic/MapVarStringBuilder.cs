using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Logic
{
	// Token: 0x02000587 RID: 1415
	public class MapVarStringBuilder : MonoBehaviour
	{
		// Token: 0x06001FF5 RID: 8181 RVA: 0x00102DDC File Offset: 0x00100FDC
		private void OnEnable()
		{
			if (this.buildOnEnable)
			{
				this.BuildString();
			}
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x00102DEC File Offset: 0x00100FEC
		private void Update()
		{
			if (this.buildOnUpdate)
			{
				this.BuildString();
			}
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x00102DFC File Offset: 0x00100FFC
		public void BuildString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (StringPart stringPart in this.stringParts)
			{
				stringBuilder.Append(stringPart.GetString());
			}
			if (this.textTarget != null)
			{
				if (this.textMethod == TextSetMethod.SetText)
				{
					this.textTarget.text = stringBuilder.ToString();
				}
				else if (this.textMethod == TextSetMethod.AppendText)
				{
					Text text = this.textTarget;
					text.text += stringBuilder.ToString();
				}
				else if (this.textMethod == TextSetMethod.PrependText)
				{
					this.textTarget.text = stringBuilder.ToString() + this.textTarget.text;
				}
			}
			if (!string.IsNullOrEmpty(this.stringVariableName))
			{
				MonoSingleton<MapVarManager>.Instance.SetString(this.stringVariableName, stringBuilder.ToString(), false);
			}
		}

		// Token: 0x04002C46 RID: 11334
		[Header("Input")]
		public StringPart[] stringParts;

		// Token: 0x04002C47 RID: 11335
		[Header("Output")]
		[SerializeField]
		private string stringVariableName;

		// Token: 0x04002C48 RID: 11336
		[SerializeField]
		private TextSetMethod textMethod;

		// Token: 0x04002C49 RID: 11337
		[SerializeField]
		private Text textTarget;

		// Token: 0x04002C4A RID: 11338
		[Header("Events")]
		[SerializeField]
		private bool buildOnEnable;

		// Token: 0x04002C4B RID: 11339
		[SerializeField]
		private bool buildOnUpdate;
	}
}
