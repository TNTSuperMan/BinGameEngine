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
                () => [.. stack],
                () => pc,
                memory.Load,
                memory.Store);
        }
    }
    public class RuntimeDebug
    {
        public delegate byte[] DelegateStackList();
        public delegate ushort DelegatePC();
        public delegate byte DelegateLoad(ushort addr);
        public delegate void DelegateStore(ushort addr, byte value);

        readonly DelegateStackList _GetStackList;
        readonly DelegatePC _GetPC;

        public byte[] StackList => _GetStackList();
        public ushort PC => _GetPC();
        public readonly DelegateLoad Load;
        public readonly DelegateStore Store;
        public RuntimeDebug(DelegateStackList stlist, DelegatePC pc, DelegateLoad load, DelegateStore store)
        {
            _GetStackList = stlist;
            _GetPC = pc;
            Load = load;
            Store = store;
        }
    }
}
