﻿using System.Drawing;

namespace bgeruntime
{
    public class GraphRect
    {
        public readonly Color color;
        public readonly bool isPixel = false;
        public readonly bool isDraw = true;
        public readonly byte X;
        public readonly byte Y;
        public readonly byte Width = 1;
        public readonly byte Height = 1;
        public GraphRect(byte c, byte h, byte w, byte y, byte x)
        {
            X = Math.Min(x, (byte)0x7f);
            Y = Math.Min(y, (byte)0x7f);
            Width = (byte)Math.Min(w, 0x7f - X);
            Height =(byte)Math.Min(h, 0x7f - Y);
            color = Color.FromArgb(
                ((c & 0b00110000) >> 4) * 85,
                ((c & 0b00001100) >> 2) * 85,
                ((c & 0b00000011) >> 0) * 85
            );
            if ((c & 0b11000000) >> 6 == 0b01)
                isDraw = false;
            if (Width == 0)
                isDraw = false;
            if (Height == 0)
                isDraw = false;
        }
        public GraphRect(byte x, byte y, byte c)
        {
            color = Color.FromArgb(
                ((c & 0b00110000) >> 4) * 85,
                ((c & 0b00001100) >> 2) * 85,
                ((c & 0b00000011) >> 0) * 85
            );
            isPixel = true;
            isDraw = ((c & 0b11000000) >> 6) != 0b01;
            X = x;
            Y = y;
        }
        public GraphRect(GraphRect g, byte x, byte y)
        {
            color = g.color;
            isPixel = g.isPixel;
            isDraw = g.isDraw;
            X = Math.Min(x, (byte)0x7f);
            Y = Math.Min(y, (byte)0x7f);
            Width = (byte)Math.Min(g.Width, 0x7f - X);
            Height = (byte)Math.Min(g.Height, 0x7f - Y);
            if (Width == 0)
                isDraw = false;
            if (Height == 0)
                isDraw = false;
        }
    }
    public class Graphic
    {
        private GraphRect[] rawdata = [];
        public Graphic(byte[] data)
        {
            List<GraphRect> raws = new();
            byte x = 0, y = 0;
            foreach (var d in data)
            {
                if ((d & 0b11000000) >> 6 == 0b10)
                {
                    y++;
                    x = 0;
                    continue;
                }else raws.Add(new GraphRect(x, y, d));
                x++;
            }
            rawdata = raws.ToArray();
        }
        public GraphRect[] Draw(byte x, byte y)
        {
            List<GraphRect> ret = new();
            foreach (GraphRect data in rawdata)
            {
                ret.Add(new(data, (byte)(data.X + x), (byte)(data.Y + y)));
            }
            return ret.ToArray();
        }
        static public Graphic[] Bin2Graphics(byte[] data)
        {
            List<Graphic> graphics = new();
            List<byte> graphStack = new();
            for (int i = 0; i < data.Length; i++)
            {
                if ((data[i] & 0b11000000) >> 6 == 0b11)
                {
                    graphics.Add(new(graphStack.ToArray()));
                    graphStack.Clear();
                }
                else graphStack.Add(data[i]);
            }
            return graphics.ToArray();
        }
    }
    public partial class Runtime
    {
        private List<GraphRect> redrawStack = new();
        private Graphic[] graphics = [];
    }
}
