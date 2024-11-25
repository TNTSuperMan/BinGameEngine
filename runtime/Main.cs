namespace runtime
{
    public partial class Runtime
    {
        private List<ushort> callstack = new();
        private Stack stack = new();
        private Memory memory;

        private ushort pc = 0;

        public Runtime(byte[] rom)
        {
            memory = new Memory(rom);
        }
    }
}
