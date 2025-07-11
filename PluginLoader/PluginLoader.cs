using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Plugins; 

namespace PluginLoader;

public class Loader
{
    private string _path = string.Empty;

    public Loader(string plugins_folder_path)
    {
        _path = plugins_folder_path;
    }

    public void FindPluginLoaderAndLoad()
    {
        var plugin_types = new List<Type>();

        foreach (var dll_path in Directory.GetFiles(_path, "*.dll"))
        {
            try 
            {
                Assembly assembly = Assembly.LoadFrom(dll_path);

                foreach (var type in assembly.GetTypes())
                {
                    if (type.GetCustomAttribute<PluginLoadAttribute>() != null && typeof(IPlugin).IsAssignableFrom(type))
                    {
                        plugin_types.Add(type);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading assembly from {dll_path}: {ex.Message}");
            }
        }

        var plugins = plugin_types
            .Select(type => new
            {
                Type = type,
                Name = type.Name,
                DependsOn = type.GetCustomAttribute<PluginLoadAttribute>()?.DependsOn ?? Array.Empty<string>()
            })
            .ToList();

        var loaded = new HashSet<string>();
        int loadedCount = 0; 

        while (loaded.Count < plugins.Count)
        {
            loadedCount = 0;
            foreach (var plugin in plugins)
            {
                if (loaded.Contains(plugin.Name))
                    continue;

                if (plugin.DependsOn.All(loaded.Contains))
                {

                    if (Activator.CreateInstance(plugin.Type) is IPlugin pluginInstance)
                    {
                        pluginInstance.Execute();
                        loaded.Add(plugin.Name);
                        loadedCount++;
                        Console.WriteLine($"Plugin {plugin.Name} loaded and executed.");

                    }
                }
            }

            if (loadedCount == 0 && loaded.Count < plugins.Count)
            {
                Console.WriteLine("Circular dependency detected or dependency not met.  Aborting plugin loading.");
                break;
            }
        }

        if (loaded.Count == plugins.Count)
        {
            Console.WriteLine("All plugins loaded and executed successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to load all plugins. Loaded {loaded.Count} of {plugins.Count}.");
        }
    }
}