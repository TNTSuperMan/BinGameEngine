using bgeruntime;
using System.Media;

namespace testr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        GraphRect[] disps = [];
        private void Form1_Load(object sender, EventArgs e)
        {
            byte[][] data = Sound.Bin2WavBins([
                120,       //Sq1
                0b01000100, 0b01010000,
                0b01000100, 0b01010001,
                0b01000100, 0b01010010,
                0b11000000,//Sq2
                0b01000100, 0b01000000,
                0b01000100, 0b01000001,
                0b01000100, 0b01000010,
                0b11000000,//Tri
                0b01000100, 0b01010000,
                0b01000100, 0b01010001,
                0b01000100, 0b01010010,
                0b11000000,//Nse
                0b11000000,//End?
            ]);
            if (data.Length == 1)
            {
                var sp = new SoundPlayer(new MemoryStream(data[0]));
                sp.Play();
            }
            else
            {
                MessageBox.Show("生成されてないお:" + data.Length.ToString());
            }
            //JSのツールでつくったサンプル画像
            Graphic[] graphics = Graphic.Bin2Graphics([
                128,64,64,64,64,57,57,128,64,64,64,64,57,57,128,64,64,64,57,3,3,57,128,64,64,64,57,3,3,57,128,64,64,64,57,3,3,57,128,64,64,64,64,57,57,128,64,64,64,64,57,57,128,64,64,64,64,63,63,128,128,192,63,0,0,0,63,128,0,63,0,63,0,128,0,0,63,0,0,128,0,63,0,63,0,128,63,0,0,0,63,128,192
            ]);

            if (graphics.Length == 2)
            {
                disps = graphics[1].Draw(100, 100);
                panel1.Invalidate();
            }
            else
            {
                MessageBox.Show("生成されてないお:" + graphics.Length.ToString());
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            foreach(var rect in disps) if(rect.isDraw) e.Graphics.FillRectangle(new SolidBrush(rect.color), rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}
