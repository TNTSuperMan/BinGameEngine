using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
/**
 * 1.変数マップ
 */
namespace compiler
{
    internal class Variable
    {
        private string _name;
        private ushort _addr;
        public Variable(string name)
        {
            _name = name;
            _addr = 0;
        }
        public void Shift(ushort v)
        {
            _addr += v;
        }
    }
    internal class Module
    {
        public List<string> Imports;
        public List<Variable> ExportVariables;
        public Module(string src)
        {

        }
        private string Minify(string src)
        {
            string result = string.Empty;
            try
            {
                for (int i = 0; i < src.Length; i++)
                {
                    switch (src[i])
                    {
                        case '\r':
                        case '\n':
                        case ' ':
                            break;
                        default:
                            if (src[i] == '/' && src.Length > (i + 1))
                            {
                                if (src[++i] == '/')
                                {
                                    while (src[++i] == '\n') ;
                                }
                                else if (src[++i] == '*')
                                {
                                    while (src[++i] == '*')
                                    {
                                        if (src[i + 1] == '/')
                                        {
                                            i++;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                result += src[i];
                            }
                            break;
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {

            }
            return result;
        }
    }
    internal class Function
    {
        public bool isVoid;
        public string Name;
        public List<string> Arguments;
    }
}
