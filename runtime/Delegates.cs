namespace bgeruntime
{
    public delegate void Redraw(Graphic[] g);
    public delegate byte GetKeyState();
    public delegate void Save(byte[] data);
    public delegate byte[] Load();
    public delegate void Sound(byte hz, byte len);
    public delegate void StopSound();
    public delegate void End();

    public partial class Runtime
    {
        public Redraw      onRedraw    = (Graphic[] g)=>{};
        public GetKeyState getKeyState = ()=>0;
        public Save        onSave      = (byte[] data)=>{};
        public Load        onLoad      = ()=>new byte[0x1000];
        public Sound       onSound     = (byte hz, byte len)=>{};
        public StopSound   onStopSound = ()=>{};
        public End         onEnd       = ()=>{};
    }
}
