namespace ClassSplitter.Tests;
using Xunit;
using System.Reflection;

public class SplitMainTests
{
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
        Assert.Equal(expectedSplittedFileCount, Directory.GetDirectories(baseExampleCsharpFolderPath,"Split*").Sum(dir => Directory.GetFiles(dir,"*_Splitted.cs").Length));

        //Clean-up
        Directory.Delete(baseExampleCsharpFolderPath, true);
    }
}