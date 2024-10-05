namespace BMMCompiler.Parts.Expressions
{
    internal class If : Statement
    {
        public readonly Expression? Condition;
        public readonly List<Statement> TrueThen;
        private enum Mode
        {
            BeforeCond, Condition,
            BeforeTrue, True
        }
        public If(string src)
        {
            string stack = "";
            int i = 2;
            int layer = 0;
            Mode mode = Mode.BeforeCond;
            while (src.Length > i)
            {
                switch (mode)
                {
                    case Mode.BeforeCond:
                        if (src[i] == '(')
                        {
                            mode = Mode.Condition;
                            layer = 0;
                        }
                        break;
                    case Mode.Condition:
                        if (src[i] == '(')
                        {
                            layer++;
                            stack += src[i];
                        }
                        else if (src[i] == ')')
                        {
                            layer--;
                            if(layer < 0)
                            {
                                Condition = new(stack);
                                stack = "";
                                mode = Mode.BeforeTrue;
                            }
                            else
                            {
                                stack += src[i];
                            }
                        }
                        else
                        {
                            stack += src[i];
                        }
                        break;
                    case Mode.BeforeTrue:
                        if (src[i] == '{')
                        {
                            mode = Mode.True;
                        }
                        break;
                    case Mode.True:
                        stack += src[i];
                        break;
                }
                i++;
            }
            TrueThen = Statement.fromString(
                stack.Substring(0, stack.Length - 1), v => v.ToString());
        }
        public override string Compile(List<Variable> variables)
        {
            if(Condition == null)
            {
                Errors.Infos.Add(new("Can't Tokenize Condition"));
                return "";
            }
            string Token = TagToken.Make();
            string ret = "";
            ret += Condition.Compile(variables);
            ret += "/ :" + Token + "_True truejump\n";
            ret += "/ :" + Token + "_End jump\n";
            ret += ":" + Token + "_True\n";
            foreach (Statement s in TrueThen) ret += s.Compile(variables);
            ret += ":" + Token + "_End\n";
            return ret;
        }
    }
}
