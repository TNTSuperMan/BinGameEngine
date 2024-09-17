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
        public static string? Compile(string path)
        {
            List<string> files = [];
            files.Add(path);
            List<string> completed = [];
            List<Parts.Module> modules = [];
            while (files.Count > 0)
            {
                string file = files.Last();
                files.RemoveAt(files.Count - 1);
                if (!completed.Contains(file))
                {
                    ErrorInfo.FileName = file;
                    if (!File.Exists(file))
                    {
                        Console.WriteLine("Not Found File: " + file);
                        return null;
                    }
                    modules.Add(new Parts.Module(File.ReadAllText(file)));
                }
            }
            return "";
        }
    }
    public class Variable
    {
        public readonly string Name;
        private ushort _addr;
        public Variable(string name)
        {
            Name = name;
            _addr = 0;
        }
        public void Shift(ushort v)
        {
            _addr += v;
        }
        public string Rad16
        {
            get
            {
                return Convert.ToString(_addr, 16);
            }
        }
    }
    namespace Parts
    {
        public abstract class Statement
        {
            public abstract string Compile(List<Variable> variables, List<string> functions, List<string> exportedFunctions);
        }
    }
}
