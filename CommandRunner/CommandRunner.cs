using CommandLib;
using System.Reflection;

namespace CommandRunner
{
    public class Program
    {
        public static void Main()
        {
            const string commandsDll = "FileSystemCommands.dll";
            var commandAssembly = Assembly.LoadFrom(commandsDll);

            var testData = SetupTestEnvironment();

            ExecuteSizeCommand(commandAssembly, testData.SizeTestDir);
            ExecuteFindCommand(commandAssembly, testData.FindTestDir);

            CleanupTestEnvironment(testData);
        }

        private static (string SizeTestDir, string FindTestDir) SetupTestEnvironment()
        {
            var sizeTestDir = Path.Combine(Path.GetTempPath(), "SizeTest_" + Guid.NewGuid());
            var findTestDir = Path.Combine(Path.GetTempPath(), "FindTest_" + Guid.NewGuid());

            Directory.CreateDirectory(sizeTestDir);
            File.WriteAllText(Path.Combine(sizeTestDir, "test1.txt"), "Test content");
            Directory.CreateDirectory(findTestDir);
            File.WriteAllText(Path.Combine(findTestDir, "file.txt"), "Text content");

            return (sizeTestDir, findTestDir);
        }

        private static void ExecuteSizeCommand(Assembly assembly, string testDir)
        {
            var commandType = assembly.GetType("FileSystemCommands.DirectorySizeCommand");
            var command = commandType != null
                ? Activator.CreateInstance(commandType, testDir) as dynamic
                : null;

            command?.Execute();
            Console.WriteLine($"Размер каталога: {command?.DirectorySize ?? 0} байт");
        }

        private static void ExecuteFindCommand(Assembly assembly, string testDir)
        {
            var commandType = assembly.GetType("FileSystemCommands.FindFilesCommand");
            var command = commandType != null
                ? Activator.CreateInstance(commandType, testDir, "*.txt") as dynamic
                : null;

            command?.Execute();

            Console.WriteLine("\nНайденные файлы:");
            if (command?.FoundFiles != null)
            {
                foreach (var file in command.FoundFiles)
                    Console.WriteLine(Path.GetFileName(file));
            }
        }

        private static void CleanupTestEnvironment((string SizeTestDir, string FindTestDir) dirs)
        {
            if (Directory.Exists(dirs.SizeTestDir))
                Directory.Delete(dirs.SizeTestDir, true);

            if (Directory.Exists(dirs.FindTestDir))
                Directory.Delete(dirs.FindTestDir, true);
        }
    }
}
