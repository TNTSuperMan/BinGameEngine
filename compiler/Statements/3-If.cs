using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMMCompiler.Parts.Expressions
{
    internal class If : Statement
    {
        public readonly Expression Condition;
        public readonly List<Statement> TrueThen;
        public readonly List<Statement> FalseThen;
        public If(string src)
        {
            throw new NotImplementedException();
        }
        public override string Compile(List<Variable> variables)
        {
            string Token = TagToken.Make();
            string ret = "";
            ret += Condition.Compile(variables);
            ret += "/ :" + Token + "_TrueJump truejump\n";
            foreach (Statement s in FalseThen) ret += s.Compile(variables);
            ret += "/ :" + Token + "_End jump\n";
            ret += ":" + Token + "_TrueJump\n";
            foreach (Statement s in TrueThen) ret += s.Compile(variables);
            ret += ":" + Token + "_End\n";
            return ret;
        }
    }
}
