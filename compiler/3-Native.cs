using System.Text.RegularExpressions;

namespace BMMCompiler.Parts.Expressions
{
    public class Native : Statement
    {
        public readonly string text;
        public Native(string src)
        {
            text = src.Substring(8, src.Length-9);
        }
        public override string Compile(List<Variable> variables)
        {
            return text + "\n";
        }
    }
}
