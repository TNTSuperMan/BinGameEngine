namespace runtime
{
    public partial class Runtime
    {
        public List<ushort> callstack
        {
            get; private set;
        } = new();
        private Stack stack = new();
        private Memory _memory;
        public byte[] Memory
        {
            get { return _memory.Value; }
            set { _memory.Value = value; }
        }

        private ushort pc = 0;

        public Runtime(byte[] rom)
        {
            _memory = new Memory(rom);
        }
        public List<byte> DebugStack => stack.StackList;
    }
}
