using bgeruntime;
using System.Reflection;
namespace debugger
{
    public partial class Form1 : Form
    {
        ushort pc = 0;
        byte[] bin = [];
        List<string> programTexts = new List<string>();
        GraphRect[] graphicsStack = [];
        Runtime vm;

        bool debug = true;
        private void DoubleBuffer(Control c)
        {
            c.GetType().InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.SetProperty, null, c, [true]);
        }
        public Form1(string[] args)
        {
            InitializeComponent();
            DoubleBuffer(stackListBox);
            DoubleBuffer(callStackListBox);
            DoubleBuffer(memoryListBox);
            DoubleBuffer(programListBox);
            DoubleBuffer(panel1);
            bin = [];
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
                            case 0x16: t = "stopsound"; break;
                            case 0x17: t = "io"; break;
                            default: t = "Undefined"; break;
                        }
                    }
                    programTexts.Add(t);
                }
                programListBox.Items.Clear();
                foreach (string a in programTexts)
                    programListBox.Items.Add("> " + a);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("not found file: '" + args[0] + "'");
                Application.Exit();
            }
            vm = new(bin);
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
        private void Next()
        {
            if (pc >= bin.Length || programTexts.Count <= PC2Line())
            {
                End();
                return;
            }
            if (debug)
            {
                int start = Math.Max(PC2Line() - 2, 0);
                programListBox.Items.Clear();
                for (int i = 0; i < 23; i++)
                    if (programTexts.Count > (start + i))
                        programListBox.Items.Add((start + i == PC2Line() ? "@" : ">") + " " + programTexts[start + i]);
            }
            try
            {
                vm.EmulateNext();
            }
            catch (StackOutOfRangeException e)
            {
                End();
                stateText.Text = e.Message;
            }

            pcBox.Text = pc.ToString();
            pc = vm.debug.PC;
            stackListBox.Items.Clear();
            foreach (var stack in vm.debug.StackList)
                stackListBox.Items.Add(stack);
            memoryPos_ValueChanged(new object(), new EventArgs());
            return;
        }
        private void Start(object sender, EventArgs e)
        {
            vm = new(bin);
            vm.onEnd = End;
            vm.onRedraw = (GraphRect[] e) =>
            {
                graphicsStack = e;
                panel1.Invalidate();
            };
            startBtn.Enabled = false;
            debug = !fasterCheck.Checked;
            pc = 0;
            stackListBox.Items.Clear();
            callStackListBox.Items.Clear();
            memoryListBox.Items.Clear();
            stateText.Text = "Running";
            runningCheck.Enabled = true;
            runningCheck.Checked = true;
            fasterCheck.Enabled = false;
            if (debug) nextBtn.Enabled = true;
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
        private void Draw(object sender, PaintEventArgs e)
        {
            foreach (var d in graphicsStack)
                if (d.isDraw)
                    e.Graphics.FillRectangle(new SolidBrush(d.color), d.X, d.Y, d.Width, d.Height);
            graphicsStack = [];
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
                vm.EmulateFrame();
            }
        }
        private void NextBtnClicked(object sender, EventArgs e) => Next();

        private void fasterCheck_CheckedChanged(object sender, EventArgs e) => tickNum.ReadOnly = fasterCheck.Checked;

        private void memoryPos_ValueChanged(object sender, EventArgs e)
        {
            memoryListBox.Items.Clear();
            for (int i = (int)memoryPos.Value; i < (int)memoryPos.Value + 23 && i < ushort.MaxValue; i++)
                memoryListBox.Items.Add(vm.debug.Load((ushort)i));
        }
    }
}
