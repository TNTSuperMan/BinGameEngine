using bgeruntime;
using System.Diagnostics;

byte[] rom = File.ReadAllBytes(Console.ReadLine());
const int count = 100;
long sum = 0;
for (int i = 0; i < count; i++)
{
    Stopwatch sw = new();
    Runtime vm = new Runtime(rom);
    sw.Start();
    vm.EmulateFrame();
    sw.Stop();
    sum += (sw.ElapsedTicks);
    if(i == 0) Console.WriteLine("Emulate count: " + vm.EmulateCount.ToString());
    Console.WriteLine(sw.ElapsedTicks);
}
Console.Write("Avg: ");
Console.WriteLine(sum / count);