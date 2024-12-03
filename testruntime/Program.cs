using bgeruntime;
using System.Text.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("usage: testruntime [srcbin] [outpath]");
            return;
        }
        Runtime vm = new Runtime(File.ReadAllBytes(args[0]));
        vm.EmulateFrame();
        File.WriteAllText(args[1], JsonSerializer.Serialize(vm.debug.StackList.ToList()));
    }
}
