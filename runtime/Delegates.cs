using System.Drawing;

namespace bgeruntime
{
    public delegate void Redraw(IGraphic[] g);
    public delegate byte GetKeyState();
    public delegate void Save(byte[] data);
    public delegate byte[] Load();
    public delegate void PlaySound(Sound sound);
    public delegate void StopSound();
    public delegate void End();

    public delegate Graphic GraphicConstructor(byte[] data);
    public delegate IGraphic RectangleConstructor(byte x, byte y, byte w, byte h, Color c);

    public partial class Runtime
    {
        public Redraw      onRedraw    = (IGraphic[] g)=>{};
        public GetKeyState getKeyState = ()=>0;
        public Save        onSave      = (byte[] data)=>{};
        public Load        onLoad      = ()=>new byte[0x1000];
        public PlaySound   onSound     = (Sound sound)=>{};
        public StopSound   onStopSound = ()=>{};
        public End         onEnd       = ()=>{};

        public GraphicConstructor? createGraphic;
        public RectangleConstructor? createRectangle;
    }
}
