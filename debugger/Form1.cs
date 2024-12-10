using bgeruntime;
using System.Media;
using System.Reflection;
namespace debugger
{
    public partial class Form1 : Form
    {
        ushort pc = 0;
        byte[] bin = [];
        List<string> programTexts = new List<string>();
        GraphRect[] graphicsStack = [];
        List<SoundPlayer> Sounds = new();
        Runtime vm;

        bool debug => !fasterCheck.Checked;
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
                    if (bin[i] == 1)
                    {
                        t = "push ";
                        t += bin[++i].ToString("x2");
                    }
                    else
                    {
                        t = ((Runtime.Command)bin[i]).ToString();
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
                if (bin[i] == 1) i += 1;
            }
            return res;
        }
        private void updateDumps()
        {
            if (programTexts.Count <= PC2Line())
            {
                programListBox.Items.Clear();
                programListBox.Items.Add("---far pc---");
            }
            else
            {
                int start = Math.Max(PC2Line() - 2, 0);
                programListBox.Items.Clear();
                for (int i = 0; i < 23; i++)
                    if (programTexts.Count > (start + i))
                        programListBox.Items.Add((start + i == PC2Line() ? "@" : ">") + " " + programTexts[start + i]);
            }
            pcBox.Text = pc.ToString();
            pc = vm.debug.PC;
            stackListBox.Items.Clear();
            foreach (var stack in vm.debug.StackList)
                stackListBox.Items.Add(stack);
            callStackListBox.Items.Clear();
            foreach (var callstack in vm.debug.CallStackList)
                callStackListBox.Items.Add(callstack);
            memoryPos_ValueChanged(new object(), new EventArgs());
        }
        private void Next()
        {
            try
            {
                vm.EmulateNext();
            }
            catch (InvalidOperationException e)
            {
                End();
                stateText.Text = e.Message;
            }
            updateDumps();
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
            vm.onSound = (Sound sound) =>
            {
                Sounds.Add(new(sound.Stream()));
                if (sound.isLoop) Sounds.Last().PlayLooping();
                else Sounds.Last().Play();
            };
            vm.onStopSound = () =>
            {
                foreach (var sound in Sounds) sound.Stop();
                Sounds.Clear();
            };
            startBtn.Enabled = false;
            pc = 0;
            stackListBox.Items.Clear();
            callStackListBox.Items.Clear();
            memoryPos_ValueChanged(sender, e);
            stateText.Text = "Running";
            runningCheck.Enabled = true;
            runningCheck.Checked = true;
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
                    e.Graphics.FillRectangle(new SolidBrush(d.color), d.X * 2, d.Y * 2, d.Width * 2, d.Height * 2);
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
                try
                {
                    if (vm.EmulateFrame())
                    {
                        fasterCheck.Checked = false;
                    }
                }
                catch(InvalidOperationException ex)
                {
                    End();
                    stateText.Text = ex.Message;
                    pc = vm.debug.PC;
                    updateDumps();
                }
            }
        }
        private void NextBtnClicked(object sender, EventArgs e) => Next();

        private void fasterCheck_CheckedChanged(object sender, EventArgs e) => tickNum.ReadOnly = fasterCheck.Checked;

        private void memoryPos_ValueChanged(object sender, EventArgs e)
        {
            if(memoryListBox.Items.Count != 28)
            {
                memoryListBox.Items.Clear();
                for (int i = 0; i < memoryListBox.Items.Count; i++) memoryListBox.Items.Add("");
            }
            for (int i = (int)memoryPos.Value; i < (int)memoryPos.Value + 28 && i < ushort.MaxValue; i++)
                memoryListBox.Items[i - (int)memoryPos.Value] = i.ToString("X4") + ":" + vm.debug.Load((ushort)i);
        }
    }
}
