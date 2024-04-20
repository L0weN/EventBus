using System;
using System.Collections.Generic;
using System.Reflection;

public static class PredefinedAssemblyUtil
{
    enum AssemblyType
    {
        AssemblyCSharp,
        AssemblyCSharpEditor,
        AssemblyCSharpFirstPass,
        AssemblyCSharpFirstPassEditor,
    }

    static AssemblyType? GetAssemblyType(string assemblyName)
    {
        return assemblyName switch
        {
            "Assembly-CSharp" => AssemblyType.AssemblyCSharp,
            "Assembly-CSharp-Editor" => AssemblyType.AssemblyCSharpEditor,
            "Assembly-CSharp-firstpass" => AssemblyType.AssemblyCSharpFirstPass,
            "Assembly-CSharp-firstpass-Editor" => AssemblyType.AssemblyCSharpFirstPassEditor,
            _ => null,
        };
    }

    static void AddTypesFromAssembly(Type[] assembly, Type interfaceType, ICollection<Type> results)
    {
        if (assembly == null) return;
        for (int i = 0; i < assembly.Length; i++)
        {
            Type type = assembly[i];
            if (type != interfaceType && interfaceType.IsAssignableFrom(type))
            {
                results.Add(type);
            }
        }
    }

    public static List<Type> GetTypes(Type interfaceType)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

        Dictionary<AssemblyType, Type[]> assemblyTypes = new Dictionary<AssemblyType, Type[]>();
        List<Type> types = new List<Type>();

        for (int i = 0; i < assemblies.Length; i++)
        {
            AssemblyType? assemblyType = GetAssemblyType(assemblies[i].GetName().Name);
            if (assemblyType != null)
            {
                assemblyTypes.Add((AssemblyType)assemblyType, assemblies[i].GetTypes());
            }
        }

        AddTypesFromAssembly(assemblyTypes[AssemblyType.AssemblyCSharp], interfaceType, types);
        AddTypesFromAssembly(assemblyTypes[AssemblyType.AssemblyCSharpFirstPass], interfaceType, types);

        return types;
    }
}
