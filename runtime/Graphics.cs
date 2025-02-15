﻿using System.Drawing;
using System.Reflection.Emit;

namespace bgeruntime
{
    /*
    public class GraphRect
    {
        public readonly Color color;
        public readonly bool isPixel = false;
        public readonly bool isDraw = true;
        public readonly int X;
        public readonly int Y;
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
        }
        public GraphRect(int x, int y, byte c)
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
        public GraphRect(GraphRect g, int x, int y)
        {
            color = g.color;
            isPixel = g.isPixel;
            isDraw = g.isDraw;
            X = Math.Min(x, 0x7f);
            Y = Math.Min(y, 0x7f);
            Width = (byte)Math.Min(g.Width, 0x7f - X);
            Height = (byte)Math.Min(g.Height, 0x7f - Y);
            if (Width == 0)
                isDraw = false;
            if (Height == 0)
                isDraw = false;
            if (X < 0)
                isDraw = false;
            if (Y < 0)
                isDraw = false;
        }
    }
    public class Graphic
    {
        private GraphRect[] rawdata = [];
        public Graphic(byte[] data)
        {
            List<GraphRect> raws = new();
            int x = 0, y = 0;
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
            int xs = (x & 0b10000000) == 0 ? x : -(0x100-x);
            int ys = (y & 0b10000000) == 0 ? y : -(0x100-y);
            foreach (GraphRect data in rawdata)
                ret.Add(new(data, xs + data.X, ys + data.Y));
            return ret.FindAll(e=>e.isDraw).ToArray();
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
    }*/
    public interface IGraphic
    {
        public void Draw();
    }
    public abstract class Graphic
    {
        public abstract IGraphic ImgAt(byte x, byte y);

        public static Color ToColor(byte c) =>
            Color.FromArgb(
                (c & 0b11000000) >> 6 == 0b01 ? 0 : 255,
                ((c & 0b00110000) >> 4) * 85,
                ((c & 0b00001100) >> 2) * 85,
                ((c & 0b00000011) >> 0) * 85
            );
        static public Graphic[] Bin2Graphics(byte[] data, GraphicConstructor createGraphic)
        {
            List<Graphic> graphics = new();
            List<byte> graphStack = new();
            for (int i = 0; i < data.Length; i++)
            {
                if ((data[i] & 0b11000000) >> 6 == 0b11)
                {
                    graphics.Add(createGraphic(graphStack.ToArray()));
                    graphStack.Clear();
                }
                else graphStack.Add(data[i]);
            }
            return graphics.ToArray();
        }
    }
    public partial class Runtime
    {
        private List<IGraphic> redrawStack = new();
        private Graphic[] graphics = [];
    }
}
