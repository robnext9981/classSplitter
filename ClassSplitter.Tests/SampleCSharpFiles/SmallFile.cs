namespace ClassSplitter.Tests
{
    public interface ISmallFile
    {
        string SayHelloToAnother(string friend);
    }

    public class SmallFile : ISmallFile
    {
        public string SayHelloToAnother(string friend)
        {
            return "Hello, my dear" + friend;
        }
    }
}