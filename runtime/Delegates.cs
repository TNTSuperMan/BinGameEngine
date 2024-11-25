using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace runtime
{
    public delegate void Redraw();
    public delegate byte GetKeyState();
    public delegate void Save(byte[] data);
    public delegate byte[] Load();
    public delegate void Sound(byte hz, byte len);
    public delegate void StopSound();

    public partial class Runtime
    {
        public Redraw onRedraw;
        public GetKeyState onKeyState;
        public Save onSave;
        public Load onLoad;
        public Sound onSound;
        public StopSound onStopSound;
    }
}
