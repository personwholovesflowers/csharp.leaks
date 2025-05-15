using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmplifyImpostors
{
	// Token: 0x02000310 RID: 784
	[CreateAssetMenu(fileName = "New Bake Preset", order = 86)]
	public class AmplifyImpostorBakePreset : ScriptableObject
	{
		// Token: 0x04001010 RID: 4112
		[SerializeField]
		public Shader BakeShader;

		// Token: 0x04001011 RID: 4113
		[SerializeField]
		public Shader RuntimeShader;

		// Token: 0x04001012 RID: 4114
		[SerializeField]
		public PresetPipeline Pipeline;

		// Token: 0x04001013 RID: 4115
		[SerializeField]
		public int AlphaIndex;

		// Token: 0x04001014 RID: 4116
		[SerializeField]
		public List<TextureOutput> Output = new List<TextureOutput>();
	}
}
