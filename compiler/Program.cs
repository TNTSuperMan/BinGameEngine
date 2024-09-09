internal class Program
{
    private static void Main(string[] args)
    {
        if(args.Length < 2)
        {
            Console.WriteLine("Using: compiler [EntryFile] [OutputFile] [AssemblyDir(Default:TempDir)]");
            return;
        }

    }
}