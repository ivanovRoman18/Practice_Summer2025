using System;

namespace Plugins
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginLoadAttribute : Attribute
    {
        public string[] DependsOn { get; }

        public PluginLoadAttribute(params string[] dependsOn)
        {
            DependsOn = dependsOn ?? Array.Empty<string>();
        }
    }

    public interface IPlugin
    {
        void Execute();
    }
}
