namespace bgeruntime
{
    public delegate void Redraw(GraphRect[] g);
    public delegate byte GetKeyState();
    public delegate void Save(byte[] data);
    public delegate byte[] Load();
    public delegate void PlaySound(byte[] wave);
    public delegate void StopSound();
    public delegate void End();

    public partial class Runtime
    {
        public Redraw      onRedraw    = (GraphRect[] g)=>{};
        public GetKeyState getKeyState = ()=>0;
        public Save        onSave      = (byte[] data)=>{};
        public Load        onLoad      = ()=>new byte[0x1000];
        public PlaySound   onSound     = (byte[] wave)=>{};
        public StopSound   onStopSound = ()=>{};
        public End         onEnd       = ()=>{};
    }
}
