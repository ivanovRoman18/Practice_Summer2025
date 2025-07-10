using System;
using System.Linq;
using System.Reflection;

namespace task07
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public class DisplayNameAttribute : Attribute
    {
        public string DisplayName { get; }

        public DisplayNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class VersionAttribute : Attribute
    {
        public int Major { get; }
        public int Minor { get; }

        public VersionAttribute(int major, int minor)
        {
            Major = major;
            Minor = minor;
        }
    }
    [DisplayName("Пример класса")]
    [Version(1, 0)]
    public class SampleClass
    {
        [DisplayName("Числовое свойство")]
        public int Number { get; set; }

        [DisplayName("Тестовый метод")]
        public void TestMethod()
        {

        }

    }
    public static class ReflectionHelper
    {
        public static void PrintTypeInfo(Type type)
        {
            var displayNameAttribute = type.GetCustomAttribute<DisplayNameAttribute>();
            Console.WriteLine($"Класс: {(displayNameAttribute != null ? displayNameAttribute.DisplayName : type.Name)}");

            var versionAttribute = type.GetCustomAttribute<VersionAttribute>();
            if (versionAttribute != null)
            {
                Console.WriteLine($"Версия: {versionAttribute.Major}.{versionAttribute.Minor}");
            }

            Console.WriteLine("Методы:");
            var methodsWithDisplayName = type.GetMethods()
                .Where(method => method.GetCustomAttribute<DisplayNameAttribute>() != null)
                .Select(method => method.Name);

            foreach (var methodName in methodsWithDisplayName)
            {
                Console.WriteLine($" {methodName}");
            }

            Console.WriteLine("Свойства:");
            var propertiesWithDisplayName = type.GetProperties()
                .Where(property => property.GetCustomAttribute<DisplayNameAttribute>() != null)
                .Select(property => property.Name);
            foreach (var propertyName in propertiesWithDisplayName)
            {
                Console.WriteLine($" {propertyName}");
            }
        }
    }
}
