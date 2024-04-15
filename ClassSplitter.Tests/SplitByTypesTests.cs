namespace ClassSplitter.Tests;
using Xunit;
using System.Reflection;

public class SplitByTypesTests
{
    [Theory]
    [InlineData([@"SmallFile.cs", 2])]
    [InlineData([@"SmallFileWithEnum.cs", 2])]
    [InlineData([@"SmallFileWithStruct.cs", 2])]
    public void Given_OneFile_With_These_Types_Should_Be_Splitted(string fileName, int expectedSplittedFileCount)
    {
        // Arrange      
        var fullPathLocationTestAssembly = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);
        if(fullPathLocationTestAssembly.Parent == null) Assert.Fail("Executing Assembly directory not exists");

        string baseExampleCsharpFolderPath = Path.Combine(fullPathLocationTestAssembly.Parent.FullName, @"SampleCSharpFiles");
        var sourcefilePath =  Path.Combine(baseExampleCsharpFolderPath, fileName);
        string sourceFileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var destinationDirectoryPath = Path.Combine(baseExampleCsharpFolderPath, $"Split{sourceFileNameWithoutExtension}");
        
        //Act 
        Program.Main([baseExampleCsharpFolderPath, destinationDirectoryPath]);

        //Arrange
        Assert.True(File.Exists(sourcefilePath));
        Assert.Equal(expectedSplittedFileCount, Directory.GetFiles(destinationDirectoryPath, $"*_Splitted.cs").Length);
    }
}