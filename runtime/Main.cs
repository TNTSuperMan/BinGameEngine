namespace runtime
{
    public partial class Runtime
    {
        private List<ushort> callstack = new();

        private Memory memory;

        private ushort pc = 0;

        public Runtime(byte[] rom)
        {
            memory = new Memory(rom);
        }
        private byte pop()
        {
            byte ret = stack.Last();
            stack.RemoveAt(stack.Count - 1);
            return ret;
        }
        private void push(byte data) => stack.Add(data);
        private ushort popAddr()
        {
            byte bottom = pop();
            byte top = pop();
            return (ushort)((top << 8) | bottom);
        }
    }
}
