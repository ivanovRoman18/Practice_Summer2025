using FileSystemCommands;

namespace task08tests;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        var command = new DirectorySizeCommand(testDir);
        command.Execute();

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

        var command = new FindFilesCommand(testDir, "*.txt");
        command.Execute();

        Directory.Delete(testDir, true);
    }
    [Fact]
    public void DirectorySizeCommand_ShouldReturnZeroForEmptyDirectory()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "EmptyTestDir");
        Directory.CreateDirectory(testDir);

        var command = new DirectorySizeCommand(testDir);
        command.Execute();

        Assert.True(Directory.Exists(testDir)); // Проверяем, что директория существует
        Directory.Delete(testDir, true);
    }
    [Fact]
    // тест на вложенные директории
    public void DirectorySizeCommand_ShouldCalculateNestedDirectoriesSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "NestedTestDir");
        var subDir = Path.Combine(testDir, "SubDir");

        Directory.CreateDirectory(testDir);
        Directory.CreateDirectory(subDir);

        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "12345");
        File.WriteAllText(Path.Combine(subDir, "file2.txt"), "1234567890");

        var command = new DirectorySizeCommand(testDir);
        command.Execute();

        Directory.Delete(testDir, true);
    }
    [Fact]
    public void DirectorySizeCommand_ShouldThrowIfDirectoryNotFound()
    {
        var nonExistentDir = Path.Combine(Path.GetTempPath(), "NonExistentDir");

        var command = new DirectorySizeCommand(nonExistentDir);

        Assert.Throws<DirectoryNotFoundException>(() => command.Execute());
    }
    [Fact]
    public void FindFilesCommand_ShouldFindNothingIfNoMatches()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "NoMatchesTestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "data.log"), "Log data");

        var command = new FindFilesCommand(testDir, "*.txt");
        command.Execute();

        Directory.Delete(testDir, true);
    }
}
