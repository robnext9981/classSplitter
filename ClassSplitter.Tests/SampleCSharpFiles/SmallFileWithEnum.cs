namespace ClassSplitter.Tests
{
    public enum Smally  { }

    public static class SmallFileSisterClass 
    {
        public static string SayHelloToAnother(string friend)
        {
            return "Hello, my dear" + friend;
        }
    }
}