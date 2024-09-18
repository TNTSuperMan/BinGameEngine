namespace BMMCompiler.Parts.Expressions
{
    public class Substitution : Statement
    {
        public Expression content;
        public string variable;
        private enum SubMode
        {
            Var, EqualBeforeSpace, Content
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
        public override string Compile(List<Variable> variables)
        {
            string ret = "";
            ret += content.Compile(variables);
            Variable? v = variables.Find(v => v.Name == variable);
            if (v == null)
            {
                Errors.Infos.Add(new("Not Found Variable: " + variable));
                return "\n";
            }
            ret += "/ " + v.Rad16 + " store\n";
            return ret;
        }
    }
}