namespace runtime
{
    internal class Stack
    {
        private List<byte> stack = new();
        public byte pop()
        {
            byte ret = stack.Last();
            stack.RemoveAt(stack.Count - 1);
            return ret;
        }
        public void push(byte data) => stack.Add(data);
        public ushort PopAddr()
        {
            byte bottom = pop();
            byte top = pop();
            return (ushort)((top << 8) | bottom);
        }
    }
}
