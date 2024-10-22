using BMMCompiler.Parts;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace BMMCompiler
{
    public class ErrorInfo
    {
        public static string FileName = "";
        public static int Line;
        public static int Point;
        public readonly string message;
        public ErrorInfo(string msg)
        {
            message = "Error: ";
            message += msg + "\n    in ";
            message += FileName;
            message += ":" + Line.ToString();
            message += "." + Point.ToString();
        }
    }
    public class TagToken
    {
        private static int idx = 0;
        public static string Make()
        {
            string Token = "";
            foreach (char c in idx.ToString())
            {
                Token += c + (char)17;
            }
            idx++;
            return Token;
        }
    }
    public class Errors
    {
        private static List<ErrorInfo> _info = [];
        public static List<ErrorInfo> Infos
        {
            get { return _info; }
            set {  _info = value; }
        }
        public static bool Show()
        {
            if (Infos.Count == 0)
            {
                return false;
            }
            else
            {
                foreach (ErrorInfo e in Infos)
                {
                    Console.WriteLine(e.message);
                }
                return true;
            }
        }
    }
    public class Compiler
    {
        public static void Compile(string path, string memmapPath)
        {
            List<string> files = [];
            files.Add(path);
            List<string> completed = [];
            List<Parts.Module> modules = [];
            string TempPath = Path.GetTempPath() + "BMMCompiler_" + DateTime.Now.ToString() + Path.DirectorySeparatorChar;

            Console.WriteLine("Tokenizing...");
            while (files.Count > 0)
            {
                string file = files.Last();
                files.RemoveAt(files.Count - 1);
                if (!completed.Contains(file))
                {
                    ErrorInfo.FileName = file;
                    if (!File.Exists(file))
                    {
                        Console.WriteLine("Error: Not Found File: " + file);
                        return;
                    }
                    Console.WriteLine(file);
                    modules.Add(new Parts.Module(File.ReadAllText(file)));
                    completed.Add(Path.GetFullPath(path));
                }
            }
            Console.WriteLine("\nGenerating Memorymap...");
            ushort i = 0;
            string map = "Addr,Name,At\n";
            List<Variable> exported = [];
            foreach (Parts.Module m in modules)
            {
                foreach (Variable v in m.ExportVariables)
                {
                    exported.Add(v);
                    v.Shift(i++);
                    map += v.Rad16 + "," + v.Name + "," + completed[modules.IndexOf(m)] + "\n";
                }
            }
            foreach (Parts.Module m in modules)
            {
                foreach (Parts.Function f in m.Functions)
                {
                    foreach (Variable v in f.Variables)
                    {
                        v.Shift(i++);
                        map += v.Rad16 + "," + v.Name + "," + f.Name + " in " + completed[modules.IndexOf(m)] + "\n";
                    }
                }
            }
            File.WriteAllText(memmapPath, map);
            for(int j = 0;j < modules.Count; j++)
            {
                File.WriteAllText(completed[j]+".bge", modules[j].Compile(exported));
            }
            return;
        }
    }
    public class Variable
    {
        public readonly string Name;
        private ushort _addr;
        public Variable(string name)
        {
            Name = name;
            _addr = 0xA000;
        }
        public void Shift(ushort v)
        {
            _addr += v;
        }
        public string Rad16
        {
            get
            {
                return _addr.ToString("X4");
            }
        }
    }
}