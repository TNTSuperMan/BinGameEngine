using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
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
            List<Parts.Module> modules = [];
            while (files.Count > 0)
            {
                string file = files.Last();
                files.RemoveAt(files.Count - 1);
                if (!completed.Contains(file))
                {
                    if (!File.Exists(file))
                    {
                        throw new BMMCompilerException("Not Found File: " + file);
                    }
                    modules.Add(new Parts.Module(File.ReadAllText(file)));
                }
            }
            return "";
        }
    }
    namespace Parts
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
            public string Rad16
            {
                get
                {
                    return Convert.ToString(_addr, 16);
                }
            }
            public string Name
            {
                get
                {
                    return _name;
                }
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
                                    if (src[i + 1] == '/')
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
            public string name;
            public List<string> arguments;
            public List<Variable> variables;
            public List<Statement> statements;
            private enum FuncBuildMode
            {
                Type,
                NameBeforeSpace, Name,
                ArgumentBeforeSpace, Argument,
                CodeBeforeSpace, Code
            }
            public Function(string src)
            {
                arguments = [];
                variables = [];
                statements = [];
                name = "";
                int i = 0;
                FuncBuildMode mode = FuncBuildMode.Type;
                string stack = "";
                while (src.Length > i)
                {
                    switch (mode)
                    {
                        case FuncBuildMode.Type:
                            switch (src[i])
                            {
                                case '\n':
                                case '\r':
                                case '\t':
                                case ' ':
                                    mode = FuncBuildMode.NameBeforeSpace;
                                    switch (stack)
                                    {
                                        case "void":
                                            isVoid = true;
                                            break;
                                        case "func":
                                            isVoid = false;
                                            break;
                                        default:
                                            throw new BMMCompilerException("Unknown function define: " + stack);
                                    }
                                    stack = "";
                                    break;
                                default:
                                    stack += src[i];
                                    break;
                            }
                            break;
                        case FuncBuildMode.NameBeforeSpace:
                            switch (src[i])
                            {
                                case '\n':
                                case '\r':
                                case '\t':
                                case ' ':
                                    break;
                                default:
                                    i--;
                                    mode = FuncBuildMode.Name;
                                    break;
                            }
                            break;
                        case FuncBuildMode.Name:
                            switch (src[i])
                            {
                                case '\n':
                                case '\r':
                                case '\t':
                                case ' ':
                                case '(':
                                    name = stack;
                                    mode = FuncBuildMode.ArgumentBeforeSpace;
                                    stack = "";
                                    if (src[i] == '(') i--;
                                    break;
                                default:
                                    stack += src[i];
                                    break;
                            }
                            break;
                        case FuncBuildMode.ArgumentBeforeSpace:
                            if (src[i] == '(')
                            {
                                mode = FuncBuildMode.Argument;
                            }
                            break;
                        case FuncBuildMode.Argument:
                            switch (src[i])
                            {
                                case ')':
                                    if (stack != "")
                                    {
                                        arguments.Add(stack);
                                        stack = "";
                                    }
                                    mode = FuncBuildMode.CodeBeforeSpace;
                                    break;
                                case ',':
                                    arguments.Add(stack);
                                    stack = "";
                                    break;
                                case '\t':
                                case '\n':
                                case '\r':
                                case ' ':
                                    break;
                                default:
                                    stack += src[i];
                                    break;
                            }
                            break;
                        case FuncBuildMode.CodeBeforeSpace:
                            if (src[i] == '{')
                            {
                                mode = FuncBuildMode.Code;
                            }
                            break;
                        case FuncBuildMode.Code:
                            if (src[i] == ';')
                            {
                                stack = stack.Trim();
                                if (Regex.IsMatch("^var\\s+\\w+", stack))
                                {

                                }
                                else if(Regex.IsMatch("^\\w+\\s*=.+",stack))
                                {
                                    statements.Add(new Expressions.Substitution(stack));
                                    stack = "";
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
            public string Compile(List<Variable> exportedVariables, List<string> functions, List<string> exportedFunctions)
            {
                List<Variable> allVar = [];
                foreach (Variable v in variables) allVar.Add(v);
                foreach (Variable v in exportedVariables) allVar.Add(v);
                string ret = name + ":\n";
                foreach(Statement s in statements)
                {
                    ret += s.Compile(allVar,functions , exportedFunctions);
                }
                return ret;
            }
        }
        public abstract class Statement
        {
            public abstract string Compile(List<Variable> variables, List<string> functions, List<string> exportedFunctions);
        }
        namespace Expressions
        {
            public class Expression : Statement
            {
                public List<Expression> pushes;
                public ExpressionMode mode;
                public Operator @operator;
                public string func;
                public ushort num;
                public enum ExpressionMode
                {
                    Operation, Variable, Number, ExportedFunc, Func, NativeFunc
                }
                public enum Operator
                {
                    Pls,Sub,Mul,Div,Rem,Nand,Equal,Greater
                }
                private enum ExpFncCompileMode
                {
                    Name,ArgBeforeSpace,Arg
                }
                public Expression(string src)
                {
                    /**9/13メモ
                     * ここ演算子を判定する
                     * 優先順位は(*Mul/Div%Rem)(+Pls-Sub)(=Equal>Greater)(^Nand)
                     * 関数だったらそこがさき
                     */
                    src = src.Trim();
                    func = "";
                    pushes = [];
                    if (Regex.IsMatch("^\\w+$", src)) // variable
                    {
                        mode = ExpressionMode.Variable;
                        func = src;
                    }else if(Regex.IsMatch("^\\w+\\(.*\\)$", src)) //func
                    {
                        mode = ExpressionMode.Variable;
                        ExpFncCompileMode fncmode = ExpFncCompileMode.Name;
                        int i = 0;
                        string stack = "";
                        int KakkoLayer = 0;
                        while(i < src.Length)
                        {
                            switch (fncmode)
                            {
                                case ExpFncCompileMode.Name:
                                    switch (src[i])
                                    {
                                        case '\t':
                                        case '\r':
                                        case '\n':
                                        case ' ':
                                        case '(':
                                            if (src[i] == '(') i--;
                                            func = stack;
                                            fncmode = ExpFncCompileMode.ArgBeforeSpace;
                                            break;
                                        default:
                                            stack += src[i];
                                            break;
                                    }
                                    break;
                                case ExpFncCompileMode.ArgBeforeSpace:
                                    if (src[i] == '(')
                                    {
                                        fncmode = ExpFncCompileMode.Arg;
                                    }
                                    break;
                                case ExpFncCompileMode.Arg:
                                    if (src[i] == '(') 
                                    { 
                                        KakkoLayer++;
                                        stack += src[i];
                                    }else if (src[i] == ')')
                                    {
                                        KakkoLayer--;
                                        if (KakkoLayer < 0)
                                        {
                                            pushes.Add(new Expression(stack));
                                            return;
                                        }
                                        stack += src[i];
                                    }
                                    else if (src[i] == ',' && KakkoLayer == 0)
                                    {
                                        pushes.Add(new Expression(stack));
                                        stack = "";
                                    }
                                    else stack += src[i];
                                    break;
                            }
                            i++;
                        }
                    }
                    else if(Regex.IsMatch("^\\d{1,5}$", src)) //number
                    {
                        mode = ExpressionMode.Number;
                        if (!ushort.TryParse(src,out num))
                        {
                            throw new BMMCompilerException("Number Max is 65535 but input is: " + src);
                        }
                    }
                }
                public override string Compile(List<Variable> variables, List<string> functions, List<string> exportedFunctions)
                {
                    string ret = "";
                    switch (mode)
                    {
                        case ExpressionMode.Operation:
                            if(pushes.Count != 2)
                            {
                                throw new BMMCompilerException("Argument is Not 2");
                            }
                            ret += pushes[0].Compile(variables, functions, exportedFunctions);
                            ret += pushes[1].Compile(variables, functions, exportedFunctions);
                            ret += "/ ";
                            switch (@operator)
                            {
                                case Operator.Pls: ret += "pls"; break;
                                case Operator.Sub: ret += "sub"; break;
                                case Operator.Mul: ret += "mul"; break;
                                case Operator.Div: ret += "div"; break;
                                case Operator.Rem: ret += "rem"; break;
                                case Operator.Nand: ret += "nand"; break;
                                case Operator.Equal: ret += "equal"; break;
                                case Operator.Greater: ret += "greater"; break;
                            }
                            break;
                        case ExpressionMode.Variable:
                            Variable? v = variables.Find(v => v.Name == func);
                            if (v == null)
                            {
                                throw new BMMCompilerException("Not Found Variable: " + func);
                            }
                            ret += "/ " + v.Name;
                            ret += " load";
                            break;
                        case ExpressionMode.Number:
                            ret += "/ " + Convert.ToString(num, 16);
                            break;
                        case ExpressionMode.ExportedFunc:
                            ret += "/ \\" + func + " call";
                            break;
                        case ExpressionMode.Func:
                            ret += "/ :" + func + " call";
                            break;
                        case ExpressionMode.NativeFunc:
                            ret += "/ " + func;
                            break;
                    }
                    ret += "\n";
                    return ret;
                }
            }
            public class Substitution : Statement
            {
                public Expression content;
                public string variable;
                private enum SubMode
                {
                    Var,EqualBeforeSpace,Content
                }
                public Substitution(string src)
                {
                    string stack = "";
                    int i = 0;
                    variable = "";
                    SubMode mode = SubMode.Var;
                    while (i < src.Length)
                    {
                        switch (mode)
                        {
                            case SubMode.Var:
                                switch (src[i]) 
                                {
                                    case '\t':
                                    case '\r':
                                    case '\n':
                                    case ' ':
                                        mode = SubMode.EqualBeforeSpace;
                                        variable = stack;
                                        stack = "";
                                        break;
                                    default:
                                        stack += src[i];
                                        break;
                                }
                                break;
                            case SubMode.EqualBeforeSpace:
                                if (src[i] == '=') mode = SubMode.Content;
                                break;
                            case SubMode.Content:
                                stack += src[i];
                                break;
                        }
                        i++;
                    }
                    content = new(stack);
                }
                public override string Compile(List<Variable> variables, List<string> functions, List<string> exportedFunctions)
                {
                    string ret = "";
                    ret += content.Compile(variables, functions, exportedFunctions);
                    Variable? v = variables.Find(v => v.Name == variable);
                    if(v == null)
                    {
                        throw new BMMCompilerException("Not Found Variable: " + variable);
                    }
                    ret += "/ " + v.Rad16 + " store\n";
                    return ret;
                }
            }
        }
    }
}
