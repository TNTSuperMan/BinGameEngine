using System.Text.RegularExpressions;

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
            if (!File.Exists(path)) throw new BGEException("ファイル'" + path + "'が存在しません");
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
                            len += (ushort)bge.Last().length;
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
                if (Regex.IsMatch(text, "^[\\da-fA-F]{1,4}$"))
                {
                    ushort num = 0;
                    foreach (char t in text)
                    {
                        num = (ushort)(num << 4);
                        num += toInt(t);
                    }
                    ret.Add(new BGEData(num));
                }
                else if (Regex.IsMatch(text, "^:.+"))
                {
                    if (exportedTagName == null || exportedTagPoint == null)
                    {
                        ret.Add(new BGEData((ushort)0));
                    }
                    else
                    {
                        int i = jumpTagName.IndexOf(text.Substring(1));
                        if (i == -1)
                        {
                            throw new BGEException("ラベル'" + text.Substring(1) + "'が見つかりません", fpath, line);
                        }
                        else
                        {
                            ret.Add(new BGEData((jumpTagPoint[i])));
                        }
                    }
                }
                else if (Regex.IsMatch(text, "^\\\\.+"))
                {
                    if (exportedTagName == null || exportedTagPoint == null)
                    {
                        ret.Add(new BGEData((ushort)0));
                    }
                    else
                    {
                        int i = exportedTagName.IndexOf(text.Substring(1));
                        if (i == -1)
                        {
                            throw new BGEException("ラベル'" + text.Substring(1) + "'が見つかりません", fpath, line);
                        }
                        else
                        {
                            ret.Add(new BGEData(exportedTagPoint[i]));
                        }
                    }
                }
                else if (text != string.Empty)
                {
                    switch (text.ToLower())
                    {
                        case "push":    ret.Add(new(BGEOperator.push)); break;
                        case "pop":     ret.Add(new(BGEOperator.pop)); break;
                        case "cls":     ret.Add(new(BGEOperator.cls)); break;
                        case "add":     ret.Add(new(BGEOperator.add)); break;
                        case "sub":     ret.Add(new(BGEOperator.sub)); break;
                        case "mul":     ret.Add(new(BGEOperator.mul)); break;
                        case "div":     ret.Add(new(BGEOperator.div)); break;
                        case "rem":     ret.Add(new(BGEOperator.rem)); break;
                        case "nand":    ret.Add(new(BGEOperator.nand)); break;
                        case "truejump":ret.Add(new(BGEOperator.truejump)); break;
                        case "jump":    ret.Add(new(BGEOperator.jump)); break;
                        case "call":    ret.Add(new(BGEOperator.call)); break;
                        case "equal":   ret.Add(new(BGEOperator.equal)); break;
                        case "greater": ret.Add(new(BGEOperator.greater)); break;
                        case "load":    ret.Add(new(BGEOperator.load)); break;
                        case "store":   ret.Add(new(BGEOperator.store)); break;
                        case "ret":     ret.Add(new(BGEOperator.ret)); break;
                        case "redraw":  ret.Add(new(BGEOperator.redraw)); break;
                        case "rect":    ret.Add(new(BGEOperator.rect)); break;
                        case "chkkey":  ret.Add(new(BGEOperator.chkkey)); break;
                        case "sound":   ret.Add(new(BGEOperator.sound)); break;
                        default:
                            throw new BGEException(text.ToLower() + "という演算子はありません", fpath, line);
                    }
                }
                line++;
            }
            return ret;
        }
    }
    public enum BGEOperator : byte
    {
        push,
        pop,
        cls,
        add,
        sub,
        mul,
        div,
        rem,
        nand,
        truejump,
        jump,
        call,
        equal,
        greater,
        load,
        store,
        ret,
        redraw,
        rect,
        chkkey,
        sound,
        loadgraph,
        graph
    }

    public class BGEData
    {
        public BGEOperator _operator;
        public ushort? _pushdata;
        public uint length
        {
            get
            {
                return 1 + (uint)(_pushdata == null ? 0 : 2);
            }
        }
        public BGEData()
        {
            _operator = BGEOperator.push;
            _pushdata = null;
        }
        public BGEData(BGEOperator @operator)
        {
            _operator = @operator;
        }
        public BGEData(ushort pushdata)
        {
            _operator = BGEOperator.push;
            _pushdata = pushdata;
        }
        public byte[] bin
        {
            get
            {
                byte[] ret = new byte[_pushdata == null ? 1 : 3];

                ret[0] = (byte)_operator;
                if (_pushdata != null)
                {
                    ret[1] = (byte)((_pushdata & 0xFF00) >> 8);
                    ret[2] = (byte)(_pushdata & 0x00FF);
                }
                return ret;
            }
        }
    }
}
