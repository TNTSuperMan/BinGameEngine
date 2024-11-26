using bgeruntime;
using System.Diagnostics;

byte[] rom = File.ReadAllBytes(Console.ReadLine());
const int count = 10;
long sum = 0;
for (int i = 0; i < count; i++)
{
    Stopwatch sw = new();
    Runtime vm = new Runtime(rom);
    sw.Start();
    vm.EmulateFrame();
    sw.Stop();
    sum += (sw.ElapsedTicks);
    Console.WriteLine(sw.ElapsedTicks);
}
Console.Write("Sum: ");
Console.WriteLine(sum / count);