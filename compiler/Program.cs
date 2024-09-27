using BMMCompiler;

internal class Program
{
    private static void Main(string[] args)
    {
        /*if(args.Length < 2)
        {
            Console.WriteLine("Using: compiler [EntryFile] [OutputFile] [AssemblyDir(Default:TempDir)]");
            return;
        }*/
        BMMCompiler.Parts.Module m = new BMMCompiler.Parts.Module(
            "func Load(dea,r){\n" +
            "    var TTT;\n" +
            "    TTT = 0;\n" +
            "    rect(10,10,10,10,r,dea,TTT);\n" +
            "    redraw();\n" +
            "    return(12);\n" +
            "}");
        Console.WriteLine(m.Compile([]));
        Errors.Show();
    }
}