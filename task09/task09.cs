using System;
using System.Reflection;
using System.IO;
using System.Linq;

public class task09
{
    public static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: MetadataViewer <path_to_dll>");
            return;
        }

        string dllPath = args[0];

        if (!File.Exists(dllPath))
        {
            Console.WriteLine($"Error: File not found: {dllPath}");
            return;
        }

        Assembly assembly = Assembly.LoadFile(dllPath);
        Console.WriteLine($"Metadata for: {dllPath}\n");

        foreach (Type type in assembly.GetTypes())
        {
            Console.WriteLine($"Class: {type.FullName}");

            Console.WriteLine("  Attributes:");
            foreach (CustomAttributeData attribute in type.CustomAttributes)
            {
                Console.WriteLine($"    - {attribute.AttributeType.FullName}");
                if (attribute.ConstructorArguments.Count > 0)
                {
                    Console.WriteLine("      Arguments:");
                    foreach (var arg in attribute.ConstructorArguments)
                    {
                        Console.WriteLine($"        - {arg.ArgumentType?.FullName}: {arg.Value}");
                    }
                }
            }

            Console.WriteLine("  Constructors:");
            foreach (ConstructorInfo constructor in type.GetConstructors())
            {
                Console.WriteLine($"    - {constructor.Name}");
                Console.WriteLine("      Parameters:");
                foreach (ParameterInfo parameter in constructor.GetParameters())
                {
                    Console.WriteLine($"        - {parameter.ParameterType.FullName} {parameter.Name}");
                }
            }

            Console.WriteLine("  Methods:");
            foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (method.DeclaringType != type) continue;
                Console.WriteLine($"    - {method.Name} (Returns: {method.ReturnType.FullName})");
                Console.WriteLine("      Parameters:");
                foreach (ParameterInfo parameter in method.GetParameters())
                {
                    Console.WriteLine($"        - {parameter.ParameterType.FullName} {parameter.Name}");
                }

                Console.WriteLine("      Attributes:");
                foreach (CustomAttributeData attribute in method.CustomAttributes)
                {
                    Console.WriteLine($"    - {attribute.AttributeType.FullName}");
                    if (attribute.ConstructorArguments.Count > 0)
                    {
                        Console.WriteLine("          Arguments:");
                        foreach (var arg in attribute.ConstructorArguments)
                        {
                            Console.WriteLine($"            - {arg.ArgumentType?.FullName}: {arg.Value}");
                        }
                    }
                }
            }

            Console.WriteLine();
        }

    }
}
