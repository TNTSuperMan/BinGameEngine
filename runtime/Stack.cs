namespace runtime
{
    internal class Stack
    {
        public List<byte> StackList
        {
            get; private set;
        } = new();
        public byte pop()
        {
            byte ret = StackList.Last();
            StackList.RemoveAt(StackList.Count - 1);
            return ret;
        }
        public void push(byte data) => StackList.Add(data);
        public ushort popAddr()
        {
            byte bottom = pop();
            byte top = pop();
            return (ushort)((top << 8) | bottom);
        }
        public void clear() => StackList.Clear();
    }
}
