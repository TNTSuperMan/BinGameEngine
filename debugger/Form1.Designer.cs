namespace debugger
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panel1 = new Panel();
            stackListBox = new ListBox();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            callStackListBox = new ListBox();
            groupBox3 = new GroupBox();
            memoryPos = new NumericUpDown();
            memoryListBox = new ListBox();
            groupBox4 = new GroupBox();
            pcBox = new TextBox();
            programListBox = new ListBox();
            label1 = new Label();
            stateText = new TextBox();
            openFileDialog1 = new OpenFileDialog();
            label2 = new Label();
            tickNum = new NumericUpDown();
            clock = new System.Windows.Forms.Timer(components);
            startBtn = new Button();
            runningCheck = new CheckBox();
            nextBtn = new Button();
            fasterCheck = new CheckBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)memoryPos).BeginInit();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tickNum).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Location = new Point(12, 12);
            panel1.MaximumSize = new Size(256, 256);
            panel1.MinimumSize = new Size(256, 256);
            panel1.Name = "panel1";
            panel1.Size = new Size(256, 256);
            panel1.TabIndex = 0;
            panel1.Paint += Draw;
            // 
            // stackListBox
            // 
            stackListBox.FormattingEnabled = true;
            stackListBox.ItemHeight = 15;
            stackListBox.Items.AddRange(new object[] { "255", "255", "0", "0" });
            stackListBox.Location = new Point(6, 22);
            stackListBox.Name = "stackListBox";
            stackListBox.Size = new Size(43, 379);
            stackListBox.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(stackListBox);
            groupBox1.Location = new Point(274, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(55, 414);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Stack";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(callStackListBox);
            groupBox2.Location = new Point(335, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(70, 414);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "CallStack";
            // 
            // callStackListBox
            // 
            callStackListBox.FormattingEnabled = true;
            callStackListBox.ItemHeight = 15;
            callStackListBox.Items.AddRange(new object[] { "65535", "65535", "0", "0", "0" });
            callStackListBox.Location = new Point(6, 22);
            callStackListBox.Name = "callStackListBox";
            callStackListBox.Size = new Size(58, 379);
            callStackListBox.TabIndex = 1;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(memoryPos);
            groupBox3.Controls.Add(memoryListBox);
            groupBox3.Location = new Point(411, 12);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(67, 414);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "Memory";
            // 
            // memoryPos
            // 
            memoryPos.Location = new Point(6, 22);
            memoryPos.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            memoryPos.Name = "memoryPos";
            memoryPos.Size = new Size(55, 23);
            memoryPos.TabIndex = 7;
            memoryPos.Value = new decimal(new int[] { 40960, 0, 0, 0 });
            memoryPos.ValueChanged += memoryPos_ValueChanged;
            // 
            // memoryListBox
            // 
            memoryListBox.FormattingEnabled = true;
            memoryListBox.ItemHeight = 15;
            memoryListBox.Items.AddRange(new object[] { "255", "255", "0", "0", "0", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" });
            memoryListBox.Location = new Point(6, 52);
            memoryListBox.Name = "memoryListBox";
            memoryListBox.Size = new Size(55, 349);
            memoryListBox.TabIndex = 1;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(pcBox);
            groupBox4.Controls.Add(programListBox);
            groupBox4.Location = new Point(484, 12);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(102, 414);
            groupBox4.TabIndex = 4;
            groupBox4.TabStop = false;
            groupBox4.Text = "Program";
            // 
            // pcBox
            // 
            pcBox.Location = new Point(6, 21);
            pcBox.Name = "pcBox";
            pcBox.ReadOnly = true;
            pcBox.Size = new Size(90, 23);
            pcBox.TabIndex = 1;
            // 
            // programListBox
            // 
            programListBox.FormattingEnabled = true;
            programListBox.ItemHeight = 15;
            programListBox.Items.AddRange(new object[] { "　push 1f", "＞cls", "　ret", "　dumpkey", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27" });
            programListBox.Location = new Point(6, 52);
            programListBox.Name = "programListBox";
            programListBox.Size = new Size(90, 349);
            programListBox.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 277);
            label1.Name = "label1";
            label1.Size = new Size(35, 15);
            label1.TabIndex = 7;
            label1.Text = "state:";
            // 
            // stateText
            // 
            stateText.Location = new Point(53, 274);
            stateText.Name = "stateText";
            stateText.ReadOnly = true;
            stateText.Size = new Size(215, 23);
            stateText.TabIndex = 7;
            stateText.Text = "Not Initialized";
            // 
            // openFileDialog1
            // 
            openFileDialog1.Filter = "バイナリファイル|*.bin|すべてのファイル|*.*";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 305);
            label2.Name = "label2";
            label2.Size = new Size(29, 15);
            label2.TabIndex = 2;
            label2.Text = "tick:";
            // 
            // tickNum
            // 
            tickNum.Location = new Point(53, 303);
            tickNum.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            tickNum.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            tickNum.Name = "tickNum";
            tickNum.Size = new Size(55, 23);
            tickNum.TabIndex = 8;
            tickNum.Value = new decimal(new int[] { 10, 0, 0, 0 });
            tickNum.ValueChanged += TickChanged;
            // 
            // clock
            // 
            clock.Interval = 10;
            clock.Tick += Tick;
            // 
            // startBtn
            // 
            startBtn.Location = new Point(193, 303);
            startBtn.Name = "startBtn";
            startBtn.Size = new Size(75, 23);
            startBtn.TabIndex = 9;
            startBtn.Text = "Start";
            startBtn.UseVisualStyleBackColor = true;
            startBtn.Click += Start;
            // 
            // runningCheck
            // 
            runningCheck.AutoSize = true;
            runningCheck.Enabled = false;
            runningCheck.Location = new Point(114, 335);
            runningCheck.Name = "runningCheck";
            runningCheck.Size = new Size(71, 19);
            runningCheck.TabIndex = 10;
            runningCheck.Text = "Running";
            runningCheck.UseVisualStyleBackColor = true;
            runningCheck.CheckedChanged += RunningChanged;
            // 
            // nextBtn
            // 
            nextBtn.Enabled = false;
            nextBtn.Location = new Point(193, 332);
            nextBtn.Name = "nextBtn";
            nextBtn.Size = new Size(75, 23);
            nextBtn.TabIndex = 11;
            nextBtn.Text = "Next";
            nextBtn.UseVisualStyleBackColor = true;
            nextBtn.Click += NextBtnClicked;
            // 
            // fasterCheck
            // 
            fasterCheck.AutoSize = true;
            fasterCheck.Location = new Point(114, 305);
            fasterCheck.Name = "fasterCheck";
            fasterCheck.Size = new Size(59, 19);
            fasterCheck.TabIndex = 12;
            fasterCheck.Text = "Faster!";
            fasterCheck.UseVisualStyleBackColor = true;
            fasterCheck.CheckedChanged += fasterCheck_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(595, 433);
            Controls.Add(fasterCheck);
            Controls.Add(nextBtn);
            Controls.Add(runningCheck);
            Controls.Add(startBtn);
            Controls.Add(tickNum);
            Controls.Add(label2);
            Controls.Add(stateText);
            Controls.Add(label1);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(panel1);
            DoubleBuffered = true;
            MaximizeBox = false;
            MaximumSize = new Size(611, 472);
            MinimumSize = new Size(611, 472);
            Name = "Form1";
            Text = "BGEDebugger";
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)memoryPos).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tickNum).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private ListBox stackListBox;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private ListBox callStackListBox;
        private GroupBox groupBox3;
        private ListBox memoryListBox;
        private GroupBox groupBox4;
        private ListBox programListBox;
        private NumericUpDown memoryPos;
        private Label label1;
        private TextBox stateText;
        private OpenFileDialog openFileDialog1;
        private Label label2;
        private NumericUpDown tickNum;
        private System.Windows.Forms.Timer clock;
        private Button startBtn;
        private CheckBox runningCheck;
        private Button nextBtn;
        private CheckBox fasterCheck;
        private TextBox pcBox;
    }
}
