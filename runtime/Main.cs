namespace bgeruntime
{
    public partial class Runtime
    {
        public Stack<ushort> callstack
        {
            get; private set;
        } = new();
        private Stack<byte> stack = new();
        private Memory memory;

        private ushort pc = 0;

        public readonly RuntimeDebug debug;

        public Runtime(byte[] rom)
        {
            memory = new Memory(rom);
            debug = new(
                () => stack.Reverse().ToArray(),
                () => callstack.Reverse().ToArray(),
                () => pc,
                memory.Load,
                memory.Store);
        }
    }
    public class RuntimeDebug
    {
        public delegate byte[] DelegateStackList();
        public delegate ushort[] DelegateCallStackList();
        public delegate ushort DelegatePC();
        public delegate byte DelegateLoad(ushort addr);
        public delegate void DelegateStore(ushort addr, byte value);

        readonly DelegateStackList _GetStackList;
        readonly DelegateCallStackList _GetCallStackList;
        readonly DelegatePC _GetPC;

        public byte[] StackList => _GetStackList();
        public ushort[] CallStackList => _GetCallStackList();
        public ushort PC => _GetPC();
        public readonly DelegateLoad Load;
        public readonly DelegateStore Store;
        public RuntimeDebug(DelegateStackList stlist, DelegateCallStackList cslist, DelegatePC pc, DelegateLoad load, DelegateStore store)
        {
            _GetStackList = stlist;
            _GetCallStackList = cslist;
            _GetPC = pc;
            Load = load;
            Store = store;
        }
    }
}
