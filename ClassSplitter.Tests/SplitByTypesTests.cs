namespace ClassSplitter.Tests;
using Xunit;
using System.Reflection;

public class SplitByTypesTests : IDisposable
{
    private string _baseExampleCsharpFolderPath;
    private string _destinationDirectoryPath;

    public SplitByTypesTests()
    {
        var fullPathLocationTestAssembly = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);
        if(fullPathLocationTestAssembly.Parent == null) Assert.Fail("Executing Assembly directory not exists");
        _baseExampleCsharpFolderPath = Path.Combine(fullPathLocationTestAssembly.Parent.FullName, @"SampleCSharpFiles");
        _destinationDirectoryPath = Path.Combine(_baseExampleCsharpFolderPath, $"Split");
    }

     public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        //Clean-up
        Directory.Delete(_destinationDirectoryPath, true);
    }

    [Theory]
    [InlineData([@"SmallFile.cs", 2])]
    [InlineData([@"SmallFileWithEnum.cs", 2])]
    [InlineData([@"SmallFileWithStruct.cs", 2])]
    public void Given_OneFile_With_These_Types_Should_Be_Splitted(string fileName, int expectedSplittedFileCount)
    {
        // Arrange      

        var sourcefilePath =  Path.Combine(_baseExampleCsharpFolderPath, fileName);
        string sourceFileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        _destinationDirectoryPath = Path.Combine(_baseExampleCsharpFolderPath, $"Split{sourceFileNameWithoutExtension}");
        
        //Act 
        Program.Main([_baseExampleCsharpFolderPath, _destinationDirectoryPath]);

        //Arrange
        Assert.True(File.Exists(sourcefilePath));
        Assert.Equal(expectedSplittedFileCount, Directory.GetFiles(_destinationDirectoryPath, $"*_Splitted.cs").Length);

    }
}