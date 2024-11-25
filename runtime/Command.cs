using System.Data;

namespace runtime
{
    public partial class Runtime
    {
        enum Commands: byte
        {
            push,pop,cls,
            add,sub,mun,div,rem,nand,equal,greater,
            truejump,jump,call,ret,
            load,store,
            dumpkey,
            redraw,rect,graph,
            sound,stopsound,
            io
        }
        public void EmulateFrame()
        {
            while (memory[pc] != (byte)Commands.redraw)
                EmulateNext();
        }
        public void EmulateNext()
        {

        }
    }
}
