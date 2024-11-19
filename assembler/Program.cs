using compiler;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        if(args.Length != 2)
        {
            Console.WriteLine("使用法: assembler.exe [ソース] [出力]");
        }
        else
        {
            try
            {
                Console.WriteLine("--- タグ・構文の解析");
                List<string> impstack = new List<string>();
                List<string> imported = new List<string>();
                List<Module> modules = new List<Module>();
                impstack.Add(args[0]);
                while (impstack.Count > 0)
                {
                    string path = impstack.Last();
                    impstack.Remove(path);
                    if (imported.Contains(path)) continue;
                    Console.Write(path);
                    imported.Add(path);
                    modules.Add(new Module(path));
                    Console.WriteLine(" - "+modules.Last().length + "b");
                    foreach (string i in modules.Last().importPath) impstack.Add(i);
                }

                List<string> expName = new List<string>();
                List<ushort> expAddr = new List<ushort>();

                ushort shift = 0;
                foreach (Module m in modules)
                {
                    m.Shift(shift);
                    shift += m.length;
                    for (int i = 0;i < m.exportedTagName.Count; i++)
                    {
                        Console.WriteLine(m.exportedTagName[i]+": "+m.exportedTagPoint);
                        expName.Add(m.exportedTagName[i]);
                        expAddr.Add(m.exportedTagPoint[i]);
                    }
                }

                Console.WriteLine("--- コンパイル");
                List<byte> ret = new List<byte>();
                foreach(Module m in modules)
                {
                    Console.WriteLine(m.fpath);
                    byte[] d = m.Compile(expName, expAddr);
                    foreach (byte b in d) ret.Add(b);
                }
                File.WriteAllBytes(args[1], ret.ToArray());
                Console.WriteLine("\n正常に\"" + args[1] + "\"に出力されました。");
                return;
            }
            catch (BGEException ex)
            { 
                Console.WriteLine("文法エラー: "+ex.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine("コンパイラエラー:" + e.Message);
            }
        }
    }
}