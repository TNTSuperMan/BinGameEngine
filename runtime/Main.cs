namespace runtime
{
    public partial class Runtime
    {
        private List<ushort> callstack = new();
        private Stack stack = new();
        private Memory _memory;
        private byte[] memory
        {
            get { return _memory.Value; }
            set { _memory.Value = value; }
        }

        private ushort pc = 0;

        public Runtime(byte[] rom)
        {
            _memory = new Memory(rom);
        }
    }
}
