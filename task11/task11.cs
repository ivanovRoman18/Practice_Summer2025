using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;


public interface ICalculator
{
    int Add(int a, int b);
    int Minus(int a, int b); 
    int Mul(int a, int b);
    int Div(int a, int b);
}

public static class DynamicCalculatorCreator
{
    public static ICalculator CreateCalculator()
    {
        string code = @"
using System;
public class Calculator : ICalculator 
{
    public int Add(int a, int b) => a + b;
    public int Minus(int a, int b) => a - b;
    public int Mul(int a, int b) => a * b;
    public int Div(int a, int b) => a / b;
}";

        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        var references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(ICalculator).Assembly.Location) // Добавляем ссылку на сборку с интерфейсом
        };

        var compilation = CSharpCompilation.Create(
            "DynamicCalculatorAssembly",
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var ms = new System.IO.MemoryStream();
        var result = compilation.Emit(ms);

        if (!result.Success)
        {
            throw new Exception("Compilation failed: " +
                string.Join("\n", result.Diagnostics));
        }

        ms.Seek(0, System.IO.SeekOrigin.Begin);
        var assembly = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(ms);

        var instance = Activator.CreateInstance(
            assembly?.GetType("Calculator") ??
            throw new InvalidOperationException("Type not found")
        ) ?? throw new InvalidOperationException("Instance creation failed");

        return (ICalculator)instance;
    }
}
