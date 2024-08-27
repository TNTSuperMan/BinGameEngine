using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        if(args.Length != 2)
        {
            Console.WriteLine("Usage: compiler.exe [Source] [Output]");
        }
        else
        {
            File.WriteAllBytes(args[1], Compile(File.ReadAllText(args[0])));
        }
    }
    private static byte[] Compile(string rawsrc)
    {
        string s = "";
        foreach (string a in rawsrc.Split('\r'))
        {
            s += a;
        }
        string[] source = s.Split("\n");
        List<string> gotoTagName = new List<string>();
        List<uint> gotoJmpAddr = new List<uint>();
        uint idx = 0;
        Console.WriteLine("Loading jump tag...");
        foreach (string l in source) //Goto Tag Find
        {
            string line = l.Trim();
            if (line.Length == 0) continue;
            switch (line[0])
            {
                case ':': //GotoTag
                    gotoTagName.Add(line.Substring(1));
                    gotoJmpAddr.Add(idx);
                    break;
                case '/': //Operate
                    foreach (var ld in CompileLine(line.Substring(1).Split(' ')))
                    {
                        idx += ld.length;
                    }
                    break;
            }
        }
        List<BGEData> alldata = new List<BGEData>();
        Console.WriteLine("Compiling...");
        foreach (string l in source) //Goto Tag Find
        {
            string line = l.Trim();
            if (line.Length == 0) continue;
            if (line[0] == '/')
            {
                foreach (var ld in CompileLine(line.Substring(1).Split(' '), gotoTagName, gotoJmpAddr))
                {
                    alldata.Add(ld);
                }
            }
        }
        List<byte> listBytes = new List<byte>();
        foreach (BGEData data in alldata)
        {
            foreach(byte b in data.bin)
            {
                listBytes.Add(b);
            }
        }
        return listBytes.ToArray();
    }
    public enum BGEOperator:byte
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
    public static List<BGEData> CompileLine(string[] texts, List<string>? gotoTagName = null, List<uint>? gotoJmpAddr = null)
    {
        List<BGEData> ret = new List<BGEData>();
        foreach (string text in texts)
        {
            if (Regex.IsMatch(text, "^[\\da-fA-F]{1,4}$"))
            {
                ushort num = 0;
                foreach(char t in text)
                {
                    num = (ushort)(num << 4);
                    num += (ushort)(toInt(t));
                }
                ret.Add(new BGEData(num));
            }
            else if (Regex.IsMatch(text, "^:.+"))
            {
                if (gotoTagName == null || gotoJmpAddr == null)
                {
                    ret.Add(new BGEData((ushort)0));
                    ret.Add(new BGEData((ushort)0));
                }
                else
                {
                    int i = gotoTagName.IndexOf(text.Substring(1));
                    if (i == -1)
                    {
                        Console.WriteLine("Error can't find jump label");
                    }
                    else
                    {
                        ret.Add(new BGEData((ushort)((gotoJmpAddr[i] & 0xFFFF0000) >> 16)));
                        ret.Add(new BGEData((ushort)(gotoJmpAddr[i] & 0x0000FFFF)));
                    }
                }
            }
            else if (text == string.Empty)
            {

            }
            else
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
                        Console.WriteLine("Error unknown operator: " + text.ToLower());
                        break;
                }
            }
        }
        return ret;
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
                if(_pushdata != null)
                {
                    ret[1] = (byte)((_pushdata & (ushort)0xFF00) >> 8);
                    ret[2] = (byte)(_pushdata & (ushort)0x00FF);
                }
                return ret;
            }
        }
    }
    public static ushort toInt(char data)
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
            default:
                Console.WriteLine("Error it's not num");
                return 0x0;
        }
    }
}