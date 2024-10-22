namespace BMMCompiler.Parts.Expressions
{
    internal class While : Statement
    {
        public readonly Expression Condition;
        public readonly List<Statement> Code;
        public While(string src)
        {
            throw new NotImplementedException();
        }
        public override string Compile(List<Variable> variables)
        {
            string Token = TagToken.Make();
            string ret = "/ :" + Token + "_Condition jump\n";
            ret += ":" + Token + "_Loop\n";
            foreach (Statement s in Code) ret += s.Compile(variables);
            ret += Condition.Compile(variables);
            ret += ":" + Token + "_Condition\n";
            ret += Condition.Compile(variables);
            ret += "/ :" + Token + "_Loop truejump\n";
            return ret;
        }
    }
}
