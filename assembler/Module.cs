
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using static bgeruntime.Runtime;

namespace compiler
{
    public class BGEException : Exception
    {
        public BGEException(string message) : base(message) { }
        public BGEException(string message, string file, int line) : base(file + " " + line + "行目:" + message) { }
    }
    public class Module
    {
        private List<string> jumpTagName;
        private List<ushort> jumpTagPoint;
        public List<string> exportedTagName;
        public List<ushort> exportedTagPoint;

        public List<string> importPath;
        private string source;
        public string fpath;
        public ushort length;
        public Module(string path)
        {
            fpath = path;
            List<BGEData> bge = new();
            jumpTagName = new List<string>();
            jumpTagPoint = new List<ushort>();
            exportedTagName = new List<string>();
            exportedTagPoint = new List<ushort>();
            importPath = new List<string>();
            if (!File.Exists(path)) throw new BGEException("Not found file: '" + path + "'");
            source = File.ReadAllText(path);
            ushort len = 0;
            foreach (string l in source.Split('\n'))
            {
                string line = l.Trim();
                if (line.Length == 0) continue;
                switch (line[0])
                {
                    case '/':
                        foreach (BGEData d in compileLine(line.Substring(1)))
                        {
                            bge.Add(d);
                            len += (ushort)d.length;
                        }
                        break;
                    case ':':
                        jumpTagName.Add(line.Substring(1));
                        jumpTagPoint.Add(len);
                        break;
                    case 'e':
                        if (Regex.IsMatch(line, "^export"))
                        {
                            exportedTagName.Add(line.Substring(7));
                            exportedTagPoint.Add(len);
                        }
                        break;
                    case 'i':
                        if (Regex.IsMatch(line, "^import"))
                        {
                            importPath.Add(line.Substring(7));
                        }
                        else if(Regex.IsMatch(line, "^inject_fromB64\\s"))
                        {
                            len += (ushort)Convert.FromBase64String(line.Substring(15)).Length;
                        }
                        else if(Regex.IsMatch(line, "^inject\\s"))
                        {
                            len += (ushort)File.ReadAllBytes(line.Substring(7)).Length;
                        }
                        break;
                    case '!':
                        string name = line.Substring(1);
                        int idx = jumpTagName.FindIndex(e => e == name);
                        if (idx == -1)
                        {
                            Console.WriteLine("[Tag]Err: not found var: " + name);
                        }
                        else
                        {
                            Console.WriteLine("[Tag]'" + name + "' at " + jumpTagPoint[idx]);
                        }
                        break;
                }
            }
            length = len;
        }
        private ushort toInt(char data)
        {
            switch (data)
            {
                case '0': return 0x00;
                case '1': return 0x01;
                case '2': return 0x02;
                case '3': return 0x03;
                case '4': return 0x04;
                case '5': return 0x05;
                case '6': return 0x06;
                case '7': return 0x07;
                case '8': return 0x08;
                case '9': return 0x09;
                case 'a': case 'A': return 0x0a;
                case 'b': case 'B': return 0x0b;
                case 'c': case 'C': return 0x0c;
                case 'd': case 'D': return 0x0d;
                case 'e': case 'E': return 0x0e;
                case 'f': case 'F': return 0x0f;
                default: return 0;
            }
        }
        public void Shift(ushort num)
        {
            for (int i = 0; i < jumpTagPoint.Count; i++)
            {
                jumpTagPoint[i] += num;
            }
            for (int i = 0; i < exportedTagPoint.Count; i++)
            {
                exportedTagPoint[i] += num;
            }
        }
        public byte[] Compile(List<string>? exportedTagName = null, List<ushort>? exportedTagPoint = null)
        {
            List<BGEData> bge = new();
            foreach (string l in source.Split('\n'))
            {
                string line = l.Trim();
                if (line.Length == 0) continue;
                if (line[0] == '/') foreach (BGEData d in compileLine(line.Substring(1), exportedTagName, exportedTagPoint)) bge.Add(d);
                else if (Regex.IsMatch(line, "^inject_fromB64\\s"))
                    foreach (byte b in Convert.FromBase64String(line.Substring(15)))
                        bge.Add(new(b));
                else if (Regex.IsMatch(line, "^inject\\s"))
                    foreach (byte b in File.ReadAllBytes(line.Substring(7)))
                        bge.Add(new(b));
            }
            List<byte> data = new();
            foreach (BGEData b in bge) foreach (byte c in b.bin) data.Add(c);
            return data.ToArray();
        }
        private List<BGEData> compileLine(string source, List<string>? exportedTagName = null, List<ushort>? exportedTagPoint = null)
        {
            List<BGEData> ret = new();
            int line = 0;
            foreach (string text in source.Split(' '))
            {
                if (Regex.IsMatch(text, "^[\\da-fA-F]{1,2}$"))
                {
                    char num = (char)0;
                    foreach (char t in text)
                    {
                        num = (char)(num << 4);
                        num += (char)toInt(t);
                    }
                    ret.Add(new(num));
                }
                else if (Regex.IsMatch(text, "^:.+"))
                {
                    if (exportedTagName == null || exportedTagPoint == null)
                    {
                        ret.Add(new((char)0));
                        ret.Add(new((char)0));
                    }
                    else
                    {
                        int i = jumpTagName.IndexOf(text.Substring(1));
                        if (i == -1)
                        {
                            throw new BGEException("Not found label: '" + text.Substring(1) + "'", fpath, line);
                        }
                        else
                        {
                            ret.Add(new((char)(jumpTagPoint[i] >> 8)));
                            ret.Add(new((char)(jumpTagPoint[i] & 0x00ff)));
                        }
                    }
                }
                else if (Regex.IsMatch(text, @"^\\.+"))
                {
                    if (exportedTagName == null || exportedTagPoint == null)
                    {
                        ret.Add(new((char)0));
                        ret.Add(new((char)0));
                    }
                    else
                    {
                        int i = exportedTagName.IndexOf(text.Substring(1));
                        if (i == -1)
                        {
                            throw new BGEException("Not found label: '" + text.Substring(1) + "'", fpath, line);
                        }
                        else
                        {
                            ret.Add(new((char)(exportedTagPoint[i] >> 8)));
                            ret.Add(new((char)(exportedTagPoint[i] & 0x00ff)));
                        }
                    }
                }
                else if (text != string.Empty)
                {
                    switch (text.ToLower())
                    {
                        case "push":    ret.Add(new(Command.push)); break;
                        case "pop":     ret.Add(new(Command.pop)); break;
                        case "cls":     ret.Add(new(Command.cls)); break;
                        case "add":     ret.Add(new(Command.add)); break;
                        case "sub":     ret.Add(new(Command.sub)); break;
                        case "mul":     ret.Add(new(Command.mul)); break;
                        case "div":     ret.Add(new(Command.div)); break;
                        case "rem":     ret.Add(new(Command.rem)); break;
                        case "nand":    ret.Add(new(Command.nand)); break;
                        case "equal":   ret.Add(new(Command.equal)); break;
                        case "greater": ret.Add(new(Command.greater)); break;
                        case "truejump":ret.Add(new(Command.truejump)); break;
                        case "jump":    ret.Add(new(Command.jump)); break;
                        case "call":    ret.Add(new(Command.call)); break;
                        case "ret":     ret.Add(new(Command.ret)); break;
                        case "load":    ret.Add(new(Command.load)); break;
                        case "store":   ret.Add(new(Command.store)); break;
                        case "dumpkey": ret.Add(new(Command.dumpkey)); break;
                        case "redraw":  ret.Add(new(Command.redraw)); break;
                        case "rect":    ret.Add(new(Command.rect)); break;
                        case "graph":   ret.Add(new(Command.graph)); break;
                        case "sound":   ret.Add(new(Command.sound)); break;
                        case "io":      ret.Add(new(Command.io)); break;
                        case "break":   ret.Add(new(Command.breakpoint)); break;
                        default:
                            throw new BGEException("Not found operator: " + text.ToLower(), fpath, line);
                    }
                }
                line++;
            }
            return ret;
        }
    }
    public class BGEData
    {
        public readonly Command _operator;
        public readonly char? _pushdata;
        public readonly bool _isBinary = false;
        public uint length
        {
            get
            {
                return (uint)((!_isBinary && _operator == Command.push) ? 2 : 1);
            }
        }
        public BGEData(Command @operator)
        {
            _operator = @operator;
        }
        public BGEData(char pushdata)
        {
            _operator = Command.push;
            _pushdata = pushdata;
        }
        public BGEData(byte data)
        {
            _isBinary = true;
            _operator = (Command)data;
        }
        public byte[] bin
        {
            get
            {
                byte[] ret = new byte[length];

                ret[0] = (byte)_operator;
                if (!_isBinary && _pushdata != null)
                {
                    ret[1] = (byte)_pushdata;
                }
                return ret;
            }
        }
    }
}
