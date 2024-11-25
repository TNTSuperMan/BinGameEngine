using System.Drawing;

namespace runtime
{
    public class Graphic
    {
        public readonly Color color;
        public readonly bool isPixel = false;
        public readonly bool isDraw = true;
        public readonly byte X;
        public readonly byte Y;
        public readonly byte Width = 1;
        public readonly byte Height = 1;
        public Graphic(byte c, byte h, byte w, byte y, byte x)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
            color = Color.FromArgb(
                ((c & 0b00110000) >> 4) * 85,
                ((c & 0b00001100) >> 2) * 85,
                ((c & 0b00000011) >> 0) * 85
            );
            if ((c & 0b11000000) >> 6 == 0b01)
                isDraw = false;
        }
        public Graphic(byte x, byte y,Color c)
        {
            color = c;
            isPixel = true;
            X = x;
            Y = y;
        }
    }
    public partial class Runtime
    {
        private List<Graphic> graphicsStack;
    }
}
