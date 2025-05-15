using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

// Token: 0x0200038A RID: 906
public sealed class RestrictedSerializationBinder : SerializationBinder
{
	// Token: 0x17000179 RID: 377
	// (get) Token: 0x060014D4 RID: 5332 RVA: 0x000A7AB9 File Offset: 0x000A5CB9
	public HashSet<Type> AllowedTypes { get; } = new HashSet<Type>();

	// Token: 0x060014D5 RID: 5333 RVA: 0x000A7AC4 File Offset: 0x000A5CC4
	public override Type BindToType(string assemblyName, string typeName)
	{
		string text = Assembly.CreateQualifiedName(assemblyName, typeName);
		foreach (Type type in this.AllowedTypes)
		{
			if (type.AssemblyQualifiedName == text)
			{
				return type;
			}
		}
		throw new SerializationException("Attempted to serialize restricted type: " + text);
	}
}
