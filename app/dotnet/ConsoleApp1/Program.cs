namespace HelloWorld
{
    class Program
    {
        static int Main(string[] args)
        {
            SomeClass someClass = new SomeClass();
            someClass.Value1 = 1;
            return Helper(someClass);
        }
        private static int Helper(SomeClass someClass) 
        {
            return someClass.Value1;
        }
    }
    public class SomeClass
    {
        public int Value1;
    }
}
