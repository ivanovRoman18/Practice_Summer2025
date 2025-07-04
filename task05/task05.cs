namespace task05;

using System;
using System.Reflection;
using System.Collections.Generic;

public class ClassAnalyzer
{
    private readonly Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type;
    }

    public IEnumerable<string> GetPublicMethods()
    {
        return _type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Select(m => m.Name);
    }

    public IEnumerable<string> GetAllFields()
    {
        return _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Select(f => f.Name);
    }

    public IEnumerable<string> GetMethodParams(string methodName)
        => _type.GetMethod(methodName)?
           .GetParameters()
           .Select(p => p.Name)
           .Where(name => name != null)!;

    public IEnumerable<string> GetProperties()
    {
        return _type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(p => p.Name);
    }

    public bool HasAttribute<T>() where T : Attribute
    {
        return _type.GetCustomAttributes(typeof(T), true).Length > 0;
    }
}
