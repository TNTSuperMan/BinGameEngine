using System.Reflection;
namespace runtime
{
    public partial class Form1 : Form
    {
        ushort pc = 0;
        ushort pcbefore = 0;
        byte[] bin = [];
        List<byte> stack = new List<byte>();
        List<ushort> callstack = new List<ushort>();
        byte[] memory = new byte[0x6000];
        List<string> programTexts = new List<string>();
        List<BGEGraphic> graphicsStack = new List<BGEGraphic>();
        bool[] keymap = new bool[0x2b];
        bool debug = true;
        private void DoubleBuffer(Control c)
        {
            c.GetType().InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.SetProperty, null, c, new object[] { true });
        }
        public Form1(string[] args)
        {
            InitializeComponent();
            DoubleBuffer(stackListBox);
            DoubleBuffer(callStackListBox);
            DoubleBuffer(memoryListBox);
            DoubleBuffer(programListBox);
            DoubleBuffer(panel1);
            try
            {
                stateText.Text = "Loading";
                if (args.Length < 1)
                {
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        bin = File.ReadAllBytes(openFileDialog1.FileName);
                    }
                    else
                    {
                        MessageBox.Show("please select file");
                        Application.Exit();
                    }
                }
                else
                {
                    bin = File.ReadAllBytes(args[0]);
                }
                for (int i = 0; i < bin.Length; i++)
                {
                    string t = string.Empty;
                    if (bin[i] == 0)
                    {
                        t = "push ";
                        t += bin[++i].ToString("x2");
                    }
                    else
                    {
                        switch (bin[i])
                        {
                            case 0x01: t = "pop"; break;
                            case 0x02: t = "cls"; break;
                            case 0x03: t = "add"; break;
                            case 0x04: t = "sub"; break;
                            case 0x05: t = "mul"; break;
                            case 0x06: t = "div"; break;
                            case 0x07: t = "rem"; break;
                            case 0x08: t = "nand"; break;
                            case 0x09: t = "equal"; break;
                            case 0x0a: t = "greater"; break;
                            case 0x0b: t = "truejump"; break;
                            case 0x0c: t = "jump"; break;
                            case 0x0d: t = "call"; break;
                            case 0x0e: t = "ret"; break;
                            case 0x0f: t = "load"; break;
                            case 0x10: t = "store"; break;
                            case 0x11: t = "dumpkey"; break;
                            case 0x12: t = "redraw"; break;
                            case 0x13: t = "rect"; break;
                            case 0x14: t = "graph"; break;
                            case 0x15: t = "sound"; break;
                            case 0x16: t = "io"; break;
                            default: t = "Undefined"; break;
                        }
                    }
                    programTexts.Add(t);
                }
                programListBox.Items.Clear();
                foreach (string a in programTexts)
                {
                    programListBox.Items.Add("< " + a);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("not found file: '" + args[0] + "'");
                Application.Exit();
            }
        }
        private int PC2Line()
        {
            int res = 0;
            for (uint i = 0; i < pc; i++)
            {
                res++;
                if (bin[i] == 0) i += 1;
            }
            return res;
        }
        private int Keycode2ID(Keys k)
        {
            switch (k)
            {
                case Keys.D0: return 0;
                case Keys.D1: return 1;
                case Keys.D2: return 2;
                case Keys.D3: return 3;
                case Keys.D4: return 4;
                case Keys.D5: return 5;
                case Keys.D6: return 6;
                case Keys.D7: return 7;
                case Keys.D8: return 8;
                case Keys.D9: return 9;
                case Keys.Space: return 10;
                case Keys.A: return 11;
                case Keys.B: return 12;
                case Keys.C: return 13;
                case Keys.D: return 14;
                case Keys.E: return 15;
                case Keys.F: return 16;
                case Keys.G: return 17;
                case Keys.H: return 18;
                case Keys.I: return 19;
                case Keys.J: return 20;
                case Keys.K: return 21;
                case Keys.L: return 22;
                case Keys.M: return 23;
                case Keys.N: return 24;
                case Keys.O: return 25;
                case Keys.P: return 26;
                case Keys.Q: return 27;
                case Keys.R: return 28;
                case Keys.S: return 29;
                case Keys.T: return 30;
                case Keys.U: return 31;
                case Keys.V: return 32;
                case Keys.W: return 33;
                case Keys.X: return 34;
                case Keys.Y: return 35;
                case Keys.Z: return 36;
                case Keys.Left: return 37;
                case Keys.Right: return 38;
                case Keys.Up: return 39;
                case Keys.Down: return 40;
                case Keys.Enter: return 41;
                case Keys.Back: return 42;
                default: return int.MaxValue;
            }
        }
        private byte Pop()
        {
            if (stack.Count == 0)
            {
                End();
                stateText.Text = "Stack underflow";
                return 0;
            }
            byte d = stack.Last();
            stack.RemoveAt(stack.Count - 1);
            if (debug) stackListBox.Items.RemoveAt(stackListBox.Items.Count - 1);
            return d;
        }
        private void Push(byte d)
        {
            stack.Add(d);
            if (debug) stackListBox.Items.Add((int)d);
        }
        private void Push(int d)
        {
            stack.Add((byte)d);
            if (debug) stackListBox.Items.Add(d);
        }
        private ushort PopAddr()
        {
            byte bottom = Pop();
            byte upside = Pop();
            return (ushort)((upside << 8) | bottom);
        }
        private byte Load(ushort addr)
        {
            if(addr < 0xa000)
            {
                return bin[addr];
            }
            else
            {
                return memory[addr - 0xa000];
            }
        }
        private void Store(ushort addr, byte value)
        {
            if(addr >= 0xa000)
            {
                memory[addr - 0xa000] = value;
            }
        }
        private bool Next()
        {
            if (pc >= bin.Length || programTexts.Count <= PC2Line())
            {
                End();
                return false;
            }
            if (debug)
            {
                if (pc > 0) programListBox.Items[pcbefore] = "< " + programTexts[pcbefore];
                programListBox.Items[PC2Line()] = "@ " + programTexts[PC2Line()];
                programListBox.TopIndex = PC2Line() - 2;
                pcbefore = (ushort)PC2Line();
            }
            byte m1;
            ushort ptr;
            byte x, y, w, h, r, g, b;
            switch (Load(pc))
            {
                case 0x00: //push
                    Push(bin[++pc]);
                    break;
                case 0x01: //pop
                    stack.RemoveAt(stack.Count - 1);
                    break;
                case 0x02: //cls
                    stack.Clear();
                    break;
                case 0x03: //add
                    Push(Pop() + Pop());
                    break;
                case 0x04: //sub
                    m1 = Pop();
                    Push(Pop() - m1);
                    break;
                case 0x05: //mul
                    Push(Pop() * Pop());
                    break;
                case 0x06: //div
                    m1 = Pop();
                    Push(Pop() / m1);
                    break;
                case 0x07: //rem
                    m1 = Pop();
                    Push(Pop() % m1);
                    break;
                case 0x08: //nand
                    Push(~(Pop() & Pop()));
                    break;
                case 0x09: //equal
                    Push((Pop() == Pop()) ? 1 : 0);
                    break;
                case 0x0a: //greater
                    Push(Pop() < Pop() ? 1 : 0);
                    break;
                case 0x0b: //truejump
                    ptr = (ushort)(PopAddr()-1);
                    if (Pop() != 0) pc = ptr;
                    break;
                case 0x0c: //jump
                    pc = (ushort)(PopAddr()-1);
                    break;
                case 0x0d: //call
                    callstack.Add(pc);
                    pc = (ushort)(PopAddr()-1);
                    if (debug) callStackListBox.Items.Add(pc);
                    break;
                case 0x0e: //ret
                    if (callstack.Count == 0)
                    {
                        End();
                        return false;
                    }
                    else
                    {
                        ptr = callstack.Last();
                        callstack.RemoveAt(callstack.Count - 1);
                        pc = ptr;
                        if (debug) callStackListBox.Items.RemoveAt(callStackListBox.Items.Count - 1);
                    }
                    break;
                case 0x0f: //load
                    Push(Load(PopAddr()));
                    break;
                case 0x10: //store
                    ptr = PopAddr();
                    Store(ptr, Pop());
                    if (debug)
                    {
                        for (int i = 0; i <= (ptr+1 - memoryListBox.Items.Count); i++) memoryListBox.Items.Add(0);
                        memoryListBox.Items[ptr] = (int)memory[ptr];
                    }
                    break;
                case 0x11: //dumpkey
                    Push(keymap[Pop()] ? 1 : 0);
                    break;
                case 0x12: //redraw
                    panel1.Invalidate();
                    pc++;
                    return false;
                case 0x13: //rect
                    b = Pop();
                    g = Pop();
                    r = Pop();
                    h = Pop();
                    w = Pop();
                    y = Pop();
                    x = Pop();
                    graphicsStack.Add(new BGEGraphic(x, y, w, h, r, g, b));
                    break;
            }
            pc++;
            return true;
        }
        private void Start(object sender, EventArgs e)
        {
            startBtn.Enabled = false;
            debug = !fasterCheck.Checked;
            pc = 0;
            stack.Clear();
            callstack.Clear();
            stackListBox.Items.Clear();
            callStackListBox.Items.Clear();
            memoryListBox.Items.Clear();
            stateText.Text = "Running";
            runningCheck.Enabled = true;
            runningCheck.Checked = true;
            fasterCheck.Enabled = false;
            if (debug) nextBtn.Enabled = true;
            for (int i = 0; i < keymap.Length; i++) keymap[i] = false;
            for (int i = 0; i < memory.Length; i++) memory[i] = 0;
            clock.Start();
        }
        private void End()
        {
            clock.Stop();
            stateText.Text = "Ended";
            runningCheck.Enabled = false;
            nextBtn.Enabled = false;
            startBtn.Enabled = true;
            fasterCheck.Enabled = true;
        }
        private void TickChanged(object sender, EventArgs e)
        {
            clock.Interval = (int)tickNum.Value;
        }
        private void RunningChanged(object sender, EventArgs e)
        {
            if (runningCheck.Checked)
            {
                clock.Start();
                stateText.Text = "Running";
            }
            else
            {
                clock.Stop();
                stateText.Text = "Stopping";
            }
        }
        private void KeyDownEvent(object sender, KeyEventArgs e)
        {
            if (Keycode2ID(e.KeyCode) != int.MaxValue) keymap[Keycode2ID(e.KeyCode)] = true;
        }
        private void KeyUpEvent(object sender, KeyEventArgs e)
        {
            if (Keycode2ID(e.KeyCode) != int.MaxValue) keymap[Keycode2ID(e.KeyCode)] = false;
        }
        private void Draw(object sender, PaintEventArgs e)
        {
            foreach (var d in graphicsStack)
            {
                d.Draw(e.Graphics);
            }
            graphicsStack.Clear();
            return;
        }
        private void Tick(object sender, EventArgs e)
        {
            if (debug)
            {
                Next();
            }
            else
            {
                while (Next()) ;
            }
        }
        private void NextBtnClicked(object sender, EventArgs e) => Next();
        private void memoryEditor_ValueChanged(object sender, EventArgs e)
        {
            memoryListBox.SelectedIndex = (int)memoryEditor.Value;
        }
    }
    public class BGEGraphic
    {
        public ushort width;
        public ushort height;
        public ushort x;
        public ushort y;
        public ushort r;
        public ushort g;
        public ushort b;
        public BGEGraphic(ushort x, ushort y, ushort r, ushort g, ushort b)
        {
            this.x = x;
            this.y = y;
            this.r = r;
            this.g = g;
            this.b = b;
            width = 1;
            height = 1;
        }
        public BGEGraphic(ushort x, ushort y, ushort w, ushort h, ushort r, ushort g, ushort b)
        {
            this.x = x;
            this.y = y;
            this.r = r;
            this.g = g;
            this.b = b;
            width = w;
            height = h;
        }
        public void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb(r, this.g, b)), x, y, width, height);
        }
    }
}
