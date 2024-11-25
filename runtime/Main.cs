namespace bgeruntime
{
    public partial class Runtime
    {
        public List<ushort> callstack
        {
            get; private set;
        } = new();
        private Stack stack = new();
        private Memory memory;

        private ushort pc = 0;

        public readonly RuntimeDebug debug;

        public Runtime(byte[] rom)
        {
            memory = new Memory(rom);
            debug = new(
                () => stack.StackList,
                () => pc,
                memory.Load,
                memory.Store);
        }
    }
    public class RuntimeDebug
    {
        public delegate List<byte> DelegateStackList();
        public delegate ushort DelegatePC();
        public delegate byte DelegateLoad(ushort addr);
        public delegate void DelegateStore(ushort addr, byte value);

        readonly DelegateStackList _GetStackList;
        readonly DelegatePC _GetPC;

        public List<byte> StackList => _GetStackList();
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
