﻿using System.Runtime.ConstrainedExecution;
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
                        break;
                    case '!':
                        string name = line.Substring(1);
                        int idx = jumpTagName.FindIndex(e=>e==name);
                        if(idx == -1){
                            Console.WriteLine("[Tag]Err: not found var: "+name);
                        }else{
                            Console.WriteLine("[Tag]'"+name+"' at "+jumpTagPoint[idx]);
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
                            ret.Add(new((char)(jumpTagPoint[i] >> 8)));
                            ret.Add(new((char)(jumpTagPoint[i] & 0x00ff)));
                        }
                    }
                }
                else if (Regex.IsMatch(text, "^\\\\.+"))
                {
                    if (exportedTagName == null || exportedTagPoint == null)
                    {
                        ret.Add(new((char)0));
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
                            ret.Add(new((char)(exportedTagPoint[i] >> 8)));
                            ret.Add(new((char)(exportedTagPoint[i] & 0x00ff)));
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
                        case "equal":   ret.Add(new(BGEOperator.equal)); break;
                        case "greater": ret.Add(new(BGEOperator.greater)); break;
                        case "truejump":ret.Add(new(BGEOperator.truejump)); break;
                        case "jump":    ret.Add(new(BGEOperator.jump)); break;
                        case "call":    ret.Add(new(BGEOperator.call)); break;
                        case "ret":     ret.Add(new(BGEOperator.ret)); break;
                        case "load":    ret.Add(new(BGEOperator.load)); break;
                        case "store":   ret.Add(new(BGEOperator.store)); break;
                        case "dumpkey":  ret.Add(new(BGEOperator.dumpkey)); break;
                        case "redraw":  ret.Add(new(BGEOperator.redraw)); break;
                        case "rect":    ret.Add(new(BGEOperator.rect)); break;
                        case "graph":   ret.Add(new(BGEOperator.graph)); break;
                        case "sound":   ret.Add(new(BGEOperator.sound)); break;
                        case "io":      ret.Add(new(BGEOperator.io)); break;
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
        equal,
        greater,
        truejump,
        jump,
        call,
        ret,
        load,
        store,
        dumpkey,
        redraw,
        rect,
        graph,
        sound,
        io
    }

    public class BGEData
    {
        public BGEOperator _operator;
        public char? _pushdata;
        public uint length
        {
            get
            {
                return (uint)(_operator == BGEOperator.push ? 2 : 1);
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
        public BGEData(char pushdata)
        {
            _operator = BGEOperator.push;
            _pushdata = pushdata;
        }
        public byte[] bin
        {
            get
            {
                byte[] ret = new byte[_operator == BGEOperator.push ? 2 : 1];

                ret[0] = (byte)_operator;
                if (_pushdata != null)
                {
                    ret[1] = (byte)_pushdata;
                }
                return ret;
            }
        }
    }
}
