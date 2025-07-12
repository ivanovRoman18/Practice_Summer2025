using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace task11;

public interface ICalculator
{
    int Add(int a, int b);
    int Minus(int a, int b);
    int Mul(int a, int b);
    int Div(int a, int b);
}

public class DynamicCalculatorCreator
{
    public static ICalculator CreateCalculator()
    {
        string code = @"
using task11;
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
            MetadataReference.CreateFromFile(typeof(ICalculator).Assembly.Location)
        };

        var compilation = CSharpCompilation.Create(
            "DynamicCalculatorAssembly",
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);

        if (!result.Success)
        {
            throw new Exception("Compilation failed: " +
                string.Join("\n", result.Diagnostics));
        }

        ms.Seek(0, SeekOrigin.Begin);
        var assembly = Assembly.Load(ms.ToArray());
        var calculatorType = assembly.GetType("Calculator") ?? 
            throw new InvalidOperationException("Type not found");

        return (ICalculator)Activator.CreateInstance(calculatorType)!;
    }
}