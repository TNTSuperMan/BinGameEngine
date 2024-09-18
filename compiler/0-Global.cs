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
    public class Errors
    {
        public static List<ErrorInfo> Infos = [];
        public static bool Show()
        {
            if(Infos.Count == 0)
            {
                return false;
            }
            else
            {
                foreach(ErrorInfo e in Infos)
                {
                    Console.WriteLine(e.message);
                }
                return true;
            }
        }
    }
    public class Compiler
    {
        public static string? Compile(string path, string memmapPath)
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
                        return null;
                    }
                    Console.WriteLine(file);
                    modules.Add(new Parts.Module(File.ReadAllText(file)));
                    completed.Add(file);
                }
            }
            Console.WriteLine("\nGenerating Memorymap...");
            ushort i = 0;
            string map = "Addr,Name,At\n";
            foreach(Parts.Module m in modules)
            {
                foreach(Variable v in m.ExportVariables)
                {
                    v.Shift(i++);
                    map += v.Rad16 + "," + v.Name + "," + completed[modules.IndexOf(m)] + "\n";
                }
            }
            foreach(Parts.Module m in modules)
            {
                foreach(Parts.Function f in m.Functions)
                {
                    foreach(Variable v in f.Variables)
                    {
                        v.Shift(i++);
                        map += v.Rad16 + "," + v.Name + "," + f.Name + " in " + completed[modules.IndexOf(m)] + "\n";
                    }
                }
            }
            return modules[0].Compile([]);
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
    namespace Parts
    {
        public abstract class Statement
        {
            public abstract string Compile(List<Variable> variables);
        }
    }
}
