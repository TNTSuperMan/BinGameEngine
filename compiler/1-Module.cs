// 製作段階：あとコンパイラ
namespace BMMCompiler.Parts
{
    public class Module
    {
        private enum CompileMode
        {
            None,
            Comment,
            Function,
            Include,
            Extern
        }
        public readonly List<string> Imports;
        public readonly List<Variable> ExportVariables;
        public readonly List<Function> Functions;
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
                                cblayer = 0;
                                mode = CompileMode.Include; break;
                            case "extern":
                                i++;
                                cblayer = 0;stack = "";
                                mode = CompileMode.Extern; break;
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
                    case CompileMode.Include:
                        if (cblayer == 0 && src[i] == '"')
                        {
                            cblayer = 1;
                            stack = "";
                        }
                        else if (cblayer == 1 && src[i] == '"')
                        {
                            Imports.Add(stack);
                            mode = CompileMode.None;
                            stack = "";
                        }
                        else if (cblayer == 1)
                        {
                            stack += src[i];
                        }
                        break;
                    case CompileMode.Extern:
                        switch (src[i])
                        {
                            case '\n':
                            case '\t':
                            case '\r':
                            case ' ':
                            case ';':
                                if(cblayer == 1)
                                {
                                    ExportVariables.Add(new(stack));
                                    stack = "";
                                    mode = CompileMode.None;
                                    break;
                                }
                                break;
                            default:
                                cblayer = 1;
                                stack += src[i];
                                break;
                        }
                        break;
                }
                i++;
            }
        }
        public string Compile(List<Variable> exportedVariables)
        {
            string ret = "";
            foreach (Function f in Functions)
            {
                ret += f.Compile(exportedVariables);
            }
            return ret;
        }
    }
}
