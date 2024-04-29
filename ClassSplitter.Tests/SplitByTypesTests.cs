namespace ClassSplitter.Tests;
using Xunit;
using System.Reflection;
using System.Linq;

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
        foreach (var directoryPath in Directory.GetDirectories(_baseExampleCsharpFolderPath, "Split*"))
        {
            Directory.Delete(directoryPath, true);
        }
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
        Assert.Equal(expectedSplittedFileCount, Directory.GetFiles(Path.Combine(_destinationDirectoryPath, $"Split{sourceFileNameWithoutExtension}"), $"*_Splitted.cs").Length);
    }

    [Fact]
    public void Should_Skip_SplittedFiles_Even_If_Source_And_DestinationDirectory_Are_The_Same()
    {
        // Arrange      
        var fullPathLocationTestAssembly = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);
        if(fullPathLocationTestAssembly.Parent == null) Assert.Fail("Executing Assembly directory not exists");

        string baseExampleCsharpFolderPath = Path.Combine(fullPathLocationTestAssembly.Parent.FullName, @"SampleCSharpFiles");
        var sourcefilePath = Path.Combine(baseExampleCsharpFolderPath, "SmallFile.cs");
        
        //Act 
        Program.Main([baseExampleCsharpFolderPath, baseExampleCsharpFolderPath]);

        //Arrange
        var expectedSplittedFileCount = 6;
        Assert.True(File.Exists(sourcefilePath));
        Assert.Equal(expectedSplittedFileCount, Directory.GetDirectories(baseExampleCsharpFolderPath,"Split*").Sum(dir => Directory.GetFiles(dir,"*.cs").Length));

    }

    [Fact]
    public void Should_Skip_ExcludedFiles_When_Specified()
    {
        // Arrange      
        var fullPathLocationTestAssembly = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);
        if(fullPathLocationTestAssembly.Parent == null) Assert.Fail("Executing Assembly directory not exists");

        string baseExampleCsharpFolderPath = Path.Combine(fullPathLocationTestAssembly.Parent.FullName, @"SampleCSharpFiles");
        var sourcefilePath = Path.Combine(baseExampleCsharpFolderPath, "SmallFile.cs");

        //Act 
        Program.Main([baseExampleCsharpFolderPath, baseExampleCsharpFolderPath, ".\\SmallFileWithEnum.cs"]);

        //Arrange
        var expectedSplittedFileCount = 4;
        Assert.True(File.Exists(sourcefilePath));
        Assert.Equal(expectedSplittedFileCount, Directory.GetDirectories(baseExampleCsharpFolderPath,"Split*").Sum(dir => Directory.GetFiles(dir,"*.cs").Length));

    }
}