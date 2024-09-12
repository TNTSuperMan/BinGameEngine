using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
/**
 * 1.変数マップ
 */
namespace BMMCompiler
{
    public class BMMCompilerException : Exception
    {
        public BMMCompilerException(string message) : base(message) { }
    }
    public class Compiler
    {
        public static string Compile(string path)
        {
            List<string> files = [];
            files.Add(path);
            List<string> completed = [];
            List<Token.Module> modules = [];
            while(files.Count > 0)
            {
                string file = files.Last();
                files.RemoveAt(files.Count - 1);
                if (!completed.Contains(file))
                {
                    if (!File.Exists(file))
                    {
                        throw new BMMCompilerException("Not Found File: " + file);
                    }
                    modules.Add(new Token.Module(File.ReadAllText(file)));
                }
            }
            return "";
        }
    }
    namespace Token
    {
        public class Variable
        {
            private readonly string _name;
            private ushort _addr;
            public Variable(string name)
            {
                _name = name;
                _addr = 0;
            }
            public void Shift(ushort v)
            {
                _addr += v;
            }
        }
        public class Module
        {
            private enum CompileMode
            {
                None,
                Comment,
                Function,
                Include,
                Define,
                Export
            }
            public List<string> Imports;
            public List<Variable> ExportVariables;
            public List<Function> Functions;
            public Module(string src)
            {
                Functions = [];
                ExportVariables = [];
                Imports = [];
                string stack = string.Empty;
                int i = 0;
                int cblayer = 0;
                bool isComment = false;
                CompileMode mode = CompileMode.None;
                while (src.Length > i)
                {
                    switch (mode)
                    {
                        case CompileMode.None:
                            switch (src[i])
                            {
                                case '\r':
                                case '\n':
                                case '\t':
                                case ' ':
                                    break;
                                default:
                                    stack += src[i];
                                    break;
                            }
                            bool ________ = true;
                            switch (stack)
                            {
                                case "//":
                                    mode = CompileMode.Comment; break;
                                case "#include":
                                    i++;
                                    mode = CompileMode.Include; break;
                                case "#define":
                                    i++;
                                    mode = CompileMode.Define; break;
                                case "export":
                                    i++;
                                    mode = CompileMode.Export; break;
                                case "void":
                                case "func":
                                    i -= 4; cblayer = 0;
                                    mode = CompileMode.Function; break;
                                default: ________ = false; break;
                            }
                            if (________)
                            {
                                stack = "";
                            }
                            break;
                        case CompileMode.Comment:
                            if (src[i] == '\n')
                            {
                                mode = CompileMode.None;
                            }
                            break;
                        case CompileMode.Function:
                            if (isComment)
                            {
                                if (src[i] == '\n')
                                {
                                    isComment = false;
                                }
                            }
                            else if (src[i] == '/')
                            {
                                if (src.Length > i + 1)
                                {
                                    if (src[i+1] == '/')
                                    {
                                        i++;
                                        isComment = true;
                                    }
                                } 
                            }
                            else if (src[i] == '{')
                            {
                                stack += src[i];
                                cblayer++;
                            }
                            else if (src[i] == '}')
                            {
                                stack += src[i];
                                cblayer--;
                                if (cblayer <= 0)
                                {
                                    mode = CompileMode.None;
                                    Functions.Add(new Function(stack));
                                }
                            }
                            else
                            {
                                stack += src[i];
                            }
                            break;
                    }
                    i++;
                }
            }
        }
        public class Function
        {
            public bool isVoid;
            public string Name;
            public List<string> Arguments;
            public List<Variable> Variables;
            public Function(string src)
            {
                Arguments = [];
                Variables = [];
                //Name = "";
                Name = src;
            }
            public string Compile(List<Variable> exportedVariables, List<string> exportedFunctions)
            {
                return "";
            }
        }
        namespace Statements
        {
            public abstract class Statememnt
            {
                public static Statememnt FromString(string src)
                {
                    string stack = string.Empty;
                    int i = 0;
                    while(src.Length > i)
                    {

                    }
                }
                public abstract string Compile(List<Variable> variables, List<Variable> exportedVariables, List<string> exportedFunctions);
            }
            public class Substitute : Statements.Statememnt
            {
                public override string Compile(List<Variable> variables, List<Variable> exportedVariables, List<string> exportedFunctions)
                {
                    return "";
                }
            }
        }
    }
}
