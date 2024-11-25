namespace runtime
{
    public partial class Runtime
    {
        delegate int operate(byte b, byte a);
        enum Commands: byte
        {
            push,pop,cls,
            add,sub,mul,div,rem,nand,equal,greater,
            truejump,jump,call,ret,
            load,store,
            dumpkey,
            redraw,rect,graph,
            sound,stopsound,
            io
        }
        public void EmulateFrame()
        {
            while (memory[pc] != (byte)Commands.redraw)
                EmulateNext();
        }
        public void EmulateNext()
        {
            var StackOperate = (operate op) =>
                stack.push((byte)(op(stack.pop(), stack.pop())));
            switch ((Commands)memory[pc])
            {
                case Commands.push:
                    stack.push(memory[++pc]);
                    break;
                case Commands.pop:
                    stack.pop();
                    break;
                case Commands.cls:
                    stack.clear();
                    break;
                case Commands.add:
                    StackOperate((b, a) => a + b);
                    break;
                case Commands.sub:
                    StackOperate((b, a) => a - b);
                    break;
                case Commands.mul:
                    StackOperate((b, a) => a * b);
                    break;
                case Commands.div:
                    StackOperate((b, a) => a / b);
                    break;
                case Commands.rem:
                    StackOperate((b, a) => a & b);
                    break;
                case Commands.nand:
                    StackOperate((b, a) => ~(a&b));
                    break;
                case Commands.greater:
                    StackOperate((b, a) => a>b ? 1:0);
                    break;
                case Commands.equal:
                    StackOperate((b, a) => a == b ? 1:0);
                    break;
            }
        }
    }
}
