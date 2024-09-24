using System.Text.RegularExpressions;

namespace BMMCompiler.Parts
{
    public abstract class Statement
    {
        public abstract string Compile(List<Variable> variables);
        public static Statement fromString(string stack)
        {
            stack = stack.Trim();
            if (Regex.IsMatch(stack, @"^\w+\s*=.+"))
            {
                return new Expressions.Substitution(stack);
            }
            else if (Regex.IsMatch(stack, @"^__bge__\(.+\)$"))
            {
                return new Expressions.Native(stack);
            }
            else if (Regex.IsMatch(stack, @"^\w+\(.*\)"))
            {
                return new Expressions.Expression(stack);
            }/*
            else if (Regex.IsMatch(stack, @"if\s*\(.+\)\s*{.*}"))
            {

            }
            else if (Regex.IsMatch(stack, @"while\s*\(.+\)\s*{.*}"))
            {

            }*/
            else
            {
                throw new Exception("");
            }
        }
    }
}