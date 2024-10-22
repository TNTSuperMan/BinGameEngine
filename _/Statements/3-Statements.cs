using System.Text.RegularExpressions;

namespace BMMCompiler.Parts
{
    /// <summary>
    /// 命令
    /// </summary>
    public abstract class Statement
    {
        public abstract string Compile(List<Variable> variables);
        public static Statement Tokenize(string stack)
        {
            stack = stack.Trim();
            if (Regex.IsMatch(stack, @"^if\s*\(.+\)\s*\{.*\}$"))
            {
                return new Expressions.If(stack);
            }
            else if (Regex.IsMatch(stack, @"^\w+\s*="))
            {
                return new Expressions.Substitution(stack);
            }
            else if (Regex.IsMatch(stack, @"^__bge__\(.+\)$"))
            {
                return new Expressions.Native(stack);
            }
            else if (Regex.IsMatch(stack, @"^\w+\(.*\)$"))
            {
                return new Expressions.Expression(stack);
            }
            /*
            else if (Regex.IsMatch(stack, @"while\s*\(.+\)\s*{.*}"))
            {

            }*/
            else
            {
                throw new Exception("Unknown statement: \"" + stack + "\"");
            }
        }
        public static List<Statement> fromString(string src, Action<string> act)
        {
            List<Statement> ret = [];
            string stack = "";
            int i = 0;
            int layer = 0;
            while (src.Length > i)
            {
                if (src[i] == ';' && layer == 0)
                {
                    if (Regex.IsMatch(stack.Trim(), "^var\\s+\\w+"))
                    {
                        int j = 4;
                        string trimed = stack.Trim();
                        string varstack = "";
                        while (j < trimed.Length)
                        {
                            if (!Regex.IsMatch(trimed[j].ToString(), "\\s"))
                            {
                                varstack += trimed[j];
                            }
                            j++;
                        }
                        act(varstack);
                    }
                    else
                    {
                        ret.Add(Tokenize(stack));
                    }
                    stack = "";
                }
                else
                {
                    stack += src[i];
                    if (src[i] == '{')
                    {
                        layer++;
                    }
                    else if (src[i] == '}')
                    {
                        layer--;
                    }
                }
                i++;
            }
            return ret;
        }
    }
}