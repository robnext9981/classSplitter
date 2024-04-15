namespace ClassSplitter.Tests
{
    public struct SmallFileStruct    { }

    public static class SmallFileBrotherClass 
    {
        public static string SayHelloToAnother(string friend)
        {
            return "Hello, my dear" + friend;
        }
    }
}