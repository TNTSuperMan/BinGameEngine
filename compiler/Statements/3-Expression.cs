using System.Text.RegularExpressions;

namespace BMMCompiler.Parts.Expressions
{
    /// <summary>
    /// 演算子、値とか
    /// </summary>
    public class Expression : Statement
    {
        public List<Expression> pushes;
        public ExpressionMode mode;
        public Operator @operator;
        public string func;
        public ushort num;
        public enum ExpressionMode
        {
            Operation, Variable, Number, Function
        }
        public enum Operator
        {
            Pls, Sub, Mul, Div, Rem, Nand, Equal, Greater
        }
        private enum ExpFncCompileMode
        {
            Name, ArgBeforeSpace, Arg
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
            if (Regex.IsMatch(src, "^\\d{1,5}$")) //number
            {
                mode = ExpressionMode.Number;
                if (!ushort.TryParse(src, out num))
                {
                    Errors.Infos.Add(new("Number Max is 65535 but input is: " + src));
                }
            }
            else if (Regex.IsMatch(src, "^\\w+$")) // variable
            {
                mode = ExpressionMode.Variable;
                func = src;
            }
            else if (Regex.IsMatch(src, "^\\w+\\(.*\\)$")) //func
            {
                mode = ExpressionMode.Function;
                ExpFncCompileMode fncmode = ExpFncCompileMode.Name;
                int i = 0;
                string stack = "";
                int KakkoLayer = 0;
                while (i < src.Length)
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
                                    stack = "";
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
                            }
                            else if (src[i] == ')')
                            {
                                KakkoLayer--;
                                if (KakkoLayer < 0)
                                {
                                    if(stack.Length != 0)pushes.Add(new Expression(stack));
                                    return;
                                }
                                stack += src[i];
                            }
                            else if (src[i] == ',' && KakkoLayer == 0)
                            {
                                pushes.Add(new Expression(stack));
                                stack = "";
                            }
                            else if (!Regex.IsMatch(stack,@"\s")) stack += src[i];
                            break;
                    }
                    i++;
                }
            }
            else
            {

            }
        }
        public override string Compile(List<Variable> variables)
        {
            string ret = "";
            switch (mode)
            {
                case ExpressionMode.Operation:
                    if (pushes.Count != 2)
                    {
                        Errors.Infos.Add(new("Argument is Not 2"));
                        return "";
                    }
                    ret += pushes[0].Compile(variables);
                    ret += pushes[1].Compile(variables);
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
                        Errors.Infos.Add(new("Not Found Variable: " + func));
                        return "\n";
                    }
                    ret += "/ " + v.Rad16;
                    ret += " load";
                    break;
                case ExpressionMode.Number:
                    ret += "/ " + Convert.ToString(num, 16);
                    break;
                case ExpressionMode.Function:
                    switch (func)
                    {
                        #region NativeFuncs
                        case "redraw"://0
                        case "rand":  //0
                            if (pushes.Count != 0) Errors.Infos.Add(new("Not Equal Argument Length(0): " + pushes.Count.ToString()));
                            ret = "/ " + func;
                            break;
                        case "sin":   //1
                        case "sqrt":  //1
                        case "chkkey"://1
                            if (pushes.Count != 1) {
                                Errors.Infos.Add(new("Not Equal Argument Length(1): " + pushes.Count.ToString()));
                                break;
                            }
                            foreach (Expression p in pushes)
                            {
                                ret += p.Compile(variables);
                            }
                            ret += "/ " + func;
                            break;
                        case "sound": //2
                            if (pushes.Count != 2)
                            {
                                Errors.Infos.Add(new("Not Equal Argument Length(2): " + pushes.Count.ToString()));
                                break;
                            }
                            foreach (Expression p in pushes)
                            {
                                ret += p.Compile(variables);
                            }
                            ret += "/ " + func;
                            break;
                        case "pixel": //5
                            if (pushes.Count != 5)
                            {
                                Errors.Infos.Add(new("Not Equal Argument Length(5): " + pushes.Count.ToString()));
                                break;
                            }
                            foreach (Expression p in pushes)
                            {
                                ret += p.Compile(variables);
                            }
                            ret += "/ " + func;
                            break;
                        case "rect":  //7
                            if (pushes.Count != 7)
                            {
                                Errors.Infos.Add(new("Not Equal Argument Length(7): " + pushes.Count.ToString()));
                                break;
                            }
                            foreach (Expression p in pushes)
                            {
                                ret += p.Compile(variables);
                            }
                            ret += "/ " + func;
                            break;
                        case "return": //Any
                            foreach (Expression p in pushes)
                            {
                                ret += p.Compile(variables);
                            }
                            ret += "/ ret";
                            break;
                        #endregion
                        default:
                            ret = "/ \\" + func + " call";
                            break;
                    }
                    break;
            }
            ret += "\n";
            return ret;
        }
    }
}
