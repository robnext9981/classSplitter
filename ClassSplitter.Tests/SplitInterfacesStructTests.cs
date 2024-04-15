namespace ClassSplitter.Tests;
using Xunit;
using System.Reflection;

public class SplitInterfacesStructTests
{
    [Fact]
    public void Given_OneFile_With_OneClass_And_OneInterface_Should_Create_Two_Files()
    {
        // Arrange      
        var fullPathLocationTestAssembly = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);
        if(fullPathLocationTestAssembly.Parent == null) Assert.Fail("Executing Assembly directory not exists");

        string baseExampleCsharpFolderPath = Path.Combine(fullPathLocationTestAssembly.Parent.FullName, @"SampleCSharpFiles");
        var sourcefilePath = Path.Combine(baseExampleCsharpFolderPath, "SmallFile.cs");
        var destinationDirectoryPath = Path.Combine(baseExampleCsharpFolderPath, "SplitSmallFile");
        
        //Act 
        Program.Main([baseExampleCsharpFolderPath]);

        //Arrange
        Assert.True(File.Exists(sourcefilePath));
        Assert.Equal(2, Directory.GetFiles(destinationDirectoryPath, "*_Splitted.cs").Length);
    }

    [Fact]
    public void Given_OneFile_With_OneClass_And_Struct_Should_Create_Two_Files()
    {
        // Arrange      
        var fullPathLocationTestAssembly = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);
        if(fullPathLocationTestAssembly.Parent == null) Assert.Fail("Executing Assembly directory not exists");

        string baseExampleCsharpFolderPath = Path.Combine(fullPathLocationTestAssembly.Parent.FullName, @"SampleCSharpFiles");
        var sourcefilePath =  Path.Combine(baseExampleCsharpFolderPath, @"SmallFileWithStruct.cs");
        var destinationDirectoryPath = Path.Combine(baseExampleCsharpFolderPath, "SplitSmallFileWithStruct");
        
        //Act 
        Program.Main([baseExampleCsharpFolderPath]);

        //Arrange
        Assert.True(File.Exists(sourcefilePath));
        Assert.Equal(2, Directory.GetFiles(destinationDirectoryPath, "*_Splitted.cs").Length);
    }

    [Fact]
    public void Given_OneFile_With_OneClass_And_Enum_Should_Create_Two_Files()
    {
        // Arrange      
        var fullPathLocationTestAssembly = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);
        if(fullPathLocationTestAssembly.Parent == null) Assert.Fail("Executing Assembly directory not exists");

        string baseExampleCsharpFolderPath = Path.Combine(fullPathLocationTestAssembly.Parent.FullName, @"SampleCSharpFiles");
        var sourcefilePath =  Path.Combine(baseExampleCsharpFolderPath, @"SmallFileWithEnum.cs");
        var destinationDirectoryPath = Path.Combine(baseExampleCsharpFolderPath, "SplitSmallFileWithEnum");
        
        //Act 
        Program.Main([baseExampleCsharpFolderPath]);

        //Arrange
        Assert.True(File.Exists(sourcefilePath));
        Assert.Equal(2, Directory.GetFiles(destinationDirectoryPath, "*_Splitted.cs").Length);
    }
}