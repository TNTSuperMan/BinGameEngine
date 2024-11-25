namespace bgeruntime
{
    internal class Stack
    {
        public List<byte> StackList
        {
            get; private set;
        } = new();
        public void push(byte data) => StackList.Add(data);
        public byte pop()
        {
            if (StackList.Count == 0) throw new StackOutOfRangeException("Stack underflow");
            byte ret = StackList.Last();
            StackList.RemoveAt(StackList.Count - 1);
            return ret;
        }
        public ushort popAddr()
        {
            byte bottom = pop();
            byte top = pop();
            return (ushort)((top << 8) | bottom);
        }
        public void clear() => StackList.Clear();
    }
}
