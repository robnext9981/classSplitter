namespace ClassSplitter.Tests
{
    public interface IFilettino
    {
        string SayHelloToAnother(string friend);
    }

    public class Filettino : IFilettino
    {
        public string SayHelloToAnother(string friend)
        {
            return "Hello, my dear" + friend;
        }
    }
}