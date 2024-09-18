﻿using System.Text.RegularExpressions;

namespace BMMCompiler.Parts
{
    public class Function
    {
        public readonly bool IsVoid;
        public readonly string Name;
        public readonly List<string> Arguments;
        public readonly List<Variable> Variables;
        public readonly List<Statement> Statements;
        private enum FuncBuildMode
        {
            Type,
            NameBeforeSpace, Name,
            ArgumentBeforeSpace, Argument,
            CodeBeforeSpace, Code
        }
        public Function(string src)
        {
            Arguments = [];
            Variables = [];
            Statements = [];
            Name = "";
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
                                        IsVoid = true;
                                        break;
                                    case "func":
                                        IsVoid = false;
                                        break;
                                    default:
                                        Errors.Infos.Add(new("Unknown function define: " + stack));
                                        break;
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
                                Name = stack;
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
                                    Arguments.Add(stack);
                                    stack = "";
                                }
                                mode = FuncBuildMode.CodeBeforeSpace;
                                break;
                            case ',':
                                Arguments.Add(stack);
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
                            if (Regex.IsMatch(stack, "^var\\s+\\w+"))
                            {

                            }
                            else if (Regex.IsMatch(stack, "^\\w+\\s*=.+"))
                            {
                                Statements.Add(new Expressions.Substitution(stack));
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
        public string Compile(List<Variable> exportedVariables)
        {
            List<Variable> allVar = [];
            foreach (Variable v in Variables) allVar.Add(v);
            foreach (Variable v in exportedVariables) allVar.Add(v);
            string ret = "export " + Name + "\n";
            foreach (Statement s in Statements)
            {
                ret += s.Compile(allVar);
            }
            return ret;
        }
    }
}
