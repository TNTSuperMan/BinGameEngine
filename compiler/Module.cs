using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace compiler
{
    public class BGEException : Exception
    {
        public BGEException(string message) : base(message){ }
        public BGEException(string message, string file, int line) : base(file + " " + line + "行目:" + message) { }
    }
    public class Module
    {
        private List<string> jumpTagName;
        private List<uint> jumpTagPoint;
        public List<string> exportedTagName;
        public List<uint> exportedTagPoint;

        public List<string> importPath;
        private string source;
        public string fpath;
        public uint length;
        public Module(string path)
        {
            fpath = path;
            List<BGEData> bge = new List<BGEData>();
            jumpTagName = new List<string>();
            jumpTagPoint = new List<uint>();
            exportedTagName = new List<string>();
            exportedTagPoint = new List<uint>();
            importPath = new List<string>();
            if (!File.Exists(path)) throw new BGEException("ファイル'" + path + "'が存在しません");
            source = File.ReadAllText(path);
            uint len = 0;
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
                            len += bge.Last().length;
                        }
                        break;
                    case ':':
                        jumpTagName.Add(line.Substring(1));
                        jumpTagPoint.Add(len);
                        break;
                    case 'e':
                        if(line.Length > 7)
                        {
                            exportedTagName.Add(line.Substring(7));
                            exportedTagPoint.Add(len);
                        }
                        break;
                    case 'i':
                        if (line.Length > 7)
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
                case '0': return 0x0;
                case '1': return 0x1;
                case '2': return 0x2;
                case '3': return 0x3;
                case '4': return 0x4;
                case '5': return 0x5;
                case '6': return 0x6;
                case '7': return 0x7;
                case '8': return 0x8;
                case '9': return 0x9;
                case 'a': case 'A': return 0xa;
                case 'b': case 'B': return 0xb;
                case 'c': case 'C': return 0xc;
                case 'd': case 'D': return 0xd;
                case 'e': case 'E': return 0xe;
                case 'f': case 'F': return 0xf;
                default: return 0;
            }
        }
        public void Shift(uint num)
        {
            for(int i = 0;i < jumpTagPoint.Count; i++)
            {
                jumpTagPoint[i] += num;
            }
            for (int i = 0; i < exportedTagPoint.Count; i++)
            {
                exportedTagPoint[i] += num;
            }
        }
        public byte[] Compile(List<string>? exportedTagName = null, List<uint>? exportedTagPoint = null)
        {
            List<BGEData> bge = new List<BGEData>();
            foreach (string l in source.Split('\n'))
            {
                string line = l.Trim();
                if (line.Length == 0) continue;
                if (line[0] == '/') foreach (BGEData d in compileLine(line.Substring(1), exportedTagName, exportedTagPoint)) bge.Add(d);
            }
            List<byte> data = new List<byte>();
            foreach(BGEData b in bge) foreach (byte c in b.bin) data.Add(c);
            return data.ToArray();
        }
        private List<BGEData> compileLine(string source, List<string>? exportedTagName = null, List<uint>? exportedTagPoint = null)
        {
            List<BGEData> ret = new List<BGEData>();
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
                        ret.Add(new BGEData((ushort)0));
                    }
                    else
                    {
                        int i = jumpTagName.IndexOf(text.Substring(1));
                        if (i == -1)
                        {
                            throw new BGEException("ラベル'"+text.Substring(1)+"'が見つかりません",fpath,line);
                        }
                        else
                        {
                            ret.Add(new BGEData((ushort)((jumpTagPoint[i] & 0xFFFF0000) >> 16)));
                            ret.Add(new BGEData((ushort)(jumpTagPoint[i] & 0x0000FFFF)));
                        }
                    }
                }
                else if(Regex.IsMatch(text, "^\\\\.+"))
                {
                    if (exportedTagName == null || exportedTagPoint == null)
                    {
                        ret.Add(new BGEData((ushort)0));
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
                            ret.Add(new BGEData((ushort)((exportedTagPoint[i] & 0xFFFF0000) >> 16)));
                            ret.Add(new BGEData((ushort)(exportedTagPoint[i] & 0x0000FFFF)));
                        }
                    }
                }
                else if (text != string.Empty)
                {
                    switch (text.ToLower())
                    {
                        case "push": ret.Add(new BGEData(BGEOperator.push)); break;
                        case "pop": ret.Add(new BGEData(BGEOperator.pop)); break;
                        case "cls": ret.Add(new BGEData(BGEOperator.cls)); break;
                        case "pls": ret.Add(new BGEData(BGEOperator.pls)); break;
                        case "sub": ret.Add(new BGEData(BGEOperator.sub)); break;
                        case "mul": ret.Add(new BGEData(BGEOperator.mul)); break;
                        case "div": ret.Add(new BGEData(BGEOperator.div)); break;
                        case "rem": ret.Add(new BGEData(BGEOperator.rem)); break;
                        case "nand": ret.Add(new BGEData(BGEOperator.nand)); break;
                        case "sin": ret.Add(new BGEData(BGEOperator.sin)); break;
                        case "sqrt": ret.Add(new BGEData(BGEOperator.sqrt)); break;
                        case "truejump": ret.Add(new BGEData(BGEOperator.truejump)); break;
                        case "jump": ret.Add(new BGEData(BGEOperator.jump)); break;
                        case "call": ret.Add(new BGEData(BGEOperator.call)); break;
                        case "equal": ret.Add(new BGEData(BGEOperator.equal)); break;
                        case "greater": ret.Add(new BGEData(BGEOperator.greater)); break;
                        case "load": ret.Add(new BGEData(BGEOperator.load)); break;
                        case "store": ret.Add(new BGEData(BGEOperator.store)); break;
                        case "ret": ret.Add(new BGEData(BGEOperator.ret)); break;
                        case "redraw": ret.Add(new BGEData(BGEOperator.redraw)); break;
                        case "pixel": ret.Add(new BGEData(BGEOperator.pixel)); break;
                        case "rect": ret.Add(new BGEData(BGEOperator.rect)); break;
                        case "chkkey": ret.Add(new BGEData(BGEOperator.chkkey)); break;
                        default:
                            throw new BGEException(text.ToLower() + "という演算子はありません",fpath,line);
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
        pls,
        sub,
        mul,
        div,
        rem,
        nand,
        sin,
        sqrt,
        truejump,
        jump,
        call,
        equal,
        greater,
        load,
        store,
        ret,
        redraw,
        pixel,
        rect,
        chkkey
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
                    ret[1] = (byte)((_pushdata & (ushort)0xFF00) >> 8);
                    ret[2] = (byte)(_pushdata & (ushort)0x00FF);
                }
                return ret;
            }
        }
    }
}
