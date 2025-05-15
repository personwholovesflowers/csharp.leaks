using System;
using JetBrains.Annotations;

// Token: 0x020002FF RID: 767
[BaseTypeRequired(typeof(MonoSingleton<>))]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ConfigureSingletonAttribute : Attribute
{
	// Token: 0x1700016A RID: 362
	// (get) Token: 0x0600117A RID: 4474 RVA: 0x000884AC File Offset: 0x000866AC
	public SingletonFlags Flags { get; }

	// Token: 0x0600117B RID: 4475 RVA: 0x000884B4 File Offset: 0x000866B4
	public ConfigureSingletonAttribute(SingletonFlags flags)
	{
		this.Flags = flags;
	}
}
