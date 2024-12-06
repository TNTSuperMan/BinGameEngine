using System.Media;

namespace testr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            byte[][] data = bgeruntime.Sound.Bin2WavBins([
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
            MessageBox.Show(data[0].Length.ToString());
            var sp = new SoundPlayer(new MemoryStream(data[0]));
            sp.Play();
        }
    }
}
