using bgeruntime;
using System;
using System.Collections.Generic;

namespace debugger
{
    delegate Graphics GraphicProxy();
    internal class BGERectangle : IGraphic
    {
        byte X, Y, W, H;
        Color Color;
        GraphicProxy Graphic;
        public BGERectangle(byte x, byte y, byte w, byte h, Color c, GraphicProxy graphic)
        {
            X = Math.Min(x, (byte)0x7f);
            Y = Math.Min(y, (byte)0x7f);
            W = (byte)Math.Min(w, 0x7f - x);
            H = (byte)Math.Min(h, 0x7f - y);
            Color = c;
            Graphic = graphic;
        }
        public void Draw()
        {
            Graphic().FillRectangle(new SolidBrush(Color), X * 2, Y * 2, W * 2, H * 2);
        }
    }
    internal class BGEGraphic : Graphic
    {
        GraphicProxy Graph;
        Image Img;
        public BGEGraphic(byte[] data, GraphicProxy graphic)
        {
            Bitmap img = new Bitmap(256, 256);
            Graphics g = Graphics.FromImage(img);

            int x = 0, y = 0;
            foreach (var d in data)
            {
                if ((d & 0b11000000) >> 6 == 0b10)
                {
                    y++;
                    x = 0;
                    continue;
                }
                else g.FillRectangle(new SolidBrush(ToColor(d)), x * 2, y * 2, 1, 1);
                x++;
            }

            g.Dispose();
            Img = img;
            Graph = graphic;
        }
        public override IGraphic ImgAt(byte x, byte y)
        {
            return new BGERealGraphic(Img, Graph, x, y);
        }
    }
    internal class BGERealGraphic : IGraphic
    {
        GraphicProxy Graphic;
        Image Img;
        byte X, Y;
        public BGERealGraphic(Image img, GraphicProxy graphic, byte x, byte y)
        {
            Graphic = graphic; Img = img; X = x; Y = y;
        }
        public void Draw()
        {
            Graphic().DrawImage(Img, X * 2, Y * 2);
        }
    }
}
